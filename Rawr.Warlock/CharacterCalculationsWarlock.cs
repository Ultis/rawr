using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Rawr.Warlock {

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
        public Pet Pet { get; private set; }
        public Stats PreProcStats { get; private set; }
        public Stats PetBuffs { get; private set; }
        public Stats Stats { get; private set; }
        public CalculationOptionsWarlock Options { get; private set; }
        public WarlockTalents Talents { get; private set; }
        public SpellModifiers SpellModifiers { get; private set; }
        public List<WeightedStat> Haste { get; private set; }

        public float BaseMana { get; private set; }
        public float HitChance { get; private set; }
        public float AvgTimeUsed { get; private set; }
        public float ExtraCritAtMax { get; private set; }
        public float AvgHaste { get; private set; }

        public List<Spell> Priorities { get; private set; }
        public Dictionary<string, Spell> Spells { get; private set; }
        public Dictionary<string, Spell> CastSpells { get; private set; }

        #endregion


        #region constructors

        public CharacterCalculationsWarlock() { }

        /// <param name="stats">
        /// This should already have buffStats factored in.
        /// </param>
        public CharacterCalculationsWarlock(
            Character character, Stats stats, Stats petBuffs) {

            Character = character;
            Options = (CalculationOptionsWarlock) character.CalculationOptions;
            if (Options == null) {
                Options = CalculationOptionsWarlock.MakeDefaultOptions();
            }
            Talents = character.WarlockTalents;
            Stats = stats;
            PreProcStats = Stats.Clone();
            PetBuffs = petBuffs;
            BaseMana = BaseStats.GetBaseStats(character).Mana;
            Spells = new Dictionary<string, Spell>();
            CastSpells = new Dictionary<string, Spell>();
            HitChance
                = Math.Min(
                    1f,
                    Options.GetBaseHitRate() / 100f + CalcSpellHit());

            if (!Options.Pet.Equals("None")
                && (Talents.SummonFelguard > 0
                    || !Options.Pet.Equals("Felguard"))) {

                Type type = Type.GetType("Rawr.Warlock." + Options.Pet);
                Pet = (Pet) Activator.CreateInstance(
                        type, new object[] { this });
            }

            float personalDps = CalcPersonalDps();
            float petDps = CalcPetDps();
            SubPoints = new float[] { personalDps, petDps };
            OverallPoints = personalDps + petDps;
        }

        #endregion


        #region Stat accessors

        public float CalcStamina() { return StatUtils.CalcStamina(Stats); }

        public float CalcIntellect() { return StatUtils.CalcIntellect(Stats); }

        public float CalcSpirit() { return StatUtils.CalcSpirit(Stats); }

        public float CalcHealth() { return StatUtils.CalcHealth(Stats); }

        public float CalcMana() { return StatUtils.CalcMana(Stats); }

        public float CalcUsableMana(float fightLen) {
            
            return StatUtils.CalcUsableMana(Stats, fightLen);
        }

        public float CalcSpellCrit() { return StatUtils.CalcSpellCrit(Stats); }

        public float CalcSpellHit() { return StatUtils.CalcSpellHit(Stats); }

        public float CalcSpellPower() {
            
            return StatUtils.CalcSpellPower(Stats);
        }

        public float CalcSpellHaste() {
            
            return StatUtils.CalcSpellHaste(Stats);
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

            dictValues.Add("Personal DPS", string.Format("{0:0}", PersonalDps));
            dictValues.Add("Pet DPS", string.Format("{0:0}", PetDps));
            dictValues.Add("Total DPS", string.Format("{0:0}", OverallPoints));

            dictValues.Add(
                "Health",
                string.Format(
                    "{0:0.0}*{1:0.0} stamina",
                    CalcHealth(),
                    CalcStamina()));
            dictValues.Add(
                "Mana",
                string.Format(
                    "{0:0.0}*{1:0.0} intellect",
                    CalcMana(),
                    CalcIntellect()));
            dictValues.Add(
                "Spirit", string.Format("{0:0.0}", CalcSpirit()));

            dictValues.Add(
                "Bonus Damage",
                string.Format(
                    "{0:0.0}*{1:0.0}\tBefore Procs",
                    CalcSpellPower(),
                    StatUtils.CalcSpellPower(PreProcStats)));

            #region Hit Rating
            float onePercentOfHitRating
                = (1 / StatConversion.GetSpellHitFromRating(1));
            float hitFromRating
                = StatConversion.GetSpellHitFromRating(Stats.HitRating);
            float hitFromTalents = Talents.Suppression * 0.01f;
            float hitFromBuffs
                = (CalcSpellHit() - hitFromRating - hitFromTalents);
            float targetHit = Options.GetBaseHitRate() / 100f;
            float totalHit = targetHit + CalcSpellHit();
            float missChance = totalHit > 1 ? 0 : (1 - totalHit);
            dictValues.Add(
                "Hit Rating",
                string.Format(
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

            dictValues.Add(
                "Crit Chance",
                string.Format(
                    "{0:0.00%}*{1:0.00%}\tBefore Procs",
                    CalcSpellCrit(),
                    StatUtils.CalcSpellCrit(PreProcStats)));

            dictValues.Add(
                "Average Haste",
                string.Format(
                    "{0:0.00}%*"
                        + "{1:0.00}s\tGlobal Cooldown\n"
                        + "{2:0.00}%\tBefore Procs",
                    (AvgHaste - 1) * 100f,
                    Math.Max(1.0f, 1.5f / AvgHaste),
                    (StatUtils.CalcSpellHaste(PreProcStats) - 1) * 100f));

            // Pet Stats
            if (Pet == null) {
                dictValues.Add("Pet Stamina", "-");
                dictValues.Add("Pet Intellect", "-");
                dictValues.Add("Pet Health", "-");
            } else {
                dictValues.Add(
                    "Pet Stamina",
                    string.Format("{0:0.0}", Pet.CalcStamina()));
                dictValues.Add(
                    "Pet Intellect",
                    string.Format("{0:0.0}", Pet.CalcIntellect()));
                dictValues.Add(
                    "Pet Health",
                    string.Format("{0:0.0}", Pet.CalcHealth()));
            }


            // Spell Stats
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

            if (Options.GetActiveRotation().GetError() != null) {
                return 0f;
            }

            CalcHasteAndManaProcs();
            AvgTimeUsed
                = Spell.GetTimeUsed(
                    CalculationsWarlock.AVG_UNHASTED_CAST_TIME,
                    0f,
                    Haste,
                    Options.Latency);

            float timeRemaining = Options.Duration;
            float manaRemaining = CalcUsableMana(timeRemaining);

            #region Calculate NumCasts for each spell
            Priorities = new List<Spell>();
            foreach (
                string spellName
                in Options.GetActiveRotation().GetPrioritiesForCalcs(Talents)) {

                Spell spell = GetSpell(spellName);
                if (spell.IsCastable()) {
                    Priorities.Add(spell);
                    CastSpells.Add(spellName, spell);
                }
            }
            Spell filler = GetSpell(Options.GetActiveRotation().Filler);
            RecordCollisionDelays(new CastingState(this, filler));
            foreach (Spell spell in Priorities) {
                float numCasts = spell.GetNumCasts();
                timeRemaining -= spell.GetAvgTimeUsed() * numCasts;
                manaRemaining -= spell.ManaCost * numCasts;
            }
            LifeTap lifeTap = (LifeTap) GetSpell("Life Tap");
            timeRemaining
                -= lifeTap.GetAvgTimeUsed()
                    * lifeTap.AddCastsForRegen(
                        timeRemaining, manaRemaining, filler);
            filler.Spam(timeRemaining);
            CastSpells.Add(Options.GetActiveRotation().Filler, filler);

            foreach (Spell spell in CastSpells.Values) {
                spell.AdjustAfterCastingIsSet();
            }
            #endregion

            #region Calculate spell modifiers

            // add procs to RawStats
            if (CastSpells.ContainsKey("Curse Of The Elements")) {

                // If the raid is already providing this debuff, the curse will
                // not actually end up casting, so this will not double-count
                // the debuff.
                Stats.BonusFireDamageMultiplier
                    = Stats.BonusShadowDamageMultiplier
                    = Stats.BonusHolyDamageMultiplier
                    = Stats.BonusFrostDamageMultiplier
                    = Stats.BonusNatureDamageMultiplier
                    = PetBuffs.BonusFireDamageMultiplier
                    = PetBuffs.BonusShadowDamageMultiplier
                    = PetBuffs.BonusHolyDamageMultiplier
                    = PetBuffs.BonusFrostDamageMultiplier
                    = PetBuffs.BonusNatureDamageMultiplier
                    = .13f;
            }
            if (Talents.ImprovedShadowBolt > 0
                && Stats.SpellCritOnTarget < .05f) {

                // TODO this should somehow affect Pyroclasm

                // If the 5% crit debuff is not already being maintained by
                // somebody else (i.e. it's not selected in the buffs tab), we
                // may supply it via Improved Shadow Bolt.
                float casts = 0f;
                if (CastSpells.ContainsKey("Shadow Bolt")) {
                    casts += CastSpells["Shadow Bolt"].GetNumCasts();
                }
                if (CastSpells.ContainsKey("Shadow Bolt (Instant)")) {
                    casts += CastSpells["Shadow Bolt (Instant)"].GetNumCasts();
                }
                float uprate = Spell.CalcUprate(
                    Talents.ImprovedShadowBolt * .2f, // proc rate
                    30f, // duration
                    Options.Duration / casts); // trigger period
                float benefit = .05f - Stats.SpellCritOnTarget;
                Stats.SpellCritOnTarget += benefit * uprate;
                PetBuffs.SpellCritOnTarget += benefit * uprate;
            }
            Stats.SpellPower += lifeTap.GetAvgBonusSpellPower();

            // create the SpellModifiers object
            SpellModifiers = new SpellModifiers();
            SpellModifiers.AddMultiplicativeMultiplier(
                Stats.BonusDamageMultiplier);
            SpellModifiers.AddMultiplicativeMultiplier(
                Talents.Malediction * .01f);
            SpellModifiers.AddMultiplicativeMultiplier(
                Talents.DemonicPact * .02f);
            SpellModifiers.AddCritChance(CalcSpellCrit());
            SpellModifiers.AddCritOverallMultiplier(
                Stats.BonusCritMultiplier);
            if (Talents.Metamorphosis > 0) {
                SpellModifiers.AddMultiplicativeMultiplier(
                    GetMetamorphosisBonus());
            }
            if (Pet is Felguard) {
                SpellModifiers.AddMultiplicativeMultiplier(
                    Talents.MasterDemonologist * .01f);
            }
            Add4pT10(SpellModifiers);

            if (Pet != null) {
                Pet.CalcStats1();
                Stats.SpellPower
                    += Talents.DemonicKnowledge
                        * .04f
                        * (Pet.CalcStamina() + Pet.CalcIntellect());
                float empower = Talents.EmpoweredImp / 3f;
                if (Pet is Imp && empower > 0) {
                    SpellModifiers.AddCritChance(
                        empower * AvgTimeUsed * Pet.GetCritsPerSec());
                }
                float pact = Pet.GetPactProcBenefit();
                Stats.SpellPower += pact;
                Pet.CalcStats2(pact);
            }

            // finilize each spell's modifiers.
            // Start with Conflagrate, since pyroclasm depends on its results.
            if (CastSpells.ContainsKey("Conflagrate")) {
                CastSpells["Conflagrate"].FinalizeSpellModifiers();
            }
            foreach (Spell spell in CastSpells.Values) {
                if (!(spell is Conflagrate)) {
                    spell.FinalizeSpellModifiers();
                }
            }
            #endregion

            float damageDone = CalcRemainingProcs();

            #region Calculate damage done for each spell
            Spell conflagrate = null;
            float spellPower = CalcSpellPower();
            foreach (KeyValuePair<string, Spell> pair in CastSpells) {
                Spell spell = pair.Value;
                if (pair.Key.Equals("Conflagrate")) {
                    conflagrate = spell;
                    continue; // save until we're sure immolate is done
                }
                spell.SetDamageStats(spellPower);
                damageDone += spell.GetNumCasts() * spell.AvgDamagePerCast;
            }
            if (conflagrate != null) {
                conflagrate.SetDamageStats(spellPower);
                damageDone
                    += conflagrate.GetNumCasts() * conflagrate.AvgDamagePerCast;
            }
            #endregion

            return damageDone / Options.Duration;
        }

        private float CalcPetDps() {

            if (Pet == null) {
                return 0f;
            } else {
                return Pet.CalcMeleeDps() + Pet.CalcSpecialDps();
            }
        }

        private void CalcHasteAndManaProcs() {

            float nonProcHaste = StatUtils.CalcSpellHaste(PreProcStats);
            if (Options.NoProcs) {
                WeightedStat staticHaste = new WeightedStat();
                staticHaste.Chance = 1f;
                staticHaste.Value = nonProcHaste;
                Haste = new List<WeightedStat> { staticHaste };
                AvgHaste = nonProcHaste;
                return;
            }

            // the trigger rates are all guestimates at this point, since the
            // real values depend on haste (which obviously has not been
            // finalized yet)
            Dictionary<int, float> periods
                = new Dictionary<int, float>();
            Dictionary<int, float> chances
                = new Dictionary<int, float>();
            float corruptionPeriod = 0f;
            if (Options.GetActiveRotation().Contains("Corruption")) {
                corruptionPeriod = 3.1f;
                if (Talents.GlyphQuickDecay) {
                    corruptionPeriod /= nonProcHaste;
                }
            }
            PopulateTriggers(
                periods,
                chances,
                CalculationsWarlock.AVG_UNHASTED_CAST_TIME / nonProcHaste
                    + Options.Latency,
                1 / 1.5f,
                corruptionPeriod,
                1f);

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
                if (!periods.ContainsKey((int) effect.Trigger)) {
                    continue;
                }

                if (effect.Stats.HasteRating > 0) {
                    hasteRatingEffects.Add(effect);
                    hasteRatingIntervals.Add(periods[(int) effect.Trigger]);
                    hasteRatingChances.Add(chances[(int) effect.Trigger]);
                    if (IsDoublePot(effect)) {
                        hasteRatingOffsets.Add(.75f * Options.Duration);
                    } else {
                        hasteRatingOffsets.Add(0f);
                    }
                    hasteRatingScales.Add(1f);
                    hasteRatingValues.Add(effect.Stats.HasteRating);
                }
                if (effect.Stats.SpellHaste > 0) {
                    hasteEffects.Add(effect);
                    hasteIntervals.Add(periods[(int) effect.Trigger]);
                    hasteChances.Add(chances[(int) effect.Trigger]);
                    hasteOffsets.Add(0f);
                    hasteScales.Add(1f);
                    hasteValues.Add(effect.Stats.SpellHaste);
                }
            }
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
                = SpecialEffect
                        .GetAverageCombinedUptimeCombinationsMultiplicative(
                    hasteEffects.ToArray(),
                    hasteIntervals.ToArray(),
                    hasteChances.ToArray(),
                    hasteOffsets.ToArray(),
                    hasteScales.ToArray(),
                    CalculationsWarlock.AVG_UNHASTED_CAST_TIME,
                    Options.Duration,
                    hasteValues.ToArray());
            Haste = new List<WeightedStat>();
            for (int p = percentages.Length, f = 0; --p >= 0; ) {
                if (percentages[p].Chance == 0) {
                    continue;
                }
                for (int r = ratings.Length; --r >= 0; ++f) {
                    if (ratings[r].Chance == 0) {
                        continue;
                    }
                    WeightedStat s = new WeightedStat();
                    s.Chance = percentages[p].Chance * ratings[r].Chance;
                    s.Value
                        = (1 + percentages[p].Value)
                            * (1 + StatConversion.GetSpellHasteFromRating(
                                    ratings[r].Value + Stats.HasteRating))
                            * (1 + Stats.SpellHaste);
                    Haste.Add(s);
                    AvgHaste += s.Chance * s.Value;
                }
            }
        }

        private float CalcRemainingProcs() {

            if (Options.NoProcs) {
                return 0f;
            }

            Dictionary<int, float> periods
                = new Dictionary<int, float>();
            Dictionary<int, float> chances
                = new Dictionary<int, float>();
            float totalCasts = 0f;
            float totalTicks = 0f;
            float corruptionTicks = 0f;
            SimulatedStat castsPerCrittable = new SimulatedStat();
            foreach (Spell spell in CastSpells.Values) {
                if (spell.BaseDamage == 0 && spell.BaseTickDamage == 0) {
                    continue;
                }

                float numCasts = spell.GetNumCasts();
                float numTicks = HitChance * numCasts * spell.NumTicks;
                totalCasts += numCasts;
                totalTicks += numTicks;

                float numCrittables = 0f;
                if (spell.BaseDamage > 0) {
                    numCrittables += HitChance * numCasts;
                }
                if (spell.BaseTickDamage > 0 && spell.CanTickCrit) {
                    numCrittables += numTicks;
                }
                castsPerCrittable.AddSample(
                    numCrittables == 0 ? 0f : numCasts / numCrittables,
                    numCasts);

                if (spell is Corruption) {
                    corruptionTicks += numTicks;
                }
            }
            PopulateTriggers(
                periods,
                chances,
                Options.Duration / totalCasts,
                totalTicks / Options.Duration,
                corruptionTicks == 0 ? -1 : Options.Duration / corruptionTicks,
                castsPerCrittable.GetValue());

            float procdDamage = 0f;
            Stats procStats = new Stats();
            foreach (SpecialEffect effect in Stats.SpecialEffects()) {
                if (!periods.ContainsKey((int) effect.Trigger)) {
                    continue;
                }

                float interval = periods[(int) effect.Trigger];
                float chance = chances[(int) effect.Trigger];

                Stats effectStats = effect.Stats;
                if (effectStats.ValkyrDamage > 0) {
                    SpellModifiers mods = new SpellModifiers();
                    mods.AddCritChance(.05f + Stats.SpellCritOnTarget);
                    mods.AddMultiplicativeMultiplier(
                        Stats.BonusHolyDamageMultiplier);
                    procdDamage
                        += CalcDamageProc(
                            effect,
                            effect.Stats.ValkyrDamage,
                            periods[(int) Trigger.DamageDone],
                            chance,
                            mods);
                } else if (
                    effectStats.ShadowDamage > 0
                        || effectStats.FireDamage > 0
                        || effectStats.NatureDamage > 0
                        || effectStats.HolyDamage > 0
                        || effectStats.FrostDamage > 0) {
                    SpellModifiers mods = new SpellModifiers();
                    mods.Accumulate(SpellModifiers);
                    if (Options.Imbue.Equals("Grand Firestone")) {
                        mods.AddAdditiveDirectMultiplier(.01f);
                    }
                    if (effectStats.ShadowDamage > 0) {
                        AddShadowModifiers(mods);
                    } else if (effectStats.FireDamage > 0) {
                        AddFireModifiers(mods);
                    }
                    procdDamage
                        += CalcDamageProc(
                            effect,
                            effectStats.ShadowDamage
                                + effectStats.FireDamage
                                + effectStats.NatureDamage
                                + effectStats.HolyDamage
                                + effectStats.FrostDamage,
                            interval,
                            chance,
                            mods);
                } else {
                    Stats proc = effect.GetAverageStats(
                        interval,
                        chance,
                        CalculationsWarlock.AVG_UNHASTED_CAST_TIME,
                        Options.Duration);

                    // Handle "recursive effects" - i.e. those that *enable* a
                    // proc during a short window.
                    if (effect.Stats._rawSpecialEffectDataSize == 1
                        && periods.ContainsKey(
                            (int) effect.Stats._rawSpecialEffectData[0].Trigger)) {

                        SpecialEffect inner
                            = effect.Stats._rawSpecialEffectData[0];
                        Stats innerStats
                            = inner.GetAverageStats(
                                periods[(int) inner.Trigger],
                                chances[(int) inner.Trigger],
                                1f,
                                effect.Duration);
                        float upTime
                            = effect.GetAverageUptime(
                                periods[(int) effect.Trigger],
                                chances[(int) effect.Trigger],
                                1f,
                                Options.Duration);
                        proc.Accumulate(innerStats, upTime);
                    }

                    procStats.Accumulate(proc);
                    if (effect.Trigger == Trigger.Use && !IsDoublePot(effect)) {
                        ExtraCritAtMax
                            += StatUtils.CalcSpellCrit(effect.Stats)
                                - StatUtils.CalcSpellCrit(proc);
                    }
                }
            }

            procStats.HasteRating
                = procStats.SpellHaste
                = procStats.Mana
                = procStats.ManaCostPerc
                = procStats.ManacostReduceWithin15OnHealingCast
                = procStats.ManaGainOnGreaterHealOverheal
                = procStats.ManaorEquivRestore
                = procStats.ManaRestore
                = procStats.ManaRestoreFromBaseManaPPM
                = procStats.ManaRestoreFromMaxManaPerSecond
                = procStats.ManaRestoreOnCast_5_15
                = procStats.ManaSpringMp5Increase
                = 0;
            Stats.Accumulate(procStats);

            return procdDamage;
        }

        private float CalcDamageProc(
            SpecialEffect effect,
            float damagePerProc,
            float interval,
            float chance,
            SpellModifiers modifiers) {

            damagePerProc *=
                (1 + (modifiers.GetFinalCritMultiplier() - 1)
                        * modifiers.CritChance)
                    * modifiers.GetFinalDirectMultiplier()
                    * (1
                        - StatConversion.GetAverageResistance(
                            80, Options.TargetLevel, 0f, 0f));
            float numProcs
                = Options.Duration
                    * effect.GetAverageProcsPerSecond(
                        interval,
                        chance,
                        CalculationsWarlock.AVG_UNHASTED_CAST_TIME,
                        Options.Duration);
            return numProcs * damagePerProc;
        }

        private bool IsDoublePot(SpecialEffect effect) {

            return effect.Cooldown == 1200f && effect.Duration == 14f;
        }

        /// <param name="castPeriod">
        /// SHOULD include casts that miss
        /// </param>
        /// <param name="dotFrequency">
        /// Should NOT include casts that miss
        /// </param>
        /// <param name="corruptionPeriod">
        /// Should NOT include casts that miss
        /// </param>
        /// <param name="castsPerCrittable">
        /// SHOULD include casts that miss
        /// </param>
        private void PopulateTriggers(
            Dictionary<int, float> periods,
            Dictionary<int, float> chances,
            float castPeriod,
            float dotFrequency,
            float corruptionPeriod,
            float castsPerCrittable) {

            periods[(int) Trigger.Use] = 0f;
            periods[(int) Trigger.SpellHit]
                = periods[(int) Trigger.SpellCast]
                = periods[(int) Trigger.SpellMiss]
                = periods[(int) Trigger.DamageSpellHit]
                = periods[(int) Trigger.DamageSpellCast]
                = castPeriod;
            periods[(int) Trigger.SpellCrit]
                = periods[(int) Trigger.DamageSpellCrit]
                = castPeriod * castsPerCrittable;
            periods[(int) Trigger.DoTTick] = 1 / dotFrequency;
            periods[(int) Trigger.DamageDone]
                = periods[(int) Trigger.DamageOrHealingDone]
                = 1f / (dotFrequency + 1f / castPeriod);
            periods[(int) Trigger.CorruptionTick] = corruptionPeriod;

            chances[(int) Trigger.Use] = 1f;
            chances[(int) Trigger.SpellHit]
                = chances[(int) Trigger.DamageSpellHit]
                = chances[(int) Trigger.DamageDone]
                = chances[(int) Trigger.DamageOrHealingDone]
                = HitChance;
            chances[(int) Trigger.SpellCrit]
                = chances[(int) Trigger.DamageSpellCrit]
                = CalcSpellCrit();
            chances[(int) Trigger.SpellCast]
                = chances[(int) Trigger.DamageSpellCast]
                = 1f;
            chances[(int) Trigger.SpellMiss]
                = 1 - HitChance;
            chances[(int) Trigger.DoTTick] = 1f;
            chances[(int) Trigger.CorruptionTick]
                = corruptionPeriod == 0f ? 0f : 1f;
        }

        // This technique assumes that if you pick a random time during filler
        // spell(s) or downtime, the "cooldowns" remaining on the rest of your
        // spells are all equally likely to be at any value. This is unrealistic
        // (e.g. it's impossible for them all to be at their full value), but
        // for some classes is a reasonable approximation.
        private void RecordCollisionDelays(CastingState state) {

            float pRemaining = 1f;
            foreach (Spell spell in Priorities) {
                float p = spell.GetQueueProbability(state);
                if (p == 0f) {
                    continue;
                }

                List<CastingState> nextStates =
                    spell.SimulateCast(state, p * pRemaining);
                foreach (CastingState nextState in nextStates) {
                    if (nextState.Probability > .0001f) {

                        // Only calculate if the probabilty of the state is
                        // large enough to make any difference at all.
                        RecordCollisionDelays(nextState);
                    }
                }
                if (p == 1f) {
                    return;
                }

                pRemaining *= 1f - p;
            }

            //System.Console.WriteLine(state.ToString());
        }

        public float GetMetamorphosisBonus() {

            if (Talents.Metamorphosis == 0) {
                return 0;
            }

            float cooldown = 180f * (1f - Talents.Nemesis * .1f);
            float duration = 30f;
            if (Talents.GlyphMetamorphosis) {
                duration += 6f;
            }
            return .2f * duration / cooldown;
        }

        public void AddShadowModifiers(SpellModifiers modifiers) {

            modifiers.AddMultiplicativeMultiplier(
                Stats.BonusShadowDamageMultiplier);
            modifiers.AddAdditiveMultiplier(
                Talents.ShadowMastery * .03f);
            if (Options.GetActiveRotation().Contains("Shadow Bolt")
                || (Options.GetActiveRotation().Contains("Haunt")
                    && Talents.Haunt > 0)) {

                modifiers.AddMultiplicativeTickMultiplier(
                    Talents.ShadowEmbrace * .01f * 3f);
            }
            if (CastSpells.ContainsKey("Haunt")) {
                modifiers.AddMultiplicativeTickMultiplier(
                    ((Haunt) CastSpells["Haunt"]).GetAvgTickBonus());
            }
            if (Pet is Succubus) {
                float bonus = Talents.MasterDemonologist * .01f;
                modifiers.AddMultiplicativeMultiplier(bonus);
                modifiers.AddCritChance(bonus);
            }
        }

        public void AddFireModifiers(SpellModifiers modifiers) {

            modifiers.AddMultiplicativeMultiplier(
                Stats.BonusFireDamageMultiplier);
            modifiers.AddAdditiveMultiplier(Talents.Emberstorm * .03f);
            if (Pet is Imp) {
                float bonus = Talents.MasterDemonologist * .01f;
                modifiers.AddMultiplicativeMultiplier(bonus);
                modifiers.AddCritChance(bonus);
            }
        }

        public void Add4pT10(SpellModifiers modifiers) {

            if (Stats.Warlock4T10 > 0) {
                Spell trigger = null;
                if (CastSpells.ContainsKey("Immolate")) {
                    trigger = CastSpells["Immolate"];
                } else if (CastSpells.ContainsKey("Unstable Affliction")) {
                    trigger = CastSpells["Unstable Affliction"];
                }
                if (trigger != null) {
                    float numTicks
                        = HitChance * trigger.GetNumCasts() * trigger.NumTicks;
                    float uprate
                        = Spell.CalcUprate(
                            .15f, 10f, Options.Duration / numTicks);
                    modifiers.AddMultiplicativeMultiplier(.1f * uprate);
                }
            }
        }

        #endregion


        public Spell GetSpell(string spellName) {

            if (Spells.ContainsKey(spellName)) {
                return Spells[spellName];
            }

            string className = spellName.Replace(" ", "");
            className = className.Replace("(", "_");
            className = className.Replace(")", "");
            Type type = Type.GetType("Rawr.Warlock." + className);
            Spell spell
                = (Spell) Activator.CreateInstance(type, new object[] { this });
            Spells[spellName] = spell;
            return spell;
        }

        public bool IsPriorityOrdered(Spell s1, Spell s2) {

            int i1 = Priorities.IndexOf(s1);
            int i2 = Priorities.IndexOf(s2);
            return (i1 < i2 && i1 != -1) || (i1 != -1 && i2 == -1);
        }
    }
}
//3456789 223456789 323456789 423456789 523456789 623456789 723456789 8234567890
