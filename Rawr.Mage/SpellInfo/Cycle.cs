using System;
using System.Collections.Generic;
#if RAWR3
using System.Linq;
#endif
using System.Text;

namespace Rawr.Mage
{
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
        ABSpam03MBAM,
        ABSpam3MBAM,
        ABAMABar,
        AB2AMABar,
        AB32AMABar,
        AB3ABar3MBAM,
        AB2AM,
        AB3AM,
        AB3AM23MBAM,
        AB4AM234MBAM,
        AB3AM023MBAM,
        AB4AM0234MBAM,
        ABSpam04MBAM,
        ABSpam024MBAM,
        ABSpam0234MBAM,
        ABSpam4MBAM,
        ABSpam24MBAM,
        ABSpam234MBAM,
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
        FrBIL,
        FrBILFB,
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

    public abstract class Cycle
    {
        public string Name;
        public CycleId CycleId;

        public override string ToString()
        {
            return Name;
        }

        public CastingState CastingState;

        protected Cycle(CastingState castingState)
        {
            CastingState = castingState;
        }

        public void Initialize(CastingState castingState)
        {
            CastingState = castingState;
            calculated = false;
            damagePerSecond = 0;
            effectDamagePerSecond = 0;
            effectSpellPower = 0;
            threatPerSecond = 0;
            effectThreatPerSecond = 0;
            costPerSecond = 0;
            manaRegenPerSecond = 0;
            DpsPerSpellPower = 0;

            AffectedByFlameCap = false;
            ProvidesSnare = false;
            ProvidesScorch = false;

            AreaEffect = false;
            AoeSpell = null;

            HitProcs = 0;
            Ticks = 0;
            CastProcs = 0;
            NukeProcs = 0;
            CritProcs = 0;
            IgniteProcs = 0;
            CastTime = 0;
            TargetProcs = 0;
            DamageProcs = 0;
            OO5SR = 0;
        }

        private bool calculated;

        internal float damagePerSecond;
        internal float effectDamagePerSecond;
        internal float effectSpellPower;
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

        public float DpsPerSpellPower;

        public bool AffectedByFlameCap;
        public bool ProvidesSnare;
        public bool ProvidesScorch;

        public bool AreaEffect;
        public Spell AoeSpell;

        public float HitProcs;
        public float Ticks;
        public float CastProcs;
        public float NukeProcs;
        public float CritProcs;
        public float IgniteProcs;
        public float CastTime;
        public float TargetProcs;
        public float DamageProcs;
        public float OO5SR;

        public void AddDamageContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            AddSpellContribution(dict, duration, effectSpellPower);
            AddEffectContribution(dict, duration);
        }

        public abstract void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration, float effectSpellPower);
        public abstract void AddManaUsageContribution(Dictionary<string, float> dict, float duration);

        private void Calculate()
        {
            if (!calculated)
            {
                CalculateIgniteDamageProcs();
                CalculateManaRegen();
                CalculateEffectDamage();
                calculated = true;
            }
        }

        private Spell waterbolt;

        private void CalculateIgniteDamageProcs()
        {
            if (IgniteProcs > 0)
            {
                double rate = IgniteProcs / CastTime;
                double k = Math.Exp(-2 * rate);
                double ticks = k * (1 + k);
                DamageProcs += (float)(IgniteProcs * ticks);
            }
        }

        private void CalculateEffectDamage()
        {
            Stats baseStats = CastingState.BaseStats;
            float spellPower = 0;
            if (Ticks > 0)
            {
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
                            if (CastProcs > 0)
                            {
                                spellPower += effect.Stats.SpellPower * effect.GetAverageUptime(CastTime / CastProcs, 1, 3, CastingState.CalculationOptions.FightDuration);
                            }
                            break;
                        case Trigger.MageNukeCast:
                            if (NukeProcs > 0)
                            {
                                spellPower += effect.Stats.SpellPower * effect.GetAverageUptime(CastTime / NukeProcs, 1, 3, CastingState.CalculationOptions.FightDuration);
                            }
                            break;
                        case Trigger.DamageDone:
                            if (DamageProcs > 0)
                            {
                                spellPower += effect.Stats.SpellPower * effect.GetAverageUptime(CastTime / DamageProcs, 1, 3, CastingState.CalculationOptions.FightDuration);
                            }
                            break;
                    }
                }
                if (baseStats.ShatteredSunAcumenProc > 0 && CastingState.CalculationOptions.Aldor) spellPower += 120 * 10f / (45f + CastTime / HitProcs / 0.1f);
            }
            effectSpellPower = spellPower;
            effectDamagePerSecond += spellPower * DpsPerSpellPower;
            //effectThreatPerSecond += spellPower * TpsPerSpellPower; // do we really need more threat calculations???
            if (CastingState.WaterElemental)
            {
                waterbolt = CastingState.Calculations.WaterboltTemplate.GetSpell(CastingState, CastingState.FrostSpellPower + spellPower);
                effectDamagePerSecond += waterbolt.DamagePerSecond;
            }
            if (Ticks > 0)
            {
                foreach (SpecialEffect effect in CastingState.Calculations.DamageProcEffects)
                {
                    float chance = 0;
                    float interval = 0;
                    switch (effect.Trigger)
                    {
                        case Trigger.SpellCrit:
                        case Trigger.DamageSpellCrit:
                            chance = CritProcs / Ticks;
                            // aoe modifier
                            if (TargetProcs > HitProcs)
                            {
                                chance = 1f - (float)Math.Pow(1 - chance, TargetProcs / HitProcs);
                            }
                            interval = CastTime / Ticks;
                            break;
                        case Trigger.SpellHit:
                        case Trigger.DamageSpellHit:
                            chance = HitProcs / Ticks;
                            // aoe modifier
                            if (TargetProcs > HitProcs)
                            {
                                chance = 1f - (float)Math.Pow(1 - chance, TargetProcs / HitProcs);
                            }
                            interval = CastTime / Ticks;
                            break;
                        case Trigger.DamageDone:
                            chance = 1;
                            interval = CastTime / DamageProcs;
                            break;
                    }
                    float effectsPerSecond = effect.GetAverageProcsPerSecond(interval, chance, 3f, CastingState.CalculationOptions.FightDuration);
                    if (effect.Stats.ArcaneDamage > 0)
                    {
                        float boltDps = CastingState.ArcaneAverageDamage * effect.Stats.ArcaneDamage * effectsPerSecond;
                        effectDamagePerSecond += boltDps;
                        effectThreatPerSecond += boltDps * CastingState.ArcaneThreatMultiplier;
                    }
                    if (effect.Stats.FireDamage > 0)
                    {
                        float boltDps = CastingState.FireAverageDamage * effect.Stats.FireDamage * effectsPerSecond;
                        effectDamagePerSecond += boltDps;
                        effectThreatPerSecond += boltDps * CastingState.FireThreatMultiplier;
                    }
                    /*if (effect.Stats.FrostDamage > 0)
                    {
                        float boltDps = CastingState.FrostAverageDamage * effect.Stats.FrostDamage * effectsPerSecond;
                        effectDamagePerSecond += boltDps;
                        effectThreatPerSecond += boltDps * CastingState.FrostThreatMultiplier;
                    }*/
                    if (effect.Stats.ShadowDamage > 0)
                    {
                        float boltDps = CastingState.ShadowAverageDamage * effect.Stats.ShadowDamage * effectsPerSecond;
                        effectDamagePerSecond += boltDps;
                        effectThreatPerSecond += boltDps * CastingState.ShadowThreatMultiplier;
                    }
                    if (effect.Stats.NatureDamage > 0)
                    {
                        float boltDps = CastingState.NatureAverageDamage * effect.Stats.NatureDamage * effectsPerSecond;
                        effectDamagePerSecond += boltDps;
                        effectThreatPerSecond += boltDps * CastingState.NatureThreatMultiplier;
                    }
                    /*if (effect.Stats.HolyDamage > 0)
                    {
                        float boltDps = CastingState.HolyAverageDamage * effect.Stats.HolyDamage * effectsPerSecond;
                        effectDamagePerSecond += boltDps;
                        effectThreatPerSecond += boltDps * CastingState.HolyThreatMultiplier;
                    }*/
                }
            }
            /*if (baseStats.LightningCapacitorProc > 0)
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
            }*/
            if (baseStats.ShatteredSunAcumenProc > 0 && !CastingState.CalculationOptions.Aldor)
            {
                float boltDps = CastingState.ArcaneBoltAverageDamage / (45f + CastTime / HitProcs / 0.1f);
                effectDamagePerSecond += boltDps;
                effectThreatPerSecond += boltDps * CastingState.ArcaneThreatMultiplier;
            }
            /*if (baseStats.PendulumOfTelluricCurrentsProc > 0)
            {
                float boltDps = CastingState.PendulumOfTelluricCurrentsAverageDamage / (45f + CastTime / HitProcs / 0.15f);
                effectDamagePerSecond += boltDps;
                effectThreatPerSecond += boltDps * CastingState.ShadowThreatMultiplier;
            }*/
        }

        private void CalculateManaRegen()
        {
            if (CastingState.CalculationOptions.EffectDisableManaSources) return;
            Stats baseStats = CastingState.BaseStats;
            manaRegenPerSecond = CastingState.ManaRegen5SR + OO5SR * (CastingState.ManaRegen - CastingState.ManaRegen5SR);
            float fight = CastingState.CalculationOptions.FightDuration;
            foreach (SpecialEffect effect in CastingState.Calculations.ManaRestoreEffects)
            {
                switch (effect.Trigger)
                {
                    case Trigger.Use:
                        manaRegenPerSecond += effect.Stats.ManaRestore * effect.GetAverageProcsPerSecond(0, 1, 3, fight);
                        break;
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                        if (CastProcs > 0)
                        {
                            manaRegenPerSecond += effect.Stats.ManaRestore * effect.GetAverageProcsPerSecond(CastTime / CastProcs, 1, 3, fight);
                        }
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        if (Ticks > 0)
                        {
                            manaRegenPerSecond += effect.Stats.ManaRestore * effect.GetAverageProcsPerSecond(CastTime / Ticks, CritProcs / Ticks, 3, fight);
                        }
                        break;
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        if (Ticks > 0)
                        {
                            manaRegenPerSecond += effect.Stats.ManaRestore * effect.GetAverageProcsPerSecond(CastTime / Ticks, HitProcs / Ticks, 3, fight);
                        }
                        break;
                    case Trigger.DamageDone:
                        if (DamageProcs > 0)
                        {
                            manaRegenPerSecond += effect.Stats.ManaRestore * effect.GetAverageProcsPerSecond(CastTime / DamageProcs, 1, 3, fight);
                        }
                        break;
                }
            }
            foreach (SpecialEffect effect in CastingState.Calculations.Mp5Effects)
            {
                switch (effect.Trigger)
                {
                    case Trigger.Use:
                        manaRegenPerSecond += effect.Stats.Mp5 / 5f * effect.GetAverageUptime(0f, 1f, 3, fight);
                        break;
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                        if (CastProcs > 0)
                        {
                            manaRegenPerSecond += effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / CastProcs, 1f, 3, fight);
                        }
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        if (Ticks > 0)
                        {
                            manaRegenPerSecond += effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / Ticks, CritProcs / Ticks, 3, fight);
                        }
                        break;
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        if (Ticks > 0)
                        {
                            manaRegenPerSecond += effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / Ticks, HitProcs / Ticks, 3, fight);
                        }
                        break;
                    case Trigger.DamageDone:
                        if (DamageProcs > 0)
                        {
                            manaRegenPerSecond += effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / DamageProcs, 1f, 3, fight);
                        }
                        break;
                }
            }
            //threatPerSecond += (baseStats.ManaRestoreFromBaseManaPPM * 3268 / CastTime * HitProcs) * 0.5f * (1 + baseStats.ThreatIncreaseMultiplier) * (1 - baseStats.ThreatReductionMultiplier);
            // 3.2 mode Empowered Fire ignite return
            if (IgniteProcs > 0 && CastingState.MageTalents.EmpoweredFire > 0)
            {
                // on average we have IgniteProcs per CastTime
                double rate = IgniteProcs / CastTime;
                // using an exponential distribution approximation for time between ignite procs
                // we obtain chances for 0, 1 or 2 ignite ticks from each ignite
                // using cummulative distribution function
                // Pr(T <= 2) = 1 - e ^ -2*rate
                // Pr(T <= 4) = 1 - e ^ -4*rate
                // Pr(T >= 4) = e ^ -4*rate
                // number of ticks from an ignite proc is then
                // 2 * e ^ -4*rate + 1 * (1 - e ^ -4*rate - (1 - e ^ -2*rate))
                // = 2 * e ^ -4*rate + e ^ -2*rate - e ^ -4*rate
                // = e ^ -2*rate * (1 + e ^ -2*rate)
                // an alternative would be to use geometric distribution approximation instead
                // it is not clear which one more closely matches real data
                double k = Math.Exp(-2 * rate);
                double ticks = k * (1 + k);
                // we now obtain average number of ticks per second
                // as average number of ignite procs per second times average number of ticks per proc
                double ticksPerSecond = rate * ticks;
                // finally using the proc of the ability we get the mps bonus
                manaRegenPerSecond += 0.02f * 3268f * CastingState.MageTalents.EmpoweredFire / 3.0f * (float)ticksPerSecond;
            }
        }

        public virtual void AddManaSourcesContribution(Dictionary<string, float> dict, float duration)
        {
            if (CastingState.CalculationOptions.EffectDisableManaSources) return;
            dict["Intellect/Spirit"] += duration * (CastingState.SpiritRegen * CastingState.BaseStats.SpellCombatManaRegeneration + OO5SR * (CastingState.SpiritRegen - CastingState.SpiritRegen * CastingState.BaseStats.SpellCombatManaRegeneration));
            dict["MP5"] += duration * CastingState.BaseStats.Mp5 / 5f;
            dict["Innervate"] += duration * (15732 * CastingState.CalculationOptions.Innervate / CastingState.CalculationOptions.FightDuration);
            dict["Mana Tide"] += duration * CastingState.CalculationOptions.ManaTide * 0.24f * CastingState.BaseStats.Mana / CastingState.CalculationOptions.FightDuration;
            dict["Replenishment"] += duration * CastingState.BaseStats.ManaRestoreFromMaxManaPerSecond * CastingState.BaseStats.Mana;
            //dict["Judgement of Wisdom"] += duration * CastingState.BaseStats.ManaRestoreFromBaseManaPPM * 3268 / CastTime * HitProcs;
            float fight = CastingState.CalculationOptions.FightDuration;
            foreach (SpecialEffect effect in CastingState.Calculations.ManaRestoreEffects)
            {
                switch (effect.Trigger)
                {
                    case Trigger.Use:
                        dict["Other"] += duration * effect.Stats.ManaRestore * effect.GetAverageProcsPerSecond(0, 1, 3, fight);
                        break;
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                        if (CastProcs > 0)
                        {
                            dict["Other"] += duration * effect.Stats.ManaRestore * effect.GetAverageProcsPerSecond(CastTime / CastProcs, 1, 3, fight);
                        }
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        if (Ticks > 0)
                        {
                            dict["Other"] += duration * effect.Stats.ManaRestore * effect.GetAverageProcsPerSecond(CastTime / Ticks, CritProcs / Ticks, 3, fight);
                        }
                        break;
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        if (Ticks > 0)
                        {
                            dict["Other"] += duration * effect.Stats.ManaRestore * effect.GetAverageProcsPerSecond(CastTime / Ticks, HitProcs / Ticks, 3, fight);
                        }
                        break;
                    case Trigger.DamageDone:
                        if (DamageProcs > 0)
                        {
                            dict["Other"] += duration * effect.Stats.ManaRestore * effect.GetAverageProcsPerSecond(CastTime / DamageProcs, 1, 3, fight);
                        }
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
                        if (CastProcs > 0)
                        {
                            dict["Other"] += duration * effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / CastProcs, 1f, 3, CastingState.CalculationOptions.FightDuration);
                        }
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        if (Ticks > 0)
                        {
                            dict["Other"] += duration * effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / Ticks, CritProcs / Ticks, 3, CastingState.CalculationOptions.FightDuration);
                        }
                        break;
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        if (Ticks > 0)
                        {
                            dict["Other"] += duration * effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / Ticks, HitProcs / Ticks, 3, CastingState.CalculationOptions.FightDuration);
                        }
                        break;
                    case Trigger.DamageDone:
                        if (DamageProcs > 0)
                        {
                            dict["Other"] += duration * effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / DamageProcs, 1f, 3, CastingState.CalculationOptions.FightDuration);
                        }
                        break;
                }
            }
            if (IgniteProcs > 0 && CastingState.MageTalents.EmpoweredFire > 0)
            {
                double rate = IgniteProcs / CastTime;
                double k = Math.Exp(-2 * rate);
                double ticks = k * (1 + k);
                double ticksPerSecond = rate * ticks;
                dict["Other"] += duration * 0.02f * 3268f * CastingState.MageTalents.EmpoweredFire / 3.0f * (float)ticksPerSecond;
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
            if (Ticks > 0)
            {
                foreach (SpecialEffect effect in CastingState.Calculations.DamageProcEffects)
                {
                    string name = null;
                    float chance = 0;
                    float interval = 0;
                    switch (effect.Trigger)
                    {
                        case Trigger.SpellCrit:
                        case Trigger.DamageSpellCrit:
                            chance = CritProcs / Ticks;
                            // aoe modifier
                            if (TargetProcs > HitProcs)
                            {
                                chance = 1f - (float)Math.Pow(1 - chance, TargetProcs / HitProcs);
                            }
                            interval = CastTime / Ticks;
                            break;
                        case Trigger.SpellHit:
                        case Trigger.DamageSpellHit:
                            chance = HitProcs / Ticks;
                            // aoe modifier
                            if (TargetProcs > HitProcs)
                            {
                                chance = 1f - (float)Math.Pow(1 - chance, TargetProcs / HitProcs);
                            }
                            interval = CastTime / Ticks;
                            break;
                        case Trigger.DamageDone:
                            chance = 1;
                            interval = CastTime / DamageProcs;
                            break;
                    }
                    float effectsPerSecond = effect.GetAverageProcsPerSecond(interval, chance, 3f, CastingState.CalculationOptions.FightDuration);
                    float boltDps = 0f;
                    if (effect.Stats.ArcaneDamage > 0)
                    {
                        boltDps = CastingState.ArcaneAverageDamage * effect.Stats.ArcaneDamage * effectsPerSecond;
                        name = "Arcane Damage Proc";
                    }
                    if (effect.Stats.FireDamage > 0)
                    {
                        boltDps = CastingState.FireAverageDamage * effect.Stats.FireDamage * effectsPerSecond;
                        name = "Fire Damage Proc";
                    }
                    /*if (effect.Stats.FrostDamage > 0)
                    {
                        boltDps = CastingState.FrostAverageDamage * effect.Stats.FrostDamage * effectsPerSecond;
                        name = "Frost Damage Proc";
                    }*/
                    if (effect.Stats.ShadowDamage > 0)
                    {
                        boltDps = CastingState.ShadowAverageDamage * effect.Stats.ShadowDamage * effectsPerSecond;
                        name = "Shadow Damage Proc";
                    }
                    if (effect.Stats.NatureDamage > 0)
                    {
                        boltDps = CastingState.NatureAverageDamage * effect.Stats.NatureDamage * effectsPerSecond;
                        name = "Nature Damage Proc";
                    }
                    /*if (effect.Stats.HolyDamage > 0)
                    {
                        boltDps = CastingState.HolyAverageDamage * effect.Stats.HolyDamage * effectsPerSecond;
                        name = "Holy Damage Proc";
                    }*/
                    if (!dict.TryGetValue(name, out contrib))
                    {
                        contrib = new SpellContribution() { Name = name };
                        dict[name] = contrib;
                    }
                    contrib.Hits += effectsPerSecond * duration;
                    contrib.Damage += boltDps * duration;
                }
            }
            /*if (CastingState.BaseStats.LightningCapacitorProc > 0)
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
            }*/
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
            /*if (CastingState.BaseStats.PendulumOfTelluricCurrentsProc > 0)
            {
                if (!dict.TryGetValue("Pendulum of Telluric Currents", out contrib))
                {
                    contrib = new SpellContribution() { Name = "Pendulum of Telluric Currents" };
                    dict["Pendulum of Telluric Currents"] = contrib;
                }
                float boltDps = CastingState.PendulumOfTelluricCurrentsAverageDamage / (45f + CastTime / HitProcs / 0.15f);
                contrib.Hits += duration / (45f + CastTime / HitProcs / 0.15f);
                contrib.Damage += boltDps * duration;
            }*/
            if (IgniteProcs > 0 && dict.TryGetValue("Ignite", out contrib))
            {
                double rate = IgniteProcs / CastTime;
                double k = Math.Exp(-2 * rate);
                double ticks = k * (1 + k);
                double ticksPerSecond = rate * ticks;
                contrib.Hits += duration * (float)ticksPerSecond;
            }
        }
    }

    public class SpellCustomMix : DynamicCycle
    {
        public SpellCustomMix(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
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

    public class StaticCycle : Cycle
    {
        public bool recalc5SR;

        private List<Spell> spellList;
        private FSRCalc fsr;

        public StaticCycle()
            : base(null)
        {
            spellList = new List<Spell>();
        }

        public StaticCycle(int capacity)
            : base(null)
        {
            spellList = new List<Spell>(capacity);
        }

        public StaticCycle(int capacity, bool recalcFiveSecondRule)
            : base(null)
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
                fsr.AddSpell(spell.CastTime - spell.Latency, spell.Latency, spell.Channeled);
            }
            Ticks += spell.Ticks;
            CastTime += spell.CastTime;
            NukeProcs += spell.NukeProcs;
            HitProcs += spell.HitProcs;
            CastProcs += spell.CastProcs;
            CritProcs += spell.CritProcs;
            IgniteProcs += spell.IgniteProcs;
            TargetProcs += spell.TargetProcs;
            DamageProcs += spell.HitProcs + spell.DotProcs;
            damagePerSecond += spell.DamagePerSecond * spell.CastTime;
            threatPerSecond += spell.ThreatPerSecond * spell.CastTime;
            costPerSecond += spell.CostPerSecond * spell.CastTime;
            DpsPerSpellPower += spell.DpsPerSpellPower * spell.CastTime;
            AffectedByFlameCap = AffectedByFlameCap || spell.AffectedByFlameCap;
            spellList.Add(spell);
        }

        public void AddSpell(DotSpell spell, CastingState castingState, float dotUptime)
        {
            if (recalc5SR)
            {
                fsr.AddSpell(spell.CastTime - spell.Latency, spell.Latency, spell.Channeled);
            }
            Ticks += spell.Ticks;
            CastTime += spell.CastTime;
            NukeProcs += spell.NukeProcs;
            HitProcs += spell.HitProcs;
            CastProcs += spell.CastProcs;
            CritProcs += spell.CritProcs;
            IgniteProcs += spell.IgniteProcs;
            TargetProcs += spell.TargetProcs;
            DamageProcs += spell.HitProcs + dotUptime * spell.DotProcs;
            damagePerSecond += (spell.DamagePerSecond + dotUptime * spell.DotDamagePerSecond) * spell.CastTime;
            threatPerSecond += (spell.ThreatPerSecond + dotUptime * spell.DotThreatPerSecond) * spell.CastTime;
            costPerSecond += spell.CostPerSecond * spell.CastTime;
            DpsPerSpellPower += (spell.DpsPerSpellPower + dotUptime * spell.DotDpsPerSpellPower) * spell.CastTime;
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

            costPerSecond /= CastTime;
            damagePerSecond /= CastTime;
            threatPerSecond /= CastTime;
            DpsPerSpellPower /= CastTime;
            this.CastingState = castingState;

            if (recalc5SR)
            {
                OO5SR = fsr.CalculateOO5SR(0.02f * castingState.MageTalents.ArcaneConcentration);
            }
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration, float effectSpellPower)
        {
            foreach (Spell spell in spellList)
            {
                if (spell != null)
                {
                    spell.AddSpellContribution(dict, spell.CastTime * duration / CastTime, effectSpellPower);
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

    public class DynamicCycle : Cycle
    {
        private List<Cycle> Cycle;
        private List<float> Weight;

        public DynamicCycle(bool needsDisplayCalculations, CastingState castingState)
            : base(castingState)
        {
            if (needsDisplayCalculations)
            {
                Cycle = new List<Cycle>();
                Weight = new List<float>();
            }
        }

        public static DynamicCycle New(bool needsDisplayCalculations, CastingState castingState)
        {
            ArraySet arraySet = castingState.Calculations.ArraySet;
            if (needsDisplayCalculations || arraySet == null)
            {
                return new DynamicCycle(needsDisplayCalculations, castingState);
            }
            else
            {
                return arraySet.NewDynamicCycle(castingState);
            }
        }

        public void AddCycle(bool needsDisplayCalculations, Cycle cycle, float weight)
        {
            if (needsDisplayCalculations)
            {
                Cycle.Add(cycle);
                Weight.Add(weight);
            }
            CastTime += weight * cycle.CastTime;
            CastProcs += weight * cycle.CastProcs;
            NukeProcs += weight * cycle.NukeProcs;
            Ticks += weight * cycle.Ticks;
            HitProcs += weight * cycle.HitProcs;
            CritProcs += weight * cycle.CritProcs;
            IgniteProcs += weight * cycle.IgniteProcs;
            TargetProcs += weight * cycle.TargetProcs;
            DamageProcs += weight * cycle.DamageProcs;
            costPerSecond += weight * cycle.CastTime * cycle.costPerSecond;
            damagePerSecond += weight * cycle.CastTime * cycle.damagePerSecond;
            threatPerSecond += weight * cycle.CastTime * cycle.threatPerSecond;
            DpsPerSpellPower += weight * cycle.CastTime * cycle.DpsPerSpellPower;
        }

        public void AddSpell(bool needsDisplayCalculations, Spell spell, float weight)
        {
            if (needsDisplayCalculations)
            {
                Cycle.Add(spell);
                Weight.Add(weight);
            }
            CastTime += weight * spell.CastTime;
            CastProcs += weight * spell.CastProcs;
            NukeProcs += weight * spell.NukeProcs;
            Ticks += weight * spell.Ticks;
            HitProcs += weight * spell.HitProcs;
            CritProcs += weight * spell.CritProcs;
            IgniteProcs += weight * spell.IgniteProcs;
            TargetProcs += weight * spell.TargetProcs;
            DamageProcs += weight * (spell.HitProcs + spell.DotProcs);
            costPerSecond += weight * spell.CastTime * spell.CostPerSecond;
            damagePerSecond += weight * spell.CastTime * spell.DamagePerSecond;
            threatPerSecond += weight * spell.CastTime * spell.ThreatPerSecond;
            DpsPerSpellPower += weight * spell.CastTime * spell.DpsPerSpellPower;
        }

        public void AddSpell(bool needsDisplayCalculations, DotSpell spell, float weight, float dotUptime)
        {
            if (needsDisplayCalculations)
            {
                Cycle.Add(spell);
                Weight.Add(weight);
            }
            CastTime += weight * spell.CastTime;
            CastProcs += weight * spell.CastProcs;
            NukeProcs += weight * spell.NukeProcs;
            Ticks += weight * spell.Ticks;
            HitProcs += weight * spell.HitProcs;
            CritProcs += weight * spell.CritProcs;
            IgniteProcs += weight * spell.IgniteProcs;
            TargetProcs += weight * spell.TargetProcs;
            DamageProcs += weight * (spell.HitProcs + dotUptime * spell.DotProcs);
            costPerSecond += weight * spell.CastTime * spell.CostPerSecond;
            damagePerSecond += weight * spell.CastTime * (spell.DamagePerSecond + dotUptime * spell.DotDamagePerSecond);
            threatPerSecond += weight * spell.CastTime * (spell.ThreatPerSecond + dotUptime * spell.DotThreatPerSecond);
            DpsPerSpellPower += weight * spell.CastTime * (spell.DpsPerSpellPower + dotUptime * spell.DotDpsPerSpellPower);
        }

        public void AddPause(float duration, float weight)
        {
            CastTime += weight * duration;
        }

        public void Calculate()
        {
            costPerSecond /= CastTime;
            damagePerSecond /= CastTime;
            threatPerSecond /= CastTime;
            DpsPerSpellPower /= CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration, float effectSpellPower)
        {
            for (int i = 0; i < Cycle.Count; i++)
            {
                if (Cycle[i] != null) Cycle[i].AddSpellContribution(dict, Weight[i] * Cycle[i].CastTime / CastTime * duration, effectSpellPower);
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

    public class CycleState
    {
        public List<CycleStateTransition> Transitions { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class CycleStateTransition
    {
        public CycleState TargetState { get; set; }
        public Cycle Cycle { get; set; }
        public Spell Spell { get; set; }
        public float Pause { get; set; }
        public virtual float TransitionProbability { get; set; }

        public override string ToString()
        {
            if (Spell != null)
            {
                return string.Format("{0} => {1} : {2:F}%", Spell.Name, TargetState, 100 * TransitionProbability);
            }
            else if (Cycle != null)
            {
                return string.Format("{0} => {1} : {2:F}%", Cycle.Name, TargetState, 100 * TransitionProbability);
            }
            else if (Pause > 0)
            {
                return string.Format("{0:F} sec => {1} : {2:F}%", Pause, TargetState, 100 * TransitionProbability);
            }
            return base.ToString();
        }
    }

    public class GenericCycle : DynamicCycle
    {
        public List<CycleState> StateList;
        public double[] StateWeight;
        Dictionary<Spell, double> SpellWeight = new Dictionary<Spell, double>();
        Dictionary<Cycle, double> CycleWeight = new Dictionary<Cycle, double>();
        public string SpellDistribution;

#if SILVERLIGHT
        public GenericCycle(string name, CastingState castingState, List<CycleState> stateDescription, bool needsDisplayCalculations)
#else
        public unsafe GenericCycle(string name, CastingState castingState, List<CycleState> stateDescription, bool needsDisplayCalculations)
#endif
            : base(needsDisplayCalculations, castingState)
        {
            Name = name;

            StateList = stateDescription;
            for (int i = 0; i < StateList.Count; i++)
            {
                StateList[i].Index = i;
            }

            int size = StateList.Count + 1;

            ArraySet arraySet = ArrayPool.RequestArraySet(size, size);
            LU M = new LU(size, arraySet);

            StateWeight = new double[size];

#if SILVERLIGHT
            M.BeginSafe();

            Array.Clear(arraySet.LU_U, 0, size * size);

            //U[i * rows + j]

            foreach (CycleState state in StateList)
            {
                foreach (CycleStateTransition transition in state.Transitions)
                {
                    arraySet.LU_U[transition.TargetState.Index * size + state.Index] += transition.TransitionProbability;
                }
                arraySet.LU_U[state.Index * size + state.Index] -= 1.0;
            }

            for (int i = 0; i < size - 1; i++)
            {
                arraySet.LU_U[(size - 1) * size + i] = 1;
            }

            StateWeight[size - 1] = 1;

            M.Decompose();
            M.FSolve(StateWeight);

            M.EndUnsafe();            
#else
            fixed (double* U = arraySet.LU_U, x = StateWeight)
            fixed (double* sL = arraySet.LUsparseL, column = arraySet.LUcolumn, column2 = arraySet.LUcolumn2)
            fixed (int* P = arraySet.LU_P, Q = arraySet.LU_Q, LJ = arraySet.LU_LJ, sLI = arraySet.LUsparseLI, sLstart = arraySet.LUsparseLstart)
            {
                M.BeginUnsafe(U, sL, P, Q, LJ, sLI, sLstart, column, column2);

                Array.Clear(arraySet.LU_U, 0, size * size);

                //U[i * rows + j]

                foreach (CycleState state in StateList)
                {
                    foreach (CycleStateTransition transition in state.Transitions)
                    {
                        U[transition.TargetState.Index * size + state.Index] += transition.TransitionProbability;
                    }
                    U[state.Index * size + state.Index] -= 1.0;
                }

                for (int i = 0; i < size - 1; i++)
                {
                    U[(size - 1) * size + i] = 1;
                }

                x[size - 1] = 1;

                M.Decompose();
                M.FSolve(x);

                M.EndUnsafe();
            }
#endif

            SpellWeight = new Dictionary<Spell, double>();
            CycleWeight = new Dictionary<Cycle, double>();

            foreach (CycleState state in StateList)
            {
                double stateWeight = StateWeight[state.Index];
                if (stateWeight > 0)
                {
                    foreach (CycleStateTransition transition in state.Transitions)
                    {
                        float transitionProbability = transition.TransitionProbability;
                        if (transitionProbability > 0)
                        {
                            if (transition.Spell != null)
                            {
                                double weight;
                                SpellWeight.TryGetValue(transition.Spell, out weight);
                                SpellWeight[transition.Spell] = weight + stateWeight * transitionProbability;
                            }
                            if (transition.Cycle != null)
                            {
                                double weight;
                                CycleWeight.TryGetValue(transition.Cycle, out weight);
                                CycleWeight[transition.Cycle] = weight + stateWeight * transitionProbability;
                            }
                            if (transition.Pause > 0)
                            {
                                AddPause(transition.Pause, (float)(stateWeight * transitionProbability));
                            }
                        }
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<Spell, double> kvp in SpellWeight)
            {
                AddSpell(needsDisplayCalculations, kvp.Key, (float)kvp.Value);
                if (kvp.Value > 0) sb.AppendFormat("{0}:\t{1:F}%\r\n", kvp.Key.Label ?? kvp.Key.SpellId.ToString(), 100.0 * kvp.Value);
            }
            foreach (KeyValuePair<Cycle, double> kvp in CycleWeight)
            {
                AddCycle(needsDisplayCalculations, kvp.Key, (float)kvp.Value);
                if (kvp.Value > 0) sb.AppendFormat("{0}:\t{1:F}%\r\n", kvp.Key.CycleId, 100.0 * kvp.Value);
            }

            Calculate();
           
            SpellDistribution = sb.ToString();
            ArrayPool.ReleaseArraySet(arraySet);
        }
    }

    public class CycleControlledStateTransition : CycleStateTransition
    {
        private float rawProbability;
        private int controlIndex;
        private int controlValue;
        private int[] controlStates;

        public void SetControls(int controlIndex, int[] controlStates, int controlValue)
        {
            this.controlIndex = controlIndex;
            this.controlStates = controlStates;
            this.controlValue = controlValue;
        }

        public override float TransitionProbability
        {
            get
            {
                return (controlStates[controlIndex] == controlValue) ? rawProbability : 0.0f;
            }
            set
            {
                rawProbability = value;
            }
        }

        public override string ToString()
        {
            if (Spell != null)
            {
                return string.Format("{0} => {1} : {2:F}%", Spell.Name, TargetState, 100 * rawProbability);
            }
            else if (Cycle != null)
            {
                return string.Format("{0} => {1} : {2:F}%", Cycle.Name, TargetState, 100 * rawProbability);
            }
            else if (Pause > 0)
            {
                return string.Format("{0:F} sec => {1} : {2:F}%", Pause, TargetState, 100 * rawProbability);
            }
            return base.ToString();
        }
    }

    public abstract class CycleGenerator
    {
        public List<CycleState> StateList;
        public int[] ControlOptions;
        public int[] ControlValue;
        public int[] ControlIndex;
        public virtual string StateDescription
        {
            get
            {
                return "";
            }
        }

        public void GenerateStateDescription()
        {
            List<CycleState> remainingStates = new List<CycleState>();
            List<CycleState> processedStates = new List<CycleState>();
            remainingStates.Add(GetInitialState());

            while (remainingStates.Count > 0)
            {
                CycleState state = remainingStates[remainingStates.Count - 1];
                remainingStates.RemoveAt(remainingStates.Count - 1);

                List<CycleControlledStateTransition> transitions = GetStateTransitions(state);
#if SILVERLIGHT
                state.Transitions = transitions.ConvertAll(transition => (CycleStateTransition)transition).ToList();
#else
                state.Transitions = transitions.ConvertAll(transition => (CycleStateTransition)transition);
#endif
                foreach (CycleControlledStateTransition transition in transitions)
                {
                    if (transition.TargetState != state && !processedStates.Contains(transition.TargetState) && !remainingStates.Contains(transition.TargetState))
                    {
                        remainingStates.Add(transition.TargetState);
                    }
                }

                processedStates.Add(state);
            }

            StateList = processedStates;
            for (int i = 0; i < StateList.Count; i++)
            {
                StateList[i].Index = i;
            }

            ControlIndex = new int[StateList.Count];
            List<CycleState> controlledStates = new List<CycleState>();
            foreach (CycleState state in StateList)
            {
                int controlIndex = -1;
                foreach (CycleState controlledState in controlledStates)
                {
                    if (!CanStatesBeDistinguished(state, controlledState))
                    {
                        controlIndex = ControlIndex[controlledState.Index];
                        break;
                    }
                }
                if (controlIndex == -1)
                {
                    controlIndex = controlledStates.Count;
                    controlledStates.Add(state);
                }
                ControlIndex[state.Index] = controlIndex;
            }

            ControlOptions = new int[controlledStates.Count];
            ControlValue = new int[controlledStates.Count];

            Dictionary<string, int>[] spellMap = new Dictionary<string, int>[controlledStates.Count];

            foreach (CycleState state in StateList)
            {
                int controlIndex = ControlIndex[state.Index];
                if (spellMap[controlIndex] == null)
                {
                    spellMap[controlIndex] = new Dictionary<string, int>();
                }
                foreach (CycleControlledStateTransition transition in state.Transitions)
                {
                    string n;
                    if (transition.Spell != null)
                    {
                        n = transition.Spell.Name;
                    }
                    else
                    {
                        n = "Pause";
                    }
                    int controlValue;
                    if (!spellMap[controlIndex].TryGetValue(n, out controlValue))
                    {
                        controlValue = spellMap[controlIndex].Keys.Count;
                        spellMap[controlIndex][n] = controlValue;
                    }
                    transition.SetControls(controlIndex, ControlValue, controlValue);
                }
            }

            for (int i = 0; i < ControlOptions.Length; i++)
            {
                ControlOptions[i] = spellMap[i].Keys.Count;
            }
        }

        public GenericCycle GenerateCycle(string name, CastingState castingState)
        {
            return new GenericCycle(name, castingState, StateList, false);
        }

        public List<Cycle> Analyze(CastingState castingState, Cycle wand)
        {
            return Analyze(castingState, wand, null);
        }

        public List<Cycle> Analyze(CastingState castingState, Cycle wand, System.ComponentModel.BackgroundWorker worker)
        {
            Dictionary<string, Cycle> cycleDict = new Dictionary<string, Cycle>();
            int j;
            // reset
            for (int i = 0; i < ControlValue.Length; i++)
            {
                ControlValue[i] = 0;
            }
            // count total cycles
            int total = 0;
            do
            {
                total++;
                j = ControlValue.Length - 1;
                ControlValue[j]++;
                while (ControlValue[j] >= ControlOptions[j])
                {
                    ControlValue[j] = 0;
                    j--;
                    if (j < 0)
                    {
                        break;
                    }
                    ControlValue[j]++;
                }
            } while (j >= 0);
            // reset
            for (int i = 0; i < ControlValue.Length; i++)
            {
                ControlValue[i] = 0;
            }
            int count = 0;
            do
            {
                if (worker != null && worker.CancellationPending)
                {
                    break;
                }
                if (worker != null && count % 100 == 0)
                {
                    worker.ReportProgress((100 * count) / total, count + "/" + total);
                }
                count++;
                string name = "";
                for (int i = 0; i < ControlValue.Length; i++)
                {
                    name += ControlValue[i].ToString();
                }
                GenericCycle generic = new GenericCycle(name, castingState, StateList, false);
                if (!cycleDict.ContainsKey(generic.SpellDistribution))
                {
                    cycleDict.Add(generic.SpellDistribution, generic);
                }
                // increment control
                j = ControlValue.Length - 1;
                ControlValue[j]++;
                while (ControlValue[j] >= ControlOptions[j])
                {
                    ControlValue[j] = 0;
                    j--;
                    if (j < 0)
                    {
                        break;
                    }
                    ControlValue[j]++;
                }
            } while (j >= 0);

            if (wand != null)
            {
                cycleDict["Wand"] = wand;
            }

            List<Cycle> cyclePalette = new List<Cycle>();

            double maxdps = 0;
            Cycle maxdpsCycle = null;
            foreach (Cycle cycle in cycleDict.Values)
            {
                if (cycle.DamagePerSecond > maxdps)
                {
                    maxdpsCycle = cycle;
                    maxdps = cycle.DamagePerSecond;
                }
            }

            cyclePalette.Add(maxdpsCycle);

            Cycle mindpmCycle;
            do
            {
                Cycle highdpsCycle = cyclePalette[cyclePalette.Count - 1];
            RESTART:
                mindpmCycle = null;
                double mindpm = double.PositiveInfinity;
                foreach (Cycle cycle in cycleDict.Values)
                {
                    double dpm = (cycle.DamagePerSecond - highdpsCycle.DamagePerSecond) / (cycle.ManaPerSecond - highdpsCycle.ManaPerSecond);
                    if (dpm > 0 && dpm < mindpm && cycle.ManaPerSecond < highdpsCycle.ManaPerSecond)
                    {
                        mindpm = dpm;
                        mindpmCycle = cycle;
                    }
                }
                if (mindpmCycle != null)
                {
                    // validate cycle pair theory
                    foreach (Cycle cycle in cycleDict.Values)
                    {
                        double dpm = (cycle.DamagePerSecond - mindpmCycle.DamagePerSecond) / (cycle.ManaPerSecond - mindpmCycle.ManaPerSecond);
                        if (cycle != highdpsCycle && cycle.DamagePerSecond > mindpmCycle.DamagePerSecond && dpm > mindpm + 0.000001)
                        {
                            highdpsCycle = cycle;
                            goto RESTART;
                        }
                    }
                    cyclePalette.Add(mindpmCycle);
                }
            } while (mindpmCycle != null);
            return cyclePalette;
        }

        protected abstract CycleState GetInitialState();
        // the transition probabilities should be set as given the spell/pause is executed 100%
        // the transitions should all be spell transitions and at most one can be a state changing pause
        protected abstract List<CycleControlledStateTransition> GetStateTransitions(CycleState state);
        // the states must form equivalence classes
        protected abstract bool CanStatesBeDistinguished(CycleState state1, CycleState state2);
    }
}
