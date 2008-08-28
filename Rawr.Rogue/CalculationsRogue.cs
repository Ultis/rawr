using System;
using System.Collections.Generic;

namespace Rawr.Rogue {
    [System.ComponentModel.DisplayName("Rogue|Ability_Rogue_SliceDice")]
    public class CalculationsRogue : CalculationsBase {
        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel {
            get {
                if (_calculationOptionsPanel == null) {
                    _calculationOptionsPanel = new CalculationOptionsPanelRogue();
                }
                return _calculationOptionsPanel;
            }
        }

        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels {
            get {
                if (_characterDisplayCalculationLabels == null) {
                    _characterDisplayCalculationLabels = new string[] {
                        "Base Stats:Health",
                        "Base Stats:Stamina",
                        "Base Stats:Strength",
                        "Base Stats:Agility",
                        "Base Stats:Attack Power",
                        "Base Stats:Hit",
                        "Base Stats:Expertise",
                        "Base Stats:Haste",
                        "Base Stats:Armor Penetration",
                        "Base Stats:Crit",
                        "Base Stats:Weapon Damage",

                        "DPS Breakdown:White DPS",
                        "DPS Breakdown:Sinister Strike DPS",
                        "DPS Breakdown:Rupture DPS",
                        "DPS Breakdown:Total DPS"
                    };
                }
                return _characterDisplayCalculationLabels;
            }
        }

        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors {
            get {
                if (_subPointNameColors == null) {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("DPS", System.Drawing.Color.Red);
                }
                return _subPointNameColors;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames {
            get {
                if (_customChartNames == null) {
                    _customChartNames = new string[] {
                        "Combat Table"
                    };
                }
                return _customChartNames;
            }
        }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes {
            get {
                if (_relevantItemTypes == null) {
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[] {
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
                return _relevantItemTypes;
            }
        }

        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Rogue; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationsRogue(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsRogue(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml) {
            System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRogue));
            System.IO.StringReader sr = new System.IO.StringReader(xml);
            CalculationOptionsRogue calcOpts = s.Deserialize(sr) as CalculationOptionsRogue;
            return calcOpts;
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem) {
            float chanceToMiss, chanceToBeDodged, chanceToGlance;
            float avgMHDmg, avgOHDmg, critChanceMH, critChanceOH;
            float whiteDPS;
            float hastedSpeedMH, hastedSpeedOH;
            float avgSSDmg;
            float energySS;
            float SSPerCycle;
            float fightLength, cycleEnergy, cycleLength;
            float ssDPS;
            float sndPoints, rupturePoints;
            float ruptureDmg, ruptureDPS;
            float effArmor, damageReduction;

            CalculationOptionsRogue calcOpts = character.CalculationOptions as CalculationOptionsRogue;
            Stats stats = GetCharacterStats(character, additionalItem);
            CharacterCalculationsRogue calculatedStats = new CharacterCalculationsRogue();
            calculatedStats.BasicStats = stats;

            switch(calcOpts.ImprovedSinisterStrike) {
                case 2:
                    energySS = 40f;
                    break;
                case 1:
                    energySS = 42f;
                    break;
                default:
                    energySS = 45f;
                    break;
            }

            // cycle stuff
            fightLength = 10f;
            // 4s5r
            SSPerCycle = 9f;
            sndPoints = 4f;
            rupturePoints = 5f;

            // 9 SS, 1snd, 1rupture
            cycleEnergy = SSPerCycle * energySS + 1f * 25f + 1f * 25f;
            cycleLength = cycleEnergy / 10f;
            if (cycleLength <= 0f)
                cycleLength = 1f;

            // combat table
            chanceToMiss = 28f;
            chanceToMiss -= (stats.Hit + stats.HitRating * RogueConversions.HitRatingToHit);
            if (chanceToMiss < 0f)
                chanceToMiss = 0f;

            chanceToBeDodged = 6.5f;
            chanceToBeDodged -= (stats.Expertise + stats.ExpertiseRating * RogueConversions.ExpertiseRatingToExpertise) * RogueConversions.ExpertiseToDodgeParryReduction;
            if (chanceToBeDodged < 0f)
                chanceToBeDodged = 0f;

            chanceToGlance = 25f;
            
            critChanceMH = stats.Crit + stats.CritRating / RogueConversions.CritRatingToCrit;
            critChanceOH = critChanceMH;

            effArmor = calcOpts.TargetArmor - stats.ArmorPenetration;
            if (effArmor < 0f)
                effArmor = 0f;
            damageReduction = 1 - effArmor / (effArmor + 10557.5f);

            #region White Damage
            whiteDPS = 0f;

            // MH
            if(character.MainHand != null) {
                avgMHDmg = (character.MainHand.MinDamage + character.MainHand.MaxDamage + stats.WeaponDamage * 2) / 2.0f;
                avgMHDmg += (stats.AttackPower / 14.0f) * character.MainHand.Speed;
                avgMHDmg *= damageReduction;

                hastedSpeedMH = (stats.HasteRating == 0) ? character.MainHand.Speed : character.MainHand.Speed / (1 + (stats.HasteRating + stats.DrumsOfBattle) / 1576f);

                if (character.MainHand.Type == Item.ItemType.FistWeapon && calcOpts.FistSpecialization > 0) {
                    critChanceMH += 5f;
                }

                whiteDPS += (avgMHDmg / hastedSpeedMH);
            }

            // OH
            if (character.OffHand != null) {
                avgOHDmg = (character.OffHand.MinDamage + character.OffHand.MaxDamage + stats.WeaponDamage * 2) / 2.0f;
                avgOHDmg += (stats.AttackPower / 14.0f) * character.OffHand.Speed;
                avgOHDmg *= (0.25f + calcOpts.DualWieldSpecialization * 0.1f);
                avgOHDmg *= damageReduction;

                hastedSpeedOH = (stats.HasteRating == 0) ? character.OffHand.Speed : character.OffHand.Speed / (1 + (stats.HasteRating + stats.DrumsOfBattle) / 1576f);

                if (character.OffHand.Type == Item.ItemType.FistWeapon && calcOpts.FistSpecialization > 0) {
                    critChanceOH += 5f;
                }

                whiteDPS += avgOHDmg / hastedSpeedOH;
            }
            #endregion

            #region Sinister Strike
            if (character.MainHand != null && SSPerCycle > 0f) {
                avgSSDmg = (character.MainHand.MinDamage + character.MainHand.MaxDamage + 2f * stats.WeaponDamage) / 2f;
                avgSSDmg += (stats.AttackPower / 14f) * 2.4f;
                avgSSDmg += 98f;
                avgSSDmg *= (1f + calcOpts.Aggression * 0.02f) * (1f + calcOpts.SurpriseAttacks * 0.1f);
                avgSSDmg *= damageReduction;
            }
            else {
                avgSSDmg = 0f;
            }

            ssDPS = avgSSDmg * SSPerCycle / cycleLength;
            #endregion

            #region Rupture
            if (rupturePoints > 0) {
                ruptureDmg = 1000 + 0.24f * stats.AttackPower;
                ruptureDPS = ruptureDmg / cycleLength;
            }
            else {
                ruptureDPS = 0f;
            }
            #endregion

            calculatedStats.WhiteDPS = whiteDPS;
            calculatedStats.SSDPS = ssDPS;
            calculatedStats.RuptureDPS = ruptureDPS;
            calculatedStats.DPSPoints = whiteDPS + ssDPS + ruptureDPS;
            calculatedStats.OverallPoints = calculatedStats.DPSPoints;
            return calculatedStats;
        }

        #region Rogue Racial Stats
        private static float[,] BaseRogueRaceStats = new float[,] 
		{
							//	Agility,	Strength,	Stamina
            /*Empty*/       {   0f,         0f,         0f,     },
			/*Human*/		{	158f,		95f,		89f,	},	
			/*Orc*/			{	155f,		98f,		91f,	},		
			/*Dwarf*/		{	154f,		97f,		92f,	},
			/*Night Elf*/	{	163f,		92f,		88f,	},		
			/*Undead*/		{	156f,		94f,		90f,	},	
			/*Tauren*/		{	0f,			0f,			0f,		},		
			/*Gnome*/		{	161f,		90f,		88f,	},		
			/*Troll*/		{	160f,		96f,		90f,	},	
			/*BloodElf*/	{	160f,		92f,		87f,	},
			/*Draenei*/		{	0f,			0f,			0f,		}
		};

        private Stats GetRaceStats(Character.CharacterRace race) {
            if (race == Character.CharacterRace.Tauren || race == Character.CharacterRace.Draenei)
                return new Stats();

            Stats statsRace = new Stats() {
                Health = 3524f,
                Agility = (float)BaseRogueRaceStats[(int)race, 0],
                Strength = (float)BaseRogueRaceStats[(int)race, 1],
                Stamina = (float)BaseRogueRaceStats[(int)race, 2],

                AttackPower = 120f,

                Crit = 3.73f,

                DodgeRating = (float)(-0.59 * 18.92f),
            };

            statsRace.Health += statsRace.Stamina * 10f;

            if (race == Character.CharacterRace.NightElf)
                statsRace.DodgeRating += 18.92f;

            return statsRace;
        }
        #endregion

        public override Stats GetCharacterStats(Character character, Item additionalItem) {
            CalculationOptionsRogue calcOpts = character.CalculationOptions as CalculationOptionsRogue;

            Stats statsRace;

            if (character.Race == Character.CharacterRace.Human) {
                statsRace = GetRaceStats(character.Race);
            }
            else {
                statsRace = GetRaceStats(character.Race);
            }
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            // buffs from DPSWarr
            //Add Expose Weakness since it's not listed as an AP buff
            if (statsBuffs.ExposeWeakness > 0) statsBuffs.AttackPower += 200f;

            //Mongoose
            if (character.MainHand != null && character.MainHandEnchant != null && character.MainHandEnchant.Id == 2673) {
                statsBuffs.Agility += 120f * ((40f * (1f / (60f / character.MainHand.Speed)) / 6f));
                statsBuffs.HasteRating += (15.76f * 2f) * ((40f * (1f / (60f / character.MainHand.Speed)) / 6f));
            }
            if (character.OffHand != null && character.OffHandEnchant != null && character.OffHandEnchant.Id == 2673) {
                statsBuffs.Agility += 120f * ((40f * (1f / (60f / character.OffHand.Speed)) / 6f));
                statsBuffs.HasteRating += (15.76f * 2f) * ((40f * (1f / (60f / character.OffHand.Speed)) / 6f));
            }

            //Executioner
            if (character.MainHand != null && character.MainHandEnchant != null && character.MainHandEnchant.Id == 3225) {
                statsBuffs.ArmorPenetration += 840f * ((40f * (1f / (60f / character.MainHand.Speed)) / 6f));
            }

            Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;

            TalentTree tree = character.Talents;

            float agiBase = (float)Math.Floor(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier));
            float agiBonus = (float)Math.Floor(statsGearEnchantsBuffs.Agility * (1 + statsRace.BonusAgilityMultiplier));
            float strBase = (float)Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float strBonus = (float)Math.Floor(statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float staBase = (float)Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier));
            float staBonus = (float)Math.Floor(statsGearEnchantsBuffs.Stamina * (1 + statsRace.BonusStaminaMultiplier));

            Stats statsTotal = new Stats();
            statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier) * (1 + calcOpts.Deadliness * 0.02f)) - 1;
            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier) * (1 + calcOpts.Vitality * 0.01f) * (1 + calcOpts.SinisterCalling * 0.03f)) - 1;
            statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)) - 1;
            statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier) * (1 + calcOpts.Vitality * 0.02f)) - 1;

            statsTotal.Agility = (float)Math.Floor(agiBase * (1f + statsTotal.BonusAgilityMultiplier)) + (float)Math.Floor(agiBonus * (1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Strength = (float)Math.Floor(strBase * (1f + statsTotal.BonusStrengthMultiplier)) + (float)Math.Floor(strBonus * (1 + statsTotal.BonusStrengthMultiplier));
            statsTotal.Stamina = (float)Math.Floor(staBase * (1f + statsTotal.BonusStaminaMultiplier)) + (float)Math.Floor(staBonus * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + ((statsTotal.Stamina - staBase) * 10f))));

            statsTotal.AttackPower = (statsTotal.Agility + statsTotal.Strength + statsRace.AttackPower) + statsGearEnchantsBuffs.AttackPower;
            //statsTotal.AttackPower = statsRace.AttackPower + ((statsTotal.Agility - agiBase) + (statsTotal.Strength - strBase) + statsGearEnchantsBuffs.AttackPower) * (1f + statsTotal.BonusAttackPowerMultiplier);
            //statsTotal.AttackPower = (float)Math.Floor((statsRace.AttackPower + statsGearEnchantsBuffs.AttackPower + ((statsTotal.Strength - strBase) * 1) + ((statsTotal.Agility - agiBase) * 1)) * (1f + statsTotal.BonusAttackPowerMultiplier));

            statsTotal.Hit = calcOpts.Precision;
            statsTotal.HitRating = statsGearEnchantsBuffs.HitRating;

            statsTotal.Expertise += calcOpts.WeaponExpertise * 5.0f;
            statsTotal.ExpertiseRating = statsGearEnchantsBuffs.ExpertiseRating;

            statsTotal.HasteRating = statsGearEnchantsBuffs.HasteRating;

            statsTotal.ArmorPenetration = statsGearEnchantsBuffs.ArmorPenetration;
            switch (calcOpts.SerratedBlades) {
                case 3:
                    statsTotal.ArmorPenetration += 560;
                    break;
                case 2:
                    statsTotal.ArmorPenetration += 373;
                    break;
                case 1:
                    statsTotal.ArmorPenetration += 186;
                    break;
            }

            statsTotal.CritRating = statsRace.CritRating + statsGearEnchantsBuffs.CritRating;
            statsTotal.Crit = statsRace.Crit;
            statsTotal.Crit += (statsTotal.Agility - (statsRace.Agility * (1f + statsTotal.BonusAgilityMultiplier))) * RogueConversions.AgilityToCrit;
            statsTotal.Crit += calcOpts.Malice * 1f;
            //statsTotal.CritRating += statsBuffs.LotPCritRating;

            statsTotal.Dodge += statsTotal.Agility * RogueConversions.AgilityToDodge;
            statsTotal.Dodge += calcOpts.LightningReflexes;

            statsTotal.Parry += calcOpts.Deflection;

            statsTotal.WeaponDamage = statsGearEnchantsBuffs.WeaponDamage;
            
            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName) {
            switch (chartName) {
                case "Combat Table":
                    CharacterCalculationsRogue currentCalculationsRogue = GetCharacterCalculations(character) as CharacterCalculationsRogue;
                    ComparisonCalculationsRogue calcMiss = new ComparisonCalculationsRogue();
                    ComparisonCalculationsRogue calcDodge = new ComparisonCalculationsRogue();
                    ComparisonCalculationsRogue calcParry = new ComparisonCalculationsRogue();
                    ComparisonCalculationsRogue calcBlock = new ComparisonCalculationsRogue();
                    ComparisonCalculationsRogue calcGlance = new ComparisonCalculationsRogue();
                    ComparisonCalculationsRogue calcCrit = new ComparisonCalculationsRogue();
                    ComparisonCalculationsRogue calcHit = new ComparisonCalculationsRogue();

                    if (currentCalculationsRogue != null) {
                        calcMiss.Name = "    Miss    ";
                        calcDodge.Name = "   Dodge   ";
                        calcGlance.Name = " Glance ";
                        calcCrit.Name = "  Crit  ";
                        calcHit.Name = "Hit";

                        float crits = 5f;
                        float glancing = 25f;
                        float hits = 100f - (crits + glancing);

                        calcMiss.OverallPoints = 0f;
                        calcDodge.OverallPoints = 0f;
                        calcParry.OverallPoints = 0f;
                        calcBlock.OverallPoints = 0f;
                        calcGlance.OverallPoints = 0f;
                        calcCrit.OverallPoints = 0f;
                        calcHit.OverallPoints = 0f;
                    }
                    return new ComparisonCalculationBase[] { calcMiss, calcDodge, calcParry, calcGlance, calcBlock, calcCrit, calcHit };

                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        public override Stats GetRelevantStats(Stats stats) {
            return new Stats() {
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
                ExecutionerProc = stats.ExecutionerProc
            };
        }

        public override bool HasRelevantStats(Stats stats) {
            return (stats.Agility + stats.Strength + stats.BonusAgilityMultiplier + stats.BonusStrengthMultiplier + stats.AttackPower + stats.BonusAttackPowerMultiplier + stats.Stamina + stats.BonusStaminaMultiplier + stats.Health + stats.CritRating + stats.HitRating + stats.HasteRating + stats.ExpertiseRating + stats.ArmorPenetration + stats.WeaponDamage + stats.BonusCritMultiplier + stats.WindfuryAPBonus + stats.MongooseProc + stats.MongooseProcAverage + stats.MongooseProcConstant + stats.ExecutionerProc + stats.BonusCommandingShoutHP) != 0;
        }
    }

    public class RogueConversions {
        public static readonly float StrengthToAP = 1.0f;
        public static readonly float AgilityToAP = 1.0f;
        public static readonly float AgilityToCrit = 1.0f / 40.0f;
        public static readonly float AgilityToDodge = 1.0f / 20.0f;
        public static readonly float AgilityToArmor = 2.0f;
        public static readonly float StaminaToHP = 10.0f;
        public static readonly float HitRatingToHit = 1.0f / 15.7692f;
        public static readonly float CritRatingToCrit = 1.0f / 22.0769f; //14*82/52
        public static readonly float CritToCritRating = 22.0769f; //14*82/52
        public static readonly float HasteRatingToHaste = 1.0f / 15.77f;
        public static readonly float ExpertiseRatingToExpertise = 1.0f / 3.9423f;
        public static readonly float ExpertiseToDodgeParryReduction = 0.25f;
        public static readonly float ParryRatingToParry = 1.0f / 23.6538461538462f;
    }
}