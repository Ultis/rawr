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
using System.Text;

namespace Rawr.Bear
{
	/// <summary>
	/// Core class representing the Rawr.Bear model
	/// </summary>
	[Rawr.Calculations.RawrModelInfo("Bear", "Ability_Racial_BearForm", Character.CharacterClass.Druid)]
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

				//Meta
				int austere = 41380;
				// int relentless = 41398;

				return new List<GemmingTemplate>()
				{
					new GemmingTemplate() { Model = "Bear", Group = "Uncommon", //Max Mitigation
						RedId = delicate[0], YellowId = delicate[0], BlueId = delicate[0], PrismaticId = delicate[0], MetaId = austere },
					new GemmingTemplate() { Model = "Bear", Group = "Uncommon", //Mitigation Heavier 
						RedId = delicate[0], YellowId = stalwart[0], BlueId = shifting[0], PrismaticId = delicate[0], MetaId = austere },
					new GemmingTemplate() { Model = "Bear", Group = "Uncommon", //Mitigation Heavy
						RedId = delicate[0], YellowId = glinting[0], BlueId = shifting[0], PrismaticId = delicate[0], MetaId = austere },
					new GemmingTemplate() { Model = "Bear", Group = "Uncommon", //Survivbility Heavy
						RedId = shifting[0], YellowId = enduring[0], BlueId = solid[0], PrismaticId = solid[0], MetaId = austere },
					new GemmingTemplate() { Model = "Bear", Group = "Uncommon", //Max Survivbility
						RedId = solid[0], YellowId = solid[0], BlueId = solid[0], PrismaticId = solid[0], MetaId = austere },

					new GemmingTemplate() { Model = "Bear", Group = "Rare", Enabled = true, //Max Mitigation
						RedId = delicate[1], YellowId = delicate[1], BlueId = delicate[1], PrismaticId = delicate[1], MetaId = austere },
					new GemmingTemplate() { Model = "Bear", Group = "Rare", Enabled = true, //Mitigation Heavier 
						RedId = delicate[1], YellowId = stalwart[1], BlueId = shifting[1], PrismaticId = delicate[1], MetaId = austere },
					new GemmingTemplate() { Model = "Bear", Group = "Rare", Enabled = true, //Mitigation Heavy
						RedId = delicate[1], YellowId = glinting[1], BlueId = shifting[1], PrismaticId = delicate[1], MetaId = austere },
					new GemmingTemplate() { Model = "Bear", Group = "Rare", Enabled = true, //Survivbility Heavy
						RedId = shifting[1], YellowId = enduring[1], BlueId = solid[1], PrismaticId = solid[1], MetaId = austere },
					new GemmingTemplate() { Model = "Bear", Group = "Rare", Enabled = true, //Max Survivbility
						RedId = solid[1], YellowId = solid[1], BlueId = solid[1], PrismaticId = solid[1], MetaId = austere },
						
					new GemmingTemplate() { Model = "Bear", Group = "Epic", //Max Mitigation
						RedId = delicate[2], YellowId = delicate[2], BlueId = delicate[2], PrismaticId = delicate[2], MetaId = austere },
					new GemmingTemplate() { Model = "Bear", Group = "Epic", //Mitigation Heavier 
						RedId = delicate[2], YellowId = stalwart[2], BlueId = shifting[2], PrismaticId = delicate[2], MetaId = austere },
					new GemmingTemplate() { Model = "Bear", Group = "Epic", //Mitigation Heavy
						RedId = delicate[2], YellowId = glinting[2], BlueId = shifting[2], PrismaticId = delicate[2], MetaId = austere },
					new GemmingTemplate() { Model = "Bear", Group = "Epic", //Survivbility Heavy
						RedId = shifting[2], YellowId = enduring[2], BlueId = solid[2], PrismaticId = solid[2], MetaId = austere },
					new GemmingTemplate() { Model = "Bear", Group = "Epic", //Max Survivbility
						RedId = solid[2], YellowId = solid[2], BlueId = solid[2], PrismaticId = solid[2], MetaId = austere },
						
					new GemmingTemplate() { Model = "Bear", Group = "Jeweler", //Max Mitigation
						RedId = delicate[3], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[3], MetaId = austere },
					new GemmingTemplate() { Model = "Bear", Group = "Jeweler", //Mitigation Heavy
						RedId = delicate[2], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[2], MetaId = austere },
					new GemmingTemplate() { Model = "Bear", Group = "Jeweler", //Survivbility Heavy
						RedId = solid[3], YellowId = solid[3], BlueId = solid[2], PrismaticId = solid[2], MetaId = austere },
					new GemmingTemplate() { Model = "Bear", Group = "Jeweler", //Max Survivbility
						RedId = solid[3], YellowId = solid[3], BlueId = solid[3], PrismaticId = solid[3], MetaId = austere },
				};
			}
		}

		private CalculationOptionsPanelBase _calculationOptionsPanel = null;
		/// <summary>
		/// Ppanel to be placed on the Options tab of the main form
		/// </summary>
		public override CalculationOptionsPanelBase CalculationOptionsPanel
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
					"Basic Stats:Crit Rating",
					"Basic Stats:Hit Rating",
					"Basic Stats:Expertise Rating",
					"Basic Stats:Haste Rating",
					"Basic Stats:Armor Penetration Rating",
					"Basic Stats:Dodge Rating",
					"Basic Stats:Defense Rating",
					"Basic Stats:Resilience",
					"Basic Stats:Nature Resist",
					"Basic Stats:Fire Resist",
					"Basic Stats:Frost Resist",
					"Basic Stats:Shadow Resist",
					"Basic Stats:Arcane Resist",
					"Mitigation Stats:Dodge",
					"Mitigation Stats:Miss",
					"Mitigation Stats:Mitigation",
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
					"Threat Stats:Swipe Rotation",
					"Threat Stats:Custom Rotation",
					"Threat Stats:Melee",
					"Threat Stats:Maul",
					"Threat Stats:Mangle",
					"Threat Stats:Swipe",
					"Threat Stats:Faerie Fire",
					"Threat Stats:Lacerate",
					"Threat Stats:Lacerate DoT Tick",
					"Threat Stats:Avoided Attacks",
					};
				return _characterDisplayCalculationLabels;
			}
		}

		private string[] _optimizableCalculationLabels = null;
		/// <summary>
		/// Labels of the stats available to the Optimizer 
		/// </summary>
		public override string[]  OptimizableCalculationLabels
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
					"Rotation DPS",
					"Rotation TPS"
					};
				return _customChartNames;
			}
		}

		private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
		/// <summary>
		/// Names and colors for the SubPoints that Rawr.Bear uses
		/// </summary>
		public override Dictionary<string, System.Drawing.Color> SubPointNameColors
		{
			get
			{
				if (_subPointNameColors == null)
				{
					_subPointNameColors = new Dictionary<string, System.Drawing.Color>();
					_subPointNameColors.Add("Mitigation", System.Drawing.Color.Red);
					_subPointNameColors.Add("Survival", System.Drawing.Color.Blue);
                    _subPointNameColors.Add("Threat", System.Drawing.Color.Green);
				}
				return _subPointNameColors;
			}
		}

		private List<Item.ItemType> _relevantItemTypes = null;
		/// <summary>
		/// ItemTypes that are relevant to Rawr.Bear
		/// </summary>
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

		/// <summary>
		/// The class that Rawr.Bear is designed for (Druid)
		/// </summary>
		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Druid; } }
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
			CalculationOptionsBear calcOpts = character.CalculationOptions as CalculationOptionsBear;
			int targetLevel = calcOpts.TargetLevel;
			int characterLevel = character.Level;
			Stats stats = GetCharacterStats(character, additionalItem);
			float levelDifference = (targetLevel - characterLevel) * 0.002f;
			
			CharacterCalculationsBear calculatedStats = new CharacterCalculationsBear();
			calculatedStats.BasicStats = stats;
			calculatedStats.TargetLevel = targetLevel;

			float hasteBonus = StatConversion.GetHasteFromRating(stats.HasteRating, Character.CharacterClass.Druid);//stats.HasteRating * 1.3f / 32.78998947f / 100f;
			float attackSpeed = ((2.5f) / (1f + hasteBonus)) / (1f + stats.PhysicalHaste);

			float hitBonus = StatConversion.GetPhysicalHitFromRating(stats.HitRating);//stats.HitRating / 32.78998947f / 100f;
			float expertiseBonus = (StatConversion.GetExpertiseFromRating(stats.ExpertiseRating) + stats.Expertise) * 0.0025f;//stats.ExpertiseRating / 32.78998947f / 100f + stats.Expertise * 0.0025f;
			float chanceDodge = Math.Max(0f, 0.065f + .005f * (targetLevel - 83) - expertiseBonus);
			float chanceParry = Math.Max(0f, 0.1375f - expertiseBonus); // Parry for lower levels?
			float chanceMiss = Math.Max(0f, 0.08f - hitBonus);
			if (targetLevel < 83) chanceMiss = Math.Max(0f, 0.05f + 0.005f * (targetLevel - 80f) - hitBonus);
			
			float chanceAvoided = chanceMiss + chanceDodge + chanceParry;

			if (stats.MongooseProc + stats.TerrorProc > 0)
			{
				//Add stats for Mongoose/Terror

				if (stats.TerrorProc > 0)
				{
					float terrorAgi = (1 - (float)Math.Pow(chanceAvoided, 3f)) * stats.TerrorProc * 2f / 3f * (1 + stats.BonusAgilityMultiplier);
					stats.Agility += terrorAgi;
					stats.Armor += terrorAgi * 2;
				}

				if (stats.MongooseProc > 0)
				{
					float whiteAttacksPerSecond = (1f - chanceAvoided) / attackSpeed;
					float yellowAttacksPerSecond = (1f - chanceAvoided) / 1.5f; //TODO: Calculate this
					float timeBetweenMongooseProcs = 24f / (whiteAttacksPerSecond + yellowAttacksPerSecond);
					float mongooseUptime = 15f / timeBetweenMongooseProcs;
					float mongooseAgi = 120f * mongooseUptime * (1 + stats.BonusAgilityMultiplier);
					stats.Agility += mongooseAgi;
					stats.Armor += mongooseAgi * 2;
					stats.PhysicalHaste *= 1f + (0.02f * mongooseUptime);
				}
			}
			float rawChanceCrit = StatConversion.GetPhysicalCritFromRating(stats.CritRating) + 
				StatConversion.GetPhysicalCritFromAgility(stats.Agility, Character.CharacterClass.Druid) + //(stats.CritRating / 45.90598679f + stats.Agility * 0.012f) / 100f +
				stats.PhysicalCrit - (0.006f * (targetLevel - character.Level) + (targetLevel == 83 ? 0.03f : 0f));
			float chanceCrit = rawChanceCrit * (1f - chanceAvoided);
			float chanceCritBleed = (character.DruidTalents.PrimalGore > 0 ? rawChanceCrit : 0f);
			attackSpeed = ((2.5f) / (1f + hasteBonus)) / (1f + stats.PhysicalHaste);
			
			float baseAgi = character.Race == Character.CharacterRace.NightElf ? 87 : 77;
			
			//Calculate avoidance, considering diminishing returns
			float defSkill = (float)Math.Floor(StatConversion.GetDefenseFromRating(stats.DefenseRating)); //stats.DefenseRating / 4.918498039f);
			float dodgeNonDR = stats.Dodge - levelDifference + StatConversion.GetDodgeFromAgility(baseAgi, Character.CharacterClass.Druid); //stats.Dodge * 100f - levelDifference + baseAgi * 0.024f; //TODO: Find correct Agi->Dodge ratio
			float missNonDR = stats.Miss - levelDifference;
			float dodgePreDR = StatConversion.GetDodgeFromAgility(stats.Agility - baseAgi, Character.CharacterClass.Druid) +
				StatConversion.GetDodgeFromRating(stats.DodgeRating) + defSkill * 0.0004f;
				//(stats.Agility + (stats.TerrorProc * 0.55f) - baseAgi) * 0.024f + (stats.DodgeRating / 39.34798813f) + (defSkill * 0.04f); //TODO: Find correct Agi->Dodge ratio
			float missPreDR = (defSkill * 0.0004f);
			float dodgePostDR = 0.01f / (1f / 116.890707f + 0.00972f / dodgePreDR);
			float missPostDR = 0.01f / (1f / 16f + 0.00972f / missPreDR);
			float dodgeTotal = dodgeNonDR + dodgePostDR;
			float missTotal = missNonDR + missPostDR;

			calculatedStats.Miss = missTotal;
			calculatedStats.Dodge = Math.Min(1f - calculatedStats.Miss, dodgeTotal);
			calculatedStats.ConstantDamageReduction = 1f - ((1f - Math.Min(0.75f, stats.Armor / (stats.Armor - 22167.5f + 467.5f * targetLevel))) * (1f + stats.DamageTakenMultiplier));
			calculatedStats.AvoidancePreDR = dodgeNonDR + dodgePreDR + missNonDR + missPreDR;
			calculatedStats.AvoidancePostDR = dodgeTotal + missTotal;
			calculatedStats.CritReduction = (defSkill * 0.0004f) + StatConversion.GetResilienceFromRating(stats.Resilience) + stats.CritChanceReduction; //(defSkill * 0.04f) + stats.Resilience / (2050f / 52f) + stats.CritChanceReduction * 100f;
			calculatedStats.CappedCritReduction = Math.Min(0.05f + levelDifference, calculatedStats.CritReduction);

			float targetHitChance = 1f - calculatedStats.AvoidancePostDR;
			float autoSpecialAttacksPerSecond = 1f / 1.5f + 1f / attackSpeed;
			float lacerateTicksPerSecond = (character.DruidTalents.PrimalGore > 0 ? 1f : 0f) / 3f;
			float totalAttacksPerSecond = autoSpecialAttacksPerSecond + lacerateTicksPerSecond;
			float averageSDAttackCritChance = (chanceCrit * (autoSpecialAttacksPerSecond / totalAttacksPerSecond) + chanceCritBleed * (lacerateTicksPerSecond / totalAttacksPerSecond));
			float playerAttackSpeed = 1f / totalAttacksPerSecond;
			float blockChance = 1f - targetHitChance * ((float)Math.Pow(1f - averageSDAttackCritChance, calcOpts.TargetAttackSpeed / playerAttackSpeed)) *
				1f / (1f - (1f - targetHitChance) * (float)Math.Pow(1f - averageSDAttackCritChance, calcOpts.TargetAttackSpeed / playerAttackSpeed));
			float blockValue = stats.AttackPower * 0.25f;
			float blockedPercent = Math.Min(1f, (blockValue * blockChance) / ((1f - calculatedStats.ConstantDamageReduction) * calcOpts.TargetDamage));
			calculatedStats.SavageDefenseChance = (float)Math.Round(blockChance, 5);
			calculatedStats.SavageDefenseValue = (float)Math.Floor(blockValue);
			calculatedStats.SavageDefensePercent = (float)Math.Round(blockedPercent, 5);

			//Out of 100 attacks, you'll take...
			float crits = Math.Min(Math.Max(0f, 1f - calculatedStats.AvoidancePostDR), (0.05f + levelDifference) - calculatedStats.CappedCritReduction);
			//float crushes = targetLevel == 73 ? Math.Max(0f, Math.Min(15f, 100f - (crits + calculatedStats.AvoidancePreDR)) - stats.CritChanceReduction) : 0f;
			float hits = Math.Max(0f, 1f - (crits + calculatedStats.AvoidancePostDR));
			//Apply armor and multipliers for each attack type...
			crits *= (1f - calculatedStats.ConstantDamageReduction) * 2f;
			//crushes *= (100f - calculatedStats.Mitigation) * .015f;
			hits *= (1f - calculatedStats.ConstantDamageReduction);
			calculatedStats.DamageTaken = (hits + crits) * (1f - blockedPercent);
			calculatedStats.TotalMitigation = 1f - calculatedStats.DamageTaken;

			calculatedStats.SurvivalPointsRaw = (stats.Health / (1f - calculatedStats.ConstantDamageReduction));
			double survivalCap = (double)calcOpts.SurvivalSoftCap / 1000d;
			double survivalRaw = calculatedStats.SurvivalPointsRaw / 1000f;

			//Implement Survival Soft Cap
			if (survivalRaw <= survivalCap)
				calculatedStats.SurvivalPoints = 1000f * (float)survivalRaw;
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

			calculatedStats.MitigationPoints = (17000f / calculatedStats.DamageTaken); // / (buffs.ShadowEmbrace ? 0.95f : 1f);

            #region WotLKResistance
            // Call new resistance formula and apply talent damage reduction
            // As for other survival, only use guaranteed reduction (MinimumResist), no luck
            calculatedStats.NatureSurvivalPoints = (float)(stats.Health / ((1f - StatConversion.GetMinimumResistance(80, targetLevel, stats.NatureResistance, 0)) * (1f - 0.04 * character.DruidTalents.ProtectorOfThePack))); 
            calculatedStats.FrostSurvivalPoints  = (float)(stats.Health / ((1f - StatConversion.GetMinimumResistance(80, targetLevel, stats.FrostResistance,  0)) * (1f - 0.04 * character.DruidTalents.ProtectorOfThePack))); 
            calculatedStats.FireSurvivalPoints   = (float)(stats.Health / ((1f - StatConversion.GetMinimumResistance(80, targetLevel, stats.FireResistance,   0)) * (1f - 0.04 * character.DruidTalents.ProtectorOfThePack))); 
            calculatedStats.ShadowSurvivalPoints = (float)(stats.Health / ((1f - StatConversion.GetMinimumResistance(80, targetLevel, stats.ShadowResistance, 0)) * (1f - 0.04 * character.DruidTalents.ProtectorOfThePack))); 
            calculatedStats.ArcaneSurvivalPoints = (float)(stats.Health / ((1f - StatConversion.GetMinimumResistance(80, targetLevel, stats.ArcaneResistance, 0)) * (1f - 0.04 * character.DruidTalents.ProtectorOfThePack)));
            #endregion

            #region OldResistance
            //float cappedResist = targetLevel * 5;

            //calculatedStats.NatureSurvivalPoints = (float)(stats.Health / ((1f - (System.Math.Min(cappedResist, stats.NatureResistance) / cappedResist) * .75)));
            //calculatedStats.FrostSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.FrostResistance) / cappedResist) * .75)));
            //calculatedStats.FireSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.FireResistance) / cappedResist) * .75)));
            //calculatedStats.ShadowSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.ShadowResistance) / cappedResist) * .75)));
            //calculatedStats.ArcaneSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.ArcaneResistance) / cappedResist) * .75)));
            #endregion

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
        private void CalculateThreat(Stats stats, int targetLevel, CharacterCalculationsBear calculatedStats, Character character)
        {
			CalculationOptionsBear calcOpts = character.CalculationOptions as CalculationOptionsBear;
			DruidTalents talents = character.DruidTalents;

			//Establish base multipliers and chances
			int targetArmor = calcOpts.TargetArmor;
			//float armorPenetrationPercent = stats.ArmorPenetration + stats.ArmorPenetrationRating / 1539.529991f;
			//float reducedArmor = targetArmor * (1f - armorPenetrationPercent);
			//float modArmor = 1f - (reducedArmor / ((467.5f * character.Level) + reducedArmor - 22167.5f));
			float modArmor = 1f - ArmorCalculations.GetDamageReduction(character.Level, targetArmor,
				stats.ArmorPenetration, stats.ArmorPenetrationRating);

			float critMultiplier = 2f * (1 + stats.BonusCritMultiplier);
			float spellCritMultiplier = 1.5f * (1 + stats.BonusCritMultiplier);

			float hasteBonus = StatConversion.GetPhysicalHasteFromRating(stats.HasteRating, Character.CharacterClass.Druid);//stats.HasteRating * 1.3f / 32.78998947f / 100f;
            float attackSpeed = (2.5f) / (1f + hasteBonus);
			attackSpeed = attackSpeed / (1f + stats.PhysicalHaste);

			float hitBonus = StatConversion.GetPhysicalHitFromRating(stats.HitRating);//stats.HitRating / 32.78998947f / 100f;
			float expertiseBonus = (StatConversion.GetExpertiseFromRating(stats.ExpertiseRating) + stats.Expertise) * 0.0025f; //stats.ExpertiseRating * 1.25f / 32.78998947f / 100f + stats.Expertise * 0.0025f;

            float chanceDodge = Math.Max(0f, 0.065f + .005f * (targetLevel - 83) - expertiseBonus);
            float chanceParry = Math.Max(0f, 0.1375f - expertiseBonus);
            float chanceMiss = Math.Max(0f, 0.08f - hitBonus);
			if (targetLevel < 83)
			{
				chanceParry = Math.Max(0f, 0.05f + 0.005f * (targetLevel - 80f) - expertiseBonus);
				chanceMiss = Math.Max(0f, 0.05f + 0.005f * (targetLevel - 80f) - hitBonus);
			}
            float chanceGlance = 0.2335774f;
            float glanceMultiplier = .7f;
            float chanceAvoided = chanceMiss + chanceDodge + chanceParry;

			float rawChanceCrit = StatConversion.GetPhysicalCritFromRating(stats.CritRating) +
				StatConversion.GetPhysicalCritFromAgility(stats.Agility, Character.CharacterClass.Druid) + //(stats.CritRating / 45.90598679f + stats.Agility * 0.012f) / 100f +
				stats.PhysicalCrit - (0.006f * (targetLevel - character.Level) + (targetLevel == 83 ? 0.03f : 0f));
			float chanceCrit = rawChanceCrit * (1f - chanceAvoided);
			float chanceCritBleed = character.DruidTalents.PrimalGore > 0 ? chanceCrit : 0f;
			
			calculatedStats.DodgedAttacks = chanceDodge;
            calculatedStats.ParriedAttacks = chanceParry;
            calculatedStats.MissedAttacks = chanceMiss;

			float baseDamage = 137f + (stats.AttackPower / 14f) * 2.5f;
			float bearThreatMultiplier = 29f / 14f;

			//Calculate the raw damage of each ability
			float meleeDamageRaw = baseDamage * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * modArmor;
			float maulDamageRaw = (baseDamage + 578) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusMaulDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier) * modArmor;
			float mangleDamageRaw = (baseDamage * 1.15f + 299) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusMangleDamageMultiplier) * modArmor;
			float swipeDamageRaw = (stats.AttackPower * 0.063f + 108f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusSwipeDamageMultiplier) * modArmor;
			float faerieFireDamageRaw = (stats.AttackPower * 0.15f + 1f) * (1f + stats.BonusDamageMultiplier);
			float lacerateDamageRaw = (stats.AttackPower * 0.01f + 88f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * modArmor * (1f + stats.BonusLacerateDamageMultiplier);
			float lacerateDamageDotRaw = (stats.AttackPower * 0.01f + 64f) * 5f /*stack size*/ * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier) * (1f + stats.BonusLacerateDamageMultiplier);

			//Calculate the average damage of each ability, including crits
			float meleeDamageAverage = (chanceCrit * (meleeDamageRaw * critMultiplier)) + //Damage from crits
							(chanceGlance * (meleeDamageRaw * glanceMultiplier)) + //Damage from glances
							((1f - chanceCrit - chanceAvoided - chanceGlance) * (meleeDamageRaw)); //Damage from hits
			float maulDamageAverage = (chanceCrit * (maulDamageRaw * critMultiplier)) + ((1f - chanceCrit - chanceAvoided) * (maulDamageRaw));
			float mangleDamageAverage = (chanceCrit * (mangleDamageRaw * critMultiplier)) + ((1f - chanceCrit - chanceAvoided) * (mangleDamageRaw));
			float swipeDamageAverage = (chanceCrit * (swipeDamageRaw * critMultiplier)) + ((1f - chanceCrit - chanceAvoided) * (swipeDamageRaw));
			float faerieFireDamageAverage = (0.25f * (faerieFireDamageRaw * spellCritMultiplier)) + (0.65f * (faerieFireDamageRaw)); //TODO: Assumes 25% spell crit and 10% spell miss
			float lacerateDamageAverage = (chanceCrit * (lacerateDamageRaw * critMultiplier)) + ((1f - chanceCrit - chanceAvoided) * (lacerateDamageRaw));
			float lacerateDamageDotAverage = (chanceCritBleed * (lacerateDamageDotRaw * critMultiplier)) + ((1f - chanceCritBleed) * (lacerateDamageDotRaw));

			//Calculate the raw threat of each attack
			float meleeThreatRaw = bearThreatMultiplier * meleeDamageRaw;
			float maulThreatRaw = bearThreatMultiplier * (maulDamageRaw + 424f / 1f); //NOTE! This assumes 1 target. If Maul hits 2 targets, replace 1 with 2.
			float mangleThreatRaw = bearThreatMultiplier * mangleDamageRaw * (1f + stats.BonusMangleBearThreat);
			float swipeThreatRaw = bearThreatMultiplier * swipeDamageRaw * 1.5f;
			float faerieFireThreatRaw = bearThreatMultiplier * (faerieFireDamageRaw + 632f);
			float lacerateThreatRaw = bearThreatMultiplier * (lacerateDamageRaw + 1031f) / 2f;
			float lacerateDotThreatRaw = bearThreatMultiplier * lacerateDamageDotRaw / 2f;
			
			//Calculate the average threat of each attack, including crits
			float meleeThreatAverage = bearThreatMultiplier * meleeDamageAverage;
			float maulThreatAverage = bearThreatMultiplier * (maulDamageAverage + (424f * (1 - chanceAvoided)) / 1f); //NOTE! This assumes 1 target. If Maul hits 2 targets, replace 1 with 2.
			float mangleThreatAverage = bearThreatMultiplier * mangleDamageAverage * (1 + stats.BonusMangleBearThreat);
			float swipeThreatAverage = bearThreatMultiplier * swipeDamageAverage * 1.5f;
			float faerieFireThreatAverage = bearThreatMultiplier * (faerieFireDamageAverage + (632f * (.9f))); //TODO: Assumes 10% spell miss rate
			float lacerateThreatAverage = bearThreatMultiplier * (lacerateDamageAverage + (1031f * (1 - chanceAvoided))) / 2f;
			float lacerateDotThreatAverage = bearThreatMultiplier * lacerateDamageDotAverage / 2f;
			
			//Calculate effective rage costs for the possible outcomes of each ability, and the average
			float meleeRageHit = meleeDamageRaw / 120f + 35f / 9f;
			float meleeRageGlance = (meleeDamageRaw * glanceMultiplier) / 120f + 35f / 9f;
			float meleeRageCrit = (meleeDamageRaw * critMultiplier) / 120f + 70f / 9f;
			float meleeRageDodgeParry = 9.5f;
			float meleeRageAverage = (chanceCrit * meleeRageCrit) + //Rage from crits
							(chanceGlance * meleeRageGlance) + //Rage from glances
							((chanceDodge + chanceParry) * meleeRageDodgeParry) + //Rage from dodges/parries
							((1f - chanceCrit - chanceAvoided - chanceGlance) * meleeRageHit); //Rage from hits

			float maulRageHit = 15f - talents.Ferocity + meleeRageAverage;
			float maulRageCrit = 15f - talents.Ferocity - (2.5f * talents.PrimalFury) + meleeRageAverage;
			float maulRageAvoided = ((15f - talents.Ferocity) * 0.2f) + meleeRageAverage;
			float maulRageAverage = (chanceCrit * maulRageCrit) + ((1f - chanceCrit - chanceAvoided) * maulRageHit) + (chanceAvoided * maulRageAvoided);

			float mangleRageHit = 20f - talents.Ferocity;
			float mangleRageCrit = 20f - talents.Ferocity - (2.5f * talents.PrimalFury);
			float mangleRageDodgeParry = (20f - talents.Ferocity) * 0.2f;
			float mangleRageAverage = (chanceCrit * mangleRageCrit) + ((1f - chanceCrit - chanceDodge - chanceParry) * mangleRageHit) + ((chanceDodge + chanceParry) + mangleRageDodgeParry);
			
			float swipeRageHit = 20f - talents.Ferocity;
			float swipeRageCrit = 20f - talents.Ferocity - (2.5f * talents.PrimalFury);
			float swipeRageDodgeParry = (20f - talents.Ferocity) * 0.2f;
			float swipeRageAverage = (chanceCrit * swipeRageCrit) + ((1f - chanceCrit - chanceDodge - chanceParry) * swipeRageHit) + ((chanceDodge + chanceParry) + swipeRageDodgeParry);
			
			float lacerateRageHit = 15f - talents.ShreddingAttacks;
			float lacerateRageCrit = 15f - talents.ShreddingAttacks - (2.5f * talents.PrimalFury);
			float lacerateRageDodgeParry = (15f - talents.ShreddingAttacks) * 0.2f;
			float lacerateRageAverage = (chanceCrit * lacerateRageCrit) + ((1f - chanceCrit - chanceDodge - chanceParry) * lacerateRageHit) + ((chanceDodge + chanceParry) + lacerateRageDodgeParry);
			
			//Calculate the threat per rage of each ability, on average
			float maulTPR = maulThreatAverage / maulRageAverage;
			float maulDPR = maulDamageAverage / maulRageAverage;
			float mangleTPR = mangleThreatAverage / mangleRageAverage;
			float mangleDPR = mangleDamageAverage / mangleRageAverage;
			float swipeTPR = swipeThreatAverage / swipeRageAverage;
			float swipeDPR = swipeDamageAverage / swipeRageAverage;
			float lacerateTPR = lacerateThreatAverage / lacerateRageAverage;
			float lacerateDPR = lacerateDamageAverage / lacerateRageAverage;


			//Use the BearRotationCalculator to model the potential rotations, and get their results
			BearRotationCalculator rotationCalculator = new BearRotationCalculator(meleeDamageAverage, maulDamageAverage, mangleDamageAverage, swipeDamageAverage,
				faerieFireDamageAverage, lacerateDamageAverage, lacerateDamageDotAverage, meleeThreatAverage, maulThreatAverage, mangleThreatAverage, swipeThreatAverage,
				faerieFireThreatAverage, lacerateThreatAverage, lacerateDotThreatAverage, 6f - stats.MangleCooldownReduction, attackSpeed);

			BearRotationCalculator.BearRotationCalculation rotationCalculationDPS, rotationCalculationTPS;
			rotationCalculationDPS = rotationCalculationTPS = new BearRotationCalculator.BearRotationCalculation();

			//Loop through the potential rotations...
			for (int useMaul = 0; useMaul < 3; useMaul++)
				for (int useMangle = 0; useMangle < 2; useMangle++)
					for (int useSwipe = 0; useSwipe < 2; useSwipe++)
						for (int useFaerieFire = 0; useFaerieFire < 2; useFaerieFire++)
							for (int useLacerate = 0; useLacerate < 2; useLacerate++)
							{
								//...and feed them to the BearRotationCalculator
								bool?[] useMaulValues = new bool?[] {null, false, true};
								BearRotationCalculator.BearRotationCalculation rotationCalculation = 
									rotationCalculator.GetRotationCalculations(useMaulValues[useMaul],
									useMangle == 1, useSwipe == 1, useFaerieFire == 1, useLacerate == 1);
								//Keep track of the best rotation for both DPS and TPS
								if (rotationCalculation.DPS > rotationCalculationDPS.DPS)
									rotationCalculationDPS = rotationCalculation;
								if (rotationCalculation.TPS > rotationCalculationTPS.TPS)
									rotationCalculationTPS = rotationCalculation;
							}

			//Fill the results into the CharacterCalculationsBear object
			calculatedStats.ThreatPoints = rotationCalculationTPS.TPS * calcOpts.ThreatScale;
			calculatedStats.HighestDPSRotation = rotationCalculationDPS;
			calculatedStats.HighestTPSRotation = rotationCalculationTPS;
			calculatedStats.SwipeRotation = rotationCalculator.GetRotationCalculations(null, false, true, false, false);
			calculatedStats.CustomRotation = rotationCalculator.GetRotationCalculations(calcOpts.CustomUseMaul,
				calcOpts.CustomUseMangle, calcOpts.CustomUseSwipe, calcOpts.CustomUseFaerieFire, calcOpts.CustomUseLacerate);

			calculatedStats.AttackSpeed = attackSpeed;
			calculatedStats.MangleCooldown = 6f - stats.MangleCooldownReduction;

			calculatedStats.MeleeDamageRaw = (float)Math.Round(meleeDamageRaw);
			calculatedStats.MeleeDamageAverage = (float)Math.Round(meleeDamageAverage);
			calculatedStats.MeleeThreatRaw = (float)Math.Round(meleeThreatRaw);
			calculatedStats.MeleeThreatAverage = (float)Math.Round(meleeThreatAverage);

			calculatedStats.MaulDamageRaw = (float)Math.Round(maulDamageRaw);
			calculatedStats.MaulDamageAverage = (float)Math.Round(maulDamageAverage);
			calculatedStats.MaulThreatRaw = (float)Math.Round(maulThreatRaw);
			calculatedStats.MaulThreatAverage = (float)Math.Round(maulThreatAverage);

			calculatedStats.MangleDamageRaw = (float)Math.Round(mangleDamageRaw);
			calculatedStats.MangleDamageAverage = (float)Math.Round(mangleDamageAverage);
			calculatedStats.MangleThreatRaw = (float)Math.Round(mangleThreatRaw);
			calculatedStats.MangleThreatAverage = (float)Math.Round(mangleThreatAverage);

			calculatedStats.SwipeDamageRaw = (float)Math.Round(swipeDamageRaw);
			calculatedStats.SwipeDamageAverage = (float)Math.Round(swipeDamageAverage);
			calculatedStats.SwipeThreatRaw = (float)Math.Round(swipeThreatRaw);
			calculatedStats.SwipeThreatAverage = (float)Math.Round(swipeThreatAverage);

			calculatedStats.FaerieFireDamageRaw = (float)Math.Round(faerieFireDamageRaw);
			calculatedStats.FaerieFireDamageAverage = (float)Math.Round(faerieFireDamageAverage);
			calculatedStats.FaerieFireThreatRaw = (float)Math.Round(faerieFireThreatRaw);
			calculatedStats.FaerieFireThreatAverage = (float)Math.Round(faerieFireThreatAverage);

			calculatedStats.LacerateDamageRaw = (float)Math.Round(lacerateDamageRaw);
			calculatedStats.LacerateDamageAverage = (float)Math.Round(lacerateDamageAverage);
			calculatedStats.LacerateThreatRaw = (float)Math.Round(lacerateThreatRaw);
			calculatedStats.LacerateThreatAverage = (float)Math.Round(lacerateThreatAverage);

			calculatedStats.LacerateDotDamageRaw = (float)Math.Round(lacerateDamageDotRaw);
			calculatedStats.LacerateDotDamageAverage = (float)Math.Round(lacerateDamageDotAverage);
			calculatedStats.LacerateDotThreatRaw = (float)Math.Round(lacerateDotThreatRaw);
			calculatedStats.LacerateDotThreatAverage = (float)Math.Round(lacerateDotThreatAverage);

			calculatedStats.MaulTPR = maulTPR;
			calculatedStats.MaulDPR = maulDPR;
			calculatedStats.MangleTPR = mangleTPR;
			calculatedStats.MangleDPR = mangleDPR;
			calculatedStats.SwipeTPR = swipeTPR;
			calculatedStats.SwipeDPR = swipeDPR;
			calculatedStats.LacerateTPR = lacerateTPR;
			calculatedStats.LacerateDPR = lacerateDPR;
        }

		/// <summary>
		/// Gets the total Stats of the Character
		/// </summary>
		/// <param name="character">The Character to get the total Stats of</param>
		/// <param name="additionalItem">An additional item to grant the Character the stats of (as if it were worn)</param>
		/// <returns>The total stats for the Character</returns>
		public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
			Stats statsRace = character.Race == Character.CharacterRace.NightElf ?
				new Stats() {
					Health = 7417f, 
                    Strength = 86f, 
					Agility = 87f,
					Stamina = 97f,
					AttackPower = 220f,
                    NatureResistance = 10f,
                    Dodge = 0.04951f,
					Miss = 0.07f,
					PhysicalCrit = 0.05f,
					//BonusCritMultiplier = 0.1f,
					//CritRating = 264.0768f, 
					//BonusAttackPowerMultiplier = 0.0f,
					//BonusAgilityMultiplier = 0.03f,
					//BonusStrengthMultiplier = 0.03f,
					BonusStaminaMultiplier = 0.25f
                } :
				new Stats() {
					Health = 7599f,
					Strength = 95f,
					Agility = 77f,
					Stamina = 100f,
					AttackPower = 220f,
                    NatureResistance = 10f,
					Dodge = 0.04951f,
					Miss = 0.05f,
					PhysicalCrit = 0.05f,
					//BonusCritMultiplier = 0.1f,
					//CritRating = 264.0768f, 
					//BonusAttackPowerMultiplier = 0.0f,
					//BonusAgilityMultiplier = 0.03f,
					//BonusStrengthMultiplier = 0.03f,
					BonusStaminaMultiplier = 0.25f
                };

			

			/* TODO:
			 * Threat-only:
			 * Ferocity (5: -5rage cost to Maul, Swipe, Mangle)
			 --* Feral Instinct (3: 30% damage buff to Swipe)
			 --* Savage Fury (2: 20% damage buff to Mangle, Maul)
			 --* Sharpened Claws (3: 6% crit)
			 * Shredding Attacks (2: -2rage cost to Lacerate)
			 --* Predatory Strikes (3:  105 ap, plus 20% of weapon ap)
			 --* Primal Fury (2: 5rage on crits)
			 --* Primal Precision (2: 10 expertise)
			 --* Leader of the Pack (1: 5% crit)
			 --* Predatory Instincts (3: 15% damage buff to crits)
			 --* King of the Jungle (3: 15% damage buff during Enrage)
			 -* Mangle (1: Mangle)
			 --* Improved Mangle (3: -1.5sec cooldown to Mangle)
			 --* Rend and Tear (5: 20% damage buff to Maul)
			 * Berserk (1: Berserk)
			 --* Naturalist (5: 10% damage)
			 * Omen of Clarity (1: Omen of Clarity)
			 --* Master Shapeshifter (2: 4% damage)
			 * 
			 * Mitigation/Survival:
			 --* Thick Hide (3: 10% armor buff)
			 --* Feral Swiftness (2: 4% dodge)
			 -* Natural Reaction (3: 6% dodge, 3 rage on dodges)
			 --* Heart of the Wild (5: 20% sta)
			 --* Survival of the Fittest (3: 6% stats, 6% anticrit)
			 -* Mother Bear (3: 6% ap, 12% less damage taken)
			 * 
			*/

			DruidTalents talents = character.DruidTalents;
			
			Stats statsItems = GetItemStats(character, additionalItem);
			//Stats statsEnchants = GetEnchantsStats(character);
			Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
			Stats statsTalents = new Stats()
			{
				PhysicalCrit = 0.02f * talents.SharpenedClaws + (character.ActiveBuffsContains("Leader of the Pack") ?
					0f : 0.05f * talents.LeaderOfThePack),
				Dodge = 0.02f * talents.FeralSwiftness + 0.02f * talents.NaturalReaction,
				BonusStaminaMultiplier = (1f + 0.02f * talents.HeartOfTheWild) * (1f + 0.02f * talents.SurvivalOfTheFittest) * (1f + 0.01f * talents.ImprovedMarkOfTheWild) - 1f,
				BonusAgilityMultiplier = (1f + 0.02f * talents.SurvivalOfTheFittest) * (1f + 0.01f * talents.ImprovedMarkOfTheWild) - 1f,
				BonusStrengthMultiplier = (1f + 0.02f * talents.SurvivalOfTheFittest) * (1f + 0.01f * talents.ImprovedMarkOfTheWild) - 1f,
				CritChanceReduction = 0.02f * talents.SurvivalOfTheFittest,
				BonusAttackPowerMultiplier = 0.02f * talents.ProtectorOfThePack,
				BonusPhysicalDamageMultiplier = (1f + 0.02f * talents.Naturalist) * (1f + 0.02f * talents.MasterShapeshifter) - 1,
				BonusMangleDamageMultiplier = 0.1f * talents.SavageFury,
				BonusMaulDamageMultiplier = (1f + 0.1f * talents.SavageFury) * (1f + 0.04f * talents.RendAndTear) - 1f,
				BonusEnrageDamageMultiplier = 0.05f * talents.KingOfTheJungle,
				MangleCooldownReduction = (0.5f * talents.ImprovedMangle),
				BonusRageOnCrit = (2.5f * talents.PrimalFury),
				Expertise = 5f * talents.PrimalPrecision,
				AttackPower = (character.Level / 2f) * talents.PredatoryStrikes,
				BonusSwipeDamageMultiplier = 0.1f * talents.FeralInstinct,
                DamageTakenMultiplier = -0.04f * talents.ProtectorOfThePack,
				BonusBleedDamageMultiplier = (character.ActiveBuffsContains("Mangle") ? 0f : 0.3f * talents.Mangle),
				BaseArmorMultiplier = 4.7f * (1f + 0.1f * talents.ThickHide / 3f) * (1f + 0.11f * talents.SurvivalOfTheFittest) - 1f,
			};
			
			Stats statsTotal = statsRace + statsItems + statsBuffs + statsTalents;

            // Inserted by Trolando
            if (statsTotal.GreatnessProc > 0)
            {
                float expectedAgi = (float)Math.Floor(statsTotal.Agility * (1 + statsTotal.BonusAgilityMultiplier));
                float expectedStr = (float)Math.Floor(statsTotal.Strength * (1 + statsTotal.BonusStrengthMultiplier));
                // Highest stat
                if (expectedAgi > expectedStr)
                {
                    statsTotal.Agility += statsTotal.GreatnessProc * 15f / 48f;
                }
                else
                {
                    statsTotal.Strength += statsTotal.GreatnessProc * 15f / 48f;
                }
            }

            Stats statsWeapon = character.MainHand == null ? new Stats() : character.MainHand.GetTotalStats(character).Clone();
			statsWeapon.Strength *= (1 + statsTotal.BonusStrengthMultiplier);
			statsWeapon.AttackPower += statsWeapon.Strength * 2;
			if (character.MainHand != null)
			{
				float fap = (character.MainHand.Item.DPS - 54.8f) * 14f; //TODO Find a more accurate number for this?
				statsTotal.AttackPower += fap;
				statsWeapon.AttackPower += fap;
			}

			statsTotal.Stamina *= (1f + statsTotal.BonusStaminaMultiplier);
			statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina);
			statsTotal.Strength *= (1f + statsTotal.BonusStrengthMultiplier);
			statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1f + statsTotal.BonusAgilityMultiplier));
			statsTotal.AttackPower += (float)Math.Floor(statsTotal.Strength) * 2f;
			statsTotal.AttackPower += statsWeapon.AttackPower * 0.2f * (talents.PredatoryStrikes / 3f);
			statsTotal.AttackPower = (float)Math.Floor(statsTotal.AttackPower * (1 + statsTotal.BonusAttackPowerMultiplier));
			statsTotal.Health += ((statsTotal.Stamina - 20) * 10f) + 20;
            statsTotal.Health *= (1f + statsTotal.BonusHealthMultiplier);
			statsTotal.Armor *= 1f + statsTotal.BaseArmorMultiplier;
			statsTotal.Armor += 2f * (float)Math.Floor(statsTotal.Agility) + statsTotal.BonusArmor;
			statsTotal.Armor = (float)Math.Floor(statsTotal.Armor * (1f + statsTotal.BonusArmorMultiplier));
			statsTotal.NatureResistance += statsTotal.NatureResistanceBuff + statsTotal.AllResist;
			statsTotal.FireResistance += statsTotal.FireResistanceBuff + statsTotal.AllResist;
			statsTotal.FrostResistance += statsTotal.FrostResistanceBuff + statsTotal.AllResist;
			statsTotal.ShadowResistance += statsTotal.ShadowResistanceBuff + statsTotal.AllResist;
			statsTotal.ArcaneResistance += statsTotal.ArcaneResistanceBuff + statsTotal.AllResist;
            // Haste trinket (Meteorite Whetstone)
            statsTotal.HasteRating += statsTotal.HasteRatingOnPhysicalAttack * 10f / 45f;

			return statsTotal;
			/*
			statsItems.Agility += statsEnchants.Agility;
			statsItems.DefenseRating += statsEnchants.DefenseRating;
			statsItems.DodgeRating += statsEnchants.DodgeRating;
			statsItems.Resilience += statsEnchants.Resilience;
			statsItems.Stamina += statsEnchants.Stamina;

			statsBuffs.Health += statsEnchants.Health;
			statsBuffs.Armor += statsEnchants.Armor;

			float agiBase = (float)Math.Floor(statsRace.Agility * (1+ statsRace.BonusAgilityMultiplier));
            float agiBonus = (float) Math.Floor((statsItems.Agility + statsBuffs.Agility) * (1 + statsRace.BonusAgilityMultiplier));
            
			float strBase = (float) Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float strBonus = (float) Math.Floor(statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier));
            
			float staBase = (float) Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier) * 1.25f);
            float staBonus = (statsItems.Stamina + statsBuffs.Stamina) * (1 + statsRace.BonusStaminaMultiplier) * 1.25f;
            float staHotW = (statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier) * 1.25f + staBonus) * 0.2f;
			staBonus = (float)Math.Round(Math.Floor(staBonus) + staHotW);

			Stats statsTotal = new Stats();
			statsTotal.Stamina = staBase + (float)Math.Round((staBase * statsBuffs.BonusStaminaMultiplier) + staBonus * (1 + statsBuffs.BonusStaminaMultiplier));
			statsTotal.DefenseRating = statsRace.DefenseRating + statsItems.DefenseRating + statsBuffs.DefenseRating;
			statsTotal.DodgeRating = statsRace.DodgeRating + statsItems.DodgeRating + statsBuffs.DodgeRating;
			statsTotal.Resilience = statsRace.Resilience + statsItems.Resilience + statsBuffs.Resilience;
			statsTotal.Health = (float)Math.Round(((statsRace.Health + statsItems.Health + statsBuffs.Health + (statsTotal.Stamina * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
			statsTotal.Miss = statsRace.Miss + statsItems.Miss + statsBuffs.Miss;
			statsTotal.CrushChanceReduction = statsBuffs.CrushChanceReduction;
            statsTotal.NatureResistance = statsEnchants.NatureResistance + statsRace.NatureResistance + statsItems.NatureResistance + statsBuffs.NatureResistance +
				statsEnchants.NatureResistanceBuff + statsRace.NatureResistanceBuff + statsItems.NatureResistanceBuff + statsBuffs.NatureResistanceBuff;
			statsTotal.FireResistance = statsEnchants.FireResistance + statsRace.FireResistance + statsItems.FireResistance + statsBuffs.FireResistance +
				statsEnchants.FireResistanceBuff + statsRace.FireResistanceBuff + statsItems.FireResistanceBuff + statsBuffs.FireResistanceBuff;
			statsTotal.FrostResistance = statsEnchants.FrostResistance + statsRace.FrostResistance + statsItems.FrostResistance + statsBuffs.FrostResistance +
				statsEnchants.FrostResistanceBuff + statsRace.FrostResistanceBuff + statsItems.FrostResistanceBuff + statsBuffs.FrostResistanceBuff;
			statsTotal.ShadowResistance = statsEnchants.ShadowResistance + statsRace.ShadowResistance + statsItems.ShadowResistance + statsBuffs.ShadowResistance +
				statsEnchants.ShadowResistanceBuff + statsRace.ShadowResistanceBuff + statsItems.ShadowResistanceBuff + statsBuffs.ShadowResistanceBuff;
			statsTotal.ArcaneResistance = statsEnchants.ArcaneResistance + statsRace.ArcaneResistance + statsItems.ArcaneResistance + statsBuffs.ArcaneResistance +
				statsEnchants.ArcaneResistanceBuff + statsRace.ArcaneResistanceBuff + statsItems.ArcaneResistanceBuff + statsBuffs.ArcaneResistanceBuff;
            statsTotal.AllResist = statsEnchants.AllResist + statsRace.AllResist + statsItems.AllResist + statsBuffs.AllResist;


			

            statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier)) - 1;
            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;
            statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)) - 1;
            statsTotal.Agility = agiBase + (float)Math.Floor((agiBase * statsBuffs.BonusAgilityMultiplier) + agiBonus * (1 + statsBuffs.BonusAgilityMultiplier));
            statsTotal.Strength = strBase + (float)Math.Floor((strBase * statsBuffs.BonusStrengthMultiplier) + strBonus * (1 + statsBuffs.BonusStrengthMultiplier));

 
            statsTotal.ArmorPenetration = statsRace.ArmorPenetration + statsGearEnchantsBuffs.ArmorPenetration;
            statsTotal.AttackPower = (float) Math.Floor((statsRace.AttackPower + statsGearEnchantsBuffs.AttackPower  + (statsTotal.Strength * 2)) * (1f + statsTotal.BonusAttackPowerMultiplier));
            statsTotal.BonusCritMultiplier = ((1 + statsRace.BonusCritMultiplier) * (1 + statsGearEnchantsBuffs.BonusCritMultiplier)) - 1;
            statsTotal.CritRating = statsRace.CritRating + statsGearEnchantsBuffs.CritRating;
            statsTotal.ExpertiseRating = statsRace.ExpertiseRating + statsGearEnchantsBuffs.ExpertiseRating;
            statsTotal.HasteRating = statsRace.HasteRating + statsGearEnchantsBuffs.HasteRating;
            statsTotal.HitRating = statsRace.HitRating + statsGearEnchantsBuffs.HitRating;
            statsTotal.WeaponDamage = statsRace.WeaponDamage + statsGearEnchantsBuffs.WeaponDamage;
            statsTotal.ExposeWeakness = statsRace.ExposeWeakness + statsGearEnchantsBuffs.ExposeWeakness;
            statsTotal.Bloodlust = statsRace.Bloodlust + statsGearEnchantsBuffs.Bloodlust;

            statsTotal.Armor = (float) Math.Round(((statsItems.Armor * 5.5f) + statsRace.Armor + statsBuffs.Armor + (statsTotal.Agility * 2f)) * (1 + statsBuffs.BonusArmorMultiplier));

            statsTotal.TerrorProc = statsGearEnchantsBuffs.TerrorProc;
            statsTotal.BonusSwipeDamageMultiplier = statsGearEnchantsBuffs.BonusSwipeDamageMultiplier;
            statsTotal.BonusLacerateDamage = statsGearEnchantsBuffs.BonusLacerateDamage;
            statsTotal.BonusMangleBearDamage = statsGearEnchantsBuffs.BonusMangleBearDamage;
            statsTotal.BonusMangleBearThreat = statsGearEnchantsBuffs.BonusMangleBearThreat;
            statsTotal.BonusPhysicalDamageMultiplier = statsGearEnchantsBuffs.BonusPhysicalDamageMultiplier;
            statsTotal.BonusSwipeDamageMultiplier = statsGearEnchantsBuffs.BonusSwipeDamageMultiplier;
            statsTotal.BloodlustProc = statsGearEnchantsBuffs.BloodlustProc;
            
			return statsTotal;*/
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
						float hits = Math.Max(1f - (Math.Max(0f, crits) + /*Math.Max(crushes, 0)*/ + (currentCalculationsBear.AvoidancePreDR)), 0f);
						
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

				case "Rotation DPS":
					CharacterCalculationsBear calcsDPS = GetCharacterCalculations(character) as CharacterCalculationsBear;
					List<ComparisonCalculationBase> comparisonsDPS = new List<ComparisonCalculationBase>();

					BearRotationCalculator rotationCalculatorDPS = new BearRotationCalculator(calcsDPS.MeleeDamageAverage,
						calcsDPS.MaulDamageAverage, calcsDPS.MangleDamageAverage, calcsDPS.SwipeDamageAverage,
						calcsDPS.FaerieFireDamageAverage, calcsDPS.LacerateDamageAverage, calcsDPS.LacerateDotDamageAverage, calcsDPS.MeleeThreatAverage,
						calcsDPS.MaulThreatAverage, calcsDPS.MangleThreatAverage, calcsDPS.SwipeThreatAverage,
						calcsDPS.FaerieFireThreatAverage, calcsDPS.LacerateThreatAverage, calcsDPS.LacerateDotThreatAverage,
						6f - calcsDPS.BasicStats.MangleCooldownReduction, calcsDPS.AttackSpeed);

					for (int useMaul = 0; useMaul < 3; useMaul++)
						for (int useMangle = 0; useMangle < 2; useMangle++)
							for (int useSwipe = 0; useSwipe < 2; useSwipe++)
								for (int useFaerieFire = 0; useFaerieFire < 2; useFaerieFire++)
									for (int useLacerate = 0; useLacerate < 2; useLacerate++)
									{
										bool?[] useMaulValues = new bool?[] { null, false, true };
										BearRotationCalculator.BearRotationCalculation rotationCalculation =
											rotationCalculatorDPS.GetRotationCalculations(useMaulValues[useMaul],
											useMangle == 1, useSwipe == 1, useFaerieFire == 1, useLacerate == 1);

										comparisonsDPS.Add(new ComparisonCalculationBear()
										{
                                            CharacterItems = character.GetItems(),
											Name = rotationCalculation.Name.Replace("+", " + "),
											OverallPoints = rotationCalculation.DPS,
											ThreatPoints = rotationCalculation.DPS
										});
									}

					return comparisonsDPS.ToArray();

				case "Rotation TPS":
					CharacterCalculationsBear calcsTPS = GetCharacterCalculations(character) as CharacterCalculationsBear;
					List<ComparisonCalculationBase> comparisonsTPS = new List<ComparisonCalculationBase>();

					BearRotationCalculator rotationCalculatorTPS = new BearRotationCalculator(calcsTPS.MeleeDamageAverage,
						calcsTPS.MaulDamageAverage, calcsTPS.MangleDamageAverage, calcsTPS.SwipeDamageAverage,
						calcsTPS.FaerieFireDamageAverage, calcsTPS.LacerateDamageAverage, calcsTPS.LacerateDotDamageAverage, calcsTPS.MeleeThreatAverage,
						calcsTPS.MaulThreatAverage, calcsTPS.MangleThreatAverage, calcsTPS.SwipeThreatAverage,
						calcsTPS.FaerieFireThreatAverage, calcsTPS.LacerateThreatAverage, calcsTPS.LacerateDotThreatAverage,
						6f - calcsTPS.BasicStats.MangleCooldownReduction, calcsTPS.AttackSpeed);

					for (int useMaul = 0; useMaul < 3; useMaul++)
						for (int useMangle = 0; useMangle < 2; useMangle++)
							for (int useSwipe = 0; useSwipe < 2; useSwipe++)
								for (int useFaerieFire = 0; useFaerieFire < 2; useFaerieFire++)
									for (int useLacerate = 0; useLacerate < 2; useLacerate++)
									{
										bool?[] useMaulValues = new bool?[] { null, false, true };
										BearRotationCalculator.BearRotationCalculation rotationCalculation =
											rotationCalculatorTPS.GetRotationCalculations(useMaulValues[useMaul],
											useMangle == 1, useSwipe == 1, useFaerieFire == 1, useLacerate == 1);

										comparisonsTPS.Add(new ComparisonCalculationBear()
										{
                                            CharacterItems = character.GetItems(),
											Name = rotationCalculation.Name.Replace("+", " + "),
											OverallPoints = rotationCalculation.TPS,
											ThreatPoints = rotationCalculation.TPS
										});
									}

					return comparisonsTPS.ToArray();

				default:
					return new ComparisonCalculationBase[0];
			}
		}
		#endregion

		#region Relevancy Methods
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
				Armor = stats.Armor,
				BonusArmor = stats.BonusArmor,
				Stamina = stats.Stamina,
				Agility = stats.Agility,
				DodgeRating = stats.DodgeRating,
				DefenseRating = stats.DefenseRating,
				Resilience = stats.Resilience,
				TerrorProc = stats.TerrorProc,
				BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
				BaseArmorMultiplier = stats.BaseArmorMultiplier,
				BonusArmorMultiplier = stats.BonusArmorMultiplier,
				BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                Health = stats.Health,
                BonusHealthMultiplier = stats.BonusHealthMultiplier,
				Miss = stats.Miss,
				CritChanceReduction = stats.CritChanceReduction,
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
				MongooseProc = stats.MongooseProc,

                Strength = stats.Strength,
                AttackPower = stats.AttackPower,
                CritRating = stats.CritRating,
				PhysicalCrit = stats.PhysicalCrit,
                HitRating = stats.HitRating,
				PhysicalHit = stats.PhysicalHit,
                HasteRating = stats.HasteRating,
				PhysicalHaste = stats.PhysicalHaste,
                ExpertiseRating = stats.ExpertiseRating,
                ArmorPenetration = stats.ArmorPenetration,
                WeaponDamage = stats.WeaponDamage,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                BonusMangleBearThreat = stats.BonusMangleBearThreat,
                BonusLacerateDamageMultiplier = stats.BonusLacerateDamageMultiplier,
                BonusSwipeDamageMultiplier = stats.BonusSwipeDamageMultiplier,
                BloodlustProc = stats.BloodlustProc,
                BonusMangleBearDamage = stats.BonusMangleBearDamage,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
				DamageTakenMultiplier = stats.DamageTakenMultiplier,
				ArmorPenetrationRating = stats.ArmorPenetrationRating,
                GreatnessProc = stats.GreatnessProc
			};
		}

		public override bool HasRelevantStats(Stats stats)
		{
			return (stats.Agility + stats.Armor + stats.BonusArmor + stats.BonusAgilityMultiplier + stats.BonusArmorMultiplier +
				stats.BonusStaminaMultiplier + stats.DefenseRating + stats.DodgeRating + stats.Health + stats.BonusHealthMultiplier +
				stats.Miss + stats.Resilience + stats.Stamina + stats.TerrorProc + stats.AllResist +
				stats.ArcaneResistance + stats.NatureResistance + stats.FireResistance +
				stats.FrostResistance + stats.ShadowResistance + stats.ArcaneResistanceBuff +
				stats.NatureResistanceBuff + stats.FireResistanceBuff + stats.PhysicalCrit +
				stats.FrostResistanceBuff + stats.ShadowResistanceBuff + stats.CritChanceReduction +
				stats.ArmorPenetrationRating + stats.PhysicalHaste + stats.MongooseProc
                 + stats.Strength + stats.AttackPower + stats.CritRating + stats.HitRating + stats.HasteRating
                 + stats.ExpertiseRating + stats.ArmorPenetration + stats.WeaponDamage + stats.BonusCritMultiplier
				 + stats.BonusRipDuration + stats.GreatnessProc + stats.PhysicalHit
                 + stats.TerrorProc+stats.BonusMangleBearThreat + stats.BonusLacerateDamageMultiplier + stats.BonusSwipeDamageMultiplier
                 + stats.BloodlustProc + stats.BonusMangleBearDamage + stats.BonusAttackPowerMultiplier + stats.BonusDamageMultiplier
                 + stats.DamageTakenMultiplier + stats.ArmorPenetrationRating) != 0;
		}
		#endregion

		public override void SetDefaults(Character character)
		{
			character.ActiveBuffs.Add(Buff.GetBuffByName("Horn of Winter"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Devotion Aura"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Inspiration"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Battle Shout"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Unleashed Rage"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Moonkin Form"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Commanding Shout"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Leader of the Pack"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Icy Talons"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Power Word: Fortitude"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Mark of the Wild"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Blessing of Kings"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Sunder Armor"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Faerie Fire"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Totem of Wrath"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Flask of Stoneblood"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Agility Food"));
			character.ActiveBuffs.Add(Buff.GetBuffByName("Bloodlust"));

			character.DruidTalents.GlyphOfMaul = true;
			character.DruidTalents.GlyphOfGrowl = true;
			character.DruidTalents.GlyphOfFrenziedRegeneration = true;
		}

		private static List<string> _relevantGlyphs;
		public override List<string> GetRelevantGlyphs()
		{
			if (_relevantGlyphs == null)
			{
				_relevantGlyphs = new List<string>();
				_relevantGlyphs.Add("Glyph of Maul");
				_relevantGlyphs.Add("Glyph of Growl");
				_relevantGlyphs.Add("Glyph of Frenzied Regeneration");
				_relevantGlyphs.Add("Glyph of Mangle");
				_relevantGlyphs.Add("Glyph of Berserk");
			}
			return _relevantGlyphs;
		}
	}

	/// <summary>
	/// Data container class for the results of calculations about a Character
	/// </summary>
    public class CharacterCalculationsBear : CharacterCalculationsBase
    {
		private float _overallPoints = 0f;
		public override float OverallPoints
		{
			get { return _overallPoints; }
			set { _overallPoints = value; }
		}

		private float[] _subPoints = new float[] { 0f, 0f, 0f };
		public override float[] SubPoints
		{
			get { return _subPoints; }
			set { _subPoints = value; }
		}

		public float MitigationPoints
		{
			get { return _subPoints[0]; }
			set { _subPoints[0] = value; }
		}

		public float SurvivalPoints
		{
			get { return _subPoints[1]; }
			set { _subPoints[1] = value; }
		}

        public float ThreatPoints
        {
			get { return _subPoints[2]; }
			set { _subPoints[2] = value; }
        }

		public Stats BasicStats { get; set; }
		public int TargetLevel { get; set; }
		public float Dodge { get; set; }
		public float Miss { get; set; }
		public float ConstantDamageReduction { get; set; }
		public float AvoidancePreDR { get; set; }
		public float AvoidancePostDR { get; set; }
		public float TotalMitigation { get; set; }
		public float SavageDefenseChance { get; set; }
		public float SavageDefenseValue { get; set; }
		public float SavageDefensePercent { get; set; }
		public float DamageTaken { get; set; }
		public float CritReduction { get; set; }
		public float CappedCritReduction { get; set; }
		public float SurvivalPointsRaw { get; set; }
		
		public float NatureSurvivalPoints { get; set; }
		public float FrostSurvivalPoints { get; set; }
		public float FireSurvivalPoints { get; set; }
		public float ShadowSurvivalPoints { get; set; }
		public float ArcaneSurvivalPoints { get; set; }

		public float AttackSpeed { get; set; }
		public float MangleCooldown { get; set; }
		
        public float MissedAttacks { get; set; }
		public float DodgedAttacks { get; set; }
		public float ParriedAttacks { get; set; }
		public float AvoidedAttacks { get { return MissedAttacks + DodgedAttacks + ParriedAttacks; } }

		public float MeleeThreatRaw { get; set; }
		public float MaulThreatRaw { get; set; }
		public float MangleThreatRaw { get; set; }
		public float SwipeThreatRaw { get; set; }
		public float FaerieFireThreatRaw { get; set; }
		public float LacerateThreatRaw { get; set; }
		public float LacerateDotThreatRaw { get; set; }

		public float MeleeThreatAverage { get; set; }
		public float MaulThreatAverage { get; set; }
		public float MangleThreatAverage { get; set; }
		public float SwipeThreatAverage { get; set; }
		public float FaerieFireThreatAverage { get; set; }
		public float LacerateThreatAverage { get; set; }
		public float LacerateDotThreatAverage { get; set; }

		public float MeleeDamageRaw { get; set; }
		public float MaulDamageRaw { get; set; }
		public float MangleDamageRaw { get; set; }
		public float SwipeDamageRaw { get; set; }
		public float FaerieFireDamageRaw { get; set; }
		public float LacerateDamageRaw { get; set; }
		public float LacerateDotDamageRaw { get; set; }

		public float MeleeDamageAverage { get; set; }
		public float MaulDamageAverage { get; set; }
		public float MangleDamageAverage { get; set; }
		public float SwipeDamageAverage { get; set; }
		public float FaerieFireDamageAverage { get; set; }
		public float LacerateDamageAverage { get; set; }
		public float LacerateDotDamageAverage { get; set; }

		public float MaulTPR { get; set; }
		public float MaulDPR { get; set; }
		public float MangleTPR { get; set; }
		public float MangleDPR { get; set; }
		public float SwipeTPR { get; set; }
		public float SwipeDPR { get; set; }
		public float LacerateTPR { get; set; }
		public float LacerateDPR { get; set; }

		public BearRotationCalculator.BearRotationCalculation HighestDPSRotation { get; set; }
		public BearRotationCalculator.BearRotationCalculation HighestTPSRotation { get; set; }
		public BearRotationCalculator.BearRotationCalculation SwipeRotation { get; set; }
		public BearRotationCalculator.BearRotationCalculation CustomRotation { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
		{
			Dictionary<string, string> dictValues = new Dictionary<string, string>();

			float baseMiss = (new float[] { 0.05f, 0.055f, 0.06f, 0.08f })[TargetLevel - 80] - BasicStats.PhysicalHit;
			float baseDodge = (new float[] { 0.05f, 0.055f, 0.06f, 0.065f })[TargetLevel - 80] - BasicStats.Expertise * 0.0025f;
			float baseParry = (new float[] { 0.05f, 0.055f, 0.06f, 0.1375f })[TargetLevel - 80] - BasicStats.Expertise * 0.0025f;
			float capMiss = (float)Math.Ceiling(baseMiss * StatConversion.RATING_PER_PHYSICALHIT);
			float capDodge = (float)Math.Ceiling(baseDodge * 400f * StatConversion.RATING_PER_EXPERTISE);
			float capParry = (float)Math.Ceiling(baseParry * 400f * StatConversion.RATING_PER_EXPERTISE);

			string tipMiss = string.Empty;
			if (BasicStats.HitRating > capMiss)
				tipMiss = string.Format("*Over the cap by {0} Hit Rating", BasicStats.HitRating - capMiss);
			else if (BasicStats.HitRating < capMiss)
				tipMiss = string.Format("*Under the cap by {0} Hit Rating", capMiss - BasicStats.HitRating);
			else
				tipMiss = "*Exactly at the cap";

			string tipDodgeParry = string.Empty;
			if (BasicStats.ExpertiseRating > capDodge)
				tipDodgeParry = string.Format("*Over the dodge cap by {0} Expertise Rating\r\n", BasicStats.ExpertiseRating - capDodge);
			else if (BasicStats.ExpertiseRating < capDodge)
				tipDodgeParry = string.Format("*Under the dodge cap by {0} Expertise Rating\r\n", capDodge - BasicStats.ExpertiseRating);
			else
				tipDodgeParry = "*Exactly at the dodge cap";

			if (BasicStats.ExpertiseRating > capParry)
				tipDodgeParry += string.Format("Over the parry cap by {0} Expertise Rating", BasicStats.ExpertiseRating - capParry);
			else if (BasicStats.ExpertiseRating < capParry)
				tipDodgeParry += string.Format("Under the parry cap by {0} Expertise Rating", capParry - BasicStats.ExpertiseRating);
			else
				tipDodgeParry += "Exactly at the parry cap";




			int armorCap = (int)Math.Ceiling((1402.5f * TargetLevel) - 66502.5f);
			float levelDifference = 0.002f * (TargetLevel - 80);
			float targetCritReduction = 0.05f + levelDifference;
			int defToCap = 0, resToCap = 0;
			if (CritReduction < targetCritReduction)
			{
				//while (((float)Math.Floor((BasicStats.DefenseRating + defToCap) / (123f / 52f)) * 0.04f)
				//+ BasicStats.Resilience / (2050f / 52f) + BasicStats.CritChanceReduction < targetCritReduction)
				//    defToCap++;
				//while (((float)Math.Floor(BasicStats.DefenseRating / (123f / 52f)) * 0.04f)
				//+ (BasicStats.Resilience + resToCap) / (2050f / 52f) + BasicStats.CritChanceReduction < targetCritReduction)
				//    resToCap++;
				while (((float)Math.Floor(StatConversion.GetDefenseFromRating(BasicStats.DefenseRating + defToCap)) * 0.0004f)
				+ StatConversion.GetResilienceFromRating(BasicStats.Resilience) + BasicStats.CritChanceReduction < targetCritReduction)
					defToCap++;
				while (((float)Math.Floor(StatConversion.GetDefenseFromRating(BasicStats.DefenseRating)) * 0.0004f)
				+ StatConversion.GetResilienceFromRating(BasicStats.Resilience + resToCap) + BasicStats.CritChanceReduction < targetCritReduction)
					resToCap++;
			}
			else if (CritReduction > targetCritReduction)
			{
				//while (((float)Math.Floor((BasicStats.DefenseRating + defToCap) / (123f / 52f)) * 0.04f)
				//+ BasicStats.Resilience / (2050f / 52f) + BasicStats.CritChanceReduction > targetCritReduction)
				//    defToCap--;
				//while (((float)Math.Floor(BasicStats.DefenseRating / (123f / 52f)) * 0.04f)
				//+ (BasicStats.Resilience + resToCap) / (2050f / 52f) + BasicStats.CritChanceReduction > targetCritReduction)
				//    resToCap--;
				
				while (((float)Math.Floor(StatConversion.GetDefenseFromRating(BasicStats.DefenseRating + defToCap)) * 0.0004f)
				+ StatConversion.GetResilienceFromRating(BasicStats.Resilience) + BasicStats.CritChanceReduction > targetCritReduction)
					defToCap--;
				while (((float)Math.Floor(StatConversion.GetDefenseFromRating(BasicStats.DefenseRating)) * 0.0004f)
				+ StatConversion.GetResilienceFromRating(BasicStats.Resilience + resToCap) + BasicStats.CritChanceReduction > targetCritReduction)
					resToCap--;
				defToCap++;
				resToCap++;
			}

            // Changed to not just give a resist rating, but a breakdown of the resulting resist values in the tooltip
            string tipResist = string.Empty;
            tipResist = StatConversion.GetResistanceTableString(80, TargetLevel, BasicStats.NatureResistance, 0);
            dictValues.Add("Nature Resist", (BasicStats.NatureResistance + BasicStats.AllResist).ToString() + "*" + tipResist);
            tipResist = StatConversion.GetResistanceTableString(80, TargetLevel, BasicStats.ArcaneResistance, 0);
            dictValues.Add("Arcane Resist", (BasicStats.ArcaneResistance + BasicStats.AllResist).ToString() + "*" + tipResist);
            tipResist = StatConversion.GetResistanceTableString(80, TargetLevel, BasicStats.FrostResistance, 0);
            dictValues.Add("Frost Resist", (BasicStats.FrostResistance + BasicStats.AllResist).ToString() + "*" + tipResist);
            tipResist = StatConversion.GetResistanceTableString(80, TargetLevel, BasicStats.FireResistance, 0);
            dictValues.Add("Fire Resist", (BasicStats.FireResistance + BasicStats.AllResist).ToString() + "*" + tipResist);
            tipResist = StatConversion.GetResistanceTableString(80, TargetLevel, BasicStats.ShadowResistance, 0);
            dictValues.Add("Shadow Resist", (BasicStats.ShadowResistance + BasicStats.AllResist).ToString() + "*" + tipResist);

			dictValues.Add("Health", BasicStats.Health.ToString());
			dictValues.Add("Agility", BasicStats.Agility.ToString());
			dictValues.Add("Armor", BasicStats.Armor.ToString());
			dictValues.Add("Stamina", BasicStats.Stamina.ToString());
			dictValues.Add("Dodge Rating", BasicStats.DodgeRating.ToString());
			dictValues.Add("Defense Rating", BasicStats.DefenseRating.ToString());
			dictValues.Add("Resilience", BasicStats.Resilience.ToString());
			dictValues.Add("Dodge", Dodge.ToString("0.000%"));
			dictValues.Add("Miss", Miss.ToString("0.000%"));
			if (BasicStats.Armor == armorCap)
				dictValues.Add("Mitigation", ConstantDamageReduction.ToString("0.000%")
					+ string.Format("*Exactly at the armor cap against level {0} mobs.", TargetLevel));
			else if (BasicStats.Armor > armorCap)
				dictValues.Add("Mitigation", ConstantDamageReduction.ToString("0.000%")
					+ string.Format("*Over the armor cap by {0} armor.", BasicStats.Armor - armorCap));
			else
				dictValues.Add("Mitigation", ConstantDamageReduction.ToString("0.000%")
					+ string.Format("*Short of the armor cap by {0} armor.", armorCap - BasicStats.Armor));
			dictValues.Add("Avoidance PreDR", AvoidancePreDR.ToString("0.000%"));
			dictValues.Add("Avoidance PostDR", AvoidancePostDR.ToString("0.000%"));
			dictValues.Add("Total Mitigation", TotalMitigation.ToString("0.000%"));
			dictValues.Add("Damage Taken", DamageTaken.ToString("0.000%"));
			dictValues.Add("Savage Defense", string.Format(
				"{0}  ~ {1}*{0} chance to absorb incoming hit\r\n{1} absorbed per hit\r\n{2} of incoming damage absorbed", 
				SavageDefenseChance.ToString("0.000%"), SavageDefenseValue, SavageDefensePercent.ToString("0.000%")));
			dictValues.Add("Chance to be Crit", ((0.05f + levelDifference) - CritReduction).ToString("0.000%"));
			dictValues.Add("Overall Points", OverallPoints.ToString());
			dictValues.Add("Mitigation Points", MitigationPoints.ToString());
			dictValues.Add("Survival Points", string.Format("{0}*{1} Before Soft Cap", SurvivalPoints.ToString(), SurvivalPointsRaw.ToString()));
			dictValues.Add("Threat Points", ThreatPoints.ToString());

            dictValues["Nature Survival"] = NatureSurvivalPoints.ToString();
            dictValues["Frost Survival"] = FrostSurvivalPoints.ToString();
            dictValues["Fire Survival"] = FireSurvivalPoints.ToString();
            dictValues["Shadow Survival"] = ShadowSurvivalPoints.ToString();
            dictValues["Arcane Survival"] = ArcaneSurvivalPoints.ToString();


			dictValues["Strength"] = BasicStats.Strength.ToString();
			dictValues["Attack Power"] = BasicStats.AttackPower.ToString();
			dictValues["Crit Rating"] = BasicStats.CritRating.ToString();
			dictValues["Hit Rating"] = BasicStats.HitRating.ToString() + tipMiss;
			dictValues["Expertise Rating"] = BasicStats.ExpertiseRating.ToString() + tipDodgeParry;
			dictValues["Haste Rating"] = string.Format("{0}*{1}sec Attack Speed", BasicStats.HasteRating, AttackSpeed.ToString("0.000"));
			dictValues["Armor Penetration Rating"] = BasicStats.ArmorPenetrationRating.ToString();

			dictValues["Avoided Attacks"] = String.Format("{0}*{1} Missed\r\n{2} Dodged\r\n{3} Parried", 
				AvoidedAttacks.ToString("0.000%"), MissedAttacks.ToString("0.000%"), 
				DodgedAttacks.ToString("0.000%"), ParriedAttacks.ToString("0.000%"));
           
			string rotationFormat = "{0} DPS, {1} TPS*{2}";
			dictValues["Highest DPS Rotation"] = String.Format(rotationFormat, Math.Round(HighestDPSRotation.DPS), Math.Round(HighestDPSRotation.TPS), HighestDPSRotation.Name);
			dictValues["Highest TPS Rotation"] = String.Format(rotationFormat, Math.Round(HighestTPSRotation.DPS), Math.Round(HighestTPSRotation.TPS), HighestTPSRotation.Name);
			dictValues["Swipe Rotation"] = String.Format(rotationFormat, Math.Round(SwipeRotation.DPS), Math.Round(SwipeRotation.TPS), SwipeRotation.Name);
			dictValues["Custom Rotation"] = String.Format(rotationFormat, Math.Round(CustomRotation.DPS), Math.Round(CustomRotation.TPS), CustomRotation.Name);

			string attackFormat = "{0} Dmg, {1} Threat*Per Hit: {0} Damage, {1} Threat\r\nPer Average Swing: {2} Damage, {3} Threat";
			string attackFormatWithRage = attackFormat + "\r\nThreat Per Rage: {4}\r\nDamage Per Rage: {5}";
			dictValues["Melee"] = String.Format(attackFormat, MeleeDamageRaw, MeleeThreatRaw, MeleeDamageAverage, MeleeThreatAverage);
			dictValues["Maul"] = String.Format(attackFormatWithRage, MaulDamageRaw, MaulThreatRaw, MaulDamageAverage, MaulThreatAverage, MaulTPR, MaulDPR);
			dictValues["Mangle"] = String.Format(attackFormatWithRage, MangleDamageRaw, MangleThreatRaw, MangleDamageAverage, MangleThreatAverage, MangleTPR, MangleDPR);
			dictValues["Swipe"] = String.Format(attackFormatWithRage, SwipeDamageRaw, SwipeThreatRaw, SwipeDamageAverage, SwipeThreatAverage, SwipeTPR, SwipeDPR);
			dictValues["Faerie Fire"] = String.Format(attackFormat, FaerieFireDamageRaw, FaerieFireThreatRaw, FaerieFireDamageAverage, FaerieFireThreatAverage);
			dictValues["Lacerate"] = String.Format(attackFormatWithRage, LacerateDamageRaw, LacerateThreatRaw, LacerateDamageAverage, LacerateThreatAverage, LacerateTPR, LacerateDPR);
			dictValues["Lacerate DoT Tick"] = String.Format(attackFormat, LacerateDotDamageRaw, LacerateDotThreatRaw, LacerateDotDamageAverage, LacerateDotThreatAverage).Replace("Swing", "Tick");
			
			return dictValues;
		}

		public override float GetOptimizableCalculationValue(string calculation)
		{
			switch (calculation)
			{
				case "Health": return BasicStats.Health;
				case "Hit Rating": return BasicStats.HitRating;
				case "Expertise Rating": return BasicStats.ExpertiseRating;
				case "Haste Rating": return BasicStats.HasteRating;
				case "Avoided Attacks %": return AvoidedAttacks * 100f;
				case "Mitigation % from Armor": return ConstantDamageReduction * 100f;
				case "Avoidance %": return AvoidancePostDR * 100f;
				case "% Chance to be Crit": return ((5f + (0.2f * (TargetLevel - 80))) - CritReduction * 100f);
				case "Nature Survival": return NatureSurvivalPoints;
				case "Fire Survival": return FireSurvivalPoints;
				case "Frost Survival": return FrostSurvivalPoints;
				case "Shadow Survival": return ShadowSurvivalPoints;
				case "Arcane Survival": return ArcaneSurvivalPoints;
				case "Nature Resist": return BasicStats.NatureResistance;
				case "Fire Resist": return BasicStats.FireResistance;
				case "Frost Resist": return BasicStats.FrostResistance;
				case "Shadow Resist": return BasicStats.ShadowResistance;
				case "Arcane Resist": return BasicStats.ArcaneResistance;
				case "Highest DPS": return HighestDPSRotation.DPS;
				case "Highest TPS": return HighestTPSRotation.TPS;
				case "Swipe DPS": return SwipeRotation.DPS;
				case "Swipe TPS": return SwipeRotation.TPS;
				case "Custom Rotation DPS": return CustomRotation.DPS;
				case "Custom Rotation TPS": return CustomRotation.TPS;
			}
			return 0f;
		}
    }

	/// <summary>
	/// Data container class for the results of a comparison of two CharacterCalculationsBear objects
	/// </summary>
	public class ComparisonCalculationBear : ComparisonCalculationBase
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

		private float[] _subPoints = new float[] { 0f, 0f, 0f };
		public override float[] SubPoints
		{
			get { return _subPoints; }
			set { _subPoints = value; }
		}

		public float MitigationPoints
		{
			get { return _subPoints[0]; }
			set { _subPoints[0] = value; }
		}

		public float SurvivalPoints
		{
			get { return _subPoints[1]; }
			set { _subPoints[1] = value; }
		}
		public float ThreatPoints
		{
			get { return _subPoints[2]; }
			set { _subPoints[2] = value; }
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
			return string.Format("{0}: ({1}O {2}M {3}S {4}T)", Name, Math.Round(OverallPoints), Math.Round(MitigationPoints), Math.Round(SurvivalPoints), Math.Round(ThreatPoints));
		}
	}
}
