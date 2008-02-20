using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Moonkin
{
    [System.ComponentModel.DisplayName("Moonkin")]
    class CalculationsMoonkin : CalculationsBase
    {
        private Dictionary<string, System.Drawing.Color> subColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (subColors == null)
                {
                    subColors = new Dictionary<string, System.Drawing.Color>();
                    subColors.Add("Damage", System.Drawing.Color.Red);
                    subColors.Add("Efficiency", System.Drawing.Color.Blue);
                }
                return subColors;
            }
        }

        private string[] characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (characterDisplayCalculationLabels == null)
                {
                    characterDisplayCalculationLabels = new string[] {
					"Basic Stats:Health",
					"Basic Stats:Mana",
					"Basic Stats:Armor",
                    "Basic Stats:Agility",
					"Basic Stats:Stamina",
					"Basic Stats:Intellect",
					"Basic Stats:Spirit",
                    "Spell Stats:Spell Hit",
                    "Spell Stats:Spell Crit",
                    "Spell Stats:Spell Haste",
                    "Spell Stats:Arcane Damage",
                    "Spell Stats:Nature Damage",
                    "Regeneration:OO5SR MP5",
                    "Regeneration:I5SR MP5",
                    };
                }
                return characterDisplayCalculationLabels;
            }
        }

        private string[] customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (customChartNames == null)
                {
                    customChartNames = new string[] { };
                }
                return customChartNames;
            }
        }

        private CalculationOptionsPanelBase calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (calculationOptionsPanel == null)
                {
                    calculationOptionsPanel = new CalculationOptionsPanelMoonkin();
                }
                return calculationOptionsPanel;
            }
        }

        private List<Item.ItemType> relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get
            {
                if (relevantItemTypes == null)
                {
                    relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
                        {
                            Item.ItemType.None,
                            Item.ItemType.Cloth,
                            Item.ItemType.Leather,
                            Item.ItemType.Dagger,
                            Item.ItemType.Staff,
                            Item.ItemType.OneHandMace,
                            Item.ItemType.TwoHandMace,
                            Item.ItemType.FistWeapon,
                            Item.ItemType.Idol,
                        });
                }
                return relevantItemTypes;
            }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationMoonkin();
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsMoonkin();
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            CharacterCalculationsMoonkin calcs = new CharacterCalculationsMoonkin();
            Stats stats = GetCharacterStats(character, additionalItem);
            calcs.BasicStats = stats;
            //calcs.C
            calcs.SubPoints[0] = 0.0f;
            calcs.SubPoints[1] = 0.0f;
            calcs.OverallPoints = calcs.SubPoints[0] + calcs.SubPoints[1];
            return calcs;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            // Start off with a slightly modified form of druid base character stats calculations
            Stats statsRace = character.Race == Character.CharacterRace.NightElf ?
                new Stats()
                {
                    Health = 3434f,
                    Mana = 2368f,
                    Stamina = 82f,
                    Agility = 75f,
                    Intellect = 148f,
                    Spirit = 136f,
                    BonusStaminaMultiplier = 0.03f
                } :
                new Stats()
                {
                    Health = 3434f,
                    Mana = 2368f,
                    Stamina = 85f,
                    Agility = 64.5f,
                    Intellect = 115f,
                    Spirit = 155f,
                    BonusStaminaMultiplier = 0.03f
                };

            // Get the gear/enchants/buffs stats loaded in
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;

            // Create the total stats object
            Stats statsTotal = statsGearEnchantsBuffs + statsRace;

            // Base stats: Intellect, Stamina, Spirit, Agility
            statsTotal.Intellect = (float)Math.Floor((Math.Floor(statsRace.Intellect * (1 + statsRace.BonusIntellectMultiplier)) + statsGearEnchantsBuffs.Intellect * (1 + statsRace.BonusIntellectMultiplier)) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier));
            statsTotal.Stamina = (float)Math.Floor((Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier)) + statsGearEnchantsBuffs.Stamina * (1 + statsRace.BonusStaminaMultiplier)) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier));
            statsTotal.Agility = (float)Math.Floor((Math.Floor(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier)) + statsGearEnchantsBuffs.Agility * (1 + statsRace.BonusAgilityMultiplier)) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier));
            statsTotal.Spirit = (float)Math.Floor((Math.Floor(statsRace.Spirit * (1 + statsRace.BonusSpiritMultiplier)) + statsGearEnchantsBuffs.Spirit * (1 + statsRace.BonusSpiritMultiplier)) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier));

            // Bonus multipliers
            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;
            statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;
            statsTotal.BonusIntellectMultiplier = ((1 + statsRace.BonusIntellectMultiplier) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier)) - 1;
            statsTotal.BonusSpiritMultiplier = ((1 + statsRace.BonusSpiritMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier)) - 1;

            // Base stats: Health, mana pool, armor
            statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + (statsTotal.Stamina * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
            statsTotal.Mana = (float)Math.Round(statsRace.Mana + 15f * statsTotal.Intellect);
            statsTotal.Armor = (float)Math.Round(statsGearEnchantsBuffs.Armor + statsTotal.Agility * 2f);

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            throw new NotImplementedException();
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                Health = stats.Health,
                Mp5 = stats.Mp5,
                SpellCritRating = stats.SpellCritRating,
                SpellDamageRating = stats.SpellDamageRating,
                SpellHasteRating = stats.SpellHasteRating,
                SpellHitRating = stats.SpellHitRating,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                Mana = stats.Mana,
                SpellArcaneDamageRating = stats.SpellArcaneDamageRating,
                Armor = stats.Armor,
                SpellCombatManaRegeneration = stats.SpellCombatManaRegeneration
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return stats.ToString().Equals("") || (stats.Stamina + stats.Intellect + stats.Spirit + stats.Health + stats.Mp5 + stats.SpellCritRating + stats.SpellDamageRating + stats.SpellHasteRating + stats.SpellHitRating + stats.BonusIntellectMultiplier + stats.BonusSpellCritMultiplier + stats.BonusSpellPowerMultiplier + stats.BonusStaminaMultiplier + stats.BonusSpiritMultiplier + stats.SpellArcaneDamageRating + stats.Mana + stats.SpellCombatManaRegeneration) > 0;
        }
    }
}
