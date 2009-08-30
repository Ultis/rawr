using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Rawr.Rogue.ClassAbilities;
using Rawr.Rogue.FinishingMoves;
using Rawr.Rogue.Poisons;
using Rawr.Rogue.SpecialAbilities;
#if RAWR3
    using System.Windows.Media;
#else
    using System.Drawing;
#endif

namespace Rawr.Rogue {
    [Calculations.RawrModelInfoAttribute("Rogue", "Ability_Rogue_SliceDice", CharacterClass.Rogue)]
    public class CalculationsRogue : CalculationsBase {
        public override List<GemmingTemplate> DefaultGemmingTemplates {
            get {
                ////Relevant Gem IDs for Rogues
                //Red
                int[] bold = { 39900, 39996, 40111, 42142 };
                int[] delicate = { 39905, 39997, 40112, 42143 };

                //Purple
                int[] shifting = { 39935, 40023, 40130 };
                int[] sovereign = { 39934, 40022, 40129 };

                //Blue
                int[] solid = { 39919, 40008, 40119, 36767 };

                //Green
                int[] enduring = { 39976, 40089, 40167 };

                //Yellow
                int[] thick = { 39916, 40015, 40126, 42157 };

                //Orange
                int[] etched = { 39948, 40038, 40143 };
                int[] fierce = { 39951, 40041, 40146 };
                int[] glinting = { 39953, 40044, 40148 };
                int[] stalwart = { 39964, 40056, 40160 };
                int[] deadly = { 39952, 40043, 40147 };

                //Meta
                // int austere = 41380;
                int relentless = 41398;

                return new List<GemmingTemplate> {
				    new GemmingTemplate { Model = "Rogue", Group = "Uncommon", //Max Agility
					    RedId = delicate[0], YellowId = delicate[0], BlueId = delicate[0], PrismaticId = delicate[0], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Uncommon", //Agi/Crit
					    RedId = delicate[0], YellowId = deadly[0], BlueId = shifting[0], PrismaticId = delicate[0], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Rare", Enabled = true, //Max Agility
					    RedId = delicate[1], YellowId = delicate[1], BlueId = delicate[1], PrismaticId = delicate[1], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Rare", Enabled = true, //Agi/Crit 
					    RedId = delicate[1], YellowId = deadly[1], BlueId = shifting[1], PrismaticId = delicate[1], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Epic", //Max Agility
					    RedId = delicate[2], YellowId = delicate[2], BlueId = delicate[2], PrismaticId = delicate[2], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Epic", //Agi/Crit 
					    RedId = delicate[2], YellowId = deadly[2], BlueId = shifting[2], PrismaticId = delicate[2], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Jeweler", //Max Agility
					    RedId = delicate[3], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[3], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Jeweler", //Agility Heavy
					    RedId = delicate[2], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[2], MetaId = relentless },
			    };
            }
        }

        #region Variables and Properties

        private CalculationOptionsPanelRogue _calculationOptionsPanel = null;
        #if RAWR3
        public override ICalculationOptionsPanel CalculationOptionsPanel
        #else
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        #endif
        {
            get {
                if (_calculationOptionsPanel == null) { _calculationOptionsPanel = new CalculationOptionsPanelRogue(); }
                return _calculationOptionsPanel;
            }
        }

        //private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels { get { return DisplayValue.GroupedList(); } }

        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels {
            get {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
                        "Health",
                        "Armor",
                        "Strength",
                        "Attack Power",
                        "Agility",
                        "Crit %",
                        "Haste %",
					    "Armor Penetration %",
                        "% Chance to Miss (White)",
                        "% Chance to Miss (Yellow)",
                        "% Chance to be Dodged",
                        "% Chance to be Parried",
                        "% Chance to be Avoided (Yellow/Dodge)",
					};
                return _optimizableCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames {
            get {
                if (_customChartNames == null) {
                    _customChartNames = new string[] {
                        "Combat Table",
                        /*""/*,
                        /*""/*,
                        /*""*/
                    };
                }
                return _customChartNames;
            }
        }

        private readonly Dictionary<string, Color> _subPointNameColors = new Dictionary<string, Color> { { "DPS", Color.FromArgb(255, 200, 0, 0) } };
        public override Dictionary<string, Color> SubPointNameColors { get { return _subPointNameColors; } }

        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes {
            get {
                if (_relevantItemTypes == null) {
                    _relevantItemTypes = new List<ItemType>(new[] {
                        ItemType.None,
                        ItemType.Leather,
                        ItemType.Bow,
                        ItemType.Crossbow,
                        ItemType.Gun,
                        ItemType.Thrown,
                        ItemType.Dagger,
                        ItemType.FistWeapon,
                        ItemType.OneHandMace,
                        ItemType.OneHandSword
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override CharacterClass TargetClass { get { return CharacterClass.Rogue; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationsRogue(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsRogue(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml) {
            XmlSerializer s = new XmlSerializer(typeof(CalculationOptionsRogue));
            StringReader sr = new StringReader(xml);
            CalculationOptionsRogue calcOpts = s.Deserialize(sr) as CalculationOptionsRogue;
            return calcOpts;
        }
        #endregion

        /// <summary>
        /// Calculate damage output
        /// </summary>
        /// <param name="character"></param>
        /// <param name="additionalItem"></param>
        /// <param name="referenceCalculation"></param>
        /// <param name="significantChange"></param>
        /// <param name="needsDisplayCalculations"></param>
        /// <returns></returns>
        /// Much of this code is based on Aldriana's RogueCalc
        /// 
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations) {
            TalentsAndGlyphs.Initialize(character.RogueTalents);
            CalculationOptionsRogue calcOpts = character.CalculationOptions as CalculationOptionsRogue;
            Talents.InitializeMurder(calcOpts);
            Stats stats = GetCharacterStats(character, additionalItem);
            CombatFactors combatFactors = new CombatFactors(character, stats);

            //------------------------------------------------------------------------------------
            // CALCULATE OUTPUTS
            //------------------------------------------------------------------------------------
            CharacterCalculationsRogue displayedValues = new CharacterCalculationsRogue(stats);

            WhiteAttacks whiteAttacks = new WhiteAttacks(combatFactors);
            CycleTime cycleTime = new CycleTime(calcOpts, combatFactors, whiteAttacks);

            //Now that we have the basics, reset for any SnD bonus
            stats.PhysicalHaste += SnD.CalcHasteBonus(calcOpts, cycleTime);
            whiteAttacks = new WhiteAttacks(combatFactors);
            cycleTime = new CycleTime(calcOpts, combatFactors, whiteAttacks);
            var sndUpTime = SnD.UpTime(calcOpts, cycleTime);
            var cpgDps = calcOpts.CpGenerator.CalcCpgDps(calcOpts, combatFactors, stats, cycleTime);

            var totalFinisherDps = 0f;
            
            foreach (var component in calcOpts.DpsCycle.Components) {
                var finisherDps = component.CalcFinisherDps(calcOpts, combatFactors, stats, whiteAttacks, cycleTime, displayedValues);
                displayedValues.AddToolTip(DisplayValue.FinisherDps, component + ": " + finisherDps);
                totalFinisherDps += finisherDps;
            }

            var swordSpecDps = new SwordSpec().CalcDps(calcOpts, combatFactors, whiteAttacks, cycleTime);
            var poisonDps = PoisonBase.CalcPoisonDps(calcOpts, combatFactors, stats, whiteAttacks, displayedValues, cycleTime);
            

            displayedValues.TotalDPS = whiteAttacks.CalcMhWhiteDps() + whiteAttacks.CalcOhWhiteDps() + swordSpecDps + cpgDps + totalFinisherDps + poisonDps;
            displayedValues.OverallPoints = displayedValues.TotalDPS;

            if (!needsDisplayCalculations) { return displayedValues; }

            //------------------------------------------------------------------------------------
            // ADD CALCULATED OUTPUTS TO DISPLAY
            //------------------------------------------------------------------------------------
            displayedValues.AddRoundedDisplayValue(DisplayValue.MhWeaponDamage, combatFactors.MhAvgDamage);
            displayedValues.AddRoundedDisplayValue(DisplayValue.OhWeaponDamage, combatFactors.OhAvgDamage);

            displayedValues.AddDisplayValue(DisplayValue.Cpg, calcOpts.CpGenerator.Name);
            displayedValues.AddRoundedDisplayValue(DisplayValue.CycleTime, cycleTime.Duration);

            displayedValues.AddDisplayValue(DisplayValue.EnergyRegen, combatFactors.BaseEnergyRegen.ToString());

            displayedValues.AddRoundedDisplayValue(DisplayValue.HitRating, stats.HitRating);
            displayedValues.AddToolTip(DisplayValue.HitRating, string.Format("Total % Hit: {0:0.00%}", combatFactors.HitPercent));
            displayedValues.AddToolTip(DisplayValue.HitRating, string.Format("Poison % Hit: {0:0.00%}", combatFactors.PoisonHitPercent));

            displayedValues.AddRoundedDisplayValue(DisplayValue.CritRating, stats.CritRating);
            displayedValues.AddToolTip(DisplayValue.CritRating, string.Format("Crit % from Rating: {0:00.00%}",combatFactors.CritFromCritRating));
            displayedValues.AddToolTip(DisplayValue.CritRating, string.Format("MH Crit: {0:00.00%}", combatFactors.ProbMhCrit));
            displayedValues.AddToolTip(DisplayValue.CritRating, string.Format("OH Crit: {0:00.00%}", combatFactors.ProbOhCrit));
            displayedValues.AddToolTip(DisplayValue.CritRating, "Crit Multiplier: " + combatFactors.BaseCritMultiplier);

            displayedValues.AddDisplayValue(DisplayValue.ArmorDamageReduction, Math.Round(combatFactors.ArmorDamageReduction, 2) * 100 + "%");
            displayedValues.AddToolTip(DisplayValue.ArmorDamageReduction, "Armor Penetration Rating: " + stats.ArmorPenetrationRating);

            displayedValues.AddRoundedDisplayValue(DisplayValue.BaseExpertise, combatFactors.BaseExpertise);
            displayedValues.AddToolTip(DisplayValue.BaseExpertise, "MH Expertise: " + combatFactors.MhExpertise);
            displayedValues.AddToolTip(DisplayValue.BaseExpertise, "OH Expertise: " + combatFactors.OhExpertise);
            
            displayedValues.AddRoundedDisplayValue(DisplayValue.Haste, (combatFactors.Haste <= 0 ? 0 : combatFactors.Haste - 1)*100);
            displayedValues.AddToolTip(DisplayValue.Haste, "Haste Rating: " + stats.HasteRating);
            
            displayedValues.AddRoundedDisplayValue(DisplayValue.SndUptime, sndUpTime*100f);

            displayedValues.AddRoundedDisplayValue(DisplayValue.CpgCrit, calcOpts.CpGenerator.Crit(combatFactors, calcOpts) * 100);
            displayedValues.AddToolTip(DisplayValue.CpgCrit, "Crit From Stats: " + stats.PhysicalCrit);
            displayedValues.AddToolTip(DisplayValue.CpgCrit, "Crit from Crit Rating: " + combatFactors.CritFromCritRating);
            displayedValues.AddPercentageToolTip(DisplayValue.CpgCrit, "Boss Crit Reduction: ", StatConversion.NPC_LEVEL_CRIT_MOD[calcOpts.TargetLevel-80]);

            displayedValues.AddRoundedDisplayValue(DisplayValue.WhiteDps, whiteAttacks.CalcMhWhiteDps() + whiteAttacks.CalcOhWhiteDps());
            displayedValues.AddToolTip(DisplayValue.WhiteDps, "MH White DPS: " + whiteAttacks.CalcMhWhiteDps());
            displayedValues.AddToolTip(DisplayValue.WhiteDps, "OH White DPS: " + whiteAttacks.CalcOhWhiteDps());

            displayedValues.AddRoundedDisplayValue(DisplayValue.CpgDps, cpgDps);
            displayedValues.AddRoundedDisplayValue(DisplayValue.FinisherDps, totalFinisherDps);
            displayedValues.AddRoundedDisplayValue(DisplayValue.SwordSpecDps, swordSpecDps);
            displayedValues.AddRoundedDisplayValue(DisplayValue.PoisonDps, poisonDps);

            displayedValues.AddRoundedDisplayValue(DisplayValue.Dodge, stats.Dodge);
            displayedValues.AddRoundedToolTip(DisplayValue.Dodge, "Dodge Rating: {0}", stats.DodgeRating);

            displayedValues.AddRoundedDisplayValue(DisplayValue.Parry, stats.Parry);
            displayedValues.AddRoundedToolTip(DisplayValue.Parry, "Parry Rating: {0}", stats.ParryRating);

            return displayedValues;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem) {
            CalculationOptionsRogue calcOpts = character.CalculationOptions as CalculationOptionsRogue;
            RogueTalents talents = character.RogueTalents;

            Stats statsRace = BaseStats.GetBaseStats(character.Level, CharacterClass.Rogue, character.Race);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsItems = GetItemStats(character, additionalItem);
            Stats statsOptionsPanel = new Stats() {
                // handle boss level difference
                PhysicalCrit = StatConversion.NPC_LEVEL_CRIT_MOD[calcOpts.TargetLevel - character.Level],
            };
            Stats statsTalents = new Stats() {
                BonusAgilityMultiplier = Talents.SinisterCalling.Agility.Multiplier,
                PhysicalHit = Talents.Precision.Bonus,
                BonusAttackPowerMultiplier = Talents.Deadliness.Multiplier
                                           + Talents.SavageCombat.AttackPower.Multiplier,
                PhysicalCrit = Talents.Malice.Bonus +
                                ((character.ActiveBuffs.FindAll(buff => buff.Group == "Critical Strike Chance Taken").Count == 0)
                                    ? Talents.MasterPoisoner.Crit.Bonus : 0f),
                Dodge = Talents.LightningReflexes.Dodge.Bonus,
                Parry = Talents.Deflection.Bonus,
                PhysicalHaste = (1f + Talents.BladeFlurry.Haste.Bonus)
                              * (1f + Talents.LightningReflexes.Haste.Bonus)
                              - 1f,
            };
            Stats statsGearEnchantsBuffs = statsItems + statsBuffs;
            Stats statsTotal = statsRace + statsItems + statsBuffs + statsTalents + statsOptionsPanel;
            Stats statsProcs = new Stats();

            // Stamina
            float totalBSTAM = statsTotal.BonusStaminaMultiplier;
            float staBase = (float)Math.Floor((1f + totalBSTAM) * statsRace.Stamina);
            float staBonus = (float)Math.Floor((1f + totalBSTAM) * statsGearEnchantsBuffs.Stamina);
            statsTotal.Stamina = staBase + staBonus;

            // Health
            statsTotal.Health += StatConversion.GetHealthFromStamina(statsTotal.Stamina);
            statsTotal.Health *= 1f + statsTotal.BonusHealthMultiplier;

            // Strength
            float totalBSM = statsTotal.BonusStrengthMultiplier;
            float strBase  = (float)Math.Floor((1f + totalBSM) * statsRace.Strength);
            float strBonus = (float)Math.Floor((1f + totalBSM) * statsGearEnchantsBuffs.Strength);
            statsTotal.Strength = strBase + strBonus;

            // Agility
            float totalBAM = statsTotal.BonusAgilityMultiplier;
            float agiBase  = (float)Math.Floor((1f + totalBAM) * statsRace.Agility);
            float agiBonus = (float)Math.Floor((1f + totalBAM) * statsGearEnchantsBuffs.Agility);
            statsTotal.Agility = agiBase + agiBonus;

            // Attack Power
            float totalBAPM        = statsTotal.BonusAttackPowerMultiplier;
            float apBase           = (1f + totalBAPM) * (statsRace.AttackPower);
            float apBonusAGI       = (1f + totalBAPM) * (statsTotal.Agility);
            float apBonusSTR       = (1f + totalBAPM) * (statsTotal.Strength);
            float apBonusOther     = (1f + totalBAPM) * (statsGearEnchantsBuffs.AttackPower);
            statsTotal.AttackPower = (float)Math.Floor(apBase + apBonusAGI + apBonusSTR + apBonusOther);

            // Armor (Not currently being used in Rogue)
            /*statsTotal.Armor       = (float)Math.Floor(statsTotal.Armor      * (1f + statsTotal.BaseArmorMultiplier ));
            statsTotal.BonusArmor += statsTotal.Agility * 2f;
            statsTotal.BonusArmor  = (float)Math.Floor(statsTotal.BonusArmor * (1f + statsTotal.BonusArmorMultiplier));
            statsTotal.Armor      += statsTotal.BonusArmor;*/

            /*statsTotal.PhysicalHaste *= (1f + StatConversion.GetHasteFromRating(statsTotal.HasteRating,CharacterClass.Rogue))
                                     *  (1f + Talents.BladeFlurry.Haste.Bonus)
                                     *  (1f + Talents.LightningReflexes.Haste.Bonus);*/
            statsTotal.HasteRating = (float)Math.Floor(statsTotal.HasteRating);
            float ratingHasteBonus = StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, CharacterClass.Rogue);
            statsTotal.PhysicalHaste = (1f + statsRace.PhysicalHaste) *
                                       (1f + statsItems.PhysicalHaste) *
                                       (1f + statsBuffs.PhysicalHaste) *
                                       (1f + statsTalents.PhysicalHaste) *
                                       (1f + statsOptionsPanel.PhysicalHaste) *
                                       (1f + statsProcs.PhysicalHaste) *
                                       (1f + ratingHasteBonus)
                                       - 1f;

            statsTotal.PhysicalCrit += StatConversion.GetCritFromAgility(statsTotal.Agility, CharacterClass.Rogue);

            // Defensive Stats
            statsTotal.Dodge += StatConversion.GetDodgeFromAgility(statsTotal.Agility, CharacterClass.Rogue);
            statsTotal.Parry += 5f;

            /*//Berserking
            if (character.MainHand != null && character.MainHandEnchant != null && character.MainHandEnchant.Id == 3789) {
                statsTotal.AttackPower += 135f;  //taken straight from Elitist Jerks  
            }
            if (character.OffHand != null && character.OffHandEnchant != null && character.OffHandEnchant.Id == 3789) {
                statsTotal.AttackPower += 135f;  //taken straight from Elitist Jerks  
            }*/

            // SpecialEffects: Supposed to handle all procs such as Berserking, Mirror of Truth, Grim Toll, etc.
            CombatFactors combatFactors = new CombatFactors(character, statsTotal);
            WhiteAttacks whiteAttacks = new WhiteAttacks(combatFactors);

            float fightDuration = 600f;//calcOpts.Duration;

            float mhHitsPerSecond = 0f; float ohHitsPerSecond = 0f;
            mhHitsPerSecond = 1f / (1.5f/* + calcOpts.GetLatency()*/) * 0.9f * combatFactors.ProbYellowHit;
            // White Hits per second uses hasted numbers, not un-hasted
            if (combatFactors.MH.Speed > 0f) { mhHitsPerSecond += whiteAttacks.MhHits; }
            if (combatFactors.OH.Speed > 0f) { ohHitsPerSecond += whiteAttacks.OhHits; }

            float mhHitInterval = 1f / mhHitsPerSecond;
            float ohHitInterval = 1f / ohHitsPerSecond;
            float bothHitInterval = 1f / (mhHitsPerSecond + ohHitsPerSecond);
            //float bleedHitInterval = 1f / (calcOpts.FuryStance ? 1f : 4f / 3f); // 4/3 ticks per sec with deep wounds and rend both going, 1 tick/sec with just deep wounds
            float dmgDoneInterval = 1f / (mhHitsPerSecond + ohHitsPerSecond /*+ (calcOpts.FuryStance ? 1f : 4f / 3f)*/);

            SpecialEffect bersMainHand = null;
            SpecialEffect bersOffHand = null;

            // special case for dual wielding w/ berserker enchant on one/both weapons, as they act independently
            if (character.MainHandEnchant != null && character.MainHandEnchant.Id == 3789) { // berserker enchant id
                Stats.SpecialEffectEnumerator mhEffects = character.MainHandEnchant.Stats.SpecialEffects();

                if (mhEffects.MoveNext()) {
                    bersMainHand = mhEffects.Current;
                    statsProcs += bersMainHand.GetAverageStats(mhHitInterval, 1f, combatFactors.MH.Speed, fightDuration);
                }
            }
            if (character.OffHandEnchant != null && character.OffHandEnchant.Id == 3789) {
                Stats.SpecialEffectEnumerator ohEffects = character.OffHandEnchant.Stats.SpecialEffects();

                if (ohEffects.MoveNext()) {
                    bersOffHand = ohEffects.Current;
                    statsProcs += bersOffHand.GetAverageStats(ohHitInterval, 1f, combatFactors.OH.Speed, fightDuration);
                }
            }
            foreach (SpecialEffect effect in statsTotal.SpecialEffects())
            {
                if (effect != bersMainHand && effect != bersOffHand) // bersStats is null if the char doesn't have berserking enchant
                {
                    float oldArp = effect.Stats.ArmorPenetrationRating;
                    if (effect.Stats.ArmorPenetrationRating > 0)
                    {
                        float arpenBuffs = 0;
                        float currentArp = arpenBuffs + StatConversion.GetArmorPenetrationFromRating(statsTotal.ArmorPenetrationRating);
                        float arpToHardCap = (1f - currentArp) * StatConversion.RATING_PER_ARMORPENETRATION;
                        if (arpToHardCap < effect.Stats.ArmorPenetrationRating) effect.Stats.ArmorPenetrationRating = arpToHardCap;
                    }
                    switch (effect.Trigger)
                    {
                        case Trigger.Use:
                            statsProcs += effect.GetAverageStats(0f, 1f, combatFactors.MHSpeed, fightDuration);
                            break;
                        case Trigger.MeleeHit:
                        case Trigger.PhysicalHit:
                            statsProcs += effect.GetAverageStats(bothHitInterval, 1f, combatFactors.MHSpeed, fightDuration);
                            break;
                        case Trigger.MeleeCrit:
                        case Trigger.PhysicalCrit:
                            statsProcs += effect.GetAverageStats(bothHitInterval, combatFactors.ProbMhCrit, combatFactors.MHSpeed, fightDuration);
                            break;
                        /*case Trigger.DoTTick:
                            statsProcs += effect.GetAverageStats(bleedHitInterval, 1f, combatFactors.MHSpeed, fightDuration); // 1/sec DeepWounds, 1/3sec Rend
                            break;*/
                        case Trigger.DamageDone: // physical and dots
                            statsProcs += effect.GetAverageStats(dmgDoneInterval, 1f, combatFactors.MHSpeed, fightDuration);
                            break;
                    }
                    effect.Stats.ArmorPenetrationRating = oldArp;
                }
            }

            statsProcs.Stamina   = (float)Math.Floor(statsProcs.Stamina * (1f + totalBSTAM) * (1f + statsProcs.BonusStaminaMultiplier));
            statsProcs.Strength  = (float)Math.Floor(statsProcs.Strength * (1f + totalBSM) * (1f + statsProcs.BonusStrengthMultiplier));
            statsProcs.Agility   = (float)Math.Floor(statsProcs.Agility * (1f + totalBAM) * (1f + statsProcs.BonusAgilityMultiplier));
            statsProcs.Agility  += (float)Math.Floor(statsProcs.HighestStat * (1f + totalBAM) * (1f + statsProcs.BonusAgilityMultiplier));
            statsProcs.Health   += (float)Math.Floor(statsProcs.Stamina * 10f);

            // Armor
            statsProcs.Armor = (float)Math.Floor(statsProcs.Armor * (1f + statsTotal.BaseArmorMultiplier + statsProcs.BaseArmorMultiplier));
            statsProcs.BonusArmor += statsProcs.Agility * 2f;
            statsProcs.BonusArmor = (float)Math.Floor(statsProcs.BonusArmor * (1f + statsTotal.BonusArmorMultiplier + statsProcs.BonusArmorMultiplier));
            statsProcs.Armor += statsProcs.BonusArmor;

            // Attack Power
            float totalBAPMProcs    = (1f + statsTotal.BonusAttackPowerMultiplier) * (1f + statsProcs.BonusAttackPowerMultiplier) - 1f;
            float apBonusAGIProcs   = (1f + totalBAPM) * (statsProcs.Agility);
            float apBonusSTRProcs   = (1f + totalBAPM) * (statsProcs.Strength);
            float apBonusOtherProcs = (1f + totalBAPM) * (statsProcs.AttackPower);
            statsProcs.AttackPower  = (float)Math.Floor(apBonusSTRProcs + apBonusAGIProcs + apBonusOtherProcs);

            // Haste
            statsProcs.PhysicalHaste = (1f + statsProcs.PhysicalHaste)
                                     * (1f + StatConversion.GetPhysicalHasteFromRating(statsProcs.HasteRating, CharacterClass.Rogue))
                                     - 1f;

            statsTotal += statsProcs;

            // Haste
            statsTotal.HasteRating = (float)Math.Floor(statsTotal.HasteRating);
            ratingHasteBonus = StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, CharacterClass.Rogue);
            statsTotal.PhysicalHaste = (1f + statsRace.PhysicalHaste) *
                                       (1f + statsItems.PhysicalHaste) *
                                       (1f + statsBuffs.PhysicalHaste) *
                                       (1f + statsTalents.PhysicalHaste) *
                                       (1f + statsOptionsPanel.PhysicalHaste) *
                                       (1f + statsProcs.PhysicalHaste) *
                                       (1f + ratingHasteBonus)
                                       - 1f;
            
            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName) {
            switch (chartName) {
                case "Combat Table":
                    var currentCalculationsRogue = GetCharacterCalculations(character) as CharacterCalculationsRogue;
                    var calcMiss = new ComparisonCalculationsRogue();
                    var calcDodge = new ComparisonCalculationsRogue();
                    var calcParry = new ComparisonCalculationsRogue();
                    var calcBlock = new ComparisonCalculationsRogue();
                    var calcGlance = new ComparisonCalculationsRogue();
                    var calcCrit = new ComparisonCalculationsRogue();
                    var calcHit = new ComparisonCalculationsRogue();

                    if (currentCalculationsRogue != null)
                    {
                        calcMiss.Name = "    Miss    ";
                        calcDodge.Name = "   Dodge   ";
                        calcGlance.Name = " Glance ";
                        calcCrit.Name = "  Crit  ";
                        calcHit.Name = "Hit";

                        calcMiss.OverallPoints = 0f;
                        calcDodge.OverallPoints = 0f;
                        calcParry.OverallPoints = 0f;
                        calcBlock.OverallPoints = 0f;
                        calcGlance.OverallPoints = 0f;
                        calcCrit.OverallPoints = 0f;
                        calcHit.OverallPoints = 0f;
                    }
                    return new ComparisonCalculationBase[] {calcMiss, calcDodge, calcParry, calcGlance, calcBlock, calcCrit, calcHit};

                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        public override Stats GetRelevantStats(Stats stats) {
            Stats relevantStats = new Stats {
                           Agility = stats.Agility,
                           Strength = stats.Strength,
                           AttackPower = stats.AttackPower,
                           CritRating = stats.CritRating,
                           HitRating = stats.HitRating,
                           Stamina = stats.Stamina,
                           HasteRating = stats.HasteRating,
                           ExpertiseRating = stats.ExpertiseRating,
                           ArmorPenetration = stats.ArmorPenetration,
                           ArmorPenetrationRating = stats.ArmorPenetrationRating,
                           BloodlustProc = stats.BloodlustProc,
                           WeaponDamage = stats.WeaponDamage,
                           BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                           BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                           BonusCritMultiplier = stats.BonusCritMultiplier,
                           BonusDamageMultiplier = stats.BonusDamageMultiplier,
                           BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                           BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                           Health = stats.Health,
                           ExposeWeakness = stats.ExposeWeakness,
                           Bloodlust = stats.Bloodlust,
                           ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
                           PhysicalHaste = stats.PhysicalHaste,
                           PhysicalHit = stats.PhysicalHit,
                           PhysicalCrit = stats.PhysicalCrit,
                           HighestStat = stats.HighestStat,
                           
                           /*AllResist = stats.AllResist,
                           ArcaneResistance = stats.ArcaneResistance,
                           NatureResistance = stats.NatureResistance,
                           FireResistance = stats.FireResistance,
                           FrostResistance = stats.FrostResistance,
                           ShadowResistance = stats.ShadowResistance,
                           ArcaneResistanceBuff = stats.ArcaneResistanceBuff,
                           NatureResistanceBuff = stats.NatureResistanceBuff,
                           FireResistanceBuff = stats.FireResistanceBuff,
                           FrostResistanceBuff = stats.FrostResistanceBuff,
                           ShadowResistanceBuff = stats.ShadowResistanceBuff,*/
                           
                           BonusSnDDuration = stats.BonusSnDDuration,
                           CPOnFinisher = stats.CPOnFinisher,
                           BonusEvisEnvenomDamage = stats.BonusEvisEnvenomDamage,
                           BonusFreeFinisher = stats.BonusFreeFinisher,
                           BonusCPGDamage = stats.BonusCPGDamage,
                           BonusSnDHaste = stats.BonusSnDHaste,
                           BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
                           RogueT7TwoPieceBonus = stats.RogueT7TwoPieceBonus,
                           RogueT7FourPieceBonus = stats.RogueT7FourPieceBonus,
                           RogueT8TwoPieceBonus = stats.RogueT8TwoPieceBonus,
                           RogueT8FourPieceBonus = stats.RogueT8FourPieceBonus
                       };

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit
                || effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit || effect.Trigger == Trigger.DoTTick
                    || effect.Trigger == Trigger.DamageDone)
                {
                    if (HasRelevantStats(effect.Stats))
                    {
                        relevantStats.AddSpecialEffect(effect);
                    }
                }
            }

            return relevantStats;
        }
        public override bool HasRelevantStats(Stats stats) {
            bool relevant = (
                    stats.Agility +
                    stats.Strength +
                    stats.AttackPower +
                    stats.CritRating +
                    stats.HitRating +
                    //stats.Stamina +
                    stats.HasteRating +
                    stats.ExpertiseRating +
                    stats.ArmorPenetration +
                    stats.ArmorPenetrationRating +
                    stats.BloodlustProc +
                    stats.WeaponDamage +
                    stats.BonusAgilityMultiplier +
                    stats.BonusAttackPowerMultiplier +
                    stats.BonusCritMultiplier +
                    stats.BonusDamageMultiplier +
                    stats.BonusStaminaMultiplier +
                    stats.BonusStrengthMultiplier +
                    stats.Health +
                    stats.ExposeWeakness +
                    stats.Bloodlust +
                    stats.ThreatReductionMultiplier +
                    stats.PhysicalHaste +
                    stats.PhysicalHit +
                    stats.PhysicalCrit +
                    stats.HighestStat +

                    /*stats.AllResist +
                    stats.ArcaneResistance +
                    stats.NatureResistance +
                    stats.FireResistance +
                    stats.FrostResistance +
                    stats.ShadowResistance +
                    stats.ArcaneResistanceBuff +
                    stats.NatureResistanceBuff +
                    stats.FireResistanceBuff +
                    stats.FrostResistanceBuff +
                    stats.ShadowResistanceBuff +*/

                    stats.BonusSnDDuration +
                    stats.CPOnFinisher +
                    stats.BonusEvisEnvenomDamage +
                    stats.BonusFreeFinisher +
                    stats.BonusCPGDamage +
                    stats.BonusSnDHaste +
                    stats.BonusBleedDamageMultiplier +
                    stats.RogueT7TwoPieceBonus +
                    stats.RogueT7FourPieceBonus +
                    stats.RogueT8TwoPieceBonus +
                    stats.RogueT8FourPieceBonus
                ) != 0;

            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit
                    || effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit || effect.Trigger == Trigger.DoTTick
                    || effect.Trigger == Trigger.DamageDone)
                {
                    relevant |= HasRelevantStats(effect.Stats);
                    if (relevant) { break; }
                }
            }
            return relevant;
        }

        public override bool IsEnchantRelevant(Enchant enchant) {
        	try {
				return IsEnchantWithSpecialProc(enchant) || HasRelevantStats(enchant.Stats);
			} catch (Exception) {
				return false;
			}
		}

        private static bool IsEnchantWithSpecialProc( Enchant enchant ) {
            return enchant.Id == 3789;  //Berserking
        }

        private static List<string> _relevantGlyphs = null;
        public override List<string> GetRelevantGlyphs() {
            if (_relevantGlyphs == null) {
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of Backstab");
                _relevantGlyphs.Add("Glyph of Eviscerate");
                _relevantGlyphs.Add("Glyph of Mutilate");
                _relevantGlyphs.Add("Glyph of Hunger for Blood");
                _relevantGlyphs.Add("Glyph of Sinister Strike");
                _relevantGlyphs.Add("Glyph of Slice and Dice");
                _relevantGlyphs.Add("Glyph of Feint");
                _relevantGlyphs.Add("Glyph of Rupture");
                _relevantGlyphs.Add("Glyph of Blade Flurry");
                _relevantGlyphs.Add("Glyph of Adrenaline Rush");
                /*_relevantGlyphs.Add("Glyph of Killing Spree");
                _relevantGlyphs.Add("Glyph of Vigor");
                _relevantGlyphs.Add("Glyph of Fan of Knives");
                _relevantGlyphs.Add("Glyph of Expose Armor");
                _relevantGlyphs.Add("Glyph of Ghostly Strike");*/
            }
            return _relevantGlyphs;
        }
    }
}