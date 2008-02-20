using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Mage
{
    [System.ComponentModel.DisplayName("Mage")]
    class CalculationsMage : CalculationsBase
    {
        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("Damage", System.Drawing.Color.FromArgb(160, 0, 224));
                }
                return _subPointNameColors;
            }
        }

        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[] {
					"Basic Stats:Stamina",
					"Basic Stats:Intellect",
					"Basic Stats:Spirit",
					"Basic Stats:Armor",
					"Basic Stats:Health",
					"Basic Stats:Mana",
                    "Spell Stats:Spell Crit Rate",
                    "Spell Stats:Spell Hit Rate",
                    "Spell Stats:Spell Penetration",
                    "Spell Stats:Casting Speed",
                    "Spell Stats:Arcane Damage",
                    "Spell Stats:Fire Damage",
                    "Spell Stats:Frost Damage",
                    "Regeneration:MP5",
                    "Regeneration:Mana Regen",
                    "Regeneration:Health Regen",
                    "Survivability:Arcane Resist",
                    "Survivability:Fire Resist",
                    "Survivability:Nature Resist",
                    "Survivability:Frost Resist",
                    "Survivability:Shadow Resist",
                    "Survivability:Physical Mitigation",
                    "Survivability:Resilience",
                    "Survivability:Defense",
                    "Survivability:Crit Reduction",
                    "Survivability:Dodge",
				};
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] {};
                return _customChartNames;
            }
        }

        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelMage();
                }
                return _calculationOptionsPanel;
            }
        }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
					{
						Item.ItemType.None,
						Item.ItemType.Cloth,
						Item.ItemType.Dagger,
						Item.ItemType.OneHandSword,
						Item.ItemType.Staff,
						Item.ItemType.Wand,
					});
                }
                return _relevantItemTypes;
            }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationMage(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsMage(); }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            CharacterCalculationsMage calculatedStats = new CharacterCalculationsMage();
			Stats stats = GetCharacterStats(character, additionalItem);
			calculatedStats.BasicStats = stats;

            float levelScalingFactor = (1 - (70 - 60) / 82f * 3);
            int molten = 0;
            if (character.CalculationOptions["MageArmor"].Equals("Molten")) molten = 1;

            float mindMasteryDamage = (0.05f * int.Parse(character.CalculationOptions["MindMastery"]) * stats.Intellect);
            float improvedSpiritDamage = 0;
            if (character.ActiveBuffs.Contains("Improved Divine Spirit")) improvedSpiritDamage = 0.1f * stats.Spirit;

            calculatedStats.CastingSpeed = 1 + stats.SpellHasteRating / 995f * levelScalingFactor;
            calculatedStats.ArcaneDamage = stats.SpellArcaneDamageRating + stats.SpellDamageRating + mindMasteryDamage + improvedSpiritDamage;
            calculatedStats.FireDamage = stats.SpellFireDamageRating + stats.SpellDamageRating + mindMasteryDamage + improvedSpiritDamage;
            calculatedStats.FrostDamage = stats.SpellFrostDamageRating + stats.SpellDamageRating + mindMasteryDamage + improvedSpiritDamage;

            calculatedStats.SpellCrit = 0.01f * (stats.Intellect * 0.0125f + 0.9075f) + 0.01f * int.Parse(character.CalculationOptions["ArcaneInstability"]) + 0.01f * int.Parse(character.CalculationOptions["ArcanePotency"]) + stats.SpellCritRating / 1400f * levelScalingFactor + 0.03f * molten;
            calculatedStats.SpellHit = stats.SpellHitRating * levelScalingFactor / 800f;

            calculatedStats.SpiritRegen = 0.001f + stats.Spirit * 0.009327f * (float)Math.Sqrt(stats.Intellect);
            calculatedStats.ManaRegen = calculatedStats.SpiritRegen + stats.Mp5 / 5f;
            calculatedStats.ManaRegen5SR = calculatedStats.SpiritRegen * stats.SpellCombatManaRegeneration + stats.Mp5 / 5f;
            calculatedStats.ManaRegenDrinking = calculatedStats.ManaRegen + 240f;
            calculatedStats.HealthRegen = 0.0312f * stats.Spirit + stats.Hp5 / 5f;
            calculatedStats.HealthRegenCombat = stats.Hp5 / 5f;
            calculatedStats.HealthRegenEating = calculatedStats.ManaRegen + 250f;
            calculatedStats.MeleeMitigation = (1-1/(1+0.1f*stats.Armor/(8.5f*(70+4.5f*(70-59))+40)));
            calculatedStats.Defense = 350 + stats.DefenseRating / 2.37f;
            calculatedStats.PhysicalCritReduction = (0.04f*(calculatedStats.Defense-5*70)/100+stats.Resilience/2500f*levelScalingFactor+0.05f*molten);
            calculatedStats.SpellCritReduction = (stats.Resilience/2500f*levelScalingFactor+0.05f*molten);
            calculatedStats.CritDamageReduction = (stats.Resilience/2500f*2f*levelScalingFactor);
            calculatedStats.Dodge = ((0.0443f*stats.Agility+3.28f+0.04f*(calculatedStats.Defense-5*70))/100f+stats.DodgeRating/1200*levelScalingFactor);

            return calculatedStats;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            // TODO: add racial base stats for other races
            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.BloodElf:
                    statsRace = new Stats();
                    break;
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats();
                    break;
                case Character.CharacterRace.Gnome:
                    statsRace = new Stats()
                    {
                        Health = 3213f,
                        Mana = 1961f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 50f,
                        Intellect = 154f,
                        Spirit = 145,
                        ArcaneResistance = 10,
                        BonusIntellectMultiplier = 1.05f * (1 + 0.03f * int.Parse(character.CalculationOptions["ArcaneMind"])) - 1
                    };
                    break;
                case Character.CharacterRace.Human:
                    statsRace = new Stats();
                    break;
                case Character.CharacterRace.Troll:
                    statsRace = new Stats();
                    break;
                case Character.CharacterRace.Undead:
                    statsRace = new Stats();
                    break;
                default:
                    statsRace = new Stats();
                    break;
            }
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;

            Stats statsTotal = statsGearEnchantsBuffs + statsRace;
            statsTotal.Strength = (float)Math.Floor((Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier)) + statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier)) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier));
            statsTotal.Agility = (float)Math.Floor((Math.Floor(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier)) + statsGearEnchantsBuffs.Agility * (1 + statsRace.BonusAgilityMultiplier)) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier));
            statsTotal.Intellect = (float)Math.Floor((Math.Floor(statsRace.Intellect * (1 + statsRace.BonusIntellectMultiplier)) + statsGearEnchantsBuffs.Intellect * (1 + statsRace.BonusIntellectMultiplier)) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier));
            statsTotal.Stamina = (float)Math.Floor((Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier)) + statsGearEnchantsBuffs.Stamina * (1 + statsRace.BonusStaminaMultiplier)) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier));
            statsTotal.Spirit = (float)Math.Floor((Math.Floor(statsRace.Spirit * (1 + statsRace.BonusSpiritMultiplier)) + statsGearEnchantsBuffs.Spirit * (1 + statsRace.BonusSpiritMultiplier)) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier));
            
            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;
            statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)) - 1;
            statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;
            statsTotal.BonusIntellectMultiplier = ((1 + statsRace.BonusIntellectMultiplier) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier)) - 1;
            statsTotal.BonusSpiritMultiplier = ((1 + statsRace.BonusSpiritMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier)) - 1;

            statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + (statsTotal.Stamina * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
            statsTotal.Mana = (float)Math.Round(statsRace.Mana + 15f * statsTotal.Intellect);
            statsTotal.Armor = (float)Math.Round(statsGearEnchantsBuffs.Armor + statsTotal.Agility * 2f + 0.5 * statsTotal.Intellect * int.Parse(character.CalculationOptions["ArcaneFortitude"]));

            int magicAbsorption = 2 * int.Parse(character.CalculationOptions["ArcaneFortitude"]);
            int frostWarding = int.Parse(character.CalculationOptions["FrostWarding"]);
            statsTotal.AllResist += magicAbsorption;

            if (character.CalculationOptions["MageArmor"].Equals("Mage"))
            {
                statsTotal.SpellCombatManaRegeneration += 0.3f;
                statsTotal.AllResist += 18;
            }
            if (character.CalculationOptions["MageArmor"].Equals("Ice"))
            {
                statsTotal.Armor += (float)Math.Floor(645 * (1 + 0.15f * frostWarding));
                statsTotal.FrostResistance += (float)Math.Floor(18 * (1 + 0.15f * frostWarding));
            }

            statsTotal.SpellCombatManaRegeneration += 0.1f * int.Parse(character.CalculationOptions["ArcaneMeditation"]);

            statsTotal.SpellPenetration += 5 * int.Parse(character.CalculationOptions["ArcaneSubtlety"]);

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
                AllResist = stats.AllResist,
                ArcaneResistance = stats.ArcaneResistance,
                FireResistance = stats.FireResistance,
                FrostResistance = stats.FrostResistance,
                NatureResistance = stats.NatureResistance,
                ShadowResistance = stats.ShadowResistance,
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                DefenseRating = stats.DefenseRating,
                DodgeRating = stats.DodgeRating,
                Health = stats.Health,
                Mp5 = stats.Mp5,
                Resilience = stats.Resilience,
                SpellCritRating = stats.SpellCritRating,
                SpellDamageRating = stats.SpellDamageRating,
                SpellFireDamageRating = stats.SpellFireDamageRating,
                SpellHasteRating = stats.SpellHasteRating,
                SpellHitRating = stats.SpellHitRating,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                Mana = stats.Mana,
                SpellArcaneDamageRating = stats.SpellArcaneDamageRating,
                SpellPenetration = stats.SpellPenetration,
                SpellFrostDamageRating = stats.SpellFrostDamageRating,
                Armor = stats.Armor,
                Hp5 = stats.Hp5,
                SpellCombatManaRegeneration = stats.SpellCombatManaRegeneration,
                BonusArcaneSpellPowerMultiplier = stats.BonusArcaneSpellPowerMultiplier,
                BonusFireSpellPowerMultiplier = stats.BonusFireSpellPowerMultiplier,
                BonusFrostSpellPowerMultiplier = stats.BonusFrostSpellPowerMultiplier,
                SpellFrostCritRating = stats.SpellFrostCritRating
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return stats.ToString().Equals("") || (stats.AllResist + stats.ArcaneResistance + stats.FireResistance + stats.FrostResistance + stats.NatureResistance + stats.ShadowResistance + stats.Stamina + stats.Intellect + stats.Spirit + stats.Health + stats.Mp5 + stats.Resilience + stats.SpellCritRating + stats.SpellDamageRating + stats.SpellFireDamageRating + stats.SpellHasteRating + stats.SpellHitRating + stats.BonusIntellectMultiplier + stats.BonusSpellCritMultiplier + stats.BonusSpellPowerMultiplier + stats.BonusStaminaMultiplier + stats.BonusSpiritMultiplier + stats.SpellFrostDamageRating + stats.SpellArcaneDamageRating + stats.SpellPenetration + stats.Mana + stats.SpellCombatManaRegeneration + stats.BonusArcaneSpellPowerMultiplier + stats.BonusFireSpellPowerMultiplier + stats.BonusFrostSpellPowerMultiplier + stats.SpellFrostCritRating) > 0;
        }
    }
}
