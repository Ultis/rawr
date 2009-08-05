using System;
using System.Collections.Generic;
#if SILVERLIGHT
#else
using System.Drawing;
#endif
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;

namespace Rawr.Hunter
{
	/// <summary>
    /// The rotation calculations in this module are based upon the work of Wertez and Indora
    /// over at The Hunting Lodge http://www.brigwyn.com/ 
    /// Endless thanks to them. 
    /// Please respect their work.
    /// They have allowed us to use their work on the following conditions:
    /// 1) Wertez and Indora did not get any money for doing this work, so neither can you
    /// 2) If their worked is passed on to a third party they must also give proper credit and
    ///    follow condition 1.
	/// </summary>
	[Rawr.Calculations.RawrModelInfo("Hunter", "Inv_Weapon_Bow_07", CharacterClass.Hunter)]

	public class CalculationsHunter : CalculationsBase

    {
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
				////Relevant Gem IDs for Hunters
				//Red
				int[] delicate = { 41434, 39997, 40112, 42143 };

				//Purple
				int[] shifting = { 41460, 40023, 40130 };

				//Green
                int[] vivid = { 41481, 40088, 40166 };

				//Yellow
                int[] rigid = { 41447, 40014, 40125, 42156 };

				//Orange
                int[] glinting = { 41491, 40044, 40148 };

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

#if SILVERLIGHT
        private ICalculationOptionsPanel calculationOptionsPanel = null;
#else
        private CalculationOptionsPanelBase calculationOptionsPanel = null;
#endif
        private string[] characterDisplayCalculationLabels = null;
        private string[] customChartNames = null;
        private List<ItemType> relevantItemTypes = null;
#if SILVERLIGHT
        private Dictionary<string, System.Windows.Media.Color> subPointNameColors = null;
#else
        private Dictionary<string, System.Drawing.Color> subPointNameColors = null;
#endif
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
				"Basic Stats:Haste",
				"Basic Stats:Mana Per Second",				
				"Basic Calculated Stats:Health",
				"Basic Calculated Stats:Mana",
				"Basic Calculated Stats:Hit Percentage",
				"Basic Calculated Stats:Crit Percentage",
				"Basic Calculated Stats:Ranged AP",
				"Basic Calculated Stats:Attack Speed",
                "Basic Calculated Stats:Steady Speed",
                "Basic Calculated Stats:Specials Per Second",
				"Pet Stats:Pet Attack Power",
				"Pet Stats:Pet Hit Percentage",
				"Pet Stats:Pet Crit Percentage",
				"Pet Stats:Pet Base DPS",
				"Pet Stats:Pet Special DPS*Based on all damaging or DPS boosting skills on auto-cast",
				"Shot Stats:Auto Shot",
				"Shot Stats:Steady Shot",
				"Shot Stats:Serpent Sting",
				"Shot Stats:Arcane Shot",
				"Shot Stats:Explosive Shot",
				"Shot Stats:Aimed Shot",
				"Shot Stats:Multi Shot",
				"Shot Stats:Black Arrow",
                "Shot Stats:Chimera Shot",
                "Shot Stats:Silencing Shot",
                "Shot Stats:Kill Shot",
                "Intermediate Stats:Autoshot DPS",
                "Intermediate Stats:Custom Rotation",
				"Complex Calculated Stats:Hunter Total DPS",
				"Complex Calculated Stats:Pet DPS",
				"Complex Calculated Stats:Overall DPS"
			};

            customChartNames = new string[] { "Relative Stat Values" };

			relevantItemTypes = new List<ItemType>(new ItemType[]
					{
						ItemType.None,
						ItemType.AmmoPouch,
                        ItemType.Arrow,
                        ItemType.Bow,
                        ItemType.Bullet,
                        ItemType.Crossbow,
                        ItemType.Dagger,
						ItemType.FistWeapon,
                        ItemType.Gun,
                        ItemType.Leather,
                        ItemType.Mail,
                        ItemType.OneHandAxe,                        
                        ItemType.OneHandSword,
                        ItemType.Polearm,
                        ItemType.Quiver,
                        ItemType.Staff,                    
                        ItemType.TwoHandAxe,
                        ItemType.TwoHandSword
					});

#if SILVERLIGHT
            subPointNameColors = new Dictionary<string, System.Windows.Media.Color>();
            subPointNameColors.Add("HunterDps", System.Windows.Media.Color.FromArgb(1, 0, 128, 255));
            subPointNameColors.Add("PetDps", System.Windows.Media.Color.FromArgb(1, 255, 100, 0));
#else
			subPointNameColors = new Dictionary<string, System.Drawing.Color>();
			subPointNameColors.Add("HunterDps", System.Drawing.Color.FromArgb(0, 128, 255));
			subPointNameColors.Add("PetDps", System.Drawing.Color.FromArgb(255, 100, 0));		
#endif
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
#if SILVERLIGHT
        public override ICalculationOptionsPanel CalculationOptionsPanel
        {
            get
            {
                return calculationOptionsPanel ?? (calculationOptionsPanel = new CalculationOptionsPanelHunter());
            }
        }
#else
		public override CalculationOptionsPanelBase CalculationOptionsPanel

        {
            get
            {
				return calculationOptionsPanel ?? (calculationOptionsPanel = new CalculationOptionsPanelHunter());
            }
        }
#endif
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
				BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
				BonusManaPotion = stats.BonusManaPotion,
				BonusPetDamageMultiplier = stats.BonusPetDamageMultiplier,
				BonusPetCritChance = stats.BonusPetCritChance,
				BonusDamageMultiplier = stats.BonusDamageMultiplier,
				BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
				LotPCritRating = stats.LotPCritRating,
				PhysicalCrit = stats.PhysicalCrit,
				CritRating = stats.CritRating,
				RangedCritRating = stats.RangedCritRating,
				DamageTakenMultiplier = stats.DamageTakenMultiplier,
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
				Spirit = stats.Spirit,
				ShatteredSunAcumenProc = stats.ShatteredSunAcumenProc,
				ShatteredSunMightProc = stats.ShatteredSunMightProc,
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
			stats.BonusSpiritMultiplier +
			stats.BonusPetCritChance +
			stats.PhysicalCrit +
			stats.CritRating +
			stats.RangedCritRating +
			stats.DrumsOfBattle +
			stats.DamageTakenMultiplier +
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
			stats.Spirit +
			stats.Mp5 + 
			stats.ScopeDamage +
			stats.ShatteredSunAcumenProc +
			stats.ShatteredSunMightProc +
			stats.BonusSteadyShotCrit +
			stats.BonusSteadyShotDamageMultiplier +
            stats.ManaRestoreFromMaxManaPerSecond) > 0;
        }
       
        public override List<ItemType> RelevantItemTypes
        {
            get { return relevantItemTypes; }
        }

#if SILVERLIGHT
        public override Dictionary<string, System.Windows.Media.Color> SubPointNameColors
#else
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
#endif
        {
            get { return subPointNameColors; }
        }
 
        public override CharacterClass TargetClass
        {
            get { return CharacterClass.Hunter; }
        }

		public override bool CanUseAmmo
		{
			get { return true; }
		}

		public override bool IsItemRelevant(Item item)
		{
			bool returnValue;
			if (item.Slot == ItemSlot.Ranged && item.Type == ItemType.Idol)
			{
				returnValue = false;
			}
			else if (item.Slot == ItemSlot.Projectile || 
				(item.Slot == ItemSlot.Ranged && (item.Type == ItemType.Gun || item.Type == ItemType.Bow || item.Type == ItemType.Crossbow)))
			{
				returnValue = true;
			}
			else
			{
				returnValue = base.IsItemRelevant(item);
			}
			return returnValue;
		}

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
		{
			CharacterCalculationsHunter calculatedStats = new CharacterCalculationsHunter();
			if (character == null)
			{
				return calculatedStats;
			}

			CalculationOptionsHunter options = character.CalculationOptions as CalculationOptionsHunter;
			calculatedStats.BasicStats = GetCharacterStats(character, additionalItem);
			calculatedStats.PetStats = GetPetStats(options, calculatedStats, character);
			if (character.Ranged == null || (character.Ranged.Item.Type != ItemType.Bow && character.Ranged.Item.Type != ItemType.Gun
											&& character.Ranged.Item.Type != ItemType.Crossbow))
			{
				//skip all the calculations if there is no ranged weapon
				return calculatedStats;
			}
			
			
		
			
			#region May 2009 Haste Calcs
			
			double hasteFromRacial = 1;
			if(character.Race == CharacterRace.Troll)
			{
				hasteFromRacial += 0.1 * CalcUptime ( 10, (3 * 60), options.duration);
			}
								
			//default quiver speed
			calculatedStats.hasteFromBase = 15.0;
			
			// haste from haste rating
			calculatedStats.hasteFromRating = calculatedStats.BasicStats.HasteRating / HunterRatings.HASTE_RATING_PER_PERCENT;
			
			// serpent swiftness, 
			calculatedStats.hasteFromTalentsStatic = 4.0 * character.HunterTalents.SerpentsSwiftness ;
							
			//rapid fire * (  duration/cooldown-depends on talent rapid killing)
            double rapidFireHaste = character.HunterTalents.GlyphOfRapidFire ? 48.0 : 40.0;
			double rapidFireCooldown = 300 -  (60 * character.HunterTalents.RapidKilling);

            calculatedStats.hasteFromProcs = rapidFireHaste * CalcUptime(15, rapidFireCooldown, options.duration);
			
			calculatedStats.hasteFromRangedBuffs = calculatedStats.BasicStats.RangedHaste  * 100;
			
			
			double autoShotPreProcs = 1.0;
			{
				autoShotPreProcs *=  (1.0 + calculatedStats.hasteFromBase / 100.0) ;
				autoShotPreProcs *=  (1.0 + calculatedStats.hasteFromRating / 100.0) ;
				autoShotPreProcs *=  (1.0 + calculatedStats.hasteFromTalentsStatic / 100.0) ;
				autoShotPreProcs *=  (1.0 + calculatedStats.hasteFromProcs / 100.0) ;
				autoShotPreProcs *=  (1.0 + calculatedStats.hasteFromRangedBuffs / 100);
				autoShotPreProcs *= hasteFromRacial;
				autoShotPreProcs = (character.Ranged.Item.Speed / autoShotPreProcs) ;
				
			}
			
			// improved aspect of the hawk, improved aspect of the hawk glyph, 	
			calculatedStats.hasteFromTalentsProc = 3.0 * character.HunterTalents.ImprovedAspectOfTheHawk;
			{
				if ((character.HunterTalents.ImprovedAspectOfTheHawk >= 1) && (character.HunterTalents.GlyphOfTheHawk == true))
				{
					calculatedStats.hasteFromTalentsProc += 6.0;
				}
				// uptime =  12 second duration /  time between procs aka ( autoshot speed / chancetoproc )
				calculatedStats.hasteFromTalentsProc *= 12.0 / ( autoShotPreProcs  / 0.1 ) ;
			}
				
			calculatedStats.hasteEffectsTotal = 1.0;
			
				calculatedStats.hasteEffectsTotal *=  (1.0 + calculatedStats.hasteFromBase / 100.0) ;
				calculatedStats.hasteEffectsTotal *=  (1.0 + calculatedStats.hasteFromRating / 100.0) ;
				calculatedStats.hasteEffectsTotal *=  (1.0 + calculatedStats.hasteFromTalentsProc / 100.0) ;
				calculatedStats.hasteEffectsTotal *=  (1.0 + calculatedStats.hasteFromTalentsStatic / 100.0) ;
				calculatedStats.hasteEffectsTotal *=  (1.0 + calculatedStats.hasteFromProcs / 100.0) ;	
				calculatedStats.hasteEffectsTotal *=  (1.0 + calculatedStats.hasteFromRangedBuffs / 100);
				calculatedStats.hasteEffectsTotal *= hasteFromRacial;
				calculatedStats.hasteEffectsTotal =  (calculatedStats.hasteEffectsTotal - 1.0) * 100;
				
				double hasteMultiplier = 1.0 +  (calculatedStats.hasteEffectsTotal /100);
			
			#endregion			
			#region May 2009 Shot Speeds
			double autoShotSpeed;
			if ((character.Ranged.Speed / hasteMultiplier <= 0.5 ))
            {
            	autoShotSpeed = (float) 0.5;
            }
            else
           	{
            	autoShotSpeed = (float)(character.Ranged.Speed / hasteMultiplier );
            }
			
            double shotsPerSec = 1/autoShotSpeed;
            
            shotsPerSec += 1/1.5;
            
            double GCD = 1.5;
			#endregion					
			#region May 2009 Hit Chance
			double missPercent = HunterRatings.BASE_MISS_PERCENT ;
			calculatedStats.hitBase = 1.0 - HunterRatings.BASE_MISS_PERCENT;
			
			
			double levelDifference = options.TargetLevel - HunterRatings.CHAR_LEVEL;

            missPercent += levelDifference;
            calculatedStats.hitLevelAdjustment = 0 - (levelDifference / 100);

			missPercent = missPercent / 100;			
			
			double bonusHit = (calculatedStats.BasicStats.HitRating / HunterRatings.HIT_RATING_PER_PERCENT) / 100;
					bonusHit += character.HunterTalents.FocusedAim / 100;
			
			calculatedStats.hitRating = (calculatedStats.BasicStats.HitRating / HunterRatings.HIT_RATING_PER_PERCENT) / 100;
					
			calculatedStats.hitFromTalents = (1.0 * character.HunterTalents.FocusedAim) / 100;
			
			
			//TODO: Find how to get Heroic Presence + any Hit Chance buffs
			calculatedStats.hitFromBuffs = statsBuffs.SpellHit;
				
		
			
			calculatedStats.hitOverall = calculatedStats.hitBase;
			calculatedStats.hitOverall += calculatedStats.hitLevelAdjustment;
			calculatedStats.hitOverall += calculatedStats.hitRating;
			calculatedStats.hitOverall += calculatedStats.hitFromTalents;
			calculatedStats.hitOverall += calculatedStats.hitFromBuffs;
			
			if (calculatedStats.hitOverall >= 1.0)
			{
				calculatedStats.hitOverall = 1.0;
			}
			
			double hitChance = calculatedStats.hitOverall;
					
			#endregion
			#region May 2009 Crit Chance
			
			double critHitPercent = HunterRatings.BASE_CRIT_PERCENT;
			calculatedStats.critBase = HunterRatings.BASE_CRIT_PERCENT;      
   
			critHitPercent += (calculatedStats.BasicStats.Agility / HunterRatings.AGILITY_PER_CRIT) / 100;
			calculatedStats.critFromAgi = (calculatedStats.BasicStats.Agility / HunterRatings.AGILITY_PER_CRIT) / 100;
			
			critHitPercent += (calculatedStats.BasicStats.CritRating / HunterRatings.CRIT_RATING_PER_PERCENT) / 100 ;
			calculatedStats.critFromRating = (calculatedStats.BasicStats.CritRating / HunterRatings.CRIT_RATING_PER_PERCENT) / 100 ;
			
			//TODO:  	+= Proc Crit
			
			critHitPercent += character.HunterTalents.LethalShots * 0.01;
			critHitPercent += character.HunterTalents.KillerInstinct * 0.01;
			critHitPercent += character.HunterTalents.MasterMarksman * 0.01;
			calculatedStats.critFromTalents = (character.HunterTalents.LethalShots * 0.01) + (character.HunterTalents.KillerInstinct * 0.01) + (character.HunterTalents.MasterMarksman * 0.01);
			
			// += talents.mastertactician ( 10% chance to proc on "successful attack" for 8 seconds = 2% per point)
			double masterTacticianUptime = CalcUptime(8, (0.1 * hitChance * shotsPerSec ), options.duration);
			critHitPercent += 0.02 * character.HunterTalents.MasterTactician * masterTacticianUptime;
			
			
			// Crit From target debuffs / player buffs TODO: Check this
			critHitPercent += statsBuffs.PhysicalCrit;
			calculatedStats.critFromBuffs = statsBuffs.PhysicalCrit;

			//
			double critdepression = 0;
			
			if ( levelDifference > 2)
			{
				critdepression = 0.03 + (levelDifference * 0.006);
			}
			else
			{
				critdepression += (levelDifference * 5 * 0.04) / 100;
			}
			
			critHitPercent -= critdepression;
			calculatedStats.critfromDepression	= 0 - critdepression;	
			
			calculatedStats.critRateOverall = critHitPercent;
			double normalHitPercent = 1.0 - critHitPercent;
			#endregion
			#region May 2009 Ranged Attackpower
			
			double apFromBase = 0 + HunterRatings.CHAR_LEVEL * 2;
			double apFromAgil = 0 +(calculatedStats.BasicStats.Agility ) - 10;
			double apFromCarefulAim = 0 +(character.HunterTalents.CarefulAim / 3) * (calculatedStats.BasicStats.Intellect);
			double apFromHunterVsWild = 0 + (character.HunterTalents.HunterVsWild * 0.1) * (calculatedStats.BasicStats.Stamina);
			double apFromGear = 0 + calculatedStats.BasicStats.AttackPower;
			//double apFromBuffs = 0 + character.ActiveBuffs.
			double apFromBloodFury = 0;
			if(character.Race == CharacterRace.Orc)
			{
				apFromBloodFury = (4 * HunterRatings.CHAR_LEVEL) + 2;
				apFromBloodFury *= CalcUptime(15, 120, options.duration);
				
			}
			double apFromAspectOfTheHawk = 300;
			double apFromAspectMastery = (character.HunterTalents.AspectMastery * .3 * apFromAspectOfTheHawk);
			                          
			double apFromFuriousHowl = 0;
			if ( options.PetFamily == PetFamily.Wolf)
			{
				
				apFromFuriousHowl = 320 * CalcUptime(20,40, options.duration);
			}
			double apFromCallOfTheWild = 1; 
			apFromCallOfTheWild	+= options.CallOfTheWild * (CalcUptime(20,300, options.duration) * 0.1);
			
			//TODO:
			//double apFromOnProcEffects = 
			
			
			double apFromTrueshotAura = 1 + (0.1 * character.HunterTalents.TrueshotAura);
			if (character.HunterTalents.TrueshotAura == 0)
			{
				apFromTrueshotAura = calculatedStats.BasicStats.BonusAttackPowerMultiplier + calculatedStats.BasicStats.BonusRangedAttackPowerMultiplier;
			}

			double apFromHuntersMark = HunterRatings.HUNTERS_MARK;
			apFromHuntersMark += HunterRatings.HUNTERS_MARK * 0.1 * character.HunterTalents.ImprovedHuntersMark ;
			if (character.HunterTalents.GlyphOfHuntersMark == true)
			{
				apFromHuntersMark += 0.2 * HunterRatings.HUNTERS_MARK;
			}
			double critPercent = HunterRatings.BASE_CRIT_PERCENT;
			critPercent += (calculatedStats.BasicStats.Agility / HunterRatings.AGILITY_PER_CRIT) / 100;
			critPercent += (calculatedStats.BasicStats.CritRating / HunterRatings.CRIT_RATING_PER_PERCENT) / 100 ;
			//double attackRate;
			
			// TODO: Figure out Expose Weakness
			double apFromExposeWeakness = 0;
			if (character.HunterTalents.ExposeWeakness > 0)
			{
				apFromExposeWeakness =  0.25 * calculatedStats.BasicStats.Agility;
				apFromExposeWeakness *= CalcUptime(7, ((character.HunterTalents.ExposeWeakness / 3) *  critHitPercent * hitChance * shotsPerSec), options.duration  );
			}
			
			
			double RAP = 0;
			RAP += apFromBase;
			RAP += apFromAgil;
			RAP += apFromCarefulAim;
			RAP += apFromHunterVsWild;
			RAP += apFromGear;
			//RAP += apFromBuffs; //already included in gear-stat
			RAP += apFromBloodFury;
			RAP += apFromAspectOfTheHawk;
			RAP += apFromAspectMastery;
			RAP += apFromFuriousHowl;
			RAP += apFromExposeWeakness;
			//RAP += apFromOnProcEffects;
			RAP *= apFromCallOfTheWild;
			RAP *= apFromTrueshotAura;
			RAP += apFromHuntersMark;
			
			
			calculatedStats.RAPtotal = RAP;
            calculatedStats.RAP = (float)RAP;
			
			calculatedStats.apFromBase = apFromBase;
			calculatedStats.apFromAgil = apFromAgil;
			calculatedStats.apFromCarefulAim = apFromCarefulAim;
			calculatedStats.apFromHunterVsWild = apFromHunterVsWild;
			calculatedStats.apFromGear = apFromGear;
			calculatedStats.apFromBloodFury = apFromBloodFury;
			calculatedStats.apFromAspectOfTheHawk = apFromAspectOfTheHawk;
			calculatedStats.apFromAspectMastery = apFromAspectMastery;
			calculatedStats.apFromFuriousHowl = apFromFuriousHowl;
			calculatedStats.apFromCallOfTheWild = (apFromCallOfTheWild - 1)*100;
			calculatedStats.apFromTrueshotAura = (apFromTrueshotAura - 1)*100;
			calculatedStats.apFromHuntersMark = apFromHuntersMark;
			

			#endregion
			#region May 2009 Armor Reduction
			
			
			int targetArmor = options.TargetArmor;
			float damageReduction = StatConversion.GetArmorDamageReduction(character.Level, targetArmor,
				calculatedStats.BasicStats.ArmorPenetration, 0f, calculatedStats.BasicStats.ArmorPenetrationRating); 
			calculatedStats.damageReductionFromArmor = damageReduction;
			
			
			
			#endregion
			#region May 2009 Damage Boosts (Adjustments)
			
			//Beastial Wrath
			double beastialWrathCooldown = (2 * 60);
			if( character.HunterTalents.GlyphOfBestialWrath == true)
			{
				beastialWrathCooldown -= 20;
			}
			beastialWrathCooldown *= 1 - 0.1 * character.HunterTalents.Longevity;
			
			double beastialWrathUptime = CalcUptime ( 18, beastialWrathCooldown, options.duration) ;
			
			double blackArrowUptime = CalcUptime ( 15, 30, options.duration);
			
			//TODO: calculate this properly
			double ferociousInspirationUptime = 1;

            //Focused Fire
            double focusedFireDamageAdjust = 1 + 0.01 * character.HunterTalents.FocusedFire;

            //Beast Within
            double beastWithinDamageAdjust = 1 + (0.01 * character.HunterTalents.TheBeastWithin) * beastialWrathUptime;

            //Sanc. Retribution Aura
            double sancRetributionAuraDamageAdjust = 1 + statsBuffs.BonusDamageMultiplier;

            //Black Arrow Damage Multiplier
            double blackArrowDamageAdjust = 1 + (0.06 * character.HunterTalents.BlackArrow) * blackArrowUptime;

            //Noxious Stings
            double noxiousStingsDamageAdjust = 1 + (0.01 * character.HunterTalents.NoxiousStings);

            //Ferocious Inspiration
            double ferociousInspirationDamageAdjust = 1 + (0.01 * character.HunterTalents.FerociousInspiration) * ferociousInspirationUptime;

			//Improved Tracking
            double improvedTrackingDamageAdjust = 1 + 0.01 * character.HunterTalents.ImprovedTracking;

            //Ranged Weapon Specialization
            double rangedWeaponSpecializationDamageAdjust = 1;
            if (character.HunterTalents.RangedWeaponSpecialization == 1) rangedWeaponSpecializationDamageAdjust = 1.01;
            if (character.HunterTalents.RangedWeaponSpecialization == 2) rangedWeaponSpecializationDamageAdjust = 1.03;
            if (character.HunterTalents.RangedWeaponSpecialization == 3) rangedWeaponSpecializationDamageAdjust = 1.05;

            //Marked For Death (assume hunter's mark is on target)
            double markedForDeathDamageAdjust = 1 + 0.01 * character.HunterTalents.MarkedForDeath;

			//DamageTakenDebuffs
            double targetPhysicalDebuffsDamageAdjust = 1 + statsBuffs.DamageTakenMultiplier;

            //Barrage
            double barrageDamageAdjust = 1 + 0.04 * character.HunterTalents.Barrage;

            //Sniper Training
    		double sniperTrainingDamageAdjust = 1 + 0.02 * character.HunterTalents.SniperTraining;

            //Improved Steady Shot
            // TODO: calculate this correctly
            double improvedSSAimedShotDamageAdjust = 1;
            double improvedSSChimeraShotDamageAdjust = 1;

            //Improve Stings
            double improvedStingsDamageAdjust = 1 + 0.1 * character.HunterTalents.ImprovedStings;

			// Steady Shot Glyph
            double glyphOfSteadyShotDamageAdjust = character.HunterTalents.GlyphOfSteadyShot ? 1.1 : 1;

            //Improved Arcane Shot 
            double improvedArcaneShotDamageAdjust = 1 + 0.05 * character.HunterTalents.ImprovedArcaneShot;

            //TrapMastery
            double trapMasteryDamageAdjust = 1 + 0.1 * character.HunterTalents.TrapMastery;

            // T.N.T.
            double TNTDamageAdjust = 1 + 0.02 * character.HunterTalents.TNT;


            // These intermediates group the two common sets of adjustments
            double talentDamageAdjust = focusedFireDamageAdjust
                                            * beastWithinDamageAdjust
                                            * sancRetributionAuraDamageAdjust
                                            * blackArrowDamageAdjust
                                            * noxiousStingsDamageAdjust
                                            * ferociousInspirationDamageAdjust
                                            * improvedTrackingDamageAdjust
                                            * rangedWeaponSpecializationDamageAdjust
                                            * markedForDeathDamageAdjust;

            double talentDamageStingAdjust = focusedFireDamageAdjust
                                            * beastWithinDamageAdjust
                                            * sancRetributionAuraDamageAdjust
                                            * blackArrowDamageAdjust
                                            * noxiousStingsDamageAdjust
                                            * ferociousInspirationDamageAdjust;


            // everything below here (in this region) is being deprecated in favor of more transparency
            // by calculating intermediate values and doing the summing inside the shot region.
            double globalDamageBoost = 1;
            globalDamageBoost *= focusedFireDamageAdjust;
            globalDamageBoost *= beastWithinDamageAdjust;
            globalDamageBoost *= sancRetributionAuraDamageAdjust;
            globalDamageBoost *= blackArrowDamageAdjust;
            globalDamageBoost *= noxiousStingsDamageAdjust;
            globalDamageBoost *= ferociousInspirationDamageAdjust;
            globalDamageBoost *= improvedTrackingDamageAdjust;
            globalDamageBoost *= rangedWeaponSpecializationDamageAdjust;
            globalDamageBoost *= markedForDeathDamageAdjust;
            globalDamageBoost *= targetPhysicalDebuffsDamageAdjust;

            double arcaneShotDamageBoost = globalDamageBoost * improvedArcaneShotDamageAdjust;
            double blackarrowDamageBoost = globalDamageBoost * sniperTrainingDamageAdjust * trapMasteryDamageAdjust * TNTDamageAdjust;
            double multiShotDamageBoost = globalDamageBoost * barrageDamageAdjust;
            double volleyDamageBoost = globalDamageBoost * barrageDamageAdjust;
            double killShotDamageBoost = globalDamageBoost * sniperTrainingDamageAdjust;
             
			#endregion
			#region May 2009 Bonus Crit Chance
					
			//Improved Barrage
			double improvedBarrageCritModifier = 0.04 * character.HunterTalents.ImprovedBarrage;

			// Survival instincts
			double survivalInstinctsCritModifier = 0.02 * character.HunterTalents.SurvivalInstincts ;

			// Explosive Shot Glyph
            double glyphOfExplosiveShotCritModifier = character.HunterTalents.GlyphOfExplosiveShot ? 0.04 : 0;
		
			//Trueshot Aura Glyph
            double trueshotAuraGlyphCritModifier = 0;
			if (character.HunterTalents.GlyphOfTrueshotAura == true)
			{
				if (character.HunterTalents.TrueshotAura > 0)
				{
                    trueshotAuraGlyphCritModifier = 0.1;
				}
			}


            // these are (probably) correct, but will be moved down into their shot regions
            double arcaneBonusCritChance = 1 + survivalInstinctsCritModifier;
            double multiShotBonusCritChance = 1 + improvedBarrageCritModifier;

			
			#endregion
			#region May 2009 Bonus Crit Damage
						
			//MortalShots
            double mortalShotsCritDamage = 0.06 * character.HunterTalents.MortalShots;

			//CritDamageMetaGems
            double metaGemCritDamage = statsBaseGear.CritBonusDamage;

            //Marked For Death
            double markedForDeathCritDamage = 0.02 * character.HunterTalents.MarkedForDeath;


            // these are (probably) correct, but will be moved down into their shot regions
            double arcaneBonusCritDamage = 1 + mortalShotsCritDamage + metaGemCritDamage + markedForDeathCritDamage;
            double killBonusCritDamage = 1 + mortalShotsCritDamage + metaGemCritDamage + markedForDeathCritDamage;
            double multiShotBonusCritDamage = 1 + mortalShotsCritDamage + metaGemCritDamage;            
			
			#endregion			
            #region Partial Resists
            double averageResist = (options.TargetLevel - 80) * 0.02;
            double resist10 = 5 * averageResist;
            double resist20 = 2.5 * averageResist;
            double partialResist = 1 - (resist10 * 0.1 + resist20 * 0.1);
            #endregion

            double specialsPerSec = 0;

			// shot calcs

            // for each shot we calculate:
            //  * hit chance
            //  * crit chance
            //  * average non-crit shot damage (before damage adjustments)
            //  * average crit shot damage (before damage adjustments)
            //  * average shot damage (before damage adjustments)
            //  * average shot damage (after adjustments, called 'DPS', even though it's not)


            #region July 2009 AutoShot

            double rangedWeaponDamage = 0;
            double rangedWeaponSpeed = 0;
            double rangedAmmoDPS = 0;

            if (character.Ranged != null)
            {
                rangedWeaponDamage = (float)(character.Ranged.Item.MinDamage + character.Ranged.Item.MaxDamage) / 2f;
                rangedWeaponSpeed = character.Ranged.Item.Speed;
            }
            if (character.Projectile != null)
            {
                rangedAmmoDPS = (float)(character.Projectile.Item.MaxDamage + character.Projectile.Item.MinDamage) / 2f;
            }

            // scope damage only applies to autoshot, so is not added to the normalized damage
            double rangedAmmoDamage = rangedAmmoDPS * rangedWeaponSpeed;
            double rangedAmmoDamageNormalized = rangedAmmoDPS * 2.8;
            double damageFromRAP = (float)RAP / 14 * rangedWeaponSpeed;
            double damageFromRAPNormalized = (float)RAP / 14 * 2.8;
            double autoShotDamage = rangedWeaponDamage + rangedAmmoDamage + statsBaseGear.WeaponDamage + damageFromRAP + calculatedStats.BasicStats.ScopeDamage;
            double autoShotDamageNormalized = rangedWeaponDamage + rangedAmmoDamageNormalized + statsBaseGear.WeaponDamage + damageFromRAPNormalized;

            double autoShotDamageAdjust = talentDamageAdjust * targetPhysicalDebuffsDamageAdjust;
            double autoShotCritDamage = 1 + mortalShotsCritDamage + metaGemCritDamage;

            double autoShotDamageCrit = autoShotDamage * (1.0 + autoShotCritDamage);
            double autoShotDamageTotal = autoShotDamage * (1 - critHitPercent)
                                       + autoShotDamageCrit * critHitPercent;
            double autoShotDamageReal = autoShotDamageTotal * autoShotDamageAdjust * (1 - damageReduction) * hitChance;

            calculatedStats.BaseAutoshotDPS = autoShotDamageReal / autoShotSpeed;

            // this is used below, but id somewhat cryptic - will be phased out
            double autoShotHitMissAdjust = (critHitPercent * autoShotCritDamage + 1) * hitChance;

            //Debug.WriteLine("rangedWeaponDamage = " + rangedWeaponDamage);
            //Debug.WriteLine("rangedAmmoDamage = " + rangedAmmoDamage);
            //Debug.WriteLine("statsBaseGear.WeaponDamage = " + statsBaseGear.WeaponDamage);
            //Debug.WriteLine("damageFromRAP = " + damageFromRAP);
            //Debug.WriteLine("autoShotDamage = " + autoShotDamage);
            //Debug.WriteLine("autoShotDamageNormalized = " + autoShotDamageNormalized);
            //Debug.WriteLine("autoShotDPS : " + calculatedStats.BaseAutoshotDPS);

            calculatedStats.WildQuiverDPS = 0;
            //character.HunterTalents.WildQuiver = 2; // for isolation testing
            if (character.HunterTalents.WildQuiver > 0)
            {
                double wildQuiverFrequency = (autoShotSpeed / (character.HunterTalents.WildQuiver * 0.04));
                double wildQuiverBaseDamage = 0.8 * (rangedWeaponDamage + statsBaseGear.WeaponDamage + damageFromRAP);
                double wildQuiverAdjustment = autoShotHitMissAdjust * partialResist;
                double wildQuiverTotalDamage = wildQuiverBaseDamage * wildQuiverAdjustment;

                calculatedStats.WildQuiverDPS = wildQuiverTotalDamage / wildQuiverFrequency;

                //Debug.WriteLine("wildQuiverFrequency : " + wildQuiverFrequency);
                //Debug.WriteLine("wildQuiverBaseDamage : " + wildQuiverBaseDamage);
                //Debug.WriteLine("wildQuiverDPS : " + calculatedStats.WildQuiverDPS);
            }

            calculatedStats.AutoshotDPS = calculatedStats.BaseAutoshotDPS + calculatedStats.WildQuiverDPS;

            #endregion
            #region July 2009 Steady Shot

			// base = shot_base + gear_weapon_damage + normalized_ammo_dps + (RAP * 0.1)
            //        + (rangedWeaponDamage / ranged_weapon_speed * 2.8)
            double steadyShotBaseDamage = 252
                        + statsBaseGear.WeaponDamage
                        + rangedAmmoDamageNormalized
                        + (RAP * 0.1)
                        + (rangedWeaponDamage / rangedWeaponSpeed * 2.8);

            // crit chance = base_crit + 5%_rift_stalker_bonus + (2% * survivial_instincts)
            //TODO: add rift stalker set bonus
            double steadyShotCritChance = critHitPercent + survivalInstinctsCritModifier;

            // hit chance = base_hit
            double steadyShotHitChance = hitChance;

            // mana per shot
            double steadyShotManaCost = calculatedStats.BasicStats.Mana * 0.05; // 5% of base mana

            // adjust = talent_adjust * gronnstalker_bonus * glyph_of_steadyshot
            //          * sniper_training * physcial_debuffs
            // TODO: Gronnstalker set bonus
            double steadyShotDamageAdjust = talentDamageAdjust 
                                            * targetPhysicalDebuffsDamageAdjust
                                            * sniperTrainingDamageAdjust
                                            * glyphOfSteadyShotDamageAdjust;

            double steadyShotCritDamage = 1 + mortalShotsCritDamage + metaGemCritDamage + markedForDeathCritDamage;

            // the final stats
            calculatedStats.steadyCrit = steadyShotCritChance;
            calculatedStats.steadyHit = steadyShotHitChance;
            calculatedStats.steadyDamageNormal = steadyShotBaseDamage;
            calculatedStats.steadyDamageCrit = steadyShotBaseDamage * (1.0 + steadyShotCritDamage);
            calculatedStats.steadyDamageTotal = calculatedStats.steadyDamageNormal * (1 - calculatedStats.steadyCrit)
                                              + calculatedStats.steadyDamageCrit * calculatedStats.steadyCrit;
            calculatedStats.steadyDamagePerMana = calculatedStats.steadyDamageTotal / steadyShotManaCost;
            calculatedStats.steadyDPS = (calculatedStats.steadyDamageTotal * steadyShotDamageAdjust * (1.0 - damageReduction)) * steadyShotHitChance;

            //Debug.WriteLine("calculatedStats.steadyCrit = " + calculatedStats.steadyCrit);
            //Debug.WriteLine("calculatedStats.steadyHit. = " + calculatedStats.steadyHit);
            //Debug.WriteLine("calculatedStats.steadyDamageNormal = " + calculatedStats.steadyDamageNormal);
            //Debug.WriteLine("calculatedStats.steadyDamageCrit = " + calculatedStats.steadyDamageCrit);
            //Debug.WriteLine("calculatedStats.steadyDamageTotal = " + calculatedStats.steadyDamageTotal);
            //Debug.WriteLine("calculatedStats.steadyDamagePerMana = " + calculatedStats.steadyDamagePerMana);
            //Debug.WriteLine("calculatedStats.steadyDPS = " + calculatedStats.steadyDPS);

/*
 *          This is never used...
			double steadyCastTime;
			if ((2.0 / (1+ calculatedStats.hasteEffectsTotal/100 ) <= 1.5 ))
            {
            	steadyCastTime = (float) 1.5;
            }
            else
            {
            	steadyCastTime = (float)(2.0 / (1+ calculatedStats.hasteEffectsTotal/100));
            }
*/

            if (options.SteadyInRot)
            {
                specialsPerSec += 1 / calculatedStats.SteadySpeed;
            }

			#endregion	
			#region July 2009 Serpent Sting

            // base_damage = 1210 + (0.2 * RAP)
            double serpentStingDamageBase = 1210 + (calculatedStats.RAPtotal * 0.2);

            // damage_boost = sting_talent_adjusts * improved_stings * improved_tracking
            //                  + partial_resists * tier-8_2-piece_bonus * target_nature_debuffs
            // TODO: nature debuffs & t8 bonus
            double serpentStingDamageAdjust = talentDamageStingAdjust
                                                * improvedStingsDamageAdjust
                                                * improvedTrackingDamageAdjust
                                                * partialResist;

            double serpentStingDamageReal = serpentStingDamageBase * serpentStingDamageAdjust;

            double serpentStingDamagePerTick = Math.Round(serpentStingDamageReal / 5);

            // duration and ticks
            double serpentStingDuration = 15;
            if (character.HunterTalents.GlyphOfSerpentSting)
                serpentStingDuration += 6;
            double serpentStingTicks = Math.Floor(serpentStingDuration / 3);

            // unlike some shots, this really is DPS, not damage per shot
            calculatedStats.SerpentDPS = (serpentStingDamagePerTick * serpentStingTicks) / serpentStingDuration;

            //Debug.WriteLine("serpentStingBaseDamage = " + serpentStingBaseDamage);
            //Debug.WriteLine("serpentStingDamageBoost = " + serpentStingDamageBoost);
            //Debug.WriteLine("serpentStingDamagePerTick = " + serpentStingDamagePerTick);
            //Debug.WriteLine("serpentStingDuration = " + serpentStingDuration);
            //Debug.WriteLine("serpentStingTicks = " + serpentStingTicks);
            //Debug.WriteLine("calculatedStats.SerpentDPS = " + calculatedStats.SerpentDPS);

            if (options.SerpentInRot)
                specialsPerSec += 1 / serpentStingDuration;
			
			#endregion //Has DPS	
			#region July 2009 Aimed Shot
			
            // base_damage = normalized_shot + 408
            double aimedShotDamageNormal = autoShotDamageNormalized + 408;

            // crit = base_crit + trueshot_aura_glyph + improved_barrage
            double aimedShotCrit = critHitPercent + trueshotAuraGlyphCritModifier + improvedBarrageCritModifier;

            // crit_damage = 1 + mortal_shots + gem_crit + marked_for_death
            double aimedShotCritAdjust = 1 + mortalShotsCritDamage + metaGemCritDamage + markedForDeathCritDamage;

            // damage_adjust = talent_adjust * barrage_adjust * target_debuff_adjust * sniper_training_adjust * improved_ss_adjust
            double aimedShotDamageAdjust = talentDamageAdjust * barrageDamageAdjust * targetPhysicalDebuffsDamageAdjust
                                            * sniperTrainingDamageAdjust * improvedSSAimedShotDamageAdjust;

            // calculate the effective shot damage
            double aimedShotHit = hitChance;
            double aimedShotDamageCrit = aimedShotDamageNormal * (1 + aimedShotCritAdjust);
            double aimedShotDamageTotal = (aimedShotDamageNormal * (1 - aimedShotCrit) + (aimedShotDamageCrit * aimedShotCrit));
            double aimedShotDamageReal = aimedShotDamageTotal * aimedShotDamageAdjust * (1 - damageReduction) * aimedShotHit;

			double aimedShotCooldown = character.HunterTalents.GlyphOfAimedShot ? 8 : 10;

            calculatedStats.aimedDPCD = aimedShotDamageReal / aimedShotCooldown;

            //Debug.WriteLine("aimedShotDamageNormal = " + aimedShotDamageNormal);
            //Debug.WriteLine("aimedShotDamageCrit = " + aimedShotDamageCrit);
            //Debug.WriteLine("aimedShotDamageTotal = " + aimedShotDamageTotal);
            //Debug.WriteLine("aimedShotDamageReal = " + aimedShotDamageReal);
            //Debug.WriteLine("calculatedStats.aimedDPCD = " + calculatedStats.aimedDPCD);

            if (options.AimedInRot)
            {
                specialsPerSec += 1 / aimedShotCooldown;
            }

			#endregion//Has DPS
			#region July 2009 Explosive Shot

            // base_damage = 425 + 14% of RAP
            double explosiveShotDamageNormal = 425 + (RAP * 0.14);

            // crit = base_crit + glyph_of_es + survival_instincts
            double explosiveShotCrit = critHitPercent + glyphOfExplosiveShotCritModifier + survivalInstinctsCritModifier;

            // crit_damage = 1 + mortal_shots + gem-crit
            double explosiveShotCritAdjust = 1 + mortalShotsCritDamage + metaGemCritDamage;

            // damage_adjust = talent_adjust * tnt * fire_debuffs * sinper_training * partial_resist
            // TODO: missing fire debuffs
            double explosiveShotDamageAdjust = talentDamageAdjust * TNTDamageAdjust * sniperTrainingDamageAdjust * partialResist;

            double explosiveShotDamageCrit = explosiveShotDamageNormal * (1 + explosiveShotCritAdjust);
            double explosiveShotDamageTotal = (explosiveShotDamageNormal * (1 - explosiveShotCrit) + (explosiveShotDamageCrit * explosiveShotCrit));
            double explosiveShotDamageReal = explosiveShotDamageTotal * explosiveShotDamageAdjust * hitChance;

            double explosiveShotDamagePerShot = explosiveShotDamageReal * 3;

            calculatedStats.ExplosiveShotDPS = explosiveShotDamagePerShot / 6;

            //Debug.WriteLine("explosiveShotDamageNormal = " + explosiveShotDamageNormal);
            //Debug.WriteLine("explosiveShotCrit = " + explosiveShotCrit);
            //Debug.WriteLine("explosiveShotCritAdjust = " + explosiveShotCritAdjust);
            //Debug.WriteLine("explosiveShotDamageAdjust = " + explosiveShotDamageAdjust);
            //Debug.WriteLine("explosiveShotDamagePerShot = " + explosiveShotDamagePerShot);
            //Debug.WriteLine("calculatedStats.ExplosiveShotDPS = " + calculatedStats.ExplosiveShotDPS);

            if (options.ExplosiveInRot)
                specialsPerSec += 1 / 6;

			#endregion
            #region July 2009 Chimera Shot

            // base_damage = normalized_autoshot * 125%
            double chimeraShotDamageNormal = autoShotDamageNormalized * 1.25;

            // crit for 'specials'
            double chimeraShotCritAdjust = 1 + mortalShotsCritDamage + metaGemCritDamage + markedForDeathCritDamage; 

            // damage_adjust = talent_adjust * nature_debuffs * ISS_cs_bonus * partial_resist
            // TODO: nature_debuffs
            double chimeraShotDamageAdjust = talentDamageAdjust * improvedSSChimeraShotDamageAdjust * partialResist;

            double chimeraShotDamageReal = CalcEffectiveDamage(
                                                chimeraShotDamageNormal,
                                                hitChance,
                                                critHitPercent,
                                                chimeraShotCritAdjust,
                                                chimeraShotDamageAdjust
                                           );


            // calculate damage from serpent sting
            double chimeraShotSerpentDamage = serpentStingDamageReal * 0.4;
            double chimeraShotSerpentCritAdjust = 1 + mortalShotsCritDamage + metaGemCritDamage;
            double chimeraShotSerpentDamageAdjust = talentDamageAdjust * 1; // TODO: add nature_debuffs here!

            double chimeraShotSerpentDamageReal = CalcEffectiveDamage(
                                                    chimeraShotSerpentDamage,
                                                    hitChance,
                                                    critHitPercent,
                                                    chimeraShotSerpentCritAdjust,
                                                    chimeraShotSerpentDamageAdjust
                                                 );

            double chimeraShotDamageTotal = chimeraShotDamageReal + chimeraShotSerpentDamageReal;

            double chimeraShotCooldown = character.HunterTalents.GlyphOfChimeraShot ? 9 : 10;

            calculatedStats.ChimeraShotDPS = chimeraShotDamageTotal / chimeraShotCooldown;

            //Debug.WriteLine("serpentStingDamageReal = " + serpentStingDamageReal);
            //Debug.WriteLine("chimeraShotDamageNormal = " + chimeraShotDamageNormal);
            //Debug.WriteLine("chimeraShotDamageReal = " + chimeraShotDamageReal);
            //Debug.WriteLine("chimeraShotSerpentDamage = " + chimeraShotSerpentDamage);
            //Debug.WriteLine("chimeraShotSerpentDamageReal = " + chimeraShotSerpentDamageReal);
            //Debug.WriteLine("chimeraShotDamageTotal = " + chimeraShotDamageTotal);
            //Debug.WriteLine("chimeraShotCooldown = " + chimeraShotCooldown);
            //Debug.WriteLine("calculatedStats.ChimeraShotDPS = " + calculatedStats.ChimeraShotDPS);

            if (options.ChimeraInRot)
                specialsPerSec += 0.1;

            #endregion
            #region May 2009 Arcane Shot
            double arcaneShotDamageNormal = RAP * 0.15 + 492;
			double arcaneShotCastTime = GCD	;
			double arcaneCD = 6;
			double arcaneCrit = critHitPercent * arcaneBonusCritChance;
            double arcaneHit = hitChance;
			double arcaneShotCritDamage =  arcaneShotDamageNormal * (1.0 + 1.0 * arcaneBonusCritDamage);
			double arcaneDamageTotal = (arcaneShotDamageNormal * (1 - arcaneCrit) + (arcaneShotCritDamage * arcaneCrit));
			double arcaneDPS =  ((arcaneDamageTotal * arcaneShotDamageBoost ) / arcaneShotCastTime) * arcaneHit;
			double arcaneDPCD = arcaneDPS/arcaneCD;
            calculatedStats.arcaneDPS = arcaneDPCD;

            if (options.ArcaneInRot)
                specialsPerSec += 1 / 6;
			
			#endregion		
			#region May 2009 Multi Shot
			
			double multiShotNormalDamage = autoShotDamageNormalized + 408;
			double multiShotCastTime = GCD;
			double multiShotCD = 10;
			if (character.HunterTalents.GlyphOfMultiShot)
			{
				multiShotCD -= 1;
			}
			double multiShotHit = hitChance;
			double multiShotCrit = critHitPercent * multiShotBonusCritChance;
			double multiShotCritDamage = multiShotNormalDamage * ( 1.0 + 1.0 * multiShotBonusCritDamage);
			double multiShotDamageTotal = (multiShotNormalDamage * (1 - multiShotCrit) + (multiShotCritDamage * multiShotCrit));
			double multiShotDPS = ((multiShotDamageTotal * multiShotDamageBoost * (1 - damageReduction) ) /multiShotCastTime ) * multiShotHit;
			double multishotDPCD = multiShotDPS / multiShotCD;
			calculatedStats.multiDPCD = multishotDPCD;

            if (options.MultiInRot)
                specialsPerSec += 1 / multiShotCD;

			#endregion
			#region May 2009 Black Arrow
            double baDmgFromRap = calculatedStats.RAPtotal * 0.1;
            double baDmg = (2765 + baDmgFromRap) * blackarrowDamageBoost;
            double baCD=30 - (character.HunterTalents.Resourcefulness * 2);
            calculatedStats.BlackDPS = baDmg / baCD;

            if (options.BlackInRot)
                specialsPerSec += baCD;
			#endregion
			#region May 2009 Kill Shot
            double ksDmgFromRAP = calculatedStats.RAPtotal * 0.4;
            double ksWpnDmg = (character.Ranged.MaxDamage + character.Ranged.MinDamage) / 2 + (character.Ranged.Speed * 46.5);
            double ksDmg = (650 + ksDmgFromRAP + ksWpnDmg) * killShotDamageBoost;
            double ksCD = 15;
            if (character.HunterTalents.GlyphOfKillShot)
                ksCD -= 6;
            calculatedStats.KillDPS = (ksDmg / ksCD) * 0.2;

            if (options.KillInRot)
                specialsPerSec += 1 / ksCD;
			#endregion
            #region May 2009 SilencingShot
            calculatedStats.SilencingDPS = autoShotDamageNormalized * 0.5 * autoShotHitMissAdjust;
            
            if (options.SilenceInRot)
                specialsPerSec += 0.5;
            #endregion

            #region Base Attack Speed
            //Hasted Speed = Weapon Speed / ( (1+(Haste1 %)) * (1+(Haste2 %)) * (1+(((Haste Rating 1 + Haste Rating 2 + ... )/100)/15.7)) )
            double totalStaticHaste = (1.0 + (calculatedStats.BasicStats.HasteRating / HunterRatings.HASTE_RATING_PER_PERCENT / 100.0));

            {
                totalStaticHaste *= (1.0 + .04 * character.HunterTalents.SerpentsSwiftness) * HunterRatings.QUIVER_SPEED_INCREASE;
              //  totalStaticHaste *= (1.0 + calculatedStats.BasicStats.PhysicalHaste);
            }

            double normalAutoshotsPerSecond = 0.0;
            totalStaticHaste = 1+ calculatedStats.hasteEffectsTotal/100;
            if (character.Ranged != null)
            {
                calculatedStats.BaseAttackSpeed = (float)(character.Ranged.Item.Speed / (totalStaticHaste));
                normalAutoshotsPerSecond = 1.0 / calculatedStats.BaseAttackSpeed;
            }
			
            //even if steady shot goes faster, the gcd is still 1.5
            if ((2.0 / (1+ calculatedStats.hasteEffectsTotal/100 ) <= 1.5 ))
            {
            	calculatedStats.SteadySpeed = (float) 1.5;
            }
            else
            {
            	calculatedStats.SteadySpeed = (float)(2.0 / (1+ calculatedStats.hasteEffectsTotal/100));
            }
            
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
            #region Critical Hit Damage

          //  double autoshotCritDmgModifier = 1.0;
            double abilitiesCritDmgModifier = 1.0 + character.HunterTalents.MortalShots * 0.06;

            #endregion
            #region Pet

            PetCalculations pet = new PetCalculations(character, calculatedStats, options, statsBuffs, PetFamily.Bat, statsBaseGear);

            #endregion
            #region Talent Modifiers
            double talentModifiers = 1.0f;

            talentModifiers *= 1.0f + 0.01f * character.HunterTalents.RangedWeaponSpecialization;
            talentModifiers *= 1.0f + 0.01f * character.HunterTalents.FocusedFire;
            talentModifiers *= 1.0f + 0.1 * character.HunterTalents.TrueshotAura;
            talentModifiers *= 1.0f + 0.1f * character.HunterTalents.ImprovedHuntersMark;
            talentModifiers *= 1.0f + 0.01f * character.HunterTalents.ImprovedTracking;

            double talentDmgModifiers = 1.0 + pet.ferociousInspirationUptime * character.HunterTalents.FerociousInspiration * 0.01;
            talentDmgModifiers *= calculatedStats.BasicStats.BonusDamageMultiplier;

            #endregion
            #region May 2009 Mana Regen
            double manaPerSecond = calculatedStats.BasicStats.Mp5 / 5;
            manaPerSecond += 0.6 * Math.Sqrt(calculatedStats.BasicStats.Intellect) * calculatedStats.BasicStats.Spirit * 0.005575;
            manaPerSecond += (statsBuffs.ManaRestoreFromMaxManaPerSecond * calculatedStats.BasicStats.Mana);
            double autoShotsPerMin = shotsPerSec * 60;

            //RapidRecuperation
            manaPerSecond += (0.02 * character.HunterTalents.RapidRecuperation * calculatedStats.BasicStats.Mana / 3) * CalcUptime(15, rapidFireCooldown, options.duration);

            calculatedStats.manaRegenGearBuffs = calculatedStats.BasicStats.Mp5 / 5;
            calculatedStats.manaRegenBase = Math.Sqrt(calculatedStats.BasicStats.Intellect) * calculatedStats.BasicStats.Spirit * 0.005575;
            calculatedStats.manaRegenReplenishment = (statsBuffs.ManaRestoreFromMaxManaPerSecond * calculatedStats.BasicStats.Mana);

            calculatedStats.manaRegenTotal = manaPerSecond;
            #endregion

            calculatedStats.PetDpsPoints = pet.getDPS();
            calculatedStats.HunterDpsPoints = Rotation.getDPS(options, calculatedStats);
            calculatedStats.OverallPoints = calculatedStats.HunterDpsPoints + calculatedStats.PetDpsPoints;

            return calculatedStats;            
		}
        Stats statsBaseGear = new Stats();
        Stats statsBuffs = new Stats();
		public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
            Stats statsRace = BaseStats.GetBaseStats(80, CharacterClass.Hunter, character.Race);
			statsBaseGear = GetItemStats(character, additionalItem);
			statsBuffs = GetBuffsStats(character.ActiveBuffs);
			Stats statsTalents = GetBaseTalentStats(character.HunterTalents);
			Stats statsGearEnchantsBuffs = statsBaseGear + statsBuffs;
			statsGearEnchantsBuffs.Agility += statsGearEnchantsBuffs.AverageAgility;

			CalculationOptionsHunter options = character.CalculationOptions as CalculationOptionsHunter;
			if (options == null)
			{
				options = new CalculationOptionsHunter();
				character.CalculationOptions = options;
			}
            int targetDefence = 5 * options.TargetLevel;

			Stats statsTotal = new Stats();
			statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier)) - 1;
			statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier) * (1 + statsTalents.BonusAgilityMultiplier)) - 1;
			statsTotal.BonusIntellectMultiplier = ((1 + statsRace.BonusIntellectMultiplier) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier) * (1 + statsTalents.BonusIntellectMultiplier)) - 1;
			statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;
			statsTotal.BonusSpellPowerMultiplier = ((1 + statsRace.BonusSpellPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpellPowerMultiplier)) - 1;
			statsTotal.BonusArcaneDamageMultiplier = ((1 + statsRace.BonusArcaneDamageMultiplier) * (1 + statsGearEnchantsBuffs.BonusArcaneDamageMultiplier)) - 1;
			statsTotal.BonusPetDamageMultiplier = ((1 + statsGearEnchantsBuffs.BonusPetDamageMultiplier) * (1 + statsRace.BonusPetDamageMultiplier)) - 1;
			statsTotal.BonusSteadyShotDamageMultiplier = ((1 + statsGearEnchantsBuffs.BonusSteadyShotDamageMultiplier) * (1 + statsRace.BonusSteadyShotDamageMultiplier) * (1 + statsTalents.BonusSteadyShotDamageMultiplier)) - 1;
			statsTotal.BonusSpiritMultiplier = ((1 + statsRace.BonusSpiritMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier) * (statsTalents.BonusSpiritMultiplier)) - 1;
			
			statsTotal.Agility = (statsRace.Agility + statsGearEnchantsBuffs.Agility) * (1 + statsTotal.BonusAgilityMultiplier);
			statsTotal.Intellect = (statsRace.Intellect + statsGearEnchantsBuffs.Intellect) * (1 + statsTotal.BonusIntellectMultiplier);
			statsTotal.Stamina = (statsRace.Stamina + statsGearEnchantsBuffs.Stamina) * (1 + statsTotal.BonusStaminaMultiplier); 
			statsTotal.Spirit = (statsRace.Spirit + statsGearEnchantsBuffs.Spirit);  // * (1 + statsTotal.BonusSpiritMultiplier);
			
			statsTotal.Resilience = statsRace.Resilience + statsGearEnchantsBuffs.Resilience;
			statsTotal.Armor = (float)Math.Round((statsGearEnchantsBuffs.Armor + statsRace.Armor + (statsTotal.Agility * 2f)) * (1 + statsBuffs.BonusArmorMultiplier));

            if (character.HunterTalents.ThickHide > 0)
                statsTotal.Armor *= 1 + ((character.HunterTalents.ThickHide * 0.03f) + 0.01f);

            statsTotal.Miss = 0.0f;
			statsTotal.ArmorPenetration = statsRace.ArmorPenetration + statsGearEnchantsBuffs.ArmorPenetration;
            statsTotal.ArmorPenetrationRating = statsRace.ArmorPenetrationRating + statsGearEnchantsBuffs.ArmorPenetrationRating;
			statsTotal.BloodlustProc = statsRace.BloodlustProc + statsGearEnchantsBuffs.BloodlustProc;
            statsTotal.BonusCritMultiplier = 0.0f; // ((1 + statsRace.BonusCritMultiplier) * (1 + statsGearEnchantsBuffs.BonusCritMultiplier)) - 1;
            statsTotal.PhysicalCrit = statsBuffs.PhysicalCrit;
            
            statsTotal.CritRating = (float)Math.Floor((double)statsRace.CritRating + (double)statsGearEnchantsBuffs.CritRating + (double)statsGearEnchantsBuffs.RangedCritRating + (double)statsRace.LotPCritRating + (double)statsGearEnchantsBuffs.LotPCritRating);
            statsTotal.HasteRating = statsRace.HasteRating + statsGearEnchantsBuffs.HasteRating + statsGearEnchantsBuffs.RangedHasteRating;
            // Haste trinket (Meteorite Whetstone)
           // statsTotal.HasteRating += statsGearEnchantsBuffs.HasteRatingOnPhysicalAttack * 10 / 45;
            statsTotal.RangedHaste = statsGearEnchantsBuffs.RangedHaste;
            	
            statsTotal.PhysicalHaste = statsGearEnchantsBuffs.PhysicalHaste;
			
			statsTotal.HitRating = (float)Math.Floor((double)statsRace.HitRating + (double)statsGearEnchantsBuffs.HitRating + (double)statsGearEnchantsBuffs.RangedHitRating);
			statsTotal.ExposeWeakness = statsRace.ExposeWeakness + statsGearEnchantsBuffs.ExposeWeakness;
			statsTotal.Bloodlust = statsRace.Bloodlust + statsGearEnchantsBuffs.Bloodlust;
			statsTotal.ShatteredSunMightProc = statsRace.ShatteredSunMightProc + statsGearEnchantsBuffs.ShatteredSunMightProc;
			statsTotal.Mp5 = statsRace.Mp5 + statsGearEnchantsBuffs.Mp5;
			statsTotal.BonusPetCritChance = statsGearEnchantsBuffs.BonusPetCritChance;
			statsTotal.ScopeDamage = statsGearEnchantsBuffs.ScopeDamage;
			statsTotal.BonusSteadyShotCrit = statsGearEnchantsBuffs.BonusSteadyShotCrit;

            statsTotal.BonusDamageMultiplier = 1.0f + statsGearEnchantsBuffs.BonusDamageMultiplier;
            statsTotal.BonusAttackPowerMultiplier = 1.0f + statsGearEnchantsBuffs.BonusAttackPowerMultiplier;

            // The first 20 Int = 20 Mana, while each subsequent Int = 15 Mana
            // (20-(20/15)) = 18.66666
            statsTotal.Mana = (float)Math.Round(statsRace.Mana + 15f * (statsTotal.Intellect - (20f - (20f / 15f))) + statsGearEnchantsBuffs.Mana);

            // TODO: Implement new racials
            // The first 20 Stam = 20 Health, while each subsequent Stam = 10 Health, so Health = (Stam-18)*10
            // (20-(20/10)) = 18
            statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + ((statsTotal.Stamina - 18.0f) * 10f)) * (character.Race == CharacterRace.Tauren ? 1.05f : 1f)));

			statsTotal.Health += (float)Math.Round((statsTotal.Health * character.HunterTalents.EnduranceTraining * .01f));

            float hitBonus = (float)(statsTotal.HitRating / (HunterRatings.HIT_RATING_PER_PERCENT * 100.0f) + statsTalents.PhysicalHit + statsRace.PhysicalHit);

            float chanceMiss = Math.Max(0f, 0.08f - hitBonus);
            if ((options.TargetLevel - 80f) < 3)
                chanceMiss = Math.Max(0f, 0.05f + 0.005f * (options.TargetLevel - 80f) - hitBonus);

            statsTotal.PhysicalHit = 1.0f - chanceMiss;

 			if (character.Ranged != null &&
				((character.Race == CharacterRace.Dwarf && character.Ranged.Item.Type == ItemType.Gun) ||
				(character.Race == CharacterRace.Troll && character.Ranged.Item.Type == ItemType.Bow)))
			{
                statsTotal.CritRating += (float)Math.Floor(HunterRatings.CRIT_RATING_PER_PERCENT);
			}

            statsTotal.PhysicalCrit = (float)(HunterRatings.BASE_CRIT_PERCENT + (statsTotal.Agility / HunterRatings.AGILITY_PER_CRIT / 100.0f)
                                + (statsTotal.CritRating / HunterRatings.CRIT_RATING_PER_PERCENT / 100.0f)
                                + ((350 - targetDefence) * 0.04 / 100.0f)
								+ statsTalents.PhysicalCrit
                                + statsTotal.PhysicalCrit);

			statsTotal.AttackPower = statsGearEnchantsBuffs.AttackPower + statsGearEnchantsBuffs.RangedAttackPower;
			statsTotal.BonusRangedAttackPowerMultiplier = statsGearEnchantsBuffs.BonusRangedAttackPowerMultiplier;
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
		
		
		
		public double CalcTimeToOOM (double shotsPerSecond, double shotsAvgManaUse, double mana, double regen)
		{
			double manaUsePerSecond = (1/shotsAvgManaUse) * shotsPerSecond;
			if (manaUsePerSecond <= regen)
				return 0;
			else
			{
				manaUsePerSecond -= regen;
				double timeTilOOM = mana / manaUsePerSecond;
				return timeTilOOM;
			}
		}
		
		public double AspectOfViperRegen (double shotsPerSecond, double weaponspeed, double mana, bool glyph)
		{
			double manaPerSecond = 0;
			manaPerSecond += mana * weaponspeed * shotsPerSecond;
			if (glyph == true)
			{
				manaPerSecond *= 1.1;
			}
			manaPerSecond += 0.04 * mana / 3;
			return manaPerSecond;
		}

        private double CalcUptime(double duration, double cooldown, double length)
        {
            double durationleft = length;
            double numBuff = 0;
            if (duration >= cooldown)
            {
                return 1;
            }

            while (durationleft > 0)
            {
                if (durationleft > duration)
                {
                    numBuff += 1;
                }
                else
                {
                    numBuff += (durationleft / duration);
                }
                durationleft -= cooldown;
            }
            return ((numBuff * duration) / length);

        }

        private double CalcEffectiveDamage(double damageNormal, double hitChance, double critChance, double critAdjust, double damageAdjust)
        {

            double damageCrit = damageNormal * (1 + critAdjust);
            double damageTotal = (damageNormal * (1 - critChance)
                               + (damageCrit * critChance));
            double damageReal = damageTotal * damageAdjust * hitChance;

            return damageReal;
        }

		private Stats GetPetStats(CalculationOptionsHunter options, CharacterCalculationsHunter hunterStats, Character character)
		{
            //Moved to one place so changes are easier, all now in PetCalculations
            return new PetCalculations(character, hunterStats, options, statsBuffs, options.PetFamily, statsBaseGear).petStats;
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
