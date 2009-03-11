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

    public enum SpellId
    {
        None,
        Wand,
        LightningBolt,
        ThunderBolt,
        LightweaveBolt,
        [Description("Arcane Barrage (0)")]
        ArcaneBarrage,
        [Description("Arcane Barrage (1)")]
        ArcaneBarrage1,
        [Description("Arcane Barrage (2)")]
        ArcaneBarrage2,
        [Description("Arcane Barrage (3)")]
        ArcaneBarrage3,
        ArcaneBarrage1Combo,
        ArcaneBarrage2Combo,
        ArcaneBarrage3Combo,
        ArcaneBolt,
        PendulumOfTelluricCurrents,
        [Description("Arcane Missiles (0)")]
        ArcaneMissiles,
        [Description("Arcane Missiles (1)")]
        ArcaneMissiles1,
        [Description("Arcane Missiles (2)")]
        ArcaneMissiles2,
        [Description("Arcane Missiles (3)")]
        ArcaneMissiles3,
        ArcaneMissiles0Clipped,
        ArcaneMissiles1Clipped,
        ArcaneMissiles2Clipped,
        ArcaneMissiles3Clipped,
        [Description("MBAM (0)")]
        ArcaneMissilesMB,
        [Description("MBAM (1)")]
        ArcaneMissilesMB1,
        [Description("MBAM (2)")]
        ArcaneMissilesMB2,
        [Description("MBAM (3)")]
        ArcaneMissilesMB3,
        ArcaneMissilesMB0Clipped,
        ArcaneMissilesMB1Clipped,
        ArcaneMissilesMB2Clipped,
        ArcaneMissilesMB3Clipped,
        ArcaneMissilesCC,
        ArcaneMissilesNoProc,
        //ArcaneMissilesFTF,
        //ArcaneMissilesFTT,
        Frostbolt,
        [Description("Frostbolt")]
        FrostboltFOF,
        [Description("POM+Frostbolt")]
        FrostboltPOM,
        FrostboltNoCC,
        [Description("Fireball")]
        Fireball,
        [Description("POM+Fireball")]
        FireballPOM,
        FireballBF,
        FrostfireBolt,
        [Description("Frostfire Bolt")]
        FrostfireBoltFOF,
        [Description("Pyroblast")]
        Pyroblast,
        [Description("POM+Pyroblast")]
        PyroblastPOM,
        [Description("Fire Blast")]
        FireBlast,
        [Description("Scorch")]
        Scorch,
        ScorchNoCC,
        [Description("Living Bomb")]
        LivingBomb,
        ArcaneBlastSpam,
        [Description("Arcane Blast (3)")]
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
        ABABarSc,
        ABABarCSc,
        ABAMABarSc,
        AB3AMABarSc,
        AB3ABarCSc,
        AB3MBAMABarSc,
        ABarAM,
        ABP,
        ABAM,
        ABABar,
        ABABar3C,
        ABABar2C,
        ABABar2MBAM,
        ABABar1MBAM,
        ABABar0MBAM,
        ABSpamMBAM,
        ABSpam03C,
        ABSpam3C,
        ABSpam3MBAM,
        ABAMABar,
        AB2AMABar,
        AB3AMABar,
        AB32AMABar,
        AB3ABar3MBAM,
        AB3AM,
        AB3AM2MBAM,
        ABABar0C,
        ABABar1C,
        ABABarY,
        AB2ABar,
        AB2ABarMBAM,
        AB2ABar2C,
        AB2ABar2MBAM,
        AB2ABar3C,
        AB3ABar,
        AB3ABar3C,
        AB3ABarX,
        AB3ABarY,
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
        FB2ABar,
        FrB2ABar,
        ScLBPyro,
        FrBFB,
        FrBFBIL,
        FBSc,
        FBFBlast,
        FBPyro,
        FBLBPyro,
        FFBLBPyro,
        FFBPyro,
        FBScPyro,
        FFBScPyro,
        FBScLBPyro,
        FFBScLBPyro,
        Slow,
        IceLance,
        ABABarSlow,
        FBABarSlow,
        FrBABarSlow,
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
        public bool ProvidesSnare;
        public bool ProvidesScorch;

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
        public float CritProcs;
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
                CostPerSecond += spellWeight.Weight * spell.CostPerSecond * spell.CastTime;
                DamagePerSecond += spellWeight.Weight * spell.DamagePerSecond * spell.CastTime;
                ThreatPerSecond += spellWeight.Weight * spell.ThreatPerSecond * spell.CastTime;
                ManaRegenPerSecond += spellWeight.Weight * spell.ManaRegenPerSecond * spell.CastTime;
            }
            CastTime /= weightTotal;
            CostPerSecond /= weightTotal * CastTime;
            DamagePerSecond /= weightTotal * CastTime;
            ThreatPerSecond /= weightTotal * CastTime;
            ManaRegenPerSecond /= weightTotal * CastTime;
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
            CostAmplifier *= (1.0f - 0.01f * castingState.MageTalents.ElementalPrecision);
            if (castingState.MageTalents.FrostChanneling > 0) CostAmplifier *= (1.0f - 0.01f - 0.03f * castingState.MageTalents.FrostChanneling);
            if (MagicSchool == MagicSchool.Arcane) CostAmplifier *= (1.0f - 0.01f * castingState.MageTalents.ArcaneFocus);
            if (castingState.PowerInfusion) CostModifier -= 0.2f; // don't have any information on this, going by best guess
            if (castingState.ArcanePower) CostModifier += 0.2f;
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
                case MagicSchool.Holy:
                    SpellModifier = castingState.HolySpellModifier;
                    CritRate = castingState.HolyCritRate;
                    CritBonus = castingState.HolyCritBonus;
                    RawSpellDamage = castingState.HolyDamage;
                    HitRate = castingState.HolyHitRate;
                    RealResistance = castingState.CalculationOptions.HolyResist;
                    ThreatMultiplier = castingState.HolyThreatMultiplier;
                    break;
            }

            int targetLevel = AreaEffect ? castingState.CalculationOptions.AoeTargetLevel : castingState.CalculationOptions.TargetLevel;

            // do not count debuffs for aoe effects, can't assume it will be up on all
            // do not include molten fury (molten fury relates to boss), instead amplify all by average
            if (AreaEffect)
            {
                if (castingState.MoltenFury)
                {
                    SpellModifier /= (1 + 0.06f * castingState.MageTalents.MoltenFury);
                }
                if (castingState.MageTalents.MoltenFury > 0)
                {
                    SpellModifier *= (1 + 0.06f * castingState.MageTalents.MoltenFury * castingState.CalculationOptions.MoltenFuryPercentage);
                }

                float maxHitRate = 1.00f;
                int playerLevel = castingState.CalculationOptions.PlayerLevel;
                if (MagicSchool == MagicSchool.Arcane) HitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + castingState.SpellHit + 0.01f * castingState.MageTalents.ArcaneFocus);
                if (MagicSchool == MagicSchool.Fire) HitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + castingState.SpellHit);
                if (MagicSchool == MagicSchool.Frost) HitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + castingState.SpellHit);
                if (MagicSchool == MagicSchool.Nature) HitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + castingState.SpellHit);
                if (MagicSchool == MagicSchool.Shadow) HitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + castingState.SpellHit);
                if (MagicSchool == MagicSchool.Holy) HitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + castingState.SpellHit);
            }

            if (ManualClearcasting && !ClearcastingAveraged)
            {
                CritRate -= 0.15f * 0.02f * castingState.MageTalents.ArcaneConcentration * castingState.MageTalents.ArcanePotency; // replace averaged arcane potency with actual % chance
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

        protected void CalculateDerivedStats(CastingState castingState, bool outOfFiveSecondRule)
        {
            MageTalents mageTalents = castingState.MageTalents;
            Stats baseStats = castingState.BaseStats;
            CalculationOptionsMage calculationOptions = castingState.CalculationOptions;
            if (CritRate < 0.0f) CritRate = 0.0f;

            CastingSpeed = castingState.CastingSpeed;

            if (Instant) InterruptProtection = 1;
            if (castingState.IcyVeins) InterruptProtection = 1;
            // interrupt factors of more than once per spell are not supported, so put a limit on it (up to twice is probably approximately correct)
            float InterruptFactor = Math.Min(calculationOptions.InterruptFrequency, 2 * CastingSpeed / BaseCastTime);

            float Haste = castingState.SpellHasteRating;
            float levelScalingFactor;
            levelScalingFactor = (float)((52f / 82f) * Math.Pow(63f / 131f, (calculationOptions.PlayerLevel - 70) / 10f));

            float maxPushback = 0.5f * Math.Max(0, 1 - InterruptProtection);
            if (Channeled) maxPushback = 0.0f;
            GlobalCooldown = Math.Max(castingState.GlobalCooldownLimit, 1.5f / CastingSpeed);
            CastTime = BaseCastTime / CastingSpeed + castingState.Latency;
            CastTime = CastTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + castingState.Latency) * maxPushback * InterruptFactor;
            if (CastTime < GlobalCooldown + castingState.Latency) CastTime = GlobalCooldown + castingState.Latency;

            CritRate = Math.Min(1.0f, CritRate);

            // Quagmirran
            if (baseStats.SpellHasteFor6SecOnHit_10_45 > 0 && HitProcs > 0)
            {
                // hasted casttime
                float speed = CastingSpeed / (1 + Haste / 995f * levelScalingFactor) * (1 + (Haste + baseStats.SpellHasteFor6SecOnHit_10_45) / 995f * levelScalingFactor);
                float gcd = Math.Max(castingState.GlobalCooldownLimit, 1.5f / speed);
                float cast = BaseCastTime / speed + castingState.Latency;
                cast = cast * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + castingState.Latency) * maxPushback * InterruptFactor;
                if (cast < gcd + castingState.Latency) cast = gcd + castingState.Latency;

                CastingSpeed /= (1 + Haste / 995f * levelScalingFactor);
                //discrete model
                float castsAffected = 0;
                for (int c = 0; c < HitProcs; c++) castsAffected += (float)Math.Ceiling((6f - c * CastTime / HitProcs) / cast) / HitProcs;
                Haste += baseStats.SpellHasteFor6SecOnHit_10_45 * castsAffected * cast / (45f + CastTime / HitProcs / 0.1f);
                //continuous model
                //Haste += castingState.BasicStats.SpellHasteFor6SecOnHit_10_45 * 6f / (45f + CastTime / HitProcs / 0.1f);
                CastingSpeed *= (1 + Haste / 995f * levelScalingFactor);

                GlobalCooldown = Math.Max(castingState.GlobalCooldownLimit, 1.5f / CastingSpeed);
                CastTime = BaseCastTime / CastingSpeed + castingState.Latency;
                CastTime = CastTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + castingState.Latency) * maxPushback * InterruptFactor;
                if (CastTime < GlobalCooldown + castingState.Latency) CastTime = GlobalCooldown + castingState.Latency;
            }

            // MSD
            if (baseStats.SpellHasteFor6SecOnCast_15_45 > 0 && CastProcs > 0)
            {
                // hasted casttime
                float speed = CastingSpeed / (1 + Haste / 995f * levelScalingFactor) * (1 + (Haste + baseStats.SpellHasteFor6SecOnCast_15_45) / 995f * levelScalingFactor);
                float gcd = Math.Max(castingState.GlobalCooldownLimit, 1.5f / speed);
                float cast = BaseCastTime / speed + castingState.Latency;
                cast = cast * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + castingState.Latency) * maxPushback * InterruptFactor;
                if (cast < gcd + castingState.Latency) cast = gcd + castingState.Latency;

                CastingSpeed /= (1 + Haste / 995f * levelScalingFactor);
                float castsAffected = 0;
                for (int c = 0; c < CastProcs; c++) castsAffected += (float)Math.Ceiling((6f - c * CastTime / CastProcs) / cast) / CastProcs;
                Haste += baseStats.SpellHasteFor6SecOnCast_15_45 * castsAffected * cast / (45f + CastTime / CastProcs / 0.15f);
                //Haste += castingState.BasicStats.SpellHasteFor6SecOnCast_15_45 * 6f / (45f + CastTime / CastProcs / 0.15f);
                CastingSpeed *= (1 + Haste / 995f * levelScalingFactor);

                GlobalCooldown = Math.Max(castingState.GlobalCooldownLimit, 1.5f / CastingSpeed);
                CastTime = BaseCastTime / CastingSpeed + castingState.Latency;
                CastTime = CastTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + castingState.Latency) * maxPushback * InterruptFactor;
                if (CastTime < GlobalCooldown + castingState.Latency) CastTime = GlobalCooldown + castingState.Latency;
            }

            // Embrace of the Spider
            float spellHasteFor10SecOnCast_10_45 = baseStats.SpellHasteFor10SecOnCast_10_45;
            if (Name == "Arcane Missiles") spellHasteFor10SecOnCast_10_45 += baseStats.EggOfMortalEssenceArcaneMissilesProc;
            if (spellHasteFor10SecOnCast_10_45 > 0 && CastProcs > 0)
            {
                // hasted casttime
                float speed = CastingSpeed / (1 + Haste / 995f * levelScalingFactor) * (1 + (Haste + spellHasteFor10SecOnCast_10_45) / 995f * levelScalingFactor);
                float gcd = Math.Max(castingState.GlobalCooldownLimit, 1.5f / speed);
                float cast = BaseCastTime / speed + castingState.Latency;
                cast = cast * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + castingState.Latency) * maxPushback * InterruptFactor;
                if (cast < gcd + castingState.Latency) cast = gcd + castingState.Latency;

                CastingSpeed /= (1 + Haste / 995f * levelScalingFactor);
                float castsAffected = 0;
                for (int c = 0; c < CastProcs; c++) castsAffected += (float)Math.Ceiling((10f - c * CastTime / CastProcs) / cast) / CastProcs;
                Haste += spellHasteFor10SecOnCast_10_45 * castsAffected * cast / (45f + CastTime / CastProcs / 0.1f);
                //Haste += castingState.BasicStats.SpellHasteFor10SecOnCast_10_45 * 10f / (45f + CastTime / CastProcs / 0.1f);
                CastingSpeed *= (1 + Haste / 995f * levelScalingFactor);

                GlobalCooldown = Math.Max(castingState.GlobalCooldownLimit, 1.5f / CastingSpeed);
                CastTime = BaseCastTime / CastingSpeed + castingState.Latency;
                CastTime = CastTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + castingState.Latency) * maxPushback * InterruptFactor;
                if (CastTime < GlobalCooldown + castingState.Latency) CastTime = GlobalCooldown + castingState.Latency;
            }

            // AToI, first cast after proc is not affected for non-instant
            if (baseStats.SpellHasteFor5SecOnCrit_50 > 0)
            {
                float rawHaste = Haste;

                CastingSpeed /= (1 + Haste / 995f * levelScalingFactor);
                float proccedSpeed = CastingSpeed * (1 + (rawHaste + baseStats.SpellHasteFor5SecOnCrit_50) / 995f * levelScalingFactor);
                float proccedGcd = Math.Max(castingState.GlobalCooldownLimit, 1.5f / proccedSpeed);
                float proccedCastTime = BaseCastTime / proccedSpeed + castingState.Latency;
                proccedCastTime = proccedCastTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + castingState.Latency) * maxPushback * InterruptFactor;
                if (proccedCastTime < proccedGcd + castingState.Latency) proccedCastTime = proccedGcd + castingState.Latency;
                int chancesToProc = (int)(((int)Math.Floor(5f / proccedCastTime) + 1) * HitProcs);
                if (!Instant) chancesToProc -= 1;
                chancesToProc *= (int)(TargetProcs / HitProcs);
                Haste = rawHaste + baseStats.SpellHasteFor5SecOnCrit_50 * (1 - (float)Math.Pow(1 - 0.5f * CritRate, chancesToProc));
                //Haste = rawHaste + castingState.BasicStats.SpellHasteFor5SecOnCrit_50 * ProcBuffUp(1 - (float)Math.Pow(1 - 0.5f * CritRate, HitProcs), 5, CastTime);
                CastingSpeed *= (1 + Haste / 995f * levelScalingFactor);
                GlobalCooldown = Math.Max(castingState.GlobalCooldownLimit, 1.5f / CastingSpeed);
                CastTime = BaseCastTime / CastingSpeed + castingState.Latency;
                CastTime = CastTime * (1 + InterruptFactor * maxPushback) - (maxPushback * 0.5f + castingState.Latency) * maxPushback * InterruptFactor;
                if (CastTime < GlobalCooldown + castingState.Latency) CastTime = GlobalCooldown + castingState.Latency;
            }

            Cost = (float)Math.Floor(Math.Floor(BaseCost * CostAmplifier) * CostModifier); // glyph and talent amplifiers are rounded down

            CritRate = Math.Min(1, CritRate);
            CritProcs = HitProcs * CritRate;
            //Cost *= (1f - CritRate * 0.1f * mageTalents.MasterOfElements);
            if (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.FrostFire) Cost += CritRate * Cost * 0.01f * mageTalents.Burnout; // last I read Burnout works on final pre MOE cost
            Cost -= CritRate * BaseCost * 0.1f * mageTalents.MasterOfElements; // from what I know MOE works on base cost

            CostPerSecond = Cost / CastTime;

            if (!ManualClearcasting || ClearcastingAveraged)
            {
                CostPerSecond *= (1 - 0.02f * mageTalents.ArcaneConcentration);
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

            if (baseStats.SpellDamageFor10SecOnHit_5 > 0) RawSpellDamage += baseStats.SpellDamageFor10SecOnHit_5 * ProcBuffUp(1 - (float)Math.Pow(0.95, TargetProcs), 10, CastTime);
            if (baseStats.SpellPowerFor6SecOnCrit > 0) RawSpellDamage += baseStats.SpellPowerFor6SecOnCrit * ProcBuffUp(1 - (float)Math.Pow(1 - CritRate, HitProcs), 6, CastTime);
            if (baseStats.SpellPowerFor10SecOnHit_10_45 > 0) RawSpellDamage += baseStats.SpellPowerFor10SecOnHit_10_45 * 10f / (45f + CastTime / HitProcs / 0.1f);
            if (baseStats.SpellPowerFor10SecOnCast_15_45 > 0) RawSpellDamage += baseStats.SpellPowerFor10SecOnCast_15_45 * 10f / (45f + CastTime / CastProcs / 0.15f);
            if (baseStats.SpellPowerFor10SecOnCast_10_45 > 0) RawSpellDamage += baseStats.SpellPowerFor10SecOnCast_10_45 * 10f / (45f + CastTime / CastProcs / 0.1f);
            if (baseStats.SpellPowerFor10SecOnResist > 0) RawSpellDamage += baseStats.SpellPowerFor10SecOnResist * ProcBuffUp(1 - (float)Math.Pow(HitRate, HitProcs), 10, CastTime);
            if (baseStats.SpellPowerFor15SecOnCrit_20_45 > 0) RawSpellDamage += baseStats.SpellPowerFor15SecOnCrit_20_45 * 15f / (45f + CastTime / HitProcs / 0.2f / CritRate);
            if (baseStats.SpellPowerFor10SecOnCrit_20_45 > 0) RawSpellDamage += baseStats.SpellPowerFor10SecOnCrit_20_45 * 10f / (45f + CastTime / HitProcs / 0.2f / CritRate);
            if (baseStats.ShatteredSunAcumenProc > 0 && calculationOptions.Aldor) RawSpellDamage += 120 * 10f / (45f + CastTime / HitProcs / 0.1f);

            if (!ForceMiss)
            {
                SpellDamage = RawSpellDamage * SpellDamageCoefficient;
                float baseAverage = (BaseMinDamage + BaseMaxDamage) / 2f + SpellDamage;
                float critMultiplier = 1 + (CritBonus - 1) * Math.Max(0, CritRate - castingState.ResilienceCritRateReduction);
                float resistMultiplier = (ForceHit ? 1.0f : HitRate) * PartialResistFactor;
                int targets = 1;
                if (AreaEffect) targets = calculationOptions.AoeTargets;
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

            // channeled pushback
            if (Channeled && InterruptFactor > 0)
            {
                int maxLostTicks = (int)Math.Ceiling(HitProcs * 0.25f * Math.Max(0, 1 - InterruptProtection));
                // pushbacks that happen up to pushbackCastTime cut the cast time to pushbackCastTime
                // pushbacks that happen after just terminate the channel
                // [---|---X---|---|---]
                float tickFactor = 0;
                for (int i = 0; i < maxLostTicks; i++)
                {
                    tickFactor += InterruptFactor * CastTime / HitProcs * (i + 1) / HitProcs;
                }
                tickFactor += InterruptFactor * (HitProcs - maxLostTicks) * CastTime / HitProcs * maxLostTicks / HitProcs;
                CastTime *= (1 - tickFactor);
                CostPerSecond /= (1 - tickFactor);
            }

            if (castingState.WaterElemental)
            {
                waterbolt = new Waterbolt(castingState, RawSpellDamage); // TODO should be frost damage
                DamagePerSecond += waterbolt.DamagePerSecond;
            }
            if (!ForceMiss && !EffectProc)
            {
                if (baseStats.LightningCapacitorProc > 0)
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
                if (baseStats.ThunderCapacitorProc > 0)
                {
                    BaseSpell ThunderBolt = castingState.ThunderBolt;
                    //discrete model
                    int hitsInsideCooldown = (int)(2.5f / (CastTime / HitProcs));
                    float avgCritsPerHit = CritRate * TargetProcs / HitProcs;
                    float avgHitsToDischarge = 4f / avgCritsPerHit;
                    if (avgHitsToDischarge < 1) avgHitsToDischarge = 1;
                    float boltDps = ThunderBolt.AverageDamage / ((CastTime / HitProcs) * (hitsInsideCooldown + avgHitsToDischarge));
                    DamagePerSecond += boltDps;
                    ThreatPerSecond += boltDps * castingState.NatureThreatMultiplier;
                    //continuous model
                    //DamagePerSecond += LightningBolt.AverageDamage / (2.5f + 4f * CastTime / (CritRate * TargetProcs));
                }
                if (baseStats.ShatteredSunAcumenProc > 0 && !calculationOptions.Aldor)
                {
                    BaseSpell ArcaneBolt = castingState.ArcaneBolt;
                    float boltDps = ArcaneBolt.AverageDamage / (45f + CastTime / HitProcs / 0.1f);
                    DamagePerSecond += boltDps;
                    ThreatPerSecond += boltDps * castingState.ArcaneThreatMultiplier;
                }
                if (baseStats.PendulumOfTelluricCurrentsProc > 0)
                {
                    BaseSpell PendulumOfTelluricCurrents = castingState.PendulumOfTelluricCurrents;
                    float boltDps = PendulumOfTelluricCurrents.AverageDamage / (45f + CastTime / HitProcs / 0.15f);
                    DamagePerSecond += boltDps;
                    ThreatPerSecond += boltDps * castingState.ShadowThreatMultiplier;
                }
                if (baseStats.LightweaveEmbroideryProc > 0)
                {
                    BaseSpell LightweaveBolt = castingState.LightweaveBolt;
                    float boltDps = LightweaveBolt.AverageDamage / (45f + CastTime / HitProcs / 0.5f);
                    DamagePerSecond += boltDps;
                    ThreatPerSecond += boltDps * castingState.HolyThreatMultiplier;
                }
            }
            /*float casttimeHash = castingState.ClearcastingChance * 100 + CastTime;
            float OO5SR = 0;
            if (!FSRCalc.TryGetCachedOO5SR(Name, casttimeHash, out OO5SR))
            {
                FSRCalc fsr = new FSRCalc();
                fsr.AddSpell(CastTime - castingState.Latency, castingState.Latency, Channeled);
                OO5SR = fsr.CalculateOO5SR(castingState.ClearcastingChance, Name, casttimeHash);
            }*/

            float OO5SR = 0;

            if (outOfFiveSecondRule)
            {
                OO5SR = 1;
            }

            /*if (Cost > 0)
            {
                OO5SR = FSRCalc.CalculateSimpleOO5SR(castingState.ClearcastingChance, CastTime - castingState.Latency, castingState.Latency, Channeled);
            }*/

            ManaRegenPerSecond = castingState.ManaRegen5SR + OO5SR * (castingState.ManaRegen - castingState.ManaRegen5SR) + baseStats.ManaRestoreFromBaseManaPerHit * 3268 / CastTime * HitProcs + baseStats.ManaRestorePerCast * CastProcs / CastTime + baseStats.ManaRestoreOnCrit_25_45 / (45f + CastTime / HitProcs / CritRate / 0.25f) + baseStats.ManaRestoreOnCast_5_15 / (15f + CastTime / CastProcs / 0.05f) + baseStats.ManaRestoreOnCast_10_45 / (45f + CastTime / CastProcs / 0.1f);
            if (castingState.WaterElemental)
            {
                ManaRegenPerSecond += 0.002f * baseStats.Mana / 5.0f * mageTalents.ImprovedWaterElemental;
            }
            ThreatPerSecond += (baseStats.ManaRestoreFromBaseManaPerHit * 3268 / CastTime * HitProcs + baseStats.ManaRestorePerCast * CastProcs / CastTime) * 0.5f * (1 + baseStats.ThreatIncreaseMultiplier) * (1 - baseStats.ThreatReductionMultiplier);

            if (castingState.Mp5OnCastFor20Sec > 0 && CastProcs > 0)
            {
                float totalMana = castingState.Mp5OnCastFor20Sec / 5f / CastTime * 0.5f * (20 - CastTime / HitProcs / 2f) * (20 - CastTime / HitProcs / 2f);
                ManaRegenPerSecond += totalMana / 20f;
                ThreatPerSecond += totalMana / 20f * 0.5f * (1 + baseStats.ThreatIncreaseMultiplier) * (1 - baseStats.ThreatReductionMultiplier);
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
            if (!EffectProc && castingState.BaseStats.ThunderCapacitorProc > 0)
            {
                BaseSpell ThunderBolt = castingState.ThunderBolt;
                if (!dict.TryGetValue(ThunderBolt.Name, out contrib))
                {
                    contrib = new SpellContribution() { Name = ThunderBolt.Name };
                    dict[ThunderBolt.Name] = contrib;
                }
                //discrete model
                int hitsInsideCooldown = (int)(2.5f / (CastTime / HitProcs));
                float avgCritsPerHit = CritRate * TargetProcs / HitProcs;
                float avgHitsToDischarge = 4f / avgCritsPerHit;
                if (avgHitsToDischarge < 1) avgHitsToDischarge = 1;
                float boltDps = ThunderBolt.AverageDamage / ((CastTime / HitProcs) * (hitsInsideCooldown + avgHitsToDischarge));
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
            if (!EffectProc && castingState.BaseStats.PendulumOfTelluricCurrentsProc > 0)
            {
                BaseSpell PendulumOfTelluricCurrents = castingState.PendulumOfTelluricCurrents;
                if (!dict.TryGetValue(PendulumOfTelluricCurrents.Name, out contrib))
                {
                    contrib = new SpellContribution() { Name = PendulumOfTelluricCurrents.Name };
                    dict[PendulumOfTelluricCurrents.Name] = contrib;
                }
                float boltDps = PendulumOfTelluricCurrents.AverageDamage / (45f + CastTime / HitProcs / 0.15f);
                contrib.Hits += duration / (45f + CastTime / HitProcs / 0.15f);
                contrib.Damage += boltDps * duration;
            }
            if (!EffectProc && castingState.BaseStats.LightweaveEmbroideryProc > 0)
            {
                BaseSpell LightweaveBolt = castingState.LightweaveBolt;
                if (!dict.TryGetValue(LightweaveBolt.Name, out contrib))
                {
                    contrib = new SpellContribution() { Name = LightweaveBolt.Name };
                    dict[LightweaveBolt.Name] = contrib;
                }
                float boltDps = LightweaveBolt.AverageDamage / (45f + CastTime / HitProcs / 0.5f);
                contrib.Hits += duration / (45f + CastTime / HitProcs / 0.5f);
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
            float spellCrit = 0.05f;
            if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Totem of Wrath"))) spellCrit += 0.03f;
            float hitRate = castingState.FrostHitRate;
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
                    SpellModifier *= (1 + castingState.BaseStats.BonusArcaneDamageMultiplier);
                    break;
                case MagicSchool.Fire:
                    SpellModifier *= (1 + castingState.BaseStats.BonusFireDamageMultiplier);
                    break;
                case MagicSchool.Frost:
                    SpellModifier *= (1 + castingState.BaseStats.BonusFrostDamageMultiplier);
                    break;
                case MagicSchool.Nature:
                    SpellModifier *= (1 + castingState.BaseStats.BonusNatureDamageMultiplier);
                    break;
                case MagicSchool.Shadow:
                    SpellModifier *= (1 + castingState.BaseStats.BonusShadowDamageMultiplier);
                    break;
            }
            CalculateDerivedStats(castingState, true);
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
            SpellModifier *= (1 + 0.02f * castingState.MageTalents.SpellImpact + 0.02f * castingState.MageTalents.FirePower) / (1 + 0.02f * castingState.MageTalents.FirePower);
            CalculateDerivedStats(castingState, false);
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
            SpellModifier *= (1 + 0.02f * castingState.MageTalents.SpellImpact + 0.02f * castingState.MageTalents.FirePower) / (1 + 0.02f * castingState.MageTalents.FirePower);
            CalculateDerivedStats(castingState, false);
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
            AoeDamageCap = 37500;
            CritRate += 0.02f * castingState.MageTalents.WorldInFlames ;
            CalculateDerivedStats(castingState, false);
        }
    }

    public class ConjureManaGem : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ConjureManaGem()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.75 * BaseMana[70]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[1] = new SpellData() { Cost = (int)(0.75 * BaseMana[71]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[2] = new SpellData() { Cost = (int)(0.75 * BaseMana[72]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[3] = new SpellData() { Cost = (int)(0.75 * BaseMana[73]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[4] = new SpellData() { Cost = (int)(0.75 * BaseMana[74]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[5] = new SpellData() { Cost = (int)(0.75 * BaseMana[75]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[6] = new SpellData() { Cost = (int)(0.75 * BaseMana[76]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[7] = new SpellData() { Cost = (int)(0.75 * BaseMana[77]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[8] = new SpellData() { Cost = (int)(0.75 * BaseMana[78]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[9] = new SpellData() { Cost = (int)(0.75 * BaseMana[79]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[10] = new SpellData() { Cost = (int)(0.75 * BaseMana[80]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public ConjureManaGem(CastingState castingState)
            : base("Conjure Mana Gem", false, false, false, false, 30, 3, 0, MagicSchool.Arcane, GetMaxRankSpellData(castingState.CalculationOptions), 0, 1)
        {
            base.Calculate(castingState);
            CalculateDerivedStats(castingState, false);
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
            CalculateDerivedStats(castingState, false);
        }
    }

    public class Frostbolt : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static Frostbolt()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.13 * BaseMana[70]), MinDamage = 630, MaxDamage = 680, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[1] = new SpellData() { Cost = (int)(0.13 * BaseMana[71]), MinDamage = 633, MaxDamage = 684, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[2] = new SpellData() { Cost = (int)(0.13 * BaseMana[72]), MinDamage = 637, MaxDamage = 688, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[3] = new SpellData() { Cost = (int)(0.13 * BaseMana[73]), MinDamage = 641, MaxDamage = 692, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[4] = new SpellData() { Cost = (int)(0.13 * BaseMana[74]), MinDamage = 645, MaxDamage = 696, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[5] = new SpellData() { Cost = (int)(0.13 * BaseMana[75]), MinDamage = 702, MaxDamage = 758, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.13 * BaseMana[76]), MinDamage = 706, MaxDamage = 763, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.13 * BaseMana[77]), MinDamage = 710, MaxDamage = 767, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.13 * BaseMana[78]), MinDamage = 714, MaxDamage = 771, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.13 * BaseMana[79]), MinDamage = 799, MaxDamage = 861, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.13 * BaseMana[80]), MinDamage = 803, MaxDamage = 866, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        private bool averageFingersOfFrost;

        public Frostbolt(CastingState castingState, bool manualClearcasting, bool clearcastingActive, bool pom, bool averageFingersOfFrost)
            : base("Frostbolt", false, true, false, false, 30, 3, 0, MagicSchool.Frost, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            this.averageFingersOfFrost = averageFingersOfFrost;
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
            SpellModifier *= (1 + castingState.BaseStats.BonusMageNukeMultiplier) * (1 + 0.04f * castingState.MageTalents.TormentTheWeak * castingState.SnaredTime) * (1 + 0.01f * castingState.MageTalents.ChilledToTheBone);
            if (averageFingersOfFrost)
            {
                float fof = (castingState.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * castingState.MageTalents.FingersOfFrost);
                CritRate += (1.0f - (1.0f - fof) * (1.0f - fof)) * (castingState.MageTalents.Shatter == 3 ? 0.5f : 0.17f * castingState.MageTalents.Shatter);
            }
            CalculateDerivedStats(castingState, false);
        }
    }

    public class Fireball : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static Fireball()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.19 * BaseMana[70]), MinDamage = 717, MaxDamage = 913, PeriodicDamage = 92, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[1] = new SpellData() { Cost = (int)(0.19 * BaseMana[71]), MinDamage = 721, MaxDamage = 918, PeriodicDamage = 92, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[2] = new SpellData() { Cost = (int)(0.19 * BaseMana[72]), MinDamage = 725, MaxDamage = 922, PeriodicDamage = 92, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[3] = new SpellData() { Cost = (int)(0.19 * BaseMana[73]), MinDamage = 729, MaxDamage = 926, PeriodicDamage = 92, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[4] = new SpellData() { Cost = (int)(0.19 * BaseMana[74]), MinDamage = 783, MaxDamage = 997, PeriodicDamage = 100, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[5] = new SpellData() { Cost = (int)(0.19 * BaseMana[75]), MinDamage = 787, MaxDamage = 1002, PeriodicDamage = 100, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.19 * BaseMana[76]), MinDamage = 792, MaxDamage = 1007, PeriodicDamage = 100, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.19 * BaseMana[77]), MinDamage = 796, MaxDamage = 1011, PeriodicDamage = 100, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.19 * BaseMana[78]), MinDamage = 888, MaxDamage = 1132, PeriodicDamage = 116, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.19 * BaseMana[79]), MinDamage = 893, MaxDamage = 1138, PeriodicDamage = 116, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.19 * BaseMana[80]), MinDamage = 898, MaxDamage = 1143, PeriodicDamage = 116, SpellDamageCoefficient = 3.5f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public Fireball(CastingState castingState, bool pom, bool brainFreeze)
            : base("Fireball", false, false, false, false, 35, 3.5f, 0, MagicSchool.Fire, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            if (brainFreeze)
            {
                this.Instant = true;
                this.BaseCastTime = 0.0f;
                this.BaseCost = 0;
            }
            if (pom)
            {
                this.Instant = true;
                this.BaseCastTime = 0.0f;
            }
            Calculate(castingState);
            if (castingState.CalculationOptions.GlyphOfFireball)
            {
                BasePeriodicDamage = 0.0f;
                CritRate += 0.05f;
            }
            SpammedDot = true;
            DotDuration = 8;
            InterruptProtection += castingState.BaseStats.AldorRegaliaInterruptProtection;
            BaseCastTime -= 0.1f * castingState.MageTalents.ImprovedFireball;
            SpellDamageCoefficient += 0.05f * castingState.MageTalents.EmpoweredFire;
            SpellModifier *= (1 + castingState.BaseStats.BonusMageNukeMultiplier);
            SpellModifier *= (1 + 0.02f * castingState.MageTalents.SpellImpact + 0.02f * castingState.MageTalents.FirePower) / (1 + 0.02f * castingState.MageTalents.FirePower) * (1 + 0.04f * castingState.MageTalents.TormentTheWeak * castingState.SnaredTime);
            CalculateDerivedStats(castingState, false);
        }
    }

    public class FrostfireBolt : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static FrostfireBolt()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.14 * BaseMana[70]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0f };
            SpellData[1] = new SpellData() { Cost = (int)(0.14 * BaseMana[71]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0f };
            SpellData[2] = new SpellData() { Cost = (int)(0.14 * BaseMana[72]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0f };
            SpellData[3] = new SpellData() { Cost = (int)(0.14 * BaseMana[73]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0f };
            SpellData[4] = new SpellData() { Cost = (int)(0.14 * BaseMana[74]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0f };
            SpellData[5] = new SpellData() { Cost = (int)(0.14 * BaseMana[75]), MinDamage = 629, MaxDamage = 731, PeriodicDamage = 60, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.14 * BaseMana[76]), MinDamage = 632, MaxDamage = 735, PeriodicDamage = 60, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.14 * BaseMana[77]), MinDamage = 636, MaxDamage = 739, PeriodicDamage = 60, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.14 * BaseMana[78]), MinDamage = 640, MaxDamage = 743, PeriodicDamage = 60, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.14 * BaseMana[79]), MinDamage = 644, MaxDamage = 747, PeriodicDamage = 60, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.14 * BaseMana[80]), MinDamage = 722, MaxDamage = 838, PeriodicDamage = 90, SpellDamageCoefficient = 3.0f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        private bool averageFingersOfFrost;

        public FrostfireBolt(CastingState castingState, bool pom, bool averageFingersOfFrost)
            : base("Frostfire Bolt", false, false, false, false, 40, 3.0f, 0, MagicSchool.FrostFire, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            this.averageFingersOfFrost = averageFingersOfFrost;
            if (pom)
            {
                this.Instant = true;
                this.BaseCastTime = 0.0f;
            }
            Calculate(castingState);
            if (castingState.CalculationOptions.GlyphOfFrostfire)
            {
                CritRate += 0.02f;
                DirectDamageModifier *= 1.02f;
            }
            SpellModifier *= (1 + 0.04f * castingState.MageTalents.TormentTheWeak * castingState.SnaredTime) * (1 + 0.01f * castingState.MageTalents.ChilledToTheBone);
            SpellDamageCoefficient += 0.05f * castingState.MageTalents.EmpoweredFire;
            SpammedDot = true;
            DotDuration = 9;
            if (averageFingersOfFrost)
            {
                float fof = (castingState.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * castingState.MageTalents.FingersOfFrost);
                CritRate += (1.0f - (1.0f - fof) * (1.0f - fof)) * (castingState.MageTalents.Shatter == 3 ? 0.5f : 0.17f * castingState.MageTalents.Shatter);
            }
            CalculateDerivedStats(castingState, false);
        }
    }

    public class Pyroblast : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static Pyroblast()
        {
            // spell data for Pyroblast is not level adjusted except for level 70 and 80, adjust if the needed data is found
            SpellData[0] = new SpellData() { Cost = (int)(0.22 * BaseMana[70]), MinDamage = 939, MaxDamage = 1191, PeriodicDamage = 356, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[1] = new SpellData() { Cost = (int)(0.22 * BaseMana[71]), MinDamage = 939, MaxDamage = 1191, PeriodicDamage = 356, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[2] = new SpellData() { Cost = (int)(0.22 * BaseMana[72]), MinDamage = 939, MaxDamage = 1191, PeriodicDamage = 356, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[3] = new SpellData() { Cost = (int)(0.22 * BaseMana[73]), MinDamage = 1014, MaxDamage = 1286, PeriodicDamage = 384, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[4] = new SpellData() { Cost = (int)(0.22 * BaseMana[74]), MinDamage = 1014, MaxDamage = 1286, PeriodicDamage = 384, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[5] = new SpellData() { Cost = (int)(0.22 * BaseMana[75]), MinDamage = 1014, MaxDamage = 1286, PeriodicDamage = 384, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[6] = new SpellData() { Cost = (int)(0.22 * BaseMana[76]), MinDamage = 1014, MaxDamage = 1286, PeriodicDamage = 384, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[7] = new SpellData() { Cost = (int)(0.22 * BaseMana[77]), MinDamage = 1190, MaxDamage = 1510, PeriodicDamage = 452, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[8] = new SpellData() { Cost = (int)(0.22 * BaseMana[78]), MinDamage = 1190, MaxDamage = 1510, PeriodicDamage = 452, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[9] = new SpellData() { Cost = (int)(0.22 * BaseMana[79]), MinDamage = 1190, MaxDamage = 1510, PeriodicDamage = 452, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[10] = new SpellData() { Cost = (int)(0.22 * BaseMana[80]), MinDamage = 1210, MaxDamage = 1531, PeriodicDamage = 452, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public Pyroblast(CastingState castingState, bool pom)
            : base("Pyroblast", false, false, false, false, 35, 5f, 0, MagicSchool.Fire, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            if (pom)
            {
                this.Instant = true;
                this.BaseCastTime = 0.0f;
            }
            Calculate(castingState);
            SpammedDot = false;
            DotDuration = 12;
            CritRate += 0.02f * castingState.MageTalents.WorldInFlames;
            CalculateDerivedStats(castingState, false);
        }
    }

    public class LivingBomb : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static LivingBomb()
        {
            // spell data for Living Bomb is not level adjusted except for level 70 and 80, adjust if the needed data is found
            SpellData[0] = new SpellData() { Cost = (int)(0.22 * BaseMana[70]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[1] = new SpellData() { Cost = (int)(0.22 * BaseMana[71]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[2] = new SpellData() { Cost = (int)(0.22 * BaseMana[72]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[3] = new SpellData() { Cost = (int)(0.22 * BaseMana[73]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[4] = new SpellData() { Cost = (int)(0.22 * BaseMana[74]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[5] = new SpellData() { Cost = (int)(0.22 * BaseMana[75]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[6] = new SpellData() { Cost = (int)(0.22 * BaseMana[76]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[7] = new SpellData() { Cost = (int)(0.22 * BaseMana[77]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[8] = new SpellData() { Cost = (int)(0.22 * BaseMana[78]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[9] = new SpellData() { Cost = (int)(0.22 * BaseMana[79]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[10] = new SpellData() { Cost = (int)(0.22 * BaseMana[80]), MinDamage = 690, MaxDamage = 690, PeriodicDamage = 1380, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public LivingBomb(CastingState castingState)
            : base("Living Bomb", false, false, true, false, 35, 0f, 0, MagicSchool.Fire, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            Calculate(castingState);
            SpammedDot = false;
            DotDuration = 12;
            CritRate += 0.02f * castingState.MageTalents.WorldInFlames;
            SpellModifier /= (1 + 0.02f * castingState.MageTalents.FirePower); // Living Bomb dot does not benefit from Fire Power
            DirectDamageModifier *= (1 + 0.02f * castingState.MageTalents.FirePower);
            CalculateDerivedStats(castingState, false);
        }
    }

    public class Slow : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static Slow()
        {
            // spell data for Living Bomb is not level adjusted except for level 70 and 80, adjust if the needed data is found
            SpellData[0] = new SpellData() { Cost = (int)(0.12 * BaseMana[70]) };
            SpellData[1] = new SpellData() { Cost = (int)(0.12 * BaseMana[71]) };
            SpellData[2] = new SpellData() { Cost = (int)(0.12 * BaseMana[72]) };
            SpellData[3] = new SpellData() { Cost = (int)(0.12 * BaseMana[73]) };
            SpellData[4] = new SpellData() { Cost = (int)(0.12 * BaseMana[74]) };
            SpellData[5] = new SpellData() { Cost = (int)(0.12 * BaseMana[75]) };
            SpellData[6] = new SpellData() { Cost = (int)(0.12 * BaseMana[76]) };
            SpellData[7] = new SpellData() { Cost = (int)(0.12 * BaseMana[77]) };
            SpellData[8] = new SpellData() { Cost = (int)(0.12 * BaseMana[78]) };
            SpellData[9] = new SpellData() { Cost = (int)(0.12 * BaseMana[79]) };
            SpellData[10] = new SpellData() { Cost = (int)(0.12 * BaseMana[80]) };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public Slow(CastingState castingState)
            : base("Slow", false, false, true, false, 30, 0f, 0, MagicSchool.Arcane, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            Calculate(castingState);
            CalculateDerivedStats(castingState, false);
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
            Cooldown *= (1 - 0.07f * castingState.MageTalents.IceFloes + (castingState.MageTalents.IceFloes == 3 ? 0.01f : 0.00f));
            AoeDamageCap = 37500;
            int ImprovedConeOfCold = castingState.MageTalents.ImprovedConeOfCold;
            SpellModifier *= (1 + ((ImprovedConeOfCold > 0) ? (0.05f + 0.1f * ImprovedConeOfCold) : 0)) * (1 + 0.02f * castingState.MageTalents.SpellImpact);
            CritRate += 0.02f * castingState.MageTalents.Incineration;
            CalculateDerivedStats(castingState, false);
        }
    }

    public class IceLance : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static IceLance()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.07 * BaseMana[70]), MinDamage = 161, MaxDamage = 187, SpellDamageCoefficient = 0.1429f };
            SpellData[1] = new SpellData() { Cost = (int)(0.07 * BaseMana[71]), MinDamage = 161, MaxDamage = 187, SpellDamageCoefficient = 0.1429f };
            SpellData[2] = new SpellData() { Cost = (int)(0.07 * BaseMana[72]), MinDamage = 182, MaxDamage = 210, SpellDamageCoefficient = 0.1429f };
            SpellData[3] = new SpellData() { Cost = (int)(0.07 * BaseMana[73]), MinDamage = 182, MaxDamage = 210, SpellDamageCoefficient = 0.1429f };
            SpellData[4] = new SpellData() { Cost = (int)(0.07 * BaseMana[74]), MinDamage = 182, MaxDamage = 210, SpellDamageCoefficient = 0.1429f };
            SpellData[5] = new SpellData() { Cost = (int)(0.07 * BaseMana[75]), MinDamage = 182, MaxDamage = 210, SpellDamageCoefficient = 0.1429f };
            SpellData[6] = new SpellData() { Cost = (int)(0.07 * BaseMana[76]), MinDamage = 182, MaxDamage = 210, SpellDamageCoefficient = 0.1429f };
            SpellData[7] = new SpellData() { Cost = (int)(0.07 * BaseMana[77]), MinDamage = 182, MaxDamage = 210, SpellDamageCoefficient = 0.1429f };
            SpellData[8] = new SpellData() { Cost = (int)(0.07 * BaseMana[78]), MinDamage = 221, MaxDamage = 255, SpellDamageCoefficient = 0.1429f };
            SpellData[9] = new SpellData() { Cost = (int)(0.07 * BaseMana[79]), MinDamage = 221, MaxDamage = 255, SpellDamageCoefficient = 0.1429f };
            SpellData[10] = new SpellData() { Cost = (int)(0.07 * BaseMana[80]), MinDamage = 223, MaxDamage = 258, SpellDamageCoefficient = 0.1429f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public IceLance(CastingState castingState)
            : base("Ice Lance", false, false, true, false, 30, 0, 0, MagicSchool.Frost, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            base.Calculate(castingState);
            SpellModifier *= (1 + 0.02f * castingState.MageTalents.SpellImpact) * (1 + 0.01f * castingState.MageTalents.ChilledToTheBone);
            if (castingState.Frozen) SpellModifier *= 3;
            CalculateDerivedStats(castingState, false);
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
            SpellData[10] = new SpellData() { Cost = (int)(0.18 * BaseMana[80]), MinDamage = 936, MaxDamage = 1144, SpellDamageCoefficient = 2.5f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public ArcaneBarrage(CastingState castingState, float arcaneBlastDebuff)
            : base("Arcane Barrage", false, false, true, false, 30, 0, 3, MagicSchool.Arcane, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            Calculate(castingState);
            SpellModifier *= (1 + 0.04f * castingState.MageTalents.TormentTheWeak * castingState.SnaredTime) * (1 + (castingState.CalculationOptions.GlyphOfArcaneBlast ? 0.18f : 0.15f) * arcaneBlastDebuff);
            CalculateDerivedStats(castingState, false);
        }
    }

    public class ArcaneBlast : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ArcaneBlast()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.08 * BaseMana[70]), MinDamage = 668, MaxDamage = 772, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[1] = new SpellData() { Cost = (int)(0.08 * BaseMana[71]), MinDamage = 690, MaxDamage = 800, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[2] = new SpellData() { Cost = (int)(0.08 * BaseMana[72]), MinDamage = 695, MaxDamage = 806, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[3] = new SpellData() { Cost = (int)(0.08 * BaseMana[73]), MinDamage = 700, MaxDamage = 811, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[4] = new SpellData() { Cost = (int)(0.08 * BaseMana[74]), MinDamage = 705, MaxDamage = 816, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[5] = new SpellData() { Cost = (int)(0.08 * BaseMana[75]), MinDamage = 711, MaxDamage = 822, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.08 * BaseMana[76]), MinDamage = 805, MaxDamage = 935, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.08 * BaseMana[77]), MinDamage = 811, MaxDamage = 942, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.08 * BaseMana[78]), MinDamage = 817, MaxDamage = 948, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.08 * BaseMana[79]), MinDamage = 823, MaxDamage = 954, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.08 * BaseMana[80]), MinDamage = 1185, MaxDamage = 1377, SpellDamageCoefficient = 2.5f / 3.5f };
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
            CostModifier += 2.00f * costDebuff + castingState.BaseStats.ArcaneBlastBonus;
            SpellModifier *= (1 + castingState.BaseStats.ArcaneBlastBonus + (castingState.CalculationOptions.GlyphOfArcaneBlast ? 0.18f : 0.15f) * timeDebuff + 0.02f * castingState.MageTalents.SpellImpact); // TODO verify how spell impact stacks
            SpellModifier *= (1 + 0.04f * castingState.MageTalents.TormentTheWeak * castingState.SnaredTime);
            SpellDamageCoefficient += 0.03f * castingState.MageTalents.ArcaneEmpowerment;
            CritRate += 0.02f * castingState.MageTalents.Incineration;
            CalculateDerivedStats(castingState, false);
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
        private int arcaneBlastDebuff;
        private float ticks;

        public static SpellData[] SpellData = new SpellData[11];
        static ArcaneMissiles()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.31 * BaseMana[70]), MinDamage = 287.9f * 5, MaxDamage = 287.9f * 5, SpellDamageCoefficient = 5f / 3.5f }; // there's some indication that coefficient might be slightly different
            SpellData[1] = new SpellData() { Cost = (int)(0.31 * BaseMana[71]), MinDamage = 289.1f * 5, MaxDamage = 289.1f * 5, SpellDamageCoefficient = 4.67125f / 3.5f }; // some huge downraking style penalty for some reason (seems to be 0.95 * (5/3.5 + 0.45)), for now don't place the coeff on 0.45, just use 4.67125 instead of 4.75
            SpellData[2] = new SpellData() { Cost = (int)(0.31 * BaseMana[72]), MinDamage = 290.8f * 5, MaxDamage = 290.8f * 5, SpellDamageCoefficient = 4.3425f / 3.5f }; // some huge downraking style penalty for some reason (hypothesis 0.9 * (5/3.5 + 0.45), confirmed)
            SpellData[3] = new SpellData() { Cost = (int)(0.31 * BaseMana[73]), MinDamage = 291.9f * 5, MaxDamage = 291.9f * 5, SpellDamageCoefficient = 4.01375f / 3.5f }; // some huge downraking style penalty for some reason (hypothesis 0.85 * (5/3.5 + 0.45), confirmed)
            SpellData[4] = new SpellData() { Cost = (int)(0.31 * BaseMana[74]), MinDamage = 293.0f * 5, MaxDamage = 293.0f * 5, SpellDamageCoefficient = 3.685f / 3.5f }; // some huge downraking style penalty for some reason (hypothesis 0.8 * (5/3.5 + 0.45), confirmed)
            SpellData[5] = new SpellData() { Cost = (int)(0.31 * BaseMana[75]), MinDamage = 320.0f * 5, MaxDamage = 320.0f * 5, SpellDamageCoefficient = 5f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.31 * BaseMana[76]), MinDamage = 321.7f * 5, MaxDamage = 321.7f * 5, SpellDamageCoefficient = 5f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.31 * BaseMana[77]), MinDamage = 323.0f * 5, MaxDamage = 323.0f * 5, SpellDamageCoefficient = 5f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.31 * BaseMana[78]), MinDamage = 324.8f * 5, MaxDamage = 324.8f * 5, SpellDamageCoefficient = 5f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.31 * BaseMana[79]), MinDamage = 360.0f * 5, MaxDamage = 360.0f * 5, SpellDamageCoefficient = 5f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.31 * BaseMana[80]), MinDamage = 361.9f * 5, MaxDamage = 361.9f * 5, SpellDamageCoefficient = 5f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public ArcaneMissiles(CastingState castingState, bool barrage, bool clearcastingAveraged, bool clearcastingActive, bool clearcastingProccing, int arcaneBlastDebuff, float ticks)
            : base("Arcane Missiles", true, false, false, false, 30, 5, 0, MagicSchool.Arcane, GetMaxRankSpellData(castingState.CalculationOptions), 5, 6)
        {
            ManualClearcasting = true;
            ClearcastingActive = clearcastingActive;
            ClearcastingAveraged = clearcastingAveraged;
            ClearcastingProccing = clearcastingProccing;
            this.barrage = barrage;
            this.arcaneBlastDebuff = arcaneBlastDebuff;
            this.ticks = ticks;
            Calculate(castingState);
        }

        public ArcaneMissiles(CastingState castingState, bool barrage, int arcaneBlastDebuff, float ticks)
            : base("Arcane Missiles", true, false, false, false, 30, 5, 0, MagicSchool.Arcane, GetMaxRankSpellData(castingState.CalculationOptions), 5, 6)
        {
            this.barrage = barrage;
            this.arcaneBlastDebuff = arcaneBlastDebuff;
            this.ticks = ticks;
            Calculate(castingState);
        }

        public ArcaneMissiles(CastingState castingState, bool barrage, int arcaneBlastDebuff)
            : base("Arcane Missiles", true, false, false, false, 30, 5, 0, MagicSchool.Arcane, GetMaxRankSpellData(castingState.CalculationOptions), 5, 6)
        {
            this.barrage = barrage;
            this.arcaneBlastDebuff = arcaneBlastDebuff;
            this.ticks = 5;
            Calculate(castingState);
        }

        public override void Calculate(CastingState castingState)
        {
            /*if (castingState.CalculationOptions.UseAMClipping && arcaneBlastDebuff > 0)
            {
                BaseCastTime = 4.0f;
            }*/
            BaseCastTime = ticks;
            base.Calculate(castingState);
            if (castingState.CalculationOptions.GlyphOfArcaneMissiles)
            {
                CritBonus = (1 + (1.5f * (1 + castingState.BaseStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * castingState.MageTalents.SpellPower + 0.1f * castingState.MageTalents.Burnout + castingState.BaseStats.CritBonusDamage + 0.25f));
            }
            if (barrage) BaseCastTime *= 0.5f;
            SpellDamageCoefficient += 0.15f * castingState.MageTalents.ArcaneEmpowerment;
            SpellModifier *= (1 + castingState.BaseStats.BonusMageNukeMultiplier) * (1 + 0.04f * castingState.MageTalents.TormentTheWeak * castingState.SnaredTime) * (1 + (castingState.CalculationOptions.GlyphOfArcaneBlast ? 0.18f : 0.15f) * arcaneBlastDebuff);
            /*if (castingState.CalculationOptions.UseAMClipping && arcaneBlastDebuff > 0)
            {
                SpellModifier *= 4.0f / 5.0f;
            }*/
            SpellModifier *= ticks / 5.0f;
            InterruptProtection += 0.2f * castingState.MageTalents.ArcaneStability;
            CalculateDerivedStats(castingState, false);
        }
    }

    public class ArcaneExplosion : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ArcaneExplosion()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.22 * BaseMana[70]), MinDamage = 377, MaxDamage = 407, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[1] = new SpellData() { Cost = (int)(0.22 * BaseMana[71]), MinDamage = 378, MaxDamage = 409, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[2] = new SpellData() { Cost = (int)(0.22 * BaseMana[72]), MinDamage = 380, MaxDamage = 411, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[3] = new SpellData() { Cost = (int)(0.22 * BaseMana[73]), MinDamage = 381, MaxDamage = 412, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[4] = new SpellData() { Cost = (int)(0.22 * BaseMana[74]), MinDamage = 383, MaxDamage = 414, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[5] = new SpellData() { Cost = (int)(0.22 * BaseMana[75]), MinDamage = 385, MaxDamage = 415, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.22 * BaseMana[76]), MinDamage = 481, MaxDamage = 519, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.22 * BaseMana[77]), MinDamage = 483, MaxDamage = 521, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.22 * BaseMana[78]), MinDamage = 485, MaxDamage = 523, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.22 * BaseMana[79]), MinDamage = 487, MaxDamage = 525, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.22 * BaseMana[80]), MinDamage = 538, MaxDamage = 582, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
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
            SpellModifier *= (1 + 0.02f * castingState.MageTalents.SpellImpact);
            AoeDamageCap = 37500;
            CalculateDerivedStats(castingState, false);
        }
    }

    public class BlastWave : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static BlastWave()
        {
            // spell data for Blast Wave is not level adjusted except for level 70 and 80, adjust if the needed data is found
            SpellData[0] = new SpellData() { Cost = (int)(0.28 * BaseMana[70]), MinDamage = 616, MaxDamage = 724, SpellDamageCoefficient = 0.1929f };
            SpellData[1] = new SpellData() { Cost = (int)(0.28 * BaseMana[71]), MinDamage = 616, MaxDamage = 724, SpellDamageCoefficient = 0.1929f };
            SpellData[2] = new SpellData() { Cost = (int)(0.28 * BaseMana[72]), MinDamage = 616, MaxDamage = 724, SpellDamageCoefficient = 0.1929f };
            SpellData[3] = new SpellData() { Cost = (int)(0.28 * BaseMana[73]), MinDamage = 616, MaxDamage = 724, SpellDamageCoefficient = 0.1929f };
            SpellData[4] = new SpellData() { Cost = (int)(0.28 * BaseMana[74]), MinDamage = 616, MaxDamage = 724, SpellDamageCoefficient = 0.1929f };
            SpellData[5] = new SpellData() { Cost = (int)(0.28 * BaseMana[75]), MinDamage = 882, MaxDamage = 1038, SpellDamageCoefficient = 0.1929f };
            SpellData[6] = new SpellData() { Cost = (int)(0.28 * BaseMana[76]), MinDamage = 882, MaxDamage = 1038, SpellDamageCoefficient = 0.1929f };
            SpellData[7] = new SpellData() { Cost = (int)(0.28 * BaseMana[77]), MinDamage = 882, MaxDamage = 1038, SpellDamageCoefficient = 0.1929f };
            SpellData[8] = new SpellData() { Cost = (int)(0.28 * BaseMana[78]), MinDamage = 882, MaxDamage = 1038, SpellDamageCoefficient = 0.1929f };
            SpellData[9] = new SpellData() { Cost = (int)(0.28 * BaseMana[79]), MinDamage = 882, MaxDamage = 1038, SpellDamageCoefficient = 0.1929f };
            SpellData[10] = new SpellData() { Cost = (int)(0.28 * BaseMana[80]), MinDamage = 1047, MaxDamage = 1233, SpellDamageCoefficient = 0.1929f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public BlastWave(CastingState castingState)
            : base("Blast Wave", false, false, true, true, 0, 0, 30, MagicSchool.Fire, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            base.Calculate(castingState);
            AoeDamageCap = 37500;
            SpellModifier *= (1 + 0.02f * castingState.MageTalents.SpellImpact + 0.02f * castingState.MageTalents.FirePower) / (1 + 0.02f * castingState.MageTalents.FirePower);
            CritRate += 0.02f * castingState.MageTalents.WorldInFlames;
            CalculateDerivedStats(castingState, false);
        }
    }

    public class DragonsBreath : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static DragonsBreath()
        {
            // spell data for Dragon's Breath is not level adjusted except for level 70 and 80, adjust if the needed data is found
            SpellData[0] = new SpellData() { Cost = (int)(0.31 * BaseMana[70]), MinDamage = 680, MaxDamage = 790, SpellDamageCoefficient = 0.1929f };
            SpellData[1] = new SpellData() { Cost = (int)(0.31 * BaseMana[71]), MinDamage = 680, MaxDamage = 790, SpellDamageCoefficient = 0.1929f };
            SpellData[2] = new SpellData() { Cost = (int)(0.31 * BaseMana[72]), MinDamage = 680, MaxDamage = 790, SpellDamageCoefficient = 0.1929f };
            SpellData[3] = new SpellData() { Cost = (int)(0.31 * BaseMana[73]), MinDamage = 680, MaxDamage = 790, SpellDamageCoefficient = 0.1929f };
            SpellData[4] = new SpellData() { Cost = (int)(0.31 * BaseMana[74]), MinDamage = 680, MaxDamage = 790, SpellDamageCoefficient = 0.1929f };
            SpellData[5] = new SpellData() { Cost = (int)(0.31 * BaseMana[75]), MinDamage = 935, MaxDamage = 1085, SpellDamageCoefficient = 0.1929f };
            SpellData[6] = new SpellData() { Cost = (int)(0.31 * BaseMana[76]), MinDamage = 935, MaxDamage = 1085, SpellDamageCoefficient = 0.1929f };
            SpellData[7] = new SpellData() { Cost = (int)(0.31 * BaseMana[77]), MinDamage = 935, MaxDamage = 1085, SpellDamageCoefficient = 0.1929f };
            SpellData[8] = new SpellData() { Cost = (int)(0.31 * BaseMana[78]), MinDamage = 935, MaxDamage = 1085, SpellDamageCoefficient = 0.1929f };
            SpellData[9] = new SpellData() { Cost = (int)(0.31 * BaseMana[79]), MinDamage = 935, MaxDamage = 1085, SpellDamageCoefficient = 0.1929f };
            SpellData[10] = new SpellData() { Cost = (int)(0.31 * BaseMana[80]), MinDamage = 1101, MaxDamage = 1279, SpellDamageCoefficient = 0.1929f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public DragonsBreath(CastingState castingState)
            : base("Dragon's Breath", false, false, true, true, 0, 0, 20, MagicSchool.Fire, GetMaxRankSpellData(castingState.CalculationOptions))
        {
            base.Calculate(castingState);
            AoeDamageCap = 37500;
            CritRate += 0.02f * castingState.MageTalents.WorldInFlames;
            CalculateDerivedStats(castingState, false);
        }
    }

    public class Blizzard : BaseSpell
    {
        public static SpellData[] SpellData = new SpellData[11];
        static Blizzard()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.74 * BaseMana[70]), MinDamage = 2184, MaxDamage = 2184, SpellDamageCoefficient = 1.1429f }; // TODO verify level 70 WotLK data
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
            : base("Blizzard", true, false, false, true, 0, 8, 0, MagicSchool.Frost, GetMaxRankSpellData(castingState.CalculationOptions), 4, 1)
        {
            base.Calculate(castingState);
            CritBonus = (1 + (1.5f * (1 + castingState.BaseStats.BonusSpellCritMultiplier) - 1) * (1 + castingState.MageTalents.IceShards / 3.0f + 0.25f * castingState.MageTalents.SpellPower + castingState.BaseStats.CritBonusDamage)); // special case because it is not affected by Burnout
            if (castingState.MageTalents.ImprovedBlizzard > 0)
            {
                float fof = (castingState.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * castingState.MageTalents.FingersOfFrost);
                fof = Math.Max(fof, 0.05f * castingState.MageTalents.Frostbite);
                CritRate += (1.0f - (1.0f - fof) * (1.0f - fof)) * (castingState.MageTalents.Shatter == 3 ? 0.5f : 0.17f * castingState.MageTalents.Shatter);
                //CritRate += (1.0f - (float)Math.Pow(1 - 0.05 * castingState.MageTalents.Frostbite, 5.0 / 2.0)) * (castingState.MageTalents.Shatter == 3 ? 0.5f : 0.17f * castingState.MageTalents.Shatter);
            }
            AoeDamageCap = 200000;
            CritRate += 0.02f * castingState.MageTalents.WorldInFlames;
            CalculateDerivedStats(castingState, false);
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
            CalculateDerivedStats(castingState, false);
        }
    }

    // lightning capacitor
    public class ThunderBolt : BaseSpell
    {
        public ThunderBolt(CastingState castingState)
            : base("Lightning Bolt", false, false, true, false, 0, 30, 0, 0, MagicSchool.Nature, 1181, 1371, 0, 0, 0, 0, 0, 0, false)
        {
            EffectProc = true;
            Calculate(castingState);
        }

        public override void Calculate(CastingState castingState)
        {
            base.Calculate(castingState);
            CritBonus = (1 + (1.5f * (1 + castingState.BaseStats.BonusSpellCritMultiplier) - 1)) * castingState.ResilienceCritDamageReduction;
            CalculateDerivedStats(castingState, false);
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
            CalculateDerivedStats(castingState, false);
        }
    }

    // Pendulum of Telluric Currents
    public class PendulumOfTelluricCurrents : BaseSpell
    {
        public PendulumOfTelluricCurrents(CastingState castingState)
            : base("Pendulum of Telluric Currents", false, false, true, false, 0, 50, 0, 0, MagicSchool.Shadow, 1168, 1752, 0, 0, 0, 0, 0, 0, false)
        {
            EffectProc = true;
            Calculate(castingState);
        }

        public override void Calculate(CastingState castingState)
        {
            base.Calculate(castingState);
            CritBonus = (1 + (1.5f * (1 + castingState.BaseStats.BonusSpellCritMultiplier) - 1)) * castingState.ResilienceCritDamageReduction;
            CalculateDerivedStats(castingState, false);
        }
    }

    // Lightweave Embroidery
    public class LightweaveBolt : BaseSpell
    {
        public LightweaveBolt(CastingState castingState)
            : base("Lightweave Bolt", false, false, true, false, 0, 50, 0, 0, MagicSchool.Holy, 1000, 1200, 0, 0, 0, 0, 0, 0, false)
        {
            EffectProc = true;
            Calculate(castingState);
        }

        public override void Calculate(CastingState castingState)
        {
            base.Calculate(castingState);
            CritBonus = (1 + (1.5f * (1 + castingState.BaseStats.BonusSpellCritMultiplier) - 1)) * castingState.ResilienceCritDamageReduction;
            CalculateDerivedStats(castingState, false);
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
        public bool recalc5SR;

        private List<Spell> spellList;
        private FSRCalc fsr;

        public SpellCycle()
        {
            spellList = new List<Spell>();
            //fsr = new FSRCalc();
        }

        public SpellCycle(int capacity)
        {
            spellList = new List<Spell>(capacity);
            //fsr = new FSRCalc(capacity);
        }

        public SpellCycle(int capacity, bool recalcFiveSecondRule)
        {
            spellList = new List<Spell>(capacity);
            recalc5SR = recalcFiveSecondRule;
            if (recalc5SR)
            {
                fsr = new FSRCalc(capacity);
            }
        }

        public void AddSpell(Spell spell, CastingState castingState)
        {
            if (recalc5SR)
            {
                fsr.AddSpell(spell.CastTime - castingState.Latency, castingState.Latency, spell.Channeled);
            }
            CastTime += spell.CastTime;
            HitProcs += spell.HitProcs;
            CastProcs += spell.CastProcs;
            CritProcs += spell.CritProcs;
            AverageDamage += spell.DamagePerSecond * spell.CastTime;
            AverageThreat += spell.ThreatPerSecond * spell.CastTime;
            Cost += spell.CostPerSecond * spell.CastTime;
            AffectedByFlameCap = AffectedByFlameCap || spell.AffectedByFlameCap;
            spellList.Add(spell);
            SpellCount++;
        }

        public void AddPause(float duration)
        {
            if (recalc5SR)
            {
                fsr.AddPause(duration);
            }
            CastTime += duration;
            spellList.Add(null);
        }

        public void Calculate(CastingState castingState)
        {
            //CastTime = fsr.Duration;

            CostPerSecond = Cost / CastTime;
            DamagePerSecond = AverageDamage / CastTime;
            ThreatPerSecond = AverageThreat / CastTime;

            float OO5SR = 0;

            if (recalc5SR)
            {
                OO5SR = fsr.CalculateOO5SR(castingState.ClearcastingChance);
            }

            ManaRegenPerSecond = castingState.ManaRegen5SR + OO5SR * (castingState.ManaRegen - castingState.ManaRegen5SR) + castingState.BaseStats.ManaRestoreFromBaseManaPerHit * 3268 / CastTime * HitProcs + castingState.BaseStats.ManaRestorePerCast * CastProcs / CastTime + castingState.BaseStats.ManaRestoreOnCrit_25_45 / (45f + CastTime / CritProcs / 0.25f) + castingState.BaseStats.ManaRestoreOnCast_5_15 / (15f + CastTime / CastProcs / 0.05f) + castingState.BaseStats.ManaRestoreOnCast_10_45 / (45f + CastTime / CastProcs / 0.1f);

            if (castingState.Mp5OnCastFor20Sec > 0)
            {
                float averageCastTime = CastTime / SpellCount;
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

            AMc1 = new ArcaneMissiles(castingState, false, true, false, true, 0, 5);
            AM10 = new ArcaneMissiles(castingState, false, false, true, false, 0, 5);
            AM11 = new ArcaneMissiles(castingState, false, false, true, true, 0, 5);

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

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles1);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB1);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to AB-AM
                chain1 = new SpellCycle(2, true);
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
                chain1 = new SpellCycle(2, true);
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

    class AB3AM : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB;
        float K1, K2;

        public AB3AM(CastingState castingState)
        {
            Name = "AB3AM";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            K1 = (1 - MB) * (1 - MB) * (1 - MB);
            K2 = 1 - (1 - MB) * (1 - MB) * (1 - MB);

            //AB-AM 0.85
            chain1 = new SpellCycle(4);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AB2, castingState);
            chain1.AddSpell(AM3, castingState);
            chain1.Calculate(castingState);

            //AB-MBAM 0.15
            chain2 = new SpellCycle(4);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            chain2.AddSpell(MBAM3, castingState);
            chain2.Calculate(castingState);

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, K1 * chain1.CastTime / CastTime * duration);
            chain2.AddSpellContribution(dict, K2 * chain2.CastTime / CastTime * duration);
        }
    }

    class AB3AM2MBAM : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        SpellCycle chain3;
        float MB;
        float K1, K2, K3;

        public AB3AM2MBAM(CastingState castingState)
        {
            Name = "AB3AM2MBAM";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            K1 = (1 - MB) * (1 - MB) * (1 - MB);
            K2 = (1 - MB) * (1 - (1 - MB) * (1 - MB));
            K3 = MB;

            //AB0-AB1-AB2-AM3      (1-MB)*(1-MB)*(1-MB)
            chain1 = new SpellCycle(4);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AB2, castingState);
            chain1.AddSpell(AM3, castingState);
            chain1.Calculate(castingState);

            //AB0-AB1-AB2-MBAM3    (1-MB)*(1 - (1-MB)*(1-MB))
            chain2 = new SpellCycle(4);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            chain2.AddSpell(MBAM3, castingState);
            chain2.Calculate(castingState);

            //AB0-AB1-MBAM2        MB
            chain3 = new SpellCycle(4);
            chain3.AddSpell(AB0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(MBAM2, castingState);
            chain3.Calculate(castingState);

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime + K3 * chain3.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond + K3 * chain3.CastTime * chain3.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond + K3 * chain3.CastTime * chain3.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond + K3 * chain3.CastTime * chain3.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond + K3 * chain3.CastTime * chain3.ManaRegenPerSecond) / CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, K1 * chain1.CastTime / CastTime * duration);
            chain2.AddSpellContribution(dict, K2 * chain2.CastTime / CastTime * duration);
            chain2.AddSpellContribution(dict, K3 * chain3.CastTime / CastTime * duration);
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

            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to ABar-AM
                chain1 = new SpellCycle(2, true);
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
                chain1 = new SpellCycle(2, true);
                chain1.AddSpell(ABar, castingState);
                chain1.AddSpell(AM, castingState);
                chain1.Calculate(castingState);

                //AB-MBAM 0.15
                chain2 = new SpellCycle(2);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                if (ABar.CastTime + MBAM.CastTime < 3.0) chain2.AddPause(3.0f - ABar.CastTime - MBAM.CastTime);
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

    class ABABarSlow : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        SpellCycle chain3;
        SpellCycle chain4;
        float MB;
        float X;
        float S0;
        float S1;

        public ABABarSlow(CastingState castingState)
        {
            // TODO not updated for 3.0.8 mode, consider deprecated?
            Name = "ABABarSlow";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell ABar = castingState.MaintainSnareState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.MaintainSnareState.GetSpell(SpellId.ArcaneMissilesMB);
            Spell Slow = castingState.GetSpell(SpellId.Slow);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            //S0
            //AB-Pause-ABar     1 - X  (1 - MB) * (1 - MB) => S0, 1 - (1 - MB) * (1 - MB) => S1
            //AB-Slow-ABar      X
             
            //S1
            //MBAM-Pause-ABar   1 - X  (1 - MB) => S0, MB => S1
            //MBAM-Slow-ABar    X
             
            //S0 + S1 = 1
            //S0a = (1 - X) * ((1 - MB) * (1 - MB) * S0 + (1 - MB) * S1)
            //S0b = X * ((1 - MB) * (1 - MB) * S0 + (1 - MB) * S1)
            //S1a = (1 - X) * ((1 - (1 - MB) * (1 - MB)) * S0 + MB * S1)
            //S1b = X * ((1 - (1 - MB) * (1 - MB)) * S0 + MB * S1)
             
            //S0 = (1 - MB) * (1 - MB) * S0 + (1 - MB) * S1
            //S1 = (1 - (1 - MB) * (1 - MB)) * S0 + MB * S1
             
            //S0 * (1 - (1 - MB) * (1 - MB) + (1 - MB)) = (1 - MB)
             
            //S0 = (1 - MB) / (1 + (1 - MB) * MB)
            //S1 = (2 - MB) * MB / (1 + (1 - MB) * MB)
             
            //time casting slow / all time casting = time(Slow) / 15

            //value = (1 - X) * S0 * (value(AB) + value(ABar) + value(Pause)) + X * S0 * (value(AB) + value(ABar) + value(Slow)) + (1 - X) * S1 * (value(MBAM) + value(ABar) + value(Pause)) * X * S1 * (value(MBAM) + value(ABar) + value(Slow))
            //value = S0 * value(AB-ABar) + X * S0 * value(Slow) + S1 * value(MBAM-ABar) + X * S1 * value(Slow) + (1 - X) * value(Pause)
            //value = S0 * value(AB-ABar) + S1 * value(MBAM-ABar) + X * value(Slow) + (1 - X) * value(Pause)

            // X = (S0 * time(AB-ABar) + S1 * time(MBAM-ABar) + time(Pause)) / (15 - time(Slow) + time(Pause))

            S0 = (1 - MB) / (1 + (1 - MB) * MB);
            S1 = (2 - MB) * MB / (1 + (1 - MB) * MB);
            float Pause = 0.0f;
            if (AB.CastTime + ABar.CastTime < 3.0) Pause = 3.0f + castingState.CalculationOptions.Latency - AB.CastTime - ABar.CastTime;
            X = (S0 * (AB.CastTime + ABar.CastTime) + S1 * (MBAM.CastTime + ABar.CastTime) + Pause) / (15.0f - Slow.CastTime + Pause);

            //AB-ABar
            chain1 = new SpellCycle(2);
            chain1.AddSpell(ABar, castingState);
            chain1.AddSpell(AB, castingState);
            if (AB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB.CastTime - ABar.CastTime);
            chain1.Calculate(castingState);

            //ABar-MBAM
            chain2 = new SpellCycle(2);
            chain2.AddSpell(ABar, castingState);
            chain2.AddSpell(MBAM, castingState);
            if (MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime);
            chain2.Calculate(castingState);

            //AB-ABar-Slow
            chain3 = new SpellCycle(3);
            chain3.AddSpell(ABar, castingState);
            chain3.AddSpell(AB, castingState);
            chain3.AddSpell(Slow, castingState);
            if (AB.CastTime + ABar.CastTime + Slow.CastTime < 3.0) chain3.AddPause(3.0f + castingState.CalculationOptions.Latency - AB.CastTime - ABar.CastTime - Slow.CastTime);
            chain3.Calculate(castingState);

            //ABar-MBAM-Slow
            chain4 = new SpellCycle(3);
            chain4.AddSpell(ABar, castingState);
            chain4.AddSpell(MBAM, castingState);
            chain4.AddSpell(Slow, castingState);
            if (MBAM.CastTime + ABar.CastTime + Slow.CastTime < 3.0) chain4.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime - Slow.CastTime);
            chain4.Calculate(castingState);

            commonChain = chain1;

            CastTime = (1 - X) * S0 * chain1.CastTime + (1 - X) * S1 * chain2.CastTime + X * S0 * chain3.CastTime + X * S1 * chain4.CastTime;
            CostPerSecond = ((1 - X) * S0 * chain1.CastTime * chain1.CostPerSecond + (1 - X) * S1 * chain2.CastTime * chain2.CostPerSecond + X * S0 * chain3.CastTime * chain3.CostPerSecond + X * S1 * chain4.CastTime * chain4.CostPerSecond) / CastTime;
            DamagePerSecond = ((1 - X) * S0 * chain1.CastTime * chain1.DamagePerSecond + (1 - X) * S1 * chain2.CastTime * chain2.DamagePerSecond + X * S0 * chain3.CastTime * chain3.DamagePerSecond + X * S1 * chain4.CastTime * chain4.DamagePerSecond) / CastTime;
            ThreatPerSecond = ((1 - X) * S0 * chain1.CastTime * chain1.ThreatPerSecond + (1 - X) * S1 * chain2.CastTime * chain2.ThreatPerSecond + X * S0 * chain3.CastTime * chain3.ThreatPerSecond + X * S1 * chain4.CastTime * chain4.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = ((1 - X) * S0 * chain1.CastTime * chain1.ManaRegenPerSecond + (1 - X) * S1 * chain2.CastTime * chain2.ManaRegenPerSecond + X * S0 * chain3.CastTime * chain3.ManaRegenPerSecond + X * S1 * chain4.CastTime * chain4.ManaRegenPerSecond) / CastTime;
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
            chain1.AddSpellContribution(dict, (1 - X) * S0 * chain1.CastTime / CastTime * duration);
            chain2.AddSpellContribution(dict, (1 - X) * S1 * chain2.CastTime / CastTime * duration);
            chain3.AddSpellContribution(dict, X * S0 * chain3.CastTime / CastTime * duration);
            chain4.AddSpellContribution(dict, X * S1 * chain4.CastTime / CastTime * duration);
        }
    }

    class FBABarSlow : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        SpellCycle chain3;
        SpellCycle chain4;
        float MB;
        float X;
        float S0;
        float S1;

        public FBABarSlow(CastingState castingState)
        {
            Name = "FBABarSlow";
            AffectedByFlameCap = true;

            Spell FB = castingState.MaintainSnareState.GetSpell(SpellId.Fireball);
            Spell ABar = castingState.MaintainSnareState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.MaintainSnareState.GetSpell(SpellId.ArcaneMissilesMB);
            Spell Slow = castingState.GetSpell(SpellId.Slow);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            S0 = (1 - MB) / (1 + (1 - MB) * MB);
            S1 = (2 - MB) * MB / (1 + (1 - MB) * MB);
            float Pause = 0.0f;
            if (FB.CastTime + ABar.CastTime < 3.0) Pause = 3.0f + castingState.CalculationOptions.Latency - FB.CastTime - ABar.CastTime;
            X = (S0 * (FB.CastTime + ABar.CastTime) + S1 * (MBAM.CastTime + ABar.CastTime) + Pause) / (15.0f - Slow.CastTime + Pause);

            //AB-ABar
            chain1 = new SpellCycle(2);
            chain1.AddSpell(ABar, castingState);
            chain1.AddSpell(FB, castingState);
            if (FB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FB.CastTime - ABar.CastTime);
            chain1.Calculate(castingState);

            //ABar-MBAM
            chain2 = new SpellCycle(2);
            chain2.AddSpell(ABar, castingState);
            chain2.AddSpell(MBAM, castingState);
            if (MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime);
            chain2.Calculate(castingState);

            //AB-ABar-Slow
            chain3 = new SpellCycle(3);
            chain3.AddSpell(ABar, castingState);
            chain3.AddSpell(FB, castingState);
            chain3.AddSpell(Slow, castingState);
            if (FB.CastTime + ABar.CastTime + Slow.CastTime < 3.0) chain3.AddPause(3.0f + castingState.CalculationOptions.Latency - FB.CastTime - ABar.CastTime - Slow.CastTime);
            chain3.Calculate(castingState);

            //ABar-MBAM-Slow
            chain4 = new SpellCycle(3);
            chain4.AddSpell(ABar, castingState);
            chain4.AddSpell(MBAM, castingState);
            chain4.AddSpell(Slow, castingState);
            if (MBAM.CastTime + ABar.CastTime + Slow.CastTime < 3.0) chain4.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime - Slow.CastTime);
            chain4.Calculate(castingState);

            commonChain = chain1;

            CastTime = (1 - X) * S0 * chain1.CastTime + (1 - X) * S1 * chain2.CastTime + X * S0 * chain3.CastTime + X * S1 * chain4.CastTime;
            CostPerSecond = ((1 - X) * S0 * chain1.CastTime * chain1.CostPerSecond + (1 - X) * S1 * chain2.CastTime * chain2.CostPerSecond + X * S0 * chain3.CastTime * chain3.CostPerSecond + X * S1 * chain4.CastTime * chain4.CostPerSecond) / CastTime;
            DamagePerSecond = ((1 - X) * S0 * chain1.CastTime * chain1.DamagePerSecond + (1 - X) * S1 * chain2.CastTime * chain2.DamagePerSecond + X * S0 * chain3.CastTime * chain3.DamagePerSecond + X * S1 * chain4.CastTime * chain4.DamagePerSecond) / CastTime;
            ThreatPerSecond = ((1 - X) * S0 * chain1.CastTime * chain1.ThreatPerSecond + (1 - X) * S1 * chain2.CastTime * chain2.ThreatPerSecond + X * S0 * chain3.CastTime * chain3.ThreatPerSecond + X * S1 * chain4.CastTime * chain4.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = ((1 - X) * S0 * chain1.CastTime * chain1.ManaRegenPerSecond + (1 - X) * S1 * chain2.CastTime * chain2.ManaRegenPerSecond + X * S0 * chain3.CastTime * chain3.ManaRegenPerSecond + X * S1 * chain4.CastTime * chain4.ManaRegenPerSecond) / CastTime;
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
            chain1.AddSpellContribution(dict, (1 - X) * S0 * chain1.CastTime / CastTime * duration);
            chain2.AddSpellContribution(dict, (1 - X) * S1 * chain2.CastTime / CastTime * duration);
            chain3.AddSpellContribution(dict, X * S0 * chain3.CastTime / CastTime * duration);
            chain4.AddSpellContribution(dict, X * S1 * chain4.CastTime / CastTime * duration);
        }
    }

    class FrBABarSlow : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        SpellCycle chain3;
        SpellCycle chain4;
        float MB;
        float X;
        float S0;
        float S1;

        public FrBABarSlow(CastingState castingState)
        {
            Name = "FrBABarSlow";

            Spell FrB = castingState.MaintainSnareState.GetSpell(SpellId.Frostbolt);
            Spell ABar = castingState.MaintainSnareState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.MaintainSnareState.GetSpell(SpellId.ArcaneMissilesMB);
            Spell Slow = castingState.GetSpell(SpellId.Slow);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            S0 = (1 - MB) / (1 + (1 - MB) * MB);
            S1 = (2 - MB) * MB / (1 + (1 - MB) * MB);
            float Pause = 0.0f;
            if (FrB.CastTime + ABar.CastTime < 3.0) Pause = 3.0f + castingState.CalculationOptions.Latency - FrB.CastTime - ABar.CastTime;
            X = (S0 * (FrB.CastTime + ABar.CastTime) + S1 * (MBAM.CastTime + ABar.CastTime) + Pause) / (15.0f - Slow.CastTime + Pause);

            //AB-ABar
            chain1 = new SpellCycle(2);
            chain1.AddSpell(ABar, castingState);
            chain1.AddSpell(FrB, castingState);
            if (FrB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FrB.CastTime - ABar.CastTime);
            chain1.Calculate(castingState);

            //ABar-MBAM
            chain2 = new SpellCycle(2);
            chain2.AddSpell(ABar, castingState);
            chain2.AddSpell(MBAM, castingState);
            if (MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime);
            chain2.Calculate(castingState);

            //AB-ABar-Slow
            chain3 = new SpellCycle(3);
            chain3.AddSpell(ABar, castingState);
            chain3.AddSpell(FrB, castingState);
            chain3.AddSpell(Slow, castingState);
            if (FrB.CastTime + ABar.CastTime + Slow.CastTime < 3.0) chain3.AddPause(3.0f + castingState.CalculationOptions.Latency - FrB.CastTime - ABar.CastTime - Slow.CastTime);
            chain3.Calculate(castingState);

            //ABar-MBAM-Slow
            chain4 = new SpellCycle(3);
            chain4.AddSpell(ABar, castingState);
            chain4.AddSpell(MBAM, castingState);
            chain4.AddSpell(Slow, castingState);
            if (MBAM.CastTime + ABar.CastTime + Slow.CastTime < 3.0) chain4.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime - Slow.CastTime);
            chain4.Calculate(castingState);

            commonChain = chain1;

            CastTime = (1 - X) * S0 * chain1.CastTime + (1 - X) * S1 * chain2.CastTime + X * S0 * chain3.CastTime + X * S1 * chain4.CastTime;
            CostPerSecond = ((1 - X) * S0 * chain1.CastTime * chain1.CostPerSecond + (1 - X) * S1 * chain2.CastTime * chain2.CostPerSecond + X * S0 * chain3.CastTime * chain3.CostPerSecond + X * S1 * chain4.CastTime * chain4.CostPerSecond) / CastTime;
            DamagePerSecond = ((1 - X) * S0 * chain1.CastTime * chain1.DamagePerSecond + (1 - X) * S1 * chain2.CastTime * chain2.DamagePerSecond + X * S0 * chain3.CastTime * chain3.DamagePerSecond + X * S1 * chain4.CastTime * chain4.DamagePerSecond) / CastTime;
            ThreatPerSecond = ((1 - X) * S0 * chain1.CastTime * chain1.ThreatPerSecond + (1 - X) * S1 * chain2.CastTime * chain2.ThreatPerSecond + X * S0 * chain3.CastTime * chain3.ThreatPerSecond + X * S1 * chain4.CastTime * chain4.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = ((1 - X) * S0 * chain1.CastTime * chain1.ManaRegenPerSecond + (1 - X) * S1 * chain2.CastTime * chain2.ManaRegenPerSecond + X * S0 * chain3.CastTime * chain3.ManaRegenPerSecond + X * S1 * chain4.CastTime * chain4.ManaRegenPerSecond) / CastTime;
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
            chain1.AddSpellContribution(dict, (1 - X) * S0 * chain1.CastTime / CastTime * duration);
            chain2.AddSpellContribution(dict, (1 - X) * S1 * chain2.CastTime / CastTime * duration);
            chain3.AddSpellContribution(dict, X * S0 * chain3.CastTime / CastTime * duration);
            chain4.AddSpellContribution(dict, X * S1 * chain4.CastTime / CastTime * duration);
        }
    }

    class ABABar0C : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB;

        public ABABar0C(CastingState castingState)
        {
            Name = "ABABar0C";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to AB-ABar
                chain1 = new SpellCycle(2);
                chain1.AddSpell(AB, castingState);
                if (AB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB.CastTime - ABar.CastTime);
                chain1.AddSpell(ABar1, castingState);
                chain1.Calculate(castingState);

                CastTime = chain1.CastTime;
                CostPerSecond = chain1.CostPerSecond;
                DamagePerSecond = chain1.DamagePerSecond;
                ThreatPerSecond = chain1.ThreatPerSecond;
                ManaRegenPerSecond = chain1.ManaRegenPerSecond;
            }
            else
            {
                // S0: AB0, MB0
                // ABar-MBAM => S0                MB
                // ABar-AB => S2                  (1-MB)*(1-MB)
                // ABar-AB-ABar1-MBAM => S0       (1-MB)*MB

                // S2: AB1, MB0
                // ABar1-MBAM => S0               MB
                // ABar1-AB => S2                 (1-MB)*(1-MB)
                // ABar1-AB-ABar1-MBAM => S0      (1-MB)*MB

                // S0 + S2 = 1
                // S0 = MB * (2 - MB)
                // S2 = (1-MB)*(1-MB)

                // value = MB * (2 - MB) * [value(ABar) + MB * value(MBAM) + (1-MB) * value(AB) + (1-MB)*MB * value(ABar1) + (1-MB)*MB * value(MBAM)] + (1-MB)*(1-MB) * [value(ABar1) + (MB * value(MBAM) + (1-MB) * value(AB) + (1-MB)*MB * value(ABar1) + (1-MB)*MB * value(MBAM)]
                //       = MB * (2 - MB) * value(ABar) + (1-MB)*(1-MB) * value(ABar1) + MB * value(MBAM) + (1-MB) * value(AB) + (1-MB)*MB * value(ABar1) + (1-MB)*MB * value(MBAM)
                //       = MB * (2 - MB) * value(ABar) + (1 - MB) * value(ABar1) + (1-MB) * value(AB) + (2 - MB) * MB * value(MBAM)

                // this is equivalent to

                // MBAM-ABar (2-MB)*MB
                // AB-ABar1 (1-MB)

                // pre 3.0.8
                // ABar-MBAM         0.2
                // ABar-AB           0.8*0.8
                // ABar-AB-ABar-MBAM 0.8*0.2

                // ABar-MBAM         (2-MB)*MB
                // ABar-AB           (1-MB)

                //AB-ABar 0.8 * 0.8
                chain1 = new SpellCycle(2);
                chain1.AddSpell(ABar1, castingState);
                chain1.AddSpell(AB, castingState);
                if (AB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB.CastTime - ABar.CastTime);
                chain1.Calculate(castingState);

                //ABar-MBAM
                chain2 = new SpellCycle(2);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                if (MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime);
                chain2.Calculate(castingState);

                CastTime = (1 - MB) * chain1.CastTime + (2 - MB) * MB * chain2.CastTime;
                CostPerSecond = ((1 - MB) * chain1.CastTime * chain1.CostPerSecond + (2 - MB) * MB * chain2.CastTime * chain2.CostPerSecond) / CastTime;
                DamagePerSecond = ((1 - MB) * chain1.CastTime * chain1.DamagePerSecond + (2 - MB) * MB * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
                ThreatPerSecond = ((1 - MB) * chain1.CastTime * chain1.ThreatPerSecond + (2 - MB) * MB * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
                ManaRegenPerSecond = ((1 - MB) * chain1.CastTime * chain1.ManaRegenPerSecond + (2 - MB) * MB * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
            }
        }

        public override string Sequence
        {
            get
            {
                return chain1.Sequence;
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

    class ABABar1C : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB;
        float S0, S1;

        public ABABar1C(CastingState castingState)
        {
            Name = "ABABar1C";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            //Spell ABar1C = castingState.GetSpell(SpellId.ArcaneBarrage1Combo);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            Spell MBAM1 = castingState.GetSpell(SpellId.ArcaneMissilesMB1);
            //Spell MBAM1C = castingState.GetSpell(SpellId.ArcaneMissilesMB1Clipped);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            // S0
            // AB-ABar1 => S0                  (1-MB)*(1-MB)
            //          => S1                  1 - (1-MB)*(1-MB)

            // S1
            // AB-MBAM1-ABar => S0             (1-MB)
            //               => S1             MB

            // S0 + S1 = 1
            // S0 = S0 * (1-MB)*(1-MB) + S1 * (1-MB)
            // S1 = S0 * (1 - (1-MB)*(1-MB)) + S1 * MB

            // S1 * (1 - MB) = S0 * (1 - (1-MB)*(1-MB))
            // 1 - MB = S0 * (1 + MB - MB * MB)
            // 1 - MB = S0 * (1 + MB * (1 - MB))
            // S0 = (1 - MB) / (1 + MB * (1 - MB))
            // S1 = MB * (2 - MB) / (1 + MB * (1 - MB))

            // value = S0 * value(AB-ABar1) + S1 * value(AB-MBAM1-ABar)

            S0 = (1 - MB) / (1 + MB * (1 - MB));
            S1 = MB * (2 - MB) / (1 + MB * (1 - MB));

            //AB-ABar1
            chain1 = new SpellCycle(2);
            chain1.AddSpell(AB, castingState);
            if (AB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB.CastTime - ABar.CastTime);
            chain1.AddSpell(ABar1, castingState);
            chain1.Calculate(castingState);

            //AB-MBAM1-ABar
            chain2 = new SpellCycle(3);
            chain2.AddSpell(AB, castingState);
            chain2.AddSpell(MBAM1, castingState);
            if (AB.CastTime + MBAM1.CastTime + ABar.CastTime < 3.0)
            {
                chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM1.CastTime - AB.CastTime - ABar.CastTime);
                chain2.AddSpell(ABar, castingState);
            }
            else
            {
                chain2.AddSpell(ABar, castingState);
            }
            chain2.Calculate(castingState);

            CastTime = S0 * chain1.CastTime + S1 * chain2.CastTime;
            CostPerSecond = (S0 * chain1.CastTime * chain1.CostPerSecond + S1 * chain2.CastTime * chain2.CostPerSecond) / CastTime;
            DamagePerSecond = (S0 * chain1.CastTime * chain1.DamagePerSecond + S1 * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
            ThreatPerSecond = (S0 * chain1.CastTime * chain1.ThreatPerSecond + S1 * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (S0 * chain1.CastTime * chain1.ManaRegenPerSecond + S1 * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
        }

        public override string Sequence
        {
            get
            {
                return chain1.Sequence;
            }
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, S0 * chain1.CastTime / CastTime * duration);
            chain2.AddSpellContribution(dict, S1 * chain2.CastTime / CastTime * duration);
        }
    }

    class ABABar0MBAM : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB;
        float S0, S1;

        public ABABar0MBAM(CastingState castingState)
        {
            Name = "ABABar0MBAM";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            // AB0-ABar1                   (1-MB)*(1-MB)
            // AB0-ABar1-MBAM              (1 - (1-MB)*(1-MB))

            // value = S0 * value(AB-ABar1) + S1 * value(AB-MBAM1-ABar)

            S0 = (1 - MB) * (1 - MB);
            S1 = (1 - (1 - MB) * (1 - MB));

            //AB-ABar1
            chain1 = new SpellCycle(2);
            chain1.AddSpell(AB, castingState);
            if (AB.CastTime + ABar1.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB.CastTime - ABar1.CastTime);
            chain1.AddSpell(ABar1, castingState);
            chain1.Calculate(castingState);

            //AB-MBAM1-ABar
            chain2 = new SpellCycle(3);
            chain2.AddSpell(MBAM, castingState);
            chain2.AddSpell(AB, castingState);
            if (AB.CastTime + MBAM.CastTime + ABar1.CastTime < 3.0)
            {
                chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - AB.CastTime - ABar1.CastTime);
                chain2.AddSpell(ABar1, castingState);
            }
            else
            {
                chain2.AddSpell(ABar1, castingState);
            }
            chain2.Calculate(castingState);

            CastTime = S0 * chain1.CastTime + S1 * chain2.CastTime;
            CostPerSecond = (S0 * chain1.CastTime * chain1.CostPerSecond + S1 * chain2.CastTime * chain2.CostPerSecond) / CastTime;
            DamagePerSecond = (S0 * chain1.CastTime * chain1.DamagePerSecond + S1 * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
            ThreatPerSecond = (S0 * chain1.CastTime * chain1.ThreatPerSecond + S1 * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (S0 * chain1.CastTime * chain1.ManaRegenPerSecond + S1 * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
        }

        public override string Sequence
        {
            get
            {
                return chain1.Sequence;
            }
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, S0 * chain1.CastTime / CastTime * duration);
            chain2.AddSpellContribution(dict, S1 * chain2.CastTime / CastTime * duration);
        }
    }

    class ABABarY : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB;
        float S0, S1;

        public ABABarY(CastingState castingState)
        {
            Name = "ABABarY";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            Spell MBAM1 = castingState.GetSpell(SpellId.ArcaneMissilesMB1);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            // S0
            // AB-ABar1 => S0             (1-MB)*(1-MB)
            //          => S1             1 - (1-MB)*(1-MB)

            // S1
            // AB-MBAM1 => S0             1

            // S0 + S1 = 1
            // S0 = S0 * (1-MB)*(1-MB) + S1
            // S1 = S0 * (1 - (1-MB)*(1-MB))

            // 1 = S0 * (2 - (1-MB)*(1-MB))
            // S0 = 1 / (2 - (1-MB)*(1-MB))
            // S1 = (1 - (1-MB)*(1-MB)) / (2 - (1-MB)*(1-MB))

            // value = S0 * value(AB-ABar1) + S1 * value(AB-MBAM1-ABar)

            S0 = 1 / (2 - (1 - MB) * (1 - MB));
            S1 = 1 - S0;

            //AB-ABar1
            chain1 = new SpellCycle(2);
            chain1.AddSpell(AB, castingState);
            if (AB.CastTime + ABar1.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB.CastTime - ABar1.CastTime); // this might not actually be needed if we're transitioning from S1, but we're assuming this cycle is used under low haste conditions only
            chain1.AddSpell(ABar1, castingState);
            chain1.Calculate(castingState);

            //AB-MBAM1
            chain2 = new SpellCycle(3);
            chain2.AddSpell(AB, castingState);
            chain2.AddSpell(MBAM1, castingState);
            chain2.Calculate(castingState);

            CastTime = S0 * chain1.CastTime + S1 * chain2.CastTime;
            CostPerSecond = (S0 * chain1.CastTime * chain1.CostPerSecond + S1 * chain2.CastTime * chain2.CostPerSecond) / CastTime;
            DamagePerSecond = (S0 * chain1.CastTime * chain1.DamagePerSecond + S1 * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
            ThreatPerSecond = (S0 * chain1.CastTime * chain1.ThreatPerSecond + S1 * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (S0 * chain1.CastTime * chain1.ManaRegenPerSecond + S1 * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
        }

        public override string Sequence
        {
            get
            {
                return chain1.Sequence;
            }
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, S0 * chain1.CastTime / CastTime * duration);
            chain2.AddSpellContribution(dict, S1 * chain2.CastTime / CastTime * duration);
        }
    }

    class AB2ABarMBAM : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        SpellCycle chain3;
        SpellCycle chain4;
        float K1, K2, K3, K4;

        public AB2ABarMBAM(CastingState castingState)
        {
            Name = "AB2ABarMBAM";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;

            //S0:
            //AB0-AB1-ABar2            (1 - MB) * (1 - MB) * (1 - MB)
            //AB0-AB1-ABar2-MBAM       (1 - MB) * (1 - (1 - MB) * (1 - MB))
            //AB0-AB1-MBAM2-ABar       MB * (1 - MB)
            //AB0-AB1-MBAM2-ABar-MBAM  MB * MB


            K1 = (1 - MB) * (1 - MB) * (1 - MB);
            K2 = (1 - MB) * (1 - (1 - MB) * (1 - MB));
            K3 = MB * (1 - MB);
            K4 = MB * MB;

            //AB0-AB1-ABar2
            chain1 = new SpellCycle(3);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            if (AB0.CastTime + AB1.CastTime + ABar2.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - ABar2.CastTime);
            chain1.AddSpell(ABar2, castingState);
            chain1.Calculate(castingState);

            //AB0-AB1-ABar2-MBAM
            chain2 = new SpellCycle(3);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            if (AB0.CastTime + AB1.CastTime + ABar2.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - ABar2.CastTime);
            chain2.AddSpell(ABar2, castingState);
            chain2.AddSpell(MBAM, castingState);
            chain2.Calculate(castingState);

            //AB0-AB1-MBAM2-ABar
            chain3 = new SpellCycle(3);
            chain3.AddSpell(AB0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(MBAM2, castingState);
            if (AB0.CastTime + AB1.CastTime + MBAM2.CastTime + ABar.CastTime < 3.0) chain3.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - ABar.CastTime - MBAM2.CastTime);
            chain3.AddSpell(ABar, castingState);
            chain3.Calculate(castingState);

            //AB0-AB1-MBAM2-ABar
            chain4 = new SpellCycle(3);
            chain4.AddSpell(AB0, castingState);
            chain4.AddSpell(AB1, castingState);
            chain4.AddSpell(MBAM2, castingState);
            if (AB0.CastTime + AB1.CastTime + MBAM2.CastTime + ABar.CastTime < 3.0) chain4.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - ABar.CastTime - MBAM2.CastTime);
            chain4.AddSpell(ABar, castingState);
            chain4.AddSpell(MBAM, castingState);
            chain4.Calculate(castingState);

            commonChain = chain1;

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime + K3 * chain3.CastTime + K4 * chain4.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond + K3 * chain3.CastTime * chain3.CostPerSecond + K4 * chain4.CastTime * chain4.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond + K3 * chain3.CastTime * chain3.DamagePerSecond + K4 * chain4.CastTime * chain4.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond + K3 * chain3.CastTime * chain3.ThreatPerSecond + K4 * chain4.CastTime * chain4.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond + K3 * chain3.CastTime * chain3.ManaRegenPerSecond + K4 * chain4.CastTime * chain4.ManaRegenPerSecond) / CastTime;
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
            chain1.AddSpellContribution(dict, K1 * chain1.CastTime / CastTime * duration);
            chain2.AddSpellContribution(dict, K2 * chain2.CastTime / CastTime * duration);
            chain3.AddSpellContribution(dict, K3 * chain3.CastTime / CastTime * duration);
            chain4.AddSpellContribution(dict, K4 * chain4.CastTime / CastTime * duration);
        }
    }

    class AB3ABar : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        SpellCycle chain3;
        SpellCycle chain4;
        SpellCycle chain5;
        SpellCycle chain6;
        float K1, K2, K3, K4, K5, K6;

        public AB3ABar(CastingState castingState)
        {
            Name = "AB3ABar";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;

            //S0:
            //AB0-AB1-AB2-ABar3                  (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
            //AB0-AB1-AB2-ABar3-MBAM             (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB))
            //AB0-AB1-AB2-MBAM3-ABar             (1 - MB) * MB * (1 - MB)
            //AB0-AB1-AB2-MBAM3-ABar-MBAM        (1 - MB) * MB * MB
            //AB0-AB1-MBAM2-ABar                 MB * (1 - MB)
            //AB0-AB1-MBAM2-ABar-MBAM            MB * MB

            K1 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            K2 = (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB));
            K3 = (1 - MB) * MB * (1 - MB);
            K4 = (1 - MB) * MB * MB;
            K5 = MB * (1 - MB);
            K6 = MB * MB;

            //AB0-AB1-AB2-ABar3
            chain1 = new SpellCycle(4);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AB2, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + ABar3.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - ABar3.CastTime);
            chain1.AddSpell(ABar3, castingState);
            chain1.Calculate(castingState);

            //AB0-AB1-AB2-ABar3-MBAM
            chain2 = new SpellCycle(5);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + ABar3.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - ABar3.CastTime);
            chain2.AddSpell(ABar3, castingState);
            chain2.AddSpell(MBAM, castingState);
            chain2.Calculate(castingState);

            //AB0-AB1-AB2-MBAM3-ABar
            chain3 = new SpellCycle(5);
            chain3.AddSpell(AB0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(MBAM3, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + MBAM3.CastTime + ABar.CastTime < 3.0) chain3.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - MBAM3.CastTime - ABar.CastTime);
            chain3.AddSpell(ABar, castingState);
            chain3.Calculate(castingState);

            //AB0-AB1-AB2-MBAM3-ABar-MBAM
            chain4 = new SpellCycle(6);
            chain4.AddSpell(AB0, castingState);
            chain4.AddSpell(AB1, castingState);
            chain4.AddSpell(AB2, castingState);
            chain4.AddSpell(MBAM3, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + MBAM3.CastTime + ABar.CastTime < 3.0) chain4.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - MBAM3.CastTime - ABar.CastTime);
            chain4.AddSpell(ABar, castingState);
            chain4.AddSpell(MBAM, castingState);
            chain4.Calculate(castingState);

            //AB0-AB1-MBAM2-ABar
            chain5 = new SpellCycle(4);
            chain5.AddSpell(AB0, castingState);
            chain5.AddSpell(AB1, castingState);
            chain5.AddSpell(MBAM2, castingState);
            if (AB0.CastTime + AB1.CastTime + MBAM2.CastTime + ABar.CastTime < 3.0) chain5.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - MBAM2.CastTime - ABar.CastTime);
            chain5.AddSpell(ABar, castingState);
            chain5.Calculate(castingState);

            //AB0-AB1-MBAM2-ABar-MBAM
            chain6 = new SpellCycle(4);
            chain6.AddSpell(AB0, castingState);
            chain6.AddSpell(AB1, castingState);
            chain6.AddSpell(MBAM2, castingState);
            if (AB0.CastTime + AB1.CastTime + MBAM2.CastTime + ABar.CastTime < 3.0) chain6.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - MBAM2.CastTime - ABar.CastTime);
            chain6.AddSpell(ABar, castingState);
            chain6.AddSpell(MBAM, castingState);
            chain6.Calculate(castingState);

            commonChain = chain1;

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime + K3 * chain3.CastTime + K4 * chain4.CastTime + K5 * chain5.CastTime + K6 * chain6.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond + K3 * chain3.CastTime * chain3.CostPerSecond + K4 * chain4.CastTime * chain4.CostPerSecond + K5 * chain5.CastTime * chain5.CostPerSecond + K6 * chain6.CastTime * chain6.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond + K3 * chain3.CastTime * chain3.DamagePerSecond + K4 * chain4.CastTime * chain4.DamagePerSecond + K5 * chain5.CastTime * chain5.DamagePerSecond + K6 * chain6.CastTime * chain6.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond + K3 * chain3.CastTime * chain3.ThreatPerSecond + K4 * chain4.CastTime * chain4.ThreatPerSecond + K5 * chain5.CastTime * chain5.ThreatPerSecond + K6 * chain6.CastTime * chain6.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond + K3 * chain3.CastTime * chain3.ManaRegenPerSecond + K4 * chain4.CastTime * chain4.ManaRegenPerSecond + K5 * chain5.CastTime * chain5.ManaRegenPerSecond + K6 * chain6.CastTime * chain6.ManaRegenPerSecond) / CastTime;
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
            chain1.AddSpellContribution(dict, K1 * chain1.CastTime / CastTime * duration);
            chain2.AddSpellContribution(dict, K2 * chain2.CastTime / CastTime * duration);
            chain3.AddSpellContribution(dict, K3 * chain3.CastTime / CastTime * duration);
            chain4.AddSpellContribution(dict, K4 * chain4.CastTime / CastTime * duration);
            chain5.AddSpellContribution(dict, K5 * chain5.CastTime / CastTime * duration);
            chain6.AddSpellContribution(dict, K6 * chain6.CastTime / CastTime * duration);
        }
    }

    class AB3ABarX : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        SpellCycle chain3;
        SpellCycle chain4;
        float K1, K2, K3, K4;

        public AB3ABarX(CastingState castingState)
        {
            Name = "AB3ABarX";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;

            //S0:
            //AB0-AB1-AB2-ABar3                  (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
            //AB0-AB1-AB2-ABar3-MBAM             (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB))
            //AB0-AB1-AB2-MBAM3-ABar             (1 - (1 - MB) * (1 - MB)) * (1 - MB)
            //AB0-AB1-AB2-MBAM3-ABar-MBAM        (1 - (1 - MB) * (1 - MB)) * MB

            K1 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            K2 = (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB));
            K3 = (1 - (1 - MB) * (1 - MB)) * (1 - MB);
            K4 = (1 - (1 - MB) * (1 - MB)) * MB;

            //AB0-AB1-AB2-ABar3
            chain1 = new SpellCycle(4);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AB2, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + ABar3.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - ABar3.CastTime);
            chain1.AddSpell(ABar3, castingState);
            chain1.Calculate(castingState);

            //AB0-AB1-AB2-ABar3-MBAM
            chain2 = new SpellCycle(5);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + ABar3.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - ABar3.CastTime);
            chain2.AddSpell(ABar3, castingState);
            chain2.AddSpell(MBAM, castingState);
            chain2.Calculate(castingState);

            //AB0-AB1-AB2-MBAM3-ABar
            chain3 = new SpellCycle(5);
            chain3.AddSpell(AB0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(MBAM3, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + MBAM3.CastTime + ABar.CastTime < 3.0) chain3.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - MBAM3.CastTime - ABar.CastTime);
            chain3.AddSpell(ABar, castingState);
            chain3.Calculate(castingState);

            //AB0-AB1-AB2-MBAM3-ABar-MBAM
            chain4 = new SpellCycle(6);
            chain4.AddSpell(AB0, castingState);
            chain4.AddSpell(AB1, castingState);
            chain4.AddSpell(AB2, castingState);
            chain4.AddSpell(MBAM3, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + MBAM3.CastTime + ABar.CastTime < 3.0) chain4.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - MBAM3.CastTime - ABar.CastTime);
            chain4.AddSpell(ABar, castingState);
            chain4.AddSpell(MBAM, castingState);
            chain4.Calculate(castingState);

            commonChain = chain1;

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime + K3 * chain3.CastTime + K4 * chain4.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond + K3 * chain3.CastTime * chain3.CostPerSecond + K4 * chain4.CastTime * chain4.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond + K3 * chain3.CastTime * chain3.DamagePerSecond + K4 * chain4.CastTime * chain4.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond + K3 * chain3.CastTime * chain3.ThreatPerSecond + K4 * chain4.CastTime * chain4.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond + K3 * chain3.CastTime * chain3.ManaRegenPerSecond + K4 * chain4.CastTime * chain4.ManaRegenPerSecond) / CastTime;
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
            chain1.AddSpellContribution(dict, K1 * chain1.CastTime / CastTime * duration);
            chain2.AddSpellContribution(dict, K2 * chain2.CastTime / CastTime * duration);
            chain3.AddSpellContribution(dict, K3 * chain3.CastTime / CastTime * duration);
            chain4.AddSpellContribution(dict, K4 * chain4.CastTime / CastTime * duration);
        }
    }

    class AB3ABarY : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        SpellCycle chain3;
        SpellCycle chain5;
        float K1, K2, K3, K5;

        public AB3ABarY(CastingState castingState)
        {
            Name = "AB3ABarY";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;

            //S0:
            //AB0-AB1-AB2-ABar3                  (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
            //AB0-AB1-AB2-ABar3-MBAM             (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB))
            //AB0-AB1-AB2-MBAM3                  (1 - MB) * MB
            //AB0-AB1-MBAM2                      MB

            K1 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            K2 = (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB));
            K3 = (1 - MB) * MB;
            K5 = MB;

            //AB0-AB1-AB2-ABar3
            chain1 = new SpellCycle(4);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AB2, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + ABar3.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - ABar3.CastTime);
            chain1.AddSpell(ABar3, castingState);
            chain1.Calculate(castingState);

            //AB0-AB1-AB2-ABar3-MBAM
            chain2 = new SpellCycle(5);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + ABar3.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - ABar3.CastTime);
            chain2.AddSpell(ABar3, castingState);
            chain2.AddSpell(MBAM, castingState);
            chain2.Calculate(castingState);

            //AB0-AB1-AB2-MBAM3-ABar
            chain3 = new SpellCycle(5);
            chain3.AddSpell(AB0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(MBAM3, castingState);
            chain3.Calculate(castingState);

            //AB0-AB1-MBAM2-ABar
            chain5 = new SpellCycle(4);
            chain5.AddSpell(AB0, castingState);
            chain5.AddSpell(AB1, castingState);
            chain5.AddSpell(MBAM2, castingState);
            chain5.Calculate(castingState);

            commonChain = chain1;

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime + K3 * chain3.CastTime + K5 * chain5.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond + K3 * chain3.CastTime * chain3.CostPerSecond + K5 * chain5.CastTime * chain5.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond + K3 * chain3.CastTime * chain3.DamagePerSecond + K5 * chain5.CastTime * chain5.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond + K3 * chain3.CastTime * chain3.ThreatPerSecond + K5 * chain5.CastTime * chain5.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond + K3 * chain3.CastTime * chain3.ManaRegenPerSecond + K5 * chain5.CastTime * chain5.ManaRegenPerSecond) / CastTime;
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
            chain1.AddSpellContribution(dict, K1 * chain1.CastTime / CastTime * duration);
            chain2.AddSpellContribution(dict, K2 * chain2.CastTime / CastTime * duration);
            chain3.AddSpellContribution(dict, K3 * chain3.CastTime / CastTime * duration);
            chain5.AddSpellContribution(dict, K5 * chain5.CastTime / CastTime * duration);
        }
    }

    class AB2ABar : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float K;

        public AB2ABar(CastingState castingState)
        {
            Name = "AB2ABar";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;

            {
                if (MB == 0.0f)
                {
                    Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast33);
                    // if we don't have barrage then this degenerates to AB-AB-ABar
                    chain1 = new SpellCycle(3);
                    chain1.AddSpell(AB0, castingState);
                    chain1.AddSpell(AB1, castingState);
                    if (AB3.CastTime + AB3.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB3.CastTime - AB3.CastTime - ABar.CastTime);
                    chain1.AddSpell(ABar2, castingState);
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
                    //S0:
                    //AB0-AB1-ABar2 => S0 1 - MB3
                    //              => S1  MB3

                    //S1:
                    //MBAM-AB0-ABar1 => S0 1 - MB2
                    //               => S1  MB2


                    // S0 + S1 = 1
                    // S0 = S0 * (1 - MB3) + S1 * (1 - MB2)
                    // S1 = S0 * MB3 + S1 * MB2
                    // S1 * (1 - MB2) = S0 * MB3
                    // S1 * (1 - MB2) = MB3 - S1 * MB3
                    // S1 * (1 - MB2 + MB3) = MB3
                    // K2 := S1 = MB3 / (1 - MB2 + MB3)
                    // K1 := S0 = 1 - S1

                    K = (1 - MB) * (1 - MB) * (1 - MB) / (1 - (1 - MB) * (1 - MB) + (1 - MB) * (1 - MB) * (1 - MB));

                    //AB0-AB1-ABar2
                    chain1 = new SpellCycle(3);
                    chain1.AddSpell(AB0, castingState);
                    chain1.AddSpell(AB1, castingState);
                    if (AB0.CastTime + AB1.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - ABar.CastTime);
                    chain1.AddSpell(ABar2, castingState);
                    chain1.Calculate(castingState);

                    //MBAM-AB0-ABar1
                    chain2 = new SpellCycle(3);
                    chain2.AddSpell(MBAM, castingState);
                    chain2.AddSpell(AB0, castingState);
                    if (MBAM.CastTime + AB0.CastTime + ABar1.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - AB0.CastTime - ABar1.CastTime);
                    chain2.AddSpell(ABar1, castingState);
                    chain2.Calculate(castingState);

                    commonChain = chain1;

                    CastTime = (1 - K) * chain1.CastTime + K * chain2.CastTime;
                    CostPerSecond = ((1 - K) * chain1.CastTime * chain1.CostPerSecond + K * chain2.CastTime * chain2.CostPerSecond) / CastTime;
                    DamagePerSecond = ((1 - K) * chain1.CastTime * chain1.DamagePerSecond + K * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
                    ThreatPerSecond = ((1 - K) * chain1.CastTime * chain1.ThreatPerSecond + K * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
                    ManaRegenPerSecond = ((1 - K) * chain1.CastTime * chain1.ManaRegenPerSecond + K * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
                }
            }
            /*else
            {
                if (AB1.CastTime + ABar.CastTime < 3.0)
                {
                    if (MB == 0.0f)
                    {
                        Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast33);
                        // if we don't have barrage then this degenerates to AB-AB-ABar
                        chain1 = new SpellCycle(3);
                        chain1.AddSpell(AB3, castingState);
                        chain1.AddSpell(AB3, castingState);
                        if (AB3.CastTime + AB3.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB3.CastTime - AB3.CastTime - ABar.CastTime);
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
                        Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22);
                        Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast33);
                        //AB0:
                        //AB0-AB1-ABar => AB2 1 - MB3
                        //             => MB  MB3

                        //AB1:
                        //AB1-AB2-ABar => AB3 1 - MB3
                        //             => MB  MB3

                        //AB2:
                        //AB2-AB3-ABar => AB3 1 - MB3
                        //             => MB  MB3

                        //AB3:
                        //AB3-AB3-ABar => AB3 1 - MB3
                        //             => MB  MB3

                        //MB:
                        //MBAM-AB0-ABar => AB1 1 - MB2
                        //              => MB  MB2


                        //AB0 + AB1 + AB2 + AB3 + MB = 1
                        //AB0 = 0
                        //AB1 = MB * (1 - MB2)
                        //AB2 = AB0 * (1 - MB3) = 0
                        //AB3 = (AB1 + AB2 + AB3) * (1 - MB3)
                        //MB = (AB0 + AB1 + AB2 + AB3) * MB3 + MB * MB2

                        //AB1 = MB * (1 - MB2)
                        //AB3 = (AB1 + AB3) * (1 - MB3)
                        //MB = (AB1 + AB3) * MB3 + MB * MB2


                        //(AB1 + AB3) = MB * (1 - MB2) / MB3

                        //AB1 + AB3 + MB = 1

                        // K := MB = 1 / (1 + (1 - MB2) / MB3)
                        // K1 := AB1 = MB * (1 - MB2)
                        // K2 := AB3 = AB1 * (1 - MB3) / MB3

                        K = 1.0f / (1.0f + (1 - MB) * (1 - MB) / (1.0f - (1 - MB) * (1 - MB) * (1 - MB)));
                        K1 = K * (1 - MB) * (1 - MB);
                        K2 = K1 * (1 - MB) * (1 - MB) * (1 - MB) / (1.0f - (1 - MB) * (1 - MB) * (1 - MB));

                        //AB1-AB2-ABar
                        chain1 = new SpellCycle(3);
                        chain1.AddSpell(ABar, castingState);
                        chain1.AddSpell(AB1, castingState);
                        chain1.AddSpell(AB2, castingState);
                        if (AB1.CastTime + AB2.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB1.CastTime - AB2.CastTime - ABar.CastTime);
                        chain1.Calculate(castingState);

                        //AB3-AB3-ABar
                        chain2 = new SpellCycle(3);
                        chain2.AddSpell(ABar, castingState);
                        chain2.AddSpell(AB3, castingState);
                        chain2.AddSpell(AB3, castingState);
                        if (AB0.CastTime + AB1.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - ABar.CastTime);
                        chain2.Calculate(castingState);

                        //MBAM-AB0-ABar
                        chain3 = new SpellCycle(3);
                        chain3.AddSpell(ABar, castingState);
                        chain3.AddSpell(MBAM, castingState);
                        chain3.AddSpell(AB0, castingState);
                        if (AB0.CastTime + MBAM.CastTime + ABar.CastTime < 3.0) chain3.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - ABar.CastTime);
                        chain3.Calculate(castingState);

                        commonChain = chain1;

                        CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime + K * chain3.CastTime;
                        CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond + K * chain3.CastTime * chain3.CostPerSecond) / CastTime;
                        DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond + K * chain3.CastTime * chain3.DamagePerSecond) / CastTime;
                        ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond + K * chain3.CastTime * chain3.ThreatPerSecond) / CastTime;
                        ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond + K * chain3.CastTime * chain3.ManaRegenPerSecond) / CastTime;
                    }
                }
                else
                {
                    if (MB == 0.0f)
                    {
                        // if we don't have barrage then this degenerates to AB-AB-ABar
                        chain1 = new SpellCycle(3);
                        chain1.AddSpell(AB0, castingState);
                        chain1.AddSpell(AB1, castingState);
                        if (AB0.CastTime + AB1.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - ABar.CastTime);
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
                        // A: MBAM-AB-ABar  (1-MB)*(1-MB) => B, 1 - (1-MB)*(1-MB) => A
                        // B: AB-AB-ABar    (1-MB)*(1-MB)*(1-MB) => B, 1 - (1-MB)*(1-MB)*(1-MB) => A

                        // A + B = 1
                        // A = (1 - (1-MB)*(1-MB)) * A + (1 - (1-MB)*(1-MB)*(1-MB)) * B
                        // B = (1-MB)*(1-MB) * A + (1-MB)*(1-MB)*(1-MB) * B

                        // B * (1 + (1-MB)*(1-MB) - (1-MB)*(1-MB)*(1-MB)) = (1-MB)*(1-MB)
                        // B = (1-MB)*(1-MB) / [1 + (1-MB)*(1-MB)*MB]

                        //AB-ABar 0.8 * 0.8
                        chain1 = new SpellCycle(3);
                        chain1.AddSpell(ABar, castingState);
                        chain1.AddSpell(AB0, castingState);
                        chain1.AddSpell(AB1, castingState);
                        if (AB0.CastTime + AB1.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - ABar.CastTime);
                        chain1.Calculate(castingState);

                        //ABar-MBAM
                        chain2 = new SpellCycle(3);
                        chain2.AddSpell(ABar, castingState);
                        chain2.AddSpell(MBAM, castingState);
                        chain2.AddSpell(AB0, castingState);
                        if (AB0.CastTime + MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - ABar.CastTime);
                        chain2.Calculate(castingState);

                        commonChain = chain1;

                        K = 1 - (1 - MB) * (1 - MB) / (1 + (1 - MB) * (1 - MB) * MB);

                        CastTime = (1 - K) * chain1.CastTime + K * chain2.CastTime;
                        CostPerSecond = ((1 - K) * chain1.CastTime * chain1.CostPerSecond + K * chain2.CastTime * chain2.CostPerSecond) / CastTime;
                        DamagePerSecond = ((1 - K) * chain1.CastTime * chain1.DamagePerSecond + K * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
                        ThreatPerSecond = ((1 - K) * chain1.CastTime * chain1.ThreatPerSecond + K * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
                        ManaRegenPerSecond = ((1 - K) * chain1.CastTime * chain1.ManaRegenPerSecond + K * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
                    }
                }
            }*/
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
            else /*if (chain3 == null)*/
            {
                chain1.AddSpellContribution(dict, (1 - K) * chain1.CastTime / CastTime * duration);
                chain2.AddSpellContribution(dict, K * chain2.CastTime / CastTime * duration);
            }
            /*else
            {
                chain1.AddSpellContribution(dict, K1 * chain1.CastTime / CastTime * duration);
                chain2.AddSpellContribution(dict, K2 * chain2.CastTime / CastTime * duration);
                chain3.AddSpellContribution(dict, K * chain3.CastTime / CastTime * duration);
            }*/
        }
    }

    class FB2ABar : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float K;

        public FB2ABar(CastingState castingState)
        {
            Name = "FB2ABar";
            AffectedByFlameCap = true;

            Spell FB = castingState.GetSpell(SpellId.Fireball);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0f)
            {
                // if we don't have barrage then this degenerates to AB-AB-ABar
                chain1 = new SpellCycle(3);
                chain1.AddSpell(FB, castingState);
                chain1.AddSpell(FB, castingState);
                if (FB.CastTime + FB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FB.CastTime - FB.CastTime - ABar.CastTime);
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
                // A: MBAM-AB-ABar  (1-MB)*(1-MB) => B, 1 - (1-MB)*(1-MB) => A
                // B: AB-AB-ABar    (1-MB)*(1-MB)*(1-MB) => B, 1 - (1-MB)*(1-MB)*(1-MB) => A

                // A + B = 1
                // A = (1 - (1-MB)*(1-MB)) * A + (1 - (1-MB)*(1-MB)*(1-MB)) * B
                // B = (1-MB)*(1-MB) * A + (1-MB)*(1-MB)*(1-MB) * B

                // B * (1 + (1-MB)*(1-MB) - (1-MB)*(1-MB)*(1-MB)) = (1-MB)*(1-MB)
                // B = (1-MB)*(1-MB) / [1 + (1-MB)*(1-MB)*MB]

                //AB-ABar 0.8 * 0.8
                chain1 = new SpellCycle(3);
                chain1.AddSpell(ABar, castingState);
                chain1.AddSpell(FB, castingState);
                chain1.AddSpell(FB, castingState);
                if (FB.CastTime + FB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FB.CastTime - FB.CastTime - ABar.CastTime);
                chain1.Calculate(castingState);

                //ABar-MBAM
                chain2 = new SpellCycle(3);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                chain2.AddSpell(FB, castingState);
                if (FB.CastTime + MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - FB.CastTime - MBAM.CastTime - ABar.CastTime);
                chain2.Calculate(castingState);

                commonChain = chain1;

                K = 1 - (1 - MB) * (1 - MB) / (1 + (1 - MB) * (1 - MB) * MB);

                CastTime = (1 - K) * chain1.CastTime + K * chain2.CastTime;
                CostPerSecond = ((1 - K) * chain1.CastTime * chain1.CostPerSecond + K * chain2.CastTime * chain2.CostPerSecond) / CastTime;
                DamagePerSecond = ((1 - K) * chain1.CastTime * chain1.DamagePerSecond + K * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
                ThreatPerSecond = ((1 - K) * chain1.CastTime * chain1.ThreatPerSecond + K * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
                ManaRegenPerSecond = ((1 - K) * chain1.CastTime * chain1.ManaRegenPerSecond + K * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
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
                chain1.AddSpellContribution(dict, (1 - K) * chain1.CastTime / CastTime * duration);
                chain2.AddSpellContribution(dict, K * chain2.CastTime / CastTime * duration);
            }
        }
    }

    class FrB2ABar : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float K;

        public FrB2ABar(CastingState castingState)
        {
            Name = "FrB2ABar";

            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0f)
            {
                // if we don't have barrage then this degenerates to AB-AB-ABar
                chain1 = new SpellCycle(3);
                chain1.AddSpell(FrB, castingState);
                chain1.AddSpell(FrB, castingState);
                if (FrB.CastTime + FrB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FrB.CastTime - FrB.CastTime - ABar.CastTime);
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
                // A: MBAM-AB-ABar  (1-MB)*(1-MB) => B, 1 - (1-MB)*(1-MB) => A
                // B: AB-AB-ABar    (1-MB)*(1-MB)*(1-MB) => B, 1 - (1-MB)*(1-MB)*(1-MB) => A

                // A + B = 1
                // A = (1 - (1-MB)*(1-MB)) * A + (1 - (1-MB)*(1-MB)*(1-MB)) * B
                // B = (1-MB)*(1-MB) * A + (1-MB)*(1-MB)*(1-MB) * B

                // B * (1 + (1-MB)*(1-MB) - (1-MB)*(1-MB)*(1-MB)) = (1-MB)*(1-MB)
                // B = (1-MB)*(1-MB) / [1 + (1-MB)*(1-MB)*MB]

                //AB-ABar 0.8 * 0.8
                chain1 = new SpellCycle(3);
                chain1.AddSpell(ABar, castingState);
                chain1.AddSpell(FrB, castingState);
                chain1.AddSpell(FrB, castingState);
                if (FrB.CastTime + FrB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FrB.CastTime - FrB.CastTime - ABar.CastTime);
                chain1.Calculate(castingState);

                //ABar-MBAM
                chain2 = new SpellCycle(3);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                chain2.AddSpell(FrB, castingState);
                if (FrB.CastTime + MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - FrB.CastTime - MBAM.CastTime - ABar.CastTime);
                chain2.Calculate(castingState);

                commonChain = chain1;

                K = 1 - (1 - MB) * (1 - MB) / (1 + (1 - MB) * (1 - MB) * MB);

                CastTime = (1 - K) * chain1.CastTime + K * chain2.CastTime;
                CostPerSecond = ((1 - K) * chain1.CastTime * chain1.CostPerSecond + K * chain2.CastTime * chain2.CostPerSecond) / CastTime;
                DamagePerSecond = ((1 - K) * chain1.CastTime * chain1.DamagePerSecond + K * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
                ThreatPerSecond = ((1 - K) * chain1.CastTime * chain1.ThreatPerSecond + K * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
                ManaRegenPerSecond = ((1 - K) * chain1.CastTime * chain1.ManaRegenPerSecond + K * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
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
                chain1.AddSpellContribution(dict, (1 - K) * chain1.CastTime / CastTime * duration);
                chain2.AddSpellContribution(dict, K * chain2.CastTime / CastTime * duration);
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
            AffectedByFlameCap = true;

            Spell FB = castingState.GetSpell(SpellId.Fireball);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to FB-ABar
                chain1 = new SpellCycle(2);
                chain1.AddSpell(FB, castingState);
                if (FB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FB.CastTime - ABar.CastTime);
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
                if (FB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FB.CastTime - ABar.CastTime);
                chain1.Calculate(castingState);

                chain2 = new SpellCycle(2);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                if (MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime);
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

            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to FrB-ABar
                chain1 = new SpellCycle(2);
                chain1.AddSpell(FrB, castingState);
                if (FrB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FrB.CastTime - ABar.CastTime);
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
                if (FrB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FrB.CastTime - ABar.CastTime);
                chain1.Calculate(castingState);

                chain2 = new SpellCycle(2);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                if (MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime);
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
            AffectedByFlameCap = true;

            Spell FFB = castingState.GetSpell(SpellId.FrostfireBolt);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to FB-ABar
                chain1 = new SpellCycle(2);
                chain1.AddSpell(FFB, castingState);
                if (FFB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FFB.CastTime - ABar.CastTime);
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
                if (FFB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FFB.CastTime - ABar.CastTime);
                chain1.Calculate(castingState);

                chain2 = new SpellCycle(4);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                if (MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime);
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

    class ABSpamMBAM : Spell
    {
        BaseSpell AB3;
        SpellCycle chain1;
        SpellCycle chain3;
        SpellCycle chain4;
        SpellCycle chain5;
        float MB, MB3, MB4, MB5, hit, miss;

        public ABSpamMBAM(CastingState castingState)
        {
            Name = "ABSpamMBAM";

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
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

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
            /*else if (AB3.CastTime + MBAM.CastTime < 3.0 && !castingState.CalculationOptions.Mode308)
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
            }*/
            else /*if (AB3.HitRate >= 1.0f || castingState.CalculationOptions.Mode308)*/
            {
                //AB3 0.85

                //AB3-MBAM-RAMP 0.15
                chain1 = new SpellCycle(6);
                chain1.AddSpell(AB3, castingState);
                chain1.AddSpell(AB3, castingState); // account for latency
                chain1.AddSpell(MBAM3, castingState);
                chain1.AddSpell(AB0, castingState);
                chain1.AddSpell(AB1, castingState);
                chain1.AddSpell(AB2, castingState);
                chain1.Calculate(castingState);

                chain3 = new SpellCycle(5);
                chain3.AddSpell(AB0, castingState);
                chain3.AddSpell(AB1, castingState);
                chain3.AddSpell(AB2, castingState);
                chain3.AddSpell(AB3, castingState); // account for latency
                chain3.AddSpell(MBAM3, castingState);
                chain3.Calculate(castingState);

                chain4 = new SpellCycle(4);
                chain4.AddSpell(AB0, castingState);
                chain4.AddSpell(AB1, castingState);
                chain4.AddSpell(AB2, castingState); // account for latency
                chain4.AddSpell(MBAM3, castingState);
                chain4.Calculate(castingState);

                chain5 = new SpellCycle(3);
                chain5.AddSpell(AB0, castingState);
                chain5.AddSpell(AB1, castingState); // account for latency
                chain5.AddSpell(MBAM2, castingState);
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
            /*else
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
            }*/
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            if (chain1 == null)
            {
                AB3.AddSpellContribution(dict, duration);
            }
            /*else if (chain3 == null)
            {
                AB3.AddSpellContribution(dict, duration * (1 - MB) * AB3.CastTime / CastTime);
                chain1.AddSpellContribution(dict, duration * MB * chain1.CastTime / CastTime);
            }*/
            else /*if (AB3.HitRate >= 1.0f || mode308)*/
            {
                AB3.AddSpellContribution(dict, duration * (1 - MB) * AB3.CastTime / CastTime);
                chain1.AddSpellContribution(dict, duration * MB * chain1.CastTime / CastTime);
                chain3.AddSpellContribution(dict, duration * MB * MB3 * chain3.CastTime / CastTime);
                chain4.AddSpellContribution(dict, duration * MB * MB4 * chain4.CastTime / CastTime);
                chain5.AddSpellContribution(dict, duration * MB * MB5 * chain5.CastTime / CastTime);
            }
            /*else
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
            }*/
        }
    }

    class ABSpam3C : Spell
    {
        BaseSpell AB3;
        SpellCycle chain1;
        SpellCycle chain2;
        SpellCycle chain3;
        SpellCycle chain4;
        float MB, K1, K2, K3, K4, K5, S0, S1;

        public ABSpam3C(CastingState castingState)
        {
            Name = "ABSpam3C";

            // always ramp up to 3 AB before using MBAM-ABar

            // S0: (we always enter S0 with ABar, take into account)
            // AB0-AB1-AB2-MBAM-ABar => S0       1 - (1-MB)*(1-MB)*(1-MB)      one of the first two AB or ABar procs
            // AB0-AB1-AB2-AB3-MBAM-ABar => S0   (1-MB)*(1-MB)*(1-MB)*MB       third AB procs
            // AB0-AB1-AB2 => S1                 (1-MB)*(1-MB)*(1-MB)*(1-MB)   no procs
            // S1:
            // AB3-AB3-MBAM-ABar => S0           MB                     proc
            // AB3 => S1                         (1-MB)                 no proc

            // S0 = (1 - (1-MB)*(1-MB)*(1-MB)*(1-MB)) * S0 + MB * S1
            // S1 = (1-MB)*(1-MB)*(1-MB)*(1-MB) * S0 + (1-MB) * S1
            // S0 + S1 = 1

            // S0 = MB / (MB + (1-MB)*(1-MB)*(1-MB)*(1-MB))
            // S1 = (1-MB)*(1-MB)*(1-MB)*(1-MB) / (MB + (1-MB)*(1-MB)*(1-MB)*(1-MB))

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22);
            AB3 = (BaseSpell)castingState.GetSpell(SpellId.ArcaneBlast33);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            //Spell MBAM3C = castingState.GetSpell(SpellId.ArcaneMissilesMB3Clipped);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
            //Spell ABar3C = castingState.GetSpell(SpellId.ArcaneBarrage3Combo);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            S0 = MB / (MB + (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB));
            S1 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB) / (MB + (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB));
            K1 = S0 * (1 - (1 - MB) * (1 - MB) * (1 - MB));
            K2 = S0 * (1 - MB) * (1 - MB) * (1 - MB) * MB;
            K3 = S0 * (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            K4 = S1 * MB;
            K5 = S1 * (1 - MB);

            chain1 = new SpellCycle(5);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AB2, castingState);
            chain1.AddSpell(MBAM3, castingState);
            chain1.AddSpell(ABar, castingState);
            chain1.Calculate(castingState);

            chain2 = new SpellCycle(6);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            chain2.AddSpell(AB3, castingState);
            chain2.AddSpell(MBAM3, castingState);
            chain2.AddSpell(ABar, castingState);
            chain2.Calculate(castingState);

            chain3 = new SpellCycle(3);
            chain3.AddSpell(AB0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.Calculate(castingState);

            chain4 = new SpellCycle(4);
            chain4.AddSpell(AB3, castingState);
            chain4.AddSpell(AB3, castingState);
            chain4.AddSpell(MBAM3, castingState);
            chain4.AddSpell(ABar, castingState);
            chain4.Calculate(castingState);

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime + K3 * chain3.CastTime + K4 * chain4.CastTime + K5 * AB3.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond + K3 * chain3.CastTime * chain3.CostPerSecond + K4 * chain4.CastTime * chain4.CostPerSecond + K5 * AB3.CastTime * AB3.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond + K3 * chain3.CastTime * chain3.DamagePerSecond + K4 * chain4.CastTime * chain4.DamagePerSecond + K5 * AB3.CastTime * AB3.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond + K3 * chain3.CastTime * chain3.ThreatPerSecond + K4 * chain4.CastTime * chain4.ThreatPerSecond + K5 * AB3.CastTime * AB3.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond + K3 * chain3.CastTime * chain3.ManaRegenPerSecond + K4 * chain4.CastTime * chain4.ManaRegenPerSecond + K5 * AB3.CastTime * AB3.ManaRegenPerSecond) / CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * K1 * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * K2 * chain2.CastTime / CastTime);
            chain3.AddSpellContribution(dict, duration * K3 * chain3.CastTime / CastTime);
            chain4.AddSpellContribution(dict, duration * K4 * chain4.CastTime / CastTime);
            AB3.AddSpellContribution(dict, duration * K5 * AB3.CastTime / CastTime);
        }
    }

    class ABSpam03C : Spell
    {
        BaseSpell AB3;
        SpellCycle chain1;
        SpellCycle chain2;
        SpellCycle chain3;
        SpellCycle chain4;
        SpellCycle chain6;
        float MB, K1, K2, K3, K4, K5, K6, S0, S1;

        public ABSpam03C(CastingState castingState)
        {
            Name = "ABSpam03C";

            // S0: (we always enter S0 with ABar, take into account)

            // MBAM-ABar => S0                   MB                            ABar procs
            // AB0-AB1-AB2-MBAM-ABar => S0       (1-MB)*(1 - (1-MB)*(1-MB))    one of the first two AB procs
            // AB0-AB1-AB2-AB3-MBAM-ABar => S0   (1-MB)*(1-MB)*(1-MB)*MB       third AB procs
            // AB0-AB1-AB2 => S1                 (1-MB)*(1-MB)*(1-MB)*(1-MB)   no procs
            // S1:
            // AB3-AB3-MBAM-ABar => S0           MB                     proc
            // AB3 => S1                         (1-MB)                 no proc

            // S0 = (1 - (1-MB)*(1-MB)*(1-MB)*(1-MB)) * S0 + MB * S1
            // S1 = (1-MB)*(1-MB)*(1-MB)*(1-MB) * S0 + (1-MB) * S1
            // S0 + S1 = 1

            // S0 = MB / (MB + (1-MB)*(1-MB)*(1-MB)*(1-MB))
            // S1 = (1-MB)*(1-MB)*(1-MB)*(1-MB) / (MB + (1-MB)*(1-MB)*(1-MB)*(1-MB))

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22);
            AB3 = (BaseSpell)castingState.GetSpell(SpellId.ArcaneBlast33);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            S0 = MB / (MB + (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB));
            S1 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB) / (MB + (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB));
            K6 = S0 * MB;
            K1 = S0 * (1 - MB) * (1 - (1 - MB) * (1 - MB));
            K2 = S0 * (1 - MB) * (1 - MB) * (1 - MB) * MB;
            K3 = S0 * (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            K4 = S1 * MB;
            K5 = S1 * (1 - MB);

            chain6 = new SpellCycle(2);
            chain6.AddSpell(MBAM, castingState);
            if (MBAM.CastTime + ABar.CastTime < 3.0) chain6.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime);
            chain6.AddSpell(ABar, castingState);
            chain6.Calculate(castingState);

            chain1 = new SpellCycle(5);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AB2, castingState);
            chain1.AddSpell(MBAM3, castingState);
            chain1.AddSpell(ABar, castingState);
            chain1.Calculate(castingState);

            chain2 = new SpellCycle(6);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            chain2.AddSpell(AB3, castingState);
            chain2.AddSpell(MBAM3, castingState);
            chain2.AddSpell(ABar, castingState);
            chain2.Calculate(castingState);

            chain3 = new SpellCycle(3);
            chain3.AddSpell(AB0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.Calculate(castingState);

            chain4 = new SpellCycle(4);
            chain4.AddSpell(AB3, castingState);
            chain4.AddSpell(AB3, castingState);
            chain4.AddSpell(MBAM3, castingState);
            chain4.AddSpell(ABar, castingState);
            chain4.Calculate(castingState);

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime + K3 * chain3.CastTime + K4 * chain4.CastTime + K5 * AB3.CastTime + K6 * chain6.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond + K3 * chain3.CastTime * chain3.CostPerSecond + K4 * chain4.CastTime * chain4.CostPerSecond + K5 * AB3.CastTime * AB3.CostPerSecond + K6 * chain6.CastTime * chain6.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond + K3 * chain3.CastTime * chain3.DamagePerSecond + K4 * chain4.CastTime * chain4.DamagePerSecond + K5 * AB3.CastTime * AB3.DamagePerSecond + K6 * chain6.CastTime * chain6.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond + K3 * chain3.CastTime * chain3.ThreatPerSecond + K4 * chain4.CastTime * chain4.ThreatPerSecond + K5 * AB3.CastTime * AB3.ThreatPerSecond + K6 * chain6.CastTime * chain6.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond + K3 * chain3.CastTime * chain3.ManaRegenPerSecond + K4 * chain4.CastTime * chain4.ManaRegenPerSecond + K5 * AB3.CastTime * AB3.ManaRegenPerSecond + K6 * chain6.CastTime * chain6.ManaRegenPerSecond) / CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * K1 * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * K2 * chain2.CastTime / CastTime);
            chain3.AddSpellContribution(dict, duration * K3 * chain3.CastTime / CastTime);
            chain4.AddSpellContribution(dict, duration * K4 * chain4.CastTime / CastTime);
            AB3.AddSpellContribution(dict, duration * K5 * AB3.CastTime / CastTime);
            chain6.AddSpellContribution(dict, duration * K6 * chain6.CastTime / CastTime);
        }
    }

    class ABSpam3MBAM : Spell
    {
        BaseSpell AB3;
        SpellCycle chain1;
        SpellCycle chain2;
        SpellCycle chain3;
        SpellCycle chain4;
        float MB, K1, K2, K3, K4, K5, S0, S1;

        public ABSpam3MBAM(CastingState castingState)
        {
            Name = "ABSpam3MBAM";

            // always ramp up to 3 AB before using MBAM

            // S0:
            // AB0-AB1-AB2-MBAM => S0       1 - (1-MB)*(1-MB)      one of the first two AB procs
            // AB0-AB1-AB2-AB3-MBAM => S0   (1-MB)*(1-MB)*MB       third AB procs
            // AB0-AB1-AB2 => S1            (1-MB)*(1-MB)*(1-MB)   no procs
            // S1:
            // AB3-AB3-MBAM => S0           MB                     proc
            // AB3 => S1                    (1-MB)                 no proc

            // S0 = (1 - (1-MB)*(1-MB)*(1-MB)) * S0 + MB * S1
            // S1 = (1-MB)*(1-MB)*(1-MB) * S0 + (1-MB) * S1
            // S0 + S1 = 1

            // S0 = MB / (MB + (1-MB)*(1-MB)*(1-MB))
            // S1 = (1-MB)*(1-MB)*(1-MB) / (MB + (1-MB)*(1-MB)*(1-MB))

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22);
            AB3 = (BaseSpell)castingState.GetSpell(SpellId.ArcaneBlast33);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            S0 = MB / (MB + (1 - MB) * (1 - MB) * (1 - MB));
            S1 = (1 - MB) * (1 - MB) * (1 - MB) / (MB + (1 - MB) * (1 - MB) * (1 - MB));
            K1 = S0 * (1 - (1 - MB) * (1 - MB));
            K2 = S0 * (1 - MB) * (1 - MB) * MB;
            K3 = S0 * (1 - MB) * (1 - MB) * (1 - MB);
            K4 = S1 * MB;
            K5 = S1 * (1 - MB);

            chain1 = new SpellCycle(5);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AB2, castingState);
            chain1.AddSpell(MBAM3, castingState);
            chain1.Calculate(castingState);

            chain2 = new SpellCycle(6);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            chain2.AddSpell(AB3, castingState);
            chain2.AddSpell(MBAM3, castingState);
            chain2.Calculate(castingState);

            chain3 = new SpellCycle(3);
            chain3.AddSpell(AB0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.Calculate(castingState);

            chain4 = new SpellCycle(4);
            chain4.AddSpell(AB3, castingState);
            chain4.AddSpell(AB3, castingState);
            chain4.AddSpell(MBAM3, castingState);
            chain4.Calculate(castingState);

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime + K3 * chain3.CastTime + K4 * chain4.CastTime + K5 * AB3.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond + K3 * chain3.CastTime * chain3.CostPerSecond + K4 * chain4.CastTime * chain4.CostPerSecond + K5 * AB3.CastTime * AB3.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond + K3 * chain3.CastTime * chain3.DamagePerSecond + K4 * chain4.CastTime * chain4.DamagePerSecond + K5 * AB3.CastTime * AB3.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond + K3 * chain3.CastTime * chain3.ThreatPerSecond + K4 * chain4.CastTime * chain4.ThreatPerSecond + K5 * AB3.CastTime * AB3.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond + K3 * chain3.CastTime * chain3.ManaRegenPerSecond + K4 * chain4.CastTime * chain4.ManaRegenPerSecond + K5 * AB3.CastTime * AB3.ManaRegenPerSecond) / CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * K1 * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * K2 * chain2.CastTime / CastTime);
            chain3.AddSpellContribution(dict, duration * K3 * chain3.CastTime / CastTime);
            chain4.AddSpellContribution(dict, duration * K4 * chain4.CastTime / CastTime);
            AB3.AddSpellContribution(dict, duration * K5 * AB3.CastTime / CastTime);
        }
    }

    class AB2ABar2C : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB, K1, K2;

        public AB2ABar2C(CastingState castingState)
        {
            Name = "AB2ABar2C";

            // S0: no proc at start
            // AB0-AB1-ABar2          => S0     (1-MB)*(1-MB)*(1-MB)
            //                        => S1     (1-MB)*(1 - (1-MB)*(1-MB))
            // AB0-AB1-MBAM2-ABar2C   => S0     MB*(1-MB)
            //                        => S1     MB*MB
            // S1: proc at start
            // AB0-AB1-MBAM2-ABar2C   => S0     (1-MB)
            //                        => S1     MB

            // S0 = S0 * ((1-MB)*(1-MB)*(1-MB) + MB*(1-MB)) + S1 * (1-MB)
            // S1 = S0 * ((1-MB)*(1 - (1-MB)*(1-MB)) + MB*MB) + S1 * MB
            // S0 + S1 = 1

            // (1-MB) = S0 * ((1-MB)*(1 - (1-MB)*(1-MB)) + MB*MB + 1-MB)
            // S0 = (1-MB) / ((1-MB)*(1 - (1-MB)*(1-MB)) + MB*MB + 1-MB)
            // S1 = ((1-MB)*(1 - (1-MB)*(1-MB)) + MB*MB) / ((1-MB)*(1 - (1-MB)*(1-MB)) + MB*MB + 1-MB)

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            //Spell MBAM2C = castingState.GetSpell(SpellId.ArcaneMissilesMB2Clipped);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);
            //Spell ABar2C = castingState.GetSpell(SpellId.ArcaneBarrage2Combo);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float S0 = (1 - MB) / ((1 - MB) * (1 - (1 - MB) * (1 - MB)) + MB * MB + 1 - MB);
            float S1 = 1 - S0;
            K1 = S0 * (1 - MB);
            K2 = S0 * MB + S1;

            chain1 = new SpellCycle(5);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(ABar2, castingState);
            chain1.Calculate(castingState);

            chain2 = new SpellCycle(6);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(MBAM2, castingState);
            chain2.AddSpell(ABar, castingState);
            chain2.Calculate(castingState);

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * K1 * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * K2 * chain2.CastTime / CastTime);
        }
    }

    class AB2ABar2MBAM : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB, K1, K2;

        public AB2ABar2MBAM(CastingState castingState)
        {
            Name = "AB2ABar2MBAM";

            // S0: no proc at start

            // AB0-AB1-ABar2          => S0     (1-MB)*(1-MB)*(1-MB)
            //                        => S1     (1-MB)*(1 - (1-MB)*(1-MB))
            // AB0-AB1-MBAM2          => S0     MB
            // S1: proc at start
            // AB0-AB1-MBAM2          => S0     1

            // S0 = S0 * ((1-MB)*(1-MB)*(1-MB) + MB) + S1
            // S1 = S0 * (1-MB)*(1 - (1-MB)*(1-MB))
            // S0 + S1 = 1

            // 1 = S0 * (1 + (1-MB)*(1 - (1-MB)*(1-MB)))
            // S0 = 1 / (1 + (1-MB)*(1 - (1-MB)*(1-MB)))
            // S1 = (1-MB)*(1 - (1-MB)*(1-MB)) / (1 + (1-MB)*(1 - (1-MB)*(1-MB)))

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float S0 = 1 / (1 + (1 - MB) * (1 - (1 - MB) * (1 - MB)));
            float S1 = 1 - S0;
            K1 = S0 * (1 - MB);
            K2 = S0 * MB + S1;

            chain1 = new SpellCycle(5);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(ABar2, castingState);
            chain1.Calculate(castingState);

            chain2 = new SpellCycle(6);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(MBAM2, castingState);
            chain2.Calculate(castingState);

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * K1 * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * K2 * chain2.CastTime / CastTime);
        }
    }

    class AB2ABar3C : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB, K1, K2;

        public AB2ABar3C(CastingState castingState)
        {
            Name = "AB2ABar3C";

            // S0: no proc at start
            // AB0-AB1-ABar2              => S0     (1-MB)*(1-MB)*(1-MB)
            //                            => S1     (1-MB)*(1 - (1-MB)*(1-MB))
            // AB0-AB1-AB2-MBAM3-ABar     => S0     MB*(1-MB)
            //                            => S1     MB*MB
            // S1: proc at start
            // AB0-AB1-AB2-MBAM3-ABar     => S0     (1-MB)
            //                            => S1     MB

            // S0 = S0 * ((1-MB)*(1-MB)*(1-MB) + MB*(1-MB)) + S1 * (1-MB)
            // S1 = S0 * ((1-MB)*(1 - (1-MB)*(1-MB)) + MB*MB) + S1 * MB
            // S0 + S1 = 1

            // (1-MB) = S0 * ((1-MB)*(1 - (1-MB)*(1-MB)) + MB*MB + 1-MB)
            // S0 = (1-MB) / ((1-MB)*(1 - (1-MB)*(1-MB)) + MB*MB + 1-MB)
            // S1 = ((1-MB)*(1 - (1-MB)*(1-MB)) + MB*MB) / ((1-MB)*(1 - (1-MB)*(1-MB)) + MB*MB + 1-MB)

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float S0 = (1 - MB) / ((1 - MB) * (1 - (1 - MB) * (1 - MB)) + MB * MB + 1 - MB);
            float S1 = 1 - S0;
            K1 = S0 * (1 - MB);
            K2 = S0 * MB + S1;

            chain1 = new SpellCycle(5);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(ABar2, castingState);
            chain1.Calculate(castingState);

            chain2 = new SpellCycle(6);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            chain2.AddSpell(MBAM3, castingState);
            chain2.AddSpell(ABar, castingState);
            chain2.Calculate(castingState);

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * K1 * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * K2 * chain2.CastTime / CastTime);
        }
    }

    class ABABar3C : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB, K1, K2;

        public ABABar3C(CastingState castingState)
        {
            Name = "ABABar3C";

            // S0: no proc at start
            // AB0-ABar1                  => S0     (1-MB)*(1-MB)
            //                            => S1     (1 - (1-MB)*(1-MB))
            // S1: proc at start
            // AB0-AB1-AB2-MBAM3-ABar     => S0     (1-MB)
            //                            => S1     MB

            // S0 = S0 * (1-MB)*(1-MB) + S1 * (1-MB)
            // S1 = S0 * (1 - (1-MB)*(1-MB)) + S1 * MB
            // S0 + S1 = 1

            // S0 = (1-MB) / (2 - MB - (1-MB)*(1-MB))
            // S1 = 1 - S0

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float S0 = (1 - MB) / (2 - MB - (1 - MB) * (1 - MB));
            float S1 = 1 - S0;
            K1 = S0;
            K2 = S1;

            chain1 = new SpellCycle(5);
            chain1.AddSpell(AB0, castingState);
            if (AB0.CastTime + ABar1.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - ABar1.CastTime);
            chain1.AddSpell(ABar1, castingState);
            chain1.Calculate(castingState);

            chain2 = new SpellCycle(6);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            chain2.AddSpell(MBAM3, castingState);
            chain2.AddSpell(ABar, castingState);
            chain2.Calculate(castingState);

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * K1 * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * K2 * chain2.CastTime / CastTime);
        }
    }

    class ABABar2C : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB, K1, K2;

        public ABABar2C(CastingState castingState)
        {
            Name = "ABABar2C";

            // S0: no proc at start
            // AB0-ABar1                  => S0     (1-MB)*(1-MB)
            //                            => S1     (1 - (1-MB)*(1-MB))
            // S1: proc at start
            // AB0-AB1-MBAM2-ABar         => S0     (1-MB)
            //                            => S1     MB

            // S0 = S0 * (1-MB)*(1-MB) + S1 * (1-MB)
            // S1 = S0 * (1 - (1-MB)*(1-MB)) + S1 * MB
            // S0 + S1 = 1

            // S0 = (1-MB) / (2 - MB - (1-MB)*(1-MB))
            // S1 = 1 - S0

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float S0 = (1 - MB) / (2 - MB - (1 - MB) * (1 - MB));
            float S1 = 1 - S0;
            K1 = S0;
            K2 = S1;

            chain1 = new SpellCycle(5);
            chain1.AddSpell(AB0, castingState);
            if (AB0.CastTime + ABar1.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - ABar1.CastTime);
            chain1.AddSpell(ABar1, castingState);
            chain1.Calculate(castingState);

            chain2 = new SpellCycle(6);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(MBAM2, castingState);
            chain2.AddSpell(ABar, castingState);
            chain2.Calculate(castingState);

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * K1 * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * K2 * chain2.CastTime / CastTime);
        }
    }

    class ABABar2MBAM : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB, K1, K2;

        public ABABar2MBAM(CastingState castingState)
        {
            Name = "ABABar2MBAM";

            // S0: no proc at start
            // AB0-ABar1                  => S0     (1-MB)*(1-MB)
            //                            => S1     (1 - (1-MB)*(1-MB))
            // S1: proc at start
            // AB0-AB1-MBAM2              => S0     

            // S0 = S0 * (1-MB)*(1-MB) + S1
            // S1 = S0 * (1 - (1-MB)*(1-MB))
            // S0 + S1 = 1

            // S0 = 1 / (2 - (1-MB)*(1-MB))
            // S1 = 1 - S0

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float S0 = 1 / (2 - (1 - MB) * (1 - MB));
            float S1 = 1 - S0;
            K1 = S0;
            K2 = S1;

            chain1 = new SpellCycle(5);
            chain1.AddSpell(AB0, castingState);
            if (AB0.CastTime + ABar1.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - ABar1.CastTime);
            chain1.AddSpell(ABar1, castingState);
            chain1.Calculate(castingState);

            chain2 = new SpellCycle(6);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(MBAM2, castingState);
            chain2.Calculate(castingState);

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * K1 * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * K2 * chain2.CastTime / CastTime);
        }
    }

    class ABABar1MBAM : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB, K1, K2;

        public ABABar1MBAM(CastingState castingState)
        {
            Name = "ABABar1MBAM";

            // S0: no proc at start
            // AB0-ABar1                  => S0     (1-MB)*(1-MB)
            //                            => S1     (1 - (1-MB)*(1-MB))
            // S1: proc at start
            // AB0-MBAM1                  => S0     

            // S0 = S0 * (1-MB)*(1-MB) + S1
            // S1 = S0 * (1 - (1-MB)*(1-MB))
            // S0 + S1 = 1

            // S0 = 1 / (2 - (1-MB)*(1-MB))
            // S1 = 1 - S0

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell MBAM1 = castingState.GetSpell(SpellId.ArcaneMissilesMB1);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float S0 = 1 / (2 - (1 - MB) * (1 - MB));
            float S1 = 1 - S0;
            K1 = S0;
            K2 = S1;

            chain1 = new SpellCycle(5);
            chain1.AddSpell(AB0, castingState);
            if (AB0.CastTime + ABar1.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - ABar1.CastTime);
            chain1.AddSpell(ABar1, castingState);
            chain1.Calculate(castingState);

            chain2 = new SpellCycle(6);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(MBAM1, castingState);
            chain2.Calculate(castingState);

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * K1 * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * K2 * chain2.CastTime / CastTime);
        }
    }

    class AB3ABar3C : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB, K1, K2;

        public AB3ABar3C(CastingState castingState)
        {
            Name = "AB3ABar3C";

            // S0: no proc at start
            // AB0-AB1-AB2-ABar3          => S0     (1-MB)*(1-MB)*(1-MB)*(1-MB)
            //                            => S1     (1-MB)*(1-MB)*(1 - (1-MB)*(1-MB))
            // AB0-AB1-AB2-MBAM3-ABar3C   => S0     (1 - (1-MB)*(1-MB))*(1-MB)
            //                            => S1     (1 - (1-MB)*(1-MB))*MB
            // S1: proc at start
            // AB0-AB1-AB2-MBAM3-ABar3C   => S0     (1-MB)
            //                            => S1     MB

            // S0 = S0 * ((1-MB)*(1-MB)*(1-MB)*(1-MB) + (1 - (1-MB)*(1-MB))*(1-MB)) + S1 * (1-MB)
            // S1 = S0 * ((1-MB)*(1-MB)*(1 - (1-MB)*(1-MB)) + (1 - (1-MB)*(1-MB))*MB) + S1 * MB
            // S0 + S1 = 1

            // S1 = S0 * (1 - (1-MB)*(1-MB)) * ((1-MB)*(1-MB) + MB) + S1 * MB
            // (1-MB) = S0 * [(1 - (1-MB)*(1-MB)) * ((1-MB)*(1-MB) + MB) + (1-MB)]
            // S0 = (1-MB) / [(1 - (1-MB)*(1-MB)) * ((1-MB)*(1-MB) + MB) + (1-MB)]
            // S1 = [(1 - (1-MB)*(1-MB)) * ((1-MB)*(1-MB) + MB)] / [(1 - (1-MB)*(1-MB)) * ((1-MB)*(1-MB) + MB) + (1-MB)]

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            //Spell MBAM3C = castingState.GetSpell(SpellId.ArcaneMissilesMB3Clipped);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
            //Spell ABar3C = castingState.GetSpell(SpellId.ArcaneBarrage3Combo);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float S0 = (1 - MB) / ((1 - (1 - MB) * (1 - MB)) * ((1 - MB) * (1 - MB) + MB) + (1 - MB));
            float S1 = 1 - S0;
            K1 = S0 * (1 - MB) * (1 - MB);
            K2 = S0 * (1 - (1 - MB) * (1 - MB)) + S1;

            chain1 = new SpellCycle(5);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AB2, castingState);
            chain1.AddSpell(ABar3, castingState);
            chain1.Calculate(castingState);

            chain2 = new SpellCycle(6);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            chain2.AddSpell(MBAM3, castingState);
            chain2.AddSpell(ABar, castingState);
            chain2.Calculate(castingState);

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * K1 * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * K2 * chain2.CastTime / CastTime);
        }
    }

    class AB3ABar3MBAM : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB, K1, K2;

        public AB3ABar3MBAM(CastingState castingState)
        {
            Name = "AB3ABar3MBAM";

            // S0: no proc at start
            // AB0-AB1-AB2-ABar3          => S0     (1-MB)*(1-MB)*(1-MB)*(1-MB)
            //                            => S1     (1-MB)*(1-MB)*(1 - (1-MB)*(1-MB))
            // AB0-AB1-AB2-MBAM3          => S0     (1 - (1-MB)*(1-MB))
            // S1: proc at start
            // AB0-AB1-AB2-MBAM3          => S0     1

            // S0 = S0 * ((1-MB)*(1-MB)*(1-MB)*(1-MB) + (1 - (1-MB)*(1-MB))) + S1
            // S1 = S0 * (1-MB)*(1-MB)*(1 - (1-MB)*(1-MB))
            // S0 + S1 = 1

            // S1 = S0 * (1-MB)*(1-MB)*(1 - (1-MB)*(1-MB))
            // S0 = 1 / (1 + (1-MB)*(1-MB)*(1 - (1-MB)*(1-MB)))
            // S1 = 1 - S0

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float S0 = 1 / (1 + (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB)));
            float S1 = 1 - S0;
            K1 = S0 * (1 - MB) * (1 - MB);
            K2 = S0 * (1 - (1 - MB) * (1 - MB)) + S1;

            chain1 = new SpellCycle(5);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AB2, castingState);
            chain1.AddSpell(ABar3, castingState);
            chain1.Calculate(castingState);

            chain2 = new SpellCycle(6);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            chain2.AddSpell(MBAM3, castingState);
            chain2.Calculate(castingState);

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * K1 * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * K2 * chain2.CastTime / CastTime);
        }
    }

    class AB3AMABar : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB, K1, K2;

        public AB3AMABar(CastingState castingState)
        {
            Name = "AB3AMABar";

            // ABar-AB0-AB1-AB2-AM     (1-MB)*(1-MB)*(1-MB)*(1-MB)
            // ABar-AB0-AB1-AB2-MBAM   1 - (1-MB)*(1-MB)*(1-MB)*(1-MB)

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            //Spell AM3C = castingState.GetSpell(SpellId.ArcaneMissiles3Clipped);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            //Spell MBAM3C = castingState.GetSpell(SpellId.ArcaneMissilesMB3Clipped);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
            //Spell ABar3C = castingState.GetSpell(SpellId.ArcaneBarrage3Combo);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            K1 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            K2 = (1 - (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB));

            chain1 = new SpellCycle(5);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AB2, castingState);
            chain1.AddSpell(AM3, castingState);
            chain1.AddSpell(ABar, castingState);
            chain1.Calculate(castingState);

            chain2 = new SpellCycle(6);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            chain2.AddSpell(MBAM3, castingState);
            chain2.AddSpell(ABar, castingState);
            chain2.Calculate(castingState);

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * K1 * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * K2 * chain2.CastTime / CastTime);
        }
    }

    class AB32AMABar : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        SpellCycle chain3;
        float MB, K1, K2, K3;

        public AB32AMABar(CastingState castingState)
        {
            Name = "AB32AMABar";

            // ABar-AB0-AB1-MBAM       1 - (1-MB)*(1-MB)
            // ABar-AB0-AB1-AB2-AM     (1-MB)*(1-MB)*(1-MB)*(1-MB)
            // ABar-AB0-AB1-AB2-MBAM   (1-MB)*(1-MB)*(1 - (1-MB)*(1-MB))

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22);
            Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            //Spell AM3C = castingState.GetSpell(SpellId.ArcaneMissiles3Clipped);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            //Spell MBAM2C = castingState.GetSpell(SpellId.ArcaneMissilesMB2Clipped);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            //Spell MBAM3C = castingState.GetSpell(SpellId.ArcaneMissilesMB3Clipped);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);
            //Spell ABar2C = castingState.GetSpell(SpellId.ArcaneBarrage2Combo);
            Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
            //Spell ABar3C = castingState.GetSpell(SpellId.ArcaneBarrage3Combo);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            K1 = 1 - (1 - MB) * (1 - MB);
            K2 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            K3 = (1 - (1 - MB) * (1 - MB)) * (1 - MB) * (1 - MB);

            chain1 = new SpellCycle(5);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(MBAM2, castingState);
            chain1.AddSpell(ABar, castingState);
            chain1.Calculate(castingState);

            chain2 = new SpellCycle(5);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            chain2.AddSpell(AM3, castingState);
            chain2.AddSpell(ABar, castingState);
            chain2.Calculate(castingState);

            chain3 = new SpellCycle(6);
            chain3.AddSpell(AB0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(MBAM3, castingState);
            chain3.AddSpell(ABar, castingState);
            chain3.Calculate(castingState);

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime + K3 * chain3.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond + K3 * chain3.CastTime * chain3.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond + K3 * chain3.CastTime * chain3.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond + K3 * chain3.CastTime * chain3.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond + K3 * chain3.CastTime * chain3.ManaRegenPerSecond) / CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * K1 * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * K2 * chain2.CastTime / CastTime);
            chain3.AddSpellContribution(dict, duration * K3 * chain3.CastTime / CastTime);
        }
    }

    class AB2AMABar : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB, K1, K2;

        public AB2AMABar(CastingState castingState)
        {
            Name = "AB2AMABar";

            // ABar-AB0-AB1-AM     (1-MB)*(1-MB)*(1-MB)
            // ABar-AB0-AB1-MBAM   1 - (1-MB)*(1-MB)*(1-MB)

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            Spell AM2 = castingState.GetSpell(SpellId.ArcaneMissiles2);
            //Spell AM2C = castingState.GetSpell(SpellId.ArcaneMissiles2Clipped);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            //Spell MBAM2C = castingState.GetSpell(SpellId.ArcaneMissilesMB2Clipped);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);
            //Spell ABar2C = castingState.GetSpell(SpellId.ArcaneBarrage2Combo);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            K1 = (1 - MB) * (1 - MB) * (1 - MB);
            K2 = (1 - (1 - MB) * (1 - MB) * (1 - MB));

            chain1 = new SpellCycle(5);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AM2, castingState);
            chain1.AddSpell(ABar, castingState);
            chain1.Calculate(castingState);

            chain2 = new SpellCycle(6);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(MBAM2, castingState);
            chain2.AddSpell(ABar, castingState);
            chain2.Calculate(castingState);

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * K1 * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * K2 * chain2.CastTime / CastTime);
        }
    }

    class ABAMABar : Spell
    {
        SpellCycle chain1;
        SpellCycle chain2;
        float MB, K1, K2;

        public ABAMABar(CastingState castingState)
        {
            Name = "ABAMABar";

            // ABar-AB0-AM     (1-MB)*(1-MB)
            // ABar-AB0-MBAM   1 - (1-MB)*(1-MB)

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell AM1 = castingState.GetSpell(SpellId.ArcaneMissiles1);
            //Spell AM1C = castingState.GetSpell(SpellId.ArcaneMissiles1Clipped);
            Spell MBAM1 = castingState.GetSpell(SpellId.ArcaneMissilesMB1);
            //Spell MBAM1C = castingState.GetSpell(SpellId.ArcaneMissilesMB1Clipped);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            //Spell ABar1C = castingState.GetSpell(SpellId.ArcaneBarrage1Combo);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            K1 = (1 - MB) * (1 - MB);
            K2 = (1 - (1 - MB) * (1 - MB));

            chain1 = new SpellCycle(5);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AM1, castingState);
            chain1.AddSpell(ABar, castingState);
            chain1.Calculate(castingState);

            chain2 = new SpellCycle(6);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(MBAM1, castingState);
            chain2.AddSpell(ABar, castingState);
            chain2.Calculate(castingState);

            CastTime = K1 * chain1.CastTime + K2 * chain2.CastTime;
            CostPerSecond = (K1 * chain1.CastTime * chain1.CostPerSecond + K2 * chain2.CastTime * chain2.CostPerSecond) / CastTime;
            DamagePerSecond = (K1 * chain1.CastTime * chain1.DamagePerSecond + K2 * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
            ThreatPerSecond = (K1 * chain1.CastTime * chain1.ThreatPerSecond + K2 * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (K1 * chain1.CastTime * chain1.ManaRegenPerSecond + K2 * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            chain1.AddSpellContribution(dict, duration * K1 * chain1.CastTime / CastTime);
            chain2.AddSpellContribution(dict, duration * K2 * chain2.CastTime / CastTime);
        }
    }

    class AB3AMSc : SpellCycle
    {
        public AB3AMSc(CastingState castingState) : base(12)
        {
            Name = "AB3AMSc";

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

    class ABABar : SpellCycle
    {
        public ABABar(CastingState castingState)
            : base(3)
        {
            Name = "ABABar";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);

            AddSpell(AB, castingState);
            if (AB.CastTime + ABar1.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - AB.CastTime - ABar1.CastTime);
            AddSpell(ABar1, castingState);

            Calculate(castingState);
        }
    }

    class ABAM3Sc : SpellCycle
    {
        public ABAM3Sc(CastingState castingState) : base(14)
        {
            Name = "ABAM3Sc";

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
            AffectedByFlameCap = true;

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

    class FBPyro : Spell
    {
        Fireball FB;
        SpellCycle chain2;
        float K;

        public FBPyro(CastingState castingState)
        {
            Name = "FBPyro";
            AffectedByFlameCap = true;

            FB = (Fireball)castingState.GetSpell(SpellId.Fireball);
            Spell Pyro = castingState.GetSpell(SpellId.PyroblastPOM);
            sequence = "Fireball";

            // no Pyro
            // FB => no Pyro 1 - c*c/(1+c)
            //    => Pyro c*c/(1+c)
            // Pyro
            // Pyro => no Pyro

            // 1 - c*c/(1+c)
            // FB

            // c*c/(1+c)
            chain2 = new SpellCycle(2);
            chain2.AddSpell(FB, castingState);
            chain2.AddSpell(Pyro, castingState);
            chain2.Calculate(castingState);

            K = FB.CritRate * FB.CritRate / (1.0f + FB.CritRate) * castingState.MageTalents.HotStreak / 3.0f;
            if (castingState.MageTalents.Pyroblast == 0) K = 0.0f;

            CastTime = (1 - K) * FB.CastTime + K * chain2.CastTime;
            CostPerSecond = ((1 - K) * FB.CastTime * FB.CostPerSecond + K * chain2.CastTime * chain2.CostPerSecond) / CastTime;
            DamagePerSecond = ((1 - K) * FB.CastTime * FB.DamagePerSecond + K * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
            ThreatPerSecond = ((1 - K) * FB.CastTime * FB.ThreatPerSecond + K * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = ((1 - K) * FB.CastTime * FB.ManaRegenPerSecond + K * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
            // needed for Combustion calculations
            CastProcs = (1 - K) * FB.CastProcs + K * chain2.CastProcs;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            FB.AddSpellContribution(dict, (1 - K) * FB.CastTime / CastTime * duration);
            chain2.AddSpellContribution(dict, K * chain2.CastTime / CastTime * duration);
        }
    }

    class FrBFB : Spell
    {
        BaseSpell FrB;
        SpellCycle chain2;
        float K;

        public FrBFB(CastingState castingState)
        {
            Name = "FrBFB";

            FrB = (BaseSpell)castingState.GetSpell(SpellId.FrostboltFOF);
            Spell FB = castingState.GetSpell(SpellId.FireballBF);
            sequence = "Frostbolt";

            // FrB      1 - brainFreeze
            // FrB-FB   brainFreeze

            chain2 = new SpellCycle(2);
            chain2.AddSpell(FrB, castingState);
            chain2.AddSpell(FB, castingState);
            chain2.Calculate(castingState);

            K = 0.05f * castingState.MageTalents.BrainFreeze;

            CastTime = (1 - K) * FrB.CastTime + K * chain2.CastTime;
            CostPerSecond = ((1 - K) * FrB.CastTime * FrB.CostPerSecond + K * chain2.CastTime * chain2.CostPerSecond) / CastTime;
            DamagePerSecond = ((1 - K) * FrB.CastTime * FrB.DamagePerSecond + K * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
            ThreatPerSecond = ((1 - K) * FrB.CastTime * FrB.ThreatPerSecond + K * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = ((1 - K) * FrB.CastTime * FrB.ManaRegenPerSecond + K * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
            // needed for Combustion calculations
            CastProcs = (1 - K) * FrB.CastProcs + K * chain2.CastProcs;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            FrB.AddSpellContribution(dict, (1 - K) * FrB.CastTime / CastTime * duration);
            chain2.AddSpellContribution(dict, K * chain2.CastTime / CastTime * duration);
        }
    }

    class FrBFBIL : Spell
    {
        Spell FrB, FrBS, FB, FBS, ILS;
        float KFrB, KFrBS, KFB, KFBS, KILS;

        public FrBFBIL(CastingState castingState)
        {
            Name = "FrBFBIL";

            // S00: FOF0, BF0
            // FrB => S21    fof * bf
            //        S20    fof * (1-bf)
            //        S01    (1-fof) * bf
            //        S00    (1-fof)*(1-bf)

            // S01: FOF0, BF1
            // FrB => S22    fof
            //        S02    (1-fof)

            // S02: FOF0, BF2
            // FB => S00    1

            // S10: FOF1, BF0
            // FrBS-ILS => S12    fof * bf
            //             S10    fof * (1-bf)
            //             S02    (1-fof) * bf
            //             S00    (1-fof)*(1-bf)

            // S11: FOF1, BF1
            // FrBS-FBS => S10    fof
            //             S00    (1-fof)

            // S12 = S11

            // S20: FOF0, BF0
            // FrBS => S21    fof * bf
            //         S20    fof * (1-bf)
            //         S11    (1-fof) * bf
            //         S10    (1-fof)*(1-bf)

            // S21: FOF0, BF1
            // FrBS => S22    fof
            //         S12    (1-fof)

            // S22 = S21

            // S00 = (1-fof)*(1-bf) * S00 + S02 + (1-fof)*(1-bf) * S10 + (1-fof) * S11
            // S01 = (1-fof) * bf * S00
            // S02 = (1-fof) * S01 + (1-fof) * bf * S10
            // S10 = fof * (1-bf) * S10 + fof * S11 + (1-fof)*(1-bf) * S20
            // S11 = fof * bf * S10 + (1-fof) * bf * S20 + (1-fof) * S21
            // S20 = fof * (1-bf) * S00 + fof * (1-bf) * S20
            // S21 = fof * bf * S00 + fof * S01 + fof * bf * S20 + fof * S21
            // S00 + S01 + S02 + S10 + S11 + S20 + S21 = 1

            // solved symbolically

            float bf = 0.05f * castingState.MageTalents.BrainFreeze;
            float fof = (castingState.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * castingState.MageTalents.FingersOfFrost);

            float div = ((bf * bf * bf - bf) * fof * fof * fof * fof + (3 * bf - bf * bf * bf) * fof * fof * fof + (bf * bf * bf - 4 * bf + 1) * fof * fof * fof + (-bf * bf * bf - 2 * bf * bf + 2 * bf) * fof - 2 * bf - 1);
            float S00 = ((bf * bf - bf) * fof * fof * fof + (-bf * bf + 3 * bf - 1) * fof * fof + (2 - 2 * bf) * fof - 1) / div;
            float S01 = -((bf * bf * bf - bf * bf) * fof * fof * fof * fof + (-2 * bf * bf * bf + 4 * bf * bf - bf) * fof * fof * fof + (bf * bf * bf - 5 * bf * bf + 3 * bf) * fof * fof + (2 * bf * bf - 3 * bf) * fof + bf) / div;
            float S02 = ((bf * bf - bf) * fof * fof * fof * fof + (-bf * bf * bf - bf * bf + 3 * bf) * fof * fof * fof + (2 * bf * bf * bf - 4 * bf) * fof * fof + (3 * bf - bf * bf * bf) * fof - bf) / div;
            float S10 = ((bf * bf - bf) * fof * fof * fof * fof + (3 * bf - 2 * bf * bf) * fof * fof * fof + (2 * bf * bf - 5 * bf + 1) * fof * fof + (-bf * bf + 2 * bf - 1) * fof) / div;
            float S11 = ((bf * bf * bf - 2 * bf * bf + bf) * fof * fof * fof * fof + (-bf * bf * bf + 4 * bf * bf - 3 * bf) * fof * fof * fof + (5 * bf - 4 * bf * bf) * fof * fof + (bf * bf - 3 * bf) * fof) / div;
            float S20 = -((bf * bf - bf) * fof * fof * fof + (-bf * bf + 2 * bf - 1) * fof * fof + (1 - bf) * fof) / div;
            float S21 = ((bf * bf * bf - bf * bf) * fof * fof * fof * fof + (-bf * bf * bf + 3 * bf * bf - bf) * fof * fof * fof + (2 * bf - 3 * bf * bf) * fof * fof - 2 * bf * fof) / div;

            KFrB = S00 + S01;
            KFB = S02;
            KFrBS = S10 + S11 + S20 + S21;
            KFBS = S11;
            KILS = S10;

            FrB = castingState.GetSpell(SpellId.Frostbolt);
            FrBS = castingState.FrozenState.GetSpell(SpellId.Frostbolt);
            FB = castingState.GetSpell(SpellId.FireballBF);
            FBS = castingState.FrozenState.GetSpell(SpellId.FireballBF);
            ILS = castingState.FrozenState.GetSpell(SpellId.IceLance);
            sequence = "Frostbolt";

            CastTime = KFrB * FrB.CastTime + KFB * FB.CastTime + KFrBS * FrBS.CastTime + KFBS * FBS.CastTime + KILS * ILS.CastTime;
            CostPerSecond = (KFrB * FrB.CastTime * FrB.CostPerSecond + KFB * FB.CastTime * FB.CostPerSecond + KFrBS * FrBS.CastTime * FrBS.CostPerSecond + KFBS * FBS.CastTime * FBS.CostPerSecond + KILS * ILS.CastTime * ILS.CostPerSecond) / CastTime;
            DamagePerSecond = (KFrB * FrB.CastTime * FrB.DamagePerSecond + KFB * FB.CastTime * FB.DamagePerSecond + KFrBS * FrBS.CastTime * FrBS.DamagePerSecond + KFBS * FBS.CastTime * FBS.DamagePerSecond + KILS * ILS.CastTime * ILS.DamagePerSecond) / CastTime;
            ThreatPerSecond = (KFrB * FrB.CastTime * FrB.ThreatPerSecond + KFB * FB.CastTime * FB.ThreatPerSecond + KFrBS * FrBS.CastTime * FrBS.ThreatPerSecond + KFBS * FBS.CastTime * FBS.ThreatPerSecond + KILS * ILS.CastTime * ILS.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (KFrB * FrB.CastTime * FrB.ManaRegenPerSecond + KFB * FB.CastTime * FB.ManaRegenPerSecond + KFrBS * FrBS.CastTime * FrBS.ManaRegenPerSecond + KFBS * FBS.CastTime * FBS.ManaRegenPerSecond + KILS * ILS.CastTime * ILS.ManaRegenPerSecond) / CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            FrB.AddSpellContribution(dict, KFrB * FrB.CastTime / CastTime * duration);
            FB.AddSpellContribution(dict, KFB * FB.CastTime / CastTime * duration);
            FrBS.AddSpellContribution(dict, KFrBS * FrBS.CastTime / CastTime * duration);
            FBS.AddSpellContribution(dict, KFBS * FBS.CastTime / CastTime * duration);
            ILS.AddSpellContribution(dict, KILS * ILS.CastTime / CastTime * duration);
        }
    }

    class FBLBPyro : Spell
    {
        BaseSpell FB;
        BaseSpell LB;
        BaseSpell Pyro;
        float X;
        float K;

        public FBLBPyro(CastingState castingState)
        {
            Name = "FBLBPyro";
            AffectedByFlameCap = true;

            FB = (BaseSpell)castingState.GetSpell(SpellId.Fireball);
            LB = (BaseSpell)castingState.GetSpell(SpellId.LivingBomb);
            Pyro = (BaseSpell)castingState.GetSpell(SpellId.PyroblastPOM);
            sequence = "Fireball";

            // 3.0.8 calcs

            // 0 HS charge:
            // FB     => 0 HS charge    (1 - FBcrit) * X
            //        => 1 HS charge    FBcrit * X
            // LB     => 0 HS charge    (1 - LBcrit) * (1 - X)
            //        => 1 HS charge    LBcrit * (1 - X)
            // 1 HS charge:
            // FB     => 0 HS charge    (1 - FBcrit) * X + (1 - H) * FBcrit * X
            // FBPyro => 0 HS charge    H * FBcrit * X
            // LB     => 0 HS charge    (1 - LBcrit) * (1 - X) + (1 - H) * LBcrit * (1 - X)
            // LBPyro => 0 HS charge    H * LBcrit * (1 - X)

            // S0 + S1 = 1
            // S0 = S0 * [(1 - FBcrit) * X + (1 - LBcrit) * (1 - X)] + S1
            // S1 = S0 * [FBcrit * X + LBcrit * (1 - X)]

            // 1 - S0 = S0 * [FBcrit * X + LBcrit * (1 - X)]
            // S0 = 1 / (1 + FBcrit * X + LBcrit * (1 - X))
            // C := FBcrit * X + LBcrit * (1 - X) = LBcrit + X * (FBcrit - LBcrit)
            // S0 = 1 / (1 + C)
            // S1 = C / (1 + C)

            // value = S0 * (X * value(FB) + (1 - X) * value(LB)) + S1 * (X * value(FB) + (1 - X) * value(LB) + H * C * value(Pyro))
            //       = X * value(FB) + (1 - X) * value(LB) + H * C * C / (1 + C) * value(Pyro)

            // time(LB) * (1 - X) / [time(FB) * X + time(LB) * (1 - X) + time(Pyro) * H * C * C / (1 + C)] = time(LB) / 12
            // 12 * (1 - X) = time(FB) * X + time(LB) * (1 - X) + time(Pyro) * H * C * C / (1 + C)

            // (1 + C) * (12 * (1 - X) - time(FB) * X - time(LB) * (1 - X)) = time(Pyro) * H * C * C
            // (1 + C) * (12 - 12 * X - time(FB) * X - time(LB) + time(LB) * X) = time(Pyro) * H * C * C
            // (1 + LBcrit + X * (FBcrit - LBcrit)) * (12 - time(LB) + X * (time(LB) - time(FB) - 12)) = time(Pyro) * H * C * C
            // [(1 + LBcrit) * (12 - time(LB)) - time(Pyro) * H * LBcrit * LBcrit] + X * [(FBcrit - LBcrit) * (12 - time(LB)) + (time(LB) - time(FB) - 12) * (1 + LBcrit) - time(Pyro) * H * 2 * LBcrit * (FBcrit - LBcrit)] + X * X * [(FBcrit - LBcrit) * (time(LB) - time(FB) - 12) - (FBcrit - LBcrit) * (FBcrit - LBcrit) * time(Pyro) * H] = 0

            /*if (castingState.CalculationOptions.Mode308)*/
            {
                float FBcrit = FB.CritRate;
                float LBcrit = LB.CritRate;
                float H = castingState.MageTalents.HotStreak / 3.0f;
                if (castingState.MageTalents.Pyroblast == 0) H = 0.0f;
                float A2 = (FBcrit - LBcrit) * (LB.CastTime - FB.CastTime - 12) - (FBcrit - LBcrit) * (FBcrit - LBcrit) * Pyro.CastTime * H;
                float A1 = (FBcrit - LBcrit) * (12 - LB.CastTime) + (LB.CastTime - FB.CastTime - 12) * (1 + LBcrit) - Pyro.CastTime * H * 2 * LBcrit * (FBcrit - LBcrit);
                float A0 = (1 + LBcrit) * (12 - LB.CastTime) - Pyro.CastTime * H * LBcrit * LBcrit;
                if (Math.Abs(A2) < 0.00001)
                {
                    X = -A0 / A1;
                }
                else
                {
                    X = (float)((-A1 - Math.Sqrt(A1 * A1 - 4 * A2 * A0)) / (2 * A2));
                }
                float C = LBcrit + X * (FBcrit - LBcrit);
                K = H * C * C / (1 + C);
            }

            // 3.0 calcs

            // Living Bomb shouldn't be cast more often than it's dot duration
            // time casting LB / all time casting <= LB cast time / 12

            // 0 HS charge:
            // FB     => 0 HS charge    (1 - FBcrit) * X
            //        => 1 HS charge    FBcrit * X
            // LB     => 0 HS charge    (1 - X)
            // 1 HS charge:
            // FB     => 0 HS charge    (1 - FBcrit) * X + (1 - H) * FBcrit * X
            // FBPyro => 0 HS charge    H * FBcrit * X
            // LB     => 1 HS charge    (1 - X)

            // S0 = FB0a + FB0b + LB0
            // S1 = FB1 + FBPyro + LB1

            // solve for stationary distribution
            // FB0a = (1 - FBcrit) * X * S0
            // FB0b = FBcrit * X * S0
            // LB0 = (1 - X) * S0
            // FB1 = ((1 - FBcrit) * X + (1 - H) * FBcrit * X) * S1
            // FBPyro = H * FBcrit * X * S1
            // LB1 = (1 - X) * S1

            // S0 + S1 = 1
            // S0 = FB0a + LB0 + FB1 + FBPyro
            // S1 = FB0b + LB1

            // S1 = FBcrit * X * S0 + (1 - X) * S1
            // X * S1 = FBcrit * X * S0
            // S1 = FBcrit * S0
            // S0 = 1 / (1 + FBcrit)

            // value = [value(FB) * X + value(LB) * (1 - X)] * 1 / (1 + FBcrit) + [value(FB) * X + value(LB) * (1 - X) + H * FBcrit * X * value(Pyro)] * FBcrit / (1 + FBcrit)
            //       = value(FB) * X + value(LB) * (1 - X) + value(Pyro) * H * X * FBcrit * FBcrit / (1 + FBcrit)

            // time(LB) * (1 - X) / [time(FB) * X + time(LB) * (1 - X) + time(Pyro) * H * X * FBcrit * FBcrit / (1 + FBcrit)] = time(LB) / 12
            // time(LB) * (1 - X) = time(LB) / 12 * [time(FB) * X + time(LB) * (1 - X) + time(Pyro) * H * X * FBcrit * FBcrit / (1 + FBcrit)]
            // 12 * (1 - X) = time(FB) * X + time(LB) * (1 - X) + time(Pyro) * H * X * FBcrit * FBcrit / (1 + FBcrit)
            // X * time(LB) - 12 * X - time(FB) * X - time(Pyro) * H * X * FBcrit * FBcrit / (1 + FBcrit) = time(LB) - 12
            // X * (time(LB) - 12 - time(FB) - time(Pyro) * H * FBcrit * FBcrit / (1 + FBcrit)) = time(LB) - 12
            // X = (12 - time(LB)) / (12 - time(LB) + time(FB) + time(Pyro) * H * FBcrit * FBcrit / (1 + FBcrit))

            /*else
            {
                K = FB.CritRate * FB.CritRate / (1.0f + FB.CritRate) * castingState.MageTalents.HotStreak / 3.0f;
                if (castingState.MageTalents.Pyroblast == 0) K = 0.0f;
                X = (12.0f - LB.CastTime) / (12.0f + FB.CastTime - LB.CastTime + Pyro.CastTime * K);
            }*/

            CastTime = X * FB.CastTime + (1 - X) * LB.CastTime + K * Pyro.CastTime;
            CostPerSecond = (X * FB.CastTime * FB.CostPerSecond + (1 - X) * LB.CastTime * LB.CostPerSecond + K * Pyro.CastTime * Pyro.CostPerSecond) / CastTime;
            DamagePerSecond = (X * FB.CastTime * FB.DamagePerSecond + (1 - X) * LB.CastTime * LB.DamagePerSecond + K * Pyro.CastTime * Pyro.DamagePerSecond) / CastTime;
            ThreatPerSecond = (X * FB.CastTime * FB.ThreatPerSecond + (1 - X) * LB.CastTime * LB.ThreatPerSecond + K * Pyro.CastTime * Pyro.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (X * FB.CastTime * FB.ManaRegenPerSecond + (1 - X) * LB.CastTime * LB.ManaRegenPerSecond + K * Pyro.CastTime * Pyro.ManaRegenPerSecond) / CastTime;
            CastProcs = X * FB.CastProcs + (1 - X) * LB.CastProcs + K * Pyro.CastProcs;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            FB.AddSpellContribution(dict, X * FB.CastTime / CastTime * duration);
            LB.AddSpellContribution(dict, (1 - X) * LB.CastTime / CastTime * duration);
            Pyro.AddSpellContribution(dict, K * Pyro.CastTime / CastTime * duration);
        }
    }

    class FFBLBPyro : Spell
    {
        BaseSpell FFB;
        BaseSpell LB;
        BaseSpell Pyro;
        float X;
        float K;

        public FFBLBPyro(CastingState castingState)
        {
            Name = "FFBLBPyro";
            AffectedByFlameCap = true;

            FFB = (BaseSpell)castingState.GetSpell(SpellId.FrostfireBolt);
            LB = (BaseSpell)castingState.GetSpell(SpellId.LivingBomb);
            Pyro = (BaseSpell)castingState.GetSpell(SpellId.PyroblastPOM);
            sequence = "Frostfire Bolt";

            /*if (castingState.CalculationOptions.Mode308)*/
            {
                float FFBcrit = FFB.CritRate;
                float LBcrit = LB.CritRate;
                float H = castingState.MageTalents.HotStreak / 3.0f;
                if (castingState.MageTalents.Pyroblast == 0) H = 0.0f;
                float A2 = (FFBcrit - LBcrit) * (LB.CastTime - FFB.CastTime - 12) - (FFBcrit - LBcrit) * (FFBcrit - LBcrit) * Pyro.CastTime * H;
                float A1 = (FFBcrit - LBcrit) * (12 - LB.CastTime) + (LB.CastTime - FFB.CastTime - 12) * (1 + LBcrit) - Pyro.CastTime * H * 2 * LBcrit * (FFBcrit - LBcrit);
                float A0 = (1 + LBcrit) * (12 - LB.CastTime) - Pyro.CastTime * H * LBcrit * LBcrit;
                if (Math.Abs(A2) < 0.00001)
                {
                    X = -A0 / A1;
                }
                else
                {
                    X = (float)((-A1 - Math.Sqrt(A1 * A1 - 4 * A2 * A0)) / (2 * A2));
                }
                float C = LBcrit + X * (FFBcrit - LBcrit);
                K = H * C * C / (1 + C);
            }
            /*else
            {
                K = FFB.CritRate * FFB.CritRate / (1.0f + FFB.CritRate) * castingState.MageTalents.HotStreak / 3.0f;
                if (castingState.MageTalents.Pyroblast == 0) K = 0.0f;
                X = (12.0f - LB.CastTime) / (12.0f + FFB.CastTime - LB.CastTime + Pyro.CastTime * K);
            }*/

            CastTime = X * FFB.CastTime + (1 - X) * LB.CastTime + K * Pyro.CastTime;
            CostPerSecond = (X * FFB.CastTime * FFB.CostPerSecond + (1 - X) * LB.CastTime * LB.CostPerSecond + K * Pyro.CastTime * Pyro.CostPerSecond) / CastTime;
            DamagePerSecond = (X * FFB.CastTime * FFB.DamagePerSecond + (1 - X) * LB.CastTime * LB.DamagePerSecond + K * Pyro.CastTime * Pyro.DamagePerSecond) / CastTime;
            ThreatPerSecond = (X * FFB.CastTime * FFB.ThreatPerSecond + (1 - X) * LB.CastTime * LB.ThreatPerSecond + K * Pyro.CastTime * Pyro.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (X * FFB.CastTime * FFB.ManaRegenPerSecond + (1 - X) * LB.CastTime * LB.ManaRegenPerSecond + K * Pyro.CastTime * Pyro.ManaRegenPerSecond) / CastTime;
            CastProcs = X * FFB.CastProcs + (1 - X) * LB.CastProcs + K * Pyro.CastProcs;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            FFB.AddSpellContribution(dict, X * FFB.CastTime / CastTime * duration);
            LB.AddSpellContribution(dict, (1 - X) * LB.CastTime / CastTime * duration);
            Pyro.AddSpellContribution(dict, K * Pyro.CastTime / CastTime * duration);
        }
    }

    class ScLBPyro : Spell
    {
        BaseSpell Sc;
        BaseSpell LB;
        BaseSpell Pyro;
        float X;
        float K;

        public ScLBPyro(CastingState castingState)
        {
            Name = "ScLBPyro";
            ProvidesScorch = (castingState.MageTalents.ImprovedScorch > 0);
            AffectedByFlameCap = true;

            Sc = (BaseSpell)castingState.GetSpell(SpellId.Scorch);
            LB = (BaseSpell)castingState.GetSpell(SpellId.LivingBomb);
            Pyro = (BaseSpell)castingState.GetSpell(SpellId.PyroblastPOM);
            sequence = "Scorch";

            /*if (castingState.CalculationOptions.Mode308)*/
            {
                float SCcrit = Sc.CritRate;
                float LBcrit = LB.CritRate;
                float H = castingState.MageTalents.HotStreak / 3.0f;
                if (castingState.MageTalents.Pyroblast == 0) H = 0.0f;
                float A2 = (SCcrit - LBcrit) * (LB.CastTime - Sc.CastTime - 12) - (SCcrit - LBcrit) * (SCcrit - LBcrit) * Pyro.CastTime * H;
                float A1 = (SCcrit - LBcrit) * (12 - LB.CastTime) + (LB.CastTime - Sc.CastTime - 12) * (1 + LBcrit) - Pyro.CastTime * H * 2 * LBcrit * (SCcrit - LBcrit);
                float A0 = (1 + LBcrit) * (12 - LB.CastTime) - Pyro.CastTime * H * LBcrit * LBcrit;
                if (Math.Abs(A2) < 0.00001)
                {
                    X = -A0 / A1;
                }
                else
                {
                    X = (float)((-A1 - Math.Sqrt(A1 * A1 - 4 * A2 * A0)) / (2 * A2));
                }
                float C = LBcrit + X * (SCcrit - LBcrit);
                K = H * C * C / (1 + C);
            }
            /*else
            {
                K = Sc.CritRate * Sc.CritRate / (1.0f + Sc.CritRate) * castingState.MageTalents.HotStreak / 3.0f;
                if (castingState.MageTalents.Pyroblast == 0) K = 0.0f;
                X = (12.0f - LB.CastTime) / (12.0f + Sc.CastTime - LB.CastTime + Pyro.CastTime * K);
            }*/

            CastTime = X * Sc.CastTime + (1 - X) * LB.CastTime + K * Pyro.CastTime;
            CostPerSecond = (X * Sc.CastTime * Sc.CostPerSecond + (1 - X) * LB.CastTime * LB.CostPerSecond + K * Pyro.CastTime * Pyro.CostPerSecond) / CastTime;
            DamagePerSecond = (X * Sc.CastTime * Sc.DamagePerSecond + (1 - X) * LB.CastTime * LB.DamagePerSecond + K * Pyro.CastTime * Pyro.DamagePerSecond) / CastTime;
            ThreatPerSecond = (X * Sc.CastTime * Sc.ThreatPerSecond + (1 - X) * LB.CastTime * LB.ThreatPerSecond + K * Pyro.CastTime * Pyro.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (X * Sc.CastTime * Sc.ManaRegenPerSecond + (1 - X) * LB.CastTime * LB.ManaRegenPerSecond + K * Pyro.CastTime * Pyro.ManaRegenPerSecond) / CastTime;
            CastProcs = X * Sc.CastProcs + (1 - X) * LB.CastProcs + K * Pyro.CastProcs;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            Sc.AddSpellContribution(dict, X * Sc.CastTime / CastTime * duration);
            LB.AddSpellContribution(dict, (1 - X) * LB.CastTime / CastTime * duration);
            Pyro.AddSpellContribution(dict, K * Pyro.CastTime / CastTime * duration);
        }
    }

    class FFBPyro : Spell
    {
        BaseSpell FFB;
        SpellCycle chain2;
        float K;

        public FFBPyro(CastingState castingState)
        {
            Name = "FFBPyro";
            AffectedByFlameCap = true;

            FFB = (BaseSpell)castingState.GetSpell(SpellId.FrostfireBoltFOF);
            Spell Pyro = castingState.GetSpell(SpellId.PyroblastPOM);
            sequence = "Frostfire Bolt";

            // no Pyro
            // FB => no Pyro 1 - c*c/(1+c)
            //    => Pyro c*c/(1+c)
            // Pyro
            // Pyro => no Pyro

            // 1 - c*c/(1+c)
            // FB

            // c*c/(1+c)
            chain2 = new SpellCycle(2);
            chain2.AddSpell(FFB, castingState);
            chain2.AddSpell(Pyro, castingState);
            chain2.Calculate(castingState);

            K = FFB.CritRate * FFB.CritRate / (1.0f + FFB.CritRate) * castingState.MageTalents.HotStreak / 3.0f;
            if (castingState.MageTalents.Pyroblast == 0) K = 0.0f;

            CastTime = (1 - K) * FFB.CastTime + K * chain2.CastTime;
            CostPerSecond = ((1 - K) * FFB.CastTime * FFB.CostPerSecond + K * chain2.CastTime * chain2.CostPerSecond) / CastTime;
            DamagePerSecond = ((1 - K) * FFB.CastTime * FFB.DamagePerSecond + K * chain2.CastTime * chain2.DamagePerSecond) / CastTime;
            ThreatPerSecond = ((1 - K) * FFB.CastTime * FFB.ThreatPerSecond + K * chain2.CastTime * chain2.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = ((1 - K) * FFB.CastTime * FFB.ManaRegenPerSecond + K * chain2.CastTime * chain2.ManaRegenPerSecond) / CastTime;
            // needed for Combustion calculations
            CastProcs = (1 - K) * FFB.CastProcs + K * chain2.CastProcs;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            FFB.AddSpellContribution(dict, (1 - K) * FFB.CastTime / CastTime * duration);
            chain2.AddSpellContribution(dict, K * chain2.CastTime / CastTime * duration);
        }
    }

    class FBScPyro : Spell
    {
        BaseSpell FB;
        BaseSpell Sc;
        BaseSpell Pyro;
        float K;
        float X;

        public FBScPyro(CastingState castingState)
        {
            Name = "FBScPyro";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            FB = (BaseSpell)castingState.GetSpell(SpellId.Fireball);
            Sc = (BaseSpell)castingState.GetSpell(SpellId.Scorch);
            Pyro = (BaseSpell)castingState.GetSpell(SpellId.PyroblastPOM);
            sequence = "Fireball";

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.CalculationOptions.GlyphOfImprovedScorch)
            {
                averageScorchesNeeded = 1;
                extraScorches = 0;
            }

            // proportion of time casting non-scorch spells has to be less than gap := (30 - (averageScorchesNeeded + extraScorches)) / (30 - extraScorches)
            // 0 HS charge:
            // FB     => 0 HS charge    (1 - FBcrit) * X
            //        => 1 HS charge    FBcrit * X
            // Sc     => 0 HS charge    (1 - SCcrit) * (1 - X)
            //           1 HS charge    SCcrit * (1 - X)
            // 1 HS charge:
            // FB     => 0 HS charge    (1 - FBcrit) * X + (1 - H) * FBcrit * X
            // FBPyro => 0 HS charge    H * FBcrit * X
            // Sc     => 0 HS charge    (1 - SCcrit) * (1 - X) + (1 - H) * SCcrit * (1 - X)
            // ScPyro => 0 HS charge    H * SCcrit * (1 - X)

            // S0 = FB0a + FB0b + Sc0a + Sc0b
            // S1 = FB1 + FBPyro + Sc1 + ScPyro
            
            // solve for stationary distribution
            // FB0a = (1 - FBcrit) * X * S0
            // FB0b = FBcrit * X * S0
            // Sc0a = (1 - SCcrit) * (1 - X) * S0
            // Sc0b = SCcrit * (1 - X) * S0
            // FB1 = ((1 - FBcrit) * X + (1 - H) * FBcrit * X) * S1
            // FBPyro = H * FBcrit * X * S1
            // Sc1 = ((1 - SCcrit) * (1 - X) + (1 - H) * SCcrit * (1 - X)) * S1
            // ScPyro = H * SCcrit * (1 - X) * S1

            // S0 + S1 = 1
            // S0 = FB0a + Sc0a + S1
            // S1 = FB0b + Sc0b

            // S1 = (FBcrit * X  + SCcrit * (1 - X)) * S0
            // C := (FBcrit * X  + SCcrit * (1 - X))
            //    = X * (FBcrit - SCcrit) + SCcrit

            // S1 = C * (1 - S1)
            // S1 = C / (1 + C)
            // S0 = 1 / (1 + C)

            // value = (X * value(FB) + (1 - X) * value(Sc)) * 1 / (1 + C) + (X * value(FB) + (1 - X) * value(Sc) + H * (FBcrit * X + SCcrit * (1 - X)) * value(Pyro)) * C / (1 + C)
            //         X * value(FB) + (1 - X) * value(Sc) + value(Pyro) * H * C * C / (1 + C)

            // (X * time(FB) + time(Pyro) * H * C * C / (1 + C)) / [X * time(FB) + (1 - X) * time(Sc) + time(Pyro) * H * C * C / (1 + C)] = gap
            // (X * time(FB) + time(Pyro) * H * C * C / (1 + C)) = gap * [X * time(FB) + time(Pyro) * H * C * C / (1 + C)] + gap * (1 - X) * time(Sc)
            // (X * time(FB) + time(Pyro) * H * C * C / (1 + C)) * (1 - gap) = gap * (1 - X) * time(Sc)
            // (X * (1 + C) * time(FB) + time(Pyro) * H * C * C) * (1 - gap) = gap * (1 - X) * (1 + C) * time(Sc)
            // (X * (1 + C) * time(FB) + time(Pyro) * H * C * C) * (1 - gap) = gap * (1 + C) * time(Sc) - gap * X * (1 + C) * time(Sc)
            // (X * time(FB) + X * C * time(FB) + time(Pyro) * H * C * C) * (1 - gap) = gap * time(Sc) + gap * C * time(Sc) - gap * X * time(Sc) - gap * X * C * time(Sc)
            // (X * time(FB) + X * (X * (FBcrit - SCcrit) + SCcrit) * time(FB) + time(Pyro) * H * (X * (FBcrit - SCcrit) + SCcrit) * (X * (FBcrit - SCcrit) + SCcrit)) * (1 - gap) = gap * time(Sc) + gap * (X * (FBcrit - SCcrit) + SCcrit) * time(Sc) - gap * X * time(Sc) - gap * X * (X * (FBcrit - SCcrit) + SCcrit) * time(Sc)
            // X * time(FB) * (1 - gap) + X * (X * (FBcrit - SCcrit) + SCcrit) * time(FB) * (1 - gap) + time(Pyro) * H * (X * (FBcrit - SCcrit) + SCcrit) * (X * (FBcrit - SCcrit) + SCcrit) * (1 - gap) = gap * time(Sc) + gap * (X * (FBcrit - SCcrit) + SCcrit) * time(Sc) - gap * X * time(Sc) - gap * X * (X * (FBcrit - SCcrit) + SCcrit) * time(Sc)
            // X * [time(FB) * (1 - gap) + SCcrit * time(FB) * (1 - gap) + 2 * (FBcrit - SCcrit) * SCcrit * time(Pyro) * H * (1 - gap)] + X * X * [(FBcrit - SCcrit) * time(FB) * (1 - gap) + (FBcrit - SCcrit) * (FBcrit - SCcrit) * time(Pyro) * H * (1 - gap)] + SCcrit * SCcrit * time(Pyro) * H * (1 - gap) = gap * time(Sc) + gap * (X * (FBcrit - SCcrit) + SCcrit) * time(Sc) - gap * X * time(Sc) - gap * X * (X * (FBcrit - SCcrit) + SCcrit) * time(Sc)
            // X * X * [(FBcrit - SCcrit) * time(FB) * (1 - gap) + (FBcrit - SCcrit) * (FBcrit - SCcrit) * time(Pyro) * H * (1 - gap) + gap * (FBcrit - SCcrit) * time(Sc)] + X * [time(FB) * (1 - gap) + SCcrit * time(FB) * (1 - gap) + 2 * (FBcrit - SCcrit) * SCcrit * time(Pyro) * H * (1 - gap) - gap * (FBcrit - SCcrit) * time(Sc) + gap * time(Sc) + gap * SCcrit * time(Sc)] + [SCcrit * SCcrit * time(Pyro) * H * (1 - gap) - gap * time(Sc) - gap * SCcrit * time(Sc)] = 0

            // A2 :=
            // (FBcrit - SCcrit) * time(FB) * (1 - gap) + (FBcrit - SCcrit) * (FBcrit - SCcrit) * time(Pyro) * H * (1 - gap) + gap * (FBcrit - SCcrit) * time(Sc)
            // (FBcrit - SCcrit) * [time(FB) * (1 - gap) + (FBcrit - SCcrit) * time(Pyro) * H * (1 - gap) + gap * time(Sc)]
            // (FBcrit - SCcrit) * [time(FB) * (1 - gap) + (FBcrit - SCcrit) * time(Pyro) * H * (1 - gap) - (1 - gap) * time(Sc) + time(Sc)]
            // (FBcrit - SCcrit) * [(1 - gap) * (time(FB) + (FBcrit - SCcrit) * time(Pyro) * H - time(Sc)) + time(Sc)]
            // A1 :=
            // time(FB) * (1 - gap) + SCcrit * time(FB) * (1 - gap) + 2 * (FBcrit - SCcrit) * SCcrit * time(Pyro) * H * (1 - gap) - gap * (FBcrit - SCcrit) * time(Sc) + gap * time(Sc) + gap * SCcrit * time(Sc)
            // time(FB) * [(1 - gap) + SCcrit * (1 - gap)] + time(Pyro) * H * [2 * (FBcrit - SCcrit) * SCcrit * (1 - gap)] + time(Sc) * [gap + gap * SCcrit - gap * (FBcrit - SCcrit)]
            // time(FB) * (1 - gap) * (1 + SCcrit) + time(Pyro) * H * [2 * (FBcrit - SCcrit) * SCcrit * (1 - gap)] + time(Sc) * gap * [1 + 2 * SCcrit - FBcrit]
            // (1 - gap) * [time(FB) * (1 + SCcrit) + time(Pyro) * H * 2 * (FBcrit - SCcrit) * SCcrit - time(Sc) * (1 + 2 * SCcrit - FBcrit)] + time(Sc) * (1 + 2 * SCcrit - FBcrit)
            // A0 :=
            // SCcrit * SCcrit * time(Pyro) * H * (1 - gap) - gap * time(Sc) * (1 + SCcrit)
            // (1 - gap) * (SCcrit * SCcrit * time(Pyro) * H + time(Sc) * (1 + SCcrit)) - time(Sc) * (1 + SCcrit)

            // A2 * X * X + A1 * X + A0 = 0
            // X = [- A1 +/- sqrt[A1 * A1 - 4 * A2 * A0]] / (2 * A2)

            // A1 * A1 - 4 * A2 * A0

            float gap = (30.0f - (averageScorchesNeeded + extraScorches) * Sc.CastTime) / (30.0f - extraScorches * Sc.CastTime);
            if (castingState.MageTalents.ImprovedScorch == 0)
            {
                ProvidesScorch = false; 
                gap = 1.0f;
            }
            float FBcrit = FB.CritRate;
            float SCcrit = Sc.CritRate;
            float H = castingState.MageTalents.HotStreak / 3.0f;
            if (castingState.MageTalents.Pyroblast == 0) H = 0.0f;
            float A2 = (FBcrit - SCcrit) * FB.CastTime * (1 - gap) + (FBcrit - SCcrit) * (FBcrit - SCcrit) * Pyro.CastTime * H * (1 - gap) + gap * (FBcrit - SCcrit) * Sc.CastTime;
            float A1 = FB.CastTime * (1 - gap) * (1 + SCcrit) + Pyro.CastTime * H * (2 * (FBcrit - SCcrit) * SCcrit * (1 - gap)) + Sc.CastTime * gap * (1 + 2 * SCcrit - FBcrit);
            float A0 = SCcrit * SCcrit * Pyro.CastTime * H * (1 - gap) - gap * Sc.CastTime * (1 + SCcrit);
            if (Math.Abs(A2) < 0.00001)
            {
                X = -A0 / A1;
            }
            else
            {
                X = (float)((-A1 + Math.Sqrt(A1 * A1 - 4 * A2 * A0)) / (2 * A2));
            }
            if (gap == 1.0f) X = 1.0f; //avoid rounding errors
            float C = X * (FBcrit - SCcrit) + SCcrit;
            K = H * C * C / (1 + C);

            // X * value(FB) + (1 - X) * value(Sc) + value(Pyro) * H * C * C / (1 + C)
            CastTime = X * FB.CastTime + (1 - X) * Sc.CastTime + K * Pyro.CastTime;
            CostPerSecond = (X * FB.CastTime * FB.CostPerSecond + (1 - X) * Sc.CastTime * Sc.CostPerSecond + K * Pyro.CastTime * Pyro.CostPerSecond) / CastTime;
            DamagePerSecond = (X * FB.CastTime * FB.DamagePerSecond + (1 - X) * Sc.CastTime * Sc.DamagePerSecond + K * Pyro.CastTime * Pyro.DamagePerSecond) / CastTime;
            ThreatPerSecond = (X * FB.CastTime * FB.ThreatPerSecond + (1 - X) * Sc.CastTime * Sc.ThreatPerSecond + K * Pyro.CastTime * Pyro.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (X * FB.CastTime * FB.ManaRegenPerSecond + (1 - X) * Sc.CastTime * Sc.ManaRegenPerSecond + K * Pyro.CastTime * Pyro.ManaRegenPerSecond) / CastTime;
            CastProcs = X * FB.CastProcs + (1 - X) * Sc.CastProcs + K * Pyro.CastProcs;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            FB.AddSpellContribution(dict, X * FB.CastTime / CastTime * duration);
            Sc.AddSpellContribution(dict, (1 - X) * Sc.CastTime / CastTime * duration);
            Pyro.AddSpellContribution(dict, K * Pyro.CastTime / CastTime * duration);
        }
    }

    class FBScLBPyro : Spell
    {
        BaseSpell FB;
        BaseSpell Sc;
        BaseSpell LB;
        BaseSpell Pyro;
        float K;
        float X;
        float Y;

        public FBScLBPyro(CastingState castingState)
        {
            Name = "FBScLBPyro";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            FB = (BaseSpell)castingState.GetSpell(SpellId.Fireball);
            Sc = (BaseSpell)castingState.GetSpell(SpellId.Scorch);
            LB = (BaseSpell)castingState.GetSpell(SpellId.LivingBomb);
            Pyro = (BaseSpell)castingState.GetSpell(SpellId.PyroblastPOM);
            sequence = "Fireball";

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.CalculationOptions.GlyphOfImprovedScorch)
            {
                averageScorchesNeeded = 1;
                extraScorches = 0;
            }

            // 3.0.8 calculations

            // 0 HS charge:
            // FB     => 0 HS charge    (1 - FBcrit) * X
            //        => 1 HS charge    FBcrit * X
            // LB     => 0 HS charge    (1 - LBcrit) * (1 - X - Y)
            //        => 1 HS charge    LBcrit * (1 - X - Y)
            // Sc     => 0 HS charge    (1 - SCcrit) * Y
            //           1 HS charge    SCcrit * Y
            // 1 HS charge:
            // FB     => 0 HS charge    (1 - FBcrit) * X + (1 - H) * FBcrit * X
            // FBPyro => 0 HS charge    H * FBcrit * X
            // LB     => 0 HS charge    (1 - LBcrit) * (1 - X - Y) + (1 - H) * LBcrit * (1 - X - Y)
            // LBPyro => 0 HS charge    H * LBcrit * (1 - X - Y)
            // Sc     => 0 HS charge    (1 - SCcrit) * Y + (1 - H) * SCcrit * Y
            // ScPyro => 0 HS charge    H * SCcrit * Y

            // S0 + S1 = 1
            // C := FBcrit * X + SCcrit * Y + LBcrit * (1 - X - Y)
            // S0 = S0 * (1 - C) + S1
            // S1 = S0 * C
            // 1 - S0 = S0 * C
            // S0 = 1 / (1 + C)
            // S1 = C / (1 + C)

            // value = (X * value(FB) + Y * value(Sc) + (1 - X - Y) * value(LB)) * 1 / (1 + C) + (X * value(FB) + Y * value(Sc) + (1 - X - Y) * value(LB) + H * C * value(Pyro)) * C / (1 + C)
            //         X * value(FB) + Y * value(Sc) + (1 - X - Y) * value(LB) + value(Pyro) * H * C * C / (1 + C)

            // (1 - X - Y) * time(LB) / [X * time(FB) + Y * time(Sc) + (1 - X - Y) * time(LB) + time(Pyro) * H * C * C / (1 + C)] = time(LB) / 12
            // Z = 1 - X - Y
            // Z * time(LB) / [X * time(FB) + Y * time(Sc) + Z * time(LB) + time(Pyro) * H * C * C / (1 + C)] = time(LB) / 12
            // 12 * Z = X * time(FB) + Y * time(Sc) + Z * time(LB) + time(Pyro) * H * C * C / (1 + C)
            // T := X * time(FB) + Y * time(Sc) + Z * time(LB) + time(Pyro) * H * C * C / (1 + C)
            // 12 * Z = T

            // (X * time(FB) + Z * time(LB) + time(Pyro) * H * C * C / (1 + C)) / T = gap
            // (T - Y * time(Sc)) / T = gap
            // T * (1 - gap) = Y * time(Sc)
            // if gap = 1 => Y = 0
            // 12 * Z * (1 - gap) = Y * time(Sc)
            // X = 1 - Y * [1 + time(Sc) / (12 * (1 - gap))]
            // P := 1 + time(Sc) / (12 * (1 - gap))
            // X = 1 - P * Y
            // Z = 1 - X - Y = 1 - 1 + P * Y - Y = Y * (P - 1)
            // C = (FBcrit * X  + SCcrit * Y + LBcrit * Z)
            //     (FBcrit * (1 - P * Y) + SCcrit * Y + LBcrit * Y * (P - 1))
            //     (FBcrit + Y * (SCcrit - FBcrit * P + LBcrit * (P - 1)))
            // C / (1 + C) = (FBcrit + Y * (SCcrit - FBcrit * P + LBcrit * (P - 1))) / (1 + FBcrit + Y * (SCcrit - FBcrit * P + LBcrit * (P - 1)))
            // 12 * Y * (P - 1) = T
            // CY := SCcrit - FBcrit * P + LBcrit * (P - 1)
            // C = FBcrit + Y * CY

            // T =
            // X * time(FB) + Y * time(Sc) + Z * time(LB) + time(Pyro) * H * C * C / (1 + C)
            // (1 - P * Y) * time(FB) + Y * time(Sc) + Y * (P - 1) * time(LB) + time(Pyro) * H * C * C / (1 + C)
            // time(FB) + Y * [time(Sc) - P * time(FB) + (P - 1) * time(LB)] + time(Pyro) * H * C * C / (1 + C)

            // 0 = time(FB) + Y * [time(Sc) - P * time(FB) + (P - 1) * time(LB) - 12 * (P - 1)] + time(Pyro) * H * C * C / (1 + C)
            // T1 := time(Sc) - P * time(FB) + (P - 1) * time(LB) - 12 * (P - 1)
            // 0 = time(FB) + Y * T1 + time(Pyro) * H * C * C / (1 + C)
            // 0 = time(FB) + C * time(FB) + Y * T1 + Y * C * T1 + time(Pyro) * H * C * C
            // 0 = time(FB) + (FBcrit + Y * CY) * time(FB) + Y * T1 + Y * (FBcrit + Y * CY) * T1 + time(Pyro) * H * (FBcrit + Y * CY) * (FBcrit + Y * CY)
            // 0 = [time(FB) + FBcrit * time(FB) + time(Pyro) * H * FBcrit * FBcrit] + Y * [CY * time(FB) + T1 + FBcrit * T1 + 2 * time(Pyro) * H * FBcrit * CY] + Y * Y * [CY * T1 + time(Pyro) * H * CY * CY]

            /*if (castingState.CalculationOptions.Mode308)*/
            {
                float gap = (30.0f - (averageScorchesNeeded + extraScorches) * Sc.CastTime) / (30.0f - extraScorches * Sc.CastTime);
                if (castingState.MageTalents.ImprovedScorch == 0)
                {
                    ProvidesScorch = false;
                    gap = 1.0f;
                }
                if (gap == 1.0f)
                {
                    Y = 0.0f;
                    float FBcrit = FB.CritRate;
                    float LBcrit = LB.CritRate;
                    float H = castingState.MageTalents.HotStreak / 3.0f;
                    if (castingState.MageTalents.Pyroblast == 0) H = 0.0f;
                    float A2 = (FBcrit - LBcrit) * (LB.CastTime - FB.CastTime - 12) - (FBcrit - LBcrit) * (FBcrit - LBcrit) * Pyro.CastTime * H;
                    float A1 = (FBcrit - LBcrit) * (12 - LB.CastTime) + (LB.CastTime - FB.CastTime - 12) * (1 + LBcrit) - Pyro.CastTime * H * 2 * LBcrit * (FBcrit - LBcrit);
                    float A0 = (1 + LBcrit) * (12 - LB.CastTime) - Pyro.CastTime * H * LBcrit * LBcrit;
                    if (Math.Abs(A2) < 0.00001)
                    {
                        X = -A0 / A1;
                    }
                    else
                    {
                        X = (float)((-A1 - Math.Sqrt(A1 * A1 - 4 * A2 * A0)) / (2 * A2));
                    }
                    float C = LBcrit + X * (FBcrit - LBcrit);
                    K = H * C * C / (1 + C);
                }
                else
                {
                    float P = 1.0f + Sc.CastTime / (12.0f * (1.0f - gap));
                    float FBcrit = FB.CritRate;
                    float SCcrit = Sc.CritRate;
                    float LBcrit = LB.CritRate;
                    float H = castingState.MageTalents.HotStreak / 3.0f;
                    if (castingState.MageTalents.Pyroblast == 0) H = 0.0f;
                    float T1 = Sc.CastTime - P * FB.CastTime + (P - 1) * LB.CastTime - 12 * (P - 1);
                    float CY = SCcrit - FBcrit * P + LBcrit * (P - 1);

                    float A2 = CY * T1 + Pyro.CastTime * H * CY * CY;
                    float A1 = CY * FB.CastTime + T1 + FBcrit * T1 + 2 * Pyro.CastTime * H * FBcrit * CY;
                    float A0 = FB.CastTime + FBcrit * FB.CastTime + Pyro.CastTime * H * FBcrit * FBcrit;
                    if (Math.Abs(A2) < 0.00001)
                    {
                        Y = -A0 / A1;
                    }
                    else
                    {
                        Y = (float)((-A1 - Math.Sqrt(A1 * A1 - 4 * A2 * A0)) / (2 * A2));
                    }
                    X = 1 - P * Y;
                    float C = (FBcrit * X + SCcrit * Y + LBcrit * (1 - X - Y));
                    K = H * C * C / (1 + C);
                }
            }

            // 3.0 calculations

            // Living Bomb shouldn't be cast more often than it's dot duration
            // time casting LB / all time casting <= LB cast time / 12

            // proportion of time casting non-scorch spells has to be less than gap := (30 - (averageScorchesNeeded + extraScorches)) / (30 - extraScorches)

            // 0 HS charge:
            // FB     => 0 HS charge    (1 - FBcrit) * X
            //        => 1 HS charge    FBcrit * X
            // LB     => 0 HS charge    1 - X - Y
            // Sc     => 0 HS charge    (1 - SCcrit) * Y
            //           1 HS charge    SCcrit * Y
            // 1 HS charge:
            // FB     => 0 HS charge    (1 - FBcrit) * X + (1 - H) * FBcrit * X
            // FBPyro => 0 HS charge    H * FBcrit * X
            // LB     => 1 HS charge    1 - X - Y
            // Sc     => 0 HS charge    (1 - SCcrit) * Y + (1 - H) * SCcrit * Y
            // ScPyro => 0 HS charge    H * SCcrit * Y

            // S0 = FB0a + FB0b + LB0 + Sc0a + Sc0b
            // S1 = FB1 + FBPyro + LB1 + Sc1 + ScPyro

            // solve for stationary distribution
            // FB0a = (1 - FBcrit) * X * S0
            // FB0b = FBcrit * X * S0
            // LB0 = (1 - X - Y) * S0
            // Sc0a = (1 - SCcrit) * Y * S0
            // Sc0b = SCcrit * Y * S0
            // FB1 = ((1 - FBcrit) * X + (1 - H) * FBcrit * X) * S1
            // FBPyro = H * FBcrit * X * S1
            // LB1 = (1 - X - Y) * S1
            // Sc1 = ((1 - SCcrit) * Y + (1 - H) * SCcrit * Y) * S1
            // ScPyro = H * SCcrit * Y * S1

            // S0 + S1 = 1
            // S0 = FB0a + Sc0a + S1 - LB1
            // S1 = FB0b + Sc0b + LB1

            // S1 = (FBcrit * X  + SCcrit * Y) * S0 + (1 - X - Y) * S1
            // S1 = (FBcrit * X  + SCcrit * Y) / (X + Y) * S0
            // C := (FBcrit * X  + SCcrit * Y) / (X + Y)

            // S1 = C * (1 - S1)
            // S1 = C / (1 + C)
            // S0 = 1 / (1 + C)

            // value = (X * value(FB) + Y * value(Sc) + (1 - X - Y) * value(LB)) * 1 / (1 + C) + (X * value(FB) + Y * value(Sc) + (1 - X - Y) * value(LB) + H * (FBcrit * X + SCcrit * Y) * value(Pyro)) * C / (1 + C)
            //         X * value(FB) + Y * value(Sc) + (1 - X - Y) * value(LB) + value(Pyro) * H * (X + Y) * C * C / (1 + C)

            // (1 - X - Y) * time(LB) / [X * time(FB) + Y * time(Sc) + (1 - X - Y) * time(LB) + time(Pyro) * H * (X + Y) * C * C / (1 + C)] = time(LB) / 12
            // Z = 1 - X - Y
            // Z * time(LB) / [X * time(FB) + Y * time(Sc) + Z * time(LB) + time(Pyro) * H * (1 - Z) * C * C / (1 + C)] = time(LB) / 12
            // 12 * Z = X * time(FB) + Y * time(Sc) + Z * time(LB) + time(Pyro) * H * (1 - Z) * C * C / (1 + C)
            // T := X * time(FB) + Y * time(Sc) + Z * time(LB) + time(Pyro) * H * (1 - Z) * C * C / (1 + C)
            // 12 * Z = T

            // (X * time(FB) + Z * time(LB) + time(Pyro) * H * (1 - Z) * C * C / (1 + C)) / T = gap
            // (T - Y * time(Sc)) / T = gap
            // T * (1 - gap) = Y * time(Sc)
            // if gap = 1 => Y = 0
            // 12 * Z * (1 - gap) = Y * time(Sc)
            // X = 1 - Y * [1 + time(Sc) / (12 * (1 - gap))]
            // P := 1 + time(Sc) / (12 * (1 - gap))
            // X = 1 - P * Y
            // Z = 1 - X - Y = 1 - 1 + P * Y - Y = Y * (P - 1)
            // C = (FBcrit * X  + SCcrit * Y) / (X + Y)
            //     (FBcrit * (1 - P * Y) + SCcrit * Y) / (1 - Y * (P - 1))
            //     (FBcrit + Y * (SCcrit - FBcrit * P)) / (1 - Y * (P - 1))
            // 1 + C = (1 - Y * (P - 1) + FBcrit + Y * (SCcrit - FBcrit * P)) / (1 - Y * (P - 1))
            // C / (1 + C) = (FBcrit + Y * (SCcrit - FBcrit * P)) / (1 + FBcrit + Y * (1 + SCcrit - P * (1 + FBcrit)))
            // 12 * Y * (P - 1) = T

            // T =
            // X * time(FB) + Y * time(Sc) + Z * time(LB) + time(Pyro) * H * (1 - Z) * C * C / (1 + C)
            // (1 - P * Y) * time(FB) + Y * time(Sc) + Y * (P - 1) * time(LB) + time(Pyro) * H * (1 - Y * (P - 1)) * C * C / (1 + C)
            // (1 - P * Y) * time(FB) + Y * time(Sc) + Y * (P - 1) * time(LB) + time(Pyro) * H * (FBcrit + Y * (SCcrit - FBcrit * P)) * C / (1 + C)
            // time(FB) + Y * [time(Sc) - P * time(FB) + (P - 1) * time(LB)] + time(Pyro) * H * (FBcrit + Y * (SCcrit - FBcrit * P)) * C / (1 + C)
            // time(FB) + Y * [time(Sc) - P * time(FB) + (P - 1) * time(LB)] + time(Pyro) * H * (FBcrit + Y * (SCcrit - FBcrit * P)) * (FBcrit + Y * (SCcrit - FBcrit * P)) / (1 + FBcrit + Y * (1 + SCcrit - P * (1 + FBcrit)))

            // 0 = time(FB) + Y * [time(Sc) - P * time(FB) + (P - 1) * time(LB) - 12 * (P - 1)] + time(Pyro) * H * (FBcrit + Y * (SCcrit - FBcrit * P)) * (FBcrit + Y * (SCcrit - FBcrit * P)) / (1 + FBcrit + Y * (1 + SCcrit - P * (1 + FBcrit)))
            // K1 := time(Sc) - P * time(FB) + (P - 1) * time(LB) - 12 * (P - 1)
            // K2 := SCcrit - FBcrit * P
            // K3 := 1 + SCcrit - P * (1 + FBcrit) = 1 - P + K2

            // 0 = time(FB) + Y * K1 + time(Pyro) * H * (FBcrit + Y * K2) * (FBcrit + Y * K2) / (1 + FBcrit + Y * K3)
            // 0 = time(FB) * (1 + FBcrit + Y * K3) + Y * K1 * (1 + FBcrit + Y * K3) + time(Pyro) * H * (FBcrit + Y * K2) * (FBcrit + Y * K2)
            // 0 = time(FB) * (1 + FBcrit) + time(FB) * Y * K3 + Y * K1 * (1 + FBcrit) + Y * Y * K3 * K1 + time(Pyro) * H * (FBcrit + Y * K2) * (FBcrit + Y * K2)
            // 0 = time(FB) * (1 + FBcrit) + Y * [time(FB) * K3 + K1 * (1 + FBcrit)] + Y * Y * [K3 * K1] + time(Pyro) * H * (FBcrit * FBcrit + 2 * FBcrit * K2 * Y + Y * Y * K2 * K2)
            // 0 = [time(FB) * (1 + FBcrit) + time(Pyro) * H * FBcrit * FBcrit] + Y * [time(FB) * K3 + K1 * (1 + FBcrit) + time(Pyro) * H * 2 * FBcrit * K2] + Y * Y * [K3 * K1 + time(Pyro) * H * K2 * K2]

            // A2 := K3 * K1 + time(Pyro) * H * K2 * K2
            // A1 := time(FB) * K3 + K1 * (1 + FBcrit) + time(Pyro) * H * 2 * FBcrit * K2
            // A0 := time(FB) * (1 + FBcrit) + time(Pyro) * H * FBcrit * FBcrit

            // A2 * Y * Y + A1 * Y + A0 = 0
            // Y = [- A1 +/- sqrt[A1 * A1 - 4 * A2 * A0]] / (2 * A2)

            /*else
            {
                float gap = (30.0f - (averageScorchesNeeded + extraScorches) * Sc.CastTime) / (30.0f - extraScorches * Sc.CastTime);
                if (castingState.MageTalents.ImprovedScorch == 0)
                {
                    ProvidesScorch = false;
                    gap = 1.0f;
                }
                if (gap == 1.0f)
                {
                    Y = 0.0f;
                    K = FB.CritRate * FB.CritRate / (1.0f + FB.CritRate) * castingState.MageTalents.HotStreak / 3.0f;
                    if (castingState.MageTalents.Pyroblast == 0) K = 0.0f;
                    X = (12.0f - LB.CastTime) / (12.0f + FB.CastTime - LB.CastTime + Pyro.CastTime * K);
                }
                else
                {
                    float P = 1.0f + Sc.CastTime / (12.0f * (1.0f - gap));
                    float FBcrit = FB.CritRate;
                    float SCcrit = Sc.CritRate;
                    float H = castingState.MageTalents.HotStreak / 3.0f;
                    if (castingState.MageTalents.Pyroblast == 0) H = 0.0f;
                    float K1 = Sc.CastTime - P * FB.CastTime + (P - 1) * LB.CastTime - 12 * (P - 1);
                    float K2 = SCcrit - FBcrit * P;
                    float K3 = 1.0f - P + K2;

                    float A2 = K3 * K1 + Pyro.CastTime * H * K2 * K2;
                    float A1 = FB.CastTime * K3 + K1 * (1 + FBcrit) + Pyro.CastTime * H * 2 * FBcrit * K2;
                    float A0 = FB.CastTime * (1 + FBcrit) + Pyro.CastTime * H * FBcrit * FBcrit;
                    if (Math.Abs(A2) < 0.00001)
                    {
                        Y = -A0 / A1;
                    }
                    else
                    {
                        Y = (float)((-A1 - Math.Sqrt(A1 * A1 - 4 * A2 * A0)) / (2 * A2));
                    }
                    X = 1 - P * Y;
                    float C = (FBcrit * X + SCcrit * Y) / (X + Y);
                    K = H * (X + Y) * C * C / (1 + C);
                }
            }*/

            // X * value(FB) + Y * value(Sc) + (1 - X - Y) * value(LB) + K * value(Pyro)
            CastTime = X * FB.CastTime + Y * Sc.CastTime + (1 - X - Y) * LB.CastTime + K * Pyro.CastTime;
            CostPerSecond = (X * FB.CastTime * FB.CostPerSecond + Y * Sc.CastTime * Sc.CostPerSecond + (1 - X - Y) * LB.CastTime * LB.CostPerSecond + K * Pyro.CastTime * Pyro.CostPerSecond) / CastTime;
            DamagePerSecond = (X * FB.CastTime * FB.DamagePerSecond + Y * Sc.CastTime * Sc.DamagePerSecond + (1 - X - Y) * LB.CastTime * LB.DamagePerSecond + K * Pyro.CastTime * Pyro.DamagePerSecond) / CastTime;
            ThreatPerSecond = (X * FB.CastTime * FB.ThreatPerSecond + Y * Sc.CastTime * Sc.ThreatPerSecond + (1 - X - Y) * LB.CastTime * LB.ThreatPerSecond + K * Pyro.CastTime * Pyro.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (X * FB.CastTime * FB.ManaRegenPerSecond + Y * Sc.CastTime * Sc.ManaRegenPerSecond + (1 - X - Y) * LB.CastTime * LB.ManaRegenPerSecond + K * Pyro.CastTime * Pyro.ManaRegenPerSecond) / CastTime;
            CastProcs = X * FB.CastProcs + Y * Sc.CastProcs + (1 - X - Y) * LB.CastProcs + K * Pyro.CastProcs;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            FB.AddSpellContribution(dict, X * FB.CastTime / CastTime * duration);
            Sc.AddSpellContribution(dict, Y * Sc.CastTime / CastTime * duration);
            LB.AddSpellContribution(dict, (1 - X - Y) * LB.CastTime / CastTime * duration);
            Pyro.AddSpellContribution(dict, K * Pyro.CastTime / CastTime * duration);
        }
    }

    class FFBScLBPyro : Spell
    {
        BaseSpell FFB;
        BaseSpell Sc;
        BaseSpell LB;
        BaseSpell Pyro;
        float K;
        float X;
        float Y;

        public FFBScLBPyro(CastingState castingState)
        {
            Name = "FFBScLBPyro";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            FFB = (BaseSpell)castingState.GetSpell(SpellId.FrostfireBolt);
            Sc = (BaseSpell)castingState.GetSpell(SpellId.Scorch);
            LB = (BaseSpell)castingState.GetSpell(SpellId.LivingBomb);
            Pyro = (BaseSpell)castingState.GetSpell(SpellId.PyroblastPOM);
            sequence = "Frostfire Bolt";

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.CalculationOptions.GlyphOfImprovedScorch)
            {
                averageScorchesNeeded = 1;
                extraScorches = 0;
            }

            /*if (castingState.CalculationOptions.Mode308)*/
            {
                float gap = (30.0f - (averageScorchesNeeded + extraScorches) * Sc.CastTime) / (30.0f - extraScorches * Sc.CastTime);
                if (castingState.MageTalents.ImprovedScorch == 0)
                {
                    ProvidesScorch = false;
                    gap = 1.0f;
                }
                if (gap == 1.0f)
                {
                    Y = 0.0f;
                    float FFBcrit = FFB.CritRate;
                    float LBcrit = LB.CritRate;
                    float H = castingState.MageTalents.HotStreak / 3.0f;
                    if (castingState.MageTalents.Pyroblast == 0) H = 0.0f;
                    float A2 = (FFBcrit - LBcrit) * (LB.CastTime - FFB.CastTime - 12) - (FFBcrit - LBcrit) * (FFBcrit - LBcrit) * Pyro.CastTime * H;
                    float A1 = (FFBcrit - LBcrit) * (12 - LB.CastTime) + (LB.CastTime - FFB.CastTime - 12) * (1 + LBcrit) - Pyro.CastTime * H * 2 * LBcrit * (FFBcrit - LBcrit);
                    float A0 = (1 + LBcrit) * (12 - LB.CastTime) - Pyro.CastTime * H * LBcrit * LBcrit;
                    if (Math.Abs(A2) < 0.00001)
                    {
                        X = -A0 / A1;
                    }
                    else
                    {
                        X = (float)((-A1 - Math.Sqrt(A1 * A1 - 4 * A2 * A0)) / (2 * A2));
                    }
                    float C = LBcrit + X * (FFBcrit - LBcrit);
                    K = H * C * C / (1 + C);
                }
                else
                {
                    float P = 1.0f + Sc.CastTime / (12.0f * (1.0f - gap));
                    float FFBcrit = FFB.CritRate;
                    float SCcrit = Sc.CritRate;
                    float LBcrit = LB.CritRate;
                    float H = castingState.MageTalents.HotStreak / 3.0f;
                    if (castingState.MageTalents.Pyroblast == 0) H = 0.0f;
                    float T1 = Sc.CastTime - P * FFB.CastTime + (P - 1) * LB.CastTime - 12 * (P - 1);
                    float CY = SCcrit - FFBcrit * P + LBcrit * (P - 1);

                    float A2 = CY * T1 + Pyro.CastTime * H * CY * CY;
                    float A1 = CY * FFB.CastTime + T1 + FFBcrit * T1 + 2 * Pyro.CastTime * H * FFBcrit * CY;
                    float A0 = FFB.CastTime + FFBcrit * FFB.CastTime + Pyro.CastTime * H * FFBcrit * FFBcrit;
                    if (Math.Abs(A2) < 0.00001)
                    {
                        Y = -A0 / A1;
                    }
                    else
                    {
                        Y = (float)((-A1 - Math.Sqrt(A1 * A1 - 4 * A2 * A0)) / (2 * A2));
                    }
                    X = 1 - P * Y;
                    float C = (FFBcrit * X + SCcrit * Y + LBcrit * (1 - X - Y));
                    K = H * C * C / (1 + C);
                }
            }
            /*else
            {
                float gap = (30.0f - (averageScorchesNeeded + extraScorches) * Sc.CastTime) / (30.0f - extraScorches * Sc.CastTime);
                if (castingState.MageTalents.ImprovedScorch == 0)
                {
                    ProvidesScorch = false;
                    gap = 1.0f;
                }
                if (gap == 1.0f)
                {
                    Y = 0.0f;
                    K = FFB.CritRate * FFB.CritRate / (1.0f + FFB.CritRate) * castingState.MageTalents.HotStreak / 3.0f;
                    if (castingState.MageTalents.Pyroblast == 0) K = 0.0f;
                    X = (12.0f - LB.CastTime) / (12.0f + FFB.CastTime - LB.CastTime + Pyro.CastTime * K);
                }
                else
                {
                    float P = 1.0f + Sc.CastTime / (12.0f * (1.0f - gap));
                    float FFBcrit = FFB.CritRate;
                    float SCcrit = Sc.CritRate;
                    float H = castingState.MageTalents.HotStreak / 3.0f;
                    if (castingState.MageTalents.Pyroblast == 0) H = 0.0f;
                    float K1 = Sc.CastTime - P * FFB.CastTime + (P - 1) * LB.CastTime - 12 * (P - 1);
                    float K2 = SCcrit - FFBcrit * P;
                    float K3 = 1.0f - P + K2;

                    float A2 = K3 * K1 + Pyro.CastTime * H * K2 * K2;
                    float A1 = FFB.CastTime * K3 + K1 * (1 + FFBcrit) + Pyro.CastTime * H * 2 * FFBcrit * K2;
                    float A0 = FFB.CastTime * (1 + FFBcrit) + Pyro.CastTime * H * FFBcrit * FFBcrit;
                    if (Math.Abs(A2) < 0.00001)
                    {
                        Y = -A0 / A1;
                    }
                    else
                    {
                        Y = (float)((-A1 - Math.Sqrt(A1 * A1 - 4 * A2 * A0)) / (2 * A2));
                    }
                    X = 1 - P * Y;
                    float C = (FFBcrit * X + SCcrit * Y) / (X + Y);
                    K = H * (X + Y) * C * C / (1 + C);
                }
            }*/

            // X * value(FB) + Y * value(Sc) + (1 - X - Y) * value(LB) + K * value(Pyro)
            CastTime = X * FFB.CastTime + Y * Sc.CastTime + (1 - X - Y) * LB.CastTime + K * Pyro.CastTime;
            CostPerSecond = (X * FFB.CastTime * FFB.CostPerSecond + Y * Sc.CastTime * Sc.CostPerSecond + (1 - X - Y) * LB.CastTime * LB.CostPerSecond + K * Pyro.CastTime * Pyro.CostPerSecond) / CastTime;
            DamagePerSecond = (X * FFB.CastTime * FFB.DamagePerSecond + Y * Sc.CastTime * Sc.DamagePerSecond + (1 - X - Y) * LB.CastTime * LB.DamagePerSecond + K * Pyro.CastTime * Pyro.DamagePerSecond) / CastTime;
            ThreatPerSecond = (X * FFB.CastTime * FFB.ThreatPerSecond + Y * Sc.CastTime * Sc.ThreatPerSecond + (1 - X - Y) * LB.CastTime * LB.ThreatPerSecond + K * Pyro.CastTime * Pyro.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (X * FFB.CastTime * FFB.ManaRegenPerSecond + Y * Sc.CastTime * Sc.ManaRegenPerSecond + (1 - X - Y) * LB.CastTime * LB.ManaRegenPerSecond + K * Pyro.CastTime * Pyro.ManaRegenPerSecond) / CastTime;
            CastProcs = X * FFB.CastProcs + Y * Sc.CastProcs + (1 - X - Y) * LB.CastProcs + K * Pyro.CastProcs;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            FFB.AddSpellContribution(dict, X * FFB.CastTime / CastTime * duration);
            Sc.AddSpellContribution(dict, Y * Sc.CastTime / CastTime * duration);
            LB.AddSpellContribution(dict, (1 - X - Y) * LB.CastTime / CastTime * duration);
            Pyro.AddSpellContribution(dict, K * Pyro.CastTime / CastTime * duration);
        }
    }

    class FFBScPyro : Spell
    {
        BaseSpell FFB;
        BaseSpell Sc;
        BaseSpell Pyro;
        float K;
        float X;

        public FFBScPyro(CastingState castingState)
        {
            Name = "FFBScPyro";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            FFB = (BaseSpell)castingState.GetSpell(SpellId.FrostfireBoltFOF);
            Sc = (BaseSpell)castingState.GetSpell(SpellId.Scorch);
            Pyro = (BaseSpell)castingState.GetSpell(SpellId.PyroblastPOM);
            sequence = "Frostfire Bolt";

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.CalculationOptions.GlyphOfImprovedScorch)
            {
                averageScorchesNeeded = 1;
                extraScorches = 0;
            }

            float gap = (30.0f - (averageScorchesNeeded + extraScorches) * Sc.CastTime) / (30.0f - extraScorches * Sc.CastTime);
            if (castingState.MageTalents.ImprovedScorch == 0)
            {
                ProvidesScorch = false;
                gap = 1.0f;
            }
            float FFBcrit = FFB.CritRate;
            float SCcrit = Sc.CritRate;
            float H = castingState.MageTalents.HotStreak / 3.0f;
            if (castingState.MageTalents.Pyroblast == 0) H = 0.0f;
            float A2 = (FFBcrit - SCcrit) * FFB.CastTime * (1 - gap) + (FFBcrit - SCcrit) * (FFBcrit - SCcrit) * Pyro.CastTime * H * (1 - gap) + gap * (FFBcrit - SCcrit) * Sc.CastTime;
            float A1 = FFB.CastTime * (1 - gap) * (1 + SCcrit) + Pyro.CastTime * H * (2 * (FFBcrit - SCcrit) * SCcrit * (1 - gap)) + Sc.CastTime * gap * (1 + 2 * SCcrit - FFBcrit);
            float A0 = SCcrit * SCcrit * Pyro.CastTime * H * (1 - gap) - gap * Sc.CastTime * (1 + SCcrit);
            if (Math.Abs(A2) < 0.00001)
            {
                X = -A0 / A1;
            }
            else
            {
                X = (float)((-A1 + Math.Sqrt(A1 * A1 - 4 * A2 * A0)) / (2 * A2));
            }
            if (gap == 1.0f) X = 1.0f; //avoid rounding errors
            float C = X * (FFBcrit - SCcrit) + SCcrit;
            K = H * C * C / (1 + C);

            // X * value(FB) + (1 - X) * value(Sc) + value(Pyro) * H * C * C / (1 + C)
            CastTime = X * FFB.CastTime + (1 - X) * Sc.CastTime + K * Pyro.CastTime;
            CostPerSecond = (X * FFB.CastTime * FFB.CostPerSecond + (1 - X) * Sc.CastTime * Sc.CostPerSecond + K * Pyro.CastTime * Pyro.CostPerSecond) / CastTime;
            DamagePerSecond = (X * FFB.CastTime * FFB.DamagePerSecond + (1 - X) * Sc.CastTime * Sc.DamagePerSecond + K * Pyro.CastTime * Pyro.DamagePerSecond) / CastTime;
            ThreatPerSecond = (X * FFB.CastTime * FFB.ThreatPerSecond + (1 - X) * Sc.CastTime * Sc.ThreatPerSecond + K * Pyro.CastTime * Pyro.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (X * FFB.CastTime * FFB.ManaRegenPerSecond + (1 - X) * Sc.CastTime * Sc.ManaRegenPerSecond + K * Pyro.CastTime * Pyro.ManaRegenPerSecond) / CastTime;
            CastProcs = X * FFB.CastProcs + (1 - X) * Sc.CastProcs + K * Pyro.CastProcs;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            FFB.AddSpellContribution(dict, X * FFB.CastTime / CastTime * duration);
            Sc.AddSpellContribution(dict, (1 - X) * Sc.CastTime / CastTime * duration);
            Pyro.AddSpellContribution(dict, K * Pyro.CastTime / CastTime * duration);
        }
    }

    class ABABarSc : Spell
    {
        Spell ABABar;
        BaseSpell Sc;
        float X;

        public ABABarSc(CastingState castingState)
        {
            Name = "ABABarSc";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            ABABar = castingState.GetSpell(SpellId.ABABar0C);
            Sc = (BaseSpell)castingState.GetSpell(SpellId.Scorch);
            sequence = ABABar.Sequence;

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.CalculationOptions.GlyphOfImprovedScorch)
            {
                averageScorchesNeeded = 1;
                extraScorches = 0;
            }

            float gap = (30.0f - (averageScorchesNeeded + extraScorches) * Sc.CastTime) / (30.0f - extraScorches * Sc.CastTime);
            if (castingState.MageTalents.ImprovedScorch == 0)
            {
                ProvidesScorch = false;
                gap = 1.0f;
            }

            // gap = X * ABABar.CastTime / (X * ABABar.CastTime + (1 - X) * Sc.CastTime)
            // gap * (X * ABABar.CastTime + (1 - X) * Sc.CastTime) = X * ABABar.CastTime
            // gap * X * ABABar.CastTime + gap * Sc.CastTime - gap * X * Sc.CastTime = X * ABABar.CastTime
            // X * (gap * ABABar.CastTime - gap * Sc.CastTime - ABABar.CastTime) + gap * Sc.CastTime = 0
            // X = gap * Sc.CastTime / (gap * Sc.CastTime + ABABar.CastTime * (1 - gap))

            X = gap * Sc.CastTime / (gap * Sc.CastTime + ABABar.CastTime * (1 - gap));

            CastTime = X * ABABar.CastTime + (1 - X) * Sc.CastTime;
            CostPerSecond = (X * ABABar.CastTime * ABABar.CostPerSecond + (1 - X) * Sc.CastTime * Sc.CostPerSecond) / CastTime;
            DamagePerSecond = (X * ABABar.CastTime * ABABar.DamagePerSecond + (1 - X) * Sc.CastTime * Sc.DamagePerSecond) / CastTime;
            ThreatPerSecond = (X * ABABar.CastTime * ABABar.ThreatPerSecond + (1 - X) * Sc.CastTime * Sc.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (X * ABABar.CastTime * ABABar.ManaRegenPerSecond + (1 - X) * Sc.CastTime * Sc.ManaRegenPerSecond) / CastTime;
            CastProcs = X * ABABar.CastProcs + (1 - X) * Sc.CastProcs;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            ABABar.AddSpellContribution(dict, X * ABABar.CastTime / CastTime * duration);
            Sc.AddSpellContribution(dict, (1 - X) * Sc.CastTime / CastTime * duration);
        }
    }

    class ABABarCSc : Spell
    {
        Spell ABABarC;
        BaseSpell Sc;
        float X;

        public ABABarCSc(CastingState castingState)
        {
            Name = "ABABarCSc";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            ABABarC = castingState.GetSpell(SpellId.ABABar1C);
            Sc = (BaseSpell)castingState.GetSpell(SpellId.Scorch);
            sequence = ABABarC.Sequence;

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.CalculationOptions.GlyphOfImprovedScorch)
            {
                averageScorchesNeeded = 1;
                extraScorches = 0;
            }

            float gap = (30.0f - (averageScorchesNeeded + extraScorches) * Sc.CastTime) / (30.0f - extraScorches * Sc.CastTime);
            if (castingState.MageTalents.ImprovedScorch == 0)
            {
                ProvidesScorch = false;
                gap = 1.0f;
            }

            // gap = X * ABABar.CastTime / (X * ABABar.CastTime + (1 - X) * Sc.CastTime)
            // gap * (X * ABABar.CastTime + (1 - X) * Sc.CastTime) = X * ABABar.CastTime
            // gap * X * ABABar.CastTime + gap * Sc.CastTime - gap * X * Sc.CastTime = X * ABABar.CastTime
            // X * (gap * ABABar.CastTime - gap * Sc.CastTime - ABABar.CastTime) + gap * Sc.CastTime = 0
            // X = gap * Sc.CastTime / (gap * Sc.CastTime + ABABar.CastTime * (1 - gap))

            X = gap * Sc.CastTime / (gap * Sc.CastTime + ABABarC.CastTime * (1 - gap));

            CastTime = X * ABABarC.CastTime + (1 - X) * Sc.CastTime;
            CostPerSecond = (X * ABABarC.CastTime * ABABarC.CostPerSecond + (1 - X) * Sc.CastTime * Sc.CostPerSecond) / CastTime;
            DamagePerSecond = (X * ABABarC.CastTime * ABABarC.DamagePerSecond + (1 - X) * Sc.CastTime * Sc.DamagePerSecond) / CastTime;
            ThreatPerSecond = (X * ABABarC.CastTime * ABABarC.ThreatPerSecond + (1 - X) * Sc.CastTime * Sc.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (X * ABABarC.CastTime * ABABarC.ManaRegenPerSecond + (1 - X) * Sc.CastTime * Sc.ManaRegenPerSecond) / CastTime;
            CastProcs = X * ABABarC.CastProcs + (1 - X) * Sc.CastProcs;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            ABABarC.AddSpellContribution(dict, X * ABABarC.CastTime / CastTime * duration);
            Sc.AddSpellContribution(dict, (1 - X) * Sc.CastTime / CastTime * duration);
        }
    }

    class ABAMABarSc : Spell
    {
        Spell ABAMABar;
        BaseSpell Sc;
        float X;

        public ABAMABarSc(CastingState castingState)
        {
            Name = "ABAMABarSc";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            ABAMABar = castingState.GetSpell(SpellId.ABAMABar);
            Sc = (BaseSpell)castingState.GetSpell(SpellId.Scorch);
            sequence = ABAMABar.Sequence;

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.CalculationOptions.GlyphOfImprovedScorch)
            {
                averageScorchesNeeded = 1;
                extraScorches = 0;
            }

            float gap = (30.0f - (averageScorchesNeeded + extraScorches) * Sc.CastTime) / (30.0f - extraScorches * Sc.CastTime);
            if (castingState.MageTalents.ImprovedScorch == 0)
            {
                ProvidesScorch = false;
                gap = 1.0f;
            }

            // gap = X * ABABar.CastTime / (X * ABABar.CastTime + (1 - X) * Sc.CastTime)
            // gap * (X * ABABar.CastTime + (1 - X) * Sc.CastTime) = X * ABABar.CastTime
            // gap * X * ABABar.CastTime + gap * Sc.CastTime - gap * X * Sc.CastTime = X * ABABar.CastTime
            // X * (gap * ABABar.CastTime - gap * Sc.CastTime - ABABar.CastTime) + gap * Sc.CastTime = 0
            // X = gap * Sc.CastTime / (gap * Sc.CastTime + ABABar.CastTime * (1 - gap))

            X = gap * Sc.CastTime / (gap * Sc.CastTime + ABAMABar.CastTime * (1 - gap));

            CastTime = X * ABAMABar.CastTime + (1 - X) * Sc.CastTime;
            CostPerSecond = (X * ABAMABar.CastTime * ABAMABar.CostPerSecond + (1 - X) * Sc.CastTime * Sc.CostPerSecond) / CastTime;
            DamagePerSecond = (X * ABAMABar.CastTime * ABAMABar.DamagePerSecond + (1 - X) * Sc.CastTime * Sc.DamagePerSecond) / CastTime;
            ThreatPerSecond = (X * ABAMABar.CastTime * ABAMABar.ThreatPerSecond + (1 - X) * Sc.CastTime * Sc.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (X * ABAMABar.CastTime * ABAMABar.ManaRegenPerSecond + (1 - X) * Sc.CastTime * Sc.ManaRegenPerSecond) / CastTime;
            CastProcs = X * ABAMABar.CastProcs + (1 - X) * Sc.CastProcs;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            ABAMABar.AddSpellContribution(dict, X * ABAMABar.CastTime / CastTime * duration);
            Sc.AddSpellContribution(dict, (1 - X) * Sc.CastTime / CastTime * duration);
        }
    }

    class AB3AMABarSc : Spell
    {
        Spell AB3AMABar;
        BaseSpell Sc;
        float X;

        public AB3AMABarSc(CastingState castingState)
        {
            Name = "AB3AMABarSc";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            AB3AMABar = castingState.GetSpell(SpellId.AB3AMABar);
            Sc = (BaseSpell)castingState.GetSpell(SpellId.Scorch);
            sequence = AB3AMABar.Sequence;

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.CalculationOptions.GlyphOfImprovedScorch)
            {
                averageScorchesNeeded = 1;
                extraScorches = 0;
            }

            float gap = (30.0f - (averageScorchesNeeded + extraScorches) * Sc.CastTime) / (30.0f - extraScorches * Sc.CastTime);
            if (castingState.MageTalents.ImprovedScorch == 0)
            {
                ProvidesScorch = false;
                gap = 1.0f;
            }

            // gap = X * ABABar.CastTime / (X * ABABar.CastTime + (1 - X) * Sc.CastTime)
            // gap * (X * ABABar.CastTime + (1 - X) * Sc.CastTime) = X * ABABar.CastTime
            // gap * X * ABABar.CastTime + gap * Sc.CastTime - gap * X * Sc.CastTime = X * ABABar.CastTime
            // X * (gap * ABABar.CastTime - gap * Sc.CastTime - ABABar.CastTime) + gap * Sc.CastTime = 0
            // X = gap * Sc.CastTime / (gap * Sc.CastTime + ABABar.CastTime * (1 - gap))

            X = gap * Sc.CastTime / (gap * Sc.CastTime + AB3AMABar.CastTime * (1 - gap));

            CastTime = X * AB3AMABar.CastTime + (1 - X) * Sc.CastTime;
            CostPerSecond = (X * AB3AMABar.CastTime * AB3AMABar.CostPerSecond + (1 - X) * Sc.CastTime * Sc.CostPerSecond) / CastTime;
            DamagePerSecond = (X * AB3AMABar.CastTime * AB3AMABar.DamagePerSecond + (1 - X) * Sc.CastTime * Sc.DamagePerSecond) / CastTime;
            ThreatPerSecond = (X * AB3AMABar.CastTime * AB3AMABar.ThreatPerSecond + (1 - X) * Sc.CastTime * Sc.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (X * AB3AMABar.CastTime * AB3AMABar.ManaRegenPerSecond + (1 - X) * Sc.CastTime * Sc.ManaRegenPerSecond) / CastTime;
            CastProcs = X * AB3AMABar.CastProcs + (1 - X) * Sc.CastProcs;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            AB3AMABar.AddSpellContribution(dict, X * AB3AMABar.CastTime / CastTime * duration);
            Sc.AddSpellContribution(dict, (1 - X) * Sc.CastTime / CastTime * duration);
        }
    }

    class AB3ABarCSc : Spell
    {
        Spell AB3ABarC;
        BaseSpell Sc;
        float X;

        public AB3ABarCSc(CastingState castingState)
        {
            Name = "AB3ABarCSc";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            AB3ABarC = castingState.GetSpell(SpellId.AB3ABar3C);
            Sc = (BaseSpell)castingState.GetSpell(SpellId.Scorch);
            sequence = AB3ABarC.Sequence;

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.CalculationOptions.GlyphOfImprovedScorch)
            {
                averageScorchesNeeded = 1;
                extraScorches = 0;
            }

            float gap = (30.0f - (averageScorchesNeeded + extraScorches) * Sc.CastTime) / (30.0f - extraScorches * Sc.CastTime);
            if (castingState.MageTalents.ImprovedScorch == 0)
            {
                ProvidesScorch = false;
                gap = 1.0f;
            }

            // gap = X * ABABar.CastTime / (X * ABABar.CastTime + (1 - X) * Sc.CastTime)
            // gap * (X * ABABar.CastTime + (1 - X) * Sc.CastTime) = X * ABABar.CastTime
            // gap * X * ABABar.CastTime + gap * Sc.CastTime - gap * X * Sc.CastTime = X * ABABar.CastTime
            // X * (gap * ABABar.CastTime - gap * Sc.CastTime - ABABar.CastTime) + gap * Sc.CastTime = 0
            // X = gap * Sc.CastTime / (gap * Sc.CastTime + ABABar.CastTime * (1 - gap))

            X = gap * Sc.CastTime / (gap * Sc.CastTime + AB3ABarC.CastTime * (1 - gap));

            CastTime = X * AB3ABarC.CastTime + (1 - X) * Sc.CastTime;
            CostPerSecond = (X * AB3ABarC.CastTime * AB3ABarC.CostPerSecond + (1 - X) * Sc.CastTime * Sc.CostPerSecond) / CastTime;
            DamagePerSecond = (X * AB3ABarC.CastTime * AB3ABarC.DamagePerSecond + (1 - X) * Sc.CastTime * Sc.DamagePerSecond) / CastTime;
            ThreatPerSecond = (X * AB3ABarC.CastTime * AB3ABarC.ThreatPerSecond + (1 - X) * Sc.CastTime * Sc.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (X * AB3ABarC.CastTime * AB3ABarC.ManaRegenPerSecond + (1 - X) * Sc.CastTime * Sc.ManaRegenPerSecond) / CastTime;
            CastProcs = X * AB3ABarC.CastProcs + (1 - X) * Sc.CastProcs;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            AB3ABarC.AddSpellContribution(dict, X * AB3ABarC.CastTime / CastTime * duration);
            Sc.AddSpellContribution(dict, (1 - X) * Sc.CastTime / CastTime * duration);
        }
    }

    class AB3MBAMABarSc : Spell
    {
        Spell AB3MBAMABar;
        BaseSpell Sc;
        float X;

        public AB3MBAMABarSc(CastingState castingState)
        {
            Name = "AB3MBAMABarSc";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            AB3MBAMABar = castingState.GetSpell(SpellId.ABSpam3C);
            Sc = (BaseSpell)castingState.GetSpell(SpellId.Scorch);
            sequence = AB3MBAMABar.Sequence;

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.CalculationOptions.GlyphOfImprovedScorch)
            {
                averageScorchesNeeded = 1;
                extraScorches = 0;
            }

            float gap = (30.0f - (averageScorchesNeeded + extraScorches) * Sc.CastTime) / (30.0f - extraScorches * Sc.CastTime);
            if (castingState.MageTalents.ImprovedScorch == 0)
            {
                ProvidesScorch = false;
                gap = 1.0f;
            }

            // gap = X * ABABar.CastTime / (X * ABABar.CastTime + (1 - X) * Sc.CastTime)
            // gap * (X * ABABar.CastTime + (1 - X) * Sc.CastTime) = X * ABABar.CastTime
            // gap * X * ABABar.CastTime + gap * Sc.CastTime - gap * X * Sc.CastTime = X * ABABar.CastTime
            // X * (gap * ABABar.CastTime - gap * Sc.CastTime - ABABar.CastTime) + gap * Sc.CastTime = 0
            // X = gap * Sc.CastTime / (gap * Sc.CastTime + ABABar.CastTime * (1 - gap))

            X = gap * Sc.CastTime / (gap * Sc.CastTime + AB3MBAMABar.CastTime * (1 - gap));

            CastTime = X * AB3MBAMABar.CastTime + (1 - X) * Sc.CastTime;
            CostPerSecond = (X * AB3MBAMABar.CastTime * AB3MBAMABar.CostPerSecond + (1 - X) * Sc.CastTime * Sc.CostPerSecond) / CastTime;
            DamagePerSecond = (X * AB3MBAMABar.CastTime * AB3MBAMABar.DamagePerSecond + (1 - X) * Sc.CastTime * Sc.DamagePerSecond) / CastTime;
            ThreatPerSecond = (X * AB3MBAMABar.CastTime * AB3MBAMABar.ThreatPerSecond + (1 - X) * Sc.CastTime * Sc.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (X * AB3MBAMABar.CastTime * AB3MBAMABar.ManaRegenPerSecond + (1 - X) * Sc.CastTime * Sc.ManaRegenPerSecond) / CastTime;
            CastProcs = X * AB3MBAMABar.CastProcs + (1 - X) * Sc.CastProcs;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            AB3MBAMABar.AddSpellContribution(dict, X * AB3MBAMABar.CastTime / CastTime * duration);
            Sc.AddSpellContribution(dict, (1 - X) * Sc.CastTime / CastTime * duration);
        }
    }

    class FBSc : SpellCycle
    {
        public FBSc(CastingState castingState) : base(33)
        {
            Name = "FBSc";

            Spell FB = castingState.GetSpell(SpellId.Fireball);
            BaseSpell Sc = (BaseSpell)castingState.GetSpell(SpellId.Scorch);

            if (castingState.MageTalents.ImprovedScorch == 0)
            {
                // in this case just Fireball, scorch debuff won't be applied
                AddSpell(FB, castingState);
                Calculate(castingState);
            }
            else
            {
                ProvidesScorch = true;
                int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
                int extraScorches = 1;
                if (Sc.HitRate >= 1.0) extraScorches = 0;
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

    class FBFBlast : SpellCycle
    {
        public FBFBlast(CastingState castingState)
            : base(33)
        {
            Name = "FBFBlast";

            Spell FB = castingState.GetSpell(SpellId.Fireball);
            BaseSpell Blast = (BaseSpell)castingState.GetSpell(SpellId.FireBlast);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            if (castingState.MageTalents.ImprovedScorch == 0 || !castingState.CalculationOptions.MaintainScorch)
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

    class GenericArcane : Spell
    {
        public Spell AB0, AB1, AB2, AB3, ABar0, ABar1, ABar2, ABar3, ABar1C, ABar2C, ABar3C, AM0, AM1, AM2, AM3, MBAM0, MBAM1, MBAM2, MBAM3, AM0C, AM1C, AM2C, AM3C, MBAM0C, MBAM1C, MBAM2C, MBAM3C;
        public double S00, S01, S02, S10, S11, S12, S20, S21, S22, S30, S31, S32;
        public float KAB0, KAB1, KAB2, KAB3, KABar0, KABar1, KABar2, KABar3, KABar1C, KABar2C, KABar3C, KAM0, KAM1, KAM2, KAM3, KMBAM0, KMBAM1, KMBAM2, KMBAM3, KAM0C, KAM1C, KAM2C, KAM3C, KMBAM0C, KMBAM1C, KMBAM2C, KMBAM3C;
        public string SpellDistribution;

        private void AppendFormat(StringBuilder sb, string format, double weight)
        {
            if (weight > 0) sb.AppendFormat(format, weight);
        }

        public unsafe GenericArcane(string name, CastingState castingState, double X00, double X01, double X02, double X03, double X04, double X10, double X11, double X12, double X13, double X14, double X15, double X20, double X22, double X23, double X24, double X30, double X32, double X33, double X34, double X40, double X41, double X42, double X43, double X44, double X50, double X51, double X52, double X53, double X54, double X55, double X60, double X61, double X62, double X63, double X64, double X70, double X71, double X72, double X73, double X74, double X75, double X80, double X81, double X82, double X83, double X84)
        {
            Name = name;

            ArraySet arraySet = ArrayPool.RequestArraySet(12, 12);
            LU M = new LU(12, arraySet);

            double[] X = new double[12];

            //AB0,MB0,ABar+: S00
            //AB0       => AB1,MB0,ABar+    X00*(1-MB)
            //AB0       => AB1,MB1,ABar+    X00*MB
            //ABar      => AB0,MB0,ABar-    X01*(1-MB)
            //ABar      => AB0,MB2,ABar-    X01*MB
            //AM        => AB0,MB0,ABar+    X02
            //AMABar    => AB0,MB0,ABar-    X03*(1-MB)
            //AMABar    => AB0,MB2,ABar-    X03*MB
            //AMABar*   => AB0,MB0,ABar-    X04*(1-MB)
            //AMABar*   => AB0,MB2,ABar-    X04*MB

            //AB0,MB0,ABar-: S01
            //AB0       => AB1,MB0,ABar+    X20*(1-MB)
            //AB0       => AB1,MB1,ABar+    X20*MB
            //AM        => AB0,MB0,ABar+    X22
            //AMABar    => AB0,MB0,ABar-    X23*(1-MB)
            //AMABar    => AB0,MB2,ABar-    X23*MB
            //AMABar*   => AB0,MB0,ABar-    X24*(1-MB)
            //AMABar*   => AB0,MB2,ABar-    X24*MB

            //AB0,MB2,ABar-: S02
            //AB0       => AB1,MB2,ABar+    X30
            //MBAM      => AB0,MB0,ABar+    X32
            //MBAMABar  => AB0,MB0,ABar-    X33*(1-MB)
            //MBAMABar  => AB0,MB2,ABar-    X33*MB
            //MBAMABar* => AB0,MB0,ABar-    X34*(1-MB)
            //MBAMABar* => AB0,MB2,ABar-    X34*MB

            //AB1,MB0,ABar+: S10
            //AB1       => AB2,MB0,ABar+    X10*(1-MB)
            //AB1       => AB2,MB1,ABar+    X10*MB
            //ABar1     => AB0,MB0,ABar-    X11*(1-MB)
            //ABar1     => AB0,MB2,ABar-    X11*MB
            //AM1       => AB0,MB0,ABar+    X12
            //AMABar1   => AB0,MB0,ABar-    X13*(1-MB)
            //AMABar1   => AB0,MB2,ABar-    X13*MB
            //AMABar1*  => AB0,MB0,ABar-    (X14 + X15)*(1-MB)
            //AMABar1*  => AB0,MB2,ABar-    (X14 + X15)*MB

            //AB1,MB1,ABar+: S11
            //AB1       => AB2,MB2,ABar+    X10
            //ABar1     => AB0,MB2,ABar-    X11
            //MBAM1     => AB0,MB0,ABar+    X12
            //MBAMABar1 => AB0,MB0,ABar-    (X13 + X15)*(1-MB)
            //MBAMABar1 => AB0,MB2,ABar-    (X13 + X15)*MB
            //MBAMABar1*=> AB0,MB0,ABar-    X14*(1-MB)
            //MBAMABar1*=> AB0,MB2,ABar-    X14*MB

            //AB1,MB2,ABar+: S12
            //AB1       => AB2,MB2,ABar+    X40
            //ABar1     => AB0,MB2,ABar-    X41
            //MBAM1     => AB0,MB0,ABar+    X42
            //MBAMABar1 => AB0,MB0,ABar-    X43*(1-MB)
            //MBAMABar1 => AB0,MB2,ABar-    X43*MB
            //MBAMABar1*=> AB0,MB0,ABar-    X44*(1-MB)
            //MBAMABar1*=> AB0,MB2,ABar-    X44*MB

            //AB2,MB0,ABar+: S20
            //AB2       => AB3,MB0,ABar+    X50*(1-MB)
            //AB2       => AB3,MB1,ABar+    X50*MB
            //ABar2     => AB0,MB0,ABar-    X51*(1-MB)
            //ABar2     => AB0,MB2,ABar-    X51*MB
            //AM2       => AB0,MB0,ABar+    X52
            //AMABar2   => AB0,MB0,ABar-    X53*(1-MB)
            //AMABar2   => AB0,MB2,ABar-    X53*MB
            //AMABar2*  => AB0,MB0,ABar-    (X54 + X55)*(1-MB)
            //AMABar2*  => AB0,MB2,ABar-    (X54 + X55)*MB

            //AB2,MB1,ABar+: S21
            //AB2       => AB3,MB2,ABar+    X50
            //ABar2     => AB0,MB2,ABar-    X51
            //MBAM2     => AB0,MB0,ABar+    X52
            //MBAMABar2 => AB0,MB0,ABar-    (X53 + X55)*(1-MB)
            //MBAMABar2 => AB0,MB2,ABar-    (X53 + X55)*MB
            //MBAMABar2*=> AB0,MB0,ABar-    X54*(1-MB)
            //MBAMABar2*=> AB0,MB2,ABar-    X54*MB

            //AB2,MB2,ABar+: S22
            //AB2       => AB3,MB2,ABar+    X60
            //ABar2     => AB0,MB2,ABar-    X61
            //MBAM2     => AB0,MB0,ABar+    X62
            //MBAMABar2 => AB0,MB0,ABar-    X63*(1-MB)
            //MBAMABar2 => AB0,MB2,ABar-    X63*MB
            //MBAMABar2*=> AB0,MB0,ABar-    X64*(1-MB)
            //MBAMABar2*=> AB0,MB2,ABar-    X64*MB

            //AB3,MB0,ABar+: S30
            //AB3       => AB3,MB0,ABar+    X70*(1-MB)
            //AB3       => AB3,MB1,ABar+    X70*MB
            //ABar3     => AB0,MB0,ABar-    X71*(1-MB)
            //ABar3     => AB0,MB2,ABar-    X71*MB
            //AM3       => AB0,MB0,ABar+    X72
            //AMABar3   => AB0,MB0,ABar-    X73*(1-MB)
            //AMABar3   => AB0,MB2,ABar-    X73*MB
            //AMABar3*  => AB0,MB0,ABar-    (X74 + X75)*(1-MB)
            //AMABar3*  => AB0,MB2,ABar-    (X74 + X75)*MB

            //AB3,MB1,ABar+: S31
            //AB3       => AB3,MB2,ABar+    X70
            //ABar3     => AB0,MB2,ABar-    X71
            //MBAM3     => AB0,MB0,ABar+    X72
            //MBAMABar3 => AB0,MB0,ABar-    (X73 + X75)*(1-MB)
            //MBAMABar3 => AB0,MB2,ABar-    (X73 + X75)*MB
            //MBAMABar3*=> AB0,MB0,ABar-    X74*(1-MB)
            //MBAMABar3*=> AB0,MB2,ABar-    X74*MB

            //AB3,MB2,ABar+: S32
            //AB3       => AB3,MB2,ABar+    X80
            //ABar3     => AB0,MB2,ABar-    X81
            //MBAM3     => AB0,MB0,ABar+    X82
            //MBAMABar3 => AB0,MB0,ABar-    X83*(1-MB)
            //MBAMABar3 => AB0,MB2,ABar-    X83*MB
            //MBAMABar3*=> AB0,MB0,ABar-    X84*(1-MB)
            //MBAMABar3*=> AB0,MB2,ABar-    X84*MB

            double MB = 0.04f * castingState.MageTalents.MissileBarrage;

            fixed (double* U = arraySet.LU_U, x = X)
            fixed (double* sL = arraySet.LUsparseL, column = arraySet.LUcolumn, column2 = arraySet.LUcolumn2)
            fixed (int* P = arraySet.LU_P, Q = arraySet.LU_Q, LJ = arraySet.LU_LJ, sLI = arraySet.LUsparseLI, sLstart = arraySet.LUsparseLstart)
            {
                M.BeginUnsafe(U, sL, P, Q, LJ, sLI, sLstart, column, column2);

                for (int replace = 11; replace >= 11; replace--)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        for (int j = 0; j < 12; j++)
                        {
                            U[i * 12 + j] = 0;
                        }
                    }

                    //U[i * rows + j]
                    U[0 * 12 + 0] = X02 - 1;
                    U[1 * 12 + 0] = X01 * (1 - MB) + (X03 + X04) * (1 - MB);
                    U[2 * 12 + 0] = X01 * MB + (X03 + X04) * MB;
                    U[3 * 12 + 0] = X00 * (1 - MB);
                    U[4 * 12 + 0] = X00 * MB;

                    U[0 * 12 + 1] = X22;
                    U[1 * 12 + 1] = (X23 + X24) * (1 - MB) - 1;
                    U[2 * 12 + 1] = (X23 + X24) * MB;
                    U[3 * 12 + 1] = X20 * (1 - MB);
                    U[4 * 12 + 1] = X20 * MB;

                    U[0 * 12 + 2] = X32;
                    U[1 * 12 + 2] = (X33 + X34) * (1 - MB);
                    U[2 * 12 + 2] = (X33 + X34) * MB - 1;
                    U[5 * 12 + 2] = X30;

                    U[0 * 12 + 3] = X12;
                    U[1 * 12 + 3] = X11 * (1 - MB) + (X13 + X14 + X15) * (1 - MB);
                    U[2 * 12 + 3] = X11 * MB + (X13 + X14 + X15) * MB;
                    U[6 * 12 + 3] = X10 * (1 - MB);
                    U[7 * 12 + 3] = X10 * MB;
                    U[3 * 12 + 3] = -1;

                    U[0 * 12 + 4] = X12;
                    U[1 * 12 + 4] = (X13 + X14 + X15) * (1 - MB);
                    U[2 * 12 + 4] = X11 + (X13 + X14 + X15) * MB;
                    U[8 * 12 + 4] = X10;
                    U[4 * 12 + 4] = -1;

                    U[0 * 12 + 5] = X42;
                    U[1 * 12 + 5] = (X43 + X44) * (1 - MB);
                    U[2 * 12 + 5] = X41 + (X43 + X44) * MB;
                    U[8 * 12 + 5] = X40;
                    U[5 * 12 + 5] = -1;

                    U[0 * 12 + 6] = X52;
                    U[1 * 12 + 6] = X51 * (1 - MB) + (X53 + X54 + X55) * (1 - MB);
                    U[2 * 12 + 6] = X51 * MB + (X53 + X54 + X55) * MB;
                    U[9 * 12 + 6] = X50 * (1 - MB);
                    U[10 * 12 + 6] = X50 * MB;
                    U[6 * 12 + 6] = -1;

                    U[0 * 12 + 7] = X52;
                    U[1 * 12 + 7] = (X53 + X54 + X55) * (1 - MB);
                    U[2 * 12 + 7] = X51 + (X53 + X54 + X55) * MB;
                    U[11 * 12 + 7] = X50;
                    U[7 * 12 + 7] = -1;

                    U[0 * 12 + 8] = X62;
                    U[1 * 12 + 8] = (X63 + X64) * (1 - MB);
                    U[2 * 12 + 8] = X61 + (X63 + X64) * MB;
                    U[11 * 12 + 8] = X60;
                    U[8 * 12 + 8] = -1;

                    U[0 * 12 + 9] = X72;
                    U[1 * 12 + 9] = X71 * (1 - MB) + (X73 + X74 + X75) * (1 - MB);
                    U[2 * 12 + 9] = X71 * MB + (X73 + X74 + X75) * MB;
                    U[9 * 12 + 9] = X70 * (1 - MB) - 1;
                    U[10 * 12 + 9] = X70 * MB;

                    U[0 * 12 + 10] = X72;
                    U[1 * 12 + 10] = (X73 + X74 + X75) * (1 - MB);
                    U[2 * 12 + 10] = X71 + (X73 + X74 + X75) * MB;
                    U[11 * 12 + 10] = X70;
                    U[10 * 12 + 10] = -1;

                    U[0 * 12 + 11] = X82;
                    U[1 * 12 + 11] = (X83 + X84) * (1 - MB);
                    U[2 * 12 + 11] = X81 + (X83 + X84) * MB;
                    U[11 * 12 + 11] = X80 - 1;

                    // the above system is singular, "guess" which one is dependent and replace with sum=1
                    // since not all states are used always we'll get a singular system anyway sometimes, but in those cases the FSolve should still work ok on the nonsingular part
                    for (int i = 0; i < 12; i++) x[i] = 0;

                    if (replace < 12)
                    {
                        U[replace * 12 + 0] = 1;
                        U[replace * 12 + 1] = 1;
                        U[replace * 12 + 2] = 1;
                        U[replace * 12 + 3] = 1;
                        U[replace * 12 + 4] = 1;
                        U[replace * 12 + 5] = 1;
                        U[replace * 12 + 6] = 1;
                        U[replace * 12 + 7] = 1;
                        U[replace * 12 + 8] = 1;
                        U[replace * 12 + 9] = 1;
                        U[replace * 12 + 10] = 1;
                        U[replace * 12 + 11] = 1;

                        x[replace] = 1;
                    }

                    M.Decompose();
                    if (!M.Singular) break;
                }
                M.FSolve(x);

                M.EndUnsafe();

                S00 = x[0];
                S01 = x[1];
                S02 = x[2];
                S10 = x[3];
                S11 = x[4];
                S12 = x[5];
                S20 = x[6];
                S21 = x[7];
                S22 = x[8];
                S30 = x[9];
                S31 = x[10];
                S32 = x[11];
            }

            AB0 = castingState.GetSpell(SpellId.ArcaneBlast00);
            AB1 = castingState.GetSpell(SpellId.ArcaneBlast11);
            AB2 = castingState.GetSpell(SpellId.ArcaneBlast22);
            AB3 = castingState.GetSpell(SpellId.ArcaneBlast33);
            ABar0 = castingState.GetSpell(SpellId.ArcaneBarrage);
            ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);
            ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
            ABar1C = castingState.GetSpell(SpellId.ArcaneBarrage1Combo);
            ABar2C = castingState.GetSpell(SpellId.ArcaneBarrage2Combo);
            ABar3C = castingState.GetSpell(SpellId.ArcaneBarrage3Combo);
            AM0 = castingState.GetSpell(SpellId.ArcaneMissiles);
            AM1 = castingState.GetSpell(SpellId.ArcaneMissiles1);
            AM2 = castingState.GetSpell(SpellId.ArcaneMissiles2);
            AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            AM0C = castingState.GetSpell(SpellId.ArcaneMissiles0Clipped);
            AM1C = castingState.GetSpell(SpellId.ArcaneMissiles1Clipped);
            AM2C = castingState.GetSpell(SpellId.ArcaneMissiles2Clipped);
            AM3C = castingState.GetSpell(SpellId.ArcaneMissiles3Clipped);
            MBAM0 = castingState.GetSpell(SpellId.ArcaneMissilesMB);
            MBAM1 = castingState.GetSpell(SpellId.ArcaneMissilesMB1);
            MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            MBAM0C = castingState.GetSpell(SpellId.ArcaneMissilesMB0Clipped);
            MBAM1C = castingState.GetSpell(SpellId.ArcaneMissilesMB1Clipped);
            MBAM2C = castingState.GetSpell(SpellId.ArcaneMissilesMB2Clipped);
            MBAM3C = castingState.GetSpell(SpellId.ArcaneMissilesMB3Clipped);

            KAB0 = (float)(S00 * X00 + S01 * X20 + S02 * X30);
            KABar0 = (float)(S00 * X01 + S00 * X03 + S01 * X23 + S02 * X33 + S00 * X04 + S01 * X24 + S02 * X34);
            KAM0 = (float)(S00 * X02 + S00 * X03 + S01 * X22 + S01 * X23);
            KMBAM0 = (float)(S02 * X32 + S02 * X33);
            KAB1 = (float)(S10 * X10 + S11 * X10 + S12 * X40);
            KABar1 = (float)(S10 * X11 + S11 * X11 + S12 * X41 + S10 * X14 + S11 * X14 + S12 * X44 + S10 * X15);
            KABar1C = (float)(S10 * X13 + S11 * X13 + S12 * X43 + S11 * X15);
            KAM1 = (float)(S10 * X12 + S10 * X13);
            KMBAM1 = (float)(S11 * X12 + S11 * X13 + S12 * X42 + S12 * X43 + S11 * X15);
            KAB2 = (float)(S20 * X50 + S21 * X50 + S22 * X60);
            KABar2 = (float)(S20 * X51 + S21 * X51 + S22 * X61 + S20 * X54 + S21 * X54 + S22 * X64 + S20 * X55);
            KABar2C = (float)(S20 * X53 + S21 * X53 + S22 * X63 + S21 * X55);
            KAM2 = (float)(S20 * X52 + S20 * X53);
            KMBAM2 = (float)(S21 * X52 + S21 * X53 + S22 * X62 + S22 * X63 + S21 * X55);
            KAB3 = (float)(S30 * X70 + S31 * X70 + S32 * X80);
            KABar3 = (float)(S30 * X71 + S31 * X71 + S32 * X81 + S30 * X74 + S31 * X74 + S32 * X84 + S30 * X75);
            KABar3C = (float)(S30 * X73 + S31 * X73 + S32 * X83 + S31 * X75);
            KAM3 = (float)(S30 * X72 + S30 * X73);
            KMBAM3 = (float)(S31 * X72 + S31 * X73 + S32 * X82 + S32 * X83 + S31 * X75);
            KAM0C = (float)(S00 * X04 + S01 * X24);
            KMBAM0C = (float)(S02 * X34);
            KAM1C = (float)(S10 * X14 + S10 * X15);
            KMBAM1C = (float)(S11 * X14 + S12 * X44);
            KAM2C = (float)(S20 * X54 + S20 * X55);
            KMBAM2C = (float)(S21 * X54 + S22 * X64);
            KAM3C = (float)(S30 * X74 + S30 * X75);
            KMBAM3C = (float)(S31 * X74 + S32 * X84);

            CastTime = KAB0 * AB0.CastTime + KABar0 * ABar0.CastTime + KAM0 * AM0.CastTime + KMBAM0 * MBAM0.CastTime + KAB1 * AB1.CastTime + KABar1 * ABar1.CastTime + KAM1 * AM1.CastTime + KMBAM1 * MBAM1.CastTime + KAB2 * AB2.CastTime + KABar2 * ABar2.CastTime + KAM2 * AM2.CastTime + KMBAM2 * MBAM2.CastTime + KAB3 * AB3.CastTime + KABar3 * ABar3.CastTime + KAM3 * AM3.CastTime + KMBAM3 * MBAM3.CastTime + KABar1C * ABar1C.CastTime + KABar2C * ABar2C.CastTime + KABar3C * ABar3C.CastTime + KAM0C * AM0C.CastTime + KAM1C * AM1C.CastTime + KAM2C * AM2C.CastTime + KAM3C * AM3C.CastTime + KMBAM0C * MBAM0C.CastTime + KMBAM1C * MBAM1C.CastTime + KMBAM2C * MBAM2C.CastTime + KMBAM3C * MBAM3C.CastTime;
            CostPerSecond = (KAB0 * AB0.CastTime * AB0.CostPerSecond + KABar0 * ABar0.CastTime * ABar0.CostPerSecond + KAM0 * AM0.CastTime * AM0.CostPerSecond + KMBAM0 * MBAM0.CastTime * MBAM0.CostPerSecond + KAB1 * AB1.CastTime * AB1.CostPerSecond + KABar1 * ABar1.CastTime * ABar1.CostPerSecond + KAM1 * AM1.CastTime * AM1.CostPerSecond + KMBAM1 * MBAM1.CastTime * MBAM1.CostPerSecond + KAB2 * AB2.CastTime * AB2.CostPerSecond + KABar2 * ABar2.CastTime * ABar2.CostPerSecond + KAM2 * AM2.CastTime * AM2.CostPerSecond + KMBAM2 * MBAM2.CastTime * MBAM2.CostPerSecond + KAB3 * AB3.CastTime * AB3.CostPerSecond + KABar3 * ABar3.CastTime * ABar3.CostPerSecond + KAM3 * AM3.CastTime * AM3.CostPerSecond + KMBAM3 * MBAM3.CastTime * MBAM3.CostPerSecond + KABar1C * ABar1C.CastTime * ABar1C.CostPerSecond + KABar2C * ABar2C.CastTime * ABar2C.CostPerSecond + KABar3C * ABar3C.CastTime * ABar3C.CostPerSecond + KAM0C * AM0C.CastTime * AM0C.CostPerSecond + KAM1C * AM1C.CastTime * AM1C.CostPerSecond + KAM2C * AM2C.CastTime * AM2C.CostPerSecond + KAM3C * AM3C.CastTime * AM3C.CostPerSecond + KMBAM0C * MBAM0C.CastTime * MBAM0C.CostPerSecond + KMBAM1C * MBAM1C.CastTime * MBAM1C.CostPerSecond + KMBAM2C * MBAM2C.CastTime * MBAM2C.CostPerSecond + KMBAM3C * MBAM3C.CastTime * MBAM3C.CostPerSecond) / CastTime;
            DamagePerSecond = (KAB0 * AB0.CastTime * AB0.DamagePerSecond + KABar0 * ABar0.CastTime * ABar0.DamagePerSecond + KAM0 * AM0.CastTime * AM0.DamagePerSecond + KMBAM0 * MBAM0.CastTime * MBAM0.DamagePerSecond + KAB1 * AB1.CastTime * AB1.DamagePerSecond + KABar1 * ABar1.CastTime * ABar1.DamagePerSecond + KAM1 * AM1.CastTime * AM1.DamagePerSecond + KMBAM1 * MBAM1.CastTime * MBAM1.DamagePerSecond + KAB2 * AB2.CastTime * AB2.DamagePerSecond + KABar2 * ABar2.CastTime * ABar2.DamagePerSecond + KAM2 * AM2.CastTime * AM2.DamagePerSecond + KMBAM2 * MBAM2.CastTime * MBAM2.DamagePerSecond + KAB3 * AB3.CastTime * AB3.DamagePerSecond + KABar3 * ABar3.CastTime * ABar3.DamagePerSecond + KAM3 * AM3.CastTime * AM3.DamagePerSecond + KMBAM3 * MBAM3.CastTime * MBAM3.DamagePerSecond + KABar1C * ABar1C.CastTime * ABar1C.DamagePerSecond + KABar2C * ABar2C.CastTime * ABar2C.DamagePerSecond + KABar3C * ABar3C.CastTime * ABar3C.DamagePerSecond + KAM0C * AM0C.CastTime * AM0C.DamagePerSecond + KAM1C * AM1C.CastTime * AM1C.DamagePerSecond + KAM2C * AM2C.CastTime * AM2C.DamagePerSecond + KAM3C * AM3C.CastTime * AM3C.DamagePerSecond + KMBAM0C * MBAM0C.CastTime * MBAM0C.DamagePerSecond + KMBAM1C * MBAM1C.CastTime * MBAM1C.DamagePerSecond + KMBAM2C * MBAM2C.CastTime * MBAM2C.DamagePerSecond + KMBAM3C * MBAM3C.CastTime * MBAM3C.DamagePerSecond) / CastTime;
            ThreatPerSecond = (KAB0 * AB0.CastTime * AB0.ThreatPerSecond + KABar0 * ABar0.CastTime * ABar0.ThreatPerSecond + KAM0 * AM0.CastTime * AM0.ThreatPerSecond + KMBAM0 * MBAM0.CastTime * MBAM0.ThreatPerSecond + KAB1 * AB1.CastTime * AB1.ThreatPerSecond + KABar1 * ABar1.CastTime * ABar1.ThreatPerSecond + KAM1 * AM1.CastTime * AM1.ThreatPerSecond + KMBAM1 * MBAM1.CastTime * MBAM1.ThreatPerSecond + KAB2 * AB2.CastTime * AB2.ThreatPerSecond + KABar2 * ABar2.CastTime * ABar2.ThreatPerSecond + KAM2 * AM2.CastTime * AM2.ThreatPerSecond + KMBAM2 * MBAM2.CastTime * MBAM2.ThreatPerSecond + KAB3 * AB3.CastTime * AB3.ThreatPerSecond + KABar3 * ABar3.CastTime * ABar3.ThreatPerSecond + KAM3 * AM3.CastTime * AM3.ThreatPerSecond + KMBAM3 * MBAM3.CastTime * MBAM3.ThreatPerSecond + KABar1C * ABar1C.CastTime * ABar1C.ThreatPerSecond + KABar2C * ABar2C.CastTime * ABar2C.ThreatPerSecond + KABar3C * ABar3C.CastTime * ABar3C.ThreatPerSecond + KAM0C * AM0C.CastTime * AM0C.ThreatPerSecond + KAM1C * AM1C.CastTime * AM1C.ThreatPerSecond + KAM2C * AM2C.CastTime * AM2C.ThreatPerSecond + KAM3C * AM3C.CastTime * AM3C.ThreatPerSecond + KMBAM0C * MBAM0C.CastTime * MBAM0C.ThreatPerSecond + KMBAM1C * MBAM1C.CastTime * MBAM1C.ThreatPerSecond + KMBAM2C * MBAM2C.CastTime * MBAM2C.ThreatPerSecond + KMBAM3C * MBAM3C.CastTime * MBAM3C.ThreatPerSecond) / CastTime;
            ManaRegenPerSecond = (KAB0 * AB0.CastTime * AB0.ManaRegenPerSecond + KABar0 * ABar0.CastTime * ABar0.ManaRegenPerSecond + KAM0 * AM0.CastTime * AM0.ManaRegenPerSecond + KMBAM0 * MBAM0.CastTime * MBAM0.ManaRegenPerSecond + KAB1 * AB1.CastTime * AB1.ManaRegenPerSecond + KABar1 * ABar1.CastTime * ABar1.ManaRegenPerSecond + KAM1 * AM1.CastTime * AM1.ManaRegenPerSecond + KMBAM1 * MBAM1.CastTime * MBAM1.ManaRegenPerSecond + KAB2 * AB2.CastTime * AB2.ManaRegenPerSecond + KABar2 * ABar2.CastTime * ABar2.ManaRegenPerSecond + KAM2 * AM2.CastTime * AM2.ManaRegenPerSecond + KMBAM2 * MBAM2.CastTime * MBAM2.ManaRegenPerSecond + KAB3 * AB3.CastTime * AB3.ManaRegenPerSecond + KABar3 * ABar3.CastTime * ABar3.ManaRegenPerSecond + KAM3 * AM3.CastTime * AM3.ManaRegenPerSecond + KMBAM3 * MBAM3.CastTime * MBAM3.ManaRegenPerSecond + KABar1C * ABar1C.CastTime * ABar1C.ManaRegenPerSecond + KABar2C * ABar2C.CastTime * ABar2C.ManaRegenPerSecond + KABar3C * ABar3C.CastTime * ABar3C.ManaRegenPerSecond + KAM0C * AM0C.CastTime * AM0C.ManaRegenPerSecond + KAM1C * AM1C.CastTime * AM1C.ManaRegenPerSecond + KAM2C * AM2C.CastTime * AM2C.ManaRegenPerSecond + KAM3C * AM3C.CastTime * AM3C.ManaRegenPerSecond + KMBAM0C * MBAM0C.CastTime * MBAM0C.ManaRegenPerSecond + KMBAM1C * MBAM1C.CastTime * MBAM1C.ManaRegenPerSecond + KMBAM2C * MBAM2C.CastTime * MBAM2C.ManaRegenPerSecond + KMBAM3C * MBAM3C.CastTime * MBAM3C.ManaRegenPerSecond) / CastTime;

            StringBuilder sb = new StringBuilder();
            AppendFormat(sb, "AB0:\t{0:F}%\r\n", 100.0 * (S00 * X00 + S01 * X20 + S02 * X30));
            AppendFormat(sb, "ABar0:\t{0:F}%\r\n", 100.0 * (S00 * X01));
            AppendFormat(sb, "AM0:\t{0:F}%\r\n", 100.0 * (S00 * X02 + S01 * X22));
            AppendFormat(sb, "AMABar0:\t{0:F}%\r\n", 100.0 * (S00 * X03 + S01 * X23));
            AppendFormat(sb, "AMABarClip0:\t{0:F}%\r\n", 100.0 * (S00 * X04 + S01 * X24));
            AppendFormat(sb, "MBAM0:\t{0:F}%\r\n", 100.0 * (S02 * X32));
            AppendFormat(sb, "MBAMABar0:\t{0:F}%\r\n", 100.0 * (S02 * X33));
            AppendFormat(sb, "MBAMABarClip0:\t{0:F}%\r\n", 100.0 * (S02 * X34));

            AppendFormat(sb, "AB1:\t{0:F}%\r\n", 100.0 * (S10 * X10 + S11 * X10 + S12 * X40));
            AppendFormat(sb, "ABar1:\t{0:F}%\r\n", 100.0 * (S10 * X11 + S11 * X11 + S12 * X41));
            AppendFormat(sb, "AM1:\t{0:F}%\r\n", 100.0 * (S10 * X12));
            AppendFormat(sb, "AMABar1:\t{0:F}%\r\n", 100.0 * (S10 * X13));
            AppendFormat(sb, "AMABarClip1:\t{0:F}%\r\n", 100.0 * (S10 * X14 + S10 * X15));
            AppendFormat(sb, "MBAM1:\t{0:F}%\r\n", 100.0 * (S11 * X12 + S12 * X42));
            AppendFormat(sb, "MBAMABar1:\t{0:F}%\r\n", 100.0 * (S11 * X13 + S11 * X15 + S12 * X43));
            AppendFormat(sb, "MBAMABarClip1:\t{0:F}%\r\n", 100.0 * (S11 * X14 + S12 * X44));

            AppendFormat(sb, "AB2:\t{0:F}%\r\n", 100.0 * (S20 * X50 + S21 * X50 + S22 * X60));
            AppendFormat(sb, "ABar2:\t{0:F}%\r\n", 100.0 * (S20 * X51 + S21 * X51 + S22 * X61));
            AppendFormat(sb, "AM2:\t{0:F}%\r\n", 100.0 * (S20 * X52));
            AppendFormat(sb, "AMABar2:\t{0:F}%\r\n", 100.0 * (S20 * X53));
            AppendFormat(sb, "AMABarClip2:\t{0:F}%\r\n", 100.0 * (S20 * X54 + S20 * X55));
            AppendFormat(sb, "MBAM2:\t{0:F}%\r\n", 100.0 * (S21 * X52 + S22 * X62));
            AppendFormat(sb, "MBAMABar2:\t{0:F}%\r\n", 100.0 * (S21 * X53 + S21 * X55 + S22 * X63));
            AppendFormat(sb, "MBAMABarClip2:\t{0:F}%\r\n", 100.0 * (S21 * X54 + S22 * X64));

            AppendFormat(sb, "AB3:\t{0:F}%\r\n", 100.0 * (S30 * X70 + S31 * X70 + S32 * X80));
            AppendFormat(sb, "ABar3:\t{0:F}%\r\n", 100.0 * (S30 * X71 + S31 * X71 + S32 * X81));
            AppendFormat(sb, "AM3:\t{0:F}%\r\n", 100.0 * (S30 * X72));
            AppendFormat(sb, "AMABar3:\t{0:F}%\r\n", 100.0 * (S30 * X73));
            AppendFormat(sb, "AMABarClip3:\t{0:F}%\r\n", 100.0 * (S30 * X74 + S30 * X75));
            AppendFormat(sb, "MBAM3:\t{0:F}%\r\n", 100.0 * (S31 * X72 + S32 * X82));
            AppendFormat(sb, "MBAMABar3:\t{0:F}%\r\n", 100.0 * (S31 * X73 + S31 * X75 + S32 * X83));
            AppendFormat(sb, "MBAMABarClip3:\t{0:F}%\r\n", 100.0 * (S31 * X74 + S32 * X84));

            SpellDistribution = sb.ToString();
            ArrayPool.ReleaseArraySet(arraySet);
        }

        public override string Sequence
        {
            get
            {
                return "GenericArcane";
            }
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            AB0.AddSpellContribution(dict, KAB0 * AB0.CastTime / CastTime * duration);
            ABar0.AddSpellContribution(dict, KABar0 * ABar0.CastTime / CastTime * duration);
            AM0.AddSpellContribution(dict, KAM0 * AM0.CastTime / CastTime * duration);
            MBAM0.AddSpellContribution(dict, KMBAM0 * MBAM0.CastTime / CastTime * duration);
            AB1.AddSpellContribution(dict, KAB1 * AB1.CastTime / CastTime * duration);
            ABar1.AddSpellContribution(dict, KABar1 * ABar1.CastTime / CastTime * duration);
            AM1.AddSpellContribution(dict, KAM1 * AM1.CastTime / CastTime * duration);
            MBAM1.AddSpellContribution(dict, KMBAM1 * MBAM1.CastTime / CastTime * duration);
            AB2.AddSpellContribution(dict, KAB2 * AB2.CastTime / CastTime * duration);
            ABar2.AddSpellContribution(dict, KABar2 * ABar2.CastTime / CastTime * duration);
            AM2.AddSpellContribution(dict, KAM2 * AM2.CastTime / CastTime * duration);
            MBAM2.AddSpellContribution(dict, KMBAM2 * MBAM2.CastTime / CastTime * duration);
            AB3.AddSpellContribution(dict, KAB3 * AB3.CastTime / CastTime * duration);
            ABar3.AddSpellContribution(dict, KABar3 * ABar3.CastTime / CastTime * duration);
            AM3.AddSpellContribution(dict, KAM3 * AM3.CastTime / CastTime * duration);
            MBAM3.AddSpellContribution(dict, KMBAM3 * MBAM3.CastTime / CastTime * duration);
            ABar1C.AddSpellContribution(dict, KABar1C * ABar1C.CastTime / CastTime * duration);
            ABar2C.AddSpellContribution(dict, KABar2C * ABar2C.CastTime / CastTime * duration);
            ABar3C.AddSpellContribution(dict, KABar3C * ABar3C.CastTime / CastTime * duration);
            AM0C.AddSpellContribution(dict, KAM0C * AM0C.CastTime / CastTime * duration);
            AM1C.AddSpellContribution(dict, KAM1C * AM1C.CastTime / CastTime * duration);
            AM2C.AddSpellContribution(dict, KAM2C * AM2C.CastTime / CastTime * duration);
            AM3C.AddSpellContribution(dict, KAM3C * AM3C.CastTime / CastTime * duration);
            MBAM0C.AddSpellContribution(dict, KMBAM0C * MBAM0C.CastTime / CastTime * duration);
            MBAM1C.AddSpellContribution(dict, KMBAM1C * MBAM1C.CastTime / CastTime * duration);
            MBAM2C.AddSpellContribution(dict, KMBAM2C * MBAM2C.CastTime / CastTime * duration);
            MBAM3C.AddSpellContribution(dict, KMBAM3C * MBAM3C.CastTime / CastTime * duration);
        }
    }
    #endregion
}
