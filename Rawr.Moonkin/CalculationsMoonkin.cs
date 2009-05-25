using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Moonkin
{
	[Rawr.Calculations.RawrModelInfo("Moonkin", "Spell_Nature_ForceOfNature", Character.CharacterClass.Druid)]
	class CalculationsMoonkin : CalculationsBase
    {
        public override bool SupportsMultithreading
        {
            get
            {
                return false;
            }
        }
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
					// Uncommon gems
					// All but blue = Runed Bloodstone
					// Blue = Runed Dragon's Eye (Jewelcrafting)
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Uncommon",
						RedId = 39911,
						YellowId = 39911,
						BlueId = 42144,
						PrismaticId = 39911,
						MetaId = 41285
					},
					// Perfect uncommon gems
					// All but blue = Perfect Runed Bloodstone
					// Blue = Runed Dragon's Eye (Jewelcrafting)
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Uncommon",
						RedId = 41438,
						YellowId = 41438,
						BlueId = 42144,
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
						MetaId = 41285,
                        Enabled = true
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
						MetaId = 41285,
                        Enabled = true
					},
					// Rare gems
					// All but blue = Runed Scarlet Ruby
					// Blue = Runed Dragon's Eye (Jewelcrafting)
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Rare",
						RedId = 39998,
						YellowId = 39998,
						BlueId = 42144,
						PrismaticId = 39998,
						MetaId = 41285
					}
				};
            }
        }
        public override bool EnchantFitsInSlot(Enchant enchant, Character character, Item.ItemSlot slot)
        {
            if (slot == Item.ItemSlot.OffHand || slot == Item.ItemSlot.Ranged) return false;
            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        public override bool ItemFitsInSlot(Item item, Character character, Character.CharacterSlot slot, bool ignoreUnique)
        {
            if (slot == Character.CharacterSlot.OffHand && item.Slot == Item.ItemSlot.OneHand) return false;
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
        private Dictionary<string, System.Drawing.Color> subColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (subColors == null)
                {
                    subColors = new Dictionary<string, System.Drawing.Color>();
                    subColors.Add("Sustained Damage", System.Drawing.Color.Blue);
                    subColors.Add("Burst Damage", System.Drawing.Color.Red);
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
                    "Summary Stats:Sustained Damage Points",
                    "Summary Stats:Burst Damage Points",
                    "Summary Stats:Overall Points",
                    "Spell Stats:Spell Power",
                    "Spell Stats:Spell Hit",
                    "Spell Stats:Spell Crit",
                    "Spell Stats:Spell Haste",
                    "Mana Regeneration:O5SR Per Second",
                    "Mana Regeneration:I5SR Per Second",
                    "Spell Info:Sustained DPS Rotation",
                    "Spell Info:Burst DPS Rotation",
                    "Spell Info:SF Spam BDPS",
                    "Spell Info:SF Spam DPS",
                    "Spell Info:SF Spam DPM",
                    "Spell Info:SF Spam OOM",
                    "Spell Info:W Spam BDPS",
                    "Spell Info:W Spam DPS",
                    "Spell Info:W Spam DPM",
                    "Spell Info:W Spam OOM",
                    "Spell Info:MF/SF BDPS",
                    "Spell Info:MF/SF DPS",
                    "Spell Info:MF/SF DPM",
                    "Spell Info:MF/SF OOM",
                    "Spell Info:MF/W BDPS",
                    "Spell Info:MF/W DPS",
                    "Spell Info:MF/W DPM",
                    "Spell Info:MF/W OOM",
                    "Spell Info:IS/SF BDPS",
                    "Spell Info:IS/SF DPS",
                    "Spell Info:IS/SF DPM",
                    "Spell Info:IS/SF OOM",
                    "Spell Info:IS/W BDPS",
                    "Spell Info:IS/W DPS",
                    "Spell Info:IS/W DPM",
                    "Spell Info:IS/W OOM",
                    "Spell Info:IS/MF/SF BDPS",
                    "Spell Info:IS/MF/SF DPS",
                    "Spell Info:IS/MF/SF DPM",
                    "Spell Info:IS/MF/SF OOM",
                    "Spell Info:IS/MF/W BDPS",
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
                    "Damage per Cast Time"
					//"Relative Stat Values"
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

        public static CharacterCalculationsMoonkin GetInnerCharacterCalculations(Character character, Stats stats, Item additionalItem)
        {
            CharacterCalculationsMoonkin calcs = new CharacterCalculationsMoonkin();
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            calcs.BasicStats = stats;

            calcs.SpellCrit = 0.0185f + StatConversion.GetSpellCritFromIntellect(stats.Intellect) + StatConversion.GetSpellCritFromRating(stats.CritRating) + stats.SpellCrit;
            calcs.SpellHit = StatConversion.GetSpellHitFromRating(stats.HitRating) + stats.SpellHit;
            calcs.SpellHaste = (1 + StatConversion.GetSpellHasteFromRating(stats.HasteRating)) * (1 + stats.SpellHaste) * (1 + stats.Bloodlust) - 1;

            // Fix for rounding error in converting partial points of int/spirit to spell power
            float spellPowerFromStats = (float)Math.Floor(stats.SpellDamageFromIntellectPercentage * stats.Intellect) +
                (float)Math.Floor(stats.SpellDamageFromSpiritPercentage * stats.Spirit);
            calcs.SpellPower = stats.SpellPower + spellPowerFromStats;

            calcs.Latency = calcOpts.Latency;
            calcs.FightLength = calcOpts.FightLength;
            calcs.TargetLevel = calcOpts.TargetLevel;
            calcs.Scryer = calcOpts.AldorScryer == "Scryer";

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
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

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
                BonusIntellectMultiplier = (character.ActiveBuffsContains("Moonkin Form")) ? 0.02f * character.DruidTalents.Furor : 0.0f
            };

            Stats statsEarthAndMoon = new Stats()
            {
                BonusSpellPowerMultiplier = 0.01f * character.DruidTalents.EarthAndMoon
            };

            Stats statsMasterSS = new Stats()
            {
                BonusSpellPowerMultiplier = (character.ActiveBuffsContains("Moonkin Form") && character.DruidTalents.MoonkinForm > 0) ?
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

            /*foreach (SpecialEffect s in statsTotal.SpecialEffects())
            {
                if (s.Stats.HighestStat > 0)
                {
                    if (statsTotal.Spirit > statsTotal.Intellect)
                        statsTotal.Spirit += s.Stats.HighestStat * s.GetAverageUptime(1.5f, 1f);
                    else
                        statsTotal.Intellect += s.Stats.HighestStat * s.GetAverageUptime(1.5f, 1f);
                }
                if (s.Stats.Spirit > 0 && s.MaxStack > 1)
                {
                    statsTotal.Spirit += s.Stats.Spirit * s.MaxStack;
                }
                else if (s.Stats.Spirit > 0)
                {
                    statsTotal.Spirit += s.Stats.Spirit * s.GetAverageUptime(0f, 1f);
                }
            }*/

            // Derived stats: Health, mana pool, armor
            statsTotal.Health = (float)Math.Round(((statsRace.Health * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f) + statsGearEnchantsBuffs.Health + statsTotal.Stamina * 10f))) - 180;
            statsTotal.Mana = (float)Math.Round(statsRace.Mana + 15f * statsTotal.Intellect) - 280;
			if (character.ActiveBuffsContains("Moonkin Form") && character.DruidTalents.MoonkinForm > 0)
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
            if (character.ActiveBuffsContains("Moonkin Form") && character.DruidTalents.MoonkinForm > 0)
            {
                statsTotal.CritRating += statsTotal.IdolCritRating;
                if (character.ActiveBuffsContains("Improved Moonkin Form"))
                {
                    statsTotal.SpellDamageFromSpiritPercentage += (0.1f * character.DruidTalents.ImprovedMoonkinForm);
                }
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
                                if (pairs.Key == maxRot.Name)
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
                        if (pairs.Key == manaGainsRot.Name)
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
                        c2.ActiveBuffs.Add(jow);
                    }
                    foreach (KeyValuePair<string, RotationData> pairs in calcsManaJoW.Rotations)
                    {
                        if (pairs.Key == manaGainsRot.Name)
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
                        if (pairs.Key == manaGainsRot.Name)
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
                        if (pairs.Key == manaGainsRot.Name)
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
                        c2.ActiveBuffs.Add(manaSpring);
                    }
                    foreach (KeyValuePair<string, RotationData> pairs in calcsManaManaSpring.Rotations)
                    {
                        if (pairs.Key == manaGainsRot.Name)
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
                LightningCapacitorProc = stats.LightningCapacitorProc,
                InnervateCooldownReduction = stats.InnervateCooldownReduction,
                StarfireBonusWithDot = stats.StarfireBonusWithDot,
                MoonfireExtension = stats.MoonfireExtension,
                BonusInsectSwarmDamage = stats.BonusInsectSwarmDamage,
                BonusNukeCritChance = stats.BonusNukeCritChance,
                StarfireCritChance = stats.StarfireCritChance,
                BonusManaPotion = stats.BonusManaPotion,
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,
                ManaRestoreFromBaseManaPerHit  = stats.ManaRestoreFromBaseManaPerHit,
                SpellHaste = stats.SpellHaste,
                SpellCrit = stats.SpellCrit,
                SpellHit = stats.SpellHit,
                ArmorPenetration = stats.ArmorPenetration,
                Bloodlust = stats.Bloodlust,
                ThunderCapacitorProc = stats.ThunderCapacitorProc,
                EclipseBonus = stats.EclipseBonus,
                InsectSwarmDmg = stats.InsectSwarmDmg
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
                    effect.Trigger == Trigger.InsectSwarmTick)
                {
                    if (effect.Stats.SpellPower > 0 ||
                        effect.Stats.CritRating > 0 ||
                        effect.Stats.HasteRating > 0 ||
                        effect.Stats.HighestStat > 0 ||
                        effect.Stats.ShadowDamage > 0 ||
                        effect.Stats.StarfireProc > 0 ||
                        effect.Stats.Spirit > 0 ||
                        effect.Stats.Mp5 > 0)
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
                    effect.Trigger == Trigger.InsectSwarmTick)
                {
                    if (effect.Stats.SpellPower > 0 ||
                        effect.Stats.CritRating > 0 ||
                        effect.Stats.HasteRating > 0 ||
                        effect.Stats.HighestStat > 0 ||
                        effect.Stats.ShadowDamage > 0 ||
                        effect.Stats.StarfireProc > 0 ||
                        effect.Stats.Spirit > 0 ||
                        effect.Stats.Mp5 > 0)
                    {
                        return true;
                    }
                }
            }

            return stats.ToString().Equals("") || (stats.Stamina + stats.Intellect + stats.Spirit + stats.Agility + stats.Health
                + stats.Mp5 + stats.CritRating + stats.SpellCrit + stats.SpellPower + stats.HasteRating + stats.SpellHaste
                + stats.HitRating + stats.SpellHit + stats.BonusAgilityMultiplier + stats.BonusIntellectMultiplier
                + stats.BonusSpellCritMultiplier + stats.BonusSpellPowerMultiplier + stats.BonusArcaneDamageMultiplier
                + stats.BonusNatureDamageMultiplier + stats.BonusStaminaMultiplier + stats.BonusSpiritMultiplier
                + stats.Mana + stats.SpellCombatManaRegeneration + stats.ManaRestoreFromBaseManaPerHit
                + stats.SpellDamageFromIntellectPercentage + stats.SpellDamageFromSpiritPercentage + stats.StarfireDmg
                + stats.MoonfireDmg + stats.WrathDmg + stats.IdolCritRating + stats.UnseenMoonDamageBonus + stats.LightningCapacitorProc
                + stats.StarfireCritChance + stats.MoonfireExtension + stats.InnervateCooldownReduction + stats.StarfireBonusWithDot
                + stats.BonusManaPotion + stats.ManaRestoreFromMaxManaPerSecond + stats.BonusDamageMultiplier + stats.ArmorPenetration
                + stats.Bloodlust + stats.BonusNukeCritChance + stats.BonusInsectSwarmDamage + stats.EclipseBonus + stats.InsectSwarmDmg) > 0;
        }
    }
}
