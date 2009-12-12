using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
#if RAWR3
using System.Windows.Media;
#else
using System.Drawing;
#endif

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

        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            if (_relevantGlyphs == null)
            {
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of Healing Touch");
                _relevantGlyphs.Add("Glyph of Innervate");
                _relevantGlyphs.Add("Glyph of Lifebloom");
                _relevantGlyphs.Add("Glyph of Nourish");
                _relevantGlyphs.Add("Glyph of Rebirth");
                _relevantGlyphs.Add("Glyph of Regrowth");
                _relevantGlyphs.Add("Glyph of Rejuvenation");
                _relevantGlyphs.Add("Glyph of Swiftmend");
                _relevantGlyphs.Add("Glyph of Wild Growth");
                _relevantGlyphs.Add("Glyph of Rapid Rejuvenation");
            }
            return _relevantGlyphs;
        }
        
        private Dictionary<string, Color> _subPointNameColors = null;
        private Dictionary<string, Color> _subPointNameColorsRating = null;
        private Dictionary<string, Color> _subPointNameColorsMPS = null;
        private Dictionary<string, Color> _subPointNameColorsHPS = null;
        private Dictionary<string, Color> _subPointNameColorsHPCT = null;
        private Dictionary<string, Color> _subPointNameColorsHPM = null;

        public CalculationsTree()
        {
            _subPointNameColorsRating = new Dictionary<string, Color>();
            _subPointNameColorsRating.Add("HealBurst", Color.FromArgb(255, 255, 0, 0));
            _subPointNameColorsRating.Add("HealSustained", Color.FromArgb(255, 0, 0, 255));
            _subPointNameColorsRating.Add("Survival", Color.FromArgb(255, 0, 128, 0));

            _subPointNameColorsMPS = new Dictionary<string, Color>();
            _subPointNameColorsMPS.Add("Mana per second", Color.FromArgb(128, 0, 0, 255));

            _subPointNameColorsHPS = new Dictionary<string, Color>();
            _subPointNameColorsHPS.Add("Healing per second", Color.FromArgb(128, 0, 255, 0));

            _subPointNameColorsHPCT = new Dictionary<string, Color>();
            _subPointNameColorsHPCT.Add("Healing per cast time", Color.FromArgb(128, 0, 255, 0));

            _subPointNameColorsHPM = new Dictionary<string, Color>();
            _subPointNameColorsHPM.Add("Healing per mana", Color.FromArgb(128, 0, 255, 255));

            _subPointNameColors = _subPointNameColorsRating;
        }

        public override Dictionary<string, Color> SubPointNameColors {
            get {
                Dictionary<string, Color> ret = _subPointNameColors;
                _subPointNameColors = _subPointNameColorsRating;
                return ret;
            }
        }

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

                        /*"Simulation:Result",
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
                        "Swiftmend:SM Both Regrowth Lost Ticks",*/
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
                        "Single target spell mixes",
                        "Healing per spell (single target)",
					    "Mana sources (sustained)",
                        "Mana usage per spell (sustained)",
                        "Healing per spell (sustained)",
                        "HPCT per spell",
                        "HPS per spell",
                        "HPM per spell",
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
        protected RotationSettings getRotationFromCalculationOptions(Stats stats, CalculationOptionsTree calcOpts, CharacterCalculationsTree calculatedStats) {
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
        private static Stats getAverageStats(Rawr.SpecialEffect effect, float triggerInterval, float triggerChance, float attackSpeed, float fightDuration, out float weight, out float stacksAtEnd)
        {
            Stats s = effect.GetAverageStats(triggerInterval, triggerChance, attackSpeed, fightDuration);
            
            if (effect.MaxStack > 1)
            {
                // Will the stacks drop when the effect is stopped? If not, the average stack size must be increased somehow
                // E.G. Duration = X, and secondary has Duration = Y
                // Then average stacksize total will be average accumulation with fight lenght X
                // plus the amount of stacks that are at the end times Y
                
                weight = effect.GetAverageStackSize(triggerInterval, triggerChance, attackSpeed, fightDuration);
                stacksAtEnd = Math.Min(effect.MaxStack, weight*2);
            }
            else if (effect.Duration == 0f)
            {
                weight = effect.GetAverageProcsPerSecond(triggerInterval, triggerChance, attackSpeed, fightDuration);
                stacksAtEnd = 0;
            }
            else
            {
                // Same story here - usually our secondary won't have a duration now, but it might still be possible, a stack that keeps getting refreshed...
                weight = effect.GetAverageUptime(triggerInterval, triggerChance, attackSpeed, fightDuration);
                stacksAtEnd = 1f;
            }

            return s;
        }

        private static Stats calculateSpecialEffects(Character character, Stats stats, float FightDuration, float CastInterval, float HealInterval, float CritsRatio, float RejuvInterval, float weight, bool overflow)
        {
            #region New_SpecialEffect_Handling
            Stats resultNew = new Stats();
            foreach (Rawr.SpecialEffect effect in stats.SpecialEffects())
            {
                Stats s = null;
                float averageStacks = 1f, stacksEnd = 0f;
                if (effect.Trigger == Trigger.Use)
                {
                    s = getAverageStats(effect, 0.0f, 1.0f, 2.0f, FightDuration, out averageStacks, out stacksEnd); // 0 cooldown, 100% chance to use
                }
                else if (effect.Trigger == Trigger.SpellCast)
                {
                    s = getAverageStats(effect, CastInterval, 1.0f, CastInterval, FightDuration, out averageStacks, out stacksEnd);
                }
                else if (effect.Trigger == Trigger.HealingSpellCast)
                {
                    // Same as SpellCast, but split to allow easier placement of breakpoints
                    s = getAverageStats(effect, CastInterval, 1.0f, CastInterval, FightDuration, out averageStacks, out stacksEnd);
                }
                else if (effect.Trigger == Trigger.HealingSpellHit)
                {
                    // Heal interval measures time between HoTs as well, direct heals are a different interval
                    s = getAverageStats(effect, HealInterval, 1.0f, CastInterval, FightDuration, out averageStacks, out stacksEnd);
                }
                else if (effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.HealingSpellCrit)
                {
                    s = getAverageStats(effect, CastInterval, CritsRatio, CastInterval, FightDuration, out averageStacks, out stacksEnd);
                }
                else if (effect.Trigger == Trigger.RejuvenationTick)
                {
                    s = getAverageStats(effect, RejuvInterval, 1.0f, RejuvInterval, FightDuration, out averageStacks, out stacksEnd);
                }
                else
                {
                    // Trigger isn't relevant. Physical Hit, Damage Spell etc.
                }
                if (s != null) {
                    float weightForThisEffect = weight;
                    if (overflow && !float.IsInfinity(effect.Duration)) // todo : this assumes a stack does not drop off when its parent is terminated
                    {
                        // Apparently we're allowed to overflow...
                        // this calculation basically assumes there was a hitting trigger right at the end, so assume full duration of average stacks...
                        weightForThisEffect *= (averageStacks * FightDuration + stacksEnd * effect.Duration) / (averageStacks * FightDuration);
                    }

                    s += calculateSpecialEffects(character, s, effect.Duration, CastInterval, HealInterval, CritsRatio, RejuvInterval, averageStacks, stacksEnd>=1f?true:false);
                    // Clear special effects
                    for (int i=0;i<s._rawSpecialEffectDataSize;i++) s._rawSpecialEffectData[i] = null;
                    s._rawSpecialEffectDataSize = 0;
                    resultNew += s * weightForThisEffect;
                }
            }
            #endregion


            return resultNew;
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
            float ExtraHealing = 0f;
            RotationSettings settings = getRotationFromCalculationOptions(stats, calcOpts, calculationResult);

            // Initial run
            RotationResult rot = Solver.SimulateHealing(calculationResult, stats, calcOpts, settings);

            int nPasses = 4, k;
            for (k = 0; k < nPasses; k++) {
                /*
                Stats procs = calculateSpecialEffects(character, stats,
                    rot.TotalTime, 60f / rot.TotalCastsPerMinute,
                    60f / rot.TotalHealsPerMinute, rot.TotalCritsPerMinute / rot.TotalCastsPerMinute,
                    60 / rot.RejuvenationHealsPerMinute);*/
                Stats procs = calculateSpecialEffects(character, stats,
                    rot.TotalTime, 60f / rot.CastsPerMinute,
                    60f / rot.HealsPerMinute, rot.CritsPerMinute / rot.CastsPerMinute,
                    60 / rot.RejuvenationHealsPerMinute, 1f, false);

                ExtraHealing = procs.Healed;

                // Create a new stats instance that uses the proc effects
                stats = GetCharacterStats(character, additionalItem, procs);

                // New run
                rot = Solver.SimulateHealing(calculationResult, stats, calcOpts, settings);  
            }
            calculationResult.Sustained = rot;
            calculationResult.BasicStats = stats;     // Replace BasicStats to get Spirit while casting included
            #endregion

            calculationResult.SingleTarget = Solver.CalculateSingleTargetBurst(calculationResult, stats, calcOpts, Solver.SingleTargetIndexToRotation(calcOpts.SingleTargetRotation));

            calculationResult.BurstPoints = calculationResult.SingleTarget[0].HPS;
            calculationResult.SustainedPoints = (rot.TotalHealing + ExtraHealing) / rot.TotalTime;

            #region Survival Points
            float DamageReduction = StatConversion.GetArmorDamageReduction(83, stats.Armor, 0, 0, 0);
            calculationResult.SurvivalPoints = stats.Health / (1f - DamageReduction) / 100f * calcOpts.SurvValuePer100;
            #endregion

            calculationResult.BurstPoints = 10000f * ((float)1f - calcOpts.SingleTarget / calculationResult.BurstPoints);
            calculationResult.SustainedPoints = 10000f * (1f - (float)calcOpts.SustainedTarget / calculationResult.SustainedPoints);

            calculationResult.OverallPoints = calculationResult.BurstPoints + calculationResult.SustainedPoints + calculationResult.SurvivalPoints;

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
                BonusAgilityMultiplier   = (1f + 0.01f * talents.SurvivalOfTheFittest * 2f) * (1f + 0.01f * talents.ImprovedMarkOfTheWild) - 1f,
                BonusIntellectMultiplier = (1f + 0.01f * talents.SurvivalOfTheFittest * 2f) * (1f + 0.01f * talents.ImprovedMarkOfTheWild) * (1f + 0.04f * talents.HeartOfTheWild) - 1f,
                BonusSpiritMultiplier    = (1f + 0.01f * talents.SurvivalOfTheFittest * 2f) * (1f + 0.01f * talents.ImprovedMarkOfTheWild) * (1f + 0.05f * talents.LivingSpirit) - 1f,
                BonusStaminaMultiplier   = (1f + 0.01f * talents.SurvivalOfTheFittest * 2f) * (1f + 0.01f * talents.ImprovedMarkOfTheWild) - 1f,
                BonusStrengthMultiplier  = (1f + 0.01f * talents.SurvivalOfTheFittest * 2f) * (1f + 0.01f * talents.ImprovedMarkOfTheWild) - 1f,
                BonusArmorMultiplier     = 0.80f * talents.ImprovedTreeOfLife,
                SpellHaste               = ((1f + 0.01f * talents.CelestialFocus) * (1f + 0.02f * talents.GiftOfTheEarthmother)) - 1f,
                SpellDamageFromSpiritPercentage = talents.ImprovedTreeOfLife * 0.05f,
                SpellCombatManaRegeneration = talents.Intensity * 0.5f / 3f,
            };

            statsTalents.AddSpecialEffect(new SpecialEffect(Trigger.HealingSpellCrit, new Stats() { SpellHaste = 0.2f }, 3f, 0, talents.NaturesGrace * 1f / 3f, 1));

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character, calcOpts);
            Stats statsTotal = statsBaseGear + statsBuffs + statsRace + statsTalents + statsProcs;

            statsTotal.Agility   = (float)Math.Floor(statsTotal.Agility   * (1f + statsTotal.BonusAgilityMultiplier));
            statsTotal.Stamina   = (float)Math.Floor(statsTotal.Stamina   * (1f + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1f + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit    = (float)Math.Floor(statsTotal.Spirit    * (1f + statsTotal.BonusSpiritMultiplier));
            statsTotal.Armor     = (float)Math.Floor(statsTotal.Armor     * (1f + statsTotal.BonusArmorMultiplier));

            if (statsTotal.HighestStat > 0) {
                if (statsTotal.Spirit > statsTotal.Intellect) {
                    statsTotal.Spirit += (float)Math.Floor(statsTotal.HighestStat * (1f + statsTotal.BonusSpiritMultiplier));
                } else {
                    statsTotal.Intellect += (float)Math.Floor(statsTotal.HighestStat * (1f + statsTotal.BonusIntellectMultiplier));
                }
            }

            // Add spellpower from spirit, intellect and... agility :)
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

            return statsTotal;
        }
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();

            CalculationOptionsTree calculationOptions = character.CalculationOptions as CalculationOptionsTree;
            DruidTalents talents = character.DruidTalents;
            CharacterCalculationsTree calculationResult = GetCharacterCalculations(character) as CharacterCalculationsTree;

            switch (chartName)
            {
                case "Mana sources (sustained)":
                    _subPointNameColors = _subPointNameColorsMPS;
                    ComparisonCalculationTree gear = new ComparisonCalculationTree()
                    {
                        Name = "MP5 from gear and procs",
                        Equipped = false,
                        OverallPoints = calculationResult.Sustained.GearMPS,
                        SubPoints = new float[] { calculationResult.Sustained.GearMPS }
                    };
                    comparisonList.Add(gear);
                    ComparisonCalculationTree procs = new ComparisonCalculationTree()
                    {
                        Name = "Mana from procs",
                        Equipped = false,
                        OverallPoints = calculationResult.Sustained.ProcsMPS,
                        SubPoints = new float[] { calculationResult.Sustained.ProcsMPS }
                    };
                    comparisonList.Add(procs);
                    ComparisonCalculationTree spiritIC = new ComparisonCalculationTree()
                    {
                        Name = "Spirit in combat",
                        Equipped = false,
                        OverallPoints = (1f - calculationResult.Sustained.OutOfCombatFraction) * calculationResult.Sustained.SpiritMPS * calculationResult.Sustained.SpiritInCombatFraction,
                        SubPoints = new float[] { (1f - calculationResult.Sustained.OutOfCombatFraction) * calculationResult.Sustained.SpiritMPS * calculationResult.Sustained.SpiritInCombatFraction }
                    };
                    comparisonList.Add(spiritIC);
                    ComparisonCalculationTree spiritOOC = new ComparisonCalculationTree()
                    {
                        Name = "Spirit out combat",
                        Equipped = false,
                        OverallPoints = calculationResult.Sustained.OutOfCombatFraction * calculationResult.Sustained.SpiritMPS,
                        SubPoints = new float[] { calculationResult.Sustained.OutOfCombatFraction * calculationResult.Sustained.SpiritMPS }
                    };
                    comparisonList.Add(spiritOOC);
                    ComparisonCalculationTree replenishment = new ComparisonCalculationTree()
                    {
                        Name = "Replenishment",
                        Equipped = false,
                        OverallPoints = calculationResult.Sustained.ReplenishmentMPS,
                        SubPoints = new float[] { calculationResult.Sustained.ReplenishmentMPS }
                    };
                    comparisonList.Add(replenishment);
                    ComparisonCalculationTree pots = new ComparisonCalculationTree()
                    {
                        Name = "Potions",
                        Equipped = false,
                        OverallPoints = calculationResult.Sustained.PotionMPS,
                        SubPoints = new float[] { calculationResult.Sustained.PotionMPS }
                    };
                    comparisonList.Add(pots);
                    ComparisonCalculationTree innervates = new ComparisonCalculationTree()
                    {
                        Name = "Innervates",
                        Equipped = false,
                        OverallPoints = calculationResult.Sustained.InnervateMPS,
                        SubPoints = new float[] { calculationResult.Sustained.InnervateMPS }
                    };
                    comparisonList.Add(innervates);
                    
                    return comparisonList.ToArray();
                case "Mana usage per spell (sustained)":
                    {
                        _subPointNameColors = _subPointNameColorsMPS;
                        ComparisonCalculationTree rejuv = new ComparisonCalculationTree()
                        {
                            Name = "Rejuvenation",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.RejuvMPS,
                            SubPoints = new float[] { calculationResult.Sustained.RejuvMPS }
                        };
                        comparisonList.Add(rejuv);
                        ComparisonCalculationTree regrowth = new ComparisonCalculationTree()
                        {
                            Name = "Regrowth",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.RegrowthMPS,
                            SubPoints = new float[] { calculationResult.Sustained.RegrowthMPS }
                        };
                        comparisonList.Add(regrowth);
                        ComparisonCalculationTree lifebloom = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.LifebloomMPS,
                            SubPoints = new float[] { calculationResult.Sustained.LifebloomMPS }
                        };
                        comparisonList.Add(lifebloom);
                        ComparisonCalculationTree lifebloomStack = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom Stack",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.LifebloomStackMPS,
                            SubPoints = new float[] { calculationResult.Sustained.LifebloomStackMPS }
                        };
                        comparisonList.Add(lifebloomStack);
                        ComparisonCalculationTree wildGrowth = new ComparisonCalculationTree()
                        {
                            Name = "Wild Growth",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.WildGrowthMPS,
                            SubPoints = new float[] { calculationResult.Sustained.WildGrowthMPS }
                        };
                        comparisonList.Add(wildGrowth);
                        ComparisonCalculationTree swiftmend = new ComparisonCalculationTree()
                        {
                            Name = "Swiftmend",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.SwiftmendMPS,
                            SubPoints = new float[] { calculationResult.Sustained.SwiftmendMPS }
                        };
                        comparisonList.Add(swiftmend);
                        ComparisonCalculationTree primary = new ComparisonCalculationTree()
                        {
                            Name = "Primary Spell",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.PrimaryMPS,
                            SubPoints = new float[] { calculationResult.Sustained.PrimaryMPS }
                        };
                        comparisonList.Add(primary);
                    }
                    return comparisonList.ToArray();
                case "Healing per spell (sustained)":
                    {
                        _subPointNameColors = _subPointNameColorsHPS;
                        ComparisonCalculationTree rejuv = new ComparisonCalculationTree()
                        {
                            Name = "Rejuvenation",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.RejuvHPS,
                            SubPoints = new float[] { calculationResult.Sustained.RejuvHPS }
                        };
                        comparisonList.Add(rejuv);
                        ComparisonCalculationTree regrowth = new ComparisonCalculationTree()
                        {
                            Name = "Regrowth",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.RegrowthHPS,
                            SubPoints = new float[] { calculationResult.Sustained.RegrowthHPS }
                        };
                        comparisonList.Add(regrowth);
                        ComparisonCalculationTree regrowthDH = new ComparisonCalculationTree()
                        { // RegrowthAvg * regrowth.HPSHoT + RegrowthCPS * regrowth.AverageHealingwithCrit
                            Name = "Regrowth (DH)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.RegrowthCPS * calculationResult.Sustained.regrowth.AverageHealingwithCrit,
                            SubPoints = new float[] { calculationResult.Sustained.RegrowthCPS * calculationResult.Sustained.regrowth.AverageHealingwithCrit }
                        };
                        comparisonList.Add(regrowthDH);
                        ComparisonCalculationTree regrowthHoT = new ComparisonCalculationTree()
                        { 
                            Name = "Regrowth (HoT)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.RegrowthAvg * calculationResult.Sustained.regrowth.HPSHoT,
                            SubPoints = new float[] { calculationResult.Sustained.RegrowthAvg * calculationResult.Sustained.regrowth.HPSHoT }
                        };
                        comparisonList.Add(regrowthHoT);
                        ComparisonCalculationTree lifebloom = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.LifebloomHPS,
                            SubPoints = new float[] { calculationResult.Sustained.LifebloomHPS }
                        };
                        comparisonList.Add(lifebloom);
                        ComparisonCalculationTree lifebloomDH = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom (Bloom)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.LifebloomCPS * calculationResult.Sustained.lifebloom.AverageHealingwithCrit,
                            SubPoints = new float[] { calculationResult.Sustained.LifebloomCPS * calculationResult.Sustained.lifebloom.AverageHealingwithCrit }
                        };
                        comparisonList.Add(lifebloomDH);
                        ComparisonCalculationTree lifebloomHoT = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom (HoT)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.LifebloomAvg * calculationResult.Sustained.lifebloom.HPSHoT,
                            SubPoints = new float[] { calculationResult.Sustained.LifebloomAvg * calculationResult.Sustained.lifebloom.HPSHoT }
                        };
                        comparisonList.Add(lifebloomHoT);
                        ComparisonCalculationTree lifebloomStack = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom Stack",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.LifebloomStackHPS,
                            SubPoints = new float[] { calculationResult.Sustained.LifebloomStackHPS }
                        };
                        comparisonList.Add(lifebloomStack);
                        ComparisonCalculationTree lifebloomStackDH = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom Stack (Bloom)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.LifebloomStackHPS_DH,
                            SubPoints = new float[] { calculationResult.Sustained.LifebloomStackHPS_DH }
                        };
                        comparisonList.Add(lifebloomStackDH);
                        ComparisonCalculationTree lifebloomStackHOT = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom Stack (HoT)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.LifebloomStackHPS_HOT,
                            SubPoints = new float[] { calculationResult.Sustained.LifebloomStackHPS_HOT }
                        };
                        comparisonList.Add(lifebloomStackHOT);
                        ComparisonCalculationTree wildGrowth = new ComparisonCalculationTree()
                        {
                            Name = "Wild Growth",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.WildGrowthHPS,
                            SubPoints = new float[] { calculationResult.Sustained.WildGrowthHPS }
                        };
                        comparisonList.Add(wildGrowth);
                        ComparisonCalculationTree swiftmend = new ComparisonCalculationTree()
                        {
                            Name = "Swiftmend",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.SwiftmendHPS,
                            SubPoints = new float[] { calculationResult.Sustained.SwiftmendHPS }
                        };
                        comparisonList.Add(swiftmend);
                        ComparisonCalculationTree primary = new ComparisonCalculationTree()
                        {
                            Name = "Primary Spell",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.PrimaryHPS,
                            SubPoints = new float[] { calculationResult.Sustained.PrimaryHPS }
                        };
                        comparisonList.Add(primary);
                    }
                    return comparisonList.ToArray();
                case "HPCT per spell":
                    {
                        _subPointNameColors = _subPointNameColorsHPCT;
                        ComparisonCalculationTree rejuv = new ComparisonCalculationTree()
                        {
                            Name = "Rejuvenation",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.rejuvenate.HPCT,
                            SubPoints = new float[] { calculationResult.Sustained.rejuvenate.HPCT }
                        };
                        comparisonList.Add(rejuv);
                        ComparisonCalculationTree regrowth = new ComparisonCalculationTree()
                        {
                            Name = "Regrowth",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.regrowth.HPCT,
                            SubPoints = new float[] { calculationResult.Sustained.regrowth.HPCT }
                        };
                        comparisonList.Add(regrowth);
                        ComparisonCalculationTree lifebloom = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.lifebloom.HPCT,
                            SubPoints = new float[] { calculationResult.Sustained.lifebloom.HPCT }
                        };
                        comparisonList.Add(lifebloom);
                        ComparisonCalculationTree lifebloomRollingStack = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom Rolling Stack",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.lifebloomRollingStack.HPCT,
                            SubPoints = new float[] { calculationResult.Sustained.lifebloomRollingStack.HPCT }
                        };
                        comparisonList.Add(lifebloomRollingStack);
                        ComparisonCalculationTree lifebloomSlowStack = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom Slow Stack",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.lifebloomSlowStack.HPCT,
                            SubPoints = new float[] { calculationResult.Sustained.lifebloomSlowStack.HPCT }
                        };
                        comparisonList.Add(lifebloomSlowStack);
                        ComparisonCalculationTree lifebloomFastStack = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom Fast Stack",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.lifebloomFastStack.HPCT,
                            SubPoints = new float[] { calculationResult.Sustained.lifebloomFastStack.HPCT }
                        };
                        comparisonList.Add(lifebloomFastStack);
                        ComparisonCalculationTree wildGrowth = new ComparisonCalculationTree()
                        {
                            Name = "Wild Growth",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.wildGrowth.HPCT,
                            SubPoints = new float[] { calculationResult.Sustained.wildGrowth.HPCT }
                        };
                        comparisonList.Add(wildGrowth);
                        if (calculationResult.Sustained.swiftmend != null)
                        {
                            ComparisonCalculationTree swiftmend = new ComparisonCalculationTree()
                            {
                                Name = "Swiftmend",
                                Equipped = false,
                                OverallPoints = calculationResult.Sustained.swiftmend.HPCT,
                                SubPoints = new float[] { calculationResult.Sustained.swiftmend.HPCT }
                            };
                            comparisonList.Add(swiftmend);
                        }
                        ComparisonCalculationTree nourish0 = new ComparisonCalculationTree()
                        {
                            Name = "Nourish (0)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.nourish[0].HPCT,
                            SubPoints = new float[] { calculationResult.Sustained.nourish[0].HPCT }
                        };
                        comparisonList.Add(nourish0);
                        ComparisonCalculationTree nourish1 = new ComparisonCalculationTree()
                        {
                            Name = "Nourish (1)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.nourish[1].HPCT,
                            SubPoints = new float[] { calculationResult.Sustained.nourish[1].HPCT }
                        };
                        comparisonList.Add(nourish1);
                        ComparisonCalculationTree nourish2 = new ComparisonCalculationTree()
                        {
                            Name = "Nourish (2)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.nourish[2].HPCT,
                            SubPoints = new float[] { calculationResult.Sustained.nourish[2].HPCT }
                        };
                        comparisonList.Add(nourish2);
                        ComparisonCalculationTree nourish3 = new ComparisonCalculationTree()
                        {
                            Name = "Nourish (3)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.nourish[3].HPCT,
                            SubPoints = new float[] { calculationResult.Sustained.nourish[3].HPCT }
                        };
                        comparisonList.Add(nourish3);
                        ComparisonCalculationTree nourish4 = new ComparisonCalculationTree()
                        {
                            Name = "Nourish (4)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.nourish[4].HPCT,
                            SubPoints = new float[] { calculationResult.Sustained.nourish[4].HPCT }
                        };
                        comparisonList.Add(nourish4);
                    }
                    return comparisonList.ToArray();
                case "HPS per spell":
                    {
                        _subPointNameColors = _subPointNameColorsHPS;
                        ComparisonCalculationTree rejuv = new ComparisonCalculationTree()
                        {
                            Name = "Rejuvenation",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.rejuvenate.HPCTD,
                            SubPoints = new float[] { calculationResult.Sustained.rejuvenate.HPCTD }
                        };
                        comparisonList.Add(rejuv);
                        ComparisonCalculationTree regrowth = new ComparisonCalculationTree()
                        {
                            Name = "Regrowth",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.regrowth.HPCTD,
                            SubPoints = new float[] { calculationResult.Sustained.regrowth.HPCTD }
                        };
                        comparisonList.Add(regrowth);
                        ComparisonCalculationTree lifebloom = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.lifebloom.HPCTD,
                            SubPoints = new float[] { calculationResult.Sustained.lifebloom.HPCTD }
                        };
                        comparisonList.Add(lifebloom);
                        ComparisonCalculationTree lifebloomRollingStack = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom Rolling Stack",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.lifebloomRollingStack.HPCTD,
                            SubPoints = new float[] { calculationResult.Sustained.lifebloomRollingStack.HPCTD }
                        };
                        comparisonList.Add(lifebloomRollingStack);
                        ComparisonCalculationTree lifebloomSlowStack = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom Slow Stack",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.lifebloomSlowStack.HPCTD,
                            SubPoints = new float[] { calculationResult.Sustained.lifebloomSlowStack.HPCTD }
                        };
                        comparisonList.Add(lifebloomSlowStack);
                        ComparisonCalculationTree lifebloomFastStack = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom Fast Stack",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.lifebloomFastStack.HPCTD,
                            SubPoints = new float[] { calculationResult.Sustained.lifebloomFastStack.HPCTD }
                        };
                        comparisonList.Add(lifebloomFastStack);
                        ComparisonCalculationTree wildGrowth = new ComparisonCalculationTree()
                        {
                            Name = "Wild Growth",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.wildGrowth.HPCTD,
                            SubPoints = new float[] { calculationResult.Sustained.wildGrowth.HPCTD }
                        };
                        comparisonList.Add(wildGrowth);
                        if (calculationResult.Sustained.swiftmend != null)
                        {
                            ComparisonCalculationTree swiftmend = new ComparisonCalculationTree()
                            {
                                Name = "Swiftmend",
                                Equipped = false,
                                OverallPoints = calculationResult.Sustained.swiftmend.HPCTD,
                                SubPoints = new float[] { calculationResult.Sustained.swiftmend.HPCTD }
                            };
                            comparisonList.Add(swiftmend);
                        }
                        ComparisonCalculationTree nourish0 = new ComparisonCalculationTree()
                        {
                            Name = "Nourish (0)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.nourish[0].HPCTD,
                            SubPoints = new float[] { calculationResult.Sustained.nourish[0].HPCTD }
                        };
                        comparisonList.Add(nourish0);
                        ComparisonCalculationTree nourish1 = new ComparisonCalculationTree()
                        {
                            Name = "Nourish (1)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.nourish[1].HPCTD,
                            SubPoints = new float[] { calculationResult.Sustained.nourish[1].HPCTD }
                        };
                        comparisonList.Add(nourish1);
                        ComparisonCalculationTree nourish2 = new ComparisonCalculationTree()
                        {
                            Name = "Nourish (2)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.nourish[2].HPCTD,
                            SubPoints = new float[] { calculationResult.Sustained.nourish[2].HPCTD }
                        };
                        comparisonList.Add(nourish2);
                        ComparisonCalculationTree nourish3 = new ComparisonCalculationTree()
                        {
                            Name = "Nourish (3)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.nourish[3].HPCTD,
                            SubPoints = new float[] { calculationResult.Sustained.nourish[3].HPCTD }
                        };
                        comparisonList.Add(nourish3);
                        ComparisonCalculationTree nourish4 = new ComparisonCalculationTree()
                        {
                            Name = "Nourish (4)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.nourish[4].HPCTD,
                            SubPoints = new float[] { calculationResult.Sustained.nourish[4].HPCTD }
                        };
                        comparisonList.Add(nourish4);
                    }
                    return comparisonList.ToArray();
                case "HPM per spell":
                    {
                        _subPointNameColors = _subPointNameColorsHPM;
                        ComparisonCalculationTree rejuv = new ComparisonCalculationTree()
                        {
                            Name = "Rejuvenation",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.rejuvenate.HPM,
                            SubPoints = new float[] { calculationResult.Sustained.rejuvenate.HPM }
                        };
                        comparisonList.Add(rejuv);
                        ComparisonCalculationTree regrowth = new ComparisonCalculationTree()
                        {
                            Name = "Regrowth",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.regrowth.HPM,
                            SubPoints = new float[] { calculationResult.Sustained.regrowth.HPM }
                        };
                        comparisonList.Add(regrowth);
                        ComparisonCalculationTree lifebloom = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.lifebloom.HPM,
                            SubPoints = new float[] { calculationResult.Sustained.lifebloom.HPM }
                        };
                        comparisonList.Add(lifebloom);
                        ComparisonCalculationTree lifebloomRollingStack = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom Rolling Stack",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.lifebloomRollingStack.HPM,
                            SubPoints = new float[] { calculationResult.Sustained.lifebloomRollingStack.HPM }
                        };
                        comparisonList.Add(lifebloomRollingStack);
                        ComparisonCalculationTree lifebloomSlowStack = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom Slow Stack",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.lifebloomSlowStack.HPM,
                            SubPoints = new float[] { calculationResult.Sustained.lifebloomSlowStack.HPM }
                        };
                        comparisonList.Add(lifebloomSlowStack);
                        ComparisonCalculationTree lifebloomFastStack = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom Fast Stack",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.lifebloomFastStack.HPM,
                            SubPoints = new float[] { calculationResult.Sustained.lifebloomFastStack.HPM }
                        };
                        comparisonList.Add(lifebloomFastStack);
                        ComparisonCalculationTree wildGrowth = new ComparisonCalculationTree()
                        {
                            Name = "Wild Growth",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.wildGrowth.HPM,
                            SubPoints = new float[] { calculationResult.Sustained.wildGrowth.HPM }
                        };
                        comparisonList.Add(wildGrowth);
                        if (calculationResult.Sustained.swiftmend != null)
                        {
                            ComparisonCalculationTree swiftmend = new ComparisonCalculationTree()
                            {
                                Name = "Swiftmend",
                                Equipped = false,
                                OverallPoints = calculationResult.Sustained.swiftmend.HPM,
                                SubPoints = new float[] { calculationResult.Sustained.swiftmend.HPM }
                            };
                            comparisonList.Add(swiftmend);
                        }
                        ComparisonCalculationTree nourish0 = new ComparisonCalculationTree()
                        {
                            Name = "Nourish (0)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.nourish[0].HPM,
                            SubPoints = new float[] { calculationResult.Sustained.nourish[0].HPM }
                        };
                        comparisonList.Add(nourish0);
                        ComparisonCalculationTree nourish1 = new ComparisonCalculationTree()
                        {
                            Name = "Nourish (1)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.nourish[1].HPM,
                            SubPoints = new float[] { calculationResult.Sustained.nourish[1].HPM }
                        };
                        comparisonList.Add(nourish1);
                        ComparisonCalculationTree nourish2 = new ComparisonCalculationTree()
                        {
                            Name = "Nourish (2)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.nourish[2].HPM,
                            SubPoints = new float[] { calculationResult.Sustained.nourish[2].HPM }
                        };
                        comparisonList.Add(nourish2);
                        ComparisonCalculationTree nourish3 = new ComparisonCalculationTree()
                        {
                            Name = "Nourish (3)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.nourish[3].HPM,
                            SubPoints = new float[] { calculationResult.Sustained.nourish[3].HPM }
                        };
                        comparisonList.Add(nourish3);
                        ComparisonCalculationTree nourish4 = new ComparisonCalculationTree()
                        {
                            Name = "Nourish (4)",
                            Equipped = false,
                            OverallPoints = calculationResult.Sustained.nourish[4].HPM,
                            SubPoints = new float[] { calculationResult.Sustained.nourish[4].HPM }
                        };
                        comparisonList.Add(nourish4);
                    }
                    return comparisonList.ToArray();
                case "Healing per spell (single target)":
                    {
                        _subPointNameColors = _subPointNameColorsHPS;
                        ComparisonCalculationTree rejuv = new ComparisonCalculationTree()
                        {
                            Name = "Rejuvenation",
                            Equipped = false,
                            OverallPoints = calculationResult.SingleTarget[0].rejuvContribution,
                            SubPoints = new float[] { calculationResult.SingleTarget[0].rejuvContribution }
                        };
                        comparisonList.Add(rejuv);
                        ComparisonCalculationTree regrowth = new ComparisonCalculationTree()
                        {
                            Name = "Regrowth",
                            Equipped = false,
                            OverallPoints = calculationResult.SingleTarget[0].regrowthContribution,
                            SubPoints = new float[] { calculationResult.SingleTarget[0].regrowthContribution }
                        };
                        comparisonList.Add(regrowth);
                        ComparisonCalculationTree lifebloomStack = new ComparisonCalculationTree()
                        {
                            Name = "Lifebloom Stack",
                            Equipped = false,
                            OverallPoints = calculationResult.SingleTarget[0].lifebloomContribution,
                            SubPoints = new float[] { calculationResult.SingleTarget[0].lifebloomContribution }
                        };
                        comparisonList.Add(lifebloomStack);
                        if (calculationResult.Sustained.swiftmend != null)
                        {
                            ComparisonCalculationTree swiftmend = new ComparisonCalculationTree()
                            {
                                Name = "Swiftmend",
                                Equipped = false,
                                OverallPoints = calculationResult.SingleTarget[0].swiftmendContribution,
                                SubPoints = new float[] { calculationResult.SingleTarget[0].swiftmendContribution }
                            };
                            comparisonList.Add(swiftmend);
                        }
                        ComparisonCalculationTree primary = new ComparisonCalculationTree()
                        {
                            Name = "Nourish",
                            Equipped = false,
                            OverallPoints = calculationResult.SingleTarget[0].nourishContribution,
                            SubPoints = new float[] { calculationResult.SingleTarget[0].nourishContribution }
                        };
                        comparisonList.Add(primary);
                    }
                    return comparisonList.ToArray();
                case "Single target spell mixes":
                    {
                        _subPointNameColors = _subPointNameColorsHPS;
                        for (int i = 2; i < 17; i++)
                        {

                            ComparisonCalculationTree spell = new ComparisonCalculationTree()
                            {
                                Name = Solver.SingleTargetRotationToText(calculationResult.SingleTarget[i].rotation),
                                Equipped = false,
                                OverallPoints = calculationResult.SingleTarget[i].HPS,
                                SubPoints = new float[] { calculationResult.SingleTarget[i].HPS }
                            };
                            comparisonList.Add(spell);
                        }
                    }
                    return comparisonList.ToArray();
                default:
                    return new ComparisonCalculationBase[0];
            }
        }
        public override Stats GetRelevantStats(Stats stats) {
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
                SpellsManaReduction = stats.SpellsManaReduction,
                HealingOmenProc = stats.HealingOmenProc, 
                #endregion
                #region Idols and Sets
                LifebloomCostReduction = stats.LifebloomCostReduction,  //T7 (2) Bonus
                NourishBonusPerHoT = stats.NourishBonusPerHoT,          //T7 (4) Bonus
                SwiftmendBonus = stats.SwiftmendBonus,                   //T8 (2) Bonus
                RejuvenationInstantTick = stats.RejuvenationInstantTick, //T8 (4) Bonus
                NourishCritBonus = stats.NourishCritBonus,              // T9 (2) Bonus
                RejuvenationCrit = stats.RejuvenationCrit,              // T9 (4) Bonus
                WildGrowthLessReduction = stats.WildGrowthLessReduction, // T10 (2) Bonus
                RejuvJumpChance = stats.RejuvJumpChance,                 // T10 (4) Bonus
                NourishSpellpower = stats.NourishSpellpower, // Idol of the Flourishing Life
                RejuvenationHealBonus = stats.RejuvenationHealBonus, // Idol of Pure Thoughts (lvl74)
                ReduceRejuvenationCost = stats.ReduceRejuvenationCost, // Idol of Awakening (lvl80) 
                LifebloomTickHealBonus = stats.LifebloomTickHealBonus, // Idol of Lush Mosh
                HealingTouchFinalHealBonus = stats.HealingTouchFinalHealBonus, // Idol of Health                 
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
                   + stats.Mp5 + stats.Healed + stats.HighestStat + stats.BonusHealingReceived + stats.HealingOmenProc
                   + stats.ShieldFromHealed + stats.ManaRestoreFromMaxManaPerSecond) > 0;
        }
        public override bool HasRelevantStats(Stats stats) {
            if (HasRelevantSpecialEffectStats(stats)) return true;

            if (stats.Intellect + stats.Spirit + stats.Mp5 + stats.SpellPower + stats.Mana + stats.CritRating + stats.SpellCrit
                + stats.HasteRating + stats.SpellHaste + stats.BonusSpellPowerMultiplier
                + stats.BonusSpiritMultiplier + stats.BonusIntellectMultiplier + stats.BonusStaminaMultiplier
                + stats.BonusCritHealMultiplier + stats.BonusManaMultiplier
                + stats.Armor + stats.Stamina + stats.ManaRestoreFromMaxManaPerSecond
                + stats.SpellCombatManaRegeneration // Bangle of nerfed - might be useful in future
                #region Trinkets
                + stats.HighestStat + stats.BonusManaPotion + stats.SpellsManaReduction + stats.HealingOmenProc
                #endregion
                #region Idols and Sets
                + stats.LifebloomCostReduction + stats.NourishBonusPerHoT // T7
                + stats.SwiftmendBonus + stats.RejuvenationInstantTick // T8
                + stats.NourishCritBonus + stats.RejuvenationCrit // T9
                + stats.WildGrowthLessReduction + stats.RejuvJumpChance // T10
                + stats.NourishSpellpower // Idol of the Flourishing Life
                + stats.RejuvenationHealBonus // Idol of Pure Thoughts (lvl74)
                + stats.ReduceRejuvenationCost // Idol of Awakening (lvl80) 
                + stats.LifebloomTickHealBonus // Idol of Lush Mosh
                + stats.HealingTouchFinalHealBonus // Idol of Health       
                #endregion
                > 0)
                return true;

            return false;
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
