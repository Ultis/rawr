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

                "Pet Stats:Pet Attack Power",
				"Pet Stats:Pet Hit Percentage",
				"Pet Stats:Pet Crit Percentage",
				"Pet Stats:Pet Base DPS",
				"Pet Stats:Pet Special DPS*Based on all damaging or DPS boosting skills on auto-cast",

				"Shot Stats:Aimed Shot",
				"Shot Stats:Arcane Shot",
				"Shot Stats:Multi Shot",
				"Shot Stats:Serpent Sting",
				"Shot Stats:Scorpid Sting",
				"Shot Stats:Viper Sting",
                "Shot Stats:Silencing Shot",
                "Shot Stats:Steady Shot",
                "Shot Stats:Kill Shot",
				"Shot Stats:Explosive Shot",
				"Shot Stats:Black Arrow",
                "Shot Stats:Immolation Trap",
                "Shot Stats:Chimera Shot",
                "Shot Stats:Rapid Fire",
                "Shot Stats:Readiness",
                "Shot Stats:Beastial Wrath",
                "Shot Stats:Blood Fury",
                "Shot Stats:Berserk",

                "Hunter DPS:Autoshot DPS",
                "Hunter DPS:Priority Rotation DPS",
                "Hunter DPS:Wild Quiver DPS",
                "Hunter DPS:Proc DPS",
                "Hunter DPS:Kill Shot low HP gain",
                "Hunter DPS:Aspect Loss",

				"Combined DPS:Hunter DPS",
				"Combined DPS:Pet DPS",
				"Combined DPS:Total DPS"
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

        // NOTE: setting this to true does 'bad' uptime calculations,
        // to help match the spread sheet. if a fight last 10 seconds
        // and an ability has a 4 second cooldown, the spreadsheet says
        // you can use it 2.5 times, while we say you can use it twice.
        public bool calculateUptimesLikeSpreadsheet = true;

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

            #region August 2009 Priority Rotation Setup

            calculatedStats.priorityRotation = new ShotPriority();

            calculatedStats.priorityRotation.latency = options.Latency;

            calculatedStats.priorityRotation.priorities[0] = getShotByIndex(options.PriorityIndex1, calculatedStats);
            calculatedStats.priorityRotation.priorities[1] = getShotByIndex(options.PriorityIndex2, calculatedStats);
            calculatedStats.priorityRotation.priorities[2] = getShotByIndex(options.PriorityIndex3, calculatedStats);
            calculatedStats.priorityRotation.priorities[3] = getShotByIndex(options.PriorityIndex4, calculatedStats);
            calculatedStats.priorityRotation.priorities[4] = getShotByIndex(options.PriorityIndex5, calculatedStats);
            calculatedStats.priorityRotation.priorities[5] = getShotByIndex(options.PriorityIndex6, calculatedStats);
            calculatedStats.priorityRotation.priorities[6] = getShotByIndex(options.PriorityIndex7, calculatedStats);
            calculatedStats.priorityRotation.priorities[7] = getShotByIndex(options.PriorityIndex8, calculatedStats);
            calculatedStats.priorityRotation.priorities[8] = getShotByIndex(options.PriorityIndex9, calculatedStats);
            calculatedStats.priorityRotation.priorities[9] = getShotByIndex(options.PriorityIndex10, calculatedStats);

            calculatedStats.priorityRotation.validateShots(character.HunterTalents);

            #endregion
            #region August 2009 Shot Cooldowns & Durations

            calculatedStats.serpentSting.cooldown = 1.5;
            calculatedStats.serpentSting.duration = character.HunterTalents.GlyphOfSerpentSting ? 21 : 15;

            calculatedStats.aimedShot.cooldown = character.HunterTalents.GlyphOfAimedShot ? 8 : 10;

            calculatedStats.explosiveShot.cooldown = 6;

            calculatedStats.chimeraShot.cooldown = character.HunterTalents.GlyphOfChimeraShot ? 9 : 10;

            calculatedStats.arcaneShot.cooldown = 6;

            calculatedStats.multiShot.cooldown = character.HunterTalents.GlyphOfMultiShot ? 9 : 10;
            // TODO: remove one more second from multi-shot for galdiator's pursuit set bonus

            calculatedStats.blackArrow.cooldown = 30 - (character.HunterTalents.Resourcefulness * 2);
            calculatedStats.blackArrow.duration = 15;

            calculatedStats.killShot.cooldown = character.HunterTalents.GlyphOfKillShot ? 9 : 15;

            calculatedStats.silencingShot.cooldown = 20;

            calculatedStats.rapidFire.cooldown = 300 - (60 * character.HunterTalents.RapidKilling);
            // TODO: readiness should affect rapid fire cooldown
            calculatedStats.rapidFire.duration = 15;

            // We will set the correct value for this later, after we've calculated haste
            calculatedStats.steadyShot.cooldown = 2;

            // We can calculate the rough frequencies now
            calculatedStats.priorityRotation.calculateFrequencies();

            #endregion


            #region May 2009 Haste Calcs

            double hasteFromRacial = 1;
            double hastePercentFromRacial = 0;
            if (character.Race == CharacterRace.Troll)
            {
                hastePercentFromRacial = 10 * CalcUptime(10, (3 * 60), options.duration);
                hasteFromRacial += hastePercentFromRacial / 100;
            }

            //default quiver speed
            calculatedStats.hasteFromBase = 15.0;

            // haste from haste rating
            calculatedStats.hasteFromRating = calculatedStats.BasicStats.HasteRating / HunterRatings.HASTE_RATING_PER_PERCENT;

            // serpent swiftness
            calculatedStats.hasteFromTalentsStatic = 4.0 * character.HunterTalents.SerpentsSwiftness;

            // rapid fire (we know real rotation frequency already)
            double rapidFireHaste = character.HunterTalents.GlyphOfRapidFire ? 48.0 : 40.0;
            double rapidFireCooldown = calculatedStats.rapidFire.freq;

            if (!calculatedStats.priorityRotation.containsShot(Shots.RapidFire))
            {
                rapidFireHaste = 0;
            }

            calculatedStats.hasteFromProcs = rapidFireHaste * CalcUptime(15, rapidFireCooldown, options.duration);

            calculatedStats.hasteFromRangedBuffs = calculatedStats.BasicStats.RangedHaste * 100;

            double autoShotPreProcs = 1.0;
            {
                autoShotPreProcs *= (1.0 + calculatedStats.hasteFromBase / 100.0);
                autoShotPreProcs *= (1.0 + calculatedStats.hasteFromRating / 100.0);
                autoShotPreProcs *= (1.0 + calculatedStats.hasteFromTalentsStatic / 100.0);
                autoShotPreProcs *= (1.0 + calculatedStats.hasteFromProcs / 100.0);
                autoShotPreProcs *= (1.0 + calculatedStats.hasteFromRangedBuffs / 100);
                autoShotPreProcs *= hasteFromRacial;
                autoShotPreProcs = (character.Ranged.Item.Speed / autoShotPreProcs);
            }

            // improved aspect of the hawk, improved aspect of the hawk glyph, 	
            calculatedStats.hasteFromTalentsProc = 3.0 * character.HunterTalents.ImprovedAspectOfTheHawk;
            {
                if ((character.HunterTalents.ImprovedAspectOfTheHawk >= 1) && (character.HunterTalents.GlyphOfTheHawk == true))
                {
                    calculatedStats.hasteFromTalentsProc += 6.0;
                }
                // uptime =  12 second duration /  time between procs aka ( autoshot speed / chancetoproc )
                calculatedStats.hasteFromTalentsProc *= 12.0 / (autoShotPreProcs / 0.1);
            }

            calculatedStats.hasteEffectsTotal = 1.0;
            {
                calculatedStats.hasteEffectsTotal *= (1.0 + calculatedStats.hasteFromBase / 100.0);
                calculatedStats.hasteEffectsTotal *= (1.0 + calculatedStats.hasteFromRating / 100.0);
                calculatedStats.hasteEffectsTotal *= (1.0 + calculatedStats.hasteFromTalentsProc / 100.0);
                calculatedStats.hasteEffectsTotal *= (1.0 + calculatedStats.hasteFromTalentsStatic / 100.0);
                calculatedStats.hasteEffectsTotal *= (1.0 + calculatedStats.hasteFromProcs / 100.0);
                calculatedStats.hasteEffectsTotal *= (1.0 + calculatedStats.hasteFromRangedBuffs / 100);
                calculatedStats.hasteEffectsTotal *= hasteFromRacial;
                calculatedStats.hasteEffectsTotal = (calculatedStats.hasteEffectsTotal - 1.0) * 100;
            }

            double hasteMultiplier = 1.0 + (calculatedStats.hasteEffectsTotal / 100);

            double totalStaticHaste = (1 + calculatedStats.hasteFromBase / 100)             // quiver
                                    * (1 + calculatedStats.hasteFromRating / 100)           // gear haste rating
                                    * (1 + calculatedStats.hasteFromTalentsStatic / 100)    // serpent's swiftness
                                    * (1 + calculatedStats.hasteFromRangedBuffs / 100);     // buffs like swift ret / moonkin

            double totalDynamicHaste = (1 + calculatedStats.hasteFromProcs / 100)           // rapid fire
                                     * (1 + hastePercentFromRacial / 100)                   // troll beserking
                                     * (1 + 0)                                              // TODO: heroism
                                     * (1 + 0);                                             // TODO: Proc haste from gear (trinkets, etc)

            //Debug.WriteLine("totalStaticHaste = " + totalStaticHaste);
            //Debug.WriteLine("totalDynamicHaste = " + totalDynamicHaste);
            //Debug.WriteLine("calculatedStats.hasteFromRating = " + calculatedStats.hasteFromRating);
            //Debug.WriteLine("hastePercentFromRacial = " + hastePercentFromRacial);

            // Now we have the haste, we can calculate steady shot cast time
            // And so rebuild other various times
            calculatedStats.steadyShot.cooldown = 2 * (1 / (totalStaticHaste * totalDynamicHaste));
            calculatedStats.priorityRotation.calculateFrequencies();

            #endregion
            #region August 2009 Shots Per Second

            double QSBaseFreqnecyIncrease = 0; //TODO!
            double autoShotSpeed = character.Ranged.Speed / (totalStaticHaste * totalDynamicHaste);

            double baseAutoShotsPerSecond = autoShotSpeed > 0 ? 1 / autoShotSpeed : 0;
            double autoShotsPerSecond = baseAutoShotsPerSecond + QSBaseFreqnecyIncrease;
            double specialShotsPerSecond = calculatedStats.priorityRotation.specialShotsPerSecond;
            double totalShotsPerSecond = autoShotsPerSecond + specialShotsPerSecond;

            double crittingSpecialsPerSecond = calculatedStats.priorityRotation.critSpecialShotsPerSecond;
            double crittingShotsPerSecond = autoShotsPerSecond + crittingSpecialsPerSecond;

            calculatedStats.BaseAttackSpeed = (float)autoShotSpeed;

            //Debug.WriteLine("baseAutoShotsPerSecond = " + baseAutoShotsPerSecond);
            //Debug.WriteLine("autoShotsPerSecond = " + autoShotsPerSecond);
            //Debug.WriteLine("specialShotsPerSecond = " + specialShotsPerSecond);
            //Debug.WriteLine("Total shots per second = " + totalShotsPerSecond);

            #endregion
            #region May 2009 Hit Chance
            double missPercent = HunterRatings.BASE_MISS_PERCENT;
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
            #region August 2009 Crit Chance

            calculatedStats.critBase = HunterRatings.BASE_CRIT_PERCENT;

            calculatedStats.critFromAgi = (calculatedStats.BasicStats.Agility / HunterRatings.AGILITY_PER_CRIT) / 100;

            calculatedStats.critFromRating = (calculatedStats.BasicStats.CritRating / HunterRatings.CRIT_RATING_PER_PERCENT) / 100;

            // TODO: DK Anguish & Dark Matter trinket
            calculatedStats.critFromProcRating = 0;

            // Simple talents
            calculatedStats.critFromLethalShots = character.HunterTalents.LethalShots * 0.01;
            calculatedStats.critFromKillerInstincts = character.HunterTalents.KillerInstinct * 0.01;
            calculatedStats.critFromMasterMarksman = character.HunterTalents.MasterMarksman * 0.01;

            // Master Tactician
            double masterTacticianProcChance = 0.02 * character.HunterTalents.MasterTactician;
            double masterTacticianShotsIn8Seconds = totalShotsPerSecond * 8 * hitChance;
            double masterTacticianCritBonus = (1 - (Math.Pow(1 - 0.1, masterTacticianShotsIn8Seconds))) * masterTacticianProcChance;
            calculatedStats.critFromMasterTactician = masterTacticianCritBonus;
            //Debug.WriteLine("masterTacticianCritBonus = " + masterTacticianCritBonus);

            // Crit From target debuffs / player buffs TODO: Check this
            calculatedStats.critFromBuffs = statsBuffs.PhysicalCrit;

            // Crit Depression
            double critdepression = (levelDifference > 2) ? 0.03 + (levelDifference * 0.006) : (levelDifference * 5 * 0.04) / 100;
            calculatedStats.critfromDepression = 0 - critdepression;

            calculatedStats.critRateOverall = 0
                + Math.Round(calculatedStats.critBase, 4)
                + Math.Round(calculatedStats.critFromAgi, 4)
                + Math.Round(calculatedStats.critFromRating, 4)
                + Math.Round(calculatedStats.critFromProcRating, 4)
                + Math.Round(calculatedStats.critFromLethalShots, 4)
                + Math.Round(calculatedStats.critFromKillerInstincts, 4)
                + Math.Round(calculatedStats.critFromMasterMarksman, 4)
                + Math.Round(calculatedStats.critFromMasterTactician, 4)
                + Math.Round(calculatedStats.critFromBuffs, 4)
                + Math.Round(calculatedStats.critfromDepression, 4);

            double critHitPercent = calculatedStats.critRateOverall;

            #endregion
            #region August 2009 Bonus Crit Chance

            //Improved Barrage
            double improvedBarrageCritModifier = 0.04 * character.HunterTalents.ImprovedBarrage;

            // Survival instincts
            double survivalInstinctsCritModifier = 0.02 * character.HunterTalents.SurvivalInstincts;

            // Explosive Shot Glyph
            double glyphOfExplosiveShotCritModifier = character.HunterTalents.GlyphOfExplosiveShot ? 0.04 : 0;

            // Sniper Training
            double sniperTrainingCritModifier = character.HunterTalents.SniperTraining * 0.05;

            //Trueshot Aura Glyph
            double trueshotAuraGlyphCritModifier = 0;
            if (character.HunterTalents.GlyphOfTrueshotAura == true)
            {
                if (character.HunterTalents.TrueshotAura > 0)
                {
                    trueshotAuraGlyphCritModifier = 0.1;
                }
            }

            #endregion

            // now we can calculate crit chances.
            // we need to do this and then generate composite crit rates from the rotation
            // so that we can correctly generate effective RAP (since expose weakness depends
            // on how many shots in our rotation crit)

            #region August 2009 Shot Crit Chances

            // crit chance = base_crit + 5%_rift_stalker_bonus + (2% * survivial_instincts)
            //TODO: add rift stalker set bonus
            double steadyShotCritChance = critHitPercent + survivalInstinctsCritModifier;
            calculatedStats.steadyShot.critChance = steadyShotCritChance;

            // crit = base_crit + trueshot_aura_glyph + improved_barrage
            double aimedShotCrit = critHitPercent + trueshotAuraGlyphCritModifier + improvedBarrageCritModifier;
            calculatedStats.aimedShot.critChance = aimedShotCrit;

            // crit = base_crit + glyph_of_es + survival_instincts
            double explosiveShotCrit = critHitPercent + glyphOfExplosiveShotCritModifier + survivalInstinctsCritModifier;
            calculatedStats.explosiveShot.critChance = explosiveShotCrit;

            calculatedStats.chimeraShot.critChance = critHitPercent;

            double arcaneShotCrit = critHitPercent + survivalInstinctsCritModifier;
            calculatedStats.arcaneShot.critChance = arcaneShotCrit;

            double multiShotCrit = critHitPercent + improvedBarrageCritModifier;
            calculatedStats.multiShot.critChance = multiShotCrit;

            double killShotCrit = critHitPercent + sniperTrainingCritModifier;
            calculatedStats.killShot.critChance = killShotCrit;

            calculatedStats.silencingShot.critChance = critHitPercent;

            calculatedStats.priorityRotation.calculateCrits();
            
            #endregion
            #region August 2009 Ranged Attack Power

            calculatedStats.apFromBase = 0 + HunterRatings.CHAR_LEVEL * 2;
            calculatedStats.apFromAgil = 0 + (calculatedStats.BasicStats.Agility) - 10;
            calculatedStats.apFromCarefulAim = 0 + (character.HunterTalents.CarefulAim / 3) * (calculatedStats.BasicStats.Intellect);
            calculatedStats.apFromHunterVsWild = Math.Floor((character.HunterTalents.HunterVsWild * 0.1) * (calculatedStats.BasicStats.Stamina));
            calculatedStats.apFromGear = 0 + calculatedStats.BasicStats.AttackPower;

            calculatedStats.apFromBloodFury = 0;
            if (character.Race == CharacterRace.Orc)
            {
                calculatedStats.apFromBloodFury = (4 * HunterRatings.CHAR_LEVEL) + 2;
                calculatedStats.apFromBloodFury *= CalcUptime(15, 120, options.duration);
            }

            calculatedStats.apFromAspectOfTheHawk = 300;
            calculatedStats.apFromAspectMastery = (character.HunterTalents.AspectMastery * 0.3 * calculatedStats.apFromAspectOfTheHawk);

            calculatedStats.apFromFuriousHowl = 0;
            if (options.PetFamily == PetFamily.Wolf)
            {
                calculatedStats.apFromFuriousHowl = 320 * CalcUptime(20, 40, options.duration);
            }

            // Expose Weakness
            double exposeWeaknessShotsPerSecond = crittingShotsPerSecond;
            double exposeWeaknessCritChance = calculatedStats.priorityRotation.critsCompositeSum;
            double exposeWeaknessAgility = calculatedStats.BasicStats.Agility * 0.25;
            double exposeWeaknessUptime = 1 - Math.Pow(1 - ((character.HunterTalents.ExposeWeakness / 3) * exposeWeaknessCritChance), 7 * crittingShotsPerSecond);
            calculatedStats.apFromExposeWeakness = exposeWeaknessUptime * exposeWeaknessAgility;

            calculatedStats.apFromCallOfTheWild = options.petCallOfTheWild * (CalcUptime(20, 300, options.duration) * 0.1);

            calculatedStats.apFromTrueshotAura = (0.1 * character.HunterTalents.TrueshotAura);
            if (character.HunterTalents.TrueshotAura == 0)
            {
                calculatedStats.apFromTrueshotAura = (calculatedStats.BasicStats.BonusAttackPowerMultiplier + calculatedStats.BasicStats.BonusRangedAttackPowerMultiplier) - 1;
            }

            calculatedStats.apFromHuntersMark = HunterRatings.HUNTERS_MARK;
            calculatedStats.apFromHuntersMark += HunterRatings.HUNTERS_MARK * 0.1 * character.HunterTalents.ImprovedHuntersMark;
            if (character.HunterTalents.GlyphOfHuntersMark == true)
            {
                calculatedStats.apFromHuntersMark += 0.2 * HunterRatings.HUNTERS_MARK;
            }

            calculatedStats.apFromProc = 0; // mine is 322.99
            // TODO: proc AP effects!

            // additive AP bonuses
            calculatedStats.apTotal = 0
                + calculatedStats.apFromBase
                + calculatedStats.apFromAgil
                + calculatedStats.apFromCarefulAim
                + calculatedStats.apFromHunterVsWild
                + calculatedStats.apFromGear // includes buffs
                + calculatedStats.apFromBloodFury
                + calculatedStats.apFromAspectOfTheHawk
                + calculatedStats.apFromAspectMastery
                + calculatedStats.apFromFuriousHowl
                + calculatedStats.apFromExposeWeakness
                + calculatedStats.apFromProc
                + calculatedStats.apFromHuntersMark;

            // multiplicitive AP bonuses
            calculatedStats.apTotal *= 1
                * (1 + calculatedStats.apFromCallOfTheWild) 
                * (1 + calculatedStats.apFromTrueshotAura);

            double RAP = calculatedStats.apTotal;

            #endregion
            #region August 2009 Damage Adjustments

            //Damage Reduction
            calculatedStats.damageReductionFromArmor = StatConversion.GetArmorDamageReduction(
                    character.Level,
                    options.TargetArmor,
                    calculatedStats.BasicStats.ArmorPenetration,
                    0f,
                    calculatedStats.BasicStats.ArmorPenetrationRating
                  );
            double armorReductionDamageAdjust = 1 - calculatedStats.damageReductionFromArmor;

            //Partial Resists
            double averageResist = (options.TargetLevel - 80) * 0.02;
            double resist10 = 5 * averageResist;
            double resist20 = 2.5 * averageResist;
            double partialResistDamageAdjust = 1 - (resist10 * 0.1 + resist20 * 0.1);

            //Beastial Wrath
            double beastialWrathCooldown = character.HunterTalents.GlyphOfBestialWrath ? 100 : 120;

            beastialWrathCooldown *= 1 - 0.1 * character.HunterTalents.Longevity;

            double beastialWrathUptime = CalcUptime(18, beastialWrathCooldown, options.duration);


            //TODO: calculate this properly
            double ferociousInspirationUptime = 1;

            //Focused Fire
            double focusedFireDamageAdjust = 1 + 0.01 * character.HunterTalents.FocusedFire;

            //Beast Within
            double beastWithinDamageAdjust = 1 + (0.01 * character.HunterTalents.TheBeastWithin) * beastialWrathUptime;

            //Sanc. Retribution Aura
            double sancRetributionAuraDamageAdjust = 1 + statsBuffs.BonusDamageMultiplier;

            //Black Arrow Damage Multiplier
            double blackArrowUptime = 0;
            if (calculatedStats.priorityRotation.containsShot(Shots.BlackArrow))
            {
                blackArrowUptime = CalcUptime(calculatedStats.blackArrow.duration, calculatedStats.blackArrow.freq, options.duration);
            }
            double blackArrowAuraDamageAdjust = 1 + (0.06 * blackArrowUptime);
            double blackArrowSelfDamageAdjust = 1 + (RAP / 225000);

            //Noxious Stings
            double noxiousStingsSerpentUptime = 0;
            if (calculatedStats.serpentSting.freq > 0) noxiousStingsSerpentUptime = calculatedStats.serpentSting.duration / calculatedStats.serpentSting.freq;
            if (calculatedStats.priorityRotation.chimeraRefreshesSerpent) noxiousStingsSerpentUptime = 1;
            double noxiousStingsDamageAdjust = 1 + (0.01 * character.HunterTalents.NoxiousStings * noxiousStingsSerpentUptime);
            double noxiousStingsSerpentDamageAdjust = 1 + (0.01 * character.HunterTalents.NoxiousStings);

            //Ferocious Inspiration
            double ferociousInspirationDamageAdjust = 1 + (0.01 * character.HunterTalents.FerociousInspiration) * ferociousInspirationUptime;
            double ferociousInspirationArcaneDamageAdjust = 1 + (0.03 * character.HunterTalents.FerociousInspiration);

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
            double improvedSSArcaneShotDamageAdjust = 1;

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
                                            * blackArrowAuraDamageAdjust
                                            * noxiousStingsDamageAdjust
                                            * ferociousInspirationDamageAdjust
                                            * improvedTrackingDamageAdjust
                                            * rangedWeaponSpecializationDamageAdjust
                                            * markedForDeathDamageAdjust;

            double talentDamageStingAdjust = focusedFireDamageAdjust
                                            * beastWithinDamageAdjust
                                            * sancRetributionAuraDamageAdjust
                                            * blackArrowAuraDamageAdjust
                                            * noxiousStingsDamageAdjust
                                            * ferociousInspirationDamageAdjust;

            #endregion
            #region August 2009 Bonus Crit Damage

            //MortalShots
            double mortalShotsCritDamage = 0.06 * character.HunterTalents.MortalShots;

            //CritDamageMetaGems
            double metaGemCritDamage = 1 + (statsBaseGear.BonusCritMultiplier * 2);

            //Marked For Death
            double markedForDeathCritDamage = 0.02 * character.HunterTalents.MarkedForDeath;

            #endregion
            #region August 2009 Mana Adjustments

            double efficiencyManaAdjust = 1 - (character.HunterTalents.Efficiency * 0.03);

            // TODO: this should use the 'composite' crit chance, based on out actual rotation,
            // not the base crit chance (since some shots in our rotation don't crit)
            double thrillOfTheHuntManaAdjust = 1 - (critHitPercent * 0.4 * (character.HunterTalents.ThrillOfTheHunt / 3));

            double masterMarksmanManaAdjust = 1 - (character.HunterTalents.MasterMarksman * 0.05);

            // TODO: this should only activate if we have serpent or scorpid in the rotation
            double glyphOfArcaneShotManaAdjust = character.HunterTalents.GlyphOfArcaneShot ? 0.8 : 1;

            double ISSAimedShotManaAdjust = 1; // TODO: calculate this!
            double ISSArcaneShotManaAdjust = 1; // TODO: calculate this!
            double ISSChimeraShotManaAdjust = 1; // TODO: calculate this!

            double resourcefulnessManaAdjust = 1 - (character.HunterTalents.Resourcefulness * 0.2);

            Stats statsRace = BaseStats.GetBaseStats(80, CharacterClass.Hunter, character.Race);
            float baseMana = statsRace.Mana;

            #endregion

            // shot calcs
            // for all special shots, we populate a ShotData object

            #region August 2009 AutoShot

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

            double autoShotDamageAdjust = talentDamageAdjust * targetPhysicalDebuffsDamageAdjust * armorReductionDamageAdjust;
            double autoShotCritAdjust = 1 * metaGemCritDamage;

            double autoShotDamageReal = CalcEffectiveDamage(
                                           autoShotDamage,
                                           hitChance,
                                           critHitPercent,
                                           autoShotCritAdjust,
                                           autoShotDamageAdjust
                                         );

            calculatedStats.AutoshotDPS = autoShotDamageReal / autoShotSpeed;

            //Debug.WriteLine("rangedWeaponDamage = " + rangedWeaponDamage);
            //Debug.WriteLine("rangedAmmoDamage = " + rangedAmmoDamage);
            //Debug.WriteLine("statsBaseGear.WeaponDamage = " + statsBaseGear.WeaponDamage);
            //Debug.WriteLine("damageFromRAP = " + damageFromRAP);
            //Debug.WriteLine("autoShotDamage = " + autoShotDamage);
            //Debug.WriteLine("autoShotDamageNormalized = " + autoShotDamageNormalized);
            //Debug.WriteLine("autoShotDPS : " + calculatedStats.BaseAutoshotDPS);
            //Debug.WriteLine("autoShotCritDamage = " + autoShotCritDamage);

            #endregion
            #region August 2009 Wild Quiver

            calculatedStats.WildQuiverDPS = 0;
            //character.HunterTalents.WildQuiver = 2; // for isolation testing
            if (character.HunterTalents.WildQuiver > 0)
            {
                double wildQuiverProcChance = character.HunterTalents.WildQuiver * 0.04;
                double wildQuiverProcFrequency = (autoShotSpeed / wildQuiverProcChance);
                double wildQuiverDamageNormal = 0.8 * (rangedWeaponDamage + statsBaseGear.WeaponDamage + damageFromRAP);
                double wildQuiverDamageAdjust = talentDamageAdjust * partialResistDamageAdjust; // TODO: add nature_debuffs

                double wildQuiverDamageReal = CalcEffectiveDamage(
                                                wildQuiverDamageNormal,
                                                hitChance,
                                                critHitPercent,
                                                1,
                                                wildQuiverDamageAdjust
                                              );

                calculatedStats.WildQuiverDPS = wildQuiverDamageReal / wildQuiverProcFrequency;

                //Debug.WriteLine("wildQuiverProcFrequency : " + wildQuiverProcFrequency);
                //Debug.WriteLine("wildQuiverDamageReal : " + wildQuiverDamageReal);
                //Debug.WriteLine("wildQuiverDPS : " + calculatedStats.WildQuiverDPS);
            }

            #endregion
            #region August 2009 Steady Shot

            // base = shot_base + gear_weapon_damage + normalized_ammo_dps + (RAP * 0.1)
            //        + (rangedWeaponDamage / ranged_weapon_speed * 2.8)
            double steadyShotDamageNormal = 252
                        + statsBaseGear.WeaponDamage
                        + rangedAmmoDamageNormalized
                        + (RAP * 0.1)
                        + (rangedWeaponDamage / rangedWeaponSpeed * 2.8);


            // mana per shot
            double steadyShotManaCost = (baseMana * 0.05)
                                        * efficiencyManaAdjust * thrillOfTheHuntManaAdjust * masterMarksmanManaAdjust;

            // adjust = talent_adjust * gronnstalker_bonus * glyph_of_steadyshot
            //          * sniper_training * physcial_debuffs
            // TODO: Gronnstalker set bonus
            double steadyShotDamageAdjust = talentDamageAdjust
                                            * targetPhysicalDebuffsDamageAdjust
                                            * sniperTrainingDamageAdjust
                                            * glyphOfSteadyShotDamageAdjust
                                            * armorReductionDamageAdjust;

            double steadyShotCritAdjust = (1 + mortalShotsCritDamage + markedForDeathCritDamage) * metaGemCritDamage;

            double steadyShotDamageReal = CalcEffectiveDamage(
                                            steadyShotDamageNormal,
                                            hitChance,
                                            steadyShotCritChance,
                                            steadyShotCritAdjust,
                                            steadyShotDamageAdjust
                                          );

            calculatedStats.steadyShot.damage = steadyShotDamageReal;
            calculatedStats.steadyShot.mana = steadyShotManaCost;
            //calculatedStats.steadyShot.Dump("Steady Shot");

            #endregion
            #region August 2009 Serpent Sting

            // base_damage = 1210 + (0.2 * RAP)
            double serpentStingDamageBase = 1210 + (RAP * 0.2);

            // damage_adjust = (sting_talent_adjusts ~ noxious stings) * improved_stings * improved_tracking
            //                  + partial_resists * tier-8_2-piece_bonus * target_nature_debuffs * 100%_noxious_stings
            // TODO: nature debuffs & t8 bonus
            double serpentStingDamageAdjust = focusedFireDamageAdjust
                                                * beastWithinDamageAdjust
                                                * sancRetributionAuraDamageAdjust
                                                * blackArrowAuraDamageAdjust
                                                * ferociousInspirationDamageAdjust
                                                * noxiousStingsSerpentDamageAdjust
                                                * improvedStingsDamageAdjust
                                                * improvedTrackingDamageAdjust
                                                * partialResistDamageAdjust;

            double serpentStingTicks = calculatedStats.serpentSting.duration / 3;
            double serpentStingDamagePerTick = Math.Round(serpentStingDamageBase * serpentStingDamageAdjust / 5, 1);
            double serpentStingDamageReal = serpentStingDamagePerTick * serpentStingTicks;

            double serpentStingManaCost = (baseMana * 0.09) * efficiencyManaAdjust;

            calculatedStats.serpentSting.type = Shots.SerpentSting;
            calculatedStats.serpentSting.damage = serpentStingDamageReal;
            calculatedStats.serpentSting.mana = serpentStingManaCost;
            //calculatedStats.serpentSting.Dump("Serpent Sting");

            #endregion
            #region August 2009 Aimed Shot

            // base_damage = normalized_shot + 408
            double aimedShotDamageNormal = autoShotDamageNormalized + 408;

            // crit_damage = 1 + mortal_shots + gem_crit + marked_for_death
            double aimedShotCritAdjust = (1 + mortalShotsCritDamage + markedForDeathCritDamage) * metaGemCritDamage;

            // damage_adjust = talent_adjust * barrage_adjust * target_debuff_adjust * sniper_training_adjust * improved_ss_adjust
            double aimedShotDamageAdjust = talentDamageAdjust * barrageDamageAdjust * targetPhysicalDebuffsDamageAdjust
                                            * sniperTrainingDamageAdjust * improvedSSAimedShotDamageAdjust
                                            * armorReductionDamageAdjust;

            double aimedShotDamageReal = CalcEffectiveDamage(
                                            aimedShotDamageNormal,
                                            hitChance,
                                            aimedShotCrit,
                                            aimedShotCritAdjust,
                                            aimedShotDamageAdjust
                                          );

            //Debug.WriteLine("aimedShotDamageNormal = " + aimedShotDamageNormal);
            //Debug.WriteLine("aimedShotCritAdjust = " + aimedShotCritAdjust);
            //Debug.WriteLine("aimedShotDamageAdjust = " + aimedShotDamageAdjust);

            double aimedShotManaCost = (baseMana * 0.08) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust 
                                        * masterMarksmanManaAdjust * ISSAimedShotManaAdjust;

            calculatedStats.aimedShot.damage = aimedShotDamageReal;
            calculatedStats.aimedShot.mana = aimedShotManaCost;
            //calculatedStats.aimedShot.Dump("Aimed Shot");

            #endregion
            #region August 2009 Explosive Shot

            // base_damage = 425 + 14% of RAP
            double explosiveShotDamageNormal = 425 + (RAP * 0.14);

            // crit_damage = 1 + mortal_shots + gem-crit
            double explosiveShotCritAdjust = (1 + mortalShotsCritDamage) * metaGemCritDamage;

            // damage_adjust = talent_adjust * tnt * fire_debuffs * sinper_training * partial_resist
            // TODO: missing fire debuffs
            double explosiveShotDamageAdjust = talentDamageAdjust * TNTDamageAdjust * sniperTrainingDamageAdjust * partialResistDamageAdjust;

            double explosiveShotDamageReal = CalcEffectiveDamage(
                                                explosiveShotDamageNormal,
                                                hitChance,
                                                explosiveShotCrit,
                                                explosiveShotCritAdjust,
                                                explosiveShotDamageAdjust
                                              );

            double explosiveShotDamagePerShot = explosiveShotDamageReal * 3;

            double explosiveShotManaCost = (baseMana * 0.07) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust;

            calculatedStats.explosiveShot.damage = explosiveShotDamagePerShot;
            calculatedStats.explosiveShot.mana = explosiveShotManaCost;            
            //calculatedStats.explosiveShot.Dump("Explosive Shot");

            #endregion
            #region August 2009 Chimera Shot

            // base_damage = normalized_autoshot * 125%
            double chimeraShotDamageNormal = autoShotDamageNormalized * 1.25;

            // crit for 'specials'
            double chimeraShotCritAdjust = (1 + mortalShotsCritDamage + markedForDeathCritDamage) * metaGemCritDamage;

            // damage_adjust = talent_adjust * nature_debuffs * ISS_cs_bonus * partial_resist
            // TODO: nature_debuffs
            double chimeraShotDamageAdjust = talentDamageAdjust * improvedSSChimeraShotDamageAdjust * partialResistDamageAdjust;

            double chimeraShotDamageReal = CalcEffectiveDamage(
                                                chimeraShotDamageNormal,
                                                hitChance,
                                                critHitPercent,
                                                chimeraShotCritAdjust,
                                                chimeraShotDamageAdjust
                                           );


            // calculate damage from serpent sting
            double chimeraShotSerpentDamage = serpentStingDamageReal * 0.4;
            double chimeraShotSerpentCritAdjust = (1 + mortalShotsCritDamage) * metaGemCritDamage;
            double chimeraShotSerpentDamageAdjust = talentDamageAdjust * 1; // TODO: add nature_debuffs here!

            double chimeraShotSerpentDamageReal = CalcEffectiveDamage(
                                                    chimeraShotSerpentDamage,
                                                    hitChance,
                                                    critHitPercent,
                                                    chimeraShotSerpentCritAdjust,
                                                    chimeraShotSerpentDamageAdjust
                                                 );

            double chimeraShotDamageTotal = chimeraShotDamageReal + chimeraShotSerpentDamageReal;

            double chimeraShotManaCost = (baseMana * 0.12) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust
                                            * masterMarksmanManaAdjust * ISSChimeraShotManaAdjust;

            calculatedStats.chimeraShot.damage = chimeraShotDamageTotal;
            calculatedStats.chimeraShot.mana = chimeraShotManaCost;
            //calculatedStats.chimeraShot.Dump("Chimera Shot");

            #endregion
            #region August 2009 Arcane Shot

            // base_damage = 492 + weapon_damage_gear + (RAP * 15%)
            double arcaneShotDamageNormal = 492 + statsBaseGear.WeaponDamage + (RAP * 0.15);

            double arcaneShotCritAdjust = (1 + mortalShotsCritDamage + markedForDeathCritDamage) * metaGemCritDamage;
            double arcaneShotDamageAdjust = talentDamageAdjust * partialResistDamageAdjust * improvedArcaneShotDamageAdjust
                                            * ferociousInspirationArcaneDamageAdjust * improvedSSArcaneShotDamageAdjust; // missing arcane_debuffs!

            double arcaneShotDamageReal = CalcEffectiveDamage(
                                            arcaneShotDamageNormal,
                                            hitChance,
                                            arcaneShotCrit,
                                            arcaneShotCritAdjust,
                                            arcaneShotDamageAdjust
                                          );

            double arcaneShotManaCost = (baseMana * 0.05) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust
                                        * ISSArcaneShotManaAdjust * glyphOfArcaneShotManaAdjust;

            calculatedStats.arcaneShot.damage = arcaneShotDamageReal;
            calculatedStats.arcaneShot.mana = arcaneShotManaCost;
            //calculatedStats.arcaneShot.Dump("Arcane Shot");

            #endregion
            #region August 2009 Multi Shot

            double multiShotDamageNormal = rangedWeaponDamage + statsBaseGear.WeaponDamage + rangedAmmoDamage
                                        + calculatedStats.BasicStats.ScopeDamage + 408 + (RAP * 0.2);
            double multiShotDamageAdjust = talentDamageAdjust * barrageDamageAdjust * targetPhysicalDebuffsDamageAdjust
                                            * armorReductionDamageAdjust; // missing: pvp gloves bonus

            double multiShotDamageReal = CalcEffectiveDamage(
                                            multiShotDamageNormal,
                                            hitChance,
                                            multiShotCrit,
                                            1,
                                            multiShotDamageAdjust
                                         );

            double multiShotManaCost = (baseMana * 0.09) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust;

            calculatedStats.multiShot.damage = multiShotDamageReal;
            calculatedStats.multiShot.mana = multiShotManaCost;
            //calculatedStats.multiShot.Dump("Multi Shot");

            #endregion
            #region August 2009 Black Arrow

            double blackArrowDamageNormal = 2765 + (RAP * 0.1);

            // this is a long list...
            // TODO: add shadow_debuffs
            double blackArrowDamageAdjust = partialResistDamageAdjust * focusedFireDamageAdjust * beastWithinDamageAdjust
                                          * sancRetributionAuraDamageAdjust * noxiousStingsDamageAdjust
                                          * ferociousInspirationDamageAdjust * improvedTrackingDamageAdjust
                                          * rangedWeaponSpecializationDamageAdjust * markedForDeathDamageAdjust
                                          * targetPhysicalDebuffsDamageAdjust
                                          * (sniperTrainingDamageAdjust + trapMasteryDamageAdjust + TNTDamageAdjust - 2)
                                          * blackArrowSelfDamageAdjust;

            double blackArrowDamage = blackArrowDamageNormal * blackArrowDamageAdjust;

            double blackArrowManaCost = (baseMana * 0.06) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust * resourcefulnessManaAdjust;

            calculatedStats.blackArrow.damage = blackArrowDamage;
            calculatedStats.blackArrow.mana = blackArrowManaCost;
            //calculatedStats.blackArrow.Dump("Black Arrow");

            #endregion
            #region August 2009 Kill Shot

            double killShotDamageNormal = (autoShotDamage * 2) + statsBaseGear.WeaponDamage + 650 + (RAP * 0.4);
            double killShotCritAdjust = (1 + mortalShotsCritDamage + markedForDeathCritDamage) * metaGemCritDamage;
            double killShotDamageAdjust = talentDamageAdjust * targetPhysicalDebuffsDamageAdjust * armorReductionDamageAdjust;

            double killShotDamageReal = CalcEffectiveDamage(
                                            killShotDamageNormal,
                                            hitChance,
                                            killShotCrit,
                                            killShotCritAdjust,
                                            killShotDamageAdjust
                                        );

            double killShotManaCost = (baseMana * 0.07) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust;

            calculatedStats.killShot.damage = killShotDamageReal;
            calculatedStats.killShot.mana = killShotManaCost;
            //calculatedStats.killShot.Dump("Kill Shot");

            #endregion
            #region August 2009 Silencing Shot

            double silencingShotDamageNormal = (rangedWeaponDamage + rangedAmmoDamage + damageFromRAPNormalized) * 0.5 ;
            double silencingShotDamageAdjust = talentDamageAdjust * targetPhysicalDebuffsDamageAdjust * armorReductionDamageAdjust;
            double silencingShotCritAdjust = 1 * metaGemCritDamage;

            double silencingShotDamageReal = CalcEffectiveDamage(
                                                silencingShotDamageNormal,
                                                hitChance,
                                                critHitPercent,
                                                silencingShotCritAdjust,
                                                silencingShotDamageAdjust
                                             );

            double silencingShotManaCost = (baseMana * 0.06) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust;

            calculatedStats.silencingShot.damage = silencingShotDamageReal;
            calculatedStats.silencingShot.mana = silencingShotManaCost;
            //calculatedStats.silencingShot.Dump("Silencing Shot");

            #endregion
            #region August 2009 Rapid Fire

            calculatedStats.rapidFire.damage = 0;
            calculatedStats.rapidFire.mana = (baseMana * 0.03);

            #endregion


            #region Quick Shots

            //TODO: this is in the wrong place and is not being used.
            // move this somewhere sensible (after wild quiver?) and plug the values in

            // Model Quickshots

            double quickShotsUpTime = 0;
            double quickShotHaste = 1.0;
            if (character.HunterTalents.ImprovedAspectOfTheHawk > 0)
            {
                quickShotHaste = .03 * character.HunterTalents.ImprovedAspectOfTheHawk;
                double quickAutoShotsPerSecond = (1.0f + quickShotHaste) / calculatedStats.BaseAttackSpeed;
                //Quick Shot Uptime From Cheeky's DPS Spreadsheet with special notation - "By Norwest"
                double shotsInProc = (Math.Floor((12f - calculatedStats.BaseAttackSpeed) * baseAutoShotsPerSecond) + 1) * calculatedStats.BasicStats.PhysicalHit;
                double shotsInReProc = Math.Floor(12f * quickAutoShotsPerSecond) * calculatedStats.BasicStats.PhysicalHit;
                double reprocChanceInitial = 1 - Math.Pow(.9, shotsInProc);
                double reprocChanceSub = 1 - Math.Pow(.9, shotsInReProc);
                double AvgShotBeforeFirstReProc = ((1 - Math.Pow(0.9, (shotsInProc + 1))) / Math.Pow(.1, 2) - (shotsInProc + 1) * Math.Pow(0.9, shotsInProc) / .1) / reprocChanceInitial * 0.1;
                double AvgShotBeforeNthReProc = ((1 - Math.Pow(0.9, (shotsInReProc + 1))) / Math.Pow(.1, 2) - (shotsInReProc + 1) * Math.Pow(0.9, shotsInReProc) / .1) / reprocChanceSub * 0.1;
                double avgQuickShotChain = shotsInProc * (1 - reprocChanceInitial) + reprocChanceInitial * (1 - reprocChanceSub) * (AvgShotBeforeNthReProc * reprocChanceSub / Math.Pow((1 - reprocChanceSub), 2) + (AvgShotBeforeFirstReProc + shotsInReProc) / (1 - reprocChanceSub));
                quickShotsUpTime = avgQuickShotChain / (avgQuickShotChain + 10);
            }
            #endregion
            #region Pet

            PetCalculations pet = new PetCalculations(character, calculatedStats, options, statsBuffs, PetFamily.Bat, statsBaseGear);

            #endregion
            #region August 2009 Mana Regen

            calculatedStats.manaRegenGearBuffs = 0; // mp5 on gear

            calculatedStats.manaRegenViper = 0; //TODO

            // Roar of Recovery
            calculatedStats.manaRegenRoarOfRecovery = 0; //TODO

            // Rapid Recuperation
            calculatedStats.manaRegenRapidRecuperation = 0;
            if (calculatedStats.rapidFire.freq > 0)
            {
                double rapidRecuperationManaGain = 0.02 * character.HunterTalents.RapidRecuperation * calculatedStats.BasicStats.Mana * 5;
                calculatedStats.manaRegenRapidRecuperation = rapidRecuperationManaGain / calculatedStats.rapidFire.freq;
            }

            // Chimera shot refreshing Viper
            calculatedStats.manaRegenChimeraViperProc = 0;
            if (calculatedStats.priorityRotation.chimeraRefreshesViper)
            {
                if (calculatedStats.chimeraShot.freq > 0)
                {
                    calculatedStats.manaRegenChimeraViperProc = 0.6 * 3092 / calculatedStats.chimeraShot.freq;
                }
            }

            // Invigoration
            calculatedStats.manaRegenInvigoration = 0; // TODO

            // Hunting Party

            double huntingPartyProc = (float)(character.HunterTalents.HuntingParty / 3.0);

            double huntingPartyArcaneFreq = calculatedStats.arcaneShot.freq;
            double huntingPartyArcaneCrit = calculatedStats.arcaneShot.critChance;
            double huntingPartyArcaneUptime = huntingPartyArcaneFreq > 0 ? 1 - Math.Pow(1-huntingPartyArcaneCrit * huntingPartyProc, 15/huntingPartyArcaneFreq) : 0;

            double huntingPartyExplosiveFreq = calculatedStats.explosiveShot.freq; // spreadsheet divides by 3, but doesn't use that value?
            double huntingPartyExplosiveCrit = calculatedStats.explosiveShot.critChance;
            double huntingPartyExplosiveUptime = huntingPartyExplosiveFreq > 0 ? 1 - Math.Pow(1 - huntingPartyExplosiveCrit * huntingPartyProc, 15 / huntingPartyExplosiveFreq) : 0;

            double huntingPartySteadyFreq = calculatedStats.steadyShot.freq;
            double huntingPartySteadyCrit = calculatedStats.steadyShot.critChance;
            double huntingPartySteadyUptime = huntingPartySteadyFreq > 0 ? 1 - Math.Pow(1 - huntingPartySteadyCrit * huntingPartyProc, 15 / huntingPartySteadyFreq) : 0;

            double huntingPartyCumulativeUptime = huntingPartyArcaneUptime + ((1 - huntingPartyArcaneUptime) * huntingPartyExplosiveUptime);
            double huntingPartyUptime = huntingPartyCumulativeUptime + ((1 - huntingPartyCumulativeUptime) * huntingPartySteadyUptime);

            calculatedStats.manaRegenHuntingParty = 0.0025 * calculatedStats.BasicStats.Mana * huntingPartyUptime;

            // We actually compare the replenishment buff value with the hunting party
            // value and pick the largest. the spreadsheet just ignores hunting party
            // if aay replenishment is up?
            double manaRegenReplenishment = statsBuffs.ManaRestoreFromMaxManaPerSecond * calculatedStats.BasicStats.Mana;
            if (manaRegenReplenishment > calculatedStats.manaRegenHuntingParty)
            {
                calculatedStats.manaRegenHuntingParty = manaRegenReplenishment;
            }

            // Target Debuffs
            calculatedStats.manaRegenTargetDebuffs = 0; // TODO!
 
            // Total
            calculatedStats.manaRegenTotal =
                calculatedStats.manaRegenGearBuffs +
                calculatedStats.manaRegenViper +
                calculatedStats.manaRegenRoarOfRecovery +
                calculatedStats.manaRegenRapidRecuperation +
                calculatedStats.manaRegenChimeraViperProc +
                calculatedStats.manaRegenInvigoration +
                calculatedStats.manaRegenHuntingParty +
                calculatedStats.manaRegenTargetDebuffs;


            #endregion
            #region August 2009 Shot Rotation

            calculatedStats.priorityRotation.calculateRotationDPS(character);
            calculatedStats.CustomDPS = calculatedStats.priorityRotation.DPS;

            //Debug.WriteLine("Rotation DPS = " + calculatedStats.priorityRotation.DPS);
            //Debug.WriteLine("Rotation MPS = " + calculatedStats.priorityRotation.MPS);

            #endregion

            calculatedStats.PetDpsPoints = pet.getDPS();
            calculatedStats.HunterDpsPoints = (float)(calculatedStats.AutoshotDPS + calculatedStats.WildQuiverDPS + calculatedStats.CustomDPS);
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
			statsTotal.BonusIntellectMultiplier = ((1 + statsRace.BonusIntellectMultiplier) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier) * (1 + statsTalents.BonusIntellectMultiplier)) - 1;
            statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier) * (1 + statsTalents.BonusStaminaMultiplier)) - 1;
			statsTotal.BonusSpellPowerMultiplier = ((1 + statsRace.BonusSpellPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpellPowerMultiplier)) - 1;
			statsTotal.BonusArcaneDamageMultiplier = ((1 + statsRace.BonusArcaneDamageMultiplier) * (1 + statsGearEnchantsBuffs.BonusArcaneDamageMultiplier)) - 1;
			statsTotal.BonusPetDamageMultiplier = ((1 + statsGearEnchantsBuffs.BonusPetDamageMultiplier) * (1 + statsRace.BonusPetDamageMultiplier)) - 1;
			statsTotal.BonusSteadyShotDamageMultiplier = ((1 + statsGearEnchantsBuffs.BonusSteadyShotDamageMultiplier) * (1 + statsRace.BonusSteadyShotDamageMultiplier) * (1 + statsTalents.BonusSteadyShotDamageMultiplier)) - 1;
			statsTotal.BonusSpiritMultiplier = ((1 + statsRace.BonusSpiritMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier) * (statsTalents.BonusSpiritMultiplier)) - 1;
			
            // Stamina
            double stam_from_gear = statsGearEnchantsBuffs.Stamina * (1 + statsTotal.BonusStaminaMultiplier);
            double stam_from_race = statsRace.Stamina * (1 + statsTotal.BonusStaminaMultiplier);
            statsTotal.Stamina = (float)(Math.Round(stam_from_gear) + Math.Floor(stam_from_race));

            // Agility
            double agi_race_talent_adjusted = Math.Floor(statsRace.Agility * (1 + character.HunterTalents.LightningReflexes * 0.03));
            double agi_part_1 = Math.Round(statsBaseGear.Agility * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier) * (1 + statsTalents.BonusAgilityMultiplier));
            double agi_part_2 = Math.Round(statsRace.Agility * character.HunterTalents.HuntingParty * 0.01);
            double agi_part_3 = Math.Round(agi_race_talent_adjusted * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier));
            statsTotal.Agility = (float)(agi_part_1 + agi_part_2 + agi_part_3);


            statsTotal.Intellect = (statsRace.Intellect + statsGearEnchantsBuffs.Intellect) * (1 + statsTotal.BonusIntellectMultiplier);
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

            statsTotal.CritRating = (float)Math.Floor(
                                               (double)statsRace.CritRating +                       
                                               (double)statsBaseGear.CritRating +                   // gear crit
                                               (double)statsBuffs.CritRating +                      // master of anatomy!
                                               (double)statsGearEnchantsBuffs.RangedCritRating +    // crit from scopes
                                               (double)statsRace.LotPCritRating +                   // leader of the pack
                                               (double)statsGearEnchantsBuffs.LotPCritRating        // leader of the pack
                                    );

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
                talents.BonusAgilityMultiplier = addCumulativePercentage(talents.BonusAgilityMultiplier, 0.02f * talentTree.CombatExperience);
                talents.BonusIntellectMultiplier = addCumulativePercentage(talents.BonusIntellectMultiplier, 0.02f * talentTree.CombatExperience);
			}

            // Survival Talents
            {
				//Killer Instincts
				talents.PhysicalCrit += (talentTree.KillerInstinct * 0.01f);

				//Lighting Reflexes
				talents.BonusAgilityMultiplier = addCumulativePercentage(talents.BonusAgilityMultiplier, 0.03f * talentTree.LightningReflexes);

                //Hunting Party
                talents.BonusAgilityMultiplier = addCumulativePercentage(talents.BonusAgilityMultiplier, 0.01f * talentTree.HuntingParty);

                // Survivalist
                talents.BonusStaminaMultiplier = addCumulativePercentage(talents.BonusStaminaMultiplier, 0.02f * talentTree.Survivalist);
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
            if (calculateUptimesLikeSpreadsheet)
            {
                return cooldown > 0 ? duration / cooldown : 0;
            }

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

        private float addCumulativePercentage(float current, double new_chance)
        {
            // helper function for calculating multiplicitive bonuses, such as agi from talents.
            // if we gain 2% from Hunting Party and 15% from Lightning Reflexes, we actually
            // get a final 17.3% bonus, not 17% ( [1.15 * 1.02] - 1 )

            return (float)((current + 1) * (new_chance + 1)) - 1;
        }

        private ShotData getShotByIndex(int index, CharacterCalculationsHunter calculatedStats)
        {
            if (index == 1) return calculatedStats.aimedShot;
            if (index == 2) return calculatedStats.arcaneShot;
            if (index == 3) return calculatedStats.multiShot;
            if (index == 4) return calculatedStats.serpentSting;
            if (index == 5) return calculatedStats.scorpidSting;
            if (index == 6) return calculatedStats.viperSting;
            if (index == 7) return calculatedStats.silencingShot;
            if (index == 8) return calculatedStats.steadyShot;
            if (index == 9) return calculatedStats.killShot;
            if (index == 10) return calculatedStats.explosiveShot;
            if (index == 11) return calculatedStats.blackArrow;
            if (index == 12) return calculatedStats.immolationTrap;
            if (index == 13) return calculatedStats.chimeraShot;
            if (index == 14) return calculatedStats.rapidFire;
            if (index == 15) return calculatedStats.readiness;
            if (index == 16) return calculatedStats.beastialWrath;
            if (index == 17) return calculatedStats.bloodFury;
            if (index == 18) return calculatedStats.berserk;
            return null;
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
