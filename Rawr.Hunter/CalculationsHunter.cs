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
		


        /**
         * Implemented Talents:
         * 
         ***** Beast Mastery:
         * Improved Aspect of the Hawk
         * Focus Fire+
         * Aspect Mastery+
         * Serpent's Swiftness
         * Unleased Fury
         * Ferocity
         * Bestial Disciplin
         * Animal Handler
         * Kindred Spirits
         * Ferocious Inspiration
         * 
         ***** Marksmanship:
         * Focused Aim+
         * Lethal Shots
         * Mortal Shots
         * Careful Aim
         * Improved Arcane Shot
         * Improved Stings+
         * Combat Experience
         * Master Marksman+
         * Wild Quiver
         * Improved Steady Shot+
         * Chimera Shot
         * Piercing Shots
         * 
         ***** Survival:
         * Survival Instincts
         * Survivalist
         * Hunter vs. Wild+
         * Killer Instinct
         * Lightning Reflexes
         * Explosive Shot
         */

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
                "Basic Calculated Stats:Steady Speed",
				"Pet Stats:Pet Attack Power",
				"Pet Stats:Pet Hit Percentage",
				"Pet Stats:Pet Crit Percentage",
				"Pet Stats:Pet Base DPS",
				"Pet Stats:Pet Special DPS",
				"Pet Stats:Pet KC DPS",
                "Intermediate Stats:Autoshot DPS",
                "Intermediate Stats:Steady Spam",
                "Intermediate Stats:AS 3xSteady",
                "Intermediate Stats:AS 2xSteady",
                "Intermediate Stats:SerpASSteady",
                "Intermediate Stats:ExpSteadySerp*Only with Explosive Shot Talent",
                "Intermediate Stats:ChimASSteady*Only with Chimera Shot Talent and Serpent Sting up",
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
				BonusDamageMultiplier = stats.BonusDamageMultiplier,
				BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
				LotPCritRating = stats.LotPCritRating,
				PhysicalCrit = stats.PhysicalCrit,
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
			stats.BonusDamageMultiplier +
			stats.BonusStaminaMultiplier +
			stats.BonusPetCritChance +
			stats.LotPCritRating +
			stats.PhysicalCrit +
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

            HunterRatings ratings = new HunterRatings();

            double hawkRAPBonus = 155.0f * (1.0 + 0.5 * character.HunterTalents.AspectMastery); // TODO: Level80

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
            double totalStaticHaste = ratings.QUIVER_SPEED_INCREASE * (1 + (calculatedStats.BasicStats.HasteRating / ratings.HASTE_RATING_PER_PERCENT / 100));

            {
                totalStaticHaste = totalStaticHaste * (1 + .04 * character.HunterTalents.SerpentsSwiftness);
            }

            double normalAutoshotsPerSecond = 0.0;
            if (character.Ranged != null)
            {
                calculatedStats.BaseAttackSpeed = (float)(character.Ranged.Speed / totalStaticHaste);
                normalAutoshotsPerSecond = 1.0 / calculatedStats.BaseAttackSpeed;
            }

            calculatedStats.SteadySpeed = (float)(2.0 / totalStaticHaste);

            #endregion

            #region OnProc Haste effects

            // Model DST

            double quickShotsUpTime = 0;
            double quickShotHaste = 1.0;
            if (character.HunterTalents.ImprovedAspectOfTheHawk > 0)
            {
                quickShotHaste = .03 * character.HunterTalents.ImprovedAspectOfTheHawk;
                double quickAutoShotsPerSecond = (1.0f + quickShotHaste) / calculatedStats.BaseAttackSpeed;
                //Quick Shot Uptime From Cheeky's DPS Spreadsheet with special notation - "By Norwest"
                double shotsInProc = (Math.Floor((12f - calculatedStats.BaseAttackSpeed) * normalAutoshotsPerSecond) + 1) * calculatedStats.BasicStats.Hit;
                double shotsInReProc = Math.Floor(12f * quickAutoShotsPerSecond) * calculatedStats.BasicStats.Hit;
                double reprocChanceInitial = 1 - Math.Pow(.9, shotsInProc);
                double reprocChanceSub = 1 - Math.Pow(.9, shotsInReProc);
                double AvgShotBeforeFirstReProc = ((1 - Math.Pow(0.9, (shotsInProc + 1))) / Math.Pow(.1, 2) - (shotsInProc + 1) * Math.Pow(0.9, shotsInProc) / .1) / reprocChanceInitial * 0.1;
                double AvgShotBeforeNthReProc = ((1 - Math.Pow(0.9, (shotsInReProc + 1))) / Math.Pow(.1, 2) - (shotsInReProc + 1) * Math.Pow(0.9, shotsInReProc) / .1) / reprocChanceSub * 0.1;
                double avgQuickShotChain = shotsInProc * (1 - reprocChanceInitial) + reprocChanceInitial * (1 - reprocChanceSub) * (AvgShotBeforeNthReProc * reprocChanceSub / Math.Pow((1 - reprocChanceSub), 2) + (AvgShotBeforeFirstReProc + shotsInReProc) / (1 - reprocChanceSub));
                quickShotsUpTime = avgQuickShotChain / (avgQuickShotChain + 10);
            }
            #endregion

            #region RAP Against Target
            double effectiveRAPAgainstMob = calculatedStats.BasicStats.RangedAttackPower + 1.0/3.0 * character.HunterTalents.CarefulAim * calculatedStats.BasicStats.Intellect;
            effectiveRAPAgainstMob += character.HunterTalents.HunterVsWild * 0.10 * calculatedStats.BasicStats.Stamina;
            #endregion

            #region Critical Hit Damage

            double autoshotCritDmgModifier = 1.0;
            double abilitiesCritDmgModifier = 1.0 + character.HunterTalents.MortalShots * 0.06;

            #endregion

            #region Weapon Damage
            double weaponDamageAverage = 0;
            double ammoDamage = 0;

            if (character.Ranged != null && character.Projectile != null)
            {
                weaponDamageAverage = (float)(character.Ranged.MinDamage + character.Ranged.MaxDamage) / 2f;
                ammoDamage = character.Ranged.Speed * ((float)(character.Projectile.MaxDamage + character.Projectile.MinDamage) / 2f);
            }
            #endregion

            #region Pet

            PetCalculations pet = new PetCalculations(character, calculatedStats, options, GetBuffsStats(character.ActiveBuffs));

            #endregion

            #region Talent Modifiers
            double talentModifiers = 1.0f;

            talentModifiers *= 1.0f + 0.01f * character.HunterTalents.RangedWeaponSpecialization;
            talentModifiers *= 1.0f + 0.01f * character.HunterTalents.FocusedFire;

            double talentDmgModifiers = 1.0 + pet.ferociousInspirationUptime * character.HunterTalents.FerociousInspiration * 0.01;

            #endregion

            #region Autoshot
            calculatedStats.AutoshotDPS = 0;
            if (character.Ranged != null)
            {
                double critHitModifier = (calculatedStats.BasicStats.PhysicalCrit * autoshotCritDmgModifier + 1.0) * calculatedStats.BasicStats.Hit;

                double autoshotDmg = (weaponDamageAverage + ammoDamage
                    + calculatedStats.BasicStats.ScopeDamage
                    + ((effectiveRAPAgainstMob + hawkRAPBonus) / 14 * character.Ranged.Speed)) * critHitModifier;

                double wildQuiverChance = 0.0;
                if (character.HunterTalents.WildQuiver == 1)
                {
                    wildQuiverChance = 0.04;
                }
                else if (character.HunterTalents.WildQuiver == 2)
                {
                    wildQuiverChance = 0.07;
                }
                else if (character.HunterTalents.WildQuiver == 3)
                {
                    wildQuiverChance = 0.10;
                }
                autoshotDmg = autoshotDmg + (autoshotDmg / 2.0) * wildQuiverChance;  // TODO: Check if this works with crits

                autoshotDmg *= talentModifiers;

                double targetArmor = options.TargetArmor - calculatedStats.BasicStats.ArmorPenetration;
                double armorReduction = (targetArmor / (467.5 * options.TargetLevel + targetArmor - 22167.5));

                autoshotDmg *= 1.0 - armorReduction;

                calculatedStats.AutoshotDPS = autoshotDmg / (calculatedStats.BaseAttackSpeed * (1.0 - quickShotsUpTime) + (calculatedStats.BaseAttackSpeed / (1.0f + quickShotHaste)) * quickShotsUpTime);
                calculatedStats.AutoshotDPS *= talentDmgModifiers;
            }

            #endregion

            #region Rotations

            ShotRotationCalculator rotation = new ShotRotationCalculator(character, calculatedStats, options, totalStaticHaste, effectiveRAPAgainstMob, abilitiesCritDmgModifier, autoshotCritDmgModifier, weaponDamageAverage, ammoDamage, talentModifiers);

            double bestDPS = 0.0;

            calculatedStats.SteadySpamDPS = rotation.SteadyRotation().DPS * talentDmgModifiers;
            bestDPS = Math.Max(calculatedStats.SteadySpamDPS, bestDPS);

            calculatedStats.Arcane3xSteadyDPS = rotation.ASSteadyRotation(3).DPS * talentDmgModifiers;
            bestDPS = Math.Max(calculatedStats.Arcane3xSteadyDPS, bestDPS);

            calculatedStats.Arcane2xSteadyDPS = rotation.ASSteadyRotation(2).DPS * talentDmgModifiers;
            bestDPS = Math.Max(calculatedStats.Arcane2xSteadyDPS, bestDPS);

            calculatedStats.SerpASSteadyDPS = rotation.ASSteadySerpentRotation().DPS * talentDmgModifiers;
            bestDPS = Math.Max(calculatedStats.SerpASSteadyDPS, bestDPS);

            calculatedStats.ExpSteadySerpDPS = rotation.ExpSteadySerpRotation().DPS * talentDmgModifiers;
            if (character.HunterTalents.ExplosiveShot > 0)
            {
                bestDPS = Math.Max(calculatedStats.ExpSteadySerpDPS, bestDPS);
            }

            calculatedStats.ChimASSteadyDPS = rotation.ChimASSteadyRotation().DPS * talentDmgModifiers;
            if (character.HunterTalents.ChimeraShot > 0)
            {
                bestDPS = Math.Max(calculatedStats.ChimASSteadyDPS, bestDPS);
            }
            #endregion

 
            calculatedStats.HunterDpsPoints = (float)(calculatedStats.AutoshotDPS + bestDPS);
            calculatedStats.PetDpsPoints = pet.getDPS();
            calculatedStats.OverallPoints = calculatedStats.HunterDpsPoints + calculatedStats.PetDpsPoints;

            return calculatedStats;

            // ---------------------------------------------

            
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


            HunterRatings ratings = new HunterRatings();

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
			

    		//Begin non Base Stats.

			statsTotal.Mana = (float)Math.Round(statsRace.Mana + 15f * (statsTotal.Intellect-10) + statsGearEnchantsBuffs.Mana);

            // TODO: Implement new racials
            statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + ((statsTotal.Stamina - 10.0f) * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
			//add up health talents, based on health, not base stat, so cannot roll it into Talent Stats

            // endurance training
			{
				statsTotal.Health += (float)Math.Round((statsTotal.Health * character.HunterTalents.EnduranceTraining * .01f));
			}

            statsTotal.Hit = (float)(ratings.BASE_HIT_PERCENT + (statsTotal.HitRating / (ratings.HIT_RATING_PER_PERCENT * 100.0f)) 
								- (statsTotal.Miss / 100.0f) 
								+ (character.Race == Character.CharacterRace.Draenei ? .01 : 0)
								+ statsTalents.Hit);

            // TODO: Change for level 80
			//=IF((B54-B53) - 10 > 0,  ((B54-B53) -10) * -0.004 - 0.02, (B54-B53) * -0.001)
			if (targetDefence > 360)
			{
				statsTotal.Hit = statsTotal.Hit - (targetDefence - 360) * .004f - .02f;
			} else {
				statsTotal.Hit -= ((targetDefence - 350) * .001f);
			}
            if (statsTotal.Hit > 1.0f)
			{
                statsTotal.Hit = 1.0f;
			}

			if (character.Ranged != null &&
				((character.Race == Character.CharacterRace.Dwarf && character.Ranged.Type == Item.ItemType.Gun) ||
				(character.Race == Character.CharacterRace.Troll && character.Ranged.Type == Item.ItemType.Bow)))
			{
                statsTotal.CritRating += (float)Math.Floor(ratings.CRIT_RATING_PER_PERCENT);
			}

            statsTotal.PhysicalCrit = (float)(ratings.BASE_CRIT_PERCENT + (statsTotal.Agility / ratings.AGILITY_PER_CRIT / 100.0f)
                                + (statsTotal.CritRating / ratings.CRIT_RATING_PER_PERCENT / 100.0f)
                                + ((350 - targetDefence) * 0.04 / 100.0f)
								+ statsTalents.PhysicalCrit);

		
			
			//TODO:Target Resilience
			//TODO:Darkmoon Card: Wrath


			#region Base RAP
            float RAP = 140f + (statsTotal.Agility - 10.0f);
		

			//TODO: Better model on proc events (current model is a static AP gain on the item, not based on shot speed, crit chance, etc)
			RAP += statsGearEnchantsBuffs.AttackPower + statsGearEnchantsBuffs.RangedAttackPower;

			RAP += statsTalents.AttackPower + statsTalents.RangedAttackPower;

			//TODO: Add new racials

			RAP += (RAP * statsTalents.BonusRangedAttackPowerMultiplier);

			statsTotal.RangedAttackPower = RAP;
			#endregion 

			return statsTotal;
		}

		#endregion //overrides 

        #region Private Functions

		private Stats GetBaseTalentStats(HunterTalents talentTree)
		{
			Stats talents = new Stats();


            // Marksmanship Talents
			{
				//Lethal Shots
				talents.PhysicalCrit += (talentTree.LethalShots * 0.01f);

                //Master Marksman
                talents.PhysicalCrit += (talentTree.MasterMarksman * 0.01f);

				//Combat Experience
				int combatExperience = talentTree.CombatExperience;
				talents.BonusAgilityMultiplier = .02f * combatExperience;
				talents.BonusIntellectMultiplier = .02f * combatExperience;
			}

            // Survival Talents
            {
				//Killer Instincts
				talents.PhysicalCrit += (talentTree.KillerInstinct * 0.01f);

				//Master Tactitian
				// TODO: Implement properly

				//surefooted
				talents.Hit += (talentTree.FocusedAim * 0.01f);

				//Lighting Reflexes
				talents.BonusAgilityMultiplier = ((1.0f + talents.BonusAgilityMultiplier) * (1.0f + .03f * talentTree.LightningReflexes)) - 1.0f;

                // Survivalist
                talents.BonusStaminaMultiplier = ((1.0f + talents.BonusStaminaMultiplier) * (1.0f + .02f * talentTree.Survivalist)) - 1.0f;
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
			petStats.PhysicalCrit += petStats.Agility / 2560f;
			//if (character.AllTalents.Trees.ContainsKey(BEAST_MASTER))
			{
				petStats.PhysicalCrit += (.02f * character.HunterTalents.Ferocity);
			}
			petStats.PhysicalCrit -= ((options.TargetLevel * 5f - 350f) * .0004f);
			petStats.PhysicalCrit += hunterStats.BonusPetCritChance;
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
