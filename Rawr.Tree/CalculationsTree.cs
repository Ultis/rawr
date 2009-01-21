using System;
using System.Collections.Generic;


namespace Rawr.Tree
{
    [Rawr.Calculations.RawrModelInfo("Tree", "Ability_Druid_TreeofLife", Character.CharacterClass.Druid)]
    class CalculationsTree : CalculationsBase
    {

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

        public override string[] CustomChartNames
        {
            get { return new string[1]; }
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
            // Mp5 on cast for 10 sec, 45 sec cooldown
            if (calcs.BasicStats.ManaRestoreOnCast_10_45 > 0)
            {
                mp5FromTrinket += calcs.BasicStats.ManaRestoreOnCast_10_45 / 15f;
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
            if (calcs.BasicStats.MementoProc > 0)
            {
                mp5FromTrinket += 17; // 3 sec cast time, 15.5 mp5, 1.5 sec cast time 19 mp5
            }
            return mp5FromTrinket;
        }
        
        protected float[] SimulateHealing(CharacterCalculationsTree calculatedStats, int wgPerMin, bool rejuvOnTank, bool rgOnTank, bool lbOnTank, int nTanks, Spell primaryHeal)
        {
            #region Spells
            Spell regrowth = new Regrowth(calculatedStats, false);
            Spell regrowthWhileActive = new Regrowth(calculatedStats, true);
            Spell lifebloom = new Lifebloom(calculatedStats);
            Spell stack = new LifebloomStack(calculatedStats);
            Spell rejuvenate = new Rejuvenation(calculatedStats);
            Spell nourish = new Nourish(calculatedStats);
            Spell nourishWithHoT = new Nourish(calculatedStats, true);
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

            calculatedStats.BasicStats.SpellCrit = (float)Math.Round((calculatedStats.BasicStats.Intellect * 0.006f) + (calculatedStats.BasicStats.CritRating / 45.906f) + 1.85 + character.DruidTalents.NaturalPerfection, 2);

            calculatedStats.BasicStats.SpellCombatManaRegeneration += 0.1f * character.DruidTalents.Intensity;

            //Improved Tree of Live Aura increases your Healing Spellpower ... 
            //this is not implemented in Rawr, so I take the normal Spellpower since I wont calculate damagespells :>
            calculatedStats.BasicStats.SpellPower += ((calculatedStats.BasicStats.Spirit + calculatedStats.BasicStats.ExtraSpiritWhileCasting)* character.DruidTalents.ImprovedTreeOfLife * 0.05f);

            if (calculatedStats.BasicStats.ShatteredSunRestoProc > 0 && calcOpts.ShattrathFaction == "Aldor")
            {
                calculatedStats.BasicStats.AverageHeal += 44; // 1 proc/50 sec
            }

            #region TrinketEffects
            calculatedStats.BasicStats.AverageHeal += calculatedStats.BasicStats.SpellPowerFor15SecOnUse90Sec / 8;
            calculatedStats.BasicStats.AverageHeal += calculatedStats.BasicStats.SpellPowerFor20SecOnUse2Min / 6;
            // For 10% chance, 10 second 45 icd trinkets, the uptime is 10 seconds per 60-65 seconds, .17 to be optimistic
            calculatedStats.BasicStats.HasteRating += calculatedStats.BasicStats.SpellHasteFor10SecOnCast_10_45 * .17f;
            calculatedStats.BasicStats.HasteRating += calculatedStats.BasicStats.SpellHasteFor10SecOnHeal_10_45 * .17f;
            calculatedStats.BasicStats.AverageHeal += calculatedStats.BasicStats.SpellPowerFor10SecOnHeal_10_45 * .17f;
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
                            new Nourish(calculatedStats, true));
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
                            new Nourish(calculatedStats, true));
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
                            new Nourish(calculatedStats, false));
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
                            new Nourish(calculatedStats, false));
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

            float spiritRegen = CalculateManaRegen(calculatedStats.BasicStats.Intellect, calculatedStats.BasicStats.Spirit);
            float spiritRegenWhileCasting = CalculateManaRegen(calculatedStats.BasicStats.Intellect, calculatedStats.BasicStats.ExtraSpiritWhileCasting + calculatedStats.BasicStats.Spirit);
            calculatedStats.replenishRegen = calculatedStats.BasicStats.Mana * 0.0025f * 5 * (calcOpts.ReplenishmentUptime / 100f);
            //spirit regen + mp5 + replenishmp5
            calculatedStats.ManaRegInFSR = spiritRegenWhileCasting * calculatedStats.BasicStats.SpellCombatManaRegeneration + calculatedStats.BasicStats.Mp5 + calculatedStats.replenishRegen;
            calculatedStats.ManaRegOutFSR = spiritRegenWhileCasting + calculatedStats.BasicStats.Mp5 + calculatedStats.replenishRegen;
            float ratio_extraspi = 0.8f; // OK, lets assume a mana starved person keeps 80% of the extra spirit effect, because they will keep casting anyway
            float ManaRegOutFSRNoCasting = (1-ratio_extraspi)*spiritRegen + ratio_extraspi*spiritRegenWhileCasting + calculatedStats.BasicStats.Mp5 + calculatedStats.replenishRegen;
            float ratio = 1f / 100f * calcOpts.FSRRatio;

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
            extraMana *= (calculatedStats.BasicStats.BonusManaPotion + 1f); float trinketRegen = DoTrinketManaRestoreCalcs(calculatedStats, calculatedStats.Simulation[7]);
            float manaRegen = ratio * calculatedStats.ManaRegInFSR + (1 - ratio) * calculatedStats.ManaRegOutFSR + trinketRegen;

            calculatedStats.BurstPoints = HPS;

            //Survival Points
            int health = (int)calculatedStats.BasicStats.Health;
            int healthBelow = (int)(health < calcOpts.SurvTargetLife ? health : calcOpts.SurvTargetLife);
            int healthAbove = health - healthBelow;

            calculatedStats.TimeUntilOOM = 0;
            if (manaRegen/5f >= MPS) calculatedStats.TimeUntilOOM = calcOpts.FightDuration;
            else calculatedStats.TimeUntilOOM = (extraMana+calculatedStats.BasicStats.Mana) / (MPS - manaRegen/5f);
            if (calculatedStats.TimeUntilOOM > calcOpts.FightDuration) 
                calculatedStats.TimeUntilOOM = calcOpts.FightDuration;
            calculatedStats.TotalHealing = calculatedStats.TimeUntilOOM * calculatedStats.BurstPoints;
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

            calculatedStats.ManaRegen = manaRegen;

            calculatedStats.SustainedPoints = calculatedStats.TotalHealing / calcOpts.FightDuration;

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
            //TODO: Check it out again ... later as it should do it now
            CalculationOptionsTree calcOpts = character.CalculationOptions as CalculationOptionsTree;

            Stats statsRace = GetRacialBaseStats(character.Race);

            Stats statsTalents = new Stats();

            statsTalents.BonusAgilityMultiplier = 0.01f * character.DruidTalents.SurvivalOfTheFittest * 2; //SurvivalOfTheFittest increases Stats 2% per point
            statsTalents.BonusIntellectMultiplier = 0.01f * character.DruidTalents.SurvivalOfTheFittest * 2;
            statsTalents.BonusSpiritMultiplier = 0.01f * character.DruidTalents.SurvivalOfTheFittest * 2;
            statsTalents.BonusStaminaMultiplier = 0.01f * character.DruidTalents.SurvivalOfTheFittest * 2;
            statsTalents.BonusStrengthMultiplier = 0.01f * character.DruidTalents.SurvivalOfTheFittest * 2;

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTotal = statsBaseGear + statsEnchants + statsBuffs + statsRace + statsTalents;

            statsTotal.Agility = (float)Math.Floor((statsTotal.Agility) * (1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Stamina = (float)Math.Floor((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor((statsTotal.Intellect) * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Intellect = (float)Math.Round((statsTotal.Intellect) * (1 + character.DruidTalents.HeartOfTheWild * 0.04f));
            statsTotal.Spirit += statsTotal.SpiritFor20SecOnUse2Min / 6f;
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + character.DruidTalents.LivingSpirit * 0.05f));
            statsTotal.ExtraSpiritWhileCasting = (float)Math.Floor((statsTotal.ExtraSpiritWhileCasting) * (1 + statsTotal.BonusSpiritMultiplier) * (1 + character.DruidTalents.LivingSpirit * 0.05f));

            statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower + statsTotal.Intellect * character.DruidTalents.LunarGuidance * 0.04); //LunarGuidance, 4% per Point
            statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower + (statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit) + (statsTotal.Intellect * character.DruidTalents.LunarGuidance * 0.04) + (character.DruidTalents.NurturingInstinct * 0.35f * statsTotal.Agility));

            statsTotal.Mana = statsTotal.Mana + ((statsTotal.Intellect - 20f) * 15f + 20f); //don't know why, but it's right..
            statsTotal.Mana *= (1f + statsTotal.BonusManaMultiplier);

            statsTotal.Health = (float)Math.Round(statsTotal.Health + (statsTotal.Stamina - 20f) * 10f + 20f);
            statsTotal.Mp5 += (float)Math.Floor(statsTotal.Intellect * (character.DruidTalents.Dreamstate > 0 ? character.DruidTalents.Dreamstate * 0.03f + 0.01f : 0f));

            if (statsTotal.TrollDivinity > 0)
            {
                // 58 +healing, stacks 5 times, lasts 10 seconds, 20 seconds, with a 2 minute cooldown
                // Direct heals: Nourish (1.5) HT (3) Regrowth (2)
                // Lets assume Nourish, a 5 time stack takes 8-10 seconds. Means there are 20 seconds left
                // with 58*5 and 8 seconds with 58*4, 58*3, 58*2 and 58*1
                // (2*(1+2+3+4)+20*5)*58 / 120
                // = 120 * 58 / 120 = 58
                // But remember that the spellpower will increase for others in the raid too!
                statsTotal.AverageHeal += 58;
            }

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

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            return null;
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
                BonusHoTOnDirectHeals = stats.BonusHoTOnDirectHeals,
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
                //ReduceRejuvenationCost = stats.ReduceRejuvenationCost, //T7 (2) Set: the cost of your Rejuvenation is reduced by 5%.
                //LvL70: Rejuv cost: 376 | 18,8 (~19) mana less
                //LvL80: Rejuv cost: 683 | 34 (Idol of Budding Life reduces by 36 Mana)

                //T7 (4) Set: Your Nourish heals an additional 5% for each of your heal over time effects present on the target.
                //Probably not that important
                #endregion
                #region Gems
                //BonusManaGem = stats.BonusManaGem; //Insightful Eathrstorm/Earthsiege Diamond?
                #endregion
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            //remove DST ... has accourding to Item Editor fixed 81.25 Haste ..
            if (stats.HasteRating == 81.25)
                return false;

            if (stats.Intellect + stats.Spirit + stats.Mp5 + stats.SpellPower + stats.CritChanceReduction + stats.HasteRating + stats.Mana + stats.CritRating
                + stats.BonusSpiritMultiplier + stats.BonusIntellectMultiplier + stats.BonusStaminaMultiplier // Blessing of Kings
                + stats.BonusManaPotion + stats.TrollDivinity + stats.ExtraSpiritWhileCasting + stats.MementoProc + stats.AverageHeal + /*stats.ManaRestorePerCast_5_15 +*/ stats.BangleProc + stats.SpiritFor20SecOnUse2Min + stats.ManacostReduceWithin15OnUse1Min + stats.FullManaRegenFor15SecOnSpellcast + stats.HealingDoneFor15SecOnUse2Min + stats.SpellPowerFor15SecOnUse90Sec + stats.SpellPowerFor20SecOnUse2Min
                + stats.ShatteredSunRestoProc + stats.SpellHasteFor10SecOnHeal_10_45 + stats.SpellHasteFor10SecOnCast_10_45 + stats.SpellPowerFor10SecOnHeal_10_45 + stats.SpellPowerFor10SecOnCast_15_45 + stats.BonusHoTOnDirectHeals
                + stats.TreeOfLifeAura + stats.ReduceRegrowthCost + stats.ReduceRejuvenationCost + stats.RejuvenationHealBonus + stats.LifebloomTickHealBonus + stats.LifebloomFinalHealBonus + stats.ReduceHealingTouchCost + stats.HealingTouchFinalHealBonus
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
