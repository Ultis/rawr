using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using Rawr.Rogue.BasicStats;
using Rawr.Rogue.ClassAbilities;
using Rawr.Rogue.FinishingMoves;
using Rawr.Rogue.Poisons;
using Rawr.Rogue.SpecialAbilities;

namespace Rawr.Rogue
{
    [Calculations.RawrModelInfoAttribute("Rogue", "Ability_Rogue_SliceDice", Character.CharacterClass.Rogue)]
    public class CalculationsRogue : CalculationsBase
    {
        public CalculationsRogue(){}
        private readonly CalculationOptionsPanelBase _calculationOptionsPanel = new CalculationOptionsPanelRogue();
        private readonly string[] _customChartNames = new[] {"Combat Table"};
        private readonly Dictionary<string, Color> _subPointNameColors = new Dictionary<string, Color> {{"DPS", Color.Red}};

        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get { return _calculationOptionsPanel; }
        }

        public override string[] CharacterDisplayCalculationLabels
        {
            get { return DisplayValue.GroupedList(); }
        }

        public override Dictionary<string, Color> SubPointNameColors
        {
            get { return _subPointNameColors; }
        }

        public override string[] CustomChartNames
        {
            get { return _customChartNames; }
        }

        public override List<Item.ItemType> RelevantItemTypes
        {
            get { return RelevantItems.List; }
        }

        public override Character.CharacterClass TargetClass
        {
            get { return Character.CharacterClass.Rogue; }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationsRogue();
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsRogue();
        }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            var s = new XmlSerializer(typeof (CalculationOptionsRogue));
            var sr = new StringReader(xml);
            var calcOpts = s.Deserialize(sr) as CalculationOptionsRogue;
            return calcOpts;
        }

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
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            TalentsAndGlyphs.Initialize(character.RogueTalents);
            var stats = GetCharacterStats(character, additionalItem);
            var calcOpts = character.CalculationOptions as CalculationOptionsRogue;
            var combatFactors = new CombatFactors(character, stats);
            return GetCalculations(calcOpts, combatFactors, stats, needsDisplayCalculations);
        }

        public static CharacterCalculationsBase GetCalculations(CalculationOptionsRogue calcOpts, CombatFactors combatFactors, Stats stats, bool needsDisplayCalculations)
        {
            //------------------------------------------------------------------------------------
            // CALCULATE OUTPUTS
            //------------------------------------------------------------------------------------
            var displayedValues = new CharacterCalculationsRogue(stats);
            var whiteAttacks = new WhiteAttacks(combatFactors);
            var cycleTime = new CycleTime(calcOpts, combatFactors, whiteAttacks);
            var cpgDps = calcOpts.CpGenerator.CalcCpgDps(calcOpts, combatFactors, stats, cycleTime);

            var totalFinisherDps = 0f;
            
            foreach (var component in calcOpts.DpsCycle.Components)
            {
                var finisherDps = component.CalcFinisherDps(calcOpts, combatFactors, stats, whiteAttacks, cycleTime, displayedValues);
                displayedValues.AddToolTip(DisplayValue.FinisherDps, component + ": " + finisherDps);
                totalFinisherDps += finisherDps;
            }

            var swordSpecDps = new SwordSpec().CalcDps(calcOpts, combatFactors, whiteAttacks, cycleTime);
            var poisonDps = PoisonBase.CalcPoisonDps(calcOpts, combatFactors, stats, whiteAttacks, displayedValues, cycleTime);
            var sndUpTime = SnD.UpTime(calcOpts, cycleTime);

            displayedValues.TotalDPS = whiteAttacks.CalcMhWhiteDps() + whiteAttacks.CalcOhWhiteDps() + swordSpecDps + cpgDps + totalFinisherDps + poisonDps;
            displayedValues.OverallPoints = displayedValues.TotalDPS;

            if (!needsDisplayCalculations)
            {
                return displayedValues;
            }

            //------------------------------------------------------------------------------------
            // ADD CALCULATED OUTPUTS TO DISPLAY
            //------------------------------------------------------------------------------------
            displayedValues.AddRoundedDisplayValue(DisplayValue.MhWeaponDamage, combatFactors.MhAvgDamage);
            displayedValues.AddRoundedDisplayValue(DisplayValue.OhWeaponDamage, combatFactors.OhAvgDamage);

            displayedValues.AddDisplayValue(DisplayValue.Cpg, calcOpts.CpGenerator.Name);
            displayedValues.AddRoundedDisplayValue(DisplayValue.CycleTime, cycleTime.Duration);

            displayedValues.AddDisplayValue(DisplayValue.EnergyRegen, combatFactors.BaseEnergyRegen.ToString());

            displayedValues.AddRoundedDisplayValue(DisplayValue.HitRating, stats.HitRating);
            displayedValues.AddPercentageToolTip(DisplayValue.HitRating, "Total % Hit: ", combatFactors.HitPercent);
            displayedValues.AddPercentageToolTip(DisplayValue.HitRating, "Poison % Hit: ", combatFactors.PoisonHitPercent);

            displayedValues.AddRoundedDisplayValue(DisplayValue.CritRating, stats.CritRating);
            displayedValues.AddToolTip(DisplayValue.CritRating, "Crit % from Rating: " + combatFactors.CritFromCritRating);
            displayedValues.AddPercentageToolTip(DisplayValue.CritRating, "MH Crit %: ", combatFactors.ProbMhCrit);
            displayedValues.AddPercentageToolTip(DisplayValue.CritRating, "OH Crit%: ", combatFactors.ProbOhCrit);
            displayedValues.AddToolTip(DisplayValue.CritRating, "Crit Multiplier: " + combatFactors.BaseCritMultiplier);

            displayedValues.AddRoundedDisplayValue(DisplayValue.ArmorPenetration, combatFactors.TotalArmorPenetration);
            displayedValues.AddToolTip(DisplayValue.ArmorPenetration, "Armor Penetration Rating: " + stats.ArmorPenetrationRating);

            displayedValues.AddRoundedDisplayValue(DisplayValue.BaseExpertise, combatFactors.BaseExpertise);
            displayedValues.AddToolTip(DisplayValue.BaseExpertise, "MH Expertise: " + combatFactors.MhExpertise);
            displayedValues.AddToolTip(DisplayValue.BaseExpertise, "OH Expertise: " + combatFactors.OhExpertise);
            
            displayedValues.AddRoundedDisplayValue(DisplayValue.HasteRating, stats.HasteRating);
            displayedValues.AddPercentageToolTip(DisplayValue.HasteRating, "Total Haste %: ", (combatFactors.BaseHaste <= 0 ? 0 : combatFactors.BaseHaste - 1) );
            
            displayedValues.AddRoundedDisplayValue(DisplayValue.SndUptime, sndUpTime*100f);

            displayedValues.AddRoundedDisplayValue(DisplayValue.CpgCrit, calcOpts.CpGenerator.Crit(combatFactors, calcOpts) * 100);
            displayedValues.AddToolTip(DisplayValue.CpgCrit, "Crit From Stats: " + stats.PhysicalCrit);
            displayedValues.AddToolTip(DisplayValue.CpgCrit, "Crit from Crit Rating: " + combatFactors.CritFromCritRating);
            displayedValues.AddPercentageToolTip(DisplayValue.CpgCrit, "Boss Crit Reduction: ", combatFactors.BossCriticalReductionChance);

            displayedValues.AddRoundedDisplayValue(DisplayValue.WhiteDps, whiteAttacks.CalcMhWhiteDps() + whiteAttacks.CalcOhWhiteDps());
            displayedValues.AddToolTip(DisplayValue.WhiteDps, "MH White DPS: " + whiteAttacks.CalcMhWhiteDps());
            displayedValues.AddToolTip(DisplayValue.WhiteDps, "OH White DPS: " + whiteAttacks.CalcOhWhiteDps());

            displayedValues.AddRoundedDisplayValue(DisplayValue.CpgDps, cpgDps);
            displayedValues.AddRoundedDisplayValue(DisplayValue.FinisherDps, totalFinisherDps);
            displayedValues.AddRoundedDisplayValue(DisplayValue.SwordSpecDps, swordSpecDps);
            displayedValues.AddRoundedDisplayValue(DisplayValue.PoisonDps, poisonDps);

            return displayedValues;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            var statsRace = new BaseRogueStats(character.Race);
            var statsBaseGear = GetItemStats(character, additionalItem);
            var statsBuffs = GetBuffsStats(character.ActiveBuffs);
            var statsGearEnchantsBuffs = statsBaseGear + statsBuffs;

            //TalentTree tree = character.AllTalents;

            var agiBase = (float) Math.Floor(statsRace.Agility*(1 + statsRace.BonusAgilityMultiplier));
            var agiBonus = (float) Math.Floor(statsGearEnchantsBuffs.Agility*(1 + statsRace.BonusAgilityMultiplier));
            var strBase = (float) Math.Floor(statsRace.Strength*(1 + statsRace.BonusStrengthMultiplier));
            var strBonus = (float) Math.Floor(statsGearEnchantsBuffs.Strength*(1 + statsRace.BonusStrengthMultiplier));
            var staBase = (float) Math.Floor(statsRace.Stamina*(1 + statsRace.BonusStaminaMultiplier));
            var staBonus = (float) Math.Floor(statsGearEnchantsBuffs.Stamina*(1 + statsRace.BonusStaminaMultiplier));

            var statsTotal = new Stats();
            statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier)*(1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier)*(1 + character.RogueTalents.Deadliness*0.02f)) - 1;
            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier)*(1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)*(1 + character.RogueTalents.Vitality*0.01f)*(1 + character.RogueTalents.SinisterCalling*0.03f)) - 1;
            statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier)*(1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)) - 1;
            statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier)*(1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)*(1 + character.RogueTalents.Vitality*0.02f)) - 1;

            statsTotal.Agility = (float) Math.Floor(agiBase*(1f + statsTotal.BonusAgilityMultiplier)) + (float) Math.Floor(agiBonus*(1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Strength = (float) Math.Floor(strBase*(1f + statsTotal.BonusStrengthMultiplier)) + (float) Math.Floor(strBonus*(1 + statsTotal.BonusStrengthMultiplier));
            statsTotal.Stamina = (float) Math.Floor(staBase*(1f + statsTotal.BonusStaminaMultiplier)) + (float) Math.Floor(staBonus*(1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Health = (float) Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + ((statsTotal.Stamina - staBase)*10f))));

            statsTotal.AttackPower = (float)Math.Floor((statsTotal.Agility + statsTotal.Strength + statsRace.AttackPower + statsGearEnchantsBuffs.AttackPower) * (1+statsTotal.BonusAttackPowerMultiplier));

            statsTotal.PhysicalHit = character.RogueTalents.Precision;
            statsTotal.HitRating = statsGearEnchantsBuffs.HitRating;

            statsTotal.Expertise += character.RogueTalents.WeaponExpertise*5.0f;
            statsTotal.ExpertiseRating = statsGearEnchantsBuffs.ExpertiseRating;

            statsTotal.HasteRating = statsGearEnchantsBuffs.HasteRating;
            statsTotal.ArmorPenetration = statsGearEnchantsBuffs.ArmorPenetration;
            statsTotal.ArmorPenetrationRating = statsGearEnchantsBuffs.ArmorPenetrationRating;

			var calcOpts = character.CalculationOptions as CalculationOptionsRogue;

            switch (character.RogueTalents.SerratedBlades)
            {
                case 3:
                    statsTotal.ArmorPenetration += 640f / calcOpts.TargetArmor;
                    break;
                case 2:
                    statsTotal.ArmorPenetration += 434.4f / calcOpts.TargetArmor;
                    break;
                case 1:
                    statsTotal.ArmorPenetration += 213.6f / calcOpts.TargetArmor;
                    break;
            }

            statsTotal.CritRating = statsRace.CritRating + statsGearEnchantsBuffs.CritRating;

            statsTotal.PhysicalCrit = -0.295f;
            statsTotal.PhysicalCrit += statsTotal.Agility*RogueConversions.AgilityToCrit;
            statsTotal.PhysicalCrit += character.RogueTalents.Malice*1f;

            statsTotal.BonusCritMultiplier = statsGearEnchantsBuffs.BonusCritMultiplier;

            statsTotal.Dodge += statsTotal.Agility*RogueConversions.AgilityToDodge;
            statsTotal.Dodge += character.RogueTalents.LightningReflexes;

            statsTotal.Parry += character.RogueTalents.Deflection;

            statsTotal.WeaponDamage = statsGearEnchantsBuffs.WeaponDamage;

            statsTotal.BonusBleedDamageMultiplier = statsGearEnchantsBuffs.BonusBleedDamageMultiplier;

            // T4 bonuses
            statsTotal.BonusSnDDuration = statsGearEnchantsBuffs.BonusSnDDuration;
            statsTotal.CPOnFinisher = statsGearEnchantsBuffs.CPOnFinisher;

            // T5 bonuses
            statsTotal.BonusEvisEnvenomDamage = statsGearEnchantsBuffs.BonusEvisEnvenomDamage;
            statsTotal.BonusFreeFinisher = statsGearEnchantsBuffs.BonusFreeFinisher;

            // T6 bonuses
            statsTotal.BonusCPGDamage = statsGearEnchantsBuffs.BonusCPGDamage;
            statsTotal.BonusSnDHaste = statsGearEnchantsBuffs.BonusSnDHaste;

            //-----------------------------------------------------------------
            // FYI:  T7 and T8 are pulled from the base RAWR as true/false, 
            //       not as the actual buff values.  The actual benefit is
            //       defined/calculated in the CombatFactors class
            //-----------------------------------------------------------------

            //T7 bonuses
            statsTotal.RogueT7TwoPieceBonus = statsGearEnchantsBuffs.RogueT7TwoPieceBonus;
            statsTotal.RogueT7FourPieceBonus = statsGearEnchantsBuffs.RogueT7FourPieceBonus;

            //T8 bonuses
            statsTotal.RogueT8TwoPieceBonus = statsGearEnchantsBuffs.RogueT8TwoPieceBonus;
            statsTotal.RogueT8FourPieceBonus = statsGearEnchantsBuffs.RogueT8FourPieceBonus;

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            switch (chartName)
            {
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

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats
                       {
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
                           
                           AllResist = stats.AllResist,
                           ArcaneResistance = stats.ArcaneResistance,
                           NatureResistance = stats.NatureResistance,
                           FireResistance = stats.FireResistance,
                           FrostResistance = stats.FrostResistance,
                           ShadowResistance = stats.ShadowResistance,
                           ArcaneResistanceBuff = stats.ArcaneResistanceBuff,
                           NatureResistanceBuff = stats.NatureResistanceBuff,
                           FireResistanceBuff = stats.FireResistanceBuff,
                           FrostResistanceBuff = stats.FrostResistanceBuff,
                           ShadowResistanceBuff = stats.ShadowResistanceBuff,
                           
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
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return
                (
                    stats.Agility +
                    stats.Strength +
                    stats.AttackPower +
                    stats.CritRating +
                    stats.HitRating +
                    stats.Stamina +
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

                    stats.AllResist +
                    stats.ArcaneResistance +
                    stats.NatureResistance +
                    stats.FireResistance +
                    stats.FrostResistance +
                    stats.ShadowResistance +
                    stats.ArcaneResistanceBuff +
                    stats.NatureResistanceBuff +
                    stats.FireResistanceBuff +
                    stats.FrostResistanceBuff +
                    stats.ShadowResistanceBuff +

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
        }

        public override List<string> GetRelevantGlyphs()
        {
            return ModeledGlyphs.List;
        }

        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get { return RogueGemmingTemplates.List; }
        }
    }
}