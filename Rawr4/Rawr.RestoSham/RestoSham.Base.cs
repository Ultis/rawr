using System.Collections.Generic;
#if RAWR3 || RAWR4
using System.Windows.Media;
#else
using System.Drawing;
#endif

namespace Rawr.RestoSham
{
    /// <summary>
    /// Contains various lists of relevant stats, armor types, etc
    /// </summary>
    public static class Relevants
    {
        public static string[] RelevantStats = { 
            "Stamina",
            "Intellect",
            "SpellPower",
            "CritRating",
            "HasteRating",
            "SpellCrit",
            "SpellHaste",
            "Mana",
            "Mp5",
            "Earthliving",
            "ManacostReduceWithin15OnHealingCast",
            "ManaRestoreFromMaxManaPerSecond",
            "Spirit",
            "WaterShieldIncrease",
            "CHHWHealIncrease",
            "CHCTDecrease",
            "RTCDDecrease",
            "ManaRestoreFromMaxManaPerSecond",
            "ManaRestore",
            "BonusCritHealMultiplier",
            "BonusHealingDoneMultiplier",
            "BonusManaMultiplier",
            "BonusIntellectMultiplier",
            "RestoSham2T9",
            "RestoSham4T9",
            "RestoSham2T10",
            "RestoSham4T10",
            "RestoShamRelicT9",
            "RestoShamRelicT10",
            "Healed",
            "Hp5",
            "MovementSpeed"
        };

        public static List<string> RelevantGlyphs = new List<string>(10) {
            "Glyph of Healing Wave",
            "Glyph of Water Shield",
            "Glyph of Chain Heal",
            "Glyph of Earth Shield",
            "Glyph of Lesser Healing Wave",
            "Glyph of Earthliving Weapon",
            "Glyph of Mana Tide Totem",
            "Glyph of Healing Stream Totem",
            "Glyph of Riptide"
        };

        public static List<ItemType> RelevantItemTypes = new List<ItemType>(new ItemType[] {
            ItemType.None,
            ItemType.Cloth,
            ItemType.Leather,
            ItemType.Mail,
            ItemType.Totem,
            ItemType.OneHandMace,
            ItemType.OneHandAxe,
            ItemType.Shield,
            ItemType.Staff,
            ItemType.FistWeapon,
            ItemType.Dagger }
        );
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
            "Basic Stats:TC Mana Restore",
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
            "Average Cast Times:Lightning Bolt*Telluric Currents mana restoration in Basic stats area.)",
        };

        public static string[] OptimizableCalculationLabels = new string[] {
            "Haste %",
            "Crit %",
        };

        public static Dictionary<string, Color> SubPointNameColors = new Dictionary<string, Color>()
        {
            {"Burst", Color.FromArgb(255, 255, 0, 0)},
            {"Sustained", Color.FromArgb(255, 0, 0, 255)},
            {"Survival", Color.FromArgb(255, 0, 128, 0)},
        };
    }
};
