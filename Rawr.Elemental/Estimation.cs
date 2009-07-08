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
                LB[0] = new LightningBolt(baseStats + procStats, talents, calcOpts);
                CL[0] = new ChainLightning(baseStats + procStats, talents, calcOpts, 0);
                CL3[0] = new ChainLightning(baseStats + procStats, talents, calcOpts, 3);
                CL4[0] = new ChainLightning(baseStats + procStats, talents, calcOpts, 4);
                LvB[0] = new LavaBurst(baseStats + procStats, talents, calcOpts, 0);
                LvBFS[0] = new LavaBurst(baseStats + procStats, talents, calcOpts, 1);
                FS[0] = new FlameShock(baseStats + procStats, talents, calcOpts);
                ES[0] = new EarthShock(baseStats + procStats, talents, calcOpts);
                FrS[0] = new FrostShock(baseStats + procStats, talents, calcOpts);
                TS[0] = new Thunderstorm(baseStats + procStats, talents, calcOpts);
            }
            else
            {
                int k;
                float delta = num > 1 ? 2f / (num - 1) : 0;
                for (k = 0; k < num; k++)
                {
                    LB[k] = new LightningBolt(baseStats + procStats * (k * delta), talents, calcOpts);
                    CL[k] = new ChainLightning(baseStats + procStats * (k * delta), talents, calcOpts, 0);
                    CL3[k] = new ChainLightning(baseStats + procStats * (k * delta), talents, calcOpts, 3);
                    CL4[k] = new ChainLightning(baseStats + procStats * (k * delta), talents, calcOpts, 4);
                    LvB[k] = new LavaBurst(baseStats + procStats * (k * delta), talents, calcOpts, 0);
                    LvBFS[k] = new LavaBurst(baseStats + procStats * (k * delta), talents, calcOpts, 1);
                    FS[k] = new FlameShock(baseStats + procStats * (k * delta), talents, calcOpts);
                    ES[k] = new EarthShock(baseStats + procStats * (k * delta), talents, calcOpts);
                    FrS[k] = new FrostShock(baseStats + procStats * (k * delta), talents, calcOpts);
                    TS[k] = new Thunderstorm(baseStats + procStats * (k * delta), talents, calcOpts);
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
                float EMmod = SpecialEffect.EstimateUptime(15f, talents.GlyphofElementalMastery ? 150f : 180f, 0, calcOpts.FightDuration, out procs);
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
            float timeBetweenLvB = LvB.CDRefreshTime; // cast whenever available
            // Flame Shock
            float timeBetweenFS = FS.PeriodicRefreshTime; // cast whenever DoT drops
            if (!talents.GlyphofFlameShock)
            {
                timeBetweenFS = Math.Max(timeBetweenLvB, FS.CDRefreshTime);
            }
            else
            {
                timeBetweenLvB = Math.Max(LvB.CDRefreshTime, timeBetweenFS / 2);
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
                float shift = ((float)Math.Ceiling(nLBfirst) - nLBfirst) * LB.CastTime;
                nLBfirst = (float)Math.Ceiling(nLBfirst);
                shift += ((float)Math.Ceiling(nLBsecond) - nLBsecond) * LB.CastTime;
                nLBsecond = (float)Math.Ceiling(nLBsecond);
                timeBetweenLvB += shift;
                if (!talents.GlyphofFlameShock)
                {
                    timeBetweenFS = Math.Max(timeBetweenLvB, FS.CDRefreshTime);
                }
                else
                {
                    timeBetweenLvB = Math.Max(LvB.CDRefreshTime, timeBetweenFS / 2);
                    timeBetweenFS = 2 * timeBetweenLvB;
                }
            }
            else if (type == 1)
            {
                timeWasted += (nLBfirst - (float)Math.Floor(nLBfirst)) * LB.CastTime;
                nLBfirst = (float)Math.Floor(nLBfirst);
                float shift = ((float)Math.Ceiling(nLBsecond) - nLBsecond) * LB.CastTime;
                nLBsecond = (float)Math.Ceiling(nLBsecond);
                timeBetweenLvB += shift;
                if (!talents.GlyphofFlameShock)
                {
                    timeBetweenFS = Math.Max(timeBetweenLvB, FS.CDRefreshTime);
                }
                else
                {
                    timeBetweenLvB = Math.Max(LvB.CDRefreshTime, timeBetweenFS / 2);
                    timeBetweenFS = 2 * timeBetweenLvB;
                }
            }
            else if (type == 2)
            {
                float shift = ((float)Math.Ceiling(nLBfirst) - nLBfirst) * LB.CastTime;
                nLBfirst = (float)Math.Ceiling(nLBfirst);
                timeWasted += (nLBfirst - (float)Math.Floor(nLBsecond)) * LB.CastTime;
                nLBsecond = (float)Math.Floor(nLBsecond);
                timeBetweenLvB += shift;
                if (!talents.GlyphofFlameShock)
                {
                    timeBetweenFS = Math.Max(timeBetweenLvB, FS.CDRefreshTime);
                }
                else
                {
                    timeBetweenLvB = Math.Max(LvB.CDRefreshTime, timeBetweenFS / 2);
                    timeBetweenFS = 2 * timeBetweenLvB;
                }
            }
            else if (type == 3)
            {
                timeWasted += (nLBfirst - (float)Math.Floor(nLBfirst)) * LB.CastTime;
                nLBfirst = (float)Math.Floor(nLBfirst);
                timeWasted += (nLBfirst - (float)Math.Floor(nLBsecond)) * LB.CastTime;
                nLBsecond = (float)Math.Floor(nLBsecond);
            }
            float castFractionLvB = LvB.CastTime / timeBetweenLvB; // LvB casting time per second
            float castFractionFS = FS.CastTime / timeBetweenFS; // FS casting time per second
            float castFractionLB = LB.CastTime * (nLBfirst + nLBsecond) / (2 * timeBetweenLvB);
            float mpsFromTS = calcOpts.UseThunderstorm ? TS.ManaCost / TS.CDRefreshTime : 0;
            float dpsFromLvB = LvB.HitChance * LvB.TotalDamage / timeBetweenLvB;
            float mpsFromLvB = LvB.ManaCost / timeBetweenLvB;
            float dpsFromLB = LB.HitChance * LB.DpCT * castFractionLB;
            float mpsFromLB = castFractionLB * LB.ManaCost / LB.CastTime;
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
                float CCchance2LB = 1 - ((1 - critLB * LB.HitChance) * (1 - critLB * LB.HitChance));
                float CCchanceLvBLB = 1 - ((1 - LvB.HitChance) * (1 - critLB * LB.HitChance));
                float CCchanceLvBFS = 1 - ((1 - LvB.HitChance) * (1 - FS.CritChance * FS.HitChance));
                float CCchanceLBFS = 1 - ((1 - critLB * LB.HitChance) * (1 - FS.CritChance * FS.HitChance));
                if (talents.GlyphofFlameShock)
                {
                    clearcastingLvB = (CCchanceLBFS + CCchance2LB) / 2f;
                    clearcastingFS = CCchance2LB;
                    clearcastingLB = (
                        Math.Max(nLBsecond + nLBfirst - 4, 0) * CCchance2LB +
                        Math.Min(2, nLBsecond + nLBfirst - 2) * CCchanceLvBLB +
                        Math.Min(1, nLBsecond) * CCchanceLvBLB +
                        Math.Min(1, nLBfirst) * CCchanceLvBFS
                        ) / (nLBsecond + nLBfirst);
                }
                else
                {
                    clearcastingLvB = CCchance2LB;
                    clearcastingFS = CCchanceLvBLB;
                    clearcastingLB = (
                        Math.Max(nLBsecond + nLBfirst - 4, 0) * CCchance2LB +
                        Math.Min(2, nLBsecond + nLBfirst - 2) * CCchanceLvBLB +
                        Math.Min(2, nLBsecond + nLBfirst) * CCchanceLvBFS
                        ) / (nLBsecond + nLBfirst);
                }
                mpsFromLB *= 1 - .4f * clearcastingLB;
                mpsFromLvB *= 1 - .4f * clearcastingLvB;
                mpsFromFS *= 1 - .4f * clearcastingFS;
                dpsFromLB *= 1 + .05f * clearcastingLB * talents.ElementalOath;
                dpsFromLvB *= 1 + .05f * clearcastingLvB * talents.ElementalOath;
                dpsFromFS *= 1 + .05f * clearcastingFS * talents.ElementalOath;
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
                LBPerSecond = castFractionLB / LB.CastTime,
                LvBPerSecond = castFractionLvB / LvB.CastTime,
                FSPerSecond = castFractionFS / FS.CastTime,
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
             * Elemental Mastery (+20% crit chance, -20% mana cost, 30 sec/3 min cd)
             * Trinkets
             * 
             * Assume LvB used on CD and FS either after LvB, on dot drop or before LvB
             * Filler: LB 
             * NYI Optional: use CL after every LB
             * Optional: finish LB cast, or wait until LvB available
             */

            #region Lightning Bolt Haste Trinket
            stats += new Stats
            {
                HasteRating = stats.LightningBoltHasteProc_15_45 * 10f / 55f,
            };
            #endregion

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
                procStats = getTrinketStats(character, stats, calcOpts.FightDuration, rot.CastFraction, rot.CritFraction, rot.MissFraction, 1f / 3f, out damage);
                e = new Estimation(stats, procStats, talents, calcOpts, 4+k); // 4+k
                rot = e.getAvgRotation(calcOpts.rotationType);
            }

            /* Regen variables: (divide by 5 for regen per second)
             * While casting: ManaRegInFSR
             * While casting: ManaRegOutFSR (stop casting, but keep trinket effects)
             * During regen: ManaRegOutFSRNoCasting */
            #region Calculate Regen
            float spiRegen = CalculateManaRegen(stats.Intellect, stats.Spirit);
            float spiRegenMDF = CalculateManaRegen(stats.Intellect, stats.ExtraSpiritWhileCasting + stats.Spirit);
            float replenishRegen = stats.Mana * 0.0025f * 5 * (calcOpts.ReplenishmentUptime / 100f);
            float ManaRegInFSR = spiRegenMDF * stats.SpellCombatManaRegeneration + stats.Mp5 + replenishRegen;
            float ManaRegOutFSR = spiRegenMDF + stats.Mp5 + replenishRegen;
            float ratio_extraspi = 0.8f; // OK, lets assume a mana starved person keeps 80% of the extra spirit effect, because they will keep casting anyway
            float ManaRegOutFSRNoCasting = (1 - ratio_extraspi) * spiRegen + ratio_extraspi * spiRegenMDF + stats.Mp5 + replenishRegen;
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
                float procs;
                SpecialEffect.EstimateUptime(0, 45f, 0, calcOpts.FightDuration, out procs);
                rot.MPS -= .08f * stats.Mana * procs / calcOpts.FightDuration;
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

            #region Effect from Darkmoon card: Death and Pendulum and Thunder/Lightning Capacitor
            getTrinketStats(character, stats, calcOpts.FightDuration, rot.CastFraction, rot.CritFraction, rot.MissFraction, 1f / 3f, out damage);
            damage *= (1 + stats.SpellCrit);
            rot.DPS += damage / calcOpts.FightDuration;
            #endregion

            float TotalDamage = TimeUntilOOM * rot.DPS;
            float TimeToRegenFull = 5f * calculatedStats.BasicStats.Mana / ManaRegOutFSRNoCasting;
            float TimeToBurnAll = calculatedStats.BasicStats.Mana / effectiveMPS;
            float CastFraction = 1f;
            if (ManaRegOutFSRNoCasting > 0 && FightDuration > TimeUntilOOM)
            {
                float timeLeft = FightDuration - TimeUntilOOM;
                if (TimeToRegenFull + TimeToBurnAll == 0) CastFraction = 0;
                else CastFraction = TimeToBurnAll / (TimeToRegenFull + TimeToBurnAll);
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

        private static Stats getTrinketStats(Character character, Stats stats, float FightDuration, float HitsFraction, float CritsFraction, float MissFraction, float TickFraction, out float Damage)
        {
            SpecialEffects effects = new SpecialEffects(stats);
            Stats result = effects.estimateAll(FightDuration, HitsFraction + MissFraction, HitsFraction, CritsFraction, MissFraction, TickFraction, out Damage);

            return new Stats()
            {
                Spirit = result.Spirit,
                HasteRating = result.HasteRating,
                //SpellHaste = character.StatConversion.GetSpellHasteFromRating(result.HasteRating) / 100f,
                SpellPower = result.SpellPower,
                Mp5 = result.Mp5,
                SpellCombatManaRegeneration = result.SpellCombatManaRegeneration,
            };
        }

        private static float CalculateManaRegen(float intel, float spi)
        {
            float baseRegen = 0.005575f;
            return (float)Math.Round(5f * (0.001f + (float)Math.Sqrt(intel) * spi * baseRegen));
        }
    }
}
