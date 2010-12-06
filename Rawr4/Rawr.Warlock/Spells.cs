using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Rawr.Warlock
{
    public class Spell
    {
        // index is level - 80
        protected static float[] WARLOCKSPELLBASEVALUES = { 
            861.855224609375000f, 881.517456054687500f, 901.397033691406250f, 
            921.494018554687500f, 941.806640625000000f, 962.335632324218750f
        };

        public enum SpellTree { Affliction, Demonology, Destruction }
        public static List<string> ALL_SPELLS = new List<String>();

        static Spell()
        {
            Type spellType = Type.GetType("Rawr.Warlock.Spell");
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsSubclassOf(spellType))
                {
                    string name = type.Name;
                    for (int i = 1; i < name.Length; ++i)
                    {
                        if (char.IsUpper(name[i]))
                        {
                            name = name.Insert(i, " ");
                            ++i;
                        }
                        else if (name[i] == '_')
                        {
                            name = name.Replace("_", " (");
                            name = name.Insert(name.Length, ")");
                            i += 2;
                        }
                    }
                    ALL_SPELLS.Add(name);
                }
            }
        }

        public static float CalcUprate(float procRate, float duration, float triggerPeriod)
        {
            return 1f - (float)Math.Pow(1f - procRate, duration / triggerPeriod);
        }
        public static float GetTimeUsed(float baseCastTime, float gcdBonus, List<WeightedStat> haste, float lag)
        {
            float minGCD = 1f - gcdBonus;
            float unhasted = Math.Max(baseCastTime, 1.5f - gcdBonus);

            float avgHasted = 0f;
            foreach (WeightedStat h in haste)
            {
                avgHasted += h.Chance * Math.Max(minGCD, unhasted / h.Value);
            }
            return avgHasted + lag;
        }

        #region properties

        // set via constructor
        public CharacterCalculationsWarlock Mommy { get; protected set; }
        public MagicSchool MagicSchool { get; protected set; }
        public SpellTree MySpellTree { get; protected set; }
        public float ManaCost { get; protected set; }
        public float BaseCastTime { get; protected set; }
        public float GCDBonus { get; protected set; }
        public bool CanMiss { get; protected set; }
        public float BaseDamage { get; protected set; }
        public float DirectCoefficient { get; protected set; }
        public float BaseTickDamage { get; protected set; }
        public float TickCoefficient { get; protected set; }
        public bool CanTickCrit { get; protected set; }

        // set via constructor but only used for internal optimizations
        public bool RecordMissesSeparately = false;

        // set via constructor but sometimes modified via RecordCollisionDelay()
        public float Cooldown { get; protected set; }
        public float RecastPeriod { get; protected set; }
        public float NumTicks { get; protected set; }
        public SpellModifiers SpellModifiers { get; protected set; }

        // set via RecordCollisionDelay()
        public Dictionary<string, SimulatedStat> SimulatedStats
        {
            get;
            protected set;
        }

        // set via SetDamageStats()
        public float AvgDirectHit { get; protected set; }
        public float AvgDirectCrit { get; protected set; }
        public float AvgTickHit { get; protected set; }
        public float AvgTickCrit { get; protected set; }
        public float AvgDamagePerCast { get; protected set; }

        // cached values (can be determined from the values in other props)
        protected float NumCasts;
        protected float AvgDelay;

        #endregion

        #region ctors
        //Non-Damage Constructor
        public Spell(
            CharacterCalculationsWarlock mommy,
            MagicSchool magicSchool,
            SpellTree spellTree,
            float percentBaseMana,
            float baseCastTime,
            float cooldown,
            float recastPeriod,
            bool canMiss)
            : this(
                mommy,
                magicSchool,
                spellTree,
                percentBaseMana,
                baseCastTime,
                cooldown,
                recastPeriod,
                canMiss,
                0f, // avgDirectDamage,
                0f, // directCoefficient,
                0f, // addedDirectMultiplier,
                0f, // baseTickDamage,
                0f, // numTicks,
                0f, // tickCoefficient,
                0f, // addedTickMultiplier,
                false, // canTickCrit,
                0f, // bonusCritChance,
                0f) { } // bonusCritMultiplier,

        //Direct Damage Constructor
        public Spell(
            CharacterCalculationsWarlock mommy,
            MagicSchool magicSchool,
            SpellTree spellTree,
            float percentBaseMana,
            float baseCastTime,
            float cooldown,
            float recastPeriod,
            float avgDirectDamage,
            float directCoefficient,
            float addedDirectMultiplier,
            float bonusCritChance,
            float bonusCritMultiplier)
            : this(
                mommy,
                magicSchool,
                spellTree,
                percentBaseMana,
                baseCastTime,
                cooldown,
                recastPeriod,
                true,
                avgDirectDamage,
                directCoefficient,
                addedDirectMultiplier,
                0f, // baseTickDamage,
                0f, // numTicks,
                0f, // tickCoefficient,
                0f, // addedTickMultiplier,
                false, // canTickCrit,
                bonusCritChance,
                bonusCritMultiplier) { }

        //DoT Constructor
        public Spell(
            CharacterCalculationsWarlock mommy,
            MagicSchool magicSchool,
            SpellTree spellTree,
            float percentBaseMana,
            float baseCastTime,
            float cooldown,
            float recastPeriod,
            float baseTickDamage,
            float numTicks,
            float tickCoefficient,
            float addedTickMultiplier,
            bool canTickCrit,
            float bonusCritChance,
            float bonusCritMultiplier)
            : this(
                mommy,
                magicSchool,
                spellTree,
                percentBaseMana,
                baseCastTime,
                cooldown,
                recastPeriod,
                true,
                0f, // direct avg damage
                0f, // direct coefficient
                0f, // addedDirectMultiplier
                baseTickDamage,
                numTicks,
                tickCoefficient,
                addedTickMultiplier,
                canTickCrit,
                bonusCritChance,
                bonusCritMultiplier) { }

        // Kitchen Sink Constructor
        public Spell(
            CharacterCalculationsWarlock mommy,
            MagicSchool magicSchool,
            SpellTree spellTree,
            float percentBaseMana,
            float baseCastTime,
            float cooldown,
            float recastPeriod,
            bool canMiss,
            float avgDirectDamage,
            float directCoefficient,
            float addedDirectMultiplier,
            float baseTickDamage,
            float numTicks,
            float tickCoefficient,
            float addedTickMultiplier,
            bool canTickCrit,
            float bonusCritChance,
            float bonusCritMultiplier)
        {
            Mommy = mommy;
            MagicSchool = magicSchool;
            MySpellTree = spellTree;
            ManaCost = mommy.BaseMana * percentBaseMana;
            BaseCastTime = baseCastTime;
            BaseDamage = avgDirectDamage;
            Cooldown = cooldown;
            RecastPeriod = recastPeriod;
            CanMiss = canMiss;
            DirectCoefficient = directCoefficient;
            BaseTickDamage = baseTickDamage;
            NumTicks = numTicks;
            TickCoefficient = tickCoefficient;
            CanTickCrit = canTickCrit;
            SimulatedStats = new Dictionary<string, SimulatedStat>();
            WarlockTalents talents = mommy.Talents;
            SpellModifiers = new SpellModifiers();
            SpellModifiers.AddMultiplicativeDirectMultiplier(addedDirectMultiplier);
            SpellModifiers.AddMultiplicativeDirectMultiplier(addedTickMultiplier);
            SpellModifiers.AddCritChance(bonusCritChance);
            SpellModifiers.AddCritBonusMultiplier(bonusCritMultiplier);
        }
        #endregion

        protected Rotation GetRotation()
        {
            return Mommy.Options.GetActiveRotation();
        }
        protected void ApplySoulLeech()
        {
            float reductionOnProc = Mommy.CalcMana() * Mommy.Talents.SoulLeech * .05f;
            ManaCost -= .3f * reductionOnProc;
        }
        public virtual bool IsCastable()
        {
            return true;
        }
        public string GetToolTip()
        {
            float numCasts = GetNumCasts();
            float castTime = GetAvgTimeUsed() - Mommy.Options.Latency;

            string toolTip;
            if (AvgDamagePerCast > 0)
            {
                float dps = numCasts * AvgDamagePerCast / Mommy.Options.Duration;
                toolTip = string.Format("{0:0.0} dps ({1:0.0%})*", dps, dps / Mommy.PersonalDps);
            }
            else
            {
                toolTip = string.Format("{0:0.0} casts*", numCasts);
            }
            if (AvgDirectHit > 0)
            {
                toolTip += string.Format("{0:0.0}\tAverage Hit\r\n" + "{1:0.0}\tAverage Crit\r\n", 
                                           AvgDirectHit,                AvgDirectCrit);
            }
            if (AvgTickHit > 0)
            {
                toolTip += string.Format("{0:0.0}\tAverage Tick\r\n" + "{1:0.0}\tAverage Tick Crit\r\n" + "{2:0.0}\tTicks Per Cast\r\n",
                                           AvgTickHit,                   AvgTickCrit,                       NumTicks);
            }
            if (AvgDamagePerCast > 0)
            {
                toolTip += "\r\n";
            }
            if (AvgDirectHit > 0)
            {
                toolTip += string.Format("{0:0.0}%\tDirect Coefficient\r\n", 100 * DirectCoefficient);
            }
            if (AvgTickHit > 0)
            {
                toolTip += string.Format("{0:0.0}%\tPer Tick Coefficient\r\n", 100 * TickCoefficient);
            }
            if (AvgDirectHit > 0)
            {
                toolTip += string.Format("{0:0.0}%\tDirect Multiplier\r\n", 100 * SpellModifiers.GetFinalDirectMultiplier());
            }
            if (AvgTickHit > 0)
            {
                toolTip += string.Format("{0:0.0}%\tPer Tick Multiplier\r\n", 100 * SpellModifiers.GetFinalTickMultiplier());
            }
            if (AvgDirectCrit > AvgDirectHit || AvgTickCrit > AvgTickHit)
            {
                toolTip += string.Format("{0:0.0}%\tCrit Chance\r\n" +    "{1:0}%\tCrit Multiplier\r\n",
                                           100 * SpellModifiers.CritChance, 100 * SpellModifiers.GetFinalCritMultiplier());
            }
            if (AvgDamagePerCast > 0)
            {
                toolTip += string.Format("{0:0.0}%\tResisted\r\n\r\n", 100 * GetResist());
            }
            toolTip += string.Format("{0:0.00}s\tCast Time\r\n" + "{1:0.0}\tMana\r\n",
                                       castTime,                    ManaCost);
            if (Cooldown > 0)
            {
                toolTip += string.Format("{0:0.0}s\tCooldown\r\n", Cooldown);
            }
            toolTip += string.Format("{0:0.0}s\tBetween Casts (Average)\r\n", GetAvgTimeBetweenCasts());
            if (AvgDamagePerCast > 0)
            {
                toolTip += string.Format("\r\n" + "{0:0.0}\tDPC\r\n" + "{1:0.0}\tDPCT\r\n" +        "{2:0.0}\tDPM\r\n" +         "{3:0.0}\tCasts",
                                                    AvgDamagePerCast,    AvgDamagePerCast / castTime, AvgDamagePerCast / ManaCost, numCasts);
            }
            return toolTip;
        }

        //setting NumCasts
        public void Spam(float timeRemaining)
        {
            Debug.Assert(Cooldown == 0 && RecastPeriod == 0);
            NumCasts += timeRemaining / GetAvgTimeUsed();
        }
        public virtual float GetNumCasts()
        {
            if (NumCasts == 0)
            {
                float duration = Mommy.Options.Duration;
                if (!IsCastDuringExecute())
                {
                    duration *= 1 - Mommy.GetExecutePercentage();
                }
                float delay = SimulatedStats["delay"].GetValue();
                NumCasts = duration / (GetAvgRequeueTime() + delay);
            }
            return NumCasts;
        }
        public virtual bool IsCastDuringExecute()
        {
            return true;
        }
        private bool IsBinary()
        {
            return false;
        }
        public virtual float GetQueueProbability(CastingState state)
        {
            if (state.Cooldowns.ContainsKey(this))
            {
                if (state.Cooldowns[this] <= 0)
                {
                    return 1f;
                }
                else
                {
                    return 0f;
                }
            }
            else
            {
                float maxQueued = state.GetMaxTimeQueued(this);
                float unqueuable = state.Elapsed - maxQueued;
                return Math.Min(maxQueued / (GetAvgRequeueTime() - unqueuable), 1f);
            }
        }
        public float GetAvgRequeueTime()
        {
            // TODO this should really not be averaged (for the cases this
            // method is used).  Instead it should return all possible cooldowns
            // and their probabilities (and calling methods should be modified
            // accordingly).

            float period = Math.Max(RecastPeriod, Cooldown);
            float hitChance = Mommy.HitChance;
            if (IsBinary())
            {
                hitChance -= GetResist();
            }
            if (CanMiss && hitChance < 1 && Cooldown < period)
            {
                // If a spell misses, and it can be recast sooner than it
                // normally would otherwise, it will instead wait for either its
                // cooldown (if it has one), or for one spell to be cast
                // inbetween (to allow for travel time + reaction time).
                float missDelay = Math.Max(Cooldown + GetCastTime(null), Mommy.AvgTimeUsed);
                period = Utilities.GetWeightedSum(period, hitChance, missDelay, 1 - hitChance);
            }
            return period;
        }
        public virtual List<CastingState> SimulateCast(CastingState stateBeforeCast, float chanceOfCast)
        {
            // record stats about this spellcast
            float p = chanceOfCast * stateBeforeCast.Probability;
            float timeUsed = GetTimeUsed(stateBeforeCast);
            RecordSimulatedStat("delay", stateBeforeCast.GetMaxTimeQueued(this) / 2f, p);
            RecordSimulatedStat("time used", timeUsed, p);

            // construct the casting state(s) that can result from this cast
            List<CastingState> results = new List<CastingState>();
            float hitChance = Mommy.HitChance;
            if (IsBinary())
            {
                hitChance -= GetResist();
            }
            float newCooldown = Cooldown - timeUsed + GetCastTime(stateBeforeCast);
            if (CanMiss
                && hitChance < 1f
                && (RecordMissesSeparately || Cooldown < RecastPeriod))
            {
                // state when spell hits
                PopulateNextState(results, stateBeforeCast, timeUsed, Math.Max(RecastPeriod - timeUsed, newCooldown), hitChance * chanceOfCast, true);

                // state when spell misses
                if (newCooldown <= 0)
                {
                    // ensure at least 1 spell is cast before this one is
                    // requeued, to allow for travel time + reaction time for
                    // the player to detect the miss
                    newCooldown = .0001f;
                }
                PopulateNextState(results, stateBeforeCast, timeUsed, newCooldown, (1 - hitChance) * chanceOfCast, false);
            }
            else
            {
                PopulateNextState(results, stateBeforeCast, timeUsed, Math.Max(newCooldown, RecastPeriod - timeUsed), chanceOfCast, true);
            }

            return results;
        }
        public void RecordSimulatedStat(string statName, float value, float weight)
        {
            if (!SimulatedStats.ContainsKey(statName))
            {
                SimulatedStats[statName] = new SimulatedStat();
            }
            SimulatedStats[statName].AddSample(value, weight);
        }
        private void PopulateNextState(List<CastingState> results, CastingState stateBeforeCast, float timeUsed, float cooldownAfterAdvance, float p, bool isHit)
        {
            CastingState nextState = new CastingState(stateBeforeCast);
            nextState.Probability *= p;
            nextState.AddSpell(this, timeUsed, isHit);
            nextState.Cooldowns[this] = cooldownAfterAdvance;

            if (MySpellTree == SpellTree.Destruction && nextState.BackdraftCharges > 0)
            {
                --nextState.BackdraftCharges;
            }

            results.Add(nextState);
        }
        public float GetTimeUsed(CastingState state)
        {
            return MaybeApplyBackdraft(GetTimeUsed(BaseCastTime, GCDBonus, Mommy.Haste, Mommy.Options.Latency), state);
        }
        public float GetCastTime(CastingState state)
        {
            if (BaseCastTime == 0f)
            {
                return 0f;
            }

            float avg = 0f;
            foreach (WeightedStat h in Mommy.Haste)
            {
                avg += h.Chance * BaseCastTime / h.Value;
            }
            return MaybeApplyBackdraft(avg, state);
        }
        private float MaybeApplyBackdraft(float time, CastingState state)
        {
            if (state != null
                && state.BackdraftCharges > 0
                && MySpellTree == SpellTree.Destruction)
            {
                time /= 1f + Mommy.Talents.Backdraft * .1f;
            }
            return time;
        }
        public float GetAvgTimeUsed()
        {
            if (SimulatedStats.ContainsKey("time used"))
            {
                return SimulatedStats["time used"].GetValue();
            }
            else
            {
                return GetTimeUsed(BaseCastTime, GCDBonus, Mommy.Haste, Mommy.Options.Latency);
            }
        }
        public float GetTotalWeight()
        {
            return SimulatedStats["delay"].GetTotalWeight();
        }
        public float GetAvgTimeBetweenCasts()
        {
            return Mommy.Options.Duration / GetNumCasts();
        }
        public float GetUprate(CastingState state, Spell spell)
        {
            // Assumes the effect == requeue time.
            // If this method is going to be called, be sure to enable RecordMissesSeparately.
            Debug.Assert(RecordMissesSeparately);

            float castTime = spell.GetCastTime(state);
            if (state.Cooldowns.ContainsKey(this))
            {
                float cooldown = state.Cooldowns[this] - castTime;
                if (cooldown <= 0 || !state.LastCastHit(this))
                {
                    return 0f;
                }
                else
                {
                    return 1f;
                }
            }

            float maxQueued = castTime;
            if (Mommy.IsPriorityOrdered(spell, this))
            {
                maxQueued += state.GetMaxTimeQueued(this);
            }
            float unqueuable = (state.Elapsed + castTime) - maxQueued;
            float chanceQueued = Math.Min(maxQueued / (GetAvgRequeueTime() - unqueuable), 1f);
            return 1 - chanceQueued;
        }
        public virtual void AdjustAfterCastingIsSet()
        {
            // here for subclasses to override if desired.
        }

        //setting avg damage
        public virtual void FinalizeSpellModifiers()
        {
            SpellModifiers.Accumulate(Mommy.SpellModifiers);
            switch (MagicSchool)
            {
                case MagicSchool.Shadow: Mommy.AddShadowModifiers(SpellModifiers); break;
                case MagicSchool.Fire:   Mommy.AddFireModifiers(SpellModifiers);   break;
            }
        }
        public virtual void SetDamageStats(float baseSpellPower)
        {
            AvgDirectHit = (BaseDamage + DirectCoefficient * baseSpellPower) * SpellModifiers.GetFinalDirectMultiplier() * (1 - GetResist());

            if (this is BaneOfDoom)
            {
                // Bane of Doom does not get periodic spell damage bonuses, despite being a DOT effect
                AvgTickHit = (BaseTickDamage + TickCoefficient * baseSpellPower) * SpellModifiers.GetFinalDirectMultiplier() * (1 - GetResist());
            }
            else
            {
                AvgTickHit = (BaseTickDamage + TickCoefficient * baseSpellPower) * SpellModifiers.GetFinalTickMultiplier() * (1 - GetResist());
            }

            float critChance = SpellModifiers.CritChance;
            float critMultiplier = SpellModifiers.GetFinalCritMultiplier();

            AvgDirectCrit = AvgDirectHit * critMultiplier;
            float avgDirectDamage = Utilities.GetWeightedSum(AvgDirectCrit, critChance, AvgDirectHit, 1 - critChance);

            float avgTickDamage;
            if (CanTickCrit)
            {
                AvgTickCrit = AvgTickHit * critMultiplier;
                avgTickDamage = Utilities.GetWeightedSum(AvgTickHit, 1 - critChance, AvgTickCrit, critChance);
            }
            else
            {
                AvgTickCrit = AvgTickHit;
                avgTickDamage = AvgTickHit;
            }

            AvgDamagePerCast = Mommy.HitChance * (avgDirectDamage + NumTicks * avgTickDamage);
        }
        protected float GetResist()
        {
            return StatConversion.GetAverageResistance(Mommy.Options.PlayerLevel, Mommy.Options.TargetLevel, 0f, 0f);
        }
    }

    // spell 50796, spellEffect 43048
    public class ChaosBolt : Spell
    {
        static float[] talentValues = { .1f, .3f, .5f };
        private const float SCALE = 1.5470000505f;
        private const float COEFF = 0.6280000210f;

        // TODO: is this order right? or is the Bane reduction applied after the 10% discount?
        public ChaosBolt(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Fire,
                SpellTree.Destruction,
                .07f, // percentBaseMana,
                (2.5f - talentValues[mommy.Talents.Bane]) * (mommy.Stats.Warlock2T11 > 0f ? .9f : 1f), // baseCastTime,
                mommy.Talents.GlyphChaosBolt ? 10f : 12f, // cooldown,
                0f, // recastPeriod,
                WARLOCKSPELLBASEVALUES[mommy.Options.PlayerLevel - 80] * SCALE, // avgDirectDamage
                COEFF, // directCoefficient
                0f, // addedDirectMultiplier,
                0f, // bonusCritChance,
                1f) // bonus crit multiplier
        { 
            ApplySoulLeech();
        }

        public override bool IsCastable()
        {
            return Mommy.Talents.ChaosBolt > 0;
        }
        public override List<CastingState> SimulateCast(CastingState stateBeforeCast, float chanceOfCast)
        {
            if (Mommy.Talents.FireAndBrimstone > 0)
            {
                ((Immolate)Mommy.GetSpell("Immolate")).RecordUpChance(this, stateBeforeCast);
            }
            return base.SimulateCast(stateBeforeCast, chanceOfCast);
        }
        public override void FinalizeSpellModifiers()
        {
            base.FinalizeSpellModifiers();
            if (Mommy.Talents.FireAndBrimstone > 0)
            {
                float uprate = SimulatedStats["immolate up-chance"].GetValue();
                float fullBonus = Mommy.Talents.FireAndBrimstone * .02f;
                SpellModifiers.AddMultiplicativeMultiplier(uprate * fullBonus);
            }
        }
    }

    // spell 17962, spellEffect 9553, 9554 (placeholders)
    public class Conflagrate : Spell
    {
        public const float COOLDOWN = 10f;
        public static bool WillBeCast(CharacterCalculationsWarlock mommy)
        {
            return mommy.Options.GetActiveRotation().Contains("Conflagrate") && mommy.Destruction;
        }

        public Conflagrate(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Fire,
                SpellTree.Destruction,
                .16f, // percentBaseMana,
                0f, // baseCastTime,
                COOLDOWN - (mommy.Talents.GlyphConflag ? 2f : 0f), // cooldown,
                0f, // recastPeriod,
                0f, // avgDirectDamage, will be set in SetDamageStats
                0f, // directCoefficient, already accounted for by Immolate AvgTickDamage
                0f, // addedDirectMultiplier, already accounted for by Immolate AvgTickDamage
                mommy.Talents.FireAndBrimstone * .05f, // bonusCritChance,
                1f) // bonusCritMultiplier)
        { 
            RecordMissesSeparately = true;
        }

        public override bool IsCastable()
        {
            return Mommy.Destruction;
        }
        public override void SetDamageStats(float baseSpellPower)
        {
            // this is guaranteed to be called after Immolate has been solved
            // 60% factor added in build 13156
            BaseDamage = Mommy.CastSpells["Immolate"].AvgTickHit * Mommy.CastSpells["Immolate"].NumTicks * .6f;
            base.SetDamageStats(baseSpellPower);
        }
        public override List<CastingState> SimulateCast(CastingState stateBeforeCast, float chanceOfCast)
        {
            List<CastingState> states = base.SimulateCast(stateBeforeCast, chanceOfCast);

            CastingState stateOnHit = states[0];
            if (Mommy.Talents.Backdraft > 0)
            {
                stateOnHit.BackdraftCharges = 3;
            }

            return states;
        }
    }

    // spell 172, spellEffect 97, 98
    public class Corruption : Spell
    {
        private const float SCALE = 0.1529999971f;
        private const float COEFF = 0.1759999990f;

        public static float GetTickPeriod(CharacterCalculationsWarlock mommy)
        {
            float period = 3.1f; // total guess
            period /= mommy.AvgHaste;
            return period;
        }

        public Corruption(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Shadow, // magic school
                SpellTree.Affliction, // spell tree
                .14f, // percent base mana
                0f, // cast time
                0f, // cooldown
                18f, // recast period
                WARLOCKSPELLBASEVALUES[mommy.Options.PlayerLevel - 80] * SCALE, // base damage per tick
                (int)((6f / mommy.AvgHaste) + 0.5f), // hasted num ticks
                COEFF, // tick coefficient
                mommy.Talents.ImprovedCorruption * .04f, // addedTickMultiplier
                true, // canTickCrit
                (mommy.Talents.EverlastingAffliction * .05f + mommy.Stats.Warlock2T10), // bonus crit chance
                1f) // bonus crit multiplier
        { 
            WarlockTalents talents = Mommy.Talents;

            // Malficus solved the average duration of a rolling Corruption, see
            // http://rawr.codeplex.com/Thread/View.aspx?ThreadId=203628
            //
            // D = the non-rolling duration
            // T = the time between reset triggers
            // P = the chance a trigger will actually reset the duration
            // TC = the time between corruption ticks
            float d = RecastPeriod;
            float t = GuessRollingTriggerFrequency();
            float[] points = { 0f, .33f, .66f, 1f };
            float p = Mommy.HitChance * points[talents.EverlastingAffliction];
            float tc = d / NumTicks;

            if (p == 1 && t <= d)
            {
                RecastPeriod = Mommy.Options.Duration;
            }
            else if (p > 0 && t > 0)
            {
                float fightLen = Mommy.Options.Duration;
                int maxTriggers = (int)(fightLen * t);
                int maxTicks = (int)(fightLen / tc);

                RecastPeriod = tc * maleficusDuration(new float[maxTicks + 1, maxTriggers + 1], tc, p, 1 / t, (int)(fightLen / tc), 6, 0);
            }
            NumTicks = RecastPeriod / tc;
        }

        public override void FinalizeSpellModifiers()
        {
            base.FinalizeSpellModifiers();
            SpellModifiers.AddMultiplicativeMultiplier(Mommy.Stats.Warlock4T9);
        }
        private static float maleficusDuration(float[,] cache, float TC, float P, float T, int maxTicks, int accumTicks, int triggerIndex)
        {
            if (accumTicks >= maxTicks)
            {
                return maxTicks;
            }
            double now = triggerIndex * T;
            double curLen = accumTicks * TC;
            if (now > curLen)
            {
                return accumTicks;
            }

            if (cache[accumTicks, triggerIndex] > 0)
            {
                return cache[accumTicks, triggerIndex];
            }

            float procDuration = maleficusDuration(cache, TC, P, T, maxTicks, (int)(now / TC) + 7, triggerIndex + 1);
            float nonProcDuration = maleficusDuration(cache, TC, P, T, maxTicks, accumTicks, triggerIndex + 1);
            float res = procDuration * P + nonProcDuration * (1 - P);

            cache[accumTicks, triggerIndex] = res;
            return res;
        }
        private float GuessRollingTriggerFrequency()
        {
            float freq = 0f;
            if (GetRotation().Contains("Shadow Bolt"))
            {
                // assume about 1/2 the time will be spent spamming shadow bolt
                freq += .5f / GetTimeUsed(ShadowBolt.GetBaseCastTime(Mommy), 0f, Mommy.Haste, Mommy.Options.Latency);
            }
            if (GetRotation().Contains("Haunt") && Mommy.Talents.Haunt > 0)
            {
                // assume 11 seconds between haunt casts, on average
                freq += 1f / 11f;
            }
            if (GetRotation().Execute == "Drain Soul (Execute)")
            {
                // assume drain soul spam, since it refreshes Corruption and UA
                // maybe account for need to redo BoA
                freq += Mommy.GetExecutePercentage() * Mommy.Options.Duration / GetTimeUsed(15f, 0f, Mommy.Haste, Mommy.Options.Latency);
            }
            return freq;
        }
    }

    //spellid 980, effectid 374
    public class BaneOfAgony : Spell
    {
        private const float SCALE = 0.1330000013f;
        private const float COEFF = 0.0879999995f;

        public BaneOfAgony(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Shadow, // magic school
                SpellTree.Affliction, // spell tree
                .1f, // percent base mana
                0f, // cast time
                0f, // cooldown
                mommy.Talents.GlyphCoA ? 28f : 24f, // recast period
                0f, // damage per tick - set below
                0f, // num ticks - set below
                COEFF, // tick coefficient
                0f, // addedTickMultiplier
                true, //canTickCrit
                mommy.Talents.DoomAndGloom * .04f, // bonus crit chance
                1f) // bonus crit multiplier
        { 
            GCDBonus = mommy.Talents.Pandemic * .25f;

            // BoA consists of 4 weak ticks, 4 medium ticks, 4 strong ticks, and if glyphed, 2 very strong ticks.
            // Additional ticks from haste are weak ticks. TODO: this is wrong.
            float baseDamage = WARLOCKSPELLBASEVALUES[mommy.Options.PlayerLevel - 80] * SCALE * 12f;
            float baseNumTicks = mommy.Talents.GlyphCoA ? 14f : 12f;
            NumTicks = (int)((baseNumTicks / mommy.AvgHaste) + 0.5f);
            BaseTickDamage = baseDamage * (1f + (NumTicks - baseNumTicks) * .042f + (mommy.Talents.GlyphCoA ? .3325f : 0f)) / NumTicks;
        }
    }

    //spellid 603, effectid 246, 86707
    public class BaneOfDoom : Spell
    {
        private const float SCALE = 2.0239999294f;
        private const float COEFF = 0.8799999952f;

        public BaneOfDoom(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Shadow, // magic school
                SpellTree.Affliction, // spell tree
                .15f, // percent base mana
                0f, // baseCastTime,
                0f, // cooldown,
                60f, // recastPeriod,
                WARLOCKSPELLBASEVALUES[mommy.Options.PlayerLevel - 80] * SCALE, // baseTickDamage,
                4f, // numTicks,
                COEFF, // tickCoefficient,
                0f, // addedTickMultiplier,
                true, //    canTickCrit,
                mommy.Talents.DoomAndGloom * .04f, // bonus crit chance
                1f) // bonusCritMultiplier
        { 
            GCDBonus = mommy.Talents.Pandemic * .25f;

            //UNMODELED: 20% chance to summon Ebon Imp with each tick (+ Impending Doom * 10%)
            //Ebon Imp does avg X damage over Y seconds; Z cooldown
        }
    }

    //spellid 1490, effectID 470, 471, 98702
    public class CurseOfTheElements : Spell
    {
        public CurseOfTheElements(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                0, // magic school
                SpellTree.Affliction, // spell tree
                .1f, // percent base mana
                0f, // cast time
                0f, // cooldown
                300f, // recast period
                true) // can miss
        { 
            GCDBonus = mommy.Talents.Pandemic * .25f;
        }

        public override bool IsCastable()
        {
            return Mommy.Stats.BonusShadowDamageMultiplier == 0;
        }
    }

    public class DemonSoul : Spell
    {
        public DemonSoul(CharacterCalculationsWarlock mommy) 
            : base(
                mommy,
                0,
                SpellTree.Demonology,
                .15f,
                0f,
                120f,
                0f,
                false) 
        { 
            /*
            Imp - Critical Strike damage on cast time Destruction spells increased by 60% for 30 sec.  
            Each spell cast benefitting from this effect reduces the bonus by 20% until the bonus expires after 3 casts.

            Succubus - Shadow Bolt damage increased by 10% for 20 sec.

            Felhunter - Periodic shadow damage increased by 20% for 20 sec.

            Felguard - Spell haste increased by 15% and fire and shadow damage done increased by 10% for 20 sec.
             */
        }

        public override bool IsCastable()
        {
            return Mommy.Options.PlayerLevel >= 85;
        }
    }

    //spell ID 1120, effectID 397, 399
    public class DrainSoul : Spell
    {
        private const float SCALE = 0.0799999982f;
        private const float COEFF = 0.3779999912f;

        public DrainSoul(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Shadow,
                SpellTree.Affliction,
                .14f, //percent base mana
                15f, //base cast time
                0f, //cooldown
                0f, //recast period
                WARLOCKSPELLBASEVALUES[mommy.Options.PlayerLevel - 80] * SCALE * 4f, //baseTickDamage during execute
                (int)((3f / mommy.AvgHaste) + 0.5f), // hasted num ticks
                COEFF, //tickCoefficient
                0f, //addedTickMultiplier
                true, //canTickCrit
                0f, //bonusCritChance
                1f) //bonusCritMultiplier
            { }
    }

    //spellID 77799 effectID 68189
    public class FelFlame : Spell
    {
        private const float SCALE = 0.2479999959f;
        private const float COEFF = 0.3019999862f;

        public FelFlame(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Shadow,
                SpellTree.Destruction,
                .06f, // percentBaseMana,
                0f, // baseCastTime,
                0f, // cooldown,
                0f, // recastPeriod,
                WARLOCKSPELLBASEVALUES[mommy.Options.PlayerLevel - 80] * SCALE, // avgDirectDamage
                COEFF, // directCoefficient
                0f, // addedDirectMultiplier,
                0f, // bonusCritChance,
                1f) // bonus crit multiplier
        { 
            // UNMODELED: extends Immolate or Unstable Affliction duration by 6 sec
        }

        public override bool IsCastable()
        {
            return Mommy.Options.PlayerLevel >= 81;
        }
    }

    public class FelFlameWithFelSpark : FelFlame
    {
        public FelFlameWithFelSpark(CharacterCalculationsWarlock mommy) : base(mommy)
        {
            SpellModifiers.AddCritChance(1f);
        }
        public override bool IsCastable()
        {
            return base.IsCastable() && Mommy.Stats.Warlock4T11 > 0f;
        }
        public override float GetNumCasts()
        {
            // TODO: number of Immolate ticks + number of UnstableAffliction ticks * 0.2f
            return 0f;
        }
    }

    //spellID 71521 effectID 65178
    public class HandOfGuldan : Spell
    {
        private const float SCALE = 1.5930000544f;
        private const float COEFF = 0.9679999948f;

        public HandOfGuldan(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Shadow,
                SpellTree.Demonology,
                .07f, // percentBaseMana,
                2f * (mommy.Stats.Warlock2T11 > 0f ? .9f : 1f), // baseCastTime,
                12f, // cooldown,
                0f, // recastPeriod,
                WARLOCKSPELLBASEVALUES[mommy.Options.PlayerLevel - 80] * SCALE, // avgDirectDamage
                COEFF, // directCoefficient
                0f, // addedDirectMultiplier,
                0f, // bonusCritChance,
                1f) // bonus crit multiplier
        { 
            // UNMODELED: 50% * #Cremation chance to refresh Immolate
            // UNMODELED: 5% * #Impending Doom chance to reduce Metamorphosis cooldown by 6 sec
        }
    }

    //spellID 48181, effectID 40331
    public class Haunt : Spell
    {
        private const float SCALE = 0.7369999886f;
        private const float COEFF = 2f / 3.5f; //client tooltips show no scaling, but that contradicts experiments

        private float AvgBonus;

        public Haunt(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Shadow,
                SpellTree.Affliction,
                .12f, // percent base mana
                1.5f * (mommy.Stats.Warlock2T11 > 0f ? .9f : 1f), // cast time
                8f, // cooldown
                0f, // recast period
                WARLOCKSPELLBASEVALUES[mommy.Options.PlayerLevel - 80] * SCALE, // avg direct damage
                COEFF, // direct coefficient
                0f, // bonus direct multiplier
                0f, // bonus crit chance
                1f) // bonus crit multiplier
        { } 

        public override bool IsCastable()
        {
            return Mommy.Talents.Haunt > 0;
        }
        public float GetAvgTickBonus()
        {
            if (AvgBonus > 0)
            {
                return AvgBonus;
            }

            // Consider the window from cast time until the next (actual, not
            // planned) cast time.  This can be broken into three cases, when
            // it hits, when it misses & the previous cast hit, and when it
            // misses & the previous cast missed. Calculate the average uprate
            // in each case, then combine them with a weighted average to get
            // the overall uprate.

            float uprate;
            float tolerance = 12f - RecastPeriod;
            SimulatedStat delayStats = SimulatedStats["delay"];
            SimulatedStat hitUprate = new SimulatedStat();
            SimulatedStat missFollowingHitUprate = new SimulatedStat();
            for (int i = delayStats.Values.Count; --i >= 0; )
            {
                float collisionDelay = delayStats.Values[i];
                float probability = delayStats.Weights[i];

                // CASE 1: this cast hits.
                // uprate = duration / window
                // duration = 12
                // window = RecastPeriod + collisionDelay
                uprate = Math.Min(1f, 12f / (RecastPeriod + collisionDelay));
                hitUprate.AddSample(uprate, probability);

                // CASE 2: this cast misses, previous cast hit.
                // uprate = leftoverUptime / window
                // leftoverUptime = tolerance - collisionDelay
                // window = Cooldown + collisionDelay
                uprate = Math.Max(tolerance - collisionDelay, 0) / (Cooldown + delayStats.GetValue());
                missFollowingHitUprate.AddSample(uprate, probability);

                // CASE 3: this cast misses, previous cast missed.
                // This case will always yeild zero uptime/uprate.
            }

            // average them all together for the overall uprate
            float hitChance = Mommy.HitChance;
            float missChance = 1 - hitChance;
            uprate = Utilities.GetWeightedSum(hitUprate.GetValue(), hitChance, 
                                              missFollowingHitUprate.GetValue(), missChance * hitChance, 
                                              0f, missChance * missChance);

            AvgBonus = (Mommy.Talents.GlyphHaunt ? .23f : .2f) * uprate;
            return AvgBonus;
        }
    }

    //spellID 348 effectID 145, 146
    public class Immolate : Spell
    {
        private const float DIRECTSCALE = 0.6919999719f;
        private const float DIRECTCOEFF = 0.2199999988f;
        private const float TICKSCALE = 0.4390000105f;
        private const float TICKCOEFF = 0.1759999990f;
        private static float[] talentValues = { 0f, 0.1f, 0.3f, 0.5f };
        public static float GetTickPeriod(CharacterCalculationsWarlock mommy)
        {
            return 3.1f / mommy.AvgHaste;
        }

        public Immolate(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Fire,
                SpellTree.Destruction,
                .17f, // percentBaseMana,
                2f - talentValues[mommy.Talents.Bane], // baseCastTime,
                0f, // cooldown,
                15f, // recastPeriod,
                true, // canMiss,
                WARLOCKSPELLBASEVALUES[mommy.Options.PlayerLevel - 80] * DIRECTSCALE, // avgDirectDamage,
                DIRECTCOEFF, // directCoefficient,
                mommy.Talents.ImprovedImmolate * .1f, // addedDirectMultiplier,
                WARLOCKSPELLBASEVALUES[mommy.Options.PlayerLevel - 80] * TICKSCALE, // baseTickDamage,
                (int)((5f / mommy.AvgHaste) + 0.5f), // numTicks,
                TICKCOEFF, // tickCoefficient,
                (mommy.Talents.GlyphImmolate ? .1f : 0f)
                    + mommy.Talents.ImprovedImmolate * .1f, // addedTickMultiplier,
                true, // canTickCrit,
                0f, // bonusCritChance,
                1f) // bonusCritMultiplier
        { 
            RecordMissesSeparately = true;
        }

        public override void FinalizeSpellModifiers()
        {
            base.FinalizeSpellModifiers();
            SpellModifiers.AddMultiplicativeMultiplier(Mommy.Stats.Warlock4T9);
            if (Mommy.Stats.Warlock2T8 > 0)
            {
                SpellModifiers.AddAdditiveMultiplier(.1f);
            }
        }

        /// <summary>
        /// Records the chance that a given spell is cast while immolate is on
        /// the target.
        /// </summary>
        public void RecordUpChance(Spell spell, CastingState state)
        {
            float chance;
            chance = GetUprate(state, spell);
            spell.RecordSimulatedStat("immolate up-chance", chance, state.Probability);
        }
    }

    //spellID 50590 effectID 42811
    public class ImmolationAura : Spell
    {
        //base damage doesn't scale
        private const float COEFF = 0.1430000067f;

        public ImmolationAura(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Fire,
                SpellTree.Demonology,
                .64f, // percentBaseMana,
                0f, // baseCastTime,
                180f, // cooldown,
                0f, // recastPeriod,
                251f, // baseTickDamage,
                (int)((15f / mommy.AvgHaste) + 0.5f), // numTicks,
                COEFF, // tickCoefficient,
                0f, // addedTickMultiplier,
                true, // canTickCrit,
                0f, // bonusCritChance,
                1f) // bonusCritMultiplier
        { }

        public override bool IsCastable()
        {
            return Mommy.Talents.Metamorphosis > 0;
        }
        public override void FinalizeSpellModifiers()
        {
            base.FinalizeSpellModifiers();

            // immolation aura always gets the full metamorphosis bonus
            SpellModifiers.AddMultiplicativeDirectMultiplier(1.2f / (1 + Mommy.GetMetamorphosisBonus()) - 1);
        }
    }

    //spellID 29722 effectID 19297
    public class Incinerate : Spell
    {
        private const float SCALE = 0.5730000138f;
        private const float COEFF = 0.5389999747f;

        public Incinerate(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Fire,
                SpellTree.Destruction,
                .14f, // percentBaseMana,
                2.5f - mommy.Talents.Emberstorm * .0125f, // baseCastTime,
                0f, // cooldown,
                0f, // recastPeriod,
                WARLOCKSPELLBASEVALUES[mommy.Options.PlayerLevel - 80] * SCALE, // avgDirectDamage,
                COEFF, // directCoefficient,
                (1f + (mommy.Talents.GlyphIncinerate ? .05f : 0f)) * (1f + mommy.Talents.ShadowAndFlame * .04f), // addedDirectMultiplier,
                mommy.Stats.Warlock2T10 + mommy.Stats.Warlock4T8, // bonusCritChance,
                1f) // bonus crit multiplier
        { }

        public override void FinalizeSpellModifiers()
        {
            base.FinalizeSpellModifiers();
            float immoUp = GetImmolateUpRate();
            BaseDamage *= 1f + (.25f * immoUp);
            if (Mommy.Talents.FireAndBrimstone > 0)
            {
                float fullBonus = Mommy.Talents.FireAndBrimstone * .02f;
                SpellModifiers.AddMultiplicativeMultiplier(immoUp * fullBonus);
            }
        }
        protected virtual float GetImmolateUpRate()
        {
            return 1 - GetCastTime(null) / 15f;
        }
    }

    public class Incinerate_UnderBackdraft : Incinerate
    {
        public Incinerate_UnderBackdraft(CharacterCalculationsWarlock mommy) : base(mommy) { }
        public override bool IsCastable()
        {
            return Mommy.Talents.Backdraft > 0 && Conflagrate.WillBeCast(Mommy);
        }
        public override float GetQueueProbability(CastingState state)
        {
            if (state.BackdraftCharges > 0)
            {
                return 1f;
            }
            else
            {
                return 0f;
            }
        }
        public override float GetNumCasts()
        {
            if (NumCasts == 0)
            {
                Spell conflagrate = Mommy.CastSpells["Conflagrate"];
                float castsPerConf = GetTotalWeight() / conflagrate.GetTotalWeight();
                NumCasts = castsPerConf * conflagrate.GetNumCasts();
            }
            return NumCasts;
        }
        public override List<CastingState> SimulateCast(CastingState stateBeforeCast, float chanceOfCast) 
        {
            if (Mommy.Talents.FireAndBrimstone > 0)
            {
                ((Immolate)Mommy.GetSpell("Immolate")).RecordUpChance(this, stateBeforeCast);
            }
            return base.SimulateCast(stateBeforeCast, chanceOfCast);
        }
        protected override float GetImmolateUpRate()
        {
            if (!SimulatedStats.ContainsKey("immolate up-chance"))
            {
                return 0f;
            }
            return SimulatedStats["immolate up-chance"].GetValue();
        }
    }

    public class Incinerate_UnderMoltenCore : Incinerate
    {
        public Incinerate_UnderMoltenCore(CharacterCalculationsWarlock mommy) : base(mommy)
        {
            BaseCastTime /= 1f + Mommy.Talents.MoltenCore * .1f;
            float procChance = Mommy.Talents.MoltenCore * .02f;
            float tickPeriod = Immolate.GetTickPeriod(mommy);
            Cooldown = tickPeriod / procChance;
        }
        public override bool IsCastable()
        {
            return Mommy.Talents.MoltenCore > 0 && GetRotation().Contains("Immolate");
        }
        public override bool IsCastDuringExecute()
        {
            return true;
        }
        public override float GetQueueProbability(CastingState state)
        {
            if (state.MoltenCoreCharges > 0)
            {
                return 1f;
            }
            else
            {
                return base.GetQueueProbability(state);
            }
        }
        public override List<CastingState> SimulateCast(CastingState stateBeforeCast, float chanceOfCast)
        {
            List<CastingState> states = base.SimulateCast(stateBeforeCast, chanceOfCast);
            foreach (CastingState state in states)
            {
                state.Cooldowns.Remove(this);
                if (state.MoltenCoreCharges > 0)
                {
                    --state.MoltenCoreCharges;
                }
                else
                {
                    state.MoltenCoreCharges = 2;
                }
            }
            return states;
        }
        public override void FinalizeSpellModifiers()
        {
            base.FinalizeSpellModifiers();
            SpellModifiers.AddMultiplicativeMultiplier(Mommy.Talents.MoltenCore * .06f);
        }
    }

    //spellID 1454 effect ID 458, 96397, 96427
    public class LifeTap : Spell
    {
        public LifeTap(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                0, // magic school
                SpellTree.Affliction, // spell tree
                0f, // percent base mana (overwritten below)
                0f, // cast time
                0f, // cooldown
                0f, // recast period
                false) // can miss
        { 
            ManaCost = (mommy.CalcHealth() * .15f) * (1.2f + mommy.Talents.ImprovedLifeTap * .1f);
            GCDBonus = mommy.Talents.GlyphLifeTap ? 0.5f : 0f;
        }

        public override bool IsCastable()
        {
            return false;
        }
        public float AddCastsForRegen(float timeRemaining, float manaRemaining, Spell spammedSpell)
        {
            // getting our cast time is not safe until the backdraft multilpier
            // has been set.  fortunetly that's easy to "calculate"
            // BackdraftMultiplier = 1f;

            // The number of needed lifetaps is obtained by solving this
            // system of equations:
            //
            // spamCasts * spamMana + ltCasts * ltMana = manaRemaining
            // spamCasts * spamCast + ltCasts * ltCast = timeRemaining

            float latency = Mommy.Options.Latency;
            float a = spammedSpell.ManaCost;
            float b = ManaCost;
            float c = manaRemaining;
            float d = spammedSpell.GetAvgTimeUsed();
            float e = GetAvgTimeUsed();
            float f = timeRemaining;
            float toAdd = Math.Max(0f, (c * d - a * f) / (b * d - a * e));

            NumCasts += toAdd;

            if (toAdd > 0 && !Mommy.CastSpells.ContainsKey("Life Tap"))
            {
                Mommy.CastSpells.Add("Life Tap", this);
            }

            return toAdd;
        }
    }

    //spellID 5676 effectID 2018
    public class SearingPain : Spell
    {
        private const float SCALE = 0.3219999969f;
        private const float COEFF = 0.3779999912f;

        public SearingPain(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Shadow,
                SpellTree.Demonology,
                .12f, // percentBaseMana,
                1.5f, // baseCastTime,
                0f, // cooldown,
                0f, // recastPeriod,
                WARLOCKSPELLBASEVALUES[mommy.Options.PlayerLevel - 80] * SCALE, // avgDirectDamage
                COEFF, // directCoefficient
                0f, // addedDirectMultiplier,
                0f, // bonusCritChance,
                1f) // bonus crit multiplier
        { 
            // UNMODELED: bonus crit chance = 20% * #ImprovedSearingPain if target < 50% health
        }
    }

    //spellID 686 effectID 267
    public class ShadowBolt : Spell
    {
        private const float SCALE = 0.6200000048f;
        private const float COEFF = 0.7540000081f;
        private static float[] talentValues = { 0f, 0.1f, 0.3f, 0.5f };

        public static float GetBaseCastTime(CharacterCalculationsWarlock mommy)
        {
            return 3f - talentValues[mommy.Talents.Bane];
        }

        public ShadowBolt(CharacterCalculationsWarlock mommy)
            : base(
                mommy, // options
                MagicSchool.Shadow, // magic school
                SpellTree.Destruction, // spell tree
                .1f * (mommy.Talents.GlyphSB ? 0.85f : 1f), // percent base mana
                GetBaseCastTime(mommy), // cast time
                0f, // cooldown
                0f, // recast period
                WARLOCKSPELLBASEVALUES[mommy.Options.PlayerLevel - 80] * SCALE, // avg base
                COEFF, // direct coefficient
                mommy.Talents.ShadowAndFlame * .04f, // addedDirectMultiplier
                mommy.Stats.Warlock2T10 + mommy.Stats.Warlock4T8, // bonus crit chance
                1f) // bonus crit multiplier
        { }
    }

    public class ShadowBolt_Instant : ShadowBolt
    {
        public static bool IsCastable(WarlockTalents talents, List<string> priorities)
        {
            return priorities.Contains("Corruption") && (talents.GlyphCorruption || talents.Nightfall > 0);
        }

        public ShadowBolt_Instant(CharacterCalculationsWarlock mommy) : base(mommy)
        {
            BaseCastTime = 0f;

            // Currently modeled as a spell on a cooldown equal to the
            // average time between procs.  This lengthens the time between
            // casts according to the rules for cooldown collision, which is not
            // completely accurate, but close enough.  To be accurate it
            // should instead factor in the probability that it will proc twice
            // (or more) while casting higher priority spells.
            float procChance = Mommy.Talents.Nightfall * .02f;
            if (Mommy.Talents.GlyphCorruption)
            {
                procChance += .04f;
            }
            float tickPeriod = Corruption.GetTickPeriod(mommy);
            Cooldown = tickPeriod / procChance;
        }

        public override bool IsCastable()
        {
            return IsCastable(Mommy.Talents, GetRotation().SpellPriority);
        }
    }

    //spellID 17877 effectID 9475
    public class Shadowburn : Spell
    {
        private const float SCALE = 0.7139999866f;
        private const float COEFF = 1.0559999943f;

        public Shadowburn(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Shadow,
                SpellTree.Destruction,
                .15f, // percentBaseMana,
                0f, // baseCastTime,
                15f, // cooldown,
                0f, // recastPeriod,
                WARLOCKSPELLBASEVALUES[mommy.Options.PlayerLevel - 80] * SCALE, // avgDirectDamage
                COEFF, // directCoefficient
                0f, // addedDirectMultiplier,
                0f, // bonusCritChance,
                1f) // bonus crit multiplier
        { 
            // UNMODELED: if glyphed and doesn't kill a target, cooldown is reset; cooldown 6 sec for this effect
        }

        public override bool IsCastable()
        {
            return false; //target below 20% health
        }
    }

    //spellID 6353 effectID 2210
    public class SoulFire : Spell
    {
        private const float SCALE = 2.5429999828f;
        private const float COEFF = 0.6280000210f;

        public SoulFire(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Fire, // magicSchool,
                SpellTree.Destruction, // spellTree,
                .09f, // percentBaseMana,
                4f - mommy.Talents.Emberstorm * 0.5f, // baseCastTime,
                0f, // cooldown,
                0f, // recastPeriod,
                true,
                WARLOCKSPELLBASEVALUES[mommy.Options.PlayerLevel - 80] * SCALE, // avgDirectDamage,
                COEFF, // directCoefficient,
                0f, // addedDirectMultiplier,
                WARLOCKSPELLBASEVALUES[mommy.Options.PlayerLevel - 80] * SCALE * mommy.Talents.BurningEmbers * .15f / 7f, // baseTickDamage,
                7f, // numTicks,
                COEFF, // tickCoefficient,
                0f, // addedTickMultiplier,
                true, // canTickCrit,
                0f, // bonusCritChance,
                1f) // bonusCritMultiplier
        {
            //UNMODELED: when target below 35% health, cast time reduced by #Decimation * 20% for 10 sec
        } 
    }

    //spellID 30108 effectID 19678
    public class UnstableAffliction : Spell
    {
        private const float SCALE = 0.2319999933f;
        private const float COEFF = 0.2000000030f;

        public UnstableAffliction(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Shadow,
                SpellTree.Affliction,
                .15f, // percent base mana
                mommy.Talents.GlyphUA ? 1.3f : 1.5f, // cast time
                0f, // cooldown
                15f, // recast period
                WARLOCKSPELLBASEVALUES[mommy.Options.PlayerLevel - 80] * SCALE, // tick damage
                (int)((5f / mommy.AvgHaste) + 0.5f), // num ticks
                COEFF, // tick coefficient
                0f, // addedTickMultiplier
                true, // canTickCrit
                mommy.Talents.EverlastingAffliction * .05f, // bonus crit chance
                1f) // bonus crit multiplier
        { 
            if (mommy.Talents.GlyphUA)
            {
                GCDBonus = .2f;
            }
        }

        public override bool IsCastable()
        {
            return Mommy.Affliction;
        }
        public override void FinalizeSpellModifiers()
        {
            base.FinalizeSpellModifiers();
            SpellModifiers.AddMultiplicativeMultiplier(Mommy.Stats.Warlock4T9);
            if (Mommy.Stats.Warlock2T8 > 0)
            {
                SpellModifiers.AddAdditiveMultiplier(.2f);
            }
        }
    }
}