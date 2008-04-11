using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
    [System.ComponentModel.DisplayName("ProtWarr|Ability_Warrior_ShieldMastery")]
	public class CalculationsProtWarr : CalculationsBase
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
					_calculationOptionsPanel = new CalculationOptionsPanelProtWarr();
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
                    "Basic Stats:Parry Rating",
                    "Basic Stats:Block Rating",
                    "Basic Stats:Block Value",
					"Basic Stats:Defense Rating",
					"Basic Stats:Resilience",
					"Basic Stats:Nature Resist",
					"Basic Stats:Fire Resist",
					"Basic Stats:Frost Resist",
					"Basic Stats:Shadow Resist",
					"Basic Stats:Arcane Resist",
                    "Complex Stats:Limited Threat",
                    "Complex Stats:Unlimited Threat",
                    "Complex Stats:Missed Attacks",
					"Complex Stats:Dodge",
                    "Complex Stats:Parry",
                    "Complex Stats:Block",
					"Complex Stats:Miss",
					"Complex Stats:Mitigation",
					"Complex Stats:Avoidance",
                    "Complex Stats:Avoidance + Block",
					"Complex Stats:Total Mitigation",
					"Complex Stats:Damage Taken",
					"Complex Stats:Chance to be Crit",
                    "Complex Stats:Chance Crushed",
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
                    "% Chance to be Crushed",
                    "Nature Survival",
                    "Fire Survival",
                    "Frost Survival",
                    "Shadow Survival",
                    "Arcane Survival",
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
						Item.ItemType.Cloth,
                        Item.ItemType.Leather,
                        Item.ItemType.Mail,
                        Item.ItemType.Plate,
                        Item.ItemType.Bow,
                        Item.ItemType.Crossbow,
                        Item.ItemType.Gun,
                        Item.ItemType.Thrown,
                        Item.ItemType.Arrow,
                        Item.ItemType.Bullet,
                        Item.ItemType.FistWeapon,
                        Item.ItemType.Dagger,
                        Item.ItemType.OneHandAxe,
                        Item.ItemType.OneHandMace,
                        Item.ItemType.OneHandSword,
                        Item.ItemType.Shield,
                        Item.ItemType.Polearm,
                        Item.ItemType.TwoHandAxe,
                        Item.ItemType.TwoHandMace,
                        Item.ItemType.TwoHandSword
					});
				}
				return _relevantItemTypes;
			}
		}

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Warrior; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationProtWarr(); }
		public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsProtWarr(); }

		public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
		{
			_cachedCharacter = character;
			int targetLevel = int.Parse(character.CalculationOptions["TargetLevel"]);
            float talentParry = 5.0f;
            float talentBlock = 5.0f;
            float talentBlockValue = 1.3f;
			Stats stats = GetCharacterStats(character, additionalItem);
			float levelDifference = (targetLevel - 70f) * 0.2f;
			CharacterCalculationsProtWarr calculatedStats = new CharacterCalculationsProtWarr();
			calculatedStats.BasicStats = stats;
			calculatedStats.TargetLevel = targetLevel;
            calculatedStats.Miss = 5f + (((float)Math.Floor(stats.DefenseRating * 0.423f)) / 25f) + stats.Miss - levelDifference;
            calculatedStats.Dodge = Math.Min(100f - calculatedStats.Miss, stats.Agility / 30.0f + stats.DodgeRating / 18.9231f + (((float)Math.Floor(stats.DefenseRating * 0.423f)) / 25f) - levelDifference);
            calculatedStats.Parry = talentParry + Math.Min(100f - calculatedStats.Miss - calculatedStats.Dodge, stats.ParryRating / 22.4f + (((float)Math.Floor(stats.DefenseRating * 0.423f)) / 25f) - levelDifference); ;
            calculatedStats.Block = talentBlock + Math.Min(100f - calculatedStats.Miss - calculatedStats.Dodge - calculatedStats.Parry, stats.BlockRating / 7.8846f + (((float)Math.Floor(stats.DefenseRating * 0.423f)) / 25f) - levelDifference); ;
            calculatedStats.BlockValue = talentBlockValue * (stats.BlockValue + ((float)Math.Floor(stats.Strength / 20.0f)));
			calculatedStats.Mitigation = (stats.Armor / (stats.Armor - 22167.5f + (467.5f * targetLevel))) * 100f; //(stats.Armor / (stats.Armor + 11959.5f)) * 100f; for only 73s
			calculatedStats.CappedMitigation = Math.Min(75f, calculatedStats.Mitigation);
			calculatedStats.DodgePlusMissPlusParry = calculatedStats.Dodge + calculatedStats.Miss + calculatedStats.Parry;
            calculatedStats.DodgePlusMissPlusParryPlusBlock = calculatedStats.Dodge + calculatedStats.Miss + calculatedStats.Parry + calculatedStats.Block;
			calculatedStats.CritReduction = (((float)Math.Floor(stats.DefenseRating * 0.423f)) / 25f) + (stats.Resilience / 39.423f);
			calculatedStats.CappedCritReduction = Math.Min(5f + levelDifference, calculatedStats.CritReduction);

            //Miss -> Dodge -> Parry -> Block -> Crushing Blow -> Critical Strikes -> Hit
			//Out of 100 attacks, you'll take...
            //Technically block's happen before crits, but I'd rather not risk it, I'm assuming
            //you have to be crit immune from defense/resiliance.
            float crits = Math.Max(5f + (0.2f * levelDifference) - calculatedStats.CritReduction, 0f);
            float blocked = Math.Min(100f - (crits + (calculatedStats.DodgePlusMissPlusParry)), calculatedStats.Block);
            float crushes = targetLevel == 73 ? Math.Max(Math.Min(100f - (crits + (calculatedStats.DodgePlusMissPlusParryPlusBlock)), 15f) - stats.CrushChanceReduction, 0f) : 0f;
            float hits = Math.Max(100f - (crits + crushes + blocked + (calculatedStats.DodgePlusMissPlusParry)), 0f);

            float damagePerBossAttack = float.Parse(character.CalculationOptions["BossAttackValue"]); ;
			//Apply armor and multipliers for each attack type...
			crits *= (100f - calculatedStats.CappedMitigation) * .02f;
			crushes *= (100f - calculatedStats.CappedMitigation) * .015f;
			hits *= (100f - calculatedStats.CappedMitigation) * .01f;
            blocked *= (100f - calculatedStats.CappedMitigation) * .01f * (1f - Math.Max((calculatedStats.BlockValue / (damagePerBossAttack * (100f - calculatedStats.CappedMitigation) / 100)), 0f)); //assume the block value is negligiable
			calculatedStats.DamageTaken = blocked + hits + crushes + crits;
			calculatedStats.TotalMitigation = 100f - calculatedStats.DamageTaken;

			calculatedStats.SurvivalPoints = (stats.Health / (1f - (calculatedStats.CappedMitigation / 100f))); // / (buffs.ShadowEmbrace ? 0.95f : 1f);
            calculatedStats.MitigationPoints = (7000f * (1f / (calculatedStats.DamageTaken / 100f))); // / (buffs.ShadowEmbrace ? 0.95f : 1f);

            float cappedResist = targetLevel * 5;

            calculatedStats.NatureSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.NatureResistance + stats.AllResist) / cappedResist) * .75)));
            calculatedStats.FrostSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.FrostResistance + stats.AllResist) / cappedResist) * .75)));
            calculatedStats.FireSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.FireResistance + stats.AllResist) / cappedResist) * .75)));
            calculatedStats.ShadowSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.ShadowResistance + stats.AllResist) / cappedResist) * .75)));
            calculatedStats.ArcaneSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.ArcaneResistance + stats.AllResist) / cappedResist) * .75)));

            /*float targetArmor = 7700;
            float baseArmor = Math.Max(0f, targetArmor - stats.ArmorPenetration);
            float modArmor = 1-(baseArmor / (baseArmor + 10557.5f));

            float critMultiplier = 2 * (1 + stats.BonusCritMultiplier);
            float attackPower = stats.AttackPower + ((1 + stats.BonusAttackPowerMultiplier));


            float hasteBonus = stats.HasteRating / 15.76f / 100f;
            float attackSpeed = (2.5f ) / (1f + hasteBonus);


            float hitBonus = stats.HitRating * 52f / 82f / 1000f;
            float expertiseBonus = 2*stats.ExpertiseRating * 52f / 82f / 2.5f * 0.0025f;


            float chanceCrit = Math.Min(0.75f, (stats.CritRating / 22.08f + (stats.Agility / 33f)) / 100f) - 0.042f; 
            float chanceDodge = Math.Max(0f, 2*0.065f - expertiseBonus);
            float chanceMiss = Math.Max(0f, 0.09f - hitBonus) + chanceDodge;*/


            //calculatedStats.AvoidedAttacks = chanceMiss * 100f;
            //calculatedStats.DodgedAttacks = chanceDodge * 100f;
            //calculatedStats.MissedAttacks = calculatedStats.AvoidedAttacks - calculatedStats.DodgedAttacks;

            calculatedStats.AvoidedAttacks = 0f;
            calculatedStats.DodgedAttacks = 0f;
            calculatedStats.MissedAttacks = 0f;


            /*float critRageTPS = chanceCrit*(1 / attackSpeed + 1 / 6.0f)*25;

            float averageDamage = 1 - chanceMiss +  (1 + stats.BonusCritMultiplier) * chanceCrit;

            float weaponDamage = stats.WeaponDamage+(2.5f* + (768f + attackPower) / 14f);

            float whiteTPS = weaponDamage / attackSpeed ;

            // need 2T6 bonus, idol bonus
            float mangleTPS = 1.3f* 1.15f * (weaponDamage + 155f) / 6;

            // 3 rips per 6 second mangle cooldown
            float LacerateTPS = 285 / 2;

            // need to add 4t5 bonus
            float lacerateDotTPS = 1.3f*0.2f*(155 + 5*stats.AttackPower / 20)/15;

            */
            calculatedStats.ThreatScale = float.Parse(character.CalculationOptions["ThreatScale"]);
            /*
            //need to add 2t4 bonus
            
            //calculatedStats.ThreatPoints = calculatedStats.ThreatScale * 1.45f * (1.1f * ((whiteTPS + mangleTPS) * averageDamage *modArmor + lacerateDotTPS) + LacerateTPS + critRageTPS);
            //calculatedStats.UnlimitedThreat = calculatedStats.ThreatScale * 1.45f * (1.1f * ((whiteTPS + 179 / attackSpeed + mangleTPS) * averageDamage * modArmor + lacerateDotTPS) + LacerateTPS + critRageTPS) + 322 / attackSpeed;
            */

            calculatedStats.ThreatPoints = 0f;
            calculatedStats.UnlimitedThreat = 0f;

            calculatedStats.OverallPoints = calculatedStats.MitigationPoints + calculatedStats.SurvivalPoints + calculatedStats.ThreatPoints;
			return calculatedStats;
		}

        private static float[,] BaseWarriorRaceStats = new float[,] 
		{
							//	Agility,	Strength,	Stamina
            /*Human*/		{	0f,		    0f,		    0f,	    },
            /*Orc*/			{	93f,		148f,		135f,	},
            /*Dwarf*/		{	0f,		    0f,		    0f,	    },
			/*Night Elf*/	{	0f,		    0f,		    0f,	    },
	        /*Undead*/		{	0f,		    0f,		    0f,	    },
			/*Tauren*/		{	0f,			0f,			0f,		},
	        /*Gnome*/		{	0f,		    0f,		    0f,	    },
			/*Troll*/		{	0f,		    0f,		    0f,	    },	
			/*BloodElf*/	{	0f,		    0f,		    0f,	    },
			/*Draenei*/		{	0f,			0f,			0f,		},
		};

        private Stats GetRaceStats(Character.CharacterRace race)
        {
            if (race == Character.CharacterRace.BloodElf)
                return new Stats();

            Stats statsRace = new Stats()
            {
                Health = 4264f,
                Agility = (float)BaseWarriorRaceStats[(int)race-1, 0],
                Strength = (float)BaseWarriorRaceStats[(int)race-1, 1],
                Stamina = (float)BaseWarriorRaceStats[(int)race-1, 2],

                AttackPower = 190f,

                DodgeRating = (float)(-0.59 * 18.92f),
            };

            if (race == Character.CharacterRace.NightElf)
                statsRace.DodgeRating += 18.92f;

            return statsRace;
        }

		public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
            Stats statsRace = GetRaceStats(character.Race);
			Stats statsBaseGear = GetItemStats(character, additionalItem);
			Stats statsEnchants = GetEnchantsStats(character);
			Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
			
            Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;
			
			statsBaseGear.Agility += statsEnchants.Agility;
			statsBaseGear.DefenseRating += statsEnchants.DefenseRating;
			statsBaseGear.DodgeRating += statsEnchants.DodgeRating;
            statsBaseGear.ParryRating += statsEnchants.ParryRating;
            statsBaseGear.BlockRating += statsEnchants.BlockRating;
            statsBaseGear.BlockValue += statsEnchants.BlockValue;
			statsBaseGear.Resilience += statsEnchants.Resilience;
			statsBaseGear.Stamina += statsEnchants.Stamina;

			statsBuffs.Health += statsEnchants.Health;
			statsBuffs.Armor += statsEnchants.Armor;

            float vitalityBonus = 1.05f;
            float toughnessBonus = 1.1f;

			float agiBase = (float)Math.Floor(statsRace.Agility * (1+ statsRace.BonusAgilityMultiplier));
            float agiBonus = (float) Math.Floor((statsBaseGear.Agility + statsBuffs.Agility) * (1 + statsRace.BonusAgilityMultiplier));
            float staBase = (float)Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier) * vitalityBonus);
            float strBase = (float) Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float strBonus = (float) Math.Floor(statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float staBonus = (statsBaseGear.Stamina + statsBuffs.Stamina) * (1 + statsRace.BonusStaminaMultiplier) * vitalityBonus;
			staBonus = (float)Math.Round(Math.Floor(staBonus));

			Stats statsTotal = new Stats();
			statsTotal.Stamina = staBase + (float)Math.Round((staBase * statsBuffs.BonusStaminaMultiplier) + staBonus * (1 + statsBuffs.BonusStaminaMultiplier));
			statsTotal.DefenseRating = statsRace.DefenseRating + statsBaseGear.DefenseRating + statsBuffs.DefenseRating;
			statsTotal.DodgeRating = statsRace.DodgeRating + statsBaseGear.DodgeRating + statsBuffs.DodgeRating;
            statsTotal.ParryRating = statsRace.ParryRating + statsBaseGear.ParryRating + statsBuffs.ParryRating;
            statsTotal.BlockRating = statsRace.BlockRating + statsBaseGear.BlockRating + statsBuffs.BlockRating;
            statsTotal.BlockValue = statsRace.BlockValue + statsBaseGear.BlockValue + statsBuffs.BlockValue;
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
            statsTotal.Agility = (agiBase + agiBonus);
            statsTotal.Strength = (strBase + strBonus);
 
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

            statsTotal.Armor = (float)Math.Round(((statsBaseGear.Armor * toughnessBonus) + statsRace.Armor + statsBuffs.Armor + (statsTotal.Agility * 2f)) * (1 + statsBuffs.BonusArmorMultiplier));

            
            
            
			return statsTotal;
		}

		public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
		{
			switch (chartName)
			{
				case "Combat Table":
					CharacterCalculationsProtWarr currentCalculationsProtWarr = GetCharacterCalculations(character) as CharacterCalculationsProtWarr;
					ComparisonCalculationProtWarr calcMiss = new ComparisonCalculationProtWarr();
					ComparisonCalculationProtWarr calcDodge = new ComparisonCalculationProtWarr();
                    ComparisonCalculationProtWarr calcParry = new ComparisonCalculationProtWarr();
                    ComparisonCalculationProtWarr calcBlock= new ComparisonCalculationProtWarr();
					ComparisonCalculationProtWarr calcCrit = new ComparisonCalculationProtWarr();
					ComparisonCalculationProtWarr calcCrush = new ComparisonCalculationProtWarr();
					ComparisonCalculationProtWarr calcHit = new ComparisonCalculationProtWarr();
					if (currentCalculationsProtWarr != null)
					{
						calcMiss.Name = "    Miss    ";
						calcDodge.Name = "   Dodge   ";
                        calcParry.Name = "   Parry   ";
                        calcBlock.Name = "   Block   ";
						calcCrit.Name = "  Crit  ";
						calcCrush.Name = " Crush ";
						calcHit.Name = "Hit";

						float crits = 5f + (0.2f * (currentCalculationsProtWarr.TargetLevel - 70)) - currentCalculationsProtWarr.CappedCritReduction;
                        float crushes = currentCalculationsProtWarr.TargetLevel == 73 ? Math.Max(Math.Min(100f - (crits + (currentCalculationsProtWarr.DodgePlusMissPlusParry) + (currentCalculationsProtWarr.Block)), 15f), 0f) : 0f;
                        float hits = Math.Max(100f - (crits + crushes + (currentCalculationsProtWarr.DodgePlusMissPlusParry) + (currentCalculationsProtWarr.Block)), 0f);

						calcMiss.OverallPoints = calcMiss.MitigationPoints = currentCalculationsProtWarr.Miss;
						calcDodge.OverallPoints = calcDodge.MitigationPoints = currentCalculationsProtWarr.Dodge;
                        calcParry.OverallPoints = calcParry.MitigationPoints = currentCalculationsProtWarr.Parry;
                        calcBlock.OverallPoints = calcBlock.MitigationPoints = currentCalculationsProtWarr.Block;
						calcCrit.OverallPoints = calcCrit.SurvivalPoints = crits;
						calcCrush.OverallPoints = calcCrush.SurvivalPoints = crushes;
						calcHit.OverallPoints = calcHit.SurvivalPoints = hits;
					}
					return new ComparisonCalculationBase[] { calcMiss, calcDodge, calcParry, calcBlock, calcCrit, calcCrush, calcHit };
				
				case "Relative Stat Values":
					CharacterCalculationsProtWarr calcBaseValue = GetCharacterCalculations(character) as CharacterCalculationsProtWarr;
					//CharacterCalculationsProtWarr calcAgiValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 10 } }) as CharacterCalculationsProtWarr;
					//CharacterCalculationsProtWarr calcACValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Armor = 10 } }) as CharacterCalculationsProtWarr;
					//CharacterCalculationsProtWarr calcStaValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = 10 } }) as CharacterCalculationsProtWarr;
					CharacterCalculationsProtWarr calcDefValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DefenseRating = 1 } }) as CharacterCalculationsProtWarr;
					CharacterCalculationsProtWarr calcDodgeValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DodgeRating = 1 } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcParryValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { ParryRating = 1 } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcBlockValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { BlockRating = 1 } }) as CharacterCalculationsProtWarr;
					CharacterCalculationsProtWarr calcHealthValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Health = 1 } }) as CharacterCalculationsProtWarr;
					CharacterCalculationsProtWarr calcResilValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Resilience = 1 } }) as CharacterCalculationsProtWarr;
					
					//Differential Calculations for Agi
					CharacterCalculationsProtWarr calcAtAdd = calcBaseValue;
					float agiToAdd = 0f;
					while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && agiToAdd < 2)
					{
						agiToAdd += 0.01f;
						calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToAdd } }) as CharacterCalculationsProtWarr;
					}

					CharacterCalculationsProtWarr calcAtSubtract = calcBaseValue;
					float agiToSubtract = 0f;
					while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && agiToSubtract > -2)
					{
						agiToSubtract -= 0.01f;
						calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToSubtract } }) as CharacterCalculationsProtWarr;
					}
					agiToSubtract += 0.01f;

					ComparisonCalculationProtWarr comparisonAgi = new ComparisonCalculationProtWarr() { Name = "Agility", OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (agiToAdd - agiToSubtract),
						MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (agiToAdd - agiToSubtract), SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (agiToAdd - agiToSubtract)};


					//Differential Calculations for AC
					calcAtAdd = calcBaseValue;
					float acToAdd = 0f;
					while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && acToAdd < 2)
					{
						acToAdd += 0.01f;
						calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Armor = acToAdd } }) as CharacterCalculationsProtWarr;
					}

					calcAtSubtract = calcBaseValue;
					float acToSubtract = 0f;
					while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && acToSubtract > -2)
					{
						acToSubtract -= 0.01f;
						calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Armor = acToSubtract } }) as CharacterCalculationsProtWarr;
					}
					acToSubtract += 0.01f;

					ComparisonCalculationProtWarr comparisonAC = new ComparisonCalculationProtWarr() { Name = "Armor", OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (acToAdd - acToSubtract),
						MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (acToAdd - acToSubtract), SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (acToAdd - acToSubtract)};


					//Differential Calculations for Sta
					calcAtAdd = calcBaseValue;
					float staToAdd = 0f;
					while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && staToAdd < 2)
					{
						staToAdd += 0.01f;
						calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = staToAdd } }) as CharacterCalculationsProtWarr;
					}

					calcAtSubtract = calcBaseValue;
					float staToSubtract = 0f;
					while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && staToSubtract > -2)
					{
						staToSubtract -= 0.01f;
						calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = staToSubtract } }) as CharacterCalculationsProtWarr;
					}
					staToSubtract += 0.01f;

					ComparisonCalculationProtWarr comparisonSta = new ComparisonCalculationProtWarr() { Name = "Stamina", OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (staToAdd - staToSubtract),
						MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (staToAdd - staToSubtract), SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (staToAdd - staToSubtract)};

					return new ComparisonCalculationBase[] { 
						comparisonAgi,
						comparisonAC,
						comparisonSta,
						new ComparisonCalculationProtWarr() { Name = "Defense Rating", OverallPoints = (calcDefValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcDefValue.MitigationPoints - calcBaseValue.MitigationPoints), SurvivalPoints = (calcDefValue.SurvivalPoints - calcBaseValue.SurvivalPoints)},
						new ComparisonCalculationProtWarr() { Name = "Dodge Rating", OverallPoints = (calcDodgeValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcDodgeValue.MitigationPoints - calcBaseValue.MitigationPoints), SurvivalPoints = (calcDodgeValue.SurvivalPoints - calcBaseValue.SurvivalPoints)},
                        new ComparisonCalculationProtWarr() { Name = "Parry Rating", OverallPoints = (calcParryValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcParryValue.MitigationPoints - calcBaseValue.MitigationPoints), SurvivalPoints = (calcDodgeValue.SurvivalPoints - calcBaseValue.SurvivalPoints)},
                        new ComparisonCalculationProtWarr() { Name = "Block Rating", OverallPoints = (calcBlockValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcBlockValue.MitigationPoints - calcBaseValue.MitigationPoints), SurvivalPoints = (calcDodgeValue.SurvivalPoints - calcBaseValue.SurvivalPoints)},
						new ComparisonCalculationProtWarr() { Name = "Health", OverallPoints = (calcHealthValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcHealthValue.MitigationPoints - calcBaseValue.MitigationPoints), SurvivalPoints = (calcHealthValue.SurvivalPoints - calcBaseValue.SurvivalPoints)},
						new ComparisonCalculationProtWarr() { Name = "Resilience", OverallPoints = (calcResilValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcResilValue.MitigationPoints - calcBaseValue.MitigationPoints), SurvivalPoints = (calcResilValue.SurvivalPoints - calcBaseValue.SurvivalPoints)},
					};

				//case "Agi Test":
				//    CharacterCalculationsProtWarr calcBaseAgiValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 0 } }) as CharacterCalculationsProtWarr;
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

		public override Stats GetRelevantStats(Stats stats)
		{
			return new Stats()
			{
				Armor = stats.Armor,
				Stamina = stats.Stamina,
				Agility = stats.Agility,
				DodgeRating = stats.DodgeRating,
                ParryRating = stats.ParryRating,
                BlockRating = stats.BlockRating,
                BlockValue = stats.BlockValue,
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
                BonusCritMultiplier = stats.BonusCritMultiplier

			};
		}

		public override bool HasRelevantStats(Stats stats)
		{
            return true;
			/*return (stats.Agility + stats.Armor + stats.BonusAgilityMultiplier + stats.BonusArmorMultiplier +
				stats.BonusStaminaMultiplier + stats.DefenseRating + stats.DodgeRating + stats.ParryRating 
                stats.BlockRating + stats.Health + 
				stats.Miss + stats.Resilience + stats.Stamina + stats.TerrorProc + stats.AllResist +
				stats.ArcaneResistance + stats.NatureResistance + stats.FireResistance +
				stats.FrostResistance + stats.ShadowResistance
                 + stats.Strength + stats.AttackPower + stats.CritRating + stats.HitRating + stats.HasteRating
                 + stats.ExpertiseRating + stats.ArmorPenetration + stats.WeaponDamage + stats.BonusCritMultiplier
                ) != 0;*/
		}
    }

    public class CharacterCalculationsProtWarr : CharacterCalculationsBase
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

        private float _parry;
        public float Parry
        {
            get { return _parry; }
            set { _parry = value; }
        }

        private float _block;
        public float Block
        {
            get { return _block; }
            set { _block = value; }
        }

        private float _blockValue;
        public float BlockValue
        {
            get { return _blockValue; }
            set { _blockValue = value; }
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

        private float _dodgePlusMissPlusParry;
        public float DodgePlusMissPlusParry
        {
            get { return _dodgePlusMissPlusParry; }
            set { _dodgePlusMissPlusParry = value; }
        }

        private float _dodgePlusMissPlusParryPlusBlock;
        public float DodgePlusMissPlusParryPlusBlock
        {
            get { return _dodgePlusMissPlusParryPlusBlock; }
            set { _dodgePlusMissPlusParryPlusBlock = value; }
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

        private float _avoidedAttacks;
	    public float AvoidedAttacks
	    {
		    get { return _avoidedAttacks; }
		    set { _avoidedAttacks = value; }
	    }
        
        private float _dodgedAttacks;
	    public float DodgedAttacks
	    {
		    get { return _dodgedAttacks; }
		    set { _dodgedAttacks = value; }
	    }

        private float _parriedAttacks;
        public float ParriedAttacks
        {
            get { return _parriedAttacks; }
            set { _parriedAttacks = value; }
        }

        private float _blockedAttacks;
        public float BlockedAttacks
        {
            get { return _blockedAttacks; }
            set { _blockedAttacks = value; }
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

        public float NatureSurvivalPoints{get;set;}
        public float FrostSurvivalPoints{get;set;}
        public float FireSurvivalPoints{get;set;}
        public float ShadowSurvivalPoints{get;set;}
        public float ArcaneSurvivalPoints{get;set;}

		public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
		{
			Dictionary<string, string> dictValues = new Dictionary<string, string>();
			int armorCap = (int)Math.Ceiling((1402.5f * TargetLevel) - 66502.5f);
			float levelDifference = 0.2f * (TargetLevel - 70);

            dictValues["Nature Resist"] = (BasicStats.NatureResistance+BasicStats.AllResist).ToString();
            dictValues["Arcane Resist"] = (BasicStats.ArcaneResistance+BasicStats.AllResist).ToString();
            dictValues["Frost Resist"] = (BasicStats.FrostResistance+BasicStats.AllResist).ToString();
            dictValues["Fire Resist"] = (BasicStats.FireResistance+BasicStats.AllResist).ToString();
            dictValues["Shadow Resist"] = (BasicStats.ShadowResistance + BasicStats.AllResist).ToString();
			dictValues.Add("Health", BasicStats.Health.ToString());
			dictValues.Add("Agility", BasicStats.Agility.ToString());
			dictValues.Add("Armor", BasicStats.Armor.ToString());
			dictValues.Add("Stamina", BasicStats.Stamina.ToString());
			if (Calculations.CachedCharacter.Race == Character.CharacterRace.NightElf)
				dictValues.Add("Dodge Rating", (BasicStats.DodgeRating - 59).ToString());
			else
				dictValues.Add("Dodge Rating", (BasicStats.DodgeRating - 40).ToString());
            dictValues.Add("Parry Rating", BasicStats.ParryRating.ToString());
            dictValues.Add("Block Rating", BasicStats.BlockRating.ToString());
            dictValues.Add("Block Value", BasicStats.BlockValue.ToString());
			dictValues.Add("Defense Rating", BasicStats.DefenseRating.ToString());
			dictValues.Add("Resilience", BasicStats.Resilience.ToString());
			dictValues.Add("Dodge", Dodge.ToString() + "%");
            dictValues.Add("Parry", Parry.ToString() + "%");
            dictValues.Add("Block", Block.ToString() + "%");
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
            dictValues.Add("Avoidance", DodgePlusMissPlusParry.ToString() + "%");
            dictValues.Add("Avoidance + Block", DodgePlusMissPlusParryPlusBlock.ToString() + "%");
			dictValues.Add("Total Mitigation", TotalMitigation.ToString() + "%");
			dictValues.Add("Damage Taken", DamageTaken.ToString() + "%");
			if (CritReduction == (5f + levelDifference))
				dictValues.Add("Chance to be Crit", ((5f + levelDifference) - CritReduction).ToString()
					+ "%*Exactly enough defense rating/resilience to be uncrittable by bosses.");
			else if (CritReduction < (5f + levelDifference))
				dictValues.Add("Chance to be Crit", ((5f + levelDifference) - CritReduction).ToString()
					+ string.Format("%*CRITTABLE! Short by {0} defense rating or {1} resilience to be uncrittable by bosses.",
					Math.Ceiling(((5f + levelDifference) - CritReduction) * 60f), Math.Ceiling(((5f + levelDifference) - CritReduction) * 39.423f)));
			else
				dictValues.Add("Chance to be Crit", ((5f + levelDifference) - CritReduction).ToString()
					+ string.Format("%*Uncrittable by bosses. {0} defense rating or {1} resilience over the crit cap.",
					Math.Floor(((5f + levelDifference) - CritReduction) * -60f), Math.Floor(((5f + levelDifference) - CritReduction) * -39.423f)));
            dictValues.Add("Chance Crushed", (100 + levelDifference * 4 - DodgePlusMissPlusParryPlusBlock).ToString() + "%");
            dictValues.Add("Overall Points", OverallPoints.ToString());
			dictValues.Add("Mitigation Points", MitigationPoints.ToString());
			dictValues.Add("Survival Points", SurvivalPoints.ToString());

            dictValues["Nature Survival"] = NatureSurvivalPoints.ToString();
            dictValues["Frost Survival"] = FrostSurvivalPoints.ToString();
            dictValues["Fire Survival"] = FireSurvivalPoints.ToString();
            dictValues["Shadow Survival"] = ShadowSurvivalPoints.ToString();
            dictValues["Arcane Survival"] = ArcaneSurvivalPoints.ToString(); 

            float critRating = BasicStats.CritRating;
            if (Calculations.CachedCharacter.ActiveBuffs.Contains("Improved Judgement of the Crusade"))
                critRating -= 66.24f;
            critRating -= 131.4768f; //Base 5%

            dictValues["Strength"] = BasicStats.Strength.ToString();
            dictValues["Attack Power"] = BasicStats.AttackPower.ToString();
            dictValues["Hit Rating"] = BasicStats.HitRating.ToString();
            dictValues["Expertise"] = BasicStats.ExpertiseRating.ToString();
            dictValues["Haste Rating"] = BasicStats.HasteRating.ToString();
            dictValues["Armor Penetration"] = BasicStats.ArmorPenetration.ToString();
            dictValues["Crit Rating"] = critRating.ToString();
            dictValues["Weapon Damage"] = BasicStats.WeaponDamage.ToString();


            dictValues["Limited Threat"] = (ThreatPoints / ThreatScale).ToString();
            dictValues["Unlimited Threat"] = (UnlimitedThreat / ThreatScale).ToString();
            dictValues["Missed Attacks"] = AvoidedAttacks.ToString();

			return dictValues;
		}

		public override float GetOptimizableCalculationValue(string calculation)
		{
			switch (calculation)
			{
				case "Health": return BasicStats.Health;
				case "Mitigation % from Armor": return Mitigation;
                case "Avoidance %": return DodgePlusMissPlusParry;
				case "% Chance to be Crit": return ((5f + (0.2f * (TargetLevel - 70))) - CritReduction);
				case "Nature Survival": return NatureSurvivalPoints;
				case "Fire Survival": return FireSurvivalPoints;
				case "Frost Survival": return FrostSurvivalPoints;
				case "Shadow Survival": return ShadowSurvivalPoints;
				case "Arcane Survival": return ArcaneSurvivalPoints;
			}
			return 0f;
		}
    }

	public class ComparisonCalculationProtWarr : ComparisonCalculationBase
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
