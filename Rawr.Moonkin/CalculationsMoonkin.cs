using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Moonkin
{
	[Rawr.Calculations.RawrModelInfo("Moonkin", "Spell_Nature_ForceOfNature", Character.CharacterClass.Druid)]
	class CalculationsMoonkin : CalculationsBase
    {
        public static float hitRatingConversionFactor = 800f * (41.0f / 26.0f);
        public static float hasteRatingConversionFactor = 1000f * (41.0f / 26.0f);
        public static float critRatingConversionFactor = 1400f * (41.0f / 26.0f);
        private Dictionary<string, System.Drawing.Color> subColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (subColors == null)
                {
                    subColors = new Dictionary<string, System.Drawing.Color>();
                    subColors.Add("Damage", System.Drawing.Color.Blue);
                    subColors.Add("Raw Damage", System.Drawing.Color.Red);
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
                    "Mana Regeneration:O5SR Per Second",
                    "Mana Regeneration:I5SR Per Second",
                    "Spell Info:Selected Rotation",
                    "Spell Info:Max DPS Rotation",
                    "Spell Info:MF/SFx4 RDPS",
                    "Spell Info:MF/SFx4 DPS",
                    "Spell Info:MF/SFx4 DPM",
                    "Spell Info:MF/SFx4 OOM",
                    "Spell Info:SF Spam RDPS",
                    "Spell Info:SF Spam DPS",
                    "Spell Info:SF Spam DPM",
                    "Spell Info:SF Spam OOM",
                    "Spell Info:W Spam RDPS",
                    "Spell Info:W Spam DPS",
                    "Spell Info:W Spam DPM",
                    "Spell Info:W Spam OOM",
                    "Spell Info:MF/SFx3/W RDPS",
                    "Spell Info:MF/SFx3/W DPS",
                    "Spell Info:MF/SFx3/W DPM",
                    "Spell Info:MF/SFx3/W OOM",
                    "Spell Info:MF/Wx8 RDPS",
                    "Spell Info:MF/Wx8 DPS",
                    "Spell Info:MF/Wx8 DPM",
                    "Spell Info:MF/Wx8 OOM",
                    "Spell Info:IS/MF/SFx3 RDPS",
                    "Spell Info:IS/MF/SFx3 DPS",
                    "Spell Info:IS/MF/SFx3 DPM",
                    "Spell Info:IS/MF/SFx3 OOM",
                    "Spell Info:IS/MF/Wx7 RDPS",
                    "Spell Info:IS/MF/Wx7 DPS",
                    "Spell Info:IS/MF/Wx7 DPM",
                    "Spell Info:IS/MF/Wx7 OOM",
                    "Spell Info:IS/SFx3/W RDPS",
                    "Spell Info:IS/SFx3/W DPS",
                    "Spell Info:IS/SFx3/W DPM",
                    "Spell Info:IS/SFx3/W OOM",
                    "Spell Info:IS/SFx4 RDPS",
                    "Spell Info:IS/SFx4 DPS",
                    "Spell Info:IS/SFx4 DPM",
                    "Spell Info:IS/SFx4 OOM",
                    "Spell Info:IS/Wx8 RDPS",
                    "Spell Info:IS/Wx8 DPS",
                    "Spell Info:IS/Wx8 DPM",
                    "Spell Info:IS/Wx8 OOM"
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
                    customChartNames = new string[] { 
					"Relative Stat Values"
                    };
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

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Druid; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationMoonkin();
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsMoonkin();
        }

		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsMoonkin));
			System.IO.StringReader reader = new System.IO.StringReader(xml);
			CalculationOptionsMoonkin calcOpts = serializer.Deserialize(reader) as CalculationOptionsMoonkin;
			return calcOpts;
		}

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            CharacterCalculationsMoonkin calcs = new CharacterCalculationsMoonkin();
            Stats stats = GetCharacterStats(character, additionalItem);
            calcs.BasicStats = stats;

            float hitRatingMultiplier = 1.0f / CalculationsMoonkin.hitRatingConversionFactor;
            float critRatingMultiplier = 1.0f / CalculationsMoonkin.critRatingConversionFactor;

            calcs.SpellCrit = stats.CritRating * critRatingMultiplier;
            calcs.SpellHit = stats.HitRating * hitRatingMultiplier;

			CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            // All spells: Damage +((0.08/0.16/0.25) * Int)
            switch (character.DruidTalents.LunarGuidance)
            {
                case 1:
                    stats.SpellDamageFromIntellectPercentage += 0.08f;
                    break;
                case 2:
                    stats.SpellDamageFromIntellectPercentage += 0.16f;
                    break;
                case 3:
                    stats.SpellDamageFromIntellectPercentage += 0.25f;
                    break;
                default:
                    stats.SpellDamageFromIntellectPercentage += 0.0f;
                    break;
            }
            calcs.ArcaneDamage = stats.SpellPower + stats.SpellArcaneDamageRating + stats.SpellDamageFromIntellectPercentage * stats.Intellect + stats.SpellDamageFromSpiritPercentage * stats.Spirit;
            calcs.NatureDamage = stats.SpellPower + stats.SpellNatureDamageRating + stats.SpellDamageFromIntellectPercentage * stats.Intellect + stats.SpellDamageFromSpiritPercentage * stats.Spirit;

			calcs.Latency = calcOpts.Latency;
			calcs.FightLength = calcOpts.FightLength;
			calcs.TargetLevel = calcOpts.TargetLevel;
            calcs.Scryer = calcOpts.AldorScryer == "Scryer";

            // 2.4 spirit regen
            float baseRegenConstant = 0.00932715221261f;
            float spiritRegen = 0.001f + baseRegenConstant * (float)Math.Sqrt(calcs.BasicStats.Intellect) * calcs.BasicStats.Spirit;
            calcs.ManaRegen = spiritRegen + stats.Mp5 / 5f;
            calcs.ManaRegen5SR = spiritRegen * stats.SpellCombatManaRegeneration + stats.Mp5 / 5f;

            // Run the solver to do final calculations
            MoonkinSolver.Solve(character, ref calcs);

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

			CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            // Create the total stats object
            Stats statsTotal = statsGearEnchantsBuffs + statsRace;

            // Base stats: Intellect, Stamina, Spirit, Agility
            statsTotal.Intellect = (float)Math.Floor((Math.Floor(statsRace.Intellect * (1 + statsRace.BonusIntellectMultiplier)) + statsGearEnchantsBuffs.Intellect * (1 + statsRace.BonusIntellectMultiplier)) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier));
            statsTotal.Stamina = (float)Math.Floor((Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier)) + statsGearEnchantsBuffs.Stamina * (1 + statsRace.BonusStaminaMultiplier)) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier));
            statsTotal.Agility = (float)Math.Floor((Math.Floor(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier)) + statsGearEnchantsBuffs.Agility * (1 + statsRace.BonusAgilityMultiplier)) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier));
            statsTotal.Spirit = (float)Math.Floor((Math.Floor(statsRace.Spirit * (1 + statsRace.BonusSpiritMultiplier)) + statsGearEnchantsBuffs.Spirit * (1 + statsRace.BonusSpiritMultiplier)) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier));

            // Base stats: Intellect% +(0.04 * Heart of the Wild)
            statsTotal.Intellect *= 1 + 0.04f * character.DruidTalents.HeartOfTheWild;
            // Base stats: Stam%, Int%, Spi%, Agi% +(0.01 * Survival of the Fittest)
            statsTotal.Intellect *= 1 + 0.01f * character.DruidTalents.SurvivalOfTheFittest;
            statsTotal.Stamina *= 1 + 0.01f * character.DruidTalents.SurvivalOfTheFittest;
            statsTotal.Agility *= 1 + 0.01f * character.DruidTalents.SurvivalOfTheFittest;
            statsTotal.Spirit *= 1 + 0.01f * character.DruidTalents.SurvivalOfTheFittest;
            // Base stats: Spirit% +(0.05 * Living Spirit)
			statsTotal.Spirit *= 1 + 0.05f * character.DruidTalents.LivingSpirit;
            // Base stats: Int% +(0.02 * Furor)
            if (character.ActiveBuffsContains("Moonkin Aura"))
                statsTotal.Intellect *= 1 + 0.02f * character.DruidTalents.Furor;

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
			statsTotal.SpellCombatManaRegeneration += 0.1f * character.DruidTalents.Intensity;
            // Regen mechanic: mp5 +(0.04/0.07/0.10) * Int)
            float dreamstatePercent = 0.0f;
            switch (character.DruidTalents.Dreamstate)
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
            statsTotal.Mp5 += (float)(int)(dreamstatePercent * statsTotal.Intellect);

            // Hit rating
            // All spells: Hit% +(0.02 * Balance of Power)
            statsTotal.HitRating += 0.02f * character.DruidTalents.BalanceOfPower * hitRatingConversionFactor;

            // Crit rating
            // Application order: Stats, Talents, Gear
            // Add druid base crit
            statsTotal.CritRating += (0.0185f * critRatingConversionFactor);
            // Add intellect-based crit rating to crit (all classes except warlock: 1/80)
            statsTotal.CritRating += (statsTotal.Intellect / 8000.0f) * critRatingConversionFactor;
            // All spells: Crit% + (0.01 * Natural Perfection)
            statsTotal.CritRating += 0.01f * character.DruidTalents.NaturalPerfection * critRatingConversionFactor;
            // All spells: Haste% + (0.01 * Celestial Focus)
            statsTotal.HasteRating += 0.01f * character.DruidTalents.CelestialFocus * hasteRatingConversionFactor;
            // All spells: Haste% + (0.01 * Imp Moonkin)
            // All spells: Spell Power + (0.5 * Imp Moonkin) * Spirit
            // Add the crit bonus from the idol, if present
            if (character.ActiveBuffsContains("Moonkin Aura"))
            {
                statsTotal.CritRating += statsTotal.IdolCritRating;
                statsTotal.HasteRating += 0.01f * character.DruidTalents.ImprovedMoonkinForm * hasteRatingConversionFactor;
                statsTotal.SpellPower += (0.5f * character.DruidTalents.ImprovedMoonkinForm) * statsTotal.Spirit;
            }
            // All spells: Crit% + (0.01 * Improved Faerie Fire)
            if (character.ActiveBuffsContains("Improved Faerie Fire"))
            {
                statsTotal.CritRating += 0.01f * character.DruidTalents.ImprovedFaerieFire * critRatingConversionFactor;
            }
            // All spells: Spell Power + (0.01 * Earth and Moon)
            statsTotal.SpellPower *= 1 + (0.01f * character.DruidTalents.EarthAndMoon);
            // All spells: Spell Power + (0.02 * Master Shapeshifter)
            if (character.ActiveBuffsContains("Moonkin Aura"))
                statsTotal.SpellPower *= 1 + (0.02f * character.DruidTalents.MasterShapeshifter);

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            switch (chartName)
            {
                case "Relative Stat Values":
                    CharacterCalculationsMoonkin calcsBase = GetCharacterCalculations(character) as CharacterCalculationsMoonkin;
                    CharacterCalculationsMoonkin calcsIntellect = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = 1 } }) as CharacterCalculationsMoonkin;
                    CharacterCalculationsMoonkin calcsSpirit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Spirit = 1 } }) as CharacterCalculationsMoonkin;
                    CharacterCalculationsMoonkin calcsSpellPower = GetCharacterCalculations(character, new Item() { Stats = new Stats() { SpellPower = 1 } }) as CharacterCalculationsMoonkin;
                    CharacterCalculationsMoonkin calcsHaste = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 1 } }) as CharacterCalculationsMoonkin;
                    CharacterCalculationsMoonkin calcsHit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 1 } }) as CharacterCalculationsMoonkin;
                    CharacterCalculationsMoonkin calcsCrit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 1 } }) as CharacterCalculationsMoonkin;

                    return new ComparisonCalculationBase[] {
                        new ComparisonCalculationMoonkin() { Name = "Intellect",
                            OverallPoints = calcsIntellect.OverallPoints - calcsBase.OverallPoints,
                            DamagePoints = calcsIntellect.SubPoints[0] - calcsBase.SubPoints[0],
                            RawDamagePoints = calcsIntellect.SubPoints[1] - calcsBase.SubPoints[1]
                        },
                        new ComparisonCalculationMoonkin() { Name = "Spirit",
                            OverallPoints = calcsSpirit.OverallPoints - calcsBase.OverallPoints,
                            DamagePoints = calcsSpirit.SubPoints[0] - calcsBase.SubPoints[0],
                            RawDamagePoints = calcsSpirit.SubPoints[1] - calcsBase.SubPoints[1]
                        },
                        new ComparisonCalculationMoonkin() { Name = "Spell Power",
                            OverallPoints = calcsSpellPower.OverallPoints - calcsBase.OverallPoints,
                            DamagePoints = calcsSpellPower.SubPoints[0] - calcsBase.SubPoints[0],
                            RawDamagePoints = calcsSpellPower.SubPoints[1] - calcsBase.SubPoints[1]
                        },
                        new ComparisonCalculationMoonkin() { Name = "Haste",
                            OverallPoints = calcsHaste.OverallPoints - calcsBase.OverallPoints,
                            DamagePoints = calcsHaste.SubPoints[0] - calcsBase.SubPoints[0],
                            RawDamagePoints = calcsHaste.SubPoints[1] - calcsBase.SubPoints[1]
                        },
                        new ComparisonCalculationMoonkin() { Name = "Hit",
                            OverallPoints = calcsHit.OverallPoints - calcsBase.OverallPoints,
                            DamagePoints = calcsHit.SubPoints[0] - calcsBase.SubPoints[0],
                            RawDamagePoints = calcsHit.SubPoints[1] - calcsBase.SubPoints[1]
                        },
                        new ComparisonCalculationMoonkin() { Name = "Crit",
                            OverallPoints = calcsCrit.OverallPoints - calcsBase.OverallPoints,
                            DamagePoints = calcsCrit.SubPoints[0] - calcsBase.SubPoints[0],
                            RawDamagePoints = calcsCrit.SubPoints[1] - calcsBase.SubPoints[1]
                        }};
            }
            return new ComparisonCalculationBase[0];
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
                CritRating = stats.CritRating,
                SpellPower = stats.SpellPower,
                HasteRating = stats.HasteRating,
                HitRating = stats.HitRating,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                BonusArcaneSpellPowerMultiplier = stats.BonusArcaneSpellPowerMultiplier,
                BonusNatureSpellPowerMultiplier = stats.BonusNatureSpellPowerMultiplier,
                SpellDamageFromIntellectPercentage = stats.SpellDamageFromIntellectPercentage,
                SpellDamageFromSpiritPercentage = stats.SpellDamageFromSpiritPercentage,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                Mana = stats.Mana,
                SpellArcaneDamageRating = stats.SpellArcaneDamageRating,
                SpellNatureDamageRating = stats.SpellNatureDamageRating,
                Armor = stats.Armor,
                SpellCombatManaRegeneration = stats.SpellCombatManaRegeneration,
                SpellDamageFor20SecOnUse2Min = stats.SpellDamageFor20SecOnUse2Min,
                SpellHasteFor20SecOnUse2Min = stats.SpellHasteFor20SecOnUse2Min,
                Mp5OnCastFor20SecOnUse2Min = stats.Mp5OnCastFor20SecOnUse2Min,
                ManaRestorePerCast = stats.ManaRestorePerCast,
                ManaRestorePerHit = stats.ManaRestorePerHit,
                SpellDamageFor10SecOnHit_10_45 = stats.SpellDamageFor10SecOnHit_10_45,
                SpellDamageFor10SecOnResist = stats.SpellDamageFor10SecOnResist,
                SpellDamageFor15SecOnCrit_20_45 = stats.SpellDamageFor15SecOnCrit_20_45,
                SpellDamageFor15SecOnUse90Sec = stats.SpellDamageFor15SecOnUse90Sec,
                SpellHasteFor6SecOnCast_15_45 = stats.SpellHasteFor6SecOnCast_15_45,
                SpellHasteFor6SecOnHit_10_45 = stats.SpellHasteFor6SecOnHit_10_45,
                StarfireDmg = stats.StarfireDmg,
                MoonfireDmg = stats.MoonfireDmg,
                WrathDmg = stats.WrathDmg,
                IdolCritRating = stats.IdolCritRating,
                UnseenMoonDamageBonus = stats.UnseenMoonDamageBonus,
                LightningCapacitorProc = stats.LightningCapacitorProc,
                InnervateCooldownReduction = stats.InnervateCooldownReduction,
                StarfireBonusWithDot = stats.StarfireBonusWithDot,
                MoonfireExtension = stats.MoonfireExtension,
                StarfireCritChance = stats.StarfireCritChance,
                BonusManaPotion = stats.BonusManaPotion,
                ShatteredSunAcumenProc = stats.ShatteredSunAcumenProc,
                TimbalsProc = stats.TimbalsProc,
                DruidAshtongueTrinket = stats.DruidAshtongueTrinket,
				ThreatReductionMultiplier = stats.ThreatReductionMultiplier
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
			return stats.ToString().Equals("") || (stats.Stamina + stats.Intellect + stats.Spirit + stats.Agility + stats.Health + stats.Mp5 + stats.CritRating + stats.SpellPower + stats.SpellArcaneDamageRating + stats.SpellNatureDamageRating + stats.HasteRating + stats.HitRating + +stats.BonusAgilityMultiplier + stats.BonusIntellectMultiplier + stats.BonusSpellCritMultiplier + stats.BonusSpellPowerMultiplier + stats.BonusArcaneSpellPowerMultiplier + stats.BonusNatureSpellPowerMultiplier + stats.BonusStaminaMultiplier + stats.BonusSpiritMultiplier + stats.Mana + stats.SpellCombatManaRegeneration + stats.SpellDamageFor20SecOnUse2Min + stats.SpellHasteFor20SecOnUse2Min + stats.Mp5OnCastFor20SecOnUse2Min + stats.ManaRestorePerHit + stats.ManaRestorePerCast + stats.SpellDamageFor10SecOnHit_10_45 + stats.SpellDamageFromIntellectPercentage + stats.SpellDamageFromSpiritPercentage + stats.SpellDamageFor10SecOnResist + stats.SpellDamageFor15SecOnCrit_20_45 + stats.SpellDamageFor15SecOnUse90Sec + stats.SpellHasteFor5SecOnCrit_50 + stats.SpellHasteFor6SecOnCast_15_45 + stats.SpellHasteFor6SecOnHit_10_45 + stats.StarfireDmg + stats.MoonfireDmg + stats.WrathDmg + stats.IdolCritRating + stats.UnseenMoonDamageBonus + stats.LightningCapacitorProc + stats.StarfireCritChance + stats.MoonfireExtension + stats.InnervateCooldownReduction + stats.StarfireBonusWithDot + stats.BonusManaPotion + stats.ShatteredSunAcumenProc + stats.TimbalsProc + stats.DruidAshtongueTrinket + stats.ThreatReductionMultiplier) > 0;
        }
    }
}
