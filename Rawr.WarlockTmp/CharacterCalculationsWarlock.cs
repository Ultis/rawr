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

        private Character Character { get; set; }
        public Stats TotalStats { get; private set; }
        public CalculationOptionsWarlock Options { get; private set; }

        private float BaseMana;
        private float PersonalDps = -1f;
        private float PetDps = -1f;

        private WarlockSpell ShadowboltStats;

        #endregion


        #region constructors

        public CharacterCalculationsWarlock() { }

        public CharacterCalculationsWarlock(Character character, Stats stats) {

            Character = character;
            TotalStats = stats;
            Options = (CalculationOptionsWarlock) character.CalculationOptions;
            if (Options == null) {
                Options = new CalculationOptionsWarlock();
            }

            BaseMana = BaseStats.GetBaseStats(Character).Mana;
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

            #region Bonus Damage
            //pet scaling consts: http://www.wowwiki.com/Warlock_minions
            const float petInheritedAttackPowerPercentage = 0.57f;
            const float petInheritedSpellPowerPercentage = 0.15f;
            float firePower
                = TotalStats.SpellPower + TotalStats.SpellFireDamageRating;
            dictValues.Add(
                "Bonus Damage",
                String.Format(
                    "{0}*"
                        + "{1}\tShadow Damage\r\n"
                        + "{2}\tFire Damage\r\n"
                        + "\r\n"
                        + "Your Fire Damage increases your pet's Attack Power by {3} and Spell Damage by {4}.",
                    TotalStats.SpellPower,
                    TotalStats.SpellPower + TotalStats.SpellShadowDamageRating,
                    TotalStats.SpellPower + TotalStats.SpellFireDamageRating,
                    Math.Round(
                        firePower * petInheritedAttackPowerPercentage, 0),
                    Math.Round(
                        firePower * petInheritedSpellPowerPercentage, 0)));
            #endregion

            #region Hit Rating
            float onePercentOfHitRating
                = (1 / StatConversion.GetSpellHitFromRating(1));
            float hitFromRating
                = StatConversion.GetSpellHitFromRating(TotalStats.HitRating);
            float hitFromTalents
                = (Character.WarlockTalents.Suppression * 0.01f);
            float hitFromBuffs
                = (TotalStats.SpellHit - hitFromRating - hitFromTalents);
            float targetHit = Options.GetBaseHitRate() / 100f;
            float totalHit = targetHit + TotalStats.SpellHit;
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
                    TotalStats.HitRating,
                    totalHit,
                    missChance,
                    targetHit,
                    Options.TargetLevel,
                    hitFromRating,
                    TotalStats.HitRating,
                    hitFromTalents,
                    hitFromBuffs,
                    Math.Ceiling(
                        Math.Abs((totalHit - 1) * onePercentOfHitRating)),
                    (totalHit > 1) ? "above" : "below"));
            #endregion

            return dictValues;
        }
        #endregion


        #region dps calculations

        private float GetPersonalDps() {

            if (PersonalDps >= 0) {
                return PersonalDps;
            }

            float damageDone = 0f;
            float timeRemaining = Options.Duration;
            foreach (String spellName in Options.SpellPriority) {
                WarlockSpell spell = GetSpell(spellName);
                spell.SetNumCasts(
                    timeRemaining, Options.Duration, Options.Latency);
                damageDone += spell.NumCasts * spell.GetAvgDamagePerCast();
                timeRemaining -= spell.CastTime * spell.NumCasts;
            }

            PersonalDps = damageDone / Options.Duration;
            return PersonalDps;
        }

        private float GetPetDps() {

            return 0f;
        }

        #endregion


        #region spell stats

        private WarlockSpell GetSpell(String spellName) {

            switch (spellName) {
                case "Shadow Bolt":
                    return GetShadowboltStats();
                default:
                    return null;
            }
        }

        private WarlockSpell GetShadowboltStats() {

            if (ShadowboltStats != null) {
                return ShadowboltStats;
            }

            ShadowboltStats = new WarlockSpell(
                this,
                "Shadowbolt", // name
                .17f, // percent base mana
                BaseMana, // base mana
                1f, // cost multiplier
                3f, // cast time
                1f, // haste divisor
                0f); // crit chance
            ShadowboltStats.SetAverageDamageOnHit(
                TotalStats.SpellPower, // spell power
                690f, // low base
                770f, // high base
                .8571f, // coefficient
                1f, // multiplier
                1.5f); // bonus crit multiplier
            return ShadowboltStats;
        }

        #endregion
    }
}
//3456789 223456789 323456789 423456789 523456789 623456789 723456789 8234567890
