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
        public float ManaPer5OutRotation;
        public float ManaFromInnervate;
        public float ReplenishRegen;
        public float TotalMod;
        public float TotalHealing;
        public float TotalCastsPerMinute;
        public float TotalCritsPerMinute;
        public float TotalHealsPerMinute;
        public float UnusedMana;
        public float UnusedCastTimeFrac;
    }

    public enum HealTargetTypes { TankHealing = 0, RaidHealing = 1 };

    [Rawr.Calculations.RawrModelInfo("Tree", "Ability_Druid_TreeofLife", Character.CharacterClass.Druid)]
    class CalculationsTree : CalculationsBase
    {
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

        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("HealBurst", System.Drawing.Color.Red);
                    _subPointNameColors.Add("HealSustained", System.Drawing.Color.Blue);
                }
                return _subPointNameColors;
            }
        }

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
                        "Points:Overall",

                        "Basic Stats:Health",
                        "Basic Stats:Mana",
                        "Basic Stats:Stamina",
                        "Basic Stats:Intellect",
                        "Basic Stats:Spirit",
                        "Basic Stats:Healing",
                        "Basic Stats:MP5",
                        "Basic Stats:Spell Crit",
                        "Basic Stats:Spell Haste",
                        "Basic Stats:Global CD",
                        "Basic Stats:Lifebloom Global CD",

                        "Simulation:Result",
                        "Simulation:Time until OOM",
                        "Simulation:Unused Mana Remaining",
                        "Simulation:Unused cast time percentage",
                        "Simulation:Total healing done",
                        "Simulation:HPS for tank HoTs",
                        "Simulation:MPS for tank HoTs",
                        "Simulation:HPS for Wild Growth",
                        "Simulation:MPS for Wild Growth",
                        "Simulation:HPS for Swiftmend",
                        "Simulation:MPS for Swiftmend",
                        "Simulation:HoT refresh fraction",
                        "Simulation:HPS for primary heal",
                        "Simulation:MPS for primary heal",
                        "Simulation:Mana regen per second",
                        "Simulation:Casts per minute until OOM",
                        "Simulation:Crits per minute until OOM",

                        "Lifebloom:LB Tick",
                        "Lifebloom:LB Heal",
                        "Lifebloom:LB HPS",
                        "Lifebloom:LB HPM",

                        "Lifebloom Stacked Blooms:LBx2 HPS",
                        "Lifebloom Stacked Blooms:LBx2 HPM",
                        "Lifebloom Stacked Blooms:LBx3 HPS",
                        "Lifebloom Stacked Blooms:LBx3 HPM",

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


        private CalculationOptionsPanelTree _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                    _calculationOptionsPanel = new CalculationOptionsPanelTree();
                return _calculationOptionsPanel;
            }
        }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    // I don't know of a fist weapon or two hand mace with healing stats, so...
                    // I don't know either .. but i know there are a few with spell power
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]{
                        Item.ItemType.None,
                        Item.ItemType.Cloth,
                        Item.ItemType.Leather,
                        Item.ItemType.Dagger,
                        Item.ItemType.FistWeapon,
                        Item.ItemType.Idol,
                        Item.ItemType.OneHandMace,
                        Item.ItemType.TwoHandMace,
                        Item.ItemType.Staff
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override Character.CharacterClass TargetClass
        {
            get { return Character.CharacterClass.Druid; }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationTree();
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsTree();
        }

        private static Stats getTrinketStats(Character character, Stats stats, float FightDuration, float CastInterval, float HealInterval, float CritsRatio, out float Healing)
        {
            #region New_SpecialEffect_Handling

            Stats resultNew = new Stats();
            foreach (Rawr.SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use)
                {
                  resultNew += effect.GetAverageStats(0.0f, 1.0f, 2.0f, FightDuration);   // 0 cooldown, 100% chance to use
                }
                else if (effect.Trigger == Trigger.SpellCast ||  effect.Trigger == Trigger.SpellHit)
                {
                    resultNew += effect.GetAverageStats(CastInterval, 1.0f, CastInterval, FightDuration);
                }
                else if ( effect.Trigger == Trigger.HealingSpellCast || effect.Trigger == Trigger.HealingSpellHit)
                {
                    // Heal interval measures time between HoTs as well, direct heals are a different interval
                    resultNew += effect.GetAverageStats(HealInterval, 1.0f, CastInterval, FightDuration);
                }
                else if (effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.HealingSpellCrit )
                {
                    resultNew += effect.GetAverageStats(CastInterval, CritsRatio, CastInterval, FightDuration);
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
                
            };
        }
        
        protected Rotation SimulateHealing(CharacterCalculationsTree calculatedStats, Stats stats, CalculationOptionsTree calcOpts, int wgPerMin, bool rejuvOnTank, bool rgOnTank, int lbOnTank, int nTanks, int swiftmendPMin, Spell primaryHeal, float primaryFrac, HealTargetTypes primaryHealTarget)
        {
            float lifebloomDuration = 0.0f;
            //Value passed in primaryFrac, not used anymore, this value is calculated to let mana last for the fight duration
            #region Spells
            int hots = 0;
            if (rejuvOnTank) hots++;
            if (rgOnTank) hots++;
            if (lbOnTank > 0) hots++;
            //Spell regrowth = new Regrowth(calculatedStats, stats, false);
            Spell regrowth = new Regrowth(calculatedStats, stats, true);
            Spell lifebloom = null;
            if ((lbOnTank >= 1)&&(lbOnTank <= 3))
            {
                lifebloom = new Lifebloom(calculatedStats, stats, lbOnTank);
                lifebloomDuration = lifebloom.Duration + 1.0f; // Add 1 sec, to make sure bloom has taken place, before applying again
            }
            else if (lbOnTank == 4)
            {
                lifebloom = new LifebloomStack(calculatedStats, stats);
                lifebloomDuration = lifebloom.Duration;
            }
            
            Spell rejuvenate = new Rejuvenation(calculatedStats, stats);
            //Spell nourish = new Nourish(calculatedStats, stats);
            //Spell nourishWithHoT = new Nourish(calculatedStats, stats, hots > 0 ? hots : 1);
            //Spell healingTouch = new HealingTouch(calculatedStats, stats);
            WildGrowth wildGrowth = new WildGrowth(calculatedStats, stats);
            #endregion

            float castsPerMinute = 0;
            float critsPerMinute = 0;
            float healsPerMinute = 0;

            #region WildGrowthPerMinute
            float wgCastTime = wildGrowth.CastTime / 60f * wgPerMin;
            float wgMPS = wildGrowth.ManaCost / 60f * wgPerMin;
            float wgHPS = wildGrowth.PeriodicTick * wildGrowth.maxTargets * wildGrowth.Duration / 60f * wgPerMin;  // Assume no overhealing
            castsPerMinute += wgPerMin;
            healsPerMinute += wgPerMin * 10; // assumption it will go 10 times ;)
            #endregion

            #region HotsOnTanks
            float hotsHPS = 0;
            float hotsCastTime = 0;
            float hotsMPS = 0;
            float hotsCastsPerMinute = 0;
            float hotsCritsPerMinute = 0;
            float hotsHealsPerMinute = 0;
            if (rejuvOnTank)
            {
                hotsHPS += rejuvenate.HPSHoT + (rejuvenate.HPS / rejuvenate.Duration); // rejuvenate.HPS to cater for instant tick from T8_4 set bonus
                hotsMPS += rejuvenate.ManaCost / rejuvenate.Duration;
                hotsCastTime += rejuvenate.CastTime / rejuvenate.Duration;
                hotsCastsPerMinute += 60f / rejuvenate.Duration;
                hotsHealsPerMinute += 20; // hot component
            }
            if (rgOnTank)
            {
                hotsHPS += regrowth.HPSHoT + regrowth.HPS / regrowth.Duration;
                hotsMPS += regrowth.ManaCost / regrowth.Duration;
                hotsCastTime += regrowth.CastTime / regrowth.Duration;
                hotsCastsPerMinute += 60f / regrowth.Duration;
                hotsCritsPerMinute += 60f / regrowth.Duration * regrowth.CritPercent / 100f;
                hotsHealsPerMinute += 60f / regrowth.Duration; // direct component
                hotsHealsPerMinute += 20; // hot component
            }
            if ((lbOnTank > 0)&&(lifebloom != null))
            {
                           // HoT part                 Bloom Part
                hotsHPS += lifebloom.HPSHoT + lifebloom.AverageHealingwithCrit / lifebloomDuration;
                hotsMPS += lifebloom.ManaCost / lifebloomDuration;
                hotsCastTime += lifebloom.CastTime / lifebloomDuration;
                hotsCastsPerMinute += 60f / lifebloomDuration;
                hotsHealsPerMinute += 60; // hot component
                hotsCritsPerMinute += 60f / lifebloomDuration * lifebloom.CritPercent / 100f;  // Bloom crits
            }
            hotsHPS *= nTanks;
            hotsMPS *= nTanks;
            hotsCastTime *= nTanks;
            hotsCastsPerMinute *= nTanks;
            hotsCritsPerMinute *= nTanks;
            hotsHealsPerMinute *= nTanks;
            castsPerMinute += hotsCastsPerMinute;
            critsPerMinute += hotsCritsPerMinute;
            healsPerMinute += hotsHealsPerMinute;
            #endregion

            #region Swiftmend
            Swiftmend swift;
            float swiftHPS = 0.0f;
            float swiftMPS = 0.0f;
            float swiftCastTime = 0.0f;
            if ( (hots > 0) && (swiftmendPMin > 0) )
            {

                if (rejuvOnTank)
                    if (rgOnTank)
                        swift = new Swiftmend(calculatedStats, stats, rejuvenate, regrowth);
                    else
                        swift = new Swiftmend(calculatedStats, stats, rejuvenate, null);
                else
                    if (rgOnTank)
                        swift = new Swiftmend(calculatedStats, stats, null, regrowth);
                    else
                        swift = new Swiftmend(calculatedStats, stats, null, null);

                swiftCastTime = swift.CastTime * swiftmendPMin /60.0f;
                swiftHPS = swift.TotalAverageHealing * swiftmendPMin / 60.0f;
                swiftMPS = swift.ManaCost * swiftmendPMin / 60.0f;
                castsPerMinute += swiftmendPMin;
                healsPerMinute += swiftmendPMin;
                critsPerMinute += swift.CritPercent / 100.0f * swiftmendPMin;

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
                hotsMPS += swift.rejuvUseChance * swift.rejuvTicksLost / rejuvenate.PeriodicTicks * rejuvenate.ManaCost * swiftmendPMin / 60.0f;
                hotsMPS += swift.regrowthUseChance * swift.regrowthTicksLost / regrowth.PeriodicTicks * regrowth.ManaCost * swiftmendPMin / 60.0f;
                // Replacing Regrowths gives extra direct heals
                hotsHPS += swift.regrowthUseChance * swift.regrowthTicksLost / regrowth.PeriodicTicks * regrowth.AverageHealingwithCrit * swiftmendPMin / 60.0f; 
                // Replacing HoTs take extra time
                hotsCastTime += swift.rejuvUseChance * swift.rejuvTicksLost / rejuvenate.PeriodicTicks * rejuvenate.CastTime * swiftmendPMin / 60.0f;
                hotsCastTime += swift.regrowthUseChance * swift.regrowthTicksLost / regrowth.PeriodicTicks * regrowth.CastTime * swiftmendPMin / 60.0f;
                #endregion
                 /* */

            }
            #endregion

            #region Mana regeneration
            float spiritRegen = CalculateManaRegen(stats.Intellect, stats.Spirit);
            float spiritRegenPlusMDF = CalculateManaRegen(stats.Intellect, stats.ExtraSpiritWhileCasting + stats.Spirit);

            /*     TODO: Decide if we can into CalculateManaRegen. Check if we still need SpellCombatManaRegeneration scaling
                      if (calcOpts.newManaRegen) */
            {
                spiritRegen *= 0.6f;
                spiritRegenPlusMDF *= 0.6f; 
                stats.SpellCombatManaRegeneration *= 5f / 3f;
            } 
            
            float replenishment = stats.Mana * 0.0025f * 5 * (calcOpts.ReplenishmentUptime / 100f);
            float ManaRegenInFSR = stats.Mp5 + replenishment + spiritRegenPlusMDF * stats.SpellCombatManaRegeneration;
            float ManaRegenOutFSR = stats.Mp5 + replenishment + spiritRegenPlusMDF;
            float ManaRegenOutFSRNoCast =  stats.Mp5 + replenishment + 0.2f * spiritRegen + 0.8f * spiritRegenPlusMDF;

            float ratio = .01f * calcOpts.FSRRatio;
            float manaRegen = ratio * ManaRegenInFSR + (1 - ratio) * ManaRegenOutFSR;
            #endregion

            float extraMana = 0f;

            #region Mana potion
            float extraManaFromPot = new int[] { 0, 1800, 2200, 2400, 4300 }[calcOpts.ManaPot];
            extraManaFromPot *= (stats.BonusManaPotion + 1f);
            extraMana += extraManaFromPot;
            #endregion

            #region Innervates
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
            // lets assume the mana return is maximally 95% of your mana
            // thus take the smaller value of 95% of mana pool and total mana regenerated
            manaFromInnervate = Math.Min(manaFromInnervate, .95f * stats.Mana);

            extraMana += manaFromInnervate;
            #endregion


            #region Determine if Mana or GCD limited
//            if (calcOpts.newManaRegen)
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
            if (primaryHealTarget == HealTargetTypes.RaidHealing)
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
            if (TotalTime > TimeToOOM)
            {
                float TimeToRegenAll = 5f * stats.Mana / ManaRegenOutFSRNoCast;
                float TimeToBurnAll = stats.Mana / EffectiveManaBurn;
                //CvRFraction = (stats.Mana / EffectiveManaBurn) / TotalCycle;
                TotalMod = 1f + (TotalTime - TimeToOOM) / (TimeToRegenAll + TimeToBurnAll);
            }
            #endregion

            return new Rotation() {
                HPS = hps,
                MaxHPS = hotsHPS + hpsHeal100 + wgHPS + swiftHPS,
                MPS = mps,
                HPSFromPrimary = hpsHealing,
                HPSFromHots = hotsHPS,
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
                ManaPer5OutRotation = ManaRegenOutFSRNoCast,
                ManaFromInnervate = manaFromInnervate,
                TimeToOOM = TimeToOOM,
                TotalMod = TotalMod,
                TotalTime = TotalTime,
                TotalHealing = TimeToOOM * hps * TotalMod,
                TotalCastsPerMinute = castsPerMinute * TotalMod,
                TotalCritsPerMinute = critsPerMinute * TotalMod,
                TotalHealsPerMinute = healsPerMinute * TotalMod,
                ReplenishRegen = replenishment,
                UnusedMana = unusedMana,
                UnusedCastTimeFrac = unusedCastTimeFrac,
            };
        }

        protected Rotation predefinedRotation(int rotation, Stats stats, CalculationOptionsTree calcOpts, CharacterCalculationsTree calculatedStats)
        {
            Rotation result = null;
            bool rejuvOnTank, rgOnTank;
            int lifeBloomStackSize, noTanks;
            Spell primaryHeal;
            HealTargetTypes healTarget;
            int SwiftmendPerMin = 0;

            switch (rotation)
            {
                case 0:
                default:
                    // 1 Tank (RJ/RG/LB/N*)
                    {
                        noTanks = 1;
                        rejuvOnTank = true;
                        rgOnTank = true;
                        lifeBloomStackSize = 4;
                        primaryHeal = new Nourish(calculatedStats, stats, 3);
                        healTarget = HealTargetTypes.TankHealing;

                    }
                    break;
                case 1:
                    // 2 Tanks (RJ/RG/LB/N*)
                    {
                        noTanks = 2;
                        rejuvOnTank = true;
                        rgOnTank = true;
                        lifeBloomStackSize = 4;
                        primaryHeal = new Nourish(calculatedStats, stats, 3);
                        healTarget = HealTargetTypes.TankHealing;
                    }
                    break;
                case 2:
                    // 1 Tank (RJ/RG/LB/HT*)
                    {
                        noTanks = 1;
                        rejuvOnTank = true;
                        rgOnTank = true;
                        lifeBloomStackSize = 4;
                        primaryHeal = new HealingTouch(calculatedStats, stats);
                        healTarget = HealTargetTypes.TankHealing;
                    }
                    break;
                case 3:
                    // 2 Tanks (RJ/RG/LB/HT*)
                    {
                        noTanks = 2;
                        rejuvOnTank = true;
                        rgOnTank = true;
                        lifeBloomStackSize = 4;
                        primaryHeal = new HealingTouch(calculatedStats, stats);
                        healTarget = HealTargetTypes.TankHealing;
                    }
                    break;
                case 4:
                    // 1 Tank (RJ/RG/LB/RG*)
                    {
                        noTanks = 1;
                        rejuvOnTank = true;
                        rgOnTank = true;
                        lifeBloomStackSize = 4;
                        primaryHeal = new Regrowth(calculatedStats, stats, true);
                        healTarget = HealTargetTypes.TankHealing;
                    }
                    break;
                case 5:
                    // 2 Tanks (RJ/RG/LB/RG*)
                    {
                        noTanks = 2;
                        rejuvOnTank = true;
                        rgOnTank = true;
                        lifeBloomStackSize = 4;
                        primaryHeal = new Regrowth(calculatedStats, stats, true);
                        healTarget = HealTargetTypes.TankHealing;
                    }
                    break;
                case 6:
                    // RG Raid (1 Tank RJ/LB)
                    {
                        noTanks = 1;
                        rejuvOnTank = true;
                        rgOnTank = false;
                        lifeBloomStackSize = 4;
                        primaryHeal = new Regrowth(calculatedStats, stats, false);
                        healTarget = HealTargetTypes.RaidHealing;
                    }
                    break;
                case 7:
                    // RG Raid (2 Tanks RJ/LB)
                    {
                        noTanks = 2;
                        rejuvOnTank = true;
                        rgOnTank = false;
                        lifeBloomStackSize = 4;
                        primaryHeal = new Regrowth(calculatedStats, stats, false);
                        healTarget = HealTargetTypes.RaidHealing;
                    }
                    break;
                case 8:
                    // RJ Raid (1 Tank RJ/LB)
                    {
                        noTanks = 1;
                        rejuvOnTank = true;
                        rgOnTank = false;
                        lifeBloomStackSize = 4;
                        primaryHeal = new Rejuvenation(calculatedStats, stats);
                        healTarget = HealTargetTypes.RaidHealing;
                    }
                    break;
                case 9:
                    // RJ Raid (2 Tanks RJ/LB)
                    {
                        noTanks = 2;
                        rejuvOnTank = true;
                        rgOnTank = false;
                        lifeBloomStackSize = 4;
                        primaryHeal = new Rejuvenation(calculatedStats, stats);
                        healTarget = HealTargetTypes.RaidHealing;

                    }
                    break;
                case 10:
                    // N Raid (1 Tank RJ/LB)
                    {
                        noTanks = 1;
                        rejuvOnTank = true;
                        rgOnTank = false;
                        lifeBloomStackSize = 4;
                        primaryHeal = new Nourish(calculatedStats, stats, 0);
                        healTarget = HealTargetTypes.RaidHealing;
                    }
                    break;
                case 11:
                    // N Raid (2 Tanks RJ/LB)
                    {
                        noTanks = 2;
                        rejuvOnTank = true;
                        rgOnTank = false;
                        lifeBloomStackSize = 4;
                        primaryHeal = new Nourish(calculatedStats, stats, 0);
                        healTarget = HealTargetTypes.RaidHealing;

                    }
                    break;
                case 12:
                    // N spam
                    {
                        noTanks = 0;
                        rejuvOnTank = false;
                        rgOnTank = false;
                        lifeBloomStackSize = 0;
                        primaryHeal = new Nourish(calculatedStats, stats);
                        healTarget = HealTargetTypes.TankHealing;

                    }
                    break;
                case 13:
                    // HT spam
                    {
                        noTanks = 0;
                        rejuvOnTank = false;
                        rgOnTank = false;
                        lifeBloomStackSize = 0;
                        primaryHeal = new HealingTouch(calculatedStats, stats);
                        healTarget = HealTargetTypes.TankHealing;

                    }
                    break;
                case 14:
                    // RG spam
                    {
                        noTanks = 0;
                        rejuvOnTank = false;
                        rgOnTank = false;
                        lifeBloomStackSize = 0;
                        primaryHeal = new Regrowth(calculatedStats, stats, true);
                        healTarget = HealTargetTypes.TankHealing;
                    }
                    break;
                case 15:
                    // 2 Tanks (RJ/RG/1xLB*/RG*)
                    {
                        noTanks = 2;
                        rejuvOnTank = true;
                        rgOnTank = true;
                        lifeBloomStackSize = 1;
                        primaryHeal = new Regrowth(calculatedStats, stats, true);
                        healTarget = HealTargetTypes.TankHealing;
                    }
                    break;
                case 16:
                    // 2 Tanks (RJ/RG/2xLB*/RG*)
                    {
                        noTanks = 2;
                        rejuvOnTank = true;
                        rgOnTank = true;
                        lifeBloomStackSize = 2;
                        primaryHeal = new Regrowth(calculatedStats, stats, true);
                        healTarget = HealTargetTypes.TankHealing;
                    }
                    break;
                case 17:
                    // 2 Tanks (RJ/RG/3xLB*/RG*)
                    {
                        noTanks = 2;
                        rejuvOnTank = true;
                        rgOnTank = true;
                        lifeBloomStackSize = 3;
                        primaryHeal = new Regrowth(calculatedStats, stats, true);
                        healTarget = HealTargetTypes.TankHealing;
                    }
                    break;
  /*              case 18:
                    // 2 Tanks (RJ/RG/3xLB* /RG* /SM)
                    {
                        SwiftmendPerMin = 4;
                        noTanks = 2;
                        rejuvOnTank = true;
                        rgOnTank = true;
                        lifeBloomStackSize = 3;
                        primaryHeal = new Regrowth(calculatedStats, stats, true);
                        healTarget = HealTargetTypes.TankHealing;
                    }
                    break;
 */
            }
            SwiftmendPerMin = calcOpts.SwiftmendPerMinute;
            result = SimulateHealing(
                            calculatedStats, stats, calcOpts, calcOpts.WildGrowthPerMinute,
                            rejuvOnTank, rgOnTank, lifeBloomStackSize, noTanks, SwiftmendPerMin,
                            primaryHeal,
                            .01f * calcOpts.MainSpellFraction, healTarget);
            return result;
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
            Rotation rot = predefinedRotation(calcOpts.Rotation, stats, calcOpts, calculatedStats);
            int nPasses = 3, k;
            for (k = 0; k < nPasses; k++)
            {
                Stats procs = getTrinketStats(calculatedStats.LocalCharacter, stats,
                    rot.TotalTime, 60f / rot.TotalCastsPerMinute,
                    60f / rot.TotalHealsPerMinute, rot.TotalCritsPerMinute / rot.TotalCastsPerMinute,
                    out ExtraHPS);

                
                // Create a new stats instance that uses the proc effects
                combinedStats = GetCharacterStats(character, additionalItem, procs);
                rot = predefinedRotation(calcOpts.Rotation, combinedStats, calcOpts, calculatedStats);
//                rot = predefinedRotation(calcOpts.Rotation, stats + procs, calcOpts, calculatedStats);
            }
            calculatedStats.Simulation = rot;
            calculatedStats.BasicStats = combinedStats;     // Replace BasicStats to get Spirit while casting included
            #endregion

            calculatedStats.BurstPoints = rot.MaxHPS + ExtraHPS;
            calculatedStats.SustainedPoints = (rot.TotalHealing + ExtraHPS * rot.TotalTime) / rot.TotalTime;

            // Penalty
            if (rot.TimeToOOM < rot.TotalTime)
            {
                float frac = rot.TimeToOOM / rot.TotalTime;
                float mod = (float)Math.Pow(frac, 1 + 0.05f ) / frac;
                calculatedStats.SustainedPoints *= mod;
                if (calcOpts.PenalizeEverything)
                {
                    calculatedStats.BurstPoints *= mod;
                }
            }

            // ADJUST POINT VALUE (BURST SUSTAINED RATIO)
            float bsRatio = .01f * calcOpts.BSRatio;
            calculatedStats.BurstPoints *= (1f-bsRatio) * 2;
            calculatedStats.SustainedPoints *= bsRatio * 2;

            calculatedStats.OverallPoints = calculatedStats.BurstPoints + calculatedStats.SustainedPoints;

            calcOpts.calculatedStats = calculatedStats;
            return calculatedStats;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            return GetCharacterStats(character, additionalItem, new Stats());
        }

        public Stats GetCharacterStats(Character character, Item additionalItem, Stats statsProcs)
        {
            CalculationOptionsTree calcOpts = character.CalculationOptions as CalculationOptionsTree;

            Stats statsRace = GetRacialBaseStats(character.Race);

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
            statsTotal.Spirit += statsTotal.SpiritFor20SecOnUse2Min / 6f;
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + character.DruidTalents.LivingSpirit * 0.05f));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + 0.01f * character.DruidTalents.ImprovedMarkOfTheWild));

            // Removed, since proc effects added by GetCharacterCalculations
//            statsTotal.ExtraSpiritWhileCasting = (float)Math.Floor((statsTotal.ExtraSpiritWhileCasting) * (1 + statsTotal.BonusSpiritMultiplier) * (1 + 0.01f * character.DruidTalents.ImprovedMarkOfTheWild) * (1 + character.DruidTalents.LivingSpirit * 0.05f));
            statsTotal.ExtraSpiritWhileCasting = 0f;

            if (statsTotal.GreatnessProc>0) {
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
            }

            statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower + statsTotal.Intellect * character.DruidTalents.LunarGuidance * 0.04); //LunarGuidance, 4% per Point
            statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower + (statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit) + (statsTotal.Intellect * character.DruidTalents.LunarGuidance * 0.04) + (character.DruidTalents.NurturingInstinct * 0.35f * statsTotal.Agility));

            statsTotal.Mana = statsTotal.Mana + ((statsTotal.Intellect - 20f) * 15f + 20f); //don't know why, but it's right..
            statsTotal.Mana *= (1f + statsTotal.BonusManaMultiplier);

            statsTotal.Health = (float)Math.Round(statsTotal.Health + (statsTotal.Stamina - 20f) * 10f + 20f);
            statsTotal.Mp5 += (float)Math.Floor(statsTotal.Intellect * (character.DruidTalents.Dreamstate > 0 ? character.DruidTalents.Dreamstate * 0.03f + 0.01f : 0f));

            statsTotal.SpellCrit = (float)Math.Round((statsTotal.Intellect * 0.006f) + (statsTotal.CritRating / 45.906f) + (statsTotal.SpellCrit*100.0f) + 1.85 + character.DruidTalents.NaturalPerfection, 2);
            statsTotal.SpellCombatManaRegeneration += 0.1f * character.DruidTalents.Intensity;

            // SpellPower (actually healing only, but we have no damaging spells, so np)
            statsTotal.SpellPower += ((statsTotal.Spirit + statsTotal.ExtraSpiritWhileCasting) * character.DruidTalents.ImprovedTreeOfLife * 0.05f);

            return statsTotal;
        }

        private static Stats GetRacialBaseStats(Character.CharacterRace race)
        {
            Stats statsRace = new Stats();
            statsRace.Mana = TreeConstants.BaseMana; 

            if (race == Character.CharacterRace.NightElf)
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
        }

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

                    string[] rotations = new string[] {
                        "Tank Nourish (plus RJ/RG/LB)",
                        "Tank Nourish (2 Tanks RJ/RG/LB)",
                        "Tank Healing Touch (plus RJ/RG/LB)",
                        "Tank Healing Touch (2 Tanks RJ/RG/LB)",
                        "Tank Regrowth (plus RJ/RG/LB)",
                        "STank Regrowth (2 Tanks RJ/RG/LB)",
                        "Raid healing with Regrowth (1 Tank RJ/LB)",
                        "Raid healing with Regrowth (2 Tanks RJ/LB)",
                        "Raid healing with Rejuvenation (1 Tank RJ/LB)",
                        "Raid healing with Rejuvenation (2 Tanks RJ/LB)",
                        "Raid healing with Nourish (1 Tank RJ/LB)",
                        "Raid healing with Nourish (2 Tanks RJ/LB)",
                        "Nourish spam",
                        "Healing Touch spam",
                        "Regrowth spam",
                        "Tank Regrowth (2 Tanks RJ/RG/1xLB Blooms)",
                        "Tank Regrowth (2 Tanks RJ/RG/2xLB Blooms)",
                        "Tank Regrowth (2 Tanks RJ/RG/3xLB Blooms)",
//                        "Tank Regrowth (2 Tanks RJ/RG/3xLB Blooms/SM)",
                    };

                    for (int i = 0; i < rotations.Length; i++)
                    {
                        comparisonsDPS.Add(getRotationData(character, i, rotations[i]));
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
                #endregion
                #region Trinkets
                BonusManaPotion = stats.BonusManaPotion,
                GreatnessProc = stats.GreatnessProc,
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
                NourishSpellpower = stats.NourishSpellpower,
                SpellsManaReduction = stats.SpellsManaReduction,
                ManacostReduceWithin15OnHealingCast = stats.ManacostReduceWithin15OnHealingCast,
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
                   + stats.Mp5 + stats.Healed + stats.HighestStat + stats.BonusHealingReceived) > 0;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            foreach (Rawr.SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use ||
                    effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.SpellHit
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
                + stats.GreatnessProc + stats.TreeOfLifeAura + stats.ReduceRegrowthCost
                + stats.ReduceRejuvenationCost + stats.RejuvenationSpellpower + stats.RejuvenationHealBonus 
                + stats.LifebloomTickHealBonus + stats.LifebloomFinalHealBonus + stats.ReduceHealingTouchCost
                + stats.HealingTouchFinalHealBonus + stats.LifebloomCostReduction + stats.NourishBonusPerHoT +
                stats.RejuvenationInstantTick + stats.NourishSpellpower + stats.SpellsManaReduction + 
                stats.ManacostReduceWithin15OnHealingCast
                > 0)
                return true;

            /*
            if (stats.Strength + stats.Agility + stats.AttackPower > 0)
                return false;
            */

            if (stats.SpellCombatManaRegeneration == 0.3f)
                return false;

            return (stats.SpellCombatManaRegeneration + stats.Intellect > 0);
        }

        /* Wildebees 20090407 : Overload base function to disable all enchants on OffHand for tree druids */
        public override bool EnchantFitsInSlot(Enchant enchant, Character character, Item.ItemSlot slot)
        {
            if (slot == Item.ItemSlot.OffHand) 
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

        public float CalculateManaRegen(float intel, float spi)
        {
            float baseRegen = 0.005575f;  // TODO: Decide if we should put 0.6f change in 3.1 in here, or is this function for out of combat regen
            return (float)Math.Round(5f * (0.001f + (float)Math.Sqrt(intel) * spi * baseRegen));
        }
    }

    public static class TreeConstants
    {
        // Source: http://www.wowwiki.com/Base_mana
        public static float BaseMana = 3496f;
        public static float HasteRatingToHaste = 3279f;
    }
}
