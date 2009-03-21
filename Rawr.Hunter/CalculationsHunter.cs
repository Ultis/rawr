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
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
				////Relevant Gem IDs for Hunters
				//Red
				int[] delicate = { 39905, 39997, 40112, 42143 };

				//Purple
				int[] shifting = { 39935, 40023, 40130 };

				//Green
				int[] vivid = { 39975, 40088, 40166 };

				//Yellow
				int[] rigid = { 39915, 40014, 40125, 42156 };

				//Orange
				int[] glinting = { 39953, 40044, 40148 };

				//Meta
				int relentless = 41398;

				return new List<GemmingTemplate>()
				{
					new GemmingTemplate() { Model = "Hunter", Group = "Uncommon", //Max Agi
						RedId = delicate[0], YellowId = delicate[0], BlueId = delicate[0], PrismaticId = delicate[0], MetaId = relentless },
					new GemmingTemplate() { Model = "Hunter", Group = "Uncommon", //Agi/Hit
						RedId = delicate[0], YellowId = glinting[0], BlueId = shifting[0], PrismaticId = delicate[0], MetaId = relentless },
					new GemmingTemplate() { Model = "Hunter", Group = "Uncommon", //Hit
						RedId = glinting[0], YellowId = rigid[0], BlueId = vivid[0], PrismaticId = rigid[0], MetaId = relentless },
						
					new GemmingTemplate() { Model = "Hunter", Group = "Rare", Enabled = true, //Max Agi
						RedId = delicate[1], YellowId = delicate[1], BlueId = delicate[1], PrismaticId = delicate[1], MetaId = relentless },
					new GemmingTemplate() { Model = "Hunter", Group = "Rare", Enabled = true, //Agi/Hit
						RedId = delicate[1], YellowId = glinting[1], BlueId = shifting[1], PrismaticId = delicate[1], MetaId = relentless },
					new GemmingTemplate() { Model = "Hunter", Group = "Rare", Enabled = true, //Hit
						RedId = glinting[1], YellowId = rigid[1], BlueId = vivid[1], PrismaticId = rigid[1], MetaId = relentless },
						
					new GemmingTemplate() { Model = "Hunter", Group = "Epic", //Max Agi
						RedId = delicate[2], YellowId = delicate[2], BlueId = delicate[2], PrismaticId = delicate[2], MetaId = relentless },
					new GemmingTemplate() { Model = "Hunter", Group = "Epic", //Agi/Hit
						RedId = delicate[2], YellowId = glinting[2], BlueId = shifting[2], PrismaticId = delicate[2], MetaId = relentless },
					new GemmingTemplate() { Model = "Hunter", Group = "Epic", //Hit
						RedId = glinting[2], YellowId = rigid[2], BlueId = vivid[2], PrismaticId = rigid[2], MetaId = relentless },
						
					new GemmingTemplate() { Model = "Hunter", Group = "Jeweler", //Max Agi
						RedId = delicate[3], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[3], MetaId = relentless },
					new GemmingTemplate() { Model = "Hunter", Group = "Jeweler", //Agi/Hit
						RedId = delicate[2], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[2], MetaId = relentless },
					new GemmingTemplate() { Model = "Hunter", Group = "Jeweler", //Hit
						RedId = rigid[3], YellowId = rigid[2], BlueId = rigid[3], PrismaticId = rigid[2], MetaId = relentless },
				};
            }
        }

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
         * Frenzy
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
         * Go for the Throat
         * 
         ***** Survival:
         * Survival Instincts
         * Survivalist
         * Hunter vs. Wild+
         * Killer Instinct
         * Lightning Reflexes
         * Explosive Shot
         * Expose Weakness
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
                "Intermediate Stats:Custom Rotation",
				"Complex Calculated Stats:Hunter Total DPS",
				"Complex Calculated Stats:Pet DPS",
				"Complex Calculated Stats:Overall DPS"
			};

            customChartNames = new string[] { "Relative Stat Values" };

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
            switch (chartName)
            {
                case "Relative Stat Values":
                    CharacterCalculationsHunter baseCalc = GetCharacterCalculations(character) as CharacterCalculationsHunter;
                    CharacterCalculationsHunter calcCritValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 1 } }) as CharacterCalculationsHunter;
                    CharacterCalculationsHunter calcAPValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { AttackPower = 2 } }) as CharacterCalculationsHunter;
                    CharacterCalculationsHunter calcHitValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 1 } }) as CharacterCalculationsHunter;
                    CharacterCalculationsHunter calcHasteValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 1 } }) as CharacterCalculationsHunter;

                    CharacterCalculationsHunter calcAgiValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 1 } }) as CharacterCalculationsHunter;
                    CharacterCalculationsHunter calcArPValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { ArmorPenetrationRating = 1 } }) as CharacterCalculationsHunter;
                    CharacterCalculationsHunter calcIntValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = 1 } }) as CharacterCalculationsHunter;

                    return new ComparisonCalculationBase[] {  
						        new ComparisonCalculationHunter() { Name = "+1 Crit Rating", 
                                    HunterDpsPoints = (calcCritValue.HunterDpsPoints - baseCalc.HunterDpsPoints),
                                    PetDpsPoints = (calcCritValue.PetDpsPoints - baseCalc.PetDpsPoints), 
                                    OverallPoints = (calcCritValue.OverallPoints - baseCalc.OverallPoints),},      
						        new ComparisonCalculationHunter() { Name = "+2 Attack Power", 
                                    HunterDpsPoints = (calcAPValue.HunterDpsPoints - baseCalc.HunterDpsPoints),
                                    PetDpsPoints = (calcAPValue.PetDpsPoints - baseCalc.PetDpsPoints), 
                                    OverallPoints = (calcAPValue.OverallPoints - baseCalc.OverallPoints),},       
						        new ComparisonCalculationHunter() { Name = "+1 Hit Rating", 
                                    HunterDpsPoints = (calcHitValue.HunterDpsPoints - baseCalc.HunterDpsPoints),
                                    PetDpsPoints = (calcHitValue.PetDpsPoints - baseCalc.PetDpsPoints), 
                                    OverallPoints = (calcHitValue.OverallPoints - baseCalc.OverallPoints),},
						        new ComparisonCalculationHunter() { Name = "+1 Haste Rating", 
                                    HunterDpsPoints = (calcHasteValue.HunterDpsPoints - baseCalc.HunterDpsPoints),
                                    PetDpsPoints = (calcHasteValue.PetDpsPoints - baseCalc.PetDpsPoints), 
                                    OverallPoints = (calcHasteValue.OverallPoints - baseCalc.OverallPoints),},
						        new ComparisonCalculationHunter() { Name = "+1 Agility", 
                                    HunterDpsPoints = (calcAgiValue.HunterDpsPoints - baseCalc.HunterDpsPoints),
                                    PetDpsPoints = (calcAgiValue.PetDpsPoints - baseCalc.PetDpsPoints), 
                                    OverallPoints = (calcAgiValue.OverallPoints - baseCalc.OverallPoints),},
						        new ComparisonCalculationHunter() { Name = "+1 Intellect", 
                                    HunterDpsPoints = (calcIntValue.HunterDpsPoints - baseCalc.HunterDpsPoints),
                                    PetDpsPoints = (calcIntValue.PetDpsPoints - baseCalc.PetDpsPoints), 
                                    OverallPoints = (calcIntValue.OverallPoints - baseCalc.OverallPoints),},

                                new ComparisonCalculationHunter() { Name = "+1 ArP Rating", 
                                    HunterDpsPoints = (calcArPValue.HunterDpsPoints - baseCalc.HunterDpsPoints),
                                    PetDpsPoints = (calcArPValue.PetDpsPoints - baseCalc.PetDpsPoints), 
                                    OverallPoints = (calcArPValue.OverallPoints - baseCalc.OverallPoints),},

                    };

            }

            return new ComparisonCalculationBase[0];

        }

        public override Stats GetRelevantStats(Stats stats)
        {
			return new Stats()
			{
                ArmorPenetrationRating = stats.ArmorPenetrationRating,
				Agility = stats.Agility, 
				ArmorPenetration = stats.ArmorPenetration,
				AttackPower = stats.AttackPower,
				RangedAttackPower = stats.RangedAttackPower,
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
				RangedCritRating = stats.RangedCritRating,
				DrumsOfBattle = stats.DrumsOfBattle,
				DrumsOfWar = stats.DrumsOfWar,
				ExposeWeakness = stats.ExposeWeakness,
				HasteRating = stats.HasteRating,
				RangedHasteRating = stats.RangedHasteRating,
				PhysicalHit = stats.PhysicalHit,
				HitRating = stats.HitRating,
				RangedHitRating = stats.RangedHitRating,
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
            stats.ArmorPenetrationRating +
			stats.AttackPower +
			stats.RangedAttackPower +
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
			stats.PhysicalCrit +
			stats.CritRating +
			stats.RangedCritRating +
			stats.DrumsOfBattle +
			stats.DrumsOfWar +
			stats.ExposeWeakness +
			stats.HasteRating +
			stats.RangedHasteRating +
            stats.PhysicalHaste +
			stats.Health +
			stats.PhysicalHit +
			stats.HitRating +
			stats.RangedHitRating +
			stats.Intellect +
			stats.Mp5 + 
			stats.ScopeDamage +
			stats.ShatteredSunAcumenProc +
			stats.ShatteredSunMightProc +
			stats.AshtongueTrinketProc +
			stats.BonusSteadyShotCrit +
			stats.BonusSteadyShotDamageMultiplier +
            stats.ManaRestoreFromMaxManaPerSecond) > 0;
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
			if (character.Ranged == null || (character.Ranged.Item.Type != Item.ItemType.Bow && character.Ranged.Item.Type != Item.ItemType.Gun
											&& character.Ranged.Item.Type != Item.ItemType.Crossbow))
			{
				//skip all the calculations if there is no ranged weapon
				return calculatedStats;
			}

            HunterRatings ratings = new HunterRatings();

            double hawkRAPBonus = ratings.HAWK_BONUS_AP * (1.0 + 0.3 * character.HunterTalents.AspectMastery);

            #region Base Attack Speed
            //Hasted Speed = Weapon Speed / ( (1+(Haste1 %)) * (1+(Haste2 %)) * (1+(((Haste Rating 1 + Haste Rating 2 + ... )/100)/15.7)) )
            double totalStaticHaste = (1.0 + (calculatedStats.BasicStats.HasteRating / ratings.HASTE_RATING_PER_PERCENT / 100.0));

            {
                totalStaticHaste *= (1.0 + .04 * character.HunterTalents.SerpentsSwiftness) * ratings.QUIVER_SPEED_INCREASE;
                totalStaticHaste *= (1.0 + calculatedStats.BasicStats.PhysicalHaste);
            }

            double normalAutoshotsPerSecond = 0.0;
            if (character.Ranged != null)
            {
                calculatedStats.BaseAttackSpeed = (float)(character.Ranged.Item.Speed / (totalStaticHaste));
                normalAutoshotsPerSecond = 1.0 / calculatedStats.BaseAttackSpeed;
            }

            calculatedStats.SteadySpeed = (float)(2.0 / totalStaticHaste);

            #endregion

            #region OnProc Haste effects

            // Model Quickshots

            double quickShotsUpTime = 0;
            double quickShotHaste = 1.0;
            if (character.HunterTalents.ImprovedAspectOfTheHawk > 0)
            {
                quickShotHaste = .03 * character.HunterTalents.ImprovedAspectOfTheHawk;
                double quickAutoShotsPerSecond = (1.0f + quickShotHaste) / calculatedStats.BaseAttackSpeed;
                //Quick Shot Uptime From Cheeky's DPS Spreadsheet with special notation - "By Norwest"
                double shotsInProc = (Math.Floor((12f - calculatedStats.BaseAttackSpeed) * normalAutoshotsPerSecond) + 1) * calculatedStats.BasicStats.PhysicalHit;
                double shotsInReProc = Math.Floor(12f * quickAutoShotsPerSecond) * calculatedStats.BasicStats.PhysicalHit;
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

            double shotsPerSecond = 1.0 / calculatedStats.BaseAttackSpeed + 1.0 / 1.5;
            double ewUptime = 1.0 - Math.Pow(1.0 - calculatedStats.BasicStats.PhysicalCrit * character.HunterTalents.ExposeWeakness / 3.0, 7.0 / shotsPerSecond);

            effectiveRAPAgainstMob += calculatedStats.BasicStats.Agility * 0.25 * ewUptime;
            effectiveRAPAgainstMob *= calculatedStats.BasicStats.BonusAttackPowerMultiplier;

            calculatedStats.RAP = (float)effectiveRAPAgainstMob;
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
                weaponDamageAverage = (float)(character.Ranged.Item.MinDamage + character.Ranged.Item.MaxDamage) / 2f;
                ammoDamage = character.Ranged.Item.Speed * ((float)(character.Projectile.Item.MaxDamage + character.Projectile.Item.MinDamage) / 2f);
                weaponDamageAverage += ammoDamage
                    + calculatedStats.BasicStats.ScopeDamage
                    + ((effectiveRAPAgainstMob + hawkRAPBonus) / 14 * character.Ranged.Item.Speed);

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
            talentDmgModifiers *= calculatedStats.BasicStats.BonusDamageMultiplier;

            #endregion

            #region Autoshot
            calculatedStats.AutoshotDPS = 0;
            if (character.Ranged != null)
            {
                double critHitModifier = (calculatedStats.BasicStats.PhysicalCrit * autoshotCritDmgModifier + 1.0) * calculatedStats.BasicStats.PhysicalHit;

                double autoshotDmg = weaponDamageAverage * critHitModifier;

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

				int targetArmor = options.TargetArmor;
				float armorReduction = 1f - ArmorCalculations.GetDamageReduction(character.Level, targetArmor,
				calculatedStats.BasicStats.ArmorPenetration, calculatedStats.BasicStats.ArmorPenetrationRating);//double targetArmor = (options.TargetArmor - calculatedStats.BasicStats.ArmorPenetration) * (1.0 - calculatedStats.BasicStats.ArmorPenetrationRating / (ratings.ARP_RATING_PER_PERCENT * 100.0));
                //double armorReduction = (targetArmor / (467.5 * options.TargetLevel + targetArmor - 22167.5));

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

            calculatedStats.CustomDPS = rotation.createCustomRotation().DPS * talentDmgModifiers;

            #endregion

            if (options.UseCustomShotRotation)
            {
                calculatedStats.HunterDpsPoints = (float)(calculatedStats.AutoshotDPS + calculatedStats.CustomDPS);
            }
            else
            {
                calculatedStats.HunterDpsPoints = (float)(calculatedStats.AutoshotDPS + bestDPS);
            }
 
            
            calculatedStats.PetDpsPoints = pet.getDPS();
            calculatedStats.OverallPoints = calculatedStats.HunterDpsPoints + calculatedStats.PetDpsPoints;

            return calculatedStats;

            // ---------------------------------------------

            
		}

		public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
			Stats statsRace = GetRaceStats(character.Race);
			Stats statsBaseGear = GetItemStats(character, additionalItem);
			//Stats statsEnchants = GetEnchantsStats(character);
			Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
			Stats statsTalents = GetBaseTalentStats(character.HunterTalents);
			Stats statsGearEnchantsBuffs = statsBaseGear + statsBuffs;
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
			statsTotal.BonusArcaneDamageMultiplier = ((1 + statsRace.BonusArcaneDamageMultiplier) * (1 + statsGearEnchantsBuffs.BonusArcaneDamageMultiplier)) - 1;
			statsTotal.BonusPetDamageMultiplier = ((1 + statsGearEnchantsBuffs.BonusPetDamageMultiplier) * (1 + statsRace.BonusPetDamageMultiplier)) - 1;
			statsTotal.BonusSteadyShotDamageMultiplier = ((1 + statsGearEnchantsBuffs.BonusSteadyShotDamageMultiplier) * (1 + statsRace.BonusSteadyShotDamageMultiplier) * (1 + statsTalents.BonusSteadyShotDamageMultiplier)) - 1;

			statsTotal.Agility = (statsRace.Agility + statsGearEnchantsBuffs.Agility) * (1 + statsTotal.BonusAgilityMultiplier);
			statsTotal.Intellect = (statsRace.Intellect + statsGearEnchantsBuffs.Intellect) * (1 + statsTotal.BonusIntellectMultiplier);
			statsTotal.Stamina = (statsRace.Stamina + statsGearEnchantsBuffs.Stamina) * (1 + statsTotal.BonusStaminaMultiplier); 
			
			statsTotal.Resilience = statsRace.Resilience + statsGearEnchantsBuffs.Resilience;
			statsTotal.Armor = (float)Math.Round((statsGearEnchantsBuffs.Armor + statsRace.Armor + (statsTotal.Agility * 2f)) * (1 + statsBuffs.BonusArmorMultiplier));
            statsTotal.Miss = 0.0f;
			statsTotal.ArmorPenetration = statsRace.ArmorPenetration + statsGearEnchantsBuffs.ArmorPenetration;
            statsTotal.ArmorPenetrationRating = statsRace.ArmorPenetrationRating + statsGearEnchantsBuffs.ArmorPenetrationRating;
			statsTotal.BloodlustProc = statsRace.BloodlustProc + statsGearEnchantsBuffs.BloodlustProc;
            statsTotal.BonusCritMultiplier = 0.0f; // ((1 + statsRace.BonusCritMultiplier) * (1 + statsGearEnchantsBuffs.BonusCritMultiplier)) - 1;
            statsTotal.PhysicalCrit = statsBuffs.PhysicalCrit;
            statsTotal.CritRating = (float)Math.Floor((decimal)statsRace.CritRating + (decimal)statsGearEnchantsBuffs.CritRating + (decimal)statsGearEnchantsBuffs.RangedCritRating + (decimal)statsRace.LotPCritRating + (decimal)statsGearEnchantsBuffs.LotPCritRating);
            statsTotal.HasteRating = statsRace.HasteRating + statsGearEnchantsBuffs.HasteRating + statsGearEnchantsBuffs.RangedHasteRating;
            // Haste trinket (Meteorite Whetstone)
            statsTotal.HasteRating += statsGearEnchantsBuffs.HasteRatingOnPhysicalAttack * 10 / 45;
            statsTotal.PhysicalHaste = statsGearEnchantsBuffs.PhysicalHaste;

			statsTotal.HitRating = (float)Math.Floor((decimal)statsRace.HitRating + (decimal)statsGearEnchantsBuffs.HitRating + (decimal)statsGearEnchantsBuffs.RangedHitRating);
			statsTotal.ExposeWeakness = statsRace.ExposeWeakness + statsGearEnchantsBuffs.ExposeWeakness;
			statsTotal.Bloodlust = statsRace.Bloodlust + statsGearEnchantsBuffs.Bloodlust;
			statsTotal.ShatteredSunMightProc = statsRace.ShatteredSunMightProc + statsGearEnchantsBuffs.ShatteredSunMightProc;
			statsTotal.Mp5 = statsRace.Mp5 + statsGearEnchantsBuffs.Mp5;
			statsTotal.BonusPetCritChance = statsGearEnchantsBuffs.BonusPetCritChance;
			statsTotal.ScopeDamage = statsGearEnchantsBuffs.ScopeDamage;
			statsTotal.AshtongueTrinketProc = statsGearEnchantsBuffs.AshtongueTrinketProc;
			statsTotal.BonusSteadyShotCrit = statsGearEnchantsBuffs.BonusSteadyShotCrit;

            statsTotal.BonusDamageMultiplier = 1.0f + statsGearEnchantsBuffs.BonusDamageMultiplier;
            statsTotal.BonusAttackPowerMultiplier = 1.0f + statsGearEnchantsBuffs.BonusAttackPowerMultiplier;

    		//Begin non Base Stats.

			statsTotal.Mana = (float)Math.Round(statsRace.Mana + 15f * (statsTotal.Intellect-10) + statsGearEnchantsBuffs.Mana);

            // TODO: Implement new racials
            statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + ((statsTotal.Stamina - 10.0f) * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
			//add up health talents, based on health, not base stat, so cannot roll it into Talent Stats

            // endurance training
			{
				statsTotal.Health += (float)Math.Round((statsTotal.Health * character.HunterTalents.EnduranceTraining * .01f));
			}

            float hitBonus = (float)(statsTotal.HitRating / (ratings.HIT_RATING_PER_PERCENT * 100.0f) + statsTalents.PhysicalHit + statsRace.PhysicalHit);

            float chanceMiss = Math.Max(0f, 0.08f - hitBonus);
            if ((options.TargetLevel - 80f) < 3)
            {
                chanceMiss = Math.Max(0f, 0.05f + 0.005f * (options.TargetLevel - 80f) - hitBonus);
            }

            statsTotal.PhysicalHit = 1.0f - chanceMiss;

 			if (character.Ranged != null &&
				((character.Race == Character.CharacterRace.Dwarf && character.Ranged.Item.Type == Item.ItemType.Gun) ||
				(character.Race == Character.CharacterRace.Troll && character.Ranged.Item.Type == Item.ItemType.Bow)))
			{
                statsTotal.CritRating += (float)Math.Floor(ratings.CRIT_RATING_PER_PERCENT);
			}

            statsTotal.PhysicalCrit = (float)(ratings.BASE_CRIT_PERCENT + (statsTotal.Agility / ratings.AGILITY_PER_CRIT / 100.0f)
                                + (statsTotal.CritRating / ratings.CRIT_RATING_PER_PERCENT / 100.0f)
                                + ((350 - targetDefence) * 0.04 / 100.0f)
								+ statsTalents.PhysicalCrit
                                + statsTotal.PhysicalCrit);

		
			
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
                //Focused Aim
                talents.PhysicalHit += (talentTree.FocusedAim * 0.01f);
                
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
						Spirit = 82f,
                        PhysicalHit = 0.01f,
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
                //TODO: Level80 value?
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
			petStats.PhysicalHit = (float)petHitChance;

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



        #endregion

		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsHunter));
			StringReader reader = new StringReader(xml);
			CalculationOptionsHunter calcOpts = serializer.Deserialize(reader) as CalculationOptionsHunter;
			return calcOpts;
		}

	}
}
