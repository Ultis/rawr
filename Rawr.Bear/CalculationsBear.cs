/////////////////////////////////
//
// Written by Chadd Nervig (aka Astrylian) for Rawr. E-mail me at cnervig@hotmail.com.
//
// Rawr is a program for comparing and exploring gear for characters in the MMORPG, 
// World of Warcraft. It has been designed from the start to be fun to use, and helpful in 
// finding better combinations of gear, and what gear to obtain. (.NET, WinForm, C# 3.0, XML)
//
// Rawr is designed as a platform for hosting 'models', which implement the features and
// calculations needed by a specific class or specialization of WoW character.
//
// This file contains the majority of the classes and methods for the Rawr.Bear model,
// which models combat for the Druid Bear type of character.
//
// Please visit http://rawr.codeplex.com/ for the full source code of Rawr, or contact me
// for additional details.
//
/////////////////////////////////

using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text;

namespace Rawr.Bear
{
    /// <summary>
    /// Core class representing the Rawr.Bear model
    /// </summary>
    [Rawr.Calculations.RawrModelInfo("Bear", "Ability_Racial_BearForm", CharacterClass.Druid, CharacterRole.Tank)]
    public class CalculationsBear : CalculationsBase
    {
        #region Basic Model Properties and Methods
        #region Gemming Templates
        /// <summary>
        /// GemmingTemplates to be used by default, when none are defined
        /// </summary>
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                // Relevant Gem IDs for Bears
                //Red
                int[] delicate = { 52082, 52212, 71879 }; // Agi
                int[] precise = { 52085, 52230, 71880 }; // Exp

                //Purple
                int[] accurate = { 52105, 52203, 71863 }; // Exp/Hit
                int[] guardians = { 52099, 52221, 71870 }; // Exp/Sta
                int[] glinting = { 52102, 52220, 71862 }; // Agi/Hit
                int[] shifting = { 52096, 52238, 71869 }; // Agi/Sta
                
                //Blue
                int[] solid = { 52086, 52242, 71820 }; // Sta
                int[] rigid = { 52089, 52235, 71817 }; // Hit
                
                //Green
                int[] jagged = { 52121, 52223, 71834 }; // Crit/Sta
                int[] nimble = { 52120, 52227, 71837 }; // Dodge/Hit
                int[] piercing = { 52122, 52228, 71823 }; // Crit/Hit
                int[] puissant = { 52126, 52231, 71838 }; // Mastery/Sta
                int[] regal = { 52119, 52233, 71835 }; // Dodge/Sta
                int[] senseis = { 52128, 52233, 71825 }; // Mastery/Hit
                
                //Yellow
                int[] fractured = { 52094, 52219, 71877 }; // Mastery
                int[] subtle = { 52090, 52247, 71875 }; // Dodge
                
                //Orange
                int[] adept = { 52115, 52204, 71852 }; // Agi/Mastery
                int[] deadly = { 52109, 52209, 71840 }; // Agi/Crit
                int[] deft = { 52112, 52211, 71848 }; // Agi/Haste
                int[] keen = { 52118, 52224, 71853 }; // Exp/Mastery
                int[] polished = { 52106, 52229, 71844 }; // Agi/Dodge
                int[] resolute = { 52107, 52249, 71845 }; // Exp/Dodge

                //Prismatic

                //Meta
                int austere = 52294; //Sta/Armor - 2Y
                //int chaotic = 52291; //Crit/CritDmg - B>R
                //int destructive = 52298; //Crit/Reflect - 2R
                //int fleet = 52289; //Mastery/Runspeed - 2Y

                // Cogwheels
                int[] cog_exp = { 59489, 59489, 59489, 59489 }; fixArray(cog_exp);
                int[] cog_hit = { 59493, 59493, 59493, 59493 }; fixArray(cog_hit);
                int[] cog_mst = { 59480, 59480, 59480, 59480 }; fixArray(cog_mst);
                int[] cog_crt = { 59478, 59478, 59478, 59478 }; fixArray(cog_crt);
                int[] cog_has = { 59479, 59479, 59479, 59479 }; fixArray(cog_has);
                int[] cog_pry = { 59491, 59491, 59491, 59491 }; fixArray(cog_pry);
                int[] cog_ddg = { 59477, 59477, 59477, 59477 }; fixArray(cog_ddg);
                int[] cog_spr = { 59496, 59496, 59496, 59496 }; fixArray(cog_spr);

                List<GemmingTemplate> list = new List<GemmingTemplate>();
                for (int tier = 0; tier < 3; tier++)
                {
                    list.AddRange(new GemmingTemplate[]
                        {
                            CreateBearGemmingTemplate(tier,	 delicate,   delicate, 	delicate,	delicate,	austere), 
                            CreateBearGemmingTemplate(tier,	 delicate,   polished, 	shifting,	delicate,	austere), 
                            CreateBearGemmingTemplate(tier,	 delicate,   adept, 	shifting,	delicate,	austere),
                            
                            CreateBearGemmingTemplate(tier,	 precise,    precise, 	precise,	precise,	austere), 
                            CreateBearGemmingTemplate(tier,	 precise,    resolute, 	guardians,	precise,	austere), 
                            CreateBearGemmingTemplate(tier,	 precise,    keen, 	    guardians,	precise,	austere), 
                            CreateBearGemmingTemplate(tier,	 precise,    deadly, 	guardians,	precise,	austere),
                            CreateBearGemmingTemplate(tier,	 precise,    resolute, 	accurate,	precise,	austere), 
                            CreateBearGemmingTemplate(tier,	 precise,    keen, 	    accurate,	precise,	austere),  
                            CreateBearGemmingTemplate(tier,	 precise,    deadly, 	accurate,	precise,	austere), 
                            
                            CreateBearGemmingTemplate(tier,	 solid,      solid, 	solid,  	solid,  	austere), 
                            CreateBearGemmingTemplate(tier,	 shifting,   puissant, 	solid,  	solid,  	austere), 
                            
                            //CreateBearGemmingTemplate(tier,	 rigid,      rigid, 	rigid,	    rigid,	    austere), 
                            //CreateBearGemmingTemplate(tier,	 accurate,   piercing, 	rigid,	    rigid,	    austere),  
                            //CreateBearGemmingTemplate(tier,	 glinting,   piercing, 	rigid,	    rigid,	    austere), 
                            
                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_hit[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_mst[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_crt[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_has[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_pry[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_ddg[0], MetaId = austere, },
                            //new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_spr[0], MetaId = austere, },

                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_mst[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_crt[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_has[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_pry[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_ddg[0], MetaId = austere, },
                            //new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_spr[0], MetaId = austere, },

                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_crt[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_has[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_pry[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_ddg[0], MetaId = austere, },
                            //new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_spr[0], MetaId = austere, },

                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_crt[0], Cogwheel2Id = cog_has[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_crt[0], Cogwheel2Id = cog_pry[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_crt[0], Cogwheel2Id = cog_ddg[0], MetaId = austere, },
                            //new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_crt[0], Cogwheel2Id = cog_spr[0], MetaId = austere, },

                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_pry[0], Cogwheel2Id = cog_has[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_pry[0], Cogwheel2Id = cog_ddg[0], MetaId = austere, },
                            //new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_pry[0], Cogwheel2Id = cog_spr[0], MetaId = austere, },

                            new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_ddg[0], Cogwheel2Id = cog_has[0], MetaId = austere, },
                            //new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_ddg[0], Cogwheel2Id = cog_spr[0], MetaId = austere, },

                            //new GemmingTemplate() { Model = "Bear", Group = "Engineer", Enabled = false, CogwheelId = cog_spr[0], Cogwheel2Id = cog_has[0], MetaId = austere, },
                    });
                }

                return list;
            }
        }
        private static void fixArray(int[] thearray)
        {
            if (thearray[0] == 0) return; // Nothing to do, they are all 0
            if (thearray[1] == 0) thearray[1] = thearray[0]; // There was a Green Rarity, but no Blue Rarity
            if (thearray[2] == 0) thearray[2] = thearray[1]; // There was a Blue Rarity (or Green Rarity as set above), but no Purple Rarity
            if (thearray[3] == 0) thearray[3] = thearray[2]; // There was a Purple Rarity (or Blue Rarity/Green Rarity as set above), but no Jewel
        }
        private const int DEFAULT_GEMMING_TIER = 1;
        private GemmingTemplate CreateBearGemmingTemplate(int tier, int[] red, int[] yellow, int[] blue, int[] prismatic, int meta)
        {
            return new GemmingTemplate()
            {
                Model = "Bear",
                Group = (new string[] { "Uncommon", "Rare", "Epic", "Jewelcrafter" })[tier],
                Enabled = (tier == DEFAULT_GEMMING_TIER),
                RedId = red[tier],
                YellowId = yellow[tier],
                BlueId = blue[tier],
                PrismaticId = prismatic[tier],
                MetaId = meta
            };
        }
        #endregion

        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        /// <summary>
        /// Panel to be placed on the Options tab of the main form
        /// </summary>
        public override ICalculationOptionsPanel CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelBear();
                }
                return _calculationOptionsPanel;
            }
        }

        private string[] _characterDisplayCalculationLabels = null;
        /// <summary>
        /// Labels of the stats to display on the Stats tab of the main form
        /// </summary>
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[] {
                    @"Summary:Overall Points*Overall Points are a sum of Mitigation and Survival Points. 
Overall is typically, but not always, the best way to rate gear. 
For specific encounters, closer attention to Mitigation and 
Survival Points individually may be important.",
                    @"Summary:Mitigation Points*Mitigation Points represent the amount of damage you mitigate, 
on average, through armor mitigation and avoidance. It is directly 
relational to your Damage Taken. Ideally, you want to maximize 
Mitigation Points, while maintaining 'enough' Survival Points 
(see Survival Points). If you find yourself dying due to healers 
running OOM, or being too busy healing you and letting other 
raid members die, then focus on Mitigation Points.",
                    @"Summary:Survival Points*Survival Points represents the total raw physical damage 
(pre-mitigation) you can take before dying. Unlike 
Mitigation Points, you should not attempt to maximize this, 
but rather get 'enough' of it, and then focus on Mitigation. 
'Enough' can vary greatly by fight and by your healers, but 
keeping it roughly even with Mitigation Points is a good 
way to maintain 'enough' as you progress. If you find that 
you are being killed by burst damage, focus on Survival Points.",
                    @"Summary:Threat Points*The TPS of your Highest TPS Rotation, multiplied by
the Threat Scale defined on the Options tab.",
                    
                    "Basic Stats:Armor",
                    "Basic Stats:Agility",
                    "Basic Stats:Stamina",
                    "Basic Stats:Strength",
                    "Basic Stats:Attack Power",
                    "Basic Stats:Average Vengeance AP",
                    "Basic Stats:Crit Rating",
                    "Basic Stats:Hit Rating",
                    "Basic Stats:Expertise Rating",
                    "Basic Stats:Haste Rating",
                    //"Basic Stats:Armor Penetration Rating",
                    "Basic Stats:Dodge Rating",
                    "Basic Stats:Mastery",
                    //"Basic Stats:Defense Rating",
                    "Basic Stats:Resilience",
                    "Basic Stats:Nature Resist",
                    "Basic Stats:Fire Resist",
                    "Basic Stats:Frost Resist",
                    "Basic Stats:Shadow Resist",
                    "Basic Stats:Arcane Resist",
                    "Mitigation Stats:Dodge",
                    "Mitigation Stats:Miss",
                    "Mitigation Stats:Armor Damage Reduction",
                    "Mitigation Stats:Total Damage Reduction",
                    "Mitigation Stats:Avoidance PreDR",
                    "Mitigation Stats:Avoidance PostDR",
                    "Mitigation Stats:Savage Defense",
                    "Mitigation Stats:Total Mitigation",
                    "Mitigation Stats:Damage Taken",
                    "Survival Stats:Health",
                    "Survival Stats:Chance to be Crit",
                    "Survival Stats:Nature Survival",
                    "Survival Stats:Fire Survival",
                    "Survival Stats:Frost Survival",
                    "Survival Stats:Shadow Survival",
                    "Survival Stats:Arcane Survival",
                    "Threat Stats:Highest DPS Rotation",
                    "Threat Stats:Highest TPS Rotation",
                    //"Threat Stats:Swipe Rotation",
                    //"Threat Stats:Custom Rotation",
                    "Threat Stats:Melee",
                    "Threat Stats:Maul",
                    "Threat Stats:Mangle",
                    "Threat Stats:Lacerate",
                    "Threat Stats:Pulverize",
                    "Threat Stats:Swipe",
                    "Threat Stats:Thrash",
                    "Threat Stats:Faerie Fire",
                    "Threat Stats:Thorns",
                    "Threat Stats:Avoided Attacks",
                    };
                return _characterDisplayCalculationLabels;
            }
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
                    "Health",
                    "Avoided Attacks %",
                    "Avoided Interrupts %",
                    "Mitigation % from Armor",
                    "Avoidance %",
                    "% Chance to be Crit",
                    "Nature Survival",
                    "Fire Survival",
                    "Frost Survival",
                    "Shadow Survival",
                    "Arcane Survival",
                    "Nature Resist",
                    "Fire Resist",
                    "Frost Resist",
                    "Shadow Resist",
                    "Arcane Resist",
                    "Highest DPS",
                    "Highest TPS",
                    };
                return _optimizableCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        /// <summary>
        /// Names of the custom charts that Rawr.Bear provides
        /// </summary>
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] {
                    "Combat Table",
                    //"Relative Stat Values",
                    //"Rotation DPS",
                    //"Rotation TPS"
                    };
                return _customChartNames;
            }
        }

        private Dictionary<string, Color> _subPointNameColors = null;
        /// <summary>
        /// Names and colors for the SubPoints that Rawr.Bear uses
        /// </summary>
        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, Color>();
                    _subPointNameColors.Add("Mitigation", Colors.Red);
                    _subPointNameColors.Add("Survival", Colors.Blue);
                    _subPointNameColors.Add("Threat", Colors.Green);
                }
                return _subPointNameColors;
            }
        }

        private List<ItemType> _relevantItemTypes = null;
        /// <summary>
        /// ItemTypes that are relevant to Rawr.Bear
        /// </summary>
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<ItemType>(new ItemType[]
                    {
                        ItemType.None,
                        ItemType.Leather,
                        ItemType.Relic,
                        ItemType.Staff,
                        ItemType.TwoHandMace,
                        ItemType.Polearm
                    });
                }
                return _relevantItemTypes;
            }
        }

        /// <summary>
        /// Creates a new ComparisonCalculationBear instance
        /// </summary>
        /// <returns>A new ComparisonCalculationBear instance</returns>
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationBear(); }
        /// <summary>
        /// Creates a new CharacterCalculationsBear instance
        /// </summary>
        /// <returns>A new CharacterCalculationsBear instance</returns>
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsBear(); }

        /// <summary>
        /// Deserializes the CalculationOptionsBear object contained in xml
        /// </summary>
        /// <param name="xml">The CalculationOptionsBear object, serialized as xml</param>
        /// <returns>The deserialized CalculationOptionsBear object</returns>
        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            xml = xml.Replace("<CustomUseMaul xsi:nil=\"true\" />", "<CustomUseMaul>false</CustomUseMaul>");
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsBear));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsBear calcOpts = serializer.Deserialize(reader) as CalculationOptionsBear;
            return calcOpts;
        }
        #endregion

        #region Primary Calculation Methods
        /// <summary>
        /// Gets the results of the Character provided
        /// </summary>
        /// <param name="character">The Character to calculate results for</param>
        /// <param name="additionalItem">An additional item to grant the Character the stats of (as if it were worn)</param>
        /// <returns>The CharacterCalculationsBear containing the results of the calculations</returns>
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            #region Setup uniform variables from all models
            CharacterCalculationsBear calculatedStats = new CharacterCalculationsBear();
            if (character == null) { return calculatedStats; }
            CalculationOptionsBear calcOpts = character.CalculationOptions as CalculationOptionsBear;
            if (calcOpts == null) { return calculatedStats; }
            BossOptions bossOpts = character.BossOptions;
            // Make sure there is at least one attack in the list.
            // If there's not, add a Default Melee Attack for processing
            if (bossOpts.Attacks.Count < 1) {
                character.IsLoading = true;
                bossOpts.DamagingTargs = true;
                bossOpts.Attacks.Add(BossHandler.ADefaultMeleeAttack);
                character.IsLoading = false;
            }
            // Make sure there is a default melee attack
            // If the above processed, there will be one so this won't have to process
            // If the above didn't process and there isn't one, add one now
            if (bossOpts.DefaultMeleeAttack == null) {
                character.IsLoading = true;
                bossOpts.DamagingTargs = true;
                bossOpts.Attacks.Add(BossHandler.ADefaultMeleeAttack);
                character.IsLoading = false;
            }
            // Since the above forced there to be an attack it's safe to do this without a null check
            Attack bossAttack = bossOpts.DefaultMeleeAttack;
            StatsBear stats = GetCharacterStats(character, additionalItem) as StatsBear;
            calculatedStats.BasicStats = stats;
            calculatedStats.TargetLevel = bossOpts.Level;
            calculatedStats.CharacterLevel = character.Level;

            //calcOpts.SurvivalSoftCap = bossOpts.DefaultMeleeAttack.DamagePerHit * 3f;
            #endregion

            int levelDifference = (bossOpts.Level - character.Level);

            #region Player Stats

            #region Offensive
            float playerHasteBonus = StatConversion.GetHasteFromRating(stats.HasteRating, CharacterClass.Druid);
            float playerAttackSpeed = ((2.5f) / (1f + playerHasteBonus)) / (1f + stats.PhysicalHaste);

            float playerHitBonus = StatConversion.GetPhysicalHitFromRating(stats.HitRating, CharacterClass.Druid) + stats.PhysicalHit;
            float playerExpertiseBonus = StatConversion.GetDodgeParryReducFromExpertise(StatConversion.GetExpertiseFromRating(stats.ExpertiseRating, CharacterClass.Druid) + stats.Expertise, CharacterClass.Druid);
            float playerChanceToBeDodged = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[levelDifference] - playerExpertiseBonus);
            float playerChancetoBeParried = Math.Max(0f, StatConversion.WHITE_PARRY_CHANCE_CAP[levelDifference] - playerExpertiseBonus);
            float playerChanceToMiss = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[levelDifference] - playerHitBonus);

            float playerChanceToBeAvoided = playerChanceToMiss + playerChanceToBeDodged + playerChancetoBeParried;

            float playerRawChanceToCrit = StatConversion.GetPhysicalCritFromRating(stats.CritRating, CharacterClass.Druid)
                                + StatConversion.GetPhysicalCritFromAgility(stats.Agility, CharacterClass.Druid)
                                + stats.PhysicalCrit
                                + StatConversion.NPC_LEVEL_CRIT_MOD[levelDifference];
            float playerChanceToCrit = playerRawChanceToCrit * (1f - playerChanceToBeAvoided);
            float playerChanceToCritBleed = playerRawChanceToCrit;
            #endregion

            Stats baseStats = BaseStats.GetBaseStats(character.Level, character.Class, character.Race, BaseStats.DruidForm.Bear);

            #region Defensive Part A
            //Calculate avoidance, considering diminishing returns
            float levelDifferenceAvoidance = levelDifference * 0.002f;

            float DR_k_coefficient = StatConversion.DR_COEFFIENT[11] * 0.01f; // This is the Diminishing Returns' "k" value that changes based on what class the user is
            float DR_C_d_coefficient = StatConversion.CAP_DODGE[11]; // This is the % cap for dodge
            float DR_miss_cap = 16f;

            //float defSkill = (float)Math.Floor(StatConversion.GetDefenseFromRating(stats.DefenseRating, CharacterClass.Druid));
            float dodgeThatsNotAffectedByDR = stats.Dodge - levelDifferenceAvoidance + StatConversion.GetDodgeFromAgility(baseStats.Agility, CharacterClass.Druid);
            float missThatsNotAffectedByDR = stats.Miss - levelDifferenceAvoidance;
            float dodgeBeforeDRApplied = StatConversion.GetDodgeFromAgility(stats.Agility - baseStats.Agility, CharacterClass.Druid)
                                       + StatConversion.GetDodgeFromRating(stats.DodgeRating, CharacterClass.Druid);
            float missBeforeDRApplied = 0f;
            float dodgeAfterDRApplied = 0.01f / (1f / DR_C_d_coefficient + DR_k_coefficient / dodgeBeforeDRApplied);
            float missAfterDRApplied = 0.01f / (1f / DR_miss_cap + DR_k_coefficient / missBeforeDRApplied);
            float dodgeTotal = dodgeThatsNotAffectedByDR + dodgeAfterDRApplied;
            float missTotal = missThatsNotAffectedByDR + missAfterDRApplied;

            calculatedStats.Miss = missTotal;
            calculatedStats.Dodge = Math.Min(1f - calculatedStats.Miss, dodgeTotal);
            calculatedStats.DamageReductionFromArmor = StatConversion.GetDamageReductionFromArmor(bossOpts.Level, stats.Armor);
            calculatedStats.TotalConstantDamageReduction = 1f - (1f - calculatedStats.DamageReductionFromArmor) * (1f - stats.DamageTakenReductionMultiplier) * (1f - stats.BossPhysicalDamageDealtReductionMultiplier);
            calculatedStats.AvoidancePreDR = dodgeThatsNotAffectedByDR + dodgeBeforeDRApplied + missThatsNotAffectedByDR + missBeforeDRApplied;
            calculatedStats.AvoidancePostDR = dodgeTotal + missTotal;
            calculatedStats.CritReduction = stats.CritChanceReduction;
            calculatedStats.CappedCritReduction = Math.Min(0.05f + levelDifferenceAvoidance, calculatedStats.CritReduction);
            #endregion

            #region Vengeance
            {
                // == Evaluate damage taken once ahead of time for vengeance ==
                //Out of 100 attacks, you'll take...
                float critsVeng = Math.Min(Math.Max(0f, 1f - calculatedStats.AvoidancePostDR), (0.05f + levelDifferenceAvoidance) - calculatedStats.CappedCritReduction);
                //float crushes = targetLevel == 73 ? Math.Max(0f, Math.Min(15f, 100f - (crits + calculatedStats.AvoidancePreDR)) - stats.CritChanceReduction) : 0f;
                float hitsVeng = Math.Max(0f, 1f - (critsVeng + calculatedStats.AvoidancePostDR));
                //Apply armor and multipliers for each attack type...
                critsVeng *= (1f - calculatedStats.TotalConstantDamageReduction) * 2f;
                //crushes *= (100f - calculatedStats.Mitigation) * .015f;
                hitsVeng *= (1f - calculatedStats.TotalConstantDamageReduction);
                float damageTakenPercent = (hitsVeng + critsVeng) * (1f - stats.BossAttackSpeedReductionMultiplier);
                float damageTakenPerHit = bossAttack.DamagePerHit * damageTakenPercent;
                float damageTakenPerSecond = damageTakenPerHit / bossAttack.AttackSpeed;
                float damageTakenPerVengeanceTick = damageTakenPerSecond * 2f;
                float vengeanceCap = stats.Stamina + baseStats.Health * 0.1f;
                float vengeanceAPPreAvoidance = Math.Min(vengeanceCap, damageTakenPerVengeanceTick) ;

                double chanceHit = 1f - calculatedStats.AvoidancePostDR;
                double vengeanceMultiplierFromAvoidance = //Best-fit of results from simulation of avoidance effects on vengeance
                    -46.288470839554d * Math.Pow(chanceHit, 6) 
                    + 143.12528411194400d * Math.Pow(chanceHit, 5) 
                    - 159.9833254324610000d * Math.Pow(chanceHit, 4)
                    + 74.0451030489808d * Math.Pow(chanceHit, 3) 
                    - 10.8422088672455d * Math.Pow(chanceHit, 2) 
                    + 0.935157126508557d * chanceHit;

                float vengeanceMultiplierFromSwingSpeed = bossAttack.AttackSpeed <= 2f ? 1f :
                    (1f - 0.1f * (1f - 2f / bossAttack.AttackSpeed)); //A percentage of the ticks will be guaranteed decays for attack speeds longer than 2sec, due to no swings occuring between the current and last tick

                float vengeanceAP = (float)(vengeanceAPPreAvoidance * vengeanceMultiplierFromAvoidance * vengeanceMultiplierFromSwingSpeed);

                stats.AttackPower += vengeanceAP * (1f + stats.BonusAttackPowerMultiplier);
                calculatedStats.AverageVengeanceAP = vengeanceAP;
            }
            #endregion

            #region Defensive Part B
            float targetAttackSpeedDebuffed = bossAttack.AttackSpeed / (1f - stats.BossAttackSpeedReductionMultiplier);
            float targetHitChance = 1f - calculatedStats.AvoidancePostDR;
            float autoSpecialAttacksPerSecond = 1f / 1.5f + 1f / playerAttackSpeed + 1f / 3f /*+ (stats.Tier_13_2_piece ? (1f / calculatedStats.Abilities.MangleStats.Cooldown) : 0)*/;

            float masteryMultiplier = 1f + (8f + StatConversion.GetMasteryFromRating(stats.MasteryRating)) * 0.04f;
            float totalAttacksPerSecond = autoSpecialAttacksPerSecond;
            float averageSDAttackCritChance = 0.5f * (playerChanceToCrit * (autoSpecialAttacksPerSecond / totalAttacksPerSecond)); //Include the 50% chance to proc per crit here.
            //float T13_2PSDAttackCritChance = (playerChanceToCrit * (autoSpecialAttacksPerSecond / totalAttacksPerSecond));
            //averageSDAttackCritChance = (1 + averageSDAttackCritChance) * (1 + (stats.Tier_13_2_piece ? T13_2PSDAttackCritChance : 0)) - 1f;
            float playerAttacksInterval = 1f / totalAttacksPerSecond;
            float blockChance = 1f - targetHitChance * ((float)Math.Pow(1f - averageSDAttackCritChance, targetAttackSpeedDebuffed / playerAttacksInterval)) *
                1f / (1f - (1f - targetHitChance) * (float)Math.Pow(1f - averageSDAttackCritChance, targetAttackSpeedDebuffed / playerAttacksInterval));
            float blockValue = stats.AttackPower * 0.35f * masteryMultiplier;
            float blockedPercent = Math.Min(1f, (blockValue * blockChance) / ((1f - calculatedStats.TotalConstantDamageReduction) * bossAttack.DamagePerHit));
            calculatedStats.SavageDefenseChance = (float)Math.Round(blockChance, 5);
            calculatedStats.SavageDefenseValue = (float)Math.Floor(blockValue);
            calculatedStats.SavageDefensePercent = (float)Math.Round(blockedPercent, 5);
            #endregion

            #endregion

            #region Mitigation Points
            // Out of 100 attacks, you'll take...
            float crits = Math.Min(Math.Max(0f, 1f - calculatedStats.AvoidancePostDR), (0.05f + levelDifferenceAvoidance) - calculatedStats.CappedCritReduction);
            float hits = Math.Max(0f, 1f - (crits + calculatedStats.AvoidancePostDR));
            // Apply armor and multipliers for each attack type...
            crits *= (1f - calculatedStats.TotalConstantDamageReduction) * 2f;
            hits  *= (1f - calculatedStats.TotalConstantDamageReduction);
            calculatedStats.DamageTaken = (hits + crits) * (1f - blockedPercent) * (1f - stats.BossAttackSpeedReductionMultiplier);
            calculatedStats.TotalMitigation = 1f - calculatedStats.DamageTaken;
            // Final Mitigation Score
            calculatedStats.MitigationPoints = (StatConversion.MitigationScaler / calculatedStats.DamageTaken);
            #endregion

            #region Survivability Points
            float healingincoming = (stats.Health / 2.5f) * (1f + stats.HealingReceivedMultiplier);
            float healthRestoreFromMaxHealth = stats.Health * stats.HealthRestoreFromMaxHealth;
            float healthrestore = stats.Healed + stats.HealthRestore + stats.BonusHealingReceived + ((healingincoming > healthRestoreFromMaxHealth) ? (healingincoming - healthRestoreFromMaxHealth) : healthRestoreFromMaxHealth);
            
            calculatedStats.SurvivalPointsRaw = ((stats.Health + stats.DamageAbsorbed + healthrestore) / (1f - calculatedStats.TotalConstantDamageReduction));
            double survivalCap = /*bossAttack.DamagePerHit * calcOpts.HitsToSurvive*/ calcOpts.SurvivalSoftCap / 1000d;
            double survivalRaw = calculatedStats.SurvivalPointsRaw / 1000d;

            //Implement Survival Soft Cap
            if (survivalRaw <= survivalCap) {
                calculatedStats.SurvivabilityPoints = 1000f * (float)survivalRaw;
            } else {
                double x = survivalRaw;
                double cap = survivalCap;
                double fourToTheNegativeFourThirds = Math.Pow(4d, -4d / 3d);
                double topLeft = Math.Pow(((x - cap) / cap) + fourToTheNegativeFourThirds, 1d / 4d);
                double topRight = Math.Pow(fourToTheNegativeFourThirds, 1d / 4d);
                double fracTop = topLeft - topRight;
                double fraction = fracTop / 2d;
                double y = (cap * fraction + cap);
                calculatedStats.SurvivabilityPoints = 1000f * (float)y;
            }
            #endregion

            #region Survivability for each Resistance Type (magical attacks)
            // Call new resistance formula and apply talent damage reduction
            // As for other survival, only use guaranteed reduction (MinimumResist), no luck
            float naturalReactionMOD = (1f - 0.09f * character.DruidTalents.NaturalReaction);
            calculatedStats.NatureSurvivalPoints = (float)(stats.Health / ((1f - StatConversion.GetMinimumResistance(bossOpts.Level, character.Level, stats.NatureResistance, 0)) * naturalReactionMOD));
            calculatedStats.FrostSurvivalPoints  = (float)(stats.Health / ((1f - StatConversion.GetMinimumResistance(bossOpts.Level, character.Level, stats.FrostResistance,  0)) * naturalReactionMOD));
            calculatedStats.FireSurvivalPoints   = (float)(stats.Health / ((1f - StatConversion.GetMinimumResistance(bossOpts.Level, character.Level, stats.FireResistance,   0)) * naturalReactionMOD));
            calculatedStats.ShadowSurvivalPoints = (float)(stats.Health / ((1f - StatConversion.GetMinimumResistance(bossOpts.Level, character.Level, stats.ShadowResistance, 0)) * naturalReactionMOD));
            calculatedStats.ArcaneSurvivalPoints = (float)(stats.Health / ((1f - StatConversion.GetMinimumResistance(bossOpts.Level, character.Level, stats.ArcaneResistance, 0)) * naturalReactionMOD));
            #endregion

            //Perform Threat calculations
            CalculateThreat(stats, bossOpts.Level, calculatedStats, character);

            calculatedStats.OverallPoints = calculatedStats.MitigationPoints + calculatedStats.SurvivabilityPoints + calculatedStats.ThreatPoints;

            return calculatedStats;
        }

        /// <summary>
        /// Calculates the threat properties of the Character
        /// </summary>
        /// <param name="stats">The total Stats of the character</param>
        /// <param name="targetLevel">The level of the target</param>
        /// <param name="calculatedStats">The CharacterCalculationsBear object to fill with results</param>
        /// <param name="character">The Character to calculate the threat properties of</param>
        private void CalculateThreat(StatsBear stats, int targetLevel, CharacterCalculationsBear calculatedStats, Character character)
        {
            CalculationOptionsBear calcOpts = character.CalculationOptions as CalculationOptionsBear ?? new CalculationOptionsBear();
            BossOptions bossOpts = character.BossOptions;
            if (bossOpts.Attacks.Count < 1) { bossOpts.Attacks.Add(BossHandler.ADefaultMeleeAttack); bossOpts.Attacks[0].IsTheDefaultMelee = true; bossOpts.DamagingTargs = true; }
            if (bossOpts.DefaultMeleeAttack == null) { bossOpts.Attacks.Add(BossHandler.ADefaultMeleeAttack); bossOpts.Attacks[bossOpts.Attacks.Count - 1].IsTheDefaultMelee = true; bossOpts.DamagingTargs = true; }
            Attack bossAttack = bossOpts.DefaultMeleeAttack;
            DruidTalents talents = character.DruidTalents;

            // Establish base multipliers and chances
            float modArmor = 1f - StatConversion.GetArmorDamageReduction(character.Level, bossOpts.Armor,
                stats.TargetArmorReduction, stats.ArmorPenetration);

            float critMultiplier = 2f * (1 + stats.BonusCritDamageMultiplier);
            float spellCritMultiplier = 2f * (1 + stats.BonusCritDamageMultiplier);

            float hasteBonus = StatConversion.GetPhysicalHasteFromRating(stats.HasteRating, CharacterClass.Druid);
            float attackSpeed = (2.5f) / (1f + hasteBonus);
            attackSpeed = attackSpeed / (1f + stats.PhysicalHaste);

            float hitBonus = StatConversion.GetPhysicalHitFromRating(stats.HitRating, CharacterClass.Druid) + stats.PhysicalHit;
            float expertiseBonus = StatConversion.GetDodgeParryReducFromExpertise(StatConversion.GetExpertiseFromRating(stats.ExpertiseRating, CharacterClass.Druid) + stats.Expertise, CharacterClass.Druid);
            float spellHitBonus = StatConversion.GetSpellHitFromRating(stats.HitRating, CharacterClass.Druid) + stats.SpellHit;
            
            int levelDifference = (targetLevel - character.Level);
            float chanceDodge = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[levelDifference] - expertiseBonus);
            float chanceParry = Math.Max(0f, StatConversion.WHITE_PARRY_CHANCE_CAP[levelDifference] - expertiseBonus);
            float chanceMiss = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[levelDifference] - hitBonus);
            float chanceResist = Math.Max(0f, StatConversion.GetSpellMiss(-levelDifference, false) - spellHitBonus);
            
            float chanceGlance = StatConversion.WHITE_GLANCE_CHANCE_CAP[levelDifference]; //0.2335774f;
            //float glanceMultiplier = 0.7f;
            float chanceAvoided = chanceMiss + chanceDodge + chanceParry;
            
            float rawChanceCrit = StatConversion.GetPhysicalCritFromRating(stats.CritRating, CharacterClass.Druid)
                                + StatConversion.GetPhysicalCritFromAgility(stats.Agility, CharacterClass.Druid)
                                + stats.PhysicalCrit
                                + StatConversion.NPC_LEVEL_CRIT_MOD[levelDifference];
            float chanceCrit = rawChanceCrit * (1f - chanceAvoided);
            
            float rawChanceCritSpell = StatConversion.GetSpellCritFromRating(stats.CritRating, CharacterClass.Druid)
                                + StatConversion.GetSpellCritFromIntellect(stats.Intellect, CharacterClass.Druid)
                                + stats.SpellCrit + stats.SpellCritOnTarget
                                + StatConversion.NPC_LEVEL_CRIT_MOD[levelDifference];
            float chanceCritSpell = rawChanceCritSpell * (1f - chanceResist);

            calculatedStats.DodgedAttacks = chanceDodge;
            calculatedStats.ParriedAttacks = chanceParry;
            calculatedStats.MissedAttacks = chanceMiss;

            float movementdowntime = 3f / 5.5f / (1 + stats.MovementSpeed); // Movement Duration / Movement Frequency / (1 + Movement Speed)
            
            BearAbilityBuilder abilities = new BearAbilityBuilder(stats,
                character.MainHand == null ? 0.75f : ((character.MainHand.MinDamage + character.MainHand.MaxDamage) / 2f) / character.MainHand.Speed,
                attackSpeed, modArmor, chanceAvoided, chanceResist, chanceCrit, chanceCritSpell, chanceGlance, critMultiplier, spellCritMultiplier);
            var optimalRotations = BearRotationCalculator.GetOptimalRotations(abilities);
            calculatedStats.Abilities = abilities;
            calculatedStats.HighestDPSRotation = optimalRotations.Item1;
            float bonusdamage = stats.ArcaneDamage + stats.FireDamage + stats.FrostDamage + stats.NatureDamage + stats.ShadowDamage + stats.HolyDamage + stats.PhysicalDamage;
            calculatedStats.HighestDPSRotation.DPS += bonusdamage;
            calculatedStats.HighestDPSRotation.DPS *= 1 - movementdowntime;
            calculatedStats.HighestTPSRotation = optimalRotations.Item2;
            calculatedStats.HighestTPSRotation.TPS += bonusdamage * 5f;
            calculatedStats.HighestTPSRotation.TPS *= 1 - movementdowntime;
            calculatedStats.ThreatPoints = calculatedStats.HighestTPSRotation.TPS * calcOpts.ThreatScale / 10f;
        }

        private static readonly SpecialEffect SpecialEffect4T12 = new SpecialEffect(Trigger.Use, new Stats() { Dodge = 0.10f, }, 12f, 60f, 1f);
        private static readonly SpecialEffect LeaderOfThePackSpecialEffect = new SpecialEffect(Trigger.PhysicalCrit, new Stats() { HealthRestoreFromMaxHealth = 0.04f, }, 0f, 6f, 1f);
        /// <summary>
        /// Gets the total Stats of the Character
        /// </summary>
        /// <param name="character">The Character to get the total Stats of</param>
        /// <param name="additionalItem">An additional item to grant the Character the stats of (as if it were worn)</param>
        /// <returns>The total stats for the Character</returns>
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsBear calcOpts = character.CalculationOptions as CalculationOptionsBear ?? new CalculationOptionsBear();

            DruidTalents talents = character.DruidTalents;

            bool hasCritBuff = false;
            foreach (Buff buff in character.ActiveBuffs) {
                if (buff.Group == "Critical Strike Chance") {
                    hasCritBuff = true;
                    break;
                }
            }

            StatsBear statsTotal = new StatsBear()
            {
                BonusAttackPowerMultiplier = 0.25f,
                BonusBleedDamageMultiplier = (character.ActiveBuffsContains("Mangle") || character.ActiveBuffsContains("Trauma") ? 0f : 0.3f),

                Dodge = 0.02f * talents.FeralSwiftness + 0.03f * talents.NaturalReaction,
                FurySwipesChance = 0.05f * talents.FurySwipes,
                BonusEnrageDamageMultiplier = 0.05f * talents.KingOfTheJungle,
                HasteOnFeralCharge = 0.15f * talents.Stampede,
                BaseArmorMultiplier = 2.2f * (1f + 0.10f * talents.ThickHide / 3f) * (1f + 0.26f * talents.ThickHide) - 1f,
                CritChanceReduction = 0.02f * talents.ThickHide,
                PhysicalCrit = (hasCritBuff ? 0f : 0.05f * talents.LeaderOfThePack) + (talents.Pulverize > 0 ? 0.09f: 0f),
                SpellCrit = (hasCritBuff ? 0f : 0.05f * talents.LeaderOfThePack),
                BonusPulverizeDuration = 4f * talents.EndlessCarnage,
                DamageTakenReductionMultiplier = 0.09f * talents.NaturalReaction,
                BonusMaulDamageMultiplier = 0.04f * talents.RendAndTear,
                
                BonusStaminaMultiplier = (1f + 0.02f * talents.HeartOfTheWild) * (Character.ValidateArmorSpecialization(character, ItemType.Leather) ? 1.05f : 1f) - 1f,
                BonusPhysicalDamageMultiplier = 0.04f * talents.MasterShapeshifter,
                BonusMangleDamageMultiplier = talents.GlyphOfMangle ? 0.1f : 0f,
                BonusLacerateCritChance = talents.GlyphOfLacerate ? 0.05f : 0f,
                BonusFaerieFireStacks = talents.FeralAggression,
                BerserkDuration = (talents.GlyphOfBerserk ? 10f : 0f),
            };

            #region Set Bonuses
            int PvPCount;
            character.SetBonusCount.TryGetValue("Gladiator's Sanctuary", out PvPCount);
            if (PvPCount >= 2)
            {
                statsTotal.Agility += 70f;
                statsTotal.Resilience += 400f;
            }
            if (PvPCount >= 4)
            {
                // the 15% movement speed is only outdoors which most dungeons are not
                statsTotal.Agility += 90f;
            }
            int T11Count;
            character.SetBonusCount.TryGetValue("Stormrider's Battlegarb", out T11Count);
            if (T11Count >= 2) {
                //statsTotal.BonusDamageMultiplierRakeTick = (1f + statsTotal.BonusDamageMultiplierRakeTick) * (1f + 0.10f) - 1f;
                statsTotal.BonusDamageMultiplierLacerate = (1f + statsTotal.BonusDamageMultiplierLacerate) * (1f + 0.10f) - 1f;
            }
            if (T11Count >= 4)
            {
                /*statsBuffs.AddSpecialEffect(new SpecialEffect(Trigger.MangleCatHit,
                    new Stats() { BonusAttackPowerMultiplier = 0.01f, },
                    30, 0, 1f, 3));*/
                statsTotal.BonusSurvivalInstinctsDurationMultiplier = 0.5f;
            }
            int T12Count;
            character.SetBonusCount.TryGetValue("Obsidian Arborweave Battlegarb", out T12Count);
            if (T12Count >= 2)
            {
                statsTotal.BonusMangleDamageMultiplier = (1f + statsTotal.BonusMangleDamageMultiplier) * (1f + 0.10f) - 1f;
                statsTotal.BonusMaulDamageMultiplier = (1f + statsTotal.BonusMaulDamageMultiplier) * (1f + 0.10f) - 1f;
            }
            if (T12Count >= 4)
            {
                statsTotal.AddSpecialEffect(SpecialEffect4T12);
            }
            
            int T13Count;
            character.SetBonusCount.TryGetValue("Deep Earth Battlegarb", out T13Count);
            if (T13Count >= 2)
            {
                statsTotal.Tier_13_2_piece = true;
            }
            if (T13Count >= 4)
            {
                statsTotal.Tier_13_4_piece = (10f + 25f)/2;
            }
            #endregion

            // Leader of the Pack self-heal
            statsTotal.AddSpecialEffect(LeaderOfThePackSpecialEffect);

            // Survival Instincts
            SpecialEffect SurvivalInstinctsSpecialEffect = new SpecialEffect(Trigger.Use, new Stats() { DamageTakenReductionMultiplier = 0.50f, }, 12f * (1f + statsTotal.BonusSurvivalInstinctsDurationMultiplier), 180f, 1f);
            statsTotal.AddSpecialEffect(SurvivalInstinctsSpecialEffect);

            // Barkskin
            SpecialEffect BarkskinSpecialEffect = new SpecialEffect(Trigger.Use, new Stats() { DamageTakenReductionMultiplier = 0.20f, CritChanceReduction = (talents.GlyphOfBarkskin ? 0.25f : 0f), }, 12f, 60f, 1f);
            statsTotal.AddSpecialEffect(BarkskinSpecialEffect);

            // Frenzied Regeneration
            SpecialEffect FrenziedRegenerationSpecialEffect = new SpecialEffect(Trigger.Use, new Stats() { BonusHealthMultiplier = 0.15f, HealthRestoreFromMaxHealth = (talents.GlyphOfFrenziedRegeneration ? 0f : (0.015f * (1f + statsTotal.Tier_13_4_piece))), HealingReceivedMultiplier = (talents.GlyphOfFrenziedRegeneration ? (0.30f * (1f + statsTotal.Tier_13_4_piece)) : 0f) }, 20f, 180f, 1f);
            statsTotal.AddSpecialEffect(FrenziedRegenerationSpecialEffect);

            // Berserk
            StatsBear tempBear = new StatsBear();
            tempBear.AddSpecialEffect(new SpecialEffect(Trigger.LacerateTick, new StatsBear() { MangleCooldownReduction = 6f, MangleCostReduction = 1f }, float.PositiveInfinity, 0, 0.5f));
            SpecialEffect BerserkSpecialEffect = new SpecialEffect(Trigger.Use, tempBear, 15f + statsTotal.BerserkDuration, 180f, 1f);
            statsTotal.AddSpecialEffect(BerserkSpecialEffect);

            // Enrage
            SpecialEffect EnrageSpecialEffect = new SpecialEffect(Trigger.Use, new StatsBear() { BonusDamageMultiplier = (0.05f * talents.KingOfTheJungle) }, 10f, 60f, 1f);
            statsTotal.AddSpecialEffect(EnrageSpecialEffect);

            statsTotal.Accumulate(BaseStats.GetBaseStats(character.Level, character.Class, character.Race, BaseStats.DruidForm.Bear));
            statsTotal.Accumulate(GetItemStats(character, additionalItem));
            statsTotal.Accumulate(GetBuffsStats(character, calcOpts));

            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
            statsTotal.Strength = (float)Math.Floor(statsTotal.Strength * (1f + statsTotal.BonusStrengthMultiplier));
            statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1f + statsTotal.BonusAgilityMultiplier));
            statsTotal.AttackPower += (float)Math.Floor(statsTotal.Strength);
            statsTotal.AttackPower += (float)Math.Floor(statsTotal.Agility - 20f) * 2f + 20f;
            statsTotal.AttackPower = (float)Math.Floor(statsTotal.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
            statsTotal.Health += ((statsTotal.Stamina - 20f) * 14f) + 20f;
            statsTotal.Health *= (1f + statsTotal.BonusHealthMultiplier);
            statsTotal.Armor *= 1f + statsTotal.BaseArmorMultiplier;
            statsTotal.Armor += statsTotal.BonusArmor;
            statsTotal.Armor = (float)Math.Floor(statsTotal.Armor * (1f + statsTotal.BonusArmorMultiplier));
            statsTotal.NatureResistance += statsTotal.NatureResistanceBuff;
            statsTotal.FireResistance += statsTotal.FireResistanceBuff;
            statsTotal.FrostResistance += statsTotal.FrostResistanceBuff;
            statsTotal.ShadowResistance += statsTotal.ShadowResistanceBuff;
            statsTotal.ArcaneResistance += statsTotal.ArcaneResistanceBuff;

            AccumulateProcs(character, statsTotal);

            return statsTotal;
        }

        private static void AccumulateProcs(Character character, StatsBear statsTotal)
        {
            CalculationOptionsBear calcOpts = character.CalculationOptions as CalculationOptionsBear ?? new CalculationOptionsBear();
            BossOptions bossOpts = character.BossOptions;
            if (bossOpts.Attacks.Count < 1) { bossOpts.Attacks.Add(BossHandler.ADefaultMeleeAttack); bossOpts.Attacks[0].IsTheDefaultMelee = true; bossOpts.DamagingTargs = true; }
            if (bossOpts.DefaultMeleeAttack == null) { bossOpts.Attacks.Add(BossHandler.ADefaultMeleeAttack); bossOpts.Attacks[bossOpts.Attacks.Count-1].IsTheDefaultMelee = true; bossOpts.DamagingTargs = true; }
            Attack bossAttack = bossOpts.DefaultMeleeAttack;
            int targetLevel = bossOpts.Level;
            float fightDuration = bossOpts.BerserkTimer;

            float hasteBonus = StatConversion.GetHasteFromRating(statsTotal.HasteRating, CharacterClass.Druid);
            float playerHastedAttackSpeed = (2.5f / (1f + hasteBonus)) / (1f + statsTotal.PhysicalHaste);
            float meleeHitInterval = 1f / (1f / playerHastedAttackSpeed + 1f / 1.5f);

            int levelDifference = (bossOpts.Level - character.Level);
            float hitBonus = StatConversion.GetPhysicalHitFromRating(statsTotal.HitRating) + statsTotal.PhysicalHit;
            float expertiseBonus = StatConversion.GetDodgeParryReducFromExpertise(StatConversion.GetExpertiseFromRating(statsTotal.ExpertiseRating, CharacterClass.Druid) + statsTotal.Expertise, CharacterClass.Druid);
            float chanceDodge = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[levelDifference] - expertiseBonus);
            float chanceParry = Math.Max(0f, StatConversion.WHITE_PARRY_CHANCE_CAP[levelDifference] - expertiseBonus);
            float chanceMiss = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[levelDifference] - hitBonus);
            float chanceAvoided = chanceMiss + chanceDodge + chanceParry;

            float rawChanceCrit = StatConversion.GetPhysicalCritFromRating(statsTotal.CritRating, CharacterClass.Druid)
                                + StatConversion.GetPhysicalCritFromAgility(statsTotal.Agility, CharacterClass.Druid)
                                + statsTotal.PhysicalCrit
                                + StatConversion.NPC_LEVEL_CRIT_MOD[levelDifference];
            float chanceCrit = rawChanceCrit * (1f - chanceAvoided);
            float chanceHit = 1f - chanceAvoided;

            float levelDifferenceAvoidance = levelDifference * 0.002f;
            float baseAgi = character.Race == CharacterRace.NightElf ? 87 : 77;

            //float defSkill = (float)Math.Floor(StatConversion.GetDefenseFromRating(statsTotal.DefenseRating, CharacterClass.Druid));
            float dodgeNonDR = statsTotal.Dodge - levelDifferenceAvoidance + StatConversion.GetDodgeFromAgility(baseAgi, CharacterClass.Druid);
            float missNonDR = statsTotal.Miss - levelDifferenceAvoidance;
            float dodgePreDR = StatConversion.GetDodgeFromAgility(statsTotal.Agility - baseAgi, CharacterClass.Druid)
                             + StatConversion.GetDodgeFromRating(statsTotal.DodgeRating, CharacterClass.Druid)
                             /*+ defSkill * StatConversion.DEFENSE_RATING_AVOIDANCE_MULTIPLIER / 100f*/;
            float missPreDR = 0f;// (defSkill * StatConversion.DEFENSE_RATING_AVOIDANCE_MULTIPLIER / 100f);
            float dodgePostDR = 0.01f / (1f / 116.890707f + 0.00972f / dodgePreDR);
            float missPostDR = 0.01f / (1f / 16f + 0.00972f / missPreDR);
            float dodgeTotal = dodgeNonDR + dodgePostDR;
            float missTotal = missNonDR + missPostDR;

            float TargAttackSpeed = bossAttack.AttackSpeed / (1f - statsTotal.BossAttackSpeedReductionMultiplier);

            Stats statsProcs = new Stats();
            //float uptime;
            foreach (SpecialEffect effect in statsTotal.SpecialEffects())
            {
                switch (effect.Trigger)
                {
                    case Trigger.Use:
                        effect.AccumulateAverageStats(statsProcs, 0f, 1f, 2.5f, fightDuration);
                        break;
                    case Trigger.MeleeHit:
                    case Trigger.PhysicalHit:
                        effect.AccumulateAverageStats(statsProcs, meleeHitInterval, chanceHit, 2.5f, fightDuration);
                        break;
                    case Trigger.MeleeAttack:
                    case Trigger.PhysicalAttack:
                        if (effect.Stats.MoteOfAnger > 0) {
                            // When in effect stats, MoteOfAnger is % of melee hits
                            // When in character stats, MoteOfAnger is average procs per second
                            statsProcs.MoteOfAnger = effect.Stats.MoteOfAnger * effect.GetAverageProcsPerSecond(meleeHitInterval, 1f, 2.5f, fightDuration) / effect.MaxStack;
                        } else {
                            effect.AccumulateAverageStats(statsProcs, meleeHitInterval, 1f, 2.5f, fightDuration);
                        }
                        break;
                    case Trigger.MeleeCrit:
                    case Trigger.PhysicalCrit:
                        effect.AccumulateAverageStats(statsProcs, meleeHitInterval, chanceCrit, 2.5f, fightDuration);
                        break;
                    case Trigger.DoTTick:
                        effect.AccumulateAverageStats(statsProcs, 3f, 1f, 2.5f, fightDuration);
                        break;
                    case Trigger.DamageDone:
                        effect.AccumulateAverageStats(statsProcs, meleeHitInterval / 2f, 1f, 2.5f, fightDuration);
                        break;
                    case Trigger.DamageOrHealingDone:
                        effect.AccumulateAverageStats(statsProcs, meleeHitInterval / 2f, 1f, 2.5f, fightDuration); // also needs healing
                        break;
                    case Trigger.MangleBearHit:
                        effect.AccumulateAverageStats(statsProcs, 6f - statsTotal.MangleCooldownReduction, chanceHit, 2.5f, fightDuration);
                        break;
                    case Trigger.MangleCatOrShredOrInfectedWoundsHit:
                        effect.AccumulateAverageStats(statsProcs, 1f /
                            (1f / (6f - statsTotal.MangleCooldownReduction) + //Mangles Per Second
                            1f / meleeHitInterval) //Mauls Per Second
                            , chanceHit, 2.5f, fightDuration);
                        break;
                    case Trigger.SwipeBearOrLacerateHit:
                        effect.AccumulateAverageStats(statsProcs, 2.25f, chanceHit, 2.5f, fightDuration);
                        break;
                    case Trigger.LacerateTick:
                        effect.AccumulateAverageStats(statsProcs, 3f, 1f, 2.5f, fightDuration);
                        break;
                    case Trigger.DamageTakenPutsMeBelow35PercHealth:
                        effect.AccumulateAverageStats(statsProcs, TargAttackSpeed * 0.8f, (1f - 0.8f * (dodgeTotal + missTotal)) * 0.35f, fightDuration); //Assume you get hit by other things, like dots, aoes, etc, making you get targeted with damage 35% more often than the boss, and half the hits you take are unavoidable.
                        break;
                    case Trigger.DamageTakenPutsMeBelow50PercHealth:
                        effect.AccumulateAverageStats(statsProcs, TargAttackSpeed * 0.8f, (1f - 0.8f * (dodgeTotal + missTotal)) * 0.50f, fightDuration); //Assume you get hit by other things, like dots, aoes, etc, making you get targeted with damage 50% more often than the boss, and half the hits you take are unavoidable.
                        break;
                    case Trigger.DamageTaken:
                        effect.AccumulateAverageStats(statsProcs, TargAttackSpeed * 0.8f, 1f - 0.8f * (dodgeTotal + missTotal), fightDuration); //Assume you get hit by other things, like dots, aoes, etc, making you get targeted with damage 25% more often than the boss, and half the hits you take are unavoidable.
                        break;
                    case Trigger.DamageTakenPhysical:
                        effect.AccumulateAverageStats(statsProcs, TargAttackSpeed, 1f - (dodgeTotal + missTotal), fightDuration); //Assume you get hit by other things, like dots, aoes, etc, making you get targeted with damage 25% more often than the boss, and half the hits you take are unavoidable.
                        break;
                    case Trigger.Barkskin:
                        effect.AccumulateAverageStats(statsProcs, 60f, 1f, 0f, fightDuration);
                        break;
                    case Trigger.Berserk:
                        effect.AccumulateAverageStats(statsProcs, 180f, 1, 0f, fightDuration);
                        break;
                }
            }

            statsProcs.Agility += statsProcs.HighestStat + statsProcs.Paragon;
            statsProcs.Stamina = (float)Math.Floor(statsProcs.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
            statsProcs.Strength = (float)Math.Floor(statsProcs.Strength * (1f + statsTotal.BonusStrengthMultiplier));
            statsProcs.Agility = (float)Math.Floor(statsProcs.Agility * (1f + statsTotal.BonusAgilityMultiplier));
            statsProcs.AttackPower += statsProcs.Strength + statsProcs.Agility * 2f;
            statsProcs.AttackPower = (float)Math.Floor(statsProcs.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
            statsProcs.Health += (float)Math.Floor(statsProcs.Stamina * 14f) + (float)Math.Floor(statsProcs.BattlemasterHealthProc);
            statsProcs.Health *= (1f + statsProcs.BonusHealthMultiplier);
            statsProcs.Armor += /*2f * (float)Math.Floor(statsProcs.Agility)*/ + statsProcs.BonusArmor; // Armor no longer gets bonuses from Agi in Cata
            statsProcs.Armor = (float)Math.Floor(statsProcs.Armor * (1f + statsTotal.BonusArmorMultiplier));
            if (statsProcs.HighestSecondaryStat > 0) {
                if (statsTotal.CritRating > statsTotal.HasteRating && statsTotal.CritRating > statsTotal.MasteryRating) {
                    statsProcs.CritRating += statsProcs.HighestSecondaryStat; // this will be invalidated after this, but I'm at least putting it in for now
                }
                else if (statsTotal.HasteRating > statsTotal.CritRating && statsTotal.HasteRating > statsTotal.MasteryRating) {
                    statsProcs.HasteRating += statsProcs.HighestSecondaryStat;
                }
                else if (statsTotal.MasteryRating > statsTotal.CritRating && statsTotal.MasteryRating > statsTotal.HasteRating) {
                    statsProcs.MasteryRating += statsProcs.HighestSecondaryStat;
                }
                statsProcs.HighestSecondaryStat = 0;
            }

            statsTotal.Accumulate(statsProcs);
        }

        //NOTE: This is currently unused, because it doesn't account for procs which are both mitigation and survival (ie armor and agility procs)
        private void AccumulateSpecialEffect(Stats statsProcs, Stats statsEffect, float effectUptime, float temporarySurvivalScale)
        {
            if (temporarySurvivalScale != 1f && statsEffect.Armor + statsEffect.Health + statsEffect.Stamina > 0f)
            {
                //Subject to Temporary Survival scaling
                if (temporarySurvivalScale < 1f)
                {
                    statsProcs.Accumulate(statsEffect, effectUptime * temporarySurvivalScale);
                }
                else
                {
                    statsProcs.Accumulate(statsEffect, 1f - ((2f - temporarySurvivalScale) * (1f - effectUptime)));
                }
            }
            else
            {
                statsProcs.Accumulate(statsEffect, effectUptime);
            }
        }

        /// <summary>
        /// Gets data for a custom chart that Rawr.Bear provides
        /// </summary>
        /// <param name="character">The Character to get the chart data for</param>
        /// <param name="chartName">The name of the custom chart to get data for</param>
        /// <returns>The data for the custom chart</returns>
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            switch (chartName)
            {
                case "Combat Table":
                    CharacterCalculationsBear currentCalculationsBear = GetCharacterCalculations(character) as CharacterCalculationsBear;
                    ComparisonCalculationBear calcMiss = new ComparisonCalculationBear();
                    ComparisonCalculationBear calcDodge = new ComparisonCalculationBear();
                    ComparisonCalculationBear calcCrit = new ComparisonCalculationBear();
                    //ComparisonCalculationBear calcCrush = new ComparisonCalculationBear();
                    ComparisonCalculationBear calcHit = new ComparisonCalculationBear();
                    if (currentCalculationsBear != null)
                    {
                        calcMiss.Name = "    Miss    ";
                        calcDodge.Name = "   Dodge   ";
                        calcCrit.Name = "  Crit  ";
                        //calcCrush.Name = " Crush ";
                        calcHit.Name = "Hit";

                        float crits = 0.02f + (0.002f * (currentCalculationsBear.TargetLevel - character.Level)) - currentCalculationsBear.CappedCritReduction;
                        //float crushes = currentCalculationsBear.TargetLevel == 73 ? Math.Max(Math.Min(100f - (crits + (currentCalculationsBear.AvoidancePreDR)), 15f) - currentCalculationsBear.BasicStats.CritChanceReduction, 0f) : 0f;
                        float hits = Math.Max(1f - (Math.Max(0f, crits) + /*Math.Max(crushes, 0)*/ +(currentCalculationsBear.AvoidancePreDR)), 0f);

                        calcMiss.OverallPoints = calcMiss.MitigationPoints = currentCalculationsBear.Miss * 100f;
                        calcDodge.OverallPoints = calcDodge.MitigationPoints = currentCalculationsBear.Dodge * 100f;
                        calcCrit.OverallPoints = calcCrit.SurvivalPoints = crits * 100f;
                        //calcCrush.OverallPoints = calcCrush.SurvivalPoints = crushes;
                        calcHit.OverallPoints = calcHit.SurvivalPoints = hits * 100f;
                    }
                    return new ComparisonCalculationBase[] { calcMiss, calcDodge, calcCrit, /*calcCrush,*/ calcHit };

                //Not used anymore
                //case "Relative Stat Values": 
                //    CharacterCalculationsBear calcBaseValue = GetCharacterCalculations(character) as CharacterCalculationsBear;
                //    //CharacterCalculationsBear calcAgiValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 10 } }) as CharacterCalculationsBear;
                //    //CharacterCalculationsBear calcACValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Armor = 10 } }) as CharacterCalculationsBear;
                //    //CharacterCalculationsBear calcStaValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = 10 } }) as CharacterCalculationsBear;
                //    //CharacterCalculationsBear calcDefValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DefenseRating = 1 } }) as CharacterCalculationsBear;
                //    CharacterCalculationsBear calcDodgeValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DodgeRating = 1 } }) as CharacterCalculationsBear;
                //    CharacterCalculationsBear calcHealthValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Health = 1 } }) as CharacterCalculationsBear;
                //    CharacterCalculationsBear calcResilValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Resilience = 1 } }) as CharacterCalculationsBear;
                //    //CharacterCalculationsBear calcStrengthValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = 1 } }) as CharacterCalculationsBear;
                //    CharacterCalculationsBear calcAPValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { AttackPower = 1 } }) as CharacterCalculationsBear;
                //    CharacterCalculationsBear calcExpertiseValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { ExpertiseRating = 1 } }) as CharacterCalculationsBear;
                //    CharacterCalculationsBear calcHitValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 1 } }) as CharacterCalculationsBear;
                //    CharacterCalculationsBear calcPenValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { ArmorPenetration = 1 } }) as CharacterCalculationsBear;
                //    CharacterCalculationsBear calcHasteValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 1 } }) as CharacterCalculationsBear;
                //    CharacterCalculationsBear calcDamageValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { WeaponDamage = 1 } }) as CharacterCalculationsBear;
                //    CharacterCalculationsBear calcCritValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 1 } }) as CharacterCalculationsBear;

                //    //Differential Calculations for Agi
                //    CharacterCalculationsBear calcAtAdd = calcBaseValue;
                //    float agiToAdd = 0f;
                //    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && agiToAdd < 2)
                //    {
                //        agiToAdd += 0.01f;
                //        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToAdd } }) as CharacterCalculationsBear;
                //    }

                //    CharacterCalculationsBear calcAtSubtract = calcBaseValue;
                //    float agiToSubtract = 0f;
                //    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && agiToSubtract > -2)
                //    {
                //        agiToSubtract -= 0.01f;
                //        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToSubtract } }) as CharacterCalculationsBear;
                //    }
                //    agiToSubtract += 0.01f;

                //    ComparisonCalculationBear comparisonAgi = new ComparisonCalculationBear() { 
                //        Name = string.Format("Agility ({0})", agiToAdd-agiToSubtract), 
                //        OverallPoints =     (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (agiToAdd - agiToSubtract),
                //        MitigationPoints =  (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (agiToAdd - agiToSubtract), 
                //        SurvivalPoints =    (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (agiToAdd - agiToSubtract),
                //        ThreatPoints =      (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (agiToAdd - agiToSubtract)};


                //    //Differential Calculations for Str
                //    calcAtAdd = calcBaseValue;
                //    float strToAdd = 0f;
                //    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && strToAdd < 2)
                //    {
                //        strToAdd += 0.01f;
                //        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToAdd } }) as CharacterCalculationsBear;
                //    }

                //    calcAtSubtract = calcBaseValue;
                //    float strToSubtract = 0f;
                //    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && strToSubtract > -2)
                //    {
                //        strToSubtract -= 0.01f;
                //        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToSubtract } }) as CharacterCalculationsBear;
                //    }
                //    strToSubtract += 0.01f;

                //    ComparisonCalculationBear comparisonStr = new ComparisonCalculationBear()
                //    {
                //        Name = string.Format("Strength ({0})", strToAdd-strToSubtract),
                //        OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (strToAdd - strToSubtract),
                //        MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (strToAdd - strToSubtract),
                //        SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (strToAdd - strToSubtract),
                //        ThreatPoints = (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (strToAdd - strToSubtract)
                //    };


                //    //Differential Calculations for Def
                //    calcAtAdd = calcBaseValue;
                //    float defToAdd = 0f;
                //    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && defToAdd < 20)
                //    {
                //        defToAdd += 0.01f;
                //        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DefenseRating = defToAdd } }) as CharacterCalculationsBear;
                //    }

                //    calcAtSubtract = calcBaseValue;
                //    float defToSubtract = 0f;
                //    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && defToSubtract > -20)
                //    {
                //        defToSubtract -= 0.01f;
                //        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DefenseRating = defToSubtract } }) as CharacterCalculationsBear;
                //    }
                //    defToSubtract += 0.01f;

                //    ComparisonCalculationBear comparisonDef = new ComparisonCalculationBear()
                //    {
                //        Name = string.Format("Defense Rating ({0})", defToAdd-defToSubtract),
                //        OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (defToAdd - defToSubtract),
                //        MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (defToAdd - defToSubtract),
                //        SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (defToAdd - defToSubtract),
                //        ThreatPoints = (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (defToAdd - defToSubtract)
                //    };


                //    //Differential Calculations for AC
                //    calcAtAdd = calcBaseValue;
                //    float acToAdd = 0f;
                //    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && acToAdd < 2)
                //    {
                //        acToAdd += 0.01f;
                //        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Armor = acToAdd } }) as CharacterCalculationsBear;
                //    }

                //    calcAtSubtract = calcBaseValue;
                //    float acToSubtract = 0f;
                //    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && acToSubtract > -2)
                //    {
                //        acToSubtract -= 0.01f;
                //        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Armor = acToSubtract } }) as CharacterCalculationsBear;
                //    }
                //    acToSubtract += 0.01f;

                //    ComparisonCalculationBear comparisonAC = new ComparisonCalculationBear() {
                //        Name = string.Format("Armor ({0})", acToAdd-acToSubtract),
                //        OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (acToAdd - acToSubtract),
                //        MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (acToAdd - acToSubtract), 
                //        SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (acToAdd - acToSubtract), 
                //        ThreatPoints = (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (acToAdd - acToSubtract)
                //    };


                //    //Differential Calculations for BonusAC
                //    calcAtAdd = calcBaseValue;
                //    float bacToAdd = 0f;
                //    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && bacToAdd < 2)
                //    {
                //        bacToAdd += 0.01f;
                //        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { BonusArmor = bacToAdd } }) as CharacterCalculationsBear;
                //    }

                //    calcAtSubtract = calcBaseValue;
                //    float bacToSubtract = 0f;
                //    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && bacToSubtract > -2)
                //    {
                //        bacToSubtract -= 0.01f;
                //        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { BonusArmor = bacToSubtract } }) as CharacterCalculationsBear;
                //    }
                //    bacToSubtract += 0.01f;

                //    ComparisonCalculationBear comparisonBAC = new ComparisonCalculationBear()
                //    {
                //        Name = string.Format("Bonus Armor ({0})", bacToAdd - bacToSubtract),
                //        OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (bacToAdd - bacToSubtract),
                //        MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (bacToAdd - bacToSubtract),
                //        SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (bacToAdd - bacToSubtract),
                //        ThreatPoints = (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (bacToAdd - bacToSubtract)
                //    };


                //    //Differential Calculations for Sta
                //    calcAtAdd = calcBaseValue;
                //    float staToAdd = 0f;
                //    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && staToAdd < 2)
                //    {
                //        staToAdd += 0.01f;
                //        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = staToAdd } }) as CharacterCalculationsBear;
                //    }

                //    calcAtSubtract = calcBaseValue;
                //    float staToSubtract = 0f;
                //    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && staToSubtract > -2)
                //    {
                //        staToSubtract -= 0.01f;
                //        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = staToSubtract } }) as CharacterCalculationsBear;
                //    }
                //    staToSubtract += 0.01f;

                //    ComparisonCalculationBear comparisonSta = new ComparisonCalculationBear() {
                //        Name = string.Format("Stamina ({0})", staToAdd-staToSubtract),
                //        OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (staToAdd - staToSubtract),
                //        MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (staToAdd - staToSubtract), 
                //        SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (staToAdd - staToSubtract), 
                //        ThreatPoints = (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (staToAdd - staToSubtract)};

                //    return new ComparisonCalculationBase[] { 
                //        comparisonAgi,
                //        comparisonAC,
                //        comparisonBAC,
                //        comparisonSta,
                //        comparisonDef,
                //        comparisonStr,
                //        new ComparisonCalculationBear() { Name = "Attack Power", 
                //            OverallPoints = (calcAPValue.OverallPoints - calcBaseValue.OverallPoints), 
                //            MitigationPoints = (calcAPValue.MitigationPoints - calcBaseValue.MitigationPoints), 
                //            SurvivalPoints = (calcAPValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
                //            ThreatPoints = (calcAPValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                //        new ComparisonCalculationBear() { Name = "Hit Rating", 
                //            OverallPoints = (calcHitValue.OverallPoints - calcBaseValue.OverallPoints), 
                //            MitigationPoints = (calcHitValue.MitigationPoints - calcBaseValue.MitigationPoints), 
                //            SurvivalPoints = (calcHitValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
                //            ThreatPoints = (calcHitValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                //        new ComparisonCalculationBear() { Name = "Crit Rating", 
                //            OverallPoints = (calcCritValue.OverallPoints - calcBaseValue.OverallPoints), 
                //            MitigationPoints = (calcCritValue.MitigationPoints - calcBaseValue.MitigationPoints), 
                //            SurvivalPoints = (calcCritValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
                //            ThreatPoints = (calcCritValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                //        new ComparisonCalculationBear() { Name = "Weapon Damage", 
                //            OverallPoints = (calcDamageValue.OverallPoints - calcBaseValue.OverallPoints), 
                //            MitigationPoints = (calcDamageValue.MitigationPoints - calcBaseValue.MitigationPoints), 
                //            SurvivalPoints = (calcDamageValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
                //            ThreatPoints = (calcDamageValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                //        new ComparisonCalculationBear() { Name = "Haste Rating", 
                //            OverallPoints = (calcHasteValue.OverallPoints - calcBaseValue.OverallPoints), 
                //            MitigationPoints = (calcHasteValue.MitigationPoints - calcBaseValue.MitigationPoints), 
                //            SurvivalPoints = (calcHasteValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
                //            ThreatPoints = (calcHasteValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                //        new ComparisonCalculationBear() { Name = "Expertise Rating", 
                //            OverallPoints = (calcExpertiseValue.OverallPoints - calcBaseValue.OverallPoints), 
                //            MitigationPoints = (calcExpertiseValue.MitigationPoints - calcBaseValue.MitigationPoints), 
                //            SurvivalPoints = (calcExpertiseValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
                //            ThreatPoints = (calcExpertiseValue.ThreatPoints - calcBaseValue.ThreatPoints)},

                //        new ComparisonCalculationBear() { Name = "Dodge Rating", 
                //            OverallPoints = (calcDodgeValue.OverallPoints - calcBaseValue.OverallPoints), 
                //            MitigationPoints = (calcDodgeValue.MitigationPoints - calcBaseValue.MitigationPoints), 
                //            SurvivalPoints = (calcDodgeValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
                //            ThreatPoints = (calcDodgeValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                //        new ComparisonCalculationBear() { Name = "Health", 
                //            OverallPoints = (calcHealthValue.OverallPoints - calcBaseValue.OverallPoints), 
                //            MitigationPoints = (calcHealthValue.MitigationPoints - calcBaseValue.MitigationPoints), 
                //            SurvivalPoints = (calcHealthValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
                //            ThreatPoints = (calcHealthValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                //        new ComparisonCalculationBear() { Name = "Resilience", 
                //            OverallPoints = (calcResilValue.OverallPoints - calcBaseValue.OverallPoints), 
                //            MitigationPoints = (calcResilValue.MitigationPoints - calcBaseValue.MitigationPoints), 
                //            SurvivalPoints = (calcResilValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
                //            ThreatPoints = (calcResilValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                //    };

                //case "Rotation DPS":
                //    CharacterCalculationsBear calcsDPS = GetCharacterCalculations(character) as CharacterCalculationsBear;
                //    List<ComparisonCalculationBase> comparisonsDPS = new List<ComparisonCalculationBase>();

                //    BearRotationCalculator rotationCalculatorDPS = new BearRotationCalculator(calcsDPS.MeleeDamageAverage,
                //        calcsDPS.MaulDamageAverage, calcsDPS.MangleDamageAverage, calcsDPS.SwipeDamageAverage,
                //        calcsDPS.FaerieFireDamageAverage, calcsDPS.LacerateDamageAverage, calcsDPS.LacerateDotDamageAverage, calcsDPS.MeleeThreatAverage,
                //        calcsDPS.MaulThreatAverage, calcsDPS.MangleThreatAverage, calcsDPS.SwipeThreatAverage,
                //        calcsDPS.FaerieFireThreatAverage, calcsDPS.LacerateThreatAverage, calcsDPS.LacerateDotThreatAverage,
                //        6f - calcsDPS.BasicStats.MangleCooldownReduction, calcsDPS.AttackSpeed);

                //    for (int useMaul = 0; useMaul < 3; useMaul++)
                //        for (int useMangle = 0; useMangle < 2; useMangle++)
                //            for (int useSwipe = 0; useSwipe < 2; useSwipe++)
                //                for (int useFaerieFire = 0; useFaerieFire < 2; useFaerieFire++)
                //                    for (int useLacerate = 0; useLacerate < 2; useLacerate++)
                //                    {
                //                        bool?[] useMaulValues = new bool?[] { null, false, true };
                //                        BearRotationCalculator.BearRotationCalculation rotationCalculation =
                //                            rotationCalculatorDPS.GetRotationCalculations(useMaulValues[useMaul],
                //                            useMangle == 1, useSwipe == 1, useFaerieFire == 1, useLacerate == 1);

                //                        comparisonsDPS.Add(new ComparisonCalculationBear()
                //                        {
                //                            CharacterItems = character.GetItems(),
                //                            Name = rotationCalculation.Name.Replace("+", " + "),
                //                            OverallPoints = rotationCalculation.DPS,
                //                            ThreatPoints = rotationCalculation.DPS
                //                        });
                //                    }

                //    return comparisonsDPS.ToArray();

                //case "Rotation TPS":
                //    CharacterCalculationsBear calcsTPS = GetCharacterCalculations(character) as CharacterCalculationsBear;
                //    List<ComparisonCalculationBase> comparisonsTPS = new List<ComparisonCalculationBase>();

                //    BearRotationCalculator rotationCalculatorTPS = new BearRotationCalculator(calcsTPS.MeleeDamageAverage,
                //        calcsTPS.MaulDamageAverage, calcsTPS.MangleDamageAverage, calcsTPS.SwipeDamageAverage,
                //        calcsTPS.FaerieFireDamageAverage, calcsTPS.LacerateDamageAverage, calcsTPS.LacerateDotDamageAverage, calcsTPS.MeleeThreatAverage,
                //        calcsTPS.MaulThreatAverage, calcsTPS.MangleThreatAverage, calcsTPS.SwipeThreatAverage,
                //        calcsTPS.FaerieFireThreatAverage, calcsTPS.LacerateThreatAverage, calcsTPS.LacerateDotThreatAverage,
                //        6f - calcsTPS.BasicStats.MangleCooldownReduction, calcsTPS.AttackSpeed);

                //    for (int useMaul = 0; useMaul < 3; useMaul++)
                //        for (int useMangle = 0; useMangle < 2; useMangle++)
                //            for (int useSwipe = 0; useSwipe < 2; useSwipe++)
                //                for (int useFaerieFire = 0; useFaerieFire < 2; useFaerieFire++)
                //                    for (int useLacerate = 0; useLacerate < 2; useLacerate++)
                //                    {
                //                        bool?[] useMaulValues = new bool?[] { null, false, true };
                //                        BearRotationCalculator.BearRotationCalculation rotationCalculation =
                //                            rotationCalculatorTPS.GetRotationCalculations(useMaulValues[useMaul],
                //                            useMangle == 1, useSwipe == 1, useFaerieFire == 1, useLacerate == 1);

                //                        comparisonsTPS.Add(new ComparisonCalculationBear()
                //                        {
                //                            CharacterItems = character.GetItems(),
                //                            Name = rotationCalculation.Name.Replace("+", " + "),
                //                            OverallPoints = rotationCalculation.TPS,
                //                            ThreatPoints = rotationCalculation.TPS
                //                        });
                //                    }

                //    return comparisonsTPS.ToArray();

                default:
                    return new ComparisonCalculationBase[0];
            }
        }
        #endregion

        #region Relevancy Methods
        public override bool IsItemRelevant(Item item)
        {
            if (item.Slot == ItemSlot.OffHand ||
                (item.Slot == ItemSlot.Ranged && item.Type != ItemType.Relic) ||
                item.Stats.SpellPower > 0 || item.Stats.Intellect > 0) 
                return false;
            return base.IsItemRelevant(item);
        }

        public override bool IsBuffRelevant(Buff buff, Character character) {
            if (buff != null && buff.Group == "Set Bonuses")
            {
                if (buff.SetName == "Gladiator's Sanctuary"
                || buff.SetName == "Stormrider's Battlegarb"
                || buff.SetName == "Obsidian Arborweave Battlegarb"
                || buff.SetName == "Deep Earth Battlegarb")
                { return true; }
                else
                { return false; }
            }
            else
                return base.IsBuffRelevant(buff, character);
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                Stamina = stats.Stamina,
                Agility = stats.Agility,
                DodgeRating = stats.DodgeRating,
                MasteryRating = stats.MasteryRating,
                Resilience = stats.Resilience,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BaseArmorMultiplier = stats.BaseArmorMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                Health = stats.Health,
                BattlemasterHealthProc = stats.BattlemasterHealthProc,
                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                Miss = stats.Miss,
                CritChanceReduction = stats.CritChanceReduction,
                ArcaneResistance = stats.ArcaneResistance,
                NatureResistance = stats.NatureResistance,
                FireResistance = stats.FireResistance,
                FrostResistance = stats.FrostResistance,
                ShadowResistance = stats.ShadowResistance,
                ArcaneResistanceBuff = stats.ArcaneResistanceBuff,
                NatureResistanceBuff = stats.NatureResistanceBuff,
                FireResistanceBuff = stats.FireResistanceBuff,
                FrostResistanceBuff = stats.FrostResistanceBuff,
                ShadowResistanceBuff = stats.ShadowResistanceBuff,
                HighestStat = stats.HighestStat,
                Paragon = stats.Paragon,
                Dodge = stats.Dodge,

                Strength = stats.Strength,
                AttackPower = stats.AttackPower,
                CritRating = stats.CritRating,
                PhysicalCrit = stats.PhysicalCrit,
                HitRating = stats.HitRating,
                PhysicalHit = stats.PhysicalHit,
                MoteOfAnger = stats.MoteOfAnger,
                HasteRating = stats.HasteRating,
                PhysicalHaste = stats.PhysicalHaste,
                ExpertiseRating = stats.ExpertiseRating,
                ArmorPenetration = stats.ArmorPenetration,
                TargetArmorReduction = stats.TargetArmorReduction,
                WeaponDamage = stats.WeaponDamage,
                BonusCritDamageMultiplier = stats.BonusCritDamageMultiplier,
                BonusDamageMultiplierLacerate = stats.BonusDamageMultiplierLacerate,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusWhiteDamageMultiplier = stats.BonusWhiteDamageMultiplier,
                DamageTakenReductionMultiplier = stats.DamageTakenReductionMultiplier,
                BossPhysicalDamageDealtReductionMultiplier = stats.BossPhysicalDamageDealtReductionMultiplier,
                BossAttackSpeedReductionMultiplier = stats.BossAttackSpeedReductionMultiplier,
                SpellCrit = stats.SpellCrit,
                SpellCritOnTarget = stats.SpellCritOnTarget,
                //Intellect = stats.Intellect,
                ArcaneDamage = stats.ArcaneDamage,
                BonusArcaneDamageMultiplier = stats.BonusArcaneDamageMultiplier,
                FireDamage = stats.FireDamage,
                BonusFireDamageMultiplier = stats.BonusFireDamageMultiplier,
                FrostDamage = stats.FrostDamage,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                NatureDamage = stats.NatureDamage,
                BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier,
                ShadowDamage = stats.ShadowDamage,
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                PhysicalDamage = stats.PhysicalDamage,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                SpellHit = stats.SpellHit,
                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                DamageAbsorbed = stats.DamageAbsorbed,
                Healed = stats.Healed,
                HealthRestore = stats.HealthRestore,
                HealthRestoreFromMaxHealth = stats.HealthRestoreFromMaxHealth,
                BonusHealingReceived = stats.BonusHealingReceived,
                //
                MovementSpeed = stats.MovementSpeed,
                FearDurReduc = stats.FearDurReduc,
                StunDurReduc = stats.StunDurReduc,
                SnareRootDurReduc = stats.SnareRootDurReduc,
            };
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit || effect.Trigger == Trigger.MeleeAttack
                || effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit || effect.Trigger == Trigger.PhysicalAttack || effect.Trigger == Trigger.DoTTick
                || effect.Trigger == Trigger.DamageDone || effect.Trigger == Trigger.MangleBearHit || effect.Trigger == Trigger.LacerateTick
                || effect.Trigger == Trigger.SwipeBearOrLacerateHit || effect.Trigger == Trigger.DamageTaken || effect.Trigger == Trigger.DamageTakenPhysical
                || effect.Trigger == Trigger.MangleCatOrShredOrInfectedWoundsHit || effect.Trigger == Trigger.DamageOrHealingDone
                || effect.Trigger == Trigger.DamageTakenPutsMeBelow35PercHealth || effect.Trigger == Trigger.DamageTakenPutsMeBelow50PercHealth)
                {
                    if (HasRelevantStats(effect.Stats))
                    {
                        s.AddSpecialEffect(effect);
                    }
                }
            }
            return s;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            bool relevant = (
                // Core Stats
                stats.Agility + stats.BonusAgilityMultiplier +
                stats.Stamina + stats.BonusStaminaMultiplier +
                stats.Health + stats.BonusHealthMultiplier +
                stats.Strength + stats.BonusStrengthMultiplier +
                stats.AttackPower + stats.BonusAttackPowerMultiplier +
                stats.CritRating + stats.PhysicalCrit + stats.BonusCritDamageMultiplier + 
                stats.HasteRating + stats.PhysicalHaste +
                stats.Armor + stats.BonusArmor + stats.BonusArmorMultiplier +
                stats.DodgeRating + stats.Dodge +
                stats.HitRating + stats.PhysicalHit +
                stats.ExpertiseRating +
                stats.ArmorPenetration +
                stats.BonusDamageMultiplier +
                stats.BonusWhiteDamageMultiplier + stats.BonusArcaneDamageMultiplier +
                stats.BonusFireDamageMultiplier + stats.BonusFrostDamageMultiplier + 
                stats.BonusNatureDamageMultiplier + stats.BonusShadowDamageMultiplier + 
                stats.ArcaneDamage + stats.FireDamage + stats.FrostDamage +
                stats.NatureDamage + stats.ShadowDamage + stats.PhysicalDamage +
                stats.BonusPhysicalDamageMultiplier + 
                stats.MasteryRating +
                stats.ThreatIncreaseMultiplier + 
                // Health
                stats.Healed +
                stats.HealthRestore +
                stats.HealthRestoreFromMaxHealth +
                stats.DamageAbsorbed +
                stats.BonusHealingReceived +
                // Stats that are for Target
                stats.TargetArmorReduction +
                stats.BossAttackSpeedReductionMultiplier +
                stats.DamageTakenReductionMultiplier +
                stats.BossPhysicalDamageDealtReductionMultiplier +
                stats.SpellCritOnTarget +
                // Maybe Stats
                stats.Resilience +
                stats.WeaponDamage +
                //stats.SpellCrit + 
                //stats.Intellect + stats.SpellHit + 
                // Resistances
                stats.ArcaneResistance + stats.ArcaneResistanceBuff +
                stats.NatureResistance + stats.NatureResistanceBuff + 
                stats.FireResistance + stats.FireResistanceBuff +
                stats.FrostResistance + stats.FrostResistanceBuff +
                stats.ShadowResistance + stats.ShadowResistanceBuff + 
                // Specials
                stats.Miss +
                stats.CritChanceReduction +
                stats.BattlemasterHealthProc + stats.MoteOfAnger +
                stats.HighestStat + stats.HighestSecondaryStat + stats.Paragon +
                // Specific to Bear
                stats.BonusDamageMultiplierLacerate +
                // Boss Handler
                stats.MovementSpeed + stats.FearDurReduc + stats.StunDurReduc + stats.SnareRootDurReduc
                 ) != 0;

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit || effect.Trigger == Trigger.MeleeAttack
                    || effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit || effect.Trigger == Trigger.PhysicalAttack
                    || effect.Trigger == Trigger.MangleBearHit || effect.Trigger == Trigger.SwipeBearOrLacerateHit
                    || effect.Trigger == Trigger.DamageTaken || effect.Trigger == Trigger.DamageTakenPhysical
                    || effect.Trigger == Trigger.MangleCatOrShredOrInfectedWoundsHit || effect.Trigger == Trigger.LacerateTick
                    || effect.Trigger == Trigger.Barkskin || effect.Trigger == Trigger.DamageTakenPutsMeBelow35PercHealth)
                {
                    relevant |= HasRelevantStats(effect.Stats);
                    if (relevant) break;
                }
            }
            return relevant;
        }

        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            if (_relevantGlyphs == null)
            {
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of Aquatic Form");
                _relevantGlyphs.Add("Glyph of Barkskin");
                _relevantGlyphs.Add("Glyph of Berserk");
                _relevantGlyphs.Add("Glyph of Challenging Roar");
                _relevantGlyphs.Add("Glyph of Dash");
                _relevantGlyphs.Add("Glyph of Entangling Roots");
                _relevantGlyphs.Add("Glyph of Faerie Fire");
                _relevantGlyphs.Add("Glyph of Feral Charge");
                _relevantGlyphs.Add("Glyph of Ferocious Bite");
                _relevantGlyphs.Add("Glyph of Frenzied Regeneration");
                _relevantGlyphs.Add("Glyph of Lacerate");
                _relevantGlyphs.Add("Glyph of Mangle");
                _relevantGlyphs.Add("Glyph of Maul");
                _relevantGlyphs.Add("Glyph of Rake");
                _relevantGlyphs.Add("Glyph of Rebirth");
                _relevantGlyphs.Add("Glyph of Rip");
                _relevantGlyphs.Add("Glyph of Savage Roar");
                _relevantGlyphs.Add("Glyph of Shred");
                _relevantGlyphs.Add("Glyph of The Wild");
                _relevantGlyphs.Add("Glyph of Thorns");
                _relevantGlyphs.Add("Glyph of Tiger's Fury");
                _relevantGlyphs.Add("Glyph of Unburdened Rebirth");
            }
            return _relevantGlyphs;
        }
        #endregion

        public Stats GetBuffsStats(Character character, CalculationOptionsBear calcOpts)
        {
            //List<Buff> removedBuffs = new List<Buff>();
            //List<Buff> addedBuffs = new List<Buff>();

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
            }*/
            #endregion

            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs, character.SetBonusCount);

            /*foreach (Buff b in removedBuffs) {
                character.ActiveBuffsAdd(b);
            }
            foreach (Buff b in addedBuffs) {
                character.ActiveBuffs.Remove(b);
            }*/

            return statsBuffs;
        }
        public override void SetDefaults(Character character)
        {
            character.ActiveBuffsAdd("Strength of Earth Totem");
            character.ActiveBuffsAdd("Devotion Aura");
            character.ActiveBuffsAdd("Ancestral Healing");
            character.ActiveBuffsAdd("Trueshot Aura");
            character.ActiveBuffsAdd("Ferocious Inspiration");
            character.ActiveBuffsAdd("Power Word: Fortitude");
            character.ActiveBuffsAdd("Leader of the Pack");
            character.ActiveBuffsAdd("Windfury Totem");
            character.ActiveBuffsAdd("Blessing of Kings");
            character.ActiveBuffsAdd("Shadow Protection");
            character.ActiveBuffsAdd("Elemental Resistance Totem");
            character.ActiveBuffsAdd("Heroism/Bloodlust");
            character.ActiveBuffsAdd("Faerie Fire");
            character.ActiveBuffsAdd("Demoralizing Roar");
            character.ActiveBuffsAdd("Critical Mass");
            character.ActiveBuffsAdd("Master Poisoner");
            character.ActiveBuffsAdd("Savage Combat");
            character.ActiveBuffsAdd("Judgements of the Just");
            character.ActiveBuffsAdd("Flask of Steelskin");
            if (character.PrimaryProfession == Profession.Alchemy || character.SecondaryProfession == Profession.Alchemy)
                character.ActiveBuffsAdd("Flask of Steelskin (Mixology)");
            character.ActiveBuffsAdd("Indestructible Potion");
            character.ActiveBuffsAdd("Agility Food");

            //Prime
            //character.DruidTalents.GlyphOfBerserk = true;
            //character.DruidTalents.GlyphOfLacerate = true;
            //character.DruidTalents.GlyphOfMangle = true;
            
            //Major
            //character.DruidTalents.GlyphOfFaerieFire = true;
            //character.DruidTalents.GlyphOfFeralCharge = true;
            //character.DruidTalents.GlyphOfFrenziedRegeneration = true;
            //character.DruidTalents.GlyphOfMaul = true;
            //character.DruidTalents.GlyphOfThorns = true;
            
            //Minor
            //character.DruidTalents.GlyphOfChallengingRoar = true;
            //character.DruidTalents.GlyphOfUnburdenedRebirth = true;
            //character.DruidTalents.GlyphOfDash = true;

            #region Boss Options
            // Never in back of the Boss
            character.BossOptions.InBack = false;

            int avg = character.AvgWornItemLevel;
            int[] points = new int[] { 365, 378, 384 };
            #region Need a Boss Attack
            character.BossOptions.DamagingTargs = true;
            if (character.BossOptions.DefaultMeleeAttack == null) {
                character.BossOptions.Attacks.Add(BossHandler.ADefaultMeleeAttack);
            }
            if        (avg <= points[0]) {
                character.BossOptions.Health = 20000000;
                character.BossOptions.DefaultMeleeAttack.DamagePerHit = BossHandler.StandardMeleePerHit[(int)BossHandler.TierLevels.T12_10];
            } else if (avg <= points[1]) {
                character.BossOptions.Health = 35000000;
                character.BossOptions.DefaultMeleeAttack.DamagePerHit = BossHandler.StandardMeleePerHit[(int)BossHandler.TierLevels.T12_25];
            } else if (avg <= points[2]) {
                character.BossOptions.Health = 50000000;
                character.BossOptions.DefaultMeleeAttack.DamagePerHit = BossHandler.StandardMeleePerHit[(int)BossHandler.TierLevels.T12_10H];
            } else if (avg >  points[2]) {
                character.BossOptions.Health = 65000000;
                character.BossOptions.DefaultMeleeAttack.DamagePerHit = BossHandler.StandardMeleePerHit[(int)BossHandler.TierLevels.T12_25H];
            }
            #endregion
            #endregion
        }
    }
}
