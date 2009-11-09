using System;
using System.Collections.Generic;
using System.Text;
#if RAWR3
using System.Windows.Media;
#endif

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
					new GemmingTemplate() { Model = "Cat", Group = "Uncommon", //Agi/Hit
					    RedId = delicate[0], YellowId = glinting[0], BlueId = shifting[0], PrismaticId = delicate[0], MetaId = relentless },

					new GemmingTemplate() { Model = "Cat", Group = "Rare", //Max Agility
						RedId = delicate[1], YellowId = delicate[1], BlueId = delicate[1], PrismaticId = delicate[1], MetaId = relentless },
					new GemmingTemplate() { Model = "Cat", Group = "Rare", //Agi/Crit 
						RedId = delicate[1], YellowId = deadly[1], BlueId = shifting[1], PrismaticId = delicate[1], MetaId = relentless },
					new GemmingTemplate() { Model = "Cat", Group = "Rare", //Agi/Hit
					    RedId = delicate[1], YellowId = glinting[1], BlueId = shifting[1], PrismaticId = delicate[1], MetaId = relentless },
						
					new GemmingTemplate() { Model = "Cat", Group = "Epic", Enabled = true, //Max Agility
						RedId = delicate[2], YellowId = delicate[2], BlueId = delicate[2], PrismaticId = delicate[2], MetaId = relentless },
					new GemmingTemplate() { Model = "Cat", Group = "Epic", Enabled = true, //Agi/Crit 
						RedId = delicate[2], YellowId = deadly[2], BlueId = shifting[2], PrismaticId = delicate[2], MetaId = relentless },
					new GemmingTemplate() { Model = "Cat", Group = "Epic", Enabled = true, //Agi/Hit
					    RedId = delicate[2], YellowId = glinting[2], BlueId = shifting[2], PrismaticId = delicate[2], MetaId = relentless },
						
					new GemmingTemplate() { Model = "Cat", Group = "Jeweler", //Max Agility
						RedId = delicate[3], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[3], MetaId = relentless },
					new GemmingTemplate() { Model = "Cat", Group = "Jeweler", //Agility Heavy
						RedId = delicate[2], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[2], MetaId = relentless },
				};
			}
        }

#if RAWR3
        private ICalculationOptionsPanel _calculationOptionsPanel = null;
		public override ICalculationOptionsPanel CalculationOptionsPanel
#else
        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
		public override CalculationOptionsPanelBase CalculationOptionsPanel
#endif
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
					
					"Abilities:Optimal Rotation",
					"Abilities:Optimal Rotation DPS",
					"Abilities:Custom Rotation DPS",
					"Abilities:Melee",
					"Abilities:Mangle",
					"Abilities:Shred",
					"Abilities:Rake",
					"Abilities:Rip",
					"Abilities:Bite",
					//"Abilities:Melee Usage",
					//"Abilities:Melee Stats",
					//"Abilities:Mangle Usage",
					//"Abilities:Mangle Stats",
					//"Abilities:Shred Usage",
					//"Abilities:Shred Stats",
					//"Abilities:Rake Usage",
					//"Abilities:Rake Stats",
					//"Abilities:Rip Usage",
					//"Abilities:Rip Stats",
					//"Abilities:Bite Usage",
					//"Abilities:Bite Stats",
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
					};
				return _customChartNames;
			}
        }

#if RAWR3
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
#else
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
#endif

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
			List<float> tempArPenRatings, tempArPenRatingUptimes;
			Stats stats = GetCharacterStatsWithTemporaryArPen(character, additionalItem, out tempArPenRatings, out tempArPenRatingUptimes);
			float levelDifference = (targetLevel - 80f) * 0.2f;
			CharacterCalculationsCat calculatedStats = new CharacterCalculationsCat();
			calculatedStats.BasicStats = stats;
			calculatedStats.TargetLevel = targetLevel;
			bool maintainMangle = stats.BonusBleedDamageMultiplier == 0f;
			stats.BonusBleedDamageMultiplier = 0.3f;

			#region Basic Chances and Constants
			float modArmor = 0f;
			for (int i = 0; i < tempArPenRatings.Count; i++)
			{
				modArmor += tempArPenRatingUptimes[i] * StatConversion.GetArmorDamageReduction(character.Level, calcOpts.TargetArmor, stats.ArmorPenetration, 0f, stats.ArmorPenetrationRating + tempArPenRatings[i]);
			}

			modArmor = 1f - modArmor;
			float critMultiplier = 2f * (1f + stats.BonusCritMultiplier);
			float critMultiplierBleed = 2f * (1f + stats.BonusCritMultiplier);
			float hasteBonus = StatConversion.GetPhysicalHasteFromRating(stats.HasteRating, CharacterClass.Druid);
			hasteBonus = (1f + hasteBonus) * (1f + stats.Bloodlust * 40f / Math.Max(calcOpts.Duration, 40f)) - 1f;
			float attackSpeed = 1f / (1f + hasteBonus);
			attackSpeed = attackSpeed / (1f + stats.PhysicalHaste);

            float hitBonus = stats.PhysicalHit + StatConversion.GetPhysicalHitFromRating(stats.HitRating, CharacterClass.Druid);
            float expertiseBonus = StatConversion.GetDodgeParryReducFromExpertise(StatConversion.GetExpertiseFromRating(stats.ExpertiseRating, CharacterClass.Druid) + stats.Expertise, CharacterClass.Druid);

			float chanceDodge = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[targetLevel-80] - expertiseBonus);
			float chanceParry = 0f; //Math.Max(0f, StatConversion.WHITE_PARRY_CHANCE_CAP[targetLevel - 80] - expertiseBonus);
            float chanceMiss  = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[ targetLevel-80] - hitBonus);
			
			float glanceMultiplier = 0.7f;
            float chanceAvoided = chanceMiss + chanceDodge + chanceParry;
			float chanceGlance = StatConversion.WHITE_GLANCE_CHANCE_CAP[targetLevel-80];
            float chanceCrit = StatConversion.GetCritFromRating(stats.CritRating, CharacterClass.Druid)
                + StatConversion.GetCritFromAgility(stats.Agility, CharacterClass.Druid)
                + stats.PhysicalCrit
                + StatConversion.NPC_LEVEL_CRIT_MOD[targetLevel-80];
			float chanceCritWhite = Math.Min(chanceCrit, 1f - chanceGlance - chanceAvoided);
			float chanceCritBleed = character.DruidTalents.PrimalGore > 0 ? chanceCrit : 0f;
			float chanceHit = 1f - chanceCrit - chanceAvoided;
			float chanceHitNonGlance = 1f - chanceCrit - chanceAvoided - chanceGlance;
			float chanceNonAvoided = 1f - chanceAvoided;
			float chanceCritBite = Math.Min(1f - chanceAvoided, chanceCrit + stats.BonusFerociousBiteCrit);
			float chanceHitBite = 1f - chanceCritBite - chanceAvoided;
			float chanceCritRip = chanceCritBleed > 0 ? chanceCritBleed + stats.BonusRipCrit : 0;

			float cpPerCPG = (chanceHit + chanceCrit * (1f + stats.BonusCPOnCrit)) / chanceNonAvoided;
			calculatedStats.DodgedAttacks = chanceDodge * 100f;
            calculatedStats.ParriedAttacks = chanceParry * 100f;
			calculatedStats.MissedAttacks = chanceMiss * 100f;

			float timeToReapplyDebuffs = 1f / (1f - chanceAvoided) - 1f;
			float lagVariance = (float)calcOpts.LagVariance / 1000f;
			float mangleDurationUptime = (character.DruidTalents.GlyphOfMangle ? 18f : 12f);
			float mangleDurationAverage = mangleDurationUptime - timeToReapplyDebuffs - lagVariance;
			float rakeDurationUptime = 9f + stats.BonusRakeDuration;
			float rakeDurationAverage = rakeDurationUptime + timeToReapplyDebuffs + lagVariance;
			float ripDurationUptime = 12f + stats.BonusRipDuration; //Doesn't include Glyph of Shred
			float ripDurationAverage = ripDurationUptime + timeToReapplyDebuffs + lagVariance; //Doesn't include Glyph of Shred
			float roarBonusDuration = stats.BonusSavageRoarDuration - lagVariance;
			float berserkDuration = character.DruidTalents.Berserk > 0 ? (character.DruidTalents.GlyphOfBerserk ? 20f : 15f) : 0f;
			#endregion

			#region Attack Damages
			float baseDamage = 55f + (stats.AttackPower / 14f) + stats.WeaponDamage;
			float meleeDamageRaw = (baseDamage) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * modArmor;
			float mangleDamageRaw = (baseDamage * 2f + 566f + stats.BonusMangleCatDamage) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusMangleDamageMultiplier) * modArmor;
			float shredDamageRaw = (baseDamage * 2.25f + 666f + stats.BonusShredDamage) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusShredDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier) * modArmor;
			float rakeDamageRaw = (176f + stats.AttackPower * 0.01f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusRakeDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier);
			float rakeDamageDot = (1074f + stats.AttackPower * 0.18f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusRakeDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier) * ((9f + stats.BonusRakeDuration) / 9f);
			float ripDamageRaw = (3006f + stats.AttackPower * 0.3f + (stats.BonusRipDamagePerCPPerTick * 5f * 6f)) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusRipDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier);
			float biteBaseDamageRaw = 190f * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusFerociousBiteDamageMultiplier) * modArmor;
			float biteCPDamageRaw = (290f + stats.AttackPower * 0.07f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusFerociousBiteDamageMultiplier) * modArmor;

			float meleeDamageAverage =	chanceGlance * meleeDamageRaw * glanceMultiplier +
										chanceCritWhite * meleeDamageRaw * critMultiplier +
										chanceHitNonGlance * meleeDamageRaw;
			float mangleDamageAverage = (1f - chanceCrit) * mangleDamageRaw + chanceCrit * mangleDamageRaw * critMultiplier;
			float shredDamageAverage = (1f - chanceCrit) * shredDamageRaw + chanceCrit * shredDamageRaw * critMultiplier;
			float rakeDamageAverage = ((1f - chanceCrit) * rakeDamageRaw + chanceCrit * rakeDamageRaw * critMultiplier) + rakeDamageDot;
			float ripDamageAverage = ((1f - chanceCritRip) * ripDamageRaw + chanceCritRip * ripDamageRaw * critMultiplierBleed);
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

			#region Ability Stats
			CatAbilityStats meleeStats = new CatMeleeStats()
			{
				DamagePerHit = meleeDamageRaw,
				DamagePerSwing = meleeDamageAverage,
			};
			CatAbilityStats mangleStats = new CatMangleStats()
			{
				DamagePerHit = mangleDamageRaw,
				DamagePerSwing = mangleDamageAverage,
				DurationUptime = mangleDurationUptime,
				DurationAverage = mangleDurationAverage,
				EnergyCost = mangleEnergyAverage,
			};
			CatAbilityStats shredStats = new CatShredStats()
			{
				DamagePerHit = shredDamageRaw,
				DamagePerSwing = shredDamageAverage,
				EnergyCost = shredEnergyAverage,
			};
			CatAbilityStats rakeStats = new CatRakeStats()
			{
				DamagePerHit = rakeDamageRaw + rakeDamageDot,
				DamagePerSwing = rakeDamageAverage,
				DurationUptime = rakeDurationUptime,
				DurationAverage = rakeDurationAverage,
				EnergyCost = rakeEnergyAverage,
			};
			CatAbilityStats ripStats = new CatRipStats()
			{
				DamagePerHit = ripDamageRaw ,
				DamagePerSwing = ripDamageAverage,
				DurationUptime = ripDurationUptime,
				DurationAverage = ripDurationAverage,
				EnergyCost = ripEnergyAverage,
			};
			CatAbilityStats biteStats = new CatBiteStats()
			{
				DamagePerHit = biteBaseDamageRaw,
				DamagePerSwing = biteBaseDamageAverage,
				DamagePerHitPerCP = biteCPDamageRaw,
				DamagePerSwingPerCP = biteCPDamageAverage,
				EnergyCost = biteEnergyAverage,
			};
			CatAbilityStats roarStats = new CatRoarStats()
			{
				DurationUptime = roarBonusDuration,
				DurationAverage = 9f + roarBonusDuration,
				EnergyCost = roarEnergyAverage,
				DurationPerCP = 5f,
			};
			#endregion

			#region Rotations
			CatRotationCalculator rotationCalculator = new CatRotationCalculator(stats, calcOpts.Duration, cpPerCPG,
				maintainMangle, berserkDuration, attackSpeed, character.DruidTalents.OmenOfClarity > 0, 
				character.DruidTalents.GlyphOfShred, chanceAvoided, chanceCrit * stats.BonusCPOnCrit,
				cpgEnergyCostMultiplier, stats.ClearcastOnBleedChance, meleeStats, mangleStats, shredStats,
				rakeStats, ripStats, biteStats, roarStats);
			CatRotationCalculator.CatRotationCalculation rotationCalculationDPS = new CatRotationCalculator.CatRotationCalculation();

			for (int roarCP = 1; roarCP < 6; roarCP++)
				for (int biteCP = 0; biteCP < 6; biteCP++)
					for (int useRake = 0; useRake < 2; useRake++)
						for (int useShred = 0; useShred < 2; useShred++)
							for (int useRip = 0; useRip < 2; useRip++)
							{
								CatRotationCalculator.CatRotationCalculation rotationCalculation =
									rotationCalculator.GetRotationCalculations(
									useRake == 1, useShred == 1, useRip == 1, biteCP, roarCP);
								if (rotationCalculation.DPS > rotationCalculationDPS.DPS)
									rotationCalculationDPS = rotationCalculation;
							}

			calculatedStats.HighestDPSRotation = rotationCalculationDPS;
			calculatedStats.CustomRotation = rotationCalculator.GetRotationCalculations(
				calcOpts.CustomUseRake, calcOpts.CustomUseShred, calcOpts.CustomUseRip, calcOpts.CustomCPFerociousBite, calcOpts.CustomCPSavageRoar);
			
			if (character.DruidTalents.GlyphOfShred && rotationCalculationDPS.ShredCount > 0)
			{
				ripStats.DurationUptime += 6f;
				ripStats.DurationAverage += 6f;
			}
			ripStats.DamagePerHit *= ripStats.DurationUptime / 12f;
			ripStats.DamagePerSwing *= ripStats.DurationUptime / 12f;
			#endregion

			calculatedStats.AvoidedAttacks = chanceAvoided * 100f;
			calculatedStats.DodgedAttacks = chanceDodge * 100f;
            calculatedStats.ParriedAttacks = chanceParry * 100f;
			calculatedStats.MissedAttacks = chanceMiss * 100f;
			calculatedStats.CritChance = chanceCrit * 100f;
			calculatedStats.AttackSpeed = attackSpeed;
			calculatedStats.ArmorMitigation = (1f - modArmor) * 100f;
			calculatedStats.Duration = calcOpts.Duration;

			calculatedStats.MeleeStats = meleeStats;
			calculatedStats.MangleStats = mangleStats;
			calculatedStats.ShredStats = shredStats;
			calculatedStats.RakeStats = rakeStats;
			calculatedStats.RipStats = ripStats;
			calculatedStats.RoarStats = roarStats;
			calculatedStats.BiteStats = biteStats;

			float magicDPS = (stats.ShadowDamage + stats.ArcaneDamage) * (1f + chanceCrit);
			calculatedStats.DPSPoints = calculatedStats.HighestDPSRotation.DPS + magicDPS;
			calculatedStats.SurvivabilityPoints = stats.Health / 100f;
			calculatedStats.OverallPoints = calculatedStats.DPSPoints + calculatedStats.SurvivabilityPoints;
			return calculatedStats;
		}

		public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
			List<float> arPenRating, arPenRatingUptime;
			return GetCharacterStatsWithTemporaryArPen(character, additionalItem, out arPenRating, out arPenRatingUptime);
		}

		private Stats GetCharacterStatsWithTemporaryArPen(Character character, Item additionalItem, out List<float> tempArPenRatings, out List<float> tempArPenRatingUptimes)
		{
			CalculationOptionsCat calcOpts = character.CalculationOptions as CalculationOptionsCat;
            int targetLevel = calcOpts.TargetLevel;

			Stats statsRace = BaseStats.GetBaseStats(80, character.Class, character.Race, BaseStats.DruidForm.Cat);
			statsRace.BonusPhysicalDamageMultiplier = character.DruidTalents.GlyphOfSavageRoar ? 0.33f : 0.3f; //Savage Roar
			
			Stats statsItems = GetItemStats(character, additionalItem);
			Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
			float[] thickHideMultipliers = new float[] { 1f, 1.04f, 1.07f, 1.1f };
			statsItems.Armor *= thickHideMultipliers[character.DruidTalents.ThickHide];

			DruidTalents talents = character.DruidTalents;
			Stats statsTalents = new Stats()
			{
				PhysicalCrit = 0.02f * talents.SharpenedClaws + ((character.ActiveBuffsContains("Leader of the Pack") ||
				character.ActiveBuffsContains("Rampage"))?
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

			Stats statsTotal = statsRace + statsItems;
			statsTotal.Accumulate(statsBuffs);
			statsTotal.Accumulate(statsTalents);

			float predatoryStrikesAP = 0f;
			float fap = 0f;
			if (character.MainHand != null)
			{
				fap = Math.Max(0f, (character.MainHand.Item.DPS - 54.8f) * 14f); //TODO Find a more accurate number for this?
				predatoryStrikesAP = (fap + character.MainHand.Item.Stats.AttackPower) * 0.2f * (talents.PredatoryStrikes / 3f);
				if (character.MainHand.Enchant != null)
				{
					predatoryStrikesAP += character.MainHand.Enchant.Stats.AttackPower * 0.2f * (talents.PredatoryStrikes / 3f);
				}
			}

			statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
			statsTotal.Strength = (float)Math.Floor(statsTotal.Strength * (1f + statsTotal.BonusStrengthMultiplier));
			statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1f + statsTotal.BonusAgilityMultiplier));
			statsTotal.AttackPower += statsTotal.Strength * 2f + statsTotal.Agility + fap + predatoryStrikesAP;
			statsTotal.AttackPower = (float)Math.Floor(statsTotal.AttackPower * (1f+ statsTotal.BonusAttackPowerMultiplier));
			statsTotal.Health += (float)Math.Floor((statsTotal.Stamina - 20f) * 10f + 20f);
			statsTotal.Armor += 2f * statsTotal.Agility;
			statsTotal.Armor = (float)Math.Floor(statsTotal.Armor * (1f + statsTotal.BonusArmorMultiplier));
			statsTotal.NatureResistance += statsTotal.NatureResistanceBuff + statsTotal.AllResist;
			statsTotal.FireResistance += statsTotal.FireResistanceBuff + statsTotal.AllResist;
			statsTotal.FrostResistance += statsTotal.FrostResistanceBuff + statsTotal.AllResist;
			statsTotal.ShadowResistance += statsTotal.ShadowResistanceBuff + statsTotal.AllResist;
			statsTotal.ArcaneResistance += statsTotal.ArcaneResistanceBuff + statsTotal.AllResist;
			statsTotal.WeaponDamage += 16f; //Tiger's Fury

			float hasteBonus = StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, CharacterClass.Druid);
			hasteBonus = (1f + hasteBonus) * (1f + statsTotal.PhysicalHaste) * (1f + statsTotal.Bloodlust * 40f / Math.Max(calcOpts.Duration, 40f)) - 1f;
			float meleeHitInterval = 1f / ((1f + hasteBonus) + 1f / 3.5f);
			float hitBonus = StatConversion.GetPhysicalHitFromRating(statsTotal.HitRating) + statsTotal.PhysicalHit;
            float expertiseBonus = StatConversion.GetDodgeParryReducFromExpertise(StatConversion.GetExpertiseFromRating(statsTotal.ExpertiseRating, CharacterClass.Druid) + statsTotal.Expertise, CharacterClass.Druid);
            float chanceDodge = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[targetLevel-80] - expertiseBonus);
			float chanceParry = 0f;// Math.Max(0f, StatConversion.WHITE_PARRY_CHANCE_CAP[targetLevel - 80] - expertiseBonus);
            float chanceMiss  = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[ targetLevel-80] - hitBonus);
			float chanceAvoided = chanceMiss + chanceDodge + chanceParry;

            float rawChanceCrit = StatConversion.GetPhysicalCritFromRating(statsTotal.CritRating)
                                + StatConversion.GetPhysicalCritFromAgility(statsTotal.Agility, CharacterClass.Druid)
                                + statsTotal.PhysicalCrit
                                + StatConversion.NPC_LEVEL_CRIT_MOD[targetLevel - 80];
			float chanceCrit = rawChanceCrit * (1f - chanceAvoided);
			float chanceHit = 1f - chanceAvoided;

			Dictionary<Trigger, float> triggerIntervals = new Dictionary<Trigger, float>();
			Dictionary<Trigger, float> triggerChances = new Dictionary<Trigger, float>();
			triggerIntervals[Trigger.Use] = 0f;
			triggerIntervals[Trigger.MeleeHit] = meleeHitInterval;
			triggerIntervals[Trigger.PhysicalHit] = meleeHitInterval;
			triggerIntervals[Trigger.MeleeCrit] = meleeHitInterval;
			triggerIntervals[Trigger.PhysicalCrit] = meleeHitInterval;
			triggerIntervals[Trigger.DoTTick] = 1.5f;
			triggerIntervals[Trigger.DamageDone] = meleeHitInterval / 2f;
			if (talents.Mangle > 0 && !character.ActiveBuffsContains("Mangle") && !character.ActiveBuffsContains("Trauma"))
				triggerIntervals[Trigger.MangleCatHit] = talents.GlyphOfMangle ? 18f : 12f;
			triggerIntervals[Trigger.MangleCatOrShredHit] = 4f;
			triggerChances[Trigger.Use] = 1f;
			triggerChances[Trigger.MeleeHit] = chanceHit;
			triggerChances[Trigger.PhysicalHit] = chanceHit;
			triggerChances[Trigger.MeleeCrit] = chanceCrit;
			triggerChances[Trigger.PhysicalCrit] = chanceCrit;
			triggerChances[Trigger.DoTTick] = 1f;
			triggerChances[Trigger.DamageDone] = 1f - chanceAvoided / 2f;
			if (talents.Mangle > 0 && !character.ActiveBuffsContains("Mangle") && !character.ActiveBuffsContains("Trauma"))
				triggerChances[Trigger.MangleCatHit] = 1f;
			triggerChances[Trigger.MangleCatOrShredHit] = chanceHit;

            // Handle Trinket procs
			Stats statsProcs = new Stats();
			foreach (SpecialEffect effect in statsTotal.SpecialEffects(se => triggerIntervals.ContainsKey(se.Trigger) && se.Stats.ArmorPenetrationRating == 0))
			{ //Calculate all non-ArPen procs first.
				statsProcs.Accumulate(effect.GetAverageStats(triggerIntervals[effect.Trigger], triggerChances[effect.Trigger], 1f, calcOpts.Duration));
			}

			statsProcs.Agility += statsProcs.HighestStat + statsProcs.Paragon;
			statsProcs.Stamina = (float)Math.Floor(statsProcs.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
			statsProcs.Strength = (float)Math.Floor(statsProcs.Strength * (1f + statsTotal.BonusStrengthMultiplier));
			statsProcs.Agility = (float)Math.Floor(statsProcs.Agility * (1f + statsTotal.BonusAgilityMultiplier));
			statsProcs.AttackPower += statsProcs.Strength * 2f + statsProcs.Agility;
			statsProcs.AttackPower = (float)Math.Floor(statsProcs.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
			statsProcs.Health += (float)Math.Floor(statsProcs.Stamina * 10f);
			statsProcs.Armor += 2f * statsProcs.Agility;
			statsProcs.Armor = (float)Math.Floor(statsProcs.Armor * (1f + statsTotal.BonusArmorMultiplier));
			statsTotal.Accumulate(statsProcs);

			//Handle ArPen procs
			tempArPenRatings = new List<float>();
			tempArPenRatingUptimes = new List<float>();
			List<SpecialEffect> tempArPenEffects = new List<SpecialEffect>();
			List<float> tempArPenEffectIntervals = new List<float>();
			List<float> tempArPenEffectChances = new List<float>();

			foreach (SpecialEffect effect in statsTotal.SpecialEffects(se => triggerIntervals.ContainsKey(se.Trigger) && se.Stats.ArmorPenetrationRating > 0))
			{
				tempArPenEffects.Add(effect);
				tempArPenEffectIntervals.Add(triggerIntervals[effect.Trigger]);
				tempArPenEffectChances.Add(triggerChances[effect.Trigger]);
			}

			if (tempArPenEffects.Count == 0)
			{
				tempArPenRatings.Add(0.0f);
				tempArPenRatingUptimes.Add(1.0f);
			}
			else if (tempArPenEffects.Count == 1)
			{ //Only one, add it to
				SpecialEffect effect = tempArPenEffects[0];
				float uptime = effect.GetAverageUptime(triggerIntervals[effect.Trigger], triggerChances[effect.Trigger], 1f, calcOpts.Duration);
				tempArPenRatings.Add(effect.Stats.ArmorPenetrationRating);
				tempArPenRatingUptimes.Add(uptime);
				tempArPenRatings.Add(0.0f);
				tempArPenRatingUptimes.Add(1.0f - uptime);
			}
			else if (tempArPenEffects.Count > 1)
			{
				float[] intervals = new float[tempArPenEffects.Count];
				float[] chances = new float[tempArPenEffects.Count];
				float[] offset = new float[tempArPenEffects.Count];
				for (int i = 0; i < tempArPenEffects.Count; i++)
				{
					intervals[i] = triggerIntervals[tempArPenEffects[i].Trigger];
					chances[i] = triggerChances[tempArPenEffects[i].Trigger];
				}
				if (tempArPenEffects.Count >= 2)
				{
					offset[0] = calcOpts.TrinketOffset;
				}
				WeightedStat[] arPenWeights = SpecialEffect.GetAverageCombinedUptimeCombinations(tempArPenEffects.ToArray(), intervals, chances, offset, 1f, calcOpts.Duration, AdditiveStat.ArmorPenetrationRating);
				for (int i = 0; i < arPenWeights.Length; i++)
				{
					tempArPenRatings.Add(arPenWeights[i].Value);
					tempArPenRatingUptimes.Add(arPenWeights[i].Chance);
				}
			}

			return statsTotal;
		}

		public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
		{
			switch (chartName)
			{
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
					Paragon = stats.Paragon,
					BonusRakeDuration = stats.BonusRakeDuration,
					BonusRipCrit = stats.BonusRipCrit,

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
					|| effect.Trigger == Trigger.DamageDone || effect.Trigger == Trigger.MangleCatHit 
					|| effect.Trigger == Trigger.MangleCatOrShredHit)
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
				stats.HasteRating + stats.Health + stats.HitRating + stats.MangleCatCostReduction + /*stats.Stamina +*/
				stats.Strength + stats.CatFormStrength + stats.WeaponDamage + stats.Bloodlust +
				stats.PhysicalHit + stats.BonusRipDamagePerCPPerTick + stats.TerrorProc + stats.BonusRipCrit +
				stats.PhysicalHaste + stats.ArmorPenetrationRating + stats.BonusRipDuration + stats.BonusRakeDuration +
				stats.ThreatReductionMultiplier + stats.AllResist + stats.ArcaneDamage + stats.ShadowDamage +
				stats.ArcaneResistance + stats.NatureResistance + stats.FireResistance + stats.BonusBleedDamageMultiplier + stats.Paragon +
				stats.FrostResistance + stats.ShadowResistance + stats.ArcaneResistanceBuff + stats.TigersFuryCooldownReduction + stats.HighestStat +
				stats.NatureResistanceBuff + stats.FireResistanceBuff + stats.BonusShredDamageMultiplier + stats.BonusPhysicalDamageMultiplier +
				stats.FrostResistanceBuff + stats.ShadowResistanceBuff) > 0 || (stats.Stamina > 0 && stats.SpellPower == 0);

			foreach (SpecialEffect effect in stats.SpecialEffects())
			{
				if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit
					|| effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit
					|| effect.Trigger == Trigger.MangleCatHit || effect.Trigger == Trigger.MangleCatOrShredHit)
				{
					relevant |= HasRelevantStats(effect.Stats);
					if (relevant) break;
				}
			}
			return relevant;
		}

		public override void SetDefaults(Character character)
		{
			character.ActiveBuffsAdd(("Horn of Winter"));
			character.ActiveBuffsAdd(("Battle Shout"));
			character.ActiveBuffsAdd(("Unleashed Rage"));
			character.ActiveBuffsAdd(("Improved Moonkin Form"));
			character.ActiveBuffsAdd(("Leader of the Pack"));
			character.ActiveBuffsAdd(("Improved Icy Talons"));
			character.ActiveBuffsAdd(("Power Word: Fortitude"));
			character.ActiveBuffsAdd(("Mark of the Wild"));
			character.ActiveBuffsAdd(("Blessing of Kings"));
			character.ActiveBuffsAdd(("Sunder Armor"));
			character.ActiveBuffsAdd(("Faerie Fire"));
			character.ActiveBuffsAdd(("Totem of Wrath"));
			character.ActiveBuffsAdd(("Flask of Endless Rage"));
			character.ActiveBuffsAdd(("Agility Food"));
			character.ActiveBuffsAdd(("Heroism/Bloodlust"));

			if (character.PrimaryProfession == Profession.Alchemy ||
				character.SecondaryProfession == Profession.Alchemy)
				character.ActiveBuffsAdd(("Flask of Endless Rage (Mixology)"));

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
