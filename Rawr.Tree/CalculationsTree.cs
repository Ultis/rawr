using System;
using System.Collections.Generic;


namespace Rawr.Tree
{
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
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Perfect (Revitalizing)", false, runed[1], purified[1], luminous[1], seers[1], brilliant[1], revitalizing);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Perfect (Ember)", false, runed[1], purified[1], luminous[1], seers[1], brilliant[1], ember);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Rare (Revitalizing)", true, runed[2], purified[2], luminous[2], seers[2], brilliant[2], revitalizing);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Rare (Ember)", true, runed[2], purified[2], luminous[2], seers[2], brilliant[2], ember);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Epic (Revitalizing)", false, runed[3], purified[3], luminous[3], seers[3], brilliant[3], revitalizing);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Epic (Ember)", false, runed[3], purified[3], luminous[3], seers[3], brilliant[3], ember);
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
            list.Add(new GemmingTemplate() { Model = "Tree", Group = name, RedId = runed, YellowId = seers, BlueId = seers, PrismaticId = seers, MetaId = meta, Enabled = enabled });

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
                    _subPointNameColors.Add("Survival", System.Drawing.Color.Green);
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

                        "Points:HealBurst",
                        "Points:HealSustained",
                        "Points:Survival",
                        "Points:Overall",

                        "Simulation:Time until OOM",
                        "Simulation:Total healing done",
                        "Simulation:HPS for primary heal",
                        "Simulation:HPS for tank HoTs",
                        "Simulation:MPS for primary heal",
                        "Simulation:MPS for tank HoTs",
                        "Simulation:MPS for Wild Growth",
                        "Simulation:HoT refresh fraction",
                        "Simulation:Mana regen per second",
                        "Simulation:Casts per minute until OOM",
                        "Simulation:Time to regen full mana",
                        "Simulation:Cast% after OOM",

                        "Lifebloom:LB Tick",
                        "Lifebloom:LB Heal",
                        "Lifebloom:LB HPM",

                        "Lifebloom Stack:LBS Tick",
                        "Lifebloom Stack:LBS HPM",

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
                        "Nourish:N (HoT) Heal",
                        "Nourish:N (HoT) HPM",
                        "Nourish:N (HoT) HPS",
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

        private static float DoTrinketManaRestoreCalcs(CharacterCalculationsTree calcs, float castsPerMinute)
        {
            float mp5FromTrinket = 0.0f;
            // Memento of Tyrande
            if (calcs.BasicStats.MementoProc > 0)
            {
                calcs.BasicStats.ManaRestoreOnCast_10_45 += 76 * 3;
            }
            // Spark of Life, Je'Tze's Bell, Memento of Tyrande
            if (calcs.BasicStats.ManaRestoreOnCast_10_45 > 0)
            {
                // MP5 for 15 seconds (value = mana gained on entire duration)
                // 10% chance, 45 sec icd
                float cd = 45f + (60f / castsPerMinute) / .1f;
                mp5FromTrinket += calcs.BasicStats.ManaRestoreOnCast_10_45 / cd;
            }
            // Insightful Earthstorm Diamond
            if (calcs.BasicStats.ManaRestoreOnCast_5_15 > 0)
            {
                // 5% chance, 15 sec icd
                float cd = 15f + (60f / castsPerMinute) / .05f;
                mp5FromTrinket += calcs.BasicStats.ManaRestoreOnCast_5_15 / cd;
            }
            if (calcs.BasicStats.FullManaRegenFor15SecOnSpellcast > 0)
            {
                // Blue Dragon. 2% chance to proc on cast, no known internal cooldown. calculate as the chance to have procced during its duration. 2% proc/cast.
                float avgcastlen = 60f / castsPerMinute;
                float tmpregen = calcs.ManaRegOutFSR * (1f - calcs.BasicStats.SpellCombatManaRegeneration) * (1f - (float)Math.Pow(1f - 0.02f, 15f / avgcastlen));
                if (tmpregen > 0f)
                {
                    mp5FromTrinket += tmpregen;
                }
            }
            return mp5FromTrinket;
        }
        
        protected float[] SimulateHealing(CharacterCalculationsTree calculatedStats, int wgPerMin, bool rejuvOnTank, bool rgOnTank, bool lbOnTank, int nTanks, Spell primaryHeal)
        {
            #region Spells
            int hots = 0;
            if (rejuvOnTank) hots++;
            if (rgOnTank) hots++;
            if (lbOnTank) hots++;
            Spell regrowth = new Regrowth(calculatedStats, false);
            Spell regrowthWhileActive = new Regrowth(calculatedStats, true);
            Spell lifebloom = new Lifebloom(calculatedStats);
            Spell stack = new LifebloomStack(calculatedStats);
            Spell rejuvenate = new Rejuvenation(calculatedStats);
            Spell nourish = new Nourish(calculatedStats);
            Spell nourishWithHoT = new Nourish(calculatedStats, hots>0?hots:1);
            Spell healingTouch = new HealingTouch(calculatedStats);
            Spell wildGrowth = new WildGrowth(calculatedStats);
            #endregion

            float castsPerMinute = 0;

            #region WildGrowthPerMinute
            float wgCastTime = wildGrowth.CastTime / 60f * wgPerMin;
            float wgMPS = wildGrowth.manaCost / 60f * wgPerMin;
            castsPerMinute += wgPerMin;
            #endregion

            #region HotsOnTanks
            float hotsHPS = 0;
            float hotsCastTime = 0;
            float hotsMPS = 0;
            float hotsCastsPerMinute = 0;
            if (rejuvOnTank)
            {
                hotsHPS += rejuvenate.HPSHoT;
                hotsMPS += rejuvenate.manaCost / rejuvenate.Duration;
                hotsCastTime += rejuvenate.CastTime / rejuvenate.Duration;
                hotsCastsPerMinute += 60f / rejuvenate.Duration;
            }
            if (rgOnTank)
            {
                hotsHPS += regrowth.HPSHoT + regrowth.HPS / regrowth.Duration;
                hotsMPS += regrowth.manaCost / regrowth.Duration;
                hotsCastTime += regrowth.CastTime / regrowth.Duration;
                hotsCastsPerMinute += 60f / regrowth.Duration;
            }
            if (lbOnTank)
            {
                hotsHPS += stack.HPSHoT;
                hotsMPS += stack.manaCost / stack.Duration;
                hotsCastTime += stack.CastTime / stack.Duration;
                hotsCastsPerMinute += 60f / stack.Duration;
            }
            hotsHPS *= nTanks;
            hotsMPS *= nTanks;
            hotsCastTime *= nTanks;
            hotsCastsPerMinute *= nTanks;
            castsPerMinute += hotsCastsPerMinute;
            #endregion

            #region Primary Heal
            float tpsHealing = 1f - (hotsCastTime + wgCastTime);
            float hpsHealing = 0;
            if (primaryHeal is Lifebloom || primaryHeal is Rejuvenation)
            {
                hpsHealing = tpsHealing * primaryHeal.HPSHoT;
            } 
            else 
            {
                hpsHealing = tpsHealing * primaryHeal.HPS;
            }
            float mpsHealing = tpsHealing * primaryHeal.manaCost / primaryHeal.CastTime;
            castsPerMinute += 60f * tpsHealing / primaryHeal.CastTime;
            #endregion

            float[] result = new float[] {
                hotsHPS + hpsHealing,
                hotsMPS + mpsHealing + wgMPS,
                hpsHealing,
                hotsHPS,
                mpsHealing,
                hotsMPS,
                hotsCastTime,
                castsPerMinute,
                wgMPS
            };

            return result;
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            CalculationOptionsTree calcOpts = (CalculationOptionsTree)character.CalculationOptions;
            CharacterCalculationsTree calculatedStats = new CharacterCalculationsTree();
            calculatedStats.LocalCharacter = character;

            calculatedStats.BasicStats = GetCharacterStats(character, additionalItem);

            #region Various procs and effects
            if (calculatedStats.BasicStats.ShatteredSunRestoProc > 0 && calcOpts.ShattrathFaction == "Aldor")
            {
                calculatedStats.BasicStats.AverageHeal += 44; // 1 proc/50 sec
            }
            if (calculatedStats.BasicStats.TrollDivinity > 0)
            {
                // 58 +healing, stacks 5 times, lasts 10 seconds, 20 seconds, with a 2 minute cooldown
                // Direct heals: Nourish (1.5) HT (3) Regrowth (2)
                // Lets assume Nourish, a 5 time stack takes 8-10 seconds. Means there are 20 seconds left
                // with 58*5 and 8 seconds with 58*4, 58*3, 58*2 and 58*1
                // (2*(1+2+3+4)+20*5)*58 / 120
                // = 120 * 58 / 120 = 58
                // But remember that the spellpower will increase for others in the raid too!
                calculatedStats.BasicStats.AverageHeal += 58;
            }
            calculatedStats.BasicStats.AverageHeal += calculatedStats.BasicStats.SpellPowerFor15SecOnUse90Sec / 8;
            calculatedStats.BasicStats.AverageHeal += calculatedStats.BasicStats.SpellPowerFor20SecOnUse2Min / 6;
            // For 10% chance, 10 second 45 icd trinkets, the uptime is 10 seconds per 60-65 seconds, .17 to be optimistic
            calculatedStats.BasicStats.SpellHaste += calculatedStats.BasicStats.SpellHasteFor10SecOnCast_10_45 * .17f / TreeConstants.HasteRatingToHaste;
            calculatedStats.BasicStats.SpellHaste += calculatedStats.BasicStats.SpellHasteFor10SecOnHeal_10_45 * .17f / TreeConstants.HasteRatingToHaste;
            calculatedStats.BasicStats.AverageHeal += calculatedStats.BasicStats.SpellPowerFor10SecOnHeal_10_45 * .17f;
            calculatedStats.BasicStats.AverageHeal += calculatedStats.BasicStats.SpellPowerFor10SecOnCast_10_45 * .17f;
            // For 15% chance, 10 second 45 icd trinkets, the uptime is 10 seconds per 57-60 seconds, .18 to be optimistic
            calculatedStats.BasicStats.AverageHeal += calculatedStats.BasicStats.SpellPowerFor10SecOnCast_15_45 * .18f;
            #endregion

            float MPS = 0; // mana per second used
            float HPS = 0; // healing per second of rotation

            #region Rotations
            switch (calcOpts.Rotation)
            {
                case 0:
                default:
                    // 1 Tank (RJ/RG/LB/N*)
                    {
                        calculatedStats.Simulation = SimulateHealing(
                            calculatedStats, calcOpts.WildGrowthPerMinute, 
                            true, true, true, 1,
                            new Nourish(calculatedStats, 3));
                        HPS = calculatedStats.Simulation[0];
                        MPS = calculatedStats.Simulation[1];
                        
                    }
                    break;
                case 1:
                    // 2 Tanks (RJ/RG/LB/N*)
                    {
                        calculatedStats.Simulation = SimulateHealing(
                            calculatedStats, calcOpts.WildGrowthPerMinute,
                            true, true, true, 2,
                            new Nourish(calculatedStats, 3));
                        HPS = calculatedStats.Simulation[0];
                        MPS = calculatedStats.Simulation[1];
                    }
                    break;
                case 2:
                    // 1 Tank (RJ/RG/LB/HT*)
                    {
                        calculatedStats.Simulation = SimulateHealing(
                            calculatedStats, calcOpts.WildGrowthPerMinute,
                            true, true, true, 1,
                            new HealingTouch(calculatedStats));
                        HPS = calculatedStats.Simulation[0];
                        MPS = calculatedStats.Simulation[1];
                    }
                    break;
                case 3:
                    // 2 Tanks (RJ/RG/LB/HT*)
                    {
                        calculatedStats.Simulation = SimulateHealing(
                            calculatedStats, calcOpts.WildGrowthPerMinute,
                            true, true, true, 2,
                            new HealingTouch(calculatedStats));
                        HPS = calculatedStats.Simulation[0];
                        MPS = calculatedStats.Simulation[1];
                    }
                    break;
                case 4:
                    // 1 Tank (RJ/RG/LB/RG*)
                    {
                        calculatedStats.Simulation = SimulateHealing(
                            calculatedStats, calcOpts.WildGrowthPerMinute,
                            true, true, true, 1,
                            new Regrowth(calculatedStats, true));
                        HPS = calculatedStats.Simulation[0];
                        MPS = calculatedStats.Simulation[1];
                    }
                    break;
                case 5:
                    // 2 Tanks (RJ/RG/LB/RG*)
                    {
                        calculatedStats.Simulation = SimulateHealing(
                            calculatedStats, calcOpts.WildGrowthPerMinute,
                            true, true, true, 2,
                            new Regrowth(calculatedStats, true));
                        HPS = calculatedStats.Simulation[0];
                        MPS = calculatedStats.Simulation[1];
                    }
                    break;
                case 6:
                    // RG Raid (1 Tank RJ/LB)
                    {
                        // TODO: also include HoT from RG on raid members
                        calculatedStats.Simulation = SimulateHealing(
                            calculatedStats, calcOpts.WildGrowthPerMinute,
                            true, false, true, 1,
                            new Regrowth(calculatedStats, false));
                        HPS = calculatedStats.Simulation[0];
                        MPS = calculatedStats.Simulation[1];
                    }
                    break;
                case 7:
                    // RG Raid (2 Tanks RJ/LB)
                    {
                        // TODO: also include HoT from RG on raid members
                        calculatedStats.Simulation = SimulateHealing(
                            calculatedStats, calcOpts.WildGrowthPerMinute,
                            true, false, true, 2,
                            new Regrowth(calculatedStats, false));
                        HPS = calculatedStats.Simulation[0];
                        MPS = calculatedStats.Simulation[1];
                    }
                    break;
                case 8:
                    // RJ Raid (1 Tank RJ/LB)
                    {
                        calculatedStats.Simulation = SimulateHealing(
                            calculatedStats, calcOpts.WildGrowthPerMinute,
                            true, false, true, 1,
                            new Rejuvenation(calculatedStats));
                        HPS = calculatedStats.Simulation[0];
                        MPS = calculatedStats.Simulation[1];
                    }
                    break;
                case 9:
                    // RJ Raid (2 Tanks RJ/LB)
                    {
                        calculatedStats.Simulation = SimulateHealing(
                            calculatedStats, calcOpts.WildGrowthPerMinute,
                            true, false, true, 2,
                            new Rejuvenation(calculatedStats));
                        HPS = calculatedStats.Simulation[0];
                        MPS = calculatedStats.Simulation[1];
                    }
                    break;
                case 10:
                    // N Raid (1 Tank RJ/LB)
                    {
                        calculatedStats.Simulation = SimulateHealing(
                            calculatedStats, calcOpts.WildGrowthPerMinute,
                            true, false, true, 1,
                            new Nourish(calculatedStats, 0));
                        HPS = calculatedStats.Simulation[0];
                        MPS = calculatedStats.Simulation[1];
                    }
                    break;
                case 11:
                    // N Raid (2 Tanks RJ/LB)
                    {
                        calculatedStats.Simulation = SimulateHealing(
                            calculatedStats, calcOpts.WildGrowthPerMinute,
                            true, false, true, 2,
                            new Nourish(calculatedStats, 0));
                        HPS = calculatedStats.Simulation[0];
                        MPS = calculatedStats.Simulation[1];
                    }
                    break;
                case 12:
                    // N spam
                    {
                        calculatedStats.Simulation = SimulateHealing(
                            calculatedStats, calcOpts.WildGrowthPerMinute,
                            false, false, false, 0,
                            new Nourish(calculatedStats));
                        HPS = calculatedStats.Simulation[0];
                        MPS = calculatedStats.Simulation[1];
                    }
                    break;
                case 13:
                    // HT spam
                    {
                        calculatedStats.Simulation = SimulateHealing(
                            calculatedStats, calcOpts.WildGrowthPerMinute,
                            false, false, false, 0,
                            new HealingTouch(calculatedStats));
                        HPS = calculatedStats.Simulation[0];
                        MPS = calculatedStats.Simulation[1];
                    }
                    break;
                case 14:
                    // RG spam
                    {
                        calculatedStats.Simulation = SimulateHealing(
                            calculatedStats, calcOpts.WildGrowthPerMinute,
                            false, false, false, 0,
                            new Regrowth(calculatedStats, true));
                        HPS = calculatedStats.Simulation[0];
                        MPS = calculatedStats.Simulation[1];
                    }
                    break;
            }
            #endregion

            #region Calculate regen
            float trinketRegen = DoTrinketManaRestoreCalcs(calculatedStats, calculatedStats.Simulation[7]);
            float spiritRegen = CalculateManaRegen(calculatedStats.BasicStats.Intellect, calculatedStats.BasicStats.Spirit);
            float spiritRegenWhileCasting = CalculateManaRegen(calculatedStats.BasicStats.Intellect, calculatedStats.BasicStats.ExtraSpiritWhileCasting + calculatedStats.BasicStats.Spirit);
            calculatedStats.replenishRegen = calculatedStats.BasicStats.Mana * 0.0025f * 5 * (calcOpts.ReplenishmentUptime / 100f);
            //spirit regen + mp5 + replenishmp5
            calculatedStats.ManaRegInFSR = spiritRegenWhileCasting * calculatedStats.BasicStats.SpellCombatManaRegeneration + calculatedStats.BasicStats.Mp5 + calculatedStats.replenishRegen;
            calculatedStats.ManaRegOutFSR = spiritRegenWhileCasting + calculatedStats.BasicStats.Mp5 + calculatedStats.replenishRegen;
            float ratio_extraspi = 0.8f; // OK, lets assume a mana starved person keeps 80% of the extra spirit effect, because they will keep casting anyway
            float ManaRegOutFSRNoCasting = (1-ratio_extraspi)*spiritRegen + ratio_extraspi*spiritRegenWhileCasting + calculatedStats.BasicStats.Mp5 + calculatedStats.replenishRegen;
            float ratio = 1f / 100f * calcOpts.FSRRatio;
            float manaRegen = ratio * calculatedStats.ManaRegInFSR + (1 - ratio) * calculatedStats.ManaRegOutFSR + trinketRegen;
            calculatedStats.ManaRegen = manaRegen;
            #endregion

            #region Mana potion
            float extraMana = 0f;
            switch (calcOpts.ManaPot)
            {
                default:
                case 0:
                    break;
                case 1:
                    extraMana = 1800;
                    break;
                case 2:
                    extraMana = 2200;
                    break;
                case 3:
                    extraMana = 2400;
                    break;
                case 4:
                    extraMana = 4300;
                    break;
            }
            extraMana *= (calculatedStats.BasicStats.BonusManaPotion + 1f); 
            #endregion

            #region Calculate total healing in the fight
            calculatedStats.TimeUntilOOM = 0;
            if (manaRegen/5f >= MPS) calculatedStats.TimeUntilOOM = calcOpts.FightDuration;
            else calculatedStats.TimeUntilOOM = (extraMana+calculatedStats.BasicStats.Mana) / (MPS - manaRegen/5f);
            if (calculatedStats.TimeUntilOOM > calcOpts.FightDuration) 
                calculatedStats.TimeUntilOOM = calcOpts.FightDuration;
            calculatedStats.TotalHealing = calculatedStats.TimeUntilOOM * HPS;
            // Correct for mana returns
            calculatedStats.TimeToRegenFull = 5f * calculatedStats.BasicStats.Mana / ManaRegOutFSRNoCasting;
            if (calcOpts.FightDuration > calculatedStats.TimeUntilOOM)
            {
                float timeLeft = calcOpts.FightDuration - calculatedStats.TimeUntilOOM;
                float cycle = (calculatedStats.TimeToRegenFull + calculatedStats.TimeUntilOOM);
                calculatedStats.CvRFraction = calculatedStats.TimeUntilOOM / cycle;
                calculatedStats.TotalHealing *= 1 + timeLeft / cycle;
            }
            else
            {
                calculatedStats.CvRFraction = 0;
            }
            #endregion

            calculatedStats.BurstPoints = HPS;

            calculatedStats.SustainedPoints = calculatedStats.TotalHealing / calcOpts.FightDuration;

            //Survival Points
            int health = (int)calculatedStats.BasicStats.Health;
            int healthBelow = (int)(health < calcOpts.SurvTargetLife ? health : calcOpts.SurvTargetLife);
            int healthAbove = health - healthBelow;

            calculatedStats.SurvivalPoints =
                ((calcOpts.SurvScaleBelowTarget > 0) ? healthBelow / 10F * (calcOpts.SurvScaleBelowTarget / 100F) : 0) +
                (healthAbove / 100F);

            // ADJUST POINT VALUE (BURST SUSTAINED RATIO)
            float bsRatio = ((float)calcOpts.BSRatio) * 0.01f;
            calculatedStats.BurstPoints *= (1f-bsRatio) * 2;
            calculatedStats.SustainedPoints *= bsRatio * 2;

            calculatedStats.OverallPoints = calculatedStats.BurstPoints + calculatedStats.SustainedPoints + calculatedStats.SurvivalPoints;

            calcOpts.calculatedStats = calculatedStats;
            return calculatedStats;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsTree calcOpts = character.CalculationOptions as CalculationOptionsTree;

            Stats statsRace = GetRacialBaseStats(character.Race);

            Stats statsTalents = new Stats();

            statsTalents.BonusAgilityMultiplier = 0.01f * character.DruidTalents.SurvivalOfTheFittest * 2;
            statsTalents.BonusIntellectMultiplier = 0.01f * character.DruidTalents.SurvivalOfTheFittest * 2;
            statsTalents.BonusSpiritMultiplier = 0.01f * character.DruidTalents.SurvivalOfTheFittest * 2;
            statsTalents.BonusStaminaMultiplier = 0.01f * character.DruidTalents.SurvivalOfTheFittest * 2;
            statsTalents.BonusStrengthMultiplier = 0.01f * character.DruidTalents.SurvivalOfTheFittest * 2;

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            //Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTotal = statsBaseGear + statsBuffs + statsRace + statsTalents;

            statsTotal.Agility = (float)Math.Floor((statsTotal.Agility) * (1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Stamina = (float)Math.Floor((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor((statsTotal.Intellect) * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Intellect = (float)Math.Round((statsTotal.Intellect) * (1 + character.DruidTalents.HeartOfTheWild * 0.04f));
            statsTotal.Spirit += statsTotal.SpiritFor20SecOnUse2Min / 6f;
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + character.DruidTalents.LivingSpirit * 0.05f));
            statsTotal.ExtraSpiritWhileCasting = (float)Math.Floor((statsTotal.ExtraSpiritWhileCasting) * (1 + statsTotal.BonusSpiritMultiplier) * (1 + character.DruidTalents.LivingSpirit * 0.05f));
            if (statsTotal.GreatnessProc>0) {
                // Highest stat
                if (statsTotal.Spirit>statsTotal.Intellect) {
                    // spirit proc (Greatness)
                    float extraSpi = statsTotal.GreatnessProc * 15f / 50f;
                    extraSpi = (float)Math.Floor((extraSpi) * (1 + statsTotal.BonusSpiritMultiplier));
                    extraSpi = (float)Math.Floor((extraSpi) * (1 + character.DruidTalents.LivingSpirit * 0.05f));
                    statsTotal.Spirit += extraSpi;
                }
                else {
                    // int proc (Greatness)
                    float extraInt = statsTotal.GreatnessProc * 15f / 50f;
                    extraInt = (float)Math.Floor((extraInt) * (1 + statsTotal.BonusIntellectMultiplier));
                    extraInt = (float)Math.Round((extraInt) * (1 + character.DruidTalents.HeartOfTheWild * 0.04f));
                    statsTotal.Intellect += extraInt;
                }
            }

            statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower + statsTotal.Intellect * character.DruidTalents.LunarGuidance * 0.04); //LunarGuidance, 4% per Point
            statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower + (statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit) + (statsTotal.Intellect * character.DruidTalents.LunarGuidance * 0.04) + (character.DruidTalents.NurturingInstinct * 0.35f * statsTotal.Agility));

            statsTotal.Mana = statsTotal.Mana + ((statsTotal.Intellect - 20f) * 15f + 20f); //don't know why, but it's right..
            statsTotal.Mana *= (1f + statsTotal.BonusManaMultiplier);

            statsTotal.Health = (float)Math.Round(statsTotal.Health + (statsTotal.Stamina - 20f) * 10f + 20f);
            statsTotal.Mp5 += (float)Math.Floor(statsTotal.Intellect * (character.DruidTalents.Dreamstate > 0 ? character.DruidTalents.Dreamstate * 0.03f + 0.01f : 0f));

            statsTotal.SpellCrit = (float)Math.Round((statsTotal.Intellect * 0.006f) + (statsTotal.CritRating / 45.906f) + 1.85 + character.DruidTalents.NaturalPerfection, 2);
            statsTotal.SpellCombatManaRegeneration += 0.1f * character.DruidTalents.Intensity;

            // SpellPower (actually healing only, but we have no damaging spells, so np)
            statsTotal.SpellPower += ((statsTotal.Spirit + statsTotal.ExtraSpiritWhileCasting) * character.DruidTalents.ImprovedTreeOfLife * 0.05f);

            return statsTotal;
        }

        private static Stats GetRacialBaseStats(Character.CharacterRace race)
        {
            Stats statsRace = new Stats();
            statsRace.Mana = TreeConstants.BaseMana; //pulled in an extra class, because i've to know them for spells etc

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
                Character = character,
                Name = rotationName,
                OverallPoints = calcs.OverallPoints,
                BurstPoints = calcs.BurstPoints,
                SustainedPoints = calcs.SustainedPoints,
                SurvivalPoints = calcs.SurvivalPoints
            };
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            switch (chartName)
            {
                case "Spell rotations":
                    List<ComparisonCalculationBase> comparisonsDPS = new List<ComparisonCalculationBase>();

                    string[] rotations = new string[] {
                        "Single target Nourish (plus RJ/RG/LB)",
                        "Single target Nourish (2 Tanks RJ/RG/LB)",
                        "Single target Healing Touch (plus RJ/RG/LB)",
                        "Single target Healing Touch (2 Tanks RJ/RG/LB)",
                        "Single target Regrowth (plus RJ/RG/LB)",
                        "Single target Regrowth (2 Tanks RJ/RG/LB)",
                        "Raid healing with Regrowth (1 Tank RJ/LB)",
                        "Raid healing with Regrowth (2 Tanks RJ/LB)",
                        "Raid healing with Rejuvenation (1 Tank RJ/LB)",
                        "Raid healing with Rejuvenation (2 Tanks RJ/LB)",
                        "Raid healing with Nourish (1 Tank RJ/LB)",
                        "Raid healing with Nourish (2 Tanks RJ/LB)",
                        "Nourish spam",
                        "Healing Touch spam",
                        "Regrowth spam"
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
            return new Stats()
            {
                #region Base Stats
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Mp5 = stats.Mp5,
                SpellPower = stats.SpellPower,
                CritRating = stats.CritRating,
                HasteRating = stats.HasteRating,
                Health = stats.Health,
                Mana = stats.Mana,
                Spirit = stats.Spirit,
                SpellHaste = stats.SpellHaste,
                #endregion
                #region Alchemists Stone
                BonusManaPotion = stats.BonusManaPotion,
                #endregion
                #region Trinkets
                MementoProc = stats.MementoProc,
                AverageHeal = stats.AverageHeal,
                TrollDivinity = stats.TrollDivinity,
                ExtraSpiritWhileCasting = stats.ExtraSpiritWhileCasting,
                ManaRestoreOnCast_10_45 = stats.ManaRestoreOnCast_10_45,
                //ManaResto = stats.ManaRestorePerCast_5_15,
                BangleProc = stats.BangleProc,
                SpiritFor20SecOnUse2Min = stats.SpiritFor20SecOnUse2Min,
                ManacostReduceWithin15OnUse1Min = stats.ManacostReduceWithin15OnUse1Min,
                FullManaRegenFor15SecOnSpellcast = stats.FullManaRegenFor15SecOnSpellcast,
                SpellPowerFor15SecOnUse90Sec = stats.SpellPowerFor15SecOnUse90Sec,
                SpellPowerFor20SecOnUse2Min = stats.SpellPowerFor20SecOnUse2Min,
                SpellHasteFor10SecOnHeal_10_45 = stats.SpellHasteFor10SecOnHeal_10_45,
                SpellHasteFor10SecOnCast_10_45 = stats.SpellHasteFor10SecOnCast_10_45,
                SpellPowerFor10SecOnHeal_10_45 = stats.SpellPowerFor10SecOnHeal_10_45,
                SpellPowerFor10SecOnCast_15_45 = stats.SpellPowerFor10SecOnCast_15_45,
                SpellPowerFor10SecOnCast_10_45 = stats.SpellPowerFor10SecOnCast_10_45,
                BonusHoTOnDirectHeals = stats.BonusHoTOnDirectHeals,
                GreatnessProc = stats.GreatnessProc,
                #endregion
                #region Neck
                ShatteredSunRestoProc = stats.ShatteredSunRestoProc,
                #endregion
                #region Idols
                TreeOfLifeAura = stats.TreeOfLifeAura,
                ReduceRegrowthCost = stats.ReduceRegrowthCost,
                ReduceRejuvenationCost = stats.ReduceRejuvenationCost, //Idol of Budding Life (-36)
                RejuvenationHealBonus = stats.RejuvenationHealBonus,
                //RejuvenationPeriodicHealBonus = stats.RejuvenationPeriodicHealBonus, might end up as the same, didn't do any math yet
                LifebloomTickHealBonus = stats.LifebloomTickHealBonus,
                LifebloomFinalHealBonus = stats.LifebloomFinalHealBonus,
                ReduceHealingTouchCost = stats.ReduceHealingTouchCost, //Refered to Idol of Longevity, which grant 25mana on HT cast?
                HealingTouchFinalHealBonus = stats.HealingTouchFinalHealBonus, //HealingTouchHealBonus?
                #endregion
                #region Sets
                RegrowthExtraTicks = stats.RegrowthExtraTicks, //T5 (2) Bonus
                BonusHealingTouchMultiplier = stats.BonusHealingTouchMultiplier,  //T6 (4) Bonus
                LifebloomCostReduction = stats.LifebloomCostReduction,
                NourishBonusPerHoT = stats.NourishBonusPerHoT,
                //ReduceRejuvenationCost = stats.ReduceRejuvenationCost, //T7 (2) Set: the cost of your Rejuvenation is reduced by 5%.
                //LvL70: Rejuv cost: 376 | 18,8 (~19) mana less
                //LvL80: Rejuv cost: 683 | 34 (Idol of Budding Life reduces by 36 Mana)

                //T7 (4) Set: Your Nourish heals an additional 5% for each of your heal over time effects present on the target.
                //Probably not that important
                #endregion
                #region Gems
                //BonusManaGem = stats.BonusManaGem; //Insightful Eathrstorm/Earthsiege Diamond?
                BonusCritHealMultiplier = stats.BonusCritHealMultiplier,
                ManaRestoreOnCast_5_15 = stats.ManaRestoreOnCast_5_15,
                BonusManaMultiplier = stats.BonusManaMultiplier,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                #endregion
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            //remove DST ... has accourding to Item Editor fixed 81.25 Haste ..
            if (stats.HasteRating == 81.25)
                return false;

            if (stats.Spirit + stats.Mp5 + stats.SpellPower + stats.Mana + stats.CritRating
                + stats.CritChanceReduction + stats.HasteRating + stats.SpellHaste + stats.BonusSpellPowerMultiplier
                + stats.BonusSpiritMultiplier + stats.BonusIntellectMultiplier + stats.BonusStaminaMultiplier // Blessing of Kings
                + stats.BonusManaPotion + stats.TrollDivinity + stats.ExtraSpiritWhileCasting 
                + stats.MementoProc + stats.AverageHeal + /*stats.ManaRestorePerCast_5_15 +*/ stats.BangleProc 
                + stats.SpiritFor20SecOnUse2Min + stats.ManacostReduceWithin15OnUse1Min + stats.FullManaRegenFor15SecOnSpellcast 
                + stats.HealingDoneFor15SecOnUse2Min + stats.SpellPowerFor15SecOnUse90Sec + stats.SpellPowerFor20SecOnUse2Min
                + stats.SpellPowerFor10SecOnCast_10_45 + stats.GreatnessProc
                + stats.ShatteredSunRestoProc + stats.SpellHasteFor10SecOnHeal_10_45 + stats.SpellHasteFor10SecOnCast_10_45 
                + stats.SpellPowerFor10SecOnHeal_10_45 + stats.SpellPowerFor10SecOnCast_15_45 + stats.BonusHoTOnDirectHeals
                + stats.TreeOfLifeAura + stats.ReduceRegrowthCost + stats.ReduceRejuvenationCost + stats.RejuvenationHealBonus 
                + stats.LifebloomTickHealBonus + stats.LifebloomFinalHealBonus + stats.ReduceHealingTouchCost
                + stats.HealingTouchFinalHealBonus + stats.LifebloomCostReduction + stats.NourishBonusPerHoT
                + stats.BonusCritHealMultiplier + stats.ManaRestoreOnCast_5_15 + stats.BonusManaMultiplier
                > 0)
                return true;

            // This removes feral PvE items - they have Str, Sta and Int (but not Spirit, which means we still get buffs that raise all stats)
            // It does not remove S1 feral items sinc ethey have +healing
            if (stats.Strength + stats.Agility + stats.AttackPower > 0)
                return false;

            if (stats.SpellCombatManaRegeneration == 0.3f)
                return false;


            return (stats.SpellCombatManaRegeneration + stats.Stamina + stats.Intellect > 0);
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
            float baseRegen = 0.005575f; 
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
