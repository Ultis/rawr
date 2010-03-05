using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.WarlockTmp {

    /// <summary>
    /// Calculates a Warlock's DPS and Spell Stats.
    /// </summary>
    public class CharacterCalculationsWarlock : CharacterCalculationsBase {

        #region overridden properties

        private float _overallPoints = -1f;
        public override float OverallPoints {
            get {
                if (_overallPoints == -1f) {
                    _overallPoints = 0f;
                    foreach (float subPoint in SubPoints) {
                        _overallPoints += subPoint;
                    }
                }
                return _overallPoints;
            }
            set { }
        }

        private float[] _subPoints;
        public override float[] SubPoints {
            get {
                if (_subPoints == null) {
                    _subPoints = new float[] { GetPersonalDps(), GetPetDps() };
                }
                return _subPoints;
            }
            set { }
        }

        #endregion


        #region subclass specific properties

        public Character Character { get; private set; }
        public Stats Stats { get; private set; }
        public CalculationOptionsWarlock Options { get; private set; }
        public WarlockTalents Talents { get; private set; }

        private float PersonalDps = -1f;
        private float PetDps = -1f;

        public float BaseMana { get; private set; }
        public float BaseDirectDamageMultiplier { get; private set; }
        public float BaseTickDamageMultiplier { get; private set; }
        public float BaseCritChance { get; private set; }

        public Dictionary<string, Spell> CastSpells
            { get; private set; }
        public CorruptionSpell CorruptionStats { get; private set; }
        public LifeTapSpell LifeTapStats { get; private set; }
        public MetamorphosisSpell MetamorphosisStats { get; private set; }
        public ShadowBoltSpell ShadowBoltStats { get; private set; }
        public InstantShadowBoltSpell InstantShadowBoltSpell {
            get; private set; }

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
            CastSpells = new Dictionary<string, Spell>();
            float multiplier
                = 1f
                    + Talents.DemonicPact * .01f
                    + Talents.Malediction * .01f;
            BaseTickDamageMultiplier
                = multiplier + Stats.WarlockSpellstoneDotDamageMultiplier;
            BaseDirectDamageMultiplier
                = multiplier + Stats.WarlockFirestoneDirectDamageMultiplier;
            BaseCritChance = Stats.SpellCrit;
            float bonus = Stats.CritBonusDamage;

            // If the 5% crit debuff is not already being maintained by somebody
            // else (i.e. it's not selected in the buffs tab), we may supply it
            // via Improved Shadow Bolt.
            if (Talents.ImprovedShadowBolt > 0
                && Stats.SpellCritOnTarget == 0
                && Options.SpellPriority.Contains("Shadow Bolt")) {

                // TODO calculate uptime when less than 5 points are invested.
                BaseCritChance += .05f;
            }
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
            dictValues.Add(
                "Haste Rating",
                String.Format(
                    "{0:0.00}%"
                        + "*{1:0.00}%\tfrom {2} Haste rating\r\n"
                        + "{3:0.00}%\tfrom Buffs\r\n"
                        + "{4:0.00}s\tGlobal Cooldown",
                    Stats.SpellHaste * 100f,
                    StatConversion.GetSpellHasteFromRating(Stats.HasteRating)
                        * 100f,
                    Stats.HasteRating,
                    (Stats.SpellHaste
                            - StatConversion.GetSpellHasteFromRating(
                                Stats.HasteRating))
                        * 100f,
                    Math.Max(1.0f, 1.5f / (1 + Stats.SpellHaste))));
            #endregion Haste

            dictValues.Add("Corruption", GetCorruptionStats().GetToolTip());
            dictValues.Add("Life Tap", GetLifeTapStats().GetToolTip());
            dictValues.Add(
                "Metamorphosis", GetMetamorphosisStats().GetToolTip());
            dictValues.Add("Shadow Bolt", GetShadowBoltStats().GetToolTip());
            dictValues.Add(
                "Instant Shadow Bolt",
                GetInstantShadowBoltStats().GetToolTip());

            return dictValues;
        }
        #endregion


        #region dps calculations

        private float GetPersonalDps() {

            if (PersonalDps >= 0) {
                return PersonalDps;
            }

            // first run through the priorities and calculate how many times
            // each spell will be cast
            float timeRemaining = Options.Duration;
            float manaRemaining = Stats.Mana;
            foreach (string spellName in Options.SpellPriority) {
                Spell spell = GetSpell(spellName);
                if (!spell.IsCastable()) {
                    continue;
                }
                spell.SetCastingStats(
                    manaRemaining,
                    timeRemaining,
                    1f + Stats.SpellHaste);
                CastSpells.Add(spellName, spell);
                timeRemaining
                    -= (spell.AvgCastTime + Options.Latency) * spell.NumCasts;
                manaRemaining -= spell.ManaCost * spell.NumCasts;
                if (timeRemaining <= .0001) {
                    break;
                }
            }

            float hitChance
                = Math.Min(
                    1f, Options.GetBaseHitRate() / 100f + Stats.SpellHit);
            float spellPower = Stats.SpellPower;

            // then for each spell that is cast calculate its damage, and our
            // overall damage
            float damageDone = 0f;
            foreach (KeyValuePair<string, Spell> pair in CastSpells) {
                Spell spell = pair.Value;
                spell.SetDamageStats(spellPower, hitChance, CastSpells);
                damageDone += spell.NumCasts * spell.AvgDamagePerCast;
            }
            PersonalDps = damageDone / Options.Duration;
            return PersonalDps;
        }

        private float GetPetDps() {

            return 0f;
        }

        #endregion


        #region spell stats

        public Spell GetSpell(String spellName) {

            switch (spellName) {
                case "Corruption":
                    return GetCorruptionStats();
                case "Instant Shadow Bolt":
                    return GetInstantShadowBoltStats();
                case "Life Tap":
                    return GetLifeTapStats();
                case "Metamorphosis":
                    return GetMetamorphosisStats();
                case "Shadow Bolt":
                    return GetShadowBoltStats();
                default:
                    return null;
            }
        }

        public CorruptionSpell GetCorruptionStats() {

            if (CorruptionStats == null) {
                CorruptionStats
                    = new CorruptionSpell(
                        this, BaseTickDamageMultiplier, BaseCritChance);
            }
            return CorruptionStats;
        }

        public LifeTapSpell GetLifeTapStats() {

            if (LifeTapStats == null) {
                LifeTapStats
                    = new LifeTapSpell(this);
            }
            return LifeTapStats;
        }

        public MetamorphosisSpell GetMetamorphosisStats() {

            if (MetamorphosisStats == null) {
                MetamorphosisStats = new MetamorphosisSpell(this);
            }
            return MetamorphosisStats;
        }

        public ShadowBoltSpell GetShadowBoltStats() {

            if (ShadowBoltStats == null) {
                ShadowBoltStats = new ShadowBoltSpell(
                    this, BaseDirectDamageMultiplier, BaseCritChance);
            }
            return ShadowBoltStats;
        }

        public InstantShadowBoltSpell GetInstantShadowBoltStats() {

            if (InstantShadowBoltSpell == null) {
                InstantShadowBoltSpell
                    = new InstantShadowBoltSpell(
                        this, BaseDirectDamageMultiplier, BaseCritChance);
            }
            return InstantShadowBoltSpell;
        }

        #endregion
    }
}
//3456789 223456789 323456789 423456789 523456789 623456789 723456789 8234567890
