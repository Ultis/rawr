using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Elemental.Estimation
{
    public static class Estimation
    {
        public static Rotation getRotation(Stats stats, ShamanTalents talents, CalculationOptionsElemental calcOpts)
        {
            #region Spells
            Spell LB = new LightningBolt(stats, talents, calcOpts);
            Spell CL = new ChainLightning(stats, talents, calcOpts, 0);
            Spell CL3 = new ChainLightning(stats, talents, calcOpts, 3);
            Spell CL4 = new ChainLightning(stats, talents, calcOpts, 4);
            Spell LvB = new LavaBurst(stats, talents, calcOpts, 0);
            Spell LvBFS = new LavaBurst(stats, talents, calcOpts, 1);
            Spell FS = new FlameShock(stats, talents, calcOpts);
            Spell ES = new EarthShock(stats, talents, calcOpts);
            Spell FrS = new FrostShock(stats, talents, calcOpts);
            Spell TS = new Thunderstorm(stats, talents, calcOpts);
            #endregion
            #region Elemental Mastery
            if (talents.ElementalMastery > 0)
            {
                float EMmod = CalculateEffect(1f, 30f, 0f, calcOpts.glyphOfElementalMastery ? 150f : 180f, calcOpts.FightDuration);
                LB.ApplyEM(EMmod);
                CL.ApplyEM(EMmod);
                CL3.ApplyEM(EMmod);
                CL4.ApplyEM(EMmod);
                LvB.ApplyEM(EMmod);
                LvBFS.ApplyEM(EMmod);
                FS.ApplyEM(EMmod);
                ES.ApplyEM(EMmod);
                FrS.ApplyEM(EMmod);
            }
            #endregion
            #region Rotation
            float timeBetweenTS = TS.CDRefreshTime;
            float timeBetweenFS = FS.PeriodicRefreshTime; // cast whenever DoT drops
            float timeBetweenLvB = LvB.CDRefreshTime; // cast whenever available
            float castFractionTS = TS.CastTime / timeBetweenTS;
            if (!calcOpts.UseThunderstorm) castFractionTS = 0f;
            float castFractionFS = FS.CastTime / timeBetweenFS; // FS casting time per second
            float castFractionLvBFS = LvBFS.CastTime / timeBetweenLvB; // LvB casting time per second
            float castFractionLB = 1f - castFractionFS - castFractionLvBFS - castFractionTS; // LB casting time per second
            float dpsFromTS = 0f; // assume no targets hit
            float dpsFromFS = FS.HitChance * FS.TotalDamage / timeBetweenFS;
            float dpsFromLvB = LvBFS.HitChance * LvBFS.TotalDamage / timeBetweenLvB;
            float dpsFromLB = LB.HitChance * LB.DpCT * castFractionLB;
            float mpsFromTS = TS.ManaCost / timeBetweenTS;
            if (!calcOpts.UseThunderstorm) mpsFromTS = 0f; // 0 anyway but whatever
            float mpsFromFS = FS.ManaCost / timeBetweenFS;
            float mpsFromLvB = LvBFS.ManaCost / timeBetweenLvB;
            float mpsFromLB = castFractionLB * LB.ManaCost / LB.CastTime;
            float castsPerLvB = timeBetweenLvB * castFractionLB;
            #endregion
            #region Lightning Overload
            float critLB = LB.CritChance * (1f + .04f * talents.LightningOverload);
            dpsFromLB *= 1f + .04f * talents.LightningOverload * .5f;
            #endregion
            #region Clearcasting
            float clearcastingFS = 0f, clearcastingLvB = 0f, clearcastingLB = 0f;
            if (talents.ElementalFocus > 0)
            {
                float CCchance2LB = 1 - ((1 - critLB * LB.HitChance) * (1 - critLB * LB.HitChance));
                float CCchanceLvBLB = 1 - ((1 - LvBFS.HitChance) * (1 - critLB * LB.HitChance));
                clearcastingLB = (
                    Math.Max(castsPerLvB - 2, 0) * CCchance2LB +
                    Math.Min(1, castsPerLvB) * CCchanceLvBLB +
                    Math.Min(LvBFS.HitChance, castsPerLvB - 1) * CCchanceLvBLB
                    ) / castsPerLvB;
                clearcastingFS = CCchance2LB;
                clearcastingLvB = CCchance2LB;
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
                DPS = dpsFromFS + dpsFromLvB + dpsFromLB + dpsFromTS,
                MPS = mpsFromFS + mpsFromLvB + mpsFromLB + mpsFromTS,
                CastFraction = (
                    castFractionTS / TS.CastTime +
                    castFractionFS / FS.CastTime +
                    castFractionLvBFS / LvBFS.CastTime +
                    castFractionLB / LB.CastTime),
                CritFraction = (
                    TS.CritChance * castFractionTS / TS.CastTime +
                    FS.CritChance * castFractionFS / FS.CastTime +
                    LvBFS.CritChance * castFractionLvBFS / LvBFS.CastTime +
                    critLB * castFractionLB / LB.CastTime),
                MissFraction = (
                    TS.MissChance * castFractionTS / TS.CastTime +
                    FS.MissChance * castFractionFS / FS.CastTime +
                    LvBFS.MissChance * castFractionLvBFS / LvBFS.CastTime +
                    LB.MissChance * castFractionLB / LB.CastTime),
                LB = LB,
                CL = CL,
                CL3 = CL3,
                CL4 = CL4,
                LvB = LvB,
                LvBFS = LvBFS,
                FS = FS,
                ES = ES,
                FrS = FrS,
                CC_FS = clearcastingFS,
                CC_LvB = clearcastingLvB,
                CC_LB = clearcastingLB
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
             * Optional: use CL after every LB
             */

            // Assume: glyph of flame shock, LvB on every cd, refresh FS when it falls off, no CL use
            #region Lightning Bolt Haste Trinket
            stats += new Stats
            {
                SpellHaste = character.StatConversion.GetSpellHasteFromRating(stats.LightningBoltHasteProc_15_45 * 10f / 55f) / 100f,
            };
            #endregion

            Rotation rot = getRotation(stats, talents, calcOpts);
            stats += getTrinketStats(character, stats, calcOpts.FightDuration, rot.CastFraction, rot.CritFraction, rot.MissFraction);
            rot = getRotation(stats, talents, calcOpts);

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
                rot.MPS -= CalculateEffect(.08f * stats.Mana, 0f, 45f, 0f, calcOpts.FightDuration);
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
            rot.DPS += CalculateEffect(stats.DarkmoonCardDeathProc, 0f, 45f, 1 / (rot.CastFraction * .35f), TimeUntilOOM);
            if (stats.PendulumOfTelluricCurrentsProc > 0)
            {
                rot.DPS += CalculateEffect(1460 * (1 + stats.SpellCrit), 0f, 45f, 1 / (rot.CastFraction * .15f), TimeUntilOOM);
            }
            if (stats.ThunderCapacitorProc > 0)
            {
                rot.DPS += CalculateEffect(1276 * (1 + stats.SpellCrit), 0f, 0f, Math.Max(10f, 1 / (rot.CritFraction / 4f)), TimeUntilOOM);
            }
            if (stats.LightningCapacitorProc > 0)
            {
                rot.DPS += CalculateEffect(750 * (1 + stats.SpellCrit), 0f, 0f, Math.Max(7.5f, 1 / (rot.CritFraction / 3f)), TimeUntilOOM);
            }
            if (stats.ExtractOfNecromanticPowerProc > 0)
            {
                // Happens every 3 seconds.... 10% chance
                rot.DPS += CalculateEffect(550 * (1 + stats.SpellCrit), 0f, 0f, Math.Max(7.5f, 1 / (.1f / 3f)), TimeUntilOOM);
            }
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
            calculatedStats.RotationDPS = rot.DPS;
            calculatedStats.RotationMPS = rot.MPS;
            calculatedStats.TotalDPS = TotalDamage / FightDuration;
            calculatedStats.ClearCast_FlameShock = rot.CC_FS;
            calculatedStats.ClearCast_LavaBurst = rot.CC_LvB;
            calculatedStats.ClearCast_LightningBolt = rot.CC_LB;
        }

        private static float CalculateEffect(float value, float duration, float icd, float ecd, float FightDuration)
        {
            if (ecd + icd > FightDuration) return 0f;
            if (duration == 0)
            {
                float activity = (float)Math.Floor(FightDuration / (ecd + icd));
                activity /= FightDuration;
                return value * activity;
            }
            else
            {
                float activity = (float)Math.Floor(FightDuration / (ecd + icd)) * duration +
                    Math.Min(Math.Max(0, (FightDuration % (ecd + icd)) - ecd), duration);
                activity /= FightDuration;
                return value * activity;
            }
        }

        private static Stats getTrinketStats(Character character, Stats stats, float FightDuration, float HitsFraction, float CritsFraction, float MissFraction)
        {
            float Power = 0f, Haste = 0f, Mp5 = 0f, SpellCombatManaRegeneration = 0f, Spirit = 0f;

            Power += CalculateEffect(stats.SpellPowerFor10SecOnHit_10_45, 10f, 45f, 1 / (HitsFraction * .10f), FightDuration);
            Power += CalculateEffect(stats.SpellPowerFor10SecOnResist, 10f, 0f, 1 / (MissFraction * .10f), FightDuration);
            Power += CalculateEffect(stats.SpellPowerFor10SecOnCast_10_45, 10f, 45f, 1 / (HitsFraction * .10f), FightDuration);
            Power += CalculateEffect(stats.SpellPowerFor10SecOnCast_15_45, 10f, 45f, 1 / (HitsFraction * .15f), FightDuration);
            Power += CalculateEffect(stats.SpellPowerFor10SecOnCrit_20_45, 10f, 45f, 1 / (CritsFraction * .20f), FightDuration);
            Power += CalculateEffect(stats.SpellPowerFor15SecOnCrit_20_45, 15f, 45f, 1 / (CritsFraction * .20f), FightDuration);
            Power += CalculateEffect(stats.SpellPowerFor15SecOnUse90Sec, 15f, 90f, 0, FightDuration);
            Power += CalculateEffect(stats.SpellPowerFor15SecOnUse2Min, 15f, 120f, 0, FightDuration);
            Power += CalculateEffect(stats.SpellPowerFor20SecOnUse2Min, 15f, 120f, 0, FightDuration);
            Power += CalculateEffect(stats.SpellPowerFor20SecOnUse5Min, 15f, 120f, 0, FightDuration);

            Haste += CalculateEffect(stats.SpellHasteFor10SecOnCast_10_45, 10f, 45f, 1 / (HitsFraction * .10f), FightDuration);
            Haste += CalculateEffect(stats.SpellHasteFor6SecOnCast_15_45, 6f, 45f, 1 / (HitsFraction * .15f), FightDuration);
            Haste += CalculateEffect(stats.SpellHasteFor6SecOnHit_10_45, 6f, 45f, 1 / (HitsFraction * .10f), FightDuration);
            Haste += CalculateEffect(stats.HasteRatingFor20SecOnUse2Min, 20f, 120f, 0, FightDuration);
            // Each spell cast within 20 seconds will grant a stacking bonus of 21 mana regen per 5 sec. 
            // Expires after 20 seconds.  Abilities with no mana cost will not trigger this trinket.
            // For easy calculation, model this as a full stack for 20 seconds. (Real effect is higher)
            Mp5 += CalculateEffect(stats.Mp5OnCastFor20SecOnUse2Min * 20f * HitsFraction, 20f, 120f, 0, FightDuration);
            Mp5 += CalculateEffect(5f / 15f * stats.ManaRestoreOnCast_10_45, 15f, 45f, 1 / (HitsFraction * .10f), FightDuration);
            Mp5 += CalculateEffect(stats.ManaRestoreOnCrit_25_45, 0f, 45f, 1 / (CritsFraction * .25f), FightDuration);
            Mp5 += CalculateEffect(5f / 12f * stats.ManaregenOver20SecOnUse3Min, 12f, 180f, 0, FightDuration);
            Mp5 += CalculateEffect(5f / 12f * stats.ManaregenOver20SecOnUse5Min, 12f, 300f, 0, FightDuration);
            Mp5 += CalculateEffect(5f / 12f * stats.ManaRestore5min, 12f, 300f, 0, FightDuration);
            Mp5 += CalculateEffect(stats.MementoProc, 15f, 45f, 1 / (HitsFraction * .10f), FightDuration);
            SpellCombatManaRegeneration += CalculateEffect(1f, 15f, 0,
                1 / ((stats.FullManaRegenFor15SecOnSpellcast / 100f) * HitsFraction), FightDuration);
            Spirit += CalculateEffect(stats.SpiritFor20SecOnUse2Min, 20f, 120f, 0, FightDuration);

            return new Stats()
            {
                Spirit = Spirit,
                //HasteRating = Haste,
                SpellHaste = character.StatConversion.GetSpellHasteFromRating(Haste) / 100f,
                SpellPower = Power,
                Mp5 = Mp5,
                SpellCombatManaRegeneration = SpellCombatManaRegeneration,
            };
        }

        public static float CalculateManaRegen(float intel, float spi)
        {
            float baseRegen = 0.005575f;
            return (float)Math.Round(5f * (0.001f + (float)Math.Sqrt(intel) * spi * baseRegen));
        }
    }
}
