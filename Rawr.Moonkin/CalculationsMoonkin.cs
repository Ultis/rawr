using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Moonkin
{
	[Rawr.Calculations.RawrModelInfo("Moonkin", "Spell_Nature_ForceOfNature", CharacterClass.Druid)]
	public class CalculationsMoonkin : CalculationsBase
    {
        private string[] _optimizableCalculationLabels = null;
        /// <summary>
        /// Labels of the stats available to the Optimizer 
        /// </summary>
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
                    "Hit Rating",
					"Haste Rating",
                    "Crit Rating"
					};
                return _optimizableCalculationLabels;
            }
        }
        /// <summary>
        /// List of gemming templates available to Rawr.
        /// </summary>
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                return new List<GemmingTemplate>()
				{
					// Uncommon gems
					// All sockets = Runed Bloodstone
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Uncommon",
						RedId = 39911,
						YellowId = 39911,
						BlueId = 39911,
						PrismaticId = 39911,
						MetaId = 41285
					},
					// Perfect uncommon gems
					// All sockets = Perfect Runed Bloodstone
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Uncommon",
						RedId = 41438,
						YellowId = 41438,
						BlueId = 41438,
						PrismaticId = 41438,
						MetaId = 41285
					},
					// Uncommon gems
					// All but blue = Runed Bloodstone
					// Blue = Purified Shadow Crystal (spirit/spell power)
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Uncommon",
						RedId = 39911,
						YellowId = 39911,
						BlueId = 39941,
						PrismaticId = 39911,
						MetaId = 41285
					},
					// Perfect uncommon gems
					// All but blue = Perfect Runed Bloodstone
					// Blue = Perfect Purified Shadow Crystal (spirit/spell power)
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Uncommon",
						RedId = 41438,
						YellowId = 41438,
						BlueId = 41457,
						PrismaticId = 41438,
						MetaId = 41285
					},
					// Rare gems
					// All but blue = Runed Scarlet Ruby
					// Blue = Purified Twilight Opal (spirit/spell power)
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Rare",
						RedId = 39998,
						YellowId = 39998,
						BlueId = 40026,
						PrismaticId = 39998,
						MetaId = 41285
					},
					// Rare gems
					// All sockets = Runed Scarlet Ruby
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Rare",
						RedId = 39998,
						YellowId = 39998,
						BlueId = 39998,
						PrismaticId = 39998,
						MetaId = 41285
					},
					// Epic gems
					// All but blue = Runed Cardinal Ruby
					// Blue = Purified Dreadstone (spirit/spell power)
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Epic",
						RedId = 40113,
						YellowId = 40113,
						BlueId = 40133,
						PrismaticId = 40113,
						MetaId = 41285,
                        Enabled = true
					},
					// Epic gems
					// All sockets = Runed Cardinal Ruby
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Epic",
						RedId = 40113,
						YellowId = 40113,
						BlueId = 40113,
						PrismaticId = 40113,
						MetaId = 41285,
                        Enabled = true
					},
                    // Jewelcrafting gems
                    // Red sockets = Runed Dragon's Eye
                    // All other sockets = Runed Cardinal Ruby
                    // Meta = Chaotic
                    new GemmingTemplate()
                    {
                        Model = "Moonkin",
                        Group = "Jeweler",
                        RedId = 42144,
                        YellowId = 40113,
                        BlueId = 40113,
                        PrismaticId = 40113,
                        MetaId = 41285
                    },
                    // Jewelcrafting gems
                    // Red sockets = Runed Dragon's Eye
                    // Blue sockets = Purified Dreadstone
                    // All other sockets = Runed Cardinal Ruby
                    // Meta = Chaotic
                    new GemmingTemplate()
                    {
                        Model = "Moonkin",
                        Group = "Jeweler",
                        RedId = 42144,
                        YellowId = 40113,
                        BlueId = 40133,
                        PrismaticId = 40113,
                        MetaId = 41285
                    },
                    // Jewelcrafting gems
                    // Red, Yellow sockets = Runed Cardinal Ruby
                    // Blue sockets = Runed Dragon's Eye
                    // Meta = Chaotic
                    new GemmingTemplate()
                    {
                        Model = "Moonkin",
                        Group = "Jeweler",
                        RedId = 40113,
                        YellowId = 40113,
                        BlueId = 42144,
                        PrismaticId = 40113,
                        MetaId = 41285
                    },
                    // Jewelcrafting gems
                    // Yellow sockets = Runed Dragon's Eye
                    // All other sockets = Runed Cardinal Ruby
                    // Meta = Chaotic
                    new GemmingTemplate()
                    {
                        Model = "Moonkin",
                        Group = "Jeweler",
                        RedId = 40113,
                        YellowId = 42144,
                        BlueId = 40113,
                        PrismaticId = 40113,
                        MetaId = 41285
                    },
                    // Jewelcrafting gems
                    // Yellow sockets = Runed Dragon's Eye
                    // Blue sockets = Purified Dreadstone
                    // All other sockets = Runed Cardinal Ruby
                    // Meta = Chaotic
                    new GemmingTemplate()
                    {
                        Model = "Moonkin",
                        Group = "Jeweler",
                        RedId = 40113,
                        YellowId = 42144,
                        BlueId = 40133,
                        PrismaticId = 40113,
                        MetaId = 41285
                    },
                    // Jewelcrafting gems
                    // All sockets = Runed Dragon's Eye
                    // Meta = Chaotic
                    new GemmingTemplate()
                    {
                        Model = "Moonkin",
                        Group = "Jeweler",
                        RedId = 42144,
                        YellowId = 42144,
                        BlueId = 42144,
                        PrismaticId = 42144,
                        MetaId = 41285
                    }
				};
            }
        }

        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot)
        {
            if (slot == ItemSlot.OffHand || slot == ItemSlot.Ranged) return false;
            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique)
        {
            if (slot == CharacterSlot.OffHand && item.Slot == ItemSlot.OneHand) return false;
            return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
        }

        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            if (_relevantGlyphs == null)
            {
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of Innervate");
                _relevantGlyphs.Add("Glyph of Insect Swarm");
                _relevantGlyphs.Add("Glyph of Moonfire");
                _relevantGlyphs.Add("Glyph of Starfire");
                _relevantGlyphs.Add("Glyph of Starfall");
            }
            return _relevantGlyphs;
        }

        //public static float hitRatingConversionFactor = 100 * (8.0f * (82 / 52.0f) * (131 / 63.0f));
        //public static float critRatingConversionFactor = 100 * (14.0f * (82 / 52.0f) * (131 / 63.0f));
        //public static float hasteRatingConversionFactor = 100 * (10 * (82 / 52.0f) * (131 / 63.0f));
        //public static float intPerCritPercent = 166.0f + (2 / 3.0f);
        public static float BaseMana = 3496.0f;
        //public static float ManaRegenConstant = 0.005575f * 0.6f;
#if RAWR3
        private Dictionary<string, System.Windows.Media.Color> subColors = null;
        public override Dictionary<string, System.Windows.Media.Color> SubPointNameColors
        {
            get
            {
                if (subColors == null)
                {
                    subColors = new Dictionary<string, System.Windows.Media.Color>();
                    subColors.Add("Sustained Damage", System.Windows.Media.Color.FromArgb(255, 0, 0, 255));
                    subColors.Add("Burst Damage", System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
                }
                return subColors;
            }
        }
#else
        private Dictionary<string, System.Drawing.Color> subColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (subColors == null)
                {
                    subColors = new Dictionary<string, System.Drawing.Color>();
                    subColors.Add("Sustained Damage", System.Drawing.Color.FromArgb(255, 0, 0, 255));
                    subColors.Add("Burst Damage", System.Drawing.Color.FromArgb(255, 255, 0, 0));
                }
                return subColors;
            }
        }
#endif

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
                    "Spell Stats:Spell Power",
                    "Spell Stats:Spell Hit",
                    "Spell Stats:Spell Crit",
                    "Spell Stats:Spell Haste",
                    "Regen:MP5 Not Casting",
                    "Regen:MP5 While Casting",
                    "Solution:Total Score",
                    "Solution:Selected Rotation",
                    "Solution:Selected DPS",
                    "Solution:Selected Time To OOM",
                    "Solution:Selected Cycle Length",
                    "Solution:Selected Spell Breakdown",
                    "Solution:Burst Rotation",
                    "Solution:Burst DPS",
                    "Solution:Burst Cycle Length",
                    "Solution:Burst Spell Breakdown",
                    "Spell Info:Starfire",
                    "Spell Info:Wrath",
                    "Spell Info:Moonfire",
                    "Spell Info:Insect Swarm",
                    };
                }
                return characterDisplayCalculationLabels;
            }
        }

        private string[] customChartNames = null;
        public override string[] CustomChartNames {
            get {
                if (customChartNames == null) {
                    customChartNames = new string[] { 
                    "Talent DPS Comparison",
                    "Talent MP5 Comparison",
                    "Mana Gains",
                    "Damage per Cast Time"
                    };
                }
                return customChartNames;
            }
        }

#if RAWR3
        private ICalculationOptionsPanel calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
#else
        private CalculationOptionsPanelBase calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
#endif
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

        private List<ItemType> relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                if (relevantItemTypes == null)
                {
                    relevantItemTypes = new List<ItemType>(new ItemType[]
                        {
                            ItemType.None,
                            ItemType.Cloth,
                            ItemType.Leather,
                            ItemType.Dagger,
                            ItemType.Staff,
                            ItemType.FistWeapon,
                            ItemType.OneHandMace,
                            ItemType.TwoHandMace,
                            ItemType.Idol,
                        });
                }
                return relevantItemTypes;
            }
        }

		public override CharacterClass TargetClass { get { return CharacterClass.Druid; } }
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

        public static CharacterCalculationsMoonkin GetInnerCharacterCalculations(Character character, Stats stats, Item additionalItem)
        {
            CharacterCalculationsMoonkin calcs = new CharacterCalculationsMoonkin();
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            calcs.BasicStats = stats;

            calcs.SpellCrit = 0.0185f + StatConversion.GetSpellCritFromIntellect(stats.Intellect) + StatConversion.GetSpellCritFromRating(stats.CritRating) + stats.SpellCrit + stats.SpellCritOnTarget;
            calcs.SpellHit = StatConversion.GetSpellHitFromRating(stats.HitRating) + stats.SpellHit;
            calcs.SpellHaste = (1 + StatConversion.GetSpellHasteFromRating(stats.HasteRating)) * (1 + stats.SpellHaste) - 1;

            // Fix for rounding error in converting partial points of int/spirit to spell power
            float spellPowerFromStats = (float)Math.Floor(stats.SpellDamageFromIntellectPercentage * stats.Intellect) +
                (float)Math.Floor(stats.SpellDamageFromSpiritPercentage * stats.Spirit);
            calcs.SpellPower = stats.SpellPower + spellPowerFromStats;

            calcs.Latency = calcOpts.Latency;
            calcs.FightLength = calcOpts.FightLength;
            calcs.TargetLevel = calcOpts.TargetLevel;

            // 3.1 spirit regen
            float spiritRegen = StatConversion.GetSpiritRegenSec(calcs.BasicStats.Spirit, calcs.BasicStats.Intellect);
            calcs.ManaRegen = spiritRegen + stats.Mp5 / 5f;
            calcs.ManaRegen5SR = spiritRegen * stats.SpellCombatManaRegeneration + stats.Mp5 / 5f;

            return calcs;
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            Stats stats = GetCharacterStats(character, additionalItem);
            CharacterCalculationsMoonkin calcs = CalculationsMoonkin.GetInnerCharacterCalculations(character, stats, additionalItem);
            // Run the solver to do final calculations
            new MoonkinSolver().Solve(character, ref calcs);

            return calcs;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            // Start off with a slightly modified form of druid base character stats calculations
            Stats statsRace = character.Race == CharacterRace.NightElf ?
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
            Stats statsBuffs = GetBuffsStats(character, calcOpts);

            Stats statsGearEnchantsBuffs = statsBaseGear + statsBuffs;

            // Talented bonus multipliers

            Stats statsHotW = new Stats()
            {
                BonusIntellectMultiplier = 0.04f * character.DruidTalents.HeartOfTheWild
            };

            Stats statsSotF = new Stats()
            {
                BonusStaminaMultiplier = 0.02f * character.DruidTalents.SurvivalOfTheFittest,
                BonusIntellectMultiplier = 0.02f * character.DruidTalents.SurvivalOfTheFittest,
                BonusSpiritMultiplier = 0.02f * character.DruidTalents.SurvivalOfTheFittest,
                BonusAgilityMultiplier = 0.02f * character.DruidTalents.SurvivalOfTheFittest
            };

            Stats statsImpMotW = new Stats()
            {
                BonusStaminaMultiplier = 0.01f * character.DruidTalents.ImprovedMarkOfTheWild,
                BonusIntellectMultiplier = 0.01f * character.DruidTalents.ImprovedMarkOfTheWild,
                BonusSpiritMultiplier = 0.01f * character.DruidTalents.ImprovedMarkOfTheWild,
                BonusAgilityMultiplier = 0.01f * character.DruidTalents.ImprovedMarkOfTheWild
            };

            Stats statsLivingSpirit = new Stats()
            {
                BonusSpiritMultiplier = 0.05f * character.DruidTalents.LivingSpirit
            };

            Stats statsFuror = new Stats()
            {
                BonusIntellectMultiplier = 0.02f * character.DruidTalents.Furor
            };

            Stats statsEarthAndMoon = new Stats()
            {
                BonusSpellPowerMultiplier = 0.01f * character.DruidTalents.EarthAndMoon
            };

            Stats statsMasterSS = new Stats()
            {
                BonusSpellPowerMultiplier = (character.DruidTalents.MoonkinForm > 0) ?
                                            0.01f * character.DruidTalents.MasterShapeshifter : 0.0f
            };

            Stats statsTalents = statsHotW + statsImpMotW + statsLivingSpirit + statsSotF + statsFuror + statsEarthAndMoon + statsMasterSS;

            // Create the total stats object
            Stats statsTotal = statsGearEnchantsBuffs + statsRace + statsTalents;

            // Base stats: Intellect, Stamina, Spirit, Agility
			statsTotal.Stamina = (float)Math.Floor(statsRace.Stamina * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Stamina += (float)Math.Floor(statsGearEnchantsBuffs.Stamina * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsRace.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Intellect += (float)Math.Floor(statsGearEnchantsBuffs.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Agility = (float)Math.Floor(statsRace.Agility * (1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Agility += (float)Math.Floor(statsGearEnchantsBuffs.Agility * (1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Spirit = (float)Math.Floor(statsRace.Spirit * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.Spirit += (float)Math.Floor(statsGearEnchantsBuffs.Spirit * (1 + statsTotal.BonusSpiritMultiplier));

            // Derived stats: Health, mana pool, armor
            statsTotal.Health = (float)Math.Round(((statsRace.Health * (character.Race == CharacterRace.Tauren ? 1.05f : 1f) + statsGearEnchantsBuffs.Health + statsTotal.Stamina * 10f))) - 180;
            statsTotal.Mana = (float)Math.Round(statsRace.Mana + 15f * statsTotal.Intellect) - 280;
			if (character.DruidTalents.MoonkinForm > 0)
			{
				statsTotal.Armor = (float)Math.Round((statsBaseGear.Armor/* + statsEnchants.Armor*/) * 4.7f + statsBuffs.Armor + statsTotal.Agility * 2f);
			}
			else
			{
				statsTotal.Armor = (float)Math.Round(statsGearEnchantsBuffs.Armor + statsTotal.Agility * 2f);
			}

            // Regen mechanic: mp5 +((0.1 * Intensity) * Spiritmp5())
            statsTotal.SpellCombatManaRegeneration += (float)Math.Round(character.DruidTalents.Intensity / 6.0f, 2);
            // Regen mechanic: mp5 +(0.04/0.07/0.10) * Int)
            statsTotal.Mp5 += (int)(statsTotal.Intellect * Math.Ceiling(character.DruidTalents.Dreamstate * 10 / 3.0f) / 100.0f);

            // Hit rating
            // All spells: Hit% +(0.02 * Balance of Power)
            statsTotal.SpellHit += 0.02f * character.DruidTalents.BalanceOfPower;

            // Crit rating
            // Application order: Stats, Talents, Gear
            // All spells: Crit% + (0.01 * Natural Perfection)
            statsTotal.SpellCrit += 0.01f * character.DruidTalents.NaturalPerfection;
            // All spells: Haste% + (0.01 * Celestial Focus)
            statsTotal.SpellHaste += 0.01f * character.DruidTalents.CelestialFocus;

            // All spells: Spell Power + (0.5 * Imp Moonkin) * Spirit
            // Add the crit bonus from the idol, if present
            if (character.DruidTalents.MoonkinForm > 0)
            {
                statsTotal.CritRating += statsTotal.IdolCritRating;
                statsTotal.SpellDamageFromSpiritPercentage += (0.1f * character.DruidTalents.ImprovedMoonkinForm);
            }
            // All spells: Damage +(0.04 * Lunar Guidance * Int)
            statsTotal.SpellDamageFromIntellectPercentage += 0.04f * character.DruidTalents.LunarGuidance;
            // All spells: Crit% + (0.01 * Improved Faerie Fire)
            if (character.ActiveBuffsContains("Faerie Fire"))
            {
                statsTotal.SpellCrit += 0.01f * character.DruidTalents.ImprovedFaerieFire;
            }

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
                                if (pairs.Key == maxRot.Name.Replace("Filler", "SF") ||
                                    pairs.Key == maxRot.Name.Replace("Filler", "W"))
                                {
                                    mp5 = (pairs.Value.ManaUsed - maxRot.ManaUsed) / maxRot.Duration * 5.0f;
                                    manaGain = (maxRot.ManaGained - pairs.Value.ManaGained) / maxRot.Duration * 5.0f;
                                }
                            }
                            compsMP5.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = LookupDruidTalentName(i),
                                OverallPoints = (mp5 >= 0 ? mp5 : 0.0f) + manaGain,
                                RawDamagePoints = (mp5 >= 0 ? mp5 : 0.0f),
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

                    List<ComparisonCalculationMoonkin> manaGainsList = new List<ComparisonCalculationMoonkin>();

                    // Moonkin form
                    c2.DruidTalents.MoonkinForm = 0;
                    CharacterCalculationsMoonkin calcsManaMoonkin = GetCharacterCalculations(c2) as CharacterCalculationsMoonkin;
                    c2.DruidTalents.MoonkinForm = 1;
                    foreach (KeyValuePair<string, RotationData> pairs in calcsManaMoonkin.Rotations)
                    {
                        if (pairs.Key == manaGainsRot.Name.Replace("Filler", "SF"))
                        {
                            manaGainsList.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = "Moonkin Form",
                                OverallPoints = (manaGainsRot.ManaGained - pairs.Value.ManaGained) / manaGainsRot.Duration * calcsManaBase.FightLength * 60.0f,
                                DamagePoints = (manaGainsRot.ManaGained - pairs.Value.ManaGained) / manaGainsRot.Duration * calcsManaBase.FightLength * 60.0f,
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
                    }
                    CharacterCalculationsMoonkin calcsManaJoW = GetCharacterCalculations(c2) as CharacterCalculationsMoonkin;
                    if (character.ActiveBuffsContains("Judgement of Wisdom"))
                    {
                        c2.ActiveBuffsAdd(jow);
                    }
                    foreach (KeyValuePair<string, RotationData> pairs in calcsManaJoW.Rotations)
                    {
                        if (pairs.Key == manaGainsRot.Name.Replace("Filler", "SF"))
                        {
                            manaGainsList.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = "Judgement of Wisdom",
                                OverallPoints = (manaGainsRot.ManaGained - pairs.Value.ManaGained) / manaGainsRot.Duration * calcsManaBase.FightLength * 60.0f,
                                DamagePoints = (manaGainsRot.ManaGained - pairs.Value.ManaGained) / manaGainsRot.Duration * calcsManaBase.FightLength * 60.0f,
                                RawDamagePoints = 0
                            });
                        }
                    }

                    // Replenishment
                    CalculationOptionsMoonkin calcOpts = c2.CalculationOptions as CalculationOptionsMoonkin;
                    float oldReplenishmentUptime = calcOpts.ReplenishmentUptime;
                    calcOpts.ReplenishmentUptime = 0.0f;
                    CharacterCalculationsMoonkin calcsManaReplenishment = GetCharacterCalculations(c2) as CharacterCalculationsMoonkin;
                    calcOpts.ReplenishmentUptime = oldReplenishmentUptime;
                    foreach (KeyValuePair<string, RotationData> pairs in calcsManaReplenishment.Rotations)
                    {
                        if (pairs.Key == manaGainsRot.Name.Replace("Filler", "SF"))
                        {
                            manaGainsList.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = "Replenishment",
                                OverallPoints = (manaGainsRot.ManaGained - pairs.Value.ManaGained) / manaGainsRot.Duration * calcsManaBase.FightLength * 60.0f,
                                DamagePoints = (manaGainsRot.ManaGained - pairs.Value.ManaGained) / manaGainsRot.Duration * calcsManaBase.FightLength * 60.0f,
                                RawDamagePoints = 0
                            });
                        }
                    }

                    // Innervate
                    bool innervate = calcOpts.Innervate;
                    calcOpts.Innervate = false;
                    CharacterCalculationsMoonkin calcsManaInnervate = GetCharacterCalculations(c2) as CharacterCalculationsMoonkin;
                    calcOpts.Innervate = innervate;
                    foreach (KeyValuePair<string, RotationData> pairs in calcsManaInnervate.Rotations)
                    {
                        if (pairs.Key == manaGainsRot.Name.Replace("Filler", "SF"))
                        {
                            manaGainsList.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = "Innervate",
                                OverallPoints = (manaGainsRot.ManaGained - pairs.Value.ManaGained) / manaGainsRot.Duration * calcsManaBase.FightLength * 60.0f,
                                DamagePoints = (manaGainsRot.ManaGained - pairs.Value.ManaGained) / manaGainsRot.Duration * calcsManaBase.FightLength * 60.0f,
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
                    }
                    CharacterCalculationsMoonkin calcsManaManaSpring = GetCharacterCalculations(c2) as CharacterCalculationsMoonkin;
                    if (character.ActiveBuffsContains("Mana Spring Totem"))
                    {
                        c2.ActiveBuffsAdd(manaSpring);
                    }
                    foreach (KeyValuePair<string, RotationData> pairs in calcsManaManaSpring.Rotations)
                    {
                        if (pairs.Key == manaGainsRot.Name.Replace("Filler", "SF"))
                        {
                            manaGainsList.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = "Mana Spring Totem",
                                OverallPoints = (manaGainsRot.ManaGained - pairs.Value.ManaGained) / manaGainsRot.Duration * calcsManaBase.FightLength * 60.0f,
                                DamagePoints = (manaGainsRot.ManaGained - pairs.Value.ManaGained) / manaGainsRot.Duration * calcsManaBase.FightLength * 60.0f,
                                RawDamagePoints = 0
                            });
                        }
                    }

                    // mp5
                    manaGainsList.Add(new ComparisonCalculationMoonkin()
                    {
                        Name = "MP5",
                        OverallPoints = calcsManaBase.FightLength * 60.0f * calcsManaBase.ManaRegen5SR,
                        DamagePoints = calcsManaBase.FightLength * 60.0f * calcsManaBase.ManaRegen5SR,
                        RawDamagePoints = 0
                    });

                    return manaGainsList.ToArray();
                case "Damage per Cast Time":
                    CharacterCalculationsMoonkin calcsBase = GetCharacterCalculations(character) as CharacterCalculationsMoonkin;
                    ComparisonCalculationMoonkin sf = new ComparisonCalculationMoonkin() { Name = "Starfire" };
                    ComparisonCalculationMoonkin mf = new ComparisonCalculationMoonkin() { Name = "Moonfire" };
                    ComparisonCalculationMoonkin iSw = new ComparisonCalculationMoonkin() { Name = "Insect Swarm " };
                    ComparisonCalculationMoonkin wr = new ComparisonCalculationMoonkin() { Name = "Wrath" };
                    sf.DamagePoints = calcsBase.SelectedRotation.Solver.Starfire.DamagePerHit / calcsBase.SelectedRotation.Solver.Starfire.CastTime;
                    sf.RawDamagePoints = calcsBase.BurstDPSRotation.Solver.Starfire.DamagePerHit / calcsBase.BurstDPSRotation.Solver.Starfire.CastTime;
                    sf.OverallPoints = sf.DamagePoints + sf.RawDamagePoints;
                    mf.DamagePoints = (calcsBase.SelectedRotation.Solver.Moonfire.DamagePerHit + calcsBase.SelectedRotation.Solver.Moonfire.DotEffect.DamagePerHit) / calcsBase.SelectedRotation.Solver.Moonfire.CastTime;
                    mf.RawDamagePoints = (calcsBase.BurstDPSRotation.Solver.Moonfire.DamagePerHit + calcsBase.BurstDPSRotation.Solver.Moonfire.DotEffect.DamagePerHit) / calcsBase.BurstDPSRotation.Solver.Moonfire.CastTime;
                    mf.OverallPoints = mf.DamagePoints + mf.RawDamagePoints;
                    iSw.DamagePoints = calcsBase.SelectedRotation.Solver.InsectSwarm.DotEffect.DamagePerHit / calcsBase.SelectedRotation.Solver.InsectSwarm.CastTime;
                    iSw.RawDamagePoints = calcsBase.BurstDPSRotation.Solver.InsectSwarm.DotEffect.DamagePerHit / calcsBase.BurstDPSRotation.Solver.InsectSwarm.CastTime;
                    iSw.OverallPoints = iSw.DamagePoints + iSw.RawDamagePoints;
                    wr.DamagePoints = calcsBase.SelectedRotation.Solver.Wrath.DamagePerHit / calcsBase.SelectedRotation.Solver.Wrath.CastTime;
                    wr.RawDamagePoints = calcsBase.BurstDPSRotation.Solver.Wrath.DamagePerHit / calcsBase.BurstDPSRotation.Solver.Wrath.CastTime;
                    wr.OverallPoints = wr.DamagePoints + wr.RawDamagePoints;
                    return new ComparisonCalculationMoonkin[] { sf, mf, iSw, wr };
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
				case 27:
					return "Starfall";
				case 58:
					return "Improved Mark of the Wild";
                case 60:
                    return "Furor";
                case 63:
                    return "Natural Shapeshifter";
                case 64:
                    return "Intensity";
                case 65:
                    return "Omen of Clarity";
                case 66:
                    return "Master Shapeshifter";
                default:
                    return "Not implemented";
            }
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
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
                Armor = stats.Armor,
                SpellCombatManaRegeneration = stats.SpellCombatManaRegeneration,
                StarfireDmg = stats.StarfireDmg,
                MoonfireDmg = stats.MoonfireDmg,
                WrathDmg = stats.WrathDmg,
                IdolCritRating = stats.IdolCritRating,
                UnseenMoonDamageBonus = stats.UnseenMoonDamageBonus,
                InnervateCooldownReduction = stats.InnervateCooldownReduction,
                StarfireBonusWithDot = stats.StarfireBonusWithDot,
                MoonfireExtension = stats.MoonfireExtension,
                BonusInsectSwarmDamage = stats.BonusInsectSwarmDamage,
                BonusNukeCritChance = stats.BonusNukeCritChance,
                StarfireCritChance = stats.StarfireCritChance,
                BonusManaPotion = stats.BonusManaPotion,
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,
                ManaRestoreFromBaseManaPPM  = stats.ManaRestoreFromBaseManaPPM,
                SpellHaste = stats.SpellHaste,
                SpellCrit = stats.SpellCrit,
                SpellCritOnTarget = stats.SpellCritOnTarget,
                SpellHit = stats.SpellHit,
                ArmorPenetration = stats.ArmorPenetration,
                EclipseBonus = stats.EclipseBonus,
                InsectSwarmDmg = stats.InsectSwarmDmg,
                MoonfireDotCrit = stats.MoonfireDotCrit,
                BonusMoonkinNukeDamage = stats.BonusMoonkinNukeDamage,
                MoonkinT10CritDot = stats.MoonkinT10CritDot
            };
            // Add special effects that meet the following criteria:
            // 1) On-use OR
            // 2) On damaging spell hit/crit/cast OR
            // 3) On all spell hit/crit/cast/miss, AND
            // 4) Proc is spell power OR
            // 5) Proc is spell crit OR
            // 6) Proc is spell haste
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use ||
                    effect.Trigger == Trigger.DamageSpellCast ||
                    effect.Trigger == Trigger.DamageSpellCrit ||
                    effect.Trigger == Trigger.DamageSpellHit ||
                    effect.Trigger == Trigger.SpellCast ||
                    effect.Trigger == Trigger.SpellCrit ||
                    effect.Trigger == Trigger.SpellHit ||
                    effect.Trigger == Trigger.SpellMiss ||
                    effect.Trigger == Trigger.DoTTick ||
                    effect.Trigger == Trigger.DamageDone ||
                    effect.Trigger == Trigger.DamageOrHealingDone ||
                    effect.Trigger == Trigger.InsectSwarmOrMoonfireTick ||
                    effect.Trigger == Trigger.InsectSwarmTick ||
                    effect.Trigger == Trigger.MoonfireTick ||
                    effect.Trigger == Trigger.MoonfireCast)
                {
                    if (effect.Stats.SpellPower > 0 ||
                        effect.Stats.CritRating > 0 ||
                        effect.Stats.HasteRating > 0 ||
                        effect.Stats.SpellHaste > 0 ||
                        effect.Stats.HighestStat > 0 ||
                        effect.Stats.ShadowDamage > 0 ||
                        effect.Stats.NatureDamage > 0 ||
                        effect.Stats.FireDamage > 0 ||
                        effect.Stats.StarfireProc > 0 ||
                        effect.Stats.Spirit > 0 ||
                        effect.Stats.Mp5 > 0 ||
                        effect.Stats.BonusArcaneDamageMultiplier > 0 ||
                        effect.Stats.BonusNatureDamageMultiplier > 0)
                    {
                        s.AddSpecialEffect(effect);
                    }
                }
            }
            return s;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            // Check for special effects that meet the following criteria:
            // 1) On-use OR
            // 2) On damaging spell hit/crit/cast OR
            // 3) On all spell hit/crit/cast/miss, AND
            // 4) Proc is spell power OR
            // 5) Proc is spell crit OR
            // 6) Proc is spell haste
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use ||
                    effect.Trigger == Trigger.DamageSpellCast ||
                    effect.Trigger == Trigger.DamageSpellCrit ||
                    effect.Trigger == Trigger.DamageSpellHit ||
                    effect.Trigger == Trigger.SpellCast ||
                    effect.Trigger == Trigger.SpellCrit ||
                    effect.Trigger == Trigger.SpellHit ||
                    effect.Trigger == Trigger.SpellMiss ||
                    effect.Trigger == Trigger.DoTTick ||
                    effect.Trigger == Trigger.DamageDone ||
                    effect.Trigger == Trigger.DamageOrHealingDone ||
                    effect.Trigger == Trigger.InsectSwarmTick ||
                    effect.Trigger == Trigger.InsectSwarmOrMoonfireTick ||
                    effect.Trigger == Trigger.MoonfireTick ||
                    effect.Trigger == Trigger.MoonfireCast)
                {
                    if (effect.Stats.SpellPower > 0 ||
                        effect.Stats.CritRating > 0 ||
                        effect.Stats.HasteRating > 0 ||
                        effect.Stats.SpellHaste > 0 ||
                        effect.Stats.HighestStat > 0 ||
                        effect.Stats.ShadowDamage > 0 ||
                        effect.Stats.NatureDamage > 0 ||
                        effect.Stats.FireDamage > 0 ||
                        effect.Stats.StarfireProc > 0 ||
                        effect.Stats.Spirit > 0 ||
                        effect.Stats.Mp5 > 0 ||
                        effect.Stats.BonusArcaneDamageMultiplier > 0 ||
                        effect.Stats.BonusNatureDamageMultiplier > 0)
                    {
                        return true;
                    }
                }
            }

            return stats.ToString().Equals("") || (stats.Stamina + stats.Intellect + stats.Spirit + stats.Agility + stats.Health
                + stats.Mp5 + stats.CritRating + stats.SpellCrit + stats.SpellCritOnTarget + stats.SpellPower + stats.HasteRating + stats.SpellHaste
                + stats.HitRating + stats.SpellHit + stats.BonusAgilityMultiplier + stats.BonusIntellectMultiplier
                + stats.BonusSpellCritMultiplier + stats.BonusSpellPowerMultiplier + stats.BonusArcaneDamageMultiplier
                + stats.BonusNatureDamageMultiplier + stats.BonusStaminaMultiplier + stats.BonusSpiritMultiplier
                + stats.Mana + stats.SpellCombatManaRegeneration + stats.ManaRestoreFromBaseManaPPM
                + stats.SpellDamageFromIntellectPercentage + stats.SpellDamageFromSpiritPercentage + stats.StarfireDmg
                + stats.MoonfireDmg + stats.WrathDmg + stats.IdolCritRating + stats.UnseenMoonDamageBonus
                + stats.StarfireCritChance + stats.MoonfireExtension + stats.InnervateCooldownReduction + stats.StarfireBonusWithDot
                + stats.BonusManaPotion + stats.ManaRestoreFromMaxManaPerSecond + stats.BonusDamageMultiplier + stats.ArmorPenetration
                + stats.BonusNukeCritChance + stats.BonusInsectSwarmDamage + stats.EclipseBonus + stats.InsectSwarmDmg
                + stats.MoonfireDotCrit + stats.BonusMoonkinNukeDamage + stats.MoonkinT10CritDot) > 0;
        }

        public Stats GetBuffsStats(Character character, CalculationOptionsMoonkin calcOpts) {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            //float hasRelevantBuff;

            #region Racials to Force Enable
            // Draenei should always have this buff activated
            // NOTE: for other races we don't wanna take it off if the user has it active, so not adding code for that
            if (character.Race == CharacterRace.Draenei
                && !character.ActiveBuffs.Contains(Buff.GetBuffByName("Heroic Presence")))
            {
                character.ActiveBuffsAdd(("Heroic Presence"));
            }
            #endregion

            #region Passive Ability Auto-Fixing
            // Removes the Trueshot Aura Buff and it's equivalents Unleashed Rage and Abomination's Might if you are
            // maintaining it yourself. We are now calculating this internally for better accuracy and to provide
            // value to relevant talents
            /*{
                hasRelevantBuff = character.HunterTalents.TrueshotAura;
                Buff a = Buff.GetBuffByName("Trueshot Aura");
                Buff b = Buff.GetBuffByName("Unleashed Rage");
                Buff c = Buff.GetBuffByName("Abomination's Might");
                if (hasRelevantBuff > 0)
                {
                    if (character.ActiveBuffs.Contains(a)) { character.ActiveBuffs.Remove(a); removedBuffs.Add(a); }
                    if (character.ActiveBuffs.Contains(b)) { character.ActiveBuffs.Remove(b); removedBuffs.Add(b); }
                    if (character.ActiveBuffs.Contains(c)) { character.ActiveBuffs.Remove(c); removedBuffs.Add(c); }
                }
            }
            // Removes the Hunter's Mark Buff and it's Children 'Glyphed', 'Improved' and 'Both' if you are
            // maintaining it yourself. We are now calculating this internally for better accuracy and to provide
            // value to relevant talents
            {
                hasRelevantBuff =  character.HunterTalents.ImprovedHuntersMark
                                + (character.HunterTalents.GlyphOfHuntersMark ? 1 : 0);
                Buff a = Buff.GetBuffByName("Hunter's Mark");
                Buff b = Buff.GetBuffByName("Glyphed Hunter's Mark");
                Buff c = Buff.GetBuffByName("Improved Hunter's Mark");
                Buff d = Buff.GetBuffByName("Improved and Glyphed Hunter's Mark");
                // Since we are doing base Hunter's mark ourselves, we still don't want to double-dip
                if (character.ActiveBuffs.Contains(a)) { character.ActiveBuffs.Remove(a); /*removedBuffs.Add(a);*//* }
                // If we have an enhanced Hunter's Mark, kill the Buff
                if (hasRelevantBuff > 0) {
                    if (character.ActiveBuffs.Contains(b)) { character.ActiveBuffs.Remove(b); /*removedBuffs.Add(b);*//* }
                    if (character.ActiveBuffs.Contains(c)) { character.ActiveBuffs.Remove(c); /*removedBuffs.Add(c);*//* }
                    if (character.ActiveBuffs.Contains(d)) { character.ActiveBuffs.Remove(d); /*removedBuffs.Add(c);*//* }
                }
            }*/
            #endregion

            #region Special Pot Handling
            /*foreach (Buff potionBuff in character.ActiveBuffs.FindAll(b => b.Name.Contains("Potion")))
            {
                if (potionBuff.Stats._rawSpecialEffectData != null
                    && potionBuff.Stats._rawSpecialEffectData[0] != null)
                {
                    Stats newStats = new Stats();
                    newStats.AddSpecialEffect(new SpecialEffect(potionBuff.Stats._rawSpecialEffectData[0].Trigger,
                                                                potionBuff.Stats._rawSpecialEffectData[0].Stats,
                                                                potionBuff.Stats._rawSpecialEffectData[0].Duration,
                                                                calcOpts.Duration,
                                                                potionBuff.Stats._rawSpecialEffectData[0].Chance,
                                                                potionBuff.Stats._rawSpecialEffectData[0].MaxStack));

                    Buff newBuff = new Buff() { Stats = newStats };
                    character.ActiveBuffs.Remove(potionBuff);
                    character.ActiveBuffsAdd(newBuff);
                    removedBuffs.Add(potionBuff);
                    addedBuffs.Add(newBuff);
                }
            }*/
            #endregion

            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            foreach (Buff b in removedBuffs) {
                character.ActiveBuffsAdd(b);
            }
            foreach (Buff b in addedBuffs) {
                character.ActiveBuffs.Remove(b);
            }

            return statsBuffs;
        }
    }
}
