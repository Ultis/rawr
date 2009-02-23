using System;
using System.Collections.Generic;

namespace Rawr.Elemental
{
    class Uncertainty
    {
        protected class Part
        {
            public float value { get; private set; }
            public float start { get; private set; }
            public Part(float value, float start) { this.value = value; this.start = start; }
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
            if (value != 0)
            {
                parts.Add(new Part(value, Time));
            }
        }

        public void Run(float dT)
        {
            Time += dT;
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

    public abstract class SpecialEffect
    {
        /*
         * How to use this class?
         * If you are working with a SIMULATOR
         *  - use = returns "true" if the effect is "Used", use SpecialEffects.Clone(effect).
         *          if (effect.use(stats, out Dmg, out dT)) newEffects = effects.Clone(effect);
         *  - proc = on every action that could proc, call this method
         *  - run = whenever time progresses, use this method
         * If you are working with an ESTIMATOR
         *  - estimate = estimates effect strength in a fight
         */
        public virtual bool use(Stats stats, out float Damage, out float ProgressTime)
        {
            Damage = 0f;
            ProgressTime = 0f;
            return false;
        }
        public abstract void proc(Stats stats, float cast, float hit, float crit, float miss, float dot, out float Damage, out float Mana);
        public abstract Stats run(float dTime, out float Damage);
        public abstract Stats estimate(float TotalTime, float castsPS, float hitsPS, float critsPS, float missesPS, float ticksPS, out float Damage);
        public virtual SpecialEffect Clone()
        {
            return this.MemberwiseClone() as SpecialEffect;
        }

        public static float EstimateUptime(float duration, float icd, float pps, float FightDuration, out float procs)
        {
            procs = 0;
            float averageTimeToProc = pps == 0 ? 0 : 1 / pps;
            float totalCD = icd + averageTimeToProc;
            if (averageTimeToProc > FightDuration) return 0f;
            FightDuration -= averageTimeToProc;
            if (duration == 0) // instant proc
            {
                procs = (float)Math.Floor(FightDuration / totalCD) + 1;
                return procs / FightDuration; // return value per second
            }
            else
            {
                procs = (float)Math.Floor(FightDuration / totalCD) + 1;
                float activity = (procs - 1) * duration +
                    Math.Min(Math.Max(0, (FightDuration % totalCD) - averageTimeToProc), duration);
                activity /= FightDuration;
                return activity; // return uptime per second
            }
        }
    }

    class GeneralProc : SpecialEffect
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

        public override void proc(Stats stats, float cast, float hit, float crit, float miss, float dot, out float Damage, out float Mana)
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

        public override Stats run(float dTime, out float Damage)
        {
            float value = activity.Value;
            activity.Run(dTime);
            cooldown.Run(dTime);
            Damage = dpsOnProc * dTime * value;
            return statsOnProc * activity.Value;
        }

        public override SpecialEffect Clone()
        {
            GeneralProc p = this.MemberwiseClone() as GeneralProc;
            p.activity = activity.Clone();
            p.cooldown = cooldown.Clone();
            return p;
        }

        public override Stats estimate(float TotalTime, float castsPS, float hitsPS, float critsPS, float missesPS, float ticksPS, out float Damage)
        {
            float procs;
            float up = EstimateUptime(Duration, internalCD, hitsPS * procOnHit + castsPS * procOnCast + critsPS * procOnCrit + missesPS * procOnMiss + ticksPS * procOnDot, TotalTime, out procs);
            float upT = TotalTime * up;
            // up = average uptime for over time effects
            // upT = amount of procs for instant effects
            Stats result = statsOnProc * up;
            result.Mp5 += 5 * up * manaOnProc; // upT * manaOnProc / TotalTime
            Damage = upT * damageOnProc + dpsOnProc * up;
            return result;
        }
    }

    class CapacitorProc : SpecialEffect
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

        public override void proc(Stats stats, float cast, float hit, float crit, float miss, float dot, out float Damage, out float Mana)
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
                    Damage = damageOnProc * plus * (1f + stats.SpellCrit);
                }
                else
                {
                    value += plus;
                    //stack++;
                    Last = Time;
                    if (value >= MaxStack)
                    {
                        // pop
                        Damage = damageOnProc * (1f + stats.SpellCrit) * (1f + stats.SpellHit);
                        value -= MaxStack;
                        // stack = 0;
                    }
                }
            }
        }

        public override Stats run(float dTime, out float Damage)
        {
            Time += dTime;
            Damage = 0;
            return new Stats { };
        }

        public override SpecialEffect Clone()
        {
            CapacitorProc p = this.MemberwiseClone() as CapacitorProc;
            return p;
        }

        public override Stats estimate(float TotalTime, float castsPS, float hitsPS, float critsPS, float missesPS, float ticksPS, out float Damage)
        {
            Damage = 0;
            float pps = hitsPS * procOnHit + castsPS * procOnCast + critsPS * procOnCrit + missesPS * procOnMiss + ticksPS * procOnDot;
            if (pps > 0)
            {
                float timeToProc = 1 / pps;
                if (timeToProc < internalCD) timeToProc = internalCD;
                timeToProc *= MaxStack;
                if (TotalTime < timeToProc)
                {
                    float procs = (float)Math.Floor(TotalTime / timeToProc);
                    Damage = procs * damageOnProc;
                }
            }
            return new Stats() {};
        }
    }

    class GeneralUse : SpecialEffect
    {
        protected Stats statsOnProc;
        protected float damageOnProc;
        protected float dpsOnProc;
        protected float manaOnProc;
        protected float Duration;
        protected float internalCD;
        protected float Cooldown = 0f;
        protected bool gcdOnUse;
        protected float timeLeft = 0f;
        protected bool activateOnClone = false;

        public GeneralUse(Stats stats, float dps, float dmg, float mana, float duration, float cooldown, bool gcd)
        {
            statsOnProc = stats;
            dpsOnProc = dps;
            damageOnProc = dmg;
            manaOnProc = mana;
            Duration = duration;
            internalCD = cooldown;
            gcdOnUse = gcd;
        }

        public override bool use(Stats stats, out float Damage, out float ProgressTime)
        {
            Damage = 0;
            ProgressTime = 0;

            if (Cooldown > 0f) return false;

            activateOnClone = true;
            Damage = damageOnProc;

            if (gcdOnUse)
            {
                float Speed = (1f + stats.SpellHaste) * (1f + stats.HasteRating * 0.000304971132f);
                float gcd = (float)Math.Round(1.5f / Speed, 4);
                if (gcd < 1f) gcd = 1f;
                ProgressTime = gcd;
            }

            return true;
        }

        public override void proc(Stats stats, float cast, float hit, float crit, float miss, float dot, out float Damage, out float Mana)
        {
            Damage = 0;
            Mana = 0;
        }

        public override Stats run(float dTime, out float Damage)
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

        public override SpecialEffect Clone()
        {
            GeneralUse p = (GeneralUse)this.MemberwiseClone();
            if (activateOnClone)
            {
                p.timeLeft = Duration;
                p.Cooldown = internalCD;
                activateOnClone = false;
            }
            return p;
        }

        public override Stats estimate(float TotalTime, float castsPS, float hitsPS, float critsPS, float missesPS, float ticksPS, out float Damage)
        {
            float procs;
            float up = EstimateUptime(Duration, internalCD, 0, TotalTime, out procs);
            // up = average uptime for over time effects
            Stats result = statsOnProc * up;
            result.Mp5 += 5 * procs * manaOnProc; 
            Damage = procs * damageOnProc + dpsOnProc * up;
            return result;
        }
    }

    public class SpecialEffects
    {
        private List<SpecialEffect> _effects;
        public List<SpecialEffect> Effects
        {
            get { return _effects; }
        }

        protected SpecialEffects()
        {
            _effects = new List<SpecialEffect>();
        }

        public SpecialEffects(Stats stats)
        {
            _effects = new List<SpecialEffect>();
            convertState(stats);
        }

        protected void createSpellPowerProc(float value, float duration, float iCD, float onCast, float onHit, float onCrit, float onMiss, float onDot, float maxStack)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralProc(new Stats { SpellPower = value }, 0f, 0f, 0f, duration, iCD, onCast, onHit, onCrit, onMiss, onDot, maxStack));
            }
        }

        protected void createSpellPowerUse(float value, float duration, float CD, bool gcd)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralUse(new Stats { SpellPower = value }, 0f, 0f, 0f, duration, CD, gcd));
            }
        }

        protected void createSpellHasteProc(float value, float duration, float iCD, float onCast, float onHit, float onCrit, float onMiss, float onDot, float maxStack)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralProc(new Stats { HasteRating = value }, 0f, 0f, 0f, duration, iCD, onCast, onHit, onCrit, onMiss, onDot, maxStack));
            }
        }

        protected void createSpellHasteUse(float value, float duration, float CD, bool gcd)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralUse(new Stats { HasteRating = value }, 0f, 0f, 0f, duration, CD, gcd));
            }
        }

        protected void createMP5Proc(float value, float duration, float iCD, float onCast, float onHit, float onCrit, float onMiss, float onDot, float maxStack)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralProc(new Stats { Mp5 = value }, 0f, 0f, 0f, duration, iCD, onCast, onHit, onCrit, onMiss, onDot, maxStack));
            }
        }

        protected void createMP5Use(float value, float duration, float CD, bool gcd)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralUse(new Stats { Mp5 = value }, 0f, 0f, 0f, duration, CD, gcd));
            }
        }

        protected void createManaProc(float value, float duration, float iCD, float onCast, float onHit, float onCrit, float onMiss, float onDot, float maxStack)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralProc(new Stats { }, 0f, 0f, value, duration, iCD, onCast, onHit, onCrit, onMiss, onDot, maxStack));
            }
        }

        protected void createManaUse(float value, float duration, float CD, bool gcd)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralUse(new Stats { }, 0f, 0f, value, duration, CD, gcd));
            }
        }

        protected void createSPCProc(float value, float duration, float iCD, float onCast, float onHit, float onCrit, float onMiss, float onDot, float maxStack)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralProc(new Stats { SpellCombatManaRegeneration = value }, 0f, 0f, 0f, duration, iCD, onCast, onHit, onCrit, onMiss, onDot, maxStack));
            }
        }

        protected void createSpiritUse(float value, float duration, float CD, bool gcd)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralUse(new Stats { Spirit = value }, 0f, 0f, 0f, duration, CD, gcd));
            }
        }

        protected void createDamageProc(float value, float duration, float iCD, float onCast, float onHit, float onCrit, float onMiss, float onDot, float maxStack)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralProc(new Stats { }, 0f, value, 0f, duration, iCD, onCast, onHit, onCrit, onMiss, onDot, maxStack));
            }
        }

        protected void convertState(Stats stats)
        {
            createSpellPowerProc(stats.SpellPowerFor10SecOnHit_10_45, 10, 45, 0, .1f, 0, 0, 0, 1);
            createSpellPowerProc(stats.SpellPowerFor10SecOnResist, 10, 0, 0, 0, 0, .1f, 0, 1);
            createSpellPowerProc(stats.SpellPowerFor10SecOnCast_10_45, 10, 45, .1f, 0, 0, 0, 0, 1);
            createSpellPowerProc(stats.SpellPowerFor10SecOnCast_15_45, 10, 45, .15f, 0, 0, 0, 0, 1);
            createSpellPowerProc(stats.SpellPowerFor10SecOnCrit_20_45, 10, 45, 0, 0, .2f, 0, 0, 1);
            createSpellPowerProc(stats.SpellPowerFor15SecOnCrit_20_45, 15, 45, 0, 0, .2f, 0, 0, 1);
            createSpellPowerUse(stats.SpellPowerFor15SecOnUse90Sec, 15, 90, false);
            createSpellPowerUse(stats.SpellPowerFor15SecOnUse2Min, 15, 120, false);
            createSpellPowerUse(stats.SpellPowerFor20SecOnUse2Min, 20, 120, false);
            createSpellPowerUse(stats.SpellPowerFor20SecOnUse5Min, 20, 300, false);

            createSpellHasteProc(stats.SpellHasteFor10SecOnCast_10_45, 10, 45, .1f, 0, 0, 0, 0, 1);
            createSpellHasteProc(stats.SpellHasteFor6SecOnCast_15_45, 6, 45, .15f, 0, 0, 0, 0, 1);
            createSpellHasteProc(stats.SpellHasteFor6SecOnHit_10_45, 6, 45, 0, .1f, 0, 0, 0, 1);
            createSpellHasteUse(stats.HasteRatingFor20SecOnUse2Min, 20, 120, false);

            if (stats.Mp5OnCastFor20SecOnUse2Min > 0) 
            {
                // Pendant of the Violet Eye... old trinket.
                float Speed = (1f + stats.SpellHaste) * (1f + stats.HasteRating * 0.000304971132f);
                float gcd = (float)Math.Round(1.5f / Speed, 4);
                if (gcd < 1f) gcd = 1f;
                float maxStack = 20 / gcd; // add 50% for the amount of mp5 from the "building the stack" time
                createMP5Use(stats.Mp5OnCastFor20SecOnUse2Min * maxStack * 1.5f, 20, 120, false);
            }
            createManaProc(stats.ManaRestoreOnCrit_25_45, 0, 45, 0, 0, .25f, 0, 0, 1);
            createMP5Proc(5f / 15f * stats.ManaRestoreOnCast_10_45, 15, 45, .1f, 0, 0, 0, 0, 1);
            createMP5Use(5f / 12f * stats.ManaregenOver12SecOnUse3Min, 12, 180, false);
            createMP5Use(5f / 12f * stats.ManaregenOver12SecOnUse5Min, 12, 300, false);
            createMP5Use(5f / 12f * stats.ManaRestore5min, 12, 300, false);
            if (stats.FullManaRegenFor15SecOnSpellcast > 0)
                createSPCProc(1f, 15f, 0, stats.FullManaRegenFor15SecOnSpellcast / 100f, 0, 0, 0, 0, 1);
            createSpiritUse(stats.SpiritFor20SecOnUse2Min, 20, 120, false);

            createDamageProc(stats.DarkmoonCardDeathProc, 0, 45f, .35f, 0, 0, 0, 0, 1);
            if (stats.PendulumOfTelluricCurrentsProc > 0)
            {
                createDamageProc(1460, 0, 45, 0, .15f, 0, 0, 0, 1);
            }
            if (stats.ThunderCapacitorProc > 0)
            {
                _effects.Add(new CapacitorProc(1276, 2.5f, 0, 0, 1f, 0, 0, 4));
            }
            if (stats.LightningCapacitorProc > 0)
            {
                _effects.Add(new CapacitorProc(750, 2.5f, 0, 0, 1f, 0, 0, 3));
            }
            if (stats.ExtractOfNecromanticPowerProc > 0)
            {
                _effects.Add(new CapacitorProc(550, 0f, 0, 0, 0, 0, .1f, 1));
            }
        }

        public SpecialEffects Clone(SpecialEffect effect)
        {
            SpecialEffects clone = new SpecialEffects();
            clone._effects = new List<SpecialEffect>();
            foreach (SpecialEffect e in _effects)
            {
                if (e == effect) clone._effects.Add(effect.Clone());
                else clone._effects.Add(e);
            }
            return clone;
        }

        private SpecialEffects Clone()
        {
            SpecialEffects clone = new SpecialEffects();
            clone._effects = new List<SpecialEffect>();
            foreach (SpecialEffect e in _effects)
            {
                clone._effects.Add(e);
            }
            return clone;
        }

        public Stats estimateAll(float TotalTime, float castsPS, float hitsPS, float critsPS, float missesPS, float ticksPS, out float Damage)
        {
            float dmg = 0;
            Stats result = new Stats();
            foreach (SpecialEffect e in _effects)
            {
                float d;
                result += e.estimate(TotalTime, castsPS, hitsPS, critsPS, missesPS, ticksPS, out d);
                dmg += d;
            }
            Damage = dmg;
            return result;
        }

        public static SpecialEffects operator +(SpecialEffects a, SpecialEffects b)
        {
            SpecialEffects c = a.Clone();
            c._effects.AddRange(b._effects.FindAll(e => !a._effects.Contains(e)));
            return c;
        }
    }

}