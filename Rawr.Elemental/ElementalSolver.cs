using System;
using System.Collections.Generic;

namespace Rawr.Elemental
{
    public class Rotation
    {
        public float CastFraction;
        public float CritFraction;
        public float MissFraction;
        public float DPS;
        public float MPS;

        public float CC_FS;
        public float CC_LvB;
        public float CC_LB;

        public Spell LB;
        public Spell CL;
        public Spell CL3;
        public Spell CL4;
        public Spell LvB;
        public Spell LvBFS;
        public Spell FS;
        public Spell ES;
        public Spell FrS;
    }

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
        public List<Proc> procs = new List<Proc>();

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

        public SimuState Clone() 
        {
            SimuState clone = this.MemberwiseClone() as SimuState;
            clone.procs = new List<Proc>();
            procs.ForEach(p => clone.procs.Add(p.Clone()));
            clone.stats = stats.Clone();
            if (FlameShockDoT!=null) clone.FlameShockDoT = FlameShockDoT.Clone();
            return clone;
        }

    }

    public abstract class Proc
    {
        public abstract SimuState use(SimuState state);
        public abstract void proc(SimuState state, Stats stats, float cast, float hit, float crit, float miss, float dot, out float Damage, out float Mana);
        public abstract Stats run(SimuState state, float dTime, out float Damage);
        public virtual Proc Clone()
        {
            return this.MemberwiseClone() as Proc;
        }
    }

    public class GeneralProc : Proc
    {
        protected Stats statsOnProc;
        protected float damageOnProc;
        protected float dpsOnProc;
        protected float manaOnProc;
        protected float procOnCrit, procOnHit, procOnMiss, procOnCast, procOnDot;
        protected float Duration;
        protected float internalCD;
        protected float MaxStack;

        protected Uncertainty activity, cooldown;

        protected GeneralProc()
        {
        }

        public GeneralProc(Stats stats, float dps, float dmg, float mana, float duration, float iCD, float onCast, float onHit, float onCrit, float onMiss, float onDot, float maxStack)
        {
            statsOnProc = stats;
            dpsOnProc = dps;
            damageOnProc = dmg;
            manaOnProc = mana;
            Duration = duration;
            internalCD = iCD;
            procOnCast = onCast;
            procOnHit = onHit;
            procOnCrit = onCrit;
            procOnMiss = onMiss;
            procOnDot = onDot;
            activity = new Uncertainty(duration, maxStack);
            cooldown = new Uncertainty(iCD, 1);
        }

        public override SimuState use(SimuState state)
        {
            return null;
        }

        public override void proc(SimuState state, Stats stats, float cast, float hit, float crit, float miss, float dot, out float Damage, out float Mana)
        {
            float cdValue = cooldown.Value;
            if (cdValue <= 1)
            {
                float value = cast * procOnCast + hit * procOnHit + crit * procOnCrit + miss * procOnMiss + dot * procOnDot;
                value *= 1f - cdValue;
                cooldown.Add(value);
                activity.Add(value);
                Damage = damageOnProc * value;
                Mana = manaOnProc * value;
            }
            else 
            {
                Damage = 0;
                Mana = 0;
            }
        }

        public override Stats run(SimuState state, float dTime, out float Damage)
        {
            float value = activity.Value;
            activity.Run(dTime);
            cooldown.Run(dTime);
            Damage = dpsOnProc * dTime * value;
            return statsOnProc * value;
        }

        public override Proc Clone()
        {
            GeneralProc p = this.MemberwiseClone() as GeneralProc;
            p.activity = activity.Clone();
            p.cooldown = cooldown.Clone();
            return p;
        }
    }

    public class CapacitorProc : Proc
    {
        protected float damageOnProc;
        protected float procOnCrit, procOnHit, procOnMiss, procOnCast, procOnDot;
        protected float internalCD;
        protected int MaxStack;

        protected float Timeout;
        protected int stack = 0;
        protected float value = 0;
        protected float Time = 0f;
        protected float Last = float.NegativeInfinity;

        public CapacitorProc(float dmg, float iCD, float onCast, float onHit, float onCrit, float onMiss, float onDot, int count)
        {
            damageOnProc = dmg;
            internalCD = iCD;
            procOnCast = onCast;
            procOnHit = onHit;
            procOnCrit = onCrit;
            procOnMiss = onMiss;
            procOnDot = onDot;
            MaxStack = count;
        }

        public override SimuState use(SimuState state)
        {
            return null;
        }

        public override void proc(SimuState state, Stats stats, float cast, float hit, float crit, float miss, float dot, out float Damage, out float Mana)
        {
            Damage = 0;
            Mana = 0;

            float plus = cast * procOnCast + hit * procOnHit + crit * procOnCrit + miss * procOnMiss + dot * procOnDot;
            if (plus > 0)
            {
                if (Last != float.NegativeInfinity && Time - Last < internalCD) return;
                if (MaxStack == 1)
                {
                    Last = Time;
                    // pop
                    Damage = damageOnProc * plus * (1f + state.stats.SpellCrit);
                }
                else
                {
                    value += plus;
                    //stack++;
                    Last = Time;
                    if (value >= MaxStack)
                    {
                        // pop
                        Damage = damageOnProc * (1f + state.stats.SpellCrit) * (1f + state.stats.SpellHit);
                        value -= MaxStack;
                        // stack = 0;
                    }
                }
            }
        }

        public override Stats run(SimuState state, float dTime, out float Damage)
        {
            Time += dTime;
            Damage = 0;
            return new Stats { };
        }

        public override Proc Clone()
        {
            CapacitorProc p = this.MemberwiseClone() as CapacitorProc;
            return p;
        }
    }

    public class GeneralUse : Proc
    {
        protected Stats statsOnProc;
        protected float damageOnProc;
        protected float dpsOnProc;
        protected float manaOnProc;
        protected float Duration;
        protected float internalCD;
        protected float Cooldown;
        protected bool gcdOnUse;
        protected float timeLeft = 0f;

        public GeneralUse(Stats stats, float dps, float dmg, float mana, float duration, float cooldown, bool gcd)
        {
            statsOnProc = stats;
            dpsOnProc = dps;
            damageOnProc = dmg;
            manaOnProc = mana;
            Duration = duration;
            internalCD = cooldown;
            gcdOnUse = gcd;
            Cooldown = 0f;
        }

        public override SimuState use(SimuState state)
        {
            if (Cooldown > 0f) return null;

            timeLeft = Duration;
            Cooldown = internalCD;
            SimuState newState = state.Clone();
            Cooldown = 0f;
            timeLeft = 0f;

            newState.DamageDone += damageOnProc;
            newState.LastSpell = null;

            if (gcdOnUse)
            {
                float gcd = (float)Math.Round(1.5f / (1 + state.stats.SpellHaste), 4);
                Simulator.progressTime(newState, gcd);
            }

            return newState;
        }

        public override void proc(SimuState state, Stats stats, float cast, float hit, float crit, float miss, float dot, out float Damage, out float Mana)
        {
            Damage = 0;
            Mana = 0;
        }

        public override Stats run(SimuState state, float dTime, out float Damage)
        {
            Cooldown -= dTime; // on Use
            timeLeft -= dTime;
            if (timeLeft > 0)
            {
                Damage = dpsOnProc * dTime;
                return statsOnProc;
            }
            else
            {
                Damage = 0;
                return new Stats { };
            }
        }

        public override Proc Clone()
        {
            GeneralUse p = (GeneralUse)this.MemberwiseClone();
            return p;
        }
    }

    public class RotationSim
    {
        public virtual List<SimuState> successors(SimuState state)
        {
            List<SimuState> newStates = new List<SimuState>();
            foreach (Proc p in state.procs)
            {
                SimuState proc = p.use(state);
                if (proc != null)
                {
                    newStates.Add(proc);
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
            foreach (Proc p in state.procs)
            {
                SimuState proc = p.use(state);
                if (proc != null)
                {
                    newStates.Add(proc);
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
                foreach (Proc p in state.procs)
                {
                    float mana, damage;
                    p.proc(state, state.stats,
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
            foreach (Proc p in state.procs)
            {
                float damage;
                state.stats += p.run(state, dT, out damage);
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
            foreach (Proc p in newState.procs)
            {
                float mana;
                p.proc(newState, newState.stats,
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
            foreach (Proc p in newState.procs)
            {
                float mana;
                p.proc(newState, newState.stats,
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
            foreach (Proc p in newState.procs)
            {
                float mana;
                p.proc(newState, newState.stats,
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
            foreach (Proc p in newState.procs)
            {
                float damage, mana;
                p.proc(newState, newState.stats,
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
            foreach (Proc p in newState.procs)
            {
                float mana, damage;
                p.proc(newState, newState.stats,
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

        public static void createSpellPowerProc(SimuState state, float value, float duration, float iCD, float onCast, float onHit, float onCrit, float onMiss, float onDot, float maxStack)
        {
            if (value != 0)
            {
                Proc p = new GeneralProc(new Stats { SpellPower = value }, 0f, 0f, 0f, duration, iCD, onCast, onHit, onCrit, onMiss, onDot, maxStack);
                state.procs.Add(p);
            }
        }

        public static void createSpellPowerUse(SimuState state, float value, float duration, float CD, bool gcd)
        {
            if (value != 0)
            {
                Proc p = new GeneralUse(new Stats { SpellPower = value }, 0f, 0f, 0f, duration, CD, gcd);
                state.procs.Add(p);
            }
        }

        public static void createSpellHasteProc(SimuState state, float value, float duration, float iCD, float onCast, float onHit, float onCrit, float onMiss, float onDot, float maxStack)
        {
            if (value != 0)
            {
                Proc p = new GeneralProc(new Stats { HasteRating = value }, 0f, 0f, 0f, duration, iCD, onCast, onHit, onCrit, onMiss, onDot, maxStack);
                state.procs.Add(p);
            }
        }

        public static void createSpellHasteUse(SimuState state, float value, float duration, float CD, bool gcd)
        {
            if (value != 0)
            {
                Proc p = new GeneralUse(new Stats { HasteRating = value }, 0f, 0f, 0f, duration, CD, gcd);
                state.procs.Add(p);
            }
        }

        public static void createMP5Proc(SimuState state, float value, float duration, float iCD, float onCast, float onHit, float onCrit, float onMiss, float onDot, float maxStack)
        {
            if (value != 0)
            {
                Proc p = new GeneralProc(new Stats { Mp5 = value }, 0f, 0f, 0f, duration, iCD, onCast, onHit, onCrit, onMiss, onDot, maxStack);
                state.procs.Add(p);
            }
        }

        public static void createMP5Use(SimuState state, float value, float duration, float CD, bool gcd)
        {
            if (value != 0)
            {
                Proc p = new GeneralUse(new Stats { Mp5 = value }, 0f, 0f, 0f, duration, CD, gcd);
                state.procs.Add(p);
            }
        }

        public static void createManaProc(SimuState state, float value, float duration, float iCD, float onCast, float onHit, float onCrit, float onMiss, float onDot, float maxStack)
        {
            if (value != 0)
            {
                Proc p = new GeneralProc(new Stats { }, 0f, 0f, value, duration, iCD, onCast, onHit, onCrit, onMiss, onDot, maxStack);
                state.procs.Add(p);
            }
        }

        public static void createManaUse(SimuState state, float value, float duration, float CD, bool gcd)
        {
            if (value != 0)
            {
                Proc p = new GeneralUse(new Stats { }, 0f, 0f, value, duration, CD, gcd);
                state.procs.Add(p);
            }
        }

        public static void createSPCProc(SimuState state, float value, float duration, float iCD, float onCast, float onHit, float onCrit, float onMiss, float onDot, float maxStack)
        {
            if (value != 0)
            {
                Proc p = new GeneralProc(new Stats { SpellCombatManaRegeneration = value }, 0f, 0f, 0f, duration, iCD, onCast, onHit, onCrit, onMiss, onDot, maxStack);
                state.procs.Add(p);
            }
        }

        public static void createSpiritUse(SimuState state, float value, float duration, float CD, bool gcd)
        {
            if (value != 0)
            {
                Proc p = new GeneralUse(new Stats { Spirit = value }, 0f, 0f, 0f, duration, CD, gcd);
                state.procs.Add(p);
            }
        }

        public static void createDamageProc(SimuState state, float value, float duration, float iCD, float onCast, float onHit, float onCrit, float onMiss, float onDot, float maxStack)
        {
            if (value != 0)
            {
                Proc p = new GeneralProc(new Stats { }, 0f, value, 0f, duration, iCD, onCast, onHit, onCrit, onMiss, onDot, maxStack);
                state.procs.Add(p);
            }
        }

        public static void handleTrinket(SimuState state, Stats stats)
        {
            createSpellPowerProc(state, stats.SpellPowerFor10SecOnHit_10_45, 10, 45, 0, .1f, 0, 0, 0, 1);
            createSpellPowerProc(state, stats.SpellPowerFor10SecOnResist, 10, 0, 0, 0, 0, .1f, 0, 1);
            createSpellPowerProc(state, stats.SpellPowerFor10SecOnCast_10_45, 10, 45, .1f, 0, 0, 0, 0, 1);
            createSpellPowerProc(state, stats.SpellPowerFor10SecOnCast_15_45, 10, 45, .15f, 0, 0, 0, 0, 1);
            createSpellPowerProc(state, stats.SpellPowerFor10SecOnCrit_20_45, 10, 45, 0, 0, .2f, 0, 0, 1);
            createSpellPowerProc(state, stats.SpellPowerFor15SecOnCrit_20_45, 15, 45, 0, 0, .2f, 0, 0, 1);
            createSpellPowerUse(state, stats.SpellPowerFor15SecOnUse90Sec, 15, 90, false);
            createSpellPowerUse(state, stats.SpellPowerFor15SecOnUse2Min, 15, 120, false);
            createSpellPowerUse(state, stats.SpellPowerFor20SecOnUse2Min, 20, 120, false);
            createSpellPowerUse(state, stats.SpellPowerFor20SecOnUse5Min, 20, 300, false);

            createSpellHasteProc(state, stats.SpellHasteFor10SecOnCast_10_45, 10, 45, .1f, 0, 0, 0, 0, 1);
            createSpellHasteProc(state, stats.SpellHasteFor6SecOnCast_15_45, 6, 45, .15f, 0, 0, 0, 0, 1);
            createSpellHasteProc(state, stats.SpellHasteFor6SecOnHit_10_45, 6, 45, 0, .1f, 0, 0, 0, 1);
            createSpellHasteUse(state, stats.HasteRatingFor20SecOnUse2Min, 20, 120, false);

            // Each spell cast within 20 seconds will grant a stacking bonus of 21 mana regen per 5 sec. 
            // Expires after 20 seconds.  Abilities with no mana cost will not trigger this trinket.
            // For easy calculation, model this as a full stack for 20 seconds. Who cares?
            // Mp5 += CalculateEffect(stats.Mp5OnCastFor20SecOnUse2Min * 20f * HitsFraction, 20f, 120f, 0, FightDuration);
            // createMP5Use(state, 5f / 12f * stats.Mp5OnCastFor20SecOnUse2Min, 20, 120, false);
            createManaProc(state, stats.ManaRestoreOnCrit_25_45, 0, 45, 0, 0, .25f, 0, 0, 1);
            createMP5Proc(state, 5f / 15f * stats.ManaRestoreOnCast_10_45, 15, 45, .1f, 0, 0, 0, 0, 1);
            createMP5Use(state, 5f / 12f * stats.ManaregenOver20SecOnUse3Min, 12, 180, false);
            createMP5Use(state, 5f / 12f * stats.ManaregenOver20SecOnUse5Min, 12, 180, false);
            createMP5Use(state, 5f / 12f * stats.ManaRestore5min, 12, 300, false);
            createMP5Proc(state, 5f / 15f * stats.MementoProc, 15, 45, .1f, 0, 0, 0, 0, 1);
            if (stats.FullManaRegenFor15SecOnSpellcast > 0)
                createSPCProc(state, 1f, 15f, 0, stats.FullManaRegenFor15SecOnSpellcast / 100f, 0, 0, 0, 0, 1);
            createSpiritUse(state, stats.SpiritFor20SecOnUse2Min, 20, 120, false);

            createDamageProc(state, stats.DarkmoonCardDeathProc, 0, 45f, .35f, 0, 0, 0, 0, 1);
            if (stats.PendulumOfTelluricCurrentsProc > 0)
            {
                createDamageProc(state, 1460, 0, 45, 0, .15f, 0, 0, 0, 1);
            }
            if (stats.ThunderCapacitorProc > 0)
            {
                state.procs.Add(new CapacitorProc(1276, 2.5f, 0, 0, 1f, 0, 0, 4));
            }
            if (stats.LightningCapacitorProc > 0)
            {
                state.procs.Add(new CapacitorProc(750, 2.5f, 0, 0, 1f, 0, 0, 3));
            }
            if (stats.ExtractOfNecromanticPowerProc > 0)
            {
                state.procs.Add(new CapacitorProc(550, 0f, 0, 0, 0, 0, .1f, 1));
            }
        }

        protected static List<SimuState> search(Stats baseStats, ShamanTalents talents, CalculationOptionsElemental calcOpts, float maxTime)
        {
            List<SimuState> states = new List<SimuState>();
            Queue<SimuState> queue = new Queue<SimuState>();
            #region Base state
            SimuState baseState = new SimuState(baseStats, talents, calcOpts);
            if (calcOpts.calculatedStats.LocalCharacter.Trinket1 != null)
                handleTrinket(baseState, calcOpts.calculatedStats.LocalCharacter.Trinket1.Stats);
            if (calcOpts.calculatedStats.LocalCharacter.Trinket2 != null)
                handleTrinket(baseState, calcOpts.calculatedStats.LocalCharacter.Trinket2.Stats);
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

    public static class Solver
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
            float clearcastingFS=0f, clearcastingLvB=0f, clearcastingLB=0f;
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

            // SIMPLE MODEL until we have something better
            // Assume: glyph of flame shock, LvB on every cd, refresh FS when it falls off, no CL use
            #region Lightning Bolt Haste Trinket
            Stats effectsStats = new Stats
            {
                SpellHaste = character.StatConversion.GetSpellHasteFromRating(stats.LightningBoltHasteProc_15_45 * 10f / 55f) / 100f,
            };
            stats = stats + effectsStats;
            #endregion

            Rotation rot = getRotation(stats, talents, calcOpts);
            Stats statsFromUsage = getTrinketStats(stats,  calcOpts.FightDuration, rot.CastFraction, rot.CritFraction, rot.MissFraction);
            stats = stats + statsFromUsage;
            rot = getRotation(stats, talents, calcOpts);

            /* Regen variables: (divide by 5 for regen per second)
             * While casting: ManaRegInFSR
             * While casting: ManaRegOutFSR (stop casting, but keep trinket effects)
             * During regen: ManaRegOutFSRNoCasting */
            #region Calculate Regen
            float spiritRegen = CalculateManaRegen(stats.Intellect, stats.Spirit);
            // Also calculate extra spirit while casting from trinkets
            float spiritRegenPlusTrinket = CalculateManaRegen(stats.Intellect, stats.ExtraSpiritWhileCasting + stats.Spirit);
            float replenishRegen = stats.Mana * 0.0025f * 5 * (calcOpts.ReplenishmentUptime / 100f);
            //spirit regen + mp5 + replenishmp5
            float ManaRegInFSR = spiritRegenPlusTrinket * stats.SpellCombatManaRegeneration + stats.Mp5 + replenishRegen;
            float ManaRegOutFSR = spiritRegenPlusTrinket + stats.Mp5 + replenishRegen;
            float ratio_extraspi = 0.8f; // OK, lets assume a mana starved person keeps 80% of the extra spirit effect, because they will keep casting anyway
            float ManaRegOutFSRNoCasting = (1 - ratio_extraspi) * spiritRegen + ratio_extraspi * spiritRegenPlusTrinket + stats.Mp5 + replenishRegen;
            float ManaRegen = ManaRegInFSR;
            #endregion
            
            // Mana potion: extraMana
            #region Mana potion
            float extraMana = new float[] { 0f, 1800f, 2200, 2400, 4300 }[calcOpts.ManaPot];
            extraMana *= (stats.BonusManaPotion + 1f);
            #endregion

            // Thunderstorm usage
            #region Thunderstorm
            // THUNDERSTORM
            if (calcOpts.UseThunderstorm) rot.MPS -= CalculateEffect(.08f * stats.Mana, 0f, 45f, 0f, calcOpts.FightDuration);
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
            calculatedStats.ManaRegenInFSR = spiritRegen * calculatedStats.BasicStats.SpellCombatManaRegeneration + calculatedStats.BasicStats.Mp5;
            calculatedStats.ManaRegenOutFSR = spiritRegen + calculatedStats.BasicStats.Mp5;
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
                    Math.Min(Math.Max(0,(FightDuration % (ecd + icd)) - ecd), duration);
                activity /= FightDuration;
                return value * activity;
            }
        }

        private static Stats getTrinketStats(Stats stats, float FightDuration, float HitsFraction, float CritsFraction, float MissFraction) 
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
            // For easy calculation, model this as a full stack for 20 seconds. Who cares?
            Mp5 += CalculateEffect(stats.Mp5OnCastFor20SecOnUse2Min*20f*HitsFraction, 20f, 120f, 0, FightDuration);
            Mp5 += CalculateEffect(5f / 15f * stats.ManaRestoreOnCast_10_45, 15f, 45f, 1 / (HitsFraction * .10f), FightDuration);
            Mp5 += CalculateEffect(stats.ManaRestoreOnCrit_25_45, 0f, 45f, 1 / (CritsFraction * .25f), FightDuration);
            Mp5 += CalculateEffect(5f/12f*stats.ManaregenOver20SecOnUse3Min, 12f, 180f, 0, FightDuration);
            Mp5 += CalculateEffect(5f/12f*stats.ManaregenOver20SecOnUse5Min, 12f, 300f, 0, FightDuration);
            Mp5 += CalculateEffect(5f / 12f * stats.ManaRestore5min, 12f, 300f, 0, FightDuration);
            Mp5 += CalculateEffect(stats.MementoProc, 15f, 45f, 1 / (HitsFraction * .10f), FightDuration);
            SpellCombatManaRegeneration += CalculateEffect(1f, 15f, 0, 
                1 / ((stats.FullManaRegenFor15SecOnSpellcast / 100f) * HitsFraction), FightDuration);
            Spirit += CalculateEffect(stats.SpiritFor20SecOnUse2Min, 20f, 120f, 0, FightDuration);
            return new Stats() 
            {
                Spirit = Spirit,
                HasteRating = Haste,
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