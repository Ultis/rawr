using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Moonkin
{
	[Rawr.Calculations.RawrModelInfo("Moonkin", "Spell_Nature_ForceOfNature", CharacterClass.Druid)]
	public class CalculationsMoonkin : CalculationsBase
    {
        /// <summary>
        /// Set default buffs.
        /// </summary>
        /// <param name="character">The Character to set buffs for.</param>
        public override void SetDefaults(Character character)
        {
            character.ActiveBuffsAdd(("Arcane Intellect"));
            character.ActiveBuffsAdd(("Blessing of Might"));
            character.ActiveBuffsAdd(("Moonkin Form"));
            character.ActiveBuffsAdd(("Wrath of Air Totem"));
            character.ActiveBuffsAdd(("Power Word: Fortitude"));
            character.ActiveBuffsAdd(("Mark of the Wild"));
            character.ActiveBuffsAdd(("Earth and Moon"));
            character.ActiveBuffsAdd(("Faerie Fire"));
            character.ActiveBuffsAdd(("Sunder Armor"));
            character.ActiveBuffsAdd(("Heroism/Bloodlust"));
            character.ActiveBuffsAdd(("Flask of the Frost Wyrm"));
            character.ActiveBuffsAdd(("Fish Feast"));
        }
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
                    "Crit Rating",
                    "Mastery Rating"
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
					// Red = Runed Scarlet Ruby
					// Blue = Purified Twilight Opal (spirit/spell power)
                    // Yellow = Reckless Monarch Topaz (spell power/haste)
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Rare",
						RedId = 39998,
						YellowId = 40051,
						BlueId = 40026,
						PrismaticId = 39998,
						MetaId = 41285
					},
					// All sockets but yellow = Runed Scarlet Ruby
                    // Yellow = Reckless Monarch Topaz (spell power/haste)
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Rare",
						RedId = 39998,
						YellowId = 40051,
						BlueId = 39998,
						PrismaticId = 39998,
						MetaId = 41285
					},
					// Red = Runed Scarlet Ruby
					// Blue = Purified Twilight Opal (spirit/spell power)
                    // Yellow = Veiled Monarch Topaz (spell power/hit)
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Rare",
						RedId = 39998,
						YellowId = 40049,
						BlueId = 40026,
						PrismaticId = 39998,
						MetaId = 41285
					},
					// All sockets but yellow = Runed Scarlet Ruby
                    // Yellow = Veiled Monarch Topaz (spell power/hit)
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Rare",
						RedId = 39998,
						YellowId = 40049,
						BlueId = 39998,
						PrismaticId = 39998,
						MetaId = 41285
					},
					// Red = Runed Scarlet Ruby
					// Blue = Purified Twilight Opal (spirit/spell power)
                    // Yellow = Potent Monarch Topaz (spell power/crit)
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Rare",
						RedId = 39998,
						YellowId = 40048,
						BlueId = 40026,
						PrismaticId = 39998,
						MetaId = 41285
					},
					// Rare gems
					// All sockets but yellow = Runed Scarlet Ruby
                    // Yellow = Potent Monarch Topaz (spell power/crit)
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Rare",
						RedId = 39998,
						YellowId = 40048,
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
					// Red = Runed Cardinal Ruby
					// Blue = Purified Dreadstone (spirit/spell power)
                    // Yellow = Reckless Ametrine (spell power/haste)
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Epic",
						RedId = 40113,
						YellowId = 40155,
						BlueId = 40133,
						PrismaticId = 40113,
						MetaId = 41285,
                        Enabled = true
					},
					// All sockets but yellow = Runed Cardinal Ruby
                    // Yellow = Reckless Ametrine (spell power/haste)
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Epic",
						RedId = 40113,
						YellowId = 40155,
						BlueId = 40113,
						PrismaticId = 40113,
						MetaId = 41285,
                        Enabled = true
					},
					// Red = Runed Cardinal Ruby
					// Blue = Purified Dreadstone (spirit/spell power)
                    // Yellow = Veiled Ametrine (spell power/hit)
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Epic",
						RedId = 40113,
						YellowId = 40153,
						BlueId = 40133,
						PrismaticId = 40113,
						MetaId = 41285,
                        Enabled = true
					},
					// All sockets but yellow = Runed Cardinal Ruby
                    // Yellow = Veiled Ametrine (spell power/hit)
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Epic",
						RedId = 40113,
						YellowId = 40153,
						BlueId = 40113,
						PrismaticId = 40113,
						MetaId = 41285,
                        Enabled = true
					},
					// Red = Runed Cardinal Ruby
					// Blue = Purified Dreadstone (spirit/spell power)
                    // Yellow = Potent Ametrine (spell power/crit)
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Epic",
						RedId = 40113,
						YellowId = 40152,
						BlueId = 40133,
						PrismaticId = 40113,
						MetaId = 41285,
                        Enabled = true
					},
					// All sockets but yellow = Runed Cardinal Ruby
                    // Yellow = Potent Ametrine (spell power/crit)
					// Meta = Chaotic
					new GemmingTemplate()
					{
						Model = "Moonkin",
						Group = "Epic",
						RedId = 40113,
						YellowId = 40152,
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
                // Prime glyphs
                _relevantGlyphs.Add("Glyph of Insect Swarm");
                _relevantGlyphs.Add("Glyph of Moonfire");
                _relevantGlyphs.Add("Glyph of Starfire");
                _relevantGlyphs.Add("Glyph of Wrath");
                // Major glyphs
                _relevantGlyphs.Add("Glyph of Starfall");
                _relevantGlyphs.Add("Glyph of Focus");
                _relevantGlyphs.Add("Glyph of Innervate");
            }
            return _relevantGlyphs;
        }

        public static float BaseMana = 3496.0f;
#if RAWR3 || RAWR4
        private Dictionary<string, System.Windows.Media.Color> subColors = null;
        public override Dictionary<string, System.Windows.Media.Color> SubPointNameColors
        {
            get
            {
                if (subColors == null)
                {
                    subColors = new Dictionary<string, System.Windows.Media.Color>();
                    subColors.Add("Sustained Damage", System.Windows.Media.Color.FromArgb(255, 0, 0, 255));
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
                    "Spell Stats:Mastery",
                    "Regen:Mana Regen",
                    "Solution:Total Score",
                    "Solution:Selected Rotation",
                    "Solution:Selected DPS",
                    "Solution:Selected Time To OOM",
                    "Solution:Selected Cycle Length",
                    "Solution:Selected Spell Breakdown",
                    "Spell Info:Starfire",
                    "Spell Info:Wrath",
                    "Spell Info:Starsurge",
                    "Spell Info:Moonfire",
                    "Spell Info:Insect Swarm",
                    "Spell Info:Starfall",
                    "Spell Info:Treants"
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

#if !RAWR3 && !RAWR4
        // for RAWR3 || RAWR4 include all charts in CustomChartNames
        private string[] _customRenderedChartNames = null;
        public override string[] CustomRenderedChartNames
        {
            get
            {
                if (_customRenderedChartNames == null)
                {
                    _customRenderedChartNames = new string[] { "Stats Graph" };
                }
                return _customRenderedChartNames;
            }
        }
#endif

#if RAWR3 || RAWR4
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

            // All spells: Damage +(1 * Int)
            float spellDamageFromIntPercent = 1f;
            // Fix for rounding error in converting partial points of int/spirit to spell power
            float spellPowerFromStats = (float)Math.Floor(spellDamageFromIntPercent * stats.Intellect);
            calcs.SpellPower = stats.SpellPower + spellPowerFromStats;

            calcs.Latency = calcOpts.Latency;

            // Mastery from rating
            calcs.Mastery = 8.0f + stats.MasteryRating / 93.0f;

#if RAWR3 || RAWR4
            BossOptions bossOpts = new BossOptions();
            if (character.BossOptions != null) bossOpts = character.BossOptions;
            calcs.FightLength = bossOpts.BerserkTimer / 60f;
            calcs.TargetLevel = bossOpts.Level;
#else
            calcs.FightLength = calcOpts.FightLength;
            calcs.TargetLevel = calcOpts.TargetLevel;
#endif

            // 3.1 spirit regen
            float spiritRegen = StatConversion.GetSpiritRegenSec(calcs.BasicStats.Spirit, calcs.BasicStats.Intellect);
            calcs.ManaRegen = spiritRegen + stats.Mp5 / 5f;

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
            Stats statsRace = null;
            if (character.Race == CharacterRace.NightElf)
            {
                statsRace = new Stats()
                {
                    Health = 7416f,
                    Mana = 3496f,
                    Stamina = 97f,
                    Agility = 87f,
                    Intellect = 143f,
                    Spirit = 157f
                };
            }
            else if (character.Race == CharacterRace.Tauren)
            {
                statsRace = new Stats()
                {
                    Health = 7416f,
                    Mana = 3496f,
                    Stamina = 99f,
                    Agility = 78f,
                    Intellect = 139f,
                    Spirit = 161f
                };
            }
            // TODO: When Cataclysm goes live, these blocks will need to be filled in
            /*else if (character.Race == CharacterRace.Troll)
            {
            }
            else if (character.Race == CharacterRace.Worgen)
            {
            }*/
            // Default to night elf, Because I Said So
            else
            {
                statsRace = new Stats()
                {
                    Health = 7416f,
                    Mana = 3496f,
                    Stamina = 97f,
                    Agility = 87f,
                    Intellect = 143f,
                    Spirit = 157f
                };
            }
            
            // Get the gear/enchants/buffs stats loaded in
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character, calcOpts);

            Stats statsGearEnchantsBuffs = statsBaseGear + statsBuffs;

            // Talented bonus multipliers

            Stats statsHotW = new Stats()
            {
                BonusIntellectMultiplier = 0.02f * character.DruidTalents.HeartOfTheWild
            };

            Stats statsFuror = new Stats()
            {
                BonusManaMultiplier = 0.05f * character.DruidTalents.Furor
            };

            Stats statsBalanceOfPower = new Stats()
            {
                BonusSpellPowerMultiplier = 0.01f * character.DruidTalents.BalanceOfPower
            };

            Stats statsEarthAndMoon = new Stats()
            {
                BonusSpellPowerMultiplier = 0.02f * character.DruidTalents.EarthAndMoon
            };

            Stats statsMoonkinForm = new Stats()
            {
                BaseArmorMultiplier = 1.2f * character.DruidTalents.MoonkinForm,
                BonusArcaneDamageMultiplier = 0.01f * character.DruidTalents.MoonkinForm,
                BonusNatureDamageMultiplier = 0.01f * character.DruidTalents.MoonkinForm
            };

            Stats statsMasterSS = new Stats()
            {
                BonusSpellPowerMultiplier = (character.DruidTalents.MoonkinForm > 0) ?
                                            0.04f * character.DruidTalents.MasterShapeshifter : 0.0f
            };

            Stats statsMoonfury = new Stats()
            {
                BonusArcaneDamageMultiplier = 0.25f,
                BonusNatureDamageMultiplier = 0.25f
            };

            Stats statsTalents = statsHotW + statsBalanceOfPower + statsFuror + statsEarthAndMoon + statsMoonkinForm + statsMasterSS + statsMoonfury;

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
            statsTotal.Health = (float)Math.Floor(statsTotal.Health * (1f + statsTotal.BonusHealthMultiplier));
            statsTotal.Mana = (float)Math.Round(statsRace.Mana + 15f * statsTotal.Intellect) - 280;
            statsTotal.Mana = (float)Math.Floor(statsTotal.Mana * (1f + statsTotal.BonusManaMultiplier));
            // Armor
            statsTotal.Armor = statsTotal.Armor * (1f + statsTotal.BaseArmorMultiplier);
            statsTotal.BonusArmor += statsTotal.Agility * 2f;
            statsTotal.BonusArmor = statsTotal.BonusArmor * (1f + statsTotal.BonusArmorMultiplier);
            statsTotal.Armor += statsTotal.BonusArmor;
            statsTotal.Armor = (float)Math.Round(statsTotal.Armor);

            // Crit rating
            // Application order: Stats, Talents, Gear
            // All spells: Crit% + (0.02 * Nature's Majesty)
            statsTotal.SpellCrit += 0.02f * character.DruidTalents.NaturesMajesty;
            // All spells: Hit rating + 0.5f * Balance of Power * Spirit
            statsTotal.HitRating += 0.5f * character.DruidTalents.BalanceOfPower * statsTotal.Spirit;

            // Mastery -> Eclipse bonus
            statsTotal.EclipseBonus += (8.0f + statsTotal.MasteryRating / 93f) * 0.015f;

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            
            switch (chartName)
            {
                case "Talent DPS Comparison":
                    return new ComparisonCalculationBase[0];
                case "Talent MP5 Comparison":
                    return new ComparisonCalculationBase[0];
                case "Mana Gains":
                    return new ComparisonCalculationBase[0];
                case "Damage per Cast Time":
                    CharacterCalculationsMoonkin calcsBase = GetCharacterCalculations(character) as CharacterCalculationsMoonkin;
                    ComparisonCalculationMoonkin sf = new ComparisonCalculationMoonkin() { Name = "Starfire" };
                    ComparisonCalculationMoonkin mf = new ComparisonCalculationMoonkin() { Name = "Moonfire" };
                    ComparisonCalculationMoonkin iSw = new ComparisonCalculationMoonkin() { Name = "Insect Swarm " };
                    ComparisonCalculationMoonkin wr = new ComparisonCalculationMoonkin() { Name = "Wrath" };
                    ComparisonCalculationMoonkin ss = new ComparisonCalculationMoonkin() { Name = "Starsurge" };
                    sf.DamagePoints = calcsBase.SelectedRotation.StarfireAvgHit / calcsBase.SelectedRotation.StarfireAvgCast;
                    sf.OverallPoints = sf.DamagePoints;
                    mf.DamagePoints = calcsBase.SelectedRotation.MoonfireAvgHit / calcsBase.SelectedRotation.AverageInstantCast;
                    mf.OverallPoints = mf.DamagePoints;
                    iSw.DamagePoints = calcsBase.SelectedRotation.InsectSwarmAvgHit / calcsBase.SelectedRotation.AverageInstantCast;
                    iSw.OverallPoints = iSw.DamagePoints;
                    wr.DamagePoints = calcsBase.SelectedRotation.WrathAvgHit / calcsBase.SelectedRotation.WrathAvgCast;
                    wr.OverallPoints = wr.DamagePoints;
                    ss.DamagePoints = calcsBase.SelectedRotation.StarSurgeAvgHit / calcsBase.SelectedRotation.StarSurgeAvgCast;
                    ss.OverallPoints = ss.DamagePoints;
                    return new ComparisonCalculationMoonkin[] { sf, mf, iSw, wr, ss };
            }
            return new ComparisonCalculationBase[0];
        }

#if !RAWR3 && !RAWR4
        public override void RenderCustomChart(Character character, string chartName, System.Drawing.Graphics g, int width, int height)
        {
            string[] statNames = new string[] { "11.7 Spell Power", "4 Mana per 5 sec", "10 Hit Rating", "10 Crit Rating", "10 Haste Rating", "10 Intellect", "10 Spirit" };
            System.Drawing.Color[] statColors = new System.Drawing.Color[] { System.Drawing.Color.FromArgb(255, 255, 0, 0), System.Drawing.Color.DarkBlue, System.Drawing.Color.DarkRed, System.Drawing.Color.FromArgb(255, 255, 165, 0), System.Drawing.Color.Olive, System.Drawing.Color.FromArgb(255, 154, 205, 50), System.Drawing.Color.Aqua };


            height -= 2;
            switch (chartName)
            {
                case "Stats Graph":
                    Stats[] statsList = new Stats[] {
                        new Stats() { SpellPower = 1 },
                        new Stats() { Mp5 = 1 },
                        new Stats() { HitRating = 1 },
                        new Stats() { CritRating = 1 },
                        new Stats() { HasteRating = 1 },
                        new Stats() { Intellect = 1 },
                        new Stats() { Spirit = 1 },
                    };

                    Base.Graph.RenderStatsGraph(g, width, height, character, statsList, statColors, 40, "", "Sustained Damage", Base.Graph.Style.Mage);
                    break;
            }
        }
#endif

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

        #region Relevancy Methods

        /// <summary>
        /// List of itemtypes that are relevant for moonkin
        /// </summary>
       private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                return _relevantItemTypes ?? (_relevantItemTypes = new List<ItemType>(new ItemType[]
                {
                            ItemType.None,
                            ItemType.Leather,
                            ItemType.Dagger,
                            ItemType.Staff,
                            ItemType.FistWeapon,
                            ItemType.OneHandMace,
                            ItemType.TwoHandMace,
                            ItemType.Idol,
                }));
            }
        }

        /// <summary>
        /// List of SpecialEffect Triggers that are relevant for moonkin model
        /// Every trigger listed here needs an implementation in ProcessSpecialEffects()
        /// A trigger not listed here should not appear in ProcessSpecialEffects()
        /// </summary>
       internal static List<Trigger> _RelevantTriggers = null;
        internal static List<Trigger> RelevantTriggers
        {
            get
            {
                return _RelevantTriggers ?? (_RelevantTriggers = new List<Trigger>() {
                            Trigger.Use,
                            Trigger.DamageSpellCast,
                            Trigger.DamageSpellCrit,        // Black magic enchant ?
                            Trigger.DamageSpellHit,
                            Trigger.SpellCast,
                            Trigger.SpellCrit,        
                            Trigger.SpellHit, 
                            Trigger.SpellMiss,
                            Trigger.DoTTick,
                            Trigger.DamageDone,
                            Trigger.DamageOrHealingDone,    // Darkmoon Card: Greatness
                            Trigger.InsectSwarmOrMoonfireTick,
                            Trigger.InsectSwarmTick,
                            Trigger.MoonfireTick,
                            Trigger.MoonfireCast,
                        });
            }
            //set { _RelevantTriggers = value; }
        }

        public override bool IsItemRelevant(Item item)
        {
            // First we let normal rules (profession, class, relevant stats) decide
            bool relevant = base.IsItemRelevant(item);

            // Next we use our special stat relevancy filtering.
            if (relevant)
                relevant = HasPrimaryStats(item.Stats) || (HasSecondaryStats(item.Stats) && !HasUnwantedStats(item.Stats));

            return relevant;
        }

        public override bool IsBuffRelevant(Buff buff, Character character)
        {
            // First we let normal rules (profession, class, relevant stats) decide
            bool relevant = base.IsBuffRelevant(buff, character);

            // Temporary FIX (?): buf.AllowedClasses is not currently being tested as part of base.IsBuffRelevant(). So we'll do it ourselves.
            if (relevant && !buff.AllowedClasses.Contains(CharacterClass.Druid))
                relevant = false;

            // Next we use our special stat relevancy filtering on consumables. (party buffs only need filtering on relevant stats)
            if (relevant && (buff.Group == "Elixirs and Flasks" || buff.Group == "Potion" || buff.Group == "Food" || buff.Group == "Scrolls" || buff.Group == "Temporary Buffs"))
                relevant = HasPrimaryStats(buff.Stats) || (HasSecondaryStats(buff.Stats) && !HasUnwantedStats(buff.Stats));

            return relevant;
        }

        public override bool IsEnchantRelevant(Enchant enchant, Character character)
        {
            // First we let the normal rules (profession, class, relevant stats) decide
            bool relevant = base.IsEnchantRelevant(enchant, character);

            // Next we use our special stat relevancy filtering.
            if (relevant)
                relevant = HasPrimaryStats(enchant.Stats) || (HasSecondaryStats(enchant.Stats) && !HasUnwantedStats(enchant.Stats));

           return relevant;
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                // -- State Properties --
                // Base Stats
                Health = stats.Health,
                Mana = stats.Mana,
                Agility = stats.Agility,
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                Armor = stats.Armor,
                HasteRating = stats.HasteRating,
                HitRating = stats.HitRating,
                CritRating = stats.CritRating,
                SpellPower = stats.SpellPower,
                MasteryRating = stats.MasteryRating,
                // SpellPenetration = stats.SpellPenetration,
                Mp5 = stats.Mp5,
                BonusArmor = stats.BonusArmor,

                // Buffs / Debuffs
                ManaRestoreFromBaseManaPPM = stats.ManaRestoreFromBaseManaPPM,
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,

                // Combat Values
                SpellCrit = stats.SpellCrit,
                SpellCritOnTarget = stats.SpellCritOnTarget,
                SpellHit = stats.SpellHit,
                SpellCombatManaRegeneration = stats.SpellCombatManaRegeneration,

                // Spell Combat Ratings
                SpellArcaneDamageRating = stats.SpellArcaneDamageRating,
                SpellNatureDamageRating = stats.SpellNatureDamageRating,
        
                // Equipment Effects
                ManaRestore = stats.ManaRestore,
                ShadowDamage = stats.ShadowDamage,
                NatureDamage = stats.NatureDamage,
                FireDamage = stats.FireDamage,
                ValkyrDamage = stats.ValkyrDamage,

                // Moonkin
                StarfireDmg = stats.StarfireDmg,
                UnseenMoonDamageBonus = stats.UnseenMoonDamageBonus,
                InsectSwarmDmg = stats.InsectSwarmDmg,
                MoonfireDmg = stats.MoonfireDmg,
                WrathDmg = stats.WrathDmg,
                InnervateCooldownReduction = stats.InnervateCooldownReduction,
               StarfireBonusWithDot = stats.StarfireBonusWithDot,
                MoonfireExtension = stats.MoonfireExtension,
                StarfireCritChance = stats.StarfireCritChance,
                BonusInsectSwarmDamage = stats.BonusInsectSwarmDamage,
                BonusNukeCritChance = stats.BonusNukeCritChance,
                EclipseBonus = stats.EclipseBonus,
                StarfireProc = stats.StarfireProc,
                BonusMoonkinNukeDamage = stats.BonusMoonkinNukeDamage,
                MoonkinT10CritDot = stats.MoonkinT10CritDot,

                // -- MultiplicativeStats --
                // Buffs / Debuffs
                ArmorPenetration = stats.ArmorPenetration,
                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                BonusManaMultiplier = stats.BonusManaMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                BaseArmorMultiplier = stats.BaseArmorMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                SpellHaste = stats.SpellHaste,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusArcaneDamageMultiplier = stats.BonusArcaneDamageMultiplier,
                BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier,

                // -- NoStackStats
                MovementSpeed = stats.MovementSpeed,
                BonusManaPotion = stats.BonusManaPotion,
                HighestStat = stats.HighestStat,
            };

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (RelevantTriggers.Contains(effect.Trigger) && HasRelevantStats(effect.Stats))
                   s.AddSpecialEffect(effect);
            }
            return s;
        }

       public override bool HasRelevantStats(Stats stats)
        {
            // These 3 calls should amount to the same list of stats as used in GetRelevantStats()
            return HasPrimaryStats(stats) || HasSecondaryStats(stats) || HasExtraStats(stats);
        }

        /// <summary>
        /// HasPrimaryStats() should return true if the Stats object has any stats that define the item
        /// as being 'for your class/spec'. For melee classes this is typical melee stats like Strength, 
        /// Agility, AP, Expertise... For casters it would be spellpower, intellect, ...
        /// As soon as an item/enchant/buff has any of the stats listed here, it will be assumed to be 
        /// relevant unless explicitely filtered out.
        /// Stats that could be usefull for both casters and melee such as HitRating, CritRating and Haste
        /// don't belong here, but are SecondaryStats. Specific melee versions of these do belong here 
        /// for melee, spell versions would fit here for casters.
        /// </summary>
        public bool HasPrimaryStats(Stats stats)
        {

            float ignoreStats = stats.Defense + stats.Dodge + stats.Parry + stats.DodgeRating + stats.ParryRating + stats.ExpertiseRating + stats.Block + stats.BlockRating + stats.BlockValue + stats.SpellShadowDamageRating + stats.SpellFireDamageRating + stats.SpellFrostDamageRating + stats.ArmorPenetrationRating + stats.Health + stats.Armor + stats.PVPTrinket + stats.MovementSpeed + stats.Resilience + stats.BonusHealthMultiplier;

            bool PrimaryStats =
                // -- State Properties --
                // Base Stats
                stats.Intellect > 0 ||
                stats.Spirit > 0 ||
                stats.SpellPower > 0 ||
                // stats.SpellPenetration > 0 ||

                // Combat Values
                stats.SpellCrit > 0 ||
                stats.SpellCritOnTarget > 0 ||
                stats.SpellHit > 0 ||

                // Spell Combat Ratings
                stats.SpellArcaneDamageRating > 0 ||
                stats.SpellNatureDamageRating > 0 ||

                // Moonkin
                stats.StarfireDmg > 0 ||
                stats.UnseenMoonDamageBonus > 0 ||
                stats.InsectSwarmDmg > 0 ||
                stats.MoonfireDmg > 0 ||
                stats.WrathDmg > 0 ||
                stats.InnervateCooldownReduction > 0 ||
                stats.StarfireBonusWithDot > 0 ||
                stats.MoonfireExtension > 0 ||
                stats.StarfireCritChance > 0 ||
                stats.BonusInsectSwarmDamage > 0 ||
                stats.BonusNukeCritChance > 0 ||
                stats.EclipseBonus > 0 ||
                stats.StarfireProc > 0 ||
                stats.BonusMoonkinNukeDamage > 0 ||
                stats.MoonkinT10CritDot > 0 ||

                // -- MultiplicativeStats --
                // Buffs / Debuffs
                stats.BonusIntellectMultiplier > 0 ||
                stats.BonusSpiritMultiplier > 0 ||
                stats.SpellHaste > 0 ||
                stats.BonusSpellCritMultiplier > 0 ||
                stats.BonusSpellPowerMultiplier > 0 ||
                stats.BonusArcaneDamageMultiplier > 0 ||
                stats.BonusNatureDamageMultiplier > 0;

            if (!PrimaryStats)
            {
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (RelevantTriggers.Contains(effect.Trigger) && HasPrimaryStats(effect.Stats))
                    {
                        PrimaryStats = true;
                        break;
                    }
                }
            }

            return PrimaryStats;
        }

        /// <summary>
        /// HasSecondaryStats() should return true if the Stats object has any stats that are relevant for the 
        /// model but only to a smaller degree, so small that you wouldn't typically consider the item.
        /// Stats that are usefull to both melee and casters (HitRating, CritRating & Haste) fit in here also.
        /// An item/enchant/buff having these stats would be considered only if it doesn't have any of the 
        /// unwanted stats.  Group/Party buffs are slighly different, they would be considered regardless if 
        /// they have unwanted stats.
       /// Note that a stat may be listed here since it impacts the model, but may also be listed as an unwanted stat.
        /// </summary>
        public bool HasSecondaryStats(Stats stats)
        {
            bool SecondaryStats =
                // -- State Properties --
                // Base Stats
                stats.Mana > 0 ||
                stats.HasteRating > 0 ||
                stats.HitRating > 0 ||
                stats.CritRating > 0 ||
                stats.Mp5 > 0 ||
                stats.MasteryRating > 0 ||

                // Buffs / Debuffs
                stats.ManaRestoreFromBaseManaPPM > 0 ||
                stats.ManaRestoreFromMaxManaPerSecond > 0 ||

                // Combat Values
                stats.SpellCombatManaRegeneration > 0 ||

                // Equipment Effects
                stats.ManaRestore > 0 ||
                stats.ShadowDamage > 0 ||
                stats.NatureDamage > 0 ||
               stats.FireDamage > 0 ||
                stats.ValkyrDamage > 0 ||

                // -- MultiplicativeStats --
                // Buffs / Debuffs
                stats.ArmorPenetration > 0 ||       // benefits trees
                stats.BonusManaMultiplier > 0 ||
                stats.BonusCritMultiplier > 0 ||
               stats.BonusDamageMultiplier > 0 ||

                // -- NoStackStats
                stats.MovementSpeed > 0 ||
                stats.BonusManaPotion > 0 ||
                stats.HighestStat > 0;

            if (!SecondaryStats)
            {
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (RelevantTriggers.Contains(effect.Trigger) && HasSecondaryStats(effect.Stats))
                    {
                        SecondaryStats = true;
                        break;
                    }
                }
            }

            return SecondaryStats;
        }

        /// <summary>
        /// Return true if the Stats object has any stats that don't influence the model but that you do want 
        /// to display in tooltips and in calculated summary values.
        /// </summary>
        public bool HasExtraStats(Stats stats)
        {
            bool ExtraStats =   
                stats.Health > 0 ||
                stats.Agility > 0 ||
                stats.Stamina > 0 ||
                stats.Armor > 0 ||
                stats.BonusArmor > 0 ||
                stats.BonusHealthMultiplier > 0 ||
                stats.BonusAgilityMultiplier > 0 ||
                stats.BonusStaminaMultiplier > 0  ||
                stats.BaseArmorMultiplier > 0 ||
                stats.BonusArmorMultiplier > 0;

            if (!ExtraStats)
            {
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (RelevantTriggers.Contains(effect.Trigger) && HasExtraStats(effect.Stats))
                    {
                        ExtraStats = true;
                        break;
                    }
                }
            }

            return ExtraStats;
        }

        /// <summary>
        /// Return true if the Stats object contains any stats that are making the item undesired.
        /// Any item having only Secondary stats would be removed if it also has one of these.
        /// </summary>
        public bool HasUnwantedStats(Stats stats)
        {
            /// List of stats that will filter out some buffs (Flasks, Elixirs & Scrolls), Enchants and Items.
            bool UnwantedStats = 
                stats.Strength > 0 ||
                stats.Agility > 0 ||
                stats.AttackPower > 0 ||
                stats.ArmorPenetrationRating > 0 ||
                stats.ExpertiseRating > 0 ||
                stats.DefenseRating > 0 ||
                stats.DodgeRating > 0 ||
                stats.ParryRating > 0 ||
                stats.BlockRating > 0 ||
                stats.Resilience > 0 ||
                stats.BlockValue > 0;

            if (!UnwantedStats)
            {
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (/*RelevantTriggers.Contains(effect.Trigger) && */HasUnwantedStats(effect.Stats))    // An unwanted stat could be behind a trigger we don't model.
                    {
                        UnwantedStats = true;
                        break;
                    }
                }
            }

            return UnwantedStats;
        }
        #endregion

        public Stats GetBuffsStats(Character character, CalculationOptionsMoonkin calcOpts) {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            //float hasRelevantBuff;

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
