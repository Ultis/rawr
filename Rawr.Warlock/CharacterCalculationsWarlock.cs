using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Rawr.Warlock {
    /// <summary>Calculates a Warlock's DPS and Spell Stats.</summary>
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
        public CalculationOptionsWarlock CalcOpts { get; private set; }
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

#if RAWR4
        private bool? _affliction = null;
        private bool? _demonology = null;
        private bool? _destruction = null;
        public bool Affliction { get { if (_affliction == null) { DetermineSpec(); } return (bool)_affliction; } }
        public bool Demonology { get { if (_demonology == null) { DetermineSpec(); } return (bool)_demonology; } }
        public bool Destruction { get { if (_destruction == null) { DetermineSpec(); } return (bool)_destruction; } }

        private void DetermineSpec() {
            if (Talents == null) {
                _affliction = false;
                _demonology = false;
                _destruction = false;
                return;
            }
            int afflictionCtr = 0;
            int demonologyCtr = 0;
            int destructionCtr = 0;
            int[] talentData = Talents.Data;

            for (int i = 0; i <= 17; i++) { afflictionCtr += talentData[i]; }
            for (int i = 18; i <= 36; i++) { demonologyCtr += talentData[i]; }
            for (int i = 37; i <= 55; i++) { destructionCtr += talentData[i]; }

            _affliction = (afflictionCtr > destructionCtr) && (afflictionCtr > demonologyCtr);
            _demonology = (demonologyCtr > destructionCtr) && (demonologyCtr > afflictionCtr);
            _destruction = (destructionCtr > afflictionCtr) && (destructionCtr > demonologyCtr);
        }
#endif

        #endregion


        #region constructors

        public CharacterCalculationsWarlock() { OverallPoints = 0f; SubPoints = new float[] { 0f, 0f, 0f }; }

        /// <param name="stats">
        /// This should already have buffStats factored in.
        /// </param>
        public CharacterCalculationsWarlock(Character character, Stats stats, Stats petBuffs) {
            Character = character;
            CalcOpts = character.CalculationOptions as CalculationOptionsWarlock;
            Talents = character.WarlockTalents;
            Stats = stats;
            PreProcStats = Stats.Clone();
            PetBuffs = petBuffs;
            BaseMana = BaseStats.GetBaseStats(character).Mana;
            Spells = new Dictionary<string, Spell>();
            CastSpells = new Dictionary<string, Spell>();
            HitChance = Math.Min(1f, CalcOpts.GetBaseHitRate() / 100f + CalcSpellHit());
#if !RAWR4
            if (!Options.Pet.Equals("None")
                && (Talents.SummonFelguard > 0
                    || !Options.Pet.Equals("Felguard"))) {

                Type type = Type.GetType("Rawr.Warlock." + Options.Pet);
                Pet = (Pet) Activator.CreateInstance(
                        type, new object[] { this });
            }
#else
            if (!CalcOpts.Pet.Equals("None") && (Demonology || !CalcOpts.Pet.Equals("Felguard"))) {
                Type type = Type.GetType("Rawr.Warlock." + CalcOpts.Pet);
                Pet = (Pet)Activator.CreateInstance(type, new object[] { this });
            }
#endif

            float personalDps = CalcPersonalDps();
            float petDps = CalcPetDps();
            float raidBuff = CalcRaidBuff();
            SubPoints = new float[] { personalDps, petDps, raidBuff };
            OverallPoints = personalDps + petDps + raidBuff;
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

        public float CalcSpellCrit() { 
#if !RAWR4
            return StatUtils.CalcSpellCrit(Stats); 
#else
            return StatUtils.CalcSpellCrit(Stats, CalcOpts.PlayerLevel); 
#endif
        }

        public float CalcSpellHit() { 
#if !RAWR4
            return StatUtils.CalcSpellHit(Stats); 
#else
            return StatUtils.CalcSpellHit(Stats, CalcOpts.PlayerLevel); 
#endif
        }

        public float CalcSpellPower() {
            
            return StatUtils.CalcSpellPower(Stats);
        }

        public float CalcSpellHaste() {
#if !RAWR4
            return StatUtils.CalcSpellHaste(Stats);
#else
            return StatUtils.CalcSpellHaste(Stats, CalcOpts.PlayerLevel);
#endif
        }

#if RAWR4
        public float CalcMastery() { return StatUtils.CalcMastery(Stats, CalcOpts.PlayerLevel); }
#endif
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
#if !RAWR4
            float hitFromTalents = Talents.Suppression * 0.01f;
#else
            float hitFromTalents = 0f;
#endif
            float hitFromBuffs
                = (CalcSpellHit() - hitFromRating - hitFromTalents);
            float targetHit = CalcOpts.GetBaseHitRate() / 100f;
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
                    CalcOpts.TargetLevel,
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
#if !RAWR4
                    StatUtils.CalcSpellCrit(PreProcStats)));
#else
                    StatUtils.CalcSpellCrit(PreProcStats, CalcOpts.PlayerLevel)));
#endif

            dictValues.Add(
                "Average Haste",
                string.Format(
                    "{0:0.00}%*"
                        + "{1:0.00}%\tfrom {2:0.0} Haste rating\r\n"
                        + "{3:0.00}%\tfrom Buffs\r\n"
                        + "{4:0.0}ish%\tfrom Procs\r\n"
                        + "\r\n"
                        + "{5:0.00}s\tGlobal Cooldown\r\n",
                    (AvgHaste - 1f) * 100f,
                    StatConversion.GetSpellHasteFromRating(Stats.HasteRating)
                        * 100f,
                    Stats.HasteRating,
                    Stats.SpellHaste * 100f,
#if !RAWR4
                    (AvgHaste - StatUtils.CalcSpellHaste(PreProcStats)) * 100f,
#else
                    (AvgHaste - StatUtils.CalcSpellHaste(PreProcStats, CalcOpts.PlayerLevel)) * 100f,
#endif
                    Math.Max(1.0f, 1.5f / AvgHaste)));

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
            
            // SP & Crit: lock before pet (both affected by procs)
            // Procs after crit

            if (CalcOpts.GetActiveRotation().GetError() != null) {
                return 0f;
            }

            CalcHasteAndManaProcs();
            AvgTimeUsed
                = Spell.GetTimeUsed(
                    CalculationsWarlock.AVG_UNHASTED_CAST_TIME,
                    0f,
                    Haste,
                    CalcOpts.Latency);

            float timeRemaining = CalcOpts.Duration;
            float totalMana = CalcUsableMana(timeRemaining);
            float maxMana = StatUtils.CalcMana(PreProcStats);
            float manaFromEffects = totalMana - maxMana;
            float manaUsed = 0f;

            #region Calculate NumCasts for each spell

            // execute stage collision delays
            Spell execute = null;
            float executePercent = GetExecutePercentage();
            string executeName = CalcOpts.GetActiveRotation().Execute;
            if (executePercent > 0) {
                execute = GetSpell(executeName);
                SetupSpells(true);
                RecordCollisionDelays(
                    new CastingState(this, execute, executePercent));
            }

            // normal collision delays
            Spell filler = GetSpell(CalcOpts.GetActiveRotation().Filler);
            SetupSpells(false);
            RecordCollisionDelays(new CastingState(this, filler, 1f - executePercent));

            // calc numcasts
            foreach (Spell spell in Priorities) {
                float numCasts = spell.GetNumCasts();
                timeRemaining -= numCasts * spell.GetAvgTimeUsed();
                manaUsed += numCasts * spell.ManaCost;
            }
            LifeTap lifeTap = (LifeTap) GetSpell("Life Tap");
            if (executePercent > 0) {
                float executeTime = executePercent * timeRemaining;
                float taps
                    = lifeTap.AddCastsForRegen(
                        timeRemaining * executePercent,
                        maxMana + (manaFromEffects - manaUsed) * executePercent,
                        execute);
                executeTime -= taps * lifeTap.GetAvgTimeUsed();
                manaUsed += taps * lifeTap.ManaCost;
                execute.Spam(executeTime);
                timeRemaining -= executeTime;
                manaUsed += execute.ManaCost * execute.GetNumCasts();
                CastSpells.Add(CalcOpts.GetActiveRotation().Execute, execute);
            }
            timeRemaining
                -= lifeTap.GetAvgTimeUsed()
                    * lifeTap.AddCastsForRegen(
                        timeRemaining, totalMana - manaUsed, filler);
            filler.Spam(timeRemaining);
            CastSpells.Add(CalcOpts.GetActiveRotation().Filler, filler);

            foreach (Spell spell in CastSpells.Values) {
                spell.AdjustAfterCastingIsSet();
            }

            #endregion

            #region Calculate spell modifiers, Part 1

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
            float critBuff = CalcAddedCritBuff();
            Stats.SpellCritOnTarget += critBuff;
            PetBuffs.SpellCritOnTarget += critBuff;
#if !RAWR4
            Stats.SpellPower += lifeTap.GetAvgBonusSpellPower();
#endif
            // create the SpellModifiers object
            SpellModifiers = new SpellModifiers();
            SpellModifiers.AddMultiplicativeMultiplier(
                Stats.BonusDamageMultiplier);
#if !RAWR4
            SpellModifiers.AddMultiplicativeMultiplier(
                Talents.Malediction * .01f);
#endif
            SpellModifiers.AddMultiplicativeMultiplier(
                Talents.DemonicPact * .02f);
            SpellModifiers.AddCritOverallMultiplier(
                Stats.BonusCritMultiplier);
            if (Talents.Metamorphosis > 0) {
                SpellModifiers.AddMultiplicativeMultiplier(
                    GetMetamorphosisBonus());
            }
#if !RAWR4
            if (Pet is Felguard) {
                SpellModifiers.AddMultiplicativeMultiplier(
                    Talents.MasterDemonologist * .01f);
            }
#endif
            Add4pT10(SpellModifiers);

            Stats critProcs = CalcCritProcs();
            Stats.CritRating += critProcs.CritRating;
            Stats.SpellCrit += critProcs.SpellCrit;
            Stats.SpellCritOnTarget += critProcs.SpellCritOnTarget;
            SpellModifiers.AddCritChance(CalcSpellCrit());

            if (Pet != null) {
                Pet.CalcStats1();
#if !RAWR4
                Stats.SpellPower
                    += Talents.DemonicKnowledge
                        * .04f
                        * (Pet.CalcStamina() + Pet.CalcIntellect());
#endif
            }

            #endregion

            float damageDone = CalcRemainingProcs();

            #region Calculate Spell Modifiers, Part 2

            if (Pet != null) {
                Pet.CalcStats2();
#if !RAWR4
                Stats.SpellPower += Pet.ApplyPactProcBenefit();
#endif
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

            return damageDone / CalcOpts.Duration;
        }

        private float CalcPetDps() {

            if (Pet == null) {
                return 0f;
            } else {
                return Pet.CalcMeleeDps() + Pet.CalcSpecialDps();
            }
        }

        private float CalcRaidBuff() {

            float raidBuff = 0f;

            float perSP = CalcOpts.PerSP;
            if (perSP > 0 && Pet != null) {
#if !RAWR4
                raidBuff += perSP * Pet.GetPactProcBenefit();
#endif
                if (CalcOpts.ConvertTotem) {
                    float curTotem
                        = StatUtils.GetActiveBuff(
                            Character.ActiveBuffs,
                            "Spell Power",
                            s => s.SpellPower);
                    if (curTotem == 144f || curTotem == 165f) {
                        raidBuff += CalcOpts.PerFlametongue;
                    }
                }
            }

            if (CastSpells.ContainsKey("Curse Of The Elements")) {
                raidBuff += CalcOpts.PerMagicBuff;
            }

            raidBuff += CalcOpts.PerCritBuff * (CalcAddedCritBuff() / .05f);
#if !RAWR4
            raidBuff
                += Options.PerInt
                    * CalculationsWarlock.CalcPetIntBuff(
                        Options.Pet, Talents, Character.ActiveBuffs);
            raidBuff
                += Options.PerSpi
                    * CalculationsWarlock.CalcPetSpiBuff(
                        Options.Pet, Talents, Character.ActiveBuffs);
#endif
            raidBuff
                += CalcOpts.PerHealth
                    * CalculationsWarlock.CalcPetHealthBuff(
                        CalcOpts.Pet, Talents, Character.ActiveBuffs);

            return raidBuff;
        }

        public float GetExecutePercentage() {

            string executeName = CalcOpts.GetActiveRotation().Execute;
#if !RAWR4
            if (executeName == null || executeName == "" || executeName.Equals("Drain Soul")) {
                return 0f;
            }
#else
            if (executeName == null || executeName == "") {
                return 0f;
            }
#endif

            Spell execute = GetSpell(executeName);
            if (!execute.IsCastable()) {
                return 0f;
            }

            if (execute is SoulFire) {
                return CalcOpts.ThirtyFive;
            } else {
                return CalcOpts.TwentyFive;
            }
        }

        private float CalcAddedCritBuff() {

            // If the 5% crit debuff is not already being maintained by
            // somebody else (i.e. it's not selected in the buffs tab), we
            // may supply it via Improved Shadow Bolt.
#if !RAWR4
            if (Talents.ImprovedShadowBolt == 0
#else
            if (Talents.ShadowAndFlame == 0
#endif
                || StatUtils.GetActiveBuff(
                        Character.ActiveBuffs,
                        "Spell Critical Strike Taken",
                        s => s.SpellCritOnTarget)
                    > 0) {

                return 0f;
            }

            float casts = 0f;
            if (CastSpells.ContainsKey("Shadow Bolt")) {
                casts += CastSpells["Shadow Bolt"].GetNumCasts();
            }
            if (CastSpells.ContainsKey("Shadow Bolt (Instant)")) {
                casts += CastSpells["Shadow Bolt (Instant)"].GetNumCasts();
            }
            if (casts == 0) {
                return 0f;
            }

            float uprate = Spell.CalcUprate(
#if !RAWR4
                Talents.ImprovedShadowBolt * .2f, // proc rate
#else
                Talents.ShadowAndFlame * .33f, // proc rate
#endif
                30f, // duration
                CalcOpts.Duration / casts); // trigger period
            float benefit = .05f - Stats.SpellCritOnTarget;
            return benefit * uprate;
        }

        private void CalcHasteAndManaProcs() {
#if !RAWR4
            float nonProcHaste = StatUtils.CalcSpellHaste(PreProcStats);
#else
            float nonProcHaste = StatUtils.CalcSpellHaste(PreProcStats, CalcOpts.PlayerLevel);
#endif
            if (CalcOpts.NoProcs) {
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
            if (CalcOpts.GetActiveRotation().Contains("Corruption")) {
                corruptionPeriod = 3.1f;
#if !RAWR4
                if (Talents.GlyphQuickDecay) {
                    corruptionPeriod /= nonProcHaste;
                }
#else
                corruptionPeriod /= nonProcHaste;
#endif
            }
            PopulateTriggers(
                periods,
                chances,
                CalculationsWarlock.AVG_UNHASTED_CAST_TIME / nonProcHaste
                    + CalcOpts.Latency,
                1 / 1.5f,
                corruptionPeriod,
                1f);

            // calculate the haste procs
            Haste = new List<WeightedStat>();
            WeightedStat[] percentages
                = GetUptimes(
                    Stats,
                    periods,
                    chances,
                    s => s.SpellHaste,
                    (a, b, c, d, e, f, g, h)
                        => SpecialEffect
                                .GetAverageCombinedUptimeCombinationsMultiplicative(
                            a, b, c, d, e, f, g, h));
            WeightedStat[] ratings
                = GetUptimes(
                    Stats,
                    periods,
                    chances,
                    s => s.HasteRating,
                    (a, b, c, d, e, f, g, h)
                        => SpecialEffect.GetAverageCombinedUptimeCombinations(
                            a, b, c, d, e, f, g, h));
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

            // calculate mana procs
            Stats procStats = new Stats();
            foreach (SpecialEffect effect in Stats.SpecialEffects()) {
                if (!periods.ContainsKey((int) effect.Trigger)) {
                    continue;
                }

                Stats proc = effect.GetAverageStats(
                    periods[(int) effect.Trigger],
                    chances[(int) effect.Trigger],
                    CalculationsWarlock.AVG_UNHASTED_CAST_TIME,
                    CalcOpts.Duration);
                if (proc.ManaRestore > 0) {
                    proc.ManaRestore *= CalcOpts.Duration;
                }
                procStats.Accumulate(proc);
            }
            Stats.Mana += procStats.Mana;
            Stats.ManaRestore += procStats.ManaRestore;
            Stats.ManaRestoreFromBaseManaPPM
                += procStats.ManaRestoreFromBaseManaPPM;
            Stats.ManaRestoreFromMaxManaPerSecond
                += procStats.ManaRestoreFromMaxManaPerSecond;
            Stats.Mp5 += procStats.Mp5;
        }

        public WeightedStat[] GetUptimes(
            Stats stats,
            Dictionary<int, float> periods,
            Dictionary<int, float> chances,
            StatExtractor statExtractor,
            UptimeCombiner uptimeCombiner) {

            List<SpecialEffect> hasteEffects = new List<SpecialEffect>();
            List<float> hasteIntervals = new List<float>();
            List<float> hasteChances = new List<float>();
            List<float> hasteOffsets = new List<float>();
            List<float> hasteScales = new List<float>();
            List<float> hasteValues = new List<float>();
            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (!periods.ContainsKey((int) effect.Trigger)) {
                    continue;
                }

                float value = statExtractor(effect.Stats);
                if (value > 0) {
                    hasteEffects.Add(effect);
                    hasteIntervals.Add(periods[(int) effect.Trigger]);
                    hasteChances.Add(chances[(int) effect.Trigger]);
                    hasteOffsets.Add(0f);
                    hasteScales.Add(1f);
                    hasteValues.Add(value);
                }
            }
            return uptimeCombiner(
                    hasteEffects.ToArray(),
                    hasteIntervals.ToArray(),
                    hasteChances.ToArray(),
                    hasteOffsets.ToArray(),
                    hasteScales.ToArray(),
                    CalculationsWarlock.AVG_UNHASTED_CAST_TIME,
                    CalcOpts.Duration,
                    hasteValues.ToArray());
        }

        private Stats CalcCritProcs() {

            if (CalcOpts.NoProcs) {
                return new Stats();
            }

            Dictionary<int, float> periods = new Dictionary<int, float>();
            Dictionary<int, float> chances = new Dictionary<int, float>();
            PopulateTriggers(periods, chances);

            Stats procStats = new Stats();
            foreach (SpecialEffect effect in Stats.SpecialEffects()) {
                if (!periods.ContainsKey((int) effect.Trigger)) {
                    continue;
                }

                Stats proc = CalcNormalProc(effect, periods, chances);
                procStats.Accumulate(proc);
                if (effect.Trigger == Trigger.Use && !IsDoublePot(effect)) {
                    ExtraCritAtMax
#if !RAWR4
                        += StatUtils.CalcSpellCrit(effect.Stats)
                            - StatUtils.CalcSpellCrit(proc);
#else
                        += StatUtils.CalcSpellCrit(effect.Stats, CalcOpts.PlayerLevel)
                            - StatUtils.CalcSpellCrit(proc, CalcOpts.PlayerLevel);
#endif
                }
            }
            return procStats;
        }

        private float CalcRemainingProcs() {

            if (CalcOpts.NoProcs) {
                return 0f;
            }

            Dictionary<int, float> periods = new Dictionary<int, float>();
            Dictionary<int, float> chances = new Dictionary<int, float>();
            PopulateTriggers(periods, chances);

            float procdDamage = 0f;
            Stats procStats = new Stats();
            foreach (SpecialEffect effect in Stats.SpecialEffects()) {
                if (!periods.ContainsKey((int) effect.Trigger)) {
                    continue;
                }

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
                            periods,
                            chances,
                            mods);
                } else if (
                    effectStats.ShadowDamage > 0
                        || effectStats.FireDamage > 0
                        || effectStats.NatureDamage > 0
                        || effectStats.HolyDamage > 0
                        || effectStats.FrostDamage > 0) {
                    SpellModifiers mods = new SpellModifiers();
                    mods.Accumulate(SpellModifiers);
#if !RAWR4
                    if (Options.Imbue.Equals("Grand Firestone")) {
                        mods.AddAdditiveDirectMultiplier(.01f);
                    }
#endif
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
                            periods,
                            chances,
                            mods);
                } else {
                    procStats.Accumulate(
                        CalcNormalProc(effect, periods, chances));
                }
            }

            procStats.HasteRating
                = procStats.SpellHaste
                = procStats.Mana
                = procStats.ManaRestore
                = procStats.ManaRestoreFromBaseManaPPM
                = procStats.ManaRestoreFromMaxManaPerSecond
                = procStats.Mp5
                = procStats.CritRating
                = procStats.SpellCrit
                = procStats.SpellCritOnTarget
                = procStats.PhysicalCrit
                = 0;
            Stats.Accumulate(procStats);

            return procdDamage;
        }

        private Stats CalcNormalProc(
            SpecialEffect effect,
            Dictionary<int, float> periods,
            Dictionary<int, float> chances) {

            Stats effectStats = effect.Stats;
            Stats proc = effect.GetAverageStats(
                periods[(int) effect.Trigger],
                chances[(int) effect.Trigger],
                CalculationsWarlock.AVG_UNHASTED_CAST_TIME,
                CalcOpts.Duration);

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
                        CalcOpts.Duration);
                proc.Accumulate(innerStats, upTime);
            }

            return proc;
        }

        private float CalcDamageProc(
            SpecialEffect effect,
            float damagePerProc,
            Dictionary<int, float> periods,
            Dictionary<int, float> chances,
            SpellModifiers modifiers) {

            damagePerProc *=
                modifiers.GetFinalDirectMultiplier()
                    * (1
                        + (modifiers.GetFinalCritMultiplier() - 1)
                            * modifiers.CritChance)
                    * (1
                        - StatConversion.GetAverageResistance(
                            80, CalcOpts.TargetLevel, 0f, 0f));
            float numProcs
                = CalcOpts.Duration
                    * effect.GetAverageProcsPerSecond(
                        periods[(int) effect.Trigger],
                        chances[(int) effect.Trigger],
                        CalculationsWarlock.AVG_UNHASTED_CAST_TIME,
                        CalcOpts.Duration);
            return numProcs * damagePerProc;
        }

        private bool IsDoublePot(SpecialEffect effect) {

            return effect.Cooldown == 1200f && effect.Duration == 14f;
        }

        /// <summary>To be used only after spell casting stats are set</summary>
        private void PopulateTriggers(
            Dictionary<int, float> periods,
            Dictionary<int, float> chances) {

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
                CalcOpts.Duration / totalCasts,
                totalTicks / CalcOpts.Duration,
                corruptionTicks == 0 ? -1 : CalcOpts.Duration / corruptionTicks,
                castsPerCrittable.GetValue());
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

        private void SetupSpells(bool execute) {

            Priorities = new List<Spell>();
            foreach (
                string spellName
                in CalcOpts.GetActiveRotation().GetPrioritiesForCalcs(
                    Talents, execute)) {

                Spell spell = GetSpell(spellName);
                if (spell.IsCastable()
                    && (!execute || spell.IsCastDuringExecute())) {

                    Priorities.Add(spell);
                    if (!CastSpells.ContainsKey(spellName)) {
                        CastSpells.Add(spellName, spell);
                    }
                }
            }
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

#if !RAWR4
            float cooldown = 180f * (1f - Talents.Nemesis * .1f);
#else
            float cooldown = 180f;
#endif
            float duration = 30f;
            if (Talents.GlyphMetamorphosis) {
                duration += 6f;
            }
#if !RAWR4
            return .2f * duration / cooldown;
#else
            float bonus = .2f; //base definition of Metamorphosis
            if (this.Demonology) 
            {
                //mastery bonus for Demonology
                bonus += .12f + .015f * CalcMastery();
            }

            return bonus * duration / cooldown;
#endif
        }

        public void AddShadowModifiers(SpellModifiers modifiers) {

            modifiers.AddMultiplicativeMultiplier(
                Stats.BonusShadowDamageMultiplier);
#if !RAWR4
            modifiers.AddAdditiveMultiplier(
                Talents.ShadowMastery * .03f);
#else
            modifiers.AddMultiplicativeMultiplier(
                Affliction ? .25f : 0f);
            modifiers.AddMultiplicativeMultiplier(
                Demonology ? .15f : 0f);
            if (Affliction) {
                modifiers.AddMultiplicativeTickMultiplier(
                    .1304f + CalcMastery() * .0163f);
            }
#endif
            if (CalcOpts.GetActiveRotation().Contains("Shadow Bolt")
                || (CalcOpts.GetActiveRotation().Contains("Haunt")
                    && Talents.Haunt > 0)) {
#if !RAWR4
                modifiers.AddMultiplicativeTickMultiplier(
                    Talents.ShadowEmbrace * .01f * 3f);
#else
                float[] talentEffects = { 0f, .03f, .04f, .05f };
                modifiers.AddMultiplicativeTickMultiplier(
                    talentEffects[Talents.ShadowEmbrace] * 3f);
#endif
            }
            if (CastSpells.ContainsKey("Haunt")) {
                modifiers.AddMultiplicativeTickMultiplier(
                    ((Haunt) CastSpells["Haunt"]).GetAvgTickBonus());
            }
#if !RAWR4
            if (Pet is Succubus) {
                float bonus = Talents.MasterDemonologist * .01f;
                modifiers.AddMultiplicativeMultiplier(bonus);
                modifiers.AddCritChance(bonus);
            }
#endif
        }

        public void AddFireModifiers(SpellModifiers modifiers) {

            modifiers.AddMultiplicativeMultiplier(
                Stats.BonusFireDamageMultiplier);
#if !RAWR4
            modifiers.AddAdditiveMultiplier(Talents.Emberstorm * .03f);
            if (Pet is Imp) {
                float bonus = Talents.MasterDemonologist * .01f;
                modifiers.AddMultiplicativeMultiplier(bonus);
                modifiers.AddCritChance(bonus);
            }
#else
            modifiers.AddMultiplicativeMultiplier(Demonology ? .15f : 0f);
            modifiers.AddMultiplicativeMultiplier(Destruction ? .25f : 0f);
            if (Destruction) {
                modifiers.AddMultiplicativeMultiplier(
                    .1f + .0125f * CalcMastery());
            }
#endif
        }

        public void Add4pT10(SpellModifiers modifiers) {

            if (Stats.Warlock4T10 == 0) {
                return;
            }

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
                        .15f, 10f, CalcOpts.Duration / numTicks);
                modifiers.AddMultiplicativeMultiplier(.1f * uprate);
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
            Spell spell = (Spell) Activator.CreateInstance(type, new object[] { this });
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
