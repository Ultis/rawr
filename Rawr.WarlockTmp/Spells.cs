using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace Rawr.WarlockTmp {

    public class Spell {

        public static List<string> ALL_SPELLS = new List<String>();

        public static float CalcUprate(
            float procRate, float duration, float triggerPeriod) {

            return 1f
                - (float) Math.Pow(1f - procRate, duration / triggerPeriod);
        }

        public static float GetCastTime(
            float baseCastTime, float minGCD, WeightedStat[] haste) {

            float castTime = 0f;
            foreach (WeightedStat h in haste) {
                castTime
                    += h.Chance * Math.Max(minGCD, baseCastTime / h.Value);
            }
            return castTime;
        }

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

        public enum SpellTree { Affliction, Demonology, Destruction }

        private class IntList : List<int> {

            public IntList() : base() { }

            public IntList(int capacity) : base(capacity) { }

            public IntList(IEnumerable<int> collection) : base(collection) { }

            public override bool Equals(object obj) {
                IntList other = (IntList) obj;

                if (other.Count != this.Count)
                    return false;
                for (int i = 0; i < this.Count; i++) {
                    if (this[i] != other[i])
                        return false;
                }
                return true;
            }

            public override int GetHashCode() {
                int hashCode = 0;
                for (int i = 0; i < this.Count; i++) {
                    hashCode = hashCode ^ (this[i] << i);
                }
                return hashCode;
            }
        }

        #region properties

        // set via constructor
        public CharacterCalculationsWarlock Mommy { get; protected set; }
        public MagicSchool MagicSchool { get; protected set; }
        public SpellTree MySpellTree { get; protected set; }
        public float ManaCost { get; protected set; }
        public float BaseCastTime { get; protected set; }
        public float MinGCD { get; protected set; }
        public bool CanMiss { get; protected set; }
        public float BaseDamage { get; protected set; }
        public float DirectCoefficient { get; protected set; }
        public float BaseTickDamage { get; protected set; }
        public float TickCoefficient { get; protected set; }
        public bool CanTickCrit { get; protected set; }

        // set via constructor but sometimes modified via SetCastingStats()
        public float Cooldown { get; protected set; }
        public float RecastPeriod { get; protected set; }
        public float NumTicks { get; protected set; }
        public SpellModifiers SpellModifiers { get; protected set; }

        // set via SetCastingStats()
        public List<KeyValuePair<float, float>> CollisionProfile { get; protected set; }
        public float NumCasts { get; protected set; }
        public float AvgCollision { get; protected set; }
        public float BackdraftMultiplier { get; protected set; }

        // set via SetDamageStats()
        public float AvgDirectHit { get; protected set; }
        public float AvgDirectCrit { get; protected set; }
        public float AvgTickHit { get; protected set; }
        public float AvgTickCrit { get; protected set; }
        public float AvgDamagePerCast { get; protected set; }

        #endregion

        #region Non-Damage Constructor
        public Spell(
            CharacterCalculationsWarlock mommy,
            MagicSchool magicSchool,
            SpellTree spellTree,
            float percentBaseMana,
            float baseCastTime,
            float minGCD,
            float cooldown,
            float recastPeriod,
            bool canMiss)
            : this(
                mommy,
                magicSchool,
                spellTree,
                percentBaseMana,
                baseCastTime,
                minGCD,
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
                1f, // minGCD
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
            float minGCD,
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
                minGCD,
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
            float minGCD,
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
            MinGCD = minGCD;
            BaseDamage = (lowDirectDamage + highDirectDamage) / 2f;
            Cooldown = cooldown;
            RecastPeriod = recastPeriod;
            CanMiss = canMiss;
            DirectCoefficient = directCoefficient;
            BaseTickDamage = baseTickDamage;
            NumTicks = numTicks;
            TickCoefficient = tickCoefficient;
            CanTickCrit = canTickCrit;

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
                if (Mommy.Options.SpellPriority.Contains("Shadow Bolt")
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

        public virtual bool IsCastable() {

            return true;
        }

        public bool IsSpammed() {

            return Cooldown == 0 && RecastPeriod == 0;
        }

        /// <summary>
        /// Sets the variables NumCasts, CollisionProfile and AvgCollision.
        /// Called on spells in the order of thier priority, and before
        /// SetDamageStats on any spell. Subclasses may also override to modify
        /// RecastPeriod, Cooldown, NumTicks or SpellModifiers. Do not use the
        /// value of any of these variables before SetCastingStats has been
        /// called on that Spell.
        /// </summary>
        /// <param name="timeRemaining"></param>
        public virtual void SetCastingStats(
            float timeRemaining, float manaRemaining) {

            // calc backdraft
            BackdraftMultiplier = 1f;
            float avgCast
                = GetCastTime(
                    CalculationsWarlock.AVG_UNHASTED_CAST_TIME,
                    1f,
                    Mommy.Haste);
            float fullBackdraftBonus = Mommy.Talents.Backdraft * .1f;
            if (fullBackdraftBonus > 0 && Conflagrate.WillBeCast(Mommy)) {
                BackdraftMultiplier
                    -= CalcAvgBackdraftBonus(fullBackdraftBonus, avgCast);
            }

            // spammed?  factor in lifetaps then spam away
            float lag = Mommy.Options.Latency;
            if (IsSpammed()) {
                LifeTap lifeTap = (LifeTap) Mommy.GetSpell("Life Tap");
                float added = lifeTap.AddCastsForRegen(
                    timeRemaining, manaRemaining, this);
                timeRemaining -= added * (lifeTap.GetCastTime() + lag);
                NumCasts = timeRemaining / (GetCastTime() + lag);
                return;
            }

            // not spammed? now it's time for serious calculations
            SetCollisionDelay(avgCast, timeRemaining);
            float period = Math.Max(RecastPeriod, Cooldown) + AvgCollision;
            float hitChance = Mommy.HitChance;
            if (IsBinary()) {
                hitChance -= GetResist();
            }
            if (CanMiss && hitChance < 1 && Cooldown < RecastPeriod) {

                // If a spell misses, and it can be recast sooner than it
                // normally would otherwise, it will instead wait for either its
                // cooldown (if it has one), or for one spell to be cast
                // inbetween (to allow for travel time and reaction time for the
                // player to detect the miss). Note that AvgCollision already
                // has 1/2 an average spellcast factored into it.
                float periodAfterMiss
                    = Math.Max(Cooldown, (avgCast + lag) / 2) + AvgCollision;
                period
                    = Utilities.GetWeightedSum(
                        period,
                        hitChance,
                        periodAfterMiss,
                        1 - hitChance);
            }
            NumCasts = Mommy.Options.Duration / period;
        }

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

        public String GetToolTip() {

            string toolTip;
            if (AvgDamagePerCast > 0) {
                toolTip
                    = String.Format(
                        "{0:0.0} dps*",
                        NumCasts * AvgDamagePerCast / Mommy.Options.Duration);
            } else {
                toolTip = String.Format("{0:0.0} casts*", NumCasts);
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
                    "{0:0.0}s\tCast Time\r\n"
                        + "{1:0.0}\tMana\r\n",
                    GetCastTime(),
                    ManaCost);
            if (Cooldown > 0) {
                toolTip += String.Format("{0:0.0}s\tCooldown\r\n", Cooldown);
            }
            toolTip
                += String.Format(
                    "{0:0.0}s\tBetween Casts (Average)\r\n",
                    Mommy.Options.Duration / NumCasts);
            if (AvgDamagePerCast > 0) {
                toolTip
                    += String.Format(
                        "{0:0.0}\tDPC\r\n"
                            + "{1:0.0}\tDPCT\r\n"
                            + "{2:0.0}\tDPM\r\n"
                            + "{3:0.0}\tCasts",
                        AvgDamagePerCast,
                        AvgDamagePerCast / GetCastTime(),
                        AvgDamagePerCast / ManaCost,
                        NumCasts);
            }
            return toolTip;
        }

        /// <summary>
        /// Not accurate until after SetCastingStats() sets BackdraftMultiplier.
        /// </summary>
        public virtual float GetCastTime() {

            return BackdraftMultiplier
                * GetCastTime(BaseCastTime, MinGCD, Mommy.Haste);
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

        protected void ApplyImprovedSoulLeech() {

            float reductionOnProc
                = Mommy.Stats.Mana * Mommy.Talents.ImprovedSoulLeech * .01f;
            ManaCost -= .3f * reductionOnProc;
        }

        protected float GetResist() {

            return StatConversion.GetAverageResistance(
                80, Mommy.Options.TargetLevel, 0f, 0f);
        }

        protected virtual bool CanCollideWith(Spell spell) {

            return true;
        }

        /// <summary>
        /// This works for spells that are just as likely to be cast during
        /// backdraft as any other time, and spells that are not affected by
        /// backdraft (i.e. GetMaxBackdraftCasts() == 0). Override for other
        /// spells.
        /// </summary>
        protected virtual float CalcAvgBackdraftBonus(
            float fullBonus, float avgCast) {

            float maxCasts = GetMaxBackdraftCasts();
            if (maxCasts == 0) {
                return 0;
            }

            float lag = Mommy.Options.Latency;
            float backdraftPeriod = Conflagrate.COOLDOWN + avgCast / 2 + lag;
            float backdraftWindow = 3f * (avgCast * (1 - fullBonus) + lag);
            Debug.Assert(backdraftWindow < backdraftPeriod);

            float castsRemaining = 3f;
            foreach (KeyValuePair<string, Spell> pair in Mommy.CastSpells) {
                castsRemaining
                    -= pair.Value.CalcAvgBackdraftCasts(
                        backdraftWindow, backdraftPeriod);
            }
            if (castsRemaining < .0001) {
                return 0f;
            }

            return Math.Min(
                    castsRemaining,
                    GetMaxBackdraftCasts())
                * backdraftWindow
                / backdraftPeriod
                * fullBonus;
        }

        /// <summary>
        /// The number of times this spell is cast during backdraft, factoring
        /// in the spell's own cooldown but NOT other spells that may have
        /// already consumed charges.
        /// </summary>
        protected virtual float CalcAvgBackdraftCasts(
            float backdraftWindow, float backdraftPeriod) {

            float max = GetMaxBackdraftCasts();
            if (Cooldown == 0) {
                return max;
            } else {
                Debug.Assert(Cooldown > backdraftWindow);
                return max * backdraftWindow / backdraftPeriod;
            }
        }

        protected virtual float GetMaxBackdraftCasts() {

            if (MySpellTree == SpellTree.Destruction) {
                if (Cooldown == 0) {
                    return 3f;
                } else {
                    return 1f;
                }
            } else {
                return 0f;
            }
        }

        private bool IsBinary() {

            return false;
        }

        /// <summary>
        /// Calculates the average time a spell will have to wait between
        /// "queuing up" (e.g. coming off cooldown) and being cast, during which
        /// time a player is finishing (approx. 1/2) of whatever cast they
        /// were in the middle of, plus casting any higher priority spells that
        /// queue up during other wait times.
        /// </summary>
        /// <param name="avgSpellCastTime">
        /// The average spell casting time for *lower* priority spells.
        /// </param>
        private void SetCollisionDelay(
            float avgSpellCastTime, float timeRemaining) {

            float lag = Mommy.Options.Latency;
            avgSpellCastTime += lag;
            List<float> castTimes = new List<float>();
            List<float> frequencies = new List<float>();
            Dictionary<IntList, float> probablities
                = new Dictionary<IntList, float>();

            #region initialize variables
            float fightLength = Mommy.Options.Duration;
            foreach (KeyValuePair<string, Spell> pair in Mommy.CastSpells) {
                Spell spell = pair.Value;
                if (CanCollideWith(spell)) {
                    castTimes.Add(spell.GetCastTime() + lag);
                    frequencies.Add(spell.NumCasts / fightLength);
                }
            }
            IntList prioritySpells = new IntList();
            for (int i = castTimes.Count; --i >= 0; ) {
                prioritySpells.Add(i);
            }
            List<IntList> allSpellStrings
                = GetLongerPermutations(new IntList(), prioritySpells);
            List<IntList> withLowerPriority = new List<IntList>();
            castTimes.Add(avgSpellCastTime);
            frequencies.Add(timeRemaining / avgSpellCastTime / fightLength);
            int lowPrioritySpell = castTimes.Count - 1;
            foreach (IntList spellString in allSpellStrings) {
                IntList newString = new IntList(spellString.Count + 1);
                newString.Add(lowPrioritySpell);
                newString.AddRange(spellString);
                withLowerPriority.Add(newString);
            }
            allSpellStrings.AddRange(withLowerPriority);
            allSpellStrings.Add(new IntList() { lowPrioritySpell });
            #endregion

            #region create probabilities
            // Create a map of the probability of randomly choosing a point in
            // time while key[0] is being cast, and key[1-n] are all queued up
            // after it.  This assumes 1) spells have an equal chance of coming
            // off cooldown at any given point in time, and 2) no spell on a
            // cooldown can show up twice in the same string of spellcasts.
            // These are both simplifying assumptions, especially when
            // considering that spells like corruption will be recast immediatly
            // if they miss, but I had a hard enough time coming up with just
            // THIS math.
            foreach (IntList spellString in allSpellStrings) {

                // first calculate the probability of the given spell string
                int spell = spellString[0];
                float stringLength = castTimes[spell];
                float probability = stringLength * frequencies[spell];
                for (int i = spellString.Count; --i > 0; ) {
                    spell = spellString[i];
                    probability *= stringLength * frequencies[spell];
                    stringLength += castTimes[spell];
                }

                // then subtract the probability of it being the start
                // of a longer string of spellcasts
                foreach (
                    IntList longerString
                    in GetLongerPermutations(spellString, prioritySpells)) {

                    probability -= probablities[longerString];
                }
                probablities[spellString] = probability;
            }
            #endregion

            // Use the probabilites to calculate a weighted average of how long
            // THIS spell will have to wait between coming off cooldown and
            // and being cast.
            float weightedAverage = 0f;
            float totalProbability = 0f;
            CollisionProfile = new List<KeyValuePair<float, float>>();
            foreach (KeyValuePair<IntList, float> pair in probablities) {
                IntList spellString = pair.Key;
                float delay = castTimes[spellString[0]] / 2;
                for (int i = spellString.Count; --i > 0; ) {
                    delay += castTimes[spellString[i]];
                }
                float probability = pair.Value;
                CollisionProfile.Add(
                    new KeyValuePair<float, float>(probability, delay));
                weightedAverage += probability * delay;
                totalProbability += probability;
            }
            AvgCollision = weightedAverage;
        }

        /// <summary>
        /// Creates all permutations longer than "stem" that can be made
        /// by appending values in "values" that are not already included in
        /// "stem".
        /// </summary>
        /// <param name="stem"></param>
        /// <param name="values">The universe of values to permute.</param>
        /// <returns>
        /// The order of permutations returned will be from longest to shortest.
        /// </returns>
        private List<IntList> GetLongerPermutations(
            IntList stem, IntList values) {

            IntList remainingValues = new IntList();
            for (int i = values.Count; --i >= 0; ) {
                if (!stem.Contains(values[i])) {
                    remainingValues.Add(values[i]);
                }
            }
            List<IntList> permutations = new List<IntList>();
            if (remainingValues.Count == 0) {
                return permutations;
            }

            for (
                int appendLen = remainingValues.Count;
                appendLen > 0;
                --appendLen) {

                foreach (
                    IntList combination
                    in GetCombinations(remainingValues, appendLen)) {

                    foreach (
                        IntList permutation
                        in GetPermutations(combination)) {

                        permutation.InsertRange(0, stem);
                        permutations.Add(permutation);
                    }
                }
            }
            return permutations;
        }

        private List<IntList> GetCombinations(IntList values, int length) {

            List<IntList> list = new List<IntList>();
            AddCombinations(values, length, list);
            return list;
        }

        private void AddCombinations(
            IntList values, int length, List<IntList> list) {

            if (length == values.Count) {
                list.Add(values);
                return;
            }

            if (length == 1) {
                for (int i = values.Count; --i >= 0; ) {
                    list.Add(new IntList { values[i] });
                }
                return;
            }

            IntList subValues = new IntList(values);
            subValues.RemoveAt(subValues.Count - 1);

            // combinations that to DO include the last value
            int lastValue = values[values.Count - 1];
            foreach (
                IntList combination
                in GetCombinations(subValues, length - 1)) {

                combination.Add(lastValue);
                list.Add(combination);
            }

            // combinations that DO NOT include the last value
            AddCombinations(subValues, length, list);
        }

        private List<IntList> GetPermutations(IntList values) {

            List<IntList> permutations = new List<IntList>();
            if (values.Count == 1) {
                permutations.Add(new IntList(values));
                return permutations;
            }

            IntList subIndicies = new IntList();
            for (int i = values.Count; --i >= 0; ) {
                subIndicies.Clear();
                subIndicies.AddRange(values);
                subIndicies.RemoveAt(i);
                foreach (
                    IntList permutation in GetPermutations(subIndicies)) {

                    permutation.Add(values[i]);
                    permutations.Add(permutation);
                }
            }
            return permutations;
        }
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
                1.5f, // baseCastTime,
                1f, // minGCD,
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

        protected override bool CanCollideWith(Spell spell) {
            
            return Mommy.Talents.GlyphConflag || !(spell is Immolate);
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

        protected override float GetMaxBackdraftCasts() {
            
            return 0f;
        }
    }

    public class Corruption : Spell {

        public Corruption(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Shadow, // magic school
                SpellTree.Affliction, // spell tree
                .14f, // percent base mana
                1.5f, // cast time
                1f, // minGCD
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
                mommy.Talents.Pandemic) { } // bonus crit multiplier

        public override void SetCastingStats(
            float timeRemaining, float manaRemaining) {

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
            float t = CalcRollingTriggerFrequency();
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

            base.SetCastingStats(timeRemaining, manaRemaining);
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

        private float CalcRollingTriggerFrequency() {

            float freq = 0f;
            if (Mommy.Options.SpellPriority.Contains("Shadow Bolt")) {

                // assume about 1/2 the time will be spent spamming shadow bolt
                freq += .5f / (GetCastTime(
                                ShadowBolt.GetBaseCastTime(Mommy),
                                1.5f,
                                Mommy.Haste)
                            + Mommy.Options.Latency);
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
                1.5f - mommy.Talents.AmplifyCurse * .5f, // cast time
                1f - mommy.Talents.AmplifyCurse * .5f, // minGCD
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
                0f) { } // bonus crit multiplier
    }

    public class CurseOfTheElements : Spell {

        public CurseOfTheElements(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                0, // magic school
                SpellTree.Affliction, // spell tree
                .1f, // percent base mana
                1.5f - mommy.Talents.AmplifyCurse * .5f, // cast time
                1f - mommy.Talents.AmplifyCurse * .5f, // minGCD
                0f, // cooldown
                300f, // recast period
                true) { } // can miss

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
            float hitUprate = 0f;
            float missFollowingHitUprate = 0f;
            float tolerance = 12f - RecastPeriod;
            foreach (KeyValuePair<float, float> pair in CollisionProfile) {
                float probability = pair.Key;
                float collisionDelay = pair.Value;

                // CASE 1: this cast hits.
                // uprate = duration / window
                // duration = 12
                // window = RecastPeriod + collisionDelay
                uprate = Math.Min(1f, 12f / (RecastPeriod + collisionDelay));
                hitUprate += probability * uprate;

                // CASE 2: this cast misses, previous cast hit.
                // uprate = leftoverUptime / window
                // leftoverUptime = tolerance - collisionDelay
                // window = Cooldown + collisionDelay
                uprate
                    = Math.Max(tolerance - collisionDelay, 0)
                        / (Cooldown + AvgCollision);
                missFollowingHitUprate += probability * uprate;

                // CASE 3: this cast misses, previous cast missed.
                // This case will always yeild zero uptime/uprate.
            }

            // average them all together for the overall uprate
            float hitChance = Mommy.HitChance;
            float missChance = 1 - hitChance;
            uprate = Utilities.GetWeightedSum(
                hitUprate,
                hitChance,
                missFollowingHitUprate,
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
                1f, // minGCD,
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
                false, // canTickCrit,
                0f, // bonusCritChance,
                0f) { } // bonusCritMultiplier) {

        protected override float CalcAvgBackdraftBonus(
            float fullBonus, float avgCast) {

            if (Mommy.Talents.GlyphConflag) {
                return fullBonus;
            } else {
                return base.CalcAvgBackdraftBonus(fullBonus, avgCast);
            }
        }

        protected override float CalcAvgBackdraftCasts(
            float backdraftPeriod, float backdraftWindow) {

            if (Mommy.Talents.GlyphConflag) {
                return 1f;
            } else {
                return base.CalcAvgBackdraftCasts(
                    backdraftPeriod, backdraftWindow);
            }
        }
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

    //public class Incinerate_UnderBackdraft : Incinerate {

    //    public Incinerate_UnderBackdraft(CharacterCalculationsWarlock mommy)
    //        : base(mommy) {

    //        float multiplier = 1 - Mommy.Talents.Backdraft * .1f;
    //        BaseCastTime *= multiplier;
    //        MinGCD *= multiplier;
    //    }

    //    public override bool IsCastable() {

    //        return Mommy.Talents.Backdraft > 0;
    //    }

    //    public override void SetCastingStats(float timeRemaining) {


    //        base.SetCastingStats(timeRemaining);
    //    }
    //}

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
                1.5f, // cast time
                1f, // minGCD
                0f, // cooldown
                37f, // recast period
                false) { // can miss

            ManaCost
                = (-1490f - mommy.Stats.Spirit * 3f)
                    * (1f + mommy.Talents.ImprovedLifeTap * .1f);
        }

        public override bool IsCastable() {

            return Mommy.Talents.GlyphLifeTap;
        }

        public float AddCastsForRegen(
            float timeRemaining, float manaRemaining, Spell spammedSpell) {

            // getting our cast time is not safe until the backdraft multilpier
            // has been set.  fortunetly that's easy to "calculate"
            BackdraftMultiplier = 1f;

            // The number of needed lifetaps is obtained by solving this
            // system of equations:
            //
            // spamCasts * spamMana + ltCasts * ltMana = manaRemaining
            // spamCasts * spamCast + ltCasts * ltCast = timeRemaining

            float latency = Mommy.Options.Latency;
            float a = spammedSpell.ManaCost;
            float b = ManaCost;
            float c = manaRemaining;
            float d = spammedSpell.GetCastTime() + latency;
            float e = GetCastTime() + latency;
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
                1.5f, // cast time
                1f, // minGCD
                180f
                    * (1f
                        - mommy.Talents.Nemesis * .1f), // cooldown
                0f, // recast period
                false) { } // can miss

        public override bool IsCastable() {

            return Mommy.Talents.Metamorphosis > 0;
        }

        public override void SetCastingStats(
            float timeRemaining, float manaRemaining) {

            base.SetCastingStats(timeRemaining, manaRemaining);

            // Discretize NumCasts.  This makes sense becasue of this spell's
            // long cooldown, so that it's (correctly) modelled as more
            // valuable in a 4 minute fight than in a 5 minute fight.

            float maxUprate = GetSpellDuration() / Cooldown;
            float wholeCasts = (float) Math.Floor(NumCasts);
            float partialCast = NumCasts - wholeCasts;
            NumCasts = wholeCasts + Math.Min(1f, partialCast / maxUprate);
        }

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
            WarlockTalents talents, bool usingCorruption) {

            return usingCorruption
                && (talents.GlyphCorruption || talents.Nightfall > 0);
        }

        public ShadowBolt_Instant(CharacterCalculationsWarlock mommy)
            : base(mommy) {

            BaseCastTime = 1.5f;

            // will be calculated later, but set to > 0 now to indicate this
            // spell is not spammable
            Cooldown = 1f;
        }

        public override bool IsCastable() {

            return MightCast(
                Mommy.Talents, Mommy.CastSpells.ContainsKey("Corruption"));
        }

        public override void SetCastingStats(
            float timeRemaining, float manaRemaining) {

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
            Spell corruption = Mommy.CastSpells["Corruption"];
            float corrTicks
                = Mommy.HitChance * corruption.NumCasts * corruption.NumTicks;
            Cooldown = Mommy.Options.Duration / (corrTicks * procChance);
            base.SetCastingStats(timeRemaining, manaRemaining);
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
                1f, // minGCD
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
                mommy.Talents.Pandemic) { } // bonus crit multiplier

        public override bool IsCastable() {

            return Mommy.Talents.UnstableAffliction > 0;
        }
    }
}
