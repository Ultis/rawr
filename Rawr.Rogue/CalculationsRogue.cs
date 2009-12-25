using System;
using System.Collections.Generic;
#if RAWR3
using System.Windows.Media;
#else
using System.Drawing;
#endif
using System.IO;
using System.Xml.Serialization;
using Rawr.Rogue.ClassAbilities;
using Rawr.Rogue.FinishingMoves;
using Rawr.Rogue.Poisons;
using Rawr.Rogue.SpecialAbilities;

namespace Rawr.Rogue {
    [Calculations.RawrModelInfoAttribute("Rogue", "Ability_Rogue_SliceDice", CharacterClass.Rogue)]
    public class CalculationsRogue : CalculationsBase {
        #region Variables and Properties

        public override List<GemmingTemplate> DefaultGemmingTemplates {
            get {
                ////Relevant Gem IDs for Rogues
                //Red
                int[] bold      = { 39900, 39996, 40111, 42142 };
                int[] delicate  = { 39905, 39997, 40112, 42143 };

                //Purple
                int[] shifting  = { 39935, 40023, 40130 };
                int[] sovereign = { 39934, 40022, 40129 };

                //Blue
                int[] solid     = { 39919, 40008, 40119, 36767 };

                //Green
                int[] enduring  = { 39976, 40089, 40167 };

                //Yellow
                int[] thick     = { 39916, 40015, 40126, 42157 };

                //Orange
                int[] etched    = { 39948, 40038, 40143 };
                int[] fierce    = { 39951, 40041, 40146 };
                int[] glinting  = { 39953, 40044, 40148 };
                int[] stalwart  = { 39964, 40056, 40160 };
                int[] deadly    = { 39952, 40043, 40147 };

                //Meta
                // int austere = 41380;
                int relentless = 41398;

                return new List<GemmingTemplate> {
				    new GemmingTemplate { Model = "Rogue", Group = "Uncommon", //Max Agility
					    RedId = delicate[0], YellowId = delicate[0], BlueId = delicate[0], PrismaticId = delicate[0], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Uncommon", //Agi/Crit
					    RedId = delicate[0], YellowId = deadly[0], BlueId = shifting[0], PrismaticId = delicate[0], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Rare",  //Max Agility
					    RedId = delicate[1], YellowId = delicate[1], BlueId = delicate[1], PrismaticId = delicate[1], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Rare",  //Agi/Crit 
					    RedId = delicate[1], YellowId = deadly[1], BlueId = shifting[1], PrismaticId = delicate[1], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Epic", Enabled = true, //Max Agility
					    RedId = delicate[2], YellowId = delicate[2], BlueId = delicate[2], PrismaticId = delicate[2], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Epic", Enabled = true, //Agi/Crit 
					    RedId = delicate[2], YellowId = deadly[2], BlueId = shifting[2], PrismaticId = delicate[2], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Jeweler", //Max Agility
					    RedId = delicate[3], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[3], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Jeweler", //Agility Heavy
					    RedId = delicate[2], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[2], MetaId = relentless },
			    };
            }
        }

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
                        ItemType.OneHandAxe,
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

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations) {
            cacheChar = character;
            TalentsAndGlyphs.Initialize(character.RogueTalents);
            CalculationOptionsRogue calcOpts = character.CalculationOptions as CalculationOptionsRogue;
            //  Talents.InitializeMurder(calcOpts);
            Stats stats = GetCharacterStats(character, additionalItem);
            CombatFactors combatFactors = new CombatFactors(character, stats);

            //------------------------------------------------------------------------------------
            // CALCULATE OUTPUTS
            //------------------------------------------------------------------------------------
            CharacterCalculationsRogue displayedValues = new CharacterCalculationsRogue(stats);

            WhiteAttacks whiteAttacks = new WhiteAttacks(character, stats, combatFactors);
            CycleTime cycleTime = new CycleTime(calcOpts, combatFactors, whiteAttacks);

            //Now that we have the basics, reset for any SnD bonus
            stats.PhysicalHaste *= (1f + SnD.CalcHasteBonus(calcOpts, cycleTime));
            whiteAttacks = new WhiteAttacks(character, stats, combatFactors);
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

            displayedValues.AddRoundedDisplayValue(DisplayValue.Haste, (combatFactors.TotalHaste <= 0 ? 0 : combatFactors.TotalHaste - 1f) * 100);
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
            cacheChar = character;
            CalculationOptionsRogue calcOpts = character.CalculationOptions as CalculationOptionsRogue;
            RogueTalents talents = character.RogueTalents;

            Stats statsRace = BaseStats.GetBaseStats(character.Level, CharacterClass.Rogue, character.Race);
            Stats statsBuffs = GetBuffsStats(character, calcOpts);
            Stats statsItems = GetItemStats(character, additionalItem);
            Stats statsOptionsPanel = new Stats() {
                // handle boss level difference
                PhysicalCrit = StatConversion.NPC_LEVEL_CRIT_MOD[calcOpts.TargetLevel - character.Level],
                SpellCrit = StatConversion.NPC_LEVEL_CRIT_MOD[calcOpts.TargetLevel - character.Level],
            };
			Stats statsTalents = new Stats()
			{
				BonusAgilityMultiplier = talents.SinisterCalling * 0.03f,
				PhysicalHit = talents.Precision * 0.01f,
                SpellHit = talents.Precision * 0.01f,
				BonusAttackPowerMultiplier = talents.Deadliness * 0.02f
										   + talents.SavageCombat * 0.02f,
				/*  PhysicalCrit = talents.Malice * 0.01f +
								((character.ActiveBuffs.FindAll(buff => buff.Group == "Critical Strike Chance Taken").Count == 0)
									? talents.MasterPoisoner * 0.01f : 0f), */
                PhysicalCrit = talents.Malice * 0.01f,
                SpellCrit = talents.Malice * 0.01f,
				Dodge = talents.LightningReflexes * 0.02f,
				Parry = talents.Deflection * 0.02f,
                PhysicalHaste = (float)Math.Round((double)(0.033*talents.LightningReflexes),2),
                /*
				PhysicalHaste = (1f + talents.BladeFlurry * 0.20f)
                              * (1f + (float)Math.Ceiling((10f/3f*talents.LightningReflexes)/100f)) - 1f,
                 */
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
            /*
            statsTotal.HasteRating = (float)Math.Floor(statsTotal.HasteRating);
            float ratingHasteBonus = StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, CharacterClass.Rogue);
            statsTotal.PhysicalHaste = (1f + statsRace.PhysicalHaste) *
                                       (1f + statsItems.PhysicalHaste) *
                                       (1f + statsBuffs.PhysicalHaste) *
                                       (1f + statsTalents.PhysicalHaste) *
                                       (1f + statsOptionsPanel.PhysicalHaste) *
                                       (1f + statsProcs.PhysicalHaste) *
                                       (1f + ratingHasteBonus);
            */
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
            WhiteAttacks whiteAttacks = new WhiteAttacks(character, statsTotal, combatFactors);

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
            float poisonHitInterval = 1f;

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
                        case Trigger.SpellHit:
                            statsProcs += effect.GetAverageStats(poisonHitInterval, 1f, combatFactors.MHSpeed, fightDuration);
                            break;
                    }
                    effect.Stats.ArmorPenetrationRating = oldArp;
                }
            }

            statsProcs.Stamina   = (float)Math.Floor(statsProcs.Stamina * (1f + totalBSTAM) * (1f + statsProcs.BonusStaminaMultiplier));
            statsProcs.Strength  = (float)Math.Floor(statsProcs.Strength * (1f + totalBSM) * (1f + statsProcs.BonusStrengthMultiplier));
            statsProcs.Agility  += statsProcs.Paragon;
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
            statsProcs.PhysicalHaste = StatConversion.GetPhysicalHasteFromRating(statsProcs.HasteRating, CharacterClass.Rogue);

            statsTotal += statsProcs;

            // Haste
            statsTotal.HasteRating = (float)Math.Floor(statsTotal.HasteRating);
            float ratingHasteBonus = StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, CharacterClass.Rogue);
            statsTotal.PhysicalHaste = (1f + statsRace.PhysicalHaste) *
                                       (1f + statsItems.PhysicalHaste) *
                                       (1f + statsBuffs.PhysicalHaste) *
                                       (1f + statsTalents.PhysicalHaste) *
                                       (1f + statsOptionsPanel.PhysicalHaste) *
                                       (1f + statsProcs.PhysicalHaste) *
                                       (1f + Talents.BladeFlurry.Haste.Bonus * 15f / 120f) *
                                       (1f + ratingHasteBonus);

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

        public override bool IsBuffRelevant(Buff buff)
        {
            if (!buff.AllowedClasses.Contains(CharacterClass.Rogue)) { return false; }
            if (buff.Name == "Focus Magic") { return false; }

            if (buff.SetName == "Strength of the Clefthoof") { return false; }
            if (buff.SetName == "Skyshatter Regalia") { return false; }

            return base.IsBuffRelevant(buff);
        }

        public override bool IsEnchantRelevant(Enchant enchant)
        {
            string name = enchant.Name;
            if (name.Contains("Rune of"))
            {
                return false; // Bad DK Enchant, Bad!
            }
            return base.IsEnchantRelevant(enchant);
        }

        public Stats GetBuffsStats(Character character, CalculationOptionsRogue calcOpts) {
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
            }
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
               RogueT8FourPieceBonus = stats.RogueT8FourPieceBonus,
               ReduceEnergyCostFromRupture = stats.ReduceEnergyCostFromRupture,
               BonusCPGCritChance = stats.BonusCPGCritChance,

               BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
               SpellHit = stats.SpellHit,
               SpellCrit = stats.SpellCrit,
               SpellCritOnTarget = stats.SpellCritOnTarget,
               BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier
           };

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit
                || effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit || effect.Trigger == Trigger.DoTTick
                    || effect.Trigger == Trigger.DamageDone || effect.Trigger == Trigger.SpellHit)
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
                    //stats.Health +
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
                    //stats.RuptureCrit +
                    stats.RogueT8FourPieceBonus +
                    stats.ReduceEnergyCostFromRupture +
                    stats.BonusCPGCritChance +

                    stats.BonusPhysicalDamageMultiplier +
                    stats.SpellHit +
                    stats.SpellCrit +
                    stats.SpellCritOnTarget +
                    stats.BonusNatureDamageMultiplier
                ) != 0;

            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (effect.Trigger == Trigger.Use
                    || effect.Trigger == Trigger.MeleeHit
                    || effect.Trigger == Trigger.MeleeCrit
                    || effect.Trigger == Trigger.PhysicalHit
                    || effect.Trigger == Trigger.PhysicalCrit
                    || effect.Trigger == Trigger.DoTTick
                    || effect.Trigger == Trigger.DamageDone
                    || effect.Trigger == Trigger.SpellHit) // For Poison Hits
                {
                    relevant |= HasRelevantStats(effect.Stats);
                    if (relevant) { break; }
                }
            }
            return relevant;
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