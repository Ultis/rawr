/////////////////////////////////
//
// Written by Chadd Nervig (aka Astrylian) for Rawr. E-mail me at cnervig@hotmail.com.
//
// Rawr is a program for comparing and exploring gear for characters in the MMORPG, 
// World of Warcraft. It has been designed from the start to be fun to use, and helpful in 
// finding better combinations of gear, and what gear to obtain. (.NET, WinForm, C# 3.0, XML)
//
// Rawr is designed as a platform for hosting 'models', which implement the features and
// calculations needed by a specific class or specialization of WoW character.
//
// This file contains the majority of the classes and methods for the Rawr.Bear model,
// which models combat for the Druid Bear type of character.
//
// Please visit http://rawr.codeplex.com/ for the full source code of Rawr, or contact me
// for additional details.
//
/////////////////////////////////

using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text;

namespace Rawr.Bear
{
	/// <summary>
	/// Core class representing the Rawr.Bear model
	/// </summary>
	[Rawr.Calculations.RawrModelInfo("Bear", "Ability_Racial_BearForm", CharacterClass.Druid)]
	public class CalculationsBear : CalculationsBase
	{
		#region Basic Model Properties and Methods
		/// <summary>
		/// GemmingTemplates to be used by default, when none are defined
		/// </summary>
		public override List<GemmingTemplate> DefaultGemmingTemplates
		{
			get
			{
				// Relevant Gem IDs for Cats
				//Red
				int[] delicate	= { 52082, 52212 }; // Agi
				int[] precise = { 52085, 52230 }; // Exp

				//Purple
				int[] accurate = { 52105, 52203 }; // Exp/Hit
				int[] guardians = { 52099, 52221 }; // Exp/Sta
				int[] glinting = { 52102, 52220 }; // Agi/Hit
				int[] shifting = { 52096, 52238 }; // Agi/Sta
				
				//Blue
				int[] solid = { 52086, 52242 }; // Sta
				int[] rigid = { 52089, 52235 }; // Hit
				
				//Green
				int[] jagged = { 52121, 52223 }; // Crit/Sta
				int[] nimble = { 52120, 52227 }; // Dodge/Hit
				int[] piercing = { 52122, 52228 }; // Crit/Hit
				int[] puissant = { 52126, 52231 }; // Mastery/Sta
				int[] regal = { 52119, 52233 }; // Dodge/Sta
				int[] senseis = { 52128, 52233 }; // Mastery/Hit
				
				//Yellow
				int[] fractured = { 52094, 52219 }; // Mastery
				int[] subtle = { 52090, 52247 }; // Dodge
				
				//Orange
				int[] adept = { 52115, 52204 }; // Agi/Mastery
				int[] deadly = { 52109, 52209 }; // Agi/Crit
				int[] deft = { 52112, 52211 }; // Agi/Haste
				int[] keen = { 52118, 52224 }; // Exp/Mastery
				int[] polished = { 52106, 52229 }; // Agi/Dodge
				int[] resolute = { 52107, 52249 }; // Exp/Dodge

				//Prismatic

				//Meta
				int austere = 52294; //Sta/Armor - 2Y
				//int chaotic = 52291; //Crit/CritDmg - B>R
				//int destructive = 52298; //Crit/Reflect - 2R
				//int fleet = 52289; //Mastery/Runspeed - 2Y

				List<GemmingTemplate> list = new List<GemmingTemplate>();
				for (int tier = 0; tier < 2; tier++)
				{
					list.AddRange(new GemmingTemplate[]
						{
							CreateBearGemmingTemplate(tier,	 delicate,   delicate, 	delicate,	delicate,	austere), 
							CreateBearGemmingTemplate(tier,	 delicate,   polished, 	shifting,	delicate,	austere), 
							CreateBearGemmingTemplate(tier,	 delicate,   adept, 	shifting,	delicate,	austere),
							
							CreateBearGemmingTemplate(tier,	 precise,    precise, 	precise,	precise,	austere), 
							CreateBearGemmingTemplate(tier,	 precise,    resolute, 	guardians,	precise,	austere), 
							CreateBearGemmingTemplate(tier,	 precise,    keen, 	    guardians,	precise,	austere), 
							CreateBearGemmingTemplate(tier,	 precise,    deadly, 	guardians,	precise,	austere),
							CreateBearGemmingTemplate(tier,	 precise,    resolute, 	accurate,	precise,	austere), 
							CreateBearGemmingTemplate(tier,	 precise,    keen, 	    accurate,	precise,	austere),  
							CreateBearGemmingTemplate(tier,	 precise,    deadly, 	accurate,	precise,	austere), 
							
							CreateBearGemmingTemplate(tier,	 solid,      solid, 	solid,  	solid,  	austere), 
							CreateBearGemmingTemplate(tier,	 shifting,   puissant, 	solid,  	solid,  	austere), 
							
							//CreateBearGemmingTemplate(tier,	 rigid,      rigid, 	rigid,	    rigid,	    austere), 
							//CreateBearGemmingTemplate(tier,	 accurate,   piercing, 	rigid,	    rigid,	    austere),  
							//CreateBearGemmingTemplate(tier,	 glinting,   piercing, 	rigid,	    rigid,	    austere), 
							
						});
				}

				return list;
			}
		}

		private const int DEFAULT_GEMMING_TIER = 1;
		private GemmingTemplate CreateBearGemmingTemplate(int tier, int[] red, int[] yellow, int[] blue, int[] prismatic, int meta)
		{
			return new GemmingTemplate()
			{
				Model = "Cat",
				Group = (new string[] { "Uncommon", "Rare", "Epic", "Jeweler" })[tier],
				Enabled = (tier == DEFAULT_GEMMING_TIER),
				RedId = red[tier],
				YellowId = yellow[tier],
				BlueId = blue[tier],
				PrismaticId = prismatic[tier],
				MetaId = meta
			};
		}


		private ICalculationOptionsPanel _calculationOptionsPanel = null;
		/// <summary>
		/// Panel to be placed on the Options tab of the main form
		/// </summary>
		public override ICalculationOptionsPanel CalculationOptionsPanel
		{
			get
			{
				if (_calculationOptionsPanel == null)
				{
					_calculationOptionsPanel = new CalculationOptionsPanelBear();
				}
				return _calculationOptionsPanel;
			}
		}

		private string[] _characterDisplayCalculationLabels = null;
		/// <summary>
		/// Labels of the stats to display on the Stats tab of the main form
		/// </summary>
		public override string[] CharacterDisplayCalculationLabels
		{
			get
			{
				if (_characterDisplayCalculationLabels == null)
					_characterDisplayCalculationLabels = new string[] {
					@"Summary:Overall Points*Overall Points are a sum of Mitigation and Survival Points. 
Overall is typically, but not always, the best way to rate gear. 
For specific encounters, closer attention to Mitigation and 
Survival Points individually may be important.",
					@"Summary:Mitigation Points*Mitigation Points represent the amount of damage you mitigate, 
on average, through armor mitigation and avoidance. It is directly 
relational to your Damage Taken. Ideally, you want to maximize 
Mitigation Points, while maintaining 'enough' Survival Points 
(see Survival Points). If you find yourself dying due to healers 
running OOM, or being too busy healing you and letting other 
raid members die, then focus on Mitigation Points.",
					@"Summary:Survival Points*Survival Points represents the total raw physical damage 
(pre-mitigation) you can take before dying. Unlike 
Mitigation Points, you should not attempt to maximize this, 
but rather get 'enough' of it, and then focus on Mitigation. 
'Enough' can vary greatly by fight and by your healers, but 
keeping it roughly even with Mitigation Points is a good 
way to maintain 'enough' as you progress. If you find that 
you are being killed by burst damage, focus on Survival Points.",
					@"Summary:Threat Points*The TPS of your Highest TPS Rotation, multiplied by
the Threat Scale defined on the Options tab.",
					
					"Basic Stats:Armor",
					"Basic Stats:Agility",
					"Basic Stats:Stamina",
					"Basic Stats:Strength",
					"Basic Stats:Attack Power",
					"Basic Stats:Average Vengeance AP",
					"Basic Stats:Crit Rating",
					"Basic Stats:Hit Rating",
					"Basic Stats:Expertise Rating",
					"Basic Stats:Haste Rating",
					"Basic Stats:Armor Penetration Rating",
					"Basic Stats:Dodge Rating",
					"Basic Stats:Mastery",
					"Basic Stats:Defense Rating",
					"Basic Stats:Resilience",
					"Basic Stats:Nature Resist",
					"Basic Stats:Fire Resist",
					"Basic Stats:Frost Resist",
					"Basic Stats:Shadow Resist",
					"Basic Stats:Arcane Resist",
					"Mitigation Stats:Dodge",
					"Mitigation Stats:Miss",
					"Mitigation Stats:Armor Damage Reduction",
					"Mitigation Stats:Total Damage Reduction",
					"Mitigation Stats:Avoidance PreDR",
					"Mitigation Stats:Avoidance PostDR",
					"Mitigation Stats:Savage Defense",
					"Mitigation Stats:Total Mitigation",
					"Mitigation Stats:Damage Taken",
					"Survival Stats:Health",
					"Survival Stats:Chance to be Crit",
					"Survival Stats:Nature Survival",
					"Survival Stats:Fire Survival",
					"Survival Stats:Frost Survival",
					"Survival Stats:Shadow Survival",
					"Survival Stats:Arcane Survival",
					"Threat Stats:Highest DPS Rotation",
					"Threat Stats:Highest TPS Rotation",
					//"Threat Stats:Swipe Rotation",
					//"Threat Stats:Custom Rotation",
					"Threat Stats:Melee",
					"Threat Stats:Maul",
					"Threat Stats:Mangle",
					"Threat Stats:Lacerate",
					"Threat Stats:Pulverize",
					"Threat Stats:Swipe",
					"Threat Stats:Thrash",
					"Threat Stats:Faerie Fire",
					"Threat Stats:Thorns",
					"Threat Stats:Avoided Attacks",
					};
				return _characterDisplayCalculationLabels;
			}
		}

		private string[] _optimizableCalculationLabels = null;
		/// <summary>
		/// Labels of the stats available to the Optimizer 
		/// </summary>
		public override string[] OptimizableCalculationLabels
		{
			get
			{
				if (_optimizableCalculationLabels == null)
					_optimizableCalculationLabels = new string[] {
					"Health",
					"Hit Rating",
					"Expertise Rating",
					"Haste Rating",
					"Avoided Attacks %",
					"Mitigation % from Armor",
					"Avoidance %",
					"% Chance to be Crit",
					"Nature Survival",
					"Fire Survival",
					"Frost Survival",
					"Shadow Survival",
					"Arcane Survival",
					"Nature Resist",
					"Fire Resist",
					"Frost Resist",
					"Shadow Resist",
					"Arcane Resist",
					"Highest DPS",
					"Highest TPS",
					"Swipe DPS",
					"Swipe TPS",
					"Custom Rotation DPS",
					"Custom Rotation TPS",
					};
				return _optimizableCalculationLabels;
			}
		}

		private string[] _customChartNames = null;
		/// <summary>
		/// Names of the custom charts that Rawr.Bear provides
		/// </summary>
		public override string[] CustomChartNames
		{
			get
			{
				if (_customChartNames == null)
					_customChartNames = new string[] {
					"Combat Table",
					//"Relative Stat Values",
					//"Rotation DPS",
					//"Rotation TPS"
					};
				return _customChartNames;
			}
		}

		private Dictionary<string, Color> _subPointNameColors = null;
		/// <summary>
		/// Names and colors for the SubPoints that Rawr.Bear uses
		/// </summary>
		public override Dictionary<string, Color> SubPointNameColors
		{
			get
			{
				if (_subPointNameColors == null)
				{
					_subPointNameColors = new Dictionary<string, Color>();
					_subPointNameColors.Add("Mitigation", Colors.Red);
					_subPointNameColors.Add("Survival", Colors.Blue);
					_subPointNameColors.Add("Threat", Colors.Green);
				}
				return _subPointNameColors;
			}
		}

		private List<ItemType> _relevantItemTypes = null;
		/// <summary>
		/// ItemTypes that are relevant to Rawr.Bear
		/// </summary>
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
						ItemType.Relic,
						ItemType.Staff,
						ItemType.TwoHandMace,
						ItemType.Polearm
					});
				}
				return _relevantItemTypes;
			}
		}

		/// <summary>
		/// The class that Rawr.Bear is designed for (Druid)
		/// </summary>
		public override CharacterClass TargetClass { get { return CharacterClass.Druid; } }
		/// <summary>
		/// Creates a new ComparisonCalculationBear instance
		/// </summary>
		/// <returns>A new ComparisonCalculationBear instance</returns>
		public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationBear(); }
		/// <summary>
		/// Creates a new CharacterCalculationsBear instance
		/// </summary>
		/// <returns>A new CharacterCalculationsBear instance</returns>
		public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsBear(); }

		/// <summary>
		/// Deserializes the CalculationOptionsBear object contained in xml
		/// </summary>
		/// <param name="xml">The CalculationOptionsBear object, serialized as xml</param>
		/// <returns>The deserialized CalculationOptionsBear object</returns>
		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			xml = xml.Replace("<CustomUseMaul xsi:nil=\"true\" />", "<CustomUseMaul>false</CustomUseMaul>");
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsBear));
			System.IO.StringReader reader = new System.IO.StringReader(xml);
			CalculationOptionsBear calcOpts = serializer.Deserialize(reader) as CalculationOptionsBear;
			return calcOpts;
		}
		#endregion

		#region Primary Calculation Methods
		/// <summary>
		/// Gets the results of the Character provided
		/// </summary>
		/// <param name="character">The Character to calculate resutls for</param>
		/// <param name="additionalItem">An additional item to grant the Character the stats of (as if it were worn)</param>
		/// <returns>The CharacterCalculationsBear containing the results of the calculations</returns>
		public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
		{
			CharacterCalculationsBear calculatedStats = new CharacterCalculationsBear();
			if (character == null) { return calculatedStats; }
			CalculationOptionsBear calcOpts = character.CalculationOptions as CalculationOptionsBear;
			if (calcOpts == null) { return calculatedStats; }

			int targetLevel = calcOpts.TargetLevel;
			int characterLevel = character.Level;
			StatsBear stats = GetCharacterStats(character, additionalItem) as StatsBear;
			int levelDifference = (targetLevel - characterLevel);
			float targetAttackSpeed = calcOpts.TargetAttackSpeed * (1f - stats.BossAttackSpeedMultiplier);

			calculatedStats.BasicStats = stats;
			calculatedStats.TargetLevel = targetLevel;
			calculatedStats.CharacterLevel = characterLevel;

			float hasteBonus = StatConversion.GetHasteFromRating(stats.HasteRating, CharacterClass.Druid);
			float attackSpeed = ((2.5f) / (1f + hasteBonus)) / (1f + stats.PhysicalHaste);

			float hitBonus = StatConversion.GetPhysicalHitFromRating(stats.HitRating, CharacterClass.Druid) + stats.PhysicalHit;
			float expertiseBonus = StatConversion.GetDodgeParryReducFromExpertise(StatConversion.GetExpertiseFromRating(stats.ExpertiseRating, CharacterClass.Druid) + stats.Expertise, CharacterClass.Druid);
			float chanceDodge = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[levelDifference] - expertiseBonus);
			float chanceParry = Math.Max(0f, StatConversion.WHITE_PARRY_CHANCE_CAP[levelDifference] - expertiseBonus);
			float chanceMiss = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[levelDifference] - hitBonus);

			float chanceAvoided = chanceMiss + chanceDodge + chanceParry;

			float rawChanceCrit = StatConversion.GetPhysicalCritFromRating(stats.CritRating, CharacterClass.Druid)
								+ StatConversion.GetPhysicalCritFromAgility(stats.Agility, CharacterClass.Druid)
								+ stats.PhysicalCrit
								+ StatConversion.NPC_LEVEL_CRIT_MOD[levelDifference];
			float chanceCrit = rawChanceCrit * (1f - chanceAvoided);
			float chanceCritBleed = rawChanceCrit;

			float baseAgi = BaseStats.GetBaseStats(character.Level, character.Class, character.Race, BaseStats.DruidForm.Bear).Agility;

			//Calculate avoidance, considering diminishing returns
			float levelDifferenceAvoidance = levelDifference * 0.002f;
			float defSkill = (float)Math.Floor(StatConversion.GetDefenseFromRating(stats.DefenseRating, CharacterClass.Druid));
			float dodgeNonDR = stats.Dodge - levelDifferenceAvoidance + StatConversion.GetDodgeFromAgility(baseAgi, CharacterClass.Druid);
			float missNonDR = stats.Miss - levelDifferenceAvoidance;
			float dodgePreDR = StatConversion.GetDodgeFromAgility(stats.Agility - baseAgi, CharacterClass.Druid)
							   + StatConversion.GetDodgeFromRating(stats.DodgeRating, CharacterClass.Druid)
							   + defSkill * StatConversion.DEFENSE_RATING_AVOIDANCE_MULTIPLIER / 100f;
			float missPreDR = (defSkill * StatConversion.DEFENSE_RATING_AVOIDANCE_MULTIPLIER / 100f);
			float dodgePostDR = 0.01f / (1f / 116.890707f + 0.00972f / dodgePreDR);
			float missPostDR = 0.01f / (1f / 16f + 0.00972f / missPreDR);
			float dodgeTotal = dodgeNonDR + dodgePostDR;
			float missTotal = missNonDR + missPostDR;


			calculatedStats.Miss = missTotal;
			calculatedStats.Dodge = Math.Min(1f - calculatedStats.Miss, dodgeTotal);
			calculatedStats.DamageReductionFromArmor = StatConversion.GetDamageReductionFromArmor(targetLevel, stats.Armor);
			calculatedStats.TotalConstantDamageReduction = 1f - (1f - calculatedStats.DamageReductionFromArmor) * (1f + stats.DamageTakenMultiplier) * (1f + stats.BossPhysicalDamageDealtMultiplier);
			calculatedStats.AvoidancePreDR = dodgeNonDR + dodgePreDR + missNonDR + missPreDR;
			calculatedStats.AvoidancePostDR = dodgeTotal + missTotal;
			calculatedStats.CritReduction = (defSkill * StatConversion.DEFENSE_RATING_AVOIDANCE_MULTIPLIER / 100f)
										  + StatConversion.GetCritReductionFromResilience(stats.Resilience, CharacterClass.Druid)
										  + stats.CritChanceReduction;
			calculatedStats.CappedCritReduction = Math.Min(0.05f + levelDifferenceAvoidance, calculatedStats.CritReduction);

			{ //Evaluate damage taken once ahead of time for vengeance
				//Out of 100 attacks, you'll take...
				float critsVeng = Math.Min(Math.Max(0f, 1f - calculatedStats.AvoidancePostDR), (0.05f + levelDifferenceAvoidance) - calculatedStats.CappedCritReduction);
				//float crushes = targetLevel == 73 ? Math.Max(0f, Math.Min(15f, 100f - (crits + calculatedStats.AvoidancePreDR)) - stats.CritChanceReduction) : 0f;
				float hitsVeng = Math.Max(0f, 1f - (critsVeng + calculatedStats.AvoidancePostDR));
				//Apply armor and multipliers for each attack type...
				critsVeng *= (1f - calculatedStats.TotalConstantDamageReduction) * 2f;
				//crushes *= (100f - calculatedStats.Mitigation) * .015f;
				hitsVeng *= (1f - calculatedStats.TotalConstantDamageReduction);
				//Hack to make dodge reduce sustained vengeance
				hitsVeng *= (1f - calculatedStats.AvoidancePostDR);
				float damageTakenPercent = (hitsVeng + critsVeng) * (1f + stats.BossAttackSpeedMultiplier);
				float damageTakenPerHit = calcOpts.TargetDamage * damageTakenPercent;
				float damageTakenPerSecond = damageTakenPerHit / calcOpts.TargetAttackSpeed;
				float damageTakenPerVengeanceTick = damageTakenPerSecond * 2f;
				float vengeanceAP = Math.Min(stats.Health * 0.1f, damageTakenPerVengeanceTick) * (1f + stats.BonusAttackPowerMultiplier);

				stats.AttackPower += vengeanceAP;
				calculatedStats.AverageVengeanceAP = vengeanceAP;
			}

			float targetHitChance = 1f - calculatedStats.AvoidancePostDR;
			float autoSpecialAttacksPerSecond = 1f / 1.5f + 1f / attackSpeed + 1f / 3f;
			float lacerateTicksPerSecond = 1f / 3f;

			float masteryMultiplier = 1f + (8f + StatConversion.GetMasteryFromRating(stats.MasteryRating)) * 0.04f;
			float totalAttacksPerSecond = autoSpecialAttacksPerSecond + lacerateTicksPerSecond;
			float averageSDAttackCritChance = 0.5f * (chanceCrit * (autoSpecialAttacksPerSecond / totalAttacksPerSecond) + 
				chanceCritBleed * (lacerateTicksPerSecond / totalAttacksPerSecond)); //Include the 50% chance to proc per crit here.
			float playerAttackSpeed = 1f / totalAttacksPerSecond;
			float blockChance = 1f - targetHitChance * ((float)Math.Pow(1f - averageSDAttackCritChance, targetAttackSpeed / playerAttackSpeed)) *
				1f / (1f - (1f - targetHitChance) * (float)Math.Pow(1f - averageSDAttackCritChance, targetAttackSpeed / playerAttackSpeed));
			float blockValue = stats.AttackPower * 0.35f * masteryMultiplier;
			float blockedPercent = Math.Min(1f, (blockValue * blockChance) / ((1f - calculatedStats.TotalConstantDamageReduction) * calcOpts.TargetDamage));
			calculatedStats.SavageDefenseChance = (float)Math.Round(blockChance, 5);
			calculatedStats.SavageDefenseValue = (float)Math.Floor(blockValue);
			calculatedStats.SavageDefensePercent = (float)Math.Round(blockedPercent, 5);

			float parryHasteDamageMuliplier = 1.0f;
			if (calcOpts.TargetParryHastes)
			{
				float parryableAttacksPerSecond = 7f / 13.5f + 1f / attackSpeed; //In every 13.5sec period (9 GCDs), 2 should be FFF, so only 7 parryable instants per 13.5sec
				//Target parries within the first 40% of their swing timer advances their swing timer by 40%
				//...Within the next 40%, it advances their swing timer to 20% remaining.
				//So, we average that as a 60% window (since the last 40% only provides half as much haste on average), with a 40% swing advance
				float parryWindow = targetAttackSpeed * 0.6f;
				float parryableAttacksPerParryWindow = parryableAttacksPerSecond * parryWindow;
				float chanceParryPerWindow = 1f - (float)Math.Pow(1f - chanceParry, parryableAttacksPerParryWindow);
				float parryBonusDPSMultiplier = 1f / 0.6f - 1f; //When a parry happens, boss DPS gets muliplied by 1/0.6 for that swing
				parryHasteDamageMuliplier = 1f + (parryBonusDPSMultiplier * chanceParryPerWindow);
			}

			//Out of 100 attacks, you'll take...
			float crits = Math.Min(Math.Max(0f, 1f - calculatedStats.AvoidancePostDR), (0.05f + levelDifferenceAvoidance) - calculatedStats.CappedCritReduction);
			//float crushes = targetLevel == 73 ? Math.Max(0f, Math.Min(15f, 100f - (crits + calculatedStats.AvoidancePreDR)) - stats.CritChanceReduction) : 0f;
			float hits = Math.Max(0f, 1f - (crits + calculatedStats.AvoidancePostDR));
			//Apply armor and multipliers for each attack type...
			crits *= (1f - calculatedStats.TotalConstantDamageReduction) * 2f;
			//crushes *= (100f - calculatedStats.Mitigation) * .015f;
			hits *= (1f - calculatedStats.TotalConstantDamageReduction);
			calculatedStats.DamageTaken = (hits + crits) * (1f - blockedPercent) * parryHasteDamageMuliplier * (1f + stats.BossAttackSpeedMultiplier);
			calculatedStats.TotalMitigation = 1f - calculatedStats.DamageTaken;

			calculatedStats.SurvivalPointsRaw = (stats.Health / (1f - calculatedStats.TotalConstantDamageReduction));
			double survivalCap = (double)calcOpts.SurvivalSoftCap / 1000d;
			double survivalRaw = calculatedStats.SurvivalPointsRaw / 1000f;

			//Implement Survival Soft Cap
			if (survivalRaw <= survivalCap)
			{
				calculatedStats.SurvivalPoints = 1000f * (float)survivalRaw;
			}
			else
			{
				double x = survivalRaw;
				double cap = survivalCap;
				double fourToTheNegativeFourThirds = Math.Pow(4d, -4d / 3d);
				double topLeft = Math.Pow(((x - cap) / cap) + fourToTheNegativeFourThirds, 1d / 4d);
				double topRight = Math.Pow(fourToTheNegativeFourThirds, 1d / 4d);
				double fracTop = topLeft - topRight;
				double fraction = fracTop / 2d;
				double y = (cap * fraction + cap);
				calculatedStats.SurvivalPoints = 1000f * (float)y;
			}

			calculatedStats.MitigationPoints = (78591f / calculatedStats.DamageTaken);

			// Call new resistance formula and apply talent damage reduction
			// As for other survival, only use guaranteed reduction (MinimumResist), no luck
			calculatedStats.NatureSurvivalPoints = (float)(stats.Health / ((1f - StatConversion.GetMinimumResistance(targetLevel, character.Level, stats.NatureResistance, 0)) * (1f - 0.06f * character.DruidTalents.NaturalReaction)));
			calculatedStats.FrostSurvivalPoints = (float)(stats.Health / ((1f - StatConversion.GetMinimumResistance(targetLevel, character.Level, stats.FrostResistance, 0)) * (1f - 0.06f * character.DruidTalents.NaturalReaction)));
			calculatedStats.FireSurvivalPoints = (float)(stats.Health / ((1f - StatConversion.GetMinimumResistance(targetLevel, character.Level, stats.FireResistance, 0)) * (1f - 0.06f * character.DruidTalents.NaturalReaction)));
			calculatedStats.ShadowSurvivalPoints = (float)(stats.Health / ((1f - StatConversion.GetMinimumResistance(targetLevel, character.Level, stats.ShadowResistance, 0)) * (1f - 0.06f * character.DruidTalents.NaturalReaction)));
			calculatedStats.ArcaneSurvivalPoints = (float)(stats.Health / ((1f - StatConversion.GetMinimumResistance(targetLevel, character.Level, stats.ArcaneResistance, 0)) * (1f - 0.06f * character.DruidTalents.NaturalReaction)));
			
			//Perform Threat calculations
			CalculateThreat(stats, targetLevel, calculatedStats, character);
			calculatedStats.OverallPoints = calculatedStats.MitigationPoints +
				calculatedStats.SurvivalPoints + calculatedStats.ThreatPoints;
			return calculatedStats;
		}

		/// <summary>
		/// Calculates the threat properties of the Character
		/// </summary>
		/// <param name="stats">The total Stats of the character</param>
		/// <param name="targetLevel">The level of the target</param>
		/// <param name="calculatedStats">The CharacterCalculationsBear object to fill with results</param>
		/// <param name="character">The Character to calculate the threat properties of</param>
		private void CalculateThreat(StatsBear stats, int targetLevel, CharacterCalculationsBear calculatedStats, Character character)
		{
			CalculationOptionsBear calcOpts = character.CalculationOptions as CalculationOptionsBear;
			if (calcOpts == null) calcOpts = new CalculationOptionsBear();
			DruidTalents talents = character.DruidTalents;

			// Establish base multipliers and chances
			float modArmor = 1f - StatConversion.GetArmorDamageReduction(character.Level, calcOpts.TargetArmor,
				stats.TargetArmorReduction, stats.ArmorPenetration, Math.Max(0f, stats.ArmorPenetrationRating));

			float critMultiplier = 2f * (1 + stats.BonusCritMultiplier);
			float spellCritMultiplier = 1.5f * (1 + stats.BonusCritMultiplier);

			float hasteBonus = StatConversion.GetPhysicalHasteFromRating(stats.HasteRating, CharacterClass.Druid);
			float attackSpeed = (2.5f) / (1f + hasteBonus);
			attackSpeed = attackSpeed / (1f + stats.PhysicalHaste);

			float hitBonus = StatConversion.GetPhysicalHitFromRating(stats.HitRating, CharacterClass.Druid) + stats.PhysicalHit;
			float expertiseBonus = StatConversion.GetDodgeParryReducFromExpertise(StatConversion.GetExpertiseFromRating(stats.ExpertiseRating, CharacterClass.Druid) + stats.Expertise, CharacterClass.Druid);
			float spellHitBonus = StatConversion.GetSpellHitFromRating(stats.HitRating, CharacterClass.Druid) + stats.SpellHit;
			
			int levelDifference = (targetLevel - character.Level);
			float chanceDodge = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[levelDifference] - expertiseBonus);
			float chanceParry = Math.Max(0f, StatConversion.WHITE_PARRY_CHANCE_CAP[levelDifference] - expertiseBonus);
			float chanceMiss = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[levelDifference] - hitBonus);
			float chanceResist = Math.Max(0f, StatConversion.GetSpellMiss(-levelDifference, false) - spellHitBonus);
			
			float chanceGlance = StatConversion.WHITE_GLANCE_CHANCE_CAP[levelDifference]; //0.2335774f;
			//float glanceMultiplier = 0.7f;
			float chanceAvoided = chanceMiss + chanceDodge + chanceParry;
			
			float rawChanceCrit = StatConversion.GetPhysicalCritFromRating(stats.CritRating, CharacterClass.Druid)
								+ StatConversion.GetPhysicalCritFromAgility(stats.Agility, CharacterClass.Druid)
								+ stats.PhysicalCrit
								+ StatConversion.NPC_LEVEL_CRIT_MOD[levelDifference];
			float chanceCrit = rawChanceCrit * (1f - chanceAvoided);
			
			float rawChanceCritSpell = StatConversion.GetSpellCritFromRating(stats.CritRating, CharacterClass.Druid)
								+ StatConversion.GetSpellCritFromIntellect(stats.Intellect, CharacterClass.Druid)
								+ stats.SpellCrit + stats.SpellCritOnTarget
								+ StatConversion.NPC_LEVEL_CRIT_MOD[levelDifference];
			float chanceCritSpell = rawChanceCritSpell * (1f - chanceResist);

			calculatedStats.DodgedAttacks = chanceDodge;
			calculatedStats.ParriedAttacks = chanceParry;
			calculatedStats.MissedAttacks = chanceMiss;

			BearAbilityBuilder abilities = new BearAbilityBuilder(stats,
				character.MainHand == null ? 0.75f : ((character.MainHand.MinDamage + character.MainHand.MaxDamage) / 2f) / character.MainHand.Speed,
				attackSpeed, modArmor, chanceAvoided, chanceResist, chanceCrit, chanceCritSpell, chanceGlance, critMultiplier, spellCritMultiplier);
			var optimalRotations = BearRotationCalculator.GetOptimalRotations(abilities);
			calculatedStats.Abilities = abilities;
			calculatedStats.HighestDPSRotation = optimalRotations.Item1;
			calculatedStats.HighestTPSRotation = optimalRotations.Item2;
			calculatedStats.ThreatPoints = calculatedStats.HighestTPSRotation.TPS * calcOpts.ThreatScale / 10f;
		}

		/// <summary>
		/// Gets the total Stats of the Character
		/// </summary>
		/// <param name="character">The Character to get the total Stats of</param>
		/// <param name="additionalItem">An additional item to grant the Character the stats of (as if it were worn)</param>
		/// <returns>The total stats for the Character</returns>
		public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
			CalculationOptionsBear calcOpts = character.CalculationOptions as CalculationOptionsBear ?? new CalculationOptionsBear();

			DruidTalents talents = character.DruidTalents;

			bool leatherSpecialization =	character.Head != null && character.Head.Type == ItemType.Leather &&
											character.Shoulders != null && character.Shoulders.Type == ItemType.Leather &&
											character.Chest != null && character.Chest.Type == ItemType.Leather &&
											character.Wrist != null && character.Wrist.Type == ItemType.Leather &&
											character.Hands != null && character.Hands.Type == ItemType.Leather &&
											character.Waist != null && character.Waist.Type == ItemType.Leather &&
											character.Legs != null && character.Legs.Type == ItemType.Leather &&
											character.Feet != null && character.Feet.Type == ItemType.Leather;

			bool hasCritBuff = false;
			foreach (Buff buff in character.ActiveBuffs)
				if (buff.Group == "Critical Strike Chance")
				{
					hasCritBuff = true;
					break;
				}

			StatsBear statsTotal = new StatsBear()
			{
				BonusAttackPowerMultiplier = 0.25f,
				BonusBleedDamageMultiplier = (character.ActiveBuffsContains("Mangle") || character.ActiveBuffsContains("Trauma") ? 0f : 0.3f),

				Dodge = 0.02f * talents.FeralSwiftness + 0.03f * talents.NaturalReaction,
				FurySwipesChance = 0.05f * talents.FurySwipes,
				BonusRageOnCrit = 2.5f * talents.PrimalFury,
				BonusEnrageDamageMultiplier = 0.05f * talents.KingOfTheJungle,
				HasteOnFeralCharge = 0.15f * talents.Stampede,
				BaseArmorMultiplier = 2.2f * (1f + 0.1f * talents.ThickHide / 3f) * (1f + 0.11f * talents.ThickHide) - 1f,
				CritChanceReduction = 0.02f * talents.ThickHide,
				PhysicalCrit = (hasCritBuff ? 0f : 0.05f * talents.LeaderOfThePack) + (talents.Pulverize > 0 ? 0.09f: 0f),
				SpellCrit = (hasCritBuff ? 0f : 0.05f * talents.LeaderOfThePack),
				BonusPulverizeDuration = 4f * talents.EndlessCarnage,
				DamageTakenMultiplier = -0.06f * talents.NaturalReaction,
				BonusMaulDamageMultiplier = 0.04f * talents.RendAndTear,
				
				BonusStaminaMultiplier = (1f + 0.02f * talents.HeartOfTheWild) * (leatherSpecialization ? 1.05f : 1f) - 1f,
				BonusPhysicalDamageMultiplier = 0.04f * talents.MasterShapeshifter,
				BonusMangleDamageMultiplier = talents.GlyphOfMangle ? 0.1f : 0f,
				BonusLacerateCritChance = talents.GlyphOfLacerate ? 0.05f : 0f,
				BonusFaerieFireStacks = talents.FeralAggression
			};
			statsTotal.Accumulate(BaseStats.GetBaseStats(character.Level, character.Class, character.Race, BaseStats.DruidForm.Bear));
			statsTotal.Accumulate(GetItemStats(character, additionalItem));
			statsTotal.Accumulate(GetBuffsStats(character, calcOpts));


			statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
			statsTotal.Strength = (float)Math.Floor(statsTotal.Strength * (1f + statsTotal.BonusStrengthMultiplier));
			statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1f + statsTotal.BonusAgilityMultiplier));
			statsTotal.AttackPower += (float)Math.Floor(statsTotal.Strength - 20f) * 2f + 20f;
			statsTotal.AttackPower += (float)Math.Floor(statsTotal.Agility - 20f) * 2f + 20f;
			statsTotal.AttackPower = (float)Math.Floor(statsTotal.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
			statsTotal.Health += ((statsTotal.Stamina - 20f) * 10f) * 1.4f + 20f;
			statsTotal.Health *= (1f + statsTotal.BonusHealthMultiplier);
			statsTotal.Armor *= 1f + statsTotal.BaseArmorMultiplier;
			statsTotal.Armor += statsTotal.BonusArmor;
			statsTotal.Armor = (float)Math.Floor(statsTotal.Armor * (1f + statsTotal.BonusArmorMultiplier));
			statsTotal.NatureResistance += statsTotal.NatureResistanceBuff;
			statsTotal.FireResistance += statsTotal.FireResistanceBuff;
			statsTotal.FrostResistance += statsTotal.FrostResistanceBuff;
			statsTotal.ShadowResistance += statsTotal.ShadowResistanceBuff;
			statsTotal.ArcaneResistance += statsTotal.ArcaneResistanceBuff;

			AccumulateProcs(character, statsTotal);

			return statsTotal;
		}

		private static void AccumulateProcs(Character character, StatsBear statsTotal)
		{
			CalculationOptionsBear calcOpts = character.CalculationOptions as CalculationOptionsBear ?? new CalculationOptionsBear();
			BossOptions bossOpts = character.BossOptions;
			int targetLevel = calcOpts.TargetLevel;
			float fightDuration = bossOpts.BerserkTimer;

			float hasteBonus = StatConversion.GetHasteFromRating(statsTotal.HasteRating, CharacterClass.Druid);
			float attackSpeed = (2.5f / (1f + hasteBonus)) / (1f + statsTotal.PhysicalHaste);
			float meleeHitInterval = 1f / (1f / attackSpeed + 1f / 1.5f);

			int levelDifference = (calcOpts.TargetLevel - character.Level);
			float hitBonus = StatConversion.GetPhysicalHitFromRating(statsTotal.HitRating) + statsTotal.PhysicalHit;
			float expertiseBonus = StatConversion.GetDodgeParryReducFromExpertise(StatConversion.GetExpertiseFromRating(statsTotal.ExpertiseRating, CharacterClass.Druid) + statsTotal.Expertise, CharacterClass.Druid);
			float chanceDodge = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[levelDifference] - expertiseBonus);
			float chanceParry = Math.Max(0f, StatConversion.WHITE_PARRY_CHANCE_CAP[levelDifference] - expertiseBonus);
			float chanceMiss = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[levelDifference] - hitBonus);
			float chanceAvoided = chanceMiss + chanceDodge + chanceParry;

			float rawChanceCrit = StatConversion.GetPhysicalCritFromRating(statsTotal.CritRating, CharacterClass.Druid)
								+ StatConversion.GetPhysicalCritFromAgility(statsTotal.Agility, CharacterClass.Druid)
								+ statsTotal.PhysicalCrit
								+ StatConversion.NPC_LEVEL_CRIT_MOD[levelDifference];
			float chanceCrit = rawChanceCrit * (1f - chanceAvoided);
			float chanceHit = 1f - chanceAvoided;

			float levelDifferenceAvoidance = levelDifference * 0.002f;
			float baseAgi = character.Race == CharacterRace.NightElf ? 87 : 77;

			float defSkill = (float)Math.Floor(StatConversion.GetDefenseFromRating(statsTotal.DefenseRating, CharacterClass.Druid));
			float dodgeNonDR = statsTotal.Dodge - levelDifferenceAvoidance + StatConversion.GetDodgeFromAgility(baseAgi, CharacterClass.Druid);
			float missNonDR = statsTotal.Miss - levelDifferenceAvoidance;
			float dodgePreDR = StatConversion.GetDodgeFromAgility(statsTotal.Agility - baseAgi, CharacterClass.Druid)
							 + StatConversion.GetDodgeFromRating(statsTotal.DodgeRating, CharacterClass.Druid)
							 + defSkill * StatConversion.DEFENSE_RATING_AVOIDANCE_MULTIPLIER / 100f;
			float missPreDR = (defSkill * StatConversion.DEFENSE_RATING_AVOIDANCE_MULTIPLIER / 100f);
			float dodgePostDR = 0.01f / (1f / 116.890707f + 0.00972f / dodgePreDR);
			float missPostDR = 0.01f / (1f / 16f + 0.00972f / missPreDR);
			float dodgeTotal = dodgeNonDR + dodgePostDR;
			float missTotal = missNonDR + missPostDR;

			float TargAttackSpeed = calcOpts.TargetAttackSpeed * (1f - statsTotal.BossAttackSpeedMultiplier);

			Stats statsProcs = new Stats();
			//float uptime;
			foreach (SpecialEffect effect in statsTotal.SpecialEffects())
			{
				switch (effect.Trigger)
				{
					case Trigger.Use:
						effect.AccumulateAverageStats(statsProcs, 0f, 1f, 2.5f, fightDuration);
						break;
					case Trigger.MeleeHit:
					case Trigger.PhysicalHit:
						effect.AccumulateAverageStats(statsProcs, meleeHitInterval, chanceHit, 2.5f, fightDuration);
						break;
					case Trigger.MeleeAttack:
						if (effect.Stats.MoteOfAnger > 0)
						{
							// When in effect stats, MoteOfAnger is % of melee hits
							// When in character stats, MoteOfAnger is average procs per second
							statsProcs.MoteOfAnger = effect.Stats.MoteOfAnger * effect.GetAverageProcsPerSecond(meleeHitInterval, 1f, 2.5f, fightDuration) / effect.MaxStack;
						}
						else
						{
							effect.AccumulateAverageStats(statsProcs, meleeHitInterval, 1f, 2.5f, fightDuration);
						}
						break;
					case Trigger.MeleeCrit:
					case Trigger.PhysicalCrit:
						effect.AccumulateAverageStats(statsProcs, meleeHitInterval, chanceCrit, 2.5f, fightDuration);
						break;
					case Trigger.DoTTick:
						effect.AccumulateAverageStats(statsProcs, 3f, 1f, 2.5f, fightDuration);
						break;
					case Trigger.DamageDone:
						effect.AccumulateAverageStats(statsProcs, meleeHitInterval / 2f, 1f, 2.5f, fightDuration);
						break;
					case Trigger.DamageOrHealingDone:
						effect.AccumulateAverageStats(statsProcs, meleeHitInterval / 2f, 1f, 2.5f, fightDuration); // also needs healing
						break;
					case Trigger.MangleBearHit:
						effect.AccumulateAverageStats(statsProcs, 6f - statsTotal.MangleCooldownReduction, chanceHit, 2.5f, fightDuration);
						break;
					case Trigger.MangleCatOrShredOrInfectedWoundsHit:
						effect.AccumulateAverageStats(statsProcs, 1f /
							(1f / (6f - statsTotal.MangleCooldownReduction) + //Mangles Per Second
							1f / meleeHitInterval) //Mauls Per Second
							, chanceHit, 2.5f, fightDuration);
						break;
					case Trigger.SwipeBearOrLacerateHit:
						effect.AccumulateAverageStats(statsProcs, 2.25f, chanceHit, 2.5f, fightDuration);
						break;
					case Trigger.LacerateTick:
						effect.AccumulateAverageStats(statsProcs, 3f, 1f, 2.5f, fightDuration);
						break;
					case Trigger.DamageTaken:
						effect.AccumulateAverageStats(statsProcs, TargAttackSpeed * 0.8f, 1f - 0.8f * (dodgeTotal + missTotal), fightDuration); //Assume you get hit by other things, like dots, aoes, etc, making you get targeted with damage 25% more often than the boss, and half the hits you take are unavoidable.
						break;
					case Trigger.DamageTakenPhysical:
						effect.AccumulateAverageStats(statsProcs, TargAttackSpeed, 1f - (dodgeTotal + missTotal), fightDuration); //Assume you get hit by other things, like dots, aoes, etc, making you get targeted with damage 25% more often than the boss, and half the hits you take are unavoidable.
						break;
				}
			}

			statsProcs.Agility += statsProcs.HighestStat + statsProcs.Paragon;
			statsProcs.Stamina = (float)Math.Floor(statsProcs.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
			statsProcs.Strength = (float)Math.Floor(statsProcs.Strength * (1f + statsTotal.BonusStrengthMultiplier));
			statsProcs.Agility = (float)Math.Floor(statsProcs.Agility * (1f + statsTotal.BonusAgilityMultiplier));
			statsProcs.AttackPower += statsProcs.Strength * 2f + statsProcs.Agility * 2f;
			statsProcs.AttackPower = (float)Math.Floor(statsProcs.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
			statsProcs.Health += (float)Math.Floor(statsProcs.Stamina * 10f) + (float)Math.Floor(statsProcs.BattlemasterHealth);
			statsProcs.Health *= (1f + statsProcs.BonusHealthMultiplier);
			statsProcs.Armor += /*2f * (float)Math.Floor(statsProcs.Agility)*/ + statsProcs.BonusArmor; // Armor no longer gets bonuses from Agi in Cata
			statsProcs.Armor = (float)Math.Floor(statsProcs.Armor * (1f + statsTotal.BonusArmorMultiplier));
			statsTotal.Accumulate(statsProcs);
		}


		//NOTE: This is currently unused, because it doesn't account for procs which are both mitigation and survival (ie armor and agility procs)
		private void AccumulateSpecialEffect(Stats statsProcs, Stats statsEffect, float effectUptime, float temporarySurvivalScale)
		{
			if (temporarySurvivalScale != 1f && statsEffect.Armor + statsEffect.Health + statsEffect.Stamina > 0f)
			{
				//Subject to Temporary Survival scaling
				if (temporarySurvivalScale < 1f)
				{
					statsProcs.Accumulate(statsEffect, effectUptime * temporarySurvivalScale);
				}
				else
				{
					statsProcs.Accumulate(statsEffect, 1f - ((2f - temporarySurvivalScale) * (1f - effectUptime)));
				}
			}
			else
			{
				statsProcs.Accumulate(statsEffect, effectUptime);
			}
		}

		/// <summary>
		/// Gets data for a custom chart that Rawr.Bear provides
		/// </summary>
		/// <param name="character">The Character to get the chart data for</param>
		/// <param name="chartName">The name of the custom chart to get data for</param>
		/// <returns>The data for the custom chart</returns>
		public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
		{
			switch (chartName)
			{
				case "Combat Table":
					CharacterCalculationsBear currentCalculationsBear = GetCharacterCalculations(character) as CharacterCalculationsBear;
					ComparisonCalculationBear calcMiss = new ComparisonCalculationBear();
					ComparisonCalculationBear calcDodge = new ComparisonCalculationBear();
					ComparisonCalculationBear calcCrit = new ComparisonCalculationBear();
					//ComparisonCalculationBear calcCrush = new ComparisonCalculationBear();
					ComparisonCalculationBear calcHit = new ComparisonCalculationBear();
					if (currentCalculationsBear != null)
					{
						calcMiss.Name = "    Miss    ";
						calcDodge.Name = "   Dodge   ";
						calcCrit.Name = "  Crit  ";
						//calcCrush.Name = " Crush ";
						calcHit.Name = "Hit";

						float crits = 0.02f + (0.002f * (currentCalculationsBear.TargetLevel - character.Level)) - currentCalculationsBear.CappedCritReduction;
						//float crushes = currentCalculationsBear.TargetLevel == 73 ? Math.Max(Math.Min(100f - (crits + (currentCalculationsBear.AvoidancePreDR)), 15f) - currentCalculationsBear.BasicStats.CritChanceReduction, 0f) : 0f;
						float hits = Math.Max(1f - (Math.Max(0f, crits) + /*Math.Max(crushes, 0)*/ +(currentCalculationsBear.AvoidancePreDR)), 0f);

						calcMiss.OverallPoints = calcMiss.MitigationPoints = currentCalculationsBear.Miss * 100f;
						calcDodge.OverallPoints = calcDodge.MitigationPoints = currentCalculationsBear.Dodge * 100f;
						calcCrit.OverallPoints = calcCrit.SurvivalPoints = crits * 100f;
						//calcCrush.OverallPoints = calcCrush.SurvivalPoints = crushes;
						calcHit.OverallPoints = calcHit.SurvivalPoints = hits * 100f;
					}
					return new ComparisonCalculationBase[] { calcMiss, calcDodge, calcCrit, /*calcCrush,*/ calcHit };

				//Not used anymore
				//case "Relative Stat Values": 
				//    CharacterCalculationsBear calcBaseValue = GetCharacterCalculations(character) as CharacterCalculationsBear;
				//    //CharacterCalculationsBear calcAgiValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 10 } }) as CharacterCalculationsBear;
				//    //CharacterCalculationsBear calcACValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Armor = 10 } }) as CharacterCalculationsBear;
				//    //CharacterCalculationsBear calcStaValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = 10 } }) as CharacterCalculationsBear;
				//    //CharacterCalculationsBear calcDefValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DefenseRating = 1 } }) as CharacterCalculationsBear;
				//    CharacterCalculationsBear calcDodgeValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DodgeRating = 1 } }) as CharacterCalculationsBear;
				//    CharacterCalculationsBear calcHealthValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Health = 1 } }) as CharacterCalculationsBear;
				//    CharacterCalculationsBear calcResilValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Resilience = 1 } }) as CharacterCalculationsBear;
				//    //CharacterCalculationsBear calcStrengthValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = 1 } }) as CharacterCalculationsBear;
				//    CharacterCalculationsBear calcAPValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { AttackPower = 1 } }) as CharacterCalculationsBear;
				//    CharacterCalculationsBear calcExpertiseValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { ExpertiseRating = 1 } }) as CharacterCalculationsBear;
				//    CharacterCalculationsBear calcHitValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 1 } }) as CharacterCalculationsBear;
				//    CharacterCalculationsBear calcPenValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { ArmorPenetration = 1 } }) as CharacterCalculationsBear;
				//    CharacterCalculationsBear calcHasteValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 1 } }) as CharacterCalculationsBear;
				//    CharacterCalculationsBear calcDamageValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { WeaponDamage = 1 } }) as CharacterCalculationsBear;
				//    CharacterCalculationsBear calcCritValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 1 } }) as CharacterCalculationsBear;

				//    //Differential Calculations for Agi
				//    CharacterCalculationsBear calcAtAdd = calcBaseValue;
				//    float agiToAdd = 0f;
				//    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && agiToAdd < 2)
				//    {
				//        agiToAdd += 0.01f;
				//        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToAdd } }) as CharacterCalculationsBear;
				//    }

				//    CharacterCalculationsBear calcAtSubtract = calcBaseValue;
				//    float agiToSubtract = 0f;
				//    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && agiToSubtract > -2)
				//    {
				//        agiToSubtract -= 0.01f;
				//        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToSubtract } }) as CharacterCalculationsBear;
				//    }
				//    agiToSubtract += 0.01f;

				//    ComparisonCalculationBear comparisonAgi = new ComparisonCalculationBear() { 
				//        Name = string.Format("Agility ({0})", agiToAdd-agiToSubtract), 
				//        OverallPoints =     (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (agiToAdd - agiToSubtract),
				//        MitigationPoints =  (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (agiToAdd - agiToSubtract), 
				//        SurvivalPoints =    (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (agiToAdd - agiToSubtract),
				//        ThreatPoints =      (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (agiToAdd - agiToSubtract)};


				//    //Differential Calculations for Str
				//    calcAtAdd = calcBaseValue;
				//    float strToAdd = 0f;
				//    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && strToAdd < 2)
				//    {
				//        strToAdd += 0.01f;
				//        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToAdd } }) as CharacterCalculationsBear;
				//    }

				//    calcAtSubtract = calcBaseValue;
				//    float strToSubtract = 0f;
				//    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && strToSubtract > -2)
				//    {
				//        strToSubtract -= 0.01f;
				//        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToSubtract } }) as CharacterCalculationsBear;
				//    }
				//    strToSubtract += 0.01f;

				//    ComparisonCalculationBear comparisonStr = new ComparisonCalculationBear()
				//    {
				//        Name = string.Format("Strength ({0})", strToAdd-strToSubtract),
				//        OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (strToAdd - strToSubtract),
				//        MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (strToAdd - strToSubtract),
				//        SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (strToAdd - strToSubtract),
				//        ThreatPoints = (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (strToAdd - strToSubtract)
				//    };


				//    //Differential Calculations for Def
				//    calcAtAdd = calcBaseValue;
				//    float defToAdd = 0f;
				//    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && defToAdd < 20)
				//    {
				//        defToAdd += 0.01f;
				//        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DefenseRating = defToAdd } }) as CharacterCalculationsBear;
				//    }

				//    calcAtSubtract = calcBaseValue;
				//    float defToSubtract = 0f;
				//    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && defToSubtract > -20)
				//    {
				//        defToSubtract -= 0.01f;
				//        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DefenseRating = defToSubtract } }) as CharacterCalculationsBear;
				//    }
				//    defToSubtract += 0.01f;

				//    ComparisonCalculationBear comparisonDef = new ComparisonCalculationBear()
				//    {
				//        Name = string.Format("Defense Rating ({0})", defToAdd-defToSubtract),
				//        OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (defToAdd - defToSubtract),
				//        MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (defToAdd - defToSubtract),
				//        SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (defToAdd - defToSubtract),
				//        ThreatPoints = (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (defToAdd - defToSubtract)
				//    };


				//    //Differential Calculations for AC
				//    calcAtAdd = calcBaseValue;
				//    float acToAdd = 0f;
				//    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && acToAdd < 2)
				//    {
				//        acToAdd += 0.01f;
				//        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Armor = acToAdd } }) as CharacterCalculationsBear;
				//    }

				//    calcAtSubtract = calcBaseValue;
				//    float acToSubtract = 0f;
				//    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && acToSubtract > -2)
				//    {
				//        acToSubtract -= 0.01f;
				//        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Armor = acToSubtract } }) as CharacterCalculationsBear;
				//    }
				//    acToSubtract += 0.01f;

				//    ComparisonCalculationBear comparisonAC = new ComparisonCalculationBear() {
				//        Name = string.Format("Armor ({0})", acToAdd-acToSubtract),
				//        OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (acToAdd - acToSubtract),
				//        MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (acToAdd - acToSubtract), 
				//        SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (acToAdd - acToSubtract), 
				//        ThreatPoints = (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (acToAdd - acToSubtract)
				//    };


				//    //Differential Calculations for BonusAC
				//    calcAtAdd = calcBaseValue;
				//    float bacToAdd = 0f;
				//    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && bacToAdd < 2)
				//    {
				//        bacToAdd += 0.01f;
				//        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { BonusArmor = bacToAdd } }) as CharacterCalculationsBear;
				//    }

				//    calcAtSubtract = calcBaseValue;
				//    float bacToSubtract = 0f;
				//    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && bacToSubtract > -2)
				//    {
				//        bacToSubtract -= 0.01f;
				//        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { BonusArmor = bacToSubtract } }) as CharacterCalculationsBear;
				//    }
				//    bacToSubtract += 0.01f;

				//    ComparisonCalculationBear comparisonBAC = new ComparisonCalculationBear()
				//    {
				//        Name = string.Format("Bonus Armor ({0})", bacToAdd - bacToSubtract),
				//        OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (bacToAdd - bacToSubtract),
				//        MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (bacToAdd - bacToSubtract),
				//        SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (bacToAdd - bacToSubtract),
				//        ThreatPoints = (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (bacToAdd - bacToSubtract)
				//    };


				//    //Differential Calculations for Sta
				//    calcAtAdd = calcBaseValue;
				//    float staToAdd = 0f;
				//    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && staToAdd < 2)
				//    {
				//        staToAdd += 0.01f;
				//        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = staToAdd } }) as CharacterCalculationsBear;
				//    }

				//    calcAtSubtract = calcBaseValue;
				//    float staToSubtract = 0f;
				//    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && staToSubtract > -2)
				//    {
				//        staToSubtract -= 0.01f;
				//        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = staToSubtract } }) as CharacterCalculationsBear;
				//    }
				//    staToSubtract += 0.01f;

				//    ComparisonCalculationBear comparisonSta = new ComparisonCalculationBear() {
				//        Name = string.Format("Stamina ({0})", staToAdd-staToSubtract),
				//        OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (staToAdd - staToSubtract),
				//        MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (staToAdd - staToSubtract), 
				//        SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (staToAdd - staToSubtract), 
				//        ThreatPoints = (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (staToAdd - staToSubtract)};

				//    return new ComparisonCalculationBase[] { 
				//        comparisonAgi,
				//        comparisonAC,
				//        comparisonBAC,
				//        comparisonSta,
				//        comparisonDef,
				//        comparisonStr,
				//        new ComparisonCalculationBear() { Name = "Attack Power", 
				//            OverallPoints = (calcAPValue.OverallPoints - calcBaseValue.OverallPoints), 
				//            MitigationPoints = (calcAPValue.MitigationPoints - calcBaseValue.MitigationPoints), 
				//            SurvivalPoints = (calcAPValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
				//            ThreatPoints = (calcAPValue.ThreatPoints - calcBaseValue.ThreatPoints)},
				//        new ComparisonCalculationBear() { Name = "Hit Rating", 
				//            OverallPoints = (calcHitValue.OverallPoints - calcBaseValue.OverallPoints), 
				//            MitigationPoints = (calcHitValue.MitigationPoints - calcBaseValue.MitigationPoints), 
				//            SurvivalPoints = (calcHitValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
				//            ThreatPoints = (calcHitValue.ThreatPoints - calcBaseValue.ThreatPoints)},
				//        new ComparisonCalculationBear() { Name = "Crit Rating", 
				//            OverallPoints = (calcCritValue.OverallPoints - calcBaseValue.OverallPoints), 
				//            MitigationPoints = (calcCritValue.MitigationPoints - calcBaseValue.MitigationPoints), 
				//            SurvivalPoints = (calcCritValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
				//            ThreatPoints = (calcCritValue.ThreatPoints - calcBaseValue.ThreatPoints)},
				//        new ComparisonCalculationBear() { Name = "Weapon Damage", 
				//            OverallPoints = (calcDamageValue.OverallPoints - calcBaseValue.OverallPoints), 
				//            MitigationPoints = (calcDamageValue.MitigationPoints - calcBaseValue.MitigationPoints), 
				//            SurvivalPoints = (calcDamageValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
				//            ThreatPoints = (calcDamageValue.ThreatPoints - calcBaseValue.ThreatPoints)},
				//        new ComparisonCalculationBear() { Name = "Haste Rating", 
				//            OverallPoints = (calcHasteValue.OverallPoints - calcBaseValue.OverallPoints), 
				//            MitigationPoints = (calcHasteValue.MitigationPoints - calcBaseValue.MitigationPoints), 
				//            SurvivalPoints = (calcHasteValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
				//            ThreatPoints = (calcHasteValue.ThreatPoints - calcBaseValue.ThreatPoints)},
				//        new ComparisonCalculationBear() { Name = "Expertise Rating", 
				//            OverallPoints = (calcExpertiseValue.OverallPoints - calcBaseValue.OverallPoints), 
				//            MitigationPoints = (calcExpertiseValue.MitigationPoints - calcBaseValue.MitigationPoints), 
				//            SurvivalPoints = (calcExpertiseValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
				//            ThreatPoints = (calcExpertiseValue.ThreatPoints - calcBaseValue.ThreatPoints)},

				//        new ComparisonCalculationBear() { Name = "Dodge Rating", 
				//            OverallPoints = (calcDodgeValue.OverallPoints - calcBaseValue.OverallPoints), 
				//            MitigationPoints = (calcDodgeValue.MitigationPoints - calcBaseValue.MitigationPoints), 
				//            SurvivalPoints = (calcDodgeValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
				//            ThreatPoints = (calcDodgeValue.ThreatPoints - calcBaseValue.ThreatPoints)},
				//        new ComparisonCalculationBear() { Name = "Health", 
				//            OverallPoints = (calcHealthValue.OverallPoints - calcBaseValue.OverallPoints), 
				//            MitigationPoints = (calcHealthValue.MitigationPoints - calcBaseValue.MitigationPoints), 
				//            SurvivalPoints = (calcHealthValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
				//            ThreatPoints = (calcHealthValue.ThreatPoints - calcBaseValue.ThreatPoints)},
				//        new ComparisonCalculationBear() { Name = "Resilience", 
				//            OverallPoints = (calcResilValue.OverallPoints - calcBaseValue.OverallPoints), 
				//            MitigationPoints = (calcResilValue.MitigationPoints - calcBaseValue.MitigationPoints), 
				//            SurvivalPoints = (calcResilValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
				//            ThreatPoints = (calcResilValue.ThreatPoints - calcBaseValue.ThreatPoints)},
				//    };

				//case "Rotation DPS":
				//    CharacterCalculationsBear calcsDPS = GetCharacterCalculations(character) as CharacterCalculationsBear;
				//    List<ComparisonCalculationBase> comparisonsDPS = new List<ComparisonCalculationBase>();

				//    BearRotationCalculator rotationCalculatorDPS = new BearRotationCalculator(calcsDPS.MeleeDamageAverage,
				//        calcsDPS.MaulDamageAverage, calcsDPS.MangleDamageAverage, calcsDPS.SwipeDamageAverage,
				//        calcsDPS.FaerieFireDamageAverage, calcsDPS.LacerateDamageAverage, calcsDPS.LacerateDotDamageAverage, calcsDPS.MeleeThreatAverage,
				//        calcsDPS.MaulThreatAverage, calcsDPS.MangleThreatAverage, calcsDPS.SwipeThreatAverage,
				//        calcsDPS.FaerieFireThreatAverage, calcsDPS.LacerateThreatAverage, calcsDPS.LacerateDotThreatAverage,
				//        6f - calcsDPS.BasicStats.MangleCooldownReduction, calcsDPS.AttackSpeed);

				//    for (int useMaul = 0; useMaul < 3; useMaul++)
				//        for (int useMangle = 0; useMangle < 2; useMangle++)
				//            for (int useSwipe = 0; useSwipe < 2; useSwipe++)
				//                for (int useFaerieFire = 0; useFaerieFire < 2; useFaerieFire++)
				//                    for (int useLacerate = 0; useLacerate < 2; useLacerate++)
				//                    {
				//                        bool?[] useMaulValues = new bool?[] { null, false, true };
				//                        BearRotationCalculator.BearRotationCalculation rotationCalculation =
				//                            rotationCalculatorDPS.GetRotationCalculations(useMaulValues[useMaul],
				//                            useMangle == 1, useSwipe == 1, useFaerieFire == 1, useLacerate == 1);

				//                        comparisonsDPS.Add(new ComparisonCalculationBear()
				//                        {
				//                            CharacterItems = character.GetItems(),
				//                            Name = rotationCalculation.Name.Replace("+", " + "),
				//                            OverallPoints = rotationCalculation.DPS,
				//                            ThreatPoints = rotationCalculation.DPS
				//                        });
				//                    }

				//    return comparisonsDPS.ToArray();

				//case "Rotation TPS":
				//    CharacterCalculationsBear calcsTPS = GetCharacterCalculations(character) as CharacterCalculationsBear;
				//    List<ComparisonCalculationBase> comparisonsTPS = new List<ComparisonCalculationBase>();

				//    BearRotationCalculator rotationCalculatorTPS = new BearRotationCalculator(calcsTPS.MeleeDamageAverage,
				//        calcsTPS.MaulDamageAverage, calcsTPS.MangleDamageAverage, calcsTPS.SwipeDamageAverage,
				//        calcsTPS.FaerieFireDamageAverage, calcsTPS.LacerateDamageAverage, calcsTPS.LacerateDotDamageAverage, calcsTPS.MeleeThreatAverage,
				//        calcsTPS.MaulThreatAverage, calcsTPS.MangleThreatAverage, calcsTPS.SwipeThreatAverage,
				//        calcsTPS.FaerieFireThreatAverage, calcsTPS.LacerateThreatAverage, calcsTPS.LacerateDotThreatAverage,
				//        6f - calcsTPS.BasicStats.MangleCooldownReduction, calcsTPS.AttackSpeed);

				//    for (int useMaul = 0; useMaul < 3; useMaul++)
				//        for (int useMangle = 0; useMangle < 2; useMangle++)
				//            for (int useSwipe = 0; useSwipe < 2; useSwipe++)
				//                for (int useFaerieFire = 0; useFaerieFire < 2; useFaerieFire++)
				//                    for (int useLacerate = 0; useLacerate < 2; useLacerate++)
				//                    {
				//                        bool?[] useMaulValues = new bool?[] { null, false, true };
				//                        BearRotationCalculator.BearRotationCalculation rotationCalculation =
				//                            rotationCalculatorTPS.GetRotationCalculations(useMaulValues[useMaul],
				//                            useMangle == 1, useSwipe == 1, useFaerieFire == 1, useLacerate == 1);

				//                        comparisonsTPS.Add(new ComparisonCalculationBear()
				//                        {
				//                            CharacterItems = character.GetItems(),
				//                            Name = rotationCalculation.Name.Replace("+", " + "),
				//                            OverallPoints = rotationCalculation.TPS,
				//                            ThreatPoints = rotationCalculation.TPS
				//                        });
				//                    }

				//    return comparisonsTPS.ToArray();

				default:
					return new ComparisonCalculationBase[0];
			}
		}
		#endregion

		#region Relevancy Methods
		public override bool IsItemRelevant(Item item)
		{
			if (item.Slot == ItemSlot.OffHand ||
				(item.Slot == ItemSlot.Ranged && item.Type != ItemType.Idol && item.Type != ItemType.Relic) ||
				item.Stats.SpellPower > 0) 
				return false;
			return base.IsItemRelevant(item);
		}

		public override Stats GetRelevantStats(Stats stats)
		{
			Stats s = new Stats()
			{
				Armor = stats.Armor,
				BonusArmor = stats.BonusArmor,
				Stamina = stats.Stamina,
				Agility = stats.Agility,
				DodgeRating = stats.DodgeRating,
				DefenseRating = stats.DefenseRating,
				MasteryRating = stats.MasteryRating,
				Resilience = stats.Resilience,
				BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
				BaseArmorMultiplier = stats.BaseArmorMultiplier,
				BonusArmorMultiplier = stats.BonusArmorMultiplier,
				BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
				BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
				Health = stats.Health,
				BattlemasterHealth = stats.BattlemasterHealth,
				BonusHealthMultiplier = stats.BonusHealthMultiplier,
				Miss = stats.Miss,
				CritChanceReduction = stats.CritChanceReduction,
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
				HighestStat = stats.HighestStat,
				Paragon = stats.Paragon,
				Dodge = stats.Dodge,

				Strength = stats.Strength,
				AttackPower = stats.AttackPower,
				CritRating = stats.CritRating,
				PhysicalCrit = stats.PhysicalCrit,
				HitRating = stats.HitRating,
				PhysicalHit = stats.PhysicalHit,
				MoteOfAnger = stats.MoteOfAnger,
				HasteRating = stats.HasteRating,
				PhysicalHaste = stats.PhysicalHaste,
				ExpertiseRating = stats.ExpertiseRating,
				ArmorPenetration = stats.ArmorPenetration,
				ArmorPenetrationRating = stats.ArmorPenetrationRating,
				TargetArmorReduction = stats.TargetArmorReduction,
				WeaponDamage = stats.WeaponDamage,
				BonusCritMultiplier = stats.BonusCritMultiplier,
				BonusMangleBearThreat = stats.BonusMangleBearThreat,
				BonusLacerateDamageMultiplier = stats.BonusLacerateDamageMultiplier,
				BonusBearSwipeDamageMultiplier = stats.BonusBearSwipeDamageMultiplier,
				BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
				BonusDamageMultiplier = stats.BonusDamageMultiplier,
				DamageTakenMultiplier = stats.DamageTakenMultiplier,
				BossPhysicalDamageDealtMultiplier = stats.BossPhysicalDamageDealtMultiplier,
				BossAttackSpeedMultiplier = stats.BossAttackSpeedMultiplier,
				SpellCrit = stats.SpellCrit,
				SpellCritOnTarget = stats.SpellCritOnTarget,
				Intellect = stats.Intellect,
				BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier,
				SpellHit = stats.SpellHit,
				ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
				MovementSpeed = stats.MovementSpeed,
				FearDurReduc = stats.FearDurReduc,
				StunDurReduc = stats.StunDurReduc,
				SnareRootDurReduc = stats.SnareRootDurReduc,
			};
			foreach (SpecialEffect effect in stats.SpecialEffects())
			{
				if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit || effect.Trigger == Trigger.MeleeAttack
				|| effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit || effect.Trigger == Trigger.DoTTick
				|| effect.Trigger == Trigger.DamageDone || effect.Trigger == Trigger.MangleBearHit || effect.Trigger == Trigger.LacerateTick
				|| effect.Trigger == Trigger.SwipeBearOrLacerateHit || effect.Trigger == Trigger.DamageTaken || effect.Trigger == Trigger.DamageTakenPhysical
				|| effect.Trigger == Trigger.MangleCatOrShredOrInfectedWoundsHit || effect.Trigger == Trigger.DamageOrHealingDone)
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
			bool relevant = (
				// Core Stats
				stats.Agility + stats.BonusAgilityMultiplier +
				stats.Stamina + stats.BonusStaminaMultiplier +
				stats.Health + stats.BonusHealthMultiplier +
				stats.Strength +
				stats.AttackPower + stats.BonusAttackPowerMultiplier +
				stats.CritRating + stats.PhysicalCrit + stats.BonusCritMultiplier + 
				stats.HasteRating + stats.PhysicalHaste +
				stats.Armor + stats.BonusArmor + stats.BonusArmorMultiplier +
				stats.DodgeRating + stats.Dodge +
				stats.HitRating + stats.PhysicalHit +
				stats.ExpertiseRating +
				stats.ArmorPenetration +
				stats.BonusDamageMultiplier +
				stats.BonusNatureDamageMultiplier +
				stats.MasteryRating +
				stats.ThreatIncreaseMultiplier + 
				// Stats that are for Target
				stats.TargetArmorReduction +
				stats.BossAttackSpeedMultiplier +
				stats.DamageTakenMultiplier +
				stats.BossPhysicalDamageDealtMultiplier +
				stats.SpellCritOnTarget +
				// Maybe Stats
				stats.Resilience +
				stats.WeaponDamage +
				stats.SpellCrit + 
				stats.Intellect + stats.SpellHit + 
				// Resistances
				stats.ArcaneResistance + stats.ArcaneResistanceBuff +
				stats.NatureResistance + stats.NatureResistanceBuff + 
				stats.FireResistance + stats.FireResistanceBuff +
				stats.FrostResistance + stats.FrostResistanceBuff +
				stats.ShadowResistance + stats.ShadowResistanceBuff + 
				// Specials
				stats.Miss +
				stats.CritChanceReduction +
				stats.BattlemasterHealth + stats.MoteOfAnger +
				stats.HighestStat + stats.Paragon + 
				// Specific to Bear
				stats.BonusBearSwipeDamageMultiplier +
				stats.BonusRipDuration + 
				stats.BonusMangleBearThreat + 
				stats.BonusLacerateDamageMultiplier +
				// Boss Handler
				stats.MovementSpeed + stats.FearDurReduc + stats.StunDurReduc + stats.SnareRootDurReduc
				 ) != 0;

			foreach (SpecialEffect effect in stats.SpecialEffects())
			{
				if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit || effect.Trigger == Trigger.MeleeAttack
					|| effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit
					|| effect.Trigger == Trigger.MangleBearHit || effect.Trigger == Trigger.SwipeBearOrLacerateHit
					|| effect.Trigger == Trigger.DamageTaken || effect.Trigger == Trigger.DamageTakenPhysical
					|| effect.Trigger == Trigger.MangleCatOrShredOrInfectedWoundsHit || effect.Trigger == Trigger.LacerateTick)
				{
					relevant |= HasRelevantStats(effect.Stats);
					if (relevant) break;
				}
			}
			return relevant;
		}

		private static List<string> _relevantGlyphs;
		public override List<string> GetRelevantGlyphs()
		{
			if (_relevantGlyphs == null)
			{
				_relevantGlyphs = new List<string>();
				_relevantGlyphs.Add("Glyph of Aquatic Form");
				_relevantGlyphs.Add("Glyph of Barkskin");
				_relevantGlyphs.Add("Glyph of Berserk");
				_relevantGlyphs.Add("Glyph of Challenging Roar");
				_relevantGlyphs.Add("Glyph of Dash");
				_relevantGlyphs.Add("Glyph of Entangling Roots");
				_relevantGlyphs.Add("Glyph of Faerie Fire");
				_relevantGlyphs.Add("Glyph of Feral Charge");
				_relevantGlyphs.Add("Glyph of Ferocious Bite");
				_relevantGlyphs.Add("Glyph of Frenzied Regeneration");
				_relevantGlyphs.Add("Glyph of Lacerate");
				_relevantGlyphs.Add("Glyph of Mangle");
				_relevantGlyphs.Add("Glyph of Maul");
				_relevantGlyphs.Add("Glyph of Rake");
				_relevantGlyphs.Add("Glyph of Rebirth");
				_relevantGlyphs.Add("Glyph of Rip");
				_relevantGlyphs.Add("Glyph of Savage Roar");
				_relevantGlyphs.Add("Glyph of Shred");
				_relevantGlyphs.Add("Glyph of The Wild");
				_relevantGlyphs.Add("Glyph of Thorns");
				_relevantGlyphs.Add("Glyph of Tiger's Fury");
				_relevantGlyphs.Add("Glyph of Unburdened Rebirth");
			}
			return _relevantGlyphs;
		}

		#endregion

		public Stats GetBuffsStats(Character character, CalculationOptionsBear calcOpts)
		{
			List<Buff> removedBuffs = new List<Buff>();
			List<Buff> addedBuffs = new List<Buff>();

			//float hasRelevantBuff;

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
			}*/
			#endregion

			Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

			foreach (Buff b in removedBuffs)
			{
				character.ActiveBuffsAdd(b);
			}
			foreach (Buff b in addedBuffs)
			{
				character.ActiveBuffs.Remove(b);
			}

			return statsBuffs;
		}
		public override void SetDefaults(Character character)
		{
			character.ActiveBuffsAdd("Battle Shout");
			character.ActiveBuffsAdd("Devotion Aura");
			character.ActiveBuffsAdd("Blessing of Might (AP%)");
			character.ActiveBuffsAdd("Power Word: Fortitude");
			character.ActiveBuffsAdd("Leader of the Pack");
			character.ActiveBuffsAdd("Windfury Totem");
			character.ActiveBuffsAdd("Mark of the Wild");
			character.ActiveBuffsAdd("Faerie Fire");
			character.ActiveBuffsAdd("Curse of the Elements");
			character.ActiveBuffsAdd("Infected Wounds");
			character.ActiveBuffsAdd("Flask of Steelskin");
			character.ActiveBuffsAdd("Agility Food");

			//Prime
			character.DruidTalents.GlyphOfBerserk = true;
			character.DruidTalents.GlyphOfLacerate = true;
			character.DruidTalents.GlyphOfMangle = true;
			
			//Major
			//character.DruidTalents.GlyphOfFaerieFire = true;
			character.DruidTalents.GlyphOfFeralCharge = true;
			character.DruidTalents.GlyphOfFrenziedRegeneration = true;
			character.DruidTalents.GlyphOfMaul = true;
			//character.DruidTalents.GlyphOfThorns = true;
			
			//Minor
			character.DruidTalents.GlyphOfChallengingRoar = true;
			character.DruidTalents.GlyphOfUnburdenedRebirth = true;
			character.DruidTalents.GlyphOfDash = true;
		}
	}
}
