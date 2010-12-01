using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rawr.Warlock
{
    [Rawr.Calculations.RawrModelInfo("Warlock", "Spell_Nature_FaerieFire", CharacterClass.Warlock)]
    public class CalculationsWarlock : CalculationsBase
    {
        // Basic Model Functionality
        public override CharacterClass TargetClass { get { return CharacterClass.Warlock; } }
        private CalculationOptionsPanelWarlock _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelWarlock();
                }
                return _calculationOptionsPanel;
            }
        }
        public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationWarlock();
        }
        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsWarlock();
        }
        public const float AVG_UNHASTED_CAST_TIME = 2f; // total SWAG
        public override void SetDefaults(Character character)
        {
            character.ActiveBuffsAdd("Fel Armor");
        }
        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                {
                    _characterDisplayCalculationLabels = new string[] {
                        "Simulation:Personal DPS",
                        "Simulation:Pet DPS",
                        "Simulation:Total DPS",
                        "Basic Stats:Health",
                        "Basic Stats:Mana",
                        "Basic Stats:Bonus Damage",
                        "Basic Stats:Hit Rating",
                        "Basic Stats:Crit Chance",
                        "Basic Stats:Average Haste",
                        "Basic Stats:Mastery",
                        "Pet Stats:Pet Stamina",
                        "Pet Stats:Pet Intellect",
                        "Pet Stats:Pet Health",
                        "Affliction:Corruption",
                        "Affliction:Bane Of Agony",
                        "Affliction:Bane Of Doom",
                        "Affliction:Curse Of The Elements",
                        "Affliction:Drain Life",
                        "Affliction:Drain Soul",
                        "Affliction:Haunt",
                        "Affliction:Life Tap",
                        "Affliction:Unstable Affliction",
                        "Demonology:Immolation Aura",
                        "Destruction:Chaos Bolt",
                        "Destruction:Conflagrate",
                        "Destruction:Fel Flame",
                        "Destruction:Immolate",
                        "Destruction:Incinerate",
                        "Destruction:Incinerate (Under Backdraft)",
                        "Destruction:Incinerate (Under Molten Core)",
                        "Destruction:Searing Pain",
                        "Destruction:Soul Fire",
                        "Destruction:Shadow Bolt",
                        "Destruction:Shadow Bolt (Instant)",
                        "Destruction:Shadowburn"
                    };
                }
                return _characterDisplayCalculationLabels;
            }
        }
        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                {
                    _optimizableCalculationLabels = new string[] { "Miss Chance"};
                }
                return _optimizableCalculationLabels;
            }
        }
        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                {
                    _customChartNames = new string[] { };
                }
                return _customChartNames;
            }
        }
        private Dictionary<string, Color> _subPointNameColors = null;
        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, Color>();
                    _subPointNameColors.Add("DPS", Color.FromArgb(255, 255, 0, 0));
                    _subPointNameColors.Add("Pet DPS", Color.FromArgb(255, 0, 0, 255));
                }
                return _subPointNameColors;
            }
        }

        // Basic Calcuations
        public override ICalculationOptionBase DeserializeDataObject(string xml) 
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsWarlock));
            StringReader reader = new StringReader(xml);
            CalculationOptionsWarlock calcOpts = serializer.Deserialize(reader) as CalculationOptionsWarlock;
            return calcOpts;
        }
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item addlItem, bool _u1, bool _u2, bool _u3)
        {
            return new CharacterCalculationsWarlock(character, GetCharacterStats(character, addlItem), GetPetBuffStats(character));
        }
        private Stats GetPetBuffStats(Character character)
        {
            List<Buff> buffs = new List<Buff>();
            foreach (Buff buff in character.ActiveBuffs)
            {
                string group = buff.Group;
                if (   group != "Profession Buffs"
                    && group != "Set Bonuses"
                    && group != "Food"
                    && group != "Potion"
                    && group != "Elixirs and Flasks"
                    && group != "Focus Magic, Spell Critical Strike Chance")
                {
                    buffs.Add(buff);
                }
            }
            Stats stats = GetBuffsStats(buffs);
            var options = character.CalculationOptions as CalculationOptionsWarlock;
            ApplyPetsRaidBuff(stats, options.Pet, character.WarlockTalents, character.ActiveBuffs, options);
            return stats;
        }
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            WarlockTalents talents = character.WarlockTalents;
            CalculationOptionsWarlock options = character.CalculationOptions as CalculationOptionsWarlock;
            Stats stats = BaseStats.GetBaseStats(character);
            
            AccumulateItemStats(stats, character, additionalItem);
            AccumulateBuffsStats(stats, character.ActiveBuffs);
            ApplyPetsRaidBuff(stats, options.Pet, talents, character.ActiveBuffs, options);

            float[] demonicEmbraceValues = { 0f, .04f, .07f, .1f };
            Stats statsTalents = new Stats {
                //Demonic Embrace: increases your stamina by 4/7/10%
                BonusStaminaMultiplier = demonicEmbraceValues[talents.DemonicEmbrace],
            };

            if (talents.Eradication > 0)
            {
                float[] eradicationValues = { 0f, .06f, .12f, .20f };
                statsTalents.AddSpecialEffect(
                    new SpecialEffect(
                        Trigger.CorruptionTick,
                        new Stats() {
                            SpellHaste = eradicationValues[talents.Eradication]
                        },
                        10f,
                        0f,
                        .06f));
            }
            stats.Accumulate(statsTalents);
            stats.ManaRestoreFromMaxManaPerSecond
                = Math.Max(
                    stats.ManaRestoreFromMaxManaPerSecond,
                    .001f * Spell.CalcUprate(talents.SoulLeech > 0 ? 1f : 0f, 15f, options.Duration * 1.1f));
            return stats;
        }
        private void ApplyPetsRaidBuff(Stats stats, string pet, WarlockTalents talents, List<Buff> activeBuffs, CalculationOptionsWarlock options)
        {
            stats.Health += CalcPetHealthBuff(pet, talents, activeBuffs, options);
            stats.Mana += CalcPetManaBuff(pet, talents, activeBuffs, options);
            stats.Mp5 += CalcPetMP5Buff(pet, talents, activeBuffs, options);
        }
        private static float[] buffBaseValues = { 125f, 308f, 338f, 375f, 407f, 443f };
        public static float CalcPetHealthBuff(string pet, WarlockTalents talents, List<Buff> activeBuffs, CalculationOptionsWarlock options)
        {
            if (!pet.Equals("Imp"))
            {
                return 0f;
            }

            //spell ID 6307, effect ID 2190
            float SCALE = 1.3200000525f;
            return StatUtils.GetBuffEffect(activeBuffs, SCALE * buffBaseValues[options.PlayerLevel], "Health", s => s.Health);
        }
        public static float CalcPetManaBuff(string pet, WarlockTalents talents, List<Buff> activeBuffs, CalculationOptionsWarlock options)
        {
            if (!pet.Equals("Felhunter"))
            {
                return 0f;
            }

            //spell ID 54424, effect ID 47202
            float SCALE = 4.8000001907f;
            return StatUtils.GetBuffEffect(activeBuffs, SCALE * buffBaseValues[options.PlayerLevel - 80], "Mana", s => s.Mana);
        }
        public static float CalcPetMP5Buff(string pet, WarlockTalents talents, List<Buff> activeBuffs, CalculationOptionsWarlock options)
        {
            if (!pet.Equals("Felhunter"))
            {
                return 0f;
            }

            //spell ID 54424, effect ID 47203
            float SCALE = 0.7360000014f;
            return StatUtils.GetBuffEffect(activeBuffs, SCALE * buffBaseValues[options.PlayerLevel - 80], "Mana Regeneration", s => s.Mp5);
        }
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            return new ComparisonCalculationBase[0];
        }

        // Relevancy
        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<ItemType>(6) { 
                        ItemType.None, ItemType.Cloth, ItemType.Dagger, ItemType.Wand, ItemType.OneHandSword, ItemType.Staff 
                    };
                }
                return _relevantItemTypes;
            }
        }
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                const int WRATHUN = 0;
                const int WRATHRA = 1;
                const int WRATHEP = 2;
                const int WRATHJC = 3;
                const int CATAUN = 4;
                const int CATARA = 5;
                const int CATAJC = 6;
                const int CATAEP = 7;

                //Red
                int[] brilliant = { 39911, 39998, 40113, 42144, 52084, 52207, 52257, 0 }; //int

                //Yellow
                int[] fractured = { 0    , 0    , 0    , 0    , 52094, 0    , 52269, 0 }; //mastery
                int[] quick     = { 39918, 40017, 40128, 42150, 0    , 0    , 52268, 0 }; //haste
                int[] smooth    = { 39909, 40013, 40124, 42149, 0    , 0    , 52266, 0 }; //crit

                //Blue
                int[] rigid     = { 39915, 40014, 40125, 42156, 0    , 0    , 52264, 0 }; //hit

                //Purple
                int[] veiled    = { 39957, 40049, 40153, 0    , 52104, 0    , 0, 0 }; // int/hit

                //Orange
                int[] reckless  = { 39959, 40051, 40155, 0    , 52113, 0    , 0, 0 }; //int/haste
                int[] potent    = { 39956, 40048, 40152, 0    , 52239, 0    , 0, 0 }; //int/crit; aka shrewd?
                int[] artful    = { 0    , 0    , 0    , 0    , 52117, 52205, 0, 0 }; //int/mast

                //Green
                int[] lightning = { 39981, 40100, 40177, 0    , 0    , 52225, 0, 0 }; // haste/hit
                int[] piercing  = { 0    , 0    , 0    , 0    , 52122, 52228, 0, 0 }; // crit/hit
                int[] senseis  =  { 0    , 0    , 0    , 0    , 52128, 52237, 0, 0 }; // mast/hit

                //Meta
                const int WRATHMETA = 0;
                const int CATAMETA = 1;
                int[] ember   = { 41333, 52296 };
                int[] chaotic = { 41285, 52291 };

                return new List<GemmingTemplate>
                {
                    #region uncommon
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //Max SP - Ember
                        RedId = brilliant[WRATHUN], YellowId = brilliant[WRATHUN], BlueId = brilliant[WRATHUN], PrismaticId = brilliant[WRATHUN], MetaId = ember[WRATHMETA]
                    },
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //SP/Hit - Ember
                        RedId = brilliant[WRATHUN], YellowId = lightning[WRATHUN], BlueId = veiled[WRATHUN], PrismaticId = brilliant[WRATHUN], MetaId = ember[WRATHMETA]
                    },
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //SP/Haste - Ember
                        RedId = brilliant[WRATHUN], YellowId = reckless[WRATHUN], BlueId = lightning[WRATHUN], PrismaticId = brilliant[WRATHUN], MetaId = ember[WRATHMETA]
                    },
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //Max SP - Chaotic
                        RedId = brilliant[WRATHUN], YellowId = brilliant[WRATHUN], BlueId = brilliant[WRATHUN], PrismaticId = brilliant[WRATHUN], MetaId = chaotic[WRATHMETA]
                    },
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //SP/Hit - Chaotic
                        RedId = brilliant[WRATHUN], YellowId = lightning[WRATHUN], BlueId = veiled[WRATHUN], PrismaticId = brilliant[WRATHUN], MetaId = chaotic[WRATHMETA]
                    },
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //SP/Haste - Chaotic
                        RedId = brilliant[WRATHUN], YellowId = reckless[WRATHUN], BlueId = lightning[WRATHUN], PrismaticId = brilliant[WRATHUN], MetaId = chaotic[WRATHMETA]
                    },
                    #endregion

                    #region rare
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Rare", //Max SP - Ember
				        RedId = brilliant[WRATHRA], YellowId = brilliant[WRATHRA], BlueId = brilliant[WRATHRA], PrismaticId = brilliant[WRATHRA], MetaId = ember[WRATHMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Rare", //SP/Hit - Ember
				        RedId = brilliant[WRATHRA], YellowId = lightning[WRATHRA], BlueId = veiled[WRATHRA], PrismaticId = brilliant[WRATHRA], MetaId = ember[WRATHMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Rare", //SP/Haste - Ember
				        RedId = brilliant[WRATHRA], YellowId = reckless[WRATHRA], BlueId = lightning[WRATHRA], PrismaticId = brilliant[WRATHRA], MetaId = ember[WRATHMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Rare", //Max SP - Chaotic
				        RedId = brilliant[WRATHRA], YellowId = brilliant[WRATHRA], BlueId = brilliant[WRATHRA], PrismaticId = brilliant[WRATHRA], MetaId = chaotic[WRATHMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Rare", //SP/Hit - Chaotic
				        RedId = brilliant[WRATHRA], YellowId = lightning[WRATHRA], BlueId = veiled[WRATHRA], PrismaticId = brilliant[WRATHRA], MetaId = chaotic[WRATHMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Rare", //SP/Haste - Chaotic
				        RedId = brilliant[WRATHRA], YellowId = reckless[WRATHRA], BlueId = lightning[WRATHRA], PrismaticId = brilliant[WRATHRA], MetaId = chaotic[WRATHMETA]
                    },
                    #endregion

                    #region epic
                    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //Max SP - Ember
				        RedId = brilliant[WRATHEP], YellowId = brilliant[WRATHEP], BlueId = brilliant[WRATHEP], PrismaticId = brilliant[WRATHEP], MetaId = ember[WRATHMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //SP/Hit - Ember
				        RedId = brilliant[WRATHEP], YellowId = lightning[WRATHEP], BlueId = veiled[WRATHEP], PrismaticId = brilliant[WRATHEP], MetaId = ember[WRATHMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //SP/Haste - Ember
				        RedId = brilliant[WRATHEP], YellowId = reckless[WRATHEP], BlueId = lightning[WRATHEP], PrismaticId = brilliant[WRATHEP], MetaId = ember[WRATHMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //Max SP - Chaotic
				        RedId = brilliant[WRATHEP], YellowId = brilliant[WRATHEP], BlueId = brilliant[WRATHEP], PrismaticId = brilliant[WRATHEP], MetaId = chaotic[WRATHMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //SP/Hit - Chaotic
				        RedId = brilliant[WRATHEP], YellowId = lightning[WRATHEP], BlueId = veiled[WRATHEP], PrismaticId = brilliant[WRATHEP], MetaId = chaotic[WRATHMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //SP/Haste - Chaotic
				        RedId = brilliant[WRATHEP], YellowId = reckless[WRATHEP], BlueId = lightning[WRATHEP], PrismaticId = brilliant[WRATHEP], MetaId = chaotic[WRATHMETA]
                    },
                    #endregion

                    #region jeweler
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Jeweler", //Max SP - Ember
				        RedId = brilliant[WRATHJC], YellowId = brilliant[WRATHJC], BlueId = brilliant[WRATHJC], PrismaticId = brilliant[WRATHJC], MetaId = ember[WRATHMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Jeweler", //SP/Hit - Ember
				        RedId = brilliant[WRATHEP], YellowId = brilliant[WRATHJC], BlueId = brilliant[WRATHJC], PrismaticId = brilliant[WRATHEP], MetaId = ember[WRATHMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Jeweler", //Max SP - Chaotic
				        RedId = brilliant[WRATHJC], YellowId = brilliant[WRATHJC], BlueId = brilliant[WRATHJC], PrismaticId = brilliant[WRATHJC], MetaId = chaotic[WRATHMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Jeweler", //SP/Hit - Chaotic
				        RedId = brilliant[WRATHEP], YellowId = brilliant[WRATHJC], BlueId = brilliant[WRATHJC], PrismaticId = brilliant[WRATHEP], MetaId = chaotic[WRATHMETA]
                    },
                    #endregion

                    #region uncommon - cata
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //Max SP - Ember
                        RedId = brilliant[CATAUN], YellowId = brilliant[CATAUN], BlueId = brilliant[CATAUN], PrismaticId = brilliant[CATAUN], MetaId = ember[CATAMETA]
                    },
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //SP/Hit - Ember
                        RedId = brilliant[CATAUN], YellowId = lightning[CATAUN], BlueId = veiled[CATAUN], PrismaticId = brilliant[CATAUN], MetaId = ember[CATAMETA]
                    },
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //SP/Haste - Ember
                        RedId = brilliant[CATAUN], YellowId = reckless[CATAUN], BlueId = lightning[CATAUN], PrismaticId = brilliant[CATAUN], MetaId = ember[CATAMETA]
                    },
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //Max SP - Chaotic
                        RedId = brilliant[CATAUN], YellowId = brilliant[CATAUN], BlueId = brilliant[CATAUN], PrismaticId = brilliant[CATAUN], MetaId = chaotic[CATAMETA]
                    },
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //SP/Hit - Chaotic
                        RedId = brilliant[CATAUN], YellowId = lightning[CATAUN], BlueId = veiled[CATAUN], PrismaticId = brilliant[CATAUN], MetaId = chaotic[CATAMETA]
                    },
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //SP/Haste - Chaotic
                        RedId = brilliant[CATAUN], YellowId = reckless[CATAUN], BlueId = lightning[CATAUN], PrismaticId = brilliant[CATAUN], MetaId = chaotic[CATAMETA]
                    },
                    #endregion

                    #region rare - cata
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Rare", //Max SP - Ember
				        RedId = brilliant[CATARA], YellowId = brilliant[CATARA], BlueId = brilliant[CATARA], PrismaticId = brilliant[CATARA], MetaId = ember[CATAMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Rare", //SP/Hit - Ember
				        RedId = brilliant[CATARA], YellowId = lightning[CATARA], BlueId = veiled[CATARA], PrismaticId = brilliant[CATARA], MetaId = ember[CATAMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Rare", //SP/Haste - Ember
				        RedId = brilliant[CATARA], YellowId = reckless[CATARA], BlueId = lightning[CATARA], PrismaticId = brilliant[CATARA], MetaId = ember[CATAMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Rare", //Max SP - Chaotic
				        RedId = brilliant[CATARA], YellowId = brilliant[CATARA], BlueId = brilliant[CATARA], PrismaticId = brilliant[CATARA], MetaId = chaotic[CATAMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Rare", //SP/Hit - Chaotic
				        RedId = brilliant[CATARA], YellowId = lightning[CATARA], BlueId = veiled[CATARA], PrismaticId = brilliant[CATARA], MetaId = chaotic[CATAMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Rare", //SP/Haste - Chaotic
				        RedId = brilliant[CATARA], YellowId = reckless[CATARA], BlueId = lightning[CATARA], PrismaticId = brilliant[CATARA], MetaId = chaotic[CATAMETA]
                    },
                    #endregion

                    /*
                    #region epic - cata
                    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //Max SP - Ember
				        RedId = brilliant[CATAEP], YellowId = brilliant[CATAEP], BlueId = brilliant[CATAEP], PrismaticId = brilliant[CATAEP], MetaId = ember[CATAMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //SP/Hit - Ember
				        RedId = brilliant[CATAEP], YellowId = lightning[CATAEP], BlueId = veiled[CATAEP], PrismaticId = brilliant[CATAEP], MetaId = ember[CATAMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //SP/Haste - Ember
				        RedId = brilliant[CATAEP], YellowId = reckless[CATAEP], BlueId = lightning[CATAEP], PrismaticId = brilliant[CATAEP], MetaId = ember[CATAMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //Max SP - Chaotic
				        RedId = brilliant[CATAEP], YellowId = brilliant[CATAEP], BlueId = brilliant[CATAEP], PrismaticId = brilliant[CATAEP], MetaId = chaotic[CATAMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //SP/Hit - Chaotic
				        RedId = brilliant[CATAEP], YellowId = lightning[CATAEP], BlueId = veiled[CATAEP], PrismaticId = brilliant[CATAEP], MetaId = chaotic[CATAMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //SP/Haste - Chaotic
				        RedId = brilliant[CATAEP], YellowId = reckless[CATAEP], BlueId = lightning[CATAEP], PrismaticId = brilliant[CATAEP], MetaId = chaotic[CATAMETA]
                    },
                    #endregion
                    */

                    #region jeweler - cata
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Jeweler", //Max SP - Ember
				        RedId = brilliant[CATAJC], YellowId = brilliant[CATAJC], BlueId = brilliant[CATAJC], PrismaticId = brilliant[CATAJC], MetaId = ember[CATAMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Jeweler", //SP/Hit - Ember
				        RedId = brilliant[CATARA], YellowId = brilliant[CATAJC], BlueId = brilliant[CATAJC], PrismaticId = brilliant[CATARA], MetaId = ember[CATAMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Jeweler", //Max SP - Chaotic
				        RedId = brilliant[CATAJC], YellowId = brilliant[CATAJC], BlueId = brilliant[CATAJC], PrismaticId = brilliant[CATAJC], MetaId = chaotic[CATAMETA]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Jeweler", //SP/Hit - Chaotic
				        RedId = brilliant[CATARA], YellowId = brilliant[CATAJC], BlueId = brilliant[CATAJC], PrismaticId = brilliant[CATARA], MetaId = chaotic[CATAMETA]
                    },
                    #endregion
                };
            }
        }
        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats {
                SpellPower = stats.SpellPower,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                HitRating = stats.HitRating,
                SpellHit = stats.SpellHit,
                HasteRating = stats.HasteRating,
                SpellHaste = stats.SpellHaste,
                CritRating = stats.CritRating,
                SpellCrit = stats.SpellCrit,
                SpellCritOnTarget = stats.SpellCritOnTarget,
                MasteryRating = stats.MasteryRating,

                ShadowDamage = stats.ShadowDamage,
                SpellShadowDamageRating = stats.SpellShadowDamageRating,
                FireDamage = stats.FireDamage,
                SpellFireDamageRating = stats.SpellFireDamageRating,

                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusFireDamageMultiplier = stats.BonusFireDamageMultiplier,

                Warlock2T7 = stats.Warlock2T7,
                Warlock4T7 = stats.Warlock4T7,
                Warlock2T8 = stats.Warlock2T8,
                Warlock4T8 = stats.Warlock4T8,
                Warlock2T9 = stats.Warlock2T9,
                Warlock4T9 = stats.Warlock4T9,
                Warlock2T10 = stats.Warlock2T10,
                Warlock4T10 = stats.Warlock4T10,
                Warlock2T11 = stats.Warlock2T11,
                Warlock4T11 = stats.Warlock4T11,

                Stamina = stats.Stamina,
                Health = stats.Health,
                Mana = stats.Mana,
                Mp5 = stats.Mp5,

                HighestStat = stats.HighestStat,                                    //trinket - darkmoon card: greatness
                ManaRestoreFromBaseManaPPM = stats.ManaRestoreFromBaseManaPPM,      //paladin buff: judgement of wisdom
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,    //replenishment
                BonusManaPotion = stats.BonusManaPotion,                            //triggered when a mana pot is consumed
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,        //Bracing Eathsiege Diamond (metagem) effect
                ManaRestore = stats.ManaRestore,                                    //quite a few items that restore mana on spell cast or crit. Also used to model replenishment.
                SpellsManaReduction = stats.SpellsManaReduction,                    //spark of hope -> http://www.wowhead.com/?item=45703
            };

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (RelevantTrinket(effect))
                {
                    s.AddSpecialEffect(effect);
                }
            }
            return s;
        }
        public override bool IsBuffRelevant(Buff buff, Character character)
        {
            if (!buff.AllowedClasses.Contains(CharacterClass.Warlock)
                || buff.Group.Equals("Spell Sensitivity"))
            {
                return false;
            }
            if (character != null
                && Rawr.Properties.GeneralSettings.Default.HideProfEnchants
                && !character.HasProfession(buff.Professions))
            {
                return false;
            }
            Stats stats = buff.GetTotalStats();
            return HasRelevantStats(stats)
                || stats.Strength > 0
                || stats.Agility > 0
                || stats.AttackPower > 0
                || stats.BonusAttackPowerMultiplier > 0
                || stats.PhysicalCrit > 0
                || stats.PhysicalHaste > 0
                || stats.ArmorPenetration > 0
                || stats.TargetArmorReduction > 0
                || stats.BonusPhysicalDamageMultiplier > 0;
        }
        protected bool RelevantTrinket(SpecialEffect effect)
        {
            if (   effect.Trigger == Trigger.Use
                || effect.Trigger == Trigger.DamageSpellCast
                || effect.Trigger == Trigger.DamageSpellCrit
                || effect.Trigger == Trigger.DamageSpellHit
                || effect.Trigger == Trigger.SpellCast
                || effect.Trigger == Trigger.SpellCrit
                || effect.Trigger == Trigger.SpellHit
                || effect.Trigger == Trigger.SpellMiss
                || effect.Trigger == Trigger.DoTTick
                || effect.Trigger == Trigger.DamageDone
                || effect.Trigger == Trigger.DamageOrHealingDone)
            {
                return _HasRelevantStats(effect.Stats);
            }
            return false;
        }
        public override bool HasRelevantStats(Stats stats)
        {
            bool isRelevant = _HasRelevantStats(stats);
            foreach (SpecialEffect se in stats.SpecialEffects())
            {
                isRelevant |= RelevantTrinket(se);
            }
            return isRelevant;
        }
        protected bool _HasRelevantStats(Stats stats)
        {
            bool yes = (
                stats.SpellPower
                + stats.Intellect
                + stats.HitRating + stats.SpellHit
                + stats.HasteRating + stats.SpellHaste
                + stats.CritRating + stats.SpellCrit + stats.SpellCritOnTarget
                + stats.MasteryRating
                + stats.ShadowDamage + stats.SpellShadowDamageRating
                + stats.FireDamage + stats.SpellFireDamageRating

                + stats.BonusIntellectMultiplier
                + stats.BonusSpellCritMultiplier
                + stats.BonusDamageMultiplier + stats.BonusShadowDamageMultiplier + stats.BonusFireDamageMultiplier

                + stats.Warlock2T7
                + stats.Warlock4T7
                + stats.Warlock2T8
                + stats.Warlock4T8
                + stats.Warlock2T9
                + stats.Warlock4T9
                + stats.Warlock2T10
                + stats.Warlock4T10
                + stats.Warlock2T11
                + stats.Warlock4T11
            ) > 0;

            bool maybe = (
                  stats.Stamina + stats.Health
                + stats.Mana + stats.Mp5
                + stats.HighestStat                     //darkmoon card: greatness
                + stats.SpellsManaReduction             //spark of hope -> http://www.wowhead.com/?item=45703
                + stats.BonusManaPotion                 //triggered when a mana pot is consumed
                + stats.ManaRestoreFromBaseManaPPM      //judgement of wisdom
                + stats.ManaRestoreFromMaxManaPerSecond //replenishment sources
                + stats.ManaRestore                     //quite a few items that restore mana on spell cast or crit. Also used to model replenishment.
            ) > 0;

            bool no = (
                //ignore items with any of these stats
                + stats.Resilience
                + stats.Armor + stats.BonusArmor + stats.Agility
                + stats.ArmorPenetration + stats.ArmorPenetrationRating + stats.TargetArmorReduction
                + stats.Strength + stats.AttackPower
                + stats.Expertise + stats.ExpertiseRating
                + stats.Dodge + stats.DodgeRating
                + stats.Parry + stats.ParryRating
                + stats.Defense + stats.DefenseRating
                + stats.ThreatReductionMultiplier       //bracing earthsiege diamond (metagem) effect
            ) > 0;

            return yes || (maybe && !no);
        }
        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot)
        {
            if (slot == ItemSlot.OffHand || slot == ItemSlot.Ranged) { return false; }
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
                _relevantGlyphs
                    = new List<string>{
                        "Glyph of Metamorphosis",
                        "Glyph of Corruption",
                        "Glyph of Life Tap",
                        "Glyph of Bane of Agony",
                        "Glyph of Lash of Pain",
                        "Glyph of Shadowburn",
                        "Glyph of Unstable Affliction",
                        "Glyph of Haunt",
                        "Glyph of Chaos Bolt",
                        "Glyph of Immolate",
                        "Glyph of Incinerate",
                        "Glyph of Conflagrate",
                        "Glyph of Imp",
                        "Glyph of Felguard",
                        "Glyph of Shadow Bolt"};
            }
            return _relevantGlyphs;
        }
    }
}