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
                    _subPointNameColors.Add("HpS", System.Drawing.Color.Red);
                    _subPointNameColors.Add("Mp5", System.Drawing.Color.Blue);
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

                        "Points:HpS",
                        "Points:Mp5",
                        "Points:Survival",
                        "Points:Overall",

                        "Prim Rotation:Rota HPS",
                        "Prim Rotation:Rota Mana",
                        "Prim Rotation:Rota Time2OOM",

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
            calculatedStats.BasicStats.SpellPower += (calculatedStats.BasicStats.Spirit * character.DruidTalents.ImprovedTreeOfLife * 0.05f);

            if (calculatedStats.BasicStats.ShatteredSunRestoProc > 0 && calcOpts.ShattrathFaction == "Aldor")
            {
                calculatedStats.BasicStats.AverageHeal += 44; // 1 proc/50 sec
            }

            #region TrinketEffects
            calculatedStats.BasicStats.AverageHeal += calculatedStats.BasicStats.SpellPowerFor15SecOnUse90Sec / 8;
            calculatedStats.BasicStats.AverageHeal += calculatedStats.BasicStats.SpellPowerFor20SecOnUse2Min / 6;
            #endregion

            float spiritRegen = CalculateManaRegen(calculatedStats.BasicStats.Intellect, calculatedStats.BasicStats.Spirit);
            float replenishRegen = (calcOpts.haveReplenishSupport ? 1 : 0) * calculatedStats.BasicStats.Mana * 0.0025f * 5 * (calcOpts.averageReplenishActiveTime / 100f);
            //spirit regen + mp5 + replenishmp5
            calculatedStats.ManaRegInFSR = spiritRegen * calculatedStats.BasicStats.SpellCombatManaRegeneration + calculatedStats.BasicStats.Mp5 + replenishRegen;
            calculatedStats.ManaRegOutFSR = spiritRegen + calculatedStats.BasicStats.Mp5 + replenishRegen;

            //HPS Points
            if (calcOpts.Spellrotations.Count < 0)
                calculatedStats.HpSPoints = 0;
            else
            {
                calcOpts.Spellrotations[0].CalculateSpellRotaion(calculatedStats);
                calculatedStats.HpSPoints = calcOpts.Spellrotations[0].HPS;//calcOpts.spellRotationPlaceholder == "Healing Touch" ? (new HealingTouch(calculatedStats)).HPS : (new Regrowth(calculatedStats)).HPS;
            }

            //Survival Points
            int health = (int)calculatedStats.BasicStats.Health;
            int healthBelow = (int)(health < calcOpts.SurvTargetLife ? health : calcOpts.SurvTargetLife);
            int healthAbove = health - healthBelow;

            calculatedStats.SurvivalPoints =
                ((calcOpts.SurvScaleBelowTarget > 0) ? healthBelow / 10F * (calcOpts.SurvScaleBelowTarget / 100F) : 0) +
                (healthAbove / 100F);

            //Mp5 Points
            calculatedStats.AddMp5Points(calculatedStats.ManaRegInFSR * (calcOpts.mP5Scale / 100), "Regen");
            //calculatedStats.AddMp5Points(calculatedStats.ManaRegInFSR, "Regen");

            calculatedStats.OverallPoints = calculatedStats.HpSPoints + calculatedStats.Mp5Points + calculatedStats.SurvivalPoints;

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
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + character.DruidTalents.LivingSpirit * 0.05f));

            statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower + statsTotal.Intellect * character.DruidTalents.LunarGuidance * 0.04); //LunarGuidance, 4% per Point
            statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower + (statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit) + (statsTotal.Intellect * character.DruidTalents.LunarGuidance * 0.04) + (character.DruidTalents.NurturingInstinct * 0.35f * statsTotal.Agility));

            statsTotal.Mana = statsTotal.Mana + ((statsTotal.Intellect - 20f) * 15f + 20f); //don't know why, but it's right..

            statsTotal.Health = (float)Math.Round(statsTotal.Health + (statsTotal.Stamina * 10f));
            statsTotal.Mp5 += (float)Math.Floor(statsTotal.Intellect * (character.DruidTalents.Dreamstate > 0 ? character.DruidTalents.Dreamstate * 0.03f + 0.01f : 0f));

            return statsTotal;
        }

        private static Stats GetRacialBaseStats(Character.CharacterRace race)
        {
            Stats statsRace = new Stats();
            statsRace.Mana = TreeConstants.getBaseMana(race); //pulled in an extra class, because i've to know them for spells etc

            //if (level == 70)
            //{
            //    if (race == Character.CharacterRace.NightElf)
            //    {
            //        statsRace.Health = 3434f;
            //        statsRace.Stamina = 82f;
            //        statsRace.Agility = 75f;
            //        statsRace.Intellect = 120f;
            //        statsRace.Spirit = 133f;
            //    }
            //    else
            //    {
            //        statsRace.Health = 3434f; //Tauren racial is now a fixed ammount .. have to figure it out.. rumors say something around 600 Health
            //        statsRace.Stamina = 85f;
            //        statsRace.Agility = 64.5f;
            //        statsRace.Intellect = 115f;
            //        statsRace.Spirit = 135f;
            //    }
            //}
            //else if (level == 80)

            //taken from http://forums.worldofwarcraft.com/thread.html?topicId=9336866956&sid=2000
            if (race == Character.CharacterRace.NightElf)
            {
                statsRace.Health = 5493f;
                //statsRace.Strength = 85f;
                statsRace.Agility = 87f;
                statsRace.Stamina = 97f;
                statsRace.Intellect = 143f;
                statsRace.Spirit = 159f;
            }
            else
            {
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
                //ManaResto = stats.ManaRestorePerCast_5_15,
                BangleProc = stats.BangleProc,
                SpiritFor20SecOnUse2Min = stats.SpiritFor20SecOnUse2Min,
                ManacostReduceWithin15OnUse1Min = stats.ManacostReduceWithin15OnUse1Min,
                FullManaRegenFor15SecOnSpellcast = stats.FullManaRegenFor15SecOnSpellcast,
                SpellPowerFor15SecOnUse90Sec = stats.SpellPowerFor15SecOnUse90Sec,
                SpellPowerFor20SecOnUse2Min = stats.SpellPowerFor20SecOnUse2Min,
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

            if (stats.Intellect + stats.Spirit + stats.Mp5 + stats.SpellPower + stats.CritChanceReduction + stats.HasteRating + stats.Mana
                + stats.BonusSpiritMultiplier + stats.BonusIntellectMultiplier + stats.BonusStaminaMultiplier // Blessing of Kings
                + stats.BonusManaPotion + stats.MementoProc + stats.AverageHeal + /*stats.ManaRestorePerCast_5_15 +*/ stats.BangleProc + stats.SpiritFor20SecOnUse2Min + stats.ManacostReduceWithin15OnUse1Min + stats.FullManaRegenFor15SecOnSpellcast + stats.HealingDoneFor15SecOnUse2Min + stats.SpellPowerFor15SecOnUse90Sec + stats.SpellPowerFor20SecOnUse2Min
                + stats.ShatteredSunRestoProc
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
            float baseRegen;
            // if (level == 70) 
            //    baseRegen = 0.009327f;
            //else if (level == 80)
                baseRegen = 0.005575f; //Values Taken from StatLogic-1.0 Lib (Whitetooth wrote them there, search for BaseManaRegenPerSpi)
            //else baseRegen = 0.00932715221261f;

            return (float)Math.Round(5f * (0.001f + (float)Math.Sqrt(intel) * spi * baseRegen));
        }
    }

    public static class TreeConstants
    {
        public static float getBaseMana(Character.CharacterRace race)
        {
            if (race == Character.CharacterRace.NightElf)
            {
                //if (level == 70)
                //    return 2470f;
                //else if (level == 80)
                    return 5361f;
                //else
                //    return 2470f;
            }
            else if (race == Character.CharacterRace.Tauren)
            {
                //if (level == 70)
                //    return 2370f;
                //else if (level == 80)
                return 5286f;
                //else
                //    return 2370f;
            }
            return 5361f;
        }

        public static float hasteconversation = 3279f;
    }
}
