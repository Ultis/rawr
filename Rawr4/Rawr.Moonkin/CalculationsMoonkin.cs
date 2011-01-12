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
            character.ActiveBuffsAdd(("Arcane Tactics"));
            character.ActiveBuffsAdd(("Arcane Brilliance (Mana)"));
            character.ActiveBuffsAdd(("Arcane Brilliance (SP%)"));
            character.ActiveBuffsAdd(("Blessing of Might (Mp5)"));
            character.ActiveBuffsAdd(("Moonkin Form"));
            character.ActiveBuffsAdd(("Elemental Oath"));
            character.ActiveBuffsAdd(("Enduring Winter"));
            character.ActiveBuffsAdd(("Power Word: Fortitude"));
            character.ActiveBuffsAdd(("Mark of the Wild"));
            character.ActiveBuffsAdd(("Earth and Moon"));
            character.ActiveBuffsAdd(("Critical Mass"));
            character.ActiveBuffsAdd(("Heroism/Bloodlust"));
            character.ActiveBuffsAdd(("Power Infusion"));
            character.ActiveBuffsAdd(("Flask of the Draconic Mind"));
            character.ActiveBuffsAdd(("Intellect Food"));
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

        #region Gemming
        private string[] tierNames = { "Uncommon", "Rare", "Epic", "Jeweler" };

        // Red
        private int[] brilliant = { 52173, 52207, 52207, 52257 };

        // Orange
        private int[] reckless = { 52144, 52208, 52208, 52208 };
        private int[] artful = { 52140, 52205, 52205, 52205 };
        private int[] potent = { 52147, 52239, 52239, 52239 };

        // Purple
        private int[] veiled = { 52153, 52217, 52217, 52217 };
        private int[] timeless = { 52098, 52248, 52248, 52248 };

        // Meta
        private int chaotic = 52291;
        private int ember = 52296;
        //private int forlorn = 52302;

        //Cogwheel
        private int cog_fractured = 59480;  //Mastery
        private int cog_sparkling = 59496;  //Spirit
        private int cog_quick = 59479;  //Haste
        private int cog_rigid = 59493;  //Hit
        private int cog_smooth = 59478;  //Crit

        /// <summary>
        /// List of gemming templates available to Rawr.
        /// </summary>
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                List<GemmingTemplate> retval = new List<GemmingTemplate>();
                for (int tier = 0; tier < 4; ++tier)
                {
                    retval.AddRange(MoonkinGemmingTemplateBlock(tier, chaotic));
                    retval.AddRange(MoonkinGemmingTemplateBlock(tier, ember));
                }
                return retval;
            }
        }

        private List<GemmingTemplate> MoonkinGemmingTemplateBlock(int tier, int meta)
        {
            List<GemmingTemplate> retval = new List<GemmingTemplate>();
            retval.AddRange(new GemmingTemplate[]
                {
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, brilliant, brilliant, brilliant, meta, cog_quick), // Straight Intellect
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, reckless, brilliant, brilliant, meta, cog_quick), // Int/Haste
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, potent, brilliant, brilliant, meta, cog_smooth), // Int/Crit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, artful, brilliant, brilliant, meta, cog_fractured), // Int/Mastery
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, reckless, veiled, brilliant, meta, cog_quick), // Int/Haste/Hit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, reckless, veiled, brilliant, meta, cog_rigid), // Int/Haste/Hit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, reckless, veiled, brilliant, meta, cog_sparkling), // Int/Haste/Spirit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, potent, veiled, brilliant, meta, cog_smooth), // Int/Crit/Hit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, potent, veiled, brilliant, meta, cog_sparkling), // Int/Crit/Spirit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, artful, veiled, brilliant, meta, cog_fractured), // Int/Mastery/Hit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, artful, veiled, brilliant, meta, cog_rigid), // Int/Mastery/Hit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, artful, veiled, brilliant, meta, cog_sparkling), // Int/Mastery/Spirit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, reckless, timeless, brilliant, meta, cog_quick), // Int/Haste/Stam
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, potent, timeless, brilliant, meta, cog_smooth), // Int/Crit/Stam
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, artful, timeless, brilliant, meta, cog_fractured), // Int/Mastery/Stam
                });
            return retval;
        }

        const int DEFAULT_GEMMING_TIER = 2;
        private GemmingTemplate CreateMoonkinGemmingTemplate(int tier, string[] tierNames, int[] red, int[] yellow, int[] blue, int[] prismatic, int meta, int cogwheel)
        {
            return new GemmingTemplate
            {
                Model = "Moonkin",
                Group = tierNames[tier],
                Enabled = (tier == DEFAULT_GEMMING_TIER),
                RedId = red[tier],
                YellowId = yellow[tier],
                BlueId = blue[tier],
                PrismaticId = prismatic[tier],
                MetaId = meta,
                CogwheelId = cogwheel
            };
        }
        #endregion

        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot)
        {
            // No enchants allowed on our ranged slot
            if (slot == ItemSlot.Ranged) return false;
            // Make an exception for enchant 4091 - Enchant Off-Hand - Superior Intellect
            if (slot == ItemSlot.OffHand && enchant.Id == 4091) return true;
            // No other enchants allowed on our offhands
            if (slot == ItemSlot.OffHand) return false;
            // Otherwise, return the base value
            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique)
        {
            if (slot == CharacterSlot.OffHand && item.Slot == ItemSlot.OneHand) return false;
            return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
        }

        public override List<Reforging> GetReforgingOptions(Item baseItem)
        {
            List<Reforging> retval = base.GetReforgingOptions(baseItem);

            // If the item has spirit, do not allow reforging spirit -> hit
            if (baseItem.Stats.Spirit > 0)
            {
                retval.RemoveAll(rf => rf != null && rf.ReforgeFrom == AdditiveStat.Spirit && rf.ReforgeTo == AdditiveStat.HitRating);
            }
            // If it has hit, do not allow reforging hit -> spirit
            else if (baseItem.Stats.HitRating > 0)
            {
                retval.RemoveAll(rf => rf != null && rf.ReforgeFrom == AdditiveStat.HitRating && rf.ReforgeTo == AdditiveStat.Spirit);
            }
            // If it has neither, pick one based on the current calculation options.
            else
            {
                CalculationOptionsMoonkin calcOpts = ((Character)((CalculationOptionsPanelMoonkin)CalculationOptionsPanel).DataContext).CalculationOptions as CalculationOptionsMoonkin;
                retval.RemoveAll(rf => rf != null && rf.ReforgeTo == ((calcOpts != null && calcOpts.ReforgePriority == 0) ? AdditiveStat.HitRating : AdditiveStat.Spirit));
            }

            return retval;
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
                _relevantGlyphs.Add("Glyph of Starsurge");
                // Major glyphs
                _relevantGlyphs.Add("Glyph of Starfall");
                _relevantGlyphs.Add("Glyph of Focus");
            }
            return _relevantGlyphs;
        }

#if RAWR3 || RAWR4
        private Dictionary<string, System.Windows.Media.Color> subColors = null;
        public override Dictionary<string, System.Windows.Media.Color> SubPointNameColors
        {
            get
            {
                if (subColors == null)
                {
                    subColors = new Dictionary<string, System.Windows.Media.Color>();
                    subColors.Add("Burst Damage", System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
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
                    "Solution:Burst Rotation",
                    "Solution:Burst DPS",
                    "Solution:Burst Time To OOM",
                    "Solution:Burst Cycle Length",
                    "Solution:Burst Spell Breakdown",
                    "Solution:Nature's Grace Uptime",
                    "Solution:Solar Eclipse Uptime",
                    "Solution:Lunar Eclipse Uptime",
                    "Spell Info:Starfire",
                    "Spell Info:Wrath",
                    "Spell Info:Starsurge",
                    "Spell Info:Moonfire",
                    "Spell Info:Insect Swarm",
                    "Spell Info:Starfall",
                    "Spell Info:Treants",
                    "Spell Info:Wild Mushroom"
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
                    "Damage per Cast Time",
                    "Mana Gains By Source"
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

        public static CharacterCalculationsMoonkin GetInnerCharacterCalculations(Character character, StatsMoonkin stats, Item additionalItem)
        {
            // First things first, we need to ensure that we aren't using bad data
            CharacterCalculationsMoonkin calc = new CharacterCalculationsMoonkin();
            if (character == null) { return calc; }
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            if (calcOpts == null) { return calc; }
            //
            BossOptions bossOpts = character.BossOptions;
            calc.BasicStats = stats;

            calc.SpellCrit = StatConversion.GetSpellCritFromIntellect(stats.Intellect) + StatConversion.GetSpellCritFromRating(stats.CritRating) + stats.SpellCrit + stats.SpellCritOnTarget;
            calc.SpellHit = StatConversion.GetSpellHitFromRating(stats.HitRating) + stats.SpellHit;
            calc.SpellHaste = (1 + StatConversion.GetSpellHasteFromRating(stats.HasteRating)) * (1 + stats.SpellHaste) - 1;

            // All spells: Damage +(1 * Int)
            float spellDamageFromIntPercent = 1f;
            // Fix for rounding error in converting partial points of int/spirit to spell power
            float spellPowerFromStats = (float)Math.Floor(spellDamageFromIntPercent * (Math.Max(0f, stats.Intellect - 10)));
            calc.SpellPower = stats.SpellPower + spellPowerFromStats;

            calc.Latency = calcOpts.Latency;

            // Mastery from rating
            calc.Mastery = 8.0f + StatConversion.GetMasteryFromRating(stats.MasteryRating);

            calc.FightLength = bossOpts.BerserkTimer / 60f;
            calc.TargetLevel = bossOpts.Level;

            // 3.1 spirit regen
            // Irrelevant in Cataclysm, when we never get spirit regen
            //float spiritRegen = StatConversion.GetSpiritRegenSec(calcs.BasicStats.Spirit, calcs.BasicStats.Intellect);
            calc.ManaRegen = stats.Mp5 / 5f;

            return calc;
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            // First things first, we need to ensure that we aren't using bad data
            CharacterCalculationsMoonkin calc = new CharacterCalculationsMoonkin();
            if (character == null) { return calc; }
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            if (calcOpts == null) { return calc; }
            //
            StatsMoonkin stats = (StatsMoonkin)GetCharacterStats(character, additionalItem);
            calc = CalculationsMoonkin.GetInnerCharacterCalculations(character, stats, additionalItem);
            calc.PlayerLevel = character.Level;
            calc.PtrMode = calcOpts.PTRMode;
            // Run the solver to do final calculations
            new MoonkinSolver().Solve(character, ref calc);

            return calc;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;

            StatsMoonkin statsTotal = new StatsMoonkin();
            Stats statsBase = BaseStats.GetBaseStats(character.Level, character.Class, character.Race, BaseStats.DruidForm.Moonkin);
            statsTotal.Accumulate(statsBase);
            
            // Get the gear/enchants/buffs stats loaded in
            statsTotal.Accumulate(GetItemStats(character, additionalItem));
            statsTotal.Accumulate(GetBuffsStats(character, calcOpts));

            bool leatherSpecialization = character.Head != null && character.Head.Type == ItemType.Leather &&
                                            character.Shoulders != null && character.Shoulders.Type == ItemType.Leather &&
                                            character.Chest != null && character.Chest.Type == ItemType.Leather &&
                                            character.Wrist != null && character.Wrist.Type == ItemType.Leather &&
                                            character.Hands != null && character.Hands.Type == ItemType.Leather &&
                                            character.Waist != null && character.Waist.Type == ItemType.Leather &&
                                            character.Legs != null && character.Legs.Type == ItemType.Leather &&
                                            character.Feet != null && character.Feet.Type == ItemType.Leather;

            // Talented bonus multipliers

            Stats statsTalents = new Stats()
            {
                BonusIntellectMultiplier = (1 + 0.02f * character.DruidTalents.HeartOfTheWild) * (leatherSpecialization ? 1.05f : 1f) - 1,
                BonusManaMultiplier = 0.05f * character.DruidTalents.Furor,
                BonusSpellPowerMultiplier = (1 + 0.01f * character.DruidTalents.BalanceOfPower) * (1 + 0.02f * character.DruidTalents.EarthAndMoon) *
                                            (1 + 0.01f * character.DruidTalents.MoonkinForm) *
                                            (1 + (character.DruidTalents.MoonkinForm > 0 ? 0.04f * character.DruidTalents.MasterShapeshifter : 0.0f)) - 1,
                BaseArmorMultiplier = 1.2f * character.DruidTalents.MoonkinForm,
                BonusArcaneDamageMultiplier = 0.25f,
                BonusNatureDamageMultiplier = 0.25f
            };

            statsTotal.Accumulate(statsTalents);

            // Base stats: Intellect, Stamina, Spirit, Agility
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Spirit = (float)Math.Floor(statsTotal.Spirit * (1 + statsTotal.BonusSpiritMultiplier));

            // Derived stats: Health, mana pool, armor
            statsTotal.Health = (float)Math.Round(statsTotal.Health + StatConversion.GetHealthFromStamina(statsTotal.Stamina));
            statsTotal.Health = (float)Math.Floor(statsTotal.Health * (1f + statsTotal.BonusHealthMultiplier));
            statsTotal.Mana = (float)Math.Round(statsTotal.Mana + StatConversion.GetManaFromIntellect(statsTotal.Intellect));
            statsTotal.Mana = (float)Math.Floor(statsTotal.Mana * (1f + statsTotal.BonusManaMultiplier));
            // Armor
            statsTotal.Armor = statsTotal.Armor * (1f + statsTotal.BaseArmorMultiplier);
            statsTotal.BonusArmor += statsTotal.Agility * 2f;
            statsTotal.BonusArmor = statsTotal.BonusArmor * (1f + statsTotal.BonusArmorMultiplier);
            statsTotal.Armor += statsTotal.BonusArmor;
            statsTotal.Armor = (float)Math.Round(statsTotal.Armor);

            // Physical hit/crit/haste (for treants)
            statsTotal.PhysicalHit = StatConversion.GetHitFromRating(statsTotal.HitRating);
            // Add Bloodlust/Heroism to averaged physical haste
            foreach (SpecialEffect effect in statsTotal.SpecialEffects(se => se.Stats.PhysicalHaste > 0))
            {
                statsTotal.PhysicalHaste += effect.GetAverageUptime(0, 1) * effect.Stats.PhysicalHaste;
            }

            // Crit rating
            // Application order: Stats, Talents, Gear
            // All spells: Crit% + (0.02 * Nature's Majesty)
            statsTotal.SpellCrit += 0.02f * character.DruidTalents.NaturesMajesty;
            // All spells: Hit rating + 0.5f * Balance of Power * Spirit
            statsTotal.HitRating += 0.5f * character.DruidTalents.BalanceOfPower * (statsTotal.Spirit - statsBase.Spirit);

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            
            switch (chartName)
            {
                case "Mana Gains By Source":
                    CharacterCalculationsMoonkin calcsManaBase = GetCharacterCalculations(character) as CharacterCalculationsMoonkin;
                    RotationData manaGainsRot = calcsManaBase.SelectedRotation;
                    Character c2 = character.Clone();

                    List<ComparisonCalculationMoonkin> manaGainsList = new List<ComparisonCalculationMoonkin>();

                    // Euphoria
                    int previousTalentValue = c2.DruidTalents.Euphoria;
                    c2.DruidTalents.Euphoria = 0;
                    CharacterCalculationsMoonkin calcsManaMoonkin = GetCharacterCalculations(c2) as CharacterCalculationsMoonkin;
                    c2.DruidTalents.Euphoria = previousTalentValue;
                    foreach (KeyValuePair<string, RotationData> pairs in calcsManaMoonkin.Rotations)
                    {
                        if (pairs.Key == manaGainsRot.Name)
                        {
                            manaGainsList.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = "Euphoria",
                                OverallPoints = (manaGainsRot.ManaGained - pairs.Value.ManaGained) / manaGainsRot.Duration * calcsManaBase.FightLength * 60.0f,
                                BurstDamagePoints = (manaGainsRot.ManaGained - pairs.Value.ManaGained) / manaGainsRot.Duration * calcsManaBase.FightLength * 60.0f,
                                ImageSource = "achievement_boss_valithradreamwalker"
                            });
                        }
                    }

                    // Replenishment
                    CalculationOptionsMoonkin calcOpts = c2.CalculationOptions as CalculationOptionsMoonkin;
                    calcOpts.Notify = false;
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
                                BurstDamagePoints = (manaGainsRot.ManaGained - pairs.Value.ManaGained) / manaGainsRot.Duration * calcsManaBase.FightLength * 60.0f,
                                ImageSource = "spell_magic_managain"
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
                                BurstDamagePoints = (manaGainsRot.ManaGained - pairs.Value.ManaGained) / manaGainsRot.Duration * calcsManaBase.FightLength * 60.0f,
                                ImageSource = "spell_nature_lightning"
                            });
                        }
                    }
                    calcOpts.Notify = true;

                    // mp5
                    manaGainsList.Add(new ComparisonCalculationMoonkin()
                    {
                        Name = "MP5",
                        OverallPoints = calcsManaBase.FightLength * 60.0f * calcsManaBase.ManaRegen,
                        BurstDamagePoints = calcsManaBase.FightLength * 60.0f * calcsManaBase.ManaRegen
                    });
                    return manaGainsList.ToArray();

                case "Damage per Cast Time":
                    CharacterCalculationsMoonkin calcsBase = GetCharacterCalculations(character) as CharacterCalculationsMoonkin;
                    ComparisonCalculationMoonkin sf = new ComparisonCalculationMoonkin() { Name = "Starfire", ImageSource = "spell_arcane_starfire" };
                    ComparisonCalculationMoonkin mf = new ComparisonCalculationMoonkin() { Name = "Moonfire", ImageSource = "spell_nature_starfall" };
                    ComparisonCalculationMoonkin iSw = new ComparisonCalculationMoonkin() { Name = "Insect Swarm", ImageSource = "spell_nature_insectswarm" };
                    ComparisonCalculationMoonkin wr = new ComparisonCalculationMoonkin() { Name = "Wrath", ImageSource = "spell_nature_abolishmagic" };
                    ComparisonCalculationMoonkin ss = new ComparisonCalculationMoonkin() { Name = "Starsurge", ImageSource = "spell_arcane_arcane03" };
                    ComparisonCalculationMoonkin ssInst = new ComparisonCalculationMoonkin() { Name = "Shooting Stars Proc", ImageSource = "ability_mage_arcanebarrage" };
                    ComparisonCalculationMoonkin wm = new ComparisonCalculationMoonkin() { Name = "Wild Mushroom", ImageSource = "druid_ability_wildmushroom_b" };
                    ComparisonCalculationMoonkin sfa = new ComparisonCalculationMoonkin() { Name = "Starfall", ImageSource = "ability_druid_starfall" };
                    ComparisonCalculationMoonkin fon = new ComparisonCalculationMoonkin() { Name = "Force of Nature", ImageSource = "ability_druid_forceofnature" };
                    sf.BurstDamagePoints = calcsBase.SelectedRotation.StarfireAvgHit / calcsBase.SelectedRotation.StarfireAvgCast;
                    sf.OverallPoints = sf.BurstDamagePoints;
                    mf.BurstDamagePoints = calcsBase.SelectedRotation.MoonfireAvgHit / calcsBase.SelectedRotation.AverageInstantCast;
                    mf.OverallPoints = mf.BurstDamagePoints;
                    iSw.BurstDamagePoints = calcsBase.SelectedRotation.InsectSwarmAvgHit / calcsBase.SelectedRotation.AverageInstantCast;
                    iSw.OverallPoints = iSw.BurstDamagePoints;
                    wr.BurstDamagePoints = calcsBase.SelectedRotation.WrathAvgHit / calcsBase.SelectedRotation.WrathAvgCast;
                    wr.OverallPoints = wr.BurstDamagePoints;
                    ss.BurstDamagePoints = calcsBase.SelectedRotation.StarSurgeAvgHit / calcsBase.SelectedRotation.StarSurgeAvgCast;
                    ss.OverallPoints = ss.BurstDamagePoints;
                    ssInst.BurstDamagePoints = calcsBase.SelectedRotation.StarSurgeAvgHit / calcsBase.SelectedRotation.AverageInstantCast;
                    ssInst.OverallPoints = ssInst.BurstDamagePoints;
                    wm.BurstDamagePoints = calcsBase.SelectedRotation.MushroomDamage / 1.5f;
                    wm.OverallPoints = wm.BurstDamagePoints;
                    sfa.BurstDamagePoints = calcsBase.SelectedRotation.StarfallDamage / calcsBase.SelectedRotation.AverageInstantCast;
                    sfa.OverallPoints = sfa.BurstDamagePoints;
                    fon.BurstDamagePoints = calcsBase.SelectedRotation.TreantDamage / calcsBase.SelectedRotation.AverageInstantCast;
                    fon.OverallPoints = fon.BurstDamagePoints;
                    return new ComparisonCalculationMoonkin[] { sf, mf, iSw, wr, ss, ssInst, sfa, fon, wm };
            }
            return new ComparisonCalculationBase[0];
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
                            ItemType.Relic,
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
                            Trigger.InsectSwarmTick,
                            Trigger.MoonfireTick,
                            Trigger.MoonfireCast,
                            Trigger.EclipseProc,
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
                TargetArmorReduction = stats.TargetArmorReduction,

                // Spell Combat Ratings
                SpellArcaneDamageRating = stats.SpellArcaneDamageRating,
                SpellNatureDamageRating = stats.SpellNatureDamageRating,
        
                // Equipment Effects
                ManaRestore = stats.ManaRestore,
                ShadowDamage = stats.ShadowDamage,
                NatureDamage = stats.NatureDamage,
                FireDamage = stats.FireDamage,
                HolyDamage = stats.HolyDamage,
                ArcaneDamage = stats.ArcaneDamage,
                ValkyrDamage = stats.ValkyrDamage,

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
                SnareRootDurReduc = stats.SnareRootDurReduc,
                FearDurReduc = stats.FearDurReduc,
                StunDurReduc = stats.StunDurReduc,
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
           // Add a null call to catch relevance for set bonuses that have no actual stats
            return string.IsNullOrEmpty(stats.ToString()) || HasPrimaryStats(stats) || HasSecondaryStats(stats) || HasExtraStats(stats);
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

            float ignoreStats = stats.Dodge + stats.Parry + stats.DodgeRating + stats.ParryRating + stats.ExpertiseRating + stats.Block + stats.BlockRating + stats.SpellShadowDamageRating + stats.SpellFireDamageRating + stats.SpellFrostDamageRating + stats.Health + stats.Armor + stats.PVPTrinket + stats.MovementSpeed + stats.Resilience + stats.BonusHealthMultiplier;

            bool PrimaryStats =
                // -- State Properties --
                // Base Stats
                stats.Intellect != 0 ||
                stats.Spirit != 0 ||
                stats.SpellPower != 0 ||
                // stats.SpellPenetration != 0 ||

                // Combat Values
                stats.SpellCrit != 0 ||
                stats.SpellCritOnTarget != 0 ||
                stats.SpellHit != 0 ||

                // Spell Combat Ratings
                stats.SpellArcaneDamageRating != 0 ||
                stats.SpellNatureDamageRating != 0 ||

                // -- MultiplicativeStats --
                // Buffs / Debuffs
                stats.BonusIntellectMultiplier != 0 ||
                stats.BonusSpiritMultiplier != 0 ||
                stats.SpellHaste != 0 ||
                stats.BonusSpellCritMultiplier != 0 ||
                stats.BonusSpellPowerMultiplier != 0 ||
                stats.BonusArcaneDamageMultiplier != 0 ||
                stats.BonusNatureDamageMultiplier != 0;

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
                stats.Mana != 0 ||
                stats.HasteRating != 0 ||
                stats.HitRating != 0 ||
                stats.CritRating != 0 ||
                stats.Mp5 != 0 ||
                stats.MasteryRating != 0 ||

                // Buffs / Debuffs
                stats.ManaRestoreFromBaseManaPPM != 0 ||
                stats.ManaRestoreFromMaxManaPerSecond != 0 ||

                // Combat Values
                stats.SpellCombatManaRegeneration != 0 ||
                stats.TargetArmorReduction != 0 ||

                // Equipment Effects
                stats.ManaRestore != 0 ||
                stats.ShadowDamage != 0 ||
                stats.NatureDamage != 0 ||
               stats.FireDamage != 0 ||
                stats.ValkyrDamage != 0 ||

                // -- MultiplicativeStats --
                // Buffs / Debuffs
                stats.ArmorPenetration != 0 ||       // benefits trees
                stats.BonusManaMultiplier != 0 ||
                stats.BonusCritMultiplier != 0 ||
               stats.BonusDamageMultiplier != 0 ||

                // -- NoStackStats
                stats.MovementSpeed != 0 ||
                stats.SnareRootDurReduc != 0 ||
                stats.FearDurReduc != 0 ||
                stats.StunDurReduc != 0 ||
                stats.BonusManaPotion != 0 ||
                stats.HighestStat != 0;

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
                stats.Health != 0 ||
                stats.Agility != 0 ||
                stats.Stamina != 0 ||
                stats.Armor != 0 ||
                stats.BonusArmor != 0 ||
                stats.BonusHealthMultiplier != 0 ||
                stats.BonusAgilityMultiplier != 0 ||
                stats.BonusStaminaMultiplier != 0  ||
                stats.BaseArmorMultiplier != 0 ||
                stats.BonusArmorMultiplier != 0;

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
                stats.ExpertiseRating > 0 ||
                stats.DodgeRating > 0 ||
                stats.ParryRating > 0 ||
                stats.BlockRating > 0 ||
                stats.Resilience > 0;

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
