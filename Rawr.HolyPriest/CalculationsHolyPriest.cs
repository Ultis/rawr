using System;
using System.Collections.Generic;

using Rawr;

namespace Rawr.HolyPriest
{
	[Rawr.Calculations.RawrModelInfo("HolyPriest", "Spell_Holy_Renew", Character.CharacterClass.Priest)]
	public class CalculationsHolyPriest : CalculationsBase 
    {
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                // Interesting Gem Choices for a Holy & Discipline Priest
                // Red
                int[] runed = { 39911, 39998, 40113, 42144 }; // +spp

                // Orange
                int[] durable = { 39958, 40050, 40154 }; // +spp/+resilience
                int[] luminous = { 39946, 40047, 40151 }; // +spp/+int
                int[] potent = { 39956, 40048, 40152 }; // +spp/+crit
                int[] reckless = { 39959, 40051, 40155 }; // +spp/+haste
                //int[] veiled = { 39957, 40049, 40153 }; // +spp/+hit

                // Yellow
                int[] brilliant = { 39912, 40012, 40123, 42148 }; // +int
                int[] mystic = { 39917, 40016, 40127, 42158 }; // +resilience
                int[] quick = { 39918, 40017, 40128, 42150 }; // +haste
                // int[] rigid = { 39915, 40014, 40125, 42156 }; // +hit
                int[] smooth = { 39914, 40013, 40124, 42149 }; // +crit

                // Green
                int[] dazzling = { 39984, 40094, 40175 }; // +int/+mp5
                int[] energized = { 39989, 40105, 40179 }; // +haste/+mp5
                int[] forceful = { 39978, 40091, 40169 }; // +haste/+sta
                int[] intricate = { 39983, 40104, 40174 }; // +haste/+spi
                int[] jagged = { 39974, 40086, 40165 }; // +crit/+stamina
                // int[] lambent = { 39986, 40100, 40177 }; // +hit/+mp5
                int[] misty = { 39980, 40095, 40171 }; // +crit/+spi
                int[] opaque = { 39988, 40103, 40178 }; // +resilience/+mp5
                int[] seers = { 39979, 40092, 40170 }; // +int/+spi 
                // int[] shining = { 39981, 40099, 40172 }; // +hit/+spi
                int[] steady = { 39977, 40090, 40168 }; // +resilience/+stamina
                int[] sundered = { 39985, 40096, 40176 }; // +crit/+mp5
                int[] timeless = { 39968, 40085, 40164 }; // +int/+stamina
                int[] turbid = { 39982, 40102, 40173 }; // +resilience/+spi
                // int[] vivid = { 39975, 40088, 40166 }; // +hit/+stamina

                // Blue
                int[] lustrous = { 39927, 40010, 40121, 42146 }; // +mp5
                int[] solid = { 39919, 40008, 40119, 36767 }; // +stamina
                int[] sparkling = { 39920, 40009, 40120, 42145 }; // +spirit

                // Purple
                int[] glowing = { 39936, 40025, 40132 }; // +spp/+stamina
                int[] purified = { 39941, 40026, 40133 }; // +spp/+spirit
                int[] royal = { 39943, 40027, 40134 }; // +spp/+mp5

                // Meta
                int[] beaming = { 41389 }; // +crit rating/+2% mana
                int[] ember = { 41333 }; // +spp/+2% int
                int[] insightful = { 41401 }; // +int/manarestore
                int[] revitalizing = { 41376 }; // +mp5/+3% heal effect
                int[] powerful = { 41397 }; // +stamina/-10% stun duration
 
                return new List<GemmingTemplate>() {
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Uncommon", // Max Mana
                        RedId = luminous[0], YellowId = brilliant[0], BlueId = seers[0], PrismaticId = brilliant[0], MetaId = insightful[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Uncommon", // Max Mana Regen
                        RedId = purified[0], YellowId = seers[0], BlueId = sparkling[0], PrismaticId = sparkling[0], MetaId = insightful[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Uncommon", // Max SPP
                        RedId = runed[0], YellowId = luminous[0], BlueId = purified[0], PrismaticId = runed[0], MetaId = ember[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Uncommon", // Max SPP w/revitalizing
                        RedId = runed[0], YellowId = luminous[0], BlueId = purified[0], PrismaticId = runed[0], MetaId = revitalizing[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Uncommon", // Max Haste
                        RedId = runed[0], YellowId = reckless[0], BlueId = intricate[0], PrismaticId = runed[0], MetaId = ember[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Uncommon", // Max Crit
                        RedId = runed[0], YellowId = potent[0], BlueId = misty[0], PrismaticId = runed[0], MetaId = beaming[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Uncommon", // Max Blast
                        RedId = runed[0], YellowId = runed[0], BlueId = runed[0], PrismaticId = runed[0], MetaId = ember[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Uncommon", // PvP
                        RedId = glowing[0], YellowId = steady[0], BlueId = solid[0], PrismaticId = solid[0], MetaId = powerful[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Uncommon", // Max Mana w/MP5
                        RedId = luminous[0], YellowId = brilliant[0], BlueId = dazzling[0], PrismaticId = brilliant[0], MetaId = insightful[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Uncommon", // Max Mana Regen
                        RedId = royal[0], YellowId = dazzling[0], BlueId = lustrous[0], PrismaticId = lustrous[0], MetaId = insightful[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Uncommon", // Max SPP
                        RedId = runed[0], YellowId = luminous[0], BlueId = royal[0], PrismaticId = runed[0], MetaId = ember[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Uncommon", // Max SPP w/revitalizing
                        RedId = runed[0], YellowId = luminous[0], BlueId = royal[0], PrismaticId = runed[0], MetaId = revitalizing[0] },

                    new GemmingTemplate() { Model = "HolyPriest", Group = "Rare", Enabled = true, // Max Mana
                        RedId = luminous[1], YellowId = brilliant[1], BlueId = seers[1], PrismaticId = brilliant[1], MetaId = insightful[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Rare", // Max Mana Regen
                        RedId = purified[1], YellowId = seers[1], BlueId = sparkling[1], PrismaticId = sparkling[1], MetaId = insightful[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Rare", Enabled = true, // Max SPP
                        RedId = runed[1], YellowId = luminous[1], BlueId = purified[1], PrismaticId = runed[1], MetaId = ember[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Rare", Enabled = true, // Max SPP w/revitalizing
                        RedId = runed[1], YellowId = luminous[1], BlueId = purified[1], PrismaticId = runed[1], MetaId = revitalizing[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Rare", // Max Haste
                        RedId = runed[1], YellowId = reckless[1], BlueId = intricate[1], PrismaticId = runed[1], MetaId = ember[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Rare", // Max Crit
                        RedId = runed[1], YellowId = potent[1], BlueId = misty[1], PrismaticId = runed[1], MetaId = beaming[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Rare", Enabled = true, // Max Blast
                        RedId = runed[1], YellowId = runed[1], BlueId = runed[1], PrismaticId = runed[1], MetaId = ember[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Rare", // PvP
                        RedId = glowing[1], YellowId = steady[1], BlueId = solid[1], PrismaticId = solid[1], MetaId = powerful[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Rare", // Max Mana w/MP5
                        RedId = luminous[1], YellowId = brilliant[1], BlueId = dazzling[1], PrismaticId = brilliant[1], MetaId = insightful[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Rare", // Max Mana Regen
                        RedId = royal[1], YellowId = dazzling[1], BlueId = lustrous[1], PrismaticId = lustrous[1], MetaId = insightful[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Rare", // Max SPP
                        RedId = runed[1], YellowId = luminous[1], BlueId = royal[1], PrismaticId = runed[1], MetaId = ember[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Rare", // Max SPP w/revitalizing
                        RedId = runed[1], YellowId = luminous[1], BlueId = royal[1], PrismaticId = runed[1], MetaId = revitalizing[0] },


                    new GemmingTemplate() { Model = "HolyPriest", Group = "Epic", // Max Mana
                        RedId = luminous[2], YellowId = brilliant[2], BlueId = seers[2], PrismaticId = brilliant[2], MetaId = insightful[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Epic", // Max Mana Regen
                        RedId = purified[2], YellowId = seers[2], BlueId = sparkling[2], PrismaticId = sparkling[2], MetaId = insightful[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Epic", // Max SPP
                        RedId = runed[2], YellowId = luminous[2], BlueId = purified[2], PrismaticId = runed[2], MetaId = ember[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Epic", // Max SPP w/revitalizing
                        RedId = runed[2], YellowId = luminous[2], BlueId = purified[2], PrismaticId = runed[2], MetaId = revitalizing[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Epic", // Max Haste
                        RedId = runed[2], YellowId = reckless[2], BlueId = intricate[2], PrismaticId = runed[2], MetaId = ember[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Epic", // Max Crit
                        RedId = runed[2], YellowId = potent[2], BlueId = misty[2], PrismaticId = runed[2], MetaId = beaming[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Epic", // Max Blast
                        RedId = runed[2], YellowId = runed[2], BlueId = runed[2], PrismaticId = runed[2], MetaId = ember[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Epic", // PvP
                        RedId = glowing[2], YellowId = steady[2], BlueId = solid[2], PrismaticId = solid[2], MetaId = powerful[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Epic", // Max Mana w/MP5
                        RedId = luminous[2], YellowId = brilliant[2], BlueId = dazzling[2], PrismaticId = brilliant[2], MetaId = insightful[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Epic", // Max Mana Regen
                        RedId = royal[2], YellowId = dazzling[2], BlueId = lustrous[2], PrismaticId = lustrous[2], MetaId = insightful[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Epic", // Max SPP
                        RedId = runed[2], YellowId = luminous[2], BlueId = royal[2], PrismaticId = runed[2], MetaId = ember[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Epic", // Max SPP w/revitalizing
                        RedId = runed[2], YellowId = luminous[2], BlueId = royal[2], PrismaticId = runed[2], MetaId = revitalizing[0] },

                    new GemmingTemplate() { Model = "HolyPriest", Group = "Jeweler", // Max Mana
                        RedId = brilliant[3], YellowId = brilliant[1], BlueId = brilliant[3], PrismaticId = brilliant[1], MetaId = insightful[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Jeweler", // Max Mana Regen
                        RedId = sparkling[3], YellowId = sparkling[3], BlueId = sparkling[1], PrismaticId = sparkling[3], MetaId = insightful[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Jeweler", // Max SPP
                        RedId = runed[1], YellowId = runed[3], BlueId = runed[3], PrismaticId = runed[1], MetaId = ember[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Jeweler", // Max SPP w/revitalizing
                        RedId = runed[1], YellowId = runed[3], BlueId = runed[3], PrismaticId = runed[1], MetaId = revitalizing[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Jeweler", // Max Haste
                        RedId = quick[3], YellowId = quick[1], BlueId = quick[3], PrismaticId = quick[1], MetaId = ember[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Jeweler", // Max Crit
                        RedId = smooth[3], YellowId = smooth[1], BlueId = smooth[3], PrismaticId = smooth[1], MetaId = beaming[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Jeweler", // PvP
                        RedId = solid[3], YellowId = solid[3], BlueId = solid[1], PrismaticId = solid[1], MetaId = powerful[0] },
                    new GemmingTemplate() { Model = "HolyPriest", Group = "Jeweler", // Max Mana Regen
                        RedId = lustrous[3], YellowId = lustrous[3], BlueId = lustrous[1], PrismaticId = lustrous[1], MetaId = insightful[0] },
               
                };
            }
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
                    case "MP5 Sources":
                        _subPointNameColors.Add(string.Format("MP5 Sources ({0} total)", _currentChartTotal.ToString("0")), System.Drawing.Color.Blue);
                        break;
                    case "Spell HpS":
                        _subPointNameColors.Add("HpS", System.Drawing.Color.Red);
                        break;
                    case "Spell HpM":
                        _subPointNameColors.Add("HpM", System.Drawing.Color.Red);
                        break;
                    default:
                        _subPointNameColors.Add("HPS-Burst", System.Drawing.Color.Red);
                        _subPointNameColors.Add("HPS-Sustained", System.Drawing.Color.Blue);
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
					"Basic Stats:Mana",
					"Basic Stats:Stamina",
                    "Basic Stats:Resilience",
					"Basic Stats:Intellect",
					"Basic Stats:Spirit",
                    "Basic Stats:Spell Power",
					"Basic Stats:In FSR MP5",
                    "Basic Stats:Spell Crit",
					"Basic Stats:Healing Crit",
					"Basic Stats:Spell Haste",
                    "Basic Stats:Armor",
                    "Basic Stats:Resistance",
                    "Simulation:Role",
                    "Simulation:Burst*This is the HPS you are expected to have if you are not limited by Mana.\r\nIn Custom Role, this displays your HPS when you dump all spells in 1 stream.",
                    "Simulation:Sustained*This is the HPS are expected to have when restricted by Mana.\r\nIf this value is lower than your Burst HPS, you are running out of mana in the simulation.\r\nIn Custom Role, this displays your HPS over the length of the fight, adjusted by the amount of mana available.",
                    "Spells:Greater Heal",
                    "Spells:Flash Heal",
				    "Spells:Binding Heal",
                    "Spells:Renew",
                    "Spells:Prayer of Mending",
                    "Spells:Power Word Shield",
                    "Spells:PoH",
				    "Spells:Holy Nova",
                    "Spells:Lightwell",
				    "Spells:CoH",
                    "Spells:Penance",
                    "Spells:Gift of the Naaru"
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
                    "InFSR Regen",
                    "OutFSR Regen",
					"Haste Rating",
                    "Haste %",
                    "Crit Rating",
                    "Healing Crit %",
                    "PW:Shield",
                    "GHeal Avg",
                    "FHeal Avg",
                    "CoH Avg",
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
                    _calculationOptionsPanel = new CalculationOptionsPanelHolyPriest();
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
                    _customChartNames = new string[] { "MP5 Sources", "Spell HpS", "Spell HpM", "Spell AoE HpS", "Spell AoE HpM", "Relative Stat Values" };
                return _customChartNames;
            }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationHolyPriest(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsHolyPriest(); }

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

        public override bool ItemFitsInSlot(Item item, Character character, Character.CharacterSlot slot)
        {
            if (slot == Character.CharacterSlot.OffHand && item.Slot == Item.ItemSlot.OneHand) return false;
            return base.ItemFitsInSlot(item, character, slot);
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange)
        {
            Stats stats = GetCharacterStats(character, additionalItem);
            Stats statsRace = GetRaceStats(character);
            CharacterCalculationsHolyPriest calculatedStats = new CharacterCalculationsHolyPriest();
            CalculationOptionsPriest calculationOptions = character.CalculationOptions as CalculationOptionsPriest;
            if (calculationOptions == null)
                return null;

            calculatedStats.Race = character.Race;
            calculatedStats.BasicStats = stats;
            calculatedStats.Character = character;

            calculatedStats.SpiritRegen = (float)Math.Floor(5 * StatConversion.GetSpiritRegenSec(calculatedStats.BasicStats.Spirit, calculatedStats.BasicStats.Intellect));
            if (calculationOptions.NewManaRegen)
            {
                calculatedStats.SpiritRegen *= 0.6f;
                stats.SpellCombatManaRegeneration *= 5f / 3f;
            }
            calculatedStats.RegenInFSR = calculatedStats.SpiritRegen * calculatedStats.BasicStats.SpellCombatManaRegeneration;
            calculatedStats.RegenOutFSR = calculatedStats.SpiritRegen;

            BaseSolver solver;
            if (calculationOptions.Rotation == 10)
                solver = new AdvancedSolver(stats, character, statsRace.Mana);
            else
                solver = new Solver(stats, character, statsRace.Mana);
            solver.Calculate(calculatedStats);

            return calculatedStats;
        }

        public static float GetRaptureConst(Character character)
        {
            if (character.Level < 70)
                return 0.01035f; // Dunno
            return 0.01035f - (0.00845f * (character.Level - 70) / 10f);
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
            float ArmorBonus = 2440;

            return ArmorBonus * (1f + character.PriestTalents.ImprovedInnerFire * 0.15f);
        }

        public static Stats GetRaceStats(Character character)
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
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            //Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTalents = new Stats()
            {
                BonusStaminaMultiplier = character.PriestTalents.Enlightenment * 0.01f,
                BonusSpiritMultiplier = (1 + character.PriestTalents.Enlightenment * 0.01f) * (1f + character.PriestTalents.SpiritOfRedemption * 0.05f) - 1f,
                BonusIntellectMultiplier = character.PriestTalents.MentalStrength * 0.03f,
                SpellDamageFromSpiritPercentage = character.PriestTalents.SpiritualGuidance * 0.05f + character.PriestTalents.TwistedFaith * 0.02f,
                SpellHaste = character.PriestTalents.Enlightenment * 0.01f,
                SpellCombatManaRegeneration = character.PriestTalents.Meditation * 0.1f
            };

            Stats statsTotal = statsBaseGear + statsBuffs + statsRace + statsTalents;

            statsTotal.Stamina = (float)Math.Floor((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.SpellPower += (float)Math.Round(statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit
                + GetInnerFireSpellPowerBonus(character));
            statsTotal.Mana += (statsTotal.Intellect - 20f) * 15f + 20f;
            statsTotal.Mana *= (1f + statsTotal.BonusManaMultiplier);
            statsTotal.Health += statsTotal.Stamina * 10f;
            statsTotal.SpellCrit += StatConversion.GetSpellCritFromIntellect(statsTotal.Intellect)
                + StatConversion.GetSpellCritFromRating(statsTotal.CritRating)
                + 0.0124f;
            statsTotal.SpellHaste += StatConversion.GetSpellHasteFromRating(statsTotal.HasteRating);
            statsTotal.BonusArmor += statsTotal.Agility * 2f + GetInnerFireArmorBonus(character);    
            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            ComparisonCalculationBase comparison;
            CharacterCalculationsHolyPriest p;
            List<Spell> spellList;

            _currentChartTotal = 0;
            switch (chartName)
            {
                case "MP5 Sources":
                    _currentChartName = chartName;
                    CharacterCalculationsHolyPriest mscalcs = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    BaseSolver mssolver;
                    if ((character.CalculationOptions as CalculationOptionsPriest).Rotation == 10)
                        mssolver = new AdvancedSolver(mscalcs.BasicStats, character, CalculationsHolyPriest.GetRaceStats(character).Mana);
                    else
                        mssolver = new Solver(mscalcs.BasicStats, character, CalculationsHolyPriest.GetRaceStats(character).Mana);
                    mssolver.Calculate(mscalcs);
                    foreach (Solver.ManaSource Source in mssolver.ManaSources)
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
                case "Spell AoE HpS":
                    _currentChartName = chartName;
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>();
                    spellList.Add(new Renew(p.BasicStats, character));
                    spellList.Add(new FlashHeal(p.BasicStats, character));
                    spellList.Add(new Heal(p.BasicStats, character));
                    spellList.Add(new PrayerOfHealing(p.BasicStats, character, 5));
                    spellList.Add(new BindingHeal(p.BasicStats, character));
                    spellList.Add(new PrayerOfMending(p.BasicStats, character, 5));
                    spellList.Add(new CircleOfHealing(p.BasicStats, character, 5));
                    spellList.Add(new HolyNova(p.BasicStats, character, 5));
                    spellList.Add(new Penance(p.BasicStats, character));
                    spellList.Add(new PowerWordShield(p.BasicStats, character));

                    foreach (Spell spell in spellList)
                    {
                        if(spell.AvgHeal == 0)
                            continue;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.Equipped = false;
                        comparison.SubPoints[0] = spell.HpS;
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparisonList.Add(comparison);
                    }

                    return comparisonList.ToArray();
                case "Spell AoE HpM":
                    _currentChartName = chartName;
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>();
                    spellList.Add(new Renew(p.BasicStats, character));
                    spellList.Add(new FlashHeal(p.BasicStats, character));
                    spellList.Add(new Heal(p.BasicStats, character));
                    spellList.Add(new PrayerOfHealing(p.BasicStats, character, 5));
                    spellList.Add(new BindingHeal(p.BasicStats, character));
                    spellList.Add(new PrayerOfMending(p.BasicStats, character, 5));
                    spellList.Add(new CircleOfHealing(p.BasicStats, character, 5));
                    spellList.Add(new HolyNova(p.BasicStats, character, 5));
                    spellList.Add(new Penance(p.BasicStats, character));
                    spellList.Add(new PowerWordShield(p.BasicStats, character));

                    foreach (Spell spell in spellList)
                    {
                        if (spell.AvgHeal == 0)
                            continue;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.Equipped = false;
                        comparison.SubPoints[0] = spell.HpM;
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparisonList.Add(comparison);
                    }

                    return comparisonList.ToArray();
                case "Spell HpS":
                    _currentChartName = chartName;
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>();
                    spellList.Add(new Renew(p.BasicStats, character));
                    spellList.Add(new FlashHeal(p.BasicStats, character));
                    spellList.Add(new Heal(p.BasicStats, character));
                    spellList.Add(new PrayerOfHealing(p.BasicStats, character, 1));
                    spellList.Add(new BindingHeal(p.BasicStats, character));
                    spellList.Add(new PrayerOfMending(p.BasicStats, character, 1));
                    spellList.Add(new CircleOfHealing(p.BasicStats, character, 1));
                    spellList.Add(new HolyNova(p.BasicStats, character, 1));
                    spellList.Add(new Penance(p.BasicStats, character));
                    spellList.Add(new PowerWordShield(p.BasicStats, character));

                    foreach (Spell spell in spellList)
                    {
                        if (spell.AvgHeal == 0)
                            continue;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.Equipped = false;
                        comparison.SubPoints[0] = spell.HpS;
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparisonList.Add(comparison);
                    }

                    return comparisonList.ToArray();
                case "Spell HpM":
                    _currentChartName = chartName;
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>();
                    spellList.Add(new Renew(p.BasicStats, character));
                    spellList.Add(new FlashHeal(p.BasicStats, character));
                    spellList.Add(new Heal(p.BasicStats, character));
                    spellList.Add(new PrayerOfHealing(p.BasicStats, character, 1));
                    spellList.Add(new BindingHeal(p.BasicStats, character));
                    spellList.Add(new PrayerOfMending(p.BasicStats, character, 1));
                    spellList.Add(new CircleOfHealing(p.BasicStats, character, 1));
                    spellList.Add(new HolyNova(p.BasicStats, character, 1));
                    spellList.Add(new Penance(p.BasicStats, character));
                    spellList.Add(new PowerWordShield(p.BasicStats, character));

                    foreach (Spell spell in spellList)
                    {
                        if (spell.AvgHeal == 0)
                            continue;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.Equipped = false;
                        comparison.SubPoints[0] = spell.HpM;
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparisonList.Add(comparison);
                    }

                    return comparisonList.ToArray();

                case "Relative Stat Values":
                    _currentChartName = chartName;
                    CharacterCalculationsHolyPriest calcsBase = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsIntellect = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsSpirit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Spirit = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsMP5 = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Mp5 = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsSpellPower = GetCharacterCalculations(character, new Item() { Stats = new Stats() { SpellPower = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsHaste = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsCrit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsSta = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsRes = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Resilience = 50 } }) as CharacterCalculationsHolyPriest;

                    return new ComparisonCalculationBase[] {
                        new ComparisonCalculationHolyPriest() { Name = "1 Intellect",
                            OverallPoints = (calcsIntellect.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsIntellect.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsIntellect.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsIntellect.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Spirit",
                            OverallPoints = (calcsSpirit.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsSpirit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsSpirit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsSpirit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 MP5",
                            OverallPoints = (calcsMP5.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsMP5.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsMP5.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsMP5.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Spell Power",
                            OverallPoints = (calcsSpellPower.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsSpellPower.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsSpellPower.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsSpellPower.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Haste",
                            OverallPoints = (calcsHaste.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsHaste.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsHaste.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsHaste.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Crit",
                            OverallPoints = (calcsCrit.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsCrit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsCrit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsCrit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Stamina",
                            OverallPoints = (calcsSta.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsSta.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsSta.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsSta.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Resilience",
                            OverallPoints = (calcsRes.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsRes.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsRes.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsRes.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        }};
                default:
                    _currentChartName = null;
                    return new ComparisonCalculationBase[0];
            }
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                Health = stats.Health,
                Mana = stats.Mana,
                Mp5 = stats.Mp5,
                SpellPower = stats.SpellPower,

                SpellHaste = stats.SpellHaste,
                SpellCrit = stats.SpellCrit,

                Resilience = stats.Resilience,
                CritRating = stats.CritRating,
                HasteRating = stats.HasteRating,
                
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                BonusManaMultiplier = stats.BonusManaMultiplier,
                BonusCritHealMultiplier = stats.BonusCritHealMultiplier,

                SpellDamageFromSpiritPercentage = stats.SpellDamageFromSpiritPercentage,
                HealingReceivedMultiplier = stats.HealingReceivedMultiplier,
                BonusManaPotion = stats.BonusManaPotion,
                SpellCombatManaRegeneration = stats.SpellCombatManaRegeneration,

                ManaGainOnGreaterHealOverheal = stats.ManaGainOnGreaterHealOverheal,
                RenewDurationIncrease = stats.RenewDurationIncrease,
                BonusPoHManaCostReductionMultiplier = stats.BonusPoHManaCostReductionMultiplier,
                BonusGHHealingMultiplier = stats.BonusGHHealingMultiplier,
                PrayerOfMendingExtraJumps = stats.PrayerOfMendingExtraJumps,
                GreaterHealCostReduction = stats.GreaterHealCostReduction,
                WeakenedSoulDurationDecrease = stats.WeakenedSoulDurationDecrease,
                ManaregenFor8SecOnUse5Min = stats.ManaregenFor8SecOnUse5Min,
                SpellPowerFor20SecOnUse2Min = stats.SpellPowerFor20SecOnUse2Min,
                SpellPowerFor15SecOnUse90Sec = stats.SpellPowerFor15SecOnUse90Sec,
                SpiritFor20SecOnUse2Min = stats.SpiritFor20SecOnUse2Min,
                HasteRatingFor20SecOnUse2Min = stats.HasteRatingFor20SecOnUse2Min,
                Mp5OnCastFor20SecOnUse2Min = stats.Mp5OnCastFor20SecOnUse2Min,
                ManaregenOver12SecOnUse3Min = stats.ManaregenOver12SecOnUse3Min,
                ManaregenOver12SecOnUse5Min = stats.ManaregenOver12SecOnUse5Min,
                ManacostReduceWithin15OnHealingCast = stats.ManacostReduceWithin15OnHealingCast,
                FullManaRegenFor15SecOnSpellcast = stats.FullManaRegenFor15SecOnSpellcast,
                BangleProc = stats.BangleProc,
                SpellHasteFor10SecOnCast_10_45 = stats.SpellHasteFor10SecOnCast_10_45,
                ManaRestoreOnCrit_25_45 = stats.ManaRestoreOnCrit_25_45,
                ManaRestoreOnCast_10_45 = stats.ManaRestoreOnCast_10_45,

                GLYPH_CircleOfHealing =  stats.GLYPH_CircleOfHealing,
                GLYPH_Dispel = stats.GLYPH_Dispel,
                GLYPH_FlashHeal = stats.GLYPH_FlashHeal,
                GLYPH_PowerWordShield = stats.GLYPH_PowerWordShield,
                GLYPH_PrayerOfHealing = stats.GLYPH_PrayerOfHealing,
                GLYPH_Renew = stats.GLYPH_Renew,
                GLYPH_HolyNova = stats.GLYPH_HolyNova,
                GLYPH_Lightwell = stats.GLYPH_Lightwell,
                GLYPH_MassDispel = stats.GLYPH_MassDispel,

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
        }

        // Trinket Status:
        // Correctly implemented:
        // http://www.wowhead.com/?item=40382 - Soul of the Dead
        
        // Wrongly implemented:
        // http://www.wowhead.com/?item=37835 - Je'Tze's Bell

        // Not implemented:
        // http://www.wowhead.com/?item=44253 - Darkmoon Card: Greatness (But for 90 spi/90 int versions, not yet known)
        // http://www.wowhead.com/?item=42988 - Darkmoon Card: Illusion
        // http://www.wowhead.com/?item=40258 - Forethought Talisman
        // http://www.wowhead.com/?item=40432 - Illustration of the Dragon Soul
        // http://www.wowhead.com/?item=40532 - Living Ice Crystals
        // http://www.wowhead.com/?item=40430 - Majestic Dragon Figurine

        public override bool HasRelevantStats(Stats stats)
        {
            bool Yes = (
                stats.Intellect + stats.Spirit + stats.Mana + stats.Mp5 + stats.SpellPower
                + stats.SpellHaste + stats.SpellCrit
                + stats.HasteRating + stats.CritRating
                + stats.BonusIntellectMultiplier + stats.BonusSpiritMultiplier + stats.BonusManaMultiplier + stats.BonusCritHealMultiplier
                + stats.SpellDamageFromSpiritPercentage + stats.HealingReceivedMultiplier + stats.BonusManaPotion + stats.SpellCombatManaRegeneration

                + stats.ManaGainOnGreaterHealOverheal + stats.RenewDurationIncrease
                + stats.BonusPoHManaCostReductionMultiplier + stats.BonusGHHealingMultiplier
                + stats.PrayerOfMendingExtraJumps + stats.GreaterHealCostReduction
                + stats.WeakenedSoulDurationDecrease

                + stats.ManaregenFor8SecOnUse5Min + stats.SpellPowerFor20SecOnUse2Min
                + stats.SpellPowerFor15SecOnUse90Sec + stats.SpiritFor20SecOnUse2Min + stats.HasteRatingFor20SecOnUse2Min
                + stats.Mp5OnCastFor20SecOnUse2Min + stats.ManaregenOver12SecOnUse3Min + stats.ManaregenOver12SecOnUse5Min
                + stats.ManacostReduceWithin15OnHealingCast + stats.FullManaRegenFor15SecOnSpellcast
                + stats.BangleProc + stats.SpellHasteFor10SecOnCast_10_45 + stats.ManaRestoreOnCrit_25_45
                + stats.ManaRestoreOnCast_10_45

                + stats.GLYPH_CircleOfHealing + stats.GLYPH_Dispel + stats.GLYPH_FlashHeal
                + stats.GLYPH_PowerWordShield + stats.GLYPH_PrayerOfHealing + stats.GLYPH_Renew
                + stats.GLYPH_HolyNova + stats.GLYPH_Lightwell + stats.GLYPH_MassDispel
            ) > 0;

            bool Maybe = (
                stats.Stamina + stats.Health + stats.Resilience
                + stats.Armor + stats.BonusArmor + stats.Agility +
                + stats.SpellHit + stats.HitRating
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
                + stats.PhysicalHit
            ) > 0;

            return Yes || (Maybe && !No);
        }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsPriest));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsPriest calcOpts = serializer.Deserialize(reader) as CalculationOptionsPriest;
            return calcOpts;
        }
    }
}
