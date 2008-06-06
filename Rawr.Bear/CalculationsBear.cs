using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
	[System.ComponentModel.DisplayName("Bear|Ability_Racial_BearForm")]
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
					"Basic Stats:Health",
					"Basic Stats:Armor",
					"Basic Stats:Agility",
					"Basic Stats:Stamina",
					"Basic Stats:Strength",
					"Basic Stats:Attack Power",
					"Basic Stats:Hit Rating",
					"Basic Stats:Expertise",
					"Basic Stats:Haste Rating",
					"Basic Stats:Armor Penetration",
					"Basic Stats:Crit Rating",
					"Basic Stats:Weapon Damage",
					"Basic Stats:Dodge Rating",
					"Basic Stats:Defense Rating",
					"Basic Stats:Resilience",
					"Basic Stats:Nature Resist",
					"Basic Stats:Fire Resist",
					"Basic Stats:Frost Resist",
					"Basic Stats:Shadow Resist",
					"Basic Stats:Arcane Resist",
                    "Complex Stats:Limited Threat*Normal single target roation of 3 lacerates every 6 second Mangle cooldown.",
                    "Complex Stats:Unlimited Threat*Limited threat with Maul threat added in.",
                    "Complex Stats:Lacerate Max TPS*Normal single target roation of 3 lacerates every 6 second Mangle cooldown.  Assumes a 5 stack.",
                    "Complex Stats:Lacerate Min TPS*Minimal Lacerates to maintain a 5 stack.  Does not take into account losing the 5 stack due to a miss",
                    "Complex Stats:Swipe TPS*Swipe TPS on a single target, 3 swipes every 6 second Mangle cooldown",
                    "Complex Stats:Mangle TPS",
                    "Complex Stats:White TPS",
                    "Complex Stats:Missed Attacks",
					"Complex Stats:Dodge",
					"Complex Stats:Miss",
					"Complex Stats:Mitigation",
					"Complex Stats:Dodge + Miss",
					"Complex Stats:Total Mitigation",
					"Complex Stats:Damage Taken",
					"Complex Stats:Chance to be Crit",
					@"Complex Stats:Overall Points*Overall Points are a sum of Mitigation and Survival Points. 
Overall is typically, but not always, the best way to rate gear. 
For specific encounters, closer attention to Mitigation and 
Survival Points individually may be important.",
					@"Complex Stats:Mitigation Points*Mitigation Points represent the amount of damage you mitigate, 
on average, through armor mitigation and avoidance. It is directly 
relational to your Damage Taken. Ideally, you want to maximize 
Mitigation Points, while maintaining 'enough' Survival Points 
(see Survival Points). If you find yourself dying due to healers 
running OOM, or being too busy healing you and letting other 
raid members die, then focus on Mitigation Points.",
					@"Complex Stats:Survival Points*Survival Points represents the total raw physical damage 
(pre-mitigation) you can take before dying. Unlike 
Mitigation Points, you should not attempt to maximize this, 
but rather get 'enough' of it, and then focus on Mitigation. 
'Enough' can vary greatly by fight and by your healers, but 
keeping it roughly even with Mitigation Points is a good 
way to maintain 'enough' as you progress. If you find that 
you are being killed by burst damage, focus on Survival Points.",
                    "Complex Stats:Nature Survival",
                    "Complex Stats:Fire Survival",
                    "Complex Stats:Frost Survival",
                    "Complex Stats:Shadow Survival",
                    "Complex Stats:Arcane Survival",
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
					//"Agi Test"
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
			Stats stats = GetCharacterStats(character, additionalItem);
			float levelDifference = (targetLevel - 70f) * 0.2f;
			CharacterCalculationsBear calculatedStats = new CharacterCalculationsBear();
			calculatedStats.BasicStats = stats;
			calculatedStats.Race = character.Race;
			calculatedStats.ActiveBuffs = new List<Buff>(character.ActiveBuffs);
			calculatedStats.TargetLevel = targetLevel;
			float defSkill = (float)Math.Floor(stats.DefenseRating / (123f / 52f));
			calculatedStats.Miss = 5f + (defSkill * 0.04f) + stats.Miss - levelDifference;
			calculatedStats.Dodge = Math.Min(100f - calculatedStats.Miss, stats.Agility / 14.7059f + (stats.DodgeRating / (984f / 52f)) + (defSkill * 0.04f) - levelDifference);
			calculatedStats.Mitigation = (stats.Armor / (stats.Armor - 22167.5f + (467.5f * targetLevel))) * 100f; //(stats.Armor / (stats.Armor + 11959.5f)) * 100f; for only 73s
			calculatedStats.CappedMitigation = Math.Min(75f, calculatedStats.Mitigation);
			calculatedStats.DodgePlusMiss = calculatedStats.Dodge + calculatedStats.Miss;
			calculatedStats.CritReduction = (defSkill * 0.04f) + stats.Resilience / (2050f / 52f);
			calculatedStats.CappedCritReduction = Math.Min(2f + levelDifference, calculatedStats.CritReduction);
			//Out of 100 attacks, you'll take...
			float crits = Math.Min(100f - calculatedStats.DodgePlusMiss, 2f + (0.2f * levelDifference) - calculatedStats.CappedCritReduction);
			float crushes = targetLevel == 73 ? Math.Max(Math.Min(100f - (crits + (calculatedStats.DodgePlusMiss)), 15f) - stats.CrushChanceReduction, 0f) : 0f;
			float hits = Math.Max(100f - (Math.Max(0f, crits) + Math.Max(crushes, 0) + (calculatedStats.DodgePlusMiss)), 0f);
			//Apply armor and multipliers for each attack type...
			crits *= (100f - calculatedStats.CappedMitigation) * .02f;
			crushes *= (100f - calculatedStats.CappedMitigation) * .015f;
			hits *= (100f - calculatedStats.CappedMitigation) * .01f;
			calculatedStats.DamageTaken = hits + crushes + crits;
			calculatedStats.TotalMitigation = 100f - calculatedStats.DamageTaken;

			calculatedStats.SurvivalPoints = (stats.Health / (1f - (calculatedStats.CappedMitigation / 100f))); // / (buffs.ShadowEmbrace ? 0.95f : 1f);
			calculatedStats.MitigationPoints = (7000f * (1f / (calculatedStats.DamageTaken / 100f))); // / (buffs.ShadowEmbrace ? 0.95f : 1f);

            float cappedResist = targetLevel * 5;

            calculatedStats.NatureSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.NatureResistance + stats.AllResist) / cappedResist) * .75)));
            calculatedStats.FrostSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.FrostResistance + stats.AllResist) / cappedResist) * .75)));
            calculatedStats.FireSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.FireResistance + stats.AllResist) / cappedResist) * .75)));
            calculatedStats.ShadowSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.ShadowResistance + stats.AllResist) / cappedResist) * .75)));
            calculatedStats.ArcaneSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.ArcaneResistance + stats.AllResist) / cappedResist) * .75)));

            float targetArmor = 7700;
            float baseArmor = Math.Max(0f, targetArmor - stats.ArmorPenetration);
            float modArmor = 1-(baseArmor / (baseArmor + 10557.5f));

            float critMultiplier = 2 * (1 + stats.BonusCritMultiplier);
            float attackPower = stats.AttackPower + ((1 + stats.BonusAttackPowerMultiplier));


            float hasteBonus = stats.HasteRating / 15.76f / 100f;
            float attackSpeed = (2.5f ) / (1f + hasteBonus);


            float hitBonus = stats.HitRating * 52f / 82f / 1000f;
            float expertiseBonus = stats.ExpertiseRating * 52f / 82f / 2.5f * 0.0025f;
            
            float chanceDodge = Math.Max(0f, 0.065f+.005f*(targetLevel -73) - expertiseBonus);
            float chanceParry = Math.Max(0f, 0.1425f - expertiseBonus); // Parry for lower levels?
            float chanceBlock = 0;//ha!
            float chanceMiss = Math.Max(0f, 0.09f - hitBonus);
            if ((targetLevel - 70f) < 3)
            {
                chanceMiss = Math.Max(0f, 0.05f + 0.005f * (targetLevel - 70f) - hitBonus);
            }

            float chanceAvoided = chanceMiss + chanceDodge + chanceParry;

            float chanceCrit = Math.Min(0.75f, (stats.CritRating / 22.08f + ((stats.Agility + (1-chanceAvoided)*stats.TerrorProc*2.0f/3.0f) / 25f)) / 100f);

            calculatedStats.EnemyBlockedAttacks = chanceBlock * 100;
            calculatedStats.EnemyDodgedAttacks = chanceDodge * 100;
            calculatedStats.EnemyParriedAttacks = chanceParry * 100;
            calculatedStats.EnemyMissedAttacks = chanceMiss * 100;

            calculatedStats.DodgedAttacks = chanceDodge * 100f;
            calculatedStats.MissedAttacks = calculatedStats.EnemyAvoidedAttacks - calculatedStats.DodgedAttacks;


            float critRageTPS = 5*chanceCrit*(1 / attackSpeed + 4 / 6.0f)*5;


            float weaponDamage = (1+stats.BonusPhysicalDamageMultiplier) *(stats.WeaponDamage+(2.5f* + (768f + attackPower) / 14f));

            float whiteDPS = weaponDamage / attackSpeed ;

            float mangleDPS =  1.15f * (weaponDamage + 155f + stats.BonusMangleBearDamage) / 6;

            float LacerateDPS = (1+stats.BonusPhysicalDamageMultiplier)*1.3f *(31+stats.AttackPower/100+stats.BonusLacerateDamage) /2;
            float LacerateBaseTPS = (285) * (1 - chanceAvoided)*.5f;
            float lacerateDotDPS = (1+stats.BonusPhysicalDamageMultiplier) *(1.3f*(155 + 5*(stats.AttackPower / 100) + stats.BonusLacerateDamage))/3;

            float BloodlustTPS = 1 / attackSpeed*(1-chanceAvoided) * stats.BloodlustProc / 4.0f * 5;

            calculatedStats.ThreatScale = calcOpts.ThreatScale;
   

            float averageDamage = 1 - chanceAvoided +  (1 + stats.BonusCritMultiplier) * chanceCrit;

            calculatedStats.MaxLacerateDPS =(
                1.1f * ((LacerateDPS) * modArmor * averageDamage + lacerateDotDPS));
            calculatedStats.MaxLacerate = 1.45f * (LacerateBaseTPS +  (5*chanceCrit*(3 / 6.0f))*5 +
                calculatedStats.MaxLacerateDPS - 1.1f* 0.8f * lacerateDotDPS);


            calculatedStats.MinLacerateDPS = (1.1f * ((2 * LacerateDPS / 15.0f) * averageDamage + lacerateDotDPS));

            calculatedStats.MinLacerate = 1.45f * (2*LacerateBaseTPS/15 + (5 * chanceCrit * (1 / 15.0f)) * 5 +
                calculatedStats.MinLacerateDPS - 1.1f * 0.8f * lacerateDotDPS);

            calculatedStats.SwipeDPS = ((1 + stats.BonusSwipeDamageMultiplier) * (1 + stats.BonusPhysicalDamageMultiplier) * (1.1f * (.077f * stats.AttackPower + 92) * averageDamage * modArmor) + 5 * chanceCrit * 5) / 2.0f;
            calculatedStats.SwipeTPS = (1.45f * calculatedStats.SwipeDPS);

            calculatedStats.MangleDPS = (
                1.1f * (((mangleDPS) * modArmor) * averageDamage));
            calculatedStats.MangleTPS = 1.45f * ((5 * chanceCrit * (1 / 6.0f)) * 5 +
                calculatedStats.MangleDPS*(1+stats.BonusMangleBearThreat));

            // add in glancing
            calculatedStats.WhiteDPS = 
               1.1f * (((whiteDPS) * modArmor) * averageDamage); 
            
            calculatedStats.WhiteTPS = 1.45f * (5 * chanceCrit * (1 / attackSpeed) * 5 + BloodlustTPS +
                calculatedStats.WhiteDPS);

            calculatedStats.LimitedDPS = calculatedStats.MaxLacerateDPS + calculatedStats.MangleDPS + calculatedStats.WhiteDPS;
            calculatedStats.UnlimitedDPS = calculatedStats.LimitedDPS + (179 / attackSpeed )* modArmor* averageDamage;

            calculatedStats.ThreatPoints    = calculatedStats.ThreatScale * 1.45f * (LacerateBaseTPS + critRageTPS + BloodlustTPS +
                1.1f * (((whiteDPS + LacerateDPS + mangleDPS ) * modArmor) * averageDamage + 0.2f*lacerateDotDPS));
            calculatedStats.UnlimitedThreat = calculatedStats.ThreatScale * 1.45f * (LacerateBaseTPS + critRageTPS + BloodlustTPS + 322 / attackSpeed +
                1.1f * (((whiteDPS + LacerateDPS + 179 / attackSpeed + mangleDPS ) * modArmor) * averageDamage + 0.2f*lacerateDotDPS));

            calculatedStats.OverallPoints = calculatedStats.MitigationPoints + calculatedStats.SurvivalPoints + calculatedStats.ThreatPoints;
			return calculatedStats;
		}

		public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
			Stats statsRace = character.Race == Character.CharacterRace.NightElf ?
				new Stats() { 
                    Health = 3434, 
                    Agility = 75, 
                    Stamina = 82, 
                    DodgeRating = 59, 
                    NatureResistance = 10,
					Strength = 73f, 
					AttackPower = 225f,
					BonusCritMultiplier = 0.1f,
					CritRating = 264.0768f, 
					BonusAttackPowerMultiplier = 0.0f,
					BonusAgilityMultiplier = 0.03f,
					BonusStrengthMultiplier = 0.03f,
                    BonusStaminaMultiplier = 0.03f
                } :
				new Stats() { 
                    Health = 3434, 
                    Agility = 65, 
                    Stamina = 85, 
                    DodgeRating = 40, 
                    NatureResistance = 10,
					Strength = 81f, 
					AttackPower = 295f,
					BonusCritMultiplier = 0.1f,
					CritRating = 264.0768f, 
					BonusAttackPowerMultiplier = 0.0f,
					BonusAgilityMultiplier = 0.03f,
                    BonusStrengthMultiplier = 0.03f,
                    BonusStaminaMultiplier = 0.03f
                };

			Stats statsBaseGear = GetItemStats(character, additionalItem);
			Stats statsEnchants = GetEnchantsStats(character);
			Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
			
          
            Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;
			
			statsBaseGear.Agility += statsEnchants.Agility;
			statsBaseGear.DefenseRating += statsEnchants.DefenseRating;
			statsBaseGear.DodgeRating += statsEnchants.DodgeRating;
			statsBaseGear.Resilience += statsEnchants.Resilience;
			statsBaseGear.Stamina += statsEnchants.Stamina;

			statsBuffs.Health += statsEnchants.Health;
			statsBuffs.Armor += statsEnchants.Armor;

			float agiBase = (float)Math.Floor(statsRace.Agility * (1+ statsRace.BonusAgilityMultiplier));
            float agiBonus = (float) Math.Floor((statsBaseGear.Agility + statsBuffs.Agility) * (1 + statsRace.BonusAgilityMultiplier));
            float staBase = (float) Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier) * 1.25f);
            float strBase = (float) Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float strBonus = (float) Math.Floor(statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float staBonus = (statsBaseGear.Stamina + statsBuffs.Stamina) * (1 + statsRace.BonusStaminaMultiplier) * 1.25f;
            float staHotW = (statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier) * 1.25f + staBonus) * 0.2f;
			staBonus = (float)Math.Round(Math.Floor(staBonus) + staHotW);

			Stats statsTotal = new Stats();
			statsTotal.Stamina = staBase + (float)Math.Round((staBase * statsBuffs.BonusStaminaMultiplier) + staBonus * (1 + statsBuffs.BonusStaminaMultiplier));
			statsTotal.DefenseRating = statsRace.DefenseRating + statsBaseGear.DefenseRating + statsBuffs.DefenseRating;
			statsTotal.DodgeRating = statsRace.DodgeRating + statsBaseGear.DodgeRating + statsBuffs.DodgeRating;
			statsTotal.Resilience = statsRace.Resilience + statsBaseGear.Resilience + statsBuffs.Resilience;
			statsTotal.Health = (float)Math.Round(((statsRace.Health + statsBaseGear.Health + statsBuffs.Health + (statsTotal.Stamina * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
			statsTotal.Miss = statsRace.Miss + statsBaseGear.Miss + statsBuffs.Miss;
			statsTotal.CrushChanceReduction = statsBuffs.CrushChanceReduction;
            statsTotal.NatureResistance = statsEnchants.NatureResistance + statsRace.NatureResistance + statsBaseGear.NatureResistance + statsBuffs.NatureResistance;
            statsTotal.FireResistance = statsEnchants.FireResistance + statsRace.FireResistance + statsBaseGear.FireResistance + statsBuffs.FireResistance;
            statsTotal.FrostResistance = statsEnchants.FrostResistance + statsRace.FrostResistance + statsBaseGear.FrostResistance + statsBuffs.FrostResistance;
            statsTotal.ShadowResistance = statsEnchants.ShadowResistance + statsRace.ShadowResistance + statsBaseGear.ShadowResistance + statsBuffs.ShadowResistance;
            statsTotal.ArcaneResistance = statsEnchants.ArcaneResistance + statsRace.ArcaneResistance + statsBaseGear.ArcaneResistance + statsBuffs.ArcaneResistance;
            statsTotal.AllResist = statsEnchants.AllResist + statsRace.AllResist + statsBaseGear.AllResist + statsBuffs.AllResist;


			

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

            statsTotal.Armor = (float) Math.Round(((statsBaseGear.Armor * 5.5f) + statsRace.Armor + statsBuffs.Armor + (statsTotal.Agility * 2f)) * (1 + statsBuffs.BonusArmorMultiplier));

            statsTotal.TerrorProc = statsGearEnchantsBuffs.TerrorProc;
            statsTotal.BonusSwipeDamageMultiplier = statsGearEnchantsBuffs.BonusSwipeDamageMultiplier;
            statsTotal.BonusLacerateDamage = statsGearEnchantsBuffs.BonusLacerateDamage;
            statsTotal.BonusMangleBearDamage = statsGearEnchantsBuffs.BonusMangleBearDamage;
            statsTotal.BonusMangleBearThreat = statsGearEnchantsBuffs.BonusMangleBearThreat;
            statsTotal.BonusPhysicalDamageMultiplier = statsGearEnchantsBuffs.BonusPhysicalDamageMultiplier;
            statsTotal.BonusSwipeDamageMultiplier = statsGearEnchantsBuffs.BonusSwipeDamageMultiplier;
            statsTotal.BloodlustProc = statsGearEnchantsBuffs.BloodlustProc;
            
			return statsTotal;
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
					ComparisonCalculationBear calcCrush = new ComparisonCalculationBear();
					ComparisonCalculationBear calcHit = new ComparisonCalculationBear();
					if (currentCalculationsBear != null)
					{
						calcMiss.Name = "    Miss    ";
						calcDodge.Name = "   Dodge   ";
						calcCrit.Name = "  Crit  ";
						calcCrush.Name = " Crush ";
						calcHit.Name = "Hit";

						float crits = 2f + (0.2f * (currentCalculationsBear.TargetLevel - 70)) - currentCalculationsBear.CappedCritReduction;
						float crushes = currentCalculationsBear.TargetLevel == 73 ? Math.Max(Math.Min(100f - (crits + (currentCalculationsBear.DodgePlusMiss)), 15f) - currentCalculationsBear.BasicStats.CrushChanceReduction, 0f) : 0f;
						float hits = Math.Max(100f - (Math.Max(0f, crits) + Math.Max(crushes, 0) + (currentCalculationsBear.DodgePlusMiss)), 0f);
						
						calcMiss.OverallPoints = calcMiss.MitigationPoints = currentCalculationsBear.Miss;
						calcDodge.OverallPoints = calcDodge.MitigationPoints = currentCalculationsBear.Dodge;
						calcCrit.OverallPoints = calcCrit.SurvivalPoints = crits;
						calcCrush.OverallPoints = calcCrush.SurvivalPoints = crushes;
						calcHit.OverallPoints = calcHit.SurvivalPoints = hits;
					}
					return new ComparisonCalculationBase[] { calcMiss, calcDodge, calcCrit, calcCrush, calcHit };
				
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
					while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && defToAdd < 2)
					{
						defToAdd += 0.01f;
						calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DefenseRating = defToAdd } }) as CharacterCalculationsBear;
					}

					calcAtSubtract = calcBaseValue;
					float defToSubtract = 0f;
					while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && defToSubtract > -2)
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

				//case "Agi Test":
				//    CharacterCalculationsBear calcBaseAgiValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 0 } }) as CharacterCalculationsBear;
				//    List<ComparisonCalculationBase> comparisons = new List<ComparisonCalculationBase>();

				//    float overallCurrent = calcBaseAgiValue.OverallPoints;
				//    float overallAtAdd = calcBaseAgiValue.OverallPoints;
				//    float agiToAdd = 0f;
				//    while (overallCurrent == overallAtAdd)
				//    {
				//        agiToAdd += 0.01f;
				//        overallAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToAdd } }).OverallPoints;
				//    }

				//    float overallAtSubtract = calcBaseAgiValue.OverallPoints;
				//    float agiToSubtract = 0f;
				//    while (overallCurrent == overallAtSubtract)
				//    {
				//        agiToSubtract -= 0.01f;
				//        overallAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToSubtract } }).OverallPoints;
				//    }
				//    agiToSubtract += 0.01f;

				//    float agiDifference = agiToAdd - agiToSubtract;
				//    float overallDifference = overallAtAdd - overallCurrent;
				//    float overallPerAgi = overallDifference / agiDifference;

					
				//    return comparisons.ToArray();
				
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
				Stamina = stats.Stamina,
				Agility = stats.Agility,
				DodgeRating = stats.DodgeRating,
				DefenseRating = stats.DefenseRating,
				Resilience = stats.Resilience,
				TerrorProc = stats.TerrorProc,
				BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
				BonusArmorMultiplier = stats.BonusArmorMultiplier,
				BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
				Health = stats.Health,
				Miss = stats.Miss,
				CrushChanceReduction = stats.CrushChanceReduction,
				AllResist = stats.AllResist,
				ArcaneResistance = stats.ArcaneResistance,
				NatureResistance = stats.NatureResistance,
				FireResistance = stats.FireResistance,
				FrostResistance = stats.FrostResistance,
				ShadowResistance = stats.ShadowResistance,

                Strength = stats.Strength,
                AttackPower = stats.AttackPower,
                CritRating = stats.CritRating,
                HitRating = stats.HitRating,
                HasteRating = stats.HasteRating,
                ExpertiseRating = stats.ExpertiseRating,
                ArmorPenetration = stats.ArmorPenetration,
                WeaponDamage = stats.WeaponDamage,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                BonusMangleBearThreat = stats.BonusMangleBearThreat,
                BonusLacerateDamage = stats.BonusLacerateDamage,
                BonusSwipeDamageMultiplier = stats.BonusSwipeDamageMultiplier,
                BloodlustProc = stats.BloodlustProc,
                BonusMangleBearDamage = stats.BonusMangleBearDamage,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier
			};
		}

		public override bool HasRelevantStats(Stats stats)
		{
			return (stats.Agility + stats.Armor + stats.BonusAgilityMultiplier + stats.BonusArmorMultiplier +
				stats.BonusStaminaMultiplier + stats.DefenseRating + stats.DodgeRating + stats.Health +
				stats.Miss + stats.Resilience + stats.Stamina + stats.TerrorProc + stats.AllResist +
				stats.ArcaneResistance + stats.NatureResistance + stats.FireResistance +
				stats.FrostResistance + stats.ShadowResistance + stats.CrushChanceReduction
                 + stats.Strength + stats.AttackPower + stats.CritRating + stats.HitRating + stats.HasteRating
                 + stats.ExpertiseRating + stats.ArmorPenetration + stats.WeaponDamage + stats.BonusCritMultiplier
                 + stats.TerrorProc+stats.BonusMangleBearThreat + stats.BonusLacerateDamage + stats.BonusSwipeDamageMultiplier
                 + stats.BloodlustProc + stats.BonusMangleBearDamage + stats.BonusAttackPowerMultiplier + stats.BonusPhysicalDamageMultiplier
                ) != 0;
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
        private float _threatScale;
        public float ThreatScale
        {
			get { return _threatScale; }
			set { _threatScale = value; }
        }

		private Stats _basicStats;
		public Stats BasicStats
		{
			get { return _basicStats; }
			set { _basicStats = value; }
		}

		private int _targetLevel;
		public int TargetLevel
		{
			get { return _targetLevel; }
			set { _targetLevel = value; }
		}

		private float _dodge;
		public float Dodge
		{
			get { return _dodge; }
			set { _dodge = value; }
		}

		private float _miss;
		public float Miss
		{
			get { return _miss; }
			set { _miss = value; }
		}

		private float _mitigation;
		public float Mitigation
		{
			get { return _mitigation; }
			set { _mitigation = value; }
		}

		private float _cappedMitigation;
		public float CappedMitigation
		{
			get { return _cappedMitigation; }
			set { _cappedMitigation = value; }
		}

		private float _dodgePlusMiss;
		public float DodgePlusMiss
		{
			get { return _dodgePlusMiss; }
			set { _dodgePlusMiss = value; }
		}

		private float _totalMitigation;
		public float TotalMitigation
		{
			get { return _totalMitigation; }
			set { _totalMitigation = value; }
		}

		private float _damageTaken;
		public float DamageTaken
		{
			get { return _damageTaken; }
			set { _damageTaken = value; }
		}

		private float _critReduction;
		public float CritReduction
		{
			get { return _critReduction; }
			set { _critReduction = value; }
		}

		private float _cappedCritReduction;
		public float CappedCritReduction
		{
			get { return _cappedCritReduction; }
			set { _cappedCritReduction = value; }
		}

        private float _missedAttacks;
	    public float MissedAttacks
	    {
		    get { return _missedAttacks; }
		    set { _missedAttacks = value; }
	    }

       
        private float _dodgedAttacks;
	    public float DodgedAttacks
	    {
		    get { return _dodgedAttacks; }
		    set { _dodgedAttacks = value; }
	    }

        private float _limitedThreat;
	    public float LimitedThreat
	    {
		    get { return _limitedThreat; }
		    set { _limitedThreat = value; }
	    }

        private float _unlimitedThreat;
        public float UnlimitedThreat
        {
	        get { return _unlimitedThreat; }
	        set { _unlimitedThreat = value; }
        }
        public float MangleTPS {get;set;}
        public float WhiteTPS {get;set;}
        public float SwipeTPS {get;set;}
        public float MaxLacerate {get;set;}
        public float MinLacerate {get;set;}

        public float LimitedDPS { get; set; }
        public float UnlimitedDPS { get; set; }
        public float MangleDPS { get; set; }
        public float WhiteDPS { get; set; }
        public float SwipeDPS { get; set; }
        public float MaxLacerateDPS { get; set; }
        public float MinLacerateDPS { get; set; }


        public float EnemyAvoidedAttacks
        {
            get
            {
                return EnemyBlockedAttacks + EnemyMissedAttacks + EnemyDodgedAttacks + EnemyParriedAttacks;
            }
        }
	    public float EnemyDodgedAttacks{get;set;}
	    public float EnemyMissedAttacks{get;set;}
	    public float EnemyParriedAttacks{get;set;}
	    public float EnemyBlockedAttacks{get;set;}

        public float NatureSurvivalPoints{get;set;}
        public float FrostSurvivalPoints{get;set;}
        public float FireSurvivalPoints{get;set;}
        public float ShadowSurvivalPoints{get;set;}
        public float ArcaneSurvivalPoints{get;set;}

		public Character.CharacterRace Race { get; set; }
		public List<Buff> ActiveBuffs { get; set; }

		public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
		{
			Dictionary<string, string> dictValues = new Dictionary<string, string>();
			int armorCap = (int)Math.Ceiling((1402.5f * TargetLevel) - 66502.5f);
			float levelDifference = 0.2f * (TargetLevel - 70);
			float targetCritReduction = 2f + levelDifference;
			float currentCritReduction = ((float)Math.Floor(BasicStats.DefenseRating / (123f / 52f)) * 0.04f) +
				(BasicStats.Resilience / (2050f / 52f));
			int defToCap = 0, resToCap = 0;
			if (currentCritReduction < targetCritReduction)
			{
				while (((float)Math.Floor((BasicStats.DefenseRating + defToCap) / (123f / 52f)) * 0.04) +
						(BasicStats.Resilience / (2050f / 52f)) < targetCritReduction)
					defToCap++;
				while (((float)Math.Floor(BasicStats.DefenseRating / (123f / 52f)) * 0.04) +
						((BasicStats.Resilience + resToCap) / (2050f / 52f)) < targetCritReduction)
					resToCap++;
			}
			else if (currentCritReduction > targetCritReduction)
			{
				while (((float)Math.Floor((BasicStats.DefenseRating + defToCap) / (123f / 52f)) * 0.04) +
						(BasicStats.Resilience / (2050f / 52f)) > targetCritReduction)
					defToCap--;
				while (((float)Math.Floor(BasicStats.DefenseRating / (123f / 52f)) * 0.04) +
						((BasicStats.Resilience + resToCap) / (2050f / 52f)) > targetCritReduction)
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
			if (Race == Character.CharacterRace.NightElf)
				dictValues.Add("Dodge Rating", (BasicStats.DodgeRating - 59).ToString());
			else
				dictValues.Add("Dodge Rating", (BasicStats.DodgeRating - 40).ToString());
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
			dictValues.Add("Dodge + Miss", DodgePlusMiss.ToString() + "%");
			dictValues.Add("Total Mitigation", TotalMitigation.ToString() + "%");
			dictValues.Add("Damage Taken", DamageTaken.ToString() + "%");
			if (defToCap == 0 && resToCap == 0)
			{
				dictValues.Add("Chance to be Crit", ((2f + levelDifference) - CritReduction).ToString()
					+ "%*Exactly enough defense rating/resilience to be uncrittable by bosses.");
			}
			else if (defToCap + resToCap > 0)
			{
				dictValues.Add("Chance to be Crit", ((2f + levelDifference) - CritReduction).ToString()
					+ string.Format("%*CRITTABLE! Short by {0} defense rating or {1} resilience to be uncrittable by bosses.",
					defToCap, resToCap));
			}
			else
				dictValues.Add("Chance to be Crit", ((2f + levelDifference) - CritReduction).ToString()
					+ string.Format("%*Uncrittable by bosses. {0} defense rating or {1} resilience over the crit cap.",
					-defToCap, -resToCap));
			dictValues.Add("Overall Points", OverallPoints.ToString());
			dictValues.Add("Mitigation Points", MitigationPoints.ToString());
			dictValues.Add("Survival Points", SurvivalPoints.ToString());

            dictValues["Nature Survival"] = NatureSurvivalPoints.ToString();
            dictValues["Frost Survival"] = FrostSurvivalPoints.ToString();
            dictValues["Fire Survival"] = FireSurvivalPoints.ToString();
            dictValues["Shadow Survival"] = ShadowSurvivalPoints.ToString();
            dictValues["Arcane Survival"] = ArcaneSurvivalPoints.ToString(); 

            float critRating = BasicStats.CritRating;
            if (ActiveBuffs.Contains(Buff.GetBuffByName("Improved Judgement of the Crusade")))
                critRating -= 66.24f;
            critRating -= 264.0768f; //Base 5% + 6% from talents

            dictValues["Strength"] = BasicStats.Strength.ToString();
            dictValues["Attack Power"] = BasicStats.AttackPower.ToString();
            dictValues["Hit Rating"] = BasicStats.HitRating.ToString();
            dictValues["Expertise"] = BasicStats.ExpertiseRating.ToString();
            dictValues["Haste Rating"] = BasicStats.HasteRating.ToString();
            dictValues["Armor Penetration"] = BasicStats.ArmorPenetration.ToString();
            dictValues["Crit Rating"] = critRating.ToString();
            dictValues["Weapon Damage"] = BasicStats.WeaponDamage.ToString();


            dictValues["Limited Threat"] = String.Format("{0}*{1} DPS", (ThreatPoints / ThreatScale).ToString(), LimitedDPS);//;
            dictValues["Unlimited Threat"] = String.Format("{0}*{1} DPS", (UnlimitedThreat / ThreatScale).ToString(), UnlimitedDPS);//;

            dictValues["Lacerate Max TPS"] = String.Format("{0}*{1} DPS", MaxLacerate, MaxLacerateDPS);
            dictValues["Lacerate Min TPS"] = String.Format("{0}*{1} DPS", MinLacerate, MinLacerateDPS);//
            dictValues["Swipe TPS"] = String.Format("{0}*{1} DPS", SwipeTPS, SwipeDPS);//SwipeTPS.ToString();
            dictValues["Mangle TPS"] = String.Format("{0}*{1} DPS", MangleTPS, MangleDPS);//MangleTPS.ToString();
            dictValues["White TPS"] = String.Format("{0}*{1} DPS", WhiteTPS, WhiteDPS);//WhiteTPS.ToString();


            dictValues["Missed Attacks"] = String.Format("{0}%*Missed={1}% Dodged={2}% Parried={3}% Blocked={4}% (Block not modeled).", EnemyAvoidedAttacks, EnemyMissedAttacks, EnemyDodgedAttacks, EnemyParriedAttacks, EnemyBlockedAttacks);
           
			return dictValues;
		}

		public override float GetOptimizableCalculationValue(string calculation)
		{
			switch (calculation)
			{
				case "Health": return BasicStats.Health;
				case "Mitigation % from Armor": return Mitigation;
				case "Avoidance %": return DodgePlusMiss;
				case "% Chance to be Crit": return ((2f + (0.2f * (TargetLevel - 70))) - CritReduction);
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
			return string.Format("{0}: ({1}O {2}M {3}S)", Name, Math.Round(OverallPoints), Math.Round(MitigationPoints), Math.Round(SurvivalPoints));
		}
	}
}
