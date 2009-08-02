using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Elemental.Estimation
{
    public class Estimation
    {
        #region Spells
        LightningBolt[] LB;
        ChainLightning[] CL;
        ChainLightning[] CL3;
        ChainLightning[] CL4;
        LavaBurst[] LvB;
        LavaBurst[] LvBFS;
        FlameShock[] FS;
        EarthShock[] ES;
        FrostShock[] FrS;
        Thunderstorm[] TS;
        #endregion

        int num;

        private Stats baseStats, procStats;
        private ShamanTalents talents;
        private CalculationOptionsElemental calcOpts;

        public Estimation(Stats baseStats, Stats procStats, ShamanTalents talents, CalculationOptionsElemental calcOpts, int num)
        {
            this.baseStats = baseStats;
            this.procStats = procStats;
            this.talents = talents;
            this.calcOpts = calcOpts;
            this.num = num;
            #region Spells
            LB = new LightningBolt[num];
            CL = new ChainLightning[num];
            CL3 = new ChainLightning[num];
            CL4 = new ChainLightning[num];
            LvB = new LavaBurst[num];
            LvBFS = new LavaBurst[num];
            FS = new FlameShock[num];
            ES = new EarthShock[num];
            FrS = new FrostShock[num];
            TS = new Thunderstorm[num];
            if (num == 1)
            {
                Stats addedStats = baseStats.Clone();
                addedStats.Accumulate(procStats);
                LB[0] = new LightningBolt(addedStats, talents, calcOpts);
                CL[0] = new ChainLightning(addedStats, talents, calcOpts, 0);
                CL3[0] = new ChainLightning(addedStats, talents, calcOpts, 3);
                CL4[0] = new ChainLightning(addedStats, talents, calcOpts, 4);
                LvB[0] = new LavaBurst(addedStats, talents, calcOpts, 0);
                LvBFS[0] = new LavaBurst(addedStats, talents, calcOpts, 1);
                FS[0] = new FlameShock(addedStats, talents, calcOpts);
                ES[0] = new EarthShock(addedStats, talents, calcOpts);
                FrS[0] = new FrostShock(addedStats, talents, calcOpts);
                TS[0] = new Thunderstorm(addedStats, talents, calcOpts);
            }
            else
            {
                int k;
                float delta = num > 1 ? 2f / (num - 1) : 0;
                for (k = 0; k < num; k++)
                {
                    Stats addedStats = baseStats.Clone();
                    addedStats.Accumulate(procStats, k * delta);
                    LB[k] = new LightningBolt(addedStats, talents, calcOpts);
                    CL[k] = new ChainLightning(addedStats, talents, calcOpts, 0);
                    CL3[k] = new ChainLightning(addedStats, talents, calcOpts, 3);
                    CL4[k] = new ChainLightning(addedStats, talents, calcOpts, 4);
                    LvB[k] = new LavaBurst(addedStats, talents, calcOpts, 0);
                    LvBFS[k] = new LavaBurst(addedStats, talents, calcOpts, 1);
                    FS[k] = new FlameShock(addedStats, talents, calcOpts);
                    ES[k] = new EarthShock(addedStats, talents, calcOpts);
                    FrS[k] = new FrostShock(addedStats, talents, calcOpts);
                    TS[k] = new Thunderstorm(addedStats, talents, calcOpts);
                }
            }
            #endregion
        }

        public Rotation getAvgRotation(int type)
        {
            Rotation result = getPriorityRotation(0, type);
            if (num == 1) return result;
            int k;
            for (k = 1; k < num; k++)
            {
                result += getPriorityRotation(k, type);
            }
            return result * (1f/num);
        }

        private Rotation getPriorityRotation(int ix, int type)
        {
            LightningBolt LB = (LightningBolt)this.LB[ix].Clone();
            LavaBurst LvB = (LavaBurst)this.LvBFS[ix].Clone();
            Thunderstorm TS = (Thunderstorm)this.TS[ix].Clone();
            FlameShock FS = (FlameShock)this.FS[ix].Clone();

            LavaBurst LvBNoFS = (LavaBurst)this.LvB[ix].Clone();
            ChainLightning CL = (ChainLightning)this.CL[ix].Clone();
            ChainLightning CL3 = (ChainLightning)this.CL3[ix].Clone();
            ChainLightning CL4 = (ChainLightning)this.CL4[ix].Clone();
            EarthShock ES = (EarthShock)this.ES[ix].Clone();
            FrostShock FrS = (FrostShock)this.FrS[ix].Clone();

            #region Elemental Mastery
            if (talents.ElementalMastery > 0)
            {
                float procs;
                SpecialEffect em = new SpecialEffect(Trigger.Use, new Stats { SpellCrit = 0.2f }, 15f, talents.GlyphofElementalMastery ? 150f : 180f);
                float EMmod = em.GetAverageUptime(talents.GlyphofElementalMastery ? 150f : 180f, 1f);
                LB.ApplyEM(EMmod);
                CL.ApplyEM(EMmod);
                LvB.ApplyEM(EMmod);
                TS.ApplyEM(EMmod);
                FS.ApplyEM(EMmod);
                CL3.ApplyEM(EMmod);
                CL4.ApplyEM(EMmod);
                ES.ApplyEM(EMmod);
                FrS.ApplyEM(EMmod);
                LvBNoFS.ApplyEM(EMmod);
            }
            #endregion
            #region Rotation
            /* Rotation
             * Always cast LvB when possible
             * If glyphed, cast FS right before every second Lava Burst
             * If unglyphed, cast FS immediately after every Lava Burst
             */
            // Thunderstorm
            // float timeBetweenTS = TS.CDRefreshTime; // cast whenever available
            // float castFractionTS = calcOpts.UseThunderstorm ? TS.CastTime / timeBetweenTS : 0;
            // Lava Burst
            float timeBetweenLvB = LvB.CDRefreshTime + LvB.CastTime; // cast whenever available
            // Flame Shock
            float timeBetweenFS = FS.PeriodicRefreshTime; // cast whenever DoT drops
            if (!talents.GlyphofFlameShock)
            {
                timeBetweenFS = Math.Max(timeBetweenLvB, FS.CDRefreshTime);
            }
            else
            {
                timeBetweenLvB = Math.Max(timeBetweenLvB, timeBetweenFS / 2);
                timeBetweenFS = 2 * timeBetweenLvB;
            }
            // Lightning Bolt
            float nLBfirst = (timeBetweenLvB - LvB.CastTime) / LB.CastTime;
            float nLBsecond = (timeBetweenLvB - LvB.CastTime - FS.CastTime) / LB.CastTime;
            if (!talents.GlyphofFlameShock) nLBfirst = nLBsecond;
            //float castFractionLB = 1f - castFractionFS - castFractionLvB; // LB casting time per second
            //float castsPerLvB = timeBetweenLvB * castFractionLB / LB.CastTime;
            float timeWasted = 0;
            // You can't cast a half LB
            // Options: 
            // 0. finish the cast both times 
            // 1. wait the first time, finish the second time
            // 2. finish the first time, wait the second time
            // 3. don't finish the cast, just wait both times
            if (type == 0)
            {
                float shift1 = ((float)Math.Ceiling(nLBfirst) - nLBfirst) * LB.CastTime;
                nLBfirst = (float)Math.Ceiling(nLBfirst);
                //with flameshock recast
                float shift2 = ((float)Math.Ceiling(nLBsecond) - nLBsecond) * LB.CastTime;
                nLBsecond = (float)Math.Ceiling(nLBsecond);
                if (!talents.GlyphofFlameShock)
                {
                    timeBetweenLvB += shift2;
                    timeBetweenFS = Math.Max(timeBetweenLvB, FS.CDRefreshTime);
                }
                else
                {
                    timeBetweenLvB += (shift1 + shift2) / 2;
                    timeBetweenLvB = Math.Max(timeBetweenLvB, timeBetweenFS / 2);
                    timeBetweenFS = 2 * timeBetweenLvB;
                }
            }
            else if (type == 1)
            {
                timeWasted += (nLBfirst - (float)Math.Floor(nLBfirst)) * LB.CastTime;
                nLBfirst = (float)Math.Floor(nLBfirst);
                float shift = ((float)Math.Ceiling(nLBsecond) - nLBsecond) * LB.CastTime;
                nLBsecond = (float)Math.Ceiling(nLBsecond);
                if (!talents.GlyphofFlameShock)
                {
                    timeBetweenLvB += shift;
                    timeBetweenFS = Math.Max(timeBetweenLvB, FS.CDRefreshTime);
                }
                else
                {
                    timeBetweenLvB += shift / 2;
                    timeBetweenLvB = Math.Max(timeBetweenLvB, timeBetweenFS / 2);
                    timeBetweenFS = 2 * timeBetweenLvB;
                }
            }
            else if (type == 2)
            {
                float shift = ((float)Math.Ceiling(nLBfirst) - nLBfirst) * LB.CastTime;
                nLBfirst = (float)Math.Ceiling(nLBfirst);
                timeWasted += (nLBsecond - (float)Math.Floor(nLBsecond)) * LB.CastTime;
                nLBsecond = (float)Math.Floor(nLBsecond);
                if (!talents.GlyphofFlameShock)
                {
                    timeBetweenFS = Math.Max(timeBetweenLvB, FS.CDRefreshTime);
                }
                else
                {
                    timeBetweenLvB += shift / 2;
                    timeBetweenLvB = Math.Max(timeBetweenLvB, timeBetweenFS / 2);
                    timeBetweenFS = 2 * timeBetweenLvB;
                }
            }
            else if (type == 3)
            {
                timeWasted += (nLBfirst - (float)Math.Floor(nLBfirst)) * LB.CastTime;
                nLBfirst = (float)Math.Floor(nLBfirst);
                timeWasted += (nLBsecond - (float)Math.Floor(nLBsecond)) * LB.CastTime;
                nLBsecond = (float)Math.Floor(nLBsecond);
            }
            float LvBcps = 1f / timeBetweenLvB;
            float FScps = 1f / timeBetweenFS;
            float LBcps = (nLBfirst + nLBsecond) / timeBetweenLvB * 2;
            float castFractionLvB = LvB.CastTime / timeBetweenLvB; // LvB casting time per second
            float castFractionFS = FS.CastTime / timeBetweenFS; // FS casting time per second
            float castFractionLB = LB.CastTime * (nLBfirst + nLBsecond) / (2 * timeBetweenLvB); // LB casting time per second
            float castFractionNone = timeWasted / (2 * timeBetweenLvB); // Time wasted per second
            float mpsFromTS = calcOpts.UseThunderstorm ? TS.ManaCost / TS.CDRefreshTime : 0;
            float dpsFromLvB = LvB.HitChance * LvB.TotalDamage / timeBetweenLvB;
            float mpsFromLvB = LvB.ManaCost / timeBetweenLvB;
            float dpsFromLB = LB.HitChance * LB.DpCT * castFractionLB;
            float mpsFromLB = LB.ManaCost / LB.CastTime * castFractionLB;
            float dpsFromFS = FS.HitChance * FS.TotalDamage / timeBetweenFS;
            if (!talents.GlyphofFlameShock)
            {
                float ticks = (float)Math.Min(
                    Math.Floor((timeBetweenFS - LvB.CastTime) / FS.PeriodicTickTime),
                    FS.PeriodicTicks);
                dpsFromFS = FS.HitChance * (FS.AvgDamage + FS.PeriodicTick * ticks) / timeBetweenFS;
            }
            float mpsFromFS = FS.ManaCost / timeBetweenFS;
            #endregion
            #region Lightning Overload
            float critLB = LB.CritChance * (1f + .11f * talents.LightningOverload);
            dpsFromLB *= 1f + .11f * talents.LightningOverload * .5f;
            #endregion
            #region Clearcasting
            float clearcastingFS = 0f, clearcastingLvB = 0f, clearcastingLB = 0f;
            if (talents.ElementalFocus > 0)
            {
                // each CCchance describes the probability that at least one of the two casts is a crit, no special casting order.
                // Elemental Oath is factored in through the buffs already.
                // two LBs
                float CCchance2LB = 1 - ((1 - critLB * LB.HitChance) * (1 - critLB * LB.HitChance));
                // LvB with FS active and LB
                float CCchanceLvBLB = 1 - ((1 - LvB.HitChance) * (1 - critLB * LB.HitChance));
                // LvB with FS active and FS
                float CCchanceLvBFS = 1 - ((1 - LvB.HitChance) * (1 - FS.CritChance * FS.HitChance));
                // LB and FS
                float CCchanceLBFS = 1 - ((1 - critLB * LB.HitChance) * (1 - FS.CritChance * FS.HitChance));
                if (talents.GlyphofFlameShock)
                {
                    clearcastingLvB = (CCchanceLBFS + CCchance2LB) / 2f;
                    clearcastingFS = CCchance2LB;
                    clearcastingLB = (
                        Math.Max(nLBsecond + nLBfirst - 4, 0) * CCchance2LB + //n3..nn + m3..mm
                        Math.Min(1, nLBsecond) * CCchanceLvBLB + //m1
                        Math.Min(2, Math.Max(0, nLBsecond + nLBfirst - 2)) * CCchanceLvBLB + //n2 + m2
                        Math.Min(1, nLBfirst) * CCchanceLvBFS //n1 
                        ) / (nLBsecond + nLBfirst);
                }
                else
                {
                    clearcastingLvB = CCchance2LB;
                    clearcastingFS = CCchanceLvBLB;
                    clearcastingLB = (
                        Math.Max(nLBsecond + nLBfirst - 4, 0) * CCchance2LB + //n3..nn + m3..mm
                        Math.Min(2, Math.Max(0, nLBsecond + nLBfirst - 2)) * CCchanceLBFS + //n2 + m2
                        Math.Min(2, nLBsecond + nLBfirst) * CCchanceLvBFS //n1 + m1
                        ) / (nLBsecond + nLBfirst);
                }
                mpsFromLB *= 1 - .4f * clearcastingLB;
                mpsFromLvB *= 1 - .4f * clearcastingLvB;
                mpsFromFS *= 1 - .4f * clearcastingFS;
                dpsFromLB *= 1 + .05f * talents.ElementalOath * clearcastingLB;
                dpsFromLvB *= 1 + .05f * talents.ElementalOath * clearcastingLvB;
                dpsFromFS *= 1 + .05f * talents.ElementalOath * clearcastingFS;
            }
            #endregion

            return new Rotation()
            {
                DPS = dpsFromFS + dpsFromLvB + dpsFromLB,
                MPS = mpsFromFS + mpsFromLvB + mpsFromLB + mpsFromTS,
                CastFraction = (
                    castFractionFS / FS.CastTime +
                    castFractionLvB / LvB.CastTime +
                    castFractionLB / LB.CastTime),
                CritFraction = (
                    FS.CritChance * castFractionFS / FS.CastTime +
                    LvB.CritChance * castFractionLvB / LvB.CastTime +
                    critLB * castFractionLB / LB.CastTime),
                MissFraction = (
                    FS.MissChance * castFractionFS / FS.CastTime +
                    LvB.MissChance * castFractionLvB / LvB.CastTime +
                    LB.MissChance * castFractionLB / LB.CastTime),
                LB = LB,
                CL = CL,
                CL3 = CL3,
                CL4 = CL4,
                LvB = LvBNoFS,
                LvBFS = LvB,
                FS = FS,
                ES = ES,
                FrS = FrS,
                CC_FS = clearcastingFS,
                CC_LvB = clearcastingLvB,
                CC_LB = clearcastingLB,
                LBFraction = castFractionLB,
                LvBFraction = castFractionLvB,
                FSFraction = castFractionFS,
                LBPerSecond = LBcps,
                LvBPerSecond = LvBcps,
                FSPerSecond = FScps,
                nLBfirst = nLBfirst,
                nLBsecond = nLBsecond,
                WaitAfterLB = timeWasted
            };
        }

        public static void solve(CharacterCalculationsElemental calculatedStats, CalculationOptionsElemental calcOpts)
        {
            Stats stats = calculatedStats.BasicStats;
            Character character = calculatedStats.LocalCharacter;
            ShamanTalents talents = character.ShamanTalents;

            /* Effects:
             * Clearcasting (-40% mana cost next 2 spells)
             * Glyph of flame shock or not
             * Clearcasting (5/10% more total damage)
             * Elemental Mastery (+15% crit chance, 15 sec/3 min cd)
             * Trinkets
             * 
             * Assume LvB used on CD and FS either after LvB, on dot drop or before LvB
             * Filler: LB 
             * NYI Optional: use CL after every LB
             * Optional: finish LB cast, or wait until LvB available
             */
            Estimation e;
            Rotation rot;
            float damage;
            Stats procStats;
            // WITHOUT PROCS
            e = new Estimation(stats, new Stats{}, talents, calcOpts, 1);
            rot = e.getAvgRotation(calcOpts.rotationType);
            // WITH PROCS
            int nPasses = 2, k;
            for (k = 0; k < nPasses; k++)
            {
                procStats = getTrinketStats(character, stats, calcOpts.FightDuration, rot);
                e = new Estimation(stats, procStats, talents, calcOpts, 4+k); // 4+k
                rot = e.getAvgRotation(calcOpts.rotationType);
            }

            /* Regen variables: (divide by 5 for regen per second)
             * While casting: ManaRegInFSR
             * During regen: ManaRegOutFSR */
            #region Calculate Regen
            float spiRegen = 5 * StatConversion.GetSpiritRegenSec(stats.Spirit, stats.Intellect);
            float replenishRegen = stats.Mana * 0.0025f * 5 * (calcOpts.ReplenishmentUptime / 100f);
            float ManaRegInFSR = spiRegen * stats.SpellCombatManaRegeneration + stats.Mp5 + replenishRegen;
            float ManaRegOutFSR = spiRegen + stats.Mp5 + replenishRegen;
            float ManaRegen = ManaRegInFSR;
            #endregion

            // Mana potion: extraMana
            #region Mana potion
            float extraMana = new float[] { 0f, 1800f, 2200, 2400, 4300 }[calcOpts.ManaPot];
            extraMana *= 1 + stats.BonusManaPotion;
            #endregion

            // Thunderstorm usage
            #region Thunderstorm
            if (calcOpts.UseThunderstorm)
            {
                float procsPerSecond = Thunderstorm.getProcsPerSecond(talents.GlyphofThunder, calcOpts.FightDuration);
                rot.MPS -= (talents.GlyphofThunderstorm ? .1f : .08f) * stats.Mana * procsPerSecond;
            }
            #endregion

            // TotalDamage, CastFraction, TimeUntilOOM
            #region Calculate total damage in the fight
            float TimeUntilOOM = 0;
            float FightDuration = calcOpts.FightDuration;
            float effectiveMPS = rot.MPS - ManaRegen / 5f;
            if (effectiveMPS <= 0) TimeUntilOOM = FightDuration;
            else TimeUntilOOM = (calculatedStats.BasicStats.Mana + extraMana) / effectiveMPS;
            if (TimeUntilOOM > FightDuration) TimeUntilOOM = FightDuration;

            #region SpecialEffects from procs etc.
            procStats = getTrinketStats(character, stats, calcOpts.FightDuration, rot);
            damage = procStats.ArcaneDamage + procStats.NatureDamage + procStats.FireDamage + procStats.ShadowDamage;
            damage *= (1 + stats.SpellCrit); //is only spellcrit affecting damage procs (Thunder Capacitor etc.) ?
            rot.DPS += damage;
            #endregion

            float TotalDamage = TimeUntilOOM * rot.DPS;
            float TimeToRegenFull = 5f * calculatedStats.BasicStats.Mana / ManaRegOutFSR;
            float TimeToBurnAll = calculatedStats.BasicStats.Mana / effectiveMPS;
            float CastFraction = 1f;
            if (ManaRegOutFSR > 0 && FightDuration > TimeUntilOOM)
            {
                float timeLeft = FightDuration - TimeUntilOOM;
                if (TimeToRegenFull + TimeToBurnAll == 0)
                    CastFraction = 0;
                else
                    CastFraction = TimeToBurnAll / (TimeToRegenFull + TimeToBurnAll);
                TotalDamage += timeLeft * rot.DPS * CastFraction;
            }
            #endregion

            float bsRatio = ((float)calcOpts.BSRatio) * 0.01f;
            calculatedStats.BurstPoints = (1f - bsRatio) * 2f * rot.DPS;
            calculatedStats.SustainedPoints = bsRatio * 2f * TotalDamage / FightDuration;
            calculatedStats.OverallPoints = calculatedStats.BurstPoints + calculatedStats.SustainedPoints;
            calculatedStats.ManaRegenInFSR = spiRegen * calculatedStats.BasicStats.SpellCombatManaRegeneration + calculatedStats.BasicStats.Mp5;
            calculatedStats.ManaRegenOutFSR = spiRegen + calculatedStats.BasicStats.Mp5;
            calculatedStats.ReplenishMP5 = replenishRegen;
            calculatedStats.LightningBolt = rot.LB;
            calculatedStats.ChainLightning = rot.CL;
            calculatedStats.ChainLightning3 = rot.CL3;
            calculatedStats.ChainLightning4 = rot.CL4;
            calculatedStats.FlameShock = rot.FS;
            calculatedStats.LavaBurst = rot.LvB;
            calculatedStats.EarthShock = rot.ES;
            calculatedStats.FrostShock = rot.FrS;
            calculatedStats.TimeToOOM = TimeUntilOOM;
            calculatedStats.CastRegenFraction = CastFraction;
            calculatedStats.CastFraction = rot.CastFraction;
            calculatedStats.CritFraction = rot.CritFraction;
            calculatedStats.MissFraction = rot.MissFraction;
            calculatedStats.LvBFraction = rot.LvBFraction;
            calculatedStats.FSFraction = rot.FSFraction;
            calculatedStats.LBFraction = rot.LBFraction;
            calculatedStats.LvBPerSecond = rot.LvBPerSecond;
            calculatedStats.LBPerSecond = rot.LBPerSecond;
            calculatedStats.FSPerSecond = rot.FSPerSecond;
            calculatedStats.RotationDPS = rot.DPS;
            calculatedStats.RotationMPS = rot.MPS;
            calculatedStats.TotalDPS = TotalDamage / FightDuration;
            calculatedStats.ClearCast_FlameShock = rot.CC_FS;
            calculatedStats.ClearCast_LavaBurst = rot.CC_LvB;
            calculatedStats.ClearCast_LightningBolt = rot.CC_LB;
            calculatedStats.nLBfirst = rot.nLBfirst;
            calculatedStats.nLBsecond = rot.nLBsecond;
            calculatedStats.WaitAfterLB = rot.WaitAfterLB;
        }

        private static Stats getTrinketStats(Character character, Stats stats, float FightDuration, Rotation rot)
        {
            Stats statsAverage = new Stats();
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                float trigger = 0f; // 1 / frequency in Hz
                float procChance = 1f;
                if (effect.Trigger == Trigger.DamageDone)
                {
                    trigger = 1f / ( rot.getCastsPerSecond() + 1f/rot.FS.PeriodicTickTime );
                    procChance = rot.getWeightedHitchance(); //flameshock ticks are not taken into account. the chance would be slightly higher.
                }
                else if (effect.Trigger == Trigger.SpellMiss)
                {
                    trigger = 1f / rot.getCastsPerSecond();
                    procChance = 1f - rot.getWeightedHitchance();
                }
                else if (effect.Trigger == Trigger.SpellHit || effect.Trigger == Trigger.DamageSpellHit)
                {
                    trigger = 1f / rot.getCastsPerSecond();
                    procChance = rot.getWeightedHitchance();
                }
                else if (effect.Trigger == Trigger.DoTTick)
                {
                    trigger = 1f / rot.FS.PeriodicTickTime;
                }
                else if (effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.DamageSpellCast)
                {
                    trigger = 1f / rot.getCastsPerSecond();
                }
                else if (effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.DamageSpellCrit)
                {
                    trigger = 1f / rot.getCastsPerSecond();
                    procChance = rot.getWeightedCritchance(character.ShamanTalents.LightningOverload);
                }
                else if (effect.Trigger == Trigger.ShamanLightningBolt)
                {
                    trigger = 1f / rot.LBPerSecond;
                }
                else if (effect.Trigger == Trigger.ShamanShock)
                {
                    trigger = 1f / rot.FSPerSecond;
                }
                else if (effect.Trigger == Trigger.Use)
                {
                    trigger = 1f;
                }
                else
                    continue;
                
                if (effect.MaxStack > 1)
                    statsAverage += effect.Stats * effect.GetAverageStackSize(trigger, procChance, 3f, FightDuration);
                else
                    statsAverage += effect.GetAverageStats(trigger, procChance, 3f, FightDuration);
                //float chance = effect.GetAverageUptime(trigger, procChance, 3f, FightDuration);
            }
            return statsAverage;
        }
    }
}
