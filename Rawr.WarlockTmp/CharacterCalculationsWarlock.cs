using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Rawr.WarlockTmp {

    /// <summary>
    /// Calculates a Warlock's DPS and Spell Stats.
    /// </summary>
    public class CharacterCalculationsWarlock : CharacterCalculationsBase {

        #region overridden properties
        public override float OverallPoints { get; set; }
        public override float[] SubPoints { get; set; }
        #endregion


        #region subclass specific properties

        public float PersonalDps { get { return SubPoints[0]; } }
        public float PetDps { get { return SubPoints[1]; } }

        public Character Character { get; private set; }
        public Stats Stats { get; private set; }
        public CalculationOptionsWarlock Options { get; private set; }
        public WarlockTalents Talents { get; private set; }
        public SpellModifiers SpellModifiers { get; private set; }
        public WeightedStat[] Haste { get; private set; }

        public float BaseMana { get; private set; }
        public float HitChance { get; private set; }

        public Dictionary<string, Spell> Spells { get; private set; }
        public Dictionary<string, Spell> CastSpells { get; private set; }

        #endregion


        #region constructors

        public CharacterCalculationsWarlock() { }

        public CharacterCalculationsWarlock(Character character, Stats stats) {

            Character = character;
            Stats = stats;
            Options = (CalculationOptionsWarlock) character.CalculationOptions;
            if (Options == null) {
                Options = new CalculationOptionsWarlock();
            }
            Talents = character.WarlockTalents;
            BaseMana = BaseStats.GetBaseStats(character).Mana;
            Spells = new Dictionary<string, Spell>();
            CastSpells = new Dictionary<string, Spell>();

            HitChance
                = Math.Min(
                    1f, Options.GetBaseHitRate() / 100f + Stats.SpellHit);

            float personalDps = CalcPersonalDps();
            float petDps = CalcPetDps();
            SubPoints = new float[] { personalDps, petDps };
            OverallPoints = personalDps + petDps;
        }

        #endregion


        #region the overridden method (GetCharacterDisplayCalculationValues)
        /// <summary>
        /// Builds a dictionary containing the values to display for each of the
        /// calculations defined in CharacterDisplayCalculationLabels. The key
        /// should be the Label of each display calculation, and the value
        /// should be the value to display, optionally appended with '*'
        /// followed by any string you'd like displayed as a tooltip on the
        /// value.
        /// </summary>
        /// <returns>
        /// A Dictionary<string, string> containing the values to display for
        /// each of the calculations defined in
        /// CharacterDisplayCalculationLabels.
        /// </returns>
        public override Dictionary<string, string>
            GetCharacterDisplayCalculationValues() {

            Dictionary<string, string> dictValues
                = new Dictionary<string, string>();

            dictValues.Add("Personal DPS", String.Format("{0:0}", PersonalDps));
            dictValues.Add("Pet DPS", String.Format("{0:0}", PetDps));
            dictValues.Add("Total DPS", String.Format("{0:0}", OverallPoints));

            dictValues.Add("Health", String.Format("{0:0}", Stats.Health));
            dictValues.Add("Mana", String.Format("{0:0}", Stats.Mana));
            dictValues.Add("Spirit", String.Format("{0:0}", Stats.Spirit));

            #region Bonus Damage
            //pet scaling consts: http://www.wowwiki.com/Warlock_minions
            const float petInheritedAttackPowerPercentage = 0.57f;
            const float petInheritedSpellPowerPercentage = 0.15f;
            float firePower
                = Stats.SpellPower + Stats.SpellFireDamageRating;
            dictValues.Add(
                "Bonus Damage",
                String.Format(
                    "{0}*"
                        + "{1}\tShadow Damage\r\n"
                        + "{2}\tFire Damage\r\n"
                        + "\r\n"
                        + "Your Fire Damage increases your pet's Attack Power by {3} and Spell Damage by {4}.",
                    Stats.SpellPower,
                    Stats.SpellPower + Stats.SpellShadowDamageRating,
                    Stats.SpellPower + Stats.SpellFireDamageRating,
                    Math.Round(
                        firePower * petInheritedAttackPowerPercentage, 0),
                    Math.Round(
                        firePower * petInheritedSpellPowerPercentage, 0)));
            #endregion

            #region Hit Rating
            float onePercentOfHitRating
                = (1 / StatConversion.GetSpellHitFromRating(1));
            float hitFromRating
                = StatConversion.GetSpellHitFromRating(Stats.HitRating);
            float hitFromTalents = Talents.Suppression * 0.01f;
            float hitFromBuffs
                = (Stats.SpellHit - hitFromRating - hitFromTalents);
            float targetHit = Options.GetBaseHitRate() / 100f;
            float totalHit = targetHit + Stats.SpellHit;
            float missChance = totalHit > 1 ? 0 : (1 - totalHit);
            dictValues.Add(
                "Hit Rating",
                String.Format(
                    "{0}*{1:0.00%} Hit Chance (max 100%) | {2:0.00%} Miss Chance \r\n\r\n"
                        + "{3:0.00%}\t Base Hit Chance on a Level {4:0} target\r\n"
                        + "{5:0.00%}\t from {6:0} Hit Rating [gear, food and/or flasks]\r\n"
                        + "{7:0.00%}\t from Talent: Suppression\r\n"
                        + "{8:0.00%}\t from Buffs: Racial and/or Spell Hit Chance Taken\r\n\r\n"
                        + "You are {9} hit rating {10} the 446 hard cap [no hit from gear, talents or buffs]\r\n\r\n"
                        + "Hit Rating soft caps:\r\n"
                        + "420 - Heroic Presence\r\n"
                        + "368 - Suppression\r\n"
                        + "342 - Suppression and Heroic Presence\r\n"
                        + "289 - Suppression, Improved Faerie Fire / Misery\r\n"
                        + "263 - Suppression, Improved Faerie Fire / Misery and  Heroic Presence",
                    Stats.HitRating,
                    totalHit,
                    missChance,
                    targetHit,
                    Options.TargetLevel,
                    hitFromRating,
                    Stats.HitRating,
                    hitFromTalents,
                    hitFromBuffs,
                    Math.Ceiling(
                        Math.Abs((totalHit - 1) * onePercentOfHitRating)),
                    (totalHit > 1) ? "above" : "below"));
            #endregion

            #region Crit %
            Stats statsBase = BaseStats.GetBaseStats(Character);
            float critFromRating
                = StatConversion.GetSpellCritFromRating(Stats.CritRating);
            float critFromIntellect
                = StatConversion.GetSpellCritFromIntellect(Stats.Intellect);
            float critFromBuffs
                = Stats.SpellCrit
                    - statsBase.SpellCrit
                    - critFromRating
                    - critFromIntellect
                    - (Talents.DemonicTactics * 0.02f)
                    - (Talents.Backlash * 0.01f);
            dictValues.Add(
                "Crit Chance",
                String.Format(
                    "{0:0.00%}*"
                        + "{1:0.00%}\tfrom {2:0} Spell Crit rating\r\n"
                        + "{3:0.00%}\tfrom {4:0} Intellect\r\n"
                        + "{5:0.000%}\tfrom Warlock Class Bonus\r\n"
                        + "{6:0%}\tfrom Talent: Demonic Tactics\r\n"
                        + "{7:0%}\tfrom Talent: Backlash\r\n"
                        + "{8:0%}\tfrom Buffs",
                    Stats.SpellCrit,
                    critFromRating,
                    Stats.CritRating,
                    critFromIntellect,
                    Stats.Intellect,
                    statsBase.SpellCrit,
                    Talents.DemonicTactics * 0.02f,
                    Talents.Backlash * 0.01f,
                    critFromBuffs
                ));
            #endregion

            #region Haste
            float fromRating
                = StatConversion.GetSpellHasteFromRating(Stats.HasteRating);
            float spellHaste = (1 + Stats.SpellHaste) * (1 + fromRating) - 1;
            dictValues.Add(
                "Haste Rating",
                String.Format(
                    "{0:0.00}%"
                        + "*{1:0.00}%\tfrom {2} Haste rating\r\n"
                        + "{3:0.00}%\tfrom Buffs\r\n"
                        + "{4:0.00}s\tGlobal Cooldown",
                    spellHaste * 100f,
                    fromRating * 100f,
                    Stats.HasteRating,
                    (spellHaste - fromRating) * 100f,
                    Math.Max(1.0f, 1.5f / (1 + spellHaste))));
            #endregion Haste

            foreach (string spellName in Spell.ALL_SPELLS) {
                if (CastSpells.ContainsKey(spellName)) {
                    dictValues.Add(
                        spellName, CastSpells[spellName].GetToolTip());
                } else {
                    dictValues.Add(spellName, "-");
                }
            }

            return dictValues;
        }
        #endregion


        #region dps calculations

        private float CalcPersonalDps() {

            if (GetError(Options.SpellPriority) != null) {
                return 0f;
            }

            #region Calculate all the possible haste values
            float nonProcHaste
                = 1 + Stats.SpellHaste
                    + StatConversion.GetSpellHasteFromRating(Stats.HasteRating);

            // the trigger rates are all guestimates at this point, since the
            // real values depend on haste, this is set-up work to determine
            // haste!

            Dictionary<Trigger, float> periods
                = new Dictionary<Trigger, float>();
            Dictionary<Trigger, float> chances
                = new Dictionary<Trigger, float>();
            periods[Trigger.Use] = 0f;
            periods[Trigger.SpellHit]
                = periods[Trigger.SpellCrit]
                = periods[Trigger.SpellCast]
                = periods[Trigger.SpellMiss]
                = periods[Trigger.DamageSpellHit]
                = periods[Trigger.DamageSpellCrit]
                = periods[Trigger.DamageSpellCast]
                = CalculationsWarlock.AVG_UNHASTED_CAST_TIME / nonProcHaste;
            periods[Trigger.DoTTick] = 1.5f;
            periods[Trigger.DamageDone]
                = periods[Trigger.DamageOrHealingDone]
                = 1f
                    / (1f / periods[Trigger.DoTTick]
                        + 1f / periods[Trigger.SpellHit]);

            chances[Trigger.Use] = 1f;
            chances[Trigger.SpellHit]
                = chances[Trigger.DamageSpellHit]
                = HitChance;
            chances[Trigger.SpellCrit]
                = chances[Trigger.DamageSpellCrit]
                = chances[Trigger.DamageDone]
                = chances[Trigger.DamageOrHealingDone]
                = chances[Trigger.SpellHit]
                    * (StatConversion.GetSpellCritFromIntellect(Stats.Intellect)
                        + StatConversion.GetSpellCritFromRating(
                            Stats.CritRating
                                + Stats.WarlockFirestoneSpellCritRating
                                    * (1f + Talents.MasterConjuror * 1.5f))
                        + Stats.BonusCritChance
                        + Stats.SpellCritOnTarget);
            chances[Trigger.SpellCast] = chances[Trigger.DamageSpellCast] = 1f;
            chances[Trigger.SpellMiss] = 1 - chances[Trigger.SpellHit];
            chances[Trigger.DoTTick] = 1f;

            if (Options.SpellPriority.Contains("Corruption")) {
                periods[Trigger.CorruptionTick] = 3.1f;
                if (Talents.GlyphQuickDecay) {
                    periods[Trigger.CorruptionTick] /= nonProcHaste;
                }
                chances[Trigger.CorruptionTick] = 1f;
            } else {
                periods[Trigger.CorruptionTick] = 0f;
                chances[Trigger.CorruptionTick] = 0f;
            }

            List<SpecialEffect> hasteEffects = new List<SpecialEffect>();
            List<float> hasteIntervals = new List<float>();
            List<float> hasteChances = new List<float>();
            List<float> hasteOffsets = new List<float>();
            List<float> hasteScales = new List<float>();
            List<float> hasteValues = new List<float>();
            List<SpecialEffect> hasteRatingEffects = new List<SpecialEffect>();
            List<float> hasteRatingIntervals = new List<float>();
            List<float> hasteRatingChances = new List<float>();
            List<float> hasteRatingOffsets = new List<float>();
            List<float> hasteRatingScales = new List<float>();
            List<float> hasteRatingValues = new List<float>();
            Stats procStats = new Stats();
            foreach (SpecialEffect effect in Stats.SpecialEffects()) {
                if (!periods.ContainsKey(effect.Trigger)) {
                    continue;
                }

                procStats.Accumulate(
                    effect.GetAverageStats(
                        periods[effect.Trigger],
                        chances[effect.Trigger],
                        CalculationsWarlock.AVG_UNHASTED_CAST_TIME,
                        Options.Duration));
                if (effect.Stats.HasteRating > 0) {
                    hasteRatingEffects.Add(effect);
                    hasteRatingIntervals.Add(periods[effect.Trigger]);
                    hasteRatingChances.Add(chances[effect.Trigger]);
                    hasteRatingOffsets.Add(0f);
                    hasteRatingScales.Add(1f);
                    hasteRatingValues.Add(effect.Stats.HasteRating);
                }
                if (effect.Stats.SpellHaste > 0) {
                    hasteEffects.Add(effect);
                    hasteIntervals.Add(periods[effect.Trigger]);
                    hasteChances.Add(chances[effect.Trigger]);
                    hasteOffsets.Add(0f);
                    hasteScales.Add(1f);
                    hasteValues.Add(effect.Stats.SpellHaste);
                }
            }
            procStats.HasteRating = 0;
            procStats.SpellHaste = 0;
            Stats.Accumulate(procStats);
            WeightedStat[] ratings
                = SpecialEffect.GetAverageCombinedUptimeCombinations(
                    hasteRatingEffects.ToArray(),
                    hasteRatingIntervals.ToArray(),
                    hasteRatingChances.ToArray(),
                    hasteRatingOffsets.ToArray(),
                    hasteRatingScales.ToArray(),
                    CalculationsWarlock.AVG_UNHASTED_CAST_TIME,
                    Options.Duration,
                    hasteRatingValues.ToArray());
            WeightedStat[] percentages
                = SpecialEffect.GetAverageCombinedUptimeCombinationsMultiplicative(
                    hasteEffects.ToArray(),
                    hasteIntervals.ToArray(),
                    hasteChances.ToArray(),
                    hasteOffsets.ToArray(),
                    hasteScales.ToArray(),
                    CalculationsWarlock.AVG_UNHASTED_CAST_TIME,
                    Options.Duration,
                    hasteValues.ToArray());
            Haste = new WeightedStat[ratings.Length * percentages.Length];
            for (int p = percentages.Length, f = 0; --p >= 0; ) {
                for (int r = ratings.Length; --r >= 0; ++f) {
                    Haste[f].Chance = percentages[p].Chance * ratings[r].Chance;
                    Haste[f].Value
                        = (1 + percentages[p].Value)
                            * (1 + StatConversion.GetSpellHasteFromRating(
                                    ratings[r].Value + Stats.HasteRating));
                }
            }
            #endregion

            #region Calculate the entire fight's mana pool
            Stats.ManaRestoreFromMaxManaPerSecond
                = Math.Max(
                    Stats.ManaRestoreFromMaxManaPerSecond,
                    .002f
                        * Spell.CalcUprate(
                            Talents.ImprovedSoulLeech * .5f,
                            15f,
                            Spell.GetCastTime(
                                CalculationsWarlock.AVG_UNHASTED_CAST_TIME,
                                1.5f,
                                Haste)));
            float timeRemaining = Options.Duration;
            float manaRemaining
                = Stats.Mana
                    + Stats.ManaRestore
                    + timeRemaining
                        * (Stats.ManaRestoreFromMaxManaPerSecond * Stats.Mana
                            + Stats.Mp5 / 5f);
            #endregion

            #region Calculate NumCasts for each spell
            float lag = Options.Latency;
            foreach (string spellName in PrepForCalcs(Options.SpellPriority)) {
                Spell spell = GetSpell(spellName);
                if (!spell.IsCastable()) {
                    continue;
                }

                spell.SetCastingStats(timeRemaining, manaRemaining);
                CastSpells.Add(spellName, spell);
                timeRemaining -= (spell.GetCastTime() + lag) * spell.NumCasts;
                manaRemaining -= spell.ManaCost * spell.NumCasts;
                if (timeRemaining <= .0001) {
                    break;
                }
            }
            #endregion

            #region Calculate spell modifiers
            SpellModifiers = new SpellModifiers();
            SpellModifiers.AddMultiplicativeMultiplier(
                Stats.BonusDamageMultiplier);
            SpellModifiers.AddMultiplicativeMultiplier(
                Talents.Malediction * .01f);
            SpellModifiers.AddMultiplicativeDirectMultiplier(
                Talents.DemonicPact * .01f);
            // The spellstone bonus is added in individual spells, since it
            // doesn't actually affect Curse of Agony.
            SpellModifiers.AddAdditiveDirectMultiplier(
                Stats.WarlockFirestoneDirectDamageMultiplier);
            SpellModifiers.AddCritChance(Stats.SpellCrit);
            SpellModifiers.AddCritOverallMultiplier(Stats.BonusCritMultiplier);
            if (CastSpells.ContainsKey("Metamorphosis")) {
                SpellModifiers.AddMultiplicativeMultiplier(
                    ((Metamorphosis) CastSpells["Metamorphosis"])
                        .GetAvgBonusMultiplier());
            }
            if (CastSpells.ContainsKey("Curse Of The Elements")) {

                // If the raid is already providing this debuff, the curse will
                // not actually end up casting, so this will not double-count
                // the debuff.
                SpellModifiers.AddMultiplicativeMultiplier(.13f);
            }
            if (Talents.ImprovedShadowBolt > 0
                && Stats.SpellCritOnTarget < .05f) {

                // If the 5% crit debuff is not already being maintained by
                // somebody else (i.e. it's not selected in the buffs tab), we
                // may supply it via Improved Shadow Bolt.
                float casts = 0f;
                if (CastSpells.ContainsKey("Shadow Bolt")) {
                    casts += CastSpells["Shadow Bolt"].NumCasts;
                }
                if (CastSpells.ContainsKey("Shadow Bolt (Instant)")) {
                    casts += CastSpells["Shadow Bolt (Instant)"].NumCasts;
                }
                float uprate = Spell.CalcUprate(
                    Talents.ImprovedShadowBolt * .2f, // proc rate
                    30f, // duration
                    Options.Duration / casts); // trigger period
                float benefit = .05f - Stats.SpellCritOnTarget;
                SpellModifiers.AddCritChance(benefit * uprate);
            }
            #endregion

            #region Calculate damage done for each spell
            LifeTap lifeTap = (LifeTap) GetSpell("Life Tap");
            float spellPower
                = Stats.SpellPower + lifeTap.GetAvgBonusSpellPower();
            float damageDone = 0f;
            Spell conflagrate = null;
            foreach (KeyValuePair<string, Spell> pair in CastSpells) {
                Spell spell = pair.Value;
                if (pair.Key.Equals("Conflagrate")) {
                    conflagrate = spell;
                    continue; // save until we're sure immolate is done
                }
                spell.SetDamageStats(spellPower);
                damageDone += spell.NumCasts * spell.AvgDamagePerCast;
            }
            if (conflagrate != null) {
                conflagrate.SetDamageStats(spellPower);
                damageDone
                    += conflagrate.NumCasts * conflagrate.AvgDamagePerCast;
            }
            #endregion

            return damageDone / Options.Duration;
        }

        private float CalcPetDps() {

            return 0f;
        }

        #endregion


        public Spell GetSpell(string spellName) {

            if (Spells.ContainsKey(spellName)) {
                return Spells[spellName];
            }

            string className = spellName.Replace(" ", "");
            className = className.Replace("(", "_");
            className = className.Replace(")", "");
            Type type = Type.GetType("Rawr.WarlockTmp." + className);
            Spell spell
                = (Spell) Activator.CreateInstance(type, new object[] { this });
            Spells[spellName] = spell;
            return spell;
        }

        public static string GetError(List<string> spellPriority) {

            bool foundCurse = false;
            foreach (string spell in spellPriority) {
                if (spell.StartsWith("Curse")) {
                    if (foundCurse) {
                        return "You may only include one curse.";
                    }
                    foundCurse = true;
                }
            }

            int corr = spellPriority.IndexOf("Corruption");
            int sbInstant = spellPriority.IndexOf("Shadow Bolt (Instant)");
            if (sbInstant >= 0 && sbInstant < corr) {
                return "Shadow Bolt (Instant) may only appear after Corruption.";
            }

            int immo = spellPriority.IndexOf("Immolate");
            int conf = spellPriority.IndexOf("Conflagrate");
            if (conf >= 0 && conf < immo) {
                return "Conflagrate may only appear after Immolate.";
            }

            int spammed = spellPriority.IndexOf("Shadow Bolt");
            if (spammed == -1) {
                spammed = spellPriority.IndexOf("Incinerate");
            }
            if (spammed == -1) {
                return "You have not included a spammable spell.";
            }
            if (spammed != spellPriority.Count - 1) {
                return "No spell may appear after a spammable spell.";
            }
            return null;
        }

        /// <summary>
        /// Gets a modified version of the user's spell priorities, for internal
        /// purposes.
        /// </summary>
        /// <param name="spellPriority"></param>
        /// <returns></returns>
        public List<string> PrepForCalcs(List<string> spellPriority) {

            List<string> forCalcs = new List<string>(spellPriority);
            if (forCalcs.Contains("Shadow Bolt")
                && !forCalcs.Contains("Shadow Bolt (Instant)")
                && ShadowBolt_Instant.MightCast(
                    Talents, forCalcs.Contains("Corruption"))) {

                forCalcs.Insert(forCalcs.Count - 1, "Shadow Bolt (Instant)");
            }
            return forCalcs;
        }
    }
}
//3456789 223456789 323456789 423456789 523456789 623456789 723456789 8234567890
