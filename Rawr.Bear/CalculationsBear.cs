using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Bear
{
	[Rawr.Calculations.RawrModelInfo("Bear", "Ability_Racial_BearForm", Character.CharacterClass.Druid)]
	public class CalculationsBear : CalculationsBase
	{
		//my insides all turned to ash / so slow
		//and blew away as i collapsed / so cold
		private CalculationOptionsPanelBase _calculationOptionsPanel = null;
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
					//"Complex Stats:Limited Threat",
					//"Complex Stats:Unlimited Threat*Limited threat with Maul threat added in.",
					//"Complex Stats:1-3-0*1 Mangle, 3 Lacerate rotation.",
					//"Complex Stats:1-0-3*1 Mangle, 3 Swipe rotation.",
					//"Complex Stats:1-1-2*1 Mangle, 1 Lacerate, 2 Swipe rotation.",
					//"Complex Stats:2-1-5*2 Mangle, 1 Lacerate, 5 Swipe rotation.",
					//"Complex Stats:Lacerate Max TPS*3 lacerates every 6 second Mangle cooldown.  Assumes a 5 stack.",
					//"Complex Stats:Lacerate Min TPS*Minimal Lacerates to maintain a 5 stack",
					//"Complex Stats:Swipe Threat*Swipe threat on a single target",
					//"Complex Stats:Mangle Threat",
					//"Complex Stats:Maul TPS",
					//"Complex Stats:White TPS",
					//"Complex Stats:Missed Attacks",
					"Mitigation Stats:Dodge",
					"Mitigation Stats:Miss",
					"Mitigation Stats:Mitigation",
					"Mitigation Stats:Avoidance PreDR",
					"Mitigation Stats:Avoidance PostDR",
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
					"Threat Stats:Lacerate DOT Tick",
					"Threat Stats:Missed Attacks",
					};
				return _characterDisplayCalculationLabels;
			}
		}

		private string[] _optimizableCalculationLabels = null;
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
                    "Missed Attacks",
                    "Unlimited Threat",
                    "Limited Threat",
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
		public override string[] CustomChartNames
		{
			get
			{
				if (_customChartNames == null)
					_customChartNames = new string[] {
					"Combat Table",
					"Relative Stat Values",
					"Rotation DPS",
					"Rotation TPS"
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
					_subPointNameColors.Add("Mitigation", System.Drawing.Color.Red);
					_subPointNameColors.Add("Survival", System.Drawing.Color.Blue);
                    _subPointNameColors.Add("Threat", System.Drawing.Color.Green);
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
						Item.ItemType.TwoHandMace
					});
				}
				return _relevantItemTypes;
			}
		}

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Druid; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationBear(); }
		public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsBear(); }

		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsBear));
			System.IO.StringReader reader = new System.IO.StringReader(xml);
			CalculationOptionsBear calcOpts = serializer.Deserialize(reader) as CalculationOptionsBear;
			return calcOpts;
		}

		public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
		{
			CalculationOptionsBear calcOpts = character.CalculationOptions as CalculationOptionsBear;
			int targetLevel = calcOpts.TargetLevel;
			int characterLevel = character.Level;
			Stats stats = GetCharacterStats(character, additionalItem);
			float levelDifference = (targetLevel - characterLevel) * 0.2f;
			
			CharacterCalculationsBear calculatedStats = new CharacterCalculationsBear();
			calculatedStats.BasicStats = stats;
			calculatedStats.TargetLevel = targetLevel;

			float baseAgi = character.Race == Character.CharacterRace.NightElf ? 87 : 75; //TODO: Find correct base agi values at 80
			
			float defSkill = (float)Math.Floor(stats.DefenseRating / 4.918498039f);
			float dodgeNonDR = stats.Dodge * 100f - levelDifference + baseAgi * 0.024f; //TODO: Find correct Agi->Dodge ratio
			float missNonDR = stats.Miss * 100f - levelDifference;
			float dodgePreDR = (stats.Agility + (stats.TerrorProc * 0.55f) - baseAgi) * 0.024f + (stats.DodgeRating / 39.34798813f) + (defSkill * 0.04f); //TODO: Find correct Agi->Dodge ratio
			float missPreDR = (defSkill * 0.04f);
			float dodgePostDR = 1f / (1f / 116.890707f + 0.972f / dodgePreDR);
			float missPostDR = 1f / (1f / 116.890707f + 0.972f / missPreDR);
			float dodgeTotal = dodgeNonDR + dodgePostDR;
			float missTotal = missNonDR + missPostDR;

			calculatedStats.Miss = missTotal;
			calculatedStats.Dodge = Math.Min(100f - calculatedStats.Miss, dodgeTotal);
			calculatedStats.Mitigation = 100 - ((100 - Math.Min(75f, (stats.Armor / (stats.Armor - 22167.5f + (467.5f * targetLevel))) * 100f)) * (1f + stats.DamageTakenMultiplier));
			calculatedStats.AvoidancePreDR = dodgeNonDR + dodgePreDR + missNonDR + missPreDR;
			calculatedStats.AvoidancePostDR = dodgeTotal + missTotal;
			calculatedStats.CritReduction = (defSkill * 0.04f) + stats.Resilience / (2050f / 52f) + stats.CritChanceReduction * 100f;
			calculatedStats.CappedCritReduction = Math.Min(5f + levelDifference, calculatedStats.CritReduction);

			//Out of 100 attacks, you'll take...
			float crits = Math.Min(Math.Max(0f, 100f - calculatedStats.AvoidancePostDR), (5f + levelDifference) - calculatedStats.CappedCritReduction);
			//float crushes = targetLevel == 73 ? Math.Max(0f, Math.Min(15f, 100f - (crits + calculatedStats.AvoidancePreDR)) - stats.CritChanceReduction) : 0f;
			float hits = Math.Max(0f, 100f - (crits + calculatedStats.AvoidancePostDR));
			//Apply armor and multipliers for each attack type...
			crits *= (100f - calculatedStats.Mitigation) * .02f;
			//crushes *= (100f - calculatedStats.Mitigation) * .015f;
			hits *= (100f - calculatedStats.Mitigation) * .01f;
			calculatedStats.DamageTaken = hits + crits;
			calculatedStats.TotalMitigation = 100f - calculatedStats.DamageTaken;

			calculatedStats.SurvivalPointsRaw = (stats.Health / (1f - (calculatedStats.Mitigation / 100f)));
			double survivalCap = (double)calcOpts.SurvivalSoftCap / 1000d;
			double survivalRaw = (stats.Health / (1f - (calculatedStats.Mitigation / 100f))) / 1000f;

			if (survivalRaw <= survivalCap)
				calculatedStats.SurvivalPoints = 1000f * (float)survivalRaw; // / (buffs.ShadowEmbrace ? 0.95f : 1f);
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

			calculatedStats.MitigationPoints = (2000000f / calculatedStats.DamageTaken); // / (buffs.ShadowEmbrace ? 0.95f : 1f);

            float cappedResist = targetLevel * 5;

            calculatedStats.NatureSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.NatureResistance) / cappedResist) * .75)));
            calculatedStats.FrostSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.FrostResistance) / cappedResist) * .75)));
            calculatedStats.FireSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.FireResistance) / cappedResist) * .75)));
            calculatedStats.ShadowSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.ShadowResistance) / cappedResist) * .75)));
            calculatedStats.ArcaneSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.ArcaneResistance) / cappedResist) * .75)));

			//TODO:
            CalculateThreat(stats, targetLevel, calculatedStats, character);
			calculatedStats.OverallPoints = calculatedStats.MitigationPoints + 
				calculatedStats.SurvivalPoints + calculatedStats.ThreatPoints;
            return calculatedStats;
        }
        
        private void CalculateThreat(Stats stats, int targetLevel, CharacterCalculationsBear calculatedStats, Character character)
        {
			CalculationOptionsBear calcOpts = character.CalculationOptions as CalculationOptionsBear;
			DruidTalents talents = character.DruidTalents;

			int targetArmor = calcOpts.TargetArmor;
			float baseArmor = Math.Max(0f, targetArmor - stats.ArmorPenetration);
			baseArmor *= (1f - (stats.ArmorPenetrationRating / 15.39529991f) / 100f);
			float modArmor = 1f - (baseArmor / ((467.5f * character.Level) + baseArmor - 22167.5f));

			float critMultiplier = 2f * (1 + stats.BonusCritMultiplier);
			float spellCritMultiplier = 1.5f * (1 + stats.BonusCritMultiplier);

			float hasteBonus = stats.HasteRating / 32.78998947f / 100f;
            float attackSpeed = (2.5f) / (1f + hasteBonus);
			attackSpeed = attackSpeed / (1f + stats.PhysicalHaste);

			float hitBonus = stats.HitRating / 32.78998947f / 100f;
			float expertiseBonus = stats.ExpertiseRating / 32.78998947f / 100f + stats.Expertise * 0.0025f;

            float chanceDodge = Math.Max(0f, 0.065f + .005f * (targetLevel - 83) - expertiseBonus);
            float chanceParry = Math.Max(0f, 0.1375f - expertiseBonus); // Parry for lower levels?
            float chanceBlock = 0;//ha!
            float chanceMiss = Math.Max(0f, 0.09f - hitBonus);
            if ((targetLevel - 80f) < 3)
            {
                chanceMiss = Math.Max(0f, 0.05f + 0.005f * (targetLevel - 80f) - hitBonus);
            }
            
            float chanceGlance = 0.2335774f;
            float glanceMultiplier = .7f;
            float chanceAvoided = chanceMiss + chanceDodge + chanceParry;

			float chanceCrit = Math.Min(0.75f, (stats.CritRating / 45.90598679f + 
				((stats.Agility + (1 - (float)Math.Pow(chanceAvoided, 2f) ) * stats.TerrorProc * 2.0f / 3.0f) * 0.012f)) 
				/ 100f + stats.PhysicalCrit);

            calculatedStats.DodgedAttacks = chanceDodge * 100;
            calculatedStats.ParriedAttacks = chanceParry * 100;
            calculatedStats.MissedAttacks = chanceMiss * 100;

			float baseDamage = 137f + (stats.AttackPower / 14f) * 2.5f;
			float bearThreatMultiplier = 29f / 14f;

			float meleeDamageRaw = baseDamage * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * modArmor;
			float maulDamageRaw = (baseDamage + 578) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusMaulDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier) * modArmor;
			float mangleDamageRaw = (baseDamage * 1.15f + 299) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusMangleDamageMultiplier) * modArmor;
			float swipeDamageRaw = (stats.AttackPower * 0.063f + 108f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusSwipeDamageMultiplier) * modArmor;
			float faerieFireDamageRaw = (stats.AttackPower * 0.05f + 1f) * (1f + stats.BonusDamageMultiplier);
			float lacerateDamageRaw = (stats.AttackPower * 0.01f + 88f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * modArmor * (1f + stats.BonusLacerateDamageMultiplier);
			float lacerateDotDamage = (stats.AttackPower * 0.01f + 64f) * 5f /*stack size*/ * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier) * (1f + stats.BonusLacerateDamageMultiplier);

			float meleeDamageAverage = (chanceCrit * (meleeDamageRaw * critMultiplier)) + //Damage from crits
							(chanceGlance * (meleeDamageRaw * glanceMultiplier)) + //Damage from glances
							((1f - chanceCrit - chanceAvoided - chanceGlance) * (meleeDamageRaw)); //Damage from hits
			float maulDamageAverage = (chanceCrit * (maulDamageRaw * critMultiplier)) + ((1f - chanceCrit - chanceAvoided) * (maulDamageRaw));
			float mangleDamageAverage = (chanceCrit * (mangleDamageRaw * critMultiplier)) + ((1f - chanceCrit - chanceAvoided) * (mangleDamageRaw));
			float swipeDamageAverage = (chanceCrit * (swipeDamageRaw * critMultiplier)) + ((1f - chanceCrit - chanceAvoided) * (swipeDamageRaw));
			float faerieFireDamageAverage = (0.25f * (faerieFireDamageRaw * spellCritMultiplier)) + (0.65f * (faerieFireDamageRaw)); //TODO: Assumes 25% spell crit and 10% spell miss
			float lacerateDamageAverage = (chanceCrit * (lacerateDamageRaw * critMultiplier)) + ((1f - chanceCrit - chanceAvoided) * (lacerateDamageRaw));
			
			float meleeThreatRaw = bearThreatMultiplier * meleeDamageRaw;
			float maulThreatRaw = bearThreatMultiplier * (maulDamageRaw + 424f / 1f); //NOTE! This assumes 1 target. If Maul hits 2 targets, replace 1 with 2.
			float mangleThreatRaw = bearThreatMultiplier * mangleDamageRaw * (1 + stats.BonusMangleBearThreat);
			float swipeThreatRaw = bearThreatMultiplier * swipeDamageRaw * 1.5f;
			float faerieFireThreatRaw = bearThreatMultiplier * (faerieFireDamageRaw + 632f);
			float lacerateThreatRaw = bearThreatMultiplier * (lacerateDamageRaw + 1031f) / 2f;
			float lacerateDotThreat = bearThreatMultiplier * lacerateDotDamage / 2f;
			
			float meleeThreatAverage = bearThreatMultiplier * meleeDamageAverage;
			float maulThreatAverage = bearThreatMultiplier * (maulDamageAverage + (424f * (1 - chanceAvoided)) / 1f); //NOTE! This assumes 1 target. If Maul hits 2 targets, replace 1 with 2.
			float mangleThreatAverage = bearThreatMultiplier * mangleDamageAverage * (1 + stats.BonusMangleBearThreat);
			float swipeThreatAverage = bearThreatMultiplier * swipeDamageAverage * 1.5f;
			float faerieFireThreatAverage = bearThreatMultiplier * (faerieFireDamageAverage + (632f * (.9f))); //TODO: Assumes 10% spell miss rate
			float lacerateThreatAverage = bearThreatMultiplier * (lacerateDamageAverage + (1031f * (1 - chanceAvoided))) / 2f;

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
			
			float maulTPR = maulThreatAverage / maulRageAverage;
			float maulDPR = maulDamageAverage / maulRageAverage;
			float mangleTPR = mangleThreatAverage / mangleRageAverage;
			float mangleDPR = mangleDamageAverage / mangleRageAverage;
			float swipeTPR = swipeThreatAverage / swipeRageAverage;
			float swipeDPR = swipeDamageAverage / swipeRageAverage;
			float lacerateTPR = lacerateThreatAverage / lacerateRageAverage;
			float lacerateDPR = lacerateDamageAverage / lacerateRageAverage;

			BearRotationCalculator rotationCalculator = new BearRotationCalculator(meleeDamageAverage, maulDamageAverage, mangleDamageAverage, swipeDamageAverage,
				faerieFireDamageAverage, lacerateDamageAverage, lacerateDotDamage, meleeThreatAverage, maulThreatAverage, mangleThreatAverage, swipeThreatAverage,
				faerieFireThreatAverage, lacerateThreatAverage, lacerateDotThreat, 6f - stats.MangleCooldownReduction, attackSpeed);

			BearRotationCalculator.BearRotationCalculation rotationCalculationDPS, rotationCalculationTPS;
			rotationCalculationDPS = rotationCalculationTPS = new BearRotationCalculator.BearRotationCalculation();

			StringBuilder rotations = new StringBuilder();
			for (int useMaul = 0; useMaul < 3; useMaul++)
				for (int useMangle = 0; useMangle < 2; useMangle++)
					for (int useSwipe = 0; useSwipe < 2; useSwipe++)
						for (int useFaerieFire = 0; useFaerieFire < 2; useFaerieFire++)
							for (int useLacerate = 0; useLacerate < 2; useLacerate++)
							{
								bool?[] useMaulValues = new bool?[] {null, false, true};
								BearRotationCalculator.BearRotationCalculation rotationCalculation = 
									rotationCalculator.GetRotationCalculations(useMaulValues[useMaul],
									useMangle == 1, useSwipe == 1, useFaerieFire == 1, useLacerate == 1);
								rotations.AppendLine(rotationCalculation.Name + ": " + rotationCalculation.TPS + "TPS, " + rotationCalculation.DPS + "DPS");
								if (rotationCalculation.DPS > rotationCalculationDPS.DPS)
									rotationCalculationDPS = rotationCalculation;
								if (rotationCalculation.TPS > rotationCalculationTPS.TPS)
									rotationCalculationTPS = rotationCalculation;
							}

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

			calculatedStats.LacerateDotDamage = (float)Math.Round(lacerateDotDamage);
			calculatedStats.LacerateDotThreat = (float)Math.Round(lacerateDotThreat);

			calculatedStats.MaulTPR = maulTPR;
			calculatedStats.MaulDPR = maulDPR;
			calculatedStats.MangleTPR = mangleTPR;
			calculatedStats.MangleDPR = mangleDPR;
			calculatedStats.SwipeTPR = swipeTPR;
			calculatedStats.SwipeDPR = swipeDPR;
			calculatedStats.LacerateTPR = lacerateTPR;
			calculatedStats.LacerateDPR = lacerateDPR;


			//float bloodlustThreat = (1 - chanceAvoided) * stats.BloodlustProc / 4.0f * 5;

			//float averageDamage = 1 - chanceAvoided - chanceCrit + critMultiplier * chanceCrit;
			//float weaponDamage = (1 + stats.BonusDamageMultiplier) * (stats.WeaponDamage + (2.5f * +(768f + attackPower) / 14f));
			//float mangleUptime = 1 - (chanceAvoided * chanceAvoided);
			//float mangleMultipler = mangleUptime * 1.3f;
            
			//float MangleDamage = 1.1f * 1.15f * (weaponDamage + 155f + stats.BonusMangleBearDamage)*averageDamage*modArmor;
			//float MangleThreat = 1.45f *( 25 * chanceCrit+(1+stats.BonusMangleBearThreat)* MangleDamage+bloodlustThreat);
			//float SwipeDamage = 1.1f * (1 + stats.BonusSwipeDamageMultiplier) * (1 + stats.BonusDamageMultiplier) * ((.077f * stats.AttackPower + 92))*averageDamage*modArmor;
			//float SwipeThreat = 1.45f * ( 25 * chanceCrit+SwipeDamage+bloodlustThreat);
			//float LacerateDamage = 1.1f*(1+stats.BonusDamageMultiplier)*mangleMultipler*(31+stats.AttackPower/100+stats.BonusLacerateDamage)*averageDamage*modArmor ;
			//float LacerateThreat = 1.45f * ((285) * (1 - chanceAvoided) + 25 * chanceCrit + LacerateDamage+bloodlustThreat);
			//float LacerateDotDPS = 1.1f * (1 + stats.BonusDamageMultiplier) * (mangleMultipler * (155 + 5 * (stats.AttackPower / 100) + stats.BonusLacerateDamage))/3 ;
			//float LacerateDotTPS = 1.45f * (.2f * LacerateDotDPS);


			//float whiteDamage = 1.1f * weaponDamage*modArmor* (averageDamage - chanceGlance * glancingReduction);
			//float whiteThreat = 1.45f * (25 * chanceCrit+whiteDamage) +bloodlustThreat;

			//calculatedStats.MLS1_3_0DPS = (MangleDamage * 1  + LacerateDamage * 3  + SwipeDamage*0)/6+LacerateDotDPS + whiteDamage / attackSpeed;
			//calculatedStats.MLS1_3_0TPS = (MangleThreat * 1  + LacerateThreat * 3  + SwipeThreat*0)/6+LacerateDotTPS + whiteThreat / attackSpeed ;

			//calculatedStats.MLS1_0_3DPS = (MangleDamage * 1  + LacerateDamage * 0  + SwipeDamage*3)/6+LacerateDotDPS + whiteDamage / attackSpeed;
			//calculatedStats.MLS1_0_3TPS = (MangleThreat * 1  + LacerateThreat * 0  + SwipeThreat*3)/6+LacerateDotTPS + whiteThreat / attackSpeed ;

			////Missed lacerates are repeated until hit, removing some swipes
			//calculatedStats.MLS1_1_2DPS = (MangleDamage * 1  + LacerateDamage * 1/(1-chanceAvoided)  + SwipeDamage*2*(1-chanceAvoided))/6+LacerateDotDPS + whiteDamage / attackSpeed;
			//calculatedStats.MLS1_1_2TPS = (MangleThreat * 1  + LacerateThreat * 1/(1-chanceAvoided)  + SwipeThreat*2*(1-chanceAvoided))/6+LacerateDotTPS + whiteThreat / attackSpeed ;

			////This rotation will drop stacks if you miss
			////Calculate the average damage over time, given the probability of n stacks applied
			//float lacerateUptime = (chanceAvoided)*(
			//        (1-chanceAvoided)*1/5+
			//        (1-chanceAvoided)*(1-chanceAvoided)*2/5+
			//        (1-chanceAvoided)*(1-chanceAvoided)*(1-chanceAvoided)*3/5+
			//        (1-chanceAvoided)*(1-chanceAvoided)*(1-chanceAvoided)*(1-chanceAvoided)*4/5)+
			//        (1-chanceAvoided)*(1-chanceAvoided)*(1-chanceAvoided)*(1-chanceAvoided)*(1-chanceAvoided);

			//calculatedStats.MLS2_1_5DPS = (MangleDamage * 2  + LacerateDamage * 1  + SwipeDamage*5)/12+LacerateDotDPS*lacerateUptime + whiteDamage / attackSpeed;
			//calculatedStats.MLS2_1_5TPS = (MangleThreat * 2  + LacerateThreat * 1  + SwipeThreat*5)/12+LacerateDotTPS*lacerateUptime + whiteThreat / attackSpeed ;

			////get convert the glancing white attacks to maul hits
			//calculatedStats.MaulDPS = 1.1f* (179 * averageDamage*modArmor+chanceGlance * glancingReduction)/attackSpeed;
			//calculatedStats.MaulTPS = 1.45f*(calculatedStats.MaulDPS+ 322 / attackSpeed);

			//calculatedStats.MaxLacerate = (LacerateThreat) / 2 + LacerateDotTPS;
			//calculatedStats.MaxLacerateDPS = LacerateDamage / 2 + LacerateDotDPS;

			//calculatedStats.MinLacerate = (LacerateThreat) / 15 + LacerateDotTPS;
			//calculatedStats.MinLacerateDPS = LacerateDamage / 15 + LacerateDotDPS;

			//calculatedStats.SwipeDamage = SwipeDamage ;
			//calculatedStats.SwipeThreat = SwipeThreat ;

			//calculatedStats.MangleDamage = MangleDamage ;
			//calculatedStats.MangleThreat = MangleThreat ;

			//calculatedStats.WhiteDPS = whiteDamage / attackSpeed;
			//calculatedStats.WhiteTPS = (whiteThreat+bloodlustThreat) / attackSpeed;
                
			//calculatedStats.ThreatPoints = Math.Max(calculatedStats.MLS1_3_0TPS, 
			//    Math.Max(calculatedStats.MLS1_0_3TPS, Math.Max(calculatedStats.MLS1_1_2TPS,calculatedStats.MLS2_1_5TPS))) ;

			//if(calculatedStats.ThreatPoints == calculatedStats.MLS1_3_0TPS)
			//{
			//    calculatedStats.LimitedDPS = calculatedStats.MLS1_3_0DPS;
			//}
			//else if(calculatedStats.ThreatPoints == calculatedStats.MLS1_0_3TPS)
			//{
			//    calculatedStats.LimitedDPS = calculatedStats.MLS1_0_3DPS;
			//}
			//else if(calculatedStats.ThreatPoints == calculatedStats.MLS1_1_2TPS)
			//{
			//    calculatedStats.LimitedDPS = calculatedStats.MLS1_1_2DPS;
			//}
			//else
			//{
			//    calculatedStats.LimitedDPS = calculatedStats.MLS2_1_5DPS;
			//}
			//calculatedStats.UnlimitedDPS=calculatedStats.LimitedDPS+calculatedStats.MaulDPS;

			//calculatedStats.UnlimitedThreat = calculatedStats.ThreatPoints + calculatedStats.MaulTPS;

			//calculatedStats.ThreatScale = calcOpts.ThreatScale;
			//calculatedStats.ThreatPoints *= calculatedStats.ThreatScale;
            

			//calculatedStats.UnlimitedThreat *= calculatedStats.ThreatScale;


			//calculatedStats.OverallPoints = calculatedStats.MitigationPoints + calculatedStats.SurvivalPoints + calculatedStats.ThreatPoints;
            

        }

		public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
			Stats statsRace = character.Race == Character.CharacterRace.NightElf ?
				new Stats() { 
                    Health = 7237f, 
                    Strength = 86f, 
					Agility = 87f,
                    Stamina = 96f,
					AttackPower = 120f,
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
					Agility = 75f,
					Stamina = 100f,
					AttackPower = 190,
                    NatureResistance = 10,
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
			Stats statsEnchants = GetEnchantsStats(character);
			Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
			Stats statsTalents = new Stats()
			{
				PhysicalCrit = 0.02f * talents.SharpenedClaws + (character.ActiveBuffsContains("Leader of the Pack") ?
					0 : 0.05f * talents.LeaderOfThePack),
				Dodge = 0.02f * talents.FeralSwiftness + 0.02f * talents.NaturalReaction,
				BonusStaminaMultiplier = (1 + 0.04f * talents.HeartOfTheWild) * (1 + 0.02f * talents.SurvivalOfTheFittest) - 1,
				BonusAgilityMultiplier = 0.02f * talents.SurvivalOfTheFittest,
				BonusStrengthMultiplier = 0.02f * talents.SurvivalOfTheFittest,
				CritChanceReduction = 0.02f * talents.SurvivalOfTheFittest,
				BonusAttackPowerMultiplier = 0.02f * talents.ProtectorOfThePack,
				BonusPhysicalDamageMultiplier = (1 + 0.02f * talents.Naturalist) * (1 + 0.02f * talents.MasterShapeshifter) - 1,
				BonusMangleDamageMultiplier = 0.1f * talents.SavageFury,
				BonusMaulDamageMultiplier = (1 + 0.1f * talents.SavageFury) * (1 + 0.04f * talents.RendAndTear) - 1f,
				BonusEnrageDamageMultiplier = 0.05f * talents.KingOfTheJungle,
				MangleCooldownReduction = (0.5f * talents.ImprovedMangle),
				BonusRageOnCrit = (2.5f * talents.PrimalFury),
				Expertise = 5 * talents.PrimalPrecision,
				AttackPower = (character.Level / 2f) * talents.PredatoryStrikes,
				BonusSwipeDamageMultiplier = 0.1f * talents.FeralInstinct,
                DamageTakenMultiplier = -0.04f * talents.ProtectorOfThePack,
				BonusBleedDamageMultiplier = (character.ActiveBuffsContains("Mangle") ? 0 : 0.3f * talents.Mangle),
				BaseArmorMultiplier = 4.7f * (1f + 0.1f * talents.ThickHide / 3f) * (1f + 0.22f * talents.SurvivalOfTheFittest) - 1f,
			};
			
			Stats statsTotal = statsRace + statsItems + statsEnchants + statsBuffs + statsTalents;

			Stats statsWeapon = character.MainHand == null ? new Stats() : character.MainHand.GetTotalStats(character).Clone();
			statsWeapon.Strength *= (1 + statsTotal.BonusStrengthMultiplier);
			statsWeapon.AttackPower += statsWeapon.Strength * 2;

			statsTotal.Stamina *= (1 + statsTotal.BonusStaminaMultiplier);
			statsTotal.Strength *= (1 + statsTotal.BonusStrengthMultiplier);
			statsTotal.Agility *= (1 + statsTotal.BonusAgilityMultiplier);
			statsTotal.AttackPower += statsTotal.Strength * 2;
			statsTotal.AttackPower += statsWeapon.AttackPower * 0.2f * (talents.PredatoryStrikes / 3f);
			statsTotal.AttackPower *= (1 + statsTotal.BonusAttackPowerMultiplier);
			statsTotal.Health += (float)Math.Floor(statsTotal.Stamina) * 10f;
			statsTotal.Armor *= 1f + statsTotal.BaseArmorMultiplier;
			statsTotal.Armor += 2f * statsTotal.Agility + statsTotal.BonusArmor;
			statsTotal.Armor *= 1f + statsTotal.BonusArmorMultiplier;
			statsTotal.NatureResistance += statsTotal.NatureResistanceBuff + statsTotal.AllResist;
			statsTotal.FireResistance += statsTotal.FireResistanceBuff + statsTotal.AllResist;
			statsTotal.FrostResistance += statsTotal.FrostResistanceBuff + statsTotal.AllResist;
			statsTotal.ShadowResistance += statsTotal.ShadowResistanceBuff + statsTotal.AllResist;
			statsTotal.ArcaneResistance += statsTotal.ArcaneResistanceBuff + statsTotal.AllResist;

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

						float crits = 2f + (0.2f * (currentCalculationsBear.TargetLevel - character.Level)) - currentCalculationsBear.CappedCritReduction;
						//float crushes = currentCalculationsBear.TargetLevel == 73 ? Math.Max(Math.Min(100f - (crits + (currentCalculationsBear.AvoidancePreDR)), 15f) - currentCalculationsBear.BasicStats.CritChanceReduction, 0f) : 0f;
						float hits = Math.Max(100f - (Math.Max(0f, crits) + /*Math.Max(crushes, 0)*/ + (currentCalculationsBear.AvoidancePreDR)), 0f);
						
						calcMiss.OverallPoints = calcMiss.MitigationPoints = currentCalculationsBear.Miss;
						calcDodge.OverallPoints = calcDodge.MitigationPoints = currentCalculationsBear.Dodge;
						calcCrit.OverallPoints = calcCrit.SurvivalPoints = crits;
						//calcCrush.OverallPoints = calcCrush.SurvivalPoints = crushes;
						calcHit.OverallPoints = calcHit.SurvivalPoints = hits;
					}
					return new ComparisonCalculationBase[] { calcMiss, calcDodge, calcCrit, /*calcCrush,*/ calcHit };
				
				case "Relative Stat Values":
					CharacterCalculationsBear calcBaseValue = GetCharacterCalculations(character) as CharacterCalculationsBear;
					//CharacterCalculationsBear calcAgiValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 10 } }) as CharacterCalculationsBear;
					//CharacterCalculationsBear calcACValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Armor = 10 } }) as CharacterCalculationsBear;
					//CharacterCalculationsBear calcStaValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = 10 } }) as CharacterCalculationsBear;
					//CharacterCalculationsBear calcDefValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DefenseRating = 1 } }) as CharacterCalculationsBear;
					CharacterCalculationsBear calcDodgeValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DodgeRating = 1 } }) as CharacterCalculationsBear;
					CharacterCalculationsBear calcHealthValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Health = 1 } }) as CharacterCalculationsBear;
					CharacterCalculationsBear calcResilValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Resilience = 1 } }) as CharacterCalculationsBear;
					//CharacterCalculationsBear calcStrengthValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = 1 } }) as CharacterCalculationsBear;
					CharacterCalculationsBear calcAPValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { AttackPower = 1 } }) as CharacterCalculationsBear;
					CharacterCalculationsBear calcExpertiseValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { ExpertiseRating = 1 } }) as CharacterCalculationsBear;
					CharacterCalculationsBear calcHitValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 1 } }) as CharacterCalculationsBear;
					CharacterCalculationsBear calcPenValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { ArmorPenetration = 1 } }) as CharacterCalculationsBear;
					CharacterCalculationsBear calcHasteValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 1 } }) as CharacterCalculationsBear;
					CharacterCalculationsBear calcDamageValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { WeaponDamage = 1 } }) as CharacterCalculationsBear;
					CharacterCalculationsBear calcCritValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 1 } }) as CharacterCalculationsBear;
				
					//Differential Calculations for Agi
					CharacterCalculationsBear calcAtAdd = calcBaseValue;
					float agiToAdd = 0f;
					while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && agiToAdd < 2)
					{
						agiToAdd += 0.01f;
						calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToAdd } }) as CharacterCalculationsBear;
					}

					CharacterCalculationsBear calcAtSubtract = calcBaseValue;
					float agiToSubtract = 0f;
					while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && agiToSubtract > -2)
					{
						agiToSubtract -= 0.01f;
						calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToSubtract } }) as CharacterCalculationsBear;
					}
					agiToSubtract += 0.01f;

					ComparisonCalculationBear comparisonAgi = new ComparisonCalculationBear() { Name = "Agility", 
                        OverallPoints =     (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (agiToAdd - agiToSubtract),
						MitigationPoints =  (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (agiToAdd - agiToSubtract), 
                        SurvivalPoints =    (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (agiToAdd - agiToSubtract),
                        ThreatPoints =      (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (agiToAdd - agiToSubtract)};


					//Differential Calculations for Str
					calcAtAdd = calcBaseValue;
					float strToAdd = 0f;
					while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && strToAdd < 2)
					{
						strToAdd += 0.01f;
						calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToAdd } }) as CharacterCalculationsBear;
					}

					calcAtSubtract = calcBaseValue;
					float strToSubtract = 0f;
					while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && strToSubtract > -2)
					{
						strToSubtract -= 0.01f;
						calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToSubtract } }) as CharacterCalculationsBear;
					}
					strToSubtract += 0.01f;

					ComparisonCalculationBear comparisonStr = new ComparisonCalculationBear()
					{
						Name = "Strength",
						OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (strToAdd - strToSubtract),
						MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (strToAdd - strToSubtract),
						SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (strToAdd - strToSubtract),
						ThreatPoints = (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (strToAdd - strToSubtract)
					};


					//Differential Calculations for Def
					calcAtAdd = calcBaseValue;
					float defToAdd = 0f;
					while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && defToAdd < 20)
					{
						defToAdd += 0.01f;
						calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DefenseRating = defToAdd } }) as CharacterCalculationsBear;
					}

					calcAtSubtract = calcBaseValue;
					float defToSubtract = 0f;
					while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && defToSubtract > -20)
					{
						defToSubtract -= 0.01f;
						calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DefenseRating = defToSubtract } }) as CharacterCalculationsBear;
					}
					defToSubtract += 0.01f;

					ComparisonCalculationBear comparisonDef = new ComparisonCalculationBear()
					{
						Name = "Defense Rating",
						OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (defToAdd - defToSubtract),
						MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (defToAdd - defToSubtract),
						SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (defToAdd - defToSubtract),
						ThreatPoints = (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (defToAdd - defToSubtract)
					};


					//Differential Calculations for AC
					calcAtAdd = calcBaseValue;
					float acToAdd = 0f;
					while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && acToAdd < 2)
					{
						acToAdd += 0.01f;
						calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Armor = acToAdd } }) as CharacterCalculationsBear;
					}

					calcAtSubtract = calcBaseValue;
					float acToSubtract = 0f;
					while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && acToSubtract > -2)
					{
						acToSubtract -= 0.01f;
						calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Armor = acToSubtract } }) as CharacterCalculationsBear;
					}
					acToSubtract += 0.01f;

					ComparisonCalculationBear comparisonAC = new ComparisonCalculationBear() { Name = "Armor", 
                        OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (acToAdd - acToSubtract),
						MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (acToAdd - acToSubtract), 
                        SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (acToAdd - acToSubtract), 
                        ThreatPoints = (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (acToAdd - acToSubtract)
                    };


					//Differential Calculations for Sta
					calcAtAdd = calcBaseValue;
					float staToAdd = 0f;
					while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && staToAdd < 2)
					{
						staToAdd += 0.01f;
						calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = staToAdd } }) as CharacterCalculationsBear;
					}

					calcAtSubtract = calcBaseValue;
					float staToSubtract = 0f;
					while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && staToSubtract > -2)
					{
						staToSubtract -= 0.01f;
						calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = staToSubtract } }) as CharacterCalculationsBear;
					}
					staToSubtract += 0.01f;

					ComparisonCalculationBear comparisonSta = new ComparisonCalculationBear() { Name = "Stamina", 
                        OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (staToAdd - staToSubtract),
						MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (staToAdd - staToSubtract), 
                        SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (staToAdd - staToSubtract), 
                        ThreatPoints = (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (staToAdd - staToSubtract)};

					return new ComparisonCalculationBase[] { 
						comparisonAgi,
						comparisonAC,
						comparisonSta,
						comparisonDef,
						comparisonStr,
						new ComparisonCalculationBear() { Name = "Attack Power", 
                            OverallPoints = (calcAPValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcAPValue.MitigationPoints - calcBaseValue.MitigationPoints), 
                            SurvivalPoints = (calcAPValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
                            ThreatPoints = (calcAPValue.ThreatPoints - calcBaseValue.ThreatPoints)},
						new ComparisonCalculationBear() { Name = "Hit Rating", 
                            OverallPoints = (calcHitValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcHitValue.MitigationPoints - calcBaseValue.MitigationPoints), 
                            SurvivalPoints = (calcHitValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
                            ThreatPoints = (calcHitValue.ThreatPoints - calcBaseValue.ThreatPoints)},
						new ComparisonCalculationBear() { Name = "Crit Rating", 
                            OverallPoints = (calcCritValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcCritValue.MitigationPoints - calcBaseValue.MitigationPoints), 
                            SurvivalPoints = (calcCritValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
                            ThreatPoints = (calcCritValue.ThreatPoints - calcBaseValue.ThreatPoints)},
						new ComparisonCalculationBear() { Name = "Weapon Damage", 
                            OverallPoints = (calcDamageValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcDamageValue.MitigationPoints - calcBaseValue.MitigationPoints), 
                            SurvivalPoints = (calcDamageValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
                            ThreatPoints = (calcDamageValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationBear() { Name = "Haste Rating", 
                            OverallPoints = (calcHasteValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcHasteValue.MitigationPoints - calcBaseValue.MitigationPoints), 
                            SurvivalPoints = (calcHasteValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
                            ThreatPoints = (calcHasteValue.ThreatPoints - calcBaseValue.ThreatPoints)},
						new ComparisonCalculationBear() { Name = "Expertise Rating", 
                            OverallPoints = (calcExpertiseValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcExpertiseValue.MitigationPoints - calcBaseValue.MitigationPoints), 
                            SurvivalPoints = (calcExpertiseValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
                            ThreatPoints = (calcExpertiseValue.ThreatPoints - calcBaseValue.ThreatPoints)},

						new ComparisonCalculationBear() { Name = "Dodge Rating", 
                            OverallPoints = (calcDodgeValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcDodgeValue.MitigationPoints - calcBaseValue.MitigationPoints), 
                            SurvivalPoints = (calcDodgeValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
                            ThreatPoints = (calcDodgeValue.ThreatPoints - calcBaseValue.ThreatPoints)},
						new ComparisonCalculationBear() { Name = "Health", 
                            OverallPoints = (calcHealthValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcHealthValue.MitigationPoints - calcBaseValue.MitigationPoints), 
                            SurvivalPoints = (calcHealthValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
                            ThreatPoints = (calcHealthValue.ThreatPoints - calcBaseValue.ThreatPoints)},
						new ComparisonCalculationBear() { Name = "Resilience", 
                            OverallPoints = (calcResilValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcResilValue.MitigationPoints - calcBaseValue.MitigationPoints), 
                            SurvivalPoints = (calcResilValue.SurvivalPoints - calcBaseValue.SurvivalPoints), 
                            ThreatPoints = (calcResilValue.ThreatPoints - calcBaseValue.ThreatPoints)},
					};

				case "Rotation DPS":
					CharacterCalculationsBear calcsDPS = GetCharacterCalculations(character) as CharacterCalculationsBear;
					List<ComparisonCalculationBase> comparisonsDPS = new List<ComparisonCalculationBase>();

					BearRotationCalculator rotationCalculatorDPS = new BearRotationCalculator(calcsDPS.MeleeDamageAverage,
						calcsDPS.MaulDamageAverage, calcsDPS.MangleDamageAverage, calcsDPS.SwipeDamageAverage,
						calcsDPS.FaerieFireDamageAverage, calcsDPS.LacerateDamageAverage, calcsDPS.LacerateDotDamage, calcsDPS.MeleeThreatAverage,
						calcsDPS.MaulThreatAverage, calcsDPS.MangleThreatAverage, calcsDPS.SwipeThreatAverage,
						calcsDPS.FaerieFireThreatAverage, calcsDPS.LacerateThreatAverage, calcsDPS.LacerateDotThreat,
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
											Character = character,
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
						calcsTPS.FaerieFireDamageAverage, calcsTPS.LacerateDamageAverage, calcsTPS.LacerateDotDamage, calcsTPS.MeleeThreatAverage,
						calcsTPS.MaulThreatAverage, calcsTPS.MangleThreatAverage, calcsTPS.SwipeThreatAverage,
						calcsTPS.FaerieFireThreatAverage, calcsTPS.LacerateThreatAverage, calcsTPS.LacerateDotThreat,
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
											Character = character,
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

		public override bool IsItemRelevant(Item item)
		{
			if (item.Slot == Item.ItemSlot.OffHand ||
				(item.Slot == Item.ItemSlot.Ranged && item.Type != Item.ItemType.Idol) ||
				(item.Slot == Item.ItemSlot.TwoHand && item.Stats.AttackPower < 100))
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

                Strength = stats.Strength,
                AttackPower = stats.AttackPower,
                CritRating = stats.CritRating,
                HitRating = stats.HitRating,
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
				ArmorPenetrationRating = stats.ArmorPenetrationRating
			};
		}

		public override bool HasRelevantStats(Stats stats)
		{
			return (stats.Agility + stats.Armor + stats.BonusArmor + stats.BonusAgilityMultiplier + stats.BonusArmorMultiplier +
				stats.BonusStaminaMultiplier + stats.DefenseRating + stats.DodgeRating + stats.Health +
				stats.Miss + stats.Resilience + stats.Stamina + stats.TerrorProc + stats.AllResist +
				stats.ArcaneResistance + stats.NatureResistance + stats.FireResistance +
				stats.FrostResistance + stats.ShadowResistance + stats.ArcaneResistanceBuff +
				stats.NatureResistanceBuff + stats.FireResistanceBuff +
				stats.FrostResistanceBuff + stats.ShadowResistanceBuff + stats.CritChanceReduction +
				stats.ArmorPenetrationRating + stats.PhysicalHaste
                 + stats.Strength + stats.AttackPower + stats.CritRating + stats.HitRating + stats.HasteRating
                 + stats.ExpertiseRating + stats.ArmorPenetration + stats.WeaponDamage + stats.BonusCritMultiplier
				 + stats.BonusRipDuration
                 + stats.TerrorProc+stats.BonusMangleBearThreat + stats.BonusLacerateDamageMultiplier + stats.BonusSwipeDamageMultiplier
                 + stats.BloodlustProc + stats.BonusMangleBearDamage + stats.BonusAttackPowerMultiplier + stats.BonusDamageMultiplier
                 + stats.DamageTakenMultiplier + stats.ArmorPenetrationRating) != 0;
		}
    }

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
		public float Mitigation { get; set; }
		public float AvoidancePreDR { get; set; }
		public float AvoidancePostDR { get; set; }
		public float TotalMitigation { get; set; }
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
		public float LacerateDotThreat { get; set; }

		public float MeleeThreatAverage { get; set; }
		public float MaulThreatAverage { get; set; }
		public float MangleThreatAverage { get; set; }
		public float SwipeThreatAverage { get; set; }
		public float FaerieFireThreatAverage { get; set; }
		public float LacerateThreatAverage { get; set; }

		public float MeleeDamageRaw { get; set; }
		public float MaulDamageRaw { get; set; }
		public float MangleDamageRaw { get; set; }
		public float SwipeDamageRaw { get; set; }
		public float FaerieFireDamageRaw { get; set; }
		public float LacerateDamageRaw { get; set; }
		public float LacerateDotDamage { get; set; }

		public float MeleeDamageAverage { get; set; }
		public float MaulDamageAverage { get; set; }
		public float MangleDamageAverage { get; set; }
		public float SwipeDamageAverage { get; set; }
		public float FaerieFireDamageAverage { get; set; }
		public float LacerateDamageAverage { get; set; }

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
			int armorCap = (int)Math.Ceiling((1402.5f * TargetLevel) - 66502.5f);
			float levelDifference = 0.2f * (TargetLevel - 80);
			float targetCritReduction = 5f + levelDifference;
			float currentCritReduction = ((float)Math.Floor(BasicStats.DefenseRating / (123f / 52f)) * 0.04f) 
				+ BasicStats.Resilience / (2050f / 52f) + BasicStats.CritChanceReduction;			
			int defToCap = 0, resToCap = 0;
			if (CritReduction < targetCritReduction)
			{
				while (((float)Math.Floor((BasicStats.DefenseRating + defToCap) / (123f / 52f)) * 0.04f)
				+ BasicStats.Resilience / (2050f / 52f) + BasicStats.CritChanceReduction < targetCritReduction)
					defToCap++;
				while (((float)Math.Floor(BasicStats.DefenseRating / (123f / 52f)) * 0.04f)
				+ (BasicStats.Resilience + resToCap) / (2050f / 52f) + BasicStats.CritChanceReduction < targetCritReduction)
					resToCap++;
			}
			else if (CritReduction > targetCritReduction)
			{
				while (((float)Math.Floor((BasicStats.DefenseRating + defToCap) / (123f / 52f)) * 0.04f)
				+ BasicStats.Resilience / (2050f / 52f) + BasicStats.CritChanceReduction > targetCritReduction)
					defToCap--;
				while (((float)Math.Floor(BasicStats.DefenseRating / (123f / 52f)) * 0.04f)
				+ (BasicStats.Resilience + resToCap) / (2050f / 52f) + BasicStats.CritChanceReduction > targetCritReduction)
					resToCap--;
				defToCap++;
				resToCap++;
			}

            dictValues["Nature Resist"] = (BasicStats.NatureResistance+BasicStats.AllResist).ToString();
            dictValues["Arcane Resist"] = (BasicStats.ArcaneResistance+BasicStats.AllResist).ToString();
            dictValues["Frost Resist"] = (BasicStats.FrostResistance+BasicStats.AllResist).ToString();
            dictValues["Fire Resist"] = (BasicStats.FireResistance+BasicStats.AllResist).ToString();
            dictValues["Shadow Resist"] = (BasicStats.ShadowResistance + BasicStats.AllResist).ToString();
			dictValues.Add("Health", BasicStats.Health.ToString());
			dictValues.Add("Agility", BasicStats.Agility.ToString());
			dictValues.Add("Armor", BasicStats.Armor.ToString());
			dictValues.Add("Stamina", BasicStats.Stamina.ToString());
			dictValues.Add("Dodge Rating", BasicStats.DodgeRating.ToString());
			dictValues.Add("Defense Rating", BasicStats.DefenseRating.ToString());
			dictValues.Add("Resilience", BasicStats.Resilience.ToString());
			dictValues.Add("Dodge", Dodge.ToString() + "%");
			dictValues.Add("Miss", Miss.ToString() + "%");
			if (BasicStats.Armor == armorCap)
				dictValues.Add("Mitigation", Mitigation.ToString()
					+ string.Format("%*Exactly at the armor cap against level {0} mobs.", TargetLevel));
			else if (BasicStats.Armor > armorCap)
				dictValues.Add("Mitigation", Mitigation.ToString()
					+ string.Format("%*Over the armor cap by {0} armor.", BasicStats.Armor - armorCap));
			else
				dictValues.Add("Mitigation", Mitigation.ToString()
					+ string.Format("%*Short of the armor cap by {0} armor.", armorCap - BasicStats.Armor));
			dictValues.Add("Avoidance PreDR", AvoidancePreDR.ToString() + "%");
			dictValues.Add("Avoidance PostDR", AvoidancePostDR.ToString() + "%");
			dictValues.Add("Total Mitigation", TotalMitigation.ToString() + "%");
			dictValues.Add("Damage Taken", DamageTaken.ToString() + "%");
			dictValues.Add("Chance to be Crit", ((5f + levelDifference) - CritReduction).ToString() + "%");
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
			dictValues["Hit Rating"] = BasicStats.HitRating.ToString();
			dictValues["Expertise Rating"] = BasicStats.ExpertiseRating.ToString();
			dictValues["Haste Rating"] = string.Format("{0}*{1} Attack Speed", BasicStats.HasteRating, Math.Round(AttackSpeed * 1000f)/1000f);
			dictValues["Armor Penetration Rating"] = BasicStats.ArmorPenetrationRating.ToString();

			dictValues["Missed Attacks"] = String.Format("{0}%*Missed={1}% Dodged={2}% Parried={3}%", AvoidedAttacks, MissedAttacks, DodgedAttacks, ParriedAttacks);
           
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
			dictValues["Lacerate DOT Tick"] = String.Format("{0} Dmg, {1} Threat*Per Tick: {0} Damage, {1} Threat", LacerateDotDamage, LacerateDotThreat);
			
			return dictValues;
		}

		public override float GetOptimizableCalculationValue(string calculation)
		{
			switch (calculation)
			{
				case "Health": return BasicStats.Health;
				case "Mitigation % from Armor": return Mitigation;
				case "Avoidance %": return AvoidancePostDR;
				case "% Chance to be Crit": return ((5f + (0.2f * (TargetLevel - 80))) - CritReduction);
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
