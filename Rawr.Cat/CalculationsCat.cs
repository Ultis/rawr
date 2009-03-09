using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Cat
{
	[Rawr.Calculations.RawrModelInfo("Cat", "Ability_Druid_CatForm", Character.CharacterClass.Druid)]
	public class CalculationsCat : CalculationsBase
	{
		//my insides all turned to ash / so slow
		//and blew away as i collapsed / so cold
        public override List<GemmingTemplate> DefaultGemmingTemplates
		{
			get
			{
				////Relevant Gem IDs for Ferals
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
				int austere = 41380;
				int relentless = 41398;

				return new List<GemmingTemplate>()
				{
					new GemmingTemplate() { Model = "Cat", Group = "Uncommon", //Max Agility
						RedId = delicate[0], YellowId = delicate[0], BlueId = delicate[0], PrismaticId = delicate[0], MetaId = relentless },
					new GemmingTemplate() { Model = "Cat", Group = "Uncommon", //Agi/Crit
						RedId = delicate[0], YellowId = deadly[0], BlueId = shifting[0], PrismaticId = delicate[0], MetaId = relentless },
					//new GemmingTemplate() { Model = "Cat", Group = "Uncommon", //Agi/Hit
					//    RedId = delicate[0], YellowId = glinting[0], BlueId = shifting[0], PrismaticId = delicate[0], MetaId = relentless },

					new GemmingTemplate() { Model = "Cat", Group = "Rare", Enabled = true, //Max Agility
						RedId = delicate[1], YellowId = delicate[1], BlueId = delicate[1], PrismaticId = delicate[1], MetaId = relentless },
					new GemmingTemplate() { Model = "Cat", Group = "Rare", Enabled = true, //Agi/Crit 
						RedId = delicate[1], YellowId = deadly[1], BlueId = shifting[1], PrismaticId = delicate[1], MetaId = relentless },
					//new GemmingTemplate() { Model = "Cat", Group = "Rare", Enabled = true, //Agi/Hit
					//    RedId = delicate[1], YellowId = glinting[1], BlueId = shifting[1], PrismaticId = delicate[1], MetaId = relentless },
						
					new GemmingTemplate() { Model = "Cat", Group = "Epic", //Max Agility
						RedId = delicate[2], YellowId = delicate[2], BlueId = delicate[2], PrismaticId = delicate[2], MetaId = relentless },
					new GemmingTemplate() { Model = "Cat", Group = "Epic", //Agi/Crit 
						RedId = delicate[2], YellowId = deadly[2], BlueId = shifting[2], PrismaticId = delicate[2], MetaId = relentless },
					//new GemmingTemplate() { Model = "Cat", Group = "Epic", //Agi/Hit
					//    RedId = delicate[2], YellowId = glinting[2], BlueId = shifting[2], PrismaticId = delicate[2], MetaId = relentless },
						
					new GemmingTemplate() { Model = "Cat", Group = "Jeweler", //Max Agility
						RedId = delicate[3], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[3], MetaId = relentless },
					new GemmingTemplate() { Model = "Cat", Group = "Jeweler", //Agility Heavy
						RedId = delicate[2], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[2], MetaId = relentless },
				};
			}
        }

        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
		public override CalculationOptionsPanelBase CalculationOptionsPanel
		{
			get
			{
				if (_calculationOptionsPanel == null)
				{
					_calculationOptionsPanel = new CalculationOptionsPanelCat();
				}
				return _calculationOptionsPanel;
			}
		}

		private string[] _characterDisplayCalculationLabels = null;
		public override string[] CharacterDisplayCalculationLabels
		{
			get
			{
				if (_characterDisplayCalculationLabels == null)
					_characterDisplayCalculationLabels = new string[] {
					"Summary:Overall Points*Sum of your DPS Points and Survivability Points",
					"Summary:DPS Points*DPS Points is your theoretical DPS.",
					"Summary:Survivability Points*One hundreth of your health.",

					"Basic Stats:Health",
					"Basic Stats:Attack Power",
					"Basic Stats:Agility",
					"Basic Stats:Strength",
					"Basic Stats:Crit Rating",
					"Basic Stats:Hit Rating",
					"Basic Stats:Expertise Rating",
					"Basic Stats:Haste Rating",
					"Basic Stats:Armor Penetration Rating",
					"Basic Stats:Weapon Damage",
					
					"Complex Stats:Avoided Attacks",
					"Complex Stats:Crit Chance",
					"Complex Stats:Attack Speed",
					"Complex Stats:Armor Mitigation",
					
					"Attacks:Melee Damage",
					"Attacks:Mangle Damage",
					"Attacks:Shred Damage",
					"Attacks:Rake Damage",
					"Attacks:Rip Damage",
					"Attacks:Bite Damage",
					"Attacks:Optimal Rotation",
					"Attacks:Optimal Rotation DPS",
					"Attacks:Custom Rotation DPS",
				};
				return _characterDisplayCalculationLabels;
			}
		}

		private string[] _optimizableCalculationLabels = null;
		public override string[] OptimizableCalculationLabels
		{
			get
			{
				if (_optimizableCalculationLabels == null)
					_optimizableCalculationLabels = new string[] {
					"Custom Rotation DPS",
					"Health",
					"Avoided Attacks %",
                    "Nature Resist",
                    "Fire Resist",
                    "Frost Resist",
                    "Shadow Resist",
                    "Arcane Resist",
					};
				return _optimizableCalculationLabels;
			}
		}

		private string[] _customChartNames = null;
		public override string[] CustomChartNames
		{
			get
			{
				if (_customChartNames == null)
					_customChartNames = new string[] {
					//"Relative Stat Values",
					};
				return _customChartNames;
			}
		}

		private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
		public override Dictionary<string, System.Drawing.Color> SubPointNameColors
		{
			get
			{
				if (_subPointNameColors == null)
				{
					_subPointNameColors = new Dictionary<string, System.Drawing.Color>();
					_subPointNameColors.Add("DPS", System.Drawing.Color.FromArgb(160, 0, 224));
					_subPointNameColors.Add("Survivability", System.Drawing.Color.FromArgb(64, 128, 32));
				}
				return _subPointNameColors;
			}
		}

		private List<Item.ItemType> _relevantItemTypes = null;
		public override List<Item.ItemType> RelevantItemTypes
		{
			get
			{
				if (_relevantItemTypes == null)
				{
					_relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
					{
						Item.ItemType.None,
						Item.ItemType.Leather,
						Item.ItemType.Idol,
						Item.ItemType.Staff,
						Item.ItemType.TwoHandMace,
						Item.ItemType.Polearm
					});
				}
				return _relevantItemTypes;
			}
		}

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Druid; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationCat(); }
		public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsCat(); }

		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsCat));
			System.IO.StringReader reader = new System.IO.StringReader(xml);
			CalculationOptionsCat calcOpts = serializer.Deserialize(reader) as CalculationOptionsCat;
			return calcOpts;
		}

		public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
		{
			//_cachedCharacter = character;
			CalculationOptionsCat calcOpts = character.CalculationOptions as CalculationOptionsCat;
			if (calcOpts == null) calcOpts = new CalculationOptionsCat();
			int targetLevel = calcOpts.TargetLevel;
			float targetArmor = calcOpts.TargetArmor;
			Stats stats = GetCharacterStats(character, additionalItem);
			float levelDifference = (targetLevel - 80f) * 0.2f;
			CharacterCalculationsCat calculatedStats = new CharacterCalculationsCat();
			calculatedStats.BasicStats = stats;
			calculatedStats.TargetLevel = targetLevel;
			bool maintainMangle = stats.BonusBleedDamageMultiplier == 0f;
			stats.BonusBleedDamageMultiplier = 0.3f;

			#region Basic Chances and Constants
			//float armorReductionPercent = (1f - stats.ArmorPenetration) * (1f - stats.ArmorPenetrationRating / 1539.529991f);
			//float reducedArmor = (float)targetArmor * (armorReductionPercent);
			//float modArmor = 1f - (reducedArmor / ((467.5f * character.Level) + reducedArmor - 22167.5f));
			float modArmor = 1f - ArmorCalculations.GetDamageReduction(character.Level, calcOpts.TargetArmor,
				stats.ArmorPenetration, stats.ArmorPenetrationRating);

			float critMultiplier = 2f * (1f + stats.BonusCritMultiplier);
			float critMultiplierBleed = 2f * (1f + stats.BonusCritMultiplier);
			float hasteBonus = stats.HasteRating * 1.3f / 32.78998947f / 100f;
			hasteBonus = (1f + hasteBonus) * (1f + stats.Bloodlust * 40f / Math.Max(calcOpts.Duration, 40f)) - 1f;
			float attackSpeed = 1f / (1f + hasteBonus);
			attackSpeed = attackSpeed / (1f + stats.PhysicalHaste);

			float hitBonus = stats.HitRating / 32.78998947f / 100f + stats.PhysicalHit;
			float expertiseBonus = stats.ExpertiseRating * 1.25f / 32.78998947f / 100f + stats.Expertise * 0.0025f;

			float chanceDodge = Math.Max(0f, 0.065f + .005f * (targetLevel - 83) - expertiseBonus);
			float chanceMiss = Math.Max(0f, 0.08f - hitBonus);
			if ((targetLevel - 80f) < 3)
			{
				chanceMiss = Math.Max(0f, 0.05f + 0.005f * (targetLevel - 80f) - hitBonus);
			}

			if (stats.TerrorProc > 0)
			{
				float terrorUptime = 0.4f; //TODO: Calculate this
				stats.Agility += stats.TerrorProc * terrorUptime * (1 + stats.BonusAgilityMultiplier);
				stats.AttackPower += stats.TerrorProc * terrorUptime * (1 + stats.BonusAgilityMultiplier) * (1 + stats.BonusAttackPowerMultiplier);
			}

			if (stats.MongooseProc > 0)
			{
				float whiteAttacksPerSecond = (1f - chanceMiss - chanceDodge) / attackSpeed;
				float yellowAttacksPerSecond = (1f - chanceMiss - chanceDodge) / 3f; //TODO: Calculate this
				float timeBetweenMongooseProcs = 60f / (whiteAttacksPerSecond + yellowAttacksPerSecond);
				float mongooseUptime = 15f / timeBetweenMongooseProcs;
				stats.Agility += 120f * mongooseUptime * (1 + stats.BonusAgilityMultiplier);
				stats.AttackPower += 120f * mongooseUptime * (1 + stats.BonusAgilityMultiplier) * (1 + stats.BonusAttackPowerMultiplier);
				attackSpeed /= 1f + (0.02f * mongooseUptime);
			}

			if (stats.BerserkingProc > 0)
			{
				float whiteAttacksPerSecond = (1f - chanceMiss - chanceDodge) / attackSpeed;
				float yellowAttacksPerSecond = (1f - chanceMiss - chanceDodge) / 3f; //TODO: Calculate this
				float timeBetweenBerserkingProcs = 45f / (whiteAttacksPerSecond + yellowAttacksPerSecond);
				float berserkingUptime = 15f / timeBetweenBerserkingProcs;
				stats.AttackPower += 400f * berserkingUptime * (1 + stats.BonusAttackPowerMultiplier);
			}
			
			float glanceMultiplier = .7f;
			float chanceAvoided = chanceMiss + chanceDodge;
			float chanceGlance = 0.24f;
			float chanceCrit = (stats.CritRating / 45.90598679f + stats.Agility * 0.012f) / 100f + stats.PhysicalCrit 
				- (0.006f * (targetLevel - character.Level) + (targetLevel == 83 ? 0.03f : 0f));
			float chanceHit = 1f - chanceCrit - chanceAvoided;
			float chanceHitNonGlance = 1f - chanceCrit - chanceAvoided - chanceGlance;
			float chanceNonAvoided = 1f - chanceAvoided;
			float chanceCritBite = Math.Min(1f - chanceAvoided, chanceCrit + stats.BonusFerociousBiteCrit);
			float chanceHitBite = 1f - chanceCritBite - chanceAvoided;

			float cpPerCPG = (chanceHit + chanceCrit * (1 + stats.BonusCPOnCrit)) / chanceNonAvoided;
			calculatedStats.DodgedAttacks = chanceDodge * 100;
			calculatedStats.MissedAttacks = chanceMiss * 100;
			#endregion

			#region Attack Damages
			float baseDamage = 55f + (stats.AttackPower / 14f) + stats.WeaponDamage;
			float meleeDamageRaw = (baseDamage) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * modArmor;
			float mangleDamageRaw = (baseDamage * 2f + 634 + stats.BonusMangleCatDamage) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusMangleDamageMultiplier) * modArmor;
			float shredDamageRaw = (baseDamage * 2.25f + 742.5f + stats.BonusShredDamage) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusShredDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier) * modArmor;
			float rakeDamageRaw = (190 + stats.AttackPower * 0.01f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusRakeDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier);
			float rakeDamageDot = (1161 + stats.AttackPower * 0.18f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusRakeDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier);
			float ripDamageRaw = (3204 + stats.AttackPower * 0.3f + (stats.BonusRipDamagePerCPPerTick * 5f * 6f)) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusRipDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier);
			float biteDamageRaw = (1640 + stats.AttackPower * 0.24f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusFerociousBiteDamageMultiplier) * modArmor;

			float meleeDamageAverage =	chanceGlance * meleeDamageRaw * glanceMultiplier +
										chanceCrit * meleeDamageRaw * critMultiplier +
										chanceHitNonGlance * meleeDamageRaw;
			float mangleDamageAverage = (1f - chanceCrit) * mangleDamageRaw + chanceCrit * mangleDamageRaw * critMultiplier;
			float shredDamageAverage = (1f - chanceCrit) * shredDamageRaw + chanceCrit * shredDamageRaw * critMultiplier;
			float rakeDamageAverage = ((1f - chanceCrit) * rakeDamageRaw + chanceCrit * rakeDamageRaw * critMultiplier) + 
										((1f - chanceCrit) * rakeDamageDot + chanceCrit * rakeDamageDot * critMultiplierBleed);
			float ripDamageAverage = ((1f - chanceCrit) * ripDamageRaw + chanceCrit * ripDamageRaw * critMultiplierBleed);
			float biteDamageAverage = (1f - chanceCritBite) * biteDamageRaw + chanceCritBite * biteDamageRaw * critMultiplier;
			#endregion

			#region Energy Costs
			float mangleEnergyRaw = 45f - stats.MangleCatCostReduction;
			float shredEnergyRaw = 60f - stats.ShredCostReduction;
			float rakeEnergyRaw = 40f - stats.RakeCostReduction;
			float ripEnergyRaw = 30f;
			float biteEnergyRaw = 35f; //Assuming no wasted energy
			float roarEnergyRaw = 25f;

			//[rawCost + ((1/chance_to_land) - 1) * rawCost/5] 
			float cpgEnergyCostMultiplier = 1f + ((1f / chanceNonAvoided) - 1f) * 0.2f;
			float finisherEnergyCostMultiplier = 1f + ((1f / chanceNonAvoided) - 1f) * (1f - stats.FinisherEnergyOnAvoid);
			float mangleEnergyAverage = mangleEnergyRaw * cpgEnergyCostMultiplier;
			float shredEnergyAverage = shredEnergyRaw * cpgEnergyCostMultiplier;
			float rakeEnergyAverage = rakeEnergyRaw * cpgEnergyCostMultiplier;
			float ripEnergyAverage = ripEnergyRaw * finisherEnergyCostMultiplier;
			float biteEnergyAverage = biteEnergyRaw * finisherEnergyCostMultiplier;
			float roarEnergyAverage = roarEnergyRaw;
			#endregion

			#region Rotations
			CatRotationCalculator rotationCalculator = new CatRotationCalculator(stats, calcOpts.Duration, cpPerCPG,
				maintainMangle, calcOpts.GlyphOfMangle ? 18f : 12f, 12f + stats.BonusRipDuration,
				character.DruidTalents.Berserk > 0 ? (calcOpts.GlyphOfBerserk ? 20f : 15f) : 0f, attackSpeed, 
				character.DruidTalents.OmenOfClarity > 0, calcOpts.GlyphOfShred, chanceAvoided, 
				cpgEnergyCostMultiplier, meleeDamageAverage, mangleDamageAverage, shredDamageAverage, 
				rakeDamageAverage, ripDamageAverage, biteDamageAverage, mangleEnergyAverage, shredEnergyAverage, 
				rakeEnergyAverage, ripEnergyAverage, biteEnergyAverage, roarEnergyAverage);
			CatRotationCalculator.CatRotationCalculation rotationCalculationDPS = new CatRotationCalculator.CatRotationCalculation();

			//StringBuilder rotations = new StringBuilder();
			for (int roarCP = 1; roarCP < 6; roarCP++)
				for (int useShred = 0; useShred < 2; useShred++)
					for (int useRip = 0; useRip < 2; useRip++)
						for (int useFerociousBite = 0; useFerociousBite < 2; useFerociousBite++)
						{
							CatRotationCalculator.CatRotationCalculation rotationCalculation =
								rotationCalculator.GetRotationCalculations(
								useShred == 1, useRip == 1, useFerociousBite == 1, roarCP);
							//rotations.AppendLine(rotationCalculation.Name + ": " + rotationCalculation.DPS + "DPS");
							if (rotationCalculation.DPS > rotationCalculationDPS.DPS)
								rotationCalculationDPS = rotationCalculation;
						}

			float ripDurationMultiplier = (calcOpts.GlyphOfShred && rotationCalculationDPS.ShredDamageTotal > 0 ? 
				rotationCalculator.RipDuration + 6 : rotationCalculator.RipDuration) / 12f;

			calculatedStats.HighestDPSRotation = rotationCalculationDPS;
			calculatedStats.CustomRotation = rotationCalculator.GetRotationCalculations(
				calcOpts.CustomUseShred, calcOpts.CustomUseRip, calcOpts.CustomUseFerociousBite, calcOpts.CustomCPSavageRoar);
			//calculatedStats.Rotations = rotations.ToString();
			#endregion

			calculatedStats.AvoidedAttacks = chanceAvoided * 100f;
			calculatedStats.DodgedAttacks = chanceDodge * 100f;
			calculatedStats.MissedAttacks = chanceMiss * 100f;
			calculatedStats.CritChance = chanceCrit * 100f;
			calculatedStats.AttackSpeed = attackSpeed;
			calculatedStats.ArmorMitigation = (1f - modArmor) * 100f;

			calculatedStats.MeleeDamagePerHit = meleeDamageRaw;
			calculatedStats.MeleeDamagePerSwing = meleeDamageAverage;
			calculatedStats.MangleDamagePerHit = mangleDamageRaw;
			calculatedStats.MangleDamagePerSwing = mangleDamageAverage * chanceNonAvoided;
			calculatedStats.ShredDamagePerHit = shredDamageRaw;
			calculatedStats.ShredDamagePerSwing = shredDamageAverage * chanceNonAvoided;
			calculatedStats.RakeDamagePerHit = rakeDamageRaw + rakeDamageDot;
			calculatedStats.RakeDamagePerSwing = rakeDamageAverage * chanceNonAvoided;
			calculatedStats.RipDamagePerHit = ripDamageRaw * ripDurationMultiplier;
			calculatedStats.RipDamagePerSwing = ripDamageAverage * chanceNonAvoided * ripDurationMultiplier;
			calculatedStats.BiteDamagePerHit = biteDamageRaw;
			calculatedStats.BiteDamagePerSwing = biteDamageAverage * chanceNonAvoided;

			calculatedStats.DPSPoints = calculatedStats.HighestDPSRotation.DPS;
			calculatedStats.SurvivabilityPoints = stats.Health / 100f;
			calculatedStats.OverallPoints = calculatedStats.DPSPoints + calculatedStats.SurvivabilityPoints;
			return calculatedStats;
			
			#region OLD - Toskk's DPS calculations from 2.0
			/*
			//Begin Toskk's 
			#region Calculate Basic Chances, Costs, and Damage

			float baseArmor = Math.Max(0f, targetArmor - stats.ArmorPenetration);
			float modArmor = (baseArmor / (baseArmor + 10557.5f )) * 100f;

			float critMultiplier = 2 * (1 + stats.BonusCritMultiplier);
            float physicalCritModifier = 1.0f + ((stats.CritRating / (1148f / 52f)) / 100.0f) * (1f + stats.BonusCritMultiplier * 2f);
			float attackPower = stats.AttackPower + (stats.ExposeWeakness * exposeWeaknessAPValue * (1 + stats.BonusAttackPowerMultiplier));

			float hitBonus = stats.HitRating * 52f / 82f / 1000f;
			float expertiseBonus = stats.ExpertiseRating * 52f / 82f / 2.5f * 0.0025f;

			float glancingRate = 0.2335774f; // Glancing rate data from Toskk

			float chanceCrit = Math.Min(0.75f, (stats.CritRating / (1148f / 52f) + (stats.Agility / 25f)) / 100f) - 0.042f; // Crit Reduction data from Toskk
			float chanceDodge = Math.Max(0f, 0.065f - expertiseBonus);
			float chanceMiss = Math.Max(0f, 0.09f - hitBonus) + chanceDodge;
						
			float meleeDamage = stats.WeaponDamage + (768f + attackPower) / 14f;
			float mangleCost = 40f - stats.MangleCatCostReduction;
			float totalMangleCost = 1f / (1f - chanceMiss) * (mangleCost * (1f - chanceMiss) + mangleCost / 5f * chanceMiss);
			float mangleDamage = 1.2f * (meleeDamage * 1.6f + 264f + stats.BonusMangleCatDamage);

			float shredCost = 42f;
			float totalShredCost = chanceMiss * shredCost / 5f + (1f - chanceMiss) * shredCost;
			float shredDamage = meleeDamage * 2.25f + 405f + stats.BonusShredDamage;

			float ripCost = 30f;
			float totalRipCost = 1f / (1f - chanceMiss) * ripCost;

			float chanceWhiteCrit = Math.Min(chanceCrit, 1f - glancingRate - chanceMiss);

			float hasteBonus = stats.HasteRating / 15.76f / 100f;
			float attackSpeed = (1f - (stats.Bloodlust * bloodlustUptime)) / (1f + hasteBonus);
			float meleeTicker = attackSpeed;

			#endregion

			float dmgMelee = 0f;
			float cycleTime = 0f;
			float segmentTime = 0f;
			float energyCount = 10f;
			float terrorTicker = 0f;
			float comboPoints = 0f;
			float dmgMangles = 0f;

			#region Mangle

			if (primaryAttack == "Mangle" || primaryAttack == "Both")
			{
				// Starting immediately after Ripping, with energy to Mangle and GCD running
				dmgMelee = 0f;
				cycleTime = 1.0f;
				segmentTime = 1.0f;
				energyCount = mangleCost + 10f;

				dmgMelee += segmentTime / attackSpeed * meleeDamage * (0.7f * glancingRate + ((1 - glancingRate) - chanceMiss - chanceWhiteCrit) +
					critMultiplier * chanceWhiteCrit);
				energyCount += segmentTime / attackSpeed * stats.BloodlustProc;

				//GCD ends, Mangle hits

				energyCount -= totalMangleCost * (1f - segmentTime / 30f);
				dmgMangles = mangleDamage * ((1f - chanceCrit) + critMultiplier * chanceCrit);
				comboPoints = 1f + chanceCrit;
				energyCount += segmentTime / attackSpeed * stats.BloodlustProc;

				if (stats.TerrorProc > 0)
				{
					attackPower += 65f * 0.85f * (1f + stats.BonusAgilityMultiplier) * (1f + stats.BonusAttackPowerMultiplier);
					chanceCrit += 65f * 0.85f * stats.BonusAgilityMultiplier / 25 / 100;
					terrorTicker = 10;
				}
			}

			#endregion

			#region Shreds

			float numberAttacks = (4f - 3f * chanceCrit + 2f * (float)Math.Pow(chanceCrit, 2) - (float)Math.Pow(chanceCrit, 3)) / (1f - chanceMiss);
			float numberShreds = (numberAttacks - (1f / (1f - chanceMiss)));

			// Mangle Debuff running, waiting for energy to Shred number_shreds times

			energyCount += (numberShreds - 1f) * stats.BloodlustProc;

			segmentTime = (numberShreds * totalShredCost * (1f - numberShreds * totalShredCost / 10f / numberShreds / 30f)
				- energyCount) / 10f;
			segmentTime -= segmentTime / attackSpeed * stats.BloodlustProc / 10f;
			cycleTime += segmentTime;

			terrorTicker -= segmentTime;

			dmgMelee += segmentTime / attackSpeed * (stats.WeaponDamage + (768f + attackPower) / 14f) *
				(0.7f * glancingRate + ((1 - glancingRate) - chanceMiss - chanceWhiteCrit) + critMultiplier * chanceWhiteCrit);

			float dmgPerShred = ((stats.WeaponDamage + (768f + attackPower) / 14f) * 2.25f + 405f + stats.BonusShredDamage);
			float dmgShreds = 0;
			if (segmentTime <= 12f)
				dmgShreds = numberShreds * dmgPerShred * 1.3f * ((1f - chanceMiss) * (1f - chanceCrit) + critMultiplier *
					(1f - chanceMiss) * chanceCrit);
			else
				dmgShreds = ((12f / segmentTime) * numberShreds * dmgPerShred * 1.3f * ((1f - chanceMiss) * (1f - chanceCrit) + 
					critMultiplier * (1f - chanceMiss) * chanceCrit)) + ((1f - 12f / segmentTime) * numberShreds * 
					dmgPerShred * ((1f - chanceMiss) * (1f - chanceCrit) + critMultiplier * (1f - chanceMiss) * chanceCrit));

			energyCount = 0;
			energyCount += stats.BloodlustProc;
			
			#endregion

			#region Powershifting

			if (powershift > 0)
			{
				energyCount = 17f / powershift;
			}
			//Ignoring the wolf helm since it's pretty craptacular

			// 5 Combo points generated, waiting for energy to Rip + Mangle

			segmentTime = (((primaryAttack == "Mangle" || primaryAttack == "Both") ? mangleCost : 0) + totalRipCost * (1f - totalRipCost / 10f / 30f) - energyCount) / 10f;
			segmentTime -= segmentTime / attackSpeed * stats.BloodlustProc / 10f;

			cycleTime += segmentTime;

			if (terrorTicker > segmentTime) terrorTicker = segmentTime;

			if (stats.TerrorProc > 0)
				dmgMelee += (terrorTicker / segmentTime) * (segmentTime / attackSpeed * (stats.WeaponDamage + (768f +
					attackPower) / 14f) * (0.7f * glancingRate + ((1 - glancingRate) - chanceMiss - chanceWhiteCrit) + critMultiplier *
					chanceWhiteCrit)) + (1f - terrorTicker / segmentTime) * (segmentTime / attackSpeed * meleeDamage *
					(0.7f * glancingRate + ((1 - glancingRate) - chanceMiss - chanceWhiteCrit) + critMultiplier * chanceWhiteCrit));
			else
				dmgMelee += segmentTime / attackSpeed * meleeDamage * (0.7f * glancingRate + ((1 - glancingRate) - chanceMiss -
					chanceWhiteCrit) + critMultiplier * chanceWhiteCrit);
			#endregion

			#region Rip

			float rip5 = (chanceCrit - (float)Math.Pow(chanceCrit, 2) + (float)Math.Pow(chanceCrit, 3) - (float)Math.Pow(chanceCrit, 4));
			float rip4 = 1f - rip5;

			// Checking on the possibility of completing the cycle too quickly

			if (cycleTime < 12f)
			{
				segmentTime = 12f - cycleTime;
				cycleTime += segmentTime;
				energyCount = segmentTime * 10f;

				dmgMelee += segmentTime / attackSpeed * meleeDamage * (0.7f * glancingRate + ((1 - glancingRate) - chanceMiss - chanceWhiteCrit) +
					critMultiplier * chanceWhiteCrit);

				energyCount += segmentTime / attackSpeed * stats.BloodlustProc;

				float extraShred = energyCount / (totalShredCost * (1f - segmentTime / 30f));

				dmgShreds += extraShred * shredDamage * 1.3f * ((1f - chanceMiss) * (1 - chanceCrit) +
					critMultiplier * (1f - chanceMiss) * chanceCrit);

				rip5 += extraShred * rip4;
				rip4 -= extraShred * rip4;

				numberShreds += extraShred;
			}

			float dmgRips = (rip5 * (attackPower * 0.24f + 1553f + (stats.BonusRipDamagePerCPPerTick * 6f * 5f)) + rip4 * 
				(attackPower * 0.24f + 1272f + (stats.BonusRipDamagePerCPPerTick * 6f * 4f))) * 1.3f * (1f + stats.BonusRipDamageMultiplier);

			#endregion

            float ssoNeckProcDPS = 0f;
            
            if (stats.ShatteredSunMightProc > 0)
            {
                switch (shattrathFaction)
                {
                    case "Scryer":
                        //Need to factor in partial resists.
                        ssoNeckProcDPS = 350f * (1 + stats.BonusSpellPowerMultiplier) *
                            (1 + stats.BonusArcaneDamageMultiplier) * physicalCritModifier / 50f;
                        break;
                }
            }
            float dps = ((1.1f * (((dmgMangles + dmgShreds + dmgMelee) * (1f - modArmor / 100f) + dmgRips) / cycleTime)) 
				* (1 + stats.BonusDamageMultiplier)) + ssoNeckProcDPS;

			calculatedStats.DPSPoints = dps;
			calculatedStats.SurvivabilityPoints = stats.Health * 0.002f;
			calculatedStats.OverallPoints = calculatedStats.DPSPoints + calculatedStats.SurvivabilityPoints;
			calculatedStats.AvoidedAttacks = chanceMiss * 100f;
			calculatedStats.DodgedAttacks = chanceDodge * 100f;
			calculatedStats.MissedAttacks = calculatedStats.AvoidedAttacks - calculatedStats.DodgedAttacks;
			calculatedStats.WhiteCrit = chanceWhiteCrit * 100f;
			calculatedStats.YellowCrit = (1f - chanceMiss) * chanceCrit * 100f;
			calculatedStats.ShredsPerCycle = numberShreds;
			calculatedStats.AttackSpeed = attackSpeed;
			calculatedStats.ArmorMitigation = modArmor;
			calculatedStats.MangleDamage = dmgMangles * (1f - modArmor / 100f) / ((dmgMangles + dmgShreds + dmgMelee) * (1f - modArmor / 100f) + dmgRips) * 100f;
			calculatedStats.MeleeDamage = dmgMelee * (1f - modArmor / 100f) / ((dmgMangles + dmgShreds + dmgMelee) * (1f - modArmor / 100f) + dmgRips) * 100f;
			calculatedStats.RipDamage = dmgRips / ((dmgMangles + dmgShreds + dmgMelee) * (1f - modArmor / 100f) + dmgRips) * 100f;
			calculatedStats.Finishers4cp = rip4 * 100f;
			calculatedStats.Finishers5cp = rip5 * 100f;
			calculatedStats.ShredDamage = dmgShreds * (1f - modArmor / 100f) / ((dmgMangles + dmgShreds + dmgMelee) * (1f - modArmor / 100f) + dmgRips) * 100f;
			calculatedStats.CycleTime = cycleTime;

			

			*/
			#endregion
		}

		public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
			CalculationOptionsCat calcOpts = character.CalculationOptions as CalculationOptionsCat;

			Stats statsRace = character.Race == Character.CharacterRace.NightElf ?
				new Stats() {
					Health = 7237f,
					Strength = 86f,
					Agility = 87f,
					Stamina = 96f,
					Dodge = 0.04951f,
					AttackPower = 140f,
					BonusPhysicalDamageMultiplier = calcOpts.GlyphOfSavageRoar ? 0.36f : 0.3f, //Savage Roar
					PhysicalCrit = 0.07476f } : 
				new Stats() {
					Health = 7599f,
					Strength = 95f,
					Agility = 75f,
					Stamina = 100f,
					Dodge = 0.04951f,
					AttackPower = 140f,
					BonusPhysicalDamageMultiplier = calcOpts.GlyphOfSavageRoar ? 0.36f : 0.3f, //Savage Roar
					PhysicalCrit = 0.07476f
				};

			Stats statsItems = GetItemStats(character, additionalItem);
			//Stats statsEnchants = GetEnchantsStats(character);
			Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
			float[] thickHideMultipliers = new float[] { 1f, 1.04f, 1.07f, 1.1f };
			statsItems.Armor *= thickHideMultipliers[character.DruidTalents.ThickHide];

			DruidTalents talents = character.DruidTalents;
			Stats statsTalents = new Stats()
			{
				PhysicalCrit = 0.02f * talents.SharpenedClaws + (character.ActiveBuffsContains("Leader of the Pack") ?
					0 : 0.05f * talents.LeaderOfThePack) + 0.02f * talents.MasterShapeshifter,
				Dodge = 0.02f * talents.FeralSwiftness,
				BonusStaminaMultiplier = (1f + 0.02f * talents.SurvivalOfTheFittest) * (1f + 0.01f * talents.ImprovedMarkOfTheWild) - 1f,
				BonusAgilityMultiplier = (1f + 0.02f * talents.SurvivalOfTheFittest) * (1f + 0.01f * talents.ImprovedMarkOfTheWild) - 1f,
				BonusStrengthMultiplier = (1f + 0.02f * talents.SurvivalOfTheFittest) * (1f + 0.01f * talents.ImprovedMarkOfTheWild) - 1f,
				BonusAttackPowerMultiplier = 0.02f * talents.HeartOfTheWild,
				CritChanceReduction = 0.02f * talents.SurvivalOfTheFittest,
				BonusPhysicalDamageMultiplier = 0.02f * talents.Naturalist,
				BonusMangleDamageMultiplier = 0.1f * talents.SavageFury,
				BonusRakeDamageMultiplier = 0.1f * talents.SavageFury,
				BonusShredDamageMultiplier = 0.04f * talents.RendAndTear,
				BonusFerociousBiteCrit = 0.1f * talents.RendAndTear,
				BonusEnergyOnTigersFury = 20f * talents.KingOfTheJungle,
				MangleCatCostReduction = 1f * talents.Ferocity + 2f * talents.ImprovedMangle,
				RakeCostReduction = 1f * talents.Ferocity,
				ShredCostReduction = 9f * talents.ShreddingAttacks,
				BonusCPOnCrit = 0.5f * talents.PrimalFury,
				Expertise = 5f * talents.PrimalPrecision,
				FinisherEnergyOnAvoid = 0.4f * talents.PrimalPrecision,
				AttackPower = (character.Level / 2f) * talents.PredatoryStrikes,
				BonusCritMultiplier = 0.1f * ((float)talents.PredatoryInstincts / 3f), 
				BonusFerociousBiteDamageMultiplier = 0.03f * talents.FeralAggression,
				BonusRipDuration = (character.CalculationOptions as CalculationOptionsCat).GlyphOfRip ? 4f : 0f,
			};

			Stats statsGearEnchantsBuffs = statsItems + statsBuffs;
            statsGearEnchantsBuffs.Agility += statsGearEnchantsBuffs.AverageAgility;
			statsGearEnchantsBuffs.Strength += statsGearEnchantsBuffs.CatFormStrength;
			
			Stats statsTotal = statsRace + statsItems + statsBuffs + statsTalents;

            // Inserted by Trolando
            if (statsTotal.GreatnessProc > 0)
            {
                float expectedAgi = (float)Math.Floor(statsTotal.Agility * (1 + statsTotal.BonusAgilityMultiplier));
                float expectedStr = (float)Math.Floor(statsTotal.Strength * (1 + statsTotal.BonusStrengthMultiplier));
                // Highest stat
                if (expectedAgi > expectedStr)
                {
                    statsTotal.Agility += statsTotal.GreatnessProc * 15f / 47f;
                }
                else
                {
                    statsTotal.Strength += statsTotal.GreatnessProc * 15f / 47f;
                }
            }
            
            Stats statsWeapon = character.MainHand == null ? new Stats() : character.MainHand.GetTotalStats(character).Clone();
			statsWeapon.Strength *= (1 + statsTotal.BonusStrengthMultiplier);
			statsWeapon.AttackPower += statsWeapon.Strength * 2f;
			if (character.MainHand != null)
			{
				float fap = (character.MainHand.Item.DPS - 54.8f) * 14f; //TODO Find a more accurate number for this?
				statsTotal.AttackPower += fap;
				statsWeapon.AttackPower += fap;
			}

			statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
			statsTotal.Strength = (float)Math.Floor(statsTotal.Strength * (1f + statsTotal.BonusStrengthMultiplier));
			statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1f + statsTotal.BonusAgilityMultiplier));
			statsTotal.AttackPower += statsTotal.Strength * 2f + statsTotal.Agility;
			statsTotal.AttackPower += statsWeapon.AttackPower * 0.2f * (talents.PredatoryStrikes / 3f);
			statsTotal.AttackPower = (float)Math.Floor(statsTotal.AttackPower * (1f+ statsTotal.BonusAttackPowerMultiplier));
			statsTotal.Health += (float)Math.Floor(statsTotal.Stamina * 10f) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f);
			statsTotal.Armor += 2f * statsTotal.Agility;
			statsTotal.Armor = (float)Math.Floor(statsTotal.Armor * (1f + statsTotal.BonusArmorMultiplier));
			statsTotal.NatureResistance += statsTotal.NatureResistanceBuff + statsTotal.AllResist;
			statsTotal.FireResistance += statsTotal.FireResistanceBuff + statsTotal.AllResist;
			statsTotal.FrostResistance += statsTotal.FrostResistanceBuff + statsTotal.AllResist;
			statsTotal.ShadowResistance += statsTotal.ShadowResistanceBuff + statsTotal.AllResist;
			statsTotal.ArcaneResistance += statsTotal.ArcaneResistanceBuff + statsTotal.AllResist;
			statsTotal.WeaponDamage += 16f; //Tiger's Fury
            // Haste trinket (Meteorite Whetstone)
            statsTotal.HasteRating += statsTotal.HasteRatingOnPhysicalAttack * 10 / 45;

			#region OLD - Manual Stat Summing
			//float agiBase = (float)Math.Floor(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier));
			//float agiBonus = (float)Math.Floor(statsGearEnchantsBuffs.Agility * (1 + statsRace.BonusAgilityMultiplier));
			//float strBase = (float)Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier));
			//float strBonus = (float)Math.Floor(statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier));
			//float staBase = (float)Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier));
			//float staBonus = (float)Math.Floor(statsGearEnchantsBuffs.Stamina * (1 + statsRace.BonusStaminaMultiplier));

			//statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier)) - 1;
			//statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;
			//statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)) - 1;
			//statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;
			//statsTotal.BonusSpellPowerMultiplier = ((1 + statsRace.BonusSpellPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpellPowerMultiplier)) - 1;
			//statsTotal.BonusArcaneDamageMultiplier = ((1 + statsRace.BonusArcaneDamageMultiplier) * (1 + statsGearEnchantsBuffs.BonusArcaneDamageMultiplier)) - 1;
			//statsTotal.Agility = (agiBase + (float)Math.Floor((agiBase * statsBuffs.BonusAgilityMultiplier) + agiBonus * (1 + statsBuffs.BonusAgilityMultiplier)));
			//statsTotal.Strength = (strBase + (float)Math.Floor((strBase * statsBuffs.BonusStrengthMultiplier) + strBonus * (1 + statsBuffs.BonusStrengthMultiplier)));
			//statsTotal.Stamina = (staBase + (float)Math.Round((staBase * statsBuffs.BonusStaminaMultiplier) + staBonus * (1 + statsBuffs.BonusStaminaMultiplier)));
			//statsTotal.DefenseRating = statsRace.DefenseRating + statsGearEnchantsBuffs.DefenseRating;
			//statsTotal.DodgeRating = statsRace.DodgeRating + statsGearEnchantsBuffs.DodgeRating;
			//statsTotal.Resilience = statsRace.Resilience + statsGearEnchantsBuffs.Resilience;
			//statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + (statsTotal.Stamina * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
			//statsTotal.Armor = (float)Math.Round((statsGearEnchantsBuffs.Armor + statsRace.Armor + (statsTotal.Agility * 2f)) * (1 + statsBuffs.BonusArmorMultiplier));
			//statsTotal.Miss = statsBuffs.Miss;
			//statsTotal.ArmorPenetration = statsRace.ArmorPenetration + statsGearEnchantsBuffs.ArmorPenetration;
			//statsTotal.AttackPower = (float)Math.Floor((statsRace.AttackPower + statsGearEnchantsBuffs.AttackPower + statsTotal.Agility + (statsTotal.Strength * 2)) *  (1f + statsTotal.BonusAttackPowerMultiplier));
			//statsTotal.BloodlustProc = statsRace.BloodlustProc + statsGearEnchantsBuffs.BloodlustProc;
			//statsTotal.BonusCritMultiplier = ((1 + statsRace.BonusCritMultiplier) * (1 + statsGearEnchantsBuffs.BonusCritMultiplier)) - 1;
			//statsTotal.BonusMangleCatDamage = statsRace.BonusMangleCatDamage + statsGearEnchantsBuffs.BonusMangleCatDamage;
			//statsTotal.BonusRipDamageMultiplier = ((1 + statsRace.BonusRipDamageMultiplier) * (1 + statsGearEnchantsBuffs.BonusRipDamageMultiplier)) - 1;
			//statsTotal.BonusShredDamage = statsRace.BonusShredDamage + statsGearEnchantsBuffs.BonusShredDamage;
			//statsTotal.BonusDamageMultiplier = statsGearEnchantsBuffs.BonusDamageMultiplier;
			//statsTotal.BonusRipDamagePerCPPerTick = statsRace.BonusRipDamagePerCPPerTick + statsGearEnchantsBuffs.BonusRipDamagePerCPPerTick;
			//statsTotal.CritRating = statsRace.CritRating + statsGearEnchantsBuffs.CritRating;
			//statsTotal.ExpertiseRating = statsRace.ExpertiseRating + statsGearEnchantsBuffs.ExpertiseRating;
			//statsTotal.HasteRating = statsRace.HasteRating + statsGearEnchantsBuffs.HasteRating;
			//statsTotal.HitRating = statsRace.HitRating + statsGearEnchantsBuffs.HitRating;
			//statsTotal.MangleCatCostReduction = statsRace.MangleCatCostReduction + statsGearEnchantsBuffs.MangleCatCostReduction;
			//statsTotal.TerrorProc = statsRace.TerrorProc + statsGearEnchantsBuffs.TerrorProc;
			//statsTotal.WeaponDamage = statsRace.WeaponDamage + statsGearEnchantsBuffs.WeaponDamage;
			//statsTotal.ExposeWeakness = statsRace.ExposeWeakness + statsGearEnchantsBuffs.ExposeWeakness;
			//statsTotal.Bloodlust = statsRace.Bloodlust + statsGearEnchantsBuffs.Bloodlust;
			//statsTotal.ShatteredSunMightProc = statsRace.ShatteredSunMightProc + statsGearEnchantsBuffs.ShatteredSunMightProc;

			//statsTotal.NatureResistance = statsEnchants.NatureResistance + statsRace.NatureResistance + statsBaseGear.NatureResistance + statsBuffs.NatureResistance +
			//    statsEnchants.NatureResistanceBuff + statsRace.NatureResistanceBuff + statsBaseGear.NatureResistanceBuff + statsBuffs.NatureResistanceBuff;
			//statsTotal.FireResistance = statsEnchants.FireResistance + statsRace.FireResistance + statsBaseGear.FireResistance + statsBuffs.FireResistance +
			//    statsEnchants.FireResistanceBuff + statsRace.FireResistanceBuff + statsBaseGear.FireResistanceBuff + statsBuffs.FireResistanceBuff;
			//statsTotal.FrostResistance = statsEnchants.FrostResistance + statsRace.FrostResistance + statsBaseGear.FrostResistance + statsBuffs.FrostResistance +
			//    statsEnchants.FrostResistanceBuff + statsRace.FrostResistanceBuff + statsBaseGear.FrostResistanceBuff + statsBuffs.FrostResistanceBuff;
			//statsTotal.ShadowResistance = statsEnchants.ShadowResistance + statsRace.ShadowResistance + statsBaseGear.ShadowResistance + statsBuffs.ShadowResistance +
			//    statsEnchants.ShadowResistanceBuff + statsRace.ShadowResistanceBuff + statsBaseGear.ShadowResistanceBuff + statsBuffs.ShadowResistanceBuff;
			//statsTotal.ArcaneResistance = statsEnchants.ArcaneResistance + statsRace.ArcaneResistance + statsBaseGear.ArcaneResistance + statsBuffs.ArcaneResistance +
			//    statsEnchants.ArcaneResistanceBuff + statsRace.ArcaneResistanceBuff + statsBaseGear.ArcaneResistanceBuff + statsBuffs.ArcaneResistanceBuff;
			#endregion

			return statsTotal;
		}

		public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
		{
			switch (chartName)
			{
				//case "Combat Table (White)":
				//    CharacterCalculationsCat currentCalculationsCatWhite = GetCharacterCalculations(character) as CharacterCalculationsCat;
				//    ComparisonCalculationCat calcMissWhite = new ComparisonCalculationCat()		{ Name = "    Miss    " };
				//    ComparisonCalculationCat calcDodgeWhite = new ComparisonCalculationCat()	{ Name = "   Dodge   " };
				//    ComparisonCalculationCat calcCritWhite = new ComparisonCalculationCat()		{ Name = "  Crit  " };
				//    ComparisonCalculationCat calcGlanceWhite = new ComparisonCalculationCat()	{ Name = " Glance " };
				//    ComparisonCalculationCat calcHitWhite = new ComparisonCalculationCat()		{ Name = "Hit" };
				//    if (currentCalculationsCatWhite != null)
				//    {
				//        calcMissWhite.OverallPoints = calcMissWhite.DPSPoints = currentCalculationsCatWhite.MissedAttacks;
				//        calcDodgeWhite.OverallPoints = calcDodgeWhite.DPSPoints = currentCalculationsCatWhite.DodgedAttacks;
				//        calcCritWhite.OverallPoints = calcCritWhite.DPSPoints = currentCalculationsCatWhite.WhiteCrit;
				//        calcGlanceWhite.OverallPoints = calcGlanceWhite.DPSPoints = 23.35774f;
				//        calcHitWhite.OverallPoints = calcHitWhite.DPSPoints = (100f - calcMissWhite.OverallPoints - 
				//            calcDodgeWhite.OverallPoints - calcCritWhite.OverallPoints - calcGlanceWhite.OverallPoints);
				//    }
				//    return new ComparisonCalculationBase[] { calcMissWhite, calcDodgeWhite, calcCritWhite, calcGlanceWhite, calcHitWhite };

				//case "Combat Table (Yellow)":
				//    CharacterCalculationsCat currentCalculationsCatYellow = GetCharacterCalculations(character) as CharacterCalculationsCat;
				//    ComparisonCalculationCat calcMissYellow = new ComparisonCalculationCat()	{ Name = "    Miss    " };
				//    ComparisonCalculationCat calcDodgeYellow = new ComparisonCalculationCat()	{ Name = "   Dodge   " };
				//    ComparisonCalculationCat calcCritYellow = new ComparisonCalculationCat()	{ Name = "  Crit  " };
				//    ComparisonCalculationCat calcGlanceYellow = new ComparisonCalculationCat()	{ Name = " Glance " };
				//    ComparisonCalculationCat calcHitYellow = new ComparisonCalculationCat()		{ Name = "Hit" };
				//    if (currentCalculationsCatYellow != null)
				//    {
				//        calcMissYellow.OverallPoints = calcMissYellow.DPSPoints = currentCalculationsCatYellow.MissedAttacks;
				//        calcDodgeYellow.OverallPoints = calcDodgeYellow.DPSPoints = currentCalculationsCatYellow.DodgedAttacks;
				//        calcCritYellow.OverallPoints = calcCritYellow.DPSPoints = currentCalculationsCatYellow.YellowCrit;
				//        calcGlanceYellow.OverallPoints = calcGlanceYellow.DPSPoints = 0f;
				//        calcHitYellow.OverallPoints = calcHitYellow.DPSPoints = (100f - calcMissYellow.OverallPoints -
				//            calcDodgeYellow.OverallPoints - calcCritYellow.OverallPoints - calcGlanceYellow.OverallPoints);
				//    }
				//    return new ComparisonCalculationBase[] { calcMissYellow, calcDodgeYellow, calcCritYellow, calcGlanceYellow, calcHitYellow };
				case "Hit Test":
					float dpsBaseHit = GetCharacterCalculations(character).OverallPoints;
					float dps1 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 1 } }).OverallPoints - dpsBaseHit);
					float dps2 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 2 } }).OverallPoints - dpsBaseHit);
					float dps3 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 3 } }).OverallPoints - dpsBaseHit);
					float dps4 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 4 } }).OverallPoints - dpsBaseHit);
					float dps5 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 5 } }).OverallPoints - dpsBaseHit);
					float dps10 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 10 } }).OverallPoints - dpsBaseHit);
					float dps15 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 15 } }).OverallPoints - dpsBaseHit);
					float dps20 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 20 } }).OverallPoints - dpsBaseHit);
					float dps25 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 25 } }).OverallPoints - dpsBaseHit);
					float dps50 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 50 } }).OverallPoints - dpsBaseHit);
					float dps75 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 75 } }).OverallPoints - dpsBaseHit);
					float dps83 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 83 } }).OverallPoints - dpsBaseHit);
					float dps100 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 100 } }).OverallPoints - dpsBaseHit);
					float dps200 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 200 } }).OverallPoints - dpsBaseHit);
					float dps300 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 300 } }).OverallPoints - dpsBaseHit);
					float dps400 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 400 } }).OverallPoints - dpsBaseHit);
					float dps500 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 500 } }).OverallPoints - dpsBaseHit);

					return new ComparisonCalculationBase[] { 
						new ComparisonCalculationCat() { Name = "dps1", OverallPoints = dps1, DPSPoints = dps1 },
						new ComparisonCalculationCat() { Name = "dps2", OverallPoints = dps2, DPSPoints = dps2 },
						new ComparisonCalculationCat() { Name = "dps3", OverallPoints = dps3, DPSPoints = dps3 },
						new ComparisonCalculationCat() { Name = "dps4", OverallPoints = dps4, DPSPoints = dps4 },
						new ComparisonCalculationCat() { Name = "dps5", OverallPoints = dps5, DPSPoints = dps5 },
						new ComparisonCalculationCat() { Name = "dps10", OverallPoints = dps10, DPSPoints = dps10 },
						new ComparisonCalculationCat() { Name = "dps15", OverallPoints = dps15, DPSPoints = dps15 },
						new ComparisonCalculationCat() { Name = "dps20", OverallPoints = dps20, DPSPoints = dps20 },
						new ComparisonCalculationCat() { Name = "dps25", OverallPoints = dps25, DPSPoints = dps25 },
						new ComparisonCalculationCat() { Name = "dps50", OverallPoints = dps50, DPSPoints = dps50 },
						new ComparisonCalculationCat() { Name = "dps75", OverallPoints = dps75, DPSPoints = dps75 },
						new ComparisonCalculationCat() { Name = "dps83", OverallPoints = dps83, DPSPoints = dps83 },
						new ComparisonCalculationCat() { Name = "dps100", OverallPoints = dps100, DPSPoints = dps100 },
						new ComparisonCalculationCat() { Name = "dps200", OverallPoints = dps200, DPSPoints = dps200 },
						new ComparisonCalculationCat() { Name = "dps300", OverallPoints = dps300, DPSPoints = dps300 },
						new ComparisonCalculationCat() { Name = "dps400", OverallPoints = dps400, DPSPoints = dps400 },
						new ComparisonCalculationCat() { Name = "dps500", OverallPoints = dps500, DPSPoints = dps500 },
					};
					
					break;

				case "Relative Stat Values":
					float dpsBase =		GetCharacterCalculations(character).OverallPoints;
					//float dpsStr =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = 10 } }).OverallPoints - dpsBase) / 10f;
					//float dpsAgi =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 10 } }).OverallPoints - dpsBase) / 10f;
					//float dpsAP  =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { AttackPower = 1 } }).OverallPoints - dpsBase) / 10f;
					float dpsCrit =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 1 } }).OverallPoints - dpsBase);
					float dpsExp =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { ExpertiseRating = 1 } }).OverallPoints - dpsBase);
					float dpsHaste =	(GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 1 } }).OverallPoints - dpsBase);
					float dpsHit =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 1 } }).OverallPoints - dpsBase);
					float dpsDmg =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { WeaponDamage = 1 } }).OverallPoints - dpsBase);
					float dpsPen =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { ArmorPenetrationRating = 1 } }).OverallPoints - dpsBase);

					//Differential Calculations for Agi
					float dpsAtAdd = dpsBase;
					float agiToAdd = 0f;
					while (dpsBase == dpsAtAdd && agiToAdd < 2)
					{
						agiToAdd += 0.01f;
						dpsAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToAdd } }).OverallPoints;
					}

					float dpsAtSubtract = dpsBase;
					float agiToSubtract = 0f;
					while (dpsBase == dpsAtSubtract && agiToSubtract > -2)
					{
						agiToSubtract -= 0.01f;
						dpsAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToSubtract } }).OverallPoints;
					}
					agiToSubtract += 0.01f;

					ComparisonCalculationCat comparisonAgi = new ComparisonCalculationCat()
					{
						Name = "Agility",
						OverallPoints = (dpsAtAdd - dpsBase) / (agiToAdd - agiToSubtract),
						DPSPoints = (dpsAtAdd - dpsBase) / (agiToAdd - agiToSubtract),
					};


					//Differential Calculations for Str
					dpsAtAdd = dpsBase;
					float strToAdd = 0f;
					while (dpsBase == dpsAtAdd && strToAdd < 2)
					{
						strToAdd += 0.01f;
						dpsAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToAdd } }).OverallPoints;
					}

					dpsAtSubtract = dpsBase;
					float strToSubtract = 0f;
					while (dpsBase == dpsAtSubtract && strToSubtract > -2)
					{
						strToSubtract -= 0.01f;
						dpsAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToSubtract } }).OverallPoints;
					}
					strToSubtract += 0.01f;

					ComparisonCalculationCat comparisonStr = new ComparisonCalculationCat()
					{
						Name = "Strength",
						OverallPoints = (dpsAtAdd - dpsBase) / (strToAdd - strToSubtract),
						DPSPoints = (dpsAtAdd - dpsBase) / (strToAdd - strToSubtract),
					};


					//Differential Calculations for AP
					dpsAtAdd = dpsBase;
					float apToAdd = 0f;
					while (dpsBase == dpsAtAdd && apToAdd < 2)
					{
						apToAdd += 0.01f;
						dpsAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { AttackPower = apToAdd } }).OverallPoints;
					}

					dpsAtSubtract = dpsBase;
					float apToSubtract = 0f;
					while (dpsBase == dpsAtSubtract && apToSubtract > -2)
					{
						apToSubtract -= 0.01f;
						dpsAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { AttackPower = apToSubtract } }).OverallPoints;
					}
					apToSubtract += 0.01f;

					ComparisonCalculationCat comparisonAP = new ComparisonCalculationCat()
					{
						Name = "Attack Power",
						OverallPoints = (dpsAtAdd - dpsBase) / (apToAdd - apToSubtract),
						DPSPoints = (dpsAtAdd - dpsBase) / (apToAdd - apToSubtract),
					};



					return new ComparisonCalculationBase[] { 
						comparisonAgi,
						comparisonStr,
						comparisonAP,
						new ComparisonCalculationCat() { Name = "Crit Rating", OverallPoints = dpsCrit, DPSPoints = dpsCrit },
						new ComparisonCalculationCat() { Name = "Expertise Rating", OverallPoints = dpsExp, DPSPoints = dpsExp },
						new ComparisonCalculationCat() { Name = "Haste Rating", OverallPoints = dpsHaste, DPSPoints = dpsHaste },
						new ComparisonCalculationCat() { Name = "Hit Rating", OverallPoints = dpsHit, DPSPoints = dpsHit },
						new ComparisonCalculationCat() { Name = "Weapon Damage", OverallPoints = dpsDmg, DPSPoints = dpsDmg },
						new ComparisonCalculationCat() { Name = "Armor Penetration", OverallPoints = dpsPen, DPSPoints = dpsPen }
					};

				default:
					return new ComparisonCalculationBase[0];
			}
		}

		public override bool IsItemRelevant(Item item)
		{
			if (item.Slot == Item.ItemSlot.OffHand || 
				(item.Slot == Item.ItemSlot.Ranged && item.Type != Item.ItemType.Idol)) 
				return false;
			return base.IsItemRelevant(item);
		}

		public override Stats GetRelevantStats(Stats stats)
		{
			return new Stats()
				{
					Agility = stats.Agility,
					Strength = stats.Strength,
					CatFormStrength = stats.CatFormStrength,
					AttackPower = stats.AttackPower,
					CritRating = stats.CritRating,
					HitRating = stats.HitRating,
					Stamina = stats.Stamina,
					HasteRating = stats.HasteRating,
					ExpertiseRating = stats.ExpertiseRating,
					ArmorPenetration = stats.ArmorPenetration,
					ArmorPenetrationRating = stats.ArmorPenetrationRating,
					BloodlustProc = stats.BloodlustProc,
					TerrorProc = stats.TerrorProc,
					BonusMangleCatDamage = stats.BonusMangleCatDamage,
					BonusShredDamage = stats.BonusShredDamage,
					BonusRipDamagePerCPPerTick = stats.BonusRipDamagePerCPPerTick,
					WeaponDamage = stats.WeaponDamage,
					BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
					BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
					BonusCritMultiplier = stats.BonusCritMultiplier,
					BonusDamageMultiplier = stats.BonusDamageMultiplier,
					BonusRipDamageMultiplier = stats.BonusRipDamageMultiplier,
					BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
					BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                    BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                    BonusArcaneDamageMultiplier = stats.BonusArcaneDamageMultiplier,
					Health = stats.Health,
					MangleCatCostReduction = stats.MangleCatCostReduction,
					TigersFuryCooldownReduction = stats.TigersFuryCooldownReduction,
					ExposeWeakness = stats.ExposeWeakness,
					Bloodlust = stats.Bloodlust,
					DrumsOfBattle = stats.DrumsOfBattle,
					DrumsOfWar = stats.DrumsOfWar,
					ShatteredSunMightProc = stats.ShatteredSunMightProc,
					ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
					PhysicalHaste = stats.PhysicalHaste,
					PhysicalHit = stats.PhysicalHit,
					MongooseProc = stats.MongooseProc,
					BerserkingProc = stats.BerserkingProc,
					BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
					PhysicalCrit = stats.PhysicalCrit,

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
					BonusRipDuration = stats.BonusRipDuration,
                    GreatnessProc = stats.GreatnessProc
				};
		}

		public override bool HasRelevantStats(Stats stats)
		{
			return (stats.Agility + stats.ArmorPenetration + stats.AttackPower + stats.BloodlustProc + stats.PhysicalCrit +
				stats.BonusAgilityMultiplier + stats.BonusAttackPowerMultiplier + stats.BonusCritMultiplier +
				stats.BonusMangleCatDamage + stats.BonusDamageMultiplier + stats.BonusRipDamageMultiplier + stats.BonusShredDamage +
				stats.BonusStaminaMultiplier + stats.BonusStrengthMultiplier + stats.CritRating + stats.ExpertiseRating +
				stats.HasteRating + /*stats.Health +*/ stats.HitRating + stats.MangleCatCostReduction + /*stats.Stamina +*/
				stats.Strength + stats.CatFormStrength + stats.TerrorProc + stats.WeaponDamage + stats.ExposeWeakness + stats.Bloodlust +
				stats.PhysicalHit + stats.BonusRipDamagePerCPPerTick + stats.ShatteredSunMightProc + stats.MongooseProc +
				stats.PhysicalHaste + stats.ArmorPenetrationRating + stats.BonusRipDuration + stats.BerserkingProc +
				stats.BonusSpellPowerMultiplier + stats.BonusArcaneDamageMultiplier + stats.ThreatReductionMultiplier + stats.AllResist +
				stats.ArcaneResistance + stats.NatureResistance + stats.FireResistance + stats.BonusBleedDamageMultiplier +
				stats.FrostResistance + stats.ShadowResistance + stats.ArcaneResistanceBuff + stats.TigersFuryCooldownReduction + stats.GreatnessProc +
				stats.NatureResistanceBuff + stats.FireResistanceBuff + stats.BonusShredDamageMultiplier + stats.BonusPhysicalDamageMultiplier +
				stats.FrostResistanceBuff + stats.ShadowResistanceBuff) > 0 || (stats.Stamina > 0 && stats.SpellPower == 0);
		}
	}

    public class CharacterCalculationsCat : CharacterCalculationsBase
    {
		private float _overallPoints = 0f;
		public override float OverallPoints
		{
			get { return _overallPoints; }
			set { _overallPoints = value; }
		}

		private float[] _subPoints = new float[] { 0f, 0f };
		public override float[] SubPoints
		{
			get { return _subPoints; }
			set { _subPoints = value; }
		}

		public float DPSPoints
		{
			get { return _subPoints[0]; }
			set { _subPoints[0] = value; }
		}

		public float SurvivabilityPoints
		{
			get { return _subPoints[1]; }
			set { _subPoints[1] = value; }
		}

		public Stats BasicStats { get; set; }
		public int TargetLevel { get; set; }

		public float AvoidedAttacks { get; set; }
		public float DodgedAttacks { get; set; }
		public float MissedAttacks { get; set; }
		public float CritChance { get; set; }
		public float AttackSpeed { get; set; }
		public float ArmorMitigation { get; set; }

		public float MeleeDamagePerHit { get; set; }
		public float MangleDamagePerHit { get; set; }
		public float ShredDamagePerHit { get; set; }
		public float RakeDamagePerHit { get; set; }
		public float RipDamagePerHit { get; set; }
		public float BiteDamagePerHit { get; set; }

		public float MeleeDamagePerSwing { get; set; }
		public float MangleDamagePerSwing { get; set; }
		public float ShredDamagePerSwing { get; set; }
		public float RakeDamagePerSwing { get; set; }
		public float RipDamagePerSwing { get; set; }
		public float BiteDamagePerSwing { get; set; }

		public CatRotationCalculator.CatRotationCalculation HighestDPSRotation { get; set; }
		public CatRotationCalculator.CatRotationCalculation CustomRotation { get; set; }
		
		public string Rotations { get; set; }

		public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
		{
			Dictionary<string, string> dictValues = new Dictionary<string, string>();
			dictValues.Add("Overall Points", OverallPoints.ToString());
			dictValues.Add("DPS Points", DPSPoints.ToString());
			dictValues.Add("Survivability Points", SurvivabilityPoints.ToString());
			
			dictValues.Add("Health", BasicStats.Health.ToString());
			dictValues.Add("Attack Power", BasicStats.AttackPower.ToString());
			dictValues.Add("Agility", BasicStats.Agility.ToString());
			dictValues.Add("Strength", BasicStats.Strength.ToString());
			dictValues.Add("Crit Rating", BasicStats.CritRating.ToString());
			dictValues.Add("Hit Rating", BasicStats.HitRating.ToString());
			dictValues.Add("Expertise Rating", BasicStats.ExpertiseRating.ToString());
			dictValues.Add("Haste Rating", BasicStats.HasteRating.ToString());
			dictValues.Add("Armor Penetration Rating", BasicStats.ArmorPenetrationRating.ToString());
			dictValues.Add("Weapon Damage", "+" + BasicStats.WeaponDamage.ToString());
			
			dictValues.Add("Avoided Attacks", string.Format("{0}%*{1}% Dodged, {2}% Missed", AvoidedAttacks, DodgedAttacks, MissedAttacks));
			dictValues.Add("Crit Chance", CritChance.ToString() + "%");
			dictValues.Add("Attack Speed", AttackSpeed.ToString() + "s");
			dictValues.Add("Armor Mitigation", ArmorMitigation.ToString() + "%");

			string attackFormat = "{0}%*Damage Per Hit: {1}, Damage Per Swing: {2}";
			dictValues.Add("Melee Damage", string.Format(attackFormat, 100f * HighestDPSRotation.MeleeDamageTotal / HighestDPSRotation.DamageTotal, MeleeDamagePerHit, MeleeDamagePerSwing));
			dictValues.Add("Mangle Damage", string.Format(attackFormat, 100f * HighestDPSRotation.MangleDamageTotal / HighestDPSRotation.DamageTotal, MangleDamagePerHit, MangleDamagePerSwing));
			dictValues.Add("Shred Damage", string.Format(attackFormat, 100f * HighestDPSRotation.ShredDamageTotal / HighestDPSRotation.DamageTotal, ShredDamagePerHit, ShredDamagePerSwing));
			dictValues.Add("Rake Damage", string.Format(attackFormat, 100f * HighestDPSRotation.RakeDamageTotal / HighestDPSRotation.DamageTotal, RakeDamagePerHit, RakeDamagePerSwing));
			dictValues.Add("Rip Damage", string.Format(attackFormat, 100f * HighestDPSRotation.RipDamageTotal / HighestDPSRotation.DamageTotal, RipDamagePerHit, RipDamagePerSwing));
			dictValues.Add("Bite Damage", string.Format(attackFormat, 100f * HighestDPSRotation.BiteDamageTotal / HighestDPSRotation.DamageTotal, BiteDamagePerHit, BiteDamagePerSwing));

			string rotationDescription = string.Format("{0}*Keep {1}cp Savage Roar up.\r\nKeep Rake up.\r\n{2}{3}{4}Use {5} for combo points.",
				HighestDPSRotation.Name.Replace(" + ", "+"), HighestDPSRotation.RoarCP,
				HighestDPSRotation.Name.Contains("Rip") ? "Keep 5cp Rip up.\r\n" : "",
				HighestDPSRotation.Name.Contains("Mangle") ? "Keep Mangle up.\r\n" : "",
				HighestDPSRotation.Name.Contains("Bite") ? "Use Ferocious Bite to use up extra combo points.\r\n" : "",
				HighestDPSRotation.Name.Contains("Shred") ? "Shred" : "Mangle");

			dictValues.Add("Optimal Rotation", rotationDescription);
			dictValues.Add("Optimal Rotation DPS", HighestDPSRotation.DPS.ToString());
			dictValues.Add("Custom Rotation DPS", CustomRotation.DPS.ToString());
			
			return dictValues;
		}

		public override float GetOptimizableCalculationValue(string calculation)
		{
			switch (calculation)
			{
				case "Health": return BasicStats.Health;
				case "Avoided Attacks %": return AvoidedAttacks;
				case "Nature Resist": return BasicStats.NatureResistance;
				case "Fire Resist": return BasicStats.FireResistance;
				case "Frost Resist": return BasicStats.FrostResistance;
				case "Shadow Resist": return BasicStats.ShadowResistance;
				case "Arcane Resist": return BasicStats.ArcaneResistance;
				case "Custom Rotation DPS": return CustomRotation.DPS;
			}
			return 0f;
		}
    }

	public class ComparisonCalculationCat : ComparisonCalculationBase
	{
		private string _name = string.Empty;
		public override string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private float _overallPoints = 0f;
		public override float OverallPoints
		{
			get { return _overallPoints; }
			set { _overallPoints = value; }
		}

		private float[] _subPoints = new float[] { 0f, 0f };
		public override float[] SubPoints
		{
			get { return _subPoints; }
			set { _subPoints = value; }
		}

		public float DPSPoints
		{
			get { return _subPoints[0]; }
			set { _subPoints[0] = value; }
		}

		public float SurvivabilityPoints
		{
			get { return _subPoints[1]; }
			set { _subPoints[1] = value; }
		}

		private Item _item = null;
		public override Item Item
		{
			get { return _item; }
			set { _item = value; }
		}

        private ItemInstance _itemInstance = null;
        public override ItemInstance ItemInstance
        {
            get { return _itemInstance; }
            set { _itemInstance = value; }
        }

		private bool _equipped = false;
		public override bool Equipped
		{
			get { return _equipped; }
			set { _equipped = value; }
		}

		public override string ToString()
		{
			return string.Format("{0}: ({1}O {2}DPS)", Name, Math.Round(OverallPoints), Math.Round(DPSPoints));
		}
	}
}
