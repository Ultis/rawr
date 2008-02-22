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
                    "Mana Regeneration:OO5SR",
                    "Mana Regeneration:I5SR",
                    "Spell Rotation:Rotation Name",
                    "Spell Rotation:DPS",
                    "Spell Rotation:DPM",
                    "Spell Rotation:Time To OOM"
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

            float hitRatingDivisor = 1260.5f;

            float hasteDivisor = 1560.0f;

            float critBase = 0.0185f;
            float critIntDivisor = 7997f;
            float critRatingDivisor = 2206.6f;

            calcs.SpellCrit = critBase + stats.SpellCritRating / critRatingDivisor;
            calcs.SpellHit = stats.SpellHitRating / hitRatingDivisor;

            // All spells: Damage +((0.08/0.16/0.25) * Int)
            float lunarGuidancePercent = 0.0f;
            switch (int.Parse(character.CalculationOptions["LunarGuidance"]))
            {
                case 1:
                    lunarGuidancePercent = 0.08f;
                    break;
                case 2:
                    lunarGuidancePercent = 0.16f;
                    break;
                case 3:
                    lunarGuidancePercent = 0.25f;
                    break;
                default:
                    lunarGuidancePercent = 0.0f;
                    break;
            }
            calcs.ArcaneDamage = stats.SpellDamageRating + stats.SpellArcaneDamageRating + lunarGuidancePercent * stats.Intellect;
            calcs.NatureDamage = stats.SpellDamageRating + lunarGuidancePercent * stats.Intellect;

            calcs.Latency = float.Parse(character.CalculationOptions["Latency"]);
            calcs.FightLength = float.Parse(character.CalculationOptions["FightLength"]);

            // 2.3 spirit regen calculation for druids
            // The /2 is to convert from mana per tick (2s) to mana per second
            float spiritRegen = (stats.Spirit / 4.5f + 15f) / 2;
            calcs.ManaRegen = spiritRegen + stats.Mp5 / 5f;
            calcs.ManaRegen5SR = spiritRegen * stats.SpellCombatManaRegeneration + stats.Mp5 / 5f;

            // All spells: Hit% +(0.02 * Balance of Power)
            calcs.SpellHit += 0.02f * int.Parse(character.CalculationOptions["BalanceofPower"]);
            // All spells: Crit% + (0.01 * Natural Perfection)
            calcs.SpellCrit += 0.01f * int.Parse(character.CalculationOptions["NaturalPerfection"]);

            // Finally, add the int portion of spell crit into the equation
            calcs.SpellCrit += stats.Intellect / critIntDivisor;

            // Create the offensive spell group class
            MoonkinSpells spellList = new MoonkinSpells();
            // Add (possibly talented) +spelldmg
            // Starfire: Damage +(0.04 * Wrath of Cenarius)
            // Wrath: Damage +(0.02 * Wrath of Cenarius)
            // Wrath: Base spell coefficient 0.571
            spellList["Wrath"].damagePerHit += (0.571f + 0.02f * int.Parse(character.CalculationOptions["WrathofCenarius"])) * calcs.NatureDamage;
            // Starfire: Base spell coefficient 1.0
            spellList["Starfire"].damagePerHit += (1.0f + 0.04f * int.Parse(character.CalculationOptions["WrathofCenarius"])) * calcs.ArcaneDamage;
            // Moonfire Direct Damage: Base spell coefficient 0.15
            spellList["Moonfire"].damagePerHit += 0.15f * calcs.ArcaneDamage;
            // Moonfire DoT: Base spell coefficient 0.52 spread over all ticks
            spellList["Moonfire"].dotEffect.damagePerTick += (0.52f / spellList["Moonfire"].dotEffect.numTicks) * calcs.ArcaneDamage;
            // Insect Swarm DoT: Base spell coefficient 0.76 spread over all ticks
            spellList["Insect Swarm"].dotEffect.damagePerTick += (0.76f / spellList["Insect Swarm"].dotEffect.numTicks) * calcs.NatureDamage;

            // Add spell-specific damage
            // Starfire, Moonfire, Wrath: Damage +(0.02 * Moonfury)
            spellList["Wrath"].damagePerHit *= 1.0f + (0.02f * int.Parse(character.CalculationOptions["Moonfury"]));
            spellList["Moonfire"].damagePerHit *= 1.0f + (0.02f * int.Parse(character.CalculationOptions["Moonfury"]));
            spellList["Moonfire"].dotEffect.damagePerTick *= 1.0f + (0.02f * int.Parse(character.CalculationOptions["Moonfury"]));
            spellList["Starfire"].damagePerHit *= 1.0f + (0.02f * int.Parse(character.CalculationOptions["Moonfury"]));

            // TODO: Add spell damage from idols

            // Add spell-specific crit chance
            // Wrath, Starfire: Crit chance +(0.02 * Focused Starlight)
            spellList["Wrath"].extraCritChance += 0.02f * int.Parse(character.CalculationOptions["FocusedStarlight"]);
            spellList["Starfire"].extraCritChance += 0.02f * int.Parse(character.CalculationOptions["FocusedStarlight"]);
            // Moonfire: Damage, Crit chance +(0.05 * Imp Moonfire)
            spellList["Moonfire"].damagePerHit *= 1.0f + (0.05f * int.Parse(character.CalculationOptions["ImpMoonfire"]));
            spellList["Moonfire"].dotEffect.damagePerTick *= 1.0f + (0.05f * int.Parse(character.CalculationOptions["ImpMoonfire"]));
            spellList["Moonfire"].extraCritChance += 0.05f * int.Parse(character.CalculationOptions["ImpMoonfire"]);

            // Add spell-specific critical strike damage
            // Starfire, Moonfire, Wrath: Crit damage +(0.2 * Vengeance)
            spellList["Starfire"].critBonus += 0.2f * int.Parse(character.CalculationOptions["Vengeance"]);
            spellList["Moonfire"].critBonus += 0.2f * int.Parse(character.CalculationOptions["Vengeance"]);
            spellList["Wrath"].critBonus += 0.2f * int.Parse(character.CalculationOptions["Vengeance"]);

            // Reduce spell-specific mana costs
            // Starfire, Moonfire, Wrath: Mana cost -(0.03 * Moonglow)
            spellList["Starfire"].manaCost *= 1.0f - (0.03f * int.Parse(character.CalculationOptions["Moonglow"]));
            spellList["Moonfire"].manaCost *= 1.0f - (0.03f * int.Parse(character.CalculationOptions["Moonglow"]));
            spellList["Wrath"].manaCost *= 1.0f - (0.03f * int.Parse(character.CalculationOptions["Moonglow"]));

            // Reduce spell-specific cast times
            // Wrath, Starfire: Cast time -(0.1 * Starlight Wrath)
            spellList["Wrath"].castTime -= 0.1f * int.Parse(character.CalculationOptions["StarlightWrath"]);
            spellList["Starfire"].castTime -= 0.1f * int.Parse(character.CalculationOptions["StarlightWrath"]);

            // Haste and latency calculations
            foreach (KeyValuePair<string, Spell> pair in spellList)
            {
                pair.Value.castTime /= 1 + stats.SpellHasteRating / hasteDivisor;
                pair.Value.castTime += calcs.Latency;
            }

            // Get the optimal DPS rotation
            spellList.GetRotation(character, ref calcs);

            return calcs;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            // Start off with a slightly modified form of druid base character stats calculations
            Stats statsRace = character.Race == Character.CharacterRace.NightElf ?
                new Stats()
                {
                    Health = 3434f,
                    Mana = 2470f,
                    Stamina = 82f,
                    Agility = 75f,
                    Intellect = 120f,
                    Spirit = 133f
                } :
                new Stats()
                {
                    Health = 3434f,
                    Mana = 2470f,
                    Stamina = 85f,
                    Agility = 64.5f,
                    Intellect = 115f,
                    Spirit = 135f
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

            // Base stats: Intellect% +(0.04 * Heart of the Wild)
            statsTotal.Intellect *= 1 + 0.04f * int.Parse(character.CalculationOptions["HotW"]);
            // Base stats: Stam%, Int%, Spi%, Agi% +(0.01 * Survival of the Fittest)
            statsTotal.Intellect *= 1 + 0.01f * int.Parse(character.CalculationOptions["SotF"]);
            statsTotal.Stamina *= 1 + 0.01f * int.Parse(character.CalculationOptions["SotF"]);
            statsTotal.Agility *= 1 + 0.01f * int.Parse(character.CalculationOptions["SotF"]);
            statsTotal.Spirit *= 1 + 0.01f * int.Parse(character.CalculationOptions["SotF"]);
            // Base stats: Spirit% +(0.05 * Living Spirit)
            statsTotal.Spirit *= 1 + 0.05f * int.Parse(character.CalculationOptions["LivingSpirit"]);

            // Bonus multipliers
            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;
            statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;
            statsTotal.BonusIntellectMultiplier = ((1 + statsRace.BonusIntellectMultiplier) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier)) - 1;
            statsTotal.BonusSpiritMultiplier = ((1 + statsRace.BonusSpiritMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier)) - 1;

            // Derived stats: Health, mana pool, armor
            statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + (statsTotal.Stamina * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
            statsTotal.Mana = (float)Math.Round(statsRace.Mana + 15f * statsTotal.Intellect) - 380;
            statsTotal.Armor = (float)Math.Round(statsGearEnchantsBuffs.Armor + statsTotal.Agility * 2f);

            // Regen mechanic: mp5 +((0.1 * Intensity) * Spiritmp5())
            statsTotal.SpellCombatManaRegeneration += 0.1f * int.Parse(character.CalculationOptions["Intensity"]);
            // Regen mechanic: mp5 +(0.04/0.07/0.10) * Int)
            float dreamstatePercent = 0.0f;
            switch (int.Parse(character.CalculationOptions["Dreamstate"]))
            {
                case 1:
                    dreamstatePercent = 0.04f;
                    break;
                case 2:
                    dreamstatePercent = 0.07f;
                    break;
                case 3:
                    dreamstatePercent = 0.1f;
                    break;
                default:
                    dreamstatePercent = 0.0f;
                    break;
            }
            statsTotal.Mp5 += dreamstatePercent * statsTotal.Intellect;

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
                Agility = stats.Agility,
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
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                Mana = stats.Mana,
                SpellArcaneDamageRating = stats.SpellArcaneDamageRating,
                Armor = stats.Armor,
                SpellCombatManaRegeneration = stats.SpellCombatManaRegeneration
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return stats.ToString().Equals("") || (stats.Stamina + stats.Intellect + stats.Spirit + stats.Agility + stats.Health + stats.Mp5 + stats.SpellCritRating + stats.SpellDamageRating + stats.SpellHasteRating + stats.SpellHitRating + + stats.BonusAgilityMultiplier + stats.BonusIntellectMultiplier + stats.BonusSpellCritMultiplier + stats.BonusSpellPowerMultiplier + stats.BonusStaminaMultiplier + stats.BonusSpiritMultiplier + stats.SpellArcaneDamageRating + stats.Mana + stats.SpellCombatManaRegeneration) > 0;
        }
    }
}
