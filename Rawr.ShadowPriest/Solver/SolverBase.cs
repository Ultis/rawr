using System;
using System.Collections.Generic;
using Rawr.ShadowPriest.Spells;

namespace Rawr.ShadowPriest
{
    public class Solver
    {
        SpellBox spellbox;

        private Stats baseStats, procStats;
        private PriestTalents talents;
        private CalculationOptionsShadowPriest calcOpts;

        public Solver(Stats baseStats, Stats procStats, PriestTalents talents, CalculationOptionsShadowPriest calcOpts)
        {
            this.baseStats = baseStats;
            this.procStats = procStats;
            this.talents = talents;
            this.calcOpts = calcOpts;

            Stats addedStats = baseStats.Clone();
            addedStats.Accumulate(procStats);
            CombatFactors combatFactors = new CombatFactors(talents, addedStats, Math.Max(calcOpts.NumberOfTargets-1, 0), calcOpts.LatencyCast, calcOpts.LatencyGcd);
            spellbox = new SpellBox(combatFactors);
        }

        /// <summary>
        /// Beware when updating: The spells from an earlier returned Rotation are references to the SpellBox from this Estimation.
        /// </summary>
        /// <param name="baseStats"></param>
        /// <param name="procStats"></param>
        /// <param name="talents"></param>
        /// <param name="calcOpts"></param>
        public void Update(Stats baseStats, Stats procStats, PriestTalents talents, CalculationOptionsShadowPriest calcOpts)
        {
            this.baseStats = baseStats;
            this.procStats = procStats;
            this.talents = talents;
            this.calcOpts = calcOpts;

            Stats addedStats = baseStats.Clone();
            addedStats.Accumulate(procStats);
            CombatFactors combatFactors = new CombatFactors(talents, addedStats, Math.Max(calcOpts.NumberOfTargets - 1, 0), calcOpts.LatencyCast, calcOpts.LatencyGcd);
            spellbox.Update(combatFactors);
        }

        private Rotation getPriorityRotation() //TODO: int type)
        {
            return new Rotation(spellbox, talents, new RotationOptions());
        }

        public static void solve(CharacterCalculationsShadowPriest calculatedStats, CalculationOptionsShadowPriest calcOpts, BossOptions bossOpts)
        {
            Stats stats = calculatedStats.BasicStats;
            Character character = calculatedStats.LocalCharacter;
            PriestTalents talents = character.PriestTalents;

            Solver e;
            Rotation rot;

            float FightDuration = bossOpts.BerserkTimer;

            // WITHOUT PROCS
            e = new Solver(stats, new Stats{}, talents, calcOpts);
            rot = e.getPriorityRotation();

            calculatedStats.DpsPoints = rot.DPS;
            calculatedStats.SurvivalPoints = stats.Stamina / FightDuration; //TODO: meaningful surv points

            calculatedStats.CombatStats = stats.Clone();

            calculatedStats.DevouringPlauge = rot.DP;
            calculatedStats.MindBlast = rot.MB;
            calculatedStats.MindFlay = rot.MF;
            calculatedStats.MindSpike = rot.Spike;
            //calculatedStats.PowerWordShield = rot.shield;
            calculatedStats.ShadowFiend = rot.Fiend;
            calculatedStats.ShadowWordDeath = rot.SWD;
            calculatedStats.ShadowWordPain = rot.SWP;
            calculatedStats.VampiricTouch = rot.VT;

            calculatedStats.Rotation = rot.ToString();
            calculatedStats.RotationDetails = rot.ToDetailedString();

        }

        protected static void CalculateTriggers(Rotation rot, Dictionary<Trigger, float> triggerIntervals, Dictionary<Trigger, float> triggerChances)
        {
            //TODO: wont trigger trinkets
            /*
            float CastInterval = 1f / rot.getCastsPerSecond();
            float WeightedCritChance = rot.getWeightedCritchance();
            float WeightedHitChance = rot.getWeightedHitchance();

            triggerIntervals[Trigger.Use] = 0f;
            triggerChances[Trigger.Use] = 1f;

            triggerIntervals[Trigger.DamageDone] = 1f / (rot.getCastsPerSecond() + 1f / rot.FS.PeriodicTickTime);
            // flameshock ticks are not taken into account. the chance would be slightly higher.
            triggerChances[Trigger.DamageDone] = WeightedHitChance;

            triggerIntervals[Trigger.DamageOrHealingDone] = 1f / (rot.getCastsPerSecond() + 1f / rot.FS.PeriodicTickTime);
            // flameshock ticks are not taken into account. the chance would be slightly higher.
            // Need to add Self-Heals
            triggerChances[Trigger.DamageOrHealingDone] = WeightedHitChance;

            triggerIntervals[Trigger.SpellMiss] = CastInterval;
            triggerChances[Trigger.SpellMiss] = 1f - WeightedHitChance;

            triggerIntervals[Trigger.SpellHit] = CastInterval;
            triggerChances[Trigger.SpellHit] = WeightedHitChance;

            triggerIntervals[Trigger.DamageSpellHit] = CastInterval;
            triggerChances[Trigger.DamageSpellHit] = WeightedHitChance;

            triggerIntervals[Trigger.DoTTick] = 1f / rot.FS.PeriodicTickTime;
            triggerChances[Trigger.DoTTick] = 1f;

            triggerIntervals[Trigger.SpellCast] = CastInterval;
            triggerChances[Trigger.SpellCast] = 1f;

            triggerIntervals[Trigger.DamageSpellCast] = CastInterval;
            triggerChances[Trigger.DamageSpellCast] = 1f;

            triggerIntervals[Trigger.SpellCrit] = CastInterval;
            triggerChances[Trigger.SpellCrit] = WeightedCritChance;

            triggerIntervals[Trigger.DamageSpellCrit] = CastInterval;
            triggerChances[Trigger.DamageSpellCrit] = WeightedCritChance;

            triggerIntervals[Trigger.ShamanLightningBolt] = 1f / rot.getCastsPerSecond(typeof(LightningBolt));
            triggerChances[Trigger.ShamanLightningBolt] = 1f;

            triggerIntervals[Trigger.ShamanShock] = 1f / rot.getCastsPerSecond(typeof(Shock));
            triggerChances[Trigger.ShamanShock] = 1f;

            triggerIntervals[Trigger.ShamanFlameShockDoTTick] = 1f / rot.getTicksPerSecond(typeof(FlameShock));
            triggerChances[Trigger.ShamanFlameShockDoTTick] = 1f;
             * */
        }

        protected static Stats DoSpecialEffects(Character character, Stats stats, Rotation rot, float FightDuration)
        {
            #region Initialize Triggers
            Dictionary<Trigger, float> triggerIntervals = new Dictionary<Trigger, float>(); ;
            Dictionary<Trigger, float> triggerChances = new Dictionary<Trigger, float>(); ;
            CalculateTriggers(rot, triggerIntervals, triggerChances);
            #endregion

            Stats procStats = new Stats();

            List<SpecialEffect> effects = new List<SpecialEffect>();
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                effect.Stats.GenerateSparseData();

                #region Filter out unhandled effects
                if (!triggerIntervals.ContainsKey(effect.Trigger)) continue;
                #endregion

                effects.Add(effect);
            }

            AccumulateSpecialEffects(character, ref procStats, FightDuration, triggerIntervals, triggerChances, effects, 1f);
            return procStats;
        }

        protected static void AccumulateSpecialEffects(Character character, ref Stats stats, float FightDuration, Dictionary<Trigger, float> triggerIntervals, Dictionary<Trigger, float> triggerChances, List<SpecialEffect> effects, float weight)
        {
            foreach (SpecialEffect effect in effects)
            {
                Stats effectStats = effect.Stats;

                float upTime = 0f;

                if (effect.Trigger == Trigger.Use)
                {
                    if (effect.Stats._rawSpecialEffectDataSize >= 1)
                    {
                        upTime = effect.GetAverageUptime(0f, 1f, 0, FightDuration);
                        List<SpecialEffect> nestedEffect = new List<SpecialEffect>(effect.Stats.SpecialEffects());
                        Stats _stats2 = effectStats.Clone();
                        AccumulateSpecialEffects(character, ref _stats2, effect.Duration, triggerIntervals, triggerChances, nestedEffect, upTime);
                        effectStats = _stats2;
                    }
                    else
                    {
                        upTime = effect.GetAverageStackSize(0f, 1f, 0, FightDuration);
                    }
                }
                else if (effect.Duration == 0f)
                {
                    upTime = effect.GetAverageProcsPerSecond(triggerIntervals[effect.Trigger], triggerChances[effect.Trigger],
                                                             0, FightDuration);
                }
                else if (triggerIntervals.ContainsKey(effect.Trigger))
                {
                    upTime = effect.GetAverageStackSize(triggerIntervals[effect.Trigger], triggerChances[effect.Trigger],
                                                             0, FightDuration);
                }

                if (upTime > 0f)
                {
                    stats.Accumulate(effectStats, upTime * weight);
                }
            }
        }
        
        private static Stats getTrinketStats(Character character, Stats stats, float FightDuration, Rotation rot)
        {
            Stats statsAverage = new Stats();
            //TODO: Trinket Stats
            /*
            
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                float trigger = 0f; // 1 / frequency in Hz
                float procChance = 1f;
                if (effect.Trigger == Trigger.DamageDone)
                {
                    trigger = 1f / ( rot.getCastsPerSecond() + 1f/rot.FS.PeriodicTickTime );
                    procChance = rot.getWeightedHitchance(); //flameshock ticks are not taken into account. the chance would be slightly higher.
                }
                else if (effect.Trigger == Trigger.DamageOrHealingDone)
                {
                    // Need to Add Self-Heals
                    trigger = 1f / (rot.getCastsPerSecond() + 1f / rot.FS.PeriodicTickTime);
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
                else if (effect.Trigger == Trigger.ShamanFlameShockDoTTick)
                {
                    trigger = 1f / rot.getTicksPerSecond(typeof(FlameShock));
                }
                else if (effect.Trigger == Trigger.Use)
                {
                    trigger = 1f;
                }
                else
                    continue;
                
                effect.AccumulateAverageStats(statsAverage, trigger, procChance, 3f, FightDuration);
            }*/
            return statsAverage;
        }
    }
}
