using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Elemental.Estimation
{
    public class Estimation
    {
        #region Spells
        LightningBolt LB;
        ChainLightning CL;
        ChainLightning CL3;
        ChainLightning CL4;
        LavaBurst LvB;
        LavaBurst LvBFS;
        FlameShock FS;
        EarthShock ES;
        FrostShock FrS;
        Thunderstorm TS;
        #endregion

        private Stats baseStats, procStats;
        private ShamanTalents talents;
        private CalculationOptionsElemental calcOpts;

        public Estimation(Stats baseStats, Stats procStats, ShamanTalents talents, CalculationOptionsElemental calcOpts)
        {
            this.baseStats = baseStats;
            this.procStats = procStats;
            this.talents = talents;
            this.calcOpts = calcOpts;
            #region Spells
            Stats addedStats = baseStats.Clone();
            addedStats.Accumulate(procStats);
            LB = new LightningBolt(addedStats, talents);
            CL = new ChainLightning(addedStats, talents, 0);
            CL3 = new ChainLightning(addedStats, talents, 3);
            CL4 = new ChainLightning(addedStats, talents, 4);
            LvB = new LavaBurst(addedStats, talents, 0);
            LvBFS = new LavaBurst(addedStats, talents, 1);
            FS = new FlameShock(addedStats, talents);
            ES = new EarthShock(addedStats, talents);
            FrS = new FrostShock(addedStats, talents);
            TS = new Thunderstorm(addedStats, talents);
            #endregion
        }

        private Rotation getPriorityRotation(int type)
        {
            LightningBolt LB = (LightningBolt)this.LB.Clone();
            LavaBurst LvB = (LavaBurst)this.LvBFS.Clone();
            Thunderstorm TS = (Thunderstorm)this.TS.Clone();
            FlameShock FS = (FlameShock)this.FS.Clone();

            LavaBurst LvBNoFS = (LavaBurst)this.LvB.Clone();
            ChainLightning CL = (ChainLightning)this.CL.Clone();
            ChainLightning CL3 = (ChainLightning)this.CL3.Clone();
            ChainLightning CL4 = (ChainLightning)this.CL4.Clone();
            EarthShock ES = (EarthShock)this.ES.Clone();
            FrostShock FrS = (FrostShock)this.FrS.Clone();

            #region Elemental Mastery
            if (talents.ElementalMastery > 0)
            {
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
            
            return new Rotation(talents, LB, CL, CL3, CL4, LvBNoFS, LvB, FS, ES, FrS);
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
            e = new Estimation(stats, new Stats{}, talents, calcOpts);
            rot = e.getPriorityRotation(calcOpts.rotationType);
            // WITH PROCS
            int nPasses = 2, k;
            for (k = 0; k < nPasses; k++)
            {
                procStats = getTrinketStats(character, stats, calcOpts.FightDuration, rot);
                e = new Estimation(stats, procStats, talents, calcOpts);
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
            float judgementRegen = 5 * rot.getCastsPerSecond() * rot.getWeightedHitchance() * stats.ManaRestoreFromBaseManaPerHit * BaseStats.GetBaseStats(character).Mana * .25f; //judgment proc chance is believed 25%
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
