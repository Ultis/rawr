using System.Collections.Generic;
using System.Windows.Media;
using System;

namespace Rawr.RestoSham
{
    /// <summary>
    /// Contains various lists of relevant stats, armor types, etc
    /// </summary>
    public static class Relevants
    {
        /// <summary>
        /// List of <see cref="ItemType"/> values that are relevant to this model.
        /// </summary>
        public static List<ItemType> RelevantItemTypes = new List<ItemType>()
        {
            ItemType.None,
            ItemType.Cloth,
            ItemType.Leather,
            ItemType.Mail,
            ItemType.Relic,
            ItemType.OneHandMace,
            ItemType.OneHandAxe,
            ItemType.Shield,
            ItemType.Staff,
            ItemType.FistWeapon,
            ItemType.Dagger
        };

        /// <summary>
        /// List of <see cref="String"/> values that represent the Glyphs relevant to this model.
        /// </summary>
        public static List<String> RelevantGlyphs = new List<String>()
        {
            // Prime glyphs
            "Glyph of Water Shield",
            "Glyph of Earth Shield",
            "Glyph of Earthliving Weapon",
            "Glyph of Riptide",
            "Glyph of Shocking",
            "Glyph of Lightning Bolt",
            // Major glyphs
            "Glyph of Healing Wave",
            "Glyph of Chain Heal",
            "Glyph of Healing Stream Totem",
            "Glyph of Totemic Recall",
            "Glyph of Hex"
        };

        /// <summary>
        /// A <see cref="List"/> of <see cref="Trigger"/> objects that represent the relevant event triggers.
        /// </summary>
        public static List<Trigger> RelevantTriggers = new List<Trigger>()
        {
            Trigger.HealingSpellCast,
            Trigger.HealingSpellCrit,
            Trigger.HealingSpellHit,
            Trigger.SpellCast,
            Trigger.SpellCrit,
            Trigger.SpellHit,
            Trigger.Use
        };
    }

    /// <summary>
    /// Contains various static lists for the RestoSham module
    /// </summary>
    public static class RestoShamConfiguration
    {
        /// <summary>
        /// The Character Calculations Display configuration
        /// </summary>
        public static string[] CharacterDisplayCalculationLabels = new string[] {
            "Totals:HPS - Burst",
            "Totals:HPS - Sustained",
            "Totals:Survival",
            "Basic Stats:Health",
            "Basic Stats:Mana",
            "Basic Stats:Stamina",
            "Basic Stats:Intellect",
            "Basic Stats:Spell Power",
            "Basic Stats:Deep Healing %",
            "Basic Stats:MP5*Mana regeneration while casting",
            "Basic Stats:Mana Available*This is your total mana from all sources throughout the entire fight.",
            "Basic Stats:Heal Spell Crit*This includes all static talents including those that are not shown on the in-game character pane",
            "Basic Stats:Spell Haste",
            "Basic Stats:TC Mana Restore*Mana from Telluric Currents.",
            "Basic Stats:Mail Specialization*Increases your Intellect by 5% while wearing Mail in all armor slots.",
            "Healing Style Breakdowns:Burst Sequence",
            "Healing Style Breakdowns:Sustained Sequence",
            "Healing Style Breakdowns:Mana Available per Second",
            "Healing Style Breakdowns:Mana Used per Second*This is the mana used per second by your chosen sustained sequence",
            "Healing Style Breakdowns:Healing Stream HPS",
            "Healing Style Breakdowns:Earth Shield HPS",
            "Healing Style Breakdowns:RT+HW HPS",
            "Healing Style Breakdowns:RT+GHW HPS",
            "Healing Style Breakdowns:RT+HSrg HPS",
            "Healing Style Breakdowns:RT+CH HPS",
            "Healing Style Breakdowns:HW Spam HPS",
            "Healing Style Breakdowns:GHW Spam HPS",
            "Healing Style Breakdowns:HS Spam HPS",
            "Healing Style Breakdowns:CH Spam HPS",
            "Average Cast Times:Global Cooldown",
            "Average Cast Times:Healing Wave*Normal / Tidal Waves",
            "Average Cast Times:Greater Healing Wave*Normal / Tidal Waves",
            "Average Cast Times:Healing Surge",
            "Average Cast Times:Chain Heal",
            "Average Cast Times:Lightning Bolt*Telluric Currents mana restoration in Basic stats area.",
            "Test Area: Damage Test",
            "Test Area: Damage Test 2",
        };

        /// <summary>
        /// List of the optimizable stat calculations.
        /// </summary>
        public static string[] OptimizableCalculationLabels = new string[] {
            "Haste %",
            "Crit %",
        };

        /// <summary>
        /// List of chart series.
        /// </summary>
        public static Dictionary<string, Color> SubPointNameColors = new Dictionary<string, Color>()
        {
            {"Burst", Color.FromArgb(255, 255, 0, 0)},
            {"Sustained", Color.FromArgb(255, 0, 0, 255)},
            {"Survival", Color.FromArgb(255, 0, 128, 0)},
        };
    }
};
