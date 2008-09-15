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
                        "DPS Breakdown:CPG DPS",
                        "DPS Breakdown:Finisher DPS",
                        "DPS Breakdown:Sword Spec DPS",
                        "DPS Breakdown:Windfury DPS",
                        "DPS Breakdown:Poison DPS",
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

        /// <summary>
        /// Calculate damage output
        /// </summary>
        /// <param name="character"></param>
        /// <param name="additionalItem"></param>
        /// <returns></returns>
        /// Much of this code is based on Aldriana's RogueCalc
        /// 
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem) {
            float energyCPG, numCPG, sndLength, finisherCost, sndEnergy, sndHaste, cycleTime, energyRegen, ruthlessnessCP;
            float totalHaste;
            string cpg;
            float missChance, mhDodgeChance, ohDodgeChance, glanceChance, mhExpertise, ohExpertise, mhCrit, ohCrit, probMHHit, probOHHit;
            float mhHastedSpeed, ohHastedSpeed, avgMHDmg, avgOHDmg, totalArmor;
            float mhAttacks, ohAttacks, ohHits, ssHits, wfHits, avgWFDmg;
            float whiteDPS, finisherDPS, wfDPS, ssDPS, poisonDPS, cpgDPS;
            float mhWhite, ohWhite, damageReduction, bonusWhiteCritDmg;
            float avgCPGDmg, bonusCPGCrit, bonusCPGDmgMult, bonusCPGCritDmgMult, cpgCrit;
            float finisherDmg, evisMod, evisMin, evisMax;
            float probPoison;
            bool calcDeadly;

            CalculationOptionsRogue calcOpts = character.CalculationOptions as CalculationOptionsRogue;
            Stats stats = GetCharacterStats(character, additionalItem);
            CharacterCalculationsRogue calculatedStats = new CharacterCalculationsRogue();
            calculatedStats.BasicStats = stats;

            whiteDPS = finisherDPS = wfDPS = ssDPS = poisonDPS = cpgDPS = 0f;

            missChance = 28f;
            missChance -= calcOpts.Precision + stats.Hit + stats.HitRating * RogueConversions.HitRatingToHit;
            if (missChance < 0f) missChance = 0f;

            mhExpertise = ohExpertise = calcOpts.WeaponExpertise * 5f + stats.Expertise + stats.ExpertiseRating * RogueConversions.ExpertiseRatingToExpertise;

            if (character.Race == Character.CharacterRace.Human) {
                if (character.MainHand != null && (character.MainHand.Type == Item.ItemType.OneHandSword || character.MainHand.Type == Item.ItemType.OneHandMace))
                    mhExpertise += 5f;
                if (character.OffHand != null && (character.OffHand.Type == Item.ItemType.OneHandSword || character.OffHand.Type == Item.ItemType.OneHandMace))
                    ohExpertise += 5f;
            }

            mhDodgeChance = 6.5f;
            mhDodgeChance -= .25f * mhExpertise;

            ohDodgeChance = 6.5f;
            ohDodgeChance -= .25f * ohExpertise;

            if (mhDodgeChance < 0f)
                mhDodgeChance = 0f;
            if (ohDodgeChance < 0f)
                ohDodgeChance = 0f;

            probMHHit = 1f - missChance / 100f - mhDodgeChance / 100f;
            probOHHit = 1f - missChance / 100f - ohDodgeChance / 100f;

            glanceChance = .25f;

            mhCrit = ohCrit = stats.Crit + stats.CritRating * RogueConversions.CritRatingToCrit;
            if (character.MainHand != null && character.MainHand.Type == Item.ItemType.Dagger)
                mhCrit += calcOpts.DaggerSpecialization;
            if (character.OffHand != null && character.OffHand.Type == Item.ItemType.Dagger)
                ohCrit += calcOpts.DaggerSpecialization;
            if (character.MainHand != null && character.MainHand.Type == Item.ItemType.FistWeapon)
                mhCrit += calcOpts.FistSpecialization;
            if (character.OffHand != null && character.OffHand.Type == Item.ItemType.FistWeapon)
                ohCrit += calcOpts.FistSpecialization;

            // if we have mutilate and we're using two daggers, assume we use it to generate CPs
            if (calcOpts.Mutilate > 0 &&
                character.MainHand != null && character.MainHand.Type == Item.ItemType.Dagger &&
                character.OffHand != null && character.OffHand.Type == Item.ItemType.Dagger) {
                cpg = "mutilate";
                energyCPG = 60f;
            }
            // if we're main handing a dagger, assume we're using backstab it to generate CPs
            else if (character.MainHand != null && character.MainHand.Type == Item.ItemType.Dagger) {
                cpg = "backstab";
                energyCPG = 60f;
            }
            // if we have hemo, assume we use it to generate CPs
            else if (calcOpts.Hemorrhage > 0) {
                cpg = "hemo";
                energyCPG = 35f;
            }
            // otherwise use sinister strike
            else {
                cpg = "ss";
                switch (calcOpts.ImprovedSinisterStrike) {
                    case 2:
                        energyCPG = 40f;
                        break;
                    case 1:
                        energyCPG = 42f;
                        break;
                    default:
                        energyCPG = 45f;
                        break;
                }
            }

            // cycle stuff
            sndLength = 6f + 3f * calcOpts.DPSCycle['s'];
            sndLength += stats.NetherbladeBonusSnDDuration;
            sndLength *= 1f + 0.15f * calcOpts.ImprovedSliceandDice;

            ruthlessnessCP = .2f * calcOpts.Ruthlessness;

            numCPG = calcOpts.DPSCycle.TotalComboPoints - 2f * ruthlessnessCP;

            if (calcOpts.DPSCycle['r'] > 0) {
                finisherCost = 25f;
            }
            else if (calcOpts.DPSCycle['e'] > 0) {
                finisherCost = 35f;
            }
            else {
                finisherCost = 0f;
            }

            energyRegen = 10f;
            if (calcOpts.AdrenalineRush > 0)
                energyRegen += .5f;

            sndEnergy = (calcOpts.DPSCycle['s'] - ruthlessnessCP) * energyCPG + 25f;
            sndHaste = .3f;
            sndHaste *= (1f + stats.SlayerBonusSnDHaste);

            totalArmor = calcOpts.TargetArmor - stats.ArmorPenetration;
            damageReduction = 1f - (totalArmor / (totalArmor + 10557.5f));

            #region White Damage
            whiteDPS = mhWhite = ohWhite = 0f;

            totalHaste = 1f;
            totalHaste *= (1f + sndHaste) * (1f + (stats.HasteRating * RogueConversions.HasteRatingToHaste) / 100);
            totalHaste *= (1f + .2f * 15f / 120f * calcOpts.BladeFlurry);

            bonusWhiteCritDmg = 1f + stats.BonusCritMultiplier;

            // MH
            mhAttacks = 0f;
            avgMHDmg = 0f;
            ohHits = 0f;
            if(character.MainHand != null) {
                avgMHDmg = (character.MainHand.MinDamage + character.MainHand.MaxDamage + stats.WeaponDamage * 2) / 2.0f;
                avgMHDmg += (stats.AttackPower / 14.0f) * character.MainHand.Speed;

                //mhHastedSpeed = character.MainHand.Speed / totalHaste;
                mhAttacks = totalHaste / character.MainHand.Speed;

                mhWhite = avgMHDmg * mhAttacks * probMHHit;

                mhWhite = (1f - mhCrit / 100f) * mhWhite + (mhCrit / 100f) * (mhWhite * (2f * bonusWhiteCritDmg));
                mhWhite *= damageReduction;
            }

            // OH
            ohAttacks = 0f;
            if (character.OffHand != null) {
                avgOHDmg = (character.OffHand.MinDamage + character.OffHand.MaxDamage + stats.WeaponDamage * 2) / 2.0f;
                avgOHDmg += (stats.AttackPower / 14.0f) * character.OffHand.Speed;
                avgOHDmg *= (0.25f + calcOpts.DualWieldSpecialization * 0.1f);

                ohAttacks = totalHaste / character.OffHand.Speed;
                ohHits = ohAttacks * probOHHit;

                energyRegen += (.2f * 3f * calcOpts.CombatPotency) * ohHits;

                ohWhite = avgOHDmg * ohHits;
                ohWhite = (1f - ohCrit / 100f) * ohWhite + (ohCrit / 100f) * (ohWhite * (2f * bonusWhiteCritDmg));
                ohWhite *= damageReduction;
            }

            cycleTime = (numCPG * energyCPG + 25f + finisherCost) / energyRegen;

            #region CPG Damage
            cpgDPS = 0f;
            if (character.MainHand != null) {
                avgCPGDmg = 0f;
                cpgCrit = 0f;
                bonusCPGCrit = 0f;
                bonusCPGDmgMult = 1f;
                bonusCPGCritDmgMult = 2f;

                if (cpg == "mutilate" && character.OffHand != null) {
                    bonusCPGCrit += 5f * calcOpts.PuncturingWounds;
                    bonusCPGCritDmgMult *= (1f + .06f * calcOpts.Lethality);
                    bonusCPGDmgMult *= (1f + 0.04f * calcOpts.Opportunity);

                    avgCPGDmg = (character.MainHand.MinDamage + character.MainHand.MaxDamage) / 2f + 121.5f;
                    avgCPGDmg += stats.AttackPower / 14f * 1.7f;
                    avgCPGDmg += (character.OffHand.MinDamage + character.OffHand.MaxDamage) / 2f + 121.5f;
                    avgCPGDmg += stats.AttackPower / 14f * 1.7f;
                    avgCPGDmg *= 1.5f;
                }
                else if (cpg == "backstab") {
                    bonusCPGDmgMult *= (1f + .02f * calcOpts.Aggression);
                    bonusCPGDmgMult *= (1f + .1f * calcOpts.SurpriseAttacks);
                    bonusCPGDmgMult *= (1f + 0.04f * calcOpts.Opportunity);
                    bonusCPGCrit += 10f * calcOpts.PuncturingWounds;
                    bonusCPGCritDmgMult *= (1f + .06f * calcOpts.Lethality);

                    avgCPGDmg = (character.MainHand.MinDamage + character.MainHand.MaxDamage + stats.WeaponDamage) / 2f;
                    avgCPGDmg += stats.AttackPower / 14f * 1.7f;
                    avgCPGDmg *= 1.5f;
                    avgCPGDmg += 255f;
                }
                else if (cpg == "hemo") {
                    bonusCPGDmgMult *= (1f + .1f * calcOpts.SurpriseAttacks);
                    bonusCPGDmgMult *= (1f + stats.SlayerBonusCPGDamage);
                    bonusCPGCritDmgMult *= (1f + .06f * calcOpts.Lethality);

                    avgCPGDmg = (character.MainHand.MinDamage + character.MainHand.MaxDamage + stats.WeaponDamage) / 2f;
                    avgCPGDmg += stats.AttackPower / 14f * 2.4f;
                    avgCPGDmg *= 1.1f;
                }
                else {
                    // sinister strike
                    avgCPGDmg = (character.MainHand.MinDamage + character.MainHand.MaxDamage + stats.WeaponDamage) / 2f;
                    avgCPGDmg += stats.AttackPower / 14f * 2.4f;
                    avgCPGDmg += 98f; // TBC max rank

                    bonusCPGDmgMult *= (1f + .02f * calcOpts.Aggression);
                    bonusCPGDmgMult *= (1f + .1f * calcOpts.SurpriseAttacks);
                    bonusCPGDmgMult *= (1f + stats.SlayerBonusCPGDamage);
                    bonusCPGCritDmgMult *= (1f + .06f * calcOpts.Lethality);
                }

                avgCPGDmg *= bonusCPGDmgMult;

                cpgCrit = mhCrit + bonusCPGCrit;

                avgCPGDmg = (1f - cpgCrit / 100f) * avgCPGDmg + (cpgCrit / 100f) * (avgCPGDmg * bonusCPGCritDmgMult);

                cpgDPS = avgCPGDmg * numCPG / cycleTime;
                cpgDPS *= damageReduction;
            }
            #endregion

            #region Finisher Damage
            finisherDPS = 0f;
            if (character.MainHand != null) {
                if (calcOpts.DPSCycle['r'] > 0) {
                    switch (calcOpts.DPSCycle['r']) {
                        case 5:
                            finisherDmg = 4f * (stats.AttackPower * .01f + 81f);
                            break;
                        case 4:
                            finisherDmg = 5f * (stats.AttackPower * 0.02f + 92f);
                            break;
                        case 3:
                            finisherDmg = 6f * (stats.AttackPower * 0.03f + 103f);
                            break;
                        case 2:
                            finisherDmg = 7f * (stats.AttackPower * 0.03f + 114f);
                            break;
                        default:
                            finisherDmg = 8f * (stats.AttackPower * 0.03f + 125f);
                            break;
                    }

                    finisherDmg *= (1f + .1f * calcOpts.SerratedBlades) * (1f + stats.BonusBleedDamageMultiplier);
                    finisherDmg *= (1f - missChance / 100f);
                    if (calcOpts.SurpriseAttacks < 1)
                        finisherDmg *= (1f - mhDodgeChance / 100f);
                    finisherDPS = finisherDmg / cycleTime;
                }
                else if (calcOpts.DPSCycle['e'] > 0) {
                    evisMod = stats.AttackPower * calcOpts.DPSCycle['e'] * .03f;
                    evisMin = 245f + (calcOpts.DPSCycle['e'] - 1f) * 185f + evisMod;
                    evisMax = 365f + (calcOpts.DPSCycle['e'] - 1f) * 185f + evisMod;

                    finisherDmg = (evisMin + evisMax) / 2f;
                    finisherDmg *= (1f + 0.05f * calcOpts.ImprovedEviscerate);
                    finisherDmg *= (1f + 0.02f * calcOpts.Aggression);
                    finisherDmg = finisherDmg * (1f - (mhCrit / 100f)) + (finisherDmg * 2f) * (mhCrit / 100f);
                    finisherDmg *= (1f - (missChance / 100f));
                    if(calcOpts.SurpriseAttacks < 1)
                        finisherDmg *= (1f - (mhDodgeChance / 100f));
                    finisherDmg *= damageReduction;
                    finisherDPS = finisherDmg / cycleTime;
                }
                else {
                }
            }
            #endregion

            #region Sword Spec Damage
            ssDPS = 0f;
            ssHits = 0f;

            // main hand
            if (character.MainHand != null && character.MainHand.Type == Item.ItemType.OneHandSword) {
                ssHits += mhAttacks * 0.01f * calcOpts.SwordSpecialization * probMHHit;

                // CPG
                ssHits += (numCPG / cycleTime) * 0.01f * calcOpts.SwordSpecialization * probMHHit;

                // finishers
                ssHits += 1f / cycleTime * 0.01f * calcOpts.SwordSpecialization * probMHHit;
            }

            // offhand
            if (character.OffHand != null && character.OffHand.Type == Item.ItemType.OneHandSword) {
                ssHits += ohAttacks * 0.01f * calcOpts.SwordSpecialization * probOHHit;
            }

            ssDPS = (ssHits * avgMHDmg) * (1 - mhCrit / 100f) + (ssHits * avgMHDmg * 2f * bonusWhiteCritDmg) * (mhCrit / 100f);
            ssDPS *= damageReduction;
            #endregion

            #region WF Damage
            wfDPS = 0f;
            if (character.MainHand != null && stats.WindfuryAPBonus > 0) {
                wfHits = mhAttacks * probMHHit * .2f * probMHHit;
                wfHits += ssHits * .2f * probMHHit;

                avgWFDmg = (character.MainHand.MinDamage + character.MainHand.MaxDamage + stats.WeaponDamage * 2) / 2.0f;
                avgWFDmg += (stats.AttackPower + stats.WindfuryAPBonus) / 14f * character.MainHand.Speed;
                avgWFDmg = avgWFDmg * (1f - mhCrit / 100f) + avgMHDmg * 2f * (mhCrit / 100f);

                wfDPS = avgWFDmg * wfHits;
                wfDPS *= damageReduction;
            }
            #endregion

            whiteDPS = mhWhite + ohWhite;
            #endregion

            #region Poison DPS
            poisonDPS = 0f;
            probPoison = (.83f + .05f * calcOpts.MasterPoisoner) * (.2f + .02f * calcOpts.ImprovedPoisons);
            calcDeadly = true;

            if (character.MainHand != null && stats.WindfuryAPBonus == 0f) {
                // no WF, consider the main hand poison
                if (calcOpts.TempMainHandEnchant == "Deadly Poison" && calcDeadly) {
                    poisonDPS += 180f * calcOpts.VilePoisons * .04f / 12f;
                    calcDeadly = false;
                }
                else if (calcOpts.TempMainHandEnchant == "Instant Poison") {
                    poisonDPS += ohHits * probPoison * 170f * (1f + calcOpts.VilePoisons * 0.04f);
                }
            }
            if (character.OffHand != null) {
                if (calcOpts.TempOffHandEnchant == "Deadly Poison" && calcDeadly) {
                    poisonDPS += 180f * (1f + calcOpts.VilePoisons * .04f) / 12f;
                    calcDeadly = false;
                }
                else if (calcOpts.TempOffHandEnchant == "Instant Poison") {
                    poisonDPS += ohHits * probPoison * 170f * (1f + calcOpts.VilePoisons * 0.04f);
                }
            }
            #endregion

            calculatedStats.WhiteDPS = whiteDPS + ssDPS;
            calculatedStats.CPGDPS = cpgDPS;
            calculatedStats.FinisherDPS = finisherDPS;
            calculatedStats.WindfuryDPS = wfDPS;
            calculatedStats.SwordSpecDPS = ssDPS;
            calculatedStats.PoisonDPS = poisonDPS;
            calculatedStats.DPSPoints = whiteDPS + cpgDPS + finisherDPS + wfDPS + ssDPS + poisonDPS;
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

            statsTotal.BonusBleedDamageMultiplier = statsGearEnchantsBuffs.BonusBleedDamageMultiplier;

            statsTotal.WindfuryAPBonus = statsGearEnchantsBuffs.WindfuryAPBonus;

            // T4 bonuses
            statsTotal.NetherbladeBonusSnDDuration = statsGearEnchantsBuffs.NetherbladeBonusSnDDuration;
            statsTotal.NetherbladeCPonFinisher = statsGearEnchantsBuffs.NetherbladeCPonFinisher;

            // T5 bonuses
            statsTotal.DeathmantleBonusDamage = statsGearEnchantsBuffs.DeathmantleBonusDamage;
            statsTotal.DeathmantleBonusFreeFinisher = statsGearEnchantsBuffs.DeathmantleBonusFreeFinisher;

            // T6 bonuses
            statsTotal.SlayerBonusCPGDamage = statsGearEnchantsBuffs.SlayerBonusCPGDamage;
            statsTotal.SlayerBonusSnDHaste = statsGearEnchantsBuffs.SlayerBonusSnDHaste;

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
                ExecutionerProc = stats.ExecutionerProc,
                NetherbladeBonusSnDDuration = stats.NetherbladeBonusSnDDuration,
                NetherbladeCPonFinisher = stats.NetherbladeCPonFinisher,
                DeathmantleBonusDamage = stats.DeathmantleBonusDamage,
                DeathmantleBonusFreeFinisher = stats.DeathmantleBonusFreeFinisher,
                SlayerBonusCPGDamage = stats.SlayerBonusCPGDamage,
                SlayerBonusSnDHaste = stats.SlayerBonusSnDHaste,
                BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier
            };
        }

        public static void LoadTalentSpec(Character c, string talentSpec) {
            string talentCode = "";

            switch (talentSpec) {
                case "Combat Swords (20/41/0)":
                    // http://www.worldofwarcraft.com/info/classes/rogue/talents.html?tal=0053201054000000000000233050020050150023211510000000000000000000000
                    talentCode = "0053201054000000000000233050020050150023211510000000000000000000000";
                    break;
                case "Combat Fist/Sword (16/45/0)":
                    talentCode = "0053201050000000000000233050020050155023210510000000000000000000000";
                    break;
                case "Combat Daggers (15/41/5)":
                    talentCode = "0052031040000000000000233050020550100023211510500000000000000000000";
                    break;
                case "Trispec Hemo Swords (11/28/22)":
                    talentCode = "0053201000000000000000053050020050150020000000502530002300110000000";
                    break;
                case "Trispec Hemo Deadliness (11/21/29)":
                    talentCode = "0053201000000000000000053050020050100000000000502530030301210400000";
                    break;
                case "Mutilate (41/20/0)":
                    talentCode = "0053231055021025010510053050020050000000000000000000000000000000000";
                    break;
                default:
                    break;
            }

            LoadTalentCode(c, talentCode);
        }

        public static void LoadTalentCode(Character c, string talentCode) {
            if (talentCode == null || talentCode.Length != 67) return;
            CalculationOptionsRogue calcOpts = c.CalculationOptions as CalculationOptionsRogue;

            calcOpts.ImprovedEviscerate = int.Parse(talentCode.Substring(0, 1));
            calcOpts.RemorselessAttacks = int.Parse(talentCode.Substring(1, 1));
            calcOpts.Malice = int.Parse(talentCode.Substring(2, 1));
            calcOpts.Ruthlessness = int.Parse(talentCode.Substring(3, 1));
            calcOpts.Murder = int.Parse(talentCode.Substring(4, 1));
            calcOpts.PuncturingWounds = int.Parse(talentCode.Substring(5, 1));
            calcOpts.RelentlessStrikes = int.Parse(talentCode.Substring(6, 1));
            calcOpts.ImprovedExposeArmor = int.Parse(talentCode.Substring(7, 1));
            calcOpts.Lethality = int.Parse(talentCode.Substring(8, 1));
            calcOpts.VilePoisons = int.Parse(talentCode.Substring(9, 1));
            calcOpts.ImprovedPoisons = int.Parse(talentCode.Substring(10, 1));
            calcOpts.FleetFooted = int.Parse(talentCode.Substring(11, 1));
            calcOpts.ColdBlood = int.Parse(talentCode.Substring(12, 1));
            calcOpts.ImprovedKidneyShot = int.Parse(talentCode.Substring(13, 1));
            calcOpts.QuickRecovery = int.Parse(talentCode.Substring(14, 1));
            calcOpts.SealFate = int.Parse(talentCode.Substring(15, 1));
            calcOpts.MasterPoisoner = int.Parse(talentCode.Substring(16, 1));
            calcOpts.Vigor = int.Parse(talentCode.Substring(17, 1));
            calcOpts.DeadenedNerves = int.Parse(talentCode.Substring(18, 1));
            calcOpts.FindWeakness = int.Parse(talentCode.Substring(19, 1));
            calcOpts.Mutilate = int.Parse(talentCode.Substring(20, 1));

            calcOpts.ImprovedGouge = int.Parse(talentCode.Substring(21, 1));
            calcOpts.ImprovedSinisterStrike = int.Parse(talentCode.Substring(22, 1));
            calcOpts.LightningReflexes = int.Parse(talentCode.Substring(23, 1));
            calcOpts.ImprovedSliceandDice = int.Parse(talentCode.Substring(24, 1));
            calcOpts.Deflection = int.Parse(talentCode.Substring(25, 1));
            calcOpts.Precision = int.Parse(talentCode.Substring(26, 1));
            calcOpts.Endurance = int.Parse(talentCode.Substring(27, 1));
            calcOpts.Riposte = int.Parse(talentCode.Substring(28, 1));
            calcOpts.ImprovedSprint = int.Parse(talentCode.Substring(29, 1));
            calcOpts.ImprovedKick = int.Parse(talentCode.Substring(30, 1));
            calcOpts.DaggerSpecialization = int.Parse(talentCode.Substring(31, 1));
            calcOpts.DualWieldSpecialization = int.Parse(talentCode.Substring(32, 1));
            calcOpts.MaceSpecialization = int.Parse(talentCode.Substring(33, 1));
            calcOpts.BladeFlurry = int.Parse(talentCode.Substring(34, 1));
            calcOpts.SwordSpecialization = int.Parse(talentCode.Substring(35, 1));
            calcOpts.FistSpecialization = int.Parse(talentCode.Substring(36, 1));
            calcOpts.BladeTwisting = int.Parse(talentCode.Substring(37, 1));
            calcOpts.WeaponExpertise = int.Parse(talentCode.Substring(38, 1));
            calcOpts.Aggression = int.Parse(talentCode.Substring(39, 1));
            calcOpts.Vitality = int.Parse(talentCode.Substring(40, 1));
            calcOpts.AdrenalineRush = int.Parse(talentCode.Substring(41, 1));
            calcOpts.NervesOfSteel = int.Parse(talentCode.Substring(42, 1));
            calcOpts.CombatPotency = int.Parse(talentCode.Substring(43, 1));
            calcOpts.SurpriseAttacks = int.Parse(talentCode.Substring(44, 1));

            calcOpts.MasterOfDeception = int.Parse(talentCode.Substring(45, 1));
            calcOpts.Opportunity = int.Parse(talentCode.Substring(46, 1));
            calcOpts.SleightOfHand = int.Parse(talentCode.Substring(47, 1));
            calcOpts.DirtyTricks = int.Parse(talentCode.Substring(48, 1));
            calcOpts.Camouflage = int.Parse(talentCode.Substring(49, 1));
            calcOpts.Initiative = int.Parse(talentCode.Substring(50, 1));
            calcOpts.GhostlyStrike = int.Parse(talentCode.Substring(51, 1));
            calcOpts.ImprovedAmbush = int.Parse(talentCode.Substring(52, 1));
            calcOpts.Setup = int.Parse(talentCode.Substring(53, 1));
            calcOpts.Elusiveness = int.Parse(talentCode.Substring(54, 1));
            calcOpts.SerratedBlades = int.Parse(talentCode.Substring(55, 1));
            calcOpts.HeightenedSenses = int.Parse(talentCode.Substring(56, 1));
            calcOpts.Preparation = int.Parse(talentCode.Substring(57, 1));
            calcOpts.DirtyDeeds = int.Parse(talentCode.Substring(58, 1));
            calcOpts.Hemorrhage = int.Parse(talentCode.Substring(59, 1));
            calcOpts.MasterOfSubtlety = int.Parse(talentCode.Substring(60, 1));
            calcOpts.Deadliness = int.Parse(talentCode.Substring(61, 1));
            calcOpts.EnvelopingShadows = int.Parse(talentCode.Substring(62, 1));
            calcOpts.Premeditation = int.Parse(talentCode.Substring(63, 1));
            calcOpts.CheatDeath = int.Parse(talentCode.Substring(64, 1));
            calcOpts.SinisterCalling = int.Parse(talentCode.Substring(65, 1));
            calcOpts.Shadowstep = int.Parse(talentCode.Substring(66, 1));
        }

        public override bool HasRelevantStats(Stats stats) {
            return (stats.Agility + stats.Strength + stats.BonusAgilityMultiplier + stats.BonusStrengthMultiplier + stats.AttackPower + stats.BonusAttackPowerMultiplier + stats.CritRating + stats.HitRating + stats.HasteRating + stats.ExpertiseRating + stats.ArmorPenetration + stats.WeaponDamage + stats.BonusCritMultiplier + stats.WindfuryAPBonus + stats.MongooseProc + stats.MongooseProcAverage + stats.MongooseProcConstant + stats.ExecutionerProc + stats.NetherbladeBonusSnDDuration + stats.NetherbladeCPonFinisher + stats.DeathmantleBonusDamage + stats.DeathmantleBonusFreeFinisher + stats.SlayerBonusCPGDamage + stats.SlayerBonusSnDHaste + stats.BonusBleedDamageMultiplier) != 0;
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