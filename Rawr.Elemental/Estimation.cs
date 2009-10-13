using System;
using System.Collections.Generic;
using System.Text;
using Rawr.Elemental.Spells;

namespace Rawr.Elemental
{
    public class Estimation
    {
        SpellBox spellbox;

        private Stats baseStats, procStats;
        private ShamanTalents talents;
        private CalculationOptionsElemental calcOpts;

        public Estimation(Stats baseStats, Stats procStats, ShamanTalents talents, CalculationOptionsElemental calcOpts)
        {
            this.baseStats = baseStats;
            this.procStats = procStats;
            this.talents = talents;
            this.calcOpts = calcOpts;

            Stats addedStats = baseStats.Clone();
            addedStats.Accumulate(procStats);
            spellbox = new SpellBox(addedStats, talents);
        }

        /// <summary>
        /// Beware when updating: The spells from an earlier returned Rotation are references to the SpellBox from this Estimation.
        /// </summary>
        /// <param name="baseStats"></param>
        /// <param name="procStats"></param>
        /// <param name="talents"></param>
        /// <param name="calcOpts"></param>
        public void Update(Stats baseStats, Stats procStats, ShamanTalents talents, CalculationOptionsElemental calcOpts)
        {
            this.baseStats = baseStats;
            this.procStats = procStats;
            this.talents = talents;
            this.calcOpts = calcOpts;

            Stats addedStats = baseStats.Clone();
            addedStats.Accumulate(procStats);
            spellbox.Update(addedStats, talents);
        }

        private Rotation getPriorityRotation(int type)
        {
            #region Elemental Mastery
            if (talents.ElementalMastery > 0)
                spellbox.ApplyEM(ElementalMastery.getAverageUptime(talents.GlyphofElementalMastery, calcOpts.FightDuration));
            #endregion
            
            return new Rotation(talents, spellbox);
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
             * NYI Optional: use of CL
             */
            Estimation e;
            Rotation rot;
            float damage;
            Stats procStats;

            #region Stat changing glyphs
            if (talents.GlyphofFlameShock)
                stats.FlameShockDoTCanCrit = 1f;
            #endregion

            // WITHOUT PROCS
            e = new Estimation(stats, new Stats{}, talents, calcOpts);
            rot = e.getPriorityRotation(calcOpts.rotationType);
            // WITH PROCS
            int nPasses = 2, k;
            for (k = 0; k < nPasses; k++)
            {
                procStats = getTrinketStats(character, stats, calcOpts.FightDuration, rot);
                e.Update(stats, procStats, talents, calcOpts);
                rot = e.getPriorityRotation(calcOpts.rotationType);
            }

            // Thunderstorm usage
            float thunderstormRegen = 0f;
            #region Thunderstorm
            if (calcOpts.UseThunderstorm)
            {
                float procsPerSecond = Thunderstorm.getProcsPerSecond(talents.GlyphofThunder, calcOpts.FightDuration);
                thunderstormRegen += (talents.GlyphofThunderstorm ? .1f : .08f) * stats.Mana * procsPerSecond * 5;
            }
            #endregion

            /* Regen variables: (divide by 5 for regen per second)
             * While casting: ManaRegInFSR
             * During regen: ManaRegOutFSR */
            #region Calculate Regen
            float spiRegen = 5 * StatConversion.GetSpiritRegenSec(stats.Spirit, stats.Intellect);
            float replenishRegen = 5 * stats.Mana * stats.ManaRestoreFromMaxManaPerSecond;
            float judgementRegen = 5 * rot.GetBaseCastTime() / rot.GetTime() * stats.ManaRestoreFromBaseManaPPM / 60f * BaseStats.GetBaseStats(character).Mana;
            float ManaRegInFSR = spiRegen * stats.SpellCombatManaRegeneration + stats.Mp5 + replenishRegen + judgementRegen + thunderstormRegen;
            float ManaRegOutFSR = spiRegen + stats.Mp5 + replenishRegen + thunderstormRegen;
            float ManaRegen = ManaRegInFSR;
            #endregion

            // Mana potion: extraMana
            #region Mana potion
            float extraMana = new float[] { 0f, 1800f, 2200, 2400, 4300 }[calcOpts.ManaPot];
            extraMana *= 1 + stats.BonusManaPotion;
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
            //damage procs (Thunder Capacitor etc.) are effected by spellcrit and damage debuffs
            damage = procStats.ArcaneDamage * (1 + stats.BonusArcaneDamageMultiplier) + procStats.NatureDamage * (1 + stats.BonusNatureDamageMultiplier) + procStats.FireDamage * (1 + stats.BonusFireDamageMultiplier) + procStats.ShadowDamage * (1 + stats.BonusShadowDamageMultiplier);
            damage *= (1 + stats.SpellCrit * .5f); // but only with the normal 50% dmg bonus
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
            calculatedStats.ManaRegenInFSR = ManaRegInFSR;
            calculatedStats.ManaRegenOutFSR = ManaRegOutFSR;
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
            calculatedStats.CastsPerSecond = rot.getCastsPerSecond();
            calculatedStats.CritsPerSecond = rot.getWeightedCritchance() * rot.getCastsPerSecond();
            calculatedStats.MissesPerSecond = rot.getCastsPerSecond() * (1f - rot.getWeightedHitchance());
            calculatedStats.LvBPerSecond = rot.getCastsPerSecond(typeof(LavaBurst));
            calculatedStats.LBPerSecond = rot.getCastsPerSecond(typeof(LightningBolt));
            calculatedStats.FSPerSecond = rot.getCastsPerSecond(typeof(FlameShock));
            calculatedStats.RotationDPS = rot.DPS;
            calculatedStats.RotationMPS = rot.MPS;
            calculatedStats.TotalDPS = TotalDamage / FightDuration;
            rot.ClearCasting.TryGetValue(typeof(FlameShock), out calculatedStats.ClearCast_FlameShock);
            rot.ClearCasting.TryGetValue(typeof(LavaBurst), out calculatedStats.ClearCast_LavaBurst);
            rot.ClearCasting.TryGetValue(typeof(LightningBolt), out calculatedStats.ClearCast_LightningBolt);
            calculatedStats.Rotation = rot.ToString();
            calculatedStats.RotationDetails = rot.ToDetailedString();
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
                    procChance = rot.getWeightedCritchance();
                }
                else if (effect.Trigger == Trigger.ShamanLightningBolt)
                {
                    trigger = 1f / rot.getCastsPerSecond(typeof(LightningBolt));
                }
                else if (effect.Trigger == Trigger.ShamanShock)
                {
                    trigger = 1f / rot.getCastsPerSecond(typeof(Shock));
                }
                else if (effect.Trigger == Trigger.Use)
                {
                    trigger = 1f;
                }
                else
                    continue;
                
                effect.AccumulateAverageStats(statsAverage, trigger, procChance, 3f, FightDuration);
            }
            return statsAverage;
        }
    }
}
