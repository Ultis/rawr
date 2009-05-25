using System;
using System.Collections.Generic;

using Rawr;

namespace Rawr.ShadowPriest
{
    [Rawr.Calculations.RawrModelInfo("ShadowPriest", "Spell_Shadow_Shadowform", Character.CharacterClass.Priest)]
    public class CalculationsShadowPriest : CalculationsBase 
    {
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                // Interesting Gem Choices for a Shadow Priest
                // Red
                int[] runed = { 39911, 39998, 40113, 42144 }; // +spp

                // Orange
                int[] durable = { 39958, 40050, 40154 }; // +spp/+resilience
                int[] luminous = { 39946, 40047, 40151 }; // +spp/+int
                int[] potent = { 39956, 40048, 40152 }; // +spp/+crit
                int[] reckless = { 39959, 40051, 40155 }; // +spp/+haste
                int[] veiled = { 39957, 40049, 40153 }; // +spp/+hit

                // Yellow
                int[] brilliant = { 39912, 40012, 40123, 42148 }; // +int
                int[] mystic = { 39917, 40016, 40127, 42158 }; // +resilience
                int[] quick = { 39918, 40017, 40128, 42150 }; // +haste
                int[] rigid = { 39915, 40014, 40125, 42156 }; // +hit
                int[] smooth = { 39914, 40013, 40124, 42149 }; // +crit

                // Green
                int[] dazzling = { 39984, 40094, 40175 }; // +int/+mp5
                int[] energized = { 39989, 40105, 40179 }; // +haste/+mp5
                int[] forceful = { 39978, 40091, 40169 }; // +haste/+sta
                int[] intricate = { 39983, 40104, 40174 }; // +haste/+spi
                int[] jagged = { 39974, 40086, 40165 }; // +crit/+stamina
                int[] lambent = { 39986, 40100, 40177 }; // +hit/+mp5
                int[] misty = { 39980, 40095, 40171 }; // +crit/+spi
                int[] opaque = { 39988, 40103, 40178 }; // +resilience/+mp5
                int[] seers = { 39979, 40092, 40170 }; // +int/+spi 
                int[] shining = { 39981, 40099, 40172 }; // +hit/+spi
                int[] steady = { 39977, 40090, 40168 }; // +resilience/+stamina
                int[] sundered = { 39985, 40096, 40176 }; // +crit/+mp5
                int[] timeless = { 39968, 40085, 40164 }; // +int/+stamina
                int[] turbid = { 39982, 40102, 40173 }; // +resilience/+spi
                int[] vivid = { 39975, 40088, 40166 }; // +hit/+stamina

                // Blue
                int[] lustrous = { 39927, 40010, 40121, 42146 }; // +mp5
                int[] solid = { 39919, 40008, 40119, 36767 }; // +stamina
                int[] sparkling = { 39920, 40009, 40120, 42145 }; // +spirit

                // Purple
                int[] glowing = { 39936, 40025, 40132 }; // +spp/+stamina
                int[] purified = { 39941, 40026, 40133 }; // +spp/+spirit
                int[] royal = { 39943, 40027, 40134 }; // +spp/+mp5

                // Meta
                int[] chaotic = { 41285 }; // +spp/+3% crit damage
                int[] insightful = { 41401 }; // +int/manarestore
                int[] powerful = { 41397 }; // +stamina/-10% stun duration

                return new List<GemmingTemplate>() {
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Uncommon", // Forced Hit
                        RedId = rigid[0], YellowId = rigid[0], BlueId = rigid[0], PrismaticId = rigid[0], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Uncommon", // Balanced w/Hit
                        RedId = veiled[0], YellowId = rigid[0], BlueId = shining[0], PrismaticId = rigid[0], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Uncommon", // Power w/Hit
                        RedId = runed[0], YellowId = veiled[0], BlueId = purified[0], PrismaticId = runed[0], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Uncommon", // Full Power
                        RedId = runed[0], YellowId = runed[0], BlueId = runed[0], PrismaticId = runed[0], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Uncommon", // Power w/Haste
                        RedId = runed[0], YellowId = reckless[0], BlueId = purified[0], PrismaticId = runed[0], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Uncommon", // Power w/Crit
                        RedId = runed[0], YellowId = potent[0], BlueId = purified[0], PrismaticId = runed[0], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Uncommon", // Mana
                        RedId = luminous[0], YellowId = brilliant[0], BlueId = seers[0], PrismaticId = brilliant[0], MetaId = insightful[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Uncommon", // PvP - Stamina
                        RedId = glowing[0], YellowId = steady[0], BlueId = solid[0], PrismaticId = solid[0], MetaId = powerful[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Uncommon", // PvP - Resilience
                        RedId = durable[0], YellowId = mystic[0], BlueId = steady[0], PrismaticId = mystic[0], MetaId = powerful[0] },

                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Rare", // Forced Hit
                        RedId = rigid[1], YellowId = rigid[1], BlueId = rigid[1], PrismaticId = rigid[1], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Rare", Enabled = true, // Balanced w/Hit
                        RedId = veiled[1], YellowId = rigid[1], BlueId = shining[1], PrismaticId = rigid[1], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Rare", Enabled = true, // Power w/Hit
                        RedId = runed[1], YellowId = veiled[1], BlueId = purified[1], PrismaticId = runed[1], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Rare", Enabled = true, // Full Power
                        RedId = runed[1], YellowId = runed[1], BlueId = runed[1], PrismaticId = runed[1], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Rare", Enabled = true, // Power w/Haste
                        RedId = runed[1], YellowId = reckless[1], BlueId = purified[1], PrismaticId = runed[1], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Rare", Enabled = true, // Power w/Crit
                        RedId = runed[1], YellowId = potent[1], BlueId = purified[1], PrismaticId = runed[1], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Rare", // Mana
                        RedId = luminous[1], YellowId = brilliant[1], BlueId = seers[1], PrismaticId = brilliant[1], MetaId = insightful[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Rare", // PvP - Stamina
                        RedId = glowing[1], YellowId = steady[1], BlueId = solid[1], PrismaticId = solid[1], MetaId = powerful[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Rare", // PvP - Resilience
                        RedId = durable[1], YellowId = mystic[1], BlueId = steady[1], PrismaticId = mystic[1], MetaId = powerful[0] },


                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Epic", // Forced Hit
                        RedId = rigid[2], YellowId = rigid[2], BlueId = rigid[2], PrismaticId = rigid[2], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Epic", // Balanced w/Hit
                        RedId = veiled[2], YellowId = rigid[2], BlueId = shining[2], PrismaticId = rigid[2], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Epic", // Power w/Hit
                        RedId = runed[2], YellowId = veiled[2], BlueId = purified[2], PrismaticId = runed[2], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Epic", // Full Power
                        RedId = runed[2], YellowId = runed[2], BlueId = runed[2], PrismaticId = runed[2], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Epic", // Power w/Haste
                        RedId = runed[2], YellowId = reckless[2], BlueId = purified[2], PrismaticId = runed[2], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Epic", // Power w/Crit
                        RedId = runed[2], YellowId = potent[2], BlueId = purified[2], PrismaticId = runed[2], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Epic", // Mana
                        RedId = luminous[2], YellowId = brilliant[2], BlueId = seers[2], PrismaticId = brilliant[2], MetaId = insightful[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Epic", // PvP - Stamina
                        RedId = glowing[2], YellowId = steady[2], BlueId = solid[2], PrismaticId = solid[2], MetaId = powerful[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Epic", // PvP - Resilience
                        RedId = durable[2], YellowId = mystic[2], BlueId = steady[2], PrismaticId = mystic[2], MetaId = powerful[0] },
                    
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Jeweler Rare", // Forced Hit
                        RedId = rigid[3], YellowId = rigid[1], BlueId = rigid[3], PrismaticId = rigid[1], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Jeweler Rare", // Full Power
                        RedId = runed[1], YellowId = runed[3], BlueId = runed[3], PrismaticId = runed[1], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Jeweler Rare", // Haste
                        RedId = quick[3], YellowId = quick[1], BlueId = quick[3], PrismaticId = quick[1], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Jeweler Rare", // Crit
                        RedId = smooth[3], YellowId = smooth[1], BlueId = smooth[3], PrismaticId = smooth[1], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Jeweler Rare", // PvP - Stamina                   
                        RedId = solid[3], YellowId = solid[3], BlueId = solid[1], PrismaticId = solid[1], MetaId = powerful[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Jeweler Rare", // PvP - Resilience
                        RedId = mystic[3], YellowId = mystic[1], BlueId = mystic[3], PrismaticId = mystic[1], MetaId = powerful[0] },

                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Jeweler Epic", // Forced Hit
                        RedId = rigid[3], YellowId = rigid[2], BlueId = rigid[3], PrismaticId = rigid[2], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Jeweler Epic", // Full Power
                        RedId = runed[2], YellowId = runed[3], BlueId = runed[3], PrismaticId = runed[2], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Jeweler Epic", // Haste
                        RedId = quick[3], YellowId = quick[2], BlueId = quick[3], PrismaticId = quick[2], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Jeweler Epic", // Crit
                        RedId = smooth[3], YellowId = smooth[2], BlueId = smooth[3], PrismaticId = smooth[2], MetaId = chaotic[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Jeweler Epic", // PvP - Stamina
                        RedId = solid[3], YellowId = solid[3], BlueId = solid[2], PrismaticId = solid[2], MetaId = powerful[0] },
                    new GemmingTemplate() { Model = "ShadowPriest", Group = "Jeweler Epic", // PvP - Resilience
                        RedId = mystic[3], YellowId = mystic[2], BlueId = mystic[3], PrismaticId = mystic[2], MetaId = powerful[0] },
                };
            }
        }

        public override void SetDefaults(Character character)
        {
            character.ActiveBuffs.Add(Buff.GetBuffByName("Inner Fire"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Moonkin Form"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Arcane Intellect"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Vampiric Touch"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Mana Spring Totem"));
                character.ActiveBuffs.Add(Buff.GetBuffByName("Restorative Totems"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Moonkin Form"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Wrath of Air Totem"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Totem of Wrath (Spell Power)"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Divine Spirit"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Power Word: Fortitude"));
                character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Power Word: Fortitude"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Mark of the Wild"));
                character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Mark of the Wild"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Blessing of Kings"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Shadow Protection"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Heart of the Crusader"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Sanctified Retribution"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Scorch"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Judgement of Wisdom"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Earth and Moon"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Misery"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Flask of the Frost Wyrm"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Spell Power Food"));
        }

        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            if (_relevantGlyphs == null)
            {
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of Dispel Magic");
                _relevantGlyphs.Add("Glyph of Dispersion");
                _relevantGlyphs.Add("Glyph of Inner Fire");
                _relevantGlyphs.Add("Glyph of Mass Dispel");
                _relevantGlyphs.Add("Glyph of Mind Flay");
                _relevantGlyphs.Add("Glyph of Mind Sear");
                _relevantGlyphs.Add("Glyph of Penance");
                _relevantGlyphs.Add("Glyph of Shadow");
                _relevantGlyphs.Add("Glyph of Shadow Word: Death");
                _relevantGlyphs.Add("Glyph of Shadow Word: Pain");
                _relevantGlyphs.Add("Glyph of Smite");
                _relevantGlyphs.Add("Glyph of Fading");
            }
            return _relevantGlyphs;
        }

        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Priest; } }

        private string _currentChartName = null;
        private float _currentChartTotal = 0;

        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                switch (_currentChartName)
                {
                    case "Mana Sources":
                        _subPointNameColors.Add(string.Format("MP5 Sources ({0} Total)", _currentChartTotal.ToString("0")), System.Drawing.Color.Blue);
                        break;
                    case "DPS Sources":
                        _subPointNameColors.Add(string.Format("DPS Sources ({0} total)", _currentChartTotal.ToString("0")), System.Drawing.Color.Red);
                        break;
                    case "Mana Usage":
                        _subPointNameColors.Add(string.Format("Mana Usage ({0} total)", _currentChartTotal.ToString("0")), System.Drawing.Color.Blue);
                        break;
                    case "Haste Rating Gain":
                        _subPointNameColors.Add(string.Format("DPS-Burst"), System.Drawing.Color.Red);
                        _subPointNameColors.Add(string.Format("DPS-Sustained"), System.Drawing.Color.Blue);
                        break;
                    default:
                        _subPointNameColors.Add("DPS-Burst", System.Drawing.Color.Red);
                        _subPointNameColors.Add("DPS-Sustained", System.Drawing.Color.Blue);
                        _subPointNameColors.Add("Survivability", System.Drawing.Color.Green);
                        break;
                }
                _currentChartName = null;
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
					"Basic Stats:Health",
                    "Basic Stats:Resilience",
					"Basic Stats:Mana",
					"Basic Stats:Stamina",
					"Basic Stats:Intellect",
					"Basic Stats:Spirit",
					"Basic Stats:Spell Power",
					"Basic Stats:Regen",
					"Basic Stats:Crit",
					"Basic Stats:Hit",
					"Basic Stats:Haste",
                    "Basic Stats:Armor",
                    "Basic Stats:Resistance",
                    "Simulation:Rotation",
                    "Simulation:DPS",
                    "Simulation:SustainDPS",
                    "Shadow:Vampiric Touch",
                    "Shadow:SW Pain",
                    "Shadow:Devouring Plague",
				    "Shadow:SW Death",
                    "Shadow:Mind Blast",
                    "Shadow:Mind Flay",
                    "Shadow:Shadowfiend",
                    "Shadow:Vampiric Embrace",
                    "Holy:PW Shield",
                    "Holy:Smite",
                    "Holy:Holy Fire",
                    "Holy:Penance"
				};
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
					"Health",
                    "Resilience",
                    "Mana",
					"Haste Rating",
                    "Haste %",
                    "Crit Rating",
                    "MB Crit %",
                    "Hit Rating",
                    "MF cast time (ms)",
                    "Armor",
                    "Arcane Resistance",
                    "Fire Resistance",
                    "Frost Resistance",
                    "Nature Resistance",
                    "Shadow Resistance",
					};
                return _optimizableCalculationLabels;
            }
        }

        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelShadowPriest();
                }
                return _calculationOptionsPanel;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] { "Stat Values", "Mana Sources", "DPS Sources", "Mana Usage", "Haste Rating Gain" };
                return _customChartNames;
            }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationShadowPriest(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsShadowPriest(); }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get {
                if (_relevantItemTypes == null) {
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]{
                        Item.ItemType.None,
                        Item.ItemType.Cloth,
                        Item.ItemType.Dagger,
                        Item.ItemType.Wand,
                        Item.ItemType.OneHandMace,
                        Item.ItemType.Staff
                    });
                }
                return _relevantItemTypes;
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
        
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            ComparisonCalculationBase comparison;

            _currentChartTotal = 0;
            _currentChartName = chartName;

            switch (chartName)
            {
                case "Mana Sources":
                    CharacterCalculationsShadowPriest mscalcs = GetCharacterCalculations(character) as CharacterCalculationsShadowPriest;
                    SolverBase mssolver = mscalcs.GetSolver(character, mscalcs.BasicStats);
                    mssolver.Calculate(mscalcs);
                    foreach (SolverBase.ManaSource Source in mssolver.ManaSources)
                    {
                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = Source.Name;
                        comparison.SubPoints[0] = Source.Value * 5;
                        _currentChartTotal += comparison.SubPoints[0];
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparison.Equipped = false;
                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                case "DPS Sources":
                    CharacterCalculationsShadowPriest dpscalcs = GetCharacterCalculations(character) as CharacterCalculationsShadowPriest;
                    SolverBase dpssolver = dpscalcs.GetSolver(character, dpscalcs.BasicStats);
                    dpssolver.Calculate(dpscalcs);
                    foreach (Spell spell in dpssolver.SpellPriority)
                    {
                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.SubPoints[0] = spell.SpellStatistics.DamageDone;
                        _currentChartTotal += comparison.SubPoints[0];
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparison.Equipped = false;
                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                case "Mana Usage":
                    CharacterCalculationsShadowPriest mucalcs = GetCharacterCalculations(character) as CharacterCalculationsShadowPriest;
                    SolverBase musolver = mucalcs.GetSolver(character, mucalcs.BasicStats);
                    musolver.Calculate(mucalcs);
                    foreach (Spell spell in musolver.SpellPriority)
                    {
                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.SubPoints[0] = spell.SpellStatistics.ManaUsed * 5;
                        _currentChartTotal += comparison.SubPoints[0];
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparison.Equipped = false;
                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                case "Haste Rating Gain":
                    CharacterCalculationsShadowPriest hrbase = GetCharacterCalculations(character) as CharacterCalculationsShadowPriest;
                    for (int x = 0; x < 100; x++)
                    {
                        CharacterCalculationsShadowPriest hrnew = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = x } }) as CharacterCalculationsShadowPriest;
                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = string.Format("{0} Haste Rating", x);
                        comparison.SubPoints[0] = hrnew.DpsPoints - hrbase.DpsPoints;
                        comparison.SubPoints[1] = hrnew.SustainPoints - hrbase.SustainPoints;
                        comparison.Equipped = false;
                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                case "Stat Values":
                    CharacterCalculationsShadowPriest calcsBase = GetCharacterCalculations(character) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsIntellect = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsSpirit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Spirit = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsMP5 = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Mp5 = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsSpellPower = GetCharacterCalculations(character, new Item() { Stats = new Stats() { SpellPower = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsHaste = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsCrit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsHit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsSta = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsRes = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Resilience = 50 } }) as CharacterCalculationsShadowPriest;

                    return new ComparisonCalculationBase[] {
                        new ComparisonCalculationShadowPriest() { Name = "1 Intellect",
                            OverallPoints = (calcsIntellect.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsIntellect.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsIntellect.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsIntellect.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Spirit",
                            OverallPoints = (calcsSpirit.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsSpirit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsSpirit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsSpirit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 MP5",
                            OverallPoints = (calcsMP5.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsMP5.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsMP5.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsMP5.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Spell Power",
                            OverallPoints = (calcsSpellPower.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsSpellPower.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsSpellPower.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsSpellPower.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Haste",
                            OverallPoints = (calcsHaste.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsHaste.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsHaste.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsHaste.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Crit",
                            OverallPoints = (calcsCrit.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsCrit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsCrit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsCrit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Hit",
                            OverallPoints = (calcsHit.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsHit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsHit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsHit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Stamina",
                            OverallPoints = (calcsSta.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsSta.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsSta.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsSta.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Resilience",
                            OverallPoints = (calcsRes.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsRes.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsRes.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsRes.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        }};
                default:
                    //_customChartNames = null;
                    _currentChartName = null;
                    return new ComparisonCalculationBase[0];
            }
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            Stats stats = GetCharacterStats(character, additionalItem);
            Stats statsRace = BaseStats.GetBaseStats(character);
            CharacterCalculationsShadowPriest calculatedStats = new CharacterCalculationsShadowPriest();
            CalculationOptionsShadowPriest calculationOptions = character.CalculationOptions as CalculationOptionsShadowPriest;
            
            calculatedStats.Race = character.Race;
            calculatedStats.BasicStats = stats;
            calculatedStats.Character = character;

            calculatedStats.SpiritRegen = (float)Math.Floor(5f * StatConversion.GetSpiritRegenSec(calculatedStats.BasicStats.Spirit, calculatedStats.BasicStats.Intellect));
            calculatedStats.RegenInFSR = calculatedStats.SpiritRegen * calculatedStats.BasicStats.SpellCombatManaRegeneration + calculatedStats.BasicStats.Mp5;
            calculatedStats.RegenOutFSR = calculatedStats.SpiritRegen + calculatedStats.BasicStats.Mp5;

            SolverBase solver = calculatedStats.GetSolver(character, stats);
            solver.Calculate(calculatedStats);

            return calculatedStats;
        }

        /* Deprecated public static Stats GetBaseRaceStats(Character character)
        {
            Stats stats = new Stats();
            if (character.Level >= 70 && character.Level <= 80)
            {
                stats.Health = 3211f + (character.Level - 70) * (6960 - 3211) / 10;
                stats.Mana = 2620 + (character.Level - 70) * (3863 - 2620) / 10;
            }
            else
            {
                stats.Mana = 2620;
                stats.Health = 3211f;
            }

            switch (character.Race)
            {
                case Character.CharacterRace.NightElf:
                    if (character.Level == 80)
                    {
                        stats.Stamina = 66f;
                        stats.Intellect = 174f;
                        stats.Spirit = 181f;
                        stats.Agility = 56f;
                    }
                    else
                    {
                        stats.Stamina = 57f;
                        stats.Intellect = 145f;
                        stats.Spirit = 151f;
                    }
                    break;
                case Character.CharacterRace.Dwarf:
                    if (character.Level == 80)
                    {
                        stats.Stamina = 70f;
                        stats.Intellect = 173f;
                        stats.Spirit = 180f;
                        stats.Agility = 47f;
                    }
                    else
                    {
                        stats.Stamina = 61f;
                        stats.Intellect = 144f;
                        stats.Spirit = 150f;
                    }
                    break;
                case Character.CharacterRace.Draenei:
                    if (character.Level == 80)
                    {
                        stats.Stamina = 66f;
                        stats.Intellect = 175f;
                        stats.Spirit = 183f;
                        stats.Agility = 48f;
                    }
                    else
                    {
                        stats.Stamina = 57f;
                        stats.Intellect = 146f;
                        stats.Spirit = 153f;
                    }
                    break;
                case Character.CharacterRace.Human:
                    if (character.Level == 80)
                    {
                        stats.Stamina = 67f;
                        stats.Intellect = 174f;
                        stats.Spirit = 181f;
                        stats.Agility = 51f;
                    }
                    else
                    {
                        stats.Stamina = 58f;
                        stats.Intellect = 145f;
                        stats.Spirit = 152f;
                    }
                    stats.BonusSpiritMultiplier = 0.03f;
                    break;
                case Character.CharacterRace.BloodElf:
                    if (character.Level == 80)
                    {
                        stats.Stamina = 65f;
                        stats.Intellect = 178f;
                        stats.Spirit = 180f;
                        stats.Agility = 53f;
                    }
                    else
                    {
                        stats.Stamina = 56f;
                        stats.Intellect = 149f;
                        stats.Spirit = 150f;
                    }
                    break;
                case Character.CharacterRace.Troll:
                    if (character.Level == 80)
                    {
                        stats.Stamina = 68f;
                        stats.Intellect = 170f;
                        stats.Spirit = 182f;
                        stats.Agility = 53f;
                    }
                    else
                    {
                        stats.Stamina = 59f;
                        stats.Intellect = 141f;
                        stats.Spirit = 152f;
                    }
                    break;
                case Character.CharacterRace.Undead:
                    if (character.Level == 80)
                    {
                        stats.Stamina = 68f;
                        stats.Intellect = 172f;
                        stats.Spirit = 186f;
                        stats.Agility = 49f;
                    }
                    else
                    {
                        stats.Stamina = 59f;
                        stats.Intellect = 143f;
                        stats.Spirit = 156f;
                    }
                    break;
            }
            return stats;
        } */

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace = BaseStats.GetBaseStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            //Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTalents = new Stats()
            {
                BonusStaminaMultiplier = character.PriestTalents.ImprovedPowerWordFortitude * 0.02f,
                BonusSpiritMultiplier = (1 + character.PriestTalents.Enlightenment * 0.02f) * (1f + character.PriestTalents.SpiritOfRedemption * 0.05f) - 1f,
                BonusIntellectMultiplier = character.PriestTalents.MentalStrength * 0.03f,
                SpellDamageFromSpiritPercentage = character.PriestTalents.SpiritualGuidance * 0.05f + character.PriestTalents.TwistedFaith * 0.02f,
                SpellHaste = character.PriestTalents.Enlightenment * 0.02f,
                SpellCombatManaRegeneration = character.PriestTalents.Meditation * 0.5f / 3f,
                SpellCrit = character.PriestTalents.FocusedWill * 0.01f,
            };

            Stats statsTotal = statsBaseGear + statsBuffs + statsRace + statsTalents;

            statsTotal.Stamina = (float)Math.Floor((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.SpellPower += statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit
                + (statsTotal.PriestInnerFire > 0 ? GetInnerFireSpellPowerBonus(character) : 0);
            statsTotal.Mana += (statsTotal.Intellect - 20f) * 15f + 20f;
            statsTotal.Health += (statsTotal.Stamina - 20f) * 10f + 20f;
            statsTotal.SpellCrit += StatConversion.GetSpellCritFromIntellect(statsTotal.Intellect)
                + StatConversion.GetSpellCritFromRating(statsTotal.CritRating);
            statsTotal.SpellHaste += StatConversion.GetSpellHasteFromRating(statsTotal.HasteRating);
            statsTotal.SpellHit += StatConversion.GetSpellHitFromRating(statsTotal.HitRating);
            statsTotal.BonusArmor += statsTotal.Agility * 2f + (statsTotal.PriestInnerFire > 0 ? GetInnerFireArmorBonus(character) : 0);    

            return statsTotal;
        }

        public static float GetInnerFireSpellPowerBonus(Character character)
        {
            float InnerFireSpellPowerBonus = 0;
            if (character.Level >= 77)
                InnerFireSpellPowerBonus = 120;
            else if (character.Level >= 71)
                InnerFireSpellPowerBonus = 95;
            return InnerFireSpellPowerBonus * (1f + character.PriestTalents.ImprovedInnerFire * 0.15f);
        }

        public static float GetInnerFireArmorBonus(Character character)
        {
            float ArmorBonus = 2440 * (character.PriestTalents.GlyphofInnerFire ? 1.5f : 1);

            return ArmorBonus * (1f + character.PriestTalents.ImprovedInnerFire * 0.15f);
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                Stamina = stats.Stamina,
                Health = stats.Health,
                Resilience = stats.Resilience,
                Intellect = stats.Intellect,
                Mana = stats.Mana,
                Spirit = stats.Spirit,
                Mp5 = stats.Mp5,
                SpellPower = stats.SpellPower,
                SpellShadowDamageRating = stats.SpellShadowDamageRating,
                SpellCritRating = stats.SpellCritRating,
                CritRating = stats.CritRating,
                SpellCrit = stats.SpellCrit,
                SpellHitRating = stats.SpellHitRating,
                HitRating = stats.HitRating,
                SpellHit = stats.SpellHit,
                SpellHasteRating = stats.SpellHasteRating,
                SpellHaste = stats.SpellHaste,
                HasteRating = stats.HasteRating,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                SpellDamageFromSpiritPercentage = stats.SpellDamageFromSpiritPercentage,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusManaPotion = stats.BonusManaPotion,
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusHolyDamageMultiplier = stats.BonusHolyDamageMultiplier,
                BonusDiseaseDamageMultiplier = stats.BonusDiseaseDamageMultiplier,
                PriestInnerFire = stats.PriestInnerFire,
                SWPDurationIncrease = stats.SWPDurationIncrease,
                BonusMindBlastMultiplier = stats.BonusMindBlastMultiplier,
                MindBlastCostReduction = stats.MindBlastCostReduction,
                ShadowWordDeathCritIncrease = stats.ShadowWordDeathCritIncrease,
                WeakenedSoulDurationDecrease = stats.WeakenedSoulDurationDecrease,
                DevouringPlagueBonusDamage = stats.DevouringPlagueBonusDamage,
                MindBlastHasteProc = stats.MindBlastHasteProc,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                ManaRestore = stats.ManaRestore,
                SpellsManaReduction = stats.SpellsManaReduction,
                HighestStat = stats.HighestStat,
                ShadowDamage = stats.ShadowDamage,

                /*ManaRestoreOnCast_5_15 = stats.ManaRestoreOnCast_5_15,
                ManaRestoreFromBaseManaPerHit = stats.ManaRestoreFromBaseManaPerHit,
                SpellPowerFor15SecOnUse90Sec = stats.SpellPowerFor15SecOnUse90Sec,
                SpellPowerFor15SecOnUse2Min = stats.SpellPowerFor15SecOnUse2Min,
                SpellPowerFor20SecOnUse2Min = stats.SpellPowerFor20SecOnUse2Min,
                HasteRatingFor20SecOnUse2Min = stats.HasteRatingFor20SecOnUse2Min,
                HasteRatingFor20SecOnUse5Min = stats.HasteRatingFor20SecOnUse5Min,
                SpellPowerFor10SecOnCast_15_45 = stats.SpellPowerFor10SecOnCast_15_45,
                SpellPowerFor10SecOnHit_10_45 = stats.SpellPowerFor10SecOnHit_10_45,
                SpellHasteFor10SecOnCast_10_45 = stats.SpellHasteFor10SecOnCast_10_45,
                TimbalsProc = stats.TimbalsProc,
                PendulumOfTelluricCurrentsProc = stats.PendulumOfTelluricCurrentsProc,
                ExtractOfNecromanticPowerProc = stats.ExtractOfNecromanticPowerProc,
                */

                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                Agility = stats.Agility,
                ArcaneResistance = stats.ArcaneResistance,
                ArcaneResistanceBuff = stats.ArcaneResistanceBuff,
                FireResistance = stats.FireResistance,
                FireResistanceBuff = stats.FireResistanceBuff,
                FrostResistance = stats.FrostResistance,
                FrostResistanceBuff = stats.FrostResistanceBuff,
                NatureResistance = stats.NatureResistance,
                NatureResistanceBuff = stats.NatureResistanceBuff,
                ShadowResistance = stats.ShadowResistance,
                ShadowResistanceBuff = stats.ShadowResistanceBuff,
            };

            foreach (SpecialEffect se in stats.SpecialEffects())
                if (RelevantTrinket(se))
                    s.AddSpecialEffect(se);

            return s;
        }

        protected bool RelevantTrinket(SpecialEffect se)
        {
            if (se.Trigger == Trigger.DamageSpellCrit
                || se.Trigger == Trigger.DamageSpellHit
                || se.Trigger == Trigger.DamageSpellCast
                || se.Trigger == Trigger.DoTTick
                || se.Trigger == Trigger.SpellHit
                || se.Trigger == Trigger.SpellCast
                || se.Trigger == Trigger.SpellCrit
                || se.Trigger == Trigger.SpellMiss
                || se.Trigger == Trigger.Use)
            {
                return _HasRelevantStats(se.Stats);
            }
            return false;
        }

        protected bool _HasRelevantStats(Stats stats)
        {
            bool Yes = (
                stats.Intellect + stats.Mana + stats.Spirit + stats.Mp5 + stats.SpellPower
                + stats.SpellShadowDamageRating + stats.SpellCritRating + stats.CritRating
                + stats.SpellCrit + stats.SpellHitRating + stats.HitRating + stats.SpellHit
                + stats.SpellHasteRating + stats.SpellHaste + stats.HasteRating

                + stats.BonusSpiritMultiplier + stats.SpellDamageFromSpiritPercentage
                + stats.BonusIntellectMultiplier + stats.BonusManaPotion
                + stats.ThreatReductionMultiplier + stats.BonusDamageMultiplier
                + stats.BonusShadowDamageMultiplier + stats.BonusHolyDamageMultiplier
                + stats.BonusDiseaseDamageMultiplier + stats.PriestInnerFire

                + stats.SWPDurationIncrease + stats.BonusMindBlastMultiplier
                + stats.MindBlastCostReduction + stats.ShadowWordDeathCritIncrease
                + stats.WeakenedSoulDurationDecrease + stats.ManaRestoreOnCast_5_15
                + stats.DevouringPlagueBonusDamage + stats.MindBlastHasteProc
                + stats.ManaRestoreFromBaseManaPerHit + stats.BonusSpellCritMultiplier
                + stats.ManaRestore + stats.SpellsManaReduction + stats.HighestStat
                + stats.ShadowDamage

                /*+ stats.SpellPowerFor15SecOnUse90Sec
                + stats.SpellPowerFor15SecOnUse2Min + stats.SpellPowerFor20SecOnUse2Min
                + stats.HasteRatingFor20SecOnUse2Min + stats.HasteRatingFor20SecOnUse5Min
                + stats.SpellPowerFor10SecOnCast_15_45 + stats.SpellPowerFor10SecOnHit_10_45
                + stats.SpellHasteFor10SecOnCast_10_45 + stats.TimbalsProc
                + stats.PendulumOfTelluricCurrentsProc + stats.ExtractOfNecromanticPowerProc*/                
            ) > 0;

            bool Maybe = (
                stats.Stamina + stats.Health + stats.Resilience
                + stats.Armor + stats.BonusArmor + stats.Agility
                + stats.ArcaneResistance + stats.ArcaneResistanceBuff
                + stats.FireResistance + stats.FireResistanceBuff
                + stats.FrostResistance + stats.FrostResistanceBuff
                + stats.NatureResistance + stats.NatureResistanceBuff
                + stats.ShadowResistance + stats.ShadowResistanceBuff
            ) > 0;

            bool No = (
                stats.Strength + stats.AttackPower
                + stats.ArmorPenetration + stats.ArmorPenetrationRating
                + stats.Expertise + stats.ExpertiseRating
                + stats.Dodge + stats.DodgeRating
                + stats.Parry + stats.ParryRating
                + stats.Defense + stats.DefenseRating
            ) > 0;

            return Yes || (Maybe && !No);
        }

        public override bool HasRelevantStats(Stats stats)
        {
            bool isRelevant = _HasRelevantStats(stats);

            foreach (SpecialEffect se in stats.SpecialEffects())
                isRelevant |= RelevantTrinket(se);
            return isRelevant;
        }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsShadowPriest));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsShadowPriest calcOpts = serializer.Deserialize(reader) as CalculationOptionsShadowPriest;
            return calcOpts;
        }
    }
}
