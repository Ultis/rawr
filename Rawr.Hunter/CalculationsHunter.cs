using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Rawr.Hunter
{
	/// <summary>
	/// I started this model project as a way to expand  upon my understanding of hunter mechanics and theorycraft and to give myself a way to 
	/// view gear upgrades inside Rawr since I like using this for my druid.  Most of the modelling in the first version was not done by me but was found in 
	/// wowwiki and from Cheeky's DPS spreadsheet. The pieces that I calculated 
	/// out on my own (and then verified against known sources) are easily overshadowed by those two sources.
	/// </summary>
	[Rawr.Calculations.RawrModelInfo("Hunter", "Inv_Weapon_Bow_07", Character.CharacterClass.Hunter)]
	public class CalculationsHunter : CalculationsBase
    {
		#region Talent Constants
		private const string BEAST_MASTER = "Beast Mastery";
		private const string SURVIVAL = "Survival";
		private const string MARKSMAN = "Marksmanship";
	
		#endregion	
		
		#region Rating Constants
		private const double BASE_HIT_PERCENT = .95;
		private const double HIT_RATING_PER_PERCENT = 15.76;

		private const double AGILITY_PER_CRIT = 40;
		private const double BASE_CRIT_PERCENT = -.0153;
		private const double CRIT_RATING_PER_PERCENT = 22.0765;

		private const double HASTE_RATING_PER_PERCENT = 15.70;

		private const double QUIVER_SPEED_INCREASE = 1.15;
		private const double STEADYSHOT_BASE_DAMAGE = 150;
		private const double STEADYSHOT_BASE_MANA = 110;

		private const double AUTO_SHOT_CAST_TIME = .5;
		private const double STEADY_SHOT_CAST_TIME = 1.5;

		private const int MAX_SHOT_TABLE_LOOPS = 50;
		
		#endregion

		private CalculationOptionsPanelBase calculationOptionsPanel = null;
        private string[] characterDisplayCalculationLabels = null;
        private string[] customChartNames = null;
        private List<Item.ItemType> relevantItemTypes = null;
        private Dictionary<string, Color> subPointNameColors = null;
      
		public CalculationsHunter()
		{
			characterDisplayCalculationLabels = new string[] {
				"Basic Stats:Agility",
				"Basic Stats:Stamina",
				"Basic Stats:Intellect",
				"Basic Stats:Armor",
				"Basic Stats:Crit Rating",
				"Basic Stats:Hit Rating",
				"Basic Stats:Armor Penetration",
				"Basic Stats:Haste Rating",
				"Basic Stats:MP5",				
				"Basic Calculated Stats:Health",
				"Basic Calculated Stats:Mana",
				"Basic Calculated Stats:Hit Percentage",
				"Basic Calculated Stats:Crit Percentage",
				"Basic Calculated Stats:Ranged AP",
				"Basic Calculated Stats:Attack Speed",
				"Pet Stats:Pet Attack Power",
				"Pet Stats:Pet Hit Percentage",
				"Pet Stats:Pet Crit Percentage",
				"Pet Stats:Pet Base DPS",
				"Pet Stats:Pet Special DPS",
				"Pet Stats:Pet KC DPS",
				"Complex Calculated Stats:Hunter Total DPS",
				"Complex Calculated Stats:Pet DPS",
				"Complex Calculated Stats:Overall DPS"
			};

			customChartNames = new string[] { };

			relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
					{
						Item.ItemType.None,
						Item.ItemType.AmmoPouch,
                        Item.ItemType.Arrow,
                        Item.ItemType.Bow,
                        Item.ItemType.Bullet,
                        Item.ItemType.Crossbow,
                        Item.ItemType.Dagger,
						Item.ItemType.FistWeapon,
                        Item.ItemType.Gun,
                        Item.ItemType.Leather,
                        Item.ItemType.Mail,
                        Item.ItemType.OneHandAxe,                        
                        Item.ItemType.OneHandSword,
                        Item.ItemType.Polearm,
                        Item.ItemType.Quiver,
                        Item.ItemType.Staff,                    
                        Item.ItemType.TwoHandAxe,
                        Item.ItemType.TwoHandSword
					});

			subPointNameColors = new Dictionary<string, System.Drawing.Color>();
			subPointNameColors.Add("HunterDps", System.Drawing.Color.FromArgb(0, 128, 255));
			subPointNameColors.Add("PetDps", System.Drawing.Color.FromArgb(255, 100, 0));		
		}

		#region CalculationsBase Overrides

		private string[] _optimizableCalculationLabels = null;
		public override string[] OptimizableCalculationLabels
		{
			get
			{
				if (_optimizableCalculationLabels == null)
					_optimizableCalculationLabels = new string[] {
					"Health",
                    "Mana",
					"Crit Rating",
					"Hit Rating"
					};
				return _optimizableCalculationLabels;
			}
		}

		public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
				return calculationOptionsPanel ?? (calculationOptionsPanel = new CalculationOptionsPanelHunter());
            }
        }

        public override string[] CharacterDisplayCalculationLabels
        {
            get { return characterDisplayCalculationLabels; }
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsHunter();
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationHunter();
        }

        public override string[] CustomChartNames
        {
            get { return customChartNames; }
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {            
            return new ComparisonCalculationBase[0];            
        }

        public override Stats GetRelevantStats(Stats stats)
        {
			return new Stats()
			{
				Agility = stats.Agility, 
				ArmorPenetration = stats.ArmorPenetration,
				AttackPower = stats.AttackPower,
				BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
				BonusArmorMultiplier = stats.BonusArmorMultiplier,
				BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
				BonusCritMultiplier = stats.BonusCritMultiplier,
				BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
				BonusManaPotion = stats.BonusManaPotion,
				BonusPetDamageMultiplier = stats.BonusPetDamageMultiplier,
				BonusPetCritChance = stats.BonusPetCritChance,
				BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
				BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
				LotPCritRating = stats.LotPCritRating,
				Crit = stats.Crit,
				CritRating = stats.CritRating,
				DrumsOfBattle = stats.DrumsOfBattle,
				DrumsOfWar = stats.DrumsOfWar,
				ExposeWeakness = stats.ExposeWeakness,
				HasteRating = stats.HasteRating,
				Hit = stats.Hit,
				HitRating = stats.HitRating,
				Intellect = stats.Intellect,
				Mana = stats.Mana,
				Miss = stats.Miss,
				Mp5 = stats.Mp5,
				ScopeDamage = stats.ScopeDamage,
				ShatteredSunAcumenProc = stats.ShatteredSunAcumenProc,
				ShatteredSunMightProc = stats.ShatteredSunMightProc,
				AshtongueTrinketProc = stats.AshtongueTrinketProc,
				BonusSteadyShotCrit = stats.BonusSteadyShotCrit,
				BonusSteadyShotDamageMultiplier = stats.BonusSteadyShotDamageMultiplier
			};
        }

        public override bool HasRelevantStats(Stats stats)
        {
			return (stats.Agility +
			stats.ArmorPenetration +
			stats.AttackPower +
			stats.BonusAgilityMultiplier +
			stats.BonusArmorMultiplier +
			stats.BonusAttackPowerMultiplier +
			stats.BonusCritMultiplier +
			stats.BonusIntellectMultiplier +
			stats.BonusManaPotion +
			stats.BonusPetDamageMultiplier +
			stats.BonusPhysicalDamageMultiplier +
			stats.BonusStaminaMultiplier +
			stats.BonusPetCritChance +
			stats.LotPCritRating +
			stats.Crit +
			stats.CritRating +
			stats.DrumsOfBattle +
			stats.DrumsOfWar +
			stats.ExposeWeakness +
			stats.HasteRating +
			stats.Health +
			stats.Hit +
			stats.HitRating +
			stats.Intellect +
			stats.Miss +
			stats.Mp5 + 
			stats.ScopeDamage +
			stats.ShatteredSunAcumenProc +
			stats.ShatteredSunMightProc +
			stats.AshtongueTrinketProc +
			stats.BonusSteadyShotCrit +
			stats.BonusSteadyShotDamageMultiplier) > 0;
        }
       
        public override List<Item.ItemType> RelevantItemTypes
        {
            get { return relevantItemTypes; }
        }

        
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get { return subPointNameColors; }
        }

   
        public override Character.CharacterClass TargetClass
        {
            get { return Character.CharacterClass.Hunter; }
        }

		public override bool CanUseAmmo
		{
			get { return true; }
		}

		public override bool IsItemRelevant(Item item)
		{
			bool returnValue;
			if ((item.Slot == Item.ItemSlot.Ranged && item.Type == Item.ItemType.Idol) ||
				//its a feral staff if it has that much AP
				(item.Slot == Item.ItemSlot.TwoHand && item.Type == Item.ItemType.Staff && item.Stats.AttackPower >= 292))
			{
				returnValue = false;
			}
			else if (item.Slot == Item.ItemSlot.Projectile || 
				(item.Slot == Item.ItemSlot.Ranged && (item.Type == Item.ItemType.Gun || item.Type == Item.ItemType.Bow || item.Type == Item.ItemType.Crossbow)))
			{
				returnValue = true;
			}
			else
			{
				returnValue = base.IsItemRelevant(item);
			}
			return returnValue;
		}

		public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
		{
			CharacterCalculationsHunter calculatedStats = new CharacterCalculationsHunter();
			if (character == null)
			{
				return calculatedStats;
			}

			CalculationOptionsHunter options = character.CalculationOptions as CalculationOptionsHunter;
			calculatedStats.BasicStats = GetCharacterStats(character, additionalItem);
			calculatedStats.PetStats = GetPetStats(options, calculatedStats.BasicStats, character);
			if (character.Ranged == null || (character.Ranged.Type != Item.ItemType.Bow && character.Ranged.Type != Item.ItemType.Gun
											&& character.Ranged.Type != Item.ItemType.Crossbow))
			{
				//skip all the calculations if there is no ranged weapon
				return calculatedStats;
			}

			character.EnforceMetagemRequirements = options.EnforceMetaGem;

			#region Remove Any Incorrect Modelling
			bool hasDST = false;
			if (character.Trinket1 != null && character.Trinket1.Name == "Dragonspine Trophy")
			{
				calculatedStats.BasicStats.HasteRating -= character.Trinket1.Stats.HasteRating;
				hasDST = true;
			}
			else if (character.Trinket2 != null && character.Trinket2.Name == "Dragonspine Trophy")
			{
				calculatedStats.BasicStats.HasteRating -= character.Trinket2.Stats.HasteRating;
				hasDST = true;
			}
			#endregion

			#region Base Attack Speed
			//Hasted Speed = Weapon Speed / ( (1+(Haste1 %)) * (1+(Haste2 %)) * (1+(((Haste Rating 1 + Haste Rating 2 + ... )/100)/15.7)) )
			double totalStaticHaste = QUIVER_SPEED_INCREASE * (1 + (calculatedStats.BasicStats.HasteRating / HASTE_RATING_PER_PERCENT / 100));

			//if (character.AllTalents.Trees.ContainsKey(BEAST_MASTER))
			{
				totalStaticHaste = totalStaticHaste * (1 + .04 * character.HunterTalents.SerpentsSwiftness);
			}

			if (character.Ranged != null)
			{
				calculatedStats.BaseAttackSpeed = (float)(character.Ranged.Speed / totalStaticHaste);
			}

			double steadyShotCastTime = STEADY_SHOT_CAST_TIME / totalStaticHaste;

			SimulationResults normalShotsPerSecond = CalculateShotsPerSecond(options, character.Ranged.Speed / totalStaticHaste, steadyShotCastTime);

			#endregion


			#region QuickShots

			SimulationResults quickShotShotsPerSecond = null;
			double quickShotsUpTime = 0;
			double quickShotHaste = 0;
			if (options.Aspect == Aspect.Hawk && /*character.AllTalents.Trees.ContainsKey(BEAST_MASTER) && */character.HunterTalents.ImprovedAspectOfTheHawk > 0)
			{
				quickShotHaste = .03 * character.HunterTalents.ImprovedAspectOfTheHawk;
				quickShotShotsPerSecond = CalculateShotsPerSecond(options, calculatedStats.BaseAttackSpeed / (1 + quickShotHaste), steadyShotCastTime / (1 + quickShotHaste));

				//Quick Shot Uptime From Cheeky's DPS Spreadsheet with special notation - "By Norwest"
				double shotsInProc = (Math.Floor((12f - normalShotsPerSecond.autoShotsPerSecond) / normalShotsPerSecond.autoShotsPerSecond) + 1) * calculatedStats.BasicStats.Hit;
				double shotsInReProc = Math.Floor(12 / quickShotShotsPerSecond.autoShotsPerSecond) * calculatedStats.BasicStats.Hit;
				double reprocChanceInitial = 1 - Math.Pow(.9, shotsInProc);
				double reprocChanceSub = 1 - Math.Pow(.9, shotsInReProc);
				double AvgShotBeforeFirstReProc = ((1 - Math.Pow(0.9, (shotsInProc + 1))) / Math.Pow(.1, 2) - (shotsInProc + 1) * Math.Pow(0.9, shotsInProc) / .1) / reprocChanceInitial * 0.1;
				double AvgShotBeforeNthReProc = ((1 - Math.Pow(0.9, (shotsInReProc + 1))) / Math.Pow(.1, 2) - (shotsInReProc + 1) * Math.Pow(0.9, shotsInReProc) / .1) / reprocChanceSub * 0.1;
				double avgQuickShotChain = shotsInProc * (1 - reprocChanceInitial) + reprocChanceInitial * (1 - reprocChanceSub) * (AvgShotBeforeNthReProc * reprocChanceSub / Math.Pow((1 - reprocChanceSub), 2) + (AvgShotBeforeFirstReProc + shotsInReProc) / (1 - reprocChanceSub));
				quickShotsUpTime = avgQuickShotChain / (avgQuickShotChain + 10);
			}
			else
			{
				quickShotShotsPerSecond = new SimulationResults();
			}

			#endregion

			#region DST
			double dstUptime = 0;
			double dstQuickShotsUpTime = 0;
			double dstHaste = (325 / HASTE_RATING_PER_PERCENT / 100);
			SimulationResults dstShotsPerSecond = new SimulationResults();
			SimulationResults dstQuickShotsPerSecond = new SimulationResults();
			if (hasDST)
			{
				//assumes a 1PPM for now
				double DSTPPM = 1;
				double DSTDuration = 10;
				double DSTCooldown = 20;
				double timeForAuto = (1 / (normalShotsPerSecond.autoShotsPerSecond / (60 / DSTPPM))) * normalShotsPerSecond.autoShotsPerSecond;
				double timeForSpecial = (1 / (character.Ranged.Speed / (60 / DSTPPM))) * normalShotsPerSecond.steadyShotsPerSecond;
				double normalUptimeWeightedAverage = 1 / ((1 / timeForAuto) + (1 / timeForSpecial));
				dstUptime = DSTDuration / (normalUptimeWeightedAverage + DSTCooldown);

				dstShotsPerSecond = CalculateShotsPerSecond(options, calculatedStats.BaseAttackSpeed / (1 + dstHaste), steadyShotCastTime / (1 + dstHaste));

				if (options.Aspect == Aspect.Hawk && quickShotShotsPerSecond.autoShotsPerSecond > 0)
				{
					double timeForQSAuto = (1 / (quickShotShotsPerSecond.autoShotsPerSecond / (60 / DSTPPM))) * quickShotShotsPerSecond.autoShotsPerSecond;
					double timeForQSSpecial = (1 / (character.Ranged.Speed / (60 / DSTPPM))) * quickShotShotsPerSecond.steadyShotsPerSecond;
					double qsWeightedAverage = 1 / ((1 / timeForQSAuto) + (1 / timeForQSSpecial));
					dstQuickShotsUpTime = DSTDuration / (DSTCooldown + qsWeightedAverage);
					dstQuickShotsPerSecond = CalculateShotsPerSecond(options, calculatedStats.BaseAttackSpeed / (1 + dstHaste) / (1 + quickShotHaste), steadyShotCastTime / (1 + dstHaste) / (1 + quickShotHaste));
				}
			}
			#endregion [DST]

			double weightedTotalShotsPerSecond = normalShotsPerSecond.totalShotsPerSecond * (1 - quickShotsUpTime) * (1 - dstUptime) + 
										  quickShotShotsPerSecond.totalShotsPerSecond * quickShotsUpTime * (1 - dstQuickShotsUpTime) +
										  dstShotsPerSecond.totalShotsPerSecond * dstUptime * (1 - quickShotsUpTime) + 
										  dstQuickShotsPerSecond.totalShotsPerSecond *  quickShotsUpTime * dstQuickShotsUpTime;

			double weightedTotalSteadyShotsPerSecond = normalShotsPerSecond.steadyShotsPerSecond * (1 - quickShotsUpTime) * (1 - dstUptime) +
										  quickShotShotsPerSecond.steadyShotsPerSecond * quickShotsUpTime * (1 - dstQuickShotsUpTime) +
										  dstShotsPerSecond.steadyShotsPerSecond * dstUptime * (1 - quickShotsUpTime) +
										  dstQuickShotsPerSecond.steadyShotsPerSecond * quickShotsUpTime * dstQuickShotsUpTime;


			#region OnProc Modelling +Stats

			double addtionalAPFromProcs = 0;

			//Fix AP given, won't fix in display but calculations will be correct
			{
				double rawrHourglassAP = 0;
				if (character.Trinket1 != null && character.Trinket1.Name == "Hourglass of the Unraveller")
				{
					rawrHourglassAP = character.Trinket1.Stats.AttackPower;
				}
				else if (character.Trinket2 != null && character.Trinket2.Name == "Hourglass of the Unraveller")
				{
					rawrHourglassAP = character.Trinket2.Stats.AttackPower;
				}

				if (rawrHourglassAP > 0)
				{
					//AP Boost * Uptime
					double trueAPGain = 300 * (10 / ((45 + (1 / (calculatedStats.BasicStats.Crit * .1)) * (1/weightedTotalShotsPerSecond))));
					addtionalAPFromProcs += (trueAPGain - rawrHourglassAP);
				}
			}
			if (character.Ranged.Name == "Don Santos' Famous Hunting Rifle")
			{
				addtionalAPFromProcs += Math.Floor(250 * (1 - Math.Pow(.95, weightedTotalShotsPerSecond * 10)));
			}

			if (calculatedStats.BasicStats.ShatteredSunMightProc > 0 && options.ScryerAldor == Faction.Aldor)
			{
				//AP * (Duration / (cooldown + (1/duration * weightedShotsPerSecond)))
				addtionalAPFromProcs += (200 * (10 / (45 + ((1 / .10) * weightedTotalShotsPerSecond))));
			}

			if (calculatedStats.BasicStats.AshtongueTrinketProc > 0)
			{
				//AP Boost * ((1 - chance to proc)^(uptime / steady shots per sec))
				addtionalAPFromProcs += (275 * (1 - Math.Pow(.85, 8 / weightedTotalSteadyShotsPerSecond)));
			}
			#endregion

			#region Pet
			double petFocusPerSecond = 0;
			{
				double petFocusRegenPer4 = 24;
				//if (character.AllTalents.Trees.ContainsKey(BEAST_MASTER))
				{//Bestial Discipline
					petFocusRegenPer4 += (12 * character.HunterTalents.BestialDiscipline);
				}
				//if (character.AllTalents.Trees.ContainsKey(MARKSMAN))
				{
					//Go for the Throat
					petFocusRegenPer4 += ((weightedTotalShotsPerSecond * 4 * calculatedStats.BasicStats.Crit) * (25 * character.HunterTalents.GoForTheThroat));
				}
				petFocusPerSecond = petFocusRegenPer4 / 4f;
			}

			//TODO: Allow different ranks of special abilities and add remaining special abilities
			PetSpecialAttackData[] petSpecialAttackData = null;
			double petSpecialAttackSpeed = 0;

			if (options.PetAttackSequence != null && options.PetAttackSequence.Length > 0)
			{
				petSpecialAttackData = new PetSpecialAttackData[options.PetAttackSequence.Length];
				double petFocusPerSecondLeft = petFocusPerSecond;
				for (int i = 0; i < options.PetAttackSequence.Length; i++)
				{
					PetSpecialAttackData data = new PetSpecialAttackData();
					data.petAttack = options.PetAttackSequence[i];
					switch (data.petAttack)
					{
						case PetAttacks.Bite:
							data.FocusUsed = 35;
							data.FocusPerSecond = 3.5;
							data.CoolDown = 10;
							break;

						case PetAttacks.Claw:
							data.FocusUsed = 25;
							data.FocusPerSecond = 16.67;
							data.CoolDown = 1.5;
							break;

                        case PetAttacks.None:
                            data.FocusUsed = 0;
                            data.FocusPerSecond = 0;
                            data.CoolDown = 0;
                            break;

                        case PetAttacks.FireBreath:
                            data.FocusUsed = 50;
                            data.FocusPerSecond = 5;
                            data.CoolDown = 10;
                            break;

                        case PetAttacks.FuriousHowl:
                            data.FocusUsed = 60;
                            data.FocusPerSecond = 6;
                            data.CoolDown = 10;
                            break;

                        case PetAttacks.Gore:
                            data.FocusUsed = 25;
                            data.FocusPerSecond = 16.67;
                            data.CoolDown = 1.5;
                            break;

                        case PetAttacks.Growl:
                            data.FocusUsed = 15;
                            data.FocusPerSecond = 3;
                            data.CoolDown = 5;
                            break;

                        case PetAttacks.LightningBreath:
                            data.FocusUsed = 50;
                            data.FocusUsed = 33.33;
                            data.CoolDown = 1.5;
                            break;

                        case PetAttacks.PoisonSpit:
                            data.FocusUsed = 35;
                            data.FocusUsed = 3.5;
                            data.CoolDown = 10;
                            break;

                        case PetAttacks.ScorpidPoison:
                            data.FocusUsed = 30;
                            data.FocusPerSecond = 7.5;
                            data.CoolDown = 4;
                            break;

                        case PetAttacks.Screech:
                            data.FocusUsed = 20;
                            data.FocusPerSecond = 13.33;
                            data.CoolDown = 1.5;
                            break;

                        case PetAttacks.Thunderstomp:
                            data.FocusUsed = 60;
                            data.FocusPerSecond = 1;
                            data.CoolDown = 60;
                            break;

                        case PetAttacks.Cower:
                            data.FocusUsed = 15;
                            data.FocusPerSecond = 10;
                            data.CoolDown = 1.5;
                            break;
                    }
					if (i == 0)
					{
						data.AttackRate = petFocusPerSecondLeft >= (data.FocusPerSecond) ? data.CoolDown : data.FocusUsed / petFocusPerSecondLeft;
						data.MaximumFrequency = 1.5;
						data.AttackRateFromFocus = data.FocusUsed / petFocusPerSecondLeft;
					}
					else
					{
						data.AttackRateFromFocus = data.FocusUsed / petFocusPerSecondLeft;
						data.MaximumFrequency = 1 / ((1 / petSpecialAttackData[i - 1].MaximumFrequency) - (1 / petSpecialAttackData[i - 1].AttackRate));
						//find largest value
						data.AttackRate = data.AttackRateFromFocus;
						if (data.MaximumFrequency > data.AttackRate)
						{
							data.AttackRate = data.MaximumFrequency;
						}
						if (data.CoolDown > data.AttackRate)
						{
							data.AttackRate = data.CoolDown;
						}
					}
					petFocusPerSecondLeft -= data.FocusPerSecond;
					petSpecialAttackData[i] = data;
					petSpecialAttackSpeed += 1 / data.AttackRate;
				}

				petSpecialAttackSpeed = 1 / petSpecialAttackSpeed;
			}

			//Kill Command Frequency Factors into a lot of other pet calculations so needs to be determined very early.
			double petKillCommandFrequency = 0;
			{
				double chanceKillCommandReady = 1 - (Math.Pow(1 - calculatedStats.BasicStats.Crit, 5 * weightedTotalShotsPerSecond));
				double shotsToCrit = 1 / calculatedStats.BasicStats.Crit;
				double timefromPrevKillCommand = 5 + shotsToCrit / weightedTotalShotsPerSecond;
				//( chance Kill Command Ready * kill command cooldown) + ((1 - chance kill command ready) * weightedShotsPerSec))
				double averageTimeToUseKillCommand = (chanceKillCommandReady * 5) + ((1 - chanceKillCommandReady) * timefromPrevKillCommand);
				petKillCommandFrequency = 1 / averageTimeToUseKillCommand;
			}

			double petEffectiveAttackSpeed = 2;
			//if (character.AllTalents.Trees.ContainsKey(BEAST_MASTER))
			{
				//Serpent's Swiftness
				petEffectiveAttackSpeed = petEffectiveAttackSpeed / (1 + (.04f * character.HunterTalents.SerpentsSwiftness));
			}

			//TODO: Make a Pet Talent Form and pull this value from it.
			//Cobra Reflexes 
			petEffectiveAttackSpeed = petEffectiveAttackSpeed / 1.3;


			if (/*character.AllTalents.Trees.ContainsKey(BEAST_MASTER) && */character.HunterTalents.Frenzy > 0)
			{//Frenzy Calculations
				double frenzyChance = calculatedStats.PetStats.Crit * character.HunterTalents.Frenzy * .2;
				double frenzySpeed = petEffectiveAttackSpeed / 1.3;
				double killCommandSpeed = 1 / petKillCommandFrequency;
				double numberOfAttacksInFrenzy = Math.Floor(8 / frenzySpeed) + 8 / killCommandSpeed + ((petSpecialAttackSpeed > 0) ? (8 / petSpecialAttackSpeed) : 0);
				double frenzyUptime = 1 - Math.Pow(1 - frenzyChance, numberOfAttacksInFrenzy);

				petEffectiveAttackSpeed = (petEffectiveAttackSpeed / 1.3 * frenzyUptime) + (petEffectiveAttackSpeed * (1 - frenzyUptime));
			}

			double ferociousInspirtationEffectBenefit = 0;
			if (/*character.AllTalents.Trees.ContainsKey(BEAST_MASTER) && */character.HunterTalents.FerociousInspiration > 0)
			{
				double fiEffect = .01 * character.HunterTalents.FerociousInspiration;
				double fiCommandUpTime = 1 - Math.Pow(1 - calculatedStats.PetStats.Crit, (10 / petEffectiveAttackSpeed) + 10 / (1 / petKillCommandFrequency) + ((petSpecialAttackSpeed > 0) ? (10 / petSpecialAttackSpeed) : 0));
				ferociousInspirtationEffectBenefit = fiEffect * fiCommandUpTime;
			}


			double glancingBlowDamage = options.TargetLevel > 72 ? .94f : 1f;

			double petDamageAdjustment = 0;
			{
				petDamageAdjustment = 2 * calculatedStats.PetStats.Crit;
				petDamageAdjustment += (calculatedStats.PetStats.Hit - calculatedStats.PetStats.Crit - (.05 + (.0004 * (options.TargetLevel * 5 - 350))));
				//if (character.AllTalents.Trees.ContainsKey(BEAST_MASTER))
				{
					//Unleashed Fury
					petDamageAdjustment = (petDamageAdjustment * (character.HunterTalents.UnleashedFury * .04f + 1));

					//Beastial Wrath
					if (character.HunterTalents.BestialWrath > 0)
					{
						petDamageAdjustment = petDamageAdjustment * 1.075f;
					}
				}
				//Pet Mood
				petDamageAdjustment = petDamageAdjustment * 1.25f * (1 + calculatedStats.BasicStats.BonusPetDamageMultiplier);

				petDamageAdjustment = petDamageAdjustment * (1 + ferociousInspirtationEffectBenefit);

				//TODO: Pet Family Options (currently cat / rav)
				petDamageAdjustment = petDamageAdjustment * 1.1f;
			}

			double petDPS = 0;
			{
				//TODO: Pull from a pet talent store
				double cobraReflexesDamageReduction = .86;

				//White Attacks
				double petEffectiveArmor = options.TargetArmor - calculatedStats.PetStats.ArmorPenetration;
				//From WOWWiki DR% = Armor / (Armor + (467.5 * AttackerLevel - 22167.5)
				double petArmorDamageReductionPercentage = petEffectiveArmor / (petEffectiveArmor + (467.5 * 70 - 22167.5));

				double APDamage = (calculatedStats.PetStats.AttackPower + (addtionalAPFromProcs * .22)) / 7;
				//(average damage + ap damage) * pet damage adjustment * cobra reflexes reduction * (1 - armor mit) * glancing blows modifier
				double petTotalDamage = (60 + APDamage) * petDamageAdjustment * cobraReflexesDamageReduction * (1 - petArmorDamageReductionPercentage) * glancingBlowDamage;
				petDPS = petTotalDamage / petEffectiveAttackSpeed;
				calculatedStats.PetBaseDPS = petDPS;
				//Kill Command
				//TODO: Option to not use Kill Command
				if (true)
				{
					//attack damage + kill command base damage
					double totalKillCommandBaseDamage = ((APDamage + 60) * cobraReflexesDamageReduction) + 127;

					double totalKillCommandCritChance = calculatedStats.PetStats.Crit;
					//if (character.AllTalents.Trees.ContainsKey(BEAST_MASTER))
					{//Focus Fire
						totalKillCommandCritChance += (.1 * character.HunterTalents.FocusedFire);
					}
					//(crit damage * crit percent) + (hit percent - crit percent - dodge percent)
					double critMissKillCommandAdjustment = (totalKillCommandCritChance * 2) + (calculatedStats.PetStats.Hit - totalKillCommandCritChance - (.05 + (.0004 * (options.TargetLevel * 5 - 350))));

					//TODO: Pet Family from options
					//base damage * mood * FI * pet family modifier * armor reduction * Avg. BW
					double totalKillCommandDamage = critMissKillCommandAdjustment * 1.25 * (1 + ferociousInspirtationEffectBenefit) * 1.1 * (1 - petArmorDamageReductionPercentage);
					//if (character.AllTalents.Trees.ContainsKey(BEAST_MASTER))
					{
						//Unleashed Fury
						totalKillCommandDamage = (totalKillCommandDamage * (character.HunterTalents.UnleashedFury * .04f + 1));

						//Beastial Wrath
						if (character.HunterTalents.BestialWrath > 0)
						{
							totalKillCommandDamage = totalKillCommandDamage * 1.075f;
						}
					}
					totalKillCommandDamage = totalKillCommandDamage * totalKillCommandBaseDamage;
					calculatedStats.PetKillCommandDPS = (totalKillCommandDamage / (1 / petKillCommandFrequency));
					petDPS += calculatedStats.PetKillCommandDPS;
				}
				if (petSpecialAttackData != null)
				{
					for (int i = 0; i < petSpecialAttackData.Length; i++)
					{
						calculatedStats.PetSpecialDPS += CalculatePetSpecialAttackDPS(petSpecialAttackData[i], petDamageAdjustment, petArmorDamageReductionPercentage);
					}
					petDPS += calculatedStats.PetSpecialDPS;
				}
			}
			calculatedStats.PetDpsPoints = (float)petDPS;

			#endregion

			#region Armor Penetration
			//stats aromor penetration includes sunders
			double effectiveArmor = options.TargetArmor - calculatedStats.BasicStats.ArmorPenetration;
			//TODO: Beast Lord Proc and Maddness of Betrayer Proc

			//From WOWWiki DR% = Armor / (Armor + (467.5 * AttackerLevel - 22167.5)
			double armorDamageReductionPercentage = effectiveArmor / (effectiveArmor + (467.5 * 70 - 22167.5));

			#endregion

			#region RAP Against Target
			double effectiveRAPAgainstMob = calculatedStats.BasicStats.RangedAttackPower + addtionalAPFromProcs;
			//hunters mark (just assume max AP)
			//TODO: Add expose weakness buff
			effectiveRAPAgainstMob += (440 * (1 + calculatedStats.BasicStats.BonusRangedAttackPowerMultiplier));

			#endregion

			#region Weapon Damage
			double weaponDamageAverage = 0;
			double weaponDPS = 0;
			double ammoDamage = 0;
			if (character.Ranged != null && character.Projectile != null)
			{
				weaponDamageAverage = (float)(character.Ranged.MinDamage + character.Ranged.MaxDamage) / 2f;
				weaponDPS = weaponDamageAverage / character.Ranged.Speed;
				ammoDamage = character.Ranged.Speed * ((float)(character.Projectile.MaxDamage + character.Projectile.MinDamage) / 2f);
			}
			#endregion

			#region Critical Hit Damage
			double criticalHitDamage = 1;
			//if (character.AllTalents.Trees.ContainsKey(MARKSMAN))
			{//Mortal Shots
				criticalHitDamage += (.06 * character.HunterTalents.MortalShots);
			}
			criticalHitDamage = criticalHitDamage * (1 + calculatedStats.BasicStats.BonusCritMultiplier);
			//TODO: Slaying bonus talents & Target Resilience

			#endregion


			#region TalentAdjustment

			double talentAdjustment = 1;
			{
				//TODO: Slaying, Ferocious Inspiration actual cals;
				double rws = 1; double ff = 1; double bw = 1f; double fi = 1.0282; double slaying = 1;

				//if (character.AllTalents.Trees.ContainsKey(MARKSMAN))
				{//Ranged Weapon Specialization
					rws = 1 + (.01 * character.HunterTalents.RangedWeaponSpecialization);
				}
				//if (character.AllTalents.Trees.ContainsKey(BEAST_MASTER))
				{//Focused Fire
					ff = 1 + (.01 * character.HunterTalents.FocusedFire);

					if (character.HunterTalents.TheBeastWithin > 0)
					{//Beast Within - 15% uptime (assumes always popping it when cd is up) with a 10% damage bonus
						bw = 1.015;
					}
				}


				talentAdjustment = rws * ff * bw * fi * slaying;
			}

			#endregion

			#region Auto Shot
			double baseAutoShotDamage = 0;
			double averageAutoShotDamage = 0;

			if (character.Ranged != null)
			{
				#region Shot Damage Adjustments (crit / hit / miss)
				double critMissAdj = (calculatedStats.BasicStats.Crit * criticalHitDamage + 1) * calculatedStats.BasicStats.Hit;

				double totalDamageAdjustmentAutoShot = critMissAdj * talentAdjustment;
				#endregion

				baseAutoShotDamage = weaponDamageAverage + ammoDamage
									+ calculatedStats.BasicStats.ScopeDamage
									+ (effectiveRAPAgainstMob / 14 * character.Ranged.Speed);

				averageAutoShotDamage = baseAutoShotDamage * totalDamageAdjustmentAutoShot;
			}


			#endregion

			#region Steady Shot
			double baseSteadyShotDamage = 0;
			double averageSteadyShotDamage = 0;
			if (weaponDPS > 0)
			{
				//weapon damage might be wrong, might include too much damage for ss
				baseSteadyShotDamage = STEADYSHOT_BASE_DAMAGE + calculatedStats.BasicStats.WeaponDamage + (effectiveRAPAgainstMob * .2) + (weaponDPS * 2.8);

				#region Shot Damage Adjustments (crit / hit / miss)
				double critMissAdj = ((calculatedStats.BasicStats.Crit + calculatedStats.BasicStats.BonusSteadyShotCrit) * criticalHitDamage + 1) * calculatedStats.BasicStats.Hit;

				double totalDamageAdjustmentStreadyShot = critMissAdj * talentAdjustment * (1+calculatedStats.BasicStats.BonusSteadyShotDamageMultiplier);
				#endregion

				averageSteadyShotDamage = baseSteadyShotDamage * totalDamageAdjustmentStreadyShot;
			}
			#endregion

			SimulationResults normalShotRotation = CalculateShotRotationDPS(options, calculatedStats.BaseAttackSpeed,
																averageAutoShotDamage * (1 - armorDamageReductionPercentage),
																averageSteadyShotDamage * (1 - armorDamageReductionPercentage),
																steadyShotCastTime);

			//create empty dps simulations with all zeros
			SimulationResults dstDPS = new SimulationResults();
			SimulationResults dstQuickShotsDPS = new SimulationResults();
			SimulationResults quickShotRotation = new SimulationResults();
			if (hasDST)
			{

				dstDPS = CalculateShotRotationDPS(options, calculatedStats.BaseAttackSpeed / (1 + dstHaste),
																averageAutoShotDamage * (1 - armorDamageReductionPercentage),
																averageSteadyShotDamage * (1 - armorDamageReductionPercentage),
																steadyShotCastTime / (1 + dstHaste));

				if (options.Aspect == Aspect.Hawk)
				{
					dstQuickShotsDPS = CalculateShotRotationDPS(options, calculatedStats.BaseAttackSpeed / (1 + dstHaste) / (1 + quickShotHaste),
																averageAutoShotDamage * (1 - armorDamageReductionPercentage),
																averageSteadyShotDamage * (1 - armorDamageReductionPercentage),
																steadyShotCastTime / (1 + dstHaste) / (1 + quickShotHaste));
				}
			}

			if (options.Aspect == Aspect.Hawk)
			{
				quickShotRotation = CalculateShotRotationDPS(options, calculatedStats.BaseAttackSpeed / (1 + quickShotHaste),
																	averageAutoShotDamage * (1 - armorDamageReductionPercentage),
																	averageSteadyShotDamage * (1 - armorDamageReductionPercentage),
																	steadyShotCastTime / (1 + quickShotHaste));
			}

			double totalHunterDPS = normalShotRotation.dps * (1 - quickShotsUpTime) * (1 - dstUptime) + 
										  quickShotRotation.dps * quickShotsUpTime * (1 - dstQuickShotsUpTime) +
										  dstDPS.dps * dstUptime * (1 - quickShotsUpTime) + 
										  dstQuickShotsDPS.dps *  quickShotsUpTime * dstQuickShotsUpTime;

			


			#region OnProc +DPS
			if (calculatedStats.BasicStats.ShatteredSunMightProc > 0 && options.ScryerAldor == Faction.Scryer)
			{
				//cooldown + (1/procate)* weightedShotsPerSec
				double timePerProc = 45 + ((1 / (.1 + 2 / 300)) * weightedTotalShotsPerSecond);
				//damage * modifier * (1 + critchance)
				double averageDamage = 350 * (1 + calculatedStats.BasicStats.Crit) * talentAdjustment;

				totalHunterDPS += averageDamage / timePerProc;
			}

			#endregion

			calculatedStats.HunterDpsPoints = (float)totalHunterDPS;
			calculatedStats.OverallPoints = calculatedStats.HunterDpsPoints + calculatedStats.PetDpsPoints;

			return calculatedStats;
		}

		public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
			Stats statsRace = GetRaceStats(character.Race);
			Stats statsBaseGear = GetItemStats(character, additionalItem);
			Stats statsEnchants = GetEnchantsStats(character);
			Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
			Stats statsTalents = GetBaseTalentStats(character.HunterTalents);
			Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;
			statsGearEnchantsBuffs.Agility += statsGearEnchantsBuffs.AverageAgility;

			CalculationOptionsHunter options = character.CalculationOptions as CalculationOptionsHunter;
			if (options == null)
			{
				options = new CalculationOptionsHunter();
				character.CalculationOptions = options;
			}
			int targetDefence = 5*options.TargetLevel;
			

			Stats statsTotal = new Stats();
			statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier)) - 1;
			statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier) * (1 + statsTalents.BonusAgilityMultiplier)) - 1;
			statsTotal.BonusIntellectMultiplier = ((1 + statsRace.BonusIntellectMultiplier) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier) * (1 + statsTalents.BonusIntellectMultiplier)) - 1;
			statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;
			statsTotal.BonusSpellPowerMultiplier = ((1 + statsRace.BonusSpellPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpellPowerMultiplier)) - 1;
			statsTotal.BonusArcaneSpellPowerMultiplier = ((1 + statsRace.BonusArcaneSpellPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusArcaneSpellPowerMultiplier)) - 1;
			statsTotal.BonusPetDamageMultiplier = ((1 + statsGearEnchantsBuffs.BonusPetDamageMultiplier) * (1 + statsRace.BonusPetDamageMultiplier)) - 1;
			statsTotal.BonusSteadyShotDamageMultiplier = ((1 + statsGearEnchantsBuffs.BonusSteadyShotDamageMultiplier) * (1 + statsRace.BonusSteadyShotDamageMultiplier) * (1 + statsTalents.BonusSteadyShotDamageMultiplier)) - 1;

			statsTotal.Agility = (statsRace.Agility + statsGearEnchantsBuffs.Agility) * (1 + statsTotal.BonusAgilityMultiplier);
			statsTotal.Intellect = (statsRace.Intellect + statsGearEnchantsBuffs.Intellect) * (1 + statsTotal.BonusIntellectMultiplier);
			statsTotal.Stamina = (statsRace.Stamina + statsGearEnchantsBuffs.Stamina) * (1 + statsTotal.BonusStaminaMultiplier); 
			
			statsTotal.Resilience = statsRace.Resilience + statsGearEnchantsBuffs.Resilience;
			statsTotal.Armor = (float)Math.Round((statsGearEnchantsBuffs.Armor + statsRace.Armor + (statsTotal.Agility * 2f)) * (1 + statsBuffs.BonusArmorMultiplier));
			statsTotal.Miss = statsBuffs.Miss;
			statsTotal.ArmorPenetration = statsRace.ArmorPenetration + statsGearEnchantsBuffs.ArmorPenetration;
			statsTotal.BloodlustProc = statsRace.BloodlustProc + statsGearEnchantsBuffs.BloodlustProc;
			statsTotal.BonusCritMultiplier = ((1 + statsRace.BonusCritMultiplier) * (1 + statsGearEnchantsBuffs.BonusCritMultiplier)) - 1;
			statsTotal.CritRating = (float)Math.Floor((decimal)statsRace.CritRating + (decimal)statsGearEnchantsBuffs.CritRating + (decimal)statsRace.LotPCritRating + (decimal)statsGearEnchantsBuffs.LotPCritRating);
			statsTotal.HasteRating = statsRace.HasteRating + statsGearEnchantsBuffs.HasteRating;
			statsTotal.HitRating = (float)Math.Floor((decimal)statsRace.HitRating + (decimal)statsGearEnchantsBuffs.HitRating);
			statsTotal.ExposeWeakness = statsRace.ExposeWeakness + statsGearEnchantsBuffs.ExposeWeakness;
			statsTotal.Bloodlust = statsRace.Bloodlust + statsGearEnchantsBuffs.Bloodlust;
			statsTotal.ShatteredSunMightProc = statsRace.ShatteredSunMightProc + statsGearEnchantsBuffs.ShatteredSunMightProc;
			statsTotal.Mp5 = statsRace.Mp5 + statsGearEnchantsBuffs.Mp5;
			statsTotal.BonusPetCritChance = statsGearEnchantsBuffs.BonusPetCritChance;
			statsTotal.ScopeDamage = statsGearEnchantsBuffs.ScopeDamage;
			statsTotal.AshtongueTrinketProc = statsGearEnchantsBuffs.AshtongueTrinketProc;
			statsTotal.BonusSteadyShotCrit = statsGearEnchantsBuffs.BonusSteadyShotCrit;
			

			if (options.Aspect == Aspect.Viper)
			{
				statsTotal.Mp5 += (statsTotal.Intellect / 4f);
				//TODO: T6 Bonus here as well.
			}

			//Begin non Base Stats.

			statsTotal.Mana = (float)Math.Round(statsRace.Mana + 15f * (statsTotal.Intellect-10) + statsGearEnchantsBuffs.Mana);
			
			statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + ( (statsTotal.Stamina-10) * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
			//add up health talents, based on health, not base stat, so cannot roll it into Talent Stats
			//if (character.AllTalents.Trees.ContainsKey(BEAST_MASTER))
			{//endurance training
				statsTotal.Health += (float)Math.Round((statsTotal.Health * character.HunterTalents.EnduranceTraining * .01f));
			}
			//if (character.AllTalents.Trees.ContainsKey(SURVIVAL))
			{//survivalist
				statsTotal.Health += (float)Math.Round((statsTotal.Health * character.HunterTalents.Survivalist * .02f));
			}
			
			
			statsTotal.Hit = (float)(BASE_HIT_PERCENT + (statsTotal.HitRating / (HIT_RATING_PER_PERCENT*100)) 
								- (statsTotal.Miss / 100) 
								+ (character.Race == Character.CharacterRace.Draenei ? .01 : 0)
								+ statsTalents.Hit);

			//=IF((B54-B53) - 10 > 0,  ((B54-B53) -10) * -0.004 - 0.02, (B54-B53) * -0.001)
			if (targetDefence > 360)
			{
				statsTotal.Hit = statsTotal.Hit - (targetDefence - 360) * .004f - .02f;
			} else {
				statsTotal.Hit -= ((targetDefence - 350) * .001f);
			}
			if (statsTotal.Hit > 1)
			{
				statsTotal.Hit = 1;
			}

			if (character.Ranged != null &&
				((character.Race == Character.CharacterRace.Dwarf && character.Ranged.Type == Item.ItemType.Gun) ||
				(character.Race == Character.CharacterRace.Troll && character.Ranged.Type == Item.ItemType.Bow)))
			{
				statsTotal.CritRating += (float)Math.Floor(CRIT_RATING_PER_PERCENT);
			}
			
			statsTotal.Crit = (float)(BASE_CRIT_PERCENT + (statsTotal.Agility / AGILITY_PER_CRIT / 100) 
								+ (statsTotal.CritRating / CRIT_RATING_PER_PERCENT / 100)
								+ ((350 - targetDefence) * 0.04 / 100)
								+ statsTalents.Crit);

		
			
			//TODO:Target Resilience
			//TODO:Darkmoon Card: Wrath


			#region Base RAP
			float RAP = 140f + (statsTotal.Agility - 10);
		

			//TODO: Better model on proc events (current model is a static AP gain on the item, not based on shot speed, crit chance, etc)
			RAP += statsGearEnchantsBuffs.AttackPower + statsGearEnchantsBuffs.RangedAttackPower;

			RAP += statsTalents.AttackPower + statsTalents.RangedAttackPower;
			//TODO: Option : Average out Orc Blood Fury
			if (character.Race == Character.CharacterRace.Orc)
			{
				RAP += 35.25f;
			}

			if (options.Aspect == Aspect.Hawk)
			{
				RAP += 155f;
			}

			//if (character.AllTalents.Trees.ContainsKey(MARKSMAN))
			{
				//Careful Aim
				RAP += (.015f * character.HunterTalents.CarefulAim * statsTotal.Intellect);
			}


			RAP += (RAP * statsTalents.BonusRangedAttackPowerMultiplier);

			statsTotal.RangedAttackPower = RAP;
			#endregion 

			return statsTotal;
		}

		#endregion //overrides 

        #region Private Functions

		#region Shot Rotation DPS
		private SimulationResults CalculateAutoShotOnlyDPS(double weaponSpeed, double autoShotDamage)
		{
			double totalDamage = 0;
			double currentTime = 0;

			for (int i = 0; i < MAX_SHOT_TABLE_LOOPS; i++)
			{
				totalDamage += autoShotDamage;
				currentTime += weaponSpeed;
			}
			SimulationResults ret = new SimulationResults();
			ret.dps = totalDamage / currentTime;;
			ret.totalShotsPerSecond = MAX_SHOT_TABLE_LOOPS / currentTime;
			ret.autoShotsPerSecond = ret.totalShotsPerSecond;
			return ret;

		}

		private SimulationResults CalculateOneToOneDPS(double weaponSpeed, double autoShotDamage, double steadyShotDamage, double steadyShotCastTime, float latency)
		{
			double totalDamage = 0;
			double currentTime = 0;
			double lastAutoShotTime = 0;
			double autoShotsFired = 0;
			double steadyShotsFired = 0;
			double timeLeftTillNextAutoShot = 0;
			double gcdTimer = 0;

			//start off with an autoshot.
			totalDamage += autoShotDamage;
			autoShotsFired++;
			currentTime += AUTO_SHOT_CAST_TIME;

			for (int i = 1; i < MAX_SHOT_TABLE_LOOPS; i++)
			{
				if (i % 2 == 0)
				{
					totalDamage += autoShotDamage;
					timeLeftTillNextAutoShot = weaponSpeed - (currentTime - lastAutoShotTime);
					if (timeLeftTillNextAutoShot > 0)
					{
						currentTime += timeLeftTillNextAutoShot;
					}
					lastAutoShotTime = currentTime;
					currentTime += AUTO_SHOT_CAST_TIME;
					autoShotsFired++;
				}
				else
				{

					totalDamage += steadyShotDamage;
					if (currentTime < gcdTimer)
					{
						currentTime = gcdTimer;
					}
					currentTime += steadyShotCastTime;

					///GCD logic from spreadsheet given autoshot's wierdness given a handweaved or spammed macro.  
					if (gcdTimer < lastAutoShotTime)
					{
						gcdTimer = lastAutoShotTime + 1.5;
					}
					else
					{
						gcdTimer += 1.5 + latency;
					}

					steadyShotsFired++;
				}
			}
			timeLeftTillNextAutoShot = weaponSpeed - (currentTime - lastAutoShotTime);
			if (timeLeftTillNextAutoShot > 0)
			{
				currentTime += timeLeftTillNextAutoShot;
			}
			SimulationResults ret = new SimulationResults();
			ret.dps = totalDamage / currentTime; ;
			ret.totalShotsPerSecond = MAX_SHOT_TABLE_LOOPS / currentTime;
			ret.autoShotsPerSecond = currentTime / autoShotsFired;
			ret.steadyShotsPerSecond = currentTime / steadyShotsFired;
			return ret;
		}
		/// <summary>Based on this macro which is the current spammable BM 3:2 macro. 
		/// With enough haste, and a quick shots proc, this transforms into a 1:1 macro (add kill command and other effects as needed obviously
		/// #showtooltip Steady Shot
		///	/cast !Auto shot
		///	/cast Steady shot
		///  
		/// </summary>
		/// <param name="weaponSpeed"></param>
		/// <param name="autoShotDamage"></param>
		/// <param name="steadyShotDamage"></param>
		/// <param name="steadyShotCastTime"></param>
		/// <param name="latency"></param>
		/// <returns></returns>
		private SimulationResults CalculateThreeToTwoDPS(double weaponSpeed, double autoShotDamage, double steadyShotDamage, double steadyShotCastTime, float latency)
		{
			//StreamWriter sw = new StreamWriter("ShotTable"+weaponSpeed.ToString("N2")+".txt");
			double totalDamage = 0;
			double currentTime = 0;
			double lastAutoShotTime = 0;
			double autoShotsFired = 0;
			double steadyShotsFired = 0;
			double timeLeftTillNextAutoShot = 0;
			double timeLeftTillNextSteadyShot = 0;
			double gcdTimer = 0;
			bool lastWasAuto = true;
			//start off with an autoshot.
			totalDamage += autoShotDamage;
			autoShotsFired++;
			currentTime += AUTO_SHOT_CAST_TIME;
			//sw.WriteLine("Auto\t" + autoShotDamage.ToString("N2") + "\t0.00\t" + currentTime.ToString("N2"));
			for (int i = 1; i < MAX_SHOT_TABLE_LOOPS; i++)
			{
				timeLeftTillNextAutoShot = weaponSpeed - (currentTime - lastAutoShotTime);
				timeLeftTillNextSteadyShot = gcdTimer - currentTime;
				if (timeLeftTillNextAutoShot < 0 || timeLeftTillNextAutoShot < timeLeftTillNextSteadyShot)
				{
					totalDamage += autoShotDamage;
					if (timeLeftTillNextAutoShot > 0)
					{
						currentTime += timeLeftTillNextAutoShot;
					}
					//sw.Write("Auto\t" + autoShotDamage.ToString("N2") + "\t" + currentTime.ToString("N2") + "\t");
					lastAutoShotTime = currentTime;
					currentTime += AUTO_SHOT_CAST_TIME;
					//sw.WriteLine(currentTime.ToString("N2"));
					autoShotsFired++;
					lastWasAuto = true;
				}
				else
				{
					totalDamage += steadyShotDamage;
					if (currentTime < gcdTimer)
					{
						currentTime = gcdTimer;
					}
					//sw.Write("Steady\t" + steadyShotDamage.ToString("N2") + "\t" + currentTime.ToString("N2")+"\t");
					///GCD logic from spreadsheet given autoshot's wierdness given a handweaved or spammed macro.  
					if (lastWasAuto)
					{
						if (gcdTimer < lastAutoShotTime)
						{
							gcdTimer = lastAutoShotTime + 1.5;
						}
						else
						{
							gcdTimer += (1.5 + latency);
						}
					}
					else
					{
						gcdTimer = currentTime + 1.5;
					}

					currentTime += steadyShotCastTime;
					//sw.WriteLine(currentTime.ToString("N2"));

					steadyShotsFired++;
					lastWasAuto = false;
				}
			}

			timeLeftTillNextAutoShot = weaponSpeed - (currentTime - lastAutoShotTime);
			if (timeLeftTillNextAutoShot > 0)
			{
				currentTime += timeLeftTillNextAutoShot;
			}
			//sw.WriteLine("Auto\t" + autoShotDamage.ToString("N2") + "\t" + currentTime.ToString("N2"));
			//sw.Flush();
			//sw.Close();
			SimulationResults ret = new SimulationResults();
			ret.dps = totalDamage / currentTime; ;
			ret.totalShotsPerSecond = MAX_SHOT_TABLE_LOOPS / currentTime;
			ret.autoShotsPerSecond = currentTime / autoShotsFired;
			ret.steadyShotsPerSecond = currentTime / steadyShotsFired;
			return ret;
		}
		#endregion

		private Stats GetBaseTalentStats(HunterTalents talentTree)
		{
			Stats talents = new Stats();


			//if (talentTree.Trees.ContainsKey(MARKSMAN))
			{
				//Lethal Shots
				talents.Crit += (talentTree.LethalShots / 100f);

				//Combat Experience
				int combatExperience = talentTree.CombatExperience;
				talents.BonusAgilityMultiplier = .01f * combatExperience;
				talents.BonusIntellectMultiplier = .03f * combatExperience;

				//Master Marksman
				talents.BonusRangedAttackPowerMultiplier = (.02f * talentTree.MasterMarksman);
			}

			//if (talentTree.Trees.ContainsKey(SURVIVAL))
			{
				//Killer Instincts
				talents.Crit += (talentTree.KillerInstinct / 100f);
				//Master Tactition
				talents.Crit += (talentTree.MasterTactician / 100f);
				//surefooted
				talents.Hit += (talentTree.Surefooted / 100f);
				//Lighting Reflexes
				talents.BonusAgilityMultiplier = ((1 + talents.BonusAgilityMultiplier) * (1 + .03f * talentTree.LightningReflexes)) - 1;

				//Survival Instinct
				talents.BonusRangedAttackPowerMultiplier = ((1 + talents.BonusRangedAttackPowerMultiplier) * (1 + .02f * talentTree.SurvivalInstincts)) - 1;

			}

			return talents;
		}

		private Stats GetRaceStats(Character.CharacterRace characterRace)
		{
			Stats statsRace;
			switch (characterRace)
			{
				case Character.CharacterRace.BloodElf:
					statsRace = new Stats()
					{
						Health = 3488F,
						Mana = 3253f,
						Strength = 61f,
						Agility = 153f,
						Stamina = 106f,
						Intellect = 81f,
						Spirit = 82F,
					};
					break;
				case Character.CharacterRace.Draenei:
					statsRace = new Stats()
					{
						Health = 3488f,
						Mana = 3253f,
						Strength = 65f,
						Agility = 148f,
						Stamina = 106f,
						Intellect = 78f,
						Spirit = 82f
					};
					break;
				case Character.CharacterRace.Dwarf:
					statsRace = new Stats()
					{
						Health = 3488f,
						Mana = 3253f,
						Strength = 66f,
						Agility = 147f,
						Stamina = 111f,
						Intellect = 76f,
						Spirit = 82f
					};
					break;
				case Character.CharacterRace.NightElf:
					statsRace = new Stats()
					{
						Health = 3488f,
						Mana = 3253f,
						Strength = 61f,
						Agility = 156f,
						Stamina = 107f,
						Intellect = 77f,
						Spirit = 83f
					};
					break;
				case Character.CharacterRace.Orc:
					statsRace = new Stats()
					{
						Health = 3488f,
						Mana = 3253f,
						Strength = 67f,
						Agility = 148f,
						Stamina = 110f,
						Intellect = 74f,
						Spirit = 86f,
						BonusPetDamageMultiplier = .05f
					};
					break;
				case Character.CharacterRace.Tauren:
					statsRace = new Stats()
					{
						Health = 3488f,
						Mana = 3253f,
						Strength = 69f,
						Agility = 148f,
						Stamina = 110f,
						Intellect = 72f,
						Spirit = 85f
					};
					break;
				case Character.CharacterRace.Troll:
					statsRace = new Stats()
					{
						Health = 3488f,
						Mana = 3253f,
						Strength = 65f,
						Agility = 153f,
						Stamina = 109f,
						Intellect = 73f,
						Spirit = 84f
					};
					break;


				default:
					statsRace = new Stats();
					break;
			}
			return statsRace;
		}

		private Stats GetPetStats(CalculationOptionsHunter options, Stats hunterStats, Character character)
		{
			Stats petStats = new Stats()
			{
				Agility = 128,
				Strength = 162,
				Stamina = 307,
				Intellect = 33,
				Spirit = 99
			};

			//TODO: strength pet buffs here (e.g. kings, pet food)

			petStats.AttackPower = (petStats.Strength - 10f) * 2f;
			petStats.AttackPower += (hunterStats.RangedAttackPower * .22f);

			//TODO: AP Buffs go here, (e.g. Might, TSA)

			//TODO: TargetDebuffs to here (e.g. Imp. Hunters Mark, sunders, etc.)
			
			//Pet Hit
			double petHitChance = .95;
			//if (character.AllTalents.Trees.ContainsKey(BEAST_MASTER))
			{//Animal Handler
				petHitChance += (.02 * character.HunterTalents.AnimalHandler);
			}
			
			if (character.Race == Character.CharacterRace.Draenei)
			{
				petHitChance += .01;
			}
			if (options.TargetLevel == 73)
			{
				petHitChance -= (5 * .004 + .02);
			}
			else
			{
				petHitChance -= ((options.TargetLevel * 5 - 350) * .001);
			}
			petStats.Hit = (float)petHitChance;

			//Pet Crit
			petStats.Crit += petStats.Agility / 2560f;
			//if (character.AllTalents.Trees.ContainsKey(BEAST_MASTER))
			{
				petStats.Crit += (.02f * character.HunterTalents.Ferocity);
			}
			petStats.Crit -= ((options.TargetLevel * 5f - 350f) * .0004f);
			petStats.Crit += hunterStats.BonusPetCritChance;
			return petStats;
		}

		//TODO: Make attacks ranks and damage configurable
		private double CalculatePetSpecialAttackDPS(PetSpecialAttackData petSpecialAttackData, double damageAdjustment, double armorMitigation)
		{
			double petAttackDamage = 0;
            double petAttackSpellDamage = 0;

			switch (petSpecialAttackData.petAttack)
			{
				case PetAttacks.Bite:
					petAttackDamage = 120;
                    petAttackSpellDamage = 0;
					break;
				case PetAttacks.Claw:
					petAttackDamage = 65;
                    petAttackSpellDamage = 0;
					break;
                case PetAttacks.FireBreath:
                    petAttackDamage = 0;
                    petAttackSpellDamage = 111;
                    break;
                case PetAttacks.LightningBreath:
                    petAttackDamage = 0;
                    petAttackSpellDamage = 106;
                    break;
                case PetAttacks.Thunderstomp:
                    petAttackDamage = 0;
                    petAttackSpellDamage = 165;
                    break;
                case PetAttacks.None:
                    petAttackDamage = 0;
                    petAttackSpellDamage = 0;
                    break;
                case PetAttacks.Growl:
                    petAttackDamage = 0;
                    petAttackSpellDamage = 0;
                    break;
                case PetAttacks.Cower:
                    petAttackDamage = 0;
                    petAttackSpellDamage = 0;
                    break;
                // Current Damage set to avg added to pet and hunter's next attacks.
                case PetAttacks.FuriousHowl:
                    petAttackDamage = 102;
                    petAttackSpellDamage = 0;
                    break;
                // Gore has 50% chance to do double damage.  Already added in average below.
                case PetAttacks.Gore:
                    petAttackDamage = 73.5;
                    petAttackSpellDamage = 0;
                    break;
                case PetAttacks.PoisonSpit:
                    petAttackDamage = 0;
                    petAttackSpellDamage = 96;
                    break;
                // Scorpid poison is a DoT that stacks to 5... more calculations needed.
                // Current SpeelDamage set to one application for full duration
                case PetAttacks.ScorpidPoison:
                    petAttackDamage = 0;
                    petAttackSpellDamage = 55;
                    break;
                case PetAttacks.Screech:
                    petAttackDamage = 47;
                    petAttackSpellDamage = 0;
                    break;
            }
			return ((petAttackDamage * damageAdjustment * (1 - armorMitigation)) + (petAttackSpellDamage * damageAdjustment)) / petSpecialAttackData.AttackRate;
		}

		private SimulationResults CalculateShotsPerSecond(CalculationOptionsHunter options, double weaponSpeed, double steadyShotCastTime)
		{
			switch (options.ShotRotation)
			{
				case ShotRotation.OneToOne:
					return CalculateOneToOneDPS(weaponSpeed, 0, 0, steadyShotCastTime, options.Latency);
				case ShotRotation.AutoShotOnly:
					return CalculateAutoShotOnlyDPS(weaponSpeed, 0);
				case ShotRotation.ThreeToTwo:
					return CalculateThreeToTwoDPS(weaponSpeed, 0, 0, steadyShotCastTime, options.Latency);
				default:
					return new SimulationResults();
			}
		}

		private SimulationResults CalculateShotRotationDPS(CalculationOptionsHunter options, double weaponSpeed,
												double autoShotDamage, double steadyShotDamage, double steadyShotCastTime)
		{
			switch (options.ShotRotation)
			{
				case ShotRotation.OneToOne:
					return CalculateOneToOneDPS(weaponSpeed, autoShotDamage, steadyShotDamage, steadyShotCastTime, options.Latency);
				case ShotRotation.AutoShotOnly:
					return CalculateAutoShotOnlyDPS(weaponSpeed, autoShotDamage);
				case ShotRotation.ThreeToTwo:
					return CalculateThreeToTwoDPS(weaponSpeed, autoShotDamage, steadyShotDamage, steadyShotCastTime, options.Latency);
				default:
					return new SimulationResults();
			}
		}

        #endregion

		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsHunter));
			StringReader reader = new StringReader(xml);
			CalculationOptionsHunter calcOpts = serializer.Deserialize(reader) as CalculationOptionsHunter;
			return calcOpts;
		}

		private class SimulationResults
		{
			public double dps;
			public double totalShotsPerSecond;
			public double autoShotsPerSecond;
			public double steadyShotsPerSecond;

			public SimulationResults()
			{
				dps = 0;
				totalShotsPerSecond = 0;
				autoShotsPerSecond = 0;
				steadyShotsPerSecond = 0;
			}
		}

		private class PetSpecialAttackData
		{
			public PetAttacks petAttack;
			public double FocusUsed;
			public double CoolDown;
			public double FocusPerSecond;
			public double AttackRate;
			public double MaximumFrequency;
			public double AttackRateFromFocus;
		}
	}
}
