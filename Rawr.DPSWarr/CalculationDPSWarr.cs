using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace Rawr.DPSWarr
{
    [Rawr.Calculations.RawrModelInfo("FuryWarr", "Ability_Rogue_Ambush", Character.CharacterClass.Warrior)]
    public class CalculationsDPSWarr : CalculationsBase
    {
        public CalculationsDPSWarr()
        {
            SetupRelevantItemTypes();
        }
        
        private readonly CalculationOptionsPanelBase _calculationOptionsPanel = new CalculationOptionsPanelDPSWarr();
        private readonly string[] _customChartNames = new[] {"Item Budget"};
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
            get { return Character.CharacterClass.Warrior; }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationsDPSWarr();
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsDPSWarr();
        }

        public override bool ItemFitsInSlot(Item item, Character character, Character.CharacterSlot slot)
        {
            if (item == null || character == null || slot == null)
                return false;
            if (slot == Character.CharacterSlot.OffHand && item.Slot == Item.ItemSlot.TwoHand && character.WarriorTalents.TitansGrip == 1)
                return true;
            if (character.WarriorTalents.TitansGrip > 0 && slot == Character.CharacterSlot.MainHand && item.Type == Item.ItemType.Polearm)
                return false;
            if (slot == Character.CharacterSlot.OffHand && character.MainHand.Slot == Item.ItemSlot.TwoHand)
                return false;
            return base.ItemFitsInSlot(item, character, slot);
        }

        public override bool IncludeOffHandInCalculations(Character character)
        {
            if (character.OffHand == null)
                return false;
            if (character.CurrentTalents is WarriorTalents)
            {
                WarriorTalents talents = (WarriorTalents)character.CurrentTalents;
                if (talents.TitansGrip > 0)
                    return true;
                else// if (character.MainHand.Slot != Item.ItemSlot.TwoHand)
                    return base.IncludeOffHandInCalculations(character);
                //else
                //    return false;
            }
            return false;
        }

        public override bool EnchantFitsInSlot(Enchant enchant, Character character, Item.ItemSlot slot)
        {
            return enchant.FitsInSlot(slot) || (character.WarriorTalents.TitansGrip == 1 && enchant.Slot == Item.ItemSlot.TwoHand && slot == Item.ItemSlot.OffHand);
        }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            var s = new XmlSerializer(typeof (CalculationOptionsDPSWarr));
            var sr = new StringReader(xml);
            var calcOpts = s.Deserialize(sr) as CalculationOptionsDPSWarr;
            return calcOpts;
        }

        /// <summary>
        /// Calculate damage output
        /// </summary>
        /// <param name="character"></param>
        /// <param name="additionalItem"></param>
        /// <returns></returns>
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            var stats = GetCharacterStats(character, additionalItem);
            var calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
            var combatFactors = new CombatFactors(character, stats);
            return GetCalculations(combatFactors, stats, calcOpts, character.WarriorTalents);
        }

        private static CharacterCalculationsBase GetCalculations(CombatFactors combatFactors, Stats stats, CalculationOptionsDPSWarr calcOpts, WarriorTalents talents)
        {
            var calculatedStats = new CharacterCalculationsDPSWarr(stats);
            var whiteAttacks = new WhiteAttacks(talents, stats, combatFactors);
            var skillAttacks = new Skills(talents, stats, combatFactors, whiteAttacks);

            //calculatedStats.AddDisplayValue(DisplayValue.CPG, cpg.Name);
            calculatedStats.AddRoundedDisplayValue(DisplayValue.Haste, stats.HasteRating);
            calculatedStats.AddRoundedDisplayValue(DisplayValue.ArmorPenetration, stats.ArmorPenetrationRating);
            calculatedStats.AddRoundedDisplayValue(DisplayValue.HitPercent, stats.HitRating);
            calculatedStats.AddRoundedDisplayValue(DisplayValue.CritPercent, stats.CritRating);
            calculatedStats.AddRoundedDisplayValue(DisplayValue.MhExpertise, (int)combatFactors.MhExpertise);
            calculatedStats.AddRoundedDisplayValue(DisplayValue.OhExpertise, (int)combatFactors.OhExpertise);
            calculatedStats.AddRoundedDisplayValue(DisplayValue.Armor, (int)stats.Armor);

            calculatedStats.AddRoundedDisplayValue(DisplayValue.buffedMhCrit, combatFactors.MhCrit*100);
            calculatedStats.AddRoundedDisplayValue(DisplayValue.buffedOhCrit, combatFactors.OhCrit*100);
            calculatedStats.AddRoundedDisplayValue(DisplayValue.buffedArmorPenetration, combatFactors.EffectiveBossArmor);
            calculatedStats.AddRoundedDisplayValue(DisplayValue.damageReduc, combatFactors.DamageReduction);

            calculatedStats.AddRoundedDisplayValue(DisplayValue.WhiteRage, whiteAttacks.whiteRageGen());
            calculatedStats.AddRoundedDisplayValue(DisplayValue.OtherRage, skillAttacks.OtherRage());
            calculatedStats.AddRoundedDisplayValue(DisplayValue.FreeRage, skillAttacks.freeRage());
            
            //Arms
            calculatedStats.AddRoundedDisplayValue(DisplayValue.MortalStrike, skillAttacks.MortalStrike());
            calculatedStats.AddRoundedDisplayValue(DisplayValue.SuddenDeath, skillAttacks.SuddenDeath());
            calculatedStats.AddRoundedDisplayValue(DisplayValue.Slam, skillAttacks.Slam());
            calculatedStats.AddRoundedDisplayValue(DisplayValue.Overpower, skillAttacks.Overpower());
            calculatedStats.AddRoundedDisplayValue(DisplayValue.Rend, skillAttacks.Rend());
            calculatedStats.AddRoundedDisplayValue(DisplayValue.SwordSpec, skillAttacks.SwordSpec());
            calculatedStats.AddRoundedDisplayValue(DisplayValue.Bladestorm, skillAttacks.BladeStorm());
            //Fury
            calculatedStats.AddRoundedDisplayValue(DisplayValue.Bloodthirst, skillAttacks.Bloodthirst());
            calculatedStats.AddRoundedDisplayValue(DisplayValue.Whirlwind, skillAttacks.Whirlwind());
            calculatedStats.AddRoundedDisplayValue(DisplayValue.HeroicStrike, skillAttacks.HeroicStrike());
            calculatedStats.AddRoundedDisplayValue(DisplayValue.Bloodsurge, skillAttacks.Bloodsurge());
            calculatedStats.AddRoundedDisplayValue(DisplayValue.DeepWounds, skillAttacks.Deepwounds());
            
            //Generic
            calculatedStats.AddRoundedDisplayValue(DisplayValue.WhiteDPS, whiteAttacks.CalcMhWhiteDPS() + whiteAttacks.CalcOhWhiteDPS());// + swordSpecDPS);
            calculatedStats.AddRoundedDisplayValue(DisplayValue.DeepWounds, skillAttacks.Deepwounds());

            calculatedStats.TotalDPS = whiteAttacks.CalcMhWhiteDPS() + whiteAttacks.CalcOhWhiteDPS() + skillAttacks.Bloodthirst() + skillAttacks.Whirlwind() +
                                       skillAttacks.HeroicStrike() + skillAttacks.Bloodsurge() + skillAttacks.Deepwounds()+
                                       skillAttacks.MortalStrike() + skillAttacks.SuddenDeath() + skillAttacks.Slam() + skillAttacks.Overpower() + skillAttacks.Rend() + skillAttacks.SwordSpec() + skillAttacks.BladeStorm();
            calculatedStats.OverallPoints = calculatedStats.TotalDPS;

            return calculatedStats;
        }

        public Stats GetBuffsStats(Character character)
        {
            var statsBuffs = GetBuffsStats(character.ActiveBuffs);

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

            //Executioner
            if (character.MainHand != null && character.MainHandEnchant != null && character.MainHandEnchant.Id == 3225)
            {
                statsBuffs.ArmorPenetration += 840f * ((40f * (1f / (60f / character.MainHand.Speed)) / 6f));
            }

            return statsBuffs;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;

            var agiBase = (float) Math.Floor(statsRace.Agility*(1 + statsRace.BonusAgilityMultiplier));
            var agiBonus = (float) Math.Floor(statsGearEnchantsBuffs.Agility*(1 + statsRace.BonusAgilityMultiplier));
            var strBase = (float) Math.Floor(statsRace.Strength*(1 + statsRace.BonusStrengthMultiplier));
            var strBonus = (float) Math.Floor(statsGearEnchantsBuffs.Strength*(1 + statsRace.BonusStrengthMultiplier));
            var staBase = (float) Math.Floor(statsRace.Stamina*(1 + statsRace.BonusStaminaMultiplier));
            var staBonus = (float) Math.Floor(statsGearEnchantsBuffs.Stamina*(1 + statsRace.BonusStaminaMultiplier));

            var statsTotal = new Stats();
            statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier)*(1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier)*(1 + character.WarriorTalents.ImprovedBerserkerStance*0.02f)) - 1;
            statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier)*(1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)*(1+0.02f*character.WarriorTalents.StrengthOfArms)) - 1;
            statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier)*(1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;
            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;
            
            statsTotal.Agility = (float) Math.Floor(agiBase*(1f + statsTotal.BonusAgilityMultiplier)) + (float) Math.Floor(agiBonus*(1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Strength = (float) Math.Floor(strBase*(1f + statsTotal.BonusStrengthMultiplier)) + (float) Math.Floor(strBonus*(1 + statsTotal.BonusStrengthMultiplier));
            statsTotal.Stamina = (float) Math.Floor(staBase*(1f + statsTotal.BonusStaminaMultiplier)) + (float) Math.Floor(staBonus*(1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Health = (float) Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + ((statsTotal.Stamina - staBase)*10f))));

            statsTotal.Armor = statsGearEnchantsBuffs.Armor + statsTotal.Agility * 2;

            statsTotal.AttackPower = (statsTotal.Strength*2 + statsRace.AttackPower) + statsGearEnchantsBuffs.AttackPower;
            statsTotal.AttackPower += (statsTotal.Armor / 180) * character.WarriorTalents.ArmoredToTheTeeth;
            statsTotal.AttackPower += statsTotal.AttackPower * statsTotal.BonusAttackPowerMultiplier;
            
            statsTotal.PhysicalHit = character.WarriorTalents.Precision;
            statsTotal.HitRating = statsGearEnchantsBuffs.HitRating;

            statsTotal.ExpertiseRating = statsGearEnchantsBuffs.ExpertiseRating;
            statsTotal.Expertise += 2 * character.WarriorTalents.StrengthOfArms;
            statsTotal.HasteRating = statsGearEnchantsBuffs.HasteRating;
            statsTotal.PhysicalHaste = statsGearEnchantsBuffs.PhysicalHaste;
            statsTotal.PhysicalHaste *= (1 + 0.03f * character.WarriorTalents.BloodFrenzy);

            statsTotal.ArmorPenetration = statsGearEnchantsBuffs.ArmorPenetration;
            statsTotal.ArmorPenetrationRating = statsGearEnchantsBuffs.ArmorPenetrationRating;

            statsTotal.CritRating = statsRace.CritRating + statsGearEnchantsBuffs.CritRating;
            statsTotal.PhysicalCrit = statsRace.PhysicalCrit;
            statsTotal.PhysicalCrit += statsTotal.Agility*WarriorConversions.AgilityToCrit/100;
            statsTotal.PhysicalCrit += character.WarriorTalents.Cruelty*0.01f;
            statsTotal.PhysicalCrit += statsGearEnchantsBuffs.PhysicalCrit;

            statsTotal.BonusCritMultiplier = statsGearEnchantsBuffs.BonusCritMultiplier;

            statsTotal.BonusDamageMultiplier = statsGearEnchantsBuffs.BonusDamageMultiplier;
            statsTotal.BonusPhysicalDamageMultiplier = statsGearEnchantsBuffs.BonusPhysicalDamageMultiplier;

            statsTotal.WeaponDamage = statsGearEnchantsBuffs.WeaponDamage;

            statsTotal.BonusBleedDamageMultiplier = statsGearEnchantsBuffs.BonusBleedDamageMultiplier;
            statsTotal.BonusSlamDamage = statsGearEnchantsBuffs.BonusSlamDamage;
            statsTotal.DreadnaughtBonusRageProc = statsGearEnchantsBuffs.DreadnaughtBonusRageProc;

            return statsTotal;
        }

        /*public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            switch (chartName)
            {
                case "Combat Table":
                    var currentCalculationsRogue = GetCharacterCalculations(character) as CharacterCalculationsDPSWarr;
                    var calcMiss = new ComparisonCalculationsDPSWarr();
                    var calcDodge = new ComparisonCalculationsDPSWarr();
                    var calcParry = new ComparisonCalculationsDPSWarr();
                    var calcBlock = new ComparisonCalculationsDPSWarr();
                    var calcGlance = new ComparisonCalculationsDPSWarr();
                    var calcCrit = new ComparisonCalculationsDPSWarr();
                    var calcHit = new ComparisonCalculationsDPSWarr();

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
        }*/
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            CharacterCalculationsDPSWarr baseCalc, calc;
            ComparisonCalculationBase comparison;
            float[] subPoints;

            switch (chartName)
            {
                case "Item Budget":
                    Item[] itemList = new Item[] {
                        new Item() { Stats = new Stats() { Strength = 10 } },
                        new Item() { Stats = new Stats() { Agility = 10 } },
                        new Item() { Stats = new Stats() { AttackPower = 20 } },
                        new Item() { Stats = new Stats() { CritRating = 10 } },
                        new Item() { Stats = new Stats() { HitRating = 10 } },
                        new Item() { Stats = new Stats() { ExpertiseRating = 8.19f } },
                        new Item() { Stats = new Stats() { HasteRating = 10 } },
                        new Item() { Stats = new Stats() { ArmorPenetrationRating = 10 } },
                    };
                    string[] statList = new string[] {
                        "Strength",
                        "Agility",
                        "Attack Power",
                        "Crit Rating",
                        "Hit Rating",
                        "Expertise Rating",
                        "Haste Rating",
                        "Armor Penetration Rating",
                    };

                    baseCalc = GetCharacterCalculations(character) as CharacterCalculationsDPSWarr;

                    float mod = 1;
                    for (int index = 0; index < itemList.Length; index++)
                    {
                        calc = GetCharacterCalculations(character, itemList[index]) as CharacterCalculationsDPSWarr;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = statList[index];
                        comparison.Equipped = false;
                        comparison.OverallPoints = (calc.OverallPoints - baseCalc.OverallPoints)/10;
                        subPoints = new float[calc.SubPoints.Length];
                        for (int i = 0; i < calc.SubPoints.Length; i++)
                        {
                            if (comparison.Name == "Strength")
                            {
                                subPoints[i] = 1;
                            }
                            else
                            {
                                subPoints[i] = calc.SubPoints[i] - baseCalc.SubPoints[i];
                                subPoints[i] /= 10;
                                subPoints[i] /= mod;
                            }
                        }
                        comparison.SubPoints = subPoints;
                        if (comparison.Name == "Strength")
                        {
                            mod = comparison.OverallPoints;
                            comparison.OverallPoints = 1;
                        }
                        else if (comparison.Name == "Expertise Rating")
                        {
                            comparison.OverallPoints /= mod;
                            comparison.OverallPoints /= 8.197f;
                        }
                        else
                        {
                            comparison.OverallPoints /= mod;
                        }

                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
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
                           //WindfuryAPBonus = stats.WindfuryAPBonus,
                           MongooseProc = stats.MongooseProc,
                           MongooseProcAverage = stats.MongooseProcAverage,
                           MongooseProcConstant = stats.MongooseProcConstant,
                           ExecutionerProc = stats.ExecutionerProc,
                           BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
                           Armor = stats.Armor,
                           PhysicalCrit = stats.PhysicalCrit,
                           PhysicalHaste = stats.PhysicalHaste,
                           BonusDamageMultiplier = stats.BonusDamageMultiplier,
                           BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                           BonusSlamDamage = stats.BonusSlamDamage,
                           DreadnaughtBonusRageProc = stats.DreadnaughtBonusRageProc,
                           BerserkingProc = stats.BerserkingProc
                       };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return
            (
                stats.Agility + 
                stats.Strength + 
                stats.BonusAgilityMultiplier + 
                stats.BonusStrengthMultiplier + 
                stats.AttackPower + 
                stats.BonusAttackPowerMultiplier + 
                stats.CritRating + stats.HitRating + 
                stats.HasteRating + 
                stats.ExpertiseRating + 
                stats.ArmorPenetration + 
                stats.WeaponDamage + 
                stats.BonusCritMultiplier + 
                stats.BonusDamageMultiplier + 
                stats.MongooseProc + 
                stats.MongooseProcAverage + 
                stats.MongooseProcConstant + 
                stats.ExecutionerProc + 
                stats.BonusBleedDamageMultiplier + 
                stats.BonusCritChance + 
                stats.Armor + 
                stats.PhysicalHaste + 
                stats.PhysicalCrit +
                stats.BonusPhysicalDamageMultiplier +
                stats.DreadnaughtBonusRageProc +
                stats.BonusSlamDamage +
                stats.BerserkingProc) != 0;
        }



        private void SetupRelevantItemTypes()
        {
            _relevantItemTypes = new List<Item.ItemType>(new[]
                                                             {
                                                                 Item.ItemType.None,
                                                                 Item.ItemType.Leather,
                                                                 Item.ItemType.Plate,
                                                                 Item.ItemType.Bow,
                                                                 Item.ItemType.Crossbow,
                                                                 Item.ItemType.Gun,
                                                                 Item.ItemType.Thrown,
                                                                 Item.ItemType.Dagger,
                                                                 Item.ItemType.FistWeapon,
                                                                 Item.ItemType.OneHandMace,
                                                                 Item.ItemType.OneHandSword,
                                                                 Item.ItemType.OneHandAxe,
                                                                 Item.ItemType.TwoHandMace,
                                                                 Item.ItemType.TwoHandSword,
                                                                 Item.ItemType.TwoHandAxe
                                                             });
        }

        #region Warrior Race Stats
        private static float[,] BaseWarriorRaceStats = new float[,] 
		{
							//	Strength,	Agility,	Stamina
            /*Human*/		{	174f,	    113f,	    159f,   },
            /*Orc*/			{	178f,		110f,		162f,	},
            /*Dwarf*/		{	176f,	    109f,	    162f,   },
			/*Night Elf*/	{	142f,	    101f,	    132f,   },
	        /*Undead*/		{	174f,	    111f,	    160f,   },
			/*Tauren*/		{	180f,		108f,		162f,	},
	        /*Gnome*/		{	170f,	    116f,	    159f,   },
			/*Troll*/		{	175f,	    115f,	    160f,   },	
			/*BloodElf*/	{	0f,		    0f,		    0f,	    },
			/*Draenei*/		{	176f,		110f,		159f,	},
		};

        private Stats GetRaceStats(Character character)
        {
            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.Human:
                    statsRace = new Stats()
                    {
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[0, 0],
                        Agility = (float)BaseWarriorRaceStats[0, 1],
                        Stamina = (float)BaseWarriorRaceStats[0, 2],

                        AttackPower = 220f,
                        Dodge = 3.4636f,
                        Miss = 0.05f,
                        Parry = 5f,
                        PhysicalCrit = 0.03186f,
                    };
                    if ((character.MainHand != null) &&
                        ((character.MainHand.Type == Item.ItemType.OneHandSword) ||
                         (character.MainHand.Type == Item.ItemType.OneHandMace)))
                    {
                        statsRace.Expertise += 5f;
                    }
                    break;
                case Character.CharacterRace.Orc:
                    statsRace = new Stats()
                    {
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[1, 0],
                        Agility = (float)BaseWarriorRaceStats[1, 1],
                        Stamina = (float)BaseWarriorRaceStats[1, 2],

                        AttackPower = 220f,
                        Dodge = 3.4636f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.03186f,
                    };

                    if ((character.MainHand != null) &&
                        (character.MainHand.Type == Item.ItemType.OneHandAxe))
                    {
                        statsRace.Expertise += 5f;
                    }
                    break;
                case Character.CharacterRace.Dwarf:
                    statsRace = new Stats()
                    {
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[2, 0],
                        Agility = (float)BaseWarriorRaceStats[2, 1],
                        Stamina = (float)BaseWarriorRaceStats[2, 2],

                        AttackPower = 220f,
                        Dodge = 3.4636f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.03186f,
                    };
                    break;
                case Character.CharacterRace.NightElf:
                    statsRace = new Stats()
                    {
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[3, 0],
                        Agility = (float)BaseWarriorRaceStats[3, 1],
                        Stamina = (float)BaseWarriorRaceStats[3, 2],

                        AttackPower = 220f,
                        Dodge = 3.4636f,
                        Miss = 0.05f + 0.02f,
                        Parry = 5f,
                        PhysicalCrit = 0.03186f,
                    };
                    break;
                case Character.CharacterRace.Undead:
                    statsRace = new Stats()
                    {
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[4, 0],
                        Agility = (float)BaseWarriorRaceStats[4, 1],
                        Stamina = (float)BaseWarriorRaceStats[4, 2],

                        AttackPower = 220f,
                        Dodge = 3.4636f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.03186f,
                    };
                    break;
                case Character.CharacterRace.Tauren:
                    statsRace = new Stats()
                    {
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[5, 0],
                        Agility = (float)BaseWarriorRaceStats[5, 1],
                        Stamina = (float)BaseWarriorRaceStats[5, 2],

                        AttackPower = 220f,
                        BonusHealthMultiplier = 0.05f,
                        Dodge = 3.4636f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.03186f,
                    };
                    break;
                case Character.CharacterRace.Gnome:
                    statsRace = new Stats()
                    {
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[6, 0],
                        Agility = (float)BaseWarriorRaceStats[6, 1],
                        Stamina = (float)BaseWarriorRaceStats[6, 2],

                        AttackPower = 220f,
                        Dodge = 3.4636f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.03186f,
                    };
                    break;
                case Character.CharacterRace.Troll:
                    statsRace = new Stats()
                    {
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[7, 0],
                        Agility = (float)BaseWarriorRaceStats[7, 1],
                        Stamina = (float)BaseWarriorRaceStats[7, 2],

                        AttackPower = 220f,
                        Dodge = 3.4636f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.03186f,
                    };
                    break;
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats()
                    {
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[9, 0],
                        Agility = (float)BaseWarriorRaceStats[9, 1],
                        Stamina = (float)BaseWarriorRaceStats[9, 2],

                        AttackPower = 220f,
                        PhysicalHit = 1f,
                        Dodge = 3.4636f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.03186f,
                    };
                    break;
                default:
                    statsRace = new Stats();
                    break;
            }

            return statsRace;
        }
        #endregion
    }
}