using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using Rawr.Rogue.ComboPointGenerators;

namespace Rawr.Rogue
{
    [Calculations.RawrModelInfoAttribute("Rogue", "Ability_Rogue_SliceDice", Character.CharacterClass.Rogue)]
    public class CalculationsRogue : CalculationsBase
    {
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                return new List<GemmingTemplate>();
            }
        }

        public CalculationsRogue()
        {
            SetupRelevantItemTypes();
        }
        
        private readonly CalculationOptionsPanelBase _calculationOptionsPanel = new CalculationOptionsPanelRogue();
        private readonly string[] _customChartNames = new[] {"Combat Table"};
        private readonly Dictionary<string, Color> _subPointNameColors = new Dictionary<string, Color> {{"DPS", Color.Red}};
        private List<Item.ItemType> _relevantItemTypes;

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
            get { return _relevantItemTypes; }
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
            Talents.Initialize(character.RogueTalents);
            var stats = GetCharacterStats(character, additionalItem);
            var calcOpts = character.CalculationOptions as CalculationOptionsRogue;
            var combatFactors = new CombatFactors(character, stats);
            return GetCalculations(combatFactors, stats, calcOpts, character.RogueTalents);
        }

        public static CharacterCalculationsBase GetCalculations(CombatFactors combatFactors, Stats stats, CalculationOptionsRogue calcOpts, RogueTalents talents)
        {
            //------------------------------------------------------------------------------------
            // CALCULATE OUTPUTS
            //------------------------------------------------------------------------------------
            var calculatedStats = new CharacterCalculationsRogue(stats);
           
            var numCpg = CalcComboPointsNeededForCycle(calcOpts);
            //var cpg = ComboPointGenerator.Get(talents, combatFactors);

            var whiteAttacks = new WhiteAttacks(combatFactors);
            var cycleTime = CalcCycleTime(calcOpts, combatFactors, whiteAttacks, numCpg);
            var cpgDps = calcOpts.CpGenerator.CalcCpgDPS(stats, combatFactors, calcOpts, numCpg, cycleTime);

            var totalFinisherDps = 0f;
            foreach (var component in calcOpts.DpsCycle.Components)
            {
                var finisherDps = component.CalcFinisherDPS(talents, stats, combatFactors, cycleTime);
                calculatedStats.AddToolTip(DisplayValue.FinisherDPS, component + ": " + finisherDps);
                totalFinisherDps += finisherDps;
            }

            var swordSpecDps = new SwordSpec().CalcDPS(combatFactors, whiteAttacks, numCpg, cycleTime);
            var poisonDps = CalcPoisonDps(stats, calcOpts, combatFactors, whiteAttacks, calculatedStats);

            //------------------------------------------------------------------------------------
            // ADD CALCULATED OUTPUTS TO DISPLAY
            //------------------------------------------------------------------------------------
            calculatedStats.AddRoundedDisplayValue(DisplayValue.MhWeaponDamage, combatFactors.MhAvgDamage);
            calculatedStats.AddRoundedDisplayValue(DisplayValue.OhWeaponDamage, combatFactors.OhAvgDamage);

            calculatedStats.AddDisplayValue(DisplayValue.CPG, calcOpts.CpGenerator.Name);
            calculatedStats.AddRoundedDisplayValue(DisplayValue.CycleTime, cycleTime);

            calculatedStats.AddDisplayValue(DisplayValue.EnergyRegen, combatFactors.BaseEnergyRegen.ToString());

            calculatedStats.AddRoundedDisplayValue(DisplayValue.HitRating, stats.HitRating);
            calculatedStats.AddPercentageToolTip(DisplayValue.HitRating, "Total % Hit: ", combatFactors.HitPercent);

            calculatedStats.AddRoundedDisplayValue(DisplayValue.CritRating, stats.CritRating);
            calculatedStats.AddToolTip(DisplayValue.CritRating, "Crit % from Rating: " + combatFactors.CritFromCritRating);
            calculatedStats.AddPercentageToolTip(DisplayValue.CritRating, "MH Crit %: ", combatFactors.ProbMhCrit);
            calculatedStats.AddPercentageToolTip(DisplayValue.CritRating, "OH Crit%: ", combatFactors.ProbOhCrit);
            calculatedStats.AddToolTip(DisplayValue.CritRating, "Crit Multiplier: " + combatFactors.BaseCritMultiplier);

            calculatedStats.AddRoundedDisplayValue(DisplayValue.ArmorPenetration, combatFactors.TotalArmorPenetration);
            calculatedStats.AddToolTip(DisplayValue.ArmorPenetration, "Armor Penetration Rating: " + stats.ArmorPenetrationRating);

            calculatedStats.AddRoundedDisplayValue(DisplayValue.BaseExpertise, combatFactors.BaseExpertise);
            calculatedStats.AddToolTip(DisplayValue.BaseExpertise, "MH Expertise: " + combatFactors.MhExpertise);
            calculatedStats.AddToolTip(DisplayValue.BaseExpertise, "OH Expertise: " + combatFactors.OhExpertise);
            
            calculatedStats.AddRoundedDisplayValue(DisplayValue.HasteRating, stats.HasteRating);
            calculatedStats.AddPercentageToolTip(DisplayValue.HasteRating, "Total Haste %: ", (combatFactors.BaseHaste <= 0 ? 0 : combatFactors.BaseHaste - 1) );

            calculatedStats.AddRoundedDisplayValue(DisplayValue.CpgCrit, calcOpts.CpGenerator.Crit(combatFactors) * 100);
            calculatedStats.AddToolTip(DisplayValue.CpgCrit, "Crit From Stats: " + stats.PhysicalCrit);
            calculatedStats.AddToolTip(DisplayValue.CpgCrit, "Crit from Crit Rating: " + combatFactors.CritFromCritRating);
            calculatedStats.AddPercentageToolTip(DisplayValue.CpgCrit, "Boss Crit Reduction: ", combatFactors.BossCriticalReductionChance);

            calculatedStats.AddRoundedDisplayValue(DisplayValue.WhiteDPS, whiteAttacks.CalcMhWhiteDPS() + whiteAttacks.CalcOhWhiteDPS());
            calculatedStats.AddToolTip(DisplayValue.WhiteDPS, "MH White DPS: " + whiteAttacks.CalcMhWhiteDPS());
            calculatedStats.AddToolTip(DisplayValue.WhiteDPS, "OH White DPS: " + whiteAttacks.CalcOhWhiteDPS());

            calculatedStats.AddRoundedDisplayValue(DisplayValue.CPGDPS, cpgDps);
            calculatedStats.AddRoundedDisplayValue(DisplayValue.FinisherDPS, totalFinisherDps);
            calculatedStats.AddRoundedDisplayValue(DisplayValue.SwordSpecDPS, swordSpecDps);
            calculatedStats.AddRoundedDisplayValue(DisplayValue.PoisonDPS, poisonDps);

            calculatedStats.TotalDPS = whiteAttacks.CalcMhWhiteDPS() + whiteAttacks.CalcOhWhiteDPS() + swordSpecDps + cpgDps + totalFinisherDps + poisonDps;
            calculatedStats.OverallPoints = calculatedStats.TotalDPS;

            return calculatedStats;
        }

        private static float CalcComboPointsNeededForCycle(CalculationOptionsRogue calcOpts)
        {
            return calcOpts.DpsCycle.TotalComboPoints - (calcOpts.DpsCycle.Components.Count * Talents.Ruthlessness.Bonus);
        }

        private static float CalcCycleTime(CalculationOptionsRogue calcOpts, CombatFactors combatFactors, WhiteAttacks whiteAttacks, float numCpg)
        {
            var energyRegen = combatFactors.BaseEnergyRegen;
            energyRegen += Talents.CombatPotency.Bonus * whiteAttacks.OhHits;
            energyRegen += Talents.FocusedAttacks.Bonus * ( whiteAttacks.MhCrits + whiteAttacks.OhCrits );

            if(calcOpts.TempMainHandEnchant.IsDeadlyPoison || calcOpts.TempOffHandEnchant.IsDeadlyPoison)
            {
                energyRegen += combatFactors.Tier8TwoPieceEnergyBonus;    
            }

            var cpgDuration = calcOpts.CpGenerator.CalcDuration(numCpg, energyRegen, combatFactors);

            var finisherEnergyCost = 0f;
            foreach(var component in calcOpts.DpsCycle.Components)
            {
                finisherEnergyCost += component.Finisher.EnergyCost(combatFactors, component.Rank);
            }
            return cpgDuration + (finisherEnergyCost / energyRegen);
        }

        private static float CalcPoisonDps( Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, WhiteAttacks whiteAttacks, CharacterCalculationsRogue calculatedStats )
        {
            var mhPoisonDps = calcOpts.TempMainHandEnchant.CalcPoisonDPS(stats, calcOpts, combatFactors, 0f);
            calculatedStats.AddToolTip(DisplayValue.PoisonDPS, "MH Poison DPS: " + Math.Round(mhPoisonDps, 2));

            if (calcOpts.TempMainHandEnchant.IsDeadlyPoison && calcOpts.TempOffHandEnchant.IsDeadlyPoison)
            {
                //not modeled yet:  envenom & loss of DP
                return mhPoisonDps;
            }

            var ohPoisonDps = calcOpts.TempOffHandEnchant.CalcPoisonDPS(stats, calcOpts, combatFactors, whiteAttacks.OhHits);
            calculatedStats.AddToolTip(DisplayValue.PoisonDPS, "OH Poison DPS: " + Math.Round(ohPoisonDps, 2));

            return mhPoisonDps + ohPoisonDps;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace = GetRaceStats(character.Race);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            //Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsGearEnchantsBuffs = statsBaseGear + statsBuffs;

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
            // Haste trinket (Meteorite Whetstone)
            statsTotal.HasteRating += statsGearEnchantsBuffs.HasteRatingOnPhysicalAttack * 10 / 45;

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

            statsTotal.WindfuryAPBonus = statsGearEnchantsBuffs.WindfuryAPBonus;

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

        public Stats GetBuffsStats(Character character)
        {
            var statsBuffs = GetBuffsStats(character.ActiveBuffs);

            // buffs from DPSWarr
            //Add Expose Weakness since it's not listed as an AP buff
            if (statsBuffs.ExposeWeakness > 0) statsBuffs.AttackPower += 200f;

            //Mongoose
            if (character.MainHand != null && character.MainHandEnchant != null && character.MainHandEnchant.Id == 2673)
            {
                statsBuffs.Agility += 120f * ((40f * (1f / (60f / character.MainHand.Speed)) / 6f));
                statsBuffs.HasteRating += (15.76f * 2f) * ((40f * (1f / (60f / character.MainHand.Speed)) / 6f));
            }
            if (character.OffHand != null && character.OffHandEnchant != null && character.OffHandEnchant.Id == 2673)
            {
                statsBuffs.Agility += 120f * ((40f * (1f / (60f / character.OffHand.Speed)) / 6f));
                statsBuffs.HasteRating += (15.76f * 2f) * ((40f * (1f / (60f / character.OffHand.Speed)) / 6f));
            }

            ////Executioner
            //if (character.MainHand != null && character.MainHandEnchant != null && character.MainHandEnchant.Id == 3225)
            //{
            //    statsBuffs.ArmorPenetration += 840f * ((40f * (1f / (60f / character.MainHand.Speed)) / 6f));
            //}

            return statsBuffs;
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
                           Stamina = stats.Stamina,
                           Agility = stats.Agility,
                           Strength = stats.Strength,
                           AttackPower = stats.AttackPower,
                           BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                           BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                           BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                           BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                           Health = stats.Health,
                           CritRating = stats.CritRating,
                           HitRating = stats.HitRating,
                           HasteRating = stats.HasteRating,
                           ExpertiseRating = stats.ExpertiseRating,
                           ArmorPenetration = stats.ArmorPenetration,
                           WeaponDamage = stats.WeaponDamage,
                           BonusCritMultiplier = stats.BonusCritMultiplier,
                           WindfuryAPBonus = stats.WindfuryAPBonus,
                           MongooseProc = stats.MongooseProc,
                           MongooseProcAverage = stats.MongooseProcAverage,
                           MongooseProcConstant = stats.MongooseProcConstant,
                           ExecutionerProc = stats.ExecutionerProc,
                           BonusSnDDuration = stats.BonusSnDDuration,
                           CPOnFinisher = stats.CPOnFinisher,
                           BonusEvisEnvenomDamage = stats.BonusEvisEnvenomDamage,
                           BonusFreeFinisher = stats.BonusFreeFinisher,
                           BonusCPGDamage = stats.BonusCPGDamage,
                           BonusSnDHaste = stats.BonusSnDHaste,
                           BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier
                       };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Agility + stats.Strength + stats.BonusAgilityMultiplier + stats.BonusStrengthMultiplier + stats.AttackPower + stats.BonusAttackPowerMultiplier + stats.CritRating + stats.HitRating + stats.HasteRating + stats.ExpertiseRating + stats.ArmorPenetration + stats.WeaponDamage + stats.BonusCritMultiplier + stats.WindfuryAPBonus + stats.MongooseProc + stats.MongooseProcAverage + stats.MongooseProcConstant + stats.ExecutionerProc + stats.BonusSnDDuration + stats.CPOnFinisher + stats.BonusEvisEnvenomDamage + stats.BonusFreeFinisher + stats.BonusCPGDamage + stats.BonusSnDHaste + stats.BonusBleedDamageMultiplier) != 0;
        }

        private void SetupRelevantItemTypes()
        {
            _relevantItemTypes = new List<Item.ItemType>(new[]
                                                             {
                                                                 Item.ItemType.None,
                                                                 Item.ItemType.Leather,
                                                                 Item.ItemType.Bow,
                                                                 Item.ItemType.Crossbow,
                                                                 Item.ItemType.Gun,
                                                                 Item.ItemType.Thrown,
                                                                 Item.ItemType.Dagger,
                                                                 Item.ItemType.FistWeapon,
                                                                 Item.ItemType.OneHandMace,
                                                                 Item.ItemType.OneHandSword
                                                             });
        }


        private static readonly Dictionary<Character.CharacterRace, float[]> BaseRogueRaceStats = new Dictionary<Character.CharacterRace, float[]>
                                                                  {
                                                                    // Agility,Strength,Stamina
                                                                    { Character.CharacterRace.Human, new [] {158f, 95f, 89f}},
                                                                    { Character.CharacterRace.Orc, new [] {155f, 98f, 91f}},
                                                                    { Character.CharacterRace.Dwarf, new [] {154f, 97f, 92f,}},
                                                                    { Character.CharacterRace.NightElf, new [] {194f, 110f, 104f}},
                                                                    { Character.CharacterRace.Undead, new [] {156f, 94f, 90f}},
                                                                    { Character.CharacterRace.Tauren, new [] {0f, 0f, 0f}},
                                                                    { Character.CharacterRace.Gnome, new [] {161f, 90f, 88f}},
                                                                    { Character.CharacterRace.Troll, new [] {160f, 96f, 90f}},
                                                                    { Character.CharacterRace.BloodElf, new [] {160f, 92f, 87f}},
                                                                    { Character.CharacterRace.Draenei, new [] {0f, 0f, 0f}}
                                                                  };

        private static Stats GetRaceStats(Character.CharacterRace race)
        {
            if (race == Character.CharacterRace.Tauren || race == Character.CharacterRace.Draenei)
                return new Stats();

            var statsRace = new Stats
                                {
                                    Health = 7924f,
                                    Agility = BaseRogueRaceStats[race][0],
                                    Strength = BaseRogueRaceStats[race][1],
                                    Stamina = BaseRogueRaceStats[race][2],
                                    AttackPower = 140,
                                    PhysicalCrit = 3.73f,
                                    DodgeRating = (float) (-0.59*18.92f),
                                };

            statsRace.Health += statsRace.Stamina*10f;

            return statsRace;
        }
    }
}