using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Rawr.Cat
{
	[Rawr.Calculations.RawrModelInfo("Cat", "Ability_Druid_CatForm", CharacterClass.Druid)]
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
				// int austere = 41380;
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

		private ICalculationOptionsPanel _calculationOptionsPanel = null;
		public override ICalculationOptionsPanel CalculationOptionsPanel
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
					//"Hit Test",
						//"Relative Stat Values",
					};
				return _customChartNames;
			}
		}

		private Dictionary<string, Color> _subPointNameColors = null;
		public override Dictionary<string, Color> SubPointNameColors
		{
			get
			{
				if (_subPointNameColors == null)
				{
					_subPointNameColors = new Dictionary<string, Color>();
					_subPointNameColors.Add("DPS", Color.FromArgb(255, 160, 0, 224));
					_subPointNameColors.Add("Survivability", Color.FromArgb(255, 64, 128, 32));
				}
				return _subPointNameColors;
			}
		}

		private List<ItemType> _relevantItemTypes = null;
		public override List<ItemType> RelevantItemTypes
		{
			get
			{
				if (_relevantItemTypes == null)
				{
					_relevantItemTypes = new List<ItemType>(new ItemType[]
					{
						ItemType.None,
						ItemType.Leather,
						ItemType.Idol,
						ItemType.Staff,
						ItemType.TwoHandMace,
						ItemType.Polearm
					});
				}
				return _relevantItemTypes;
			}
		}

		public override CharacterClass TargetClass { get { return CharacterClass.Druid; } }
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

		public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
		{
			//_cachedCharacter = character;
			CalculationOptionsCat calcOpts = character.CalculationOptions as CalculationOptionsCat;
			if (calcOpts == null) calcOpts = new CalculationOptionsCat();
			int targetLevel = calcOpts.TargetLevel;
			float targetArmor = calcOpts.TargetArmor;
			float tempArPenRating, tempArPenRatingUptime;
			Stats stats = GetCharacterStatsWithTemporaryArPen(character, additionalItem, out tempArPenRating, out tempArPenRatingUptime);
			float levelDifference = (targetLevel - 80f) * 0.2f;
			CharacterCalculationsCat calculatedStats = new CharacterCalculationsCat();
			calculatedStats.BasicStats = stats;
			calculatedStats.TargetLevel = targetLevel;
			bool maintainMangle = stats.BonusBleedDamageMultiplier == 0f;
			stats.BonusBleedDamageMultiplier = 0.3f;

			#region Basic Chances and Constants
			float modArmorStatic = 1f - StatConversion.GetArmorDamageReduction(character.Level, calcOpts.TargetArmor,
				stats.ArmorPenetration, 0f, stats.ArmorPenetrationRating);
			float modArmorTemporary = 1f - StatConversion.GetArmorDamageReduction(character.Level, calcOpts.TargetArmor,
				stats.ArmorPenetration, 0f, stats.ArmorPenetrationRating + tempArPenRating);

			float modArmor = modArmorStatic + (modArmorTemporary - modArmorStatic) * tempArPenRatingUptime;

			float critMultiplier = 2f * (1f + stats.BonusCritMultiplier);
			float critMultiplierBleed = 2f * (1f + stats.BonusCritMultiplier);
			float hasteBonus = StatConversion.GetPhysicalHasteFromRating(stats.HasteRating, CharacterClass.Druid);//stats.HasteRating * 1.3f / 32.78998947f / 100f;
			hasteBonus = (1f + hasteBonus) * (1f + stats.Bloodlust * 40f / Math.Max(calcOpts.Duration, 40f)) - 1f;
			float attackSpeed = 1f / (1f + hasteBonus);
			attackSpeed = attackSpeed / (1f + stats.PhysicalHaste);

			float hitBonus = stats.PhysicalHit + StatConversion.GetPhysicalHitFromRating(stats.HitRating);//stats.HitRating / 32.78998947f / 100f + stats.PhysicalHit;
			float expertiseBonus = (StatConversion.GetExpertiseFromRating(stats.ExpertiseRating) + stats.Expertise) * 0.0025f;//stats.ExpertiseRating / 32.78998947f / 100f + stats.Expertise * 0.0025f;

			float chanceDodge = Math.Max(0f, 0.065f + .005f * (targetLevel - 83) - expertiseBonus);
			float chanceMiss = Math.Max(0f, 0.08f - hitBonus);
			if ((targetLevel - 80f) < 3)
			{
				chanceMiss = Math.Max(0f, 0.05f + 0.005f * (targetLevel - 80f) - hitBonus);
			}

			float glanceMultiplier = .7f;
			float chanceAvoided = chanceMiss + chanceDodge;
			float chanceGlance = 0.24f;
			float chanceCrit = StatConversion.GetCritFromRating(stats.CritRating) + stats.PhysicalCrit +
				StatConversion.GetCritFromAgility(stats.Agility, CharacterClass.Druid) //(stats.CritRating / 45.90598679f + stats.Agility * 0.012f) / 100f + stats.PhysicalCrit 
				- (0.006f * (targetLevel - character.Level) + (targetLevel == 83 ? 0.03f : 0f));
			float chanceCritBleed = character.DruidTalents.PrimalGore > 0 ? chanceCrit : 0f;
			float chanceHit = 1f - chanceCrit - chanceAvoided;
			float chanceHitNonGlance = 1f - chanceCrit - chanceAvoided - chanceGlance;
			float chanceNonAvoided = 1f - chanceAvoided;
			float chanceCritBite = Math.Min(1f - chanceAvoided, chanceCrit + stats.BonusFerociousBiteCrit);
			float chanceHitBite = 1f - chanceCritBite - chanceAvoided;

			float timeToReapplyDebuffs = 1f / (1f - chanceAvoided) - 1f;

			float cpPerCPG = (chanceHit + chanceCrit * (1f + stats.BonusCPOnCrit)) / chanceNonAvoided;
			calculatedStats.DodgedAttacks = chanceDodge * 100f;
			calculatedStats.MissedAttacks = chanceMiss * 100f;
			#endregion

			#region Attack Damages
			float baseDamage = 55f + (stats.AttackPower / 14f) + stats.WeaponDamage;
			float meleeDamageRaw = (baseDamage) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * modArmor;
			float mangleDamageRaw = (baseDamage * 2f + 634 + stats.BonusMangleCatDamage) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusMangleDamageMultiplier) * modArmor;
			float shredDamageRaw = (baseDamage * 2.25f + 742.5f + stats.BonusShredDamage) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusShredDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier) * modArmor;
			float rakeDamageRaw = (190f + stats.AttackPower * 0.01f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusRakeDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier);
			float rakeDamageDot = (1161f + stats.AttackPower * 0.18f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusRakeDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier);
			float ripDamageRaw = (3204f + stats.AttackPower * 0.3f + (stats.BonusRipDamagePerCPPerTick * 5f * 6f)) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusRipDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier);
			float biteBaseDamageRaw = 190f * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusFerociousBiteDamageMultiplier) * modArmor;
			float biteCPDamageRaw = (290f + stats.AttackPower * 0.07f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusFerociousBiteDamageMultiplier) * modArmor;

			float meleeDamageAverage = chanceGlance * meleeDamageRaw * glanceMultiplier +
										chanceCrit * meleeDamageRaw * critMultiplier +
										chanceHitNonGlance * meleeDamageRaw;
			float mangleDamageAverage = (1f - chanceCrit) * mangleDamageRaw + chanceCrit * mangleDamageRaw * critMultiplier;
			float shredDamageAverage = (1f - chanceCrit) * shredDamageRaw + chanceCrit * shredDamageRaw * critMultiplier;
			float rakeDamageAverage = ((1f - chanceCrit) * rakeDamageRaw + chanceCrit * rakeDamageRaw * critMultiplier) + rakeDamageDot;
			float ripDamageAverage = ((1f - chanceCritBleed) * ripDamageRaw + chanceCritBleed * ripDamageRaw * critMultiplierBleed);
			float biteBaseDamageAverage = (1f - chanceCritBite) * biteBaseDamageRaw + chanceCritBite * biteBaseDamageRaw * critMultiplier;
			float biteCPDamageAverage = (1f - chanceCritBite) * biteCPDamageRaw + chanceCritBite * biteCPDamageRaw * critMultiplier;
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
				maintainMangle, (character.DruidTalents.GlyphOfMangle ? 18f : 12f) + timeToReapplyDebuffs,
				12f + stats.BonusRipDuration + timeToReapplyDebuffs, 9f + timeToReapplyDebuffs, stats.BonusSavageRoarDuration,
				character.DruidTalents.Berserk > 0 ? (character.DruidTalents.GlyphOfBerserk ? 20f : 15f) : 0f, attackSpeed,
				character.DruidTalents.OmenOfClarity > 0, character.DruidTalents.GlyphOfShred, chanceAvoided, chanceCrit * stats.BonusCPOnCrit,
				cpgEnergyCostMultiplier, stats.ClearcastOnBleedChance, meleeDamageAverage, mangleDamageAverage, shredDamageAverage,
				rakeDamageAverage, ripDamageAverage, biteBaseDamageAverage, biteCPDamageAverage, mangleEnergyAverage, shredEnergyAverage,
				rakeEnergyAverage, ripEnergyAverage, biteEnergyAverage, roarEnergyAverage);
			CatRotationCalculator.CatRotationCalculation rotationCalculationDPS = new CatRotationCalculator.CatRotationCalculation();

			//StringBuilder rotations = new StringBuilder();
			for (int roarCP = 1; roarCP < 6; roarCP++)
				for (int biteCP = 0; biteCP < 6; biteCP++)
					for (int useRake = 0; useRake < 2; useRake++)
						for (int useShred = 0; useShred < 2; useShred++)
							for (int useRip = 0; useRip < 2; useRip++)
							{
								CatRotationCalculator.CatRotationCalculation rotationCalculation =
									rotationCalculator.GetRotationCalculations(
									useRake == 1, useShred == 1, useRip == 1, biteCP, roarCP);
								//rotations.AppendLine(rotationCalculation.Name + ": " + rotationCalculation.DPS + "DPS");
								if (rotationCalculation.DPS > rotationCalculationDPS.DPS)
									rotationCalculationDPS = rotationCalculation;
							}

			float ripDurationMultiplier = (character.DruidTalents.GlyphOfShred && rotationCalculationDPS.ShredDamageTotal > 0 ?
				rotationCalculator.RipDuration + 6 : rotationCalculator.RipDuration) / 12f;

			calculatedStats.HighestDPSRotation = rotationCalculationDPS;
			calculatedStats.CustomRotation = rotationCalculator.GetRotationCalculations(
				calcOpts.CustomUseRake, calcOpts.CustomUseShred, calcOpts.CustomUseRip, calcOpts.CustomCPFerociousBite, calcOpts.CustomCPSavageRoar);
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
			calculatedStats.BiteDamagePerHit = biteBaseDamageRaw + biteCPDamageRaw * 5f;
			calculatedStats.BiteDamagePerSwing = (biteBaseDamageAverage + biteCPDamageAverage * 5f) * chanceNonAvoided;

			float magicDPS = (stats.ShadowDamage + stats.ArcaneDamage) * (1f + chanceCrit);
			calculatedStats.DPSPoints = calculatedStats.HighestDPSRotation.DPS + magicDPS;
			calculatedStats.SurvivabilityPoints = stats.Health / 100f;
			calculatedStats.OverallPoints = calculatedStats.DPSPoints + calculatedStats.SurvivabilityPoints;
			return calculatedStats;
		}

		public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
			float arPenRating, arPenRatingUptime;
			return GetCharacterStatsWithTemporaryArPen(character, additionalItem, out arPenRating, out arPenRatingUptime);
		}

		private Stats GetCharacterStatsWithTemporaryArPen(Character character, Item additionalItem, out float tempArPenRating, out float tempArPenRatingUptime)
		{
			CalculationOptionsCat calcOpts = character.CalculationOptions as CalculationOptionsCat;

			Stats statsRace = character.Race == CharacterRace.NightElf ?
				new Stats()
				{
					Health = 7417f,
					Strength = 86f,
					Agility = 87f,
					Stamina = 97f,
					Dodge = 0.04951f,
					AttackPower = 140f,
					BonusPhysicalDamageMultiplier = character.DruidTalents.GlyphOfSavageRoar ? 0.33f : 0.3f, //Savage Roar
					PhysicalCrit = 0.07476f
				} :
				new Stats()
				{
					Health = 7599f,
					Strength = 95f,
					Agility = 75f,
					Stamina = 100f,
					Dodge = 0.04951f,
					AttackPower = 140f,
					BonusPhysicalDamageMultiplier = character.DruidTalents.GlyphOfSavageRoar ? 0.33f : 0.3f, //Savage Roar
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
				PhysicalCrit = 0.02f * talents.SharpenedClaws + ((character.ActiveBuffsContains("Leader of the Pack") ||
				character.ActiveBuffsContains("Rampage")) ?
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
				BonusFerociousBiteCrit = 0.05f * talents.RendAndTear,
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
				BonusRipDuration = talents.GlyphOfRip ? 4f : 0f,
			};

			Stats statsGearEnchantsBuffs = statsItems + statsBuffs;
			statsGearEnchantsBuffs.Agility += statsGearEnchantsBuffs.AverageAgility;
			statsGearEnchantsBuffs.Strength += statsGearEnchantsBuffs.CatFormStrength;

			Stats statsTotal = statsRace + statsItems + statsBuffs + statsTalents;

			Stats statsWeapon = character.MainHand == null ? new Stats() : character.MainHand.GetTotalStats(character).Clone();
			statsWeapon.Strength *= (1f + statsTotal.BonusStrengthMultiplier);
			statsWeapon.AttackPower += statsWeapon.Strength * 2f;
			if (character.MainHand != null)
			{
				float fap = Math.Max(0f, (character.MainHand.Item.DPS - 54.8f) * 14f); //TODO Find a more accurate number for this?
				statsTotal.AttackPower += fap;
				statsWeapon.AttackPower += fap;
			}

			statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
			statsTotal.Strength = (float)Math.Floor(statsTotal.Strength * (1f + statsTotal.BonusStrengthMultiplier));
			//statsTotal.Agility += statsTotal.HighestStat;
			statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1f + statsTotal.BonusAgilityMultiplier));
			statsTotal.AttackPower += statsTotal.Strength * 2f + statsTotal.Agility;
			statsTotal.AttackPower += statsWeapon.AttackPower * 0.2f * (talents.PredatoryStrikes / 3f);
			statsTotal.AttackPower = (float)Math.Floor(statsTotal.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
			statsTotal.Health += (float)Math.Floor((statsTotal.Stamina - 20f) * 10f + 20f);
			statsTotal.Armor += 2f * statsTotal.Agility;
			statsTotal.Armor = (float)Math.Floor(statsTotal.Armor * (1f + statsTotal.BonusArmorMultiplier));
			statsTotal.NatureResistance += statsTotal.NatureResistanceBuff + statsTotal.AllResist;
			statsTotal.FireResistance += statsTotal.FireResistanceBuff + statsTotal.AllResist;
			statsTotal.FrostResistance += statsTotal.FrostResistanceBuff + statsTotal.AllResist;
			statsTotal.ShadowResistance += statsTotal.ShadowResistanceBuff + statsTotal.AllResist;
			statsTotal.ArcaneResistance += statsTotal.ArcaneResistanceBuff + statsTotal.AllResist;
			statsTotal.WeaponDamage += 16f; //Tiger's Fury

			//TODO: TEMPORARY! Remove this once we have ability-specific procs working!
			if (statsTotal.TerrorProc > 0)
			{
				float terrorUptime = 0.4f; //TODO: Calculate this
				statsTotal.Agility += statsTotal.TerrorProc * terrorUptime * (1f + statsTotal.BonusAgilityMultiplier);
				statsTotal.AttackPower += statsTotal.TerrorProc * terrorUptime * (1f + statsTotal.BonusAgilityMultiplier) * (1f + statsTotal.BonusAttackPowerMultiplier);
			}

			float hasteBonus = StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, CharacterClass.Druid);//stats.HasteRating * 1.3f / 32.78998947f / 100f;
			hasteBonus = (1f + hasteBonus) * (1f + statsTotal.PhysicalHaste) * (1f + statsTotal.Bloodlust * 40f / Math.Max(calcOpts.Duration, 40f)) - 1f;
			float meleeHitInterval = 1f / ((1f + hasteBonus) + 1f / 3.5f);
			float chanceCrit = StatConversion.GetCritFromRating(statsTotal.CritRating) + statsTotal.PhysicalCrit +
				StatConversion.GetCritFromAgility(statsTotal.Agility, CharacterClass.Druid) //(stats.CritRating / 45.90598679f + stats.Agility * 0.012f) / 100f + stats.PhysicalCrit 
				- (0.006f * (calcOpts.TargetLevel - character.Level) + (calcOpts.TargetLevel == 83 ? 0.03f : 0f));

			Stats statsProcs = new Stats();

			tempArPenRating = 0f;
			tempArPenRatingUptime = 0f;
			List<float> tempArPenRatings = new List<float>();
			List<float> tempArPenRatingUptimes = new List<float>();
			foreach (SpecialEffect effect in statsTotal.SpecialEffects())
			{
				if (effect.Stats.ArmorPenetrationRating == 0)
				{
					switch (effect.Trigger)
					{
						case Trigger.Use:
							statsProcs += effect.GetAverageStats(0f, 1f, 1f, calcOpts.Duration);
							break;
						case Trigger.MeleeHit:
						case Trigger.PhysicalHit:
							statsProcs += effect.GetAverageStats(meleeHitInterval, 1f, 1f, calcOpts.Duration);
							break;
						case Trigger.MeleeCrit:
						case Trigger.PhysicalCrit:
							statsProcs += effect.GetAverageStats(meleeHitInterval, chanceCrit, 1f, calcOpts.Duration);
							break;
						case Trigger.DoTTick:
							statsProcs += effect.GetAverageStats(1.5f, 1f, 1f, calcOpts.Duration);
							break;
						case Trigger.DamageDone:
							statsProcs += effect.GetAverageStats(meleeHitInterval / 2f, 1f, 1f, calcOpts.Duration);
							break;
					}
				}
				else
				{
					switch (effect.Trigger)
					{
						case Trigger.Use:
							tempArPenRatings.Add(effect.Stats.ArmorPenetrationRating);
							tempArPenRatingUptimes.Add(effect.GetAverageUptime(0f, 1f, 1f, calcOpts.Duration));
							break;
						case Trigger.MeleeHit:
						case Trigger.PhysicalHit:
							tempArPenRatings.Add(effect.Stats.ArmorPenetrationRating);
							tempArPenRatingUptimes.Add(effect.GetAverageUptime(meleeHitInterval, 1f, 1f, calcOpts.Duration));
							break;
						case Trigger.MeleeCrit:
						case Trigger.PhysicalCrit:
							tempArPenRatings.Add(effect.Stats.ArmorPenetrationRating);
							tempArPenRatingUptimes.Add(effect.GetAverageUptime(meleeHitInterval, chanceCrit, 1f, calcOpts.Duration));
							break;
						case Trigger.DoTTick:
							tempArPenRatings.Add(effect.Stats.ArmorPenetrationRating);
							tempArPenRatingUptimes.Add(effect.GetAverageUptime(1.5f, 1f, 1f, calcOpts.Duration));
							break;
						case Trigger.DamageDone:
							tempArPenRatings.Add(effect.Stats.ArmorPenetrationRating);
							tempArPenRatingUptimes.Add(effect.GetAverageUptime(meleeHitInterval / 2f, 1f, 1f, calcOpts.Duration));
							break;
					}
				}
			}
			float totalTempArPenRating = 0f, totalTempArPenRatingUptime = 0f;
			for (int i = 0; i < tempArPenRatings.Count; i++)
			{
				totalTempArPenRating += tempArPenRatings[i];
				totalTempArPenRatingUptime += tempArPenRatingUptimes[i];
			}
			tempArPenRating = totalTempArPenRating;
			for (int i = 0; i < tempArPenRatings.Count; i++)
			{
				tempArPenRatingUptime += tempArPenRatingUptimes[i] * (tempArPenRatings[i] / totalTempArPenRating);
			}

			statsProcs.Agility += statsProcs.HighestStat;
			statsProcs.Stamina = (float)Math.Floor(statsProcs.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
			statsProcs.Strength = (float)Math.Floor(statsProcs.Strength * (1f + statsTotal.BonusStrengthMultiplier));
			statsProcs.Agility = (float)Math.Floor(statsProcs.Agility * (1f + statsTotal.BonusAgilityMultiplier));
			statsProcs.AttackPower += statsProcs.Strength * 2f + statsProcs.Agility;
			statsProcs.AttackPower = (float)Math.Floor(statsProcs.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
			statsProcs.Health += (float)Math.Floor(statsProcs.Stamina * 10f);
			statsProcs.Armor += 2f * statsProcs.Agility;
			statsProcs.Armor = (float)Math.Floor(statsProcs.Armor * (1f + statsTotal.BonusArmorMultiplier));
			statsTotal += statsProcs;

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
					float dpsBaseAgi = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = -50 } }).OverallPoints);
					ComparisonCalculationCat[] calcs = new ComparisonCalculationCat[101];
					for (int i = -50; i <= 50; i++)
					{
						float dps = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = i } }).OverallPoints - dpsBaseAgi);
						calcs[i + 50] = new ComparisonCalculationCat() { Name = "dps" + i.ToString(), OverallPoints = dps, DPSPoints = dps };
					}
					return calcs;
					// Jothay's Note: Unreachable Code Detected
					float dps1 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 1 } }).OverallPoints - dpsBaseHit);
					float dps2 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 2 } }).OverallPoints - dpsBaseHit);
					float dps3 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 3 } }).OverallPoints - dpsBaseHit);
					float dps4 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 4 } }).OverallPoints - dpsBaseHit);
					float dps5 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 5 } }).OverallPoints - dpsBaseHit);
					float dps6 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 6 } }).OverallPoints - dpsBaseHit);
					float dps7 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 7 } }).OverallPoints - dpsBaseHit);
					float dps8 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 8 } }).OverallPoints - dpsBaseHit);
					float dps9 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 9 } }).OverallPoints - dpsBaseHit);
					float dps10 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 10 } }).OverallPoints - dpsBaseHit);
					float dps15 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 15 } }).OverallPoints - dpsBaseHit);
					float dps20 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 20 } }).OverallPoints - dpsBaseHit);
					float dps25 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 25 } }).OverallPoints - dpsBaseHit);
					float dps50 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 50 } }).OverallPoints - dpsBaseHit);
					float dps75 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 75 } }).OverallPoints - dpsBaseHit);
					float dps83 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 83 } }).OverallPoints - dpsBaseHit);
					float dps100 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 100 } }).OverallPoints - dpsBaseHit);
					float dps200 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 200 } }).OverallPoints - dpsBaseHit);
					float dps300 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 300 } }).OverallPoints - dpsBaseHit);
					float dps400 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 400 } }).OverallPoints - dpsBaseHit);
					float dps500 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 500 } }).OverallPoints - dpsBaseHit);

					return new ComparisonCalculationBase[] { 
						new ComparisonCalculationCat() { Name = "dps1", OverallPoints = dps1, DPSPoints = dps1 },
						new ComparisonCalculationCat() { Name = "dps2", OverallPoints = dps2, DPSPoints = dps2 },
						new ComparisonCalculationCat() { Name = "dps3", OverallPoints = dps3, DPSPoints = dps3 },
						new ComparisonCalculationCat() { Name = "dps4", OverallPoints = dps4, DPSPoints = dps4 },
						new ComparisonCalculationCat() { Name = "dps5", OverallPoints = dps5, DPSPoints = dps5 },
						new ComparisonCalculationCat() { Name = "dps6", OverallPoints = dps6, DPSPoints = dps6 },
						new ComparisonCalculationCat() { Name = "dps7", OverallPoints = dps7, DPSPoints = dps7 },
						new ComparisonCalculationCat() { Name = "dps8", OverallPoints = dps8, DPSPoints = dps8 },
						new ComparisonCalculationCat() { Name = "dps9", OverallPoints = dps9, DPSPoints = dps9 },
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

				case "Relative Stat Values":
					float dpsBase = GetCharacterCalculations(character).OverallPoints;
					//float dpsStr =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = 10 } }).OverallPoints - dpsBase) / 10f;
					//float dpsAgi =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 10 } }).OverallPoints - dpsBase) / 10f;
					//float dpsAP  =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { AttackPower = 1 } }).OverallPoints - dpsBase) / 10f;
					float dpsCrit = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 1 } }).OverallPoints - dpsBase);
					float dpsExp = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { ExpertiseRating = 1 } }).OverallPoints - dpsBase);
					float dpsHaste = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 1 } }).OverallPoints - dpsBase);
					float dpsHit = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 1 } }).OverallPoints - dpsBase);
					float dpsDmg = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { WeaponDamage = 1 } }).OverallPoints - dpsBase);
					float dpsPen = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { ArmorPenetrationRating = 1 } }).OverallPoints - dpsBase);

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
			if (item.Slot == ItemSlot.OffHand ||
				(item.Slot == ItemSlot.Ranged && item.Type != ItemType.Idol))
				return false;
			return base.IsItemRelevant(item);
		}

		public override Stats GetRelevantStats(Stats stats)
		{
			Stats s = new Stats()
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
				Health = stats.Health,
				MangleCatCostReduction = stats.MangleCatCostReduction,
				TigersFuryCooldownReduction = stats.TigersFuryCooldownReduction,
				ExposeWeakness = stats.ExposeWeakness,
				Bloodlust = stats.Bloodlust,
				ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
				PhysicalHaste = stats.PhysicalHaste,
				PhysicalHit = stats.PhysicalHit,
				BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
				PhysicalCrit = stats.PhysicalCrit,
				BonusSavageRoarDuration = stats.BonusSavageRoarDuration,
				ClearcastOnBleedChance = stats.ClearcastOnBleedChance,
				ArcaneDamage = stats.ArcaneDamage,
				ShadowDamage = stats.ShadowDamage,
				TerrorProc = stats.TerrorProc,
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
				BonusRipDuration = stats.BonusRipDuration,
			};
			foreach (SpecialEffect effect in stats.SpecialEffects())
			{
				if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit
				|| effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit || effect.Trigger == Trigger.DoTTick
					|| effect.Trigger == Trigger.DamageDone)
				{
					if (HasRelevantStats(effect.Stats))
					{
						s.AddSpecialEffect(effect);
					}
				}
			}
			return s;
		}

		public override bool HasRelevantStats(Stats stats)
		{
			bool relevant = (stats.Agility + stats.ArmorPenetration + stats.AttackPower + stats.BloodlustProc + stats.PhysicalCrit +
				stats.BonusAgilityMultiplier + stats.BonusAttackPowerMultiplier + stats.BonusCritMultiplier +
				stats.ClearcastOnBleedChance + stats.BonusSavageRoarDuration +
				stats.BonusMangleCatDamage + stats.BonusDamageMultiplier + stats.BonusRipDamageMultiplier + stats.BonusShredDamage +
				stats.BonusStaminaMultiplier + stats.BonusStrengthMultiplier + stats.CritRating + stats.ExpertiseRating +
				stats.HasteRating + /*stats.Health +*/ stats.HitRating + stats.MangleCatCostReduction + /*stats.Stamina +*/
				stats.Strength + stats.CatFormStrength + stats.WeaponDamage + stats.ExposeWeakness + stats.Bloodlust +
				stats.PhysicalHit + stats.BonusRipDamagePerCPPerTick + stats.TerrorProc +
				stats.PhysicalHaste + stats.ArmorPenetrationRating + stats.BonusRipDuration +
				stats.ThreatReductionMultiplier + stats.AllResist + stats.ArcaneDamage + stats.ShadowDamage +
				stats.ArcaneResistance + stats.NatureResistance + stats.FireResistance + stats.BonusBleedDamageMultiplier +
				stats.FrostResistance + stats.ShadowResistance + stats.ArcaneResistanceBuff + stats.TigersFuryCooldownReduction + stats.HighestStat +
				stats.NatureResistanceBuff + stats.FireResistanceBuff + stats.BonusShredDamageMultiplier + stats.BonusPhysicalDamageMultiplier +
				stats.FrostResistanceBuff + stats.ShadowResistanceBuff) > 0 || (stats.Stamina > 0 && stats.SpellPower == 0);

			foreach (SpecialEffect effect in stats.SpecialEffects())
			{
				if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit
					|| effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit)
				{
					relevant |= HasRelevantStats(effect.Stats);
					if (relevant) break;
				}
			}
			return relevant;
		}

		public override void SetDefaults(Character character)
		{
			character.ActiveBuffs.Add(Buff.GetBuffByName("Horn of Winter"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Battle Shout"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Unleashed Rage"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Moonkin Form"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Leader of the Pack"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Icy Talons"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Power Word: Fortitude"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Mark of the Wild"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Blessing of Kings"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Sunder Armor"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Faerie Fire"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Totem of Wrath"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Flask of Endless Rage"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Agility Food"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Bloodlust"));

			character.DruidTalents.GlyphOfSavageRoar = true;
			character.DruidTalents.GlyphOfShred = true;
			character.DruidTalents.GlyphOfRip = true;
		}

		private static List<string> _relevantGlyphs;
		public override List<string> GetRelevantGlyphs()
		{
			if (_relevantGlyphs == null)
			{
				_relevantGlyphs = new List<string>();
				_relevantGlyphs.Add("Glyph of Mangle");
				_relevantGlyphs.Add("Glyph of Shred");
				_relevantGlyphs.Add("Glyph of Rip");
				_relevantGlyphs.Add("Glyph of Berserk");
				_relevantGlyphs.Add("Glyph of Savage Roar");
			}
			return _relevantGlyphs;
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

			float baseMiss = (new float[] { 0.05f, 0.055f, 0.06f, 0.08f })[TargetLevel - 80] - BasicStats.PhysicalHit;
			float baseDodge = (new float[] { 0.05f, 0.055f, 0.06f, 0.065f })[TargetLevel - 80] - BasicStats.Expertise * 0.0025f;
			float capMiss = (float)Math.Ceiling(baseMiss * 100f * 32.78998947f);
			float capDodge = (float)Math.Ceiling(baseDodge * 100f * 32.78998947f);

			string tipMiss = string.Empty;
			if (BasicStats.HitRating > capMiss)
				tipMiss = string.Format("*Over the cap by {0} Hit Rating", BasicStats.HitRating - capMiss);
			else if (BasicStats.HitRating < capMiss)
				tipMiss = string.Format("*Under the cap by {0} Hit Rating", capMiss - BasicStats.HitRating);
			else
				tipMiss = "*Exactly at the cap";

			string tipDodge = string.Empty;
			if (BasicStats.ExpertiseRating > capDodge)
				tipDodge = string.Format("*Over the cap by {0} Expertise Rating", BasicStats.ExpertiseRating - capDodge);
			else if (BasicStats.ExpertiseRating < capDodge)
				tipDodge = string.Format("*Under the cap by {0} Expertise Rating", capDodge - BasicStats.ExpertiseRating);
			else
				tipDodge = "*Exactly at the cap";


			dictValues.Add("Health", BasicStats.Health.ToString());
			dictValues.Add("Attack Power", BasicStats.AttackPower.ToString());
			dictValues.Add("Agility", BasicStats.Agility.ToString());
			dictValues.Add("Strength", BasicStats.Strength.ToString());
			dictValues.Add("Crit Rating", BasicStats.CritRating.ToString());
			dictValues.Add("Hit Rating", BasicStats.HitRating.ToString() + tipMiss);
			dictValues.Add("Expertise Rating", BasicStats.ExpertiseRating.ToString() + tipDodge);
			dictValues.Add("Haste Rating", BasicStats.HasteRating.ToString());
			dictValues.Add("Armor Penetration Rating", BasicStats.ArmorPenetrationRating.ToString());
			dictValues.Add("Weapon Damage", "+" + BasicStats.WeaponDamage.ToString());

			dictValues.Add("Avoided Attacks", string.Format("{0}%*{1}% Dodged, {2}% Missed", AvoidedAttacks, DodgedAttacks, MissedAttacks));
			dictValues.Add("Crit Chance", CritChance.ToString() + "%");
			dictValues.Add("Attack Speed", AttackSpeed.ToString() + "s");
			dictValues.Add("Armor Mitigation", ArmorMitigation.ToString() + "%");

			string attackFormat = "{0}%*Damage Per Hit: {1}, Damage Per Swing: {2}\r\n{0}% of Total Damage, {3} Damage Done";
			dictValues.Add("Melee Damage", string.Format(attackFormat, 100f * HighestDPSRotation.MeleeDamageTotal / HighestDPSRotation.DamageTotal, MeleeDamagePerHit, MeleeDamagePerSwing, HighestDPSRotation.MeleeDamageTotal));
			dictValues.Add("Mangle Damage", string.Format(attackFormat, 100f * HighestDPSRotation.MangleDamageTotal / HighestDPSRotation.DamageTotal, MangleDamagePerHit, MangleDamagePerSwing, HighestDPSRotation.MangleDamageTotal));
			dictValues.Add("Shred Damage", string.Format(attackFormat, 100f * HighestDPSRotation.ShredDamageTotal / HighestDPSRotation.DamageTotal, ShredDamagePerHit, ShredDamagePerSwing, HighestDPSRotation.ShredDamageTotal));
			dictValues.Add("Rake Damage", string.Format(attackFormat, 100f * HighestDPSRotation.RakeDamageTotal / HighestDPSRotation.DamageTotal, RakeDamagePerHit, RakeDamagePerSwing, HighestDPSRotation.RakeDamageTotal));
			dictValues.Add("Rip Damage", string.Format(attackFormat, 100f * HighestDPSRotation.RipDamageTotal / HighestDPSRotation.DamageTotal, RipDamagePerHit, RipDamagePerSwing, HighestDPSRotation.RipDamageTotal));
			dictValues.Add("Bite Damage", string.Format(attackFormat, 100f * HighestDPSRotation.BiteDamageTotal / HighestDPSRotation.DamageTotal, BiteDamagePerHit, BiteDamagePerSwing, HighestDPSRotation.BiteDamageTotal));

			string rotationDescription = string.Empty;
			try
			{
				rotationDescription = string.Format("{0}*Keep {1}cp Savage Roar up.\r\n{2}{3}{4}{5}Use {6} for combo points.",
					HighestDPSRotation.Name.Replace(" + ", "+"), HighestDPSRotation.RoarCP,
					HighestDPSRotation.Name.Contains("Rake") ? "Keep Rake up.\r\n" : "",
					HighestDPSRotation.Name.Contains("Rip") ? "Keep 5cp Rip up.\r\n" : "",
					HighestDPSRotation.Name.Contains("Mangle") ? "Keep Mangle up.\r\n" : "",
					HighestDPSRotation.Name.Contains("Bite") ? string.Format("Use {0}cp Ferocious Bites to spend extra combo points.\r\n", HighestDPSRotation.BiteCP) : "",
					HighestDPSRotation.Name.Contains("Shred") ? "Shred" : "Mangle");
			}
			catch (Exception ex)
			{
				ex.ToString();
			}

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
