using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Moonkin
{
	[Rawr.Calculations.RawrModelInfo("Moonkin", "Spell_Nature_ForceOfNature", Character.CharacterClass.Druid)]
	class CalculationsMoonkin : CalculationsBase
    {
        public static float hitRatingConversionFactor = 100 * (8.0f * (82 / 52.0f) * (131 / 63.0f));
        public static float critRatingConversionFactor = 100 * (14.0f * (82 / 52.0f) * (131 / 63.0f));
        public static float hasteRatingConversionFactor = 100 * (10 * (82 / 52.0f) * (131 / 63.0f));
        public static float intPerCritPercent = 166.0f + (2 / 3.0f);
        public static float BaseMana = 3496.0f;
        public static float ManaRegenConstant = 0.005575f;
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
                    "Basic Stats:Agility",
					"Basic Stats:Stamina",
					"Basic Stats:Intellect",
					"Basic Stats:Spirit",
					"Basic Stats:Armor",
                    "Spell Stats:Spell Hit",
                    "Spell Stats:Spell Crit",
                    "Spell Stats:Spell Haste",
                    "Spell Stats:Arcane Damage",
                    "Spell Stats:Nature Damage",
                    "Mana Regeneration:O5SR Per Second",
                    "Mana Regeneration:I5SR Per Second",
                    "Spell Info:Selected Rotation",
                    "Spell Info:Max DPS Rotation",
                    "Spell Info:SF Spam RDPS",
                    "Spell Info:SF Spam DPS",
                    "Spell Info:SF Spam DPM",
                    "Spell Info:SF Spam OOM",
                    "Spell Info:W Spam RDPS",
                    "Spell Info:W Spam DPS",
                    "Spell Info:W Spam DPM",
                    "Spell Info:W Spam OOM",
                    "Spell Info:MF/SF RDPS",
                    "Spell Info:MF/SF DPS",
                    "Spell Info:MF/SF DPM",
                    "Spell Info:MF/SF OOM",
                    "Spell Info:MF/W RDPS",
                    "Spell Info:MF/W DPS",
                    "Spell Info:MF/W DPM",
                    "Spell Info:MF/W OOM",
                    "Spell Info:IS/SF RDPS",
                    "Spell Info:IS/SF DPS",
                    "Spell Info:IS/SF DPM",
                    "Spell Info:IS/SF OOM",
                    "Spell Info:IS/W RDPS",
                    "Spell Info:IS/W DPS",
                    "Spell Info:IS/W DPM",
                    "Spell Info:IS/W OOM",
                    "Spell Info:IS/MF/SF RDPS",
                    "Spell Info:IS/MF/SF DPS",
                    "Spell Info:IS/MF/SF DPM",
                    "Spell Info:IS/MF/SF OOM",
                    "Spell Info:IS/MF/W RDPS",
                    "Spell Info:IS/MF/W DPS",
                    "Spell Info:IS/MF/W DPM",
                    "Spell Info:IS/MF/W OOM"
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
                    "Talent DPS Comparison",
                    "Talent MP5 Comparison",
                    "Mana Gains",
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
                            Item.ItemType.FistWeapon,
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
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            Stats stats = GetCharacterStats(character, additionalItem);
            calcs.BasicStats = stats;

            float hitRatingMultiplier = 1.0f / CalculationsMoonkin.hitRatingConversionFactor;
            float critRatingMultiplier = 1.0f / CalculationsMoonkin.critRatingConversionFactor;
            float hasteRatingMultiplier = 1.0f / CalculationsMoonkin.hasteRatingConversionFactor;

            calcs.SpellCrit = stats.CritRating * critRatingMultiplier + stats.SpellCrit;
            calcs.SpellHit = stats.HitRating * hitRatingMultiplier + stats.SpellHit;
            calcs.SpellHaste = (stats.HasteRating + stats.DrumsOfBattle) * hasteRatingMultiplier + stats.SpellHaste + stats.Bloodlust;

            stats.SpellPower += stats.DrumsOfWar / 2.0f;

            // All spells: Damage +(0.04 * Lunar Guidance * Int)
            stats.SpellDamageFromIntellectPercentage += 0.04f * character.DruidTalents.LunarGuidance;

            // Fix for rounding error in converting partial points of int/spirit to spell power
            float spellPowerFromStats = (float)Math.Floor(stats.SpellDamageFromIntellectPercentage * stats.Intellect) +
                (float)Math.Floor(stats.SpellDamageFromSpiritPercentage * stats.Spirit);
            calcs.ArcaneDamage = stats.SpellPower + stats.SpellArcaneDamageRating + spellPowerFromStats;
            calcs.NatureDamage = stats.SpellPower + stats.SpellNatureDamageRating + spellPowerFromStats;

			calcs.Latency = calcOpts.Latency;
			calcs.FightLength = calcOpts.FightLength;
			calcs.TargetLevel = calcOpts.TargetLevel;
            calcs.Scryer = calcOpts.AldorScryer == "Scryer";

            // 2.4 spirit regen
            float spiritRegen = 0.001f + ManaRegenConstant * (float)Math.Sqrt(calcs.BasicStats.Intellect) * calcs.BasicStats.Spirit;
            calcs.ManaRegen = spiritRegen + stats.Mp5 / 5f;
            calcs.ManaRegen5SR = spiritRegen * stats.SpellCombatManaRegeneration + stats.Mp5 / 5f;

            // Run the solver to do final calculations
            MoonkinSolver.Solve(character, ref calcs);

            return calcs;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            // Start off with a slightly modified form of druid base character stats calculations
            Stats statsRace = character.Race == Character.CharacterRace.NightElf ?
                new Stats()
                {
                    Health = 7416f,
                    Mana = 3496f,
                    Stamina = 97f,
                    Agility = 87f,
                    Intellect = 143f,
                    Spirit = 157f
                } :
                new Stats()
                {
                    Health = 7416f,
                    Mana = 3496f,
                    Stamina = 100f,
                    Agility = 77f,
                    Intellect = 138f,
                    Spirit = 161f
                };
            
            // Get the gear/enchants/buffs stats loaded in
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;

            // Bonus multipliers
			Stats statsTalents = new Stats()
			{
				BonusStaminaMultiplier = (1 + 0.04f * character.DruidTalents.HeartOfTheWild) * (1 + 0.02f * character.DruidTalents.SurvivalOfTheFittest) - 1,
				BonusAgilityMultiplier = 0.02f * character.DruidTalents.SurvivalOfTheFittest,
				BonusStrengthMultiplier = 0.02f * character.DruidTalents.SurvivalOfTheFittest,
				BonusIntellectMultiplier = (1 + 0.04f * character.DruidTalents.HeartOfTheWild) * (1 + 0.01f * character.DruidTalents.SurvivalOfTheFittest) - 1,
				BonusSpiritMultiplier = (1 + 0.04f * character.DruidTalents.HeartOfTheWild) * (1 + 0.05f * character.DruidTalents.LivingSpirit) - 1
			};
			if (character.ActiveBuffsContains("Moonkin Form"))
			{
				statsTalents.BonusIntellectMultiplier += 1;
				statsTalents.BonusIntellectMultiplier *= 1 + 0.02f * character.DruidTalents.Furor;
				statsTalents.BonusIntellectMultiplier -= 1;
			}

            // Create the total stats object
            Stats statsTotal = statsGearEnchantsBuffs + statsRace + statsTalents;

            // Base stats: Intellect, Stamina, Spirit, Agility
			statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1 + statsTotal.BonusStaminaMultiplier));
			statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
			statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1 + statsTotal.BonusAgilityMultiplier));
			statsTotal.Spirit = (float)Math.Floor(statsTotal.Spirit * (1 + statsTotal.BonusSpiritMultiplier));


            // Derived stats: Health, mana pool, armor
            statsTotal.Health = (float)Math.Round(((statsRace.Health * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f) + statsGearEnchantsBuffs.Health + statsTotal.Stamina * 10f))) - 180;
            statsTotal.Mana = (float)Math.Round(statsRace.Mana + 15f * statsTotal.Intellect) - 280;
			if (character.ActiveBuffsContains("Moonkin Form") && character.DruidTalents.MoonkinForm > 0)
			{
				statsTotal.Armor = (float)Math.Round((statsBaseGear.Armor + statsEnchants.Armor) * 4.7f + statsBuffs.Armor + statsTotal.Agility * 2f);
			}
			else
			{
				statsTotal.Armor = (float)Math.Round(statsGearEnchantsBuffs.Armor + statsTotal.Agility * 2f);
			}

            // Regen mechanic: mp5 +((0.1 * Intensity) * Spiritmp5())
			statsTotal.SpellCombatManaRegeneration += 0.1f * character.DruidTalents.Intensity;
            // Regen mechanic: mp5 +(0.04/0.07/0.10) * Int)
            statsTotal.Mp5 += (int)(statsTotal.Intellect * Math.Ceiling(character.DruidTalents.Dreamstate * 10 / 3.0f) / 100.0f);

            // Hit rating
            // All spells: Hit% +(0.02 * Balance of Power)
            statsTotal.SpellHit += 0.02f * character.DruidTalents.BalanceOfPower;

            // Crit rating
            // Application order: Stats, Talents, Gear
            // Add druid base crit
            statsTotal.SpellCrit += 0.0185f;
            // Add intellect-based crit rating to crit (all classes except warlock: 1/80)
            statsTotal.SpellCrit += statsTotal.Intellect / (100 * CalculationsMoonkin.intPerCritPercent);
            // All spells: Crit% + (0.01 * Natural Perfection)
            statsTotal.SpellCrit += 0.01f * character.DruidTalents.NaturalPerfection;
            // All spells: Haste% + (0.01 * Celestial Focus)
            statsTotal.SpellHaste += 0.01f * character.DruidTalents.CelestialFocus;

            // All spells: Spell Power + (0.5 * Imp Moonkin) * Spirit
            // Add the crit bonus from the idol, if present
            if (character.ActiveBuffsContains("Moonkin Form") && character.DruidTalents.MoonkinForm > 0)
            {
                statsTotal.CritRating += statsTotal.IdolCritRating;
                if (character.ActiveBuffsContains("Improved Moonkin Form"))
                {
                    statsTotal.SpellDamageFromSpiritPercentage += (0.05f * character.DruidTalents.ImprovedMoonkinForm);
                }
            }
            // All spells: Crit% + (0.01 * Improved Faerie Fire)
            if (character.ActiveBuffsContains("Faerie Fire"))
            {
                statsTotal.SpellCrit += 0.01f * character.DruidTalents.ImprovedFaerieFire;
            }

            // Put in this check so that the multiplicative spell power multipliers work correctly
            if (statsTotal.BonusSpellPowerMultiplier == 0.0f)
                statsTotal.BonusSpellPowerMultiplier = 1.0f;

            // All spells: Damage + (0.01 * Earth and Moon)
            statsTotal.BonusSpellPowerMultiplier *= 1.0f + (0.01f * character.DruidTalents.EarthAndMoon);
            // All spells: Damage + (0.02 * Master Shapeshifter)
            if (character.ActiveBuffsContains("Moonkin Form") && character.DruidTalents.MoonkinForm > 0)
                statsTotal.BonusSpellPowerMultiplier *= 1.0f + (0.02f * character.DruidTalents.MasterShapeshifter);

            // Make sure we revert the value to a proper percentage after multiplicative calculations are done
            if (statsTotal.BonusSpellPowerMultiplier >= 1.0f)
                statsTotal.BonusSpellPowerMultiplier -= 1.0f;

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            switch (chartName)
            {
                case "Talent DPS Comparison":
                    CharacterCalculationsMoonkin calcsTalentBase = GetCharacterCalculations(character) as CharacterCalculationsMoonkin;
                    List<ComparisonCalculationBase> comps = new List<ComparisonCalculationBase>();
                    for (int i = 0; i < character.DruidTalents.Data.Length; ++i)
                    {
                        if (character.DruidTalents.Data[i] > 0)
                        {
                            int oldValue = character.DruidTalents.Data[i];
                            character.DruidTalents.Data[i] = 0;
                            CharacterCalculationsMoonkin calcsTalented = GetCharacterCalculations(character) as CharacterCalculationsMoonkin;
                            comps.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = LookupDruidTalentName(i),
                                OverallPoints = calcsTalentBase.OverallPoints - calcsTalented.OverallPoints,
                                DamagePoints = calcsTalentBase.SubPoints[0] - calcsTalented.SubPoints[0],
                                RawDamagePoints = calcsTalentBase.SubPoints[1] - calcsTalented.SubPoints[1]
                            });

                            character.DruidTalents.Data[i] = oldValue;
                        }
                    }
                    comps.RemoveAll(
                        delegate(ComparisonCalculationBase val)
                        {
                            return val.OverallPoints <= 0;
                        });
                    return comps.ToArray();
                case "Talent MP5 Comparison":
                    CharacterCalculationsMoonkin calcsMP5TalentBase = GetCharacterCalculations(character) as CharacterCalculationsMoonkin;
                    List<ComparisonCalculationBase> compsMP5 = new List<ComparisonCalculationBase>();
                    SpellRotation maxRot = calcsMP5TalentBase.SelectedRotation;
                    for (int i = 0; i < character.DruidTalents.Data.Length; ++i)
                    {
                        if (character.DruidTalents.Data[i] > 0)
                        {
                            int oldValue = character.DruidTalents.Data[i];
                            character.DruidTalents.Data[i] = 0;
                            CharacterCalculationsMoonkin calcsTalented = GetCharacterCalculations(character) as CharacterCalculationsMoonkin;
                            float mp5 = 0.0f;
                            float manaGain = 0.0f;
                            foreach (KeyValuePair<string, RotationData> pairs in calcsTalented.Rotations)
                            {
                                if (pairs.Key == maxRot.Name)
                                {
                                    mp5 = (pairs.Value.ManaUsed - maxRot.ManaUsed) / maxRot.Duration * 5.0f;
                                    manaGain = (maxRot.ManaGained - pairs.Value.ManaGained) / (calcsMP5TalentBase.FightLength * 60.0f) * 5.0f;
                                }
                            }
                            compsMP5.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = LookupDruidTalentName(i),
                                OverallPoints = mp5 + manaGain,
                                RawDamagePoints = mp5,
                                DamagePoints = manaGain
                            });

                            character.DruidTalents.Data[i] = oldValue;
                        }
                    }
                    compsMP5.RemoveAll(
                        delegate(ComparisonCalculationBase val)
                        {
                            return val.OverallPoints <= 0;
                        });
                    return compsMP5.ToArray();
                case "Mana Gains":
                    CharacterCalculationsMoonkin calcsManaBase = GetCharacterCalculations(character) as CharacterCalculationsMoonkin;
                    SpellRotation manaGainsRot = calcsManaBase.SelectedRotation;
                    Character c2 = character.Clone();
                    Character c3 = character.Clone();

                    List<ComparisonCalculationMoonkin> manaGainsList = new List<ComparisonCalculationMoonkin>();

                    // Moonkin form
                    c2.DruidTalents.MoonkinForm = 0;
                    c3.DruidTalents.MoonkinForm = 0;
                    CharacterCalculationsMoonkin calcsManaMoonkin = GetCharacterCalculations(c2) as CharacterCalculationsMoonkin;
                    c2.DruidTalents.MoonkinForm = 1;
                    foreach (KeyValuePair<string, RotationData> pairs in calcsManaMoonkin.Rotations)
                    {
                        if (pairs.Key == manaGainsRot.Name)
                        {
                            manaGainsList.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = "Moonkin Form",
                                OverallPoints = (pairs.Value.ManaUsed - manaGainsRot.ManaUsed) / manaGainsRot.Duration * calcsManaBase.FightLength * 60.0f,
                                DamagePoints = (pairs.Value.ManaUsed - manaGainsRot.ManaUsed) / manaGainsRot.Duration * calcsManaBase.FightLength * 60.0f,
                                RawDamagePoints = 0
                            });
                        }
                    }

                    // JoW
                    Buff jow = c2.ActiveBuffs.Find(delegate(Buff b)
                    {
                        return b.Name == "Judgement of Wisdom";
                    });
                    if (character.ActiveBuffsContains("Judgement of Wisdom"))
                    {
                        c2.ActiveBuffs.Remove(jow);
                        c3.ActiveBuffs.Remove(jow);
                    }
                    CharacterCalculationsMoonkin calcsManaJoW = GetCharacterCalculations(c2) as CharacterCalculationsMoonkin;
                    if (character.ActiveBuffsContains("Judgement of Wisdom"))
                    {
                        c2.ActiveBuffs.Add(jow);
                    }
                    foreach (KeyValuePair<string, RotationData> pairs in calcsManaJoW.Rotations)
                    {
                        if (pairs.Key == manaGainsRot.Name)
                        {
                            manaGainsList.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = "Judgement of Wisdom",
                                OverallPoints = (pairs.Value.ManaUsed - manaGainsRot.ManaUsed) / manaGainsRot.Duration * calcsManaBase.FightLength * 60.0f,
                                DamagePoints = (pairs.Value.ManaUsed - manaGainsRot.ManaUsed) / manaGainsRot.Duration * calcsManaBase.FightLength * 60.0f,
                                RawDamagePoints = 0
                            });
                        }
                    }

                    // Replenishment
                    CalculationOptionsMoonkin calcOpts = c2.CalculationOptions as CalculationOptionsMoonkin;
                    CalculationOptionsMoonkin calcOpts3 = c3.CalculationOptions as CalculationOptionsMoonkin;
                    float oldReplenishmentUptime = calcOpts.ReplenishmentUptime;
                    calcOpts.ReplenishmentUptime = 0.0f;
                    calcOpts3.ReplenishmentUptime = 0.0f;
                    CharacterCalculationsMoonkin calcsManaReplenishment = GetCharacterCalculations(c2) as CharacterCalculationsMoonkin;
                    calcOpts.ReplenishmentUptime = oldReplenishmentUptime;
                    foreach (KeyValuePair<string, RotationData> pairs in calcsManaReplenishment.Rotations)
                    {
                        if (pairs.Key == manaGainsRot.Name)
                        {
                            manaGainsList.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = "Replenishment",
                                OverallPoints = manaGainsRot.ManaGained - pairs.Value.ManaGained,
                                DamagePoints = manaGainsRot.ManaGained - pairs.Value.ManaGained,
                                RawDamagePoints = 0
                            });
                        }
                    }

                    // Innervate
                    bool innervate = calcOpts.Innervate;
                    calcOpts.Innervate = false;
                    calcOpts3.Innervate = false;
                    CharacterCalculationsMoonkin calcsManaInnervate = GetCharacterCalculations(c2) as CharacterCalculationsMoonkin;
                    calcOpts.Innervate = innervate;
                    foreach (KeyValuePair<string, RotationData> pairs in calcsManaInnervate.Rotations)
                    {
                        if (pairs.Key == manaGainsRot.Name)
                        {
                            manaGainsList.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = "Innervate",
                                OverallPoints = manaGainsRot.ManaGained - pairs.Value.ManaGained,
                                DamagePoints = manaGainsRot.ManaGained - pairs.Value.ManaGained,
                                RawDamagePoints = 0
                            });
                        }
                    }

                    // Mana spring
                    Buff manaSpring = c2.ActiveBuffs.Find(delegate(Buff b)
                    {
                        return b.Name == "Mana Spring Totem";
                    });
                    if (character.ActiveBuffsContains("Mana Spring Totem"))
                    {
                        c2.ActiveBuffs.Remove(manaSpring);
                        c3.ActiveBuffs.Remove(manaSpring);
                    }
                    CharacterCalculationsMoonkin calcsManaManaSpring = GetCharacterCalculations(c2) as CharacterCalculationsMoonkin;
                    if (character.ActiveBuffsContains("Mana Spring Totem"))
                    {
                        c2.ActiveBuffs.Add(manaSpring);
                    }
                    foreach (KeyValuePair<string, RotationData> pairs in calcsManaManaSpring.Rotations)
                    {
                        if (pairs.Key == manaGainsRot.Name)
                        {
                            manaGainsList.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = "Mana Spring Totem",
                                OverallPoints = manaGainsRot.ManaGained - pairs.Value.ManaGained,
                                DamagePoints = manaGainsRot.ManaGained - pairs.Value.ManaGained,
                                RawDamagePoints = 0
                            });
                        }
                    }

                    // mp5
                    CharacterCalculationsMoonkin calcsMinimum = GetCharacterCalculations(c3) as CharacterCalculationsMoonkin;
                    foreach (KeyValuePair<string, RotationData> pairs in calcsMinimum.Rotations)
                    {
                        if (pairs.Key == manaGainsRot.Name)
                        {
                            manaGainsList.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = "MP5",
                                OverallPoints = pairs.Value.ManaGained - calcsMinimum.BasicStats.Mana,
                                DamagePoints = pairs.Value.ManaGained - calcsMinimum.BasicStats.Mana,
                                RawDamagePoints = 0
                            });
                        }
                    }

                    return manaGainsList.ToArray();
                case "Relative Stat Values":
                    CharacterCalculationsMoonkin calcsBase = GetCharacterCalculations(character) as CharacterCalculationsMoonkin;
                    //CharacterCalculationsMoonkin calcsIntellect = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = 1 } }) as CharacterCalculationsMoonkin;
                    //CharacterCalculationsMoonkin calcsSpirit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Spirit = 1 } }) as CharacterCalculationsMoonkin;
                    CharacterCalculationsMoonkin calcsSpellPower = GetCharacterCalculations(character, new Item() { Stats = new Stats() { SpellPower = 1 } }) as CharacterCalculationsMoonkin;
                    CharacterCalculationsMoonkin calcsHaste = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 1 } }) as CharacterCalculationsMoonkin;
                    CharacterCalculationsMoonkin calcsHit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 1 } }) as CharacterCalculationsMoonkin;
                    CharacterCalculationsMoonkin calcsCrit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 1 } }) as CharacterCalculationsMoonkin;

                    // Intellect calculations
                    CharacterCalculationsMoonkin calcAtAdd = calcsBase;
                    float intToAdd = 0.0f;
                    while (calcsBase.OverallPoints == calcAtAdd.OverallPoints && intToAdd < 2)
                    {
                        intToAdd += 0.01f;
                        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = intToAdd } }) as CharacterCalculationsMoonkin;
                    }
                    CharacterCalculationsMoonkin calcAtSubtract = calcsBase;
                    float intToSubtract = 0.0f;
                    while (calcsBase.OverallPoints == calcAtSubtract.OverallPoints && intToSubtract > -2)
                    {
                        intToSubtract -= 0.01f;
                        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = intToSubtract } }) as CharacterCalculationsMoonkin;
                    }
                    intToSubtract += 0.01f;

                    ComparisonCalculationMoonkin comparisonInt = new ComparisonCalculationMoonkin()
                    {
                        Name = "Intellect",
                        OverallPoints = (calcAtAdd.OverallPoints - calcsBase.OverallPoints) / (intToAdd - intToSubtract),
                        DamagePoints = (calcAtAdd.SubPoints[0] - calcsBase.SubPoints[0]) / (intToAdd - intToSubtract),
                        RawDamagePoints = (calcAtAdd.SubPoints[1] - calcsBase.SubPoints[1]) / (intToAdd - intToSubtract)
                    };

                    // Spirit calculations
                    calcAtAdd = calcsBase;
                    float spiToAdd = 0.0f;
                    while (calcsBase.OverallPoints == calcAtAdd.OverallPoints && spiToAdd < 10)
                    {
                        spiToAdd += 0.01f;
                        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Spirit = spiToAdd } }) as CharacterCalculationsMoonkin;
                    }
                    calcAtSubtract = calcsBase;
                    float spiToSubtract = 0.0f;
                    while (calcsBase.OverallPoints == calcAtSubtract.OverallPoints && spiToSubtract > -10)
                    {
                        spiToSubtract -= 0.01f;
                        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Spirit = spiToSubtract } }) as CharacterCalculationsMoonkin;
                    }
                    spiToSubtract += 0.01f;

                    ComparisonCalculationMoonkin comparisonSpi = new ComparisonCalculationMoonkin()
                    {
                        Name = "Spirit",
                        OverallPoints = (calcAtAdd.OverallPoints - calcsBase.OverallPoints) / (spiToAdd - spiToSubtract),
                        DamagePoints = (calcAtAdd.SubPoints[0] - calcsBase.SubPoints[0]) / (spiToAdd - spiToSubtract),
                        RawDamagePoints = (calcAtAdd.SubPoints[1] - calcsBase.SubPoints[1]) / (spiToAdd - spiToSubtract)
                    };

                    return new ComparisonCalculationBase[] {
                        comparisonInt,
                        comparisonSpi,
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

        private string LookupDruidTalentName(int index)
        {
            switch (index)
            {
                case 0:
                    return "Starlight Wrath";
                case 1:
                    return "Genesis";
                case 2:
                    return "Moonglow";
                case 3:
                    return "Nature's Majesty";
                case 4:
                    return "Improved Moonfire";
                case 5:
                    return "Brambles";
                case 6:
                    return "Nature's Grace";
                case 7:
                    return "Nature's Splendor";
                case 9:
                    return "Vengeance";
                case 10:
                    return "Celestial Focus";
                case 11:
                    return "Lunar Guidance";
                case 12:
                    return "Insect Swarm";
                case 13:
                    return "Improved Insect Swarm";
                case 14:
                    return "Dreamstate";
                case 15:
                    return "Moonfury";
                case 16:
                    return "Balance of Power";
                case 17:
                    return "Moonkin Form";
                case 18:
                    return "Improved Moonkin Form";
                case 19:
                    return "Improved Faerie Fire";
                case 21:
                    return "Wrath of Cenarius";
                case 22:
                    return "Eclipse";
                case 24:
                    return "Force of Nature";
                case 26:
                    return "Earth and Moon";
                case 59:
                    return "Furor";
                case 62:
                    return "Natural Shapeshifter";
                case 63:
                    return "Intensity";
                case 64:
                    return "Omen of Clarity";
                case 65:
                    return "Master Shapeshifter";
                default:
                    return "Not implemented";
            }
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
                BonusArcaneDamageMultiplier = stats.BonusArcaneDamageMultiplier,
                BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier,
                SpellDamageFromIntellectPercentage = stats.SpellDamageFromIntellectPercentage,
                SpellDamageFromSpiritPercentage = stats.SpellDamageFromSpiritPercentage,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                Mana = stats.Mana,
                SpellArcaneDamageRating = stats.SpellArcaneDamageRating,
                SpellNatureDamageRating = stats.SpellNatureDamageRating,
                Armor = stats.Armor,
                SpellCombatManaRegeneration = stats.SpellCombatManaRegeneration,
                SpellPowerFor20SecOnUse2Min = stats.SpellPowerFor20SecOnUse2Min,
                HasteRatingFor20SecOnUse2Min = stats.HasteRatingFor20SecOnUse2Min,
                Mp5OnCastFor20SecOnUse2Min = stats.Mp5OnCastFor20SecOnUse2Min,
                ManaRestorePerCast = stats.ManaRestorePerCast,
                ManaRestoreFromMaxManaPerHit = stats.ManaRestoreFromMaxManaPerHit,
                SpellPowerFor10SecOnHit_10_45 = stats.SpellPowerFor10SecOnHit_10_45,
                SpellPowerFor10SecOnResist = stats.SpellPowerFor10SecOnResist,
                SpellPowerFor15SecOnCrit_20_45 = stats.SpellPowerFor15SecOnCrit_20_45,
                SpellPowerFor15SecOnUse90Sec = stats.SpellPowerFor15SecOnUse90Sec,
                SpellHasteFor6SecOnCast_15_45 = stats.SpellHasteFor6SecOnCast_15_45,
                SpellHasteFor6SecOnHit_10_45 = stats.SpellHasteFor6SecOnHit_10_45,
                SpellPowerFor10SecOnCast_15_45 = stats.SpellPowerFor10SecOnCast_15_45,
                SpellHasteFor10SecOnCast_10_45 = stats.SpellHasteFor10SecOnCast_10_45,
                ManaRestoreOnCast_10_45 = stats.ManaRestoreOnCast_10_45,
                ManaRestorePerCrit = stats.ManaRestorePerCrit,
                StarfireDmg = stats.StarfireDmg,
                MoonfireDmg = stats.MoonfireDmg,
                WrathDmg = stats.WrathDmg,
                IdolCritRating = stats.IdolCritRating,
                UnseenMoonDamageBonus = stats.UnseenMoonDamageBonus,
                LightningCapacitorProc = stats.LightningCapacitorProc,
                InnervateCooldownReduction = stats.InnervateCooldownReduction,
                StarfireBonusWithDot = stats.StarfireBonusWithDot,
                MoonfireExtension = stats.MoonfireExtension,
                BonusInsectSwarmDamage = stats.BonusInsectSwarmDamage,
                BonusNukeCritChance = stats.BonusNukeCritChance,
                StarfireCritChance = stats.StarfireCritChance,
                BonusManaPotion = stats.BonusManaPotion,
                ShatteredSunAcumenProc = stats.ShatteredSunAcumenProc,
                TimbalsProc = stats.TimbalsProc,
                DruidAshtongueTrinket = stats.DruidAshtongueTrinket,
				ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,
                SpellHaste = stats.SpellHaste,
                SpellCrit = stats.SpellCrit,
                SpellHit = stats.SpellHit,
                ArmorPenetration = stats.ArmorPenetration,
                Bloodlust = stats.Bloodlust,
                DrumsOfBattle = stats.DrumsOfBattle,
                DrumsOfWar = stats.DrumsOfWar,
                ThunderCapacitorProc = stats.ThunderCapacitorProc,
                PendulumOfTelluricCurrentsProc = stats.PendulumOfTelluricCurrentsProc,
                ExtractOfNecromanticPowerProc = stats.ExtractOfNecromanticPowerProc
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
			return stats.ToString().Equals("") || (stats.Stamina + stats.Intellect + stats.Spirit + stats.Agility + stats.Health + stats.Mp5 + stats.CritRating + stats.SpellCrit + stats.SpellPower + stats.SpellArcaneDamageRating + stats.SpellNatureDamageRating + stats.HasteRating + stats.SpellHaste + stats.HitRating + stats.SpellHit + +stats.BonusAgilityMultiplier + stats.BonusIntellectMultiplier + stats.BonusSpellCritMultiplier + stats.BonusSpellPowerMultiplier + stats.BonusArcaneDamageMultiplier + stats.BonusNatureDamageMultiplier + stats.BonusStaminaMultiplier + stats.BonusSpiritMultiplier + stats.Mana + stats.SpellCombatManaRegeneration + stats.SpellPowerFor20SecOnUse2Min + stats.HasteRatingFor20SecOnUse2Min + stats.Mp5OnCastFor20SecOnUse2Min + stats.ManaRestoreFromMaxManaPerHit + stats.ManaRestorePerCast + stats.SpellPowerFor10SecOnHit_10_45 + stats.SpellDamageFromIntellectPercentage + stats.SpellDamageFromSpiritPercentage + stats.SpellPowerFor10SecOnResist + stats.SpellPowerFor15SecOnCrit_20_45 + stats.SpellPowerFor15SecOnUse90Sec + stats.SpellHasteFor5SecOnCrit_50 + stats.SpellHasteFor6SecOnCast_15_45 + stats.SpellHasteFor6SecOnHit_10_45 + stats.StarfireDmg + stats.MoonfireDmg + stats.WrathDmg + stats.IdolCritRating + stats.UnseenMoonDamageBonus + stats.LightningCapacitorProc + stats.StarfireCritChance + stats.MoonfireExtension + stats.InnervateCooldownReduction + stats.StarfireBonusWithDot + stats.BonusManaPotion + stats.ShatteredSunAcumenProc + stats.TimbalsProc + stats.DruidAshtongueTrinket + stats.ThreatReductionMultiplier + stats.ManaRestoreFromMaxManaPerSecond + stats.BonusDamageMultiplier + stats.ArmorPenetration + stats.Bloodlust + stats.DrumsOfBattle + stats.DrumsOfWar + stats.BonusNukeCritChance + stats.BonusInsectSwarmDamage + stats.SpellHasteFor10SecOnCast_10_45 + stats.SpellPowerFor10SecOnCast_15_45 + stats.SpellPowerFor10SecOnCrit_20_45 + stats.ManaRestoreOnCast_10_45 + stats.ManaRestorePerCrit + stats.ThunderCapacitorProc + stats.PendulumOfTelluricCurrentsProc + stats.ExtractOfNecromanticPowerProc) > 0;
        }
    }
}
