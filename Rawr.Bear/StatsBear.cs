using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Bear
{
    /// <summary>
    /// Bear custom implementation of the Stats object to expand it with Bear Specific variables
    /// </summary>
    public class StatsBear : Stats
    {
        /// <summary>
        /// Percentage mod for Mangle. Use as 1.05 = 105% = +5%
        /// </summary>
        public float BonusMangleDamageMultiplier { get; set; }
        /// <summary>
        /// Percentage mod for Maul. Use as 1.05 = 105% = +5%
        /// </summary>
        public float BonusMaulDamageMultiplier { get; set; }
        /// <summary>
        /// Percentage mod for Enrage. Use as 1.05 = 105% = +5%
        /// </summary>
        public float BonusEnrageDamageMultiplier { get; set; }
        /// <summary>
        /// Mod for Mangle Cooldown. Use as 1 = 1 sec reduction
        /// </summary>
        public float MangleCooldownReduction { get; set; }
        /// <summary>
        /// Percentage mod for Swipe. Use as 1.05 = 105% = +5%
        /// </summary>
        public float BonusSwipeDamageMultiplier { get; set; }
        /// <summary>
        /// Percentage value for chance to proc Fury Swipes. Use as 0.05 = 005% = +5%
        /// </summary>
        public float FurySwipesChance { get; set; }
        /// <summary>
        /// Percentage value for Haste. Use as 0.05 = 005% = +5%
        /// </summary>
        public float HasteOnFeralCharge { get; set; }
        /// <summary>
        /// Value mod for Pulverize. Use as 1 = 1 sec bonus
        /// </summary>
        public float BonusPulverizeDuration { get; set; }
        /// <summary>
        /// Percentage mod for Lacerate's Chance to Crit. Use as 0.05 = 005% = +5%
        /// </summary>
        public float BonusLacerateCritChance { get; set; }
        /// <summary>
        /// Value mod for Mangle. Use as 1 = 1 bonus stack
        /// </summary>
        public float BonusFaerieFireStacks { get; set; }
        /// <summary>
        /// Percentage mod for Survival Instincts Duration. Use as 0.05 = 005% = +5%
        /// </summary>
        public float BonusSurvivalInstinctsDurationMultiplier { get; set; }
        /// <summary>
        /// Contains the individual values for the uptimes of Armor
        /// </summary>
        public WeightedStat[] TemporaryArmorUptimes { get; set; }
    }
}
