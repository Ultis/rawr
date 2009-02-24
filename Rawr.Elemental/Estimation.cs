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
                float procs;
                float EMmod = SpecialEffect.EstimateUptime(30f, calcOpts.glyphOfElementalMastery ? 150f : 180f, 0, calcOpts.FightDuration, out procs);
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
            /* Rotation
             * Center of attention is Lava Burst.
             * If glyphed, cast FS whenever the dot falls off
             * If unglyphed, cast FS immediately after Lava Burst OR right before Lava Burst
             * OPTION: cast FS whenever available
             * 
             */
            // Thunderstorm
            float timeBetweenTS = TS.CDRefreshTime; // cast whenever available
            float castFractionTS = calcOpts.UseThunderstorm ? TS.CastTime / timeBetweenTS : 0;
            float dpsFromTS = 0f; // assume no targets hit
            float mpsFromTS = calcOpts.UseThunderstorm ? TS.ManaCost / timeBetweenTS : 0;
            // Lava Burst
            float timeBetweenLvB = LvB.CDRefreshTime; // cast whenever available
            float castFractionLvBFS = LvBFS.CastTime / timeBetweenLvB; // LvB casting time per second
            float dpsFromLvB = LvBFS.HitChance * LvBFS.TotalDamage / timeBetweenLvB;
            float mpsFromLvB = LvBFS.ManaCost / timeBetweenLvB;
            // Flame Shock
            float timeBetweenFS = FS.PeriodicRefreshTime; // cast whenever DoT drops
            float dpsFromFS = FS.HitChance * FS.TotalDamage / timeBetweenFS;
            if (!calcOpts.glyphOfFlameShock)
            {
                // cast AFTER LvB
                timeBetweenFS = timeBetweenLvB;
                if (timeBetweenFS < FS.CDRefreshTime) timeBetweenFS = FS.CDRefreshTime; // Should Not Happen
                float ticks = (float)Math.Floor((timeBetweenFS - LvB.CastTime) / FS.PeriodicTickTime);
                if (ticks > FS.PeriodicTicks) ticks = FS.PeriodicTicks; // Should Not Happen
                dpsFromFS = FS.HitChance * (FS.AvgDamage + FS.PeriodicTick * ticks) / timeBetweenFS;
            }
            float mpsFromFS = FS.ManaCost / timeBetweenFS;
            float castFractionFS = FS.CastTime / timeBetweenFS; // FS casting time per second
            // Lightning Bolt
            float castFractionLB = 1f - castFractionFS - castFractionLvBFS - castFractionTS; // LB casting time per second
            float dpsFromLB = LB.HitChance * LB.DpCT * castFractionLB;
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
                float CCchanceLvBFS = 1 - ((1 - LvBFS.HitChance) * (1 - FS.CritChance * FS.HitChance));
                float CCchanceLBFS = 1 - ((1 - critLB * LB.HitChance) * (1 - FS.CritChance * FS.HitChance));
                if (calcOpts.glyphOfFlameShock)
                {
                    clearcastingLvB = CCchance2LB;
                    clearcastingFS = CCchance2LB;
                    clearcastingLB = (
                        Math.Max(castsPerLvB - 2, 0) * CCchance2LB +
                        Math.Min(1, castsPerLvB - 1) * CCchanceLvBLB +
                        Math.Min(1, castsPerLvB) * CCchanceLvBLB
                        ) / castsPerLvB;
                }
                else
                {
                    clearcastingLvB = CCchance2LB;
                    clearcastingFS = CCchanceLvBLB;
                    clearcastingLB = (
                        Math.Max(castsPerLvB - 2, 0) * CCchance2LB +
                        Math.Min(1, castsPerLvB - 1) * CCchanceLBFS +
                        Math.Min(1, castsPerLvB) * CCchanceLvBFS
                        ) / castsPerLvB;
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
                CC_LB = clearcastingLB,
                LBFraction = castFractionLB,
                LvBFraction = castFractionLvBFS,
                FSFraction = castFractionFS
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
                HasteRating = stats.LightningBoltHasteProc_15_45 * 10f / 55f,
            };
            #endregion

            Rotation rot = getRotation(stats, talents, calcOpts);
            float damage;
            stats += getTrinketStats(character, stats, calcOpts.FightDuration, rot.CastFraction, rot.CritFraction, rot.MissFraction, 1f / 3f, out damage);
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
            calculatedStats.RotationDPS = rot.DPS;
            calculatedStats.RotationMPS = rot.MPS;
            calculatedStats.TotalDPS = TotalDamage / FightDuration;
            calculatedStats.ClearCast_FlameShock = rot.CC_FS;
            calculatedStats.ClearCast_LavaBurst = rot.CC_LvB;
            calculatedStats.ClearCast_LightningBolt = rot.CC_LB;
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

        public static float CalculateManaRegen(float intel, float spi)
        {
            float baseRegen = 0.005575f;
            return (float)Math.Round(5f * (0.001f + (float)Math.Sqrt(intel) * spi * baseRegen));
        }
    }
}
