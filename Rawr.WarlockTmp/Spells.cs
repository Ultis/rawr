using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace Rawr.WarlockTmp {

    public class Spell {

        public enum SpellTree { Affliction, Demonology, Destruction }

        public static List<string> ALL_SPELLS = new List<String>();

        static Spell() {

            Type spellType = Type.GetType("Rawr.WarlockTmp.Spell");
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()) {
                if (type.IsSubclassOf(spellType)) {
                    string name = type.Name;
                    for (int i = 1; i < name.Length; ++i) {
                        if (char.IsUpper(name[i])) {
                            name = name.Insert(i, " ");
                            ++i;
                        } else if (name[i] == '_') {
                            name = name.Replace("_", " (");
                            name = name.Insert(name.Length, ")");
                            i += 2;
                        }
                    }
                    ALL_SPELLS.Add(name);
                }
            }
        }

        public static float CalcUprate(
            float procRate, float duration, float triggerPeriod) {

            return 1f
                - (float) Math.Pow(1f - procRate, duration / triggerPeriod);
        }

        public static float GetTimeUsed(
            float baseCastTime,
            float gcdBonus,
            WeightedStat[] haste,
            float lag) {

            float minGCD = 1f - gcdBonus;
            float unhasted = Math.Max(baseCastTime, 1.5f - gcdBonus);

            float avgHasted = 0f;
            foreach (WeightedStat h in haste) {
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

        // set via constructor but sometimes modified via RecordCollisionDelay()
        public float Cooldown { get; protected set; }
        public float RecastPeriod { get; protected set; }
        public float NumTicks { get; protected set; }
        public SpellModifiers SpellModifiers { get; protected set; }

        // set via RecordCollisionDelay()
        public Dictionary<string, SimulatedStat> SimulatedStats {
            get; protected set; }

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


        #region Non-Damage Constructor
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
                0f, // lowDirectDamage,
                0f, // highDirectDamage,
                0f, // directCoefficient,
                0f, // addedDirectMultiplier,
                0f, // baseTickDamage,
                0f, // numTicks,
                0f, // tickCoefficient,
                0f, // addedTickMultiplier,
                false, // canTickCrit,
                0f, // bonusCritChance,
                0f) { } // bonusCritMultiplier,
        #endregion

        #region Direct Damage Constructor
        public Spell(
            CharacterCalculationsWarlock mommy,
            MagicSchool magicSchool,
            SpellTree spellTree,
            float percentBaseMana,
            float baseCastTime,
            float cooldown,
            float recastPeriod,
            float lowDirectDamage,
            float highDirectDamage,
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
                lowDirectDamage,
                highDirectDamage,
                directCoefficient,
                addedDirectMultiplier,
                0f, // baseTickDamage,
                0f, // numTicks,
                0f, // tickCoefficient,
                0f, // addedTickMultiplier,
                false, // canTickCrit,
                bonusCritChance,
                bonusCritMultiplier) { }
        #endregion

        #region DoT Constructor
        public Spell(
            CharacterCalculationsWarlock mommy,
            MagicSchool magicSchool,
            SpellTree spellTree,
            float percentBaseMana,
            float baseCastTime,
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
                0f, // cooldown
                recastPeriod,
                true,
                0f, // direct low damage
                0f, // direct high damage
                0f, // direct coefficient
                0f, // addedDirectMultiplier
                baseTickDamage,
                numTicks,
                tickCoefficient,
                addedTickMultiplier,
                canTickCrit,
                bonusCritChance,
                bonusCritMultiplier) { }
        #endregion

        #region Kitchen Sink Constructor
        public Spell(
            CharacterCalculationsWarlock mommy,
            MagicSchool magicSchool,
            SpellTree spellTree,
            float percentBaseMana,
            float baseCastTime,
            float cooldown,
            float recastPeriod,
            bool canMiss,
            float lowDirectDamage,
            float highDirectDamage,
            float directCoefficient,
            float addedDirectMultiplier,
            float baseTickDamage,
            float numTicks,
            float tickCoefficient,
            float addedTickMultiplier,
            bool canTickCrit,
            float bonusCritChance,
            float bonusCritMultiplier) {

            Mommy = mommy;
            MagicSchool = magicSchool;
            MySpellTree = spellTree;
            // TODO factor in "mana cost reduction" proc trinket(s?)
            // TODO factor in mana restore procs (as cost reduction)
            ManaCost = mommy.BaseMana * percentBaseMana;
            BaseCastTime = baseCastTime;
            BaseDamage = (lowDirectDamage + highDirectDamage) / 2f;
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
            SpellModifiers.AddAdditiveDirectMultiplier(
                addedDirectMultiplier);
            SpellModifiers.AddAdditiveTickMultiplier(
                addedTickMultiplier);
            SpellModifiers.AddCritChance(bonusCritChance);
            SpellModifiers.AddCritBonusMultiplier(bonusCritMultiplier);
            if (magicSchool == MagicSchool.Shadow) {
                SpellModifiers.AddMultiplicativeMultiplier(
                    Mommy.Stats.BonusShadowDamageMultiplier);
                SpellModifiers.AddAdditiveMultiplier(
                    talents.ShadowMastery * .03f);
                if (Mommy.Options.Filler.Equals("Shadow Bolt")
                    || (Mommy.Options.SpellPriority.Contains("Haunt")
                        && talents.Haunt > 0)) {

                    SpellModifiers.AddMultiplicativeTickMultiplier(
                        Mommy.Talents.ShadowEmbrace * .01f * 3f);
                }
            } else if (magicSchool == MagicSchool.Fire) {
                SpellModifiers.AddMultiplicativeMultiplier(
                    Mommy.Stats.BonusFireDamageMultiplier);
                SpellModifiers.AddAdditiveMultiplier(talents.Emberstorm * .03f);
            }
            if (spellTree == SpellTree.Destruction) {
                SpellModifiers.AddCritBonusMultiplier(talents.Ruin * .2f);
                SpellModifiers.AddCritChance(talents.Devastation * .05f);
                float[] talentAffects = { 0f, .04f, .07f, .1f };
                ManaCost *= (1 - talentAffects[Mommy.Talents.Cataclysm]);
            } else if (spellTree == SpellTree.Affliction) {
                ManaCost *= (1 - Mommy.Talents.Suppression * .01f);
            }
        }
        #endregion


        protected void ApplyImprovedSoulLeech() {

            float reductionOnProc
                = Mommy.Stats.Mana * Mommy.Talents.ImprovedSoulLeech * .01f;
            ManaCost -= .3f * reductionOnProc;
        }

        public virtual bool IsCastable() {

            return true;
        }

        public String GetToolTip() {

            float numCasts = GetNumCasts();
            float castTime = GetAvgTimeUsed() - Mommy.Options.Latency;

            string toolTip;
            if (AvgDamagePerCast > 0) {
                toolTip
                    = String.Format(
                        "{0:0.0} dps*",
                        numCasts * AvgDamagePerCast / Mommy.Options.Duration);
            } else {
                toolTip = String.Format("{0:0.0} casts*", numCasts);
            }
            if (AvgDirectHit > 0) {
                toolTip
                    += String.Format(
                        "{0:0.0}\tAverage Hit\r\n"
                            + "{1:0.0}\tAverage Crit\r\n",
                        AvgDirectHit,
                        AvgDirectCrit);
            }
            if (AvgTickHit > 0) {
                toolTip
                    += String.Format(
                        "{0:0.0}\tAverage Tick\r\n"
                            + "{1:0.0}\tAverage Tick Crit\r\n"
                            + "{2:0.0}\tTicks Per Cast\r\n",
                        AvgTickHit,
                        AvgTickCrit,
                        NumTicks);
            }
            toolTip
                += String.Format(
                    "{0:0.00}s\tCast Time\r\n"
                        + "{1:0.0}\tMana\r\n",
                    castTime,
                    ManaCost);
            if (Cooldown > 0) {
                toolTip += String.Format("{0:0.0}s\tCooldown\r\n", Cooldown);
            }
            toolTip
                += String.Format(
                    "{0:0.0}s\tBetween Casts (Average)\r\n",
                    Mommy.Options.Duration / numCasts);
            if (AvgDamagePerCast > 0) {
                toolTip
                    += String.Format(
                        "{0:0.0}\tDPC\r\n"
                            + "{1:0.0}\tDPCT\r\n"
                            + "{2:0.0}\tDPM\r\n"
                            + "{3:0.0}\tCasts",
                        AvgDamagePerCast,
                        AvgDamagePerCast / castTime,
                        AvgDamagePerCast / ManaCost,
                        numCasts);
            }
            return toolTip;
        }


        #region setting NumCasts

        public void Spam(float timeRemaining) {

            Debug.Assert(Cooldown == 0 && RecastPeriod == 0);
            NumCasts += timeRemaining / GetAvgTimeUsed();
        }

        public virtual float GetNumCasts() {

            if (NumCasts == 0) {
                float delay = SimulatedStats["delay"].GetValue();
                NumCasts
                    = Mommy.Options.Duration / (GetAvgRequeueTime() + delay);
            }
            return NumCasts;
        }

        private bool IsBinary() {

            return false;
        }

        public virtual float GetQueueProbability(CastingState state) {

            if (state.Cooldowns.ContainsKey(this)) {
                if (state.Cooldowns[this] <= 0) {
                    return 1f;
                } else {
                    return 0f;
                }
            } else {
                return Math.Min(state.Elapsed / GetAvgRequeueTime(), 1f);
            }
        }

        private float GetAvgRequeueTime() {

            float period = Math.Max(RecastPeriod, Cooldown);
            float hitChance = Mommy.HitChance;
            if (IsBinary()) {
                hitChance -= GetResist();
            }
            if (CanMiss && hitChance < 1 && Cooldown < period) {

                // If a spell misses, and it can be recast sooner than it
                // normally would otherwise, it will instead wait for either its
                // cooldown (if it has one), or for one spell to be cast
                // inbetween (to allow for travel time + reaction time).
                float missDelay
                    = Math.Max(Cooldown + GetCastTime(null), Mommy.AvgTimeUsed);
                period
                    = Utilities.GetWeightedSum(
                        period, hitChance, missDelay, 1 - hitChance);
            }
            return period;
        }

	    public virtual List<CastingState> SimulateCast(
            CastingState stateBeforeCast, float chanceOfCast) {

            // record stats about this spellcast
            float p = chanceOfCast * stateBeforeCast.Probability;
            float timeUsed = GetTimeUsed(stateBeforeCast);
            RecordSimulatedStat(
                "delay", stateBeforeCast.GetAvgTimeSinceQueued(this), p);
            RecordSimulatedStat("time used", timeUsed, p);

            // construct the casting state(s) that can result from this cast
            List<CastingState> results = new List<CastingState>();
            float hitChance = Mommy.HitChance;
            if (IsBinary()) {
                hitChance -= GetResist();
            }
            float newCooldown
                = Cooldown - timeUsed + GetCastTime(stateBeforeCast);
            if (CanMiss && hitChance < 1f && Cooldown < RecastPeriod) {
                
                // this spell has a different "cooldown" when it hits ...
                PopulateNextState(
                    results,
                    stateBeforeCast,
                    timeUsed,
                    RecastPeriod - timeUsed,
                    hitChance * chanceOfCast);

                // ... than when it misses
                if (newCooldown <= 0) {

                    // ensure at least 1 spell is cast before this one is
                    // requeued, to allow for travel time + reaction time for
                    // the player to detect the miss
                    newCooldown = .0001f;
                }
                PopulateNextState(
                    results,
                    stateBeforeCast,
                    timeUsed,
                    newCooldown,
                    (1 - hitChance) * chanceOfCast);
            } else {
                PopulateNextState(
                    results,
                    stateBeforeCast,
                    timeUsed,
                    Math.Max(newCooldown, RecastPeriod - timeUsed),
                    chanceOfCast);
            }

            return results;
        }

        private void RecordSimulatedStat(
            string statName, float value, float weight) {

            if (!SimulatedStats.ContainsKey(statName)) {
                SimulatedStats[statName] = new SimulatedStat();
            }
            SimulatedStats[statName].AddSample(value, weight);
        }

        private void PopulateNextState(
            List<CastingState> results,
            CastingState stateBeforeCast,
            float timeUsed,
            float cooldownAfterAdvance,
            float p) {

            CastingState nextState = new CastingState(stateBeforeCast);
            nextState.Probability *= p;
            nextState.AddSpell(this, timeUsed);
            nextState.Cooldowns[this] = cooldownAfterAdvance;

            if (MySpellTree == SpellTree.Destruction
                && nextState.ExtraState.ContainsKey("Backdraft Aura")) {

                int nextCharges
                    = (int) nextState.ExtraState["Backdraft Aura"] - 1;
                if (nextCharges == 0) {
                    nextState.ExtraState.Remove("Backdraft Aura");
                } else {
                    nextState.ExtraState["Backdraft Aura"] = nextCharges;
                }
            }

            results.Add(nextState);
        }

        private float GetTimeUsed(CastingState state) {

            return MaybeApplyBackdraft(
                GetTimeUsed(
                    BaseCastTime, GCDBonus, Mommy.Haste, Mommy.Options.Latency),
                state);
        }

        private float GetCastTime(CastingState state) {

            if (BaseCastTime == 0f) {
                return 0f;
            }

            float avg = 0f;
            foreach (WeightedStat h in Mommy.Haste) {
                avg += h.Chance * BaseCastTime / h.Value;
            }
            return MaybeApplyBackdraft(avg, state);
        }

        private float MaybeApplyBackdraft(float time, CastingState state) {

            if (state != null
                && state.ExtraState.ContainsKey("Backdraft Aura")
                && MySpellTree == SpellTree.Destruction) {

                time /= 1f + Mommy.Talents.Backdraft * .1f;
            }
            return time;
        }

        public float GetAvgTimeUsed() {

            if (SimulatedStats.ContainsKey("time used")) {
                return SimulatedStats["time used"].GetValue();
            } else {
                return GetTimeUsed(
                    BaseCastTime, GCDBonus, Mommy.Haste, Mommy.Options.Latency);
            }
        }

        public float GetTotalWeight() {

            return SimulatedStats["delay"].GetTotalWeight();
        }

        #endregion


        #region setting avg damage

        public virtual void SetDamageStats(float baseSpellPower) {

            FinalizeSpellModifiers();

            AvgDirectHit
                = (BaseDamage + DirectCoefficient * baseSpellPower)
                    * SpellModifiers.GetFinalDirectMultiplier();
            AvgTickHit
                = (BaseTickDamage + TickCoefficient * baseSpellPower)
                    * SpellModifiers.GetFinalTickMultiplier();

            // crit direct damage
            float critChance = SpellModifiers.CritChance;
            float critMultiplier = SpellModifiers.GetFinalCritMultiplier();

            // crit direct damage
            AvgDirectCrit = AvgDirectHit * critMultiplier;
            float avgDirectDamage
                    = Utilities.GetWeightedSum(
                        AvgDirectCrit,
                        critChance,
                        AvgDirectHit,
                        1 - critChance);

            // crit tick damage (if possible)
            float avgTickDamage;
            if (CanTickCrit) {
                AvgTickCrit = AvgTickHit * critMultiplier;
                avgTickDamage
                    = Utilities.GetWeightedSum(
                        AvgTickHit, 1 - critChance, AvgTickCrit, critChance);
            } else {
                AvgTickCrit = AvgTickHit;
                avgTickDamage = AvgTickHit;
            }

            // overall damage
            AvgDamagePerCast
                = Mommy.HitChance
                    * (1 - GetResist())
                    * (avgDirectDamage + NumTicks * avgTickDamage);
        }

        protected float GetResist() {

            return StatConversion.GetAverageResistance(
                80, Mommy.Options.TargetLevel, 0f, 0f);
        }

        protected virtual void FinalizeSpellModifiers() {

            SpellModifiers.Accumulate(Mommy.SpellModifiers);
            if (MagicSchool == MagicSchool.Shadow
                && Mommy.CastSpells.ContainsKey("Haunt")) {

                SpellModifiers.AddMultiplicativeTickMultiplier(
                    ((Haunt) Mommy.CastSpells["Haunt"]).GetAvgTickBonus());
                //.2f);
            }
        }

        #endregion

    }

    public class ChaosBolt : Spell {

        public ChaosBolt(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Fire,
                SpellTree.Destruction,
                .07f, // percentBaseMana,
                2.5f - mommy.Talents.Bane * .1f, // baseCastTime,
                mommy.Talents.GlyphChaosBolt ? 10f : 12f, // cooldown,
                0f, // recastPeriod,
                1429f, // lowDirectDamage,
                1813f, // highDirectDamage,
                (1 + mommy.Talents.ShadowAndFlame * .04f)
                    * .7142f, // directCoefficient,
                0f, // addedDirectMultiplier,
                0f, // bonusCritChance,
                0f) { // bonus crit multiplier

            ApplyImprovedSoulLeech();
        }

        public override bool IsCastable() {

            return Mommy.Talents.ChaosBolt > 0;
        }
    }

    public class Conflagrate : Spell {

        public const float COOLDOWN = 10f;

        public static bool WillBeCast(CharacterCalculationsWarlock mommy) {

            return mommy.Options.SpellPriority.Contains("Conflagrate")
                && mommy.Talents.Conflagrate > 0;
        }

        public Conflagrate(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Fire,
                SpellTree.Destruction,
                .16f, // percentBaseMana,
                0f, // baseCastTime,
                COOLDOWN, // cooldown,
                6f, // recastPeriod,
                true, // canMiss,
                .6f * 785f, // lowDirectDamage
                .6f * 785f, // highDirectDamage
                .6f * .2f * 5f, // directCoefficient,
                0f, // addedDirectMultiplier, modified in SetDamageStats
                .4f * 785f / 3f, // baseTickDamage
                3f, // numTicks,
                .4f * .2f * 5f / 3f, // tickCoefficient,
                0f, // addedTickMultiplier, modified in SetDamageStats
                true, // canTickCrit,
                mommy.Talents.FireAndBrimstone * .05f, // bonusCritChance,
                0f) { // bonusCritMultiplier) {

            ApplyImprovedSoulLeech();
        }

        public override bool IsCastable() {

            return Mommy.Talents.Conflagrate > 0;
        }

        protected override void FinalizeSpellModifiers() {

            base.FinalizeSpellModifiers();

            // For some reason I may never understand, firestone's 1% bonus gets
            // 1% of Shadow and Flame and 1% of Emberstorm subtracted from it.
            // Also, that modifier becomes multiplicitive instead of additive.
            float direct = SpellModifiers.AdditiveDirectMultiplier;
            SpellModifiers.AddAdditiveDirectMultiplier(-direct);
            direct -= direct * Mommy.Talents.Emberstorm * .03f;
            SpellModifiers.AddMultiplicativeDirectMultiplier(direct);

            // Also account for improvements to immolate, which in turn improve
            // conflagrate
            SpellModifiers.AddAdditiveMultiplier(
                Mommy.Talents.ImprovedImmolate * .1f
                    + (Mommy.Talents.GlyphImmolate ? .1f : 0f)
                    + Mommy.Talents.Aftermath * .03f);
        }

        public override List<CastingState> SimulateCast(
            CastingState stateBeforeCast, float chanceOfCast) {
            
            List<CastingState> states
                = base.SimulateCast(stateBeforeCast, chanceOfCast);
            Debug.Assert(states.Count == 1);
            CastingState stateOnHit = states[0];
            if (Mommy.Talents.Backdraft > 0) {
                if (Mommy.HitChance < 1f) {
                    CastingState onMiss = new CastingState(stateOnHit);
                    onMiss.Probability *= 1 - Mommy.HitChance;
                    states.Add(onMiss);
                    stateOnHit.Probability *= Mommy.HitChance;
                }
                stateOnHit.ExtraState["Backdraft Aura"] = 3;
            }
            return states;
        }
    }

    public class Corruption : Spell {

        public Corruption(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Shadow, // magic school
                SpellTree.Affliction, // spell tree
                .14f, // percent base mana
                0f, // cast time
                18f, // recast period
                1080f / 6f, // damage per tick
                6f, // num ticks
                (1.2f + mommy.Talents.EmpoweredCorruption * .12f) / 6f
                    + mommy.Talents.EverlastingAffliction
                        * .01f, // tick coefficient
                mommy.Stats.WarlockSpellstoneDotDamageMultiplier
                    + mommy.Talents.ImprovedCorruption * .02f
                    + mommy.Talents.Contagion * .01f
                    + mommy.Talents.SiphonLife * .05f, // addedTickMultiplier
                mommy.Talents.Pandemic > 0, // canTickCrit
                mommy.Talents.Malediction * .03f
                    * mommy.Talents.Pandemic, // bonus crit chance
                mommy.Talents.Pandemic) { // bonus crit multiplier

            WarlockTalents talents = Mommy.Talents;
            if (talents.GlyphQuickDecay) {
                float avgHaste = 0f;
                foreach (WeightedStat h in Mommy.Haste) {
                    avgHaste += h.Chance * h.Value;
                }
                RecastPeriod /= avgHaste;
            }

            #region rolling corruption
            // Malficus solved the average duration of a rolling Corruption, see
            // http://rawr.codeplex.com/Thread/View.aspx?ThreadId=203628
            //
            // D = the non-rolling duration
            // T = the time between reset triggers
            // P = the chance a trigger will actually reset the duration
            // TC = the time between corruption ticks
            float d = RecastPeriod;
            float t = GuessRollingTriggerFrequency();
            float p = Mommy.HitChance * talents.EverlastingAffliction * .2f;
            float tc = d / 6f;
            if (p == 1 && t <= d) {
                RecastPeriod = Mommy.Options.Duration;
            } else if (p > 0 && t > 0) {
                float fightLen = Mommy.Options.Duration;
                int maxTriggers = (int) (fightLen * t);
                int maxTicks = (int) (fightLen / tc);

                RecastPeriod
                    = tc
                        * maleficusDuration(
                            new float[maxTicks + 1, maxTriggers + 1],
                            tc,
                            p,
                            1 / t,
                            (int) (fightLen / tc),
                            6,
                            0);
            }
            NumTicks = RecastPeriod / tc;
            #endregion
        }

        private static float maleficusDuration(
            float[,] cache,
            float TC,
            float P,
            float T,
            int maxTicks,
            int accumTicks,
            int triggerIndex) {

            if (accumTicks >= maxTicks)
                return maxTicks;
            double now = triggerIndex * T;
            double curLen = accumTicks * TC;
            if (now > curLen)
                return accumTicks;

            if (cache[accumTicks, triggerIndex] > 0) {
                return cache[accumTicks, triggerIndex];
            }

            float procDuration =
                maleficusDuration(
                cache,
                TC,
                P,
                T,
                maxTicks,
                (int) (now / TC) + 7,
                triggerIndex + 1);
            float nonProcDuration =
                maleficusDuration(
                cache,
                TC,
                P,
                T,
                maxTicks,
                accumTicks,
                triggerIndex + 1);
            float res = procDuration * P + nonProcDuration * (1 - P);

            cache[accumTicks, triggerIndex] = res;
            return res;
        }

        private float GuessRollingTriggerFrequency() {

            float freq = 0f;
            if (Mommy.Options.Filler.Equals("Shadow Bolt")) {

                // assume about 1/2 the time will be spent spamming shadow bolt
                freq
                    += .5f
                        / GetTimeUsed(
                            ShadowBolt.GetBaseCastTime(Mommy),
                            0f,
                            Mommy.Haste,
                            Mommy.Options.Latency);
            }
            if (Mommy.Options.SpellPriority.Contains("Haunt")
                && Mommy.Talents.Haunt > 0) {

                // assume 11 seconds between haunt casts, on average
                freq += 1f / 11f;
            }
            return freq;
        }
    }

    public class CurseOfAgony : Spell {

        public CurseOfAgony(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Shadow, // magic school
                SpellTree.Affliction, // spell tree
                .1f, // percent base mana
                0f, // cast time
                mommy.Talents.GlyphCoA ? 28f : 24f, // recast period

                // Glyph of Curse of Agony raises the *average* tick to
                // 8/7 its unglyphed value.

                1740f / 12f
                    * (mommy.Talents.GlyphCoA
                        ? 8f / 7f : 1f), // damage per tick
                mommy.Talents.GlyphCoA ? 14f : 12f, // num ticks
                1.2f / 12f, // tick coefficient

                // Notice that spellstone is NOT applied to Curse of Agony.
                // Why?  Ask Blizzard, I have no idea.

                mommy.Talents.ImprovedCurseOfAgony * .05f
                    + mommy.Talents.Contagion * .01f, // addedTickMultiplier
                false, // canTickCrit
                0f, // bonus crit chance
                0f) { // bonus crit multiplier

            GCDBonus = mommy.Talents.AmplifyCurse * .5f;
        }
    }

    public class CurseOfTheElements : Spell {

        public CurseOfTheElements(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                0, // magic school
                SpellTree.Affliction, // spell tree
                .1f, // percent base mana
                0f, // cast time
                0f, // cooldown
                300f, // recast period
                true) { // can miss

            GCDBonus = mommy.Talents.AmplifyCurse * .5f;
        }

        public override bool IsCastable() {

            return Mommy.Stats.BonusShadowDamageMultiplier == 0;
        }
    }

    public class Haunt : Spell {

        private float AvgBonus;

        public Haunt(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Shadow,
                SpellTree.Affliction,
                .12f, // percent base mana
                1.5f, // cast time
                8f, // cooldown
                10f, // recast period
                645f, // low direct damage
                753f, // high direct damage
                .4266f, // direct coefficient
                0f, // bonus direct multiplier
                0f, // bonus crit chance
                mommy.Talents.Pandemic) { } // bonus crit multiplier

        public override bool IsCastable() {

            return Mommy.Talents.Haunt > 0;
        }

        public float GetAvgTickBonus() {

            if (AvgBonus > 0) {
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
            for (int i = delayStats.Values.Count; --i >= 0; ) {
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
                uprate
                    = Math.Max(tolerance - collisionDelay, 0)
                        / (Cooldown + delayStats.GetValue());
                missFollowingHitUprate.AddSample(uprate, probability);

                // CASE 3: this cast misses, previous cast missed.
                // This case will always yeild zero uptime/uprate.
            }

            // average them all together for the overall uprate
            float hitChance = Mommy.HitChance;
            float missChance = 1 - hitChance;
            uprate = Utilities.GetWeightedSum(
                hitUprate.GetValue(),
                hitChance,
                missFollowingHitUprate.GetValue(),
                missChance * hitChance,
                0f,
                missChance * missChance);

            AvgBonus = (Mommy.Talents.GlyphHaunt ? .23f : .2f) * uprate;
            return AvgBonus;
        }
    }

    public class Immolate : Spell {

        private static bool IsClippedByConflagrate(
            CharacterCalculationsWarlock mommy) {

            return mommy.Options.SpellPriority.Contains("Conflagrate")
                && !mommy.Talents.GlyphConflag;
        }

        public Immolate(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Fire,
                SpellTree.Destruction,
                .17f, // percentBaseMana,
                2f - mommy.Talents.Bane * .1f, // baseCastTime,
                0f, // cooldown,
                IsClippedByConflagrate(mommy) ? 10f : 15f, // recastPeriod,
                true, // canMiss,
                460f, // lowDirectDamage,
                460f, // highDirectDamage,
                .2f, // directCoefficient,
                mommy.Talents.ImprovedImmolate * .1f, // addedDirectMultiplier,
                785f / 5f, // baseTickDamage,
                IsClippedByConflagrate(mommy) ? 3f : 5f, // numTicks,
                .2f, // tickCoefficient,
                mommy.Stats.WarlockSpellstoneDotDamageMultiplier
                    + (mommy.Talents.GlyphImmolate ? .1f : 0f)
                    + mommy.Talents.ImprovedImmolate * .1f
                    + mommy.Talents.Aftermath * .03f, // addedTickMultiplier,
                true, // canTickCrit,
                0f, // bonusCritChance,
                0f) { } // bonusCritMultiplier) {

        //protected override float CalcAvgBackdraftBonus(
        //    float fullBonus, float avgCast) {

        //    if (Mommy.Talents.GlyphConflag) {
        //        return base.CalcAvgBackdraftBonus(fullBonus, avgCast);
        //    } else {
        //        // TODO if its possible to have less than 1 charge available
        //        // this should be reduced.
        //        return fullBonus;
        //    }
        //}

        //protected override float CalcAvgBackdraftCasts(
        //    float backdraftPeriod, float backdraftWindow, float avgCast) {

        //    if (Mommy.Talents.GlyphConflag) {
        //        return base.CalcAvgBackdraftCasts(
        //            backdraftPeriod, backdraftWindow, avgCast);
        //    } else {
        //        return 1f;
        //    }
        //}

        //protected override float GetMaxBackdraftCasts() {
            
        //    return 1f;
        //}
    }

    public class Incinerate : Spell {

        public Incinerate(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Fire,
                SpellTree.Destruction,
                .14f, // percentBaseMana,
                2.5f - mommy.Talents.Emberstorm * .05f, // baseCastTime,
                0f, // cooldown,
                0f, // recastPeriod,
                582f, // lowDirectDamage,
                676f, // highDirectDamage,
                (1 + mommy.Talents.ShadowAndFlame * .04f)
                    * .7143f, // directCoefficient,
                mommy.Talents.GlyphIncinerate
                    ? .05f : 0f, // addedDirectMultiplier,
                0f, // bonusCritChance,
                0f) { // bonus crit multiplier

            ApplyImprovedSoulLeech();
        }
    }

    public class Incinerate_UnderBackdraft : Incinerate {

        public Incinerate_UnderBackdraft(CharacterCalculationsWarlock mommy)
            : base(mommy) { }

        public override bool IsCastable() {

            return Mommy.Talents.Backdraft > 0
                && Mommy.Options.SpellPriority.Contains("Conflagrate")
                && Mommy.Talents.Conflagrate > 0;
        }

        public override float GetQueueProbability(CastingState state) {

            if (state.ExtraState.ContainsKey("Backdraft Aura")) {
                return 1f;
            } else {
                return 0f;
            }
        }

        public override float GetNumCasts() {

            if (NumCasts == 0) {
                Spell conflagrate = Mommy.CastSpells["Conflagrate"];
                float castsPerConf
                    = GetTotalWeight() / conflagrate.GetTotalWeight();
                NumCasts = castsPerConf * conflagrate.GetNumCasts();
            }
            return NumCasts;
        }
    }

    public class LifeTap : Spell {

        // This exists as a spell object only for the case when we would have
        // enough mana to never life tap, but still need to keep up the gyph. So
        // make it's cooldown 37s (40s duration of the glyph minus just about
        // enough time to make cooldown collisions & such don't let it drop
        // off).

        public LifeTap(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                0, // magic school
                SpellTree.Affliction, // spell tree
                0f, // percent base mana (overwritten below)
                0f, // cast time
                0f, // cooldown
                37f, // recast period
                false) { // can miss

            ManaCost
                = (-2000f - mommy.Stats.SpellPower * .5f)
                    * (1f + mommy.Talents.ImprovedLifeTap * .1f);
        }

        public override bool IsCastable() {

            return Mommy.Talents.GlyphLifeTap;
        }

        public float AddCastsForRegen(
            float timeRemaining, float manaRemaining, Spell spammedSpell) {

            // getting our cast time is not safe until the backdraft multilpier
            // has been set.  fortunetly that's easy to "calculate"
            //BackdraftMultiplier = 1f;

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

            if (toAdd > 0 && !Mommy.CastSpells.ContainsKey("Life Tap")) {
                Mommy.CastSpells.Add("Life Tap", this);
            }

            return toAdd;
        }

        public float GetAvgBonusSpellPower() {

            if (!Mommy.Talents.GlyphLifeTap) {
                return 0f;
            }

            float uprate
                = Math.Min(
                    1f, (NumCasts + .8f) * 40f / Mommy.Options.Duration);
            return uprate * Mommy.Stats.Spirit * .2f;
        }
    }

    public class Metamorphosis : Spell {

        public Metamorphosis(CharacterCalculationsWarlock mommy)
            : base(
                mommy, // options
                0, // magic school
                SpellTree.Demonology, // spell tree
                0f, // percent base mana
                0f, // cast time
                180f
                    * (1f
                        - mommy.Talents.Nemesis * .1f), // cooldown
                0f, // recast period
                false) { } // can miss

        public override bool IsCastable() {

            return Mommy.Talents.Metamorphosis > 0;
        }

        //public override void SetCastingStats(
        //    float timeRemaining, float manaRemaining) {

        //    base.SetCastingStats(timeRemaining, manaRemaining);

        //    // Discretize NumCasts.  This makes sense becasue of this spell's
        //    // long cooldown, so that it's (correctly) modelled as more
        //    // valuable in a 4 minute fight than in a 5 minute fight.

        //    float maxUprate = GetSpellDuration() / Cooldown;
        //    float wholeCasts = (float) Math.Floor(NumCasts);
        //    float partialCast = NumCasts - wholeCasts;
        //    NumCasts = wholeCasts + Math.Min(1f, partialCast / maxUprate);
        //}

        public float GetAvgBonusMultiplier() {

            float uprate
                = NumCasts * GetSpellDuration() / Mommy.Options.Duration;
            return .2f * uprate;
        }

        private float GetSpellDuration() {

            if (Mommy.Talents.GlyphMetamorphosis) {
                return 36f;
            } else {
                return 30f;
            }
        }
    }

    public class ShadowBolt : Spell {

        public static float GetBaseCastTime(
            CharacterCalculationsWarlock mommy) {

            return 3f - mommy.Talents.Bane * .1f;
        }

        public ShadowBolt(CharacterCalculationsWarlock mommy)
            : base(
                mommy, // options
                MagicSchool.Shadow, // magic school
                SpellTree.Destruction, // spell tree
                .17f, // percent base mana
                GetBaseCastTime(mommy), // cast time
                0f, // cooldown
                0f, // recast period
                694f, // low base
                775f, // high base
                (1 + mommy.Talents.ShadowAndFlame * .04f)
                    * .8571f, // direct coefficient
                mommy.Talents.ImprovedShadowBolt
                    * .01f, // addedDirectMultiplier
                mommy.Stats.Warlock4T8
                    + mommy.Stats.Warlock2T10, // bonus crit chance
                0f) { // bonus crit multiplier

            ApplyImprovedSoulLeech();
        }
    }

    public class ShadowBolt_Instant : ShadowBolt {

        public static bool MightCast(
            WarlockTalents talents, List<string> priorities) {

            return priorities.Contains("Corruption")
                && (talents.GlyphCorruption || talents.Nightfall > 0);
        }

        public ShadowBolt_Instant(CharacterCalculationsWarlock mommy)
            : base(mommy) {

            BaseCastTime = 0f;

            // Currently modeled as a spell on a cooldown equal to the
            // average time between procs.  This lengthens the time between
            // casts according to the rules for cooldown collision, which is not
            // completely accurate, but close enough.  To be accurate it
            // should instead factor in the probability that it will proc twice
            // (or more) while casting higher priority spells.
            float procChance = Mommy.Talents.Nightfall * .02f;
            if (Mommy.Talents.GlyphCorruption) {
                procChance += .04f;
            }
            float tickPeriod = 3.1f; // total guess
            if (Mommy.Talents.GlyphQuickDecay) {
                tickPeriod = GetTimeUsed(tickPeriod, 1f, Mommy.Haste, 0f);
            }
            Cooldown = tickPeriod / procChance;
        }

        public override bool IsCastable() {

            return MightCast(Mommy.Talents, Mommy.Options.SpellPriority);
        }
    }

    public class UnstableAffliction : Spell {

        public UnstableAffliction(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Shadow,
                SpellTree.Affliction,
                .15f, // percent base mana
                mommy.Talents.GlyphUA ? 1.3f : 1.5f, // cast time
                15f, // recast period
                1150f / 5f, // tick damage
                5f, // num ticks
                mommy.Talents.EverlastingAffliction * .01f
                    + 1f / 5f, // tick coefficient
                mommy.Stats.WarlockSpellstoneDotDamageMultiplier
                    + mommy.Talents.SiphonLife * .05f, // addedTickMultiplier
                mommy.Talents.Pandemic > 0, // canTickCrit
                mommy.Talents.Malediction * .03f
                    * mommy.Talents.Pandemic, // bonus crit chance
                mommy.Talents.Pandemic) { // bonus crit multiplier

            if (mommy.Talents.GlyphUA) {
                GCDBonus = .2f;
            }
        }

        public override bool IsCastable() {

            return Mommy.Talents.UnstableAffliction > 0;
        }
    }
}
