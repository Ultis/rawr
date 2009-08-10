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
				"Pet Stats:Pet White DPS",
				"Pet Stats:Pet Kill Command DPS",
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

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            CharacterCalculationsHunter calculatedStats = new CharacterCalculationsHunter();
            if (character == null)
            {
                return calculatedStats;
            }

            CalculationOptionsHunter options = character.CalculationOptions as CalculationOptionsHunter;

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsRace = BaseStats.GetBaseStats(80, CharacterClass.Hunter, character.Race);

            calculatedStats.BasicStats = GetCharacterStats(character, additionalItem);

            calculatedStats.pet = new PetCalculations(character, calculatedStats, options, statsBuffs, statsBaseGear, this);
            
            //calculatedStats.PetStats = GetPetStats(options, calculatedStats, character, statsBuffs, statsBaseGear);

            if (character.Ranged == null || (character.Ranged.Item.Type != ItemType.Bow && character.Ranged.Item.Type != ItemType.Gun
                                            && character.Ranged.Item.Type != ItemType.Crossbow))
            {
                //skip all the calculations if there is no ranged weapon
                return calculatedStats;
            }

            // NOTE: this model just breaks if you're not level 80.
            // we should be using character.Level everywhere, but also
            // all of the spell levels will be wrong. do we care?

            //Debug.WriteLine("Buffs:");
            //foreach (Buff buff in character.ActiveBuffs)
            //{
            //    Debug.WriteLine(buff);
            //}

            // shot basics
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
            calculatedStats.multiShot.cooldown -= calculatedStats.BasicStats.MultiShotCooldownReduction; // PVP S1 Set Bonus

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
            calculatedStats.priorityRotation.calculateLALProcs(character);
            calculatedStats.priorityRotation.calculateFrequencies();

            #endregion

            // speed
            #region August 2009 Ranged Weapon Stats

            double rangedWeaponDamage = 0;
            double rangedWeaponSpeed = 0;
            double rangedAmmoDPS = 0;

            if (character.Ranged != null)
            {
                rangedWeaponDamage = (float)(character.Ranged.Item.MinDamage + character.Ranged.Item.MaxDamage) / 2f;
                rangedWeaponSpeed = Math.Round(character.Ranged.Item.Speed * 10) / 10;
            }
            if (character.Projectile != null)
            {
                rangedAmmoDPS = (float)(character.Projectile.Item.MaxDamage + character.Projectile.Item.MinDamage) / 2f;
            }

            #endregion
            #region August 2009 Haste Calcs

            // troll berserking
            calculatedStats.hasteFromRacial = 0;
            if (character.Race == CharacterRace.Troll)
            {
                if (calculatedStats.priorityRotation.containsShot(Shots.Berserk))
                {
                    calculatedStats.hasteFromRacial = 10 / 180 * 0.2;
                }
            }

            //default quiver speed
            calculatedStats.hasteFromBase = 15.0;

            // haste from haste rating
            calculatedStats.hasteFromRating = calculatedStats.BasicStats.HasteRating / HunterRatings.HASTE_RATING_PER_PERCENT;

            // serpent swiftness
            calculatedStats.hasteFromTalentsStatic = 4.0 * character.HunterTalents.SerpentsSwiftness;

            // rapid fire
            double rapidFireHaste = character.HunterTalents.GlyphOfRapidFire ? 48.0 : 40.0;
            double rapidFireCooldown = calculatedStats.rapidFire.freq;
            if (!calculatedStats.priorityRotation.containsShot(Shots.RapidFire))  rapidFireHaste = 0;
            calculatedStats.hasteFromProcs = rapidFireHaste * CalcUptime(15, rapidFireCooldown, options);

            // haste buffs
            calculatedStats.hasteFromRangedBuffs = calculatedStats.BasicStats.RangedHaste * 100;

            // total hastes
            double totalStaticHaste = (1 + calculatedStats.hasteFromBase / 100)             // quiver
                                    * (1 + calculatedStats.hasteFromRating / 100)           // gear haste rating
                                    * (1 + calculatedStats.hasteFromTalentsStatic / 100)    // serpent's swiftness
                                    * (1 + calculatedStats.hasteFromRangedBuffs / 100);     // buffs like swift ret / moonkin

            double totalDynamicHaste = (1 + calculatedStats.hasteFromProcs / 100)           // rapid fire
                                     * (1 + calculatedStats.hasteFromRacial / 100)          // troll beserking
                                     * (1 + 0)                                              // TODO: heroism
                                     * (1 + 0);                                             // TODO: Proc haste from gear (trinkets, etc)


            calculatedStats.hasteEffectsTotal = (totalStaticHaste * totalDynamicHaste) - 1;

            // Now we have the haste, we can calculate steady shot cast time
            // And so rebuild other various times
            calculatedStats.steadyShot.cooldown = 2 * (1 / (totalStaticHaste * totalDynamicHaste));
            calculatedStats.priorityRotation.calculateFrequencies();

            #endregion

            // hits
            #region August 2009 Hit Chance

            // hit base
            calculatedStats.hitFromBase = 1.0 - HunterRatings.BASE_MISS_PERCENT;

            // level adjustment
            double levelDifference = options.TargetLevel - HunterRatings.CHAR_LEVEL;
            calculatedStats.hitFromLevelAdjustment = 0 - (levelDifference / 100);

            // gear +hit rating
            calculatedStats.hitFromRating = (calculatedStats.BasicStats.HitRating / HunterRatings.HIT_RATING_PER_PERCENT) / 100;

            // Focused Aim
            calculatedStats.hitFromTalents = (1.0 * character.HunterTalents.FocusedAim) / 100;

            // TODO: Heroic Presence should appear here
            calculatedStats.hitFromBuffs = statsBuffs.SpellHit;

            // No debuffs in spreadsheet that give +hit
            calculatedStats.hitFromTargetDebuffs = 0;

            calculatedStats.hitOverall = calculatedStats.hitFromBase
                                       + calculatedStats.hitFromLevelAdjustment
                                       + calculatedStats.hitFromRating
                                       + calculatedStats.hitFromTalents
                                       + calculatedStats.hitFromBuffs
                                       + calculatedStats.hitFromTargetDebuffs;

            if (calculatedStats.hitOverall >= 1.0) calculatedStats.hitOverall = 1.0;

            double hitChance = calculatedStats.hitOverall;

            #endregion
            #region August 2009 Quick Shots

            double QSBaseFreqnecyIncrease = 0;
            double autoShotSpeed = rangedWeaponSpeed / (totalStaticHaste * totalDynamicHaste);

            if (options.selectedAspect == Aspect.Hawk || options.selectedAspect == Aspect.Dragonhawk)
            {
                if (character.HunterTalents.ImprovedAspectOfTheHawk > 0)
                {
                    double quickShotsProcChance = 0.1;
                    double quickShotsEffect = 0.03 * character.HunterTalents.ImprovedAspectOfTheHawk;
                    if (character.HunterTalents.GlyphOfTheHawk) quickShotsEffect += 0.06;

                    double quickShotsSpeed = autoShotSpeed / (1 + quickShotsEffect);

                    double quickShotsInInitialProc = (autoShotSpeed > 0 ? (12 - autoShotSpeed) / quickShotsSpeed + 1 : 1) * hitChance;
                    double quickShotsInReProc = (quickShotsSpeed > 0 ? 12 / quickShotsSpeed : 1) * hitChance;

                    double quickShotsProcInitial = 1 - Math.Pow(1 - quickShotsProcChance, quickShotsInInitialProc);
                    double quickShotsProcSubsequent = 1 - Math.Pow(1 - quickShotsProcChance, quickShotsInReProc);

                    double quickShotsAvgShotsBeforeInit = 0;
                    if (quickShotsProcChance > 0 && quickShotsProcInitial > 0)
                    {
                        quickShotsAvgShotsBeforeInit = ((1 - Math.Pow(0.9, quickShotsInInitialProc + 1)) / 0.01 - (quickShotsInInitialProc + 1) * Math.Pow(0.9, quickShotsInInitialProc) / 0.1 ) / quickShotsProcInitial * 0.1;
                    }
                        
                    double quickShotsAvgShotsBeforeNext = 0;
                    if (quickShotsProcChance > 0 && quickShotsProcSubsequent > 0)
                    {
                        quickShotsAvgShotsBeforeNext = ((1 - Math.Pow(0.9, quickShotsInReProc + 1)) / 0.01 - (quickShotsInReProc + 1) * Math.Pow(0.9, quickShotsInReProc) / 0.1) / quickShotsProcSubsequent * 0.1;
                    }

                    double quickShotsAverageChainQuick = quickShotsInInitialProc * (1 - quickShotsProcInitial)
                                                       + quickShotsProcInitial * (1 - quickShotsProcSubsequent)
                                                       * (quickShotsAvgShotsBeforeNext * quickShotsProcSubsequent / (Math.Pow(1 - quickShotsProcSubsequent, 2))
                                                       + (quickShotsAvgShotsBeforeInit + quickShotsInReProc) / (1 - quickShotsProcSubsequent));

                    double quickShotsAverageChainSlow = quickShotsProcChance > 0 ? 1 / quickShotsProcChance : 0;

                    // TODO: use rotation test to get this value
                    double quickShotsUptime = quickShotsProcChance > 0 ? quickShotsAverageChainQuick / (quickShotsAverageChainQuick + quickShotsAverageChainSlow) : 0;

                    QSBaseFreqnecyIncrease = autoShotSpeed > 0 ? (1 / quickShotsSpeed - 1 / autoShotSpeed) * quickShotsUptime : 0;
                }
            }



            #endregion
            #region August 2009 Shots Per Second

            double baseAutoShotsPerSecond = autoShotSpeed > 0 ? 1 / autoShotSpeed : 0;
            double autoShotsPerSecond = baseAutoShotsPerSecond + QSBaseFreqnecyIncrease;
            double specialShotsPerSecond = calculatedStats.priorityRotation.specialShotsPerSecond;
            double totalShotsPerSecond = autoShotsPerSecond + specialShotsPerSecond;

            double crittingSpecialsPerSecond = calculatedStats.priorityRotation.critSpecialShotsPerSecond;
            double crittingShotsPerSecond = autoShotsPerSecond + crittingSpecialsPerSecond;

            double shotsPerSecondWithoutHawk = specialShotsPerSecond + baseAutoShotsPerSecond;

            calculatedStats.BaseAttackSpeed = (float)autoShotSpeed;
            calculatedStats.shotsPerSecondCritting = crittingShotsPerSecond;

            //Debug.WriteLine("baseAutoShotsPerSecond = " + baseAutoShotsPerSecond);
            //Debug.WriteLine("autoShotsPerSecond = " + autoShotsPerSecond);
            //Debug.WriteLine("specialShotsPerSecond = " + specialShotsPerSecond);
            //Debug.WriteLine("Total shots per second = " + totalShotsPerSecond);

            #endregion

            // crits
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
            calculatedStats.critFromDepression = 0 - critdepression;

            double critHitPercent = 0
                + calculatedStats.critBase
                + calculatedStats.critFromAgi
                + calculatedStats.critFromRating
                + calculatedStats.critFromProcRating
                + calculatedStats.critFromLethalShots
                + calculatedStats.critFromKillerInstincts
                + calculatedStats.critFromMasterMarksman
                + calculatedStats.critFromMasterTactician
                + calculatedStats.critFromBuffs
                + calculatedStats.critFromDepression;

            calculatedStats.critRateOverall = critHitPercent;

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

            // pet - part 1
            #region Pet MPS/Timing Calculations

            // this first block needs to run before the mana adjustments code,
            // since kill command effects mana usage.

            float baseMana = statsRace.Mana;
            calculatedStats.baseMana = statsRace.Mana;

            calculatedStats.pet.calculateTimings();

            #endregion

            // target debuffs
            #region Target Debuffs

            double targetDebuffsAP = 0; // Buffs!E77
            double targetDebuffsHit = 0; // Buffs!F77

            double targetDebuffsArmorCurseOfWeakness = 0; // TODO
            double targetDebuffsArmorFaerieFire = 0; // TODO
            double targetDebuffsArmorSunder = 0; // TODO
            double targetDebuffsArmorPet = calculatedStats.petArmorDebuffs;
            
            // only use one of CoW, FF or Pet
            double targetDebuffsArmorUse = targetDebuffsArmorCurseOfWeakness;
            if (targetDebuffsArmorUse == 0) targetDebuffsArmorUse = targetDebuffsArmorFaerieFire;
            if (targetDebuffsArmorUse == 0) targetDebuffsArmorUse = targetDebuffsArmorPet;

            double targetDebuffsArmor = 1 - (1 - targetDebuffsArmorSunder) * (1 - targetDebuffsArmorUse); // Buffs!G77

            double targetDebuffsMP5JudgmentOfWisdom = 0; // TODO!
            double targetDebuffsMP5 = targetDebuffsMP5JudgmentOfWisdom; // Buffs!H77

            double targetDebuffsMagicCurseOfElements = 0; // TODO
            double targetDebuffsMagicEarthAndMoon = 0; // TODO
            double targetDebuffsMagic = Math.Max(targetDebuffsMagicCurseOfElements, targetDebuffsMagicEarthAndMoon);

            double targetDebuffsFire = targetDebuffsMagic; // Buffs!I77
            double targetDebuffsArcane = targetDebuffsMagic; // Buffs!J77
            double targetDebuffsNature = targetDebuffsMagic; // Buffs!K77

            double targetDebuffsCritHeartOfTheCrusader = 0; // TODO
            double targetDebuffsCrit = targetDebuffsCritHeartOfTheCrusader; // Buffs!L77

            calculatedStats.targetDebuffsArmor = 1 - targetDebuffsArmor;
            calculatedStats.targetDebuffsNature = 1 + targetDebuffsNature;

            #endregion

            // mana consumption
            #region August 2009 Mana Adjustments

            double efficiencyManaAdjust = 1 - (character.HunterTalents.Efficiency * 0.03);

            // TODO: this should use the 'composite' crit chance, based on out actual rotation,
            // not the base crit chance (since some shots in our rotation don't crit)
            double thrillOfTheHuntManaAdjust = 1 - (calculatedStats.priorityRotation.critsCompositeSum * 0.4 * (character.HunterTalents.ThrillOfTheHunt / 3));

            double masterMarksmanManaAdjust = 1 - (character.HunterTalents.MasterMarksman * 0.05);

            // TODO: this should only activate if we have serpent or scorpid in the rotation
            double glyphOfArcaneShotManaAdjust = character.HunterTalents.GlyphOfArcaneShot ? 0.8 : 1;

            double ISSAimedShotManaAdjust = 1; // TODO: calculate this!
            double ISSArcaneShotManaAdjust = 1; // TODO: calculate this!
            double ISSChimeraShotManaAdjust = 1; // TODO: calculate this!

            double resourcefulnessManaAdjust = 1 - (character.HunterTalents.Resourcefulness * 0.2);

            #endregion
            #region August 2009 Shot Mana Usage

            // we do this ASAP so that we can get the MPS.
            // this allows us to calculate viper/aspect bonuses & penalties

            calculatedStats.steadyShot.mana = (baseMana * 0.05) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust * masterMarksmanManaAdjust;
            calculatedStats.serpentSting.mana = (baseMana * 0.09) * efficiencyManaAdjust;
            calculatedStats.aimedShot.mana = (baseMana * 0.08) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust * masterMarksmanManaAdjust * ISSAimedShotManaAdjust;
            calculatedStats.explosiveShot.mana = (baseMana * 0.07) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust;
            calculatedStats.chimeraShot.mana = (baseMana * 0.12) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust * masterMarksmanManaAdjust * ISSChimeraShotManaAdjust;
            calculatedStats.arcaneShot.mana = (baseMana * 0.05) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust * ISSArcaneShotManaAdjust * glyphOfArcaneShotManaAdjust;
            calculatedStats.multiShot.mana = (baseMana * 0.09) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust;
            calculatedStats.blackArrow.mana = (baseMana * 0.06) * efficiencyManaAdjust * resourcefulnessManaAdjust;
            calculatedStats.killShot.mana = (baseMana * 0.07) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust;
            calculatedStats.silencingShot.mana = (baseMana * 0.06) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust;
            calculatedStats.rapidFire.mana = (baseMana * 0.03);

            calculatedStats.priorityRotation.calculateRotationMPS();

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

            double huntingPartyProc = (double)character.HunterTalents.HuntingParty / 3.0;

            double huntingPartyArcaneFreq = calculatedStats.arcaneShot.freq;
            double huntingPartyArcaneCrit = calculatedStats.arcaneShot.critChance;
            double huntingPartyArcaneUptime = huntingPartyArcaneFreq > 0 ? 1 - Math.Pow(1 - huntingPartyArcaneCrit * huntingPartyProc, 15 / huntingPartyArcaneFreq) : 0;

            double huntingPartyExplosiveFreq = calculatedStats.explosiveShot.freq; // spreadsheet divides by 3, but doesn't use that value?
            double huntingPartyExplosiveCrit = calculatedStats.explosiveShot.critChance;
            double huntingPartyExplosiveUptime = huntingPartyExplosiveFreq > 0 ? 1 - Math.Pow(1 - huntingPartyExplosiveCrit * huntingPartyProc, 15 / huntingPartyExplosiveFreq) : 0;

            double huntingPartySteadyFreq = calculatedStats.steadyShot.freq;
            double huntingPartySteadyCrit = calculatedStats.steadyShot.critChance;
            double huntingPartySteadyUptime = huntingPartySteadyFreq > 0 ? 1 - Math.Pow(1 - huntingPartySteadyCrit * huntingPartyProc, 15 / huntingPartySteadyFreq) : 0;

            double huntingPartyCumulativeUptime = huntingPartyArcaneUptime + ((1 - huntingPartyArcaneUptime) * huntingPartyExplosiveUptime);
            double huntingPartyUptime = huntingPartyCumulativeUptime + ((1 - huntingPartyCumulativeUptime) * huntingPartySteadyUptime);

            calculatedStats.manaRegenHuntingParty = 0.002 * calculatedStats.BasicStats.Mana * huntingPartyUptime;

            // If we've got a replenishment buff up, use that instead of our own Hunting Party
            double manaRegenReplenishment = statsBuffs.ManaRestoreFromMaxManaPerSecond * calculatedStats.BasicStats.Mana;
            if (manaRegenReplenishment > 0)
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
            #region August 2009 Aspect Usage

            double manaRegenTier7ViperBonus = character.ActiveBuffsContains("Cryptstalker Battlegear 4 Piece Bonus") ? 1.2 : 1;

            double glpyhOfAspectOfTheViperBonus = character.HunterTalents.GlyphOfAspectOfTheViper ? 1.1 : 1;

            double manaRegenFromViper = calculatedStats.BasicStats.Mana * Math.Round(rangedWeaponSpeed, 1) / 100 * shotsPerSecondWithoutHawk
                                        * manaRegenTier7ViperBonus * glpyhOfAspectOfTheViperBonus
                                        + calculatedStats.BasicStats.Mana * 0.04 / 3;

            double manaFromPotion = 0;
            if (options.useManaPotion == ManaPotionType.RunicManaPotion) manaFromPotion = 4300;
            if (options.useManaPotion == ManaPotionType.SuperManaPotion) manaFromPotion = 2400;

            bool manaHasAlchemistStone = false;
            if (options.emulateSpreadsheetBugs)
            {
                if (IsWearingTrinket(character, 40684)) manaHasAlchemistStone = true; // Mirror of Truth (bug)
                if (IsWearingTrinket(character, 31856)) manaHasAlchemistStone = true; // Darkmoon Card: Crusade (bug)
            }
            else
            {
                if (IsWearingTrinket(character, 35751)) manaHasAlchemistStone = true; // Assassin's Alchemist Stone
                if (IsWearingTrinket(character, 44324)) manaHasAlchemistStone = true; // Mighty Alchemist's Stone
            }

            double manaRegenFromPotion = manaFromPotion / options.duration * (manaHasAlchemistStone ? 1.4 : 1.0);

            double manaExpenditure = calculatedStats.priorityRotation.MPS;
            manaExpenditure += calculatedStats.petKillCommandMPS;

            double manaChangeDuringViper = manaRegenFromViper + manaRegenFromPotion + calculatedStats.manaRegenTotal - manaExpenditure;
            double manaChangeDuringNormal = manaExpenditure - calculatedStats.manaRegenTotal - manaRegenFromPotion;

            double timeToFull = manaChangeDuringViper > 0 ? calculatedStats.BasicStats.Mana / manaChangeDuringViper : -1;
            double timeToOOM = manaChangeDuringNormal > 0 ? calculatedStats.BasicStats.Mana / manaChangeDuringNormal : -1;

            double viperTimeNeededToLastFight = 0;
            if (timeToOOM >= 0 && timeToOOM < options.duration && manaRegenFromViper > 0)
            {
                viperTimeNeededToLastFight = ((manaChangeDuringNormal * options.duration) - calculatedStats.BasicStats.Mana) / manaRegenFromViper;
            }

            double aspectUptimeHawk = 0;

            double aspectUptimeViper = 0;
            if (timeToOOM >= 0 && options.aspectUsage != AspectUsage.AlwaysOn)
            {
                if (options.aspectUsage == AspectUsage.ViperRegen)
                {
                    aspectUptimeViper = timeToFull / (timeToFull + timeToOOM);
                }
                else
                {
                    if (viperTimeNeededToLastFight > 0)
                    {
                        aspectUptimeViper = viperTimeNeededToLastFight / options.duration;
                    }
                }
            }

            // TODO: use BW uptime here                
            double aspectUptimeBeast = options.useBeastDuringBeastialWrath ? 0 : 0;

            switch (options.selectedAspect)
            {
                case Aspect.Viper:
                    aspectUptimeViper = options.useBeastDuringBeastialWrath ? 1 - aspectUptimeBeast : 1;
                    break;

                case Aspect.Beast:
                    aspectUptimeBeast = (options.aspectUsage == AspectUsage.AlwaysOn) ? 1 : 1 - aspectUptimeViper;
                    break;

                case Aspect.Hawk:
                case Aspect.Dragonhawk:
                    aspectUptimeHawk = 1 - aspectUptimeViper - aspectUptimeBeast;
                    break;
            }


            // we now know aspect uptimes - calculate bonuses and penalties

            double viperDamageEffect = character.HunterTalents.AspectMastery == 1 ? 0.4 : 0.5;
            double viperDamagePenalty = aspectUptimeViper * viperDamageEffect;

            double beastStaticAPBonus = character.HunterTalents.GlyphOfTheBeast ? 0.12 : 0.1;
            double beastAPBonus = aspectUptimeBeast * beastStaticAPBonus;

            double tier7ViperDamageAdjust = 1.0 + (character.ActiveBuffsContains("Cryptstalker Battlegear 4 Piece Bonus") ? 0.2 * aspectUptimeViper : 0);

            calculatedStats.aspectUptimeHawk = aspectUptimeHawk;
            calculatedStats.aspectUptimeBeast = aspectUptimeBeast;
            calculatedStats.aspectUptimeViper = aspectUptimeViper;
            calculatedStats.aspectViperPenalty = viperDamagePenalty;

            #endregion

            // damage
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
                calculatedStats.apFromBloodFury *= CalcUptime(15, 120, options);
            }

            // Aspect of the Hawk
            calculatedStats.apFromAspectOfTheHawk = 0;
            if (options.selectedAspect == Aspect.Hawk || options.selectedAspect == Aspect.Dragonhawk)
            {
                calculatedStats.apFromAspectOfTheHawk = 300 * aspectUptimeHawk;
            }

            calculatedStats.apFromFuriousHowl = 0;
            if (options.PetFamily == PetFamily.Wolf)
            {
                calculatedStats.apFromFuriousHowl = 320 * CalcUptime(20, 40, options);
            }

            // Expose Weakness
            double exposeWeaknessShotsPerSecond = crittingShotsPerSecond;
            double exposeWeaknessCritChance = calculatedStats.priorityRotation.critsCompositeSum;
            double exposeWeaknessAgility = calculatedStats.BasicStats.Agility * 0.25;
            double exposeWeaknessPercent = 0;
            if (character.HunterTalents.ExposeWeakness == 1) exposeWeaknessPercent = 0.33;
            if (character.HunterTalents.ExposeWeakness == 2) exposeWeaknessPercent = 0.66;
            if (character.HunterTalents.ExposeWeakness == 3) exposeWeaknessPercent = 1;
            double exposeWeaknessUptime = 1 - Math.Pow(1 - (exposeWeaknessPercent * exposeWeaknessCritChance), 7 * exposeWeaknessShotsPerSecond);

            calculatedStats.apFromExposeWeakness = exposeWeaknessUptime * exposeWeaknessAgility;

            calculatedStats.apFromCallOfTheWild = options.petCallOfTheWild * (CalcUptime(20, 300, options) * 0.1);

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


            calculatedStats.apFromProc = 0;

            // Mirror of Truth
            if (IsWearingTrinket(character, 40684))
            {
                if (options.emulateSpreadsheetBugs)
                {
                    calculatedStats.apFromProc += 1000 * CalcTrinketUptime(10, 45, 0.1, crittingShotsPerSecond * critHitPercent);
                }
                else
                {
                    calculatedStats.apFromProc += 1000 * CalcTrinketUptime(10, 45, 0.1, crittingShotsPerSecond * critHitPercent * hitChance);
                }
            }

            // Anvil of Titans
            if (IsWearingTrinket(character, 44914))
            {
                calculatedStats.apFromProc += 1000 * CalcTrinketUptime(10, 45, 0.1, totalShotsPerSecond * hitChance);
            }

            // Swordguard Embroidery
            if (character.BackEnchant != null && character.BackEnchant.Id == 3730)
            {
                if (options.emulateSpreadsheetBugs)
                {
                    calculatedStats.apFromProc += 300 * CalcTrinketUptime(15, 45, 0.5, totalShotsPerSecond * critHitPercent);
                }
                else
                {
                    calculatedStats.apFromProc += 300 * CalcTrinketUptime(15, 45, 0.5, totalShotsPerSecond * hitChance);
                }
            }

            // TODO: more proc AP effects!

            // TODO: add multiplicitive buffs
            double apScalingFactor = 1
                * (1 + calculatedStats.apFromCallOfTheWild)
                * (1 + calculatedStats.apFromTrueshotAura);

            // use for pet calculations
            calculatedStats.apSelfBuffed = 0
                + calculatedStats.apFromBase
                + calculatedStats.apFromAgil
                + calculatedStats.apFromCarefulAim
                + calculatedStats.apFromHunterVsWild
                + calculatedStats.apFromGear // includes buffs
                + calculatedStats.apFromBloodFury
                + calculatedStats.apFromAspectOfTheHawk
                + calculatedStats.apFromAspectMastery
                + calculatedStats.apFromFuriousHowl
                // TODO: target debuffs
                + calculatedStats.apFromProc;

            // used for hunter calculations
            calculatedStats.apTotal = calculatedStats.apSelfBuffed
                + calculatedStats.apFromExposeWeakness
                + calculatedStats.apFromHuntersMark;

            // apply scaling
            calculatedStats.apTotal *= apScalingFactor;
            calculatedStats.apSelfBuffed *= apScalingFactor;

            double RAP = calculatedStats.apTotal;

            #endregion
            #region August 2009 Damage Adjustments

            // Armor Penetration & Debuffs
            double targetArmorSubtotal = options.TargetArmor * calculatedStats.targetDebuffsArmor;
            double arpOnProcRating = 0; // TODO
            double arpGearRating = calculatedStats.BasicStats.ArmorPenetrationRating;
            double arpTotal = arpGearRating + arpOnProcRating;

            double arpPercentReduction = arpTotal / HunterRatings.ARP_RATING_PER_PERCENT / 100;
            if (arpPercentReduction > 1) arpPercentReduction = 1;

            double arpEffectCap = (targetArmorSubtotal + (400 + 85 * character.Level + 4.5 * 85 * (character.Level - 59))) / 3;
            if (arpEffectCap > targetArmorSubtotal) arpEffectCap = targetArmorSubtotal;

            double targetArmorRemoved = arpEffectCap * arpPercentReduction;

            double effectiveArmor = targetArmorSubtotal - targetArmorRemoved;
            double armorReduction = effectiveArmor / (effectiveArmor - 22167.5 + (467.5 * character.Level));

            double armorReductionDamageAdjust = 1 - armorReduction;

            //Partial Resists
            double averageResist = (options.TargetLevel - 80) * 0.02;
            double resist10 = 5 * averageResist;
            double resist20 = 2.5 * averageResist;
            double partialResistDamageAdjust = 1 - (resist10 * 0.1 + resist20 * 0.1);

            //Beastial Wrath
            double beastialWrathCooldown = character.HunterTalents.GlyphOfBestialWrath ? 100 : 120;

            beastialWrathCooldown *= 1 - 0.1 * character.HunterTalents.Longevity;

            double beastialWrathUptime = CalcUptime(18, beastialWrathCooldown, options);

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
                blackArrowUptime = CalcUptime(calculatedStats.blackArrow.duration, calculatedStats.blackArrow.freq, options);
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

            // pet - part 2
            #region Pet DPS Calculations

            calculatedStats.pet.calculateDPS();

            #endregion

            // shot damage calcs
            #region August 2009 AutoShot

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

            double hunterAutoDPS = autoShotsPerSecond * autoShotDamageReal
                                    * (1 - viperDamagePenalty) * tier7ViperDamageAdjust;

            calculatedStats.aspectBeastLostDPS = (0 - QSBaseFreqnecyIncrease) * (1 - aspectUptimeHawk) * hunterAutoDPS;

            calculatedStats.AutoshotDPS = hunterAutoDPS;

            


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

            calculatedStats.serpentSting.type = Shots.SerpentSting;
            calculatedStats.serpentSting.damage = serpentStingDamageReal;
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

            calculatedStats.aimedShot.damage = aimedShotDamageReal;
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

            calculatedStats.explosiveShot.damage = explosiveShotDamagePerShot;
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

            calculatedStats.chimeraShot.damage = chimeraShotDamageTotal;
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

            calculatedStats.arcaneShot.damage = arcaneShotDamageReal;
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

            calculatedStats.multiShot.damage = multiShotDamageReal;
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

            calculatedStats.blackArrow.damage = blackArrowDamage;
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

            calculatedStats.killShot.damage = killShotDamageReal;
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

            calculatedStats.silencingShot.damage = silencingShotDamageReal;
            //calculatedStats.silencingShot.Dump("Silencing Shot");

            #endregion
            #region August 2009 Rapid Fire

            calculatedStats.rapidFire.damage = 0;

            #endregion

            #region August 2009 On-Proc DPS
            // calculatedStats.OnProcDPS
            // TODO: Bandit's Insignia
            // TODO: Gnomish Lightning Generator
            // TODO: Darkmoon Card: Death
            // TODO: Hand-Mounted Pyro Rocket
            // TODO: Vestige of Haldor
            #endregion
            #region August 2009 Shot Rotation

            calculatedStats.priorityRotation.viperDamagePenalty = viperDamagePenalty;
            calculatedStats.priorityRotation.calculateRotationDPS();
            calculatedStats.CustomDPS = calculatedStats.priorityRotation.DPS;

            //Debug.WriteLine("Rotation DPS = " + calculatedStats.priorityRotation.DPS);
            //Debug.WriteLine("Rotation MPS = " + calculatedStats.priorityRotation.MPS);

            #endregion
            #region August 2009 Kill Shot Sub-20% Usage

            double killShotCurrentFreq = calculatedStats.killShot.freq;
            double killShotPossibleFreq = calculatedStats.killShot.start_freq;
            double steadyShotCurrentFreq = calculatedStats.steadyShot.freq;

            double steadyShotNewFreq = steadyShotCurrentFreq;
            if (killShotCurrentFreq == 0 && steadyShotCurrentFreq > 0 && killShotPossibleFreq > 0)
            {
                steadyShotNewFreq = 1 / (1 / steadyShotCurrentFreq - 1 / killShotPossibleFreq);
            }

            double oldKillShotDPS = calculatedStats.killShot.dps;
            double newKillDhotDPS = killShotPossibleFreq > 0 ? calculatedStats.killShot.damage / killShotPossibleFreq : 0;

            double oldSteadyShotDPS = calculatedStats.steadyShot.dps;
            double newSteadyShotDPS = steadyShotNewFreq > 0 ? calculatedStats.steadyShot.damage / steadyShotNewFreq : 0;

            double killShotDPSGain = newKillDhotDPS > 0 ? (newKillDhotDPS + newSteadyShotDPS) - (oldKillShotDPS + oldSteadyShotDPS) : 0;

            double timeSpentSubTwenty = 0;
            if (options.duration > 0 && options.timeSpentSub20 > 0) timeSpentSubTwenty = (double)options.timeSpentSub20 / (double)options.duration;
            if (options.bossHPPercentage < 0.2) timeSpentSubTwenty = 1;

            double killShotSubGain = timeSpentSubTwenty * killShotDPSGain * (1 - viperDamagePenalty);

            calculatedStats.killShotSub20NewSteadyFreq = steadyShotNewFreq;
            calculatedStats.killShotSub20NewDPS = newKillDhotDPS;
            calculatedStats.killShotSub20NewSteadyDPS = newSteadyShotDPS;
            calculatedStats.killShotSub20Gain = killShotDPSGain;
            calculatedStats.killShotSub20TimeSpent = timeSpentSubTwenty;
            calculatedStats.killShotSub20FinalGain = killShotSubGain;

            #endregion


            calculatedStats.HunterDpsPoints = (float)(
                                                    calculatedStats.AutoshotDPS
                                                  + calculatedStats.WildQuiverDPS 
                                                  + calculatedStats.CustomDPS
                                                  + calculatedStats.OnProcDPS
                                                  + calculatedStats.killShotSub20FinalGain
                                                  + calculatedStats.aspectBeastLostDPS
                                               );
            calculatedStats.OverallPoints = calculatedStats.HunterDpsPoints + calculatedStats.PetDpsPoints;

            return calculatedStats;
        }

		public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
            Stats statsRace = BaseStats.GetBaseStats(80, CharacterClass.Hunter, character.Race);
			Stats statsBaseGear = GetItemStats(character, additionalItem);
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
            // spreadsheet uses 18.7, so we will too :)
            statsTotal.Mana = (float)(statsRace.Mana + 15f * (statsTotal.Intellect - 18.7) + statsGearEnchantsBuffs.Mana);

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
            statsTotal.MultiShotCooldownReduction = statsGearEnchantsBuffs.MultiShotCooldownReduction;

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

        public double CalcUptime(double duration, double cooldown, CalculationOptionsHunter options)
        {
            if (options.calculateUptimesLikeSpreadsheet)
            {
                return cooldown > 0 ? duration / cooldown : 0;
            }

            double length = options.duration;
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

        public static double CalcEffectiveDamage(double damageNormal, double hitChance, double critChance, double critAdjust, double damageAdjust)
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

        public static double CalcTrinketUptime(double duration, double cooldown, double chance, double triggersPerSecond)
        {
            double timePerTrigger = triggersPerSecond > 0 ? 1 / triggersPerSecond : 0;
            double time_between_procs = timePerTrigger > 0 ? 1 / chance * timePerTrigger + cooldown : 0;
            return time_between_procs > 0 ? duration / time_between_procs : 0;
        }

        private static bool IsWearingTrinket(Character character, int trinket_id)
        {
            if (character.Trinket1 != null && character.Trinket1.Id == trinket_id) return true;
            if (character.Trinket2 != null && character.Trinket2.Id == trinket_id) return true;
            return false;
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
