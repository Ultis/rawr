using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Rawr.WarlockTmp {

    public class Spell {

        public static List<string> ALL_SPELLS = new List<String>();

        static Spell() {

            Type spellType = Type.GetType("Rawr.WarlockTmp.Spell");
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

        public enum SpellTree { Affliction, Demonology, Destruction }

        private class IntList : List<int> {

            public IntList() : base() { }

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

        // set via constructor (all numbers that vary between spell, but not
        // between casts of each spell)
        public CharacterCalculationsWarlock Mommy { get; protected set; }
        public float ManaCost { get; protected set; }
        public float BaseCastTime { get; protected set; }
        public float CritChance { get; protected set; }
        public float BaseDamage { get; protected set; }
        public float DirectCoefficient { get; protected set; }
        public float DirectDamageMultiplier { get; protected set; }
        public float BaseTickDamage { get; protected set; }
        public float NumTicks { get; protected set; }
        public float TickCoefficient { get; protected set; }
        public float BaseBonusCritMultiplier { get; protected set; }
        public float TickDamageMultiplier { get; protected set; }
        public float Cooldown { get; protected set; }

        // set via SetCastingStats()
        public float NumCasts { get; protected set; }

        // set via SetDamageStats()
        public float AvgDirectDamage { get; protected set; }
        public float AvgDirectCritDamage { get; protected set; }
        public float AvgTickDamage { get; protected set; }
        public float AvgTickCritDamage { get; protected set; }
        public float AvgDamagePerCast { get; protected set; }

        #region Constructor
        public Spell(
            CharacterCalculationsWarlock mommy,
            MagicSchool magicSchool,
            SpellTree spellTree,
            float percentBaseMana,
            float costMultiplier,
            float spellCastTime,
            float spellLowDamage,
            float spellHighDamage,
            float directCoefficient,
            float directMultiplier,
            float baseTickDamage,
            float numTicks,
            float tickCoefficient,
            float tickMultiplier,
            float spellCritChance,
            float spellBonusCritMultiplier,
            float cooldown) {

            Mommy = mommy;
            // TODO factor in "mana cost reduction" proc trinket(s?)
            // TODO factor in mana restore procs (as cost reduction)
            ManaCost = mommy.BaseMana * percentBaseMana * costMultiplier;
            BaseCastTime = spellCastTime;
            CritChance = spellCritChance;
            BaseDamage = (spellLowDamage + spellHighDamage) / 2f;
            DirectCoefficient = directCoefficient;
            DirectDamageMultiplier = directMultiplier;
            BaseTickDamage = baseTickDamage;
            NumTicks = numTicks;
            TickCoefficient = tickCoefficient;
            TickDamageMultiplier = tickMultiplier;
            BaseBonusCritMultiplier = spellBonusCritMultiplier;
            Cooldown = cooldown;

            // apply talents that affect entire magic schools or spell trees
            WarlockTalents talents = mommy.Talents;
            float bonusMultiplier = 0f;
            if (magicSchool == MagicSchool.Shadow) {
                bonusMultiplier
                    = talents.ShadowMastery * .03f
                        + Mommy.Stats.BonusShadowDamageMultiplier;
            } else if (magicSchool == MagicSchool.Fire) {
                bonusMultiplier = Mommy.Stats.BonusFireDamageMultiplier;
            }
            DirectDamageMultiplier += bonusMultiplier;
            TickDamageMultiplier += bonusMultiplier;
            if (spellTree == SpellTree.Destruction) {
                BaseBonusCritMultiplier += talents.Ruin * .2f;
                CritChance += talents.Devastation * .05f;
            }
        }
        #endregion

        public virtual bool IsCastable() {

            return true;
        }

        public bool IsSpammable() {

            return Cooldown == 0;
        }

        public virtual void SetCastingStats(float timeRemaining) {

            if (IsSpammable()) {
                NumCasts
                    = timeRemaining / (GetCastTime() + Mommy.Options.Latency);
            } else {
                float avgSpellCastTime = GetCastTime(2f);
                float effectiveCooldown
                    = Cooldown + GetCollisionDelay(avgSpellCastTime);
                NumCasts = Mommy.Options.Duration / effectiveCooldown;
            }
        }

        public void SetDamageStats(float baseSpellPower, float hitChance) {

            float directMultiplier = DirectDamageMultiplier;
            float tickMultiplier = TickDamageMultiplier;
            if (Mommy.CastSpells.ContainsKey("Metamorphosis")) {
                float morphBonus
                    = ((Metamorphosis) Mommy.CastSpells["Metamorphosis"])
                        .GetAvgDamageMultiplier();
                directMultiplier += morphBonus;
                tickMultiplier += morphBonus;
            }

            AvgDirectDamage
                = (BaseDamage + DirectCoefficient * baseSpellPower)
                    * directMultiplier;
            AvgDirectCritDamage
                = AvgDirectDamage * (1.5f + .5f * BaseBonusCritMultiplier);

            AvgTickDamage
                = (BaseTickDamage + TickCoefficient * baseSpellPower)
                    * tickMultiplier;
            AvgTickCritDamage
                = AvgTickDamage * (1.5f + .5f * BaseBonusCritMultiplier);

            float directDamage
                = Utilities.GetWeightedSum(
                    AvgDirectCritDamage,
                    CritChance,
                    AvgDirectDamage,
                    1 - CritChance);
            float tickDamage
                = Utilities.GetWeightedSum(
                    AvgTickCritDamage,
                    CritChance,
                    AvgTickDamage,
                    1 - CritChance);
            AvgDamagePerCast
                = hitChance * (directDamage + NumTicks * tickDamage);
        }

        public String GetToolTip() {

            string toolTip
                = String.Format(
                    "{0:0.0} dps*",
                    NumCasts * AvgDamagePerCast / Mommy.Options.Duration);
            if (AvgDirectDamage > 0) {
                toolTip
                    += String.Format(
                        "{0:0.0}\tAverage Hit\r\n"
                            + "{1:0.0}\tAverage Crit\r\n",
                        AvgDirectDamage,
                        AvgDirectCritDamage);
            }
            if (AvgTickDamage > 0) {
                toolTip
                    += String.Format(
                        "{0:0.0}\tAverage Tick\r\n"
                            + "{1:0.0}\tAverage Tick Crit\r\n"
                            + "{2:0.0}\tTicks Per Cast\r\n",
                        AvgTickDamage,
                        AvgTickCritDamage,
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
                    "{0:0.0}s\tBetween Casts (Average)\r\n"
                        + "{1:0.0}\tDPC\r\n"
                        + "{2:0.0}\tDPCT\r\n"
                        + "{3:0.0}\tDPM\r\n"
                        + "{4:0.0}\tCasts",
                    Mommy.Options.Duration / NumCasts,
                    AvgDamagePerCast,
                    AvgDamagePerCast / GetCastTime(),
                    AvgDamagePerCast / ManaCost,
                    NumCasts);
            return toolTip;
        }

        public float GetCastTime() {

            return GetCastTime(BaseCastTime);
        }

        public float GetCastTime(float baseCastTime) {

            return Math.Max(1f, baseCastTime / (1f + Mommy.Stats.SpellHaste));
        }

        /// <param name="avgSpellCastTime">
        /// The average spell casting time for *lower* priority spells.
        /// </param>
        private float GetCollisionDelay(float avgSpellCastTime) {

            List<float> castTimes = new List<float>();
            List<float> frequencies = new List<float>();
            foreach (KeyValuePair<string, Spell> pair in Mommy.CastSpells) {
                Spell spell = pair.Value;
                castTimes.Add(spell.GetCastTime() + Mommy.Options.Latency);
                frequencies.Add(spell.NumCasts / Mommy.Options.Duration);
            }

            #region create probabilities
            // Create a map of the probability of randomly choosing a point in
            // time while key[0] is being cast, and key[1-n] are all queued up
            // after it.  This assumes 1) spells have an equal chance of coming
            // off cooldown at any given point in time, and 2) no spell on a
            // cooldown can show up twice in the same string of spellcasts.  (1)
            // is a simplifying assumption.  I believe (2) holds true for any
            // in-game warlock.
            Dictionary<IntList, float> probablities
                = new Dictionary<IntList, float>();
            IntList allSpells = new IntList();
            for (int i = castTimes.Count; --i >= 0; ) {
                allSpells.Add(i);
            }
            foreach (
                IntList spellString
                in GetLongerPermutations(new IntList(), allSpells)) {

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
                    in GetLongerPermutations(spellString, allSpells)) {

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
            foreach (KeyValuePair<IntList, float> pair in probablities) {
                IntList spellString = pair.Key;
                float delay = castTimes[spellString[0]] / 2;
                for (int i = spellString.Count; --i > 0; ) {
                    delay += castTimes[spellString[i]];
                }
                float probability = pair.Value;
                weightedAverage += probability * delay;
                totalProbability += probability;
            }
            weightedAverage
                += (1 - totalProbability)
                    * (GetCastTime() + Mommy.Options.Latency) / 2;
            return weightedAverage;
        }

        /// <summary>
        /// Creates all permutations longer than "stem" that can be made
        /// by appending values in "values" that are not already included in
        /// "stem".
        /// </summary>
        /// <param name="stem">A subset of "values".</param>
        /// <param name="values">The universe of values to permute.</param>
        /// <returns>
        /// The order of permutations returned will be from longest to shortest.
        /// </returns>
        private List<IntList> GetLongerPermutations(
            IntList stem, IntList values) {

            List<IntList> permutations = new List<IntList>();
            if (stem.Count == values.Count) {
                return permutations;
            }

            IntList remainingValues = new IntList();
            for (int i = values.Count; --i >= 0; ) {
                if (!stem.Contains(values[i])) {
                    remainingValues.Add(values[i]);
                }
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

    public class Corruption : Spell {

        public Corruption(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Shadow, // magic school
                SpellTree.Affliction, // spell tree
                .14f, // percent base mana
                1f, // cost multiplier
                1.5f, // cast time
                0f, // low damage
                0f, // high damage
                0f, // direct coefficient
                0f, // direct multiplier
                1080f / 6f, // damage per tick
                6f, // num ticks
                (1.2f
                        + mommy.Talents.EmpoweredCorruption * .12f
                        + mommy.Talents.EverlastingAffliction * .01f)
                    / 6f, // tick coefficient
                mommy.BaseTickDamageMultiplier
                    + mommy.Talents.ImprovedCorruption * .02f
                    + mommy.Talents.Contagion * .01f, // tick multiplier
                (mommy.BaseCritChance + mommy.Talents.Malediction * .03f)
                    * mommy.Talents.Pandemic, // crit chance
                mommy.Stats.BonusCritMultiplier
                    + mommy.Talents.Pandemic * .5f, // crit multiplier
                18f) { } // "cooldown"

        public override void SetCastingStats(float timeRemaining) {

            if (Mommy.Talents.GlyphQuickDecay) {
                Cooldown /= 1f + Mommy.Stats.SpellHaste;
            }
            base.SetCastingStats(timeRemaining);
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
                MagicSchool.Shadow, // magic school
                SpellTree.Affliction, // spell tree
                0f, // percent base mana (overwritten below)
                0f, // cost multiplier
                1.5f, // cast time
                0f, // low damage
                0f, // high damage
                0f, // direct coefficient
                0f, // direct multiplier
                0f, // base tick damage
                0f, // num ticks
                0f, // tick coefficient
                0f, // tick multiplier
                0f, // crit chance
                0f, // bonus crit multiplier
                37f) { // cooldown

            ManaCost = -1490f - mommy.Stats.Spirit * 3f;
        }

        public override bool IsCastable() {

            return Mommy.Talents.GlyphLifeTap;
        }

        public void AddCastsForRegen(
            float timeRemaining,
            float manaRemaining,
            float baseHasteDivisor,
            Spell spammedSpell) {

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
            NumCasts += Math.Max(0f, (c * d - a * f) / (b * d - a * e));
        }

        public float GetAvgBonusSpellPower() {

            if (!Mommy.Talents.GlyphLifeTap) {
                return 0f;
            }

            float uprate
                = Math.Min(1f, NumCasts * 40f / Mommy.Options.Duration);
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
                1f, // cost multiplier
                1.5f, // cast time
                0f, // low damage
                0f, // high damage
                0f, // direct coefficient
                0f, // direct multiplier
                0f, // damage per tick
                0f, // num ticks
                0f, // tick coefficient
                0f, // tick multiplier
                0f, // crit chance
                0f, // bonus crit
                180f
                    * (1f
                        - mommy.Talents.Nemesis * .1f)) { } // cooldown

        public override bool IsCastable() {

            return Mommy.Talents.Metamorphosis > 0;
        }

        public override void SetCastingStats(float timeRemaining) {

            base.SetCastingStats(timeRemaining);

            // Discretize NumCasts.  This makes sense becasue of this spell's
            // long cooldown, so that it's (correctly) modelled as more
            // valuable in a 4 minute fight than in a 5 minute fight.

            float maxUprate = GetSpellDuration() / Cooldown;
            float wholeCasts = (float) Math.Floor(NumCasts);
            float partialCast = NumCasts - wholeCasts;
            NumCasts = wholeCasts + Math.Min(1f, partialCast / maxUprate);
        }

        public float GetAvgDamageMultiplier() {

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

        public ShadowBolt(CharacterCalculationsWarlock mommy)
            : base(
                mommy, // options
                MagicSchool.Shadow, // magic school
                SpellTree.Destruction, // spell tree
                .17f, // percent base mana
                1f, // cost multiplier
                3f - mommy.Talents.Bane * .1f, // cast time
                690f, // low base
                770f, // high base
                .8571f
                    + mommy.Talents.ShadowAndFlame * .04f, // direct coefficient
                mommy.BaseDirectDamageMultiplier
                    + mommy.Talents.ImprovedShadowBolt
                        * .01f, // damage multiplier
                0f, // damage per tick
                0f, // num ticks
                0f, // tick coefficient
                0f, // tick multiplier
                mommy.BaseCritChance
                    + mommy.Stats.Warlock4T8
                    + mommy.Stats.Warlock2T10, // crit chance
                mommy.Stats.BonusCritMultiplier, // bonus crit
                0f) { } // cooldown
    }

    public class ShadowBolt_Instant : ShadowBolt {

        public ShadowBolt_Instant(CharacterCalculationsWarlock mommy)
            : base(mommy) {

            BaseCastTime = 1.5f;

            // will be calculated later, but set to > 0 now to indicate this
            // spell is not spammable
            Cooldown = 1f; 
        }

        public override bool IsCastable() {

            return Mommy.CastSpells.ContainsKey("Corruption")
                && (Mommy.Talents.GlyphCorruption
                    || Mommy.Talents.Nightfall > 0);
        }

        public override void SetCastingStats(float timeRemaining) {

            // Currently modeled as a spell on a cooldown equal to the
            // average time between procs.  This lengthens the time between
            // casts according to the rules for cooldown collision, which is not
            // completely accurate, but close enough.  To be accurate it
            // should instead model the probability that it will proc twice (or
            // more) during the casting of higher priority spells.
            float procChance = Mommy.Talents.Nightfall * .02f;
            if (Mommy.Talents.GlyphCorruption) {
                procChance += .04f;
            }
            Spell corruption = Mommy.CastSpells["Corruption"];
            float numProcs
                = procChance * corruption.NumCasts * corruption.NumTicks;
            Cooldown = Mommy.Options.Duration / numProcs;
            base.SetCastingStats(timeRemaining);
        }
    }
}
