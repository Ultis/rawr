using System;
using System.Collections.Generic;


namespace Rawr.Tree
{

    public class Rotation
    {
        public float HPS;
        public float MPS;
        public float MaxHPS;
        public float HPSFromPrimary;
        public float HPSFromHots;
        public float HPSFromTrueHots;
        public float HPSFromWildGrowth;
        public float HPSFromSwiftmend;
        public float MPSFromPrimary;
        public float MPSFromHots;
        public float MPSFromSwiftmend;
        public float HotsFraction;
        public float CastsPerMinute;
        public float CritsPerMinute;
        public float HealsPerMinute;
        public float MPSFromWildGrowth;
        public float TotalTime;
        public float TimeToOOM;
        public float ManaPer5In5SR;
        public float ManaPer5Out5SR;
        public float ManaPer5InRotation;
        public float ManaFromEachInnervate;
        public float ManaFromInnervates;
        public float ManaFromSpirit;
        public float ManaFromMP5;
        public float ManaFromPotions;
        public float ReplenishRegen;
        public float TotalHealing;
        public float TotalCastsPerMinute;
        public float TotalCritsPerMinute;
        public float TotalHealsPerMinute;
        public float UnusedMana;
        public float UnusedCastTimeFrac;
        public float RejuvenationHealsPerMinute;
        public RotationSettings rotSettings;
//        public int LifebloomStackSize;
//        public bool LifebloomFastStacking;
    }

    public enum HealTargetTypes { TankHealing = 0, RaidHealing = 1 };

    public enum SpellList { HealingTouch = 0, Nourish = 1, Regrowth = 2, Rejuvenation = 3 };

    public class RotationSettings
    {
        public bool rejuvOnTank, rgOnTank;
        public int lifeBloomStackSize, noTanks;
        public bool lifeBloomFastStack;
        public SpellList primaryHeal;
        public HealTargetTypes healTarget;
        public int SwiftmendPerMin, WildGrowthPerMinute;
    }

    [Rawr.Calculations.RawrModelInfo("Tree", "Ability_Druid_TreeofLife", CharacterClass.Druid)]
    public class CalculationsTree : CalculationsBase
    {
        private string[] _predefRotations = null;
        public string[] PredefRotations
        {
            get
            {
                if (_predefRotations == null)
                    _predefRotations = new string[] {
                        "Tank Nourish (plus RJ/RG/Roll LB)",
                        "Tank Nourish (plus RJ/RG/Slow 3xLB)",
                        "Tank Nourish (plus RJ/RG/Fast 3xLB)",
                        "Tank Nourish (2 Tanks RJ/RG/LB)",
                        "Tank Nourish (2 Tanks RJ/RG/Slow 3xLB)",
                        "Tank Nourish (2 Tanks RJ/RG/Fast 3xLB)",
                        "Tank Healing Touch (plus RJ/RG/LB)",
                        "Tank Healing Touch (2 Tanks RJ/RG/LB)",
                        "Tank Regrowth (plus RJ/RG/Roll LB)",
                        "Tank Regrowth (plus RJ/RG/Slow 3xLB)",
                        "Tank Regrowth (plus RJ/RG/Fast 3xLB)",
                        "Tank Regrowth (2 Tanks RJ/RG/Roll LB)",
                        "Tank Regrowth (2 Tanks RJ/RG/Slow 3xLB)",
                        "Tank Regrowth (2 Tanks RJ/RG/Fast 3xLB)",
                        "Raid heal with Regrowth (1 Tank RJ/Roll LB)",
                        "Raid heal with Regrowth (1 Tank RJ/Slow 3xLB)",
                        "Raid heal with Regrowth (2 Tanks RJ/Roll LB)",
                        "Raid heal with Regrowth (2 Tanks RJ/Slow 3xLB)",
                        "Raid heal with Rejuv (1 Tank RJ/Roll LB)",
                        "Raid heal with Rejuv (1 Tank RJ/Slow 3xLB)",
                        "Raid heal with Rejuv (2 Tanks RJ/Roll LB)",
                        "Raid heal with Rejuv (2 Tanks RJ/Slow 3xLB)",
                        "Raid heal with Nourish (1 Tank RJ/Roll LB)",
                        "Raid heal with Nourish (1 Tank RJ/Slow 3xLB)",
                        "Raid heal with Nourish (2 Tanks RJ/Roll LB)",
                        "Raid heal with Nourish (2 Tanks RJ/Slow 3xLB)",
                        "Nourish spam",
                        "Healing Touch spam",
                        "Regrowth spam on tank",
                        "Regrowth spam on raid",
                        "Rejuvenation spam on raid",					};
                return _predefRotations;
            }
        }


        private List<GemmingTemplate> _defaultGemmingTemplates = null;
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                if (_defaultGemmingTemplates == null)
                {
                    // Meta
                    int ember = 41333;
                    int revitalizing = 41376;
                    int insightful = 41401;

                    // [0] uncommon
                    // [1] perfect uncommon
                    // [2] rare
                    // [3] epic
                    // [4] jewelcrafting

                    // Reds
                    int[] runed = { 39911, 41438, 39998, 40113, 42144 }; // spell power
                    // Blue
                    int[] lustrous = { 39927, 41440, 40010, 40121, 42146 }; // mp5
                    int[] sparkling = { 39920, 41442, 40009, 40120, 42145 }; // spi
                    // Yellow
                    int[] brilliant = { 39912, 41444, 40012, 40123, 42148 }; // int
                    // Purple
                    int[] purified = { 39941, 41457, 40026, 40133 }; // spell power + spirit
                    int[] royal = { 39943, 41459, 40027, 40134 }; // spell power + mp5
                    // Green
                    int[] dazzling = { 39984, 41463, 40094, 40175 }; // int + mp5
                    int[] seers = { 39979, 41473, 40092, 40170 }; // int + spi
                    // Orange
                    int[] luminous = { 39946, 41494, 40047, 40151 }; // int + spell power

                    /*
                     * Gemmings
                     * No Crit, Spirit > MP5
                     * red: Runed, Purified, Luminous
                     * yellow: Brilliant, Seer's, Luminous
                     * blue: Sparkling, Purified, Seer's
                     * = runed, purified, luminous, brilliant, seer's, sparkling
                     * seems seer's is always better than sparkling
                     * Meta: use revitalizing (better than ember)
                     */

                    _defaultGemmingTemplates = new List<GemmingTemplate>();
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Uncommon (Revitalizing)", false, runed[0], purified[0], luminous[0], seers[0], brilliant[0], revitalizing);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Uncommon (Ember)", false, runed[0], purified[0], luminous[0], seers[0], brilliant[0], ember);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Uncommon (Insightful)", false, runed[0], purified[0], luminous[0], seers[0], brilliant[0], insightful);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Perfect (Revitalizing)", false, runed[1], purified[1], luminous[1], seers[1], brilliant[1], revitalizing);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Perfect (Ember)", false, runed[1], purified[1], luminous[1], seers[1], brilliant[1], ember);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Perfect (Insightful)", false, runed[1], purified[1], luminous[1], seers[1], brilliant[1], insightful);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Rare (Revitalizing)", false, runed[2], purified[2], luminous[2], seers[2], brilliant[2], revitalizing);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Rare (Ember)", false, runed[2], purified[2], luminous[2], seers[2], brilliant[2], ember);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Rare (Insightful)", true, runed[2], purified[2], luminous[2], seers[2], brilliant[2], insightful);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Epic (Revitalizing)", false, runed[3], purified[3], luminous[3], seers[3], brilliant[3], revitalizing);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Epic (Ember)", false, runed[3], purified[3], luminous[3], seers[3], brilliant[3], ember);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Epic (Insightful)", false, runed[3], purified[3], luminous[3], seers[3], brilliant[3], insightful);
                    AddJCGemmingTemplateGroup(_defaultGemmingTemplates, "Jewelcrafting (Revitalizing)", false, runed[4], sparkling[4], brilliant[4], revitalizing);
                    AddJCGemmingTemplateGroup(_defaultGemmingTemplates, "Jewelcrafting (Ember)", false, runed[4], sparkling[4], brilliant[4], ember);
                }
                return _defaultGemmingTemplates;
            }
        }

        private void AddGemmingTemplateGroup(List<GemmingTemplate> list, string name, bool enabled, int runed, int purified, int luminous, int seers, int brilliant, int meta)
        {
            // Overrides, only "runed" and "seers"
            list.Add(new GemmingTemplate() { Model = "Tree", Group = name, RedId = runed, YellowId = runed, BlueId = runed, PrismaticId = runed, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Tree", Group = name, RedId = seers, YellowId = seers, BlueId = seers, PrismaticId = seers, MetaId = meta, Enabled = enabled });

            list.Add(new GemmingTemplate() { Model = "Tree", Group = name, RedId = runed, YellowId = luminous, BlueId = purified, PrismaticId = runed, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Tree", Group = name, RedId = luminous, YellowId = brilliant, BlueId = seers, PrismaticId = brilliant, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Tree", Group = name, RedId = luminous, YellowId = seers, BlueId = seers, PrismaticId = seers, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Tree", Group = name, RedId = purified, YellowId = seers, BlueId = seers, PrismaticId = seers, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Tree", Group = name, RedId = runed, YellowId = seers, BlueId = seers, PrismaticId = runed, MetaId = meta, Enabled = enabled });
        }

        private void AddJCGemmingTemplateGroup(List<GemmingTemplate> list, string name, bool enabled, int runed, int sparkling, int brilliant, int meta)
        {
            // Overrides, only "runed" and "seers"
            list.Add(new GemmingTemplate() { Model = "Tree", Group = name, RedId = runed, YellowId = runed, BlueId = runed, PrismaticId = runed, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Tree", Group = name, RedId = sparkling, YellowId = sparkling, BlueId = sparkling, PrismaticId = sparkling, MetaId = meta, Enabled = enabled });

            list.Add(new GemmingTemplate() { Model = "Tree", Group = name, RedId = runed, YellowId = brilliant, BlueId = sparkling, PrismaticId = runed, MetaId = meta, Enabled = enabled });
        }

#if RAWR3
        private Dictionary<string, System.Windows.Media.Color> _subPointNameColors = null;
#else
        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
#endif

#if RAWR3
        public override Dictionary<string, System.Windows.Media.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Windows.Media.Color>();
                    _subPointNameColors.Add("HealBurst", System.Windows.Media.Colors.Red);
                    _subPointNameColors.Add("HealSustained", System.Windows.Media.Colors.Blue);
                    _subPointNameColors.Add("Survival", System.Windows.Media.Colors.Green);
                }
                return _subPointNameColors;
            }
        }
#else
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("HealBurst", System.Drawing.Color.Red);
                    _subPointNameColors.Add("HealSustained", System.Drawing.Color.Blue);
                    _subPointNameColors.Add("Survival", System.Drawing.Color.Green);
                }
                return _subPointNameColors;
            }
        }
#endif
 

        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                {
                    _characterDisplayCalculationLabels = new string[] {
                        "Points:HealBurst",
                        "Points:HealSustained",
                        @"Points:Survival*Survival Points represents the total raw physical damage 
you can take before dying. Calculated based on Health
and Armor damage reduction. (Survival multiplier is 
applied and result is scaled down by 100)",
                        "Points:Overall",

                        "Basic Stats:Health",
                        "Basic Stats:Mana",
                        "Basic Stats:Stamina",
                        "Basic Stats:Intellect",
                        "Basic Stats:Spirit",
                        "Basic Stats:Healing",
                        "Basic Stats:Effective MP5",
                        "Basic Stats:Spell Crit",
                        "Basic Stats:Spell Haste",
                        "Basic Stats:Global CD",
                        "Basic Stats:Lifebloom Global CD",
                        "Basic Stats:Armor",

                        "Simulation:Result",
                        "Simulation:Time until OOM",
                        "Simulation:Unused Mana Remaining",
                        "Simulation:Unused cast time percentage",
                        "Simulation:Total healing done",
                        "Simulation:Number of tanks",
                        "Simulation:Lifebloom method",
                        "Simulation:Extra Tank HoTs",
                        "Simulation:HPS for tank HoTs",
                        "Simulation:MPS for tank HoTs",
                        "Simulation:HPS for Wild Growth",
                        "Simulation:MPS for Wild Growth",
                        "Simulation:HPS for Swiftmend",
                        "Simulation:MPS for Swiftmend",
                        "Simulation:HoT refresh fraction",
                        "Simulation:Spell for primary heal",
                        "Simulation:HPS for primary heal",
                        "Simulation:MPS for primary heal",
                        "Simulation:Mana regen per second",
                        "Simulation:Casts per minute until OOM",
                        "Simulation:Crits per minute until OOM",

                        "Lifebloom:LB Tick",
                        "Lifebloom:LB Heal",
                        "Lifebloom:LB HPS",
                        "Lifebloom:LB HPM",

                        "Lifebloom Stacked Blooms:LBx2 (fast stack) HPS",
                        "Lifebloom Stacked Blooms:LBx2 (fast stack) HPM",
                        "Lifebloom Stacked Blooms:LBx3 (fast stack) HPS",
                        "Lifebloom Stacked Blooms:LBx3 (fast stack) HPM",
                        "Lifebloom Stacked Blooms:LBx2 (slow stack) HPS",
                        "Lifebloom Stacked Blooms:LBx2 (slow stack) HPM",
                        "Lifebloom Stacked Blooms:LBx3 (slow stack) HPS",
                        "Lifebloom Stacked Blooms:LBx3 (slow stack) HPM",

                        "Lifebloom Continuous Stack:LBS Tick",
                        "Lifebloom Continuous Stack:LBS HPS",
                        "Lifebloom Continuous Stack:LBS HPM",

                        "Rejuvenation:RJ Tick",
                        "Rejuvenation:RJ HPS",
                        "Rejuvenation:RJ HPM",
            
                        "Regrowth:RG Heal",
                        "Regrowth:RG Tick",
                        "Regrowth:RG HPS",
                        "Regrowth:RG HPS (HoT)",
                        "Regrowth:RG HPM",
                        "Regrowth:RG HPM (spam)",

                        "Healing Touch:HT Heal",
                        "Healing Touch:HT HPS",
                        "Healing Touch:HT HPM",

                        "Wild Growth:WG first Tick",
                        "Wild Growth:WG HPS(single)",
                        "Wild Growth:WG HPM(single)",
                        "Wild Growth:WG HPS(max)",
                        "Wild Growth:WG HPM(max)",

                        "Nourish:N Heal",
                        "Nourish:N HPM",
                        "Nourish:N HPS",
                        "Nourish:N (1 HoT) Heal",
                        "Nourish:N (1 HoT) HPM",
                        "Nourish:N (1 HoT) HPS",
                        "Nourish:N (3 HoTs) Heal",
                        "Nourish:N (3 HoTs) HPM",
                        "Nourish:N (3 HoTs) HPS",

                        "Swiftmend:SM Rejuv Heal",
                        "Swiftmend:SM Rejuv HPM",
                        "Swiftmend:SM Rejuv Lost Ticks",
                        "Swiftmend:SM Regrowth Heal",
                        "Swiftmend:SM Regrowth HPM",
                        "Swiftmend:SM Regrowth Lost Ticks",
                        "Swiftmend:SM Both Heal",
                        "Swiftmend:SM Both HPM",
                        "Swiftmend:SM Both Rejuv Lost Ticks",
                        "Swiftmend:SM Both Regrowth Lost Ticks",
                    };
                }
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] {
					"Spell rotations"
					};
                return _customChartNames;
            }
        }

        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
					"Mana",
					"MP5",
                    "GCD (milliseconds)",
                    "Lifebloom GCD (milliseconds)",
					"Spell Haste Percentage",
                    "Haste Percentage",
                    "Combined Haste Percentage",
                    "Haste until Lifebloom Cap",
                    "Haste until Hard Cap",
					};
                return _optimizableCalculationLabels;
            }
        }


#if RAWR3
        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
#else
        private CalculationOptionsPanelTree _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
#endif
        {
            get
            {
                if (_calculationOptionsPanel == null)
                    _calculationOptionsPanel = new CalculationOptionsPanelTree();
                return _calculationOptionsPanel;
            }
        }

        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    // I don't know of a fist weapon or two hand mace with healing stats, so...
                    // I don't know either .. but i know there are a few with spell power
                    _relevantItemTypes = new List<ItemType>(new ItemType[]{
                        ItemType.None,
                        ItemType.Cloth,
                        ItemType.Leather,
                        ItemType.Dagger,
                        ItemType.FistWeapon,
                        ItemType.Idol,
                        ItemType.OneHandMace,
                        ItemType.TwoHandMace,
                        ItemType.Staff
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override CharacterClass TargetClass
        {
            get { return CharacterClass.Druid; }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationTree();
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsTree();
        }

        private static Stats getTrinketStats(Character character, Stats stats, float FightDuration, float CastInterval, float HealInterval, float CritsRatio, float RejuvInterval, out float Healing)
        {
            #region New_SpecialEffect_Handling

            Stats resultNew = new Stats();
            foreach (Rawr.SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use)
                {
                  resultNew += effect.GetAverageStats(0.0f, 1.0f, 2.0f, FightDuration);   // 0 cooldown, 100% chance to use
                }
                else if (effect.Trigger == Trigger.SpellCast )
                {
                    resultNew += effect.GetAverageStats(CastInterval, 1.0f, CastInterval, FightDuration);
                }
                else if ( effect.Trigger == Trigger.HealingSpellCast || effect.Trigger == Trigger.HealingSpellHit)
                {
                    // Heal interval measures time between HoTs as well, direct heals are a different interval
                    resultNew += effect.GetAverageStats(HealInterval, 1.0f, CastInterval, FightDuration);
                }
                else if (effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.HealingSpellCrit)
                {
                    resultNew += effect.GetAverageStats(CastInterval, CritsRatio, CastInterval, FightDuration);
                }
                else if (effect.Trigger == Trigger.RejuvenationTick)
                {
                    resultNew += effect.GetAverageStats(RejuvInterval, 1.0f, RejuvInterval, FightDuration);
                }
                else
                {
                        // Trigger isn't relevant. Physical Hit, Damage Spell etc.
                        //int i = 0;
                }
            }
            #endregion

            Healing = resultNew.Healed;
            return new Stats()
            {
                Spirit = resultNew.Spirit,
                HasteRating = resultNew.HasteRating,
                SpellPower = resultNew.SpellPower,
                CritRating = resultNew.CritRating,
                Mp5 = resultNew.Mp5 + resultNew.ManaRestore,
                SpellCombatManaRegeneration = resultNew.SpellCombatManaRegeneration,
                BonusHealingReceived = resultNew.BonusHealingReceived,
                SpellsManaReduction = resultNew.SpellsManaReduction,
                HighestStat = resultNew.HighestStat,
                
            };
        }
        
        protected Rotation SimulateHealing(CharacterCalculationsTree calculatedStats, Stats stats, CalculationOptionsTree calcOpts, RotationSettings rotSettings)
        {
            float primaryFrac;
            float lifebloomDuration = 0.0f;

            #region Setup Spells
            int hots = 0;
            if (rotSettings.rejuvOnTank) hots++;
            if (rotSettings.rgOnTank) hots++;
            if (rotSettings.lifeBloomStackSize > 0) hots++;
            //Spell regrowth = new Regrowth(calculatedStats, stats, false);
            Spell regrowth = new Regrowth(calculatedStats, stats, true);
            Spell lifebloom = null;
            if ((rotSettings.lifeBloomStackSize >= 1) && (rotSettings.lifeBloomStackSize <= 3))
            {
                lifebloom = new Lifebloom(calculatedStats, stats, rotSettings.lifeBloomStackSize, rotSettings.lifeBloomFastStack);
                lifebloomDuration = lifebloom.Duration + 1.0f; // Add 1 sec, to make sure bloom has taken place, before applying again
            }
            else if (rotSettings.lifeBloomStackSize == 4)
            {
                lifebloom = new LifebloomStack(calculatedStats, stats);
                lifebloomDuration = lifebloom.Duration;
            }
            
            Spell rejuvenate = new Rejuvenation(calculatedStats, stats);
            //Spell nourish = new Nourish(calculatedStats, stats);
            //Spell nourishWithHoT = new Nourish(calculatedStats, stats, hots > 0 ? hots : 1);
            //Spell healingTouch = new HealingTouch(calculatedStats, stats);
            WildGrowth wildGrowth = new WildGrowth(calculatedStats, stats);


            Spell primaryHeal;
            switch (rotSettings.primaryHeal)
            {
                case SpellList.HealingTouch:
                default:
                    {
                        primaryHeal = new HealingTouch(calculatedStats, stats);
                    }
                    break;

                case SpellList.Nourish:
                    {
                        if (rotSettings.healTarget == HealTargetTypes.RaidHealing)
                        {
                            primaryHeal = new Nourish(calculatedStats, stats, 0);
                        }
                        else
                        {
                            int noTankHots = 0;
                            noTankHots += (rotSettings.rgOnTank) ? 1 : 0;
                            noTankHots += (rotSettings.rejuvOnTank) ? 1 : 0;
                            noTankHots += (rotSettings.lifeBloomStackSize>0) ? 1 : 0;
                            primaryHeal = new Nourish(calculatedStats, stats, noTankHots);
                        }
                    }
                    break;

                case SpellList.Regrowth:
                    {
                        primaryHeal = new Regrowth(calculatedStats, stats, rotSettings.healTarget == HealTargetTypes.TankHealing);
                    }
                    break;

                case SpellList.Rejuvenation:
                    {
                        primaryHeal = new Rejuvenation(calculatedStats, stats);
                    }
                    break;
            }
            #endregion

            float castsPerMinute = 0;
            float critsPerMinute = 0;
            float healsPerMinute = 0;

            float rejuvTicksPerMinute = 0;

            #region WildGrowthPerMinute
            float wgCastTime = wildGrowth.CastTime / 60f * rotSettings.WildGrowthPerMinute;
            float wgMPS = wildGrowth.ManaCost / 60f * rotSettings.WildGrowthPerMinute;
            float wgHPS = wildGrowth.PeriodicTick * wildGrowth.maxTargets * wildGrowth.Duration / 60f * rotSettings.WildGrowthPerMinute;  // Assume no overhealing
            castsPerMinute += rotSettings.WildGrowthPerMinute;
            healsPerMinute += rotSettings.WildGrowthPerMinute * 10; // assumption it will go 10 times ;)
            #endregion

            #region HotsOnTanks
            float hotsHPS = 0;
            float trueHotsHPS = 0;
            float hotsCastTime = 0;
            float hotsMPS = 0;
            float hotsCastsPerMinute = 0;
            float hotsCritsPerMinute = 0;
            float hotsHealsPerMinute = 0;
            if (rotSettings.rejuvOnTank)
            {
                hotsHPS += rejuvenate.HPSHoT + (rejuvenate.HPS / rejuvenate.Duration); // rejuvenate.HPS to cater for instant tick from T8_4 set bonus
                trueHotsHPS += rejuvenate.HPSHoT + (rejuvenate.HPS / rejuvenate.Duration); // rejuvenate.HPS to cater for instant tick from T8_4 set bonus
                hotsMPS += rejuvenate.ManaCost / rejuvenate.Duration;
                hotsCastTime += rejuvenate.CastTime / rejuvenate.Duration;
                hotsCastsPerMinute += 60f / rejuvenate.Duration;
                hotsHealsPerMinute += 20; // hot component
                rejuvTicksPerMinute += 20;
            }
            if (rotSettings.rgOnTank)
            {
                hotsHPS += regrowth.HPSHoT + regrowth.HPS / regrowth.Duration;
                trueHotsHPS += regrowth.HPSHoT;
                hotsMPS += regrowth.ManaCost / regrowth.Duration;
                hotsCastTime += regrowth.CastTime / regrowth.Duration;
                hotsCastsPerMinute += 60f / regrowth.Duration;
                hotsCritsPerMinute += 60f / regrowth.Duration * regrowth.CritPercent / 100f;
                hotsHealsPerMinute += 60f / regrowth.Duration; // direct component
                hotsHealsPerMinute += 20; // hot component
            }
            if ((rotSettings.lifeBloomStackSize > 0) && (lifebloom != null))
            {
                           // HoT part                 Bloom Part
                hotsHPS += lifebloom.HPSHoT + lifebloom.AverageHealingwithCrit / lifebloomDuration;
                trueHotsHPS += lifebloom.HPSHoT;
                hotsMPS += lifebloom.ManaCost / lifebloomDuration;
                hotsCastTime += lifebloom.CastTime / lifebloomDuration;
                hotsCastsPerMinute += 60f / lifebloomDuration;
                hotsHealsPerMinute += 60; // hot component
                hotsCritsPerMinute += 60f / lifebloomDuration * lifebloom.CritPercent / 100f;  // Bloom crits
            }
            hotsHPS *= rotSettings.noTanks;
            trueHotsHPS *= rotSettings.noTanks;
            hotsMPS *= rotSettings.noTanks;
            hotsCastTime *= rotSettings.noTanks;
            hotsCastsPerMinute *= rotSettings.noTanks;
            hotsCritsPerMinute *= rotSettings.noTanks;
            hotsHealsPerMinute *= rotSettings.noTanks;
            castsPerMinute += hotsCastsPerMinute;
            critsPerMinute += hotsCritsPerMinute;
            healsPerMinute += hotsHealsPerMinute;
            rejuvTicksPerMinute *= rotSettings.noTanks;
            #endregion

            #region Swiftmend
            Swiftmend swift;
            float swiftHPS = 0.0f;
            float swiftMPS = 0.0f;
            float swiftCastTime = 0.0f;
            if ((hots > 0) && (rotSettings.SwiftmendPerMin > 0))
            {

                if (rotSettings.rejuvOnTank)
                    if (rotSettings.rgOnTank)
                        swift = new Swiftmend(calculatedStats, stats, rejuvenate, regrowth);
                    else
                        swift = new Swiftmend(calculatedStats, stats, rejuvenate, null);
                else
                    if (rotSettings.rgOnTank)
                        swift = new Swiftmend(calculatedStats, stats, null, regrowth);
                    else
                        swift = new Swiftmend(calculatedStats, stats, null, null);

                swiftCastTime = swift.CastTime * rotSettings.SwiftmendPerMin / 60.0f;
                swiftHPS = swift.TotalAverageHealing * rotSettings.SwiftmendPerMin / 60.0f;
                swiftMPS = swift.ManaCost * rotSettings.SwiftmendPerMin / 60.0f;
                castsPerMinute += rotSettings.SwiftmendPerMin;
                healsPerMinute += rotSettings.SwiftmendPerMin;
                critsPerMinute += swift.CritPercent / 100.0f * rotSettings.SwiftmendPerMin;

                /* /
                // Handle consumed HoTs
                #region Consumed HoTs if not refreshed
                // Loss of HoTs HPS if not refreshed          // Use only one of these 2 sections
                hotsHPS -= swift.rejuvUseChance * swift.rejuvTicksLost * rejuvenate.PeriodicTick * swiftmendPMin / 60.0f;
                hotsHPS -= swift.regrowthUseChance * swift.regrowthTicksLost * regrowth.PeriodicTick * swiftmendPMin / 60.0f;
                #endregion

                 /* */
                 
                /* */
                #region Replace HoTs if refreshed
                // Extra MPS if HoTs refreshed                // Use only one of these 2 sections
                hotsMPS += swift.rejuvUseChance * swift.rejuvTicksLost / rejuvenate.PeriodicTicks * rejuvenate.ManaCost * rotSettings.SwiftmendPerMin / 60.0f;
                hotsMPS += swift.regrowthUseChance * swift.regrowthTicksLost / regrowth.PeriodicTicks * regrowth.ManaCost * rotSettings.SwiftmendPerMin / 60.0f;
                // Replacing Regrowths gives extra direct heals
                hotsHPS += swift.regrowthUseChance * swift.regrowthTicksLost / regrowth.PeriodicTicks * regrowth.AverageHealingwithCrit * rotSettings.SwiftmendPerMin / 60.0f; 
                // Replacing HoTs take extra time
                hotsCastTime += swift.rejuvUseChance * swift.rejuvTicksLost / rejuvenate.PeriodicTicks * rejuvenate.CastTime * rotSettings.SwiftmendPerMin / 60.0f;
                hotsCastTime += swift.regrowthUseChance * swift.regrowthTicksLost / regrowth.PeriodicTicks * regrowth.CastTime * rotSettings.SwiftmendPerMin / 60.0f;
                #endregion
                 /* */

            }
            #endregion

            #region Mana regeneration
            float spiritRegen = StatConversion.GetSpiritRegenSec(stats.Intellect, stats.Spirit); // CalculateManaRegen(stats.Intellect, stats.Spirit);

            spiritRegen *= 5.0f;    // Change to MP5, since GetSpiritRegenSec works per sec 
            
            float replenishment;
//            if (1) //calcOpts.Patch3_2
            {
                replenishment = stats.Mana * 0.01f * (calcOpts.ReplenishmentUptime / 100f); // Now 1% every 5 sec
            }
            /*else      // patch 3.1
            {
                replenishment = stats.Mana * 0.0025f * 5 * (calcOpts.ReplenishmentUptime / 100f);   // Old: 0.25% every sec
            } */
            float ManaRegenInFSR = stats.Mp5 + replenishment + spiritRegen * stats.SpellCombatManaRegeneration;
            float ManaRegenOutFSR = stats.Mp5 + replenishment + spiritRegen;
//            float ManaRegenOutFSRNoCast =  stats.Mp5 + replenishment + 0.2f * spiritRegen + 0.8f * spiritRegen;

            float ratio = .01f * calcOpts.FSRRatio;
            float manaRegen = ratio * ManaRegenInFSR + (1 - ratio) * ManaRegenOutFSR;
            #endregion

            float extraMana = 0f;

            #region Mana potion
            float extraManaFromPot = new int[] { 0, 1800, 2200, 2400, 4300 }[calcOpts.ManaPot];
            extraManaFromPot *= (stats.BonusManaPotion + 1f);
            extraMana += extraManaFromPot;
            #endregion

 /*           #region Old_Innervates
            // Wildebees : 20090308 : Reworked Innervate region to:
            //                          move the glyth scaling before the maximum check,
            //                          fixed the maximum mana check, 
            //                          catered for glyph without self-innervate,
            //                          add the out of FSR effect (effectively 4.5 to 5 times regen based on talents) and            
            //                          saves ManaFromInnervate for display in mp5 tooltip 
            float manaFromInnervate = spiritRegenPlusMDF * (5 - stats.SpellCombatManaRegeneration) * 4; // (Adds 400% + moves out of FSR), 20 seconds = 4 * mp5, 
            if (calculatedStats.LocalCharacter.DruidTalents.GlyphOfInnervate) //(calcOpts.glyphOfInnervate)
                manaFromInnervate *= (calcOpts.Innervates + 0.2f);      // Only works for 1 innervate in the fight
            else
                manaFromInnervate *= calcOpts.Innervates;
            #endregion */

            #region Innervates
            float numInnervates = (float) Math.Ceiling(calcOpts.FightDuration / 180.0f);      // 3 min CD, for 3:15 fight durations, will give unrealistic value of 2 innervates, not practical, since one will over-mana 
            float manaFromEachInnervate = 2.25f * calcOpts.Innervates;       // Use self innervate?

            if (calculatedStats.LocalCharacter.DruidTalents.GlyphOfInnervate)
            {
                manaFromEachInnervate += 0.45f;
            }

            manaFromEachInnervate *= Rawr.Tree.TreeConstants.BaseMana;

            float manaFromInnervates = manaFromEachInnervate * numInnervates;

            //TODO: Should we add this limit back in?
            // lets assume the mana return is maximally 95% of your mana
            // thus take the smaller value of 95% of mana pool and total mana regenerated
//            manaFromInnervate = Math.Min(manaFromInnervate, .95f * stats.Mana);

            extraMana += manaFromInnervates;
            #endregion


            #region Determine if Mana or GCD limited
//            if (calcOpts.patch3_1)
            {
                primaryHeal.calculateNewNaturesGrace(primaryHeal.CritPercent / 100f);
            }
/*            else
            {
                primaryHeal.calculateOldNaturesGrace(primaryHeal.CritPercent / 100f);
            } */

            float tpsHealing = 1f - (hotsCastTime + wgCastTime + swiftCastTime);

            // Determine if Mana or GCD limited
            float effectiveManaBurnTankHots = hotsMPS + wgMPS + swiftMPS - manaRegen / 5f;
            float manaAvailForPrimaryHeal = (extraMana + stats.Mana) - effectiveManaBurnTankHots * calcOpts.FightDuration;
            float primaryHealMpsAvail = manaAvailForPrimaryHeal / calcOpts.FightDuration;
            float mpsHealing = tpsHealing * primaryHeal.ManaCost / primaryHeal.CastTime;

            float unusedMana = 0f;
            float unusedCastTimeFrac = 0f;

            if (primaryHealMpsAvail < 0)
            {
                // Not enough mana to keep hots up
                // Mana limited
                primaryFrac = 0f;

                unusedCastTimeFrac = tpsHealing;

                // Need to calculate how much tank healing is penalised/reduced in order to not over value this setup
                float reduceFactor = (effectiveManaBurnTankHots / (effectiveManaBurnTankHots - primaryHealMpsAvail));

                // Try reducing everything equally as the simplest approach
                hotsHPS *= reduceFactor;
                trueHotsHPS *= reduceFactor;
                hotsMPS *= reduceFactor;
                hotsCastTime *= reduceFactor;
                hotsCastsPerMinute *= reduceFactor;
                hotsCritsPerMinute *= reduceFactor;
                hotsHealsPerMinute *= reduceFactor;
                castsPerMinute += reduceFactor;
                critsPerMinute += reduceFactor;
                healsPerMinute += reduceFactor;
                rejuvTicksPerMinute *= reduceFactor;

            }
            else if (primaryHealMpsAvail > mpsHealing)
            {
                // GCD limited
                unusedMana = (primaryHealMpsAvail - mpsHealing) * calcOpts.FightDuration;
                primaryFrac = 1.0f;
            }
            else
            {
                // Mana limited
                primaryFrac = primaryHealMpsAvail / mpsHealing;

                unusedCastTimeFrac = tpsHealing * (1.0f - primaryFrac);
            }

            #endregion


            #region Primary Heal

            float tpsHeal100 = tpsHealing;
            tpsHealing *= primaryFrac;
            float hpsHealing = 0;
            float hpsHeal100 = 0;
            // Wildebees: 20090221 : Changed check to be based on raidHealing, instead of just handling lifebloom and rejuv differently
            if (rotSettings.healTarget == HealTargetTypes.RaidHealing)
            {
                hpsHealing = tpsHealing * primaryHeal.HPCT;     // fraction of time casting primaryHeal multiplied
                //   by total healing by primaryHeal/cast time
                // This assumes full HoT duration is effective on raid
                //   members. 
                hpsHeal100 = tpsHeal100 * primaryHeal.HPCT;
            }
            else
            {
                hpsHealing = tpsHealing * primaryHeal.HPS;      // For single target healing, don't receive the full HoT effect
                hpsHeal100 = tpsHeal100 * primaryHeal.HPS;
            }
            mpsHealing = tpsHealing * primaryHeal.ManaCost / primaryHeal.CastTime;
            castsPerMinute += 60f * tpsHealing / primaryHeal.CastTime;
            critsPerMinute += 60f * tpsHealing / primaryHeal.CastTime * primaryHeal.CritPercent / 100f;
            float primaryCPM = 60f * tpsHealing / primaryHeal.CastTime;
            healsPerMinute += primaryCPM; // direct component
            if (primaryHeal is Regrowth || primaryHeal is Rejuvenation)
                healsPerMinute += primaryCPM * primaryHeal.PeriodicTicks; // hot component
            if (primaryHeal is Rejuvenation)
                rejuvTicksPerMinute += primaryCPM * primaryHeal.PeriodicTicks;
            #endregion

            #region Calculate total healing in the fight
            float mps = hotsMPS + mpsHealing + wgMPS + swiftMPS;
            float hps = hotsHPS + hpsHealing + wgHPS + swiftHPS;
            float TotalTime = calcOpts.FightDuration;
            float TimeToOOM = TotalTime;
            float EffectiveManaBurn = hotsMPS + mpsHealing + wgMPS + swiftMPS - manaRegen / 5f;
            if (EffectiveManaBurn > 0f) 
            {
                TimeToOOM = Math.Min((extraMana + stats.Mana) / EffectiveManaBurn, TotalTime);
            }

            float TotalMod = 1f;
            if (TotalTime > TimeToOOM + 1.0f)
            {       // Should only happen if running OOM from tank HoTs and even attempts to scale back there wasn't enough
                float TimeToRegenAll = 5f * stats.Mana / ManaRegenOutFSR;
                float TimeToBurnAll = stats.Mana / EffectiveManaBurn;
                //CvRFraction = (stats.Mana / EffectiveManaBurn) / TotalCycle;
                TotalMod = 1f - (TotalTime - TimeToOOM) / (TimeToRegenAll + TimeToBurnAll);
                hps *= TotalMod;        // Penalise hps
            }
            #endregion

            #region Convert to be ready for points
            float MaxHPS = hotsHPS + hpsHeal100 + wgHPS + swiftHPS;

            #endregion

            return new Rotation() {
                HPS = hps,
                MaxHPS = MaxHPS,
                MPS = mps,
                HPSFromPrimary = hpsHealing,
                HPSFromHots = hotsHPS,
                HPSFromTrueHots = trueHotsHPS,
                HPSFromWildGrowth = wgHPS,
                HPSFromSwiftmend = swiftHPS,
                MPSFromPrimary = mpsHealing,
                MPSFromHots = hotsMPS,
                MPSFromWildGrowth = wgMPS,
                MPSFromSwiftmend = swiftMPS,
                HotsFraction = hotsCastTime,
                CastsPerMinute = castsPerMinute,
                CritsPerMinute = critsPerMinute,
                HealsPerMinute = healsPerMinute,
                ManaPer5In5SR = ManaRegenInFSR,
                ManaPer5Out5SR = ManaRegenOutFSR,
                ManaPer5InRotation = manaRegen,
                ManaFromEachInnervate = manaFromEachInnervate,
                ManaFromInnervates = manaFromInnervates / calcOpts.FightDuration * 5.0f,
                ManaFromSpirit = spiritRegen,
                ManaFromMP5 = stats.Mp5,
                ManaFromPotions = extraManaFromPot, 
                TimeToOOM = TimeToOOM,
                TotalTime = TotalTime,
                TotalHealing = TimeToOOM * hps * TotalMod,
                TotalCastsPerMinute = castsPerMinute * TotalMod,
                TotalCritsPerMinute = critsPerMinute * TotalMod,
                TotalHealsPerMinute = healsPerMinute * TotalMod,
                RejuvenationHealsPerMinute = rejuvTicksPerMinute,
                ReplenishRegen = replenishment,
                UnusedMana = unusedMana,
                UnusedCastTimeFrac = unusedCastTimeFrac,
                rotSettings = rotSettings,
            };
        }

        protected RotationSettings predefinedRotation(int rotation, Stats stats, CalculationOptionsTree calcOpts, CharacterCalculationsTree calculatedStats)
        {
//            Rotation result = null;
            RotationSettings settings = new RotationSettings();

            settings.lifeBloomFastStack = false;

            switch (rotation)
            {
                case 0:
                default:
                    // 1 Tank (RJ/RG/LB/N*)
                    {
                        settings.noTanks = 1;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = true;
                        settings.lifeBloomStackSize = 4;
                        settings.primaryHeal = SpellList.Nourish;
                        settings.healTarget = HealTargetTypes.TankHealing;

                    }
                    break;
                case 1:
                    // 1 Tank (RJ/RG/slow 3xLB/N*)
                    {
                        settings.lifeBloomFastStack = false;
                        settings.noTanks = 1;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = true;
                        settings.lifeBloomStackSize = 3;
                        settings.primaryHeal = SpellList.Nourish;
                        settings.healTarget = HealTargetTypes.TankHealing;

                    }
                    break;
                case 2:
                    // 1 Tank (RJ/RG/fast 3xLB/N*)
                    {
                        settings.lifeBloomFastStack = true;
                        settings.noTanks = 1;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = true;
                        settings.lifeBloomStackSize = 3;
                        settings.primaryHeal = SpellList.Nourish;
                        settings.healTarget = HealTargetTypes.TankHealing;

                    }
                    break;
                case 3:
                    // 2 Tanks (RJ/RG/LB/N*)
                    {
                        settings.noTanks = 2;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = true;
                        settings.lifeBloomStackSize = 4;
                        settings.primaryHeal = SpellList.Nourish;
                        settings.healTarget = HealTargetTypes.TankHealing;
                    }
                    break;
                case 4:
                    // 2 Tank (RJ/RG/slow 3xLB/N*)
                    {
                        settings.lifeBloomFastStack = false;
                        settings.noTanks = 2;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = true;
                        settings.lifeBloomStackSize = 3;
                        settings.primaryHeal = SpellList.Nourish;
                        settings.healTarget = HealTargetTypes.TankHealing;

                    }
                    break;
                case 5:
                    // 2 Tank (RJ/RG/fast 3xLB/N*)
                    {
                        settings.lifeBloomFastStack = true;
                        settings.noTanks = 2;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = true;
                        settings.lifeBloomStackSize = 3;
                        settings.primaryHeal = SpellList.Nourish;
                        settings.healTarget = HealTargetTypes.TankHealing;

                    }
                    break;

                case 6:
                    // 1 Tank (RJ/RG/LB/HT*)
                    {
                        settings.noTanks = 1;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = true;
                        settings.lifeBloomStackSize = 4;
                        settings.primaryHeal = SpellList.HealingTouch;
                        settings.healTarget = HealTargetTypes.TankHealing;
                    }
                    break;
                case 7:
                    // 2 Tanks (RJ/RG/LB/HT*)
                    {
                        settings.noTanks = 2;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = true;
                        settings.lifeBloomStackSize = 4;
                        settings.primaryHeal = SpellList.HealingTouch;
                        settings.healTarget = HealTargetTypes.TankHealing;
                    }
                    break;
                case 8:
                    // 1 Tank (RJ/RG/LB/RG*)
                    {
                        settings.noTanks = 1;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = true;
                        settings.lifeBloomStackSize = 4;
                        settings.primaryHeal = SpellList.Regrowth;
                        settings.healTarget = HealTargetTypes.TankHealing;
                    }
                    break;
                case 9:
                    // 1 Tanks (RJ/RG/slow 3xLB*/RG*)
                    {
                        settings.lifeBloomFastStack = false;
                        settings.noTanks = 1;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = true;
                        settings.lifeBloomStackSize = 3;
                        settings.primaryHeal = SpellList.Regrowth;
                        settings.healTarget = HealTargetTypes.TankHealing;
                    }
                    break;
                case 10:
                    // 1 Tanks (RJ/RG/fast 3xLB*/RG*)
                    {
                        settings.lifeBloomFastStack = true;
                        settings.noTanks = 1;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = true;
                        settings.lifeBloomStackSize = 3;
                        settings.primaryHeal = SpellList.Regrowth;
                        settings.healTarget = HealTargetTypes.TankHealing;
                    }
                    break;

                case 11:
                    // 2 Tanks (RJ/RG/LB/RG*)
                    {
                        settings.noTanks = 2;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = true;
                        settings.lifeBloomStackSize = 4;
                        settings.primaryHeal = SpellList.Regrowth;
                        settings.healTarget = HealTargetTypes.TankHealing;
                    }
                    break;
                case 12:
                    // 2 Tanks (RJ/RG/slow 3xLB*/RG*)
                    {
                        settings.lifeBloomFastStack = false;
                        settings.noTanks = 2;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = true;
                        settings.lifeBloomStackSize = 3;
                        settings.primaryHeal = SpellList.Regrowth;
                        settings.healTarget = HealTargetTypes.TankHealing;
                    }
                    break;
                case 13:
                    // 2 Tanks (RJ/RG/fast 3xLB*/RG*)
                    {
                        settings.lifeBloomFastStack = true;
                        settings.noTanks = 2;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = true;
                        settings.lifeBloomStackSize = 3;
                        settings.primaryHeal = SpellList.Regrowth;
                        settings.healTarget = HealTargetTypes.TankHealing;
                    }
                    break;

                case 14:
                    // RG Raid (1 Tank RJ/LBStack)
                    {
                        settings.noTanks = 1;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = false;
                        settings.lifeBloomStackSize = 4;
                        settings.primaryHeal = SpellList.Regrowth;
                        settings.healTarget = HealTargetTypes.RaidHealing;
                    }
                    break;
                case 15:
                    // RG Raid (1 Tank RJ/1xLB)
                    {
                        settings.lifeBloomFastStack = false;

                        settings.noTanks = 1;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = false;
                        settings.lifeBloomStackSize = 3;
                        settings.primaryHeal = SpellList.Regrowth;
                        settings.healTarget = HealTargetTypes.RaidHealing;
                    }
                    break;
                case 16:
                    // RG Raid (2 Tanks RJ/LBStack)
                    {
                        settings.noTanks = 2;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = false;
                        settings.lifeBloomStackSize = 4;
                        settings.primaryHeal = SpellList.Regrowth;
                        settings.healTarget = HealTargetTypes.RaidHealing;
                    }
                    break;
                case 17:
                    // RG Raid (2 Tanks RJ/Slow3xLB)
                    {
                        settings.lifeBloomFastStack = false;
                        settings.noTanks = 2;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = false;
                        settings.lifeBloomStackSize = 3;
                        settings.primaryHeal = SpellList.Regrowth;
                        settings.healTarget = HealTargetTypes.RaidHealing;
                    }
                    break;
                case 18:
                    // RJ Raid (1 Tank RJ/LBStack)
                    {
                        settings.noTanks = 1;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = false;
                        settings.lifeBloomStackSize = 4;
                        settings.primaryHeal = SpellList.Rejuvenation;
                        settings.healTarget = HealTargetTypes.RaidHealing;
                    }
                    break;
                case 19:
                    // RJ Raid (1 Tank RJ/3xLB)
                    {
                        settings.lifeBloomFastStack = false;
                        settings.noTanks = 1;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = false;
                        settings.lifeBloomStackSize = 3;
                        settings.primaryHeal = SpellList.Rejuvenation;
                        settings.healTarget = HealTargetTypes.RaidHealing;
                    }
                    break;
                case 20:
                    // RJ Raid (2 Tanks RJ/LBStack)
                    {
                        settings.noTanks = 2;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = false;
                        settings.lifeBloomStackSize = 4;
                        settings.primaryHeal = SpellList.Rejuvenation;
                        settings.healTarget = HealTargetTypes.RaidHealing;

                    }
                    break;
                case 21:
                    // RJ Raid (2 Tanks RJ/3xLB)
                    {
                        settings.lifeBloomFastStack = false;
                        settings.noTanks = 2;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = false;
                        settings.lifeBloomStackSize = 3;
                        settings.primaryHeal = SpellList.Rejuvenation;
                        settings.healTarget = HealTargetTypes.RaidHealing;

                    }
                    break;
                case 22:
                    // N Raid (1 Tank RJ/LBStack)
                    {
                        settings.noTanks = 1;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = false;
                        settings.lifeBloomStackSize = 4;
                        settings.primaryHeal = SpellList.Nourish;
                        settings.healTarget = HealTargetTypes.RaidHealing;
                    }
                    break;
                case 23:
                    // N Raid (1 Tank RJ/3xLB)
                    {
                        settings.lifeBloomFastStack = false;
                        settings.noTanks = 1;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = false;
                        settings.lifeBloomStackSize = 3;
                        settings.primaryHeal = SpellList.Nourish;
                        settings.healTarget = HealTargetTypes.RaidHealing;
                    }
                    break;
                case 24:
                    // N Raid (2 Tanks RJ/LBStack)
                    {
                        settings.noTanks = 2;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = false;
                        settings.lifeBloomStackSize = 4;
                        settings.primaryHeal = SpellList.Nourish;
                        settings.healTarget = HealTargetTypes.RaidHealing;

                    }
                    break;
                case 25:
                    // N Raid (2 Tanks RJ/3xLB)
                    {
                        settings.lifeBloomFastStack = false;
                        settings.noTanks = 2;
                        settings.rejuvOnTank = true;
                        settings.rgOnTank = false;
                        settings.lifeBloomStackSize = 3;
                        settings.primaryHeal = SpellList.Nourish;
                        settings.healTarget = HealTargetTypes.RaidHealing;

                    }
                    break;
                case 26:
                    // N spam
                    {
                        settings.noTanks = 0;
                        settings.rejuvOnTank = false;
                        settings.rgOnTank = false;
                        settings.lifeBloomStackSize = 0;
                        settings.primaryHeal = SpellList.Nourish;
                        settings.healTarget = HealTargetTypes.TankHealing;

                    }
                    break;
                case 27:
                    // HT spam
                    {
                        settings.noTanks = 0;
                        settings.rejuvOnTank = false;
                        settings.rgOnTank = false;
                        settings.lifeBloomStackSize = 0;
                        settings.primaryHeal = SpellList.HealingTouch;
                        settings.healTarget = HealTargetTypes.TankHealing;

                    }
                    break;
                case 28:
                    // RG spam on tank
                    {
                        settings.noTanks = 1;
                        settings.rejuvOnTank = false;
                        settings.rgOnTank = true;
                        settings.lifeBloomStackSize = 0;
                        settings.primaryHeal = SpellList.Regrowth;
                        settings.healTarget = HealTargetTypes.TankHealing;
                    }
                    break;
                case 29:
                    // RG spam on raid
                    {
                        settings.noTanks = 0;
                        settings.rejuvOnTank = false;
                        settings.rgOnTank = false;
                        settings.lifeBloomStackSize = 0;
                        settings.primaryHeal = SpellList.Regrowth;
                        settings.healTarget = HealTargetTypes.RaidHealing;
                    }
                    break;
                case 30:
                    // Rej spam
                    {
                        settings.noTanks = 0;
                        settings.rejuvOnTank = false;
                        settings.rgOnTank = false;
                        settings.lifeBloomStackSize = 0;
                        settings.primaryHeal = SpellList.Rejuvenation;
                        settings.healTarget = HealTargetTypes.RaidHealing;
                    }
                    break;

            }

            settings.SwiftmendPerMin = calcOpts.SwiftmendPerMinute;
            settings.WildGrowthPerMinute = calcOpts.WildGrowthPerMinute;
            return settings;
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            CalculationOptionsTree calcOpts = (CalculationOptionsTree)character.CalculationOptions;
            CharacterCalculationsTree calculatedStats = new CharacterCalculationsTree();
            calculatedStats.LocalCharacter = character;

            calculatedStats.BasicStats = GetCharacterStats(character, additionalItem);

            #region Rotations
            Stats combinedStats = calculatedStats.BasicStats;
            Stats stats = calculatedStats.BasicStats;
            float ExtraHPS = 0f;
            RotationSettings settings = predefinedRotation(calcOpts.Rotation, stats, calcOpts, calculatedStats);
            if (calcOpts.Rotation == 99)
            {
                settings = calcOpts.customRotationSettings;
            }

            Rotation rot = SimulateHealing(calculatedStats, stats, calcOpts, settings);
            int nPasses = 3, k;
            for (k = 0; k < nPasses; k++)
            {
                Stats procs = getTrinketStats(calculatedStats.LocalCharacter, stats,
                    rot.TotalTime, 60f / rot.TotalCastsPerMinute,
                    60f / rot.TotalHealsPerMinute, rot.TotalCritsPerMinute / rot.TotalCastsPerMinute,
                    60 / rot.RejuvenationHealsPerMinute, 
                    out ExtraHPS);

                
                // Create a new stats instance that uses the proc effects
                combinedStats = GetCharacterStats(character, additionalItem, procs);
                rot = SimulateHealing(calculatedStats, combinedStats, calcOpts, settings);  
            }
            calculatedStats.Simulation = rot;
            calculatedStats.BasicStats = combinedStats;     // Replace BasicStats to get Spirit while casting included
            #endregion

            calculatedStats.BurstPoints = rot.MaxHPS + ExtraHPS;
            calculatedStats.SustainedPoints = (rot.TotalHealing + ExtraHPS * rot.TotalTime) / rot.TotalTime;

            #region Survival Points
            float DamageReduction = StatConversion.GetArmorDamageReduction(83, combinedStats.Armor, 0, 0, 0);
            calculatedStats.SurvivalPoints = combinedStats.Health / (1.0f - DamageReduction) / 100.0f * calcOpts.SurvValuePer100;
            #endregion

/*
            // Penalty
            if (rot.TimeToOOM < rot.TotalTime)
            {
                float frac = rot.TimeToOOM / rot.TotalTime;
                float mod = (float)Math.Pow(frac, 1 + 0.05f ) / frac;
                calculatedStats.SustainedPoints *= mod;
//                if (calcOpts.PenalizeEverything)
                {
                    calculatedStats.BurstPoints *= mod;
                }
            }

 */
            // ADJUST POINT VALUE (BURST SUSTAINED RATIO)
            float bsRatio = .01f * calcOpts.BSRatio;
            calculatedStats.BurstPoints *= (1f-bsRatio) * 2;
            calculatedStats.SustainedPoints *= bsRatio * 2;

            calculatedStats.OverallPoints = calculatedStats.BurstPoints + calculatedStats.SustainedPoints + calculatedStats.SurvivalPoints;

            //calcOpts.calculatedStats = calculatedStats;
            return calculatedStats;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            return GetCharacterStats(character, additionalItem, new Stats());
        }

        public Stats GetCharacterStats(Character character, Item additionalItem, Stats statsProcs)
        {
            CalculationOptionsTree calcOpts = character.CalculationOptions as CalculationOptionsTree;

            Stats statsRace = BaseStats.GetBaseStats(character.Level, character.Class, character.Race); //( GetRacialBaseStats(character.Race);
            TreeConstants.BaseMana = statsRace.Mana;    // Setup TreeConstant

            Stats statsTalents = new Stats();

            statsTalents.BonusAgilityMultiplier = 0.01f * character.DruidTalents.SurvivalOfTheFittest * 2;
            statsTalents.BonusIntellectMultiplier = 0.01f * character.DruidTalents.SurvivalOfTheFittest * 2;
            statsTalents.BonusSpiritMultiplier = 0.01f * character.DruidTalents.SurvivalOfTheFittest * 2;
            statsTalents.BonusStaminaMultiplier = 0.01f * character.DruidTalents.SurvivalOfTheFittest * 2;
            statsTalents.BonusStrengthMultiplier = 0.01f * character.DruidTalents.SurvivalOfTheFittest * 2;

            statsTalents.SpellHaste = 0.01f * character.DruidTalents.CelestialFocus;

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsTotal = statsBaseGear + statsBuffs + statsRace + statsTalents + statsProcs;

            statsTotal.Agility = (float)Math.Floor((statsTotal.Agility) * (1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Agility = (float)Math.Floor((statsTotal.Agility) * (1 + 0.01f * character.DruidTalents.ImprovedMarkOfTheWild));
            statsTotal.Stamina = (float)Math.Floor((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Stamina = (float)Math.Floor((statsTotal.Stamina) * (1 + 0.01f * character.DruidTalents.ImprovedMarkOfTheWild));
            statsTotal.Intellect = (float)Math.Floor((statsTotal.Intellect) * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Intellect = (float)Math.Round((statsTotal.Intellect) * (1 + character.DruidTalents.HeartOfTheWild * 0.04f));
            statsTotal.Intellect = (float)Math.Floor((statsTotal.Intellect) * (1 + 0.01f * character.DruidTalents.ImprovedMarkOfTheWild));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + character.DruidTalents.LivingSpirit * 0.05f));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + 0.01f * character.DruidTalents.ImprovedMarkOfTheWild));
            statsTotal.Armor = (float)Math.Floor((statsTotal.Armor) * (1 + 0.8f * character.DruidTalents.ImprovedTreeOfLife));

            // Removed, since proc effects added by GetCharacterCalculations
//            statsTotal.ExtraSpiritWhileCasting = (float)Math.Floor((statsTotal.ExtraSpiritWhileCasting) * (1 + statsTotal.BonusSpiritMultiplier) * (1 + 0.01f * character.DruidTalents.ImprovedMarkOfTheWild) * (1 + character.DruidTalents.LivingSpirit * 0.05f));
            statsTotal.ExtraSpiritWhileCasting = 0f;

            /*if (statsTotal.GreatnessProc>0) {
                // Highest stat in combat
                if (statsTotal.Spirit + statsTotal.ExtraSpiritWhileCasting > statsTotal.Intellect)
                {
                    // spirit proc (Greatness)
                    float extraSpi = statsTotal.GreatnessProc * 15f / 50f;
                    extraSpi *= 1 + statsTotal.BonusSpiritMultiplier;
                    extraSpi *= 1 + character.DruidTalents.LivingSpirit * 0.05f;
                    extraSpi *= 1 + character.DruidTalents.ImprovedMarkOfTheWild * 0.01f;
                    statsTotal.Spirit += (float)Math.Floor(extraSpi);
                }
                else {
                    // int proc (Greatness)
                    float extraInt = statsTotal.GreatnessProc * 15f / 50f;
                    extraInt *= 1 + statsTotal.BonusIntellectMultiplier;
                    extraInt *= 1 + character.DruidTalents.HeartOfTheWild * 0.04f;
                    extraInt *= 1 + character.DruidTalents.ImprovedMarkOfTheWild * 0.01f;
                    statsTotal.Intellect += (float)Math.Floor(extraInt);
                }
            }*/

            if (statsTotal.HighestStat > 0)
            {
                // Highest stat in combat
                if (statsTotal.Spirit + statsTotal.ExtraSpiritWhileCasting > statsTotal.Intellect)
                {
                    // spirit proc (Greatness)
                    float extraSpi = statsTotal.HighestStat;
                    extraSpi *= 1 + statsTotal.BonusSpiritMultiplier;
                    extraSpi *= 1 + character.DruidTalents.LivingSpirit * 0.05f;
                    extraSpi *= 1 + character.DruidTalents.ImprovedMarkOfTheWild * 0.01f;
                    statsTotal.Spirit += (float)Math.Floor(extraSpi);
                }
                else
                {
                    // int proc (Greatness)
                    float extraInt = statsTotal.HighestStat;
                    extraInt *= 1 + statsTotal.BonusIntellectMultiplier;
                    extraInt *= 1 + character.DruidTalents.HeartOfTheWild * 0.04f;
                    extraInt *= 1 + character.DruidTalents.ImprovedMarkOfTheWild * 0.01f;
                    statsTotal.Intellect += (float)Math.Floor(extraInt);
                }
            }

            statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower + statsTotal.Intellect * character.DruidTalents.LunarGuidance * 0.04); //LunarGuidance, 4% per Point
            statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower + (statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit) + (statsTotal.Intellect * character.DruidTalents.LunarGuidance * 0.04) + (character.DruidTalents.NurturingInstinct * 0.35f * statsTotal.Agility));

//            statsTotal.Mana = statsTotal.Mana + ((statsTotal.Intellect - 20f) * 15f + 20f); //don't know why, but it's right..
            statsTotal.Mana = statsTotal.Mana + StatConversion.GetManaFromIntellect(statsTotal.Intellect);
            statsTotal.Mana *= (1f + statsTotal.BonusManaMultiplier);

//            statsTotal.Health = (float)Math.Round(statsTotal.Health + (statsTotal.Stamina - 20f) * 10f + 20f);
            statsTotal.Health = (float)Math.Round(statsTotal.Health + StatConversion.GetHealthFromStamina(statsTotal.Stamina));
            statsTotal.Mp5 += (float)Math.Floor(statsTotal.Intellect * (character.DruidTalents.Dreamstate > 0 ? character.DruidTalents.Dreamstate * 0.03f + 0.01f : 0f));

//            statsTotal.SpellCrit = (float)Math.Round((statsTotal.Intellect * 0.006f) + (statsTotal.CritRating / 45.906f) + (statsTotal.SpellCrit*100.0f) + 1.85 + character.DruidTalents.NaturalPerfection, 2);
            statsTotal.SpellCrit = (float)Math.Round( (StatConversion.GetSpellCritFromIntellect(statsTotal.Intellect) + StatConversion.GetSpellCritFromRating(statsTotal.CritRating) + (statsTotal.SpellCrit) + 0.01f * character.DruidTalents.NaturalPerfection) * 100f, 2);
            statsTotal.SpellCombatManaRegeneration += 0.5f / 3f * character.DruidTalents.Intensity;

            // SpellPower (actually healing only, but we have no damaging spells, so np)
            statsTotal.SpellPower += ((statsTotal.Spirit + statsTotal.ExtraSpiritWhileCasting) * character.DruidTalents.ImprovedTreeOfLife * 0.05f);

            return statsTotal;
        }

        /* Removed, since we rather use BaseStats
        private static Stats GetRacialBaseStats(CharacterRace race)
        {
            Stats statsRace = new Stats();
            statsRace.Mana = TreeConstants.BaseMana; 

            if (race == CharacterRace.NightElf)
            {
                statsRace.Health = 7417;
                statsRace.Strength = 86f;
                statsRace.Agility = 87f;
                statsRace.Stamina = 97f;
                statsRace.Intellect = 143f;
                statsRace.Spirit = 182f;
            }
            else
            {
                // NEED TO BE CHECKED.
                statsRace.Health = 7599f;
                //statsRace.Strength = 94f;
                statsRace.Agility = 77f;
                statsRace.Stamina = 100f;
                statsRace.Intellect = 138f;
                statsRace.Spirit = 161f;
            }
            return statsRace;
        }*/

        private ComparisonCalculationTree getRotationData(Character character, int rotation, String rotationName)
        {
            CalculationOptionsTree calcOpts = character.CalculationOptions as CalculationOptionsTree;
            int old = calcOpts.Rotation;
            calcOpts.Rotation = rotation;
            CharacterCalculationsTree calcs = GetCharacterCalculations(character) as CharacterCalculationsTree;
            calcOpts.Rotation = old;
            return new ComparisonCalculationTree()
            {
                CharacterItems = character.GetItems(),
                Name = rotationName,
                OverallPoints = calcs.OverallPoints,
                BurstPoints = calcs.BurstPoints,
                SustainedPoints = calcs.SustainedPoints,
            };
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            switch (chartName)
            {
                case "Spell rotations":
                    List<ComparisonCalculationBase> comparisonsDPS = new List<ComparisonCalculationBase>();


                    for (int i = 0; i < PredefRotations.Length; i++)
                    {
                        comparisonsDPS.Add(getRotationData(character, i, PredefRotations[i]));
                    }   


                    // Tank Healing Rotations
                    RotationSettings rotSettings = new RotationSettings();
                    string rotName;
                    rotSettings.healTarget = HealTargetTypes.TankHealing;
                    rotSettings.SwiftmendPerMin = ((CalculationOptionsTree)(character.CalculationOptions)).SwiftmendPerMinute;
                    rotSettings.WildGrowthPerMinute = ((CalculationOptionsTree)(character.CalculationOptions)).WildGrowthPerMinute;

                    for (SpellList spList = SpellList.HealingTouch; spList < SpellList.Rejuvenation; spList++)
                    {
                        rotSettings.primaryHeal = spList;
                        for (int noTanks = 1; noTanks < 3; noTanks++)
                        {
                            rotSettings.noTanks = noTanks;
                            for (int lbMode = 0; lbMode < 7; lbMode++)
                            {
                                rotName = "Tx" + noTanks.ToString() + " " + spList.ToString() + " (RG+RJ";
                                switch (lbMode)
                                {
                                    case (0):
                                        {
                                            rotSettings.lifeBloomStackSize = 0;
//                                            rotName += "No LB";
                                            break;
                                        }
                                    case (1):
                                        {
                                            rotSettings.lifeBloomStackSize = 1;
                                            rotName += "+1xLB";
                                            break;
                                        }
                                    case (2):
                                        {
                                            rotSettings.lifeBloomStackSize = 2;
                                            rotSettings.lifeBloomFastStack = false;
                                            rotName += "+Slow 2xLB";
                                            break;
                                        }
                                    case (3):
                                        {
                                            rotSettings.lifeBloomStackSize = 3;
                                            rotSettings.lifeBloomFastStack = false;
                                            rotName += "+Slow 3xLB";
                                            break;
                                        }
                                    case (4):
                                        {
                                            rotSettings.lifeBloomStackSize = 4;
                                            rotSettings.lifeBloomFastStack = false;
                                            rotName += "+Rolling LB";
                                            break;
                                        }
                                    case (5):
                                        {
                                            rotSettings.lifeBloomStackSize = 2;
                                            rotSettings.lifeBloomFastStack = true;
                                            rotName += "+Fast 2xLB";
                                            break;
                                        }
                                    case (6):
                                        {
                                            rotSettings.lifeBloomStackSize = 3;
                                            rotSettings.lifeBloomFastStack = true;
                                            rotName += "+Fast 3xLB";
                                            break;
                                        }

                                }

                                rotSettings.rgOnTank = true;
                                rotSettings.rejuvOnTank = true;
                                rotName += ")";
                                ((CalculationOptionsTree)(character.CalculationOptions)).customRotationSettings = rotSettings;
                                comparisonsDPS.Add(getRotationData(character, 99, rotName));

                            }
                        }
                    }

                    // Raid Healing Rotations
                    rotSettings.healTarget = HealTargetTypes.RaidHealing;
                    for (SpellList spList = SpellList.HealingTouch; spList <= SpellList.Rejuvenation; spList++)
                    {
                        rotSettings.primaryHeal = spList;

                        rotSettings.noTanks = 0;
                        ((CalculationOptionsTree)(character.CalculationOptions)).customRotationSettings = rotSettings;
                        rotName = "Raid only " + spList.ToString();
                        comparisonsDPS.Add(getRotationData(character, 99, rotName));

                        for (int noTanks = 1; noTanks < 3; noTanks++)
                        {
                            rotSettings.noTanks = noTanks;
                            for (int lbMode = 0; lbMode < 7; lbMode++)
                            {
                                rotName = "Raid+Tx" + noTanks.ToString() + " " + spList.ToString() + " (RG+RJ";
                                switch (lbMode)
                                {
                                    case (0):
                                        {
                                            rotSettings.lifeBloomStackSize = 0;
                                            //rotName += "No LB";
                                            break;
                                        }
                                    case (1):
                                        {
                                            rotSettings.lifeBloomStackSize = 1;
                                            rotName += "+1xLB";
                                            break;
                                        }
                                    case (2):
                                        {
                                            rotSettings.lifeBloomStackSize = 2;
                                            rotSettings.lifeBloomFastStack = false;
                                            rotName += "+Slow 2xLB";
                                            break;
                                        }
                                    case (3):
                                        {
                                            rotSettings.lifeBloomStackSize = 3;
                                            rotSettings.lifeBloomFastStack = false;
                                            rotName += "+Slow 3xLB";
                                            break;
                                        }
                                    case (4):
                                        {
                                            rotSettings.lifeBloomStackSize = 4;
                                            rotSettings.lifeBloomFastStack = false;
                                            rotName += "+Rolling LB";
                                            break;
                                        }
                                    case (5):
                                        {
                                            rotSettings.lifeBloomStackSize = 2;
                                            rotSettings.lifeBloomFastStack = true;
                                            rotName += "+Fast 2xLB";
                                            break;
                                        }
                                    case (6):
                                        {
                                            rotSettings.lifeBloomStackSize = 3;
                                            rotSettings.lifeBloomFastStack = true;
                                            rotName += "+Fast 3xLB";
                                            break;
                                        }

                                }

                                rotSettings.rgOnTank = true;
                                rotSettings.rejuvOnTank = true;
                                rotName += ")";
                                ((CalculationOptionsTree)(character.CalculationOptions)).customRotationSettings = rotSettings;
                                comparisonsDPS.Add(getRotationData(character, 99, rotName));

                            }
                        }
                    }

                    return comparisonsDPS.ToArray();
                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
//            return SpecialEffects.GetRelevantStats(stats) + new Stats()
            {
                #region Base Stats
                //Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                SpellPower = stats.SpellPower,
                CritRating = stats.CritRating,
                HasteRating = stats.HasteRating,
                SpellHaste = stats.SpellHaste,
                SpellCrit = stats.SpellCrit,
                //Health = stats.Health,
                Mana = stats.Mana,
                Mp5 = stats.Mp5,
                Armor = stats.Armor,
                Stamina = stats.Stamina,
                #endregion
                #region Trinkets
                BonusManaPotion = stats.BonusManaPotion,
//                GreatnessProc = stats.GreatnessProc,
                HighestStat = stats.HighestStat,
                #endregion
                #region Idols and Sets
                TreeOfLifeAura = stats.TreeOfLifeAura,
                ReduceRegrowthCost = stats.ReduceRegrowthCost,
                ReduceRejuvenationCost = stats.ReduceRejuvenationCost, //Idol of Budding Life (-36)
                RejuvenationHealBonus = stats.RejuvenationHealBonus,
                RejuvenationSpellpower = stats.RejuvenationSpellpower,
                LifebloomTickHealBonus = stats.LifebloomTickHealBonus,
                LifebloomFinalHealBonus = stats.LifebloomFinalHealBonus,
                ReduceHealingTouchCost = stats.ReduceHealingTouchCost, //Refered to Idol of Longevity, which grant 25mana on HT cast?
                HealingTouchFinalHealBonus = stats.HealingTouchFinalHealBonus, //HealingTouchHealBonus?
                RegrowthExtraTicks = stats.RegrowthExtraTicks, //T5 (2) Bonus
                BonusHealingTouchMultiplier = stats.BonusHealingTouchMultiplier,  //T6 (4) Bonus
                LifebloomCostReduction = stats.LifebloomCostReduction,  //T7 (2) Bonus
                NourishBonusPerHoT = stats.NourishBonusPerHoT,          //T7 (4) Bonus
                RejuvenationInstantTick = stats.RejuvenationInstantTick, //T8 (4) Bonus
                NourishCritBonus = stats.NourishCritBonus,              // T9 (2) Bonus
                RejuvenationCrit = stats.RejuvenationCrit,              // T9 (4) Bonus
                NourishSpellpower = stats.NourishSpellpower,
                SpellsManaReduction = stats.SpellsManaReduction,
                HealingOmenProc = stats.HealingOmenProc,
//                ManacostReduceWithin15OnHealingCast = stats.ManacostReduceWithin15OnHealingCast,
                SwiftmendBonus = stats.SwiftmendBonus,
                #endregion
                #region Gems
                BonusCritHealMultiplier = stats.BonusCritHealMultiplier,
                BonusManaMultiplier = stats.BonusManaMultiplier,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                #endregion
            };

            
            foreach (Rawr.SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use || 
                    effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.SpellHit
                    || effect.Trigger == Trigger.HealingSpellCast || effect.Trigger == Trigger.HealingSpellCrit || effect.Trigger == Trigger.HealingSpellHit)
                {
                    if (HasRelevantSpecialEffectStats(effect.Stats))
                    {
                        s.AddSpecialEffect(effect);
                    }
                    else
                    {
                        // Trigger seems relevant, but effect not. Sounds weird, probably DPS bonus on Use or general SpellCast ...
                    }

                }
            }
            return s;
        }

        public bool HasRelevantSpecialEffectStats(Stats stats)
        {
            return (stats.Intellect + stats.Spirit + stats.SpellPower + stats.CritRating + stats.HasteRating + stats.ManaRestore
                   + stats.Mp5 + stats.Healed + stats.HighestStat + stats.BonusHealingReceived + stats.SwiftmendBonus + stats.HealingOmenProc) > 0;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            foreach (Rawr.SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use ||
                    effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.SpellCrit 
                    || effect.Trigger == Trigger.HealingSpellCast || effect.Trigger == Trigger.HealingSpellCrit || effect.Trigger == Trigger.HealingSpellHit)
                {
                    if (HasRelevantSpecialEffectStats(effect.Stats))
                      return true;
                    else
                    {
                        // Trigger seems relevant, but effect not. Sounds weird, probably DPS bonus on Use or general SpellCast ...
                    }

                }
            }

            if (stats.Intellect + stats.Spirit + stats.Mp5 + stats.SpellPower + stats.Mana + stats.CritRating + stats.SpellCrit
                + stats.HasteRating + stats.SpellHaste + stats.BonusSpellPowerMultiplier
                + stats.BonusSpiritMultiplier + stats.BonusIntellectMultiplier + stats.BonusStaminaMultiplier
                + stats.BonusManaPotion + stats.ExtraSpiritWhileCasting
                + stats.BonusCritHealMultiplier + stats.BonusManaMultiplier
                //+ stats.GreatnessProc 
                + stats.HighestStat + stats.TreeOfLifeAura + stats.ReduceRegrowthCost
                + stats.ReduceRejuvenationCost + stats.RejuvenationSpellpower + stats.RejuvenationHealBonus 
                + stats.LifebloomTickHealBonus + stats.LifebloomFinalHealBonus + stats.ReduceHealingTouchCost
                + stats.HealingTouchFinalHealBonus + stats.LifebloomCostReduction + stats.NourishBonusPerHoT +
                stats.RejuvenationInstantTick + stats.NourishSpellpower + stats.SpellsManaReduction + 
//                stats.ManacostReduceWithin15OnHealingCast +
                stats.HealingOmenProc + stats.SwiftmendBonus + stats.NourishCritBonus + stats.RejuvenationCrit +
                stats.Armor + stats.Stamina
                > 0)
                return true;

            /*
            if (stats.Strength + stats.Agility + stats.AttackPower > 0)
                return false;
            */

/*          Not sure why this was in here  
 *          if (stats.SpellCombatManaRegeneration == 0.3f)
                return false;
            */
            return (stats.SpellCombatManaRegeneration + stats.Intellect > 0);
        }

        /* Wildebees 20090407 : Overload base function to disable all enchants on OffHand for tree druids */
        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot)
        {
            if (slot == ItemSlot.OffHand) 
                return false;

            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsTree));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsTree calcOpts = serializer.Deserialize(reader) as CalculationOptionsTree;
            return calcOpts;
        }

 /*     Removed since we rather use StatConversion.GetSpiritRegen  
  *     public float CalculateManaRegen(float intel, float spi)
        {
            float baseRegen = 0.005575f;  // TODO: Decide if we should put 0.6f change in 3.1 in here, or is this function for out of combat regen
            return (float)Math.Round(5f * (0.001f + (float)Math.Sqrt(intel) * spi * baseRegen));
        } */
    }

    public static class TreeConstants
    {
        // Master is now in Base.BaseStats and Base.StatConversion
        // Source: http://www.wowwiki.com/Base_mana
        public static float BaseMana;  // Keep since this is more convenient reference than calling the whole BaseStats function every time // = 3496f;
        //public static float HasteRatingToHaste = 3279f;
    }
}
