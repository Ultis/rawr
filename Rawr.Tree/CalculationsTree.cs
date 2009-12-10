using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Rawr.Tree {

    [Rawr.Calculations.RawrModelInfo("Tree", "Ability_Druid_TreeofLife", CharacterClass.Druid)]
    public class CalculationsTree : CalculationsBase {
        private string[] _predefRotations = null;
        public string[] PredefRotations {
            get {
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
                        "Rejuvenation spam on raid",
                    };
                return _predefRotations;
            }
        }
        private List<GemmingTemplate> _defaultGemmingTemplates = null;
        public override List<GemmingTemplate> DefaultGemmingTemplates {
            get {
                if (_defaultGemmingTemplates == null) {
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
                    AddGemmingTemplateGroup(  _defaultGemmingTemplates, "Uncommon (Revitalizing)", false, runed[0], purified[0], luminous[0], seers[0], brilliant[0], revitalizing);
                    AddGemmingTemplateGroup(  _defaultGemmingTemplates, "Uncommon (Ember)", false, runed[0], purified[0], luminous[0], seers[0], brilliant[0], ember);
                    AddGemmingTemplateGroup(  _defaultGemmingTemplates, "Uncommon (Insightful)", false, runed[0], purified[0], luminous[0], seers[0], brilliant[0], insightful);
                    AddGemmingTemplateGroup(  _defaultGemmingTemplates, "Perfect (Revitalizing)", false, runed[1], purified[1], luminous[1], seers[1], brilliant[1], revitalizing);
                    AddGemmingTemplateGroup(  _defaultGemmingTemplates, "Perfect (Ember)", false, runed[1], purified[1], luminous[1], seers[1], brilliant[1], ember);
                    AddGemmingTemplateGroup(  _defaultGemmingTemplates, "Perfect (Insightful)", false, runed[1], purified[1], luminous[1], seers[1], brilliant[1], insightful);
                    AddGemmingTemplateGroup(  _defaultGemmingTemplates, "Rare (Revitalizing)", false, runed[2], purified[2], luminous[2], seers[2], brilliant[2], revitalizing);
                    AddGemmingTemplateGroup(  _defaultGemmingTemplates, "Rare (Ember)", false, runed[2], purified[2], luminous[2], seers[2], brilliant[2], ember);
					AddGemmingTemplateGroup(  _defaultGemmingTemplates, "Rare (Insightful)", false, runed[2], purified[2], luminous[2], seers[2], brilliant[2], insightful);
                    AddGemmingTemplateGroup(  _defaultGemmingTemplates, "Epic (Revitalizing)", false, runed[3], purified[3], luminous[3], seers[3], brilliant[3], revitalizing);
                    AddGemmingTemplateGroup(  _defaultGemmingTemplates, "Epic (Ember)", false, runed[3], purified[3], luminous[3], seers[3], brilliant[3], ember);
                    AddGemmingTemplateGroup(  _defaultGemmingTemplates, "Epic (Insightful)", true, runed[3], purified[3], luminous[3], seers[3], brilliant[3], insightful);
                    AddJCGemmingTemplateGroup(_defaultGemmingTemplates, "Jewelcrafting (Revitalizing)", false, runed[4], sparkling[4], brilliant[4], revitalizing);
                    AddJCGemmingTemplateGroup(_defaultGemmingTemplates, "Jewelcrafting (Ember)", false, runed[4], sparkling[4], brilliant[4], ember);
                }
                return _defaultGemmingTemplates;
            }
        }
        private void AddGemmingTemplateGroup(List<GemmingTemplate> list, string name, bool enabled, int runed, int purified, int luminous, int seers, int brilliant, int meta) {
            // Overrides, only "runed" and "seers"
            list.Add(new GemmingTemplate() { Model = "Tree", Group = name, RedId = runed, YellowId = runed, BlueId = runed, PrismaticId = runed, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Tree", Group = name, RedId = seers, YellowId = seers, BlueId = seers, PrismaticId = seers, MetaId = meta, Enabled = enabled });

            list.Add(new GemmingTemplate() { Model = "Tree", Group = name, RedId = runed, YellowId = luminous, BlueId = purified, PrismaticId = runed, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Tree", Group = name, RedId = luminous, YellowId = brilliant, BlueId = seers, PrismaticId = brilliant, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Tree", Group = name, RedId = luminous, YellowId = seers, BlueId = seers, PrismaticId = seers, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Tree", Group = name, RedId = purified, YellowId = seers, BlueId = seers, PrismaticId = seers, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Tree", Group = name, RedId = runed, YellowId = seers, BlueId = seers, PrismaticId = runed, MetaId = meta, Enabled = enabled });
        }
        private void AddJCGemmingTemplateGroup(List<GemmingTemplate> list, string name, bool enabled, int runed, int sparkling, int brilliant, int meta) {
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
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors {
            get {
                if (_subPointNameColors == null) {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("HealBurst", System.Drawing.Color.FromArgb(255, 255, 0, 0));
                    _subPointNameColors.Add("HealSustained", System.Drawing.Color.FromArgb(255, 0, 0, 255));
                    _subPointNameColors.Add("Survival", System.Drawing.Color.FromArgb(255, 0, 128, 0));
                }
                return _subPointNameColors;
            }
        }
#endif
        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels {
            get {
                if (_characterDisplayCalculationLabels == null) {
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
        public override string[] CustomChartNames {
            get {
                if (_customChartNames == null) {
                    _customChartNames = new string[] {
					    "Spell rotations"
					};
                }
                return _customChartNames;
            }
        }
        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels {
            get {
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
            get {
                if (_calculationOptionsPanel == null) { _calculationOptionsPanel = new CalculationOptionsPanelTree(); }
                return _calculationOptionsPanel;
            }
        }
        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes {
            get {
                if (_relevantItemTypes == null) {
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
        public override CharacterClass TargetClass { get { return CharacterClass.Druid; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationTree(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsTree(); }
        protected RotationSettings predefinedRotation(Stats stats, CalculationOptionsTree calcOpts, CharacterCalculationsTree calculatedStats) {
            RotationSettings settings = new RotationSettings();

            settings.RejuvFraction = (float)calcOpts.AverageRejuv / 100.0f;
            settings.LifebloomFraction = (float)calcOpts.AverageLifebloom / 100.0f;
            settings.averageLifebloomStacks = (float)calcOpts.AverageLifebloomStack;
            settings.RegrowthFraction = (float)calcOpts.AverageRegrowths / 100.0f;
            switch (calcOpts.LifebloomStackType)
            {
                case 0:
                    settings.lifeBloomType = LifeBloomType.Slow;
                    break;
                case 1:
                    settings.lifeBloomType = LifeBloomType.Fast;
                    break;
                case 2:
                default:
                    settings.lifeBloomType = LifeBloomType.Rolling;
                    break;
            }
            switch (calcOpts.PrimaryHeal)
            {
                case 0:
                default:
                    settings.primaryHeal = SpellList.Nourish;
                    break;
                case 1:
                    settings.primaryHeal = SpellList.HealingTouch;
                    break;
                case 2:
                    settings.primaryHeal = SpellList.Regrowth;
                    break;
                case 3:
                    settings.primaryHeal = SpellList.Rejuvenation;
                    break;
            }
            settings.nourish1 = (float)calcOpts.Nourish1 / 100f;
            settings.nourish2 = (float)calcOpts.Nourish2 / 100f;
            settings.nourish3 = (float)calcOpts.Nourish3 / 100f;
            settings.nourish4 = (float)calcOpts.Nourish4 / 100f;
            settings.nourish0 = 1.0f - (settings.nourish1 + settings.nourish2 + settings.nourish3 + settings.nourish4);

            settings.healTarget = HealTargetTypes.RaidHealing;

            settings.SwiftmendPerMin = calcOpts.SwiftmendPerMinute;
            settings.WildGrowthPerMin = calcOpts.WildGrowthPerMinute;
            settings.livingSeedEfficiency = (float)calcOpts.LivingSeedEfficiency / 100f;
            return settings;
        }
        private static Stats getTrinketStats(Character character, Stats stats, float FightDuration, float CastInterval, float HealInterval, float CritsRatio, float RejuvInterval, out float Healing)
        {
            Healing = 0;
            #region New_SpecialEffect_Handling
            Stats resultNew = new Stats();
            foreach (Rawr.SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use)
                {
                    Stats s = effect.GetAverageStats(0.0f, 1.0f, 2.0f, FightDuration); // 0 cooldown, 100% chance to use
                    resultNew += s;

                    float _h;
                    Stats s2 = getTrinketStats(character, s, effect.Duration, CastInterval, HealInterval, CritsRatio, RejuvInterval, out _h);
                    Healing += _h;

                    s2 *= effect.Duration / effect.Cooldown;
                    resultNew += s2;
                }
                else if (effect.Trigger == Trigger.SpellCast)
                {
                    resultNew += effect.GetAverageStats(CastInterval, 1.0f, CastInterval, FightDuration);
                }
                else if (effect.Trigger == Trigger.HealingSpellCast)
                {
                    // Same as SpellCast, but split to allow easier placement of breakpoints
                    resultNew += effect.GetAverageStats(CastInterval, 1.0f, CastInterval, FightDuration);
                }
                else if (effect.Trigger == Trigger.HealingSpellHit)
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
                }
            }
            #endregion

            float extraSpellPower = (resultNew.Spirit * character.DruidTalents.ImprovedTreeOfLife * 0.05f);

            Healing = resultNew.Healed;
            return new Stats()
            {
                Spirit = resultNew.Spirit,
                HasteRating = resultNew.HasteRating,
                SpellPower = resultNew.SpellPower + extraSpellPower,
                CritRating = resultNew.CritRating,
                Mp5 = resultNew.Mp5 + (resultNew.ManaRestore * 5.0f),
                SpellCombatManaRegeneration = resultNew.SpellCombatManaRegeneration,
                BonusHealingReceived = resultNew.BonusHealingReceived,
                SpellsManaReduction = resultNew.SpellsManaReduction,
                HighestStat = resultNew.HighestStat,
                ShieldFromHealed = resultNew.ShieldFromHealed,
            };
        }
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            cacheChar = character;
            CalculationOptionsTree calcOpts = (CalculationOptionsTree)character.CalculationOptions;

            CharacterCalculationsTree calculationResult = new CharacterCalculationsTree();
            calculationResult.LocalCharacter = character;
            calculationResult.BasicStats = GetCharacterStats(character, additionalItem);

            #region Rotations
            Stats stats = calculationResult.BasicStats;
            float ExtraHPS = 0f;
            RotationSettings settings = predefinedRotation(stats, calcOpts, calculationResult);

            // Initial run
            RotationResult rot = Solver.SimulateHealing(calculationResult, stats, calcOpts, settings);

            int nPasses = 3, k;
            for (k = 0; k < nPasses; k++) {
                Stats procs = getTrinketStats(character, stats,
                    rot.TotalTime, 60f / rot.TotalCastsPerMinute,
                    60f / rot.TotalHealsPerMinute, rot.TotalCritsPerMinute / rot.TotalCastsPerMinute,
                    60 / rot.RejuvenationHealsPerMinute, 
                    out ExtraHPS);

                // Create a new stats instance that uses the proc effects
                stats = GetCharacterStats(character, additionalItem, procs);

                // New run
                rot = Solver.SimulateHealing(calculationResult, stats, calcOpts, settings);  
            }
            calculationResult.Simulation = rot;
            calculationResult.BasicStats = stats;     // Replace BasicStats to get Spirit while casting included
            #endregion

            SingleTargetBurstResult burst = Solver.CalculateSingleTargetBurst(calculationResult, stats, calcOpts, SingleTargetBurstRotations.AutoSelect);

            calculationResult.BurstPoints = burst.HPS;
            calculationResult.SustainedPoints = rot.TotalHealing / rot.TotalTime + ExtraHPS;

            #region Survival Points
            float DamageReduction = StatConversion.GetArmorDamageReduction(83, stats.Armor, 0, 0, 0);
            calculationResult.SurvivalPoints = stats.Health / (1f - DamageReduction) / 100f * calcOpts.SurvValuePer100;
            #endregion

            /*// Penalty
            if (rot.TimeToOOM < rot.TotalTime) {
                float frac = rot.TimeToOOM / rot.TotalTime;
                float mod = (float)Math.Pow(frac, 1 + 0.05f ) / frac;
                calculatedStats.SustainedPoints *= mod;
                //if (calcOpts.PenalizeEverything)
                {
                    calculatedStats.BurstPoints *= mod;
                }
            }*/

            // ADJUST POINT VALUE (BURST SUSTAINED RATIO)
            float bsRatio = .01f * calcOpts.BSRatio;
            calculationResult.BurstPoints *= (1f-bsRatio) * 2;
            calculationResult.SustainedPoints *= bsRatio * 2;

            calculationResult.OverallPoints = calculationResult.BurstPoints + calculationResult.SustainedPoints + calculationResult.SurvivalPoints;

            //calcOpts.calculatedStats = calculatedStats;
            return calculationResult;
        }
        public override Stats GetCharacterStats(Character character, Item additionalItem) { return GetCharacterStats(character, additionalItem, new Stats()); }
        public Stats GetCharacterStats(Character character, Item additionalItem, Stats statsProcs) {
            cacheChar = character;
            CalculationOptionsTree calcOpts = character.CalculationOptions as CalculationOptionsTree;
            DruidTalents talents = character.DruidTalents;

            Stats statsRace = BaseStats.GetBaseStats(character.Level, character.Class, character.Race); //GetRacialBaseStats(character.Race);
            TreeConstants.BaseMana = statsRace.Mana; // Setup TreeConstant

            Stats statsTalents = new Stats() {
                BonusAgilityMultiplier   = 0.01f * talents.SurvivalOfTheFittest * 2f,
                BonusIntellectMultiplier = 0.01f * talents.SurvivalOfTheFittest * 2f,
                BonusSpiritMultiplier    = 0.01f * talents.SurvivalOfTheFittest * 2f,
                BonusStaminaMultiplier   = 0.01f * talents.SurvivalOfTheFittest * 2f,
                BonusStrengthMultiplier  = 0.01f * talents.SurvivalOfTheFittest * 2f,
                SpellHaste               = ((1f + 0.01f * talents.CelestialFocus) * (1f + 0.02f * talents.GiftOfTheEarthmother)) - 1f,
            };

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character, calcOpts);
            Stats statsTotal = statsBaseGear + statsBuffs + statsRace + statsTalents + statsProcs;

            statsTotal.Agility   = (float)Math.Floor(statsTotal.Agility   * (1f + statsTotal.BonusAgilityMultiplier));
            statsTotal.Agility   = (float)Math.Floor(statsTotal.Agility   * (1f + 0.01f * talents.ImprovedMarkOfTheWild));
            statsTotal.Stamina   = (float)Math.Floor(statsTotal.Stamina   * (1f + statsTotal.BonusStaminaMultiplier));
            statsTotal.Stamina   = (float)Math.Floor(statsTotal.Stamina   * (1f + 0.01f * talents.ImprovedMarkOfTheWild));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1f + statsTotal.BonusIntellectMultiplier));
            statsTotal.Intellect = (float)Math.Round(statsTotal.Intellect * (1f + 0.04f * talents.HeartOfTheWild));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1f + 0.01f * talents.ImprovedMarkOfTheWild));
            statsTotal.Spirit    = (float)Math.Floor(statsTotal.Spirit    * (1f + statsTotal.BonusSpiritMultiplier));
            statsTotal.Spirit    = (float)Math.Floor(statsTotal.Spirit    * (1f + 0.05f * talents.LivingSpirit));
            statsTotal.Spirit    = (float)Math.Floor(statsTotal.Spirit    * (1f + 0.01f * talents.ImprovedMarkOfTheWild));
            statsTotal.Armor     = (float)Math.Floor(statsTotal.Armor     * (1f + 0.80f * talents.ImprovedTreeOfLife));

            // Removed, since proc effects added by GetCharacterCalculations
            //statsTotal.ExtraSpiritWhileCasting = (float)Math.Floor((statsTotal.ExtraSpiritWhileCasting) * (1 + statsTotal.BonusSpiritMultiplier) * (1 + 0.01f * character.DruidTalents.ImprovedMarkOfTheWild) * (1 + character.DruidTalents.LivingSpirit * 0.05f));
            statsTotal.ExtraSpiritWhileCasting = 0f;

            if (statsTotal.HighestStat > 0) {
                // Highest stat in combat
                if (statsTotal.Spirit + statsTotal.ExtraSpiritWhileCasting > statsTotal.Intellect) {
                    // spirit proc (Greatness)
                    float extraSpi = statsTotal.HighestStat;
                    extraSpi *= 1f + statsTotal.BonusSpiritMultiplier;
                    extraSpi *= 1f + character.DruidTalents.LivingSpirit * 0.05f;
                    extraSpi *= 1f + character.DruidTalents.ImprovedMarkOfTheWild * 0.01f;
                    statsTotal.Spirit += (float)Math.Floor(extraSpi);
                }else{
                    // int proc (Greatness)
                    float extraInt = statsTotal.HighestStat;
                    extraInt *= 1f + statsTotal.BonusIntellectMultiplier;
                    extraInt *= 1f + character.DruidTalents.HeartOfTheWild * 0.04f;
                    extraInt *= 1f + character.DruidTalents.ImprovedMarkOfTheWild * 0.01f;
                    statsTotal.Intellect += (float)Math.Floor(extraInt);
                }
            }

            statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower + statsTotal.Intellect * talents.LunarGuidance * 0.04); //LunarGuidance, 4% per Point
            statsTotal.SpellPower = (float)Math.Round( statsTotal.SpellPower
                                                    + (statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit)
                                                    + (statsTotal.Intellect * talents.LunarGuidance * 0.04)
                                                    + (talents.NurturingInstinct * 0.35f * statsTotal.Agility));

            statsTotal.Mana = statsTotal.Mana + StatConversion.GetManaFromIntellect(statsTotal.Intellect);
            statsTotal.Mana *= (1f + statsTotal.BonusManaMultiplier);

            statsTotal.Health = (float)Math.Round(statsTotal.Health + StatConversion.GetHealthFromStamina(statsTotal.Stamina));
            statsTotal.Mp5   += (float)Math.Floor(statsTotal.Intellect * (talents.Dreamstate > 0 ? talents.Dreamstate * 0.03f + 0.01f : 0f));

            statsTotal.SpellCrit = (float)Math.Round((StatConversion.GetSpellCritFromIntellect(statsTotal.Intellect)
                                                    + StatConversion.GetSpellCritFromRating(statsTotal.CritRating)
                                                    + (statsTotal.SpellCrit)
                                                    + 0.01f * talents.NaturalPerfection) * 100f, 2);
            statsTotal.SpellCombatManaRegeneration += 0.5f / 3f * talents.Intensity;

            // SpellPower (actually healing only, but we have no damaging spells, so np)
            statsTotal.SpellPower += ((statsTotal.Spirit + statsTotal.ExtraSpiritWhileCasting) * talents.ImprovedTreeOfLife * 0.05f);

            return statsTotal;
        }
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName) {
            return new ComparisonCalculationBase[0]; 
        }
        public override Stats GetRelevantStats(Stats stats) {
            //return SpecialEffects.GetRelevantStats(stats) + new Stats()
            Stats s = new Stats() {
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
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,
                #endregion
                #region Trinkets
                BonusManaPotion = stats.BonusManaPotion,
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
                //ManacostReduceWithin15OnHealingCast = stats.ManacostReduceWithin15OnHealingCast,
                SwiftmendBonus = stats.SwiftmendBonus,
                #endregion
                #region Gems
                BonusCritHealMultiplier = stats.BonusCritHealMultiplier,
                BonusManaMultiplier = stats.BonusManaMultiplier,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                #endregion
            };
            foreach (Rawr.SpecialEffect effect in stats.SpecialEffects()) {
                if (effect.Trigger == Trigger.Use || 
                    effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.SpellHit
                    || effect.Trigger == Trigger.HealingSpellCast || effect.Trigger == Trigger.HealingSpellCrit || effect.Trigger == Trigger.HealingSpellHit
                    || effect.Trigger == Trigger.RejuvenationTick  // Idol of Flaring Growth
                    )
                {
                    if (HasRelevantSpecialEffectStats(effect.Stats)) {
                        s.AddSpecialEffect(effect);
                    } else {
                        // Trigger seems relevant, but effect not. Sounds weird, probably DPS bonus on Use or general SpellCast ...
                    }
                }
            }
            return s;
        }
        public bool HasRelevantSpecialEffectStats(Stats stats) {
            foreach (Rawr.SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use ||
                    effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.SpellCrit
                    || effect.Trigger == Trigger.HealingSpellCast || effect.Trigger == Trigger.HealingSpellCrit || effect.Trigger == Trigger.HealingSpellHit
                    || effect.Trigger == Trigger.RejuvenationTick  // Idol of Flaring Growth
                    )
                {
                    if (HasRelevantSpecialEffectStats(effect.Stats)) return true;
                }
            }
            return (stats.Intellect + stats.Spirit + stats.SpellPower + stats.CritRating + stats.HasteRating + stats.ManaRestore
                   + stats.Mp5 + stats.Healed + stats.HighestStat + stats.BonusHealingReceived + stats.SwiftmendBonus + stats.HealingOmenProc
                   + stats.ShieldFromHealed + stats.ManaRestoreFromMaxManaPerSecond) > 0;
        }
        public override bool HasRelevantStats(Stats stats) {
            if (HasRelevantSpecialEffectStats(stats)) return true;

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
                //stats.ManacostReduceWithin15OnHealingCast +
                stats.HealingOmenProc + stats.SwiftmendBonus + stats.NourishCritBonus + stats.RejuvenationCrit +
                stats.Armor + stats.Stamina + stats.ManaRestoreFromMaxManaPerSecond
                > 0)
                return true;

            //if (stats.Strength + stats.Agility + stats.AttackPower > 0){ return false; }

            //Not sure why this was in here  
            //if (stats.SpellCombatManaRegeneration == 0.3f){ return false; }

            return (stats.SpellCombatManaRegeneration + stats.Intellect > 0);
        }
        public Stats GetBuffsStats(Character character, CalculationOptionsTree calcOpts) {
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
            /* [More Buffs to Come to this method]
             * Ferocious Inspiration | Sanctified Retribution
             * Hunting Party | Judgements of the Wise, Vampiric Touch, Improved Soul Leech, Enduring Winter
             * Acid Spit | Expose Armor, Sunder Armor (requires BM & Worm Pet)
             */
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
        // Wildebees 20090407 : Overload base function to disable all enchants on OffHand for tree druids
        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot) {
            if (slot == ItemSlot.OffHand) { return false; }
            return base.EnchantFitsInSlot(enchant, character, slot);
        }
        public override ICalculationOptionBase DeserializeDataObject(string xml) {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsTree));
            StringReader reader = new StringReader(xml);
            CalculationOptionsTree calcOpts = serializer.Deserialize(reader) as CalculationOptionsTree;
            return calcOpts;
        }
    }

}
