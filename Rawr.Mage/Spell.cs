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
        [Description("Arcane Barrage (0)")]
        ArcaneBarrage,
        [Description("Arcane Barrage (1)")]
        ArcaneBarrage1,
        [Description("Arcane Barrage (2)")]
        ArcaneBarrage2,
        [Description("Arcane Barrage (3)")]
        ArcaneBarrage3,
        [Description("Arcane Missiles (0)")]
        ArcaneMissiles,
        [Description("Arcane Missiles (1)")]
        ArcaneMissiles1,
        [Description("Arcane Missiles (2)")]
        ArcaneMissiles2,
        [Description("Arcane Missiles (3)")]
        ArcaneMissiles3,
        [Description("MBAM (0)")]
        ArcaneMissilesMB,
        [Description("MBAM (1)")]
        ArcaneMissilesMB1,
        [Description("MBAM (2)")]
        ArcaneMissilesMB2,
        [Description("MBAM (3)")]
        ArcaneMissilesMB3,
        ArcaneMissilesNoProc,
        [Description("Frostbolt")]
        FrostboltFOF,
        Frostbolt,
        [Description("POM+Frostbolt")]
        FrostboltPOM,
        FrostboltNoCC,
        [Description("Fireball")]
        Fireball,
        [Description("POM+Fireball")]
        FireballPOM,
        FireballBF,
        [Description("Frostfire Bolt")]
        FrostfireBoltFOF,
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
        [Description("Living Bomb")]
        LivingBomb,
        ArcaneBlast3NoCC,
        ArcaneBlastRaw,
        [Description("Arcane Blast (0)")]
        ArcaneBlast0,
        ArcaneBlast0NoCC,
        ArcaneBlast0POM,
        [Description("Arcane Blast (1)")]
        ArcaneBlast1,
        ArcaneBlast1NoCC,
        [Description("Arcane Blast (2)")]
        ArcaneBlast2,
        [Description("Arcane Blast (3)")]
        ArcaneBlast3,
        ArcaneBlast2NoCC,
        ArcaneBlast0Hit,
        ArcaneBlast1Hit,
        ArcaneBlast2Hit,
        ArcaneBlast3Hit,
        ArcaneBlast0Miss,
        ArcaneBlast1Miss,
        ArcaneBlast2Miss,
        ArcaneBlast3Miss,
        Slow,
        IceLance,
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
        ConeOfCold
    }

    public enum CycleId
    {
        None,
        ArcaneMissiles,
        Scorch,
        FrostboltFOF,
        Fireball,
        FrostfireBoltFOF,
        ArcaneBlastSpam,
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
        AB3AMABar,
        AB3AMABar2C,
        AB3ABar,
        AB3ABar3C,
        AB3ABarX,
        AB3ABarY,
        FBABar,
        FrBABar,
        FFBABar,
        //ABAMP,
        //AB3AMSc,
        //ABAM3Sc,
        //ABAM3Sc2,
        //ABAM3FrB,
        //ABAM3FrB2,
        //ABFrB,
        //AB3FrB,
        //ABFrB3FrB,
        //ABFrB3FrB2,
        //ABFrB3FrBSc,
        //ABFB3FBSc,
        //AB3Sc,
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
        ABABarSlow,
        FBABarSlow,
        FrBABarSlow,
        /*ABAM3ScCCAM,
        ABAM3Sc2CCAM,
        ABAM3FrBCCAM,
        ABAM3FrBCCAMFail,
        ABAM3FrBScCCAM,
        ABAMCCAM,
        ABAM3CCAM,*/
        CustomSpellMix,
        ArcaneExplosion,
        FlamestrikeSpammed,
        FlamestrikeSingle,
        Blizzard,
        BlastWave,
        DragonsBreath,
        ConeOfCold
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

    public abstract class Cycle
    {
        public string Name;
        public CycleId CycleId;

        public CastingState CastingState;

        protected Cycle(CastingState castingState)
        {
            CastingState = castingState;
        }

        private bool calculated;

        internal float damagePerSecond;
        internal float effectDamagePerSecond;
        public float DamagePerSecond
        {
            get
            {
                Calculate();
                return damagePerSecond + effectDamagePerSecond;
            }
        }

        internal float threatPerSecond;
        internal float effectThreatPerSecond;
        public float ThreatPerSecond
        {
            get
            {
                Calculate();
                return threatPerSecond + effectThreatPerSecond;
            }
        }

        internal float costPerSecond;
        public float CostPerSecond
        {
            get
            {
                Calculate();
                return costPerSecond;
            }
        }

        private float manaRegenPerSecond;
        public float ManaRegenPerSecond
        {
            get
            {
                Calculate();
                return manaRegenPerSecond;
            }
        }

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
        public Spell AoeSpell;

        protected string sequence = null;
        public virtual string Sequence
        {
            get
            {
                return sequence;
            }
        }

        public float HitProcs;
        public float Ticks;
        public float CastProcs;
        public float CritProcs;
        public float CastTime;
        public float TargetProcs;
        public float OO5SR = 0;

        public abstract void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration);
        public abstract void AddManaUsageContribution(Dictionary<string, float> dict, float duration);

        private void Calculate()
        {
            if (!calculated)
            {
                CalculateManaRegen();
                CalculateEffectDamage();
                calculated = true;
            }
        }

        private Spell waterbolt;

        private void CalculateEffectDamage()
        {
            Stats baseStats = CastingState.BaseStats;
            if (CastingState.WaterElemental)
            {
                float spellPower = CastingState.FrostSpellPower;
                foreach (SpecialEffect effect in CastingState.Calculations.SpellPowerEffects)
                {
                    switch (effect.Trigger)
                    {
                        case Trigger.DamageSpellCrit:
                        case Trigger.SpellCrit:
                            spellPower += effect.Stats.SpellPower * effect.GetAverageUptime(CastTime / Ticks, CritProcs / Ticks, 3, CastingState.CalculationOptions.FightDuration);
                            break;
                        case Trigger.DamageSpellHit:
                        case Trigger.SpellHit:
                            spellPower += effect.Stats.SpellPower * effect.GetAverageUptime(CastTime / Ticks, HitProcs / Ticks, 3, CastingState.CalculationOptions.FightDuration);
                            break;
                        case Trigger.SpellMiss:
                            spellPower += effect.Stats.SpellPower * effect.GetAverageUptime(CastTime / Ticks, 1 - HitProcs / Ticks, 3, CastingState.CalculationOptions.FightDuration);
                            break;
                        case Trigger.DamageSpellCast:
                        case Trigger.SpellCast:
                            spellPower += effect.Stats.SpellPower * effect.GetAverageUptime(CastTime / Ticks, CastProcs / Ticks, 3, CastingState.CalculationOptions.FightDuration);
                            break;
                    }
                }
                //if (baseStats.SpellPowerFor15SecOnCast_50_45 > 0) spellPower += baseStats.SpellPowerFor15SecOnCast_50_45 * 15f / (45f + CastTime / CastProcs / 0.5f);
                //if (baseStats.SpellDamageFor10SecOnHit_5 > 0) spellPower += baseStats.SpellDamageFor10SecOnHit_5 * Spell.ProcBuffUp(1 - (float)Math.Pow(0.95, TargetProcs), 10, CastTime);
                //if (baseStats.SpellPowerFor6SecOnCrit > 0) spellPower += baseStats.SpellPowerFor6SecOnCrit * Spell.ProcBuffUp(CritProcs / Ticks, 6, CastTime / Ticks);
                //if (baseStats.SpellPowerFor10SecOnHit_10_45 > 0) spellPower += baseStats.SpellPowerFor10SecOnHit_10_45 * 10f / (45f + CastTime / HitProcs / 0.1f);
                //if (baseStats.SpellPowerFor10SecOnCast_15_45 > 0) spellPower += baseStats.SpellPowerFor10SecOnCast_15_45 * 10f / (45f + CastTime / CastProcs / 0.15f);
                //if (baseStats.SpellPowerFor10SecOnCast_10_45 > 0) spellPower += baseStats.SpellPowerFor10SecOnCast_10_45 * 10f / (45f + CastTime / CastProcs / 0.1f);
                //if (baseStats.SpellPowerFor10SecOnResist > 0) spellPower += baseStats.SpellPowerFor10SecOnResist * Spell.ProcBuffUp(1 - HitProcs / Ticks, 10, CastTime / Ticks);
                //if (baseStats.SpellPowerFor15SecOnCrit_20_45 > 0) spellPower += baseStats.SpellPowerFor15SecOnCrit_20_45 * 15f / (45f + CastTime / CritProcs / 0.2f);
                //if (baseStats.SpellPowerFor10SecOnCrit_20_45 > 0) spellPower += baseStats.SpellPowerFor10SecOnCrit_20_45 * 10f / (45f + CastTime / CritProcs / 0.2f);
                if (baseStats.ShatteredSunAcumenProc > 0 && CastingState.CalculationOptions.Aldor) spellPower += 120 * 10f / (45f + CastTime / HitProcs / 0.1f);
                waterbolt = CastingState.Calculations.WaterboltTemplate.GetSpell(CastingState, spellPower);
                effectDamagePerSecond += waterbolt.DamagePerSecond;
            }
            if (baseStats.LightningCapacitorProc > 0)
            {
                //discrete model
                int hitsInsideCooldown = (int)(2.5f / (CastTime / Ticks));
                float avgCritsPerHit = CritProcs / Ticks * TargetProcs / HitProcs;
                float avgHitsToDischarge = 3f / avgCritsPerHit;
                if (avgHitsToDischarge < 1) avgHitsToDischarge = 1;
                float boltDps = CastingState.LightningBoltAverageDamage / ((CastTime / Ticks) * (hitsInsideCooldown + avgHitsToDischarge));
                effectDamagePerSecond += boltDps;
                effectThreatPerSecond += boltDps * CastingState.NatureThreatMultiplier;
                //continuous model
                //DamagePerSecond += LightningBolt.AverageDamage / (2.5f + 3f * CastTime / (CritRate * TargetProcs));
            }
            if (baseStats.ThunderCapacitorProc > 0)
            {
                //discrete model
                int hitsInsideCooldown = (int)(2.5f / (CastTime / Ticks));
                float avgCritsPerHit = CritProcs / Ticks * TargetProcs / HitProcs;
                float avgHitsToDischarge = 4f / avgCritsPerHit;
                if (avgHitsToDischarge < 1) avgHitsToDischarge = 1;
                float boltDps = CastingState.ThunderBoltAverageDamage / ((CastTime / Ticks) * (hitsInsideCooldown + avgHitsToDischarge));
                effectDamagePerSecond += boltDps;
                effectThreatPerSecond += boltDps * CastingState.NatureThreatMultiplier;
                //continuous model
                //DamagePerSecond += LightningBolt.AverageDamage / (2.5f + 4f * CastTime / (CritRate * TargetProcs));
            }
            if (baseStats.ShatteredSunAcumenProc > 0 && !CastingState.CalculationOptions.Aldor)
            {
                float boltDps = CastingState.ArcaneBoltAverageDamage / (45f + CastTime / HitProcs / 0.1f);
                effectDamagePerSecond += boltDps;
                effectThreatPerSecond += boltDps * CastingState.ArcaneThreatMultiplier;
            }
            if (baseStats.PendulumOfTelluricCurrentsProc > 0)
            {
                float boltDps = CastingState.PendulumOfTelluricCurrentsAverageDamage / (45f + CastTime / HitProcs / 0.15f);
                effectDamagePerSecond += boltDps;
                effectThreatPerSecond += boltDps * CastingState.ShadowThreatMultiplier;
            }
        }

        private void CalculateManaRegen()
        {
            Stats baseStats = CastingState.BaseStats;
            manaRegenPerSecond = CastingState.ManaRegen5SR + OO5SR * (CastingState.ManaRegen - CastingState.ManaRegen5SR) + CastingState.BaseStats.ManaRestoreFromBaseManaPerHit * 3268 / CastTime * HitProcs;
            foreach (SpecialEffect effect in CastingState.Calculations.ManaRestoreEffects)
            {
                switch (effect.Trigger)
                {
                    case Trigger.Use:
                        manaRegenPerSecond += effect.Stats.ManaRestore / effect.Cooldown * effect.Chance;
                        break;
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                        manaRegenPerSecond += effect.Stats.ManaRestore / (effect.Cooldown + CastTime / CastProcs / effect.Chance);
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        manaRegenPerSecond += effect.Stats.ManaRestore / (effect.Cooldown + CastTime / CritProcs / effect.Chance);
                        break;
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        manaRegenPerSecond += effect.Stats.ManaRestore / (effect.Cooldown + CastTime / HitProcs / effect.Chance);
                        break;
                }
            }
            foreach (SpecialEffect effect in CastingState.Calculations.Mp5Effects)
            {
                switch (effect.Trigger)
                {
                    case Trigger.Use:
                        manaRegenPerSecond += effect.Stats.Mp5 / 5f * effect.GetAverageUptime(0f, 1f, 3, CastingState.CalculationOptions.FightDuration);
                        break;
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                        manaRegenPerSecond += effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / CastProcs, 1f, 3, CastingState.CalculationOptions.FightDuration);
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        manaRegenPerSecond += effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / Ticks, CritProcs / Ticks, 3, CastingState.CalculationOptions.FightDuration);
                        break;
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        manaRegenPerSecond += effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / Ticks, HitProcs / Ticks, 3, CastingState.CalculationOptions.FightDuration);
                        break;
                }
            }
            if (CastingState.WaterElemental)
            {
                manaRegenPerSecond += 0.002f * CastingState.BaseStats.Mana / 5.0f * CastingState.MageTalents.ImprovedWaterElemental;
            }
            threatPerSecond += (baseStats.ManaRestoreFromBaseManaPerHit * 3268 / CastTime * HitProcs) * 0.5f * (1 + baseStats.ThreatIncreaseMultiplier) * (1 - baseStats.ThreatReductionMultiplier);
        }

        public virtual void AddManaSourcesContribution(Dictionary<string, float> dict, float duration)
        {
            dict["Intellect/Spirit"] += duration * (CastingState.SpiritRegen * CastingState.BaseStats.SpellCombatManaRegeneration + OO5SR * (CastingState.SpiritRegen - CastingState.SpiritRegen * CastingState.BaseStats.SpellCombatManaRegeneration));
            dict["MP5"] += duration * CastingState.BaseStats.Mp5 / 5f;
            dict["Innervate"] += duration * ((CastingState.SpiritRegen * 4 * 20 * CastingState.CalculationOptions.Innervate / CastingState.CalculationOptions.FightDuration) * (OO5SR) + (CastingState.SpiritRegen * (5 - CastingState.BaseStats.SpellCombatManaRegeneration) * 20 * CastingState.CalculationOptions.Innervate / CastingState.CalculationOptions.FightDuration) * (1 - OO5SR));
            dict["Mana Tide"] += duration * CastingState.CalculationOptions.ManaTide * 0.24f * CastingState.BaseStats.Mana / CastingState.CalculationOptions.FightDuration;
            dict["Replenishment"] += duration * CastingState.BaseStats.ManaRestoreFromMaxManaPerSecond * CastingState.BaseStats.Mana;
            dict["Judgement of Wisdom"] += duration * CastingState.BaseStats.ManaRestoreFromBaseManaPerHit * 3268 / CastTime * HitProcs;
            foreach (SpecialEffect effect in CastingState.Calculations.ManaRestoreEffects)
            {
                switch (effect.Trigger)
                {
                    case Trigger.Use:
                        dict["Other"] += duration * effect.Stats.ManaRestore / effect.Cooldown * effect.Chance;
                        break;
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                        dict["Other"] += duration * effect.Stats.ManaRestore / (effect.Cooldown + CastTime / CastProcs / effect.Chance);
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        dict["Other"] += duration * effect.Stats.ManaRestore / (effect.Cooldown + CastTime / CritProcs / effect.Chance);
                        break;
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        dict["Other"] += duration * effect.Stats.ManaRestore / (effect.Cooldown + CastTime / HitProcs / effect.Chance);
                        break;
                }
            }
            foreach (SpecialEffect effect in CastingState.Calculations.Mp5Effects)
            {
                switch (effect.Trigger)
                {
                    case Trigger.Use:
                        dict["Other"] += duration * effect.Stats.Mp5 / 5f * effect.GetAverageUptime(0f, 1f, 3, CastingState.CalculationOptions.FightDuration);
                        break;
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                        dict["Other"] += duration * effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / CastProcs, 1f, 3, CastingState.CalculationOptions.FightDuration);
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        dict["Other"] += duration * effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / Ticks, CritProcs / Ticks, 3, CastingState.CalculationOptions.FightDuration);
                        break;
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        dict["Other"] += duration * effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / Ticks, HitProcs / Ticks, 3, CastingState.CalculationOptions.FightDuration);
                        break;
                }
            }
            if (CastingState.WaterElemental)
            {
                dict["Water Elemental"] += duration * 0.002f * CastingState.BaseStats.Mana / 5.0f * CastingState.MageTalents.ImprovedWaterElemental;
            }
        }

        public void AddEffectContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            SpellContribution contrib;
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
            if (CastingState.BaseStats.LightningCapacitorProc > 0)
            {
                if (!dict.TryGetValue("Lightning Bolt", out contrib))
                {
                    contrib = new SpellContribution() { Name = "Lightning Bolt" };
                    dict["Lightning Bolt"] = contrib;
                }
                //discrete model
                int hitsInsideCooldown = (int)(2.5f / (CastTime / Ticks));
                float avgCritsPerHit = CritProcs / Ticks * TargetProcs / HitProcs;
                float avgHitsToDischarge = 3f / avgCritsPerHit;
                if (avgHitsToDischarge < 1) avgHitsToDischarge = 1;
                float boltDps = CastingState.LightningBoltAverageDamage / ((CastTime / Ticks) * (hitsInsideCooldown + avgHitsToDischarge));
                contrib.Hits += duration / ((CastTime / Ticks) * (hitsInsideCooldown + avgHitsToDischarge));
                contrib.Damage += boltDps * duration;
            }
            if (CastingState.BaseStats.ThunderCapacitorProc > 0)
            {
                if (!dict.TryGetValue("Thunder Bolt", out contrib))
                {
                    contrib = new SpellContribution() { Name = "Thunder Bolt" };
                    dict["Thunder Bolt"] = contrib;
                }
                //discrete model
                int hitsInsideCooldown = (int)(2.5f / (CastTime / Ticks));
                float avgCritsPerHit = CritProcs / Ticks * TargetProcs / HitProcs;
                float avgHitsToDischarge = 4f / avgCritsPerHit;
                if (avgHitsToDischarge < 1) avgHitsToDischarge = 1;
                float boltDps = CastingState.ThunderBoltAverageDamage / ((CastTime / Ticks) * (hitsInsideCooldown + avgHitsToDischarge));
                contrib.Hits += duration / ((CastTime / Ticks) * (hitsInsideCooldown + avgHitsToDischarge));
                contrib.Damage += boltDps * duration;
            }
            if (CastingState.BaseStats.ShatteredSunAcumenProc > 0 && !CastingState.CalculationOptions.Aldor)
            {
                if (!dict.TryGetValue("Arcane Bolt", out contrib))
                {
                    contrib = new SpellContribution() { Name = "Arcane Bolt" };
                    dict["Arcane Bolt"] = contrib;
                }
                float boltDps = CastingState.ArcaneBoltAverageDamage / (45f + CastTime / HitProcs / 0.1f);
                contrib.Hits += duration / (45f + CastTime / HitProcs / 0.1f);
                contrib.Damage += boltDps * duration;
            }
            if (CastingState.BaseStats.PendulumOfTelluricCurrentsProc > 0)
            {
                if (!dict.TryGetValue("Pendulum of Telluric Currents", out contrib))
                {
                    contrib = new SpellContribution() { Name = "Pendulum of Telluric Currents" };
                    dict["Pendulum of Telluric Currents"] = contrib;
                }
                float boltDps = CastingState.PendulumOfTelluricCurrentsAverageDamage / (45f + CastTime / HitProcs / 0.15f);
                contrib.Hits += duration / (45f + CastTime / HitProcs / 0.15f);
                contrib.Damage += boltDps * duration;
            }
        }
    }

    public class SpellCustomMix : DynamicCycle
    {
        public SpellCustomMix(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            sequence = "Custom Mix";
            Name = "Custom Mix";
            if (castingState.CalculationOptions.CustomSpellMix == null) return;
            for (int i = 0; i < castingState.CalculationOptions.CustomSpellMix.Count; i++)
            {
                SpellWeight spellWeight = castingState.CalculationOptions.CustomSpellMix[i];
                AddSpell(needsDisplayCalculations, castingState.GetSpell(spellWeight.Spell), spellWeight.Weight);
            }
            Calculate();
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
                CritProcs = spell.CritProcs;
                TargetProcs = spell.TargetProcs;
                damagePerSecond = spell.DamagePerSecond;
                threatPerSecond = spell.ThreatPerSecond;
                costPerSecond = spell.CostPerSecond;
                AffectedByFlameCap = spell.AffectedByFlameCap;
                OO5SR = spell.OO5SR;
                AreaEffect = spell.AreaEffect;
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

        public void CalculateDerivedStats(CastingState castingState, bool outOfFiveSecondRule, bool pom, bool spammedDot, bool round, bool forceHit, bool forceMiss)
        {
            MageTalents mageTalents = castingState.MageTalents;
            Stats baseStats = castingState.BaseStats;
            CalculationOptionsMage calculationOptions = castingState.CalculationOptions;

            if (CritRate < 0.0f) CritRate = 0.0f;
            if (CritRate > 1.0f) CritRate = 1.0f;

            HitProcs = Ticks * HitRate;
            CritProcs = HitProcs * CritRate;
            TargetProcs = HitProcs;
            if (AreaEffect) TargetProcs *= castingState.CalculationOptions.AoeTargets;

            if (Instant) InterruptProtection = 1;
            if (castingState.IcyVeins) InterruptProtection = 1;

            float channelReduction;
            CastTime = template.CalculateCastTime(castingState.Calculations.HasteRatingEffects, calculationOptions, castingState.CastingSpeed, castingState.SpellHasteRating, InterruptProtection, CritRate, pom, BaseCastTime, out channelReduction);

            float spellPower = RawSpellDamage;
            foreach (SpecialEffect effect in castingState.Calculations.SpellPowerEffects)
            {
                switch (effect.Trigger)
                {
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        spellPower += effect.Stats.SpellPower * effect.GetAverageUptime(CastTime / Ticks, CritProcs / Ticks, BaseCastTime, calculationOptions.FightDuration);
                        break;
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        spellPower += effect.Stats.SpellPower * effect.GetAverageUptime(CastTime / Ticks, HitProcs / Ticks, BaseCastTime, calculationOptions.FightDuration);
                        break;
                    case Trigger.SpellMiss:
                        spellPower += effect.Stats.SpellPower * effect.GetAverageUptime(CastTime / Ticks, 1 - HitProcs / Ticks, BaseCastTime, calculationOptions.FightDuration);
                        break;
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                        spellPower += effect.Stats.SpellPower * effect.GetAverageUptime(CastTime / Ticks, CastProcs / Ticks, BaseCastTime, calculationOptions.FightDuration);
                        break;
                }
            }
            //if (baseStats.SpellPowerFor15SecOnCast_50_45 > 0) spellPower += baseStats.SpellPowerFor15SecOnCast_50_45 * 15f / (45f + CastTime / CastProcs / 0.5f);
            //if (baseStats.SpellDamageFor10SecOnHit_5 > 0) spellPower += baseStats.SpellDamageFor10SecOnHit_5 * ProcBuffUp(1 - (float)Math.Pow(0.95, TargetProcs), 10, CastTime);
            //if (baseStats.SpellPowerFor6SecOnCrit > 0) spellPower += baseStats.SpellPowerFor6SecOnCrit * ProcBuffUp(CritProcs / Ticks, 6, CastTime / Ticks);
            //if (baseStats.SpellPowerFor10SecOnHit_10_45 > 0) spellPower += baseStats.SpellPowerFor10SecOnHit_10_45 * 10f / (45f + CastTime / HitProcs / 0.1f);
            //if (baseStats.SpellPowerFor10SecOnCast_15_45 > 0) spellPower += baseStats.SpellPowerFor10SecOnCast_15_45 * 10f / (45f + CastTime / CastProcs / 0.15f);
            //if (baseStats.SpellPowerFor10SecOnCast_10_45 > 0) spellPower += baseStats.SpellPowerFor10SecOnCast_10_45 * 10f / (45f + CastTime / CastProcs / 0.1f);
            //if (baseStats.SpellPowerFor10SecOnResist > 0) spellPower += baseStats.SpellPowerFor10SecOnResist * ProcBuffUp(1 - HitProcs / Ticks, 10, CastTime / Ticks);
            //if (baseStats.SpellPowerFor15SecOnCrit_20_45 > 0) spellPower += baseStats.SpellPowerFor15SecOnCrit_20_45 * 15f / (45f + CastTime / CritProcs / 0.2f);
            //if (baseStats.SpellPowerFor10SecOnCrit_20_45 > 0) spellPower += baseStats.SpellPowerFor10SecOnCrit_20_45 * 10f / (45f + CastTime / CritProcs / 0.2f);
            if (baseStats.ShatteredSunAcumenProc > 0 && calculationOptions.Aldor) spellPower += 120 * 10f / (45f + CastTime / HitProcs / 0.1f);

            SpammedDot = spammedDot;
            if (!forceMiss)
            {
                AverageDamage = CalculateAverageDamage(baseStats, calculationOptions, spellPower, spammedDot, forceHit);

                DamagePerSecond = AverageDamage / CastTime;
                ThreatPerSecond = DamagePerSecond * ThreatMultiplier;
            }
            CastTime *= (1 - channelReduction);
            CostPerSecond = CalculateCost(mageTalents, round) / CastTime;

            /*float casttimeHash = castingState.ClearcastingChance * 100 + CastTime;
            float OO5SR = 0;
            if (!FSRCalc.TryGetCachedOO5SR(Name, casttimeHash, out OO5SR))
            {
                FSRCalc fsr = new FSRCalc();
                fsr.AddSpell(CastTime - castingState.Latency, castingState.Latency, Channeled);
                OO5SR = fsr.CalculateOO5SR(castingState.ClearcastingChance, Name, casttimeHash);
            }*/

            if (outOfFiveSecondRule)
            {
                OO5SR = 1;
            }

            /*if (Cost > 0)
            {
                OO5SR = FSRCalc.CalculateSimpleOO5SR(castingState.ClearcastingChance, CastTime - castingState.Latency, castingState.Latency, Channeled);
            }*/
        }

        public float CalculateAverageDamage(Stats baseStats, CalculationOptionsMage calculationOptions, float spellPower, bool spammedDot, bool forceHit)
        {
            float baseAverage = (BaseMinDamage + BaseMaxDamage) / 2f + spellPower * SpellDamageCoefficient;
            float critMultiplier = 1 + (CritBonus - 1) * Math.Max(0, CritRate/* - castingState.ResilienceCritRateReduction*/);
            float resistMultiplier = (forceHit ? 1.0f : HitRate) * PartialResistFactor;
            int targets = 1;
            if (AreaEffect) targets = calculationOptions.AoeTargets;
            float averageDamage = baseAverage * SpellModifier * DirectDamageModifier * targets * (forceHit ? 1.0f : HitRate);
            if (AreaEffect && averageDamage > AoeDamageCap) averageDamage = AoeDamageCap;
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
            Spell spell = new Spell(this);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState);
            return spell;
        }

        public virtual float GetEffectAverageDamage(CastingState castingState)
        {
            Spell spell = new Spell(this);
            spell.Calculate(castingState);
            return spell.CalculateAverageDamage(castingState.BaseStats, castingState.CalculationOptions, 0, false, false);
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

            BaseDirectDamageModifier = 1.0f;
            BaseDotDamageModifier = 1.0f;
            BaseCostModifier = 1.0f;
            BaseCostAmplifier = 1.0f;
            BaseCostAmplifier *= (1.0f - 0.01f * mageTalents.ElementalPrecision);
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

    #region Base Spells
    public class WaterboltTemplate : SpellTemplate
    {
        Stats waterElementalBuffs;
        string[] validBuffs = new string[] { "Ferocious Inspiration", "Sanctified Retribution", "Improved Moonkin Form", "Swift Retribution", "Elemental Oath", "Moonkin Form", "Wrath of Air Totem", "Demonic Pact", "Flametongue Totem", "Enhancing Totems (Spell Power)", "Totem of Wrath (Spell Power)", "Heart of the Crusader", "Master Poisoner", "Totem of Wrath", "Winter's Chill", "Improved Scorch", "Improved Shadow Bolt", "Curse of the Elements", "Earth and Moon", "Ebon Plaguebringer", "Improved Faerie Fire", "Misery" };

        public WaterboltTemplate(CharacterCalculationsMage calculations)
        {
            Name = "Waterbolt";
            waterElementalBuffs = new Stats();
            foreach (Buff buff in calculations.ActiveBuffs)
            {
                if (Array.IndexOf(validBuffs, buff.Name) >= 0)
                {
                    waterElementalBuffs.Accumulate(buff.Stats);
                }
            }
        }

        public override Spell GetSpell(CastingState castingState)
        {
            return GetSpell(castingState, castingState.FrostSpellPower);
        }

        public Spell GetSpell(CastingState castingState, float spellPower)
        {
            Spell spell = new Spell(this);

            Character character = castingState.CalculationOptions.Character;
            CalculationOptionsMage calculationOptions = castingState.CalculationOptions;
            int playerLevel = calculationOptions.PlayerLevel;
            int targetLevel = calculationOptions.TargetLevel;
            // 45 sec, 3 min cooldown + cold snap
            // 2.5 sec Waterbolt, affected by heroism, totems, 0.4x frost damage from character
            // TODO recheck all buffs that apply
            float spellCrit = 0.05f + waterElementalBuffs.SpellCrit;
            float hitRate = castingState.FrostHitRate;
            float multiplier = hitRate;
            float haste = (1f + waterElementalBuffs.SpellHaste);
            multiplier *= (1 + waterElementalBuffs.BonusDamageMultiplier) * (1 + waterElementalBuffs.BonusFrostDamageMultiplier);
            if (castingState.Heroism) haste *= 1.3f;
            float realResistance = calculationOptions.FrostResist;
            float partialResistFactor = (realResistance == 1) ? 0 : (1 - realResistance - ((targetLevel > playerLevel) ? ((targetLevel - playerLevel) * 0.02f) : 0f));
            multiplier *= partialResistFactor;

            spell.CastTime = 2.5f / haste;
            spell.CostPerSecond = 0.0f;
            spell.DamagePerSecond = (521.5f + (0.4f * spellPower + waterElementalBuffs.SpellPower + waterElementalBuffs.BonusSpellPowerDemonicPactMultiplier * calculationOptions.WarlockSpellPower) * 2.5f / 3.5f) * multiplier * (1 + 0.5f * spellCrit) / 2.5f * haste;
            spell.ThreatPerSecond = 0.0f;

            return spell;
        }
    }

    public class WandTemplate : SpellTemplate
    {
        private float speed;

        public WandTemplate(CharacterCalculationsMage calculations, MagicSchool school, int minDamage, int maxDamage, float speed)
            : base("Wand", false, false, false, 0, 30, 0, 0, school, minDamage, maxDamage, 0, 1, 0, 0, 0, 0)
        {
            // Tested: affected by Arcane Instability, affected by Chaotic meta, not affected by Arcane Power
            Calculate(calculations);
            this.speed = speed;
            CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1));
            BaseSpellModifier = (1 + 0.01f * calculations.MageTalents.ArcaneInstability) * (1 + 0.01f * calculations.MageTalents.PlayingWithFire) * (1 + calculations.BaseStats.BonusDamageMultiplier);
            switch (school)
            {
                case MagicSchool.Arcane:
                    BaseSpellModifier *= (1 + calculations.BaseStats.BonusArcaneDamageMultiplier);
                    break;
                case MagicSchool.Fire:
                    BaseSpellModifier *= (1 + calculations.BaseStats.BonusFireDamageMultiplier);
                    break;
                case MagicSchool.Frost:
                    BaseSpellModifier *= (1 + calculations.BaseStats.BonusFrostDamageMultiplier);
                    break;
                case MagicSchool.Nature:
                    BaseSpellModifier *= (1 + calculations.BaseStats.BonusNatureDamageMultiplier);
                    break;
                case MagicSchool.Shadow:
                    BaseSpellModifier *= (1 + calculations.BaseStats.BonusShadowDamageMultiplier);
                    break;
            }
        }

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = new Spell(this);
            spell.Calculate(castingState);
            spell.CastTime = speed;
            spell.CritRate = castingState.CritRate;

            if (spell.CritRate < 0.0f) spell.CritRate = 0.0f;
            if (spell.CritRate > 1.0f) spell.CritRate = 1.0f;

            spell.SpellModifier = BaseSpellModifier;

            spell.HitProcs = HitRate;
            spell.CritProcs = spell.HitProcs * spell.CritRate;
            spell.TargetProcs = spell.HitProcs;

            spell.AverageDamage = spell.CalculateAverageDamage(castingState.BaseStats, castingState.CalculationOptions, 0, false, false);

            spell.DamagePerSecond = spell.AverageDamage / speed;
            spell.ThreatPerSecond = spell.DamagePerSecond * ThreatMultiplier;
            spell.CostPerSecond = 0;
            spell.OO5SR = 1;
            return spell;
        }
    }

    public class FireBlastTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static FireBlastTemplate()
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

        public FireBlastTemplate(CharacterCalculationsMage calculations)
            : base("Fire Blast", false, true, false, 20, 0, 8, MagicSchool.Fire, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            Cooldown -= 1.0f * calculations.MageTalents.ImprovedFireBlast;
            BaseCritRate += 0.02f * calculations.MageTalents.Incineration;
            BaseSpellModifier *= (1 + 0.02f * calculations.MageTalents.SpellImpact + 0.02f * calculations.MageTalents.FirePower) / (1 + 0.02f * calculations.MageTalents.FirePower);
        }
    }

    public class ScorchTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ScorchTemplate()
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

        public virtual Spell GetSpell(CastingState castingState, bool clearcastingActive)
        {
            Spell spell = new Spell(this);
            spell.Calculate(castingState);
            spell.CalculateManualClearcasting(true, false, clearcastingActive);
            spell.CalculateDerivedStats(castingState);
            spell.CalculateManualClearcastingCost(castingState.MageTalents, false, true, false, clearcastingActive);
            return spell;
        }

        public ScorchTemplate(CharacterCalculationsMage calculations)
            : base("Scorch", false, false, false, 30, 1.5f, 0, MagicSchool.Fire, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            BaseCritRate += 0.02f * calculations.MageTalents.Incineration;
            BaseCritRate += 0.01f * calculations.MageTalents.ImprovedScorch;
            BaseSpellModifier *= (1 + 0.02f * calculations.MageTalents.SpellImpact + 0.02f * calculations.MageTalents.FirePower) / (1 + 0.02f * calculations.MageTalents.FirePower);
        }
    }

    public class FlamestrikeTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static FlamestrikeTemplate()
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

        public virtual Spell GetSpell(CastingState castingState, bool spammedDot)
        {
            Spell spell = new Spell(this);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState, false, false, spammedDot);
            return spell;
        }

        public FlamestrikeTemplate(CharacterCalculationsMage calculations)
            : base("Flamestrike", false, false, true, 30, 3, 0, MagicSchool.Fire, GetMaxRankSpellData(calculations.CalculationOptions), 1, 1, 8f)
        {
            Calculate(calculations);
            AoeDamageCap = 37500;
            BaseCritRate += 0.02f * calculations.MageTalents.WorldInFlames;
        }
    }

    public class ConjureManaGemTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ConjureManaGemTemplate()
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

        public ConjureManaGemTemplate(CharacterCalculationsMage calculations)
            : base("Conjure Mana Gem", false, false, false, 0, 3, 0, MagicSchool.Arcane, GetMaxRankSpellData(calculations.CalculationOptions), 0, 1)
        {
            Calculate(calculations);
        }
    }

    public class FrostNovaTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static FrostNovaTemplate()
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

        public FrostNovaTemplate(CharacterCalculationsMage calculations)
            : base("Frost Nova", false, true, true, 0, 0, 25, MagicSchool.Frost, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
        }
    }

    public class FrostboltTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static FrostboltTemplate()
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

        public Spell GetSpell(CastingState castingState, bool manualClearcasting, bool clearcastingActive, bool pom, bool averageFingersOfFrost)
        {
            Spell spell = new Spell(this);
            spell.Calculate(castingState);
            if (manualClearcasting) spell.CalculateManualClearcasting(true, false, clearcastingActive);
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            if (averageFingersOfFrost)
            {
                spell.CritRate += fingersOfFrostCritRate;
            }
            spell.CalculateDerivedStats(castingState, false, pom, false);
            if (manualClearcasting) spell.CalculateManualClearcastingCost(castingState.MageTalents, false, true, false, clearcastingActive);
            return spell;
        }

        float fingersOfFrostCritRate;
        float tormentTheWeak;

        public Spell GetSpell(CastingState castingState, bool averageFingersOfFrost)
        {
            Spell spell = new Spell(this);
            spell.Calculate(castingState);
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            if (averageFingersOfFrost)
            {
                spell.CritRate += fingersOfFrostCritRate;
            }
            spell.CalculateDerivedStats(castingState);
            return spell;
        }

        public override Spell GetSpell(CastingState castingState)
        {
            return GetSpell(castingState, false);
        }

        public FrostboltTemplate(CharacterCalculationsMage calculations)
            : base("Frostbolt", false, false, false, 30, 3, 0, MagicSchool.Frost, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            if (calculations.MageTalents.GlyphOfFrostbolt)
            {
                BaseDirectDamageModifier *= 1.05f;
            }
            BaseCritRate += 0.01f * calculations.MageTalents.WintersChill;
            BaseCastTime -= 0.1f * calculations.MageTalents.ImprovedFrostbolt;
            BaseCritRate += 0.02f * calculations.MageTalents.EmpoweredFrostbolt;
            BaseInterruptProtection += calculations.BaseStats.AldorRegaliaInterruptProtection;
            SpellDamageCoefficient += 0.05f * calculations.MageTalents.EmpoweredFrostbolt;
            BaseSpellModifier *= (1 + calculations.BaseStats.BonusMageNukeMultiplier) * (1 + 0.01f * calculations.MageTalents.ChilledToTheBone);
            float fof = (calculations.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * calculations.MageTalents.FingersOfFrost);
            fingersOfFrostCritRate = (1.0f - (1.0f - fof) * (1.0f - fof)) * (calculations.MageTalents.Shatter == 3 ? 0.5f : 0.17f * calculations.MageTalents.Shatter);
            tormentTheWeak = 0.04f * calculations.MageTalents.TormentTheWeak;
        }
    }

    public class FireballTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static FireballTemplate()
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

        public Spell GetSpell(CastingState castingState, bool pom, bool brainFreeze)
        {
            Spell spell = new Spell(this);
            spell.Calculate(castingState);
            if (brainFreeze)
            {
                spell.CostAmplifier = 0;
            }
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.CalculateDerivedStats(castingState, false, pom || brainFreeze, true);
            return spell;
        }

        float tormentTheWeak;

        public FireballTemplate(CharacterCalculationsMage calculations)
            : base("Fireball", false, false, false, 35, 3.5f, 0, MagicSchool.Fire, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            if (calculations.MageTalents.GlyphOfFireball)
            {
                BasePeriodicDamage = 0.0f;
                BaseCritRate += 0.05f;
            }
            BaseCritRate += 0.01f * calculations.MageTalents.ImprovedScorch;
            DotDuration = 8;
            BaseInterruptProtection += calculations.BaseStats.AldorRegaliaInterruptProtection;
            BaseCastTime -= 0.1f * calculations.MageTalents.ImprovedFireball;
            SpellDamageCoefficient += 0.05f * calculations.MageTalents.EmpoweredFire;
            BaseSpellModifier *= (1 + calculations.BaseStats.BonusMageNukeMultiplier);
            tormentTheWeak = 0.04f * calculations.MageTalents.TormentTheWeak;
            BaseSpellModifier *= (1 + 0.02f * calculations.MageTalents.SpellImpact + 0.02f * calculations.MageTalents.FirePower) / (1 + 0.02f * calculations.MageTalents.FirePower);
        }
    }

    public class FrostfireBoltTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static FrostfireBoltTemplate()
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

        private float tormentFactor;
        private float fingersOfFrostCritRate;

        public Spell GetSpell(CastingState castingState, bool pom, bool averageFingersOfFrost)
        {
            Spell spell = new Spell(this);
            spell.Calculate(castingState);
            spell.SpellModifier *= (1 + tormentFactor * castingState.SnaredTime);
            if (averageFingersOfFrost)
            {
                spell.CritRate += fingersOfFrostCritRate;
            }
            spell.CalculateDerivedStats(castingState, false, pom, true);
            return spell;
        }

        public FrostfireBoltTemplate(CharacterCalculationsMage calculations)
            : base("Frostfire Bolt", false, false, false, 40, 3.0f, 0, MagicSchool.FrostFire, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            if (calculations.MageTalents.GlyphOfFrostfire)
            {
                BaseCritRate += 0.02f;
                BaseDirectDamageModifier *= 1.02f;
            }
            BaseCritRate += 0.01f * calculations.MageTalents.ImprovedScorch;
            tormentFactor = 0.04f * calculations.MageTalents.TormentTheWeak;
            BaseSpellModifier *= (1 + 0.01f * calculations.MageTalents.ChilledToTheBone);
            SpellDamageCoefficient += 0.05f * calculations.MageTalents.EmpoweredFire;
            DotDuration = 9;
            float fof = (calculations.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * calculations.MageTalents.FingersOfFrost);
            fingersOfFrostCritRate = (1.0f - (1.0f - fof) * (1.0f - fof)) * (calculations.MageTalents.Shatter == 3 ? 0.5f : 0.17f * calculations.MageTalents.Shatter);
        }
    }

    public class PyroblastTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static PyroblastTemplate()
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

        public Spell GetSpell(CastingState castingState, bool pom)
        {
            Spell spell = new Spell(this);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState, false, pom, false);
            return spell;
        }

        public PyroblastTemplate(CharacterCalculationsMage calculations)
            : base("Pyroblast", false, false, false, 35, 5f, 0, MagicSchool.Fire, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            DotDuration = 12;
            BaseCritRate += 0.02f * calculations.MageTalents.WorldInFlames;
        }
    }

    public class LivingBombTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static LivingBombTemplate()
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

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = new Spell(this);
            spell.Calculate(castingState);
            if (castingState.MageTalents.GlyphOfLivingBomb)
            {
                spell.DotDamageModifier = (1 + Math.Max(0.0f, Math.Min(1.0f, castingState.FireCritRate)) * (castingState.FireCritBonus - 1));
            }
            spell.CalculateDerivedStats(castingState, false, false, false);
            return spell;
        }

        public LivingBombTemplate(CharacterCalculationsMage calculations)
            : base("Living Bomb", false, true, false, 35, 0f, 0, MagicSchool.Fire, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            DotDuration = 12;
            BaseCritRate += 0.02f * calculations.MageTalents.WorldInFlames;
            BaseSpellModifier /= (1 + 0.02f * calculations.MageTalents.FirePower); // Living Bomb dot does not benefit from Fire Power
            BaseDirectDamageModifier *= (1 + 0.02f * calculations.MageTalents.FirePower);
        }
    }

    public class SlowTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static SlowTemplate()
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

        public SlowTemplate(CharacterCalculationsMage calculations)
            : base("Slow", false, true, false, 30, 0f, 0, MagicSchool.Arcane, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
        }
    }

    //922-983
    //
    //709 + k*992<=922
    //776 + k*992>=983
    //
    //0.20866935483870967741935483870968 <= k <= 0.21471774193548387096774193548387
    public class ConeOfColdTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ConeOfColdTemplate()
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

        public ConeOfColdTemplate(CharacterCalculationsMage calculations)
            : base("Cone of Cold", false, true, true, 0, 0, 10, MagicSchool.Frost, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            Cooldown *= (1 - 0.07f * calculations.MageTalents.IceFloes + (calculations.MageTalents.IceFloes == 3 ? 0.01f : 0.00f));
            AoeDamageCap = 37500;
            int ImprovedConeOfCold = calculations.MageTalents.ImprovedConeOfCold;
            BaseSpellModifier *= (1 + ((ImprovedConeOfCold > 0) ? (0.05f + 0.1f * ImprovedConeOfCold) : 0)) * (1 + 0.02f * calculations.MageTalents.SpellImpact);
            BaseCritRate += 0.02f * calculations.MageTalents.Incineration;
        }
    }

    public class IceLanceTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static IceLanceTemplate()
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

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = new Spell(this);
            spell.Calculate(castingState);
            if (castingState.Frozen)
            {
                if (castingState.MageTalents.GlyphOfIceLance && castingState.CalculationOptions.TargetLevel > castingState.CalculationOptions.PlayerLevel)
                {
                    spell.SpellModifier *= 4;
                }
                else
                {
                    spell.SpellModifier *= 3;
                }
            }
            spell.CalculateDerivedStats(castingState);
            return spell;
        }

        public IceLanceTemplate(CharacterCalculationsMage calculations)
            : base("Ice Lance", false, true, false, 30, 0, 0, MagicSchool.Frost, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            BaseSpellModifier *= (1 + 0.02f * calculations.MageTalents.SpellImpact) * (1 + 0.01f * calculations.MageTalents.ChilledToTheBone);
        }
    }

    public class ArcaneBarrageTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ArcaneBarrageTemplate()
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

        public Spell GetSpell(CastingState castingState, float arcaneBlastDebuff)
        {
            Spell spell = new Spell(this);
            spell.Calculate(castingState);
            spell.SpellModifier *= 1 + arcaneBlastDamageMultiplier * arcaneBlastDebuff;
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.CalculateDerivedStats(castingState);
            return spell;
        }

        private float arcaneBlastDamageMultiplier;
        private float tormentTheWeak;

        public ArcaneBarrageTemplate(CharacterCalculationsMage calculations)
            : base("Arcane Barrage", false, true, false, 30, 0, 3, MagicSchool.Arcane, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            tormentTheWeak = 0.04f * calculations.MageTalents.TormentTheWeak;
            arcaneBlastDamageMultiplier = calculations.MageTalents.GlyphOfArcaneBlast ? 0.18f : 0.15f;
            if (calculations.MageTalents.GlyphOfArcaneBarrage)
            {
                BaseCostAmplifier *= 0.8f; // TODO is it additive or multiplicative?
            }
        }
    }

    public class ArcaneBlastTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ArcaneBlastTemplate()
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

        public Spell GetSpell(CastingState castingState, int debuff, bool manualClearcasting, bool clearcastingActive, bool pom)
        {
            Spell spell = new Spell(this);
            spell.Calculate(castingState);
            if (manualClearcasting) spell.CalculateManualClearcasting(true, false, clearcastingActive);
            spell.SpellModifier *= baseAdditiveSpellModifier + arcaneBlastDamageMultiplier * debuff;
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.CalculateDerivedStats(castingState, false, pom, false, true, false, false);
            if (manualClearcasting) spell.CalculateManualClearcastingCost(castingState.MageTalents, false, true, false, clearcastingActive);
            return spell;
        }

        public Spell GetSpell(CastingState castingState, int debuff, bool forceHit)
        {
            Spell spell = new Spell(this);
            spell.Calculate(castingState);
            spell.SpellModifier *= baseAdditiveSpellModifier + arcaneBlastDamageMultiplier * debuff;
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.CalculateDerivedStats(castingState, false, false, false, true, forceHit, !forceHit);
            return spell;
        }

        public Spell GetSpell(CastingState castingState, int debuff)
        {
            Spell spell = new Spell(this);
            spell.Calculate(castingState);
            spell.SpellModifier *= baseAdditiveSpellModifier + arcaneBlastDamageMultiplier * debuff;
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.CostModifier += 2.00f * debuff;
            spell.CalculateDerivedStats(castingState, false, false, false, true, false, false);
            return spell;
        }

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = new Spell(this);
            spell.Calculate(castingState);
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.CalculateDerivedStats(castingState, false, false, false, true, false, false);
            return spell;
        }

        public void AddToCycle(MageTalents mageTalents, Cycle cycle, Spell rawSpell, float weight0, float weight1, float weight2, float weight3)
        {
            float weight = weight0 + weight1 + weight2 + weight3;
            cycle.CastTime += weight * rawSpell.CastTime;
            cycle.CastProcs += weight * rawSpell.CastProcs;
            cycle.Ticks += weight * rawSpell.Ticks;
            cycle.HitProcs += weight * rawSpell.HitProcs;
            cycle.CritProcs += weight * rawSpell.CritProcs;
            cycle.TargetProcs += weight * rawSpell.TargetProcs;

            double roundCost = Math.Round(rawSpell.BaseCost * rawSpell.CostAmplifier);
            cycle.costPerSecond += (1 - 0.02f * mageTalents.ArcaneConcentration) * (weight0 * (float)Math.Floor(roundCost * rawSpell.CostModifier) + weight1 * (float)Math.Floor(roundCost * (rawSpell.CostModifier + 2.00f)) + weight2 * (float)Math.Floor(roundCost * (rawSpell.CostModifier + 4.00f)) + weight3 * (float)Math.Floor(roundCost * (rawSpell.CostModifier + 6.00f)));
            cycle.costPerSecond -= weight * rawSpell.CritRate * rawSpell.BaseCost * 0.1f * mageTalents.MasterOfElements;

            float multiplier = weight * baseAdditiveSpellModifier + arcaneBlastDamageMultiplier * (weight1 + 2 * weight2 + 3 * weight3);
            cycle.damagePerSecond += multiplier * rawSpell.CastTime * rawSpell.DamagePerSecond;
            cycle.threatPerSecond += multiplier * rawSpell.CastTime * rawSpell.ThreatPerSecond;
        }

        private float arcaneBlastDamageMultiplier;
        private float baseAdditiveSpellModifier;
        private float tormentTheWeak;

        public ArcaneBlastTemplate(CharacterCalculationsMage calculations)
            : base("Arcane Blast", false, false, false, 30, 2.5f, 0, MagicSchool.Arcane, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Stats baseStats = calculations.BaseStats;
            MageTalents mageTalents = calculations.MageTalents;
            CalculationOptionsMage calculationOptions = calculations.CalculationOptions;
            Calculate(calculations);
            BaseInterruptProtection += 0.2f * mageTalents.ArcaneStability;
            BaseCostModifier += baseStats.ArcaneBlastBonus;
            arcaneBlastDamageMultiplier = mageTalents.GlyphOfArcaneBlast ? 0.18f : 0.15f;
            baseAdditiveSpellModifier = 1.0f + baseStats.ArcaneBlastBonus + 0.02f * mageTalents.SpellImpact;
            tormentTheWeak = 0.04f * mageTalents.TormentTheWeak;
            SpellDamageCoefficient += 0.03f * mageTalents.ArcaneEmpowerment;
            BaseCritRate += 0.02f * mageTalents.Incineration;
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
    public class ArcaneMissilesTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ArcaneMissilesTemplate()
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

        public Spell GetSpell(CastingState castingState, bool barrage, bool clearcastingAveraged, bool clearcastingActive, bool clearcastingProccing, int arcaneBlastDebuff, float ticks)
        {
            Spell spell = new Spell(this);
            //spell.ClearcastingProccing = clearcastingProccing;
            spell.Calculate(castingState);
            spell.CalculateManualClearcasting(true, clearcastingAveraged, clearcastingActive);
            spell.BaseCastTime = ticks;
            if (barrage) spell.BaseCastTime *= 0.5f;
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime) * (1 + arcaneBlastDamageMultiplier * arcaneBlastDebuff);
            spell.SpellModifier *= ticks / 5.0f;
            spell.CalculateDerivedStats(castingState);
            spell.CalculateManualClearcastingCost(castingState.MageTalents, false, true, clearcastingAveraged, clearcastingActive);
            return spell;
        }

        public Spell GetSpell(CastingState castingState, bool barrage, int arcaneBlastDebuff)
        {
            return GetSpell(castingState, barrage, arcaneBlastDebuff, 5);
        }

        public Spell GetSpell(CastingState castingState, bool barrage, int arcaneBlastDebuff, int ticks)
        {
            Spell spell = new Spell(this);
            spell.Calculate(castingState);
            spell.BaseCastTime = ticks;
            if (barrage) spell.BaseCastTime *= 0.5f;
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime) * (1 + arcaneBlastDamageMultiplier * arcaneBlastDebuff);
            spell.SpellModifier *= ticks / 5.0f;
            spell.CalculateDerivedStats(castingState);
            return spell;
        }

        float tormentTheWeak;
        float arcaneBlastDamageMultiplier;

        public ArcaneMissilesTemplate(CharacterCalculationsMage calculations)
            : base("Arcane Missiles", true, false, false, 30, 5, 0, MagicSchool.Arcane, GetMaxRankSpellData(calculations.CalculationOptions), 5, 6)
        {
            base.Calculate(calculations);
            if (calculations.MageTalents.GlyphOfArcaneMissiles)
            {
                CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * calculations.MageTalents.SpellPower + 0.1f * calculations.MageTalents.Burnout + calculations.BaseStats.CritBonusDamage + 0.25f));
            }
            SpellDamageCoefficient += 0.15f * calculations.MageTalents.ArcaneEmpowerment;
            tormentTheWeak = 0.04f * calculations.MageTalents.TormentTheWeak;
            arcaneBlastDamageMultiplier = calculations.MageTalents.GlyphOfArcaneBlast ? 0.18f : 0.15f;
            BaseSpellModifier *= (1 + calculations.BaseStats.BonusMageNukeMultiplier);
            BaseInterruptProtection += 0.2f * calculations.MageTalents.ArcaneStability;
        }
    }

    public class ArcaneExplosionTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ArcaneExplosionTemplate()
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

        public ArcaneExplosionTemplate(CharacterCalculationsMage calculations)
            : base("Arcane Explosion", false, true, true, 0, 0, 0, MagicSchool.Arcane, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            if (calculations.MageTalents.GlyphOfArcaneExplosion) BaseCostAmplifier *= 0.9f;
            BaseCritRate += 0.02f * calculations.MageTalents.WorldInFlames;
            BaseSpellModifier *= (1 + 0.02f * calculations.MageTalents.SpellImpact);
            AoeDamageCap = 37500;
        }
    }

    public class BlastWaveTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static BlastWaveTemplate()
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

        public BlastWaveTemplate(CharacterCalculationsMage calculations)
            : base("Blast Wave", false, true, true, 0, 0, 30, MagicSchool.Fire, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            AoeDamageCap = 37500;
            BaseSpellModifier *= (1 + 0.02f * calculations.MageTalents.SpellImpact + 0.02f * calculations.MageTalents.FirePower) / (1 + 0.02f * calculations.MageTalents.FirePower);
            BaseCritRate += 0.02f * calculations.MageTalents.WorldInFlames;
        }
    }

    public class DragonsBreathTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static DragonsBreathTemplate()
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

        public DragonsBreathTemplate(CharacterCalculationsMage calculations)
            : base("Dragon's Breath", false, true, true, 0, 0, 20, MagicSchool.Fire, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            AoeDamageCap = 37500;
            BaseCritRate += 0.02f * calculations.MageTalents.WorldInFlames;
        }
    }

    public class BlizzardTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static BlizzardTemplate()
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

        public BlizzardTemplate(CharacterCalculationsMage calculations)
            : base("Blizzard", true, false, true, 30, 8, 0, MagicSchool.Frost, GetMaxRankSpellData(calculations.CalculationOptions), 4, 1)
        {
            Calculate(calculations);
            if (calculations.MageTalents.ImprovedBlizzard > 0)
            {
                float fof = (calculations.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * calculations.MageTalents.FingersOfFrost);
                fof = Math.Max(fof, 0.05f * calculations.MageTalents.Frostbite);
                BaseCritRate += (1.0f - (1.0f - fof) * (1.0f - fof)) * (calculations.MageTalents.Shatter == 3 ? 0.5f : 0.17f * calculations.MageTalents.Shatter);
                //CritRate += (1.0f - (float)Math.Pow(1 - 0.05 * castingState.MageTalents.Frostbite, 5.0 / 2.0)) * (castingState.MageTalents.Shatter == 3 ? 0.5f : 0.17f * castingState.MageTalents.Shatter);
            }
            AoeDamageCap = 200000;
            BaseCritRate += 0.02f * calculations.MageTalents.WorldInFlames;
        }
    }

    // lightning capacitor
    public class LightningBoltTemplate : SpellTemplate
    {
        public LightningBoltTemplate(CharacterCalculationsMage calculations)
            : base("Lightning Bolt", false, true, false, 0, 0, 0, 0, MagicSchool.Nature, 694, 806, 0, 0, 0, 0, 0, 0)
        {
            Calculate(calculations);
            CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1));
        }
    }

    // lightning capacitor
    public class ThunderBoltTemplate : SpellTemplate
    {
        public ThunderBoltTemplate(CharacterCalculationsMage calculations)
            : base("Lightning Bolt", false, true, false, 0, 0, 0, 0, MagicSchool.Nature, 1181, 1371, 0, 0, 0, 0, 0, 0)
        {
            Calculate(calculations);
            CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1));
        }
    }

    // Shattered Sun Pendant of Acumen
    public class ArcaneBoltTemplate : SpellTemplate
    {
        public ArcaneBoltTemplate(CharacterCalculationsMage calculations)
            : base("Arcane Bolt", false, true, false, 0, 0, 0, 0, MagicSchool.Arcane, 333, 367, 0, 0, 0, 0, 0, 0)
        {
            Calculate(calculations);
            CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1));
        }
    }

    // Pendulum of Telluric Currents
    public class PendulumOfTelluricCurrentsTemplate : SpellTemplate
    {
        public PendulumOfTelluricCurrentsTemplate(CharacterCalculationsMage calculations)
            : base("Pendulum of Telluric Currents", false, true, false, 0, 0, 0, 0, MagicSchool.Shadow, 1168, 1752, 0, 0, 0, 0, 0, 0)
        {
            Calculate(calculations);
            CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1));
        }
    }

    // Lightweave Embroidery
    public class LightweaveBoltTemplate : SpellTemplate
    {
        public LightweaveBoltTemplate(CharacterCalculationsMage calculations)
            : base("Lightweave Bolt", false, true, false, 0, 0, 0, 0, MagicSchool.Holy, 1000, 1200, 0, 0, 0, 0, 0, 0)
        {
            Calculate(calculations);
            CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1));
        }
    }
    #endregion

    public class StaticCycle : Cycle
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
        public bool recalc5SR;

        private List<Spell> spellList;
        private FSRCalc fsr;

        public StaticCycle() : base(null)
        {
            spellList = new List<Spell>();
        }

        public StaticCycle(int capacity) : base(null)
        {
            spellList = new List<Spell>(capacity);
        }

        public StaticCycle(int capacity, bool recalcFiveSecondRule) : base(null)
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
            Ticks += spell.Ticks;
            CastTime += spell.CastTime;
            HitProcs += spell.HitProcs;
            CastProcs += spell.CastProcs;
            CritProcs += spell.CritProcs;
            TargetProcs += spell.TargetProcs;
            AverageDamage += spell.DamagePerSecond * spell.CastTime;
            AverageThreat += spell.ThreatPerSecond * spell.CastTime;
            Cost += spell.CostPerSecond * spell.CastTime;
            AffectedByFlameCap = AffectedByFlameCap || spell.AffectedByFlameCap;
            spellList.Add(spell);
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

            costPerSecond = Cost / CastTime;
            damagePerSecond = AverageDamage / CastTime;
            threatPerSecond = AverageThreat / CastTime;
            this.CastingState = castingState;            

            if (recalc5SR)
            {
                OO5SR = fsr.CalculateOO5SR(0.02f * castingState.MageTalents.ArcaneConcentration);
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

        public override void AddManaUsageContribution(Dictionary<string, float> dict, float duration)
        {
            foreach (Spell spell in spellList)
            {
                if (spell != null)
                {
                    spell.AddManaUsageContribution(dict, spell.CastTime * duration / CastTime);
                }
            }
        }
    }

    #region Spell Cycles

    public class DynamicCycle : Cycle
    {
        private List<Cycle> Cycle;
        private List<float> Weight;

        protected DynamicCycle(bool needsDisplayCalculations, CastingState castingState)
            : base(castingState)
        {
            if (needsDisplayCalculations)
            {
                Cycle = new List<Cycle>();
                Weight = new List<float>();
            }
        }

        protected void AddCycle(bool needsDisplayCalculations, Cycle cycle, float weight)
        {
            if (needsDisplayCalculations)
            {
                Cycle.Add(cycle);
                Weight.Add(weight);
            }
            CastTime += weight * cycle.CastTime;
            CastProcs += weight * cycle.CastProcs;
            Ticks += weight * cycle.Ticks;
            HitProcs += weight * cycle.HitProcs;
            CritProcs += weight * cycle.CritProcs;
            TargetProcs += weight * cycle.TargetProcs;
            costPerSecond += weight * cycle.CastTime * cycle.costPerSecond;
            damagePerSecond += weight * cycle.CastTime * cycle.damagePerSecond;
            threatPerSecond += weight * cycle.CastTime * cycle.threatPerSecond;
        }

        protected void AddSpell(bool needsDisplayCalculations, Spell spell, float weight)
        {
            if (needsDisplayCalculations)
            {
                Cycle.Add(spell);
                Weight.Add(weight);
            }
            CastTime += weight * spell.CastTime;
            CastProcs += weight * spell.CastProcs;
            Ticks += weight * spell.Ticks;
            HitProcs += weight * spell.HitProcs;
            CritProcs += weight * spell.CritProcs;
            TargetProcs += weight * spell.TargetProcs;
            costPerSecond += weight * spell.CastTime * spell.CostPerSecond;
            damagePerSecond += weight * spell.CastTime * spell.DamagePerSecond;
            threatPerSecond += weight * spell.CastTime * spell.ThreatPerSecond;
        }

        protected void AddPause(float duration, float weight)
        {
            CastTime += weight * duration;
        }

        protected void Calculate()
        {
            costPerSecond /= CastTime;
            damagePerSecond /= CastTime;
            threatPerSecond /= CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            for (int i = 0; i < Cycle.Count; i++)
            {
                if (Cycle[i] != null) Cycle[i].AddSpellContribution(dict, Weight[i] * Cycle[i].CastTime / CastTime * duration);
            }
        }

        public override void AddManaUsageContribution(Dictionary<string, float> dict, float duration)
        {
            for (int i = 0; i < Cycle.Count; i++)
            {
                if (Cycle[i] != null) Cycle[i].AddManaUsageContribution(dict, Weight[i] * Cycle[i].CastTime / CastTime * duration);
            }
        }
    }

    /*class ArcaneMissilesCC : Cycle
    {
        Spell AMc1;
        Spell AM10;
        Spell AM11;
        float CC;

        public ArcaneMissilesCC(CastingState castingState) : base(castingState)
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
            costPerSecond = (AMc1.CostPerSecond + AM10.CostPerSecond + CC / (1 - CC) * AM11.CostPerSecond) / (1 + 1 / (1 - CC));
            damagePerSecond = (AMc1.DamagePerSecond + AM10.DamagePerSecond + CC / (1 - CC) * AM11.DamagePerSecond) / (1 + 1 / (1 - CC));
            threatPerSecond = (AMc1.ThreatPerSecond + AM10.ThreatPerSecond + CC / (1 - CC) * AM11.ThreatPerSecond) / (1 + 1 / (1 - CC));
            //ManaRegenPerSecond = (AMc1.ManaRegenPerSecond + AM10.ManaRegenPerSecond + CC / (1 - CC) * AM11.ManaRegenPerSecond) / (1 + 1 / (1 - CC)); // we only use it indirectly in spell cycles that recompute oo5sr
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            AMc1.AddSpellContribution(dict, duration * 1 / (1 + 1 / (1 - CC)));
            AM10.AddSpellContribution(dict, duration * 1 / (1 + 1 / (1 - CC)));
            AM11.AddSpellContribution(dict, duration * CC / (1 - CC) / (1 + 1 / (1 - CC)));
        }

        public override void AddManaUsageContribution(Dictionary<string, float> dict, float duration)
        {
            AMc1.AddManaUsageContribution(dict, duration * 1 / (1 + 1 / (1 - CC)));
            AM10.AddManaUsageContribution(dict, duration * 1 / (1 + 1 / (1 - CC)));
            AM11.AddManaUsageContribution(dict, duration * CC / (1 - CC) / (1 + 1 / (1 - CC)));
        }
    }*/

    /*class ABAMP : StaticCycle
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
    }*/

    class ABP : StaticCycle
    {
        public ABP(CastingState castingState)
            : base(3)
        {
            Name = "ABP";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast0);

            AddSpell(AB, castingState);
            AddPause(3 - AB.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABAM : DynamicCycle
    {
        public ABAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB;
            Name = "ABAM";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles1);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB1);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to AB-AM
                AddSpell(needsDisplayCalculations, AB, 1);
                AddSpell(needsDisplayCalculations, AM, 1);
                Calculate();
            }
            else
            {
                //AB-AM 0.85
                AddSpell(needsDisplayCalculations, AB, 1 - MB);
                AddSpell(needsDisplayCalculations, AM, 1 - MB);

                //AB-MBAM 0.15
                AddSpell(needsDisplayCalculations, AB, MB);
                AddSpell(needsDisplayCalculations, MBAM, MB);

                Calculate();
            }
        }
    }

    class AB3AM : DynamicCycle
    {
        public AB3AM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB;
            float K1, K2;
            Name = "AB3AM";

            Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            K1 = (1 - MB) * (1 - MB) * (1 - MB);
            K2 = 1 - (1 - MB) * (1 - MB) * (1 - MB);

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                AddSpell(needsDisplayCalculations, AB0, K1 + K2);
                AddSpell(needsDisplayCalculations, AB1, K1 + K2);
                AddSpell(needsDisplayCalculations, AB2, K1 + K2);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.MageTalents, this, AB, K1 + K2, K1 + K2, K1 + K2, 0);
            }
            AddSpell(needsDisplayCalculations, AM3, K1);
            AddSpell(needsDisplayCalculations, MBAM3, K2);

            Calculate();
        }
    }

    class AB3AMABar : DynamicCycle
    {
        public AB3AMABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB;
            float K1, K2;
            Name = "AB3AMABar";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            K1 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            K2 = 1 - K1;

            //AB-AM 0.85
            AddSpell(needsDisplayCalculations, AB0, K1);
            AddSpell(needsDisplayCalculations, AB1, K1);
            AddSpell(needsDisplayCalculations, AB2, K1);
            AddSpell(needsDisplayCalculations, AM3, K1);
            AddSpell(needsDisplayCalculations, ABar, K1);

            //AB-MBAM 0.15
            AddSpell(needsDisplayCalculations, AB0, K2);
            AddSpell(needsDisplayCalculations, AB1, K2);
            AddSpell(needsDisplayCalculations, AB2, K2);
            AddSpell(needsDisplayCalculations, MBAM3, K2);
            AddSpell(needsDisplayCalculations, ABar, K2);

            Calculate();
        }
    }

    class AB3AM2MBAM : DynamicCycle
    {
        public AB3AM2MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB;
            float K1, K2, K3;
            Name = "AB3AM2MBAM";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            K1 = (1 - MB) * (1 - MB) * (1 - MB);
            K2 = (1 - MB) * (1 - (1 - MB) * (1 - MB));
            K3 = MB;

            //AB0-AB1-AB2-AM3      (1-MB)*(1-MB)*(1-MB)
            AddSpell(needsDisplayCalculations, AB0, K1);
            AddSpell(needsDisplayCalculations, AB1, K1);
            AddSpell(needsDisplayCalculations, AB2, K1);
            AddSpell(needsDisplayCalculations, AM3, K1);

            //AB0-AB1-AB2-MBAM3    (1-MB)*(1 - (1-MB)*(1-MB))
            AddSpell(needsDisplayCalculations, AB0, K2);
            AddSpell(needsDisplayCalculations, AB1, K2);
            AddSpell(needsDisplayCalculations, AB2, K2);
            AddSpell(needsDisplayCalculations, MBAM3, K2);

            //AB0-AB1-MBAM2        MB
            AddSpell(needsDisplayCalculations, AB0, K3);
            AddSpell(needsDisplayCalculations, AB1, K3);
            AddSpell(needsDisplayCalculations, MBAM2, K3);

            Calculate();
        }
    }

    class AB3AMABar2C : DynamicCycle
    {
        public AB3AMABar2C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB;
            float K1, K2, K3;
            Name = "AB3AMABar2C";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            K1 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            K2 = (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB));
            K3 = 1 - (1 - MB) * (1 - MB);

            //ABar-AB0-AB1-AB2-AM3      (1-MB)*(1-MB)*(1-MB)*(1-MB)
            AddSpell(needsDisplayCalculations, AB0, K1);
            AddSpell(needsDisplayCalculations, AB1, K1);
            AddSpell(needsDisplayCalculations, AB2, K1);
            AddSpell(needsDisplayCalculations, AM3, K1);
            AddSpell(needsDisplayCalculations, ABar, K1);

            //ABar-AB0-AB1-AB2-MBAM3    (1-MB)*(1-MB)*(1 - (1-MB)*(1-MB))
            AddSpell(needsDisplayCalculations, AB0, K2);
            AddSpell(needsDisplayCalculations, AB1, K2);
            AddSpell(needsDisplayCalculations, AB2, K2);
            AddSpell(needsDisplayCalculations, MBAM3, K2);
            AddSpell(needsDisplayCalculations, ABar, K2);

            //ABar-AB0-AB1-MBAM2        1 - (1-MB)*(1-MB)
            AddSpell(needsDisplayCalculations, AB0, K3);
            AddSpell(needsDisplayCalculations, AB1, K3);
            AddSpell(needsDisplayCalculations, MBAM2, K3);
            AddSpell(needsDisplayCalculations, ABar, K3);

            Calculate();
        }
    }

    class ABarAM : DynamicCycle
    {
        public ABarAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB;
            Name = "ABarAM";

            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to ABar-AM
                AddSpell(needsDisplayCalculations, ABar, 1);
                AddSpell(needsDisplayCalculations, AM, 1);

                Calculate();
            }
            else
            {
                //AB-AM 0.85
                AddSpell(needsDisplayCalculations, ABar, 1 - MB);
                AddSpell(needsDisplayCalculations, AM, 1 - MB);

                //AB-MBAM 0.15
                AddSpell(needsDisplayCalculations, ABar, MB);
                AddSpell(needsDisplayCalculations, MBAM, MB);
                if (ABar.CastTime + MBAM.CastTime < 3.0) AddPause(3.0f - ABar.CastTime - MBAM.CastTime, MB);

                Calculate();
            }
        }
    }

    class ABABarSlow : DynamicCycle
    {
        public ABABarSlow(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB;
            float X;
            float S0;
            float S1;
            // TODO not updated for 3.0.8 mode, consider deprecated?
            Name = "ABABarSlow";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast0);
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
            AddSpell(needsDisplayCalculations, ABar, (1 - X) * S0);
            AddSpell(needsDisplayCalculations, AB, (1 - X) * S0);
            if (AB.CastTime + ABar.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - AB.CastTime - ABar.CastTime, (1 - X) * S0);

            //ABar-MBAM
            AddSpell(needsDisplayCalculations, ABar, (1 - X) * S1);
            AddSpell(needsDisplayCalculations, MBAM, (1 - X) * S1);
            if (MBAM.CastTime + ABar.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime, (1 - X) * S1);

            //AB-ABar-Slow
            AddSpell(needsDisplayCalculations, ABar, X * S0);
            AddSpell(needsDisplayCalculations, AB, X * S0);
            AddSpell(needsDisplayCalculations, Slow, X * S0);
            if (AB.CastTime + ABar.CastTime + Slow.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - AB.CastTime - ABar.CastTime - Slow.CastTime, X * S0);

            //ABar-MBAM-Slow
            AddSpell(needsDisplayCalculations, ABar, X * S1);
            AddSpell(needsDisplayCalculations, MBAM, X * S1);
            AddSpell(needsDisplayCalculations, Slow, X * S1);
            if (MBAM.CastTime + ABar.CastTime + Slow.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime - Slow.CastTime, X * S1);

            Calculate();
        }
    }

    class FBABarSlow : DynamicCycle
    {
        public FBABarSlow(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            StaticCycle chain3;
            StaticCycle chain4;
            float MB;
            float X;
            float S0;
            float S1;
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
            chain1 = new StaticCycle(2);
            chain1.AddSpell(ABar, castingState);
            chain1.AddSpell(FB, castingState);
            if (FB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FB.CastTime - ABar.CastTime);
            chain1.Calculate(castingState);

            //ABar-MBAM
            chain2 = new StaticCycle(2);
            chain2.AddSpell(ABar, castingState);
            chain2.AddSpell(MBAM, castingState);
            if (MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime);
            chain2.Calculate(castingState);

            //AB-ABar-Slow
            chain3 = new StaticCycle(3);
            chain3.AddSpell(ABar, castingState);
            chain3.AddSpell(FB, castingState);
            chain3.AddSpell(Slow, castingState);
            if (FB.CastTime + ABar.CastTime + Slow.CastTime < 3.0) chain3.AddPause(3.0f + castingState.CalculationOptions.Latency - FB.CastTime - ABar.CastTime - Slow.CastTime);
            chain3.Calculate(castingState);

            //ABar-MBAM-Slow
            chain4 = new StaticCycle(3);
            chain4.AddSpell(ABar, castingState);
            chain4.AddSpell(MBAM, castingState);
            chain4.AddSpell(Slow, castingState);
            if (MBAM.CastTime + ABar.CastTime + Slow.CastTime < 3.0) chain4.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime - Slow.CastTime);
            chain4.Calculate(castingState);

            commonChain = chain1;

            AddCycle(needsDisplayCalculations, chain1, (1 - X) * S0);
            AddCycle(needsDisplayCalculations, chain2, (1 - X) * S1);
            AddCycle(needsDisplayCalculations, chain3, X * S0);
            AddCycle(needsDisplayCalculations, chain4, X * S1);
            Calculate();
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class FrBABarSlow : DynamicCycle
    {
        public FrBABarSlow(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            StaticCycle chain3;
            StaticCycle chain4;
            float MB;
            float X;
            float S0;
            float S1;
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
            chain1 = new StaticCycle(2);
            chain1.AddSpell(ABar, castingState);
            chain1.AddSpell(FrB, castingState);
            if (FrB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FrB.CastTime - ABar.CastTime);
            chain1.Calculate(castingState);

            //ABar-MBAM
            chain2 = new StaticCycle(2);
            chain2.AddSpell(ABar, castingState);
            chain2.AddSpell(MBAM, castingState);
            if (MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime);
            chain2.Calculate(castingState);

            //AB-ABar-Slow
            chain3 = new StaticCycle(3);
            chain3.AddSpell(ABar, castingState);
            chain3.AddSpell(FrB, castingState);
            chain3.AddSpell(Slow, castingState);
            if (FrB.CastTime + ABar.CastTime + Slow.CastTime < 3.0) chain3.AddPause(3.0f + castingState.CalculationOptions.Latency - FrB.CastTime - ABar.CastTime - Slow.CastTime);
            chain3.Calculate(castingState);

            //ABar-MBAM-Slow
            chain4 = new StaticCycle(3);
            chain4.AddSpell(ABar, castingState);
            chain4.AddSpell(MBAM, castingState);
            chain4.AddSpell(Slow, castingState);
            if (MBAM.CastTime + ABar.CastTime + Slow.CastTime < 3.0) chain4.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime - Slow.CastTime);
            chain4.Calculate(castingState);

            commonChain = chain1;

            AddCycle(needsDisplayCalculations, chain1, (1 - X) * S0);
            AddCycle(needsDisplayCalculations, chain2, (1 - X) * S1);
            AddCycle(needsDisplayCalculations, chain3, X * S0);
            AddCycle(needsDisplayCalculations, chain4, X * S1);
            Calculate();
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABABar0C : DynamicCycle
    {
        public ABABar0C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB;
            Name = "ABABar0C";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to AB-ABar
                AddSpell(needsDisplayCalculations, AB, 1);
                if (AB.CastTime + ABar.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - AB.CastTime - ABar.CastTime, 1);
                AddSpell(needsDisplayCalculations, ABar1, 1);

                Calculate();
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
                AddSpell(needsDisplayCalculations, ABar1, 1 - MB);
                AddSpell(needsDisplayCalculations, AB, 1 - MB);
                if (AB.CastTime + ABar.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - AB.CastTime - ABar.CastTime, 1 - MB);

                //ABar-MBAM
                AddSpell(needsDisplayCalculations, ABar, (2 - MB) * MB);
                AddSpell(needsDisplayCalculations, MBAM, (2 - MB) * MB);
                if (MBAM.CastTime + ABar.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime, (2 - MB) * MB);

                Calculate();
            }
        }
    }

    class ABABar1C : DynamicCycle
    {
        public ABABar1C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB;
            float S0, S1;
            Name = "ABABar1C";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast0);
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
            AddSpell(needsDisplayCalculations, AB, S0);
            if (AB.CastTime + ABar.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - AB.CastTime - ABar.CastTime, S0);
            AddSpell(needsDisplayCalculations, ABar1, S0);

            //AB-MBAM1-ABar
            AddSpell(needsDisplayCalculations, AB, S1);
            AddSpell(needsDisplayCalculations, MBAM1, S1);
            if (AB.CastTime + MBAM1.CastTime + ABar.CastTime < 3.0)
            {
                AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM1.CastTime - AB.CastTime - ABar.CastTime, S1);
                AddSpell(needsDisplayCalculations, ABar, S1);
            }
            else
            {
                AddSpell(needsDisplayCalculations, ABar, S1);
            }

            Calculate();
        }
    }

    class ABABar0MBAM : DynamicCycle
    {
        public ABABar0MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB;
            float S0, S1;
            Name = "ABABar0MBAM";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            // AB0-ABar1                   (1-MB)*(1-MB)
            // AB0-ABar1-MBAM              (1 - (1-MB)*(1-MB))

            // value = S0 * value(AB-ABar1) + S1 * value(AB-MBAM1-ABar)

            S0 = (1 - MB) * (1 - MB);
            S1 = (1 - (1 - MB) * (1 - MB));

            //AB-ABar1
            AddSpell(needsDisplayCalculations, AB, S0);
            if (AB.CastTime + ABar1.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - AB.CastTime - ABar1.CastTime, S0);
            AddSpell(needsDisplayCalculations, ABar1, S0);

            //AB-MBAM1-ABar
            AddSpell(needsDisplayCalculations, MBAM, S1);
            AddSpell(needsDisplayCalculations, AB, S1);
            if (AB.CastTime + MBAM.CastTime + ABar1.CastTime < 3.0)
            {
                AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - AB.CastTime - ABar1.CastTime, S1);
                AddSpell(needsDisplayCalculations, ABar1, S1);
            }
            else
            {
                AddSpell(needsDisplayCalculations, ABar1, S1);
            }

            Calculate();
        }
    }

    class ABABarY : DynamicCycle
    {
        public ABABarY(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            float MB;
            float S0, S1;
            Name = "ABABarY";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast0);
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
            chain1 = new StaticCycle(2);
            chain1.AddSpell(AB, castingState);
            if (AB.CastTime + ABar1.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB.CastTime - ABar1.CastTime); // this might not actually be needed if we're transitioning from S1, but we're assuming this cycle is used under low haste conditions only
            chain1.AddSpell(ABar1, castingState);
            chain1.Calculate(castingState);

            //AB-MBAM1
            chain2 = new StaticCycle(3);
            chain2.AddSpell(AB, castingState);
            chain2.AddSpell(MBAM1, castingState);
            chain2.Calculate(castingState);

            AddCycle(needsDisplayCalculations, chain1, S0);
            AddCycle(needsDisplayCalculations, chain2, S1);
            Calculate();
        }
    }

    class AB2ABarMBAM : DynamicCycle
    {
        public AB2ABarMBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float K1, K2, K3, K4;
            Name = "AB2ABarMBAM";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
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
            AddSpell(needsDisplayCalculations, AB0, K1);
            AddSpell(needsDisplayCalculations, AB1, K1);
            if (AB0.CastTime + AB1.CastTime + ABar2.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - ABar2.CastTime, K1);
            AddSpell(needsDisplayCalculations, ABar2, K1);

            //AB0-AB1-ABar2-MBAM
            AddSpell(needsDisplayCalculations, AB0, K2);
            AddSpell(needsDisplayCalculations, AB1, K2);
            if (AB0.CastTime + AB1.CastTime + ABar2.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - ABar2.CastTime, K2);
            AddSpell(needsDisplayCalculations, ABar2, K2);
            AddSpell(needsDisplayCalculations, MBAM, K2);

            //AB0-AB1-MBAM2-ABar
            AddSpell(needsDisplayCalculations, AB0, K3);
            AddSpell(needsDisplayCalculations, AB1, K3);
            AddSpell(needsDisplayCalculations, MBAM2, K3);
            if (AB0.CastTime + AB1.CastTime + MBAM2.CastTime + ABar.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - ABar.CastTime - MBAM2.CastTime, K3);
            AddSpell(needsDisplayCalculations, ABar, K3);

            //AB0-AB1-MBAM2-ABar
            AddSpell(needsDisplayCalculations, AB0, K4);
            AddSpell(needsDisplayCalculations, AB1, K4);
            AddSpell(needsDisplayCalculations, MBAM2, K4);
            if (AB0.CastTime + AB1.CastTime + MBAM2.CastTime + ABar.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - ABar.CastTime - MBAM2.CastTime, K4);
            AddSpell(needsDisplayCalculations, ABar, K4);
            AddSpell(needsDisplayCalculations, MBAM, K4);

            Calculate();
        }
    }

    class AB3ABar : DynamicCycle
    {
        public AB3ABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            StaticCycle chain3;
            StaticCycle chain4;
            StaticCycle chain5;
            StaticCycle chain6;
            float K1, K2, K3, K4, K5, K6;
            Name = "AB3ABar";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
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
            chain1 = new StaticCycle(4);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AB2, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + ABar3.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - ABar3.CastTime);
            chain1.AddSpell(ABar3, castingState);
            chain1.Calculate(castingState);

            //AB0-AB1-AB2-ABar3-MBAM
            chain2 = new StaticCycle(5);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + ABar3.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - ABar3.CastTime);
            chain2.AddSpell(ABar3, castingState);
            chain2.AddSpell(MBAM, castingState);
            chain2.Calculate(castingState);

            //AB0-AB1-AB2-MBAM3-ABar
            chain3 = new StaticCycle(5);
            chain3.AddSpell(AB0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(MBAM3, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + MBAM3.CastTime + ABar.CastTime < 3.0) chain3.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - MBAM3.CastTime - ABar.CastTime);
            chain3.AddSpell(ABar, castingState);
            chain3.Calculate(castingState);

            //AB0-AB1-AB2-MBAM3-ABar-MBAM
            chain4 = new StaticCycle(6);
            chain4.AddSpell(AB0, castingState);
            chain4.AddSpell(AB1, castingState);
            chain4.AddSpell(AB2, castingState);
            chain4.AddSpell(MBAM3, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + MBAM3.CastTime + ABar.CastTime < 3.0) chain4.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - MBAM3.CastTime - ABar.CastTime);
            chain4.AddSpell(ABar, castingState);
            chain4.AddSpell(MBAM, castingState);
            chain4.Calculate(castingState);

            //AB0-AB1-MBAM2-ABar
            chain5 = new StaticCycle(4);
            chain5.AddSpell(AB0, castingState);
            chain5.AddSpell(AB1, castingState);
            chain5.AddSpell(MBAM2, castingState);
            if (AB0.CastTime + AB1.CastTime + MBAM2.CastTime + ABar.CastTime < 3.0) chain5.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - MBAM2.CastTime - ABar.CastTime);
            chain5.AddSpell(ABar, castingState);
            chain5.Calculate(castingState);

            //AB0-AB1-MBAM2-ABar-MBAM
            chain6 = new StaticCycle(4);
            chain6.AddSpell(AB0, castingState);
            chain6.AddSpell(AB1, castingState);
            chain6.AddSpell(MBAM2, castingState);
            if (AB0.CastTime + AB1.CastTime + MBAM2.CastTime + ABar.CastTime < 3.0) chain6.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - MBAM2.CastTime - ABar.CastTime);
            chain6.AddSpell(ABar, castingState);
            chain6.AddSpell(MBAM, castingState);
            chain6.Calculate(castingState);

            commonChain = chain1;

            AddCycle(needsDisplayCalculations, chain1, K1);
            AddCycle(needsDisplayCalculations, chain2, K2);
            AddCycle(needsDisplayCalculations, chain3, K3);
            AddCycle(needsDisplayCalculations, chain4, K4);
            AddCycle(needsDisplayCalculations, chain5, K5);
            AddCycle(needsDisplayCalculations, chain6, K6);
            Calculate();
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class AB3ABarX : DynamicCycle
    {
        public AB3ABarX(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            StaticCycle chain3;
            StaticCycle chain4;
            float K1, K2, K3, K4;
            Name = "AB3ABarX";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
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
            chain1 = new StaticCycle(4);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AB2, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + ABar3.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - ABar3.CastTime);
            chain1.AddSpell(ABar3, castingState);
            chain1.Calculate(castingState);

            //AB0-AB1-AB2-ABar3-MBAM
            chain2 = new StaticCycle(5);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + ABar3.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - ABar3.CastTime);
            chain2.AddSpell(ABar3, castingState);
            chain2.AddSpell(MBAM, castingState);
            chain2.Calculate(castingState);

            //AB0-AB1-AB2-MBAM3-ABar
            chain3 = new StaticCycle(5);
            chain3.AddSpell(AB0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(MBAM3, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + MBAM3.CastTime + ABar.CastTime < 3.0) chain3.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - MBAM3.CastTime - ABar.CastTime);
            chain3.AddSpell(ABar, castingState);
            chain3.Calculate(castingState);

            //AB0-AB1-AB2-MBAM3-ABar-MBAM
            chain4 = new StaticCycle(6);
            chain4.AddSpell(AB0, castingState);
            chain4.AddSpell(AB1, castingState);
            chain4.AddSpell(AB2, castingState);
            chain4.AddSpell(MBAM3, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + MBAM3.CastTime + ABar.CastTime < 3.0) chain4.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - MBAM3.CastTime - ABar.CastTime);
            chain4.AddSpell(ABar, castingState);
            chain4.AddSpell(MBAM, castingState);
            chain4.Calculate(castingState);

            commonChain = chain1;

            AddCycle(needsDisplayCalculations, chain1, K1);
            AddCycle(needsDisplayCalculations, chain2, K2);
            AddCycle(needsDisplayCalculations, chain3, K3);
            AddCycle(needsDisplayCalculations, chain4, K4);
            Calculate();
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class AB3ABarY : DynamicCycle
    {
        public AB3ABarY(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            StaticCycle chain3;
            StaticCycle chain5;
            float K1, K2, K3, K5;
            Name = "AB3ABarY";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
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
            chain1 = new StaticCycle(4);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AB2, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + ABar3.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - ABar3.CastTime);
            chain1.AddSpell(ABar3, castingState);
            chain1.Calculate(castingState);

            //AB0-AB1-AB2-ABar3-MBAM
            chain2 = new StaticCycle(5);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + ABar3.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - AB2.CastTime - ABar3.CastTime);
            chain2.AddSpell(ABar3, castingState);
            chain2.AddSpell(MBAM, castingState);
            chain2.Calculate(castingState);

            //AB0-AB1-AB2-MBAM3-ABar
            chain3 = new StaticCycle(5);
            chain3.AddSpell(AB0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(MBAM3, castingState);
            chain3.Calculate(castingState);

            //AB0-AB1-MBAM2-ABar
            chain5 = new StaticCycle(4);
            chain5.AddSpell(AB0, castingState);
            chain5.AddSpell(AB1, castingState);
            chain5.AddSpell(MBAM2, castingState);
            chain5.Calculate(castingState);

            commonChain = chain1;

            AddCycle(needsDisplayCalculations, chain1, K1);
            AddCycle(needsDisplayCalculations, chain2, K2);
            AddCycle(needsDisplayCalculations, chain3, K3);
            AddCycle(needsDisplayCalculations, chain5, K5);
            Calculate();
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class AB2ABar : DynamicCycle
    {
        public AB2ABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            float K;
            Name = "AB2ABar";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;

            {
                if (MB == 0.0f)
                {
                    Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                    // if we don't have barrage then this degenerates to AB-AB-ABar
                    chain1 = new StaticCycle(3);
                    chain1.AddSpell(AB0, castingState);
                    chain1.AddSpell(AB1, castingState);
                    if (AB3.CastTime + AB3.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB3.CastTime - AB3.CastTime - ABar.CastTime);
                    chain1.AddSpell(ABar2, castingState);
                    chain1.Calculate(castingState);

                    commonChain = chain1;

                    AddCycle(needsDisplayCalculations, chain1, 1);

                    CastTime = chain1.CastTime;
                    costPerSecond = chain1.costPerSecond;
                    damagePerSecond = chain1.damagePerSecond;
                    threatPerSecond = chain1.threatPerSecond;
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
                    chain1 = new StaticCycle(3);
                    chain1.AddSpell(AB0, castingState);
                    chain1.AddSpell(AB1, castingState);
                    if (AB0.CastTime + AB1.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - AB1.CastTime - ABar.CastTime);
                    chain1.AddSpell(ABar2, castingState);
                    chain1.Calculate(castingState);

                    //MBAM-AB0-ABar1
                    chain2 = new StaticCycle(3);
                    chain2.AddSpell(MBAM, castingState);
                    chain2.AddSpell(AB0, castingState);
                    if (MBAM.CastTime + AB0.CastTime + ABar1.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - AB0.CastTime - ABar1.CastTime);
                    chain2.AddSpell(ABar1, castingState);
                    chain2.Calculate(castingState);

                    commonChain = chain1;

                    AddCycle(needsDisplayCalculations, chain1, 1 - K);
                    AddCycle(needsDisplayCalculations, chain2, K);
                    Calculate();
                }
            }
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class FB2ABar : DynamicCycle
    {
        public FB2ABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            float K;
            Name = "FB2ABar";
            AffectedByFlameCap = true;

            Spell FB = castingState.GetSpell(SpellId.Fireball);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0f)
            {
                // if we don't have barrage then this degenerates to AB-AB-ABar
                chain1 = new StaticCycle(3);
                chain1.AddSpell(FB, castingState);
                chain1.AddSpell(FB, castingState);
                if (FB.CastTime + FB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FB.CastTime - FB.CastTime - ABar.CastTime);
                chain1.AddSpell(ABar, castingState);
                chain1.Calculate(castingState);

                commonChain = chain1;

                AddCycle(needsDisplayCalculations, chain1, 1);

                CastTime = chain1.CastTime;
                costPerSecond = chain1.costPerSecond;
                damagePerSecond = chain1.damagePerSecond;
                threatPerSecond = chain1.threatPerSecond;
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
                chain1 = new StaticCycle(3);
                chain1.AddSpell(ABar, castingState);
                chain1.AddSpell(FB, castingState);
                chain1.AddSpell(FB, castingState);
                if (FB.CastTime + FB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FB.CastTime - FB.CastTime - ABar.CastTime);
                chain1.Calculate(castingState);

                //ABar-MBAM
                chain2 = new StaticCycle(3);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                chain2.AddSpell(FB, castingState);
                if (FB.CastTime + MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - FB.CastTime - MBAM.CastTime - ABar.CastTime);
                chain2.Calculate(castingState);

                commonChain = chain1;

                K = 1 - (1 - MB) * (1 - MB) / (1 + (1 - MB) * (1 - MB) * MB);

                AddCycle(needsDisplayCalculations, chain1, 1 - K);
                AddCycle(needsDisplayCalculations, chain2, K);
                Calculate();
            }
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class FrB2ABar : DynamicCycle
    {
        public FrB2ABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            float K;
            Name = "FrB2ABar";

            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0f)
            {
                // if we don't have barrage then this degenerates to AB-AB-ABar
                chain1 = new StaticCycle(3);
                chain1.AddSpell(FrB, castingState);
                chain1.AddSpell(FrB, castingState);
                if (FrB.CastTime + FrB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FrB.CastTime - FrB.CastTime - ABar.CastTime);
                chain1.AddSpell(ABar, castingState);
                chain1.Calculate(castingState);

                commonChain = chain1;

                AddCycle(needsDisplayCalculations, chain1, 1);

                CastTime = chain1.CastTime;
                costPerSecond = chain1.costPerSecond;
                damagePerSecond = chain1.damagePerSecond;
                threatPerSecond = chain1.threatPerSecond;
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
                chain1 = new StaticCycle(3);
                chain1.AddSpell(ABar, castingState);
                chain1.AddSpell(FrB, castingState);
                chain1.AddSpell(FrB, castingState);
                if (FrB.CastTime + FrB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FrB.CastTime - FrB.CastTime - ABar.CastTime);
                chain1.Calculate(castingState);

                //ABar-MBAM
                chain2 = new StaticCycle(3);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                chain2.AddSpell(FrB, castingState);
                if (FrB.CastTime + MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - FrB.CastTime - MBAM.CastTime - ABar.CastTime);
                chain2.Calculate(castingState);

                commonChain = chain1;

                K = 1 - (1 - MB) * (1 - MB) / (1 + (1 - MB) * (1 - MB) * MB);

                AddCycle(needsDisplayCalculations, chain1, 1 - K);
                AddCycle(needsDisplayCalculations, chain2, K);
                Calculate();
            }
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class FBABar : DynamicCycle
    {
        public FBABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            float MB;
            Name = "FBABar";
            AffectedByFlameCap = true;

            Spell FB = castingState.GetSpell(SpellId.Fireball);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to FB-ABar
                chain1 = new StaticCycle(2);
                chain1.AddSpell(FB, castingState);
                if (FB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FB.CastTime - ABar.CastTime);
                chain1.AddSpell(ABar, castingState);
                chain1.Calculate(castingState);

                commonChain = chain1;

                AddCycle(needsDisplayCalculations, chain1, 1);

                CastTime = chain1.CastTime;
                costPerSecond = chain1.costPerSecond;
                damagePerSecond = chain1.damagePerSecond;
                threatPerSecond = chain1.threatPerSecond;
            }
            else
            {
                chain1 = new StaticCycle(2);
                chain1.AddSpell(ABar, castingState);
                chain1.AddSpell(FB, castingState);
                if (FB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FB.CastTime - ABar.CastTime);
                chain1.Calculate(castingState);

                chain2 = new StaticCycle(2);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                if (MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime);
                chain2.Calculate(castingState);

                commonChain = chain2;

                AddCycle(needsDisplayCalculations, chain1, 1 - MB);
                AddCycle(needsDisplayCalculations, chain2, (2 - MB) * MB);
                Calculate();
            }
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class FrBABar : DynamicCycle
    {
        public FrBABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            float MB;
            Name = "FrBABar";

            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to FrB-ABar
                chain1 = new StaticCycle(2);
                chain1.AddSpell(FrB, castingState);
                if (FrB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FrB.CastTime - ABar.CastTime);
                chain1.AddSpell(ABar, castingState);
                chain1.Calculate(castingState);

                commonChain = chain1;

                AddCycle(needsDisplayCalculations, chain1, 1);

                CastTime = chain1.CastTime;
                costPerSecond = chain1.costPerSecond;
                damagePerSecond = chain1.damagePerSecond;
                threatPerSecond = chain1.threatPerSecond;
            }
            else
            {
                chain1 = new StaticCycle(2);
                chain1.AddSpell(ABar, castingState);
                chain1.AddSpell(FrB, castingState);
                if (FrB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FrB.CastTime - ABar.CastTime);
                chain1.Calculate(castingState);

                chain2 = new StaticCycle(2);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                if (MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime);
                chain2.Calculate(castingState);

                commonChain = chain2;

                AddCycle(needsDisplayCalculations, chain1, 1 - MB);
                AddCycle(needsDisplayCalculations, chain2, (2 - MB) * MB);
                Calculate();
            }
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class FFBABar : DynamicCycle
    {
        public FFBABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            float MB;
            Name = "FFBABar";
            AffectedByFlameCap = true;

            Spell FFB = castingState.GetSpell(SpellId.FrostfireBolt);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to FB-ABar
                chain1 = new StaticCycle(2);
                chain1.AddSpell(FFB, castingState);
                if (FFB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FFB.CastTime - ABar.CastTime);
                chain1.AddSpell(ABar, castingState);
                chain1.Calculate(castingState);

                commonChain = chain1;

                AddCycle(needsDisplayCalculations, chain1, 1);

                CastTime = chain1.CastTime;
                costPerSecond = chain1.costPerSecond;
                damagePerSecond = chain1.damagePerSecond;
                threatPerSecond = chain1.threatPerSecond;
            }
            else
            {
                chain1 = new StaticCycle(2);
                chain1.AddSpell(ABar, castingState);
                chain1.AddSpell(FFB, castingState);
                if (FFB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.Latency - FFB.CastTime - ABar.CastTime);
                chain1.Calculate(castingState);

                chain2 = new StaticCycle(4);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                if (MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime);
                chain2.Calculate(castingState);

                commonChain = chain2;

                AddCycle(needsDisplayCalculations, chain1, 1 - MB);
                AddCycle(needsDisplayCalculations, chain2, (2 - MB) * MB);
                Calculate();
            }
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    /*class AB : Cycle
    {
        Spell AB3;
        StaticCycle chain1;
        StaticCycle chain3;
        StaticCycle chain4;
        Spell AB0M;
        float hit, k21, k31, k41;

        public AB(CastingState castingState) : base(castingState)
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

            AB3 = castingState.GetSpell(SpellId.ArcaneBlast33);
            hit = AB3.HitRate;

            if (AB3.HitRate >= 1.0f || 2 * AB3.CastTime < 3.0)
            {
                // if we have enough hit this is just AB3
                // if we have enough haste to get 2 ABs in 3 sec then assume we get to chain cast, can refine this if desired
                CastTime = AB3.CastTime;
                costPerSecond = AB3.CostPerSecond;
                damagePerSecond = AB3.DamagePerSecond;
                threatPerSecond = AB3.ThreatPerSecond;
            }
            else
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0Hit);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1Hit);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2Hit);
                AB3 = castingState.GetSpell(SpellId.ArcaneBlast3Hit);
                AB0M = castingState.GetSpell(SpellId.ArcaneBlast0Miss);
                Spell AB1M = castingState.GetSpell(SpellId.ArcaneBlast1Miss);
                Spell AB2M = castingState.GetSpell(SpellId.ArcaneBlast2Miss);
                Spell AB3M = castingState.GetSpell(SpellId.ArcaneBlast3Miss);

                // AB3 hit

                // AB3M-RAMP (1-hit)
                chain1 = new StaticCycle(4);
                chain1.AddSpell(AB3M, castingState);
                chain1.AddSpell(AB0, castingState);
                chain1.AddSpell(AB1, castingState);
                chain1.AddSpell(AB2, castingState);
                chain1.Calculate(castingState);

                chain3 = new StaticCycle(3);
                chain3.AddSpell(AB0, castingState);
                chain3.AddSpell(AB1, castingState);
                chain3.AddSpell(AB2M, castingState);
                chain3.Calculate(castingState);

                chain4 = new StaticCycle(2);
                chain4.AddSpell(AB0, castingState);
                chain4.AddSpell(AB1M, castingState);
                chain4.Calculate(castingState);

                k21 = (1 - hit) / hit;
                k31 = (1 - hit) / hit / hit;
                k41 = (1 - hit) / hit / hit / hit;

                CastTime = hit * AB3.CastTime + (1 - hit) * (chain1.CastTime + k21 * chain3.CastTime + k31 * chain4.CastTime + k41 * AB0M.CastTime);
                costPerSecond = (hit * AB3.CostPerSecond * AB3.CastTime + (1 - hit) * (chain1.costPerSecond * chain1.CastTime + k21 * chain3.costPerSecond * chain3.CastTime + k31 * chain4.costPerSecond * chain4.CastTime + k41 * AB0M.CostPerSecond * AB0M.CastTime)) / CastTime;
                damagePerSecond = (hit * AB3.DamagePerSecond * AB3.CastTime + (1 - hit) * (chain1.damagePerSecond * chain1.CastTime + k21 * chain3.damagePerSecond * chain3.CastTime + k31 * chain4.damagePerSecond * chain4.CastTime + k41 * AB0M.DamagePerSecond * AB0M.CastTime)) / CastTime;
                threatPerSecond = (hit * AB3.ThreatPerSecond * AB3.CastTime + (1 - hit) * (chain1.threatPerSecond * chain1.CastTime + k21 * chain3.threatPerSecond * chain3.CastTime + k31 * chain4.threatPerSecond * chain4.CastTime + k41 * AB0M.ThreatPerSecond * AB0M.CastTime)) / CastTime;
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

        public override void AddManaSourcesContribution(Dictionary<string, float> dict, float duration)
        {
            throw new NotImplementedException();
        }

        public override void AddManaUsageContribution(Dictionary<string, float> dict, float duration)
        {
            throw new NotImplementedException();
        }
    }*/

    class ABSpamMBAM : DynamicCycle
    {
        public ABSpamMBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Spell AB3;
            float MB, MB3, MB4, MB5, hit, miss;
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

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
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

                AddSpell(needsDisplayCalculations, AB3, 1);
                Calculate();
            }
            else
            {
                MB3 = MB / (1 - MB);
                MB4 = MB / (1 - MB) / (1 - MB);
                MB5 = MB / (1 - MB) / (1 - MB) / (1 - MB);

                //AB3 0.85

                //AB3-MBAM-RAMP 0.15
                AddSpell(needsDisplayCalculations, AB3, MB);
                AddSpell(needsDisplayCalculations, AB3, MB); // account for latency
                AddSpell(needsDisplayCalculations, MBAM3, MB);
                AddSpell(needsDisplayCalculations, AB0, MB);
                AddSpell(needsDisplayCalculations, AB1, MB);
                AddSpell(needsDisplayCalculations, AB2, MB);

                AddSpell(needsDisplayCalculations, AB0, MB * MB3);
                AddSpell(needsDisplayCalculations, AB1, MB * MB3);
                AddSpell(needsDisplayCalculations, AB2, MB * MB3);
                AddSpell(needsDisplayCalculations, AB3, MB * MB3); // account for latency
                AddSpell(needsDisplayCalculations, MBAM3, MB * MB3);

                AddSpell(needsDisplayCalculations, AB0, MB * MB4);
                AddSpell(needsDisplayCalculations, AB1, MB * MB4);
                AddSpell(needsDisplayCalculations, AB2, MB * MB4); // account for latency
                AddSpell(needsDisplayCalculations, MBAM3, MB * MB4);

                AddSpell(needsDisplayCalculations, AB0, MB * MB5);
                AddSpell(needsDisplayCalculations, AB1, MB * MB5); // account for latency
                AddSpell(needsDisplayCalculations, MBAM2, MB * MB5);

                AddSpell(needsDisplayCalculations, AB3, 1 - MB);

                Calculate();
            }
        }
    }

    class ABSpam3C : DynamicCycle
    {
        public ABSpam3C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB, K1, K2, K3, K4, K5, S0, S1;
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

            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            //Spell MBAM3C = castingState.GetSpell(SpellId.ArcaneMissilesMB3Clipped);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            //Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
            //Spell ABar3C = castingState.GetSpell(SpellId.ArcaneBarrage3Combo);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            S0 = MB / (MB + (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB));
            S1 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB) / (MB + (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB));
            K1 = S0 * (1 - (1 - MB) * (1 - MB) * (1 - MB));
            K2 = S0 * (1 - MB) * (1 - MB) * (1 - MB) * MB;
            K3 = S0 * (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            K4 = S1 * MB;
            K5 = S1 * (1 - MB);

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB3, K2 + 2 * K4 + K5);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.MageTalents, this, AB, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
            }
            AddSpell(needsDisplayCalculations, MBAM3, K1 + K2 + K4);
            AddSpell(needsDisplayCalculations, ABar, K1 + K2 + K4);

            Calculate();
        }
    }

    class ABSpam03C : DynamicCycle
    {
        public ABSpam03C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB, K1, K2, K3, K4, K5, K6, S0, S1;
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

            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            //Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
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

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB3, K2 + 2 * K4 + K5);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.MageTalents, this, AB, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
            }
            AddSpell(needsDisplayCalculations, MBAM, K6);
            if (MBAM.CastTime + ABar.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - MBAM.CastTime - ABar.CastTime, K6);
            AddSpell(needsDisplayCalculations, MBAM3, K1 + K2 + K4);
            AddSpell(needsDisplayCalculations, ABar, K1 + K2 + K4 + K6);

            Calculate();
        }
    }

    class ABSpam3MBAM : DynamicCycle
    {
        public ABSpam3MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB, K1, K2, K3, K4, K5, S0, S1;
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

            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            S0 = MB / (MB + (1 - MB) * (1 - MB) * (1 - MB));
            S1 = (1 - MB) * (1 - MB) * (1 - MB) / (MB + (1 - MB) * (1 - MB) * (1 - MB));
            K1 = S0 * (1 - (1 - MB) * (1 - MB));
            K2 = S0 * (1 - MB) * (1 - MB) * MB;
            K3 = S0 * (1 - MB) * (1 - MB) * (1 - MB);
            K4 = S1 * MB;
            K5 = S1 * (1 - MB);

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB3, K2 + 2 * K4 + K5);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.MageTalents, this, AB, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
            }
            AddSpell(needsDisplayCalculations, MBAM3, K1 + K2 + K4);

            Calculate();
        }
    }

    class AB2ABar2C : DynamicCycle
    {
        public AB2ABar2C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB, K1, K2;
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

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
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

            AddSpell(needsDisplayCalculations, AB0, K1 + K2);
            AddSpell(needsDisplayCalculations, AB1, K1 + K2);
            AddSpell(needsDisplayCalculations, ABar2, K1);
            AddSpell(needsDisplayCalculations, MBAM2, K2);
            AddSpell(needsDisplayCalculations, ABar, K2);

            Calculate();
        }
    }

    class AB2ABar2MBAM : DynamicCycle
    {
        public AB2ABar2MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB, K1, K2;
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

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float S0 = 1 / (1 + (1 - MB) * (1 - (1 - MB) * (1 - MB)));
            float S1 = 1 - S0;
            K1 = S0 * (1 - MB);
            K2 = S0 * MB + S1;

            AddSpell(needsDisplayCalculations, AB0, K1 + K2);
            AddSpell(needsDisplayCalculations, AB1, K1 + K2);
            AddSpell(needsDisplayCalculations, ABar2, K1);
            AddSpell(needsDisplayCalculations, MBAM2, K2);

            Calculate();
        }
    }

    class AB2ABar3C : DynamicCycle
    {
        public AB2ABar3C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB, K1, K2;
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

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float S0 = (1 - MB) / ((1 - MB) * (1 - (1 - MB) * (1 - MB)) + MB * MB + 1 - MB);
            float S1 = 1 - S0;
            K1 = S0 * (1 - MB);
            K2 = S0 * MB + S1;

            AddSpell(needsDisplayCalculations, AB0, K1 + K2);
            AddSpell(needsDisplayCalculations, AB1, K1 + K2);
            AddSpell(needsDisplayCalculations, ABar2, K1);
            AddSpell(needsDisplayCalculations, AB2, K2);
            AddSpell(needsDisplayCalculations, MBAM3, K2);
            AddSpell(needsDisplayCalculations, ABar, K2);

            Calculate();
        }
    }

    class ABABar3C : DynamicCycle
    {
        public ABABar3C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB, K1, K2;
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

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float S0 = (1 - MB) / (2 - MB - (1 - MB) * (1 - MB));
            float S1 = 1 - S0;
            K1 = S0;
            K2 = S1;

            AddSpell(needsDisplayCalculations, AB0, K1 + K2);
            if (AB0.CastTime + ABar1.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - ABar1.CastTime, K1);
            AddSpell(needsDisplayCalculations, ABar1, K1);
            AddSpell(needsDisplayCalculations, AB1, K2);
            AddSpell(needsDisplayCalculations, AB2, K2);
            AddSpell(needsDisplayCalculations, MBAM3, K2);
            AddSpell(needsDisplayCalculations, ABar, K2);

            Calculate();
        }
    }

    class ABABar2C : DynamicCycle
    {
        public ABABar2C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB, K1, K2;
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

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float S0 = (1 - MB) / (2 - MB - (1 - MB) * (1 - MB));
            float S1 = 1 - S0;
            K1 = S0;
            K2 = S1;

            AddSpell(needsDisplayCalculations, AB0, K1 + K2);
            if (AB0.CastTime + ABar1.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - ABar1.CastTime, K1);
            AddSpell(needsDisplayCalculations, ABar1, K1);
            AddSpell(needsDisplayCalculations, AB1, K2);
            AddSpell(needsDisplayCalculations, MBAM2, K2);
            AddSpell(needsDisplayCalculations, ABar, K2);

            Calculate();
        }
    }

    class ABABar2MBAM : DynamicCycle
    {
        public ABABar2MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB, K1, K2;
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

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float S0 = 1 / (2 - (1 - MB) * (1 - MB));
            float S1 = 1 - S0;
            K1 = S0;
            K2 = S1;

            AddSpell(needsDisplayCalculations, AB0, K1 + K2);
            if (AB0.CastTime + ABar1.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - ABar1.CastTime, K1);
            AddSpell(needsDisplayCalculations, ABar1, K1);
            AddSpell(needsDisplayCalculations, AB1, K2);
            AddSpell(needsDisplayCalculations, MBAM2, K2);

            Calculate();
        }
    }

    class ABABar1MBAM : DynamicCycle
    {
        public ABABar1MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB, K1, K2;
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

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell MBAM1 = castingState.GetSpell(SpellId.ArcaneMissilesMB1);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float S0 = 1 / (2 - (1 - MB) * (1 - MB));
            float S1 = 1 - S0;
            K1 = S0;
            K2 = S1;

            AddSpell(needsDisplayCalculations, AB0, K1 + K2);
            if (AB0.CastTime + ABar1.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - AB0.CastTime - ABar1.CastTime, K1);
            AddSpell(needsDisplayCalculations, ABar1, K1);
            AddSpell(needsDisplayCalculations, MBAM1, K2);

            Calculate();
        }
    }

    class AB3ABar3C : DynamicCycle
    {
        public AB3ABar3C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB, K1, K2;
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

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                AddSpell(needsDisplayCalculations, AB0, K1 + K2);
                AddSpell(needsDisplayCalculations, AB1, K1 + K2);
                AddSpell(needsDisplayCalculations, AB2, K1 + K2);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.MageTalents, this, AB, K1 + K2, K1 + K2, K1 + K2, 0);
            }
            AddSpell(needsDisplayCalculations, ABar3, K1);
            AddSpell(needsDisplayCalculations, MBAM3, K2);
            AddSpell(needsDisplayCalculations, ABar, K2);

            Calculate();
        }
    }

    class AB3ABar3MBAM : DynamicCycle
    {
        public AB3ABar3MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB, K1, K2;
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

            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float S0 = 1 / (1 + (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB)));
            float S1 = 1 - S0;
            K1 = S0 * (1 - MB) * (1 - MB);
            K2 = S0 * (1 - (1 - MB) * (1 - MB)) + S1;

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                AddSpell(needsDisplayCalculations, AB0, K1 + K2);
                AddSpell(needsDisplayCalculations, AB1, K1 + K2);
                AddSpell(needsDisplayCalculations, AB2, K1 + K2);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.MageTalents, this, AB, K1 + K2, K1 + K2, K1 + K2, 0);
            }
            AddSpell(needsDisplayCalculations, ABar3, K1);
            AddSpell(needsDisplayCalculations, MBAM3, K2);

            Calculate();
        }
    }

    class AB32AMABar : DynamicCycle
    {
        public AB32AMABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            StaticCycle chain3;
            float MB, K1, K2, K3;
            Name = "AB32AMABar";

            // ABar-AB0-AB1-MBAM       1 - (1-MB)*(1-MB)
            // ABar-AB0-AB1-AB2-AM     (1-MB)*(1-MB)*(1-MB)*(1-MB)
            // ABar-AB0-AB1-AB2-MBAM   (1-MB)*(1-MB)*(1 - (1-MB)*(1-MB))

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
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

            chain1 = new StaticCycle(5);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(MBAM2, castingState);
            chain1.AddSpell(ABar, castingState);
            chain1.Calculate(castingState);

            chain2 = new StaticCycle(5);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            chain2.AddSpell(AM3, castingState);
            chain2.AddSpell(ABar, castingState);
            chain2.Calculate(castingState);

            chain3 = new StaticCycle(6);
            chain3.AddSpell(AB0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(MBAM3, castingState);
            chain3.AddSpell(ABar, castingState);
            chain3.Calculate(castingState);

            AddCycle(needsDisplayCalculations, chain1, K1);
            AddCycle(needsDisplayCalculations, chain2, K2);
            AddCycle(needsDisplayCalculations, chain3, K3);
            Calculate();
        }
    }

    class AB2AMABar : DynamicCycle
    {
        public AB2AMABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            float MB, K1, K2;
            Name = "AB2AMABar";

            // ABar-AB0-AB1-AM     (1-MB)*(1-MB)*(1-MB)
            // ABar-AB0-AB1-MBAM   1 - (1-MB)*(1-MB)*(1-MB)

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
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

            chain1 = new StaticCycle(5);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AM2, castingState);
            chain1.AddSpell(ABar, castingState);
            chain1.Calculate(castingState);

            chain2 = new StaticCycle(6);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(MBAM2, castingState);
            chain2.AddSpell(ABar, castingState);
            chain2.Calculate(castingState);

            AddCycle(needsDisplayCalculations, chain1, K1);
            AddCycle(needsDisplayCalculations, chain2, K2);
            Calculate();
        }
    }

    class ABAMABar : DynamicCycle
    {
        public ABAMABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            float MB, K1, K2;
            Name = "ABAMABar";

            // ABar-AB0-AM     (1-MB)*(1-MB)
            // ABar-AB0-MBAM   1 - (1-MB)*(1-MB)

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
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

            chain1 = new StaticCycle(5);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AM1, castingState);
            chain1.AddSpell(ABar, castingState);
            chain1.Calculate(castingState);

            chain2 = new StaticCycle(6);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(MBAM1, castingState);
            chain2.AddSpell(ABar, castingState);
            chain2.Calculate(castingState);

            AddCycle(needsDisplayCalculations, chain1, K1);
            AddCycle(needsDisplayCalculations, chain2, K2);
            Calculate();
        }
    }

    /*class AB3AMSc : StaticCycle
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
    }*/

    class ABABar : StaticCycle
    {
        public ABABar(CastingState castingState)
            : base(3)
        {
            Name = "ABABar";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);

            AddSpell(AB, castingState);
            if (AB.CastTime + ABar1.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.Latency - AB.CastTime - ABar1.CastTime);
            AddSpell(ABar1, castingState);

            Calculate(castingState);
        }
    }

    /*class ABAM3Sc : StaticCycle
    {
        public ABAM3Sc(CastingState castingState) : base(14)
        {
            Name = "ABAM3Sc";

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast2);
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

    class ABAM3Sc2 : StaticCycle
    {
        public ABAM3Sc2(CastingState castingState) : base(14)
        {
            Name = "ABAM3Sc2";

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast2);
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

    class ABAM3FrB : StaticCycle
    {
        public ABAM3FrB(CastingState castingState) : base(14)
        {
            Name = "ABAM3FrB";

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast2);
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

    class ABAM3FrB2 : StaticCycle
    {
        public ABAM3FrB2(CastingState castingState) : base(14)
        {
            Name = "ABAM3FrB2";

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast2);
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

    class AB3FrB : StaticCycle
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

    class ABFrB : StaticCycle
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

    class ABFrB3FrB : StaticCycle
    {
        public ABFrB3FrB(CastingState castingState) : base(13)
        {
            Name = "ABFrB3FrB";

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast2);
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

    class ABFrB3FrB2 : StaticCycle
    {
        public ABFrB3FrB2(CastingState castingState) : base(13)
        {
            Name = "ABFrB3FrB2";

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast2);
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

    class ABFrB3FrBSc : StaticCycle
    {
        public ABFrB3FrBSc(CastingState castingState) : base(13)
        {
            Name = "ABFrB3FrBSc";

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast2);
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

    class ABFB3FBSc : StaticCycle
    {
        public ABFB3FBSc(CastingState castingState) : base(13)
        {
            Name = "ABFB3FBSc";
            AffectedByFlameCap = true;

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast2);
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

    class AB3Sc : StaticCycle
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
    }*/

    class FBPyro : DynamicCycle
    {
        public FBPyro(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Spell FB;
            StaticCycle chain2;
            float K;
            Name = "FBPyro";
            AffectedByFlameCap = true;

            FB = castingState.GetSpell(SpellId.Fireball);
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
            chain2 = new StaticCycle(2);
            chain2.AddSpell(FB, castingState);
            chain2.AddSpell(Pyro, castingState);
            chain2.Calculate(castingState);

            K = FB.CritRate * FB.CritRate / (1.0f + FB.CritRate) * castingState.MageTalents.HotStreak / 3.0f;
            if (castingState.MageTalents.Pyroblast == 0) K = 0.0f;

            AddSpell(needsDisplayCalculations, FB, 1 - K);
            AddCycle(needsDisplayCalculations, chain2, K);
            Calculate();
        }
    }

    class FrBFB : DynamicCycle
    {
        public FrBFB(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Spell FrB;
            StaticCycle chain2;
            float K;
            Name = "FrBFB";

            FrB = castingState.GetSpell(SpellId.FrostboltFOF);
            Spell FB = castingState.GetSpell(SpellId.FireballBF);
            sequence = "Frostbolt";

            // FrB      1 - brainFreeze
            // FrB-FB   brainFreeze

            chain2 = new StaticCycle(2);
            chain2.AddSpell(FrB, castingState);
            chain2.AddSpell(FB, castingState);
            chain2.Calculate(castingState);

            K = 0.05f * castingState.MageTalents.BrainFreeze;

            AddSpell(needsDisplayCalculations, FrB, 1 - K);
            AddCycle(needsDisplayCalculations, chain2, K);
            Calculate();
        }
    }

    class FrBFBIL : DynamicCycle
    {
        public FrBFBIL(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Spell FrB, FrBS, FB, FBS, ILS;
            float KFrB, KFrBS, KFB, KFBS, KILS;
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

            AddSpell(needsDisplayCalculations, FrB, KFrB);
            AddSpell(needsDisplayCalculations, FB, KFB);
            AddSpell(needsDisplayCalculations, FrBS, KFrBS);
            AddSpell(needsDisplayCalculations, FBS, KFBS);
            AddSpell(needsDisplayCalculations, ILS, KILS);
            Calculate();
        }
    }

    class FBLBPyro : DynamicCycle
    {
        public FBLBPyro(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Spell FB;
            Spell LB;
            Spell Pyro;
            float X;
            float K;
            Name = "FBLBPyro";
            AffectedByFlameCap = true;

            FB = castingState.GetSpell(SpellId.Fireball);
            LB = castingState.GetSpell(SpellId.LivingBomb);
            Pyro = castingState.GetSpell(SpellId.PyroblastPOM);
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

            AddSpell(needsDisplayCalculations, FB, X);
            AddSpell(needsDisplayCalculations, LB, 1 - X);
            AddSpell(needsDisplayCalculations, Pyro, K);
            Calculate();
        }
    }

    class FFBLBPyro : DynamicCycle
    {
        public FFBLBPyro(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Spell FFB;
            Spell LB;
            Spell Pyro;
            float X;
            float K;
            Name = "FFBLBPyro";
            AffectedByFlameCap = true;

            FFB = castingState.GetSpell(SpellId.FrostfireBolt);
            LB = castingState.GetSpell(SpellId.LivingBomb);
            Pyro = castingState.GetSpell(SpellId.PyroblastPOM);
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

            AddSpell(needsDisplayCalculations, FFB, X);
            AddSpell(needsDisplayCalculations, LB, 1 - X);
            AddSpell(needsDisplayCalculations, Pyro, K);
            Calculate();
        }
    }

    class ScLBPyro : DynamicCycle
    {
        public ScLBPyro(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Spell Sc;
            Spell LB;
            Spell Pyro;
            float X;
            float K;
            Name = "ScLBPyro";
            ProvidesScorch = (castingState.MageTalents.ImprovedScorch > 0);
            AffectedByFlameCap = true;

            Sc = castingState.GetSpell(SpellId.Scorch);
            LB = castingState.GetSpell(SpellId.LivingBomb);
            Pyro = castingState.GetSpell(SpellId.PyroblastPOM);
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

            AddSpell(needsDisplayCalculations, Sc, X);
            AddSpell(needsDisplayCalculations, LB, 1 - X);
            AddSpell(needsDisplayCalculations, Pyro, K);
            Calculate();
        }
    }

    class FFBPyro : DynamicCycle
    {
        public FFBPyro(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Spell FFB;
            StaticCycle chain2;
            float K;
            Name = "FFBPyro";
            AffectedByFlameCap = true;

            FFB = castingState.GetSpell(SpellId.FrostfireBoltFOF);
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
            chain2 = new StaticCycle(2);
            chain2.AddSpell(FFB, castingState);
            chain2.AddSpell(Pyro, castingState);
            chain2.Calculate(castingState);

            K = FFB.CritRate * FFB.CritRate / (1.0f + FFB.CritRate) * castingState.MageTalents.HotStreak / 3.0f;
            if (castingState.MageTalents.Pyroblast == 0) K = 0.0f;

            AddSpell(needsDisplayCalculations, FFB, 1 - K);
            AddCycle(needsDisplayCalculations, chain2, K);
            Calculate();
        }
    }

    class FBScPyro : DynamicCycle
    {
        public FBScPyro(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Spell FB;
            Spell Sc;
            Spell Pyro;
            float K;
            float X;
            Name = "FBScPyro";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            FB = castingState.GetSpell(SpellId.Fireball);
            Sc = castingState.GetSpell(SpellId.Scorch);
            Pyro = castingState.GetSpell(SpellId.PyroblastPOM);
            sequence = "Fireball";

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.MageTalents.GlyphOfImprovedScorch)
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

            AddSpell(needsDisplayCalculations, FB, X);
            AddSpell(needsDisplayCalculations, Sc, 1 - X);
            AddSpell(needsDisplayCalculations, Pyro, K);
            Calculate();
        }
    }

    class FBScLBPyro : DynamicCycle
    {
        public FBScLBPyro(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Spell FB;
            Spell Sc;
            Spell LB;
            Spell Pyro;
            float K;
            float X;
            float Y;
            Name = "FBScLBPyro";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            FB = castingState.GetSpell(SpellId.Fireball);
            Sc = castingState.GetSpell(SpellId.Scorch);
            LB = castingState.GetSpell(SpellId.LivingBomb);
            Pyro = castingState.GetSpell(SpellId.PyroblastPOM);
            sequence = "Fireball";

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.MageTalents.GlyphOfImprovedScorch)
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

            AddSpell(needsDisplayCalculations, FB, X);
            AddSpell(needsDisplayCalculations, Sc, Y);
            AddSpell(needsDisplayCalculations, LB, 1 - X - Y);
            AddSpell(needsDisplayCalculations, Pyro, K);
            Calculate();
        }
    }

    class FFBScLBPyro : DynamicCycle
    {
        public FFBScLBPyro(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Spell FFB;
            Spell Sc;
            Spell LB;
            Spell Pyro;
            float K;
            float X;
            float Y;
            Name = "FFBScLBPyro";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            FFB = castingState.GetSpell(SpellId.FrostfireBolt);
            Sc = castingState.GetSpell(SpellId.Scorch);
            LB = castingState.GetSpell(SpellId.LivingBomb);
            Pyro = castingState.GetSpell(SpellId.PyroblastPOM);
            sequence = "Frostfire Bolt";

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.MageTalents.GlyphOfImprovedScorch)
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

            AddSpell(needsDisplayCalculations, FFB, X);
            AddSpell(needsDisplayCalculations, Sc, Y);
            AddSpell(needsDisplayCalculations, LB, 1 - X - Y);
            AddSpell(needsDisplayCalculations, Pyro, K);
            Calculate();
        }
    }

    class FFBScPyro : DynamicCycle
    {
        public FFBScPyro(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Spell FFB;
            Spell Sc;
            Spell Pyro;
            float K;
            float X;
            Name = "FFBScPyro";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            FFB = castingState.GetSpell(SpellId.FrostfireBoltFOF);
            Sc = castingState.GetSpell(SpellId.Scorch);
            Pyro = castingState.GetSpell(SpellId.PyroblastPOM);
            sequence = "Frostfire Bolt";

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.MageTalents.GlyphOfImprovedScorch)
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

            AddSpell(needsDisplayCalculations, FFB, X);
            AddSpell(needsDisplayCalculations, Sc, 1 - X);
            AddSpell(needsDisplayCalculations, Pyro, K);
            Calculate();
        }
    }

    class ABABarSc : DynamicCycle
    {
        public ABABarSc(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Cycle ABABar;
            Spell Sc;
            float X;
            Name = "ABABarSc";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            ABABar = castingState.GetCycle(CycleId.ABABar0C);
            Sc = castingState.GetSpell(SpellId.Scorch);
            sequence = ABABar.Sequence;

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.MageTalents.GlyphOfImprovedScorch)
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

            AddCycle(needsDisplayCalculations, ABABar, X);
            AddSpell(needsDisplayCalculations, Sc, 1 - X);
            Calculate();
        }
    }

    class ABABarCSc : DynamicCycle
    {
        public ABABarCSc(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Cycle ABABarC;
            Spell Sc;
            float X;
            Name = "ABABarCSc";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            ABABarC = castingState.GetCycle(CycleId.ABABar1C);
            Sc = castingState.GetSpell(SpellId.Scorch);
            sequence = ABABarC.Sequence;

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.MageTalents.GlyphOfImprovedScorch)
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

            AddCycle(needsDisplayCalculations, ABABarC, X);
            AddSpell(needsDisplayCalculations, Sc, 1 - X);
            Calculate();
        }
    }

    class ABAMABarSc : DynamicCycle
    {
        public ABAMABarSc(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Cycle ABAMABar;
            Spell Sc;
            float X;
            Name = "ABAMABarSc";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            ABAMABar = castingState.GetCycle(CycleId.ABAMABar);
            Sc = castingState.GetSpell(SpellId.Scorch);
            sequence = ABAMABar.Sequence;

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.MageTalents.GlyphOfImprovedScorch)
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

            AddCycle(needsDisplayCalculations, ABAMABar, X);
            AddSpell(needsDisplayCalculations, Sc, 1 - X);
            Calculate();
        }
    }

    class AB3AMABarSc : DynamicCycle
    {
        public AB3AMABarSc(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Cycle AB3AMABar;
            Spell Sc;
            float X;
            Name = "AB3AMABarSc";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            AB3AMABar = castingState.GetCycle(CycleId.AB3AMABar);
            Sc = castingState.GetSpell(SpellId.Scorch);
            sequence = AB3AMABar.Sequence;

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.MageTalents.GlyphOfImprovedScorch)
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

            AddCycle(needsDisplayCalculations, AB3AMABar, X);
            AddSpell(needsDisplayCalculations, Sc, 1 - X);
            Calculate();
        }
    }

    class AB3ABarCSc : DynamicCycle
    {
        public AB3ABarCSc(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Cycle AB3ABarC;
            Spell Sc;
            float X;
            Name = "AB3ABarCSc";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            AB3ABarC = castingState.GetCycle(CycleId.AB3ABar3C);
            Sc = castingState.GetSpell(SpellId.Scorch);
            sequence = AB3ABarC.Sequence;

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.MageTalents.GlyphOfImprovedScorch)
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

            AddCycle(needsDisplayCalculations, AB3ABarC, X);
            AddSpell(needsDisplayCalculations, Sc, 1 - X);
            Calculate();
        }
    }

    class AB3MBAMABarSc : DynamicCycle
    {
        public AB3MBAMABarSc(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Cycle AB3MBAMABar;
            Spell Sc;
            float X;
            Name = "AB3MBAMABarSc";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            AB3MBAMABar = castingState.GetCycle(CycleId.ABSpam3C);
            Sc = castingState.GetSpell(SpellId.Scorch);
            sequence = AB3MBAMABar.Sequence;

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.MageTalents.GlyphOfImprovedScorch)
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

            AddCycle(needsDisplayCalculations, AB3MBAMABar, X);
            AddSpell(needsDisplayCalculations, Sc, 1 - X);
            Calculate();
        }
    }

    class FBSc : StaticCycle
    {
        public FBSc(CastingState castingState)
            : base(33)
        {
            Name = "FBSc";

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
                ProvidesScorch = true;
                int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
                int extraScorches = 1;
                if (Sc.HitRate >= 1.0) extraScorches = 0;
                if (castingState.MageTalents.GlyphOfImprovedScorch)
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

    class FBFBlast : StaticCycle
    {
        public FBFBlast(CastingState castingState)
            : base(33)
        {
            Name = "FBFBlast";

            Spell FB = castingState.GetSpell(SpellId.Fireball);
            Spell Blast = castingState.GetSpell(SpellId.FireBlast);
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
                if (castingState.MageTalents.GlyphOfImprovedScorch)
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

    /*class ABAM3ScCCAM : DynamicCycle
    {
        StaticCycle chain1;
        StaticCycle chain2;
        StaticCycle chain3;
        StaticCycle chain4;
        float CC;

        public ABAM3ScCCAM(CastingState castingState) : base(4)
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
            Spell AMCC = castingState.GetCycle(SpellId.ArcaneMissilesCC);
            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11NoCC);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22NoCC);
            Spell Sc0 = castingState.GetSpell(SpellId.ScorchNoCC);

            Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

            //AMCC-AB0                       0.1
            chain1 = new StaticCycle(2);
            chain1.AddSpell(AMCC, castingState);
            chain1.AddSpell(AB0, castingState);
            chain1.Calculate(castingState);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            chain2 = new StaticCycle(4);
            chain2.AddSpell(AMc0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AMCC, castingState);
            chain2.AddSpell(AB0, castingState);
            chain2.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            chain3 = new StaticCycle(6);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(AMCC, castingState);
            chain3.AddSpell(AB0, castingState);
            chain3.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            chain4 = new StaticCycle(13);
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

            Cycle[0] = chain1;
            Cycle[1] = chain2;
            Cycle[2] = chain3;
            Cycle[3] = chain4;
            Weight[0] = CC;
            Weight[1] = CC * (1 - CC);
            Weight[2] = CC * (1 - CC) * (1 - CC);
            Weight[3] = (1 - CC) * (1 - CC) * (1 - CC);
            Calculate();

            commonChain = chain4;
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABAM3Sc2CCAM : DynamicCycle
    {
        StaticCycle chain1;
        StaticCycle chain2;
        StaticCycle chain3;
        StaticCycle chain4;
        float CC;

        public ABAM3Sc2CCAM(CastingState castingState) : base(4)
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
            Spell AMCC = castingState.GetCycle(SpellId.ArcaneMissilesCC);
            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11NoCC);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22NoCC);
            Spell Sc0 = castingState.GetSpell(SpellId.ScorchNoCC);

            Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

            //AMCC-AB0                       0.1
            chain1 = new StaticCycle(2);
            chain1.AddSpell(AMCC, castingState);
            chain1.AddSpell(AB0, castingState);
            chain1.Calculate(castingState);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            chain2 = new StaticCycle(4);
            chain2.AddSpell(AMc0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AMCC, castingState);
            chain2.AddSpell(AB0, castingState);
            chain2.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            chain3 = new StaticCycle(13);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(AMCC, castingState);
            chain3.AddSpell(AB0, castingState);
            chain3.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            chain4 = new StaticCycle();
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

            Cycle[0] = chain1;
            Cycle[1] = chain2;
            Cycle[2] = chain3;
            Cycle[3] = chain4;
            Weight[0] = CC;
            Weight[1] = CC * (1 - CC);
            Weight[2] = CC * (1 - CC) * (1 - CC);
            Weight[3] = (1 - CC) * (1 - CC) * (1 - CC);
            Calculate();

            commonChain = chain4;
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABAM3FrBCCAM : DynamicCycle
    {
        StaticCycle chain1;
        StaticCycle chain2;
        StaticCycle chain3;
        StaticCycle chain4;
        float CC;

        public ABAM3FrBCCAM(CastingState castingState) : base(4)
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
            Spell AMCC = castingState.GetCycle(SpellId.ArcaneMissilesCC);
            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11NoCC);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22NoCC);
            Spell FrB0 = castingState.GetSpell(SpellId.FrostboltNoCC);

            Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);

            CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

            //AMCC-AB0                       0.1
            chain1 = new StaticCycle(2);
            chain1.AddSpell(AMCC, castingState);
            chain1.AddSpell(AB0, castingState);
            chain1.Calculate(castingState);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            chain2 = new StaticCycle(4);
            chain2.AddSpell(AMc0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AMCC, castingState);
            chain2.AddSpell(AB0, castingState);
            chain2.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            chain3 = new StaticCycle(6);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(AMCC, castingState);
            chain3.AddSpell(AB0, castingState);
            chain3.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            chain4 = new StaticCycle(13);
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

            Cycle[0] = chain1;
            Cycle[1] = chain2;
            Cycle[2] = chain3;
            Cycle[3] = chain4;
            Weight[0] = CC;
            Weight[1] = CC * (1 - CC);
            Weight[2] = CC * (1 - CC) * (1 - CC);
            Weight[3] = (1 - CC) * (1 - CC) * (1 - CC);
            Calculate();

            commonChain = chain4;
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABAM3FrBCCAMFail : DynamicCycle
    {
        StaticCycle chain1;
        StaticCycle chain2;
        StaticCycle chain3;
        StaticCycle chain4;
        float CC;

        public ABAM3FrBCCAMFail(CastingState castingState) : base(4)
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
            Spell AMCC = castingState.GetCycle(SpellId.ArcaneMissilesCC);
            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11NoCC);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22NoCC);
            Spell FrB0 = castingState.GetSpell(SpellId.FrostboltNoCC);

            Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast00);
            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);

            CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

            //AMCC-AB0                       0.1
            chain1 = new StaticCycle(2);
            chain1.AddSpell(AMCC, castingState);
            chain1.AddSpell(AB0, castingState);
            chain1.Calculate(castingState);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            chain2 = new StaticCycle(4);
            chain2.AddSpell(AMc0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AMCC, castingState);
            chain2.AddSpell(AB0, castingState);
            chain2.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            chain3 = new StaticCycle(6);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(AMCC, castingState);
            chain3.AddSpell(AB0, castingState);
            chain3.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            chain4 = new StaticCycle(13);
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

            Cycle[0] = chain1;
            Cycle[1] = chain2;
            Cycle[2] = chain3;
            Cycle[3] = chain4;
            Weight[0] = CC;
            Weight[1] = CC * (1 - CC);
            Weight[2] = CC * (1 - CC) * (1 - CC);
            Weight[3] = (1 - CC) * (1 - CC) * (1 - CC);
            Calculate();

            commonChain = chain4;
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABAM3FrBScCCAM : DynamicCycle
    {
        StaticCycle chain1;
        StaticCycle chain2;
        StaticCycle chain3;
        StaticCycle chain4;
        float CC;

        public ABAM3FrBScCCAM(CastingState castingState) : base(4)
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
            Spell AMCC = castingState.GetCycle(SpellId.ArcaneMissilesCC);
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
            chain1 = new StaticCycle(2);
            chain1.AddSpell(AMCC, castingState);
            chain1.AddSpell(AB0, castingState);
            chain1.Calculate(castingState);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            chain2 = new StaticCycle(4);
            chain2.AddSpell(AMc0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AMCC, castingState);
            chain2.AddSpell(AB0, castingState);
            chain2.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            chain3 = new StaticCycle(6);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AMc0, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(AMCC, castingState);
            chain3.AddSpell(AB0, castingState);
            chain3.Calculate(castingState);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            chain4 = new StaticCycle(13);
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

            Cycle[0] = chain1;
            Cycle[1] = chain2;
            Cycle[2] = chain3;
            Cycle[3] = chain4;
            Weight[0] = CC;
            Weight[1] = CC * (1 - CC);
            Weight[2] = CC * (1 - CC) * (1 - CC);
            Weight[3] = (1 - CC) * (1 - CC) * (1 - CC);
            Calculate();

            commonChain = chain4;
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABAMCCAM : DynamicCycle
    {
        StaticCycle chain1;
        StaticCycle chain2;
        float CC;

        public ABAMCCAM(CastingState castingState) : base(2)
        {
            Name = "ABAMCC";

            //AMCC-AB00-AB01-AB12-AB23       0.1
            //AM?0-AB33                      0.9

            Spell AMc0 = castingState.GetSpell(SpellId.ArcaneMissilesNoProc);
            Spell AMCC = castingState.GetCycle(SpellId.ArcaneMissilesCC);
            Spell AB00 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB01 = castingState.GetSpell(SpellId.ArcaneBlast01);
            Spell AB12 = castingState.GetSpell(SpellId.ArcaneBlast12);
            Spell AB23 = castingState.GetSpell(SpellId.ArcaneBlast23);
            Spell AB33 = castingState.GetSpell(SpellId.ArcaneBlast33NoCC);

            CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

            if (CC == 0)
            {
                // if we don't have clearcasting then this degenerates to AMc0-AB33
                chain1 = new StaticCycle(2);
                chain1.AddSpell(AMc0, castingState);
                chain1.AddSpell(AB33, castingState);
                chain1.Calculate(castingState);

                Cycle[0] = chain1;
                Weight[0] = 1;

                CastTime = chain1.CastTime;
                CostPerSecond = chain1.CostPerSecond;
                DamagePerSecond = chain1.DamagePerSecond;
                ThreatPerSecond = chain1.ThreatPerSecond;

                commonChain = chain1;
            }
            else
            {

                //AMCC-AB00-AB01-AB12-AB23       0.1
                chain1 = new StaticCycle(5);
                chain1.AddSpell(AMCC, castingState);
                chain1.AddSpell(AB00, castingState);
                chain1.AddSpell(AB01, castingState);
                chain1.AddSpell(AB12, castingState);
                chain1.AddSpell(AB23, castingState);
                chain1.Calculate(castingState);

                //AM?0-AB33                      0.9
                chain2 = new StaticCycle(2);
                chain2.AddSpell(AMc0, castingState);
                chain2.AddSpell(AB33, castingState);
                chain2.Calculate(castingState);

                Cycle[0] = chain1;
                Cycle[1] = chain2;
                Weight[0] = CC;
                Weight[1] = (1 - CC);
                Calculate();

                commonChain = chain2;
            }
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABAM3CCAM : DynamicCycle
    {
        StaticCycle chain1;
        StaticCycle chain2;
        StaticCycle chain3;
        float CC;

        public ABAM3CCAM(CastingState castingState) : base(3)
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
            Spell AMCC = castingState.GetCycle(SpellId.ArcaneMissilesCC);
            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11NoCC);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22NoCC);
            Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast33NoCC);

            CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

            if (CC == 0)
            {
                // if we don't have clearcasting then this degenerates to AMc0-AB33
                chain1 = new StaticCycle(2);
                chain1.AddSpell(AMc0, castingState);
                chain1.AddSpell(AB3, castingState);
                chain1.Calculate(castingState);

                Cycle[0] = chain1;
                Weight[0] = 1;

                CastTime = chain1.CastTime;
                CostPerSecond = chain1.CostPerSecond;
                DamagePerSecond = chain1.DamagePerSecond;
                ThreatPerSecond = chain1.ThreatPerSecond;

                commonChain = chain1;
            }
            else
            {
                //AB00-AM?0-AB11-AM?0-AB22-[(AM?0-AB33)x9+AMCC]       0.9*0.9
                chain1 = new StaticCycle(24);
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
                chain2 = new StaticCycle(4);
                chain2.AddSpell(AB0, castingState);
                chain2.AddSpell(AMc0, castingState);
                chain2.AddSpell(AB1, castingState);
                chain2.AddSpell(AMCC, castingState);
                chain2.Calculate(castingState);

                //AB00-AMCC                                           0.1
                chain3 = new StaticCycle(2);
                chain3.AddSpell(AB0, castingState);
                chain3.AddSpell(AMCC, castingState);
                chain3.Calculate(castingState);

                Cycle[0] = chain1;
                Cycle[1] = chain2;
                Cycle[2] = chain3;
                Weight[0] = (1 - CC) * (1 - CC);
                Weight[1] = CC * (1 - CC);
                Weight[2] = CC;
                Calculate();

                commonChain = chain3;
            }
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }*/

    class GenericArcane : Cycle
    {
        public Spell AB0, AB1, AB2, AB3, ABar0, ABar1, ABar2, ABar3, AM0, AM1, AM2, AM3, MBAM0, MBAM1, MBAM2, MBAM3;
        public double S00, S01, S02, S03, S10, S11, S12, S20, S21, S22, S30, S31, S32;
        public float KAB0, KAB1, KAB2, KAB3, KABar0, KABar1, KABar2, KABar3, KAM0, KAM1, KAM2, KAM3, KMBAM0, KMBAM1, KMBAM2, KMBAM3;
        public string SpellDistribution;

        private void AppendFormat(StringBuilder sb, string format, double weight)
        {
            if (weight > 0) sb.AppendFormat(format, weight);
        }

        public unsafe GenericArcane(string name, CastingState castingState, double X00, double X01, double X02, double X10, double X11, double X12, double X20, double X22, double X30, double X32, double X40, double X41, double X42, double X50, double X51, double X52, double X60, double X61, double X62, double X70, double X71, double X72, double X80, double X81, double X82, double X90, double X91, double X92) : base(castingState)
        {
            Name = name;

            const int size = 13;

            ArraySet arraySet = ArrayPool.RequestArraySet(size, size);
            LU M = new LU(size, arraySet);

            double[] X = new double[size];

            const int s00 = 0;
            const int s01 = 1;
            const int s02 = 2;
            const int s03 = 12;
            const int s10 = 3;
            const int s11 = 4;
            const int s12 = 5;
            const int s20 = 6;
            const int s21 = 7;
            const int s22 = 8;
            const int s30 = 9;
            const int s31 = 10;
            const int s32 = 11;

            double MB = 0.04 * castingState.MageTalents.MissileBarrage;
            double T8 = 0.0;

            fixed (double* U = arraySet.LU_U, x = X)
            fixed (double* sL = arraySet.LUsparseL, column = arraySet.LUcolumn, column2 = arraySet.LUcolumn2)
            fixed (int* P = arraySet.LU_P, Q = arraySet.LU_Q, LJ = arraySet.LU_LJ, sLI = arraySet.LUsparseLI, sLstart = arraySet.LUsparseLstart)
            {
                M.BeginUnsafe(U, sL, P, Q, LJ, sLI, sLstart, column, column2);

                for (int replace = size - 1; replace >= size - 1; replace--)
                {
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            U[i * size + j] = 0;
                        }
                    }

                    //U[i * rows + j]

                    //AB0,MB0,ABar+: S00
                    //AB0       => AB1,MB0,ABar+    X00*(1-MB)
                    //AB0       => AB1,MB1,ABar+    X00*MB
                    //ABar      => AB0,MB0,ABar-    X01*(1-MB)
                    //ABar      => AB0,MB2,ABar-    X01*MB
                    //AM        => AB0,MB0,ABar+    X02
                    U[s00 * size + s00] = X02 - 1;
                    U[s01 * size + s00] = X01 * (1 - MB);
                    U[s02 * size + s00] = X01 * MB;
                    U[s10 * size + s00] = X00 * (1 - MB);
                    U[s11 * size + s00] = X00 * MB;

                    //AB0,MB0,ABar-: S01
                    //AB0       => AB1,MB0,ABar+    X20*(1-MB)
                    //AB0       => AB1,MB1,ABar+    X20*MB
                    //AM        => AB0,MB0,ABar+    X22
                    U[s00 * size + s01] = X22;
                    U[s01 * size + s01] = -1;
                    U[s10 * size + s01] = X20 * (1 - MB);
                    U[s11 * size + s01] = X20 * MB;

                    //AB0,MB2,ABar-: S02
                    //AB0       => AB1,MB2,ABar+    X30
                    //MBAM      => AB0,MB0,ABar+    X32*(1-T8)
                    //MBAM      => AB0,MB2,ABar+    X32*T8
                    U[s00 * size + s02] = X32 * (1 - T8);
                    U[s02 * size + s02] = -1;
                    U[s03 * size + s02] = X32 * T8;
                    U[s12 * size + s02] = X30;

                    //AB0,MB2,ABar+: S03
                    //AB0       => AB1,MB2,ABar+    X90
                    //ABar      => AB0,MB2,ABar-    X91
                    //MBAM      => AB0,MB0,ABar+    X92*(1-T8)
                    //MBAM      => AB0,MB2,ABar+    X92*T8
                    U[s00 * size + s03] = X92 * (1 - T8);
                    U[s02 * size + s03] = X91;
                    U[s03 * size + s03] = X92 * T8 - 1;
                    U[s12 * size + s03] = X90;

                    //AB1,MB0,ABar+: S10
                    //AB1       => AB2,MB0,ABar+    X10*(1-MB)
                    //AB1       => AB2,MB1,ABar+    X10*MB
                    //ABar1     => AB0,MB0,ABar-    X11*(1-MB)
                    //ABar1     => AB0,MB2,ABar-    X11*MB
                    //AM1       => AB0,MB0,ABar+    X12
                    U[s00 * size + s10] = X12;
                    U[s01 * size + s10] = X11 * (1 - MB);
                    U[s02 * size + s10] = X11 * MB;
                    U[s20 * size + s10] = X10 * (1 - MB);
                    U[s21 * size + s10] = X10 * MB;
                    U[s10 * size + s10] = -1;

                    //AB1,MB1,ABar+: S11
                    //AB1       => AB2,MB2,ABar+    X10
                    //ABar1     => AB0,MB2,ABar-    X11
                    //MBAM1     => AB0,MB0,ABar+    X12*(1-T8)
                    //MBAM1     => AB0,MB2,ABar+    X12*T8
                    U[s00 * size + s11] = X12 * (1 - T8);
                    U[s02 * size + s11] = X11;
                    U[s03 * size + s11] = X12 * T8;
                    U[s22 * size + s11] = X10;
                    U[s11 * size + s11] = -1;

                    //AB1,MB2,ABar+: S12
                    //AB1       => AB2,MB2,ABar+    X40
                    //ABar1     => AB0,MB2,ABar-    X41
                    //MBAM1     => AB0,MB0,ABar+    X42*(1-T8)
                    //MBAM1     => AB0,MB2,ABar+    X42*T8
                    U[s00 * size + s12] = X42 * (1 - T8);
                    U[s02 * size + s12] = X41;
                    U[s03 * size + s12] = X42 * T8;
                    U[s22 * size + s12] = X40;
                    U[s12 * size + s12] = -1;

                    //AB2,MB0,ABar+: S20
                    //AB2       => AB3,MB0,ABar+    X50*(1-MB)
                    //AB2       => AB3,MB1,ABar+    X50*MB
                    //ABar2     => AB0,MB0,ABar-    X51*(1-MB)
                    //ABar2     => AB0,MB2,ABar-    X51*MB
                    //AM2       => AB0,MB0,ABar+    X52
                    U[s00 * size + s20] = X52;
                    U[s01 * size + s20] = X51 * (1 - MB);
                    U[s02 * size + s20] = X51 * MB;
                    U[s30 * size + s20] = X50 * (1 - MB);
                    U[s31 * size + s20] = X50 * MB;
                    U[s20 * size + s20] = -1;

                    //AB2,MB1,ABar+: S21
                    //AB2       => AB3,MB2,ABar+    X50
                    //ABar2     => AB0,MB2,ABar-    X51
                    //MBAM2     => AB0,MB0,ABar+    X52*(1-T8)
                    //MBAM2     => AB0,MB2,ABar+    X52*T8
                    U[s00 * size + s21] = X52 * (1 - T8);
                    U[s02 * size + s21] = X51;
                    U[s03 * size + s21] = X52 * T8;
                    U[s32 * size + s21] = X50;
                    U[s21 * size + s21] = -1;

                    //AB2,MB2,ABar+: S22
                    //AB2       => AB3,MB2,ABar+    X60
                    //ABar2     => AB0,MB2,ABar-    X61
                    //MBAM2     => AB0,MB0,ABar+    X62*(1-T8)
                    //MBAM2     => AB0,MB2,ABar+    X62*T8
                    U[s00 * size + s22] = X62 * (1 - T8);
                    U[s02 * size + s22] = X61;
                    U[s03 * size + s22] = X62 * T8;
                    U[s32 * size + s22] = X60;
                    U[s22 * size + s22] = -1;

                    //AB3,MB0,ABar+: S30
                    //AB3       => AB3,MB0,ABar+    X70*(1-MB)
                    //AB3       => AB3,MB1,ABar+    X70*MB
                    //ABar3     => AB0,MB0,ABar-    X71*(1-MB)
                    //ABar3     => AB0,MB2,ABar-    X71*MB
                    //AM3       => AB0,MB0,ABar+    X72
                    U[s00 * size + s30] = X72;
                    U[s01 * size + s30] = X71 * (1 - MB);
                    U[s02 * size + s30] = X71 * MB;
                    U[s30 * size + s30] = X70 * (1 - MB) - 1;
                    U[s31 * size + s30] = X70 * MB;

                    //AB3,MB1,ABar+: S31
                    //AB3       => AB3,MB2,ABar+    X70
                    //ABar3     => AB0,MB2,ABar-    X71
                    //MBAM3     => AB0,MB0,ABar+    X72*(1-T8)
                    //MBAM3     => AB0,MB2,ABar+    X72*T8
                    U[s00 * size + s31] = X72 * (1 - T8);
                    U[s02 * size + s31] = X71;
                    U[s03 * size + s31] = X72 * T8;
                    U[s32 * size + s31] = X70;
                    U[s31 * size + s31] = -1;

                    //AB3,MB2,ABar+: S32
                    //AB3       => AB3,MB2,ABar+    X80
                    //ABar3     => AB0,MB2,ABar-    X81
                    //MBAM3     => AB0,MB0,ABar+    X82*(1-T8)
                    //MBAM3     => AB0,MB2,ABar+    X82*T8
                    U[s00 * size + s32] = X82 * (1 - T8);
                    U[s02 * size + s32] = X81;
                    U[s03 * size + s32] = X82 * T8;
                    U[s32 * size + s32] = X80 - 1;

                    // the above system is singular, "guess" which one is dependent and replace with sum=1
                    // since not all states are used always we'll get a singular system anyway sometimes, but in those cases the FSolve should still work ok on the nonsingular part
                    for (int i = 0; i < size; i++) x[i] = 0;

                    if (replace < size)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            U[replace * size + i] = 1;
                        }

                        x[replace] = 1;
                    }

                    M.Decompose();
                    if (!M.Singular) break;
                }
                M.FSolve(x);

                M.EndUnsafe();

                S00 = x[s00];
                S01 = x[s01];
                S02 = x[s02];
                S03 = x[s03];
                S10 = x[s10];
                S11 = x[s11];
                S12 = x[s12];
                S20 = x[s20];
                S21 = x[s21];
                S22 = x[s22];
                S30 = x[s30];
                S31 = x[s31];
                S32 = x[s32];
            }

            AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
            ABar0 = castingState.GetSpell(SpellId.ArcaneBarrage);
            ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);
            ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
            AM0 = castingState.GetSpell(SpellId.ArcaneMissiles);
            AM1 = castingState.GetSpell(SpellId.ArcaneMissiles1);
            AM2 = castingState.GetSpell(SpellId.ArcaneMissiles2);
            AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            MBAM0 = castingState.GetSpell(SpellId.ArcaneMissilesMB);
            MBAM1 = castingState.GetSpell(SpellId.ArcaneMissilesMB1);
            MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

            KAB0 = (float)(S00 * X00 + S01 * X20 + S02 * X30 + S03 * X90);
            KABar0 = (float)(S00 * X01 + S03 * X91);
            KAM0 = (float)(S00 * X02 + S01 * X22);
            KMBAM0 = (float)(S02 * X32 + S03 * X92);
            KAB1 = (float)(S10 * X10 + S11 * X10 + S12 * X40);
            KABar1 = (float)(S10 * X11 + S11 * X11 + S12 * X41);
            KAM1 = (float)(S10 * X12);
            KMBAM1 = (float)(S11 * X12 + S12 * X42);
            KAB2 = (float)(S20 * X50 + S21 * X50 + S22 * X60);
            KABar2 = (float)(S20 * X51 + S21 * X51 + S22 * X61);
            KAM2 = (float)(S20 * X52);
            KMBAM2 = (float)(S21 * X52 + S22 * X62);
            KAB3 = (float)(S30 * X70 + S31 * X70 + S32 * X80);
            KABar3 = (float)(S30 * X71 + S31 * X71 + S32 * X81);
            KAM3 = (float)(S30 * X72);
            KMBAM3 = (float)(S31 * X72 + S32 * X82);

            CastTime = KAB0 * AB0.CastTime + KABar0 * ABar0.CastTime + KAM0 * AM0.CastTime + KMBAM0 * MBAM0.CastTime + KAB1 * AB1.CastTime + KABar1 * ABar1.CastTime + KAM1 * AM1.CastTime + KMBAM1 * MBAM1.CastTime + KAB2 * AB2.CastTime + KABar2 * ABar2.CastTime + KAM2 * AM2.CastTime + KMBAM2 * MBAM2.CastTime + KAB3 * AB3.CastTime + KABar3 * ABar3.CastTime + KAM3 * AM3.CastTime + KMBAM3 * MBAM3.CastTime;
            costPerSecond = (KAB0 * AB0.CastTime * AB0.CostPerSecond + KABar0 * ABar0.CastTime * ABar0.CostPerSecond + KAM0 * AM0.CastTime * AM0.CostPerSecond + KMBAM0 * MBAM0.CastTime * MBAM0.CostPerSecond + KAB1 * AB1.CastTime * AB1.CostPerSecond + KABar1 * ABar1.CastTime * ABar1.CostPerSecond + KAM1 * AM1.CastTime * AM1.CostPerSecond + KMBAM1 * MBAM1.CastTime * MBAM1.CostPerSecond + KAB2 * AB2.CastTime * AB2.CostPerSecond + KABar2 * ABar2.CastTime * ABar2.CostPerSecond + KAM2 * AM2.CastTime * AM2.CostPerSecond + KMBAM2 * MBAM2.CastTime * MBAM2.CostPerSecond + KAB3 * AB3.CastTime * AB3.CostPerSecond + KABar3 * ABar3.CastTime * ABar3.CostPerSecond + KAM3 * AM3.CastTime * AM3.CostPerSecond + KMBAM3 * MBAM3.CastTime * MBAM3.CostPerSecond) / CastTime;
            damagePerSecond = (KAB0 * AB0.CastTime * AB0.DamagePerSecond + KABar0 * ABar0.CastTime * ABar0.DamagePerSecond + KAM0 * AM0.CastTime * AM0.DamagePerSecond + KMBAM0 * MBAM0.CastTime * MBAM0.DamagePerSecond + KAB1 * AB1.CastTime * AB1.DamagePerSecond + KABar1 * ABar1.CastTime * ABar1.DamagePerSecond + KAM1 * AM1.CastTime * AM1.DamagePerSecond + KMBAM1 * MBAM1.CastTime * MBAM1.DamagePerSecond + KAB2 * AB2.CastTime * AB2.DamagePerSecond + KABar2 * ABar2.CastTime * ABar2.DamagePerSecond + KAM2 * AM2.CastTime * AM2.DamagePerSecond + KMBAM2 * MBAM2.CastTime * MBAM2.DamagePerSecond + KAB3 * AB3.CastTime * AB3.DamagePerSecond + KABar3 * ABar3.CastTime * ABar3.DamagePerSecond + KAM3 * AM3.CastTime * AM3.DamagePerSecond + KMBAM3 * MBAM3.CastTime * MBAM3.DamagePerSecond) / CastTime;
            threatPerSecond = (KAB0 * AB0.CastTime * AB0.ThreatPerSecond + KABar0 * ABar0.CastTime * ABar0.ThreatPerSecond + KAM0 * AM0.CastTime * AM0.ThreatPerSecond + KMBAM0 * MBAM0.CastTime * MBAM0.ThreatPerSecond + KAB1 * AB1.CastTime * AB1.ThreatPerSecond + KABar1 * ABar1.CastTime * ABar1.ThreatPerSecond + KAM1 * AM1.CastTime * AM1.ThreatPerSecond + KMBAM1 * MBAM1.CastTime * MBAM1.ThreatPerSecond + KAB2 * AB2.CastTime * AB2.ThreatPerSecond + KABar2 * ABar2.CastTime * ABar2.ThreatPerSecond + KAM2 * AM2.CastTime * AM2.ThreatPerSecond + KMBAM2 * MBAM2.CastTime * MBAM2.ThreatPerSecond + KAB3 * AB3.CastTime * AB3.ThreatPerSecond + KABar3 * ABar3.CastTime * ABar3.ThreatPerSecond + KAM3 * AM3.CastTime * AM3.ThreatPerSecond + KMBAM3 * MBAM3.CastTime * MBAM3.ThreatPerSecond) / CastTime;

            StringBuilder sb = new StringBuilder();
            AppendFormat(sb, "AB0:\t{0:F}%\r\n", 100.0 * (S00 * X00 + S01 * X20 + S02 * X30));
            AppendFormat(sb, "ABar0:\t{0:F}%\r\n", 100.0 * (S00 * X01));
            AppendFormat(sb, "AM0:\t{0:F}%\r\n", 100.0 * (S00 * X02 + S01 * X22));
            AppendFormat(sb, "MBAM0:\t{0:F}%\r\n", 100.0 * (S02 * X32));

            AppendFormat(sb, "AB1:\t{0:F}%\r\n", 100.0 * (S10 * X10 + S11 * X10 + S12 * X40));
            AppendFormat(sb, "ABar1:\t{0:F}%\r\n", 100.0 * (S10 * X11 + S11 * X11 + S12 * X41));
            AppendFormat(sb, "AM1:\t{0:F}%\r\n", 100.0 * (S10 * X12));
            AppendFormat(sb, "MBAM1:\t{0:F}%\r\n", 100.0 * (S11 * X12 + S12 * X42));

            AppendFormat(sb, "AB2:\t{0:F}%\r\n", 100.0 * (S20 * X50 + S21 * X50 + S22 * X60));
            AppendFormat(sb, "ABar2:\t{0:F}%\r\n", 100.0 * (S20 * X51 + S21 * X51 + S22 * X61));
            AppendFormat(sb, "AM2:\t{0:F}%\r\n", 100.0 * (S20 * X52));
            AppendFormat(sb, "MBAM2:\t{0:F}%\r\n", 100.0 * (S21 * X52 + S22 * X62));

            AppendFormat(sb, "AB3:\t{0:F}%\r\n", 100.0 * (S30 * X70 + S31 * X70 + S32 * X80));
            AppendFormat(sb, "ABar3:\t{0:F}%\r\n", 100.0 * (S30 * X71 + S31 * X71 + S32 * X81));
            AppendFormat(sb, "AM3:\t{0:F}%\r\n", 100.0 * (S30 * X72));
            AppendFormat(sb, "MBAM3:\t{0:F}%\r\n", 100.0 * (S31 * X72 + S32 * X82));

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
            AddEffectContribution(dict, duration);
        }

        public override void AddManaSourcesContribution(Dictionary<string, float> dict, float duration)
        {
            throw new NotImplementedException();
        }

        public override void AddManaUsageContribution(Dictionary<string, float> dict, float duration)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
