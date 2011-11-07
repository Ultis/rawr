using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;
using Rawr.DK;
//using Rawr.Graph;

namespace Rawr.TankDK
{
    [Rawr.Calculations.RawrModelInfo("TankDK", "spell_deathknight_darkconviction", CharacterClass.DeathKnight)]
    public class CalculationsTankDK : CalculationsBase
    {
        #region Inherited Variables and Properties
        #region Gemming Templates
        private static void fixArray(int[] thearray)
        {
            if (thearray[0] == 0) return; // Nothing to do, they are all 0
            if (thearray[1] == 0) thearray[1] = thearray[0]; // There was a Green, but no Blue
            if (thearray[2] == 0) thearray[2] = thearray[1]; // There was a Blue (or Green as set above), but no Purple
            if (thearray[3] == 0) thearray[3] = thearray[2]; // There was a Purple (or Blue/Green as set above), but no Jewel
        }
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                ////Relevant Gem IDs for TankDKs
                //Red
                //                    UC     Rare   Epic   JC
                int[] flashing = { 52083, 52216, 0, 52259 }; // +parry 
                fixArray(flashing);

                //Purple
                int[] defenders = { 52097, 52210, 0, 0}; // parry+Stam
                fixArray(defenders);
                int[] retaliating = { 52103, 52234, 0, 0 }; // parry+hit
                fixArray(retaliating);

                //Blue
                int[] solid = { 52086, 52242, 0, 52261 }; // +Stam
                fixArray(solid);

                //Green
                int[] puissant = { 52126, 52231, 0, 0}; // +mast +Stam
                fixArray(puissant);
                int[] regal = { 52119, 52233, 0, 0}; // +dodge, Stam
                fixArray(regal);
                int[] nimble = { 52120, 52227, 0, 0 }; // +dodge, hit
                fixArray(nimble);

                //Yellow
                int[] fractured = { 52094, 52219, 0, 52269 }; // +Mastery
                fixArray(fractured);
                int[] subtle = { 52090, 52247, 0, 52265 }; // +Dodge
                fixArray(subtle);

                //Orange
                int[] fine = { 52116, 52215, 0, 0 }; // +parry +mast
                fixArray(fine);
                int[] polished = { 52106, 52229, 0, 0 }; // +agi dodge
                fixArray(polished);
                int[] resolute = { 52107, 52249, 0, 0 }; // +exp dodge
                fixArray(resolute);

                //Meta
                int austere = 52294;
                int effulgent = 52295;
                int fleet = 52289;

                // Prismatic:
//                int nightmare = 49110;

                // Cogwheels
                int[] cog_exp = { 59489, 59489, 59489, 59489 }; fixArray(cog_exp);
                int[] cog_hit = { 59493, 59493, 59493, 59493 }; fixArray(cog_hit);
                int[] cog_mst = { 59480, 59480, 59480, 59480 }; fixArray(cog_mst);
                int[] cog_crt = { 59478, 59478, 59478, 59478 }; fixArray(cog_crt);
                int[] cog_has = { 59479, 59479, 59479, 59479 }; fixArray(cog_has);
                int[] cog_pry = { 59491, 59491, 59491, 59491 }; fixArray(cog_pry);
                int[] cog_ddg = { 59477, 59477, 59477, 59477 }; fixArray(cog_ddg);
                int[] cog_spr = { 59496, 59496, 59496, 59496 }; fixArray(cog_spr);

                return new List<GemmingTemplate>() {
                    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", Enabled = true, // Avoidance Stam
                        RedId = flashing[0], YellowId = subtle[0], BlueId = solid[0], PrismaticId = solid[0], MetaId = austere, HydraulicId = 0 },
                    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", Enabled = true, // Mastery 
                        RedId = fine[0], YellowId = fractured[0], BlueId = puissant[0], PrismaticId = fractured[0], MetaId = fleet, HydraulicId = 0 },
                    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", Enabled = true, // Dodge
                        RedId = polished[0], YellowId = subtle[0], BlueId = regal[0], PrismaticId = subtle[0], MetaId = austere, HydraulicId = 0  },
                    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", Enabled = true, // Parry
                        RedId = flashing[0], YellowId = fine[0], BlueId = defenders[0], PrismaticId = flashing[0], MetaId = austere, HydraulicId = 0  },
                    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", Enabled = true, // Stamina
                        RedId = defenders[0], YellowId = puissant[0], BlueId = solid[0], PrismaticId = solid[0], MetaId = effulgent, HydraulicId = 0  },
                    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", Enabled = false, // Max Stamina
                        RedId = solid[0], YellowId = solid[0], BlueId = solid[0], PrismaticId = solid[0], MetaId = austere, HydraulicId = 0  },
                        
                    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", Enabled = true, // Avoidance Stam
                        RedId = flashing[1], YellowId = subtle[1], BlueId = solid[1], PrismaticId = solid[1], MetaId = austere, HydraulicId = 0 },
                    new GemmingTemplate() { Model = "TankDK", Group = "Rare", Enabled = true, // Mastery 
                        RedId = flashing[1], YellowId = fractured[1], BlueId = puissant[1], PrismaticId = fractured[1], MetaId = fleet, HydraulicId = 0 },
                    new GemmingTemplate() { Model = "TankDK", Group = "Rare", Enabled = true, // Dodge
                        RedId = defenders[1], YellowId = subtle[1], BlueId = regal[1], PrismaticId = subtle[1], MetaId = austere, HydraulicId = 0 },
                    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", Enabled = true, // Parry
                        RedId = flashing[1], YellowId = fine[1], BlueId = defenders[1], PrismaticId = flashing[1], MetaId = austere, HydraulicId = 0  },
                    new GemmingTemplate() { Model = "TankDK", Group = "Rare", Enabled = true, // Stamina
                        RedId = defenders[1], YellowId = puissant[1], BlueId = solid[1], PrismaticId = solid[1], MetaId = effulgent, HydraulicId = 0 },
                    new GemmingTemplate() { Model = "TankDK", Group = "Rare", Enabled = false, // Max Stamina
                        RedId = solid[1], YellowId = solid[1], BlueId = solid[1], PrismaticId = solid[1], MetaId = austere, HydraulicId = 0 },

/*                    new GemmingTemplate() { Model = "TankDK", Group = "Epic", Enabled = true, //Defense 
                        RedId = stalwart[2], YellowId = thick[2], BlueId = enduring[2], PrismaticId = thick[2], MetaId = austere },
                    new GemmingTemplate() { Model = "TankDK", Group = "Epic", Enabled = true, //Dodge
                        RedId = subtle[2], YellowId = stalwart[2], BlueId = regal[2], PrismaticId = subtle[2], MetaId = austere },
                    new GemmingTemplate() { Model = "TankDK", Group = "Epic", Enabled = true, //Max Stamina
                        RedId = solid[2], YellowId = solid[2], BlueId = solid[2], PrismaticId = solid[2], MetaId = austere },
                    new GemmingTemplate() { Model = "TankDK", Group = "Epic",Enabled = true,  //Stamina
                        RedId = regal[2], YellowId = enduring[2], BlueId = solid[2], PrismaticId = nightmare, MetaId = austere },
*/
                    new GemmingTemplate() { Model = "TankDK", Group = "Jewelcrafter", //Max Mastery
                        RedId = fractured[3], YellowId = fractured[3], BlueId = fractured[3], PrismaticId = fractured[3], MetaId = fleet, HydraulicId = 0 },
                    new GemmingTemplate() { Model = "TankDK", Group = "Jewelcrafter", //Max Dodge
                        RedId = subtle[3], YellowId = subtle[3], BlueId = subtle[3], PrismaticId = subtle[3], MetaId = austere, HydraulicId = 0 },
                    new GemmingTemplate() { Model = "TankDK", Group = "Jewelcrafter", //Max parry
                        RedId = flashing[3], YellowId = flashing[3], BlueId = flashing[3], PrismaticId = flashing[3], MetaId = austere, HydraulicId = 0 },
                    new GemmingTemplate() { Model = "TankDK", Group = "Jewelcrafter", //Max Stamina
                        RedId = solid[3], YellowId = solid[3], BlueId = solid[3], PrismaticId = solid[3], MetaId = effulgent, HydraulicId = 0 },

                    new GemmingTemplate() { Model = "TankDK", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_hit[0], MetaId = austere, },
                    new GemmingTemplate() { Model = "TankDK", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_mst[0], MetaId = austere, },
                    new GemmingTemplate() { Model = "TankDK", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_crt[0], MetaId = austere, },
                    new GemmingTemplate() { Model = "TankDK", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_has[0], MetaId = austere, },
                    new GemmingTemplate() { Model = "TankDK", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_pry[0], MetaId = austere, },
                    new GemmingTemplate() { Model = "TankDK", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_ddg[0], MetaId = austere, },

                    new GemmingTemplate() { Model = "TankDK", Group = "Engineer", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_mst[0], MetaId = austere, },
                    new GemmingTemplate() { Model = "TankDK", Group = "Engineer", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_crt[0], MetaId = austere, },
                    new GemmingTemplate() { Model = "TankDK", Group = "Engineer", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_has[0], MetaId = austere, },
                    new GemmingTemplate() { Model = "TankDK", Group = "Engineer", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_pry[0], MetaId = austere, },
                    new GemmingTemplate() { Model = "TankDK", Group = "Engineer", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_ddg[0], MetaId = austere, },

                    new GemmingTemplate() { Model = "TankDK", Group = "Engineer", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_crt[0], MetaId = austere, },
                    new GemmingTemplate() { Model = "TankDK", Group = "Engineer", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_has[0], MetaId = austere, },
                    new GemmingTemplate() { Model = "TankDK", Group = "Engineer", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_pry[0], MetaId = austere, },
                    new GemmingTemplate() { Model = "TankDK", Group = "Engineer", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_ddg[0], MetaId = austere, },
                            
                    new GemmingTemplate() { Model = "TankDK", Group = "Engineer", Enabled = false, CogwheelId = cog_pry[0], Cogwheel2Id = cog_has[0], MetaId = austere, },
                    new GemmingTemplate() { Model = "TankDK", Group = "Engineer", Enabled = false, CogwheelId = cog_pry[0], Cogwheel2Id = cog_ddg[0], MetaId = austere, },

                    new GemmingTemplate() { Model = "TankDK", Group = "Engineer", Enabled = false, CogwheelId = cog_ddg[0], Cogwheel2Id = cog_has[0], MetaId = austere, },

 
                };
            }
        }
        #endregion

        /// <summary>
        /// An array of strings which will be used to build the calculation display.
        /// Each string must be in the format of "Heading:Label". Heading will be used as the
        /// text of the group box containing all labels that have the same Heading.
        /// Label will be the label of that calculation, and may be appended with '*' followed by
        /// a description of that calculation which will be displayed in a tooltip for that label.
        /// Label (without the tooltip string) must be unique.
        /// 
        /// EXAMPLE:
        /// characterDisplayCalculationLabels = new string[]
        /// {
        ///		"Basic Stats:Health",
        ///		"Basic Stats:Armor",
        ///		"Advanced Stats:Dodge",
        ///		"Advanced Stats:Miss*Chance to be missed"
        /// };
        /// </summary>
        public override string[] CharacterDisplayCalculationLabels {
            get {
                if (_characterDisplayCalculationLabels == null) {
                    List<string> labels = new List<string>(new string[] {
                        @"Summary:Survival Points*Survival Points represents the total raw damage 
(pre-Mitigation) you can take before dying. Unlike 
Mitigation Points, you should not attempt to maximize this, 
but rather get 'enough' of it, and then focus on Mitigation. 
'Enough' can vary greatly by fight and by your healers, but 
keeping it roughly even with Mitigation Points is a good 
way to maintain 'enough' as you progress. If you find that 
you are being killed by burst damage, focus on Survival Points.",
                        @"Summary:Mitigation Points*Mitigation Points represent the amount of damage you avoid, 
on average per second, through avoidance stats (Miss, Dodge, Parry) along 
with ways to improve survivablity, +heal or self healing, ability 
cooldowns.  It is directly relative to your Damage Taken. 
Ideally, you want to maximize Mitigation Points, while maintaining 
'enough' Survival Points (see Survival Points). If you find 
yourself dying due to healers running OOM, or being too busy 
healing you and letting other raid members die, then focus on 
Mitigation Points.  Represented in Damage per Second multiplied
by MitigationWeight (seconds).  Note: Subvalues do NOT represent
all mitigation sources, just common ones.",
                        @"Summary:Burst Points*Burst is a new idea that represents the 
mitigation of Burst damage that can be taken during a given 
encounter.  Specifically, this is focused around all On-Use 
Cooldowns & trinkets.",
                        @"Summary:Recovery Points*Recovery is a new idea that represents the 
Recovery of Burst damage that can be taken during a given 
encounter.  Specifically, this is focused around Death Strike
heals, and Blood Shield.",
                        @"Summary:Threat Points*Threat Points represent how much threat is capable for the current 
gear/talent/rotation setup.  Threat points are represented in Threat per second and assume Vengeance is at maximum.",
                        @"Summary:Overall Points*Overall Points are a sum of Mitigation, Survival and Threat Points. 
Overall is typically, but not always, the best way to rate gear. 
For specific encounters, closer attention to Mitigation or Survival 
Points individually may be important.",

                        "Basic Stats:Strength",
                        "Basic Stats:Agility",
                        "Basic Stats:Stamina",
                        "Basic Stats:Mastery",
                        "Basic Stats:Attack Power",
                        "Basic Stats:Crit Rating",
                        "Basic Stats:Hit Rating",
                        "Basic Stats:Expertise",
                        "Basic Stats:Haste Rating",
                        "Basic Stats:Health*Including Blood Presence",
                        "Basic Stats:Armor*Including Blood Presence",

                        "Advanced Stats:Miss*After Diminishing Returns",
                        "Advanced Stats:Dodge*After Diminishing Returns",
                        "Advanced Stats:Parry*After Diminishing Returns",
                        "Advanced Stats:Total Avoidance*Miss + Dodge + Parry",
                        "Advanced Stats:Armor Damage Reduction",
                        "Advanced Stats:Magic Damage Reduction*Currently Magic Resistance Only.",

                        "Threat Stats:Target Miss*Chance to miss the target",
                        "Threat Stats:Target Dodge*Chance the target dodges",
                        "Threat Stats:Target Parry*Chance the target parries",
                        "Threat Stats:Total Threat*Raw Total Threat Generated by the specified rotation",

                        "Damage Data:DPS*DPS done for given rotation",
                        "Damage Data:Rotation Time*Duration of the total rotation cycle",
                        "Damage Data:Blood*Number of Runes consumed",
                        "Damage Data:Frost*Number of Runes consumed",
                        "Damage Data:Unholy*Number of Runes consumed",
                        "Damage Data:Death*Number of Runes consumed",
                        "Damage Data:Runic Power*Amount of Runic Power used by rotation.\nNegative values mean more RP generated than consumed.",
                        "Damage Data:RE Runes*Number of Runes Generated by Runic Empowerment.",

                        "Combat Data:DTPS*Damage Taken Per Sec averaged over the whole fight.",
                        "Combat Data:HPS*Heals Per Sec from Death Strike",
                        "Combat Data:DPS Avoided*Physical incoming DPS reduced only by Miss, Dodge, Parry.",
                        "Combat Data:DPS Reduced By Armor*Physical incoming DPS reduced only by armor.",
                        "Combat Data:Death Strike*Total healing from Death Strikes only assuming full Berserk Timer for current boss.",
                        "Combat Data:Blood Shield*Total Absorbs from Blood Shield assuming full Berserk Timer for current boss.",

                    });
                    _characterDisplayCalculationLabels = labels.ToArray();
                }
                return _characterDisplayCalculationLabels;
            }
        }
        private string[] _characterDisplayCalculationLabels = null;

        /// <summary>
        /// An array of strings which define what calculations (in addition to the subpoint ratings)
        /// will be available to the optimizer
        /// </summary>
        public override string[] OptimizableCalculationLabels {
            get {
                if (_optimizableCalculationLabels == null) {
                    _optimizableCalculationLabels = new string[] {
                        "Chance to be Crit",
                        "Avoidance %",
                        "Damage Reduction %",
                        "% Chance to Hit",
                        "Target Parry %",
                        "Target Dodge %",
                        "Armor",
                        "Health",
                        "Hit Rating",
                        "DPS",
                        "Mastery",
                    };
                }
                return _optimizableCalculationLabels;
            }
        }
        private string[] _optimizableCalculationLabels = null;

        public override Dictionary<string, Color> SubPointNameColors {
            get {
                if (_subPointNameColors == null) {
                    _subPointNameColors = new Dictionary<string, Color>();
                    _subPointNameColors.Add("Mitigation", Colors.Red);
                    _subPointNameColors.Add("Survivability", Colors.Blue);
                    _subPointNameColors.Add("Burst", Colors.Purple);
                    _subPointNameColors.Add("Recovery", /*Colors.PaleVioletRed*/ Color.FromArgb(0xFF, 0xDB, 0x70, 0x93));
                    _subPointNameColors.Add("Threat", Colors.Green);
                }
                return _subPointNameColors;
            }
        }
        private Dictionary<string, Color> _subPointNameColors = null;

        public override void SetDefaults(Character character)
        {
            #region Boss Options
            // Never in back of the Boss
            character.BossOptions.InBack = false;

            int avg = character.AvgWornItemLevel;
            int[] points = new int[] { 350, 358, 365 };
            #region Need a Boss Attack
            character.BossOptions.DamagingTargs = true;
            if (character.BossOptions.DefaultMeleeAttack == null) {
                character.BossOptions.Attacks.Add(BossHandler.ADefaultMeleeAttack);
            }
            if        (avg <= points[0]) {
                character.BossOptions.Health = 20000000;
                character.BossOptions.DefaultMeleeAttack.DamagePerHit = BossHandler.StandardMeleePerHit[(int)BossHandler.TierLevels.T11_10];
            } else if (avg <= points[1]) {
                character.BossOptions.Health = 35000000;
                character.BossOptions.DefaultMeleeAttack.DamagePerHit = BossHandler.StandardMeleePerHit[(int)BossHandler.TierLevels.T11_25];
            } else if (avg <= points[2]) {
                character.BossOptions.Health = 50000000;
                character.BossOptions.DefaultMeleeAttack.DamagePerHit = BossHandler.StandardMeleePerHit[(int)BossHandler.TierLevels.T11_10H];
            } else if (avg >  points[2]) {
                character.BossOptions.Health = 65000000;
                character.BossOptions.DefaultMeleeAttack.DamagePerHit = BossHandler.StandardMeleePerHit[(int)BossHandler.TierLevels.T11_25H];
            }
            #endregion
            #endregion
        }

        #region Low Traffic
        /// <summary>Deserializes the model's CalculationOptions data object from xml</summary>
        /// <param name="xml">The serialized xml representing the model's CalculationOptions data object.</param>
        /// <returns>The model's CalculationOptions data object.</returns>
        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsTankDK));
            StringReader reader = new StringReader(xml);
            CalculationOptionsTankDK calcOpts = serializer.Deserialize(reader) as CalculationOptionsTankDK;
            return calcOpts;
        }

        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
        {
            get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelTankDK()); }
        }

        /// <summary>
        /// List<ItemType> containing all of the ItemTypes relevant to this model. Typically this
        /// means all types of armor/weapons that the intended class is able to use, but may also
        /// be trimmed down further if some aren't typically used. ItemType.None should almost
        /// always be included, because that type includes items with no proficiancy requirement, such
        /// as rings, necklaces, cloaks, held in off hand items, etc.
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
                        ItemType.Plate,
                        ItemType.Sigil,
                        ItemType.Relic,
                        ItemType.Polearm,
                        ItemType.TwoHandAxe,
                        ItemType.TwoHandMace,
                        ItemType.TwoHandSword,
                        ItemType.OneHandAxe,
                        ItemType.OneHandMace,
                        ItemType.OneHandSword
                    });
                }
                return _relevantItemTypes;
            }
        }
        private List<ItemType> _relevantItemTypes = null;

        /// <summary>Character class that this model is for.</summary>
        public override CharacterClass TargetClass { get { return CharacterClass.DeathKnight; } }

        /// <summary>Method to get a new instance of the model's custom ComparisonCalculation class.</summary>
        /// <returns>A new instance of the model's custom ComparisonCalculation class, 
        /// which inherits from ComparisonCalculationBase</returns>
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationTankDK(); }
        
        /// <summary>Method to get a new instance of the model's custom CharacterCalculations class.</summary>
        /// <returns>A new instance of the model's custom CharacterCalculations class, 
        /// which inherits from CharacterCalculationsBase</returns>
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsTankDK(); }
        #endregion
        #endregion

        #region Custom Charts
        /// <summary>The names of all custom charts provided by the model, if any.</summary>
        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] { 
                        "Stats Graph",
                        "Scaling vs Parry Rating"
                    };
                return _customChartNames;
            }
        }

        
        /// <summary>
        /// Gets data to fill a custom chart, based on the chart name, as defined in CustomChartNames.
        /// </summary>
        /// <param name="character">The character to build the chart for.</param>
        /// <param name="chartName">The name of the custom chart to get data for.</param>
        /// <returns>The data for the custom chart.</returns>
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            return new ComparisonCalculationBase[0];
        }

        public override System.Windows.Controls.Control GetCustomChartControl(string chartName)
        {
            switch (chartName)
            {
                case "Stats Graph":
//                case "Scaling vs Dodge Rating":
                case "Scaling vs Parry Rating":
//                case "Scaling vs Mastery Rating":
                    return Graph.Instance;
                default:
                    return null;
            }
        }


        public override void UpdateCustomChartData(Character character, string chartName, System.Windows.Controls.Control control)
        {
            Color[] statColors = new Color[] {
                Color.FromArgb(0xFF, 0xFF, 0, 0), 
                Color.FromArgb(0xFF, 0xFF, 165, 0), 
                Color.FromArgb(0xFF, 0x80, 0x80, 0x00), 
                Color.FromArgb(0xFF, 154, 205, 50), 
                Color.FromArgb(0xFF, 0x00, 0xFF, 0xFF), 
                Color.FromArgb(0xFF, 0, 0, 0xFF), 
                Color.FromArgb(0xFF, 0x80, 0, 0xFF),
                Color.FromArgb(0xFF, 0, 0x80, 0xFF),
            };

            List<float> X = new List<float>();
            List<ComparisonCalculationBase[]> Y = new List<ComparisonCalculationBase[]>();

            float fMultiplier = 1;

            Stats[] statsList = new Stats[] {
                        new Stats() { ParryRating = fMultiplier },
                        new Stats() { DodgeRating = fMultiplier },
                        new Stats() { MasteryRating = fMultiplier },
                        new Stats() { Stamina = fMultiplier },
                        new Stats() { Agility = fMultiplier },
                        new Stats() { Armor = fMultiplier },
                        new Stats() { HasteRating = fMultiplier },
                    };

            switch (chartName)
            {
                case "Stats Graph":
                    Graph.Instance.UpdateStatsGraph(character, statsList, statColors, 200, "", null);
                    break;
                case "Scaling vs Parry Rating":
                    Graph.Instance.UpdateScalingGraph(character, statsList, new Stats() { ParryRating = fMultiplier * 5 }, true, statColors, 100, "", null);
                    break;
            }
        }

        
        #endregion

        #region Stat Relevancy
        /// <summary>Filters a Stats object to just the stats relevant to the model.</summary>
        /// <param name="stats">A complete Stats object containing all stats.</param>
        /// <returns>A filtered Stats object containing only the stats relevant to the model.</returns>
        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                Strength = stats.Strength,
                Agility = stats.Agility,
                BaseAgility = stats.BaseAgility,
                Stamina = stats.Stamina,
                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                Health = stats.Health,
                BattlemasterHealthProc = stats.BattlemasterHealthProc,

                HighestStat = stats.HighestStat,
                Paragon = stats.Paragon,

                ParryRating = stats.ParryRating,
                DodgeRating = stats.DodgeRating,
                CritChanceReduction = stats.CritChanceReduction,

                Dodge = stats.Dodge,
                Parry = stats.Parry,
                Miss = stats.Miss,

                Resilience = stats.Resilience,
                SpellPenetration = stats.SpellPenetration,

                DamageAbsorbed = stats.DamageAbsorbed,
                AttackPower = stats.AttackPower,
                HitRating = stats.HitRating,
                CritRating = stats.CritRating,
                MasteryRating = stats.MasteryRating,
                ExpertiseRating = stats.ExpertiseRating,
                Expertise = stats.Expertise,
                HasteRating = stats.HasteRating,
                WeaponDamage = stats.WeaponDamage,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHaste = stats.PhysicalHaste,
                PhysicalHit = stats.PhysicalHit,
                SpellHit = stats.SpellHit,

                Healed = stats.Healed,
                HealthRestore = stats.HealthRestore,
                HealthRestoreFromMaxHealth = stats.HealthRestoreFromMaxHealth,
                Hp5 = stats.Hp5,

                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BonusCritDamageMultiplier = stats.BonusCritDamageMultiplier,
                BonusSpellCritDamageMultiplier = stats.BonusSpellCritDamageMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                BaseArmorMultiplier = stats.BaseArmorMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                DamageTakenReductionMultiplier = stats.DamageTakenReductionMultiplier,
                SpellDamageTakenReductionMultiplier = stats.SpellDamageTakenReductionMultiplier,
                PhysicalDamageTakenReductionMultiplier = stats.PhysicalDamageTakenReductionMultiplier,
                BossPhysicalDamageDealtReductionMultiplier = stats.BossPhysicalDamageDealtReductionMultiplier,
                BonusWhiteDamageMultiplier = stats.BonusWhiteDamageMultiplier,

                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,

                // General Damage Mods.
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                BonusDiseaseDamageMultiplier = stats.BonusDiseaseDamageMultiplier,

                // Ability mods.
                BonusDamageBloodStrike = stats.BonusDamageBloodStrike,
                BonusDamageDeathCoil = stats.BonusDamageDeathCoil,
                BonusDamageDeathStrike = stats.BonusDamageDeathStrike,
                BonusDamageFrostStrike = stats.BonusDamageFrostStrike,
                BonusDamageHeartStrike = stats.BonusDamageHeartStrike,
                BonusDamageIcyTouch = stats.BonusDamageIcyTouch,
                BonusDamageObliterate = stats.BonusDamageObliterate,
                BonusDamageScourgeStrike = stats.BonusDamageScourgeStrike,
                BonusFrostWeaponDamage = stats.BonusFrostWeaponDamage,

                BonusCritChanceDeathCoil = stats.BonusCritChanceDeathCoil,
                BonusCritChanceFrostStrike = stats.BonusCritChanceFrostStrike,
                BonusCritChanceObliterate = stats.BonusCritChanceObliterate,

                AntiMagicShellDamageReduction = stats.AntiMagicShellDamageReduction,

                BonusHealingReceived = stats.BonusHealingReceived,
                RPp5 = stats.RPp5,

                // Resistances
                ArcaneResistance = stats.ArcaneResistance,
                FireResistance = stats.FireResistance,
                FrostResistance = stats.FrostResistance,
                NatureResistance = stats.NatureResistance,
                ShadowResistance = stats.ShadowResistance,
                ArcaneResistanceBuff = stats.ArcaneResistanceBuff,
                FireResistanceBuff = stats.FireResistanceBuff,
                FrostResistanceBuff = stats.FrostResistanceBuff,
                NatureResistanceBuff = stats.NatureResistanceBuff,
                ShadowResistanceBuff = stats.ShadowResistanceBuff,

                // Damage Procs
                ArcaneDamage = stats.ArcaneDamage,
                FireDamage = stats.FireDamage,
                FrostDamage = stats.FrostDamage,
                ShadowDamage = stats.ShadowDamage,
                HolyDamage = stats.HolyDamage,
                NatureDamage = stats.NatureDamage,

                SnareRootDurReduc = stats.SnareRootDurReduc,
                FearDurReduc = stats.FearDurReduc,
                StunDurReduc = stats.StunDurReduc,
                MovementSpeed = stats.MovementSpeed,
                TargetArmorReduction = stats.TargetArmorReduction,
                BossAttackSpeedReductionMultiplier = stats.BossAttackSpeedReductionMultiplier,
            };

            // Also bringing in the trigger events from DPSDK - 
            // Since I'm going to move the +Def bonus for the Sigil of the Unfaltering Knight
            // To a special effect.  Also there are alot of OnUse and OnEquip special effects
            // That probably aren't being taken into effect.
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (HasRelevantStats(effect.Stats))
                {
                    s.AddSpecialEffect(effect);
                }
            }
            return s;
        }

        private StatsDK GetRelevantStatsLocal(Stats stats)
        {
            StatsDK s = new StatsDK();
            s.Accumulate(GetRelevantStats(stats));
            return s;
        }

        public override bool IsBuffRelevant(Buff buff, Character character)
        {
            return base.IsBuffRelevant(buff, character);
        }

        public List<Trigger> SuperRelevantTriggers {
            get {
                if (_superRelevantTriggers == null) {
                    _superRelevantTriggers = new List<Trigger>() {
                        // DK Specific
                        Trigger.BloodStrikeHit,
                        Trigger.DeathRuneGained,
                        Trigger.DeathStrikeHit,
                        Trigger.HeartStrikeHit,
                        Trigger.IcyTouchHit,
                        Trigger.ObliterateHit,
                        Trigger.PlagueStrikeHit,
                        Trigger.RuneStrikeHit,
                        Trigger.ScourgeStrikeHit,
                    };
                }
                return _superRelevantTriggers;
            }
        }
        private List<Trigger> _superRelevantTriggers = null;
        public List<Trigger> RelevantTriggers {
            get {
                if (_relevantTriggers == null) {
                    _relevantTriggers = new List<Trigger>() {
                        // Generic
                        Trigger.DamageDone, Trigger.DamageOrHealingDone, 
                        Trigger.DamageTaken, Trigger.DamageTakenMagical, Trigger.DamageTakenPhysical, Trigger.DamageTakenPutsMeBelow35PercHealth,
                        Trigger.DamageSpellCast, Trigger.DamageSpellCrit, Trigger.DamageSpellHit,
                        Trigger.SpellCast, Trigger.SpellCrit, Trigger.SpellHit,
                        Trigger.DoTTick,
                        Trigger.MeleeCrit, Trigger.MeleeHit, Trigger.PhysicalCrit, Trigger.PhysicalHit, Trigger.MeleeAttack, Trigger.PhysicalAttack,
                        Trigger.MainHandHit, Trigger.OffHandHit, Trigger.CurrentHandHit,
                        Trigger.Use,
                    };
                }
                return _relevantTriggers;
            }
        }
        private List<Trigger> _relevantTriggers = null;

        /// <summary>Tests whether there are positive relevant stats in the Stats object.</summary>
        /// <param name="stats">The complete Stats object containing all stats.</param>
        /// <returns>True if any of the non-Zero stats in the Stats are relevant.  
        /// I realize that there aren't many stats that have negative values, but for completeness.</returns>
        public override bool HasRelevantStats(Stats stats) {
            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (SuperRelevantTriggers.Contains(effect.Trigger)) {
                    // if it has a DK specific trigger, then just return true
                    return true;
                } else if (RelevantTriggers.Contains(effect.Trigger)) {
                    return relevantStats(effect.Stats);
                }
            }
            return relevantStats(stats);
        }

        private bool HasSuperRelevantStats(Stats stats) {
            if (stats.Strength != 0) return true;
            if (stats.BonusStrengthMultiplier != 0) return true;
            if (stats.DodgeRating != 0) return true;
            if (stats.ParryRating != 0) return true;
            if (stats.Dodge != 0) return true;
            if (stats.Parry != 0) return true;
            if (stats.Miss != 0) return true;
            //
            return false;
        }
        private bool HasWantedStats(Stats stats) {
            // Core
//            if (stats.Agility != 0) return true;
            if (stats.Stamina != 0) return true;
            if (stats.Armor != 0) return true;
            if (stats.BonusArmor != 0) return true;
            if (stats.Health != 0) return true;
            if (stats.BattlemasterHealthProc != 0) return true;
            if (stats.MasteryRating != 0) return true;
            // Special
            if (stats.HighestStat != 0) return true;
            if (stats.Paragon != 0) return true;
            // Absorb
            if (stats.DamageAbsorbed != 0) return true;
            // Offensive Stats
            if (stats.AttackPower != 0) return true;
            if (stats.HitRating != 0) return true;
            if (stats.CritRating != 0) return true;
            if (stats.ExpertiseRating != 0) return true;
            if (stats.Expertise != 0) return true;
            if (stats.HasteRating != 0) return true;
            if (stats.WeaponDamage != 0) return true;
            if (stats.PhysicalCrit != 0) return true;
            if (stats.PhysicalHaste != 0) return true;
            if (stats.PhysicalHit != 0) return true;
            if (stats.SpellHit != 0) return true;
            // Health Restoration
            if (stats.Healed != 0) return true;
            if (stats.HealthRestore != 0) return true;
            if (stats.HealthRestoreFromMaxHealth != 0) return true;
            if (stats.Hp5 != 0) return true;
            // Bonus Multipliers
            if (stats.BaseArmorMultiplier != 0) return true;
            if (stats.BonusArmorMultiplier != 0) return true;
            if (stats.BonusHealthMultiplier != 0) return true;
            if (stats.BonusStaminaMultiplier != 0) return true;
//            if (stats.BonusAgilityMultiplier != 0) return true;
            if (stats.BonusCritDamageMultiplier != 0) return true;
            if (stats.BonusSpellCritDamageMultiplier != 0) return true;
            if (stats.BonusAttackPowerMultiplier != 0) return true;
            if (stats.BonusPhysicalDamageMultiplier != 0) return true;
            if (stats.BonusDamageMultiplier != 0) return true;
            if (stats.ThreatIncreaseMultiplier != 0) return true;
            if (stats.BonusWhiteDamageMultiplier != 0) return true;
            if (stats.BonusShadowDamageMultiplier != 0) return true;
            if (stats.BonusFrostDamageMultiplier != 0) return true;
            if (stats.BonusDiseaseDamageMultiplier != 0) return true;
            // Inverse Multipliers
            if (stats.ThreatReductionMultiplier != 0) return true;
            if (stats.DamageTakenReductionMultiplier != 0) return true;
            if (stats.SpellDamageTakenReductionMultiplier != 0) return true;
            if (stats.PhysicalDamageTakenReductionMultiplier != 0) return true;
            if (stats.BossPhysicalDamageDealtReductionMultiplier != 0) return true;
            if (stats.AntiMagicShellDamageReduction != 0) return true;
            // Raw Bonus
            if (stats.BonusDamageBloodStrike != 0) return true;
            if (stats.BonusDamageDeathCoil != 0) return true;
            if (stats.BonusDamageDeathStrike != 0) return true;
            if (stats.BonusDamageFrostStrike != 0) return true;
            if (stats.BonusDamageHeartStrike != 0) return true;
            if (stats.BonusDamageIcyTouch != 0) return true;
            if (stats.BonusDamageObliterate != 0) return true;
            if (stats.BonusDamageScourgeStrike != 0) return true;
            if (stats.BonusFrostWeaponDamage != 0) return true;
            if (stats.BonusHealingReceived != 0) return true;
            if (stats.ArcaneDamage != 0) return true;
            if (stats.FireDamage != 0) return true;
            if (stats.FrostDamage != 0) return true;
            if (stats.ShadowDamage != 0) return true;
            if (stats.HolyDamage != 0) return true;
            if (stats.NatureDamage != 0) return true;
            // Increased Crit by Spell
            if (stats.BonusCritChanceDeathCoil != 0) return true;
            if (stats.BonusCritChanceFrostStrike != 0) return true;
            if (stats.BonusCritChanceObliterate != 0) return true;
            // Resistance
            if (stats.ArcaneResistance != 0) return true;
            if (stats.FireResistance != 0) return true;
            if (stats.FrostResistance != 0) return true;
            if (stats.NatureResistance != 0) return true;
            if (stats.ShadowResistance != 0) return true;
            if (stats.ArcaneResistanceBuff != 0) return true;
            if (stats.FireResistanceBuff != 0) return true;
            if (stats.FrostResistanceBuff != 0) return true;
            if (stats.NatureResistanceBuff != 0) return true;
            if (stats.ShadowResistanceBuff != 0) return true;
            // Boss Handler
            if (stats.SnareRootDurReduc != 0) return true;
            if (stats.FearDurReduc != 0) return true;
            if (stats.StunDurReduc != 0) return true;
            if (stats.MovementSpeed != 0) return true;
            if (stats.TargetArmorReduction != 0) return true;
            if (stats.BossAttackSpeedReductionMultiplier != 0) return true;
            // Other
            if (stats.RPp5 != 0) return true;
            //
            return false;
        }
        private bool HasIgnoreStats(Stats stats)
        {
            if (stats.Intellect != 0) return true;
//            if (stats.BonusIntellectMultiplier != 0) return true;
            if (stats.Spirit != 0) return true;
//            if (stats.BonusSpiritMultiplier != 0) return true;
            if (stats.Mana != 0) return true;
//            if (stats.BonusManaMultiplier != 0) return true;
            if (stats.Mp5 != 0) return true;
            if (stats.ManaRestore != 0) return true;
            if (stats.SpellPenetration != 0) return true;
            //
            return false;
        }

        /// <summary>Helper function for HasRelevantStats() function of the base class.</summary>
        /// <param name="stats"></param>
        /// <returns>true == the stats object has interesting things for this model.</returns>
        private bool relevantStats(Stats stats)
        {
            return HasSuperRelevantStats(stats)
                || (HasWantedStats(stats) && !HasIgnoreStats(stats));
        }

        public override bool IsItemRelevant(Item item) 
        {
            if (item.Slot == ItemSlot.Ranged && (item.Type != ItemType.Sigil && item.Type != ItemType.Relic)) { return false; }
            return base.IsItemRelevant(item);
        }
        #endregion

        #region Model Specific Variables, Properties and Functions
        public static int HitResultCount = EnumHelper.GetCount(typeof(HitResult));

        #region Static SpecialEffects
        private static readonly SpecialEffect _SE_FC1 = new SpecialEffect(Trigger.DamageDone, new Stats() { BonusStrengthMultiplier = .15f }, 15f, 0f, -2f, 1);
        private static readonly SpecialEffect _SE_FC2 = new SpecialEffect(Trigger.DamageDone, new Stats() { HealthRestoreFromMaxHealth = .03f }, 0, 0f, -2f, 1);
        private static readonly SpecialEffect[] _SE_IBF = new SpecialEffect[] {
            new SpecialEffect(Trigger.Use, new Stats() { StunDurReduc = 1f, DamageTakenReductionMultiplier = 0.20f }, 12 * 1.0f, 3 * 60  ), // Default IBF
            new SpecialEffect(Trigger.Use, new Stats() { StunDurReduc = 1f, DamageTakenReductionMultiplier = 0.20f }, 12 * 1.5f, 3 * 60  ), // IBF w/ 4T11
        };
        private static readonly SpecialEffect _SE_AntiMagicZone = new SpecialEffect(Trigger.Use, new Stats() { SpellDamageTakenReductionMultiplier = 0.75f }, 10f, 2f * 60f);
        private static readonly SpecialEffect _SE_RuneTap = new SpecialEffect(Trigger.Use, new Stats() { HealthRestoreFromMaxHealth = .1f }, 0, 30f);
        private static readonly SpecialEffect _SE_DeathPact = new SpecialEffect(Trigger.Use, new Stats() { HealthRestoreFromMaxHealth = .25f }, 0, 60 * 2f);
        #endregion

        private float SoftCapSurvival(TankDKChar TDK, float attackValue, float origValue)
        {
            return SoftCapSurvival(TDK, attackValue, origValue, false);
        }

        private float SoftCapSurvival(TankDKChar TDK, float attackValue, float origValue,  bool bIgnoreSoftCap)
        {
            float cappedValue = origValue;
            //
            double survivalCap = ((double)attackValue * (double)TDK.calcOpts.HitsToSurvive) / 1000d;
            if (bIgnoreSoftCap)
                survivalCap = ((double)attackValue * ((double)TDK.calcOpts.HitsToSurvive * 1.077)) / 1000d;
            double survivalRaw = origValue / 1000f;

            //Implement Survival Soft Cap
            if (survivalRaw <= survivalCap) 
            {
                cappedValue = 1000f * (float)survivalRaw;
            }
            else 
            {
                double x = survivalRaw;
                double cap = survivalCap;
                double fourToTheNegativeFourThirds = Math.Pow(4d, -4d / 3d);
                double topLeft = Math.Pow(((x - cap) / cap) + fourToTheNegativeFourThirds, 1d / 4d);
                double topRight = Math.Pow(fourToTheNegativeFourThirds, 1d / 4d);
                double fracTop = topLeft - topRight;
                double fraction = fracTop / 2d;
                double y = (cap * fraction + cap);
                cappedValue = 1000f * (float)y;
            }
            return cappedValue;
        }

        private float[] GetSurvival(ref TankDKChar TDK, StatsDK stats, float[] DamagePercentages, float ArmorDamageReduction, float fMagicDR)
        {
            // For right now Survival Rating == Effective Health will be HP + Armor/Resistance mitigation values.
            // Everything else is really mitigating damage based on RNG.

            // The health bonus from Frost presence is now include in the character by default.
            float fPhysicalSurvival = stats.Health;
            float fBleedSurvival = stats.Health;
            float fMagicalSurvival = stats.Health;

            // Physical damage:
            fPhysicalSurvival = GetEffectiveHealth(stats.Health, ArmorDamageReduction, DamagePercentages[(int)SurvivalSub.Physical]);

            // Bleed damage:
            fBleedSurvival    = GetEffectiveHealth(stats.Health, 0, DamagePercentages[(int)SurvivalSub.Bleed]);
            
            // Magic Dam:
            fMagicalSurvival  = GetEffectiveHealth(stats.Health, fMagicDR, DamagePercentages[(int)SurvivalSub.Magic]);

            // Since Armor plays a role in Survival, so shall the other damage taken adjusters.
            // Note, it's expected that (at least for tanks) that DamageTakenMultiplier will be Negative.
            // JOTHAY NOTE: The above is no longer true. DamageTakenReductionMultiplier will now be positive, but must be handled multiplicatively and inversely
            // JOTHAY NOTE: YOUR FORMULAS HAVE BEEN UPDATED TO REFLECT THIS CHANGE
            // So the next line should INCREASE survival because 
            // fPhysicalSurvival * (1 - [some negative value] * (1 - [0 or some negative value])
            // will look like:
            // fPhysicalSurvival * 1.xx * 1.xx
            fPhysicalSurvival       = fPhysicalSurvival / ((1f - stats.DamageTakenReductionMultiplier) * (1f - stats.PhysicalDamageTakenReductionMultiplier));
            fBleedSurvival          = fBleedSurvival    / ((1f - stats.DamageTakenReductionMultiplier) * (1f - stats.PhysicalDamageTakenReductionMultiplier));
            fMagicalSurvival        = fMagicalSurvival  / ((1f - stats.DamageTakenReductionMultiplier) * (1f - stats.SpellDamageTakenReductionMultiplier   ));
            float[] SurvivalResults = new float[EnumHelper.GetCount(typeof(SurvivalSub))];
            SurvivalResults[(int)SurvivalSub.Physical] = fPhysicalSurvival;
            SurvivalResults[(int)SurvivalSub.Bleed] = fBleedSurvival;
            SurvivalResults[(int)SurvivalSub.Magic] = fMagicalSurvival;
            return SurvivalResults;
        }

        private float[] GetMitigation(ref TankDKChar TDK, StatsDK stats, Rotation rot, float fPercentCritMitigation, float ArmorDamageReduction, float[] fCurrentDTPS, float fMagicDR)
        {
            return GetMitigation(ref TDK, stats, rot, fPercentCritMitigation, ArmorDamageReduction, fCurrentDTPS, fMagicDR, true);
        }

        private float[] GetMitigation(ref TankDKChar TDK, StatsDK stats, Rotation rot, float fPercentCritMitigation, float ArmorDamageReduction, float[] fCurrentDTPS, float fMagicDR, bool bFactorInAvoidance)
        {
            float[] fTotalMitigation = new float[EnumHelper.GetCount(typeof(MitigationSub))];
            // Ensure the CurrentDTPS structure is the right size.
            if (fCurrentDTPS.Length < EnumHelper.GetCount(typeof(SurvivalSub)))
            {
                fCurrentDTPS = new float[EnumHelper.GetCount(typeof(SurvivalSub))];
            }

            float fSegmentMitigation = 0f;
            float fSegmentDPS = 0f;
            float fPhyDamageDPS = fCurrentDTPS[(int)SurvivalSub.Physical];
            float fMagicDamageDPS = fCurrentDTPS[(int)SurvivalSub.Magic];
            float fBleedDamageDPS = fCurrentDTPS[(int)SurvivalSub.Bleed];
            #region *** Dam Avoided (Crit, Haste, Avoidance) ***
            #region ** Crit Mitigation **
            // Crit mitigation:
            // Crit mitigation work for Physical damage only.
            float fCritMultiplier = .12f;
            // Bleeds can't crit.
            // Neither can spells from bosses.  (As per a Loading screen ToolTip.)
            float fCritDPS = fPhyDamageDPS * fCritMultiplier;
            fSegmentMitigation = (fCritDPS * fPercentCritMitigation);
            // Add in the value of crit mitigation.
            fTotalMitigation[(int)MitigationSub.Crit] = fSegmentMitigation;
            // The max damage at this point needs to include crit.
            fCurrentDTPS[(int)SurvivalSub.Physical] = fCurrentDTPS[(int)SurvivalSub.Physical] + (fCritDPS - fSegmentMitigation);
            #endregion
            #region ** Haste Mitigation **
            // Placeholder for comparing differing DPS values related to haste.
            float fNewIncPhysDPS = 0;
            // Let's just look at Imp Icy Touch 
            #region Improved Icy Touch
            // Get the new slowed AttackSpeed based on ImpIcyTouch
            // Factor in the base slow caused by FF (14% base).
            float fBossAttackSpeedReduction = 0.0f;

            if (rot.Contains(DKability.IcyTouch)
                || rot.Contains(DKability.FrostFever))
            {
                fBossAttackSpeedReduction = 0.2f;
            }
            else if (stats.BossAttackSpeedReductionMultiplier > 0) // FF provided by someone else.
            {
                fBossAttackSpeedReduction = stats.BossAttackSpeedReductionMultiplier;
            }
            // Figure out what the new Physical DPS should be based on that.
            fSegmentDPS = TDK.bo.GetDPSByType(ATTACK_TYPES.AT_MELEE, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            fNewIncPhysDPS = fSegmentDPS / (1 + fBossAttackSpeedReduction);
            // Send the difference to the Mitigation value.
            fSegmentMitigation = fSegmentDPS - fNewIncPhysDPS;
            fTotalMitigation[(int)MitigationSub.Haste] += fSegmentMitigation;
            fCurrentDTPS[(int)SurvivalSub.Physical] -= fSegmentMitigation;
            #endregion

            #endregion
            #region ** Avoidance Mitigation **
            // Let's see how much damage was avoided.
            // Raise the total mitgation by that amount.
            if (bFactorInAvoidance)
            {
                fSegmentDPS = fNewIncPhysDPS;
                fNewIncPhysDPS = TDK.bo.GetDPSByType(ATTACK_TYPES.AT_MELEE, 0, 0, 0, fBossAttackSpeedReduction, stats.Miss, stats.Dodge, stats.EffectiveParry, 0, 0, 0, 0, 0, 0, 0);
                fSegmentMitigation = fSegmentDPS - fNewIncPhysDPS;
                fTotalMitigation[(int)MitigationSub.Avoidance] += fSegmentMitigation;
                fCurrentDTPS[(int)SurvivalSub.Physical] -= fSegmentMitigation;
            }
            #endregion
            #endregion

            #region *** Dam Reduced (AMS, Armor, Magic Resist, DamageTaken Modifiers) ***
            #region ** Anti-Magic Shell **
            // TODO: This is a CD, so would only be in BURST.
            // Anti-Magic Shell. ////////////////////////////////////////////////////////
            // Talent: MagicSuppression increases AMS by 8/16/25% per point.
            // Glyph: GlyphofAntiMagicShell increases AMS by 2 sec.
            // AMS has a 45 sec CD.
            float amsDuration = (5f + (TDK.Char.DeathKnightTalents.GlyphofAntiMagicShell == true ? 2f : 0f)) * (1 + stats.DefensiveCooldownDurationMultiplier);
            float amsUptimePct = amsDuration / 45f;
            // AMS reduces damage taken by 75% up to a max of 50% health.
            float amsReduction = 0.75f * (1f + (TDK.Char.DeathKnightTalents.MagicSuppression * .25f / 3));
            float amsReductionMax = stats.Health * 0.5f * (1 + stats.DefensiveCooldownReductionMultiplier);
            // up to 50% of health means that the amdDRvalue equates to the raw damage points removed.  
            // This means that toon health and INC damage values from the options pane are going to affect this quite a bit.
            float amsDRvalue = (Math.Min(amsReductionMax, (fMagicDamageDPS * amsDuration) * amsReduction) * amsUptimePct);
            // Raise the TotalMitigation by that amount.
            fCurrentDTPS[(int)SurvivalSub.Magic] -= amsDRvalue;
            fTotalMitigation[(int)MitigationSub.AMS] += amsDRvalue;
            #endregion
            #region ** Armor Dam Mitigation **
            // For any physical only damage reductions. 
            // Factor in armor Dam Reduction
            fSegmentMitigation = fPhyDamageDPS * ArmorDamageReduction;
//            calcs.ArmorMitigation = fSegmentMitigation;
            fTotalMitigation[(int)MitigationSub.Armor] += fSegmentMitigation;
            fCurrentDTPS[(int)SurvivalSub.Physical] *= 1f - ArmorDamageReduction;
            #endregion
            #region ** Resistance Dam Mitigation **
            // For any physical only damage reductions. 
            // Factor in armor Dam Reduction
            fSegmentMitigation = fMagicDamageDPS * fMagicDR;
            fTotalMitigation[(int)MitigationSub.Magic] += fSegmentMitigation;
            fCurrentDTPS[(int)SurvivalSub.Magic] -= fCurrentDTPS[(int)SurvivalSub.Magic] * fMagicDR;
            #endregion
            #region ** Dam Taken Mitigation **
            fTotalMitigation[(int)MitigationSub.DamageReduction] += fMagicDamageDPS * (1f - stats.DamageTakenReductionMultiplier) * (1f - stats.SpellDamageTakenReductionMultiplier   );
            fTotalMitigation[(int)MitigationSub.DamageReduction] += fBleedDamageDPS * (1f - stats.DamageTakenReductionMultiplier) * (1f - stats.PhysicalDamageTakenReductionMultiplier);
            fTotalMitigation[(int)MitigationSub.DamageReduction] += fPhyDamageDPS   * (1f - stats.DamageTakenReductionMultiplier) * (1f - stats.PhysicalDamageTakenReductionMultiplier);
            
            fCurrentDTPS[(int)SurvivalSub.Magic]    *= (1f - stats.DamageTakenReductionMultiplier) * (1f - stats.SpellDamageTakenReductionMultiplier   );
            fCurrentDTPS[(int)SurvivalSub.Bleed]    *= (1f - stats.DamageTakenReductionMultiplier) * (1f - stats.PhysicalDamageTakenReductionMultiplier);
            fCurrentDTPS[(int)SurvivalSub.Physical] *= (1f - stats.DamageTakenReductionMultiplier) * (1f - stats.PhysicalDamageTakenReductionMultiplier);
            #endregion
            #region ** Dam Absorbed ** 
            fTotalMitigation[(int)MitigationSub.DamageReduction] += stats.DamageAbsorbed;

//            fCurrentDTPS[(int)SurvivalSub.Magic] *= (1f - stats.DamageTakenReductionMultiplier) * (1f - stats.SpellDamageTakenReductionMultiplier);
//            fCurrentDTPS[(int)SurvivalSub.Bleed] *= (1f - stats.DamageTakenReductionMultiplier) * (1f - stats.PhysicalDamageTakenReductionMultiplier);
//            fCurrentDTPS[(int)SurvivalSub.Physical] *= (1f - stats.DamageTakenReductionMultiplier) * (1f - stats.PhysicalDamageTakenReductionMultiplier);

            #endregion
            #endregion

            #region ** Boss Handler Mitigation (Impedences) **
            float TotalDuration = 0;
            float ImprovedDuration = 0;
            float ImpedenceMitigation = 0;
            #region ** Fear Based Mitigation (fear reduction) **
            if (TDK.bo.Fears.Count > 0)
            {
                foreach (Impedance m in TDK.bo.Fears)
                {
                    TotalDuration += m.Duration;
                }
                ImprovedDuration = TotalDuration / (1 + stats.FearDurReduc);
                fSegmentMitigation = (TotalDuration - ImprovedDuration) /* Add some multiplier to this */;
                ImpedenceMitigation += fSegmentMitigation;
            }
            #endregion

            #region ** Movement Based Mitigation (run speed) **
            if (TDK.bo.Moves.Count > 0)
            {
                TotalDuration = 0;
                foreach (Impedance m in TDK.bo.Moves)
                {
                    TotalDuration += m.Duration;
                }
                ImprovedDuration = TotalDuration / (1 + stats.MovementSpeed);
                fSegmentMitigation = (TotalDuration - ImprovedDuration) /* Add some multiplier to this */;
                ImpedenceMitigation += fSegmentMitigation;
            }
            #endregion

            #region ** Disarm Based Mitigation (Disarm reduction) **
            if (TDK.bo.Disarms.Count > 0)
            {
                TotalDuration = 0;
                foreach (Impedance m in TDK.bo.Disarms)
                {
                    TotalDuration += m.Duration;
                }
                ImprovedDuration = TotalDuration / (1 + stats.DisarmDurReduc);
                fSegmentMitigation = (TotalDuration - ImprovedDuration) /* Add some multiplier to this */;
                ImpedenceMitigation += fSegmentMitigation;
            }
            #endregion

            #region ** Stun Based Mitigation (stun reduction) **
            if (TDK.bo.Stuns.Count > 0)
            {
                TotalDuration = 0;
                foreach (Impedance m in TDK.bo.Stuns)
                {
                    TotalDuration += m.Duration;
                }
                ImprovedDuration = TotalDuration / (1 + stats.StunDurReduc);
                fSegmentMitigation = (TotalDuration - ImprovedDuration) /* Add some multiplier to this */;
                ImpedenceMitigation += fSegmentMitigation;
            }
            #endregion
            // calcs.ImpedenceMitigation = ImpedenceMitigation;
            fTotalMitigation[(int)MitigationSub.Impedences] += ImpedenceMitigation;
            #endregion

            #region ** Heals not from DS **
            fSegmentMitigation = StatConversion.ApplyMultiplier(stats.Healed, stats.HealingReceivedMultiplier);
            fSegmentMitigation += (StatConversion.ApplyMultiplier(stats.Hp5, stats.HealingReceivedMultiplier) / 5);
            fSegmentMitigation += StatConversion.ApplyMultiplier(stats.HealthRestore, stats.HealingReceivedMultiplier);
            // Health Returned by other sources than DS:
            if (stats.HealthRestoreFromMaxHealth > 0)
                fSegmentMitigation += (stats.HealthRestoreFromMaxHealth * stats.Health);
            fTotalMitigation[(int)MitigationSub.Heals] = fSegmentMitigation;
            #endregion

            return fTotalMitigation;
        }

        /// <summary>Get the value for a sub-component of Survival</summary>
        /// <param name="fHealth">Current HP</param>
        /// <param name="fDR">Dam Reduction rate</param>
        /// <param name="PercValue">% value of the survival rank. valid range 0-1</param>
        /// <returns></returns>
        private float GetEffectiveHealth(float fHealth, float fDR, float PercValue)
        {
            // TotalSurvival == sum(Survival for each school)
            // Survival = (Health / (1 - DR)) * % damage inc from that school
            if (0f <= PercValue && PercValue <= 1f && fDR < 1f)
                return (fHealth / (1 - fDR)) * PercValue;
            else
                return 0;
        }

        /// <summary>
        /// Get Dam Per Second
        /// </summary>
        /// <param name="fPerUnitDamage">Dam per incident</param>
        /// <param name="fDamFrequency">Dam Frequency in Seconds</param>
        /// <returns></returns>
        private float GetDPS(float fPerUnitDamage, float fDamFrequency)
        {
            if (fDamFrequency > 0)
                return fPerUnitDamage / fDamFrequency;
            return 0f;
        }
        #endregion

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            #region Setup
            CharacterCalculationsTankDK basecalcs = new CharacterCalculationsTankDK();
            CharacterCalculationsTankDK calcs = new CharacterCalculationsTankDK();
            TankDKChar TDK = new TankDKChar();

            if (character == null) { return calcs; }
            TDK.calcOpts = character.CalculationOptions as CalculationOptionsTankDK;
            if (TDK.calcOpts == null) { return calcs; }
            TDK.Char = character;
            TDK.bo = character.BossOptions;
            // Make sure there is at least one attack in the list.  
            // If there's not, add a Default Melee Attack for processing  
            if (TDK.bo.Attacks.Count < 1)
            {
                TDK.Char.IsLoading = true;
                TDK.bo.DamagingTargs = true;
                TDK.bo.Attacks.Add(BossHandler.ADefaultMeleeAttack);
                TDK.Char.IsLoading = false;
            }
            // Make sure there is a default melee attack  
            // If the above processed, there will be one so this won't have to process  
            // If the above didn't process and there isn't one, add one now  
            if (TDK.bo.DefaultMeleeAttack == null)
            {
                TDK.Char.IsLoading = true;
                TDK.bo.DamagingTargs = true;
                TDK.bo.Attacks.Add(BossHandler.ADefaultMeleeAttack);
                TDK.Char.IsLoading = false;
            }
            // Since the above forced there to be an attack it's safe to do this without a null check  
            // Attack bossAttack = TDK.bo.DefaultMeleeAttack;
            #endregion

            #region Stats
            // Get base stats that will be used for paperdoll:
            StatsDK stats = GetCharacterStats(TDK.Char, additionalItem) as StatsDK;
            // validate that we get a stats object;
            if (null == stats) { return calcs; }

            // This is the point that SHOULD have the right values according to the paper-doll.
            StatsDK sPaperDoll = stats.Clone() as StatsDK;
            #endregion

            #region Evaluation
            Rawr.DPSDK.CharacterCalculationsDPSDK DPSCalcs = new Rawr.DPSDK.CharacterCalculationsDPSDK();
            Rawr.DPSDK.CalculationOptionsDPSDK DPSopts = new Rawr.DPSDK.CalculationOptionsDPSDK();
            DPSopts.presence = Presence.Blood;
            DKCombatTable ct = new DKCombatTable(TDK.Char, stats, DPSCalcs, DPSopts, TDK.bo);
            Rotation rot = new Rotation(ct, true);
            rot.PRE_BloodDiseased();
            // Base calculation values.  This will give us Mitigation, and Survival w/ base stats.
            basecalcs = GetCharacterCalculations(TDK, stats, rot, false, needsDisplayCalculations);
            // Setup max values w/ everything turned on.
            stats = GetCharacterStats(TDK.Char, additionalItem, StatType.Maximum, TDK, rot);
            calcs.SEStats = stats.Clone() as StatsDK;
            ct = new DKCombatTable(TDK.Char, stats, DPSCalcs, DPSopts, TDK.bo);
            rot = new Rotation(ct, true);
            rot.PRE_BloodDiseased();

            calcs = GetCharacterCalculations(TDK, stats, rot, true, needsDisplayCalculations);

            #region Burst
            // Burst as On-Use Abilties.
            calcs.Burst = 0;
            calcs.BurstWeight = TDK.calcOpts.BurstWeight;
            if (calcs.BurstWeight > 0)
            {
                calcs.Burst += calcs.Survivability - basecalcs.Survivability;
                if (calcs.Burst < 0 || float.IsNaN(calcs.Burst)) { calcs.Burst = 0; } // This should never happen but just in case
                calcs.Burst += calcs.Mitigation - basecalcs.Mitigation;
                if (calcs.Burst < 0 || float.IsNaN(calcs.Burst)) { calcs.Burst = 0; } // This should never happen but just in case
                // Survival
                calcs.PhysicalSurvival = basecalcs.PhysicalSurvival;
                calcs.MagicSurvival = basecalcs.MagicSurvival;
                calcs.BleedSurvival = basecalcs.BleedSurvival;
                // Mitigation
                calcs.Mitigation = basecalcs.Mitigation;
            }
            #endregion

            #region **** Recovery: DS & Blood Shield ****
            float minDSHeal = stats.Health * .07f;
            // 4.1: DS Heals for 20% of Dam Taken over the last 5 secs.
            float DTPSFactor = calcs.DTPS * 5f;
            if (TDK.calcOpts.b_RecoveryInclAvoidance == false) 
            {
                DTPSFactor = calcs.DTPSNoAvoidance * 5f;
            }
            float DamDSHeal = (DTPSFactor * .20f) * (1 + .15f * TDK.Char.DeathKnightTalents.ImprovedDeathStrike); // IDS increases heals by .15 * level
            float DSHeal = Math.Max(minDSHeal, DamDSHeal);
            calcs.DSHeal = DSHeal;
            calcs.DSOverHeal = DSHeal * TDK.calcOpts.pOverHealing;
            calcs.DSCount = TDK.bo.BerserkTimer * rot.m_DSperSec;
            float BloodShield = (DSHeal * .5f) * (1 + (stats.Mastery * .0625f));
            calcs.BShield = BloodShield;
            // 4.3 Removing Hitchance for healing
            float DSHealsPSec = (DSHeal * rot.m_DSperSec * (1f - TDK.calcOpts.pOverHealing));
            calcs.TotalDShealed = DSHealsPSec * TDK.bo.BerserkTimer;
            float BShieldPSec = BloodShield * rot.m_DSperSec; // A new shield w/ each DS.
            calcs.TotalBShield = BShieldPSec * TDK.bo.BerserkTimer;
            calcs.Recovery = BloodShield + (DSHeal * (1f - TDK.calcOpts.pOverHealing));
            calcs.HPS += DSHealsPSec;
            calcs.DTPS -= BShieldPSec;
            calcs.RecoveryWeight = TDK.calcOpts.RecoveryWeight;
            #endregion

            #endregion

            #region Key Data Validation
            if (float.IsNaN(calcs.Threat) ||
                float.IsNaN(calcs.Survivability) ||
                float.IsNaN(calcs.Burst) ||
                float.IsNaN(calcs.Recovery) ||
                float.IsNaN(calcs.Mitigation) ||
                float.IsNaN(calcs.OverallPoints))
            {
#if DEBUG
                throw new Exception("One of the Subpoints are Invalid.");
#endif
            }
            #endregion

            #region Display only work
            calcs.Miss = sPaperDoll.Miss;
            calcs.Dodge = sPaperDoll.Dodge;
            calcs.Parry = sPaperDoll.Parry;

            calcs.BasicStats = sPaperDoll;
            calcs.SEStats = stats.Clone() as StatsDK;
            // The full character data.
            calcs.TargetLevel = TDK.bo.Level;

            if (null != rot.m_CT.MH)
            {
                calcs.TargetDodge = rot.m_CT.MH.chanceDodged;
                calcs.TargetMiss = rot.m_CT.MH.chanceMissed;
                calcs.TargetParry = rot.m_CT.MH.chanceParried;
                calcs.Expertise = rot.m_CT.m_Calcs.MHExpertise;
            }
            #endregion
            return calcs;
        }

        private CharacterCalculationsTankDK GetCharacterCalculations(TankDKChar TDK, StatsDK stats, Rotation rot, bool isBurstCalc, bool needsDisplayCalcs)
        {
            CharacterCalculationsTankDK calcs = new CharacterCalculationsTankDK();

            // Level differences.
            int iLevelDiff = Math.Max(TDK.bo.Level - TDK.Char.Level, 0);

            float fChanceToGetHit = 1f - Math.Min(1f, stats.Miss + stats.Dodge + stats.EffectiveParry);
            float ArmorDamageReduction = (float)StatConversion.GetArmorDamageReduction(TDK.bo.Level, stats.Armor, 0f, 0f);

            #region **** Setup Fight parameters ****

            // Get the values of each type of damage in %.
            // So first we get each type of damage in the same units: DPS.
            // Get the total DPS.

            float[] fCurrentDTPS = new float[3];
            fCurrentDTPS[(int)SurvivalSub.Physical] = 0f;
            fCurrentDTPS[(int)SurvivalSub.Bleed] = 0f;
            fCurrentDTPS[(int)SurvivalSub.Magic] = 0f;
            float[] fCurrentDmgBiggestHit = new float[3];
            fCurrentDmgBiggestHit[(int)SurvivalSub.Physical] = 0f;
            fCurrentDmgBiggestHit[(int)SurvivalSub.Bleed] = 0f;
            fCurrentDmgBiggestHit[(int)SurvivalSub.Magic] = 0f;
            float[] fCurrentDTPSPerc = new float[3];
            fCurrentDTPSPerc[(int)SurvivalSub.Physical] = 1f;
            fCurrentDTPSPerc[(int)SurvivalSub.Bleed] = 0f;
            fCurrentDTPSPerc[(int)SurvivalSub.Magic] = 0f;
            float fTotalDTPS = 0;

            float fAvoidanceTotal = 1f - fChanceToGetHit;

            // We want to start getting the Boss Handler stuff going on.
            // Setup initial Boss data.
            // How much of what kind of damage does this boss deal with?
            #region ** Incoming Boss Dam **
            // Let's make sure this is even valid
            float DPHit = 0;
            float DPTick = 0;
            switch (TDK.calcOpts.PlayerRole)
            {
                case 0:
                    TDK.role = PLAYER_ROLES.MainTank;
                    break;
                case 1:
                    TDK.role = PLAYER_ROLES.OffTank;
                    break;
                case 2:
                    TDK.role = PLAYER_ROLES.TertiaryTank;
                    break;
                default:
                    TDK.role = PLAYER_ROLES.MainTank;
                    break;
            }
            TDK.role = PLAYER_ROLES.MainTank;
            foreach (Attack a in TDK.bo.Attacks)
            {
                // PlayerRole on calcOpts is MT=0, OT=1, TT=2, Any Tank = 3
                // Any Tank means it should be affected by anything that affects a tanking role
                if (a.AffectsRole[PLAYER_ROLES.MainTank] && (TDK.calcOpts.PlayerRole == 0 || TDK.calcOpts.PlayerRole == 3)
                    || a.AffectsRole[PLAYER_ROLES.OffTank] && (TDK.calcOpts.PlayerRole == 1 || TDK.calcOpts.PlayerRole == 3)
                    || a.AffectsRole[PLAYER_ROLES.TertiaryTank] && (TDK.calcOpts.PlayerRole == 2 || TDK.calcOpts.PlayerRole == 3))
                {
                    // TODO: Figure out a way to get the phase changes handled.
                    DPHit = a.DamagePerHit;
                    DPTick = a.DamagePerTick;
                    if (a.DamageIsPerc)
                    {
#if DEBUG
                        if ((a.DamagePerHit >= 1f) || (a.DamagePerTick >= 1f))
                            throw new Exception("Percentage Damage is >= 100%.");
#endif
                        DPHit = a.DamagePerHit * stats.Health;
                        DPTick = a.DamagePerTick * stats.Health;
                    }

                    // Bleeds vs Magic vs Physical
                    if (a.DamageType == ItemDamageType.Physical) 
                    {
                        // Bleed or Physical
                        // Need to figure out how to determine bleed vs. physical hits.
                        // Also need to balance out the physical hits and balance the hit rate.
                        // JOTHAY NOTE: Bleeds are DoTs
                        if (a.IsDoT) {
                            fCurrentDTPS[(int)SurvivalSub.Bleed] += GetDPS(DPHit, a.AttackSpeed) + GetDPS(DPTick, a.TickInterval);
                            if (fCurrentDmgBiggestHit[(int)SurvivalSub.Bleed] < DPHit + (DPTick * a.NumTicks))
                                fCurrentDmgBiggestHit[(int)SurvivalSub.Bleed] = DPHit + (DPTick * a.NumTicks);
                        } 
                        else 
                        {
                            fCurrentDTPS[(int)SurvivalSub.Physical] += GetDPS(DPHit, a.AttackSpeed);
                            if (fCurrentDmgBiggestHit[(int)SurvivalSub.Physical] < DPHit)
                                fCurrentDmgBiggestHit[(int)SurvivalSub.Physical] = DPHit;
                        }
                    } 
                    else 
                    {
                        // Magic now covering magical dots.
                        fCurrentDTPS[(int)SurvivalSub.Magic] += GetDPS(DPHit, a.AttackSpeed) + GetDPS(DPTick, a.TickInterval);
                        if (fCurrentDmgBiggestHit[(int)SurvivalSub.Magic] < DPHit + (DPTick * a.NumTicks))
                            fCurrentDmgBiggestHit[(int)SurvivalSub.Magic] = DPHit + (DPTick * a.NumTicks);
                    }
                }
            }
            fTotalDTPS += fCurrentDTPS[(int)SurvivalSub.Physical];
            fTotalDTPS += fCurrentDTPS[(int)SurvivalSub.Bleed];
            fTotalDTPS += fCurrentDTPS[(int)SurvivalSub.Magic];

            if (fTotalDTPS > 0)
            {
                fCurrentDTPSPerc[(int)SurvivalSub.Physical] = fCurrentDTPS[(int)SurvivalSub.Physical] / fTotalDTPS;
                fCurrentDTPSPerc[(int)SurvivalSub.Bleed]    = fCurrentDTPS[(int)SurvivalSub.Bleed] / fTotalDTPS;
                fCurrentDTPSPerc[(int)SurvivalSub.Magic]    = fCurrentDTPS[(int)SurvivalSub.Magic] / fTotalDTPS;
            }
            #endregion

            // Set the Fight Duration to no larger than the Berserk Timer
            // Question: What is the units for Berserk & Speed Timer? MS/S/M?
            #endregion

            #region ***** Survival Rating *****
            // Magical damage:
            // if there is a max resistance, then it's likely they are stacking for that resistance.  So factor in that Max resistance.
            float fMaxResist = Math.Max(stats.ArcaneResistance, stats.FireResistance);
            fMaxResist       = Math.Max(fMaxResist, stats.FrostResistance);
            fMaxResist       = Math.Max(fMaxResist, stats.NatureResistance);
            fMaxResist       = Math.Max(fMaxResist, stats.ShadowResistance);

            float fMagicDR = StatConversion.GetAverageResistance(TDK.bo.Level, TDK.Char.Level, fMaxResist, 0f);
            calcs.MagicDamageReduction = fMagicDR;

            float[] SurvivalResults = new float [EnumHelper.GetCount(typeof(SurvivalSub))];
            SurvivalResults = GetSurvival(ref TDK, stats, fCurrentDTPSPerc, ArmorDamageReduction, fMagicDR);

            calcs.ArmorDamageReduction   = ArmorDamageReduction;
            calcs.PhysicalSurvival       = SoftCapSurvival(TDK, fCurrentDmgBiggestHit[(int)SurvivalSub.Physical], SurvivalResults[(int)SurvivalSub.Physical], isBurstCalc);
            calcs.BleedSurvival          = SoftCapSurvival(TDK, fCurrentDmgBiggestHit[(int)SurvivalSub.Bleed],    SurvivalResults[(int)SurvivalSub.Bleed], isBurstCalc);
            calcs.MagicSurvival          = SoftCapSurvival(TDK, fCurrentDmgBiggestHit[(int)SurvivalSub.Magic],    SurvivalResults[(int)SurvivalSub.Magic], isBurstCalc);
            calcs.HitsToSurvive          = TDK.calcOpts.HitsToSurvive;
            #endregion

            #region ***** Threat Rating *****
            rot.TotalDamage += (int)(stats.FireDamage * (1 + stats.BonusFireDamageMultiplier) * rot.CurRotationDuration);
            rot.TotalThreat += (int)(stats.FireDamage * (1 + stats.BonusFireDamageMultiplier) * rot.CurRotationDuration) * 2;
            calcs.RotationTime = rot.CurRotationDuration; // Display the rot in secs.
            calcs.Threat = rot.m_TPS;
            calcs.DPS = rot.m_DPS;
            calcs.Blood = rot.m_BloodRunes;
            calcs.Frost = rot.m_FrostRunes;
            calcs.Unholy = rot.m_UnholyRunes;
            calcs.Death = rot.m_DeathRunes;
            calcs.RP = rot.m_RunicPower;
            calcs.TotalThreat = (int)rot.TotalThreat;

            calcs.ThreatWeight = TDK.calcOpts.ThreatWeight;
            if (needsDisplayCalcs)
            {
                TDK.calcOpts.szRotReport = rot.ReportRotation();
            }
            #endregion

            #region ***** Mitigation Rating *****
            float[] fCurrentDTPSNoAvoid = new float[3];
            fCurrentDTPSNoAvoid = fCurrentDTPS.Clone() as float[];
            float[] fCurrentMitigation = GetMitigation(ref TDK, stats, rot, (stats.CritChanceReduction / .06f), ArmorDamageReduction, fCurrentDTPS, fMagicDR);
            calcs.ArmorMitigation        = fCurrentMitigation[(int)MitigationSub.Armor];
            calcs.AvoidanceMitigation    = fCurrentMitigation[(int)MitigationSub.Avoidance];
            calcs.CritMitigation         = fCurrentMitigation[(int)MitigationSub.Crit];
            calcs.DamageTakenMitigation  = fCurrentMitigation[(int)MitigationSub.DamageReduction];
            calcs.DamageTakenMitigation += fCurrentMitigation[(int)MitigationSub.Haste];
            calcs.HealsMitigation        = fCurrentMitigation[(int)MitigationSub.Heals];
            calcs.ImpedenceMitigation    = fCurrentMitigation[(int)MitigationSub.Impedences];
            calcs.MagicDamageReductedByAmount = fCurrentMitigation[(int)MitigationSub.AMS];
            calcs.MagicDamageReductedByAmount += fCurrentMitigation[(int)MitigationSub.Magic];

            calcs.Crit = (.06f - stats.CritChanceReduction);
            calcs.DTPS = 0;
            calcs.DTPSNoAvoidance = 0;
            foreach (float f in fCurrentDTPS)
            {
                // These are sometimes coming back as negative.
                // Assuming we are just 100% absorbing the attack, no damage
                if (f > 0) { calcs.DTPS += f; }
            }
            if (TDK.calcOpts.b_RecoveryInclAvoidance == false)
            {
                GetMitigation(ref TDK, stats, rot, (stats.CritChanceReduction / .06f), ArmorDamageReduction, fCurrentDTPSNoAvoid, fMagicDR, false);
                foreach (float f in fCurrentDTPSNoAvoid)
                {
                    // These are sometimes coming back as negative.
                    // Assuming we are just 100% absorbing the attack, no damage
                    if (f > 0) { calcs.DTPSNoAvoidance += f; }
                }
            }
            // Have to ensure we don't divide by 0
            calcs.Mitigation = StatConversion.MitigationScaler / (Math.Max(1f, calcs.DTPS) / fTotalDTPS);
            #endregion

            return calcs;
        }

        #region Character Stats
        /// <summary>
        /// GetCharacterStats is the 2nd-most calculation intensive method in a model. Here the model will
        /// combine all of the information about the character, including race, gear, enchants, buffs,
        /// calculationoptions, etc., to form a single combined Stats object. Three of the methods below
        /// can be called from this method to help total up stats: GetItemStats(character, additionalItem),
        /// GetEnchantsStats(character), and GetBuffsStats(character.ActiveBuffs).
        /// </summary>
        /// <param name="character">The character whose stats should be totaled.</param>
        /// <param name="additionalItem">An additional item to treat the character as wearing.
        /// This is used for gems, which don't have a slot on the character to fit in, so are just
        /// added onto the character, in order to get gem calculations.</param>
        /// <returns>A Stats object containing the final totaled values of all character stats.</returns>
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            return GetCharacterStats(character, additionalItem, StatType.Unbuffed, new TankDKChar());
        }

        /// <summary>
        /// Get Character Stats for multiple calls.  Allowing means by which to stack different sets/Special effects.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="additionalItem"></param>
        /// <param name="sType">Enum describing which set of stats we want.</param>
        /// <returns></returns>
        private StatsDK GetCharacterStats(Character character, Item additionalItem, StatType sType, TankDKChar TDK, Rotation rot = null)
        {
            StatsDK statsTotal = new StatsDK();
            if (null == character.CalculationOptions)
            {
                // Possibly put some error text here.
                return statsTotal;
            }
            // Warning TDK can be blank at this point.
            TDK.Char = character;
            TDK.calcOpts = character.CalculationOptions as CalculationOptionsTankDK;
            TDK.bo = character.BossOptions;

            // Start populating data w/ Basic racial & class baseline.
            Stats BStats = BaseStats.GetBaseStats(character);
            statsTotal.Accumulate(BStats);
            statsTotal.BaseAgility = BStats.Agility;

            AccumulateItemStats(statsTotal, character, additionalItem);
            // Stack only the info we care about.
            statsTotal = GetRelevantStatsLocal(statsTotal);

            AccumulateBuffsStats(statsTotal, character.ActiveBuffs);
            AccumulateSetBonusStats(statsTotal, character.SetBonusCount);

            #region Tier Bonuses: Tank
            #region T11
            int tierCount;
            if (character.SetBonusCount.TryGetValue("Magma Plated Battlearmor", out tierCount))
            {
                if (tierCount >= 2) { statsTotal.b2T11_Tank = true; }
                if (tierCount >= 4) { statsTotal.b4T11_Tank = true; }
            }
            if (statsTotal.b4T11_Tank)
                statsTotal.AddSpecialEffect(_SE_IBF[1]);
            else
                statsTotal.AddSpecialEffect(_SE_IBF[0]);
            #endregion
            #region T12
            if (character.SetBonusCount.TryGetValue("Elementium Deathplate Battlearmor", out tierCount))
            {
                if (tierCount >= 2) { statsTotal.b2T12_Tank = true; }
                if (tierCount >= 4) { statsTotal.b4T12_Tank = true; }
            }
            if (statsTotal.b2T12_Tank)
            {
                // Your melee attacks cause Burning Blood on your target, 
                // which deals 800 Fire damage every 2 for 6 sec and 
                // causes your abilities to behave as if you had 2 diseases 
                // present on the target.
                // Implemented in CombatState DiseaseCount

                statsTotal.FireDamage = 800 / 2;
            }
            if (statsTotal.b4T12_Tank)
            {
                // Your when your Dancing Rune Weapon expires, it grants 15% additional parry chance for 12 sec.
                // Implemented in DRW talent Static Special Effect.
            }
            #endregion
            #region T13
            if (character.SetBonusCount.TryGetValue("Necrotic Boneplate Armor", out tierCount))
            {
                if (tierCount >= 2) { statsTotal.b2T13_Tank = true; }
                if (tierCount >= 4) { statsTotal.b4T13_Tank = true; }
            }
            if (statsTotal.b2T13_Tank)
            {
                // When an attack drops your health below 35%, one of your Blood Runes 
                // will immediately activate and convert into a Death Rune for the next 
                // 20 sec. This effect cannot occur more than once every 45 sec.
            }
            if (statsTotal.b4T13_Tank)
            {
                // Your Vampiric Blood ability also affects all party and raid members 
                // for 50% of the effect it has on you.
            }
            #endregion
            #endregion

            Rawr.DPSDK.CalculationsDPSDK.RemoveDuplicateRunes(statsTotal, character, true/*statsTotal.bDW*/);
            Rawr.DPSDK.CalculationsDPSDK.AccumulateTalents(statsTotal, character);
            Rawr.DPSDK.CalculationsDPSDK.AccumulatePresenceStats(statsTotal, Presence.Blood, character.DeathKnightTalents);


            statsTotal.ArcaneResistance += statsTotal.ArcaneResistanceBuff; statsTotal.ArcaneResistanceBuff = 0f;
            statsTotal.FireResistance   += statsTotal.FireResistanceBuff;   statsTotal.FireResistanceBuff   = 0f;
            statsTotal.FrostResistance  += statsTotal.FrostResistanceBuff;  statsTotal.FrostResistanceBuff  = 0f;
            statsTotal.NatureResistance += statsTotal.NatureResistanceBuff; statsTotal.NatureResistanceBuff = 0f;
            statsTotal.ShadowResistance += statsTotal.ShadowResistanceBuff; statsTotal.ShadowResistanceBuff = 0f;

            /* At this point, we're combined all the data from gear and talents and all that happy jazz.
             * However, we haven't applied any special effects nor have we applied any multipliers.
             * Many special effects are now getting dependant upon combat info (rotations).
             */
            StatsDK PreRatingsBase = statsTotal.Clone() as StatsDK;
            // Apply the ratings to actual stats.
            ProcessRatings(statsTotal);
            ProcessAvoidance(statsTotal, TDK.bo.Level, TDK.Char, PreRatingsBase);
            statsTotal.EffectiveParry = 0;
            if (character.MainHand != null)
            {
                statsTotal.EffectiveParry = statsTotal.Parry;
            }
            float fChanceToGetHit = 1f - Math.Min(1f, statsTotal.Miss + statsTotal.Dodge + statsTotal.EffectiveParry);

            // Now comes the special handling for secondary stats passes that are dependant upon Boss & Rotation values.
            if (sType != StatType.Unbuffed
                && (null != TDK.bo && null != rot)) // Make sure we have the rotation and Boss info.
            {
                #region Special Effects
                #region Talent: Bone Shield
                if (character.DeathKnightTalents.BoneShield > 0)
                {
                    int BSStacks = 4;  // The number of bones by default.  
                    if (Rawr.Properties.GeneralSettings.Default.PTRMode)
                        BSStacks = 6;  // The number of bones by default.  
                    float BoneLossRate = Math.Max(2f, TDK.bo.DynamicCompiler_Attacks.AttackSpeed / fChanceToGetHit);  // 2 sec internal cooldown on loosing bones so the DK can't get spammed to death.  
                    float moveVal = character.DeathKnightTalents.GlyphofBoneShield ? 0.15f : 0f;
                    SpecialEffect primary = new SpecialEffect(Trigger.Use,
                        new Stats() { DamageTakenReductionMultiplier = 0.20f, BonusDamageMultiplier = 0.02f, MovementSpeed = moveVal, },
                        BoneLossRate * BSStacks, 60) {BypassCache = true,};
                    statsTotal.AddSpecialEffect(primary);
                }
                #endregion
                #region Vengeance
                // Vengence has the chance to increase AP.
                int iVengenceMax = (int)(statsTotal.Stamina + (BaseStats.GetBaseStats(character).Health) * .1);
                int iAttackPowerMax = (int)statsTotal.AttackPower + iVengenceMax;
                float mitigatedDPS = TDK.bo.GetDPSByType(TDK.role, 0, statsTotal.DamageTakenReductionMultiplier,
                    0, .14f, statsTotal.Miss, statsTotal.Dodge, statsTotal.EffectiveParry, 0, 0,
                    0, 0, 0, 0, 0);
                    //statsTotal.ArcaneResistance, statsTotal.FireResistance, statsTotal.FrostResistance, statsTotal.NatureResistance, statsTotal.ShadowResistance);
                mitigatedDPS = mitigatedDPS * (1 - (float)StatConversion.GetArmorDamageReduction(TDK.bo.Level, statsTotal.Armor, 0f, 0f));
                float APStackSingle = mitigatedDPS * 0.05f * TDK.bo.DynamicCompiler_Attacks.AttackSpeed;
                int APStackCountMax = (int)Math.Floor(iVengenceMax / APStackSingle);
                SpecialEffect seVeng = new SpecialEffect(Trigger.DamageTaken,
                    new Stats() { AttackPower = APStackSingle },
                    2 * 10,
                    0,
                    1,
                    APStackCountMax) { BypassCache = true, };
                Dictionary<Trigger, float> triggerInterval = new Dictionary<Trigger,float>();
                Dictionary<Trigger, float> triggerChance = new Dictionary<Trigger,float>();
                triggerInterval.Add(Trigger.DamageTaken, TDK.bo.DynamicCompiler_Attacks.AttackSpeed);
                triggerChance.Add(Trigger.DamageTaken, 1f); // MitigatedDPS already factors in avoidance.
                statsTotal.VengenceAttackPower = seVeng.GetAverageStats(triggerInterval, triggerChance).AttackPower;
                statsTotal.AttackPower += statsTotal.VengenceAttackPower * TDK.calcOpts.VengeanceWeight;
                #endregion
                statsTotal.AddSpecialEffect(_SE_DeathPact);
                // For now we just factor them in once.
                Rawr.DPSDK.StatsSpecialEffects se = new Rawr.DPSDK.StatsSpecialEffects(rot.m_CT, rot, TDK.bo);
                StatsDK statSE = new StatsDK();

                foreach (SpecialEffect effect in statsTotal.SpecialEffects())
                {
                    if (HasRelevantStats(effect.Stats))
                    {
                        statSE.Accumulate(se.getSpecialEffects(effect));
//                        statsTotal.Accumulate(se.getSpecialEffects(effect)); // This is done further down.
                    }
                }

                // Darkmoon card greatness procs
                if (statSE.HighestStat > 0 || statSE.Paragon > 0)
                {
                    if (statSE.Strength >= statSE.Agility) { statSE.Strength += statSE.HighestStat + statSE.Paragon; }
                    else if (statSE.Agility > statSE.Strength) { statSE.Agility += statSE.HighestStat + statSE.Paragon; }
                    statSE.HighestStat = 0;
                    statSE.Paragon = 0;
                }

                // Any Modifiers from stats need to be applied to statSE
                statSE.Strength = StatConversion.ApplyMultiplier(statSE.Strength, statsTotal.BonusStrengthMultiplier);
                statSE.Agility = StatConversion.ApplyMultiplier(statSE.Agility, statsTotal.BonusAgilityMultiplier);
                statSE.Stamina = StatConversion.ApplyMultiplier(statSE.Stamina, statsTotal.BonusStaminaMultiplier);
                //            statSE.Stamina = (float)Math.Floor(statSE.Stamina);
                statSE.Armor = StatConversion.ApplyMultiplier(statSE.Armor, statsTotal.BaseArmorMultiplier);
                statSE.AttackPower = StatConversion.ApplyMultiplier(statSE.AttackPower, statsTotal.BonusAttackPowerMultiplier);
                statSE.BonusArmor = StatConversion.ApplyMultiplier(statSE.BonusArmor, statsTotal.BonusArmorMultiplier);

                statSE.Armor += statSE.BonusArmor;
                statSE.Health += StatConversion.GetHealthFromStamina(statSE.Stamina) + statSE.BattlemasterHealthProc;
                statSE.Health = statSE.Health * (1 + statSE.BonusHealthMultiplier);
                statsTotal.BonusHealthMultiplier = ((1 + statsTotal.BonusHealthMultiplier) * (1 + statSE.BonusHealthMultiplier)) - 1 ;
                if (character.DeathKnightTalents.BladedArmor > 0)
                {
                    statSE.AttackPower += (statSE.Armor / 180f) * (float)character.DeathKnightTalents.BladedArmor;
                }
                statSE.AttackPower += StatConversion.ApplyMultiplier((statSE.Strength * 2), statsTotal.BonusAttackPowerMultiplier);
                statSE.ParryRating += statSE.Strength * 0.27f;

                // Any Modifiers from statSE need to be applied to stats
                statsTotal.Strength = StatConversion.ApplyMultiplier(statsTotal.Strength, statSE.BonusStrengthMultiplier);
                statsTotal.Agility = StatConversion.ApplyMultiplier(statsTotal.Agility, statSE.BonusAgilityMultiplier);
                statsTotal.Stamina = StatConversion.ApplyMultiplier(statsTotal.Stamina, statSE.BonusStaminaMultiplier);
                //            stats.Stamina = (float)Math.Floor(stats.Stamina);
                statsTotal.Armor = StatConversion.ApplyMultiplier(statsTotal.Armor, statSE.BaseArmorMultiplier);
                statsTotal.AttackPower = StatConversion.ApplyMultiplier(statsTotal.AttackPower, statSE.BonusAttackPowerMultiplier);
                statsTotal.BonusArmor = StatConversion.ApplyMultiplier(statsTotal.BonusArmor, statSE.BonusArmorMultiplier);

                statsTotal.Accumulate(statSE);
                PreRatingsBase.Miss += statSE.Miss;
                PreRatingsBase.Dodge += statSE.Dodge;
                PreRatingsBase.Parry += statSE.Parry;
#if DEBUG
                if (float.IsNaN(statsTotal.Stamina))
                    throw new Exception("Something very wrong in stats.");
#endif
                #endregion // Special effects
            }
            // Apply the Multipliers
            ProcessStatModifiers(statsTotal, character.DeathKnightTalents.BladedArmor, character);
            ProcessAvoidance(statsTotal, TDK.bo.Level, TDK.Char, PreRatingsBase);
            if (character.MainHand != null)
            {
                statsTotal.EffectiveParry = statsTotal.Parry;
            }
            return (statsTotal);
        }


        /// <summary>
        /// Process the Stat modifier values 
        /// </summary>
        /// <param name="statsTotal">[in/out] Stats object for the total character stats.</param>
        /// <param name="iBladedArmor">[in] character.talent.BladedArmor</param>
        private void ProcessStatModifiers(StatsDK statsTotal, int iBladedArmor, Character c)
        {
            statsTotal.Strength = StatConversion.ApplyMultiplier(statsTotal.Strength, statsTotal.BonusStrengthMultiplier);
            statsTotal.Agility = StatConversion.ApplyMultiplier(statsTotal.Agility, statsTotal.BonusAgilityMultiplier);
            // The stamina value is floor in game for the calculation
            statsTotal.Stamina = StatConversion.ApplyMultiplier(statsTotal.Stamina, statsTotal.BonusStaminaMultiplier);
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina);
            statsTotal.Armor = StatConversion.ApplyMultiplier(statsTotal.Armor, statsTotal.BaseArmorMultiplier);
            statsTotal.AttackPower = StatConversion.ApplyMultiplier(statsTotal.AttackPower, statsTotal.BonusAttackPowerMultiplier);
            statsTotal.BonusArmor = StatConversion.ApplyMultiplier(statsTotal.BonusArmor, statsTotal.BonusArmorMultiplier);

            statsTotal.Armor += statsTotal.BonusArmor;
            statsTotal.Health += StatConversion.GetHealthFromStamina(statsTotal.Stamina);
            statsTotal.Health = statsTotal.Health * (1 + statsTotal.BonusHealthMultiplier);

            // Talent: BladedArmor //////////////////////////////////////////////////////////////
            if (iBladedArmor > 0)
            {
                statsTotal.AttackPower += (statsTotal.Armor / 180f) * (float)iBladedArmor;
            }
            // AP, crit, etc.  already being factored in w/ multiplier.
            statsTotal.AttackPower += StatConversion.ApplyMultiplier((statsTotal.Strength * 2), statsTotal.BonusAttackPowerMultiplier);
        }

        /// <summary>
        /// Process All the ratings score to their base values.
        /// </summary>
        /// <param name="s"></param>
        private void ProcessRatings(StatsDK statsTotal)
        {
            // Expertise Rating -> Expertise:
            statsTotal.Expertise += StatConversion.GetExpertiseFromRating(statsTotal.ExpertiseRating);
            // Mastery Rating  Handled during AccumulateTalents.
//            statsTotal.Mastery += StatConversion.GetMasteryFromRating(statsTotal.MasteryRating);

            statsTotal.PhysicalHit += StatConversion.GetPhysicalHitFromRating(statsTotal.HitRating);
            statsTotal.PhysicalCrit += StatConversion.GetPhysicalCritFromRating(statsTotal.CritRating);
            statsTotal.PhysicalCrit += StatConversion.GetPhysicalCritFromAgility(statsTotal.Agility, CharacterClass.DeathKnight);
            statsTotal.PhysicalHaste += StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, CharacterClass.DeathKnight);

            statsTotal.SpellHit += StatConversion.GetSpellHitFromRating(statsTotal.HitRating);
            statsTotal.SpellCrit += StatConversion.GetSpellCritFromRating(statsTotal.CritRating);
            statsTotal.SpellCrit += StatConversion.GetSpellCritFromIntellect(statsTotal.Intellect);
            statsTotal.SpellCrit += statsTotal.SpellCritOnTarget;
            statsTotal.SpellHaste += StatConversion.GetSpellHasteFromRating(statsTotal.HasteRating, CharacterClass.DeathKnight);
        }

        private void ProcessAvoidance(StatsDK statsTotal, int iTargetLevel, Character c, StatsDK PreRatingsBase)
        {
            // Key point to get avoidance bases.  
            // Ratings with change with multiple passes, 
            // but the actual values need to return back to the base.
            statsTotal.Miss = PreRatingsBase.Miss;
            statsTotal.Dodge = PreRatingsBase.Dodge;
            statsTotal.Parry = PreRatingsBase.Parry;
            // Get all the character avoidance numbers including deminishing returns.
            // Iterate through each hit type. and use fAvoidance array w/ the hitresult enum.
            float[] fAvoidance = new float[HitResultCount];
            for (uint i = 0; i < HitResultCount; i++)
            {
                // GetDRAvoidanceChance returns a dec. percentage.
                // Since CurrentAvoidance is a percent, need to multiply by 100.
                fAvoidance[i] = (StatConversion.GetDRAvoidanceChance(c, statsTotal, (HitResult)i, iTargetLevel));
            }

            // So let's populate the miss, dodge and parry values for the UI display as well as pulling them out of the avoidance number.
            // Cap and floor is already factored in as part of the GetDRAvoidanceChance.
            statsTotal.Miss = fAvoidance[(int)HitResult.Miss];
            statsTotal.Dodge = fAvoidance[(int)HitResult.Dodge];
            statsTotal.Parry = fAvoidance[(int)HitResult.Parry];
        }
        #endregion
    }
}
