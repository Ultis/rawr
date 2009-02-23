using System;
using System.Collections.Generic;

namespace Rawr.Elemental.Simulator
{
    public class Uncertainty
    {
        protected class Part
        {
            public float value { get; private set; }
            public float start { get; private set; }
            public Part(float value, float start) { this.value = value; this.start = start;  }
        }

        private List<Part> parts;
        private float Duration;
        private float Time;
        private float Max;

        public Uncertainty(float duration, float max)
        {
            this.Time = 0f;
            this.Duration = duration;
            this.Max = max;
            this.parts = new List<Part>();
        }

        public void Add(float value)
        {
            parts.Add(new Part(value, Time));
        }
        
        public void Run(float dT)
        {
            Time+=dT;
            float t = Time - Duration;
            while (parts.Count > 0 && parts[0].start < t) parts.RemoveAt(0);
            value = 0;
        }

        public Uncertainty Clone()
        {
            Uncertainty clone = (Uncertainty)this.MemberwiseClone();
            clone.parts = new List<Part>();
            parts.ForEach(p => clone.parts.Add(p));
            return clone;
        }

        private float value;
        public float Value
        {
            get
            {
                if (value == 0)
                {
                    parts.ForEach(p => value += p.value);
                }
                if (value > Max) value = Max;
                return value;
            }
        }
    }

    public class SimuState
    {
        public RotationSim simulator;

        public float DamageDone = 0f;
        public float Time = 0f;
        public float mana = 0f;
        public float MaxMana = 0f;
        public float Mana
        {
            get { return mana; }
            set { mana = (value > MaxMana) ? MaxMana : value; }
        }

        public float LastCast = float.NegativeInfinity;
        public Spell LastSpell = null;

        /* Flame Shock */
        public float FlameShockPeriodickTick = 0f;
        public float FlameShockTickTime = 3f;
        public float FlameShockTimer = 0f;
        public Uncertainty FlameShockDoT = null;

        /* Cooldowns */
        public float CooldownCL = 0f;
        public float CooldownFS = 0f;
        public float CooldownLvB = 0f;
        public float CooldownEM = 0f;
        public float CooldownTS = 0f;
        
        /* Trinkets */
        public SpecialEffects effects = null;

        /* Elemental Mastery */
        public float TimeLeftEM = 0f;
        
        /* Clearcasting */
        protected float Crit0 = 0f;
        protected float Crit1 = 0f;
        public float ClearCasting
        { get { return 1f - (1f - Crit0) * (1f - Crit1); } }
        public float LastSpellCritChance
        { set { Crit0 = Crit1; Crit1 = value; } }

        /* Counters */
        public float Casts = 0f;
        public float Hits = 0f;
        public float Crits = 0f;
        public float Misses = 0f;

        /* Stats */
        public Stats baseStats { get; private set; }
        public Stats stats = null;

        public ShamanTalents talents { get; private set; }
        public CalculationOptionsElemental calcOpts { get; private set; }

        public SimuState(Stats baseStats, ShamanTalents talents, CalculationOptionsElemental calcOpts)
        {
            this.stats = this.baseStats = baseStats;
            this.talents = talents;
            this.calcOpts = calcOpts;
            this.MaxMana = baseStats.Mana;
            this.Mana = baseStats.Mana;
        }

        public SimuState Clone(SpecialEffect effect)
        {
            SimuState clone = this.MemberwiseClone() as SimuState;
            clone.effects = effects.Clone(effect);
            clone.stats = stats.Clone();
            if (FlameShockDoT != null) clone.FlameShockDoT = FlameShockDoT.Clone();
            return clone;
        }

        public SimuState Clone()
        {
            SimuState clone = this.MemberwiseClone() as SimuState;
            clone.stats = stats.Clone();
            if (FlameShockDoT != null) clone.FlameShockDoT = FlameShockDoT.Clone();
            return clone;
        }

    }

    public class RotationSim
    {
        public virtual List<SimuState> successors(SimuState state)
        {
            List<SimuState> newStates = new List<SimuState>();
            foreach (SpecialEffect p in state.effects.Effects)
            {
                float Damage, ProgressTime;
                if (p.use(state.stats, out Damage, out ProgressTime))
                {
                    SimuState newState = state.Clone(p);
                    newState.DamageDone += Damage;
                    newState.LastSpell = null;
                    if (ProgressTime != 0f) Simulator.progressTime(newState, ProgressTime);
                    newStates.Add(newState);
                }
            }
            SimuState ts = Simulator.runThunderstorm(state);
            SimuState fs = Simulator.runFlameshock(state);
            SimuState lvb = Simulator.runLavaBurst(state);
            SimuState lb = Simulator.runLightningBolt(state);
            SimuState em = Simulator.runElementalMastery(state);
            SimuState nothing = Simulator.runNothing(state, .1f);
            if (ts != null)
            {
                newStates.Add(ts);
            }
            if (fs != null)
            {
                newStates.Add(fs);
            }
            if (lvb != null)
            {
                newStates.Add(lvb);
            }
            if (lb != null)
            {
                newStates.Add(lb);
            }
            if (em != null)
            {
                newStates.Add(em);
            }
            if (nothing != null)
            {
                newStates.Add(nothing);
            }
            return newStates;
        }
    }

    public class DefaultRotation  : RotationSim
    {
        public float TSmana = 0.7f;
        public bool castLvBWithoutFS = true;
        public bool castFSWheneverPossible = true;

        public override List<SimuState> successors(SimuState state)
        {
            // ROTATION
            // - use any trinkets
            // - use EM
            // - use TS if mana under 90%
            // - use LvB if FS active
            // - if FS not active, use FS
            // - use LB
            List<SimuState> newStates = new List<SimuState>();
            foreach (SpecialEffect p in state.effects.Effects)
            {
                float Damage, ProgressTime;
                if (p.use(state.stats, out Damage, out ProgressTime))
                {
                    SimuState newState = state.Clone(p);
                    newState.DamageDone += Damage;
                    newState.LastSpell = null;
                    if (ProgressTime != 0f) Simulator.progressTime(newState, ProgressTime);
                    newStates.Add(newState);
                    return newStates;
                }
            }

            SimuState em = Simulator.runElementalMastery(state);
            if (em != null)
            {
                newStates.Add(em);
                return newStates;
            }

            if (state.mana / state.MaxMana < TSmana)
            {
                SimuState ts = Simulator.runThunderstorm(state);
                if (ts != null)
                {
                    newStates.Add(ts);
                    return newStates;
                }
            }

            if (castLvBWithoutFS || state.FlameShockTimer > 0f)
            {
                SimuState lvb = Simulator.runLavaBurst(state);
                if (lvb != null)
                {
                    newStates.Add(lvb);
                    return newStates;
                }
            }

            if (state.FlameShockTimer <= 0f || castFSWheneverPossible)
            {
                SimuState fs = Simulator.runFlameshock(state);
                if (fs != null)
                {
                    newStates.Add(fs);
                    return newStates;
                }
            }

            SimuState lb = Simulator.runLightningBolt(state);
            if (lb != null)
            {
                newStates.Add(lb);
                return newStates;
            }

            newStates.Add(Simulator.runNothing(state, .1f));
            return newStates;
        }
    }

    public class Simulator
    {
        public static void progressTime(SimuState state, float dT)
        {
            if (dT < 0)
            {
                dT += 1;
                return;
            }
            state.Time += dT;
            #region Flame Shock DOT and Spell cooldowns
            if (state.FlameShockDoT != null)
            {
                state.FlameShockDoT.Run(dT);
                state.FlameShockTimer -= dT;
                int ticksLeftBefore = (int)Math.Floor((float)Math.Max(0f, state.FlameShockTimer + dT) / state.FlameShockTickTime);
                int ticksLeftAfter = (int)Math.Floor((float)Math.Max(0f, state.FlameShockTimer) / state.FlameShockTickTime);
                int ticks = ticksLeftBefore - ticksLeftAfter;
                state.DamageDone += ticks * state.FlameShockPeriodickTick * state.FlameShockDoT.Value;
                #region Procs
                foreach (SpecialEffect p in state.effects.Effects)
                {
                    float mana, damage;
                    p.proc(state.stats,
                        0,
                        0,
                        0,
                        0,
                        ticks,
                        out damage,
                        out mana);
                    state.DamageDone += damage;
                    state.Mana += mana;
                }
                #endregion
            }
            state.CooldownFS -= dT;
            state.CooldownEM -= dT;
            state.TimeLeftEM -= dT;
            state.CooldownCL -= dT;
            state.CooldownLvB -= dT;
            state.CooldownTS -= dT;
            #endregion
            RegenMana(state, dT);
            #region Procs
            state.stats = state.baseStats;
            foreach (SpecialEffect p in state.effects.Effects)
            {
                float damage;
                state.stats += p.run(dT, out damage);
                state.DamageDone += damage;
            }
            #endregion
        }

        public static SimuState runFlameshock(SimuState state)
        {
            #region Try to start casting
            if (state.CooldownFS > 0f) return null;
            FlameShock spell = new FlameShock(state.stats, state.talents, state.calcOpts);
            if (state.TimeLeftEM > 0f) spell.ApplyEM(1f);
            float manaCost = spell.ManaCost;
            if (state.talents.ElementalFocus > 0) manaCost *= 1 - .4f * state.ClearCasting;
            if (manaCost > state.Mana) return null;
            #endregion
            SimuState newState = state.Clone();
            float timeToCast = spell.CastTimeWithoutGCD;
            float timeAfterCast = spell.CastTime - spell.CastTimeWithoutGCD;
            progressTime(newState, timeToCast);
            #region Actual spellcast (recheck mana cost)
            spell = new FlameShock(newState.stats, state.talents, state.calcOpts);
            if (newState.TimeLeftEM > 0f) spell.ApplyEM(1f);
            manaCost = spell.ManaCost;
            if (state.talents.ElementalFocus > 0) manaCost *= 1 - .4f * newState.ClearCasting;
            if (manaCost > newState.Mana) return null;
            float damage = spell.AvgDamage;
            damage *= 1 + .05f * newState.ClearCasting * state.talents.ElementalOath;
            newState.LastSpellCritChance = spell.CritChance * spell.HitChance;
            #endregion
            #region General state updates
            newState.LastCast = newState.Time;
            newState.DamageDone += damage * spell.HitChance;
            newState.Mana -= manaCost;
            newState.LastSpell = spell;
            newState.Casts += 1f;
            newState.Hits += spell.HitChance;
            newState.Crits += spell.CritChance * spell.HitChance;
            newState.Misses += spell.MissChance;
            #region Procs
            foreach (SpecialEffect p in newState.effects.Effects)
            {
                float mana;
                p.proc(newState.stats,
                    1f,
                    spell.HitChance,
                    spell.CritChance * spell.HitChance,
                    spell.MissChance,
                    0,
                    out damage,
                    out mana);
                newState.DamageDone += damage;
                newState.Mana += mana;
            }
            #endregion
            #endregion
            #region Specific state updates
            newState.CooldownFS = spell.Cooldown;
            newState.FlameShockTimer = spell.Duration;
            if (newState.FlameShockDoT == null) newState.FlameShockDoT = new Uncertainty(spell.Duration, 1);
            newState.FlameShockDoT.Add(spell.HitChance);
            newState.FlameShockPeriodickTick = spell.PeriodicTick;
            newState.FlameShockTickTime = spell.PeriodicTickTime;
            #endregion
            progressTime(newState, timeAfterCast);
            return newState;
        }

        public static SimuState runLavaBurst(SimuState state)
        {
            #region Try to start casting
            if (state.CooldownLvB > 0f) return null;
            float value = 0;
            if (state.FlameShockDoT != null) value = state.FlameShockDoT.Value;
            LavaBurst spell = new LavaBurst(state.stats, state.talents, state.calcOpts, value);
            if (state.TimeLeftEM > 0f) spell.ApplyEM(1f);
            float manaCost = spell.ManaCost;
            if (state.talents.ElementalFocus > 0) manaCost *= 1 - .4f * state.ClearCasting;
            if (manaCost > state.Mana) return null;
            #endregion
            SimuState newState = state.Clone();
            float timeToCast = spell.CastTimeWithoutGCD;
            float timeAfterCast = spell.CastTime - spell.CastTimeWithoutGCD;
            progressTime(newState, timeToCast);
            #region Actual spellcast
            value = 0;
            if (newState.FlameShockDoT != null) value = newState.FlameShockDoT.Value;
            spell = new LavaBurst(newState.stats, state.talents, state.calcOpts, value);
            if (newState.TimeLeftEM > 0f) spell.ApplyEM(1f);
            manaCost = spell.ManaCost;
            if (state.talents.ElementalFocus > 0) manaCost *= 1 - .4f * newState.ClearCasting;
            if (manaCost > newState.Mana) return null;
            float damage = spell.AvgDamage;
            damage *= 1 + .05f * newState.ClearCasting * state.talents.ElementalOath;
            newState.LastSpellCritChance = spell.CritChance * spell.HitChance;
            #endregion
            #region General state updates
            newState.LastCast = newState.Time;
            newState.DamageDone += damage * spell.HitChance;
            newState.Mana -= manaCost;
            newState.LastSpell = spell;
            newState.Casts += 1f;
            newState.Hits += spell.HitChance;
            newState.Crits += spell.CritChance;
            newState.Misses += spell.MissChance;
            #region Procs
            foreach (SpecialEffect p in newState.effects.Effects)
            {
                float mana;
                p.proc(newState.stats,
                    1f,
                    spell.HitChance,
                    spell.CritChance * spell.HitChance,
                    spell.MissChance,
                    0,
                    out damage, out mana);
                newState.DamageDone += damage;
                newState.Mana += mana;
            }
            #endregion
            #endregion
            #region Specific state updates
            newState.CooldownLvB = spell.Cooldown;
            if (!newState.calcOpts.glyphOfFlameShock)
            {
                newState.FlameShockDoT = null;
                newState.FlameShockTimer = 0f;
            }
            #endregion
            progressTime(newState, timeAfterCast);
            return newState;
        }

        public static SimuState runLightningBolt(SimuState state)
        {
            #region Try to start casting
            // no cooldown
            LightningBolt spell = new LightningBolt(state.stats, state.talents, state.calcOpts);
            if (state.TimeLeftEM > 0f) spell.ApplyEM(1f);
            float manaCost = spell.ManaCost;
            if (state.talents.ElementalFocus > 0) manaCost *= 1 - .4f * state.ClearCasting;
            if (manaCost > state.Mana) return null;
            #endregion
            SimuState newState = state.Clone();
            float timeToCast = spell.CastTimeWithoutGCD;
            float timeAfterCast = spell.CastTime - spell.CastTimeWithoutGCD;
            progressTime(newState, timeToCast);
            #region Actual spellcast
            spell = new LightningBolt(newState.stats, state.talents, state.calcOpts);
            if (newState.TimeLeftEM > 0f) spell.ApplyEM(1f);
            manaCost = spell.ManaCost;
            if (state.talents.ElementalFocus > 0) manaCost *= 1 - .4f * newState.ClearCasting;
            if (manaCost > newState.Mana) return null;
            spell.increaseCritFromOverload(state.talents.LightningOverload);
            float damage = spell.AvgDamage;
            damage *= 1 + .05f * newState.ClearCasting * state.talents.ElementalOath;
            newState.LastSpellCritChance = spell.CritChance * spell.HitChance;
            damage *= 1 + .04f * state.talents.LightningOverload * .5f;
            #endregion
            #region General state updates
            newState.LastCast = newState.Time;
            newState.DamageDone += damage * spell.HitChance;
            newState.Mana -= manaCost;
            newState.LastSpell = spell;
            newState.Casts += 1f;
            newState.Hits += spell.HitChance;
            newState.Crits += spell.CritChance * spell.HitChance;
            newState.Misses += spell.MissChance;
            #region Procs
            foreach (SpecialEffect p in newState.effects.Effects)
            {
                float mana;
                p.proc(newState.stats,
                    1f,
                    spell.HitChance,
                    spell.CritChance * spell.HitChance,
                    spell.MissChance,
                    0,
                    out damage, out mana);
                newState.DamageDone += damage;
                newState.Mana += mana;
            }
            #endregion
            #endregion
            progressTime(newState, timeAfterCast);
            return newState;
        }

        public static SimuState runElementalMastery(SimuState state)
        {
            #region Try casting
            if (state.talents.ElementalMastery == 0) return null;
            if (state.CooldownEM > 0f) return null;
            ElementalMastery spell = new ElementalMastery(state.stats, state.talents, state.calcOpts);
            #endregion
            SimuState newState = state.Clone();
            #region General state updates
            newState.LastCast = newState.Time;
            newState.LastSpell = spell;
            newState.Casts += 1f;
            #region Procs
            foreach (SpecialEffect p in newState.effects.Effects)
            {
                float damage, mana;
                p.proc(newState.stats,
                    1f,
                    0,
                    0,
                    0,
                    0,
                    out damage, out mana);
                newState.DamageDone += damage;
                newState.Mana += mana;
            }
            #endregion
            #endregion
            #region Specific state updates
            newState.TimeLeftEM = 30f;
            newState.CooldownEM = spell.Cooldown;
            #endregion
            return newState;
        }

        public static SimuState runThunderstorm(SimuState state)
        {
            #region Try casting
            if (state.talents.Thunderstorm == 0) return null;
            if (state.CooldownTS > 0f) return null;
            Thunderstorm spell = new Thunderstorm(state.stats, state.talents, state.calcOpts);
            if (state.TimeLeftEM > 0f) spell.ApplyEM(1f);
            // no mana cost
            #endregion
            SimuState newState = state.Clone();
            float timeToCast = spell.CastTimeWithoutGCD;
            float timeAfterCast = spell.CastTime - spell.CastTimeWithoutGCD;
            progressTime(newState, timeToCast);
            #region Actual spellcast
            spell = new Thunderstorm(newState.stats, state.talents, state.calcOpts);
            // if (newState.TimeLeftEM > 0f) spell.ApplyEM(1f);
            // newState.LastSpellCritChance = spell.CritChance;
            // ignore damage
            // float damage = spell.AvgDamage;
            // damage *= 1 + .05f * state.ClearCasting * state.talents.ElementalOath;
            newState.LastSpellCritChance = 0f; // assume we didn't crit... but we do eat CC
            #endregion
            #region General state updates
            newState.LastCast = newState.Time;
            // newState.DamageDone += damage;
            newState.LastSpell = spell;
            newState.Casts += 1f;
            newState.Hits += spell.HitChance;
            newState.Crits += spell.CritChance;
            newState.Misses += spell.MissChance;
            #region Procs
            foreach (SpecialEffect p in newState.effects.Effects)
            {
                float mana, damage;
                p.proc(newState.stats,
                    1f,
                    spell.HitChance,
                    spell.CritChance,
                    spell.MissChance,
                    0,
                    out damage, out mana);
                newState.DamageDone += damage;
                newState.Mana += mana;
            }
            #endregion
            #endregion
            #region Specific state updates
            newState.Mana += .08f * newState.MaxMana;
            #endregion
            progressTime(newState, timeAfterCast);
            return newState;
        }

        public static SimuState runNothing(SimuState state, float length)
        {
            SimuState newState = state.Clone();
            progressTime(newState, length);
            return newState;
        }

        protected static float CalculateManaRegen(float intel, float spi)
        {
            float baseRegen = 0.005575f;
            return (float)Math.Round(5f * (0.001f + (float)Math.Sqrt(intel) * spi * baseRegen));
        }

        protected static void RegenMana(SimuState state, float timeDifference)
        {
            Stats stats = state.stats;
            float spiritRegen = CalculateManaRegen(stats.Intellect, stats.Spirit);
            float replenishRegen = stats.Mana * 0.0025f * 5 * (state.calcOpts.ReplenishmentUptime / 100f);
            float outFSRRegen = spiritRegen + stats.Mp5 + replenishRegen;
            float inFSRRegen = spiritRegen * stats.SpellCombatManaRegeneration + stats.Mp5 + replenishRegen;
            float outFSRTime = timeDifference;
            if (state.LastCast >= 0) outFSRTime = state.Time - state.LastCast - 5f;
            if (outFSRTime < 0) outFSRTime = 0;
            else if (outFSRTime > timeDifference) outFSRTime = timeDifference;
            float inFSRTime = timeDifference - outFSRTime;
            state.Mana += inFSRTime * inFSRRegen / 5f + outFSRTime * outFSRRegen / 5f;
        }

        protected static List<SimuState> successors(SimuState state)
        {
            return state.simulator.successors(state);
        }

        public static void handleTrinket(SimuState state, Stats stats)
        {
            state.effects += new SpecialEffects(stats);
        }

        protected static List<SimuState> search(Stats baseStats, ShamanTalents talents, CalculationOptionsElemental calcOpts, float maxTime)
        {
            List<SimuState> states = new List<SimuState>();
            Queue<SimuState> queue = new Queue<SimuState>();
            #region Base state
            SimuState baseState = new SimuState(baseStats, talents, calcOpts);
            /*if (calcOpts.calculatedStats.LocalCharacter.Trinket1 != null)
                handleTrinket(baseState, calcOpts.calculatedStats.LocalCharacter.Trinket1.Item.Stats);
            if (calcOpts.calculatedStats.LocalCharacter.Trinket2 != null)
                handleTrinket(baseState, calcOpts.calculatedStats.LocalCharacter.Trinket2.Item.Stats);*/
            baseState.effects = new SpecialEffects(baseStats);
            #endregion
            /*SimuState stateSim1 = baseState.Clone();
            stateSim1.simulator = new DefaultRotation { castLvBWithoutFS = true, castFSWheneverPossible = true };
            queue.Enqueue(stateSim1);
            SimuState stateSim2 = baseState.Clone();
            stateSim2.simulator = new DefaultRotation { castLvBWithoutFS = true, castFSWheneverPossible = false };
            queue.Enqueue(stateSim2);
            SimuState stateSim3 = baseState.Clone();
            stateSim3.simulator = new DefaultRotation { castLvBWithoutFS = false, castFSWheneverPossible = true };
            queue.Enqueue(stateSim3);
            SimuState stateSim4 = baseState.Clone();
            stateSim4.simulator = new DefaultRotation { castLvBWithoutFS = false, castFSWheneverPossible = false };
            queue.Enqueue(stateSim4);*/
            baseState.simulator = new DefaultRotation
            {
                castLvBWithoutFS = calcOpts.UseLvBalways,
                castFSWheneverPossible = calcOpts.UseFSalways
            };
            queue.Enqueue(baseState);
            while (queue.Count > 0)
            {
                SimuState current = queue.Dequeue();
                List<SimuState> newStates = successors(current).FindAll(s => s.Time < maxTime);
                if (newStates.Count == 0)
                {
                    states.Add(current);
                }
                else
                {
                    newStates.ForEach(s => queue.Enqueue(s));
                }
            }
            return states;
        }

        public static void solve(Stats stats, ShamanTalents talents, CalculationOptionsElemental calcOpts)
        {
            List<SimuState> states = search(stats, talents, calcOpts, calcOpts.FightDuration);
            states.Sort(delegate(SimuState s1, SimuState s2) { return s2.DamageDone.CompareTo(s1.DamageDone); });
            SimuState best = states[0];

            CharacterCalculationsElemental calculatedStats = calcOpts.calculatedStats;
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
            #region Regens
            float spiritRegen = CalculateManaRegen(stats.Intellect, stats.Spirit);
            float replenishRegen = stats.Mana * 0.0025f * 5 * (calcOpts.ReplenishmentUptime / 100f);
            #endregion
            float FightDuration = calcOpts.FightDuration;
            calculatedStats.BurstPoints = 0f;
            calculatedStats.SustainedPoints = 2f * best.DamageDone / FightDuration;
            calculatedStats.OverallPoints = calculatedStats.BurstPoints + calculatedStats.SustainedPoints;
            calculatedStats.ManaRegenInFSR = spiritRegen * calculatedStats.BasicStats.SpellCombatManaRegeneration + calculatedStats.BasicStats.Mp5;
            calculatedStats.ManaRegenOutFSR = spiritRegen + calculatedStats.BasicStats.Mp5;
            calculatedStats.ReplenishMP5 = replenishRegen;
            calculatedStats.LightningBolt = LB;
            calculatedStats.ChainLightning = CL;
            calculatedStats.ChainLightning3 = CL3;
            calculatedStats.ChainLightning4 = CL4;
            calculatedStats.FlameShock = FS;
            calculatedStats.LavaBurst = LvB;
            calculatedStats.EarthShock = ES;
            calculatedStats.FrostShock = FrS;
            calculatedStats.TimeToOOM = 0f;
            calculatedStats.CastRegenFraction = 0f;
            calculatedStats.CastFraction = best.Casts / FightDuration;
            calculatedStats.CritFraction = best.Crits / FightDuration;
            calculatedStats.MissFraction = best.Misses / FightDuration;
            calculatedStats.RotationDPS = 0f;
            calculatedStats.RotationMPS = 0f;
            calculatedStats.TotalDPS = best.DamageDone / FightDuration;
            calculatedStats.ClearCast_FlameShock = 0f;
            calculatedStats.ClearCast_LavaBurst = 0f;
            calculatedStats.ClearCast_LightningBolt = 0f;
        }
    }

}