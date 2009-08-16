using System;
using System.Collections.Generic;
#if SILVERLIGHT
using System.Windows.Media;
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
    /// All calculations in this module are based on Shandara's DPS Spreadsheet
    /// and will be updated from that sheet until it is no longer actively 
    /// maintained.
    /// 
    /// Version: 0.91a
    /// Released: July 28, 2009
    /// Link: http://elitistjerks.com/f74/t30710-wotlk_dps_spreadsheet/
    /// 
    /// Current maintainer: iamcal
    /// Previous maintainers: lawjpbo, nosnevel
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
				"Basic Stats:Static Haste",
				"Basic Stats:Dynamic Haste",
				"Basic Stats:Mana Regen Per Second",				

				"Basic Calculated Stats:Health",
				"Basic Calculated Stats:Mana",
				"Basic Calculated Stats:Hit Percentage",
				"Basic Calculated Stats:Crit Percentage",
				"Basic Calculated Stats:Ranged AP",
				"Basic Calculated Stats:Attack Speed",

                "Pet Stats:Pet Attack Power",
				"Pet Stats:Pet Hit Percentage",
				"Pet Stats:Pet Melee Crit Percentage",
				"Pet Stats:Pet Specials Crit Percentage",
				"Pet Stats:Pet White DPS",
				"Pet Stats:Pet Kill Command DPS",
				"Pet Stats:Pet Specials DPS",

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

            customChartNames = new string[] {
                "Spammed Shots DPS",
                "Spammed Shots MPS",
                "Rotation DPS",
                "Rotation MPS",
                "Shot Damage per Mana",
                "Item Budget"
            };

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

            _subPointNameColorsDPS = new Dictionary<string, Color>();
            _subPointNameColorsDPS.Add("Hunter DPS", Color.FromArgb(0, 128, 255));
            _subPointNameColorsDPS.Add("Pet DPS", Color.FromArgb(255, 100, 0));

            _subPointNameColorsMPS = new Dictionary<string, Color>();
            _subPointNameColorsMPS.Add("MPS", Color.FromArgb(0, 0, 255));

            _subPointNameColorsDPM = new Dictionary<string, Color>();
            _subPointNameColorsDPM.Add("Damage per Mana", Color.FromArgb(0, 0, 255));

            _subPointNameColors = _subPointNameColorsDPS;
		}

        private Dictionary<string, Color> _subPointNameColors = null;
        private Dictionary<string, Color> _subPointNameColorsDPS = null;
        private Dictionary<string, Color> _subPointNameColorsMPS = null;
        private Dictionary<string, Color> _subPointNameColorsDPM = null;
        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                Dictionary<string, Color> ret = _subPointNameColors;
                _subPointNameColors = _subPointNameColorsDPS;
                return ret;
            }
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
            CharacterCalculationsHunter calculations = GetCharacterCalculations(character) as CharacterCalculationsHunter;

            switch (chartName)
            {
                case "Spammed Shots DPS":

                    return new ComparisonCalculationBase[] {
                        comparisonFromShotSpammedDPS(calculations.aimedShot),
                        comparisonFromShotSpammedDPS(calculations.arcaneShot),
                        comparisonFromShotSpammedDPS(calculations.multiShot),
                        comparisonFromShotSpammedDPS(calculations.serpentSting),
                        comparisonFromShotSpammedDPS(calculations.scorpidSting),
                        comparisonFromShotSpammedDPS(calculations.viperSting),
                        comparisonFromShotSpammedDPS(calculations.silencingShot),
                        comparisonFromShotSpammedDPS(calculations.steadyShot),
                        comparisonFromShotSpammedDPS(calculations.killShot),
                        comparisonFromShotSpammedDPS(calculations.explosiveShot),
                        comparisonFromShotSpammedDPS(calculations.blackArrow),
                        comparisonFromShotSpammedDPS(calculations.immolationTrap),
                        comparisonFromShotSpammedDPS(calculations.chimeraShot),
                    };

                case "Spammed Shots MPS":

                    _subPointNameColors = _subPointNameColorsMPS;
                    return new ComparisonCalculationBase[] {
                        comparisonFromShotSpammedMPS(calculations.aimedShot),
                        comparisonFromShotSpammedMPS(calculations.arcaneShot),
                        comparisonFromShotSpammedMPS(calculations.multiShot),
                        comparisonFromShotSpammedMPS(calculations.serpentSting),
                        comparisonFromShotSpammedMPS(calculations.scorpidSting),
                        comparisonFromShotSpammedMPS(calculations.viperSting),
                        comparisonFromShotSpammedMPS(calculations.silencingShot),
                        comparisonFromShotSpammedMPS(calculations.steadyShot),
                        comparisonFromShotSpammedMPS(calculations.killShot),
                        comparisonFromShotSpammedMPS(calculations.explosiveShot),
                        comparisonFromShotSpammedMPS(calculations.blackArrow),
                        comparisonFromShotSpammedMPS(calculations.immolationTrap),
                        comparisonFromShotSpammedMPS(calculations.chimeraShot),
                        comparisonFromShotSpammedMPS(calculations.rapidFire),
                        comparisonFromShotSpammedMPS(calculations.readiness),
                        comparisonFromShotSpammedMPS(calculations.beastialWrath),
                        comparisonFromShotSpammedMPS(calculations.bloodFury),
                        comparisonFromShotSpammedMPS(calculations.berserk),
                    };

                case "Rotation DPS":

                    return new ComparisonCalculationBase[] {
                        comparisonFromShotRotationDPS(calculations.aimedShot),
                        comparisonFromShotRotationDPS(calculations.arcaneShot),
                        comparisonFromShotRotationDPS(calculations.multiShot),
                        comparisonFromShotRotationDPS(calculations.serpentSting),
                        comparisonFromShotRotationDPS(calculations.scorpidSting),
                        comparisonFromShotRotationDPS(calculations.viperSting),
                        comparisonFromShotRotationDPS(calculations.silencingShot),
                        comparisonFromShotRotationDPS(calculations.steadyShot),
                        comparisonFromShotRotationDPS(calculations.killShot),
                        comparisonFromShotRotationDPS(calculations.explosiveShot),
                        comparisonFromShotRotationDPS(calculations.blackArrow),
                        comparisonFromShotRotationDPS(calculations.immolationTrap),
                        comparisonFromShotRotationDPS(calculations.chimeraShot),
                        comparisonFromDoubles("Autoshot", calculations.AutoshotDPS, 0),
                        comparisonFromDoubles("WildQuiver", calculations.WildQuiverDPS, 0),
                        comparisonFromDoubles("OnProc", calculations.OnProcDPS, 0),
                        comparisonFromDoubles("KillShotSub20", calculations.killShotSub20FinalGain, 0),
                        comparisonFromDoubles("AspectBeastLoss", calculations.aspectBeastLostDPS, 0),
                        comparisonFromDoubles("PetAutoAttack", 0, calculations.petWhiteDPS),
                        comparisonFromDoubles("PetSkills", 0, calculations.petSpecialDPS),
                        comparisonFromDoubles("KillCommand", 0, calculations.petKillCommandDPS),
                    };

                case "Rotation MPS":

                    _subPointNameColors = _subPointNameColorsMPS;
                    return new ComparisonCalculationBase[] {
                        comparisonFromShotRotationMPS(calculations.aimedShot),
                        comparisonFromShotRotationMPS(calculations.arcaneShot),
                        comparisonFromShotRotationMPS(calculations.multiShot),
                        comparisonFromShotRotationMPS(calculations.serpentSting),
                        comparisonFromShotRotationMPS(calculations.scorpidSting),
                        comparisonFromShotRotationMPS(calculations.viperSting),
                        comparisonFromShotRotationMPS(calculations.silencingShot),
                        comparisonFromShotRotationMPS(calculations.steadyShot),
                        comparisonFromShotRotationMPS(calculations.killShot),
                        comparisonFromShotRotationMPS(calculations.explosiveShot),
                        comparisonFromShotRotationMPS(calculations.blackArrow),
                        comparisonFromShotRotationMPS(calculations.immolationTrap),
                        comparisonFromShotRotationMPS(calculations.chimeraShot),
                        comparisonFromShotRotationMPS(calculations.rapidFire),
                        comparisonFromShotRotationMPS(calculations.readiness),
                        comparisonFromShotRotationMPS(calculations.beastialWrath),
                        comparisonFromShotRotationMPS(calculations.bloodFury),
                        comparisonFromShotRotationMPS(calculations.berserk),
                        comparisonFromDouble("KillCommand", calculations.petKillCommandMPS),
                    };

                case "Shot Damage per Mana":

                    _subPointNameColors = _subPointNameColorsDPM;
                    return new ComparisonCalculationBase[] {
                        comparisonFromShotDPM(calculations.aimedShot),
                        comparisonFromShotDPM(calculations.arcaneShot),
                        comparisonFromShotDPM(calculations.multiShot),
                        comparisonFromShotDPM(calculations.serpentSting),
                        comparisonFromShotDPM(calculations.scorpidSting),
                        comparisonFromShotDPM(calculations.viperSting),
                        comparisonFromShotDPM(calculations.silencingShot),
                        comparisonFromShotDPM(calculations.steadyShot),
                        comparisonFromShotDPM(calculations.killShot),
                        comparisonFromShotDPM(calculations.explosiveShot),
                        comparisonFromShotDPM(calculations.blackArrow),
                        comparisonFromShotDPM(calculations.immolationTrap),
                        comparisonFromShotDPM(calculations.chimeraShot),
                    };

                case "Item Budget":

                    return new ComparisonCalculationBase[] { 
                        comparisonFromStat(character, calculations, new Stats() { Intellect = 10f }, "10 Intellect"),
                        comparisonFromStat(character, calculations, new Stats() { Agility = 10f }, "10 Agility"),
                        comparisonFromStat(character, calculations, new Stats() { Mp5 = 4f }, "4 MP5"),
                        comparisonFromStat(character, calculations, new Stats() { CritRating = 10f }, "10 Crit Rating"),
                        comparisonFromStat(character, calculations, new Stats() { HitRating = 10f }, "10 Hit Rating"),
                        comparisonFromStat(character, calculations, new Stats() { ArmorPenetrationRating = 10f }, "1.4 Armor Penetration Rating"),
                        comparisonFromStat(character, calculations, new Stats() { AttackPower = 20f }, "20 Attack Power"),
                        comparisonFromStat(character, calculations, new Stats() { RangedAttackPower = 25f }, "25 Ranged Attack Power"),
                        comparisonFromStat(character, calculations, new Stats() { HasteRating = 10f }, "10 Haste Rating"),
                    };
            }

            return new ComparisonCalculationBase[0];

        }

        public override Stats GetRelevantStats(Stats stats)
        {
			return new Stats()
			{
                Agility = stats.Agility,
                ArmorPenetrationRating = stats.ArmorPenetrationRating,
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
				ShatteredSunAcumenProc = stats.ShatteredSunAcumenProc,
				ShatteredSunMightProc = stats.ShatteredSunMightProc,
				BonusSteadyShotCrit = stats.BonusSteadyShotCrit,
				BonusSteadyShotDamageMultiplier = stats.BonusSteadyShotDamageMultiplier,
				BonusRangedAttackPowerMultiplier = stats.BonusRangedAttackPowerMultiplier,
				BonusAspectOfTheViperGain = stats.BonusAspectOfTheViperGain,
				BonusAspectOfTheViperAttackSpeed = stats.BonusAspectOfTheViperAttackSpeed,
				BonusSerpentStingDamage = stats.BonusSerpentStingDamage,
				BonusSteadyShotAttackPowerBuff = stats.BonusSteadyShotAttackPowerBuff,
				BonusSerpentStingCanCrit = stats.BonusSerpentStingCanCrit,
				BonusSteadyShotPetAttackPowerBuff = stats.BonusSteadyShotPetAttackPowerBuff,
				MultiShotManaDiscount = stats.MultiShotManaDiscount,
				MultiShotCooldownReduction = stats.MultiShotCooldownReduction,
				TrapCooldownReduction = stats.TrapCooldownReduction,
                FireDamage = stats.FireDamage,
                Stamina = stats.Stamina,
                ManaRestoreFromBaseManaPerHit = stats.ManaRestoreFromBaseManaPerHit,
                BonusFireDamageMultiplier = stats.BonusFireDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                BonusArcaneDamageMultiplier = stats.BonusArcaneDamageMultiplier,
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusHolyDamageMultiplier = stats.BonusHolyDamageMultiplier,
                BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier,
                BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            // this list must match the one above!
            double totalStats = stats.Agility +
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
            stats.PhysicalHit +
            stats.HitRating +
            stats.RangedHitRating +
            stats.Intellect +
            stats.Mp5 +
            stats.ScopeDamage +
            stats.ShatteredSunAcumenProc +
            stats.ShatteredSunMightProc +
            stats.BonusSteadyShotCrit +
            stats.BonusSteadyShotDamageMultiplier +
            stats.ManaRestoreFromMaxManaPerSecond +
            stats.BonusRangedAttackPowerMultiplier +
            stats.BonusAspectOfTheViperGain +
            stats.BonusAspectOfTheViperAttackSpeed +
            stats.BonusSerpentStingDamage +
            stats.BonusSteadyShotAttackPowerBuff +
            stats.BonusSerpentStingCanCrit +
            stats.BonusSteadyShotPetAttackPowerBuff +
            stats.MultiShotManaDiscount +
            stats.MultiShotCooldownReduction +
            stats.TrapCooldownReduction +
            stats.FireDamage + 
            stats.Stamina +
            stats.ManaRestoreFromBaseManaPerHit +
            stats.BonusFireDamageMultiplier +
            stats.BonusFrostDamageMultiplier +
            stats.BonusArcaneDamageMultiplier +
            stats.BonusShadowDamageMultiplier +
            stats.BonusHolyDamageMultiplier +
            stats.BonusNatureDamageMultiplier +
            stats.BonusBleedDamageMultiplier +
            stats.BonusPhysicalDamageMultiplier;

            if (totalStats > 0) return true;

            foreach (SpecialEffect e in stats.SpecialEffects())
            {
                if (e.Trigger != Trigger.MeleeHit)
                {
                    if (HasRelevantStats(e.Stats)) return true;
                }
            }

            return false;
        }

        public override bool IsEnchantRelevant(Enchant enchant)
        {
            if (enchant.Id == 3847) return false; // Rune of the Stoneskin Gargoyle - only DKs can use this
            return base.IsEnchantRelevant(enchant);
        }

        public override bool IsBuffRelevant(Buff buff)
        {
            if (buff.Name == "Concentration Aura") return false; // Gets selected due to a bug saying it increases BonusAspectOfTheViperAttackSpeed
            if (buff.Group == "Potion") return false;
            return base.IsBuffRelevant(buff);               
        }
       
        public override List<ItemType> RelevantItemTypes
        {
            get { return relevantItemTypes; }
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
            Stats statsTalents = GetBaseTalentStats(character.HunterTalents);

            calculatedStats.BasicStats = GetCharacterStats(character, additionalItem);

            calculatedStats.pet = new PetCalculations(character, calculatedStats, options, statsBuffs, statsBaseGear);
            
            if (character.Ranged == null || (character.Ranged.Item.Type != ItemType.Bow && character.Ranged.Item.Type != ItemType.Gun
                                            && character.Ranged.Item.Type != ItemType.Crossbow))
            {
                //skip all the calculations if there is no ranged weapon
                return calculatedStats;
            }

            // NOTE: this model just breaks if you're not level 80.
            // we should be using character.Level everywhere, but also
            // all of the spell levels will be wrong. do we care?


            //foreach (Buff buff in character.ActiveBuffs)
            //{
            //    Debug.WriteLine(buff);
            //}

            #region Spreadsheet bugs

            if (statsBuffs.PhysicalCrit > 0 && options.emulateSpreadsheetBugs)
            {
                // Leader of the Pack should give 5%, but instead gives 4.98845627020046000000%
                // (same as 229 crit rating)
                // BUT, *only* LotP/Rampage - other PhysicalCrit buffs (target debuffs like Heart of the Crusader) remain in place
                if (character.ActiveBuffsContains("Leader of the Pack") || character.ActiveBuffsContains("Rampage"))
                {
                    statsBuffs.CritRating += 229;
                    statsBuffs.PhysicalCrit -= 0.05f;
                }
            }

            #endregion

            // shot basics
            #region August 2009 Priority Rotation Setup

            calculatedStats.priorityRotation = new ShotPriority(options);

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

            calculatedStats.scorpidSting.cooldown = 20;
            calculatedStats.scorpidSting.duration = 15;

            calculatedStats.viperSting.cooldown = 15;
            calculatedStats.viperSting.duration = 8;

            calculatedStats.immolationTrap.cooldown = 30 - character.HunterTalents.Resourcefulness * 2;
            calculatedStats.immolationTrap.duration = character.HunterTalents.GlyphOfImmolationTrap ? 9 : 15;

            if (calculatedStats.priorityRotation.containsShot(Shots.Readiness))
            {
                calculatedStats.rapidFire.cooldown = 157.5 - (30 * character.HunterTalents.RapidKilling);
            }
            else
            {
                calculatedStats.rapidFire.cooldown = 300 - (60 * character.HunterTalents.RapidKilling);
            }
            calculatedStats.rapidFire.duration = 15;

            // We will set the correct value for this later, after we've calculated haste
            calculatedStats.steadyShot.cooldown = 2;

            calculatedStats.immolationTrap.cooldown = 30 - (character.HunterTalents.Resourcefulness * 2);
            calculatedStats.immolationTrap.duration = character.HunterTalents.GlyphOfImmolationTrap ? 9 : 15;

            calculatedStats.readiness.cooldown = 180;

            calculatedStats.beastialWrath.cooldown = (character.HunterTalents.GlyphOfBestialWrath ? 100 : 120) * (1 - character.HunterTalents.Longevity * 0.1);
            calculatedStats.beastialWrath.duration = options.PetFamily == PetFamily.None ? 0 : 18;

            calculatedStats.bloodFury.cooldown = 120;
            calculatedStats.bloodFury.duration = 15;

            calculatedStats.berserk.cooldown = 180;
            calculatedStats.berserk.duration = 10;

            // We can calculate the rough frequencies now
            calculatedStats.priorityRotation.initializeTimings();
            if (!options.useRotationTest)
            {
                calculatedStats.priorityRotation.calculateFrequencies();
                calculatedStats.priorityRotation.calculateLALProcs(character);
                calculatedStats.priorityRotation.calculateFrequencies();
                calculatedStats.priorityRotation.calculateFrequencySums();
            }

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
            #region August 2009 Static Haste Calcs

            //default quiver speed
            calculatedStats.hasteFromBase = 0.15;

            // haste from haste rating
            calculatedStats.hasteFromRating = calculatedStats.BasicStats.HasteRating / (HunterRatings.HASTE_RATING_PER_PERCENT * 100);

            // serpent swiftness
            calculatedStats.hasteFromTalentsStatic = 0.04 * character.HunterTalents.SerpentsSwiftness;

            // haste buffs
            calculatedStats.hasteFromRangedBuffs = calculatedStats.BasicStats.RangedHaste;

            // total hastes
            double totalStaticHaste = (1 + calculatedStats.hasteFromBase)             // quiver
                                    * (1 + calculatedStats.hasteFromRating)           // gear haste rating
                                    * (1 + calculatedStats.hasteFromTalentsStatic)    // serpent's swiftness
                                    * (1 + calculatedStats.hasteFromRangedBuffs);     // buffs like swift ret / moonkin

            calculatedStats.hasteStaticTotal = totalStaticHaste;


            // Needed by the rotation test
            calculatedStats.autoShotStaticSpeed = rangedWeaponSpeed / totalStaticHaste;

            #endregion
            #region Rotation Test

            // Quick shots effect is needed for rotation test
            calculatedStats.quickShotsEffect = 0;
            if (options.selectedAspect == Aspect.Hawk || options.selectedAspect == Aspect.Dragonhawk)
            {
                if (character.HunterTalents.ImprovedAspectOfTheHawk > 0)
                {
                    double quickShotsEffect = 0.03 * character.HunterTalents.ImprovedAspectOfTheHawk;
                    if (character.HunterTalents.GlyphOfTheHawk) quickShotsEffect += 0.06;

                    calculatedStats.quickShotsEffect = quickShotsEffect;
                }
            }


            // Using the rotation test will get us better frequencies
            RotationTest rotationTest = new RotationTest(character, calculatedStats, options);

            if (options.useRotationTest)
            {
                // The following properties of CalculatedStats must be ready by this call:
                //  * priorityRotation (shot order, durations, cooldowns)
                //  * quickShotsEffect
                //  * hasteStaticTotal
                //  * autoShotStaticSpeed

                rotationTest.RunTest();
            }

            #endregion
            #region August 2009 Dynamic Haste Calcs

            // troll berserking
            calculatedStats.hasteFromRacial = 0;
            if (character.Race == CharacterRace.Troll && calculatedStats.berserk.freq > 0)
            {
                double berserkingUseFreq = options.emulateSpreadsheetBugs ? calculatedStats.berserk.cooldown : calculatedStats.berserk.freq; // still an issue in 91b
                calculatedStats.hasteFromRacial = 0.2 * CalcUptime(calculatedStats.berserk.duration, berserkingUseFreq, options);
            }

            // rapid fire
            double rapidFireHaste = character.HunterTalents.GlyphOfRapidFire ? 0.48 : 0.4;
            double rapidFireCooldown = calculatedStats.rapidFire.freq;
            if (!calculatedStats.priorityRotation.containsShot(Shots.RapidFire)) rapidFireHaste = 0;
            calculatedStats.hasteFromRapidFire = rapidFireHaste * CalcUptime(15, rapidFireCooldown, options);

            // heroism
            calculatedStats.hasteFromHeroism = 0;
            if (options.heroismUsage != HeroismUsage.Never)
            {
                double heroismCooldown = options.heroismUsage == HeroismUsage.Once ? options.duration : 600;
                double heroismUptime = CalcUptime(40, heroismCooldown, options);
                calculatedStats.hasteFromHeroism = 0.3 * heroismUptime;
            }

            // proc haste from gear/trinkets/etc
            double hasteRatingFromProcs = 0;

            // Hyperspeed Accelerators
            if (character.HandsEnchant != null && character.HandsEnchant.Id == 3604)
            {
                hasteRatingFromProcs += 340 * CalcUptime(12, 60, options);
            }

            // Signet of Edward the Odd
            if (IsWearingRing(character, 44308))
            {
                // This value is 'wrong', but it depends on haste, so it's impossible to get correctly
                double signetOfEdwardTimePerShot = 0.9;
                double signetOfEdwardTimeBetweenProcs = 1 / 0.15 * signetOfEdwardTimePerShot + 45;
                hasteRatingFromProcs += 125 * CalcUptime(13, signetOfEdwardTimeBetweenProcs, options);
            }

            // Tears of Bitter Anguish
            if (IsWearingTrinket(character, 43573))
            {
                // This value is 'wrong', but it depends on haste, so it's impossible to get correctly
                double tearsOfBitterTimePerCrit = 1.8;
                double tearsOfBitterTimeBetweenProcs = 1 / 0.1 * tearsOfBitterTimePerCrit + 45;
                hasteRatingFromProcs += 410 * CalcUptime(10, tearsOfBitterTimeBetweenProcs, options);
            }

            // Comet's Trail
            if (IsWearingTrinket(character, 45609))
            {
                // This value is 'wrong', but it depends on haste, so it's impossible to get correctly
                double cometsTrailTimePerShot = 0.9;
                double cometsTrailTimeBetweenProcs = 1 / 0.15 * cometsTrailTimePerShot + 45;
                hasteRatingFromProcs += 726 * CalcUptime(10, cometsTrailTimeBetweenProcs, options);
            }

            // Meteorite Whetstone
            if (IsWearingTrinket(character, 37390))
            {
                // This value is 'wrong', but it depends on haste, so it's impossible to get correctly
                double meteoriteWhetstoneTimePerShot = 0.9;
                double meteoriteWhetstoneTimeBetweenProcs = 1 / 0.15 * meteoriteWhetstoneTimePerShot + 45;
                hasteRatingFromProcs += 444 * CalcUptime(10, meteoriteWhetstoneTimeBetweenProcs, options);
            }

            // Simple-use haste trinkets
            if (character.Trinket1 != null)
            {
                foreach (SpecialEffect e in character.Trinket1.GetTotalStats().SpecialEffects())
                {
                    if (e.Trigger == Trigger.Use && e.Stats.HasteRating > 0)
                    {
                        hasteRatingFromProcs += e.Stats.HasteRating * e.Duration / e.Cooldown;
                    }
                }
            }
            if (character.Trinket2 != null)
            {
                foreach (SpecialEffect e in character.Trinket2.GetTotalStats().SpecialEffects())
                {
                    if (e.Trigger == Trigger.Use && e.Stats.HasteRating > 0)
                    {
                        hasteRatingFromProcs += e.Stats.HasteRating * e.Duration / e.Cooldown;
                    }
                }
            }

            calculatedStats.hasteFromProcs = hasteRatingFromProcs / (HunterRatings.HASTE_RATING_PER_PERCENT * 100);

            double totalDynamicHaste = (1 + calculatedStats.hasteFromRapidFire)       // rapid fire
                                     * (1 + calculatedStats.hasteFromRacial)          // troll beserking
                                     * (1 + calculatedStats.hasteFromHeroism)         // heroism
                                     * (1 + calculatedStats.hasteFromProcs);          // procs


            calculatedStats.hasteDynamicTotal = totalDynamicHaste;
            calculatedStats.hasteEffectsTotal = (totalStaticHaste * totalDynamicHaste) - 1;

            // Now we have the haste, we can calculate steady shot cast time,
            // then rebuild other various stats.
            // (future enhancement: we only really need to rebuild steady shot)
            calculatedStats.steadyShot.cooldown = 2 * (1 / (totalStaticHaste * totalDynamicHaste));
            if (options.useRotationTest)
            {
                calculatedStats.priorityRotation.initializeTimings();
                calculatedStats.priorityRotation.recalculateRatios();
                calculatedStats.priorityRotation.calculateFrequencySums();
            }
            else
            {
                calculatedStats.priorityRotation.initializeTimings();
                calculatedStats.priorityRotation.calculateFrequencies();
                calculatedStats.priorityRotation.calculateFrequencySums();
            }

            double autoShotSpeed = rangedWeaponSpeed / (totalStaticHaste * totalDynamicHaste);

            #endregion

            // hits
            #region August 2998 Hit-related Debuffs

            double targetDebuffsHit = 0; // Buffs!F77

            double targetDebuffsCrit = statsBuffs.PhysicalCrit; // Buffs!L77

            calculatedStats.targetDebuffsCrit = targetDebuffsCrit;

            #endregion
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

            // Heroic Presence
            calculatedStats.hitFromBuffs = character.Race == CharacterRace.Draenei ? 0.01 : statsBuffs.SpellHit;

            calculatedStats.hitFromTargetDebuffs = targetDebuffsHit;

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

            if (options.selectedAspect == Aspect.Hawk || options.selectedAspect == Aspect.Dragonhawk)
            {
                if (character.HunterTalents.ImprovedAspectOfTheHawk > 0)
                {
                    double quickShotsProcChance = 0.1;

                    double quickShotsSpeed = autoShotSpeed / (1 + calculatedStats.quickShotsEffect);

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

                    double quickShotsUptime;

                    if (options.useRotationTest)
                    {
                        quickShotsUptime = rotationTest.IAotHUptime;
                    }
                    else
                    {
                        quickShotsUptime = quickShotsProcChance > 0 ? quickShotsAverageChainQuick / (quickShotsAverageChainQuick + quickShotsAverageChainSlow) : 0;
                    }

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

            #endregion

            // base stats
            #region Agility

            // We need to re-calculate this now that we can figure out
            // trinket effects correctly

            double agilityFromProcs = 0;

            // Darkmoon Card: Greatness
            // Technically you can wear multiple cards (for each stat)
            double darkmoonGreatnessCount = 0;
            if (IsWearingTrinket(character, 44253)) darkmoonGreatnessCount++; // Agi
            if (IsWearingTrinket(character, 44255)) darkmoonGreatnessCount++; // Int
            if (IsWearingTrinket(character, 42987)) darkmoonGreatnessCount++; // Str
            if (IsWearingTrinket(character, 44254)) darkmoonGreatnessCount++; // Spi
            if (darkmoonGreatnessCount > 0)
            {
                double greatnessCardTimePer = totalShotsPerSecond > 0 ? 1 / totalShotsPerSecond / hitChance : 0; // T62
                double greatnessCardTimeBetween = greatnessCardTimePer > 0 ? 1 / 0.35 * greatnessCardTimePer + 45 : 0; // T63
                double greatnessCardUptime = greatnessCardTimeBetween > 0 ? 15 / greatnessCardTimeBetween : 0; // T65
                double greatnessCardEffect = 300 * greatnessCardUptime;

                agilityFromProcs += darkmoonGreatnessCount * greatnessCardEffect;
            }

            // Death's Choice / Death's Verdict
            // (you can only have one, since they are Alliance-only and Horde-only)
            if (IsWearingTrinket(character, 47303) || IsWearingTrinket(character, 47115))
            {
                double deathsChoiceTimePer = totalShotsPerSecond > 0 ? 1 / totalShotsPerSecond / hitChance : 0; // T138
                double deathsChoiceTimeBetween = deathsChoiceTimePer > 0 ? 1 / 0.35 * deathsChoiceTimePer + 45 : 0; // T139
                double deathsChoiceUptime = deathsChoiceTimeBetween > 0 ? 15 / deathsChoiceTimeBetween : 0; // T141
                agilityFromProcs += 450 * deathsChoiceUptime;
            }

            // Agility
            double agilityMultiplier = statsBaseGear.BonusAgilityMultiplier + statsBuffs.BonusAgilityMultiplier;

            double agilityRaceTalentAdjusted = Math.Floor(statsRace.Agility * (1 + character.HunterTalents.LightningReflexes * 0.03));
            double agi_part_1 = Math.Round((statsBaseGear.Agility + statsBuffs.Agility + agilityFromProcs) * (1 + agilityMultiplier) * (1 + statsTalents.BonusAgilityMultiplier));
            double agi_part_2 = Math.Round(statsRace.Agility * character.HunterTalents.HuntingParty * 0.01);
            double agi_part_3 = agilityRaceTalentAdjusted * (1 + agilityMultiplier);
            double totalAgility = agi_part_1 + agi_part_2 + agi_part_3;
            calculatedStats.BasicStats.Agility = (float)totalAgility;

            // Armor
            double armorFromGear = (statsBaseGear.Armor + statsBuffs.Armor + statsRace.Armor + statsBuffs.BonusArmor) * (1 + statsBuffs.BonusArmorMultiplier);
            double armorFromAgility = totalAgility * 2.0;
            double armorThickHideAdjust = 1 + character.HunterTalents.ThickHide * 0.0333;
            calculatedStats.BasicStats.Armor = (float)(armorFromGear * armorThickHideAdjust + armorFromAgility);

            #endregion

            // crits
            #region August 2009 Crit Chance

            calculatedStats.critBase = HunterRatings.BASE_CRIT_PERCENT;

            calculatedStats.critFromAgi = totalAgility / (HunterRatings.AGILITY_PER_CRIT * 100);

            calculatedStats.BasicStats.CritRating = statsBaseGear.CritRating + statsBuffs.CritRating + statsBaseGear.RangedCritRating;
            calculatedStats.critFromRating = (calculatedStats.BasicStats.CritRating / HunterRatings.CRIT_RATING_PER_PERCENT) / 100;

            double critProcRating = 0;

            // DK Anguish
            if (IsWearingTrinket(character, 38212))
            {
                double dkAnguishCooldown = 45;
                double dkAnguishProcChance = 0.1; // Q41
                double dkAnguishMaxStack = 10; // Q42
                double dkAnguishCritPerStack = 15; // Q43
                double dkAnguishMaxCrit = dkAnguishCritPerStack * dkAnguishMaxStack; // Q44
                double dkAnguishSecondsPerShot = totalShotsPerSecond > 0 ? 1 / totalShotsPerSecond / hitChance : 0; // Q45
                double dkAnguishTimeForBuff = 20; // Q46
                double dkAnguishTimeToMax = dkAnguishMaxStack * dkAnguishSecondsPerShot; // Q47
                double dkAnguishAverageCritDuring = dkAnguishTimeToMax < 20 
                                                    ? ((dkAnguishTimeToMax * dkAnguishMaxCrit * 0.5) + ((dkAnguishTimeForBuff - dkAnguishTimeToMax) * dkAnguishMaxCrit)) / 20
                                                    : Math.Floor(dkAnguishTimeForBuff / dkAnguishSecondsPerShot) * dkAnguishCritPerStack * 0.5; // Q48
                double dkAnguishTimeBetweenProcs = dkAnguishSecondsPerShot > 0 ? dkAnguishCooldown + ((1 / dkAnguishProcChance) * dkAnguishSecondsPerShot) : 0; ; // Q49
                double dkAnguishUptime = dkAnguishTimeBetweenProcs > 0 ? dkAnguishTimeForBuff / dkAnguishTimeBetweenProcs : 0; // Q50
                critProcRating += dkAnguishUptime * dkAnguishAverageCritDuring;
            }

            // Dark Matter
            if (IsWearingTrinket(character, 46038))
            {
                double darkMatterTimePerShot = totalShotsPerSecond > 0 ? 1 / totalShotsPerSecond / hitChance : 0;
                double darkMatterTimeBetween = darkMatterTimePerShot > 0 ? 1 / 0.15 * darkMatterTimePerShot + 45 : 0;
                critProcRating += 612 * CalcUptime(10, darkMatterTimeBetween, options);
            }

            calculatedStats.critFromProcRating = (critProcRating / HunterRatings.CRIT_RATING_PER_PERCENT) / 100;

            // Simple talents
            calculatedStats.critFromLethalShots = character.HunterTalents.LethalShots * 0.01;
            calculatedStats.critFromKillerInstincts = character.HunterTalents.KillerInstinct * 0.01;
            calculatedStats.critFromMasterMarksman = character.HunterTalents.MasterMarksman * 0.01;

            // Master Tactician
            double masterTacticianProcChance = 0.02 * character.HunterTalents.MasterTactician;
            double masterTacticianShotsIn8Seconds = totalShotsPerSecond * 8 * hitChance;
            double masterTacticianCritBonus = (1 - (Math.Pow(1 - 0.1, masterTacticianShotsIn8Seconds))) * masterTacticianProcChance;
            calculatedStats.critFromMasterTactician = masterTacticianCritBonus;

            // Crit From target debuffs
            calculatedStats.critFromBuffs = targetDebuffsCrit;

            // Crit Depression
            double critdepression = (levelDifference > 2) ? 0.03 + (levelDifference * 0.006) : (levelDifference * 5 * 0.04) / 100;
            calculatedStats.critFromDepression = 0 - critdepression;

            double critHitPercent = 0 // FinalCrit
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
            // to-maybe-do: add rift stalker set bonus
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
            #region August 2009 Spell Crit

            double spellCritFromBase = 0.05;
            double spellCritFromIntellect = calculatedStats.BasicStats.Intellect / (HunterRatings.INTELLECT_PER_SPELL_CRIT * 100);

            double spellCritTotal = spellCritFromBase
                                  + spellCritFromIntellect
                                  + calculatedStats.critFromRating
                                  + calculatedStats.critFromProcRating; // SpellCrit

            #endregion

            // target debuffs
            #region Target Debuffs

            double targetDebuffsAP = 0; // Buffs!E77

            // The pet debuffs deal with stacking correctly themselves
            double targetDebuffsArmor = 1 - (1 - calculatedStats.petArmorDebuffs)
                                          * (1 - statsBuffs.ArmorPenetration); // Buffs!G77

            double targetDebuffsMP5JudgmentOfWisdom = 0;
            if (statsBuffs.ManaRestoreFromBaseManaPerHit > 0)
            {
                // Note: we have to multiply the chance by 2 because it's stored in Buff.cs
                // as 0.01. this is because other models are not doing this calculation :)
                double jowPPM = 15; // E95
                double jowAutosPM = autoShotsPerSecond > 0 ? 60 / (1 / autoShotsPerSecond) : 0; // E96
                double jowSpecialsPM = specialShotsPerSecond > 0 ? 60 / (1 / specialShotsPerSecond) : 0; // E97
                double jowActualPPM = jowAutosPM > 0 ? (jowAutosPM + jowSpecialsPM) / jowAutosPM * jowPPM : 0; // E98
                double jowAvgShotTime = jowAutosPM + jowSpecialsPM > 0 ? 60 / (jowAutosPM + jowSpecialsPM) : 0; // E99
                double jowProcChance = jowAvgShotTime * jowActualPPM / 60; // E100
                double jowTimeToProc = jowProcChance > 0 ? jowAvgShotTime / jowProcChance : 0; // E101
                double jowManaGained = statsRace.Mana * statsBuffs.ManaRestoreFromBaseManaPerHit * 2; // E102
                double jowMPSGained = jowTimeToProc > 0 ? jowManaGained / jowTimeToProc : 0; // E103
                targetDebuffsMP5JudgmentOfWisdom = jowTimeToProc > 0 ? jowManaGained / jowTimeToProc * 5 : 0; // E104
            }
            double targetDebuffsMP5 = targetDebuffsMP5JudgmentOfWisdom; // Buffs!H77

            double targetDebuffsFire = statsBuffs.BonusFireDamageMultiplier; // Buffs!I77
            double targetDebuffsArcane = statsBuffs.BonusArcaneDamageMultiplier; // Buffs!J77
            double targetDebuffsNature = statsBuffs.BonusNatureDamageMultiplier; // Buffs!K77
            double targetDebuffsShadow = statsBuffs.BonusShadowDamageMultiplier;

            double targetDebuffsPetDamage = statsBuffs.BonusPhysicalDamageMultiplier;

            calculatedStats.targetDebuffsArmor = 1 - targetDebuffsArmor;
            calculatedStats.targetDebuffsNature = 1 + targetDebuffsNature;
            calculatedStats.targetDebuffsPetDamage = 1 + targetDebuffsPetDamage;

            #endregion

            // pet - part 1
            #region Pet MPS/Timing Calculations

            // this first block needs to run before the mana adjustments code,
            // since kill command effects mana usage.

            float baseMana = statsRace.Mana;
            calculatedStats.baseMana = statsRace.Mana;

            calculatedStats.pet.calculateTimings();

            #endregion

            // mana consumption
            #region August 2009 Mana Adjustments

            double efficiencyManaAdjust = 1 - (character.HunterTalents.Efficiency * 0.03);

            double thrillOfTheHuntManaAdjust = 1 - (calculatedStats.priorityRotation.critsCompositeSum * 0.4 * (character.HunterTalents.ThrillOfTheHunt / 3));

            double masterMarksmanManaAdjust = 1 - (character.HunterTalents.MasterMarksman * 0.05);

            double glyphOfArcaneShotManaAdjust = 1;
            if (calculatedStats.priorityRotation.containsShot(Shots.SerpentSting)
                || calculatedStats.priorityRotation.containsShot(Shots.ScorpidSting))
            {
                glyphOfArcaneShotManaAdjust = character.HunterTalents.GlyphOfArcaneShot ? 0.8 : 1;
            }

            double resourcefullnessManaAdjust = 1 - character.HunterTalents.Resourcefulness * 0.2;

            // Improved Steady Shot

            double ISSAimedShotManaAdjust = 1;
            double ISSArcaneShotManaAdjust = 1;
            double ISSChimeraShotManaAdjust = 1;

            double ISSChimeraShotDamageAdjust = 1;
            double ISSArcaneShotDamageAdjust = 1;
            double ISSAimedShotDamageAdjust = 1;

            double ISSProcChance = 0.05 * character.HunterTalents.ImprovedSteadyShot;
            if (ISSProcChance > 0)
            {                
                if (options.useRotationTest)
                {
                    ISSChimeraShotDamageAdjust = 1 + rotationTest.ISSChimeraUptime * 0.15;
                    ISSArcaneShotDamageAdjust = 1 + rotationTest.ISSArcaneUptime * 0.15;
                    ISSAimedShotDamageAdjust = 1 + rotationTest.ISSAimedUptime * 0.15;

                    ISSChimeraShotManaAdjust = 1 - rotationTest.ISSChimeraUptime * 0.2;
                    ISSArcaneShotManaAdjust = 1 - rotationTest.ISSArcaneUptime * 0.2;
                    ISSAimedShotManaAdjust = 1 - rotationTest.ISSAimedUptime * 0.2;
                }
                else
                {
                    double ISSRealProcChance = 0; // N120
                    if (calculatedStats.steadyShot.freq > 0)
                    {
                        double ISSSteadyFreq = calculatedStats.steadyShot.freq;
                        double ISSOtherFreq = calculatedStats.arcaneShot.freq
                                            + calculatedStats.chimeraShot.freq
                                            + calculatedStats.aimedShot.freq;

                        ISSRealProcChance = 1 - Math.Pow(1 - ISSProcChance, ISSOtherFreq / ISSSteadyFreq);
                    }
                    double ISSProcFreqChimera = ISSRealProcChance > 0 ? calculatedStats.chimeraShot.freq / ISSRealProcChance : 0; // N121
                    double ISSProcFreqArcane = ISSRealProcChance > 0 ? calculatedStats.arcaneShot.freq / ISSRealProcChance : 0; // N122
                    double ISSProcFreqAimed = ISSRealProcChance > 0 ? calculatedStats.aimedShot.freq / ISSRealProcChance : 0; // N123

                    double ISSProcFreqSumInverse = (ISSProcFreqChimera > 0 ? 1 / ISSProcFreqChimera : 0)
                                                 + (ISSProcFreqArcane > 0 ? 1 / ISSProcFreqArcane : 0)
                                                 + (ISSProcFreqAimed > 0 ? 1 / ISSProcFreqAimed : 0);
                    double ISSProcFreqCombined = ISSProcFreqSumInverse > 0 ? 1 / ISSProcFreqSumInverse : 0; // N124

                    ISSChimeraShotDamageAdjust = ISSProcFreqChimera > 0 ? 1 + ISSRealProcChance * ISSProcFreqCombined / ISSProcFreqChimera * 0.15 : 1;
                    ISSArcaneShotDamageAdjust = ISSProcFreqArcane > 0 ? 1 + ISSRealProcChance * ISSProcFreqCombined / ISSProcFreqArcane * 0.15 : 1;
                    ISSAimedShotDamageAdjust = ISSProcFreqAimed > 0 ? 1 + ISSRealProcChance * ISSProcFreqCombined / ISSProcFreqAimed * 0.15 : 1;

                    ISSChimeraShotManaAdjust = ISSProcFreqChimera > 0 ? 1 - ISSRealProcChance * ISSProcFreqCombined / ISSProcFreqChimera * 0.2 : 1;
                    ISSArcaneShotManaAdjust = ISSProcFreqArcane > 0 ? 1 - ISSRealProcChance * ISSProcFreqCombined / ISSProcFreqArcane * 0.2 : 1;
                    ISSAimedShotManaAdjust = ISSProcFreqAimed > 0 ? 1 - ISSRealProcChance * ISSProcFreqCombined / ISSProcFreqAimed * 0.2 : 1;
                }
            }

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
            calculatedStats.scorpidSting.mana = (baseMana * 0.11) * efficiencyManaAdjust;
            calculatedStats.viperSting.mana = (baseMana * 0.08) * efficiencyManaAdjust;
            calculatedStats.immolationTrap.mana = (baseMana * 0.13) * resourcefullnessManaAdjust;
            calculatedStats.rapidFire.mana = (baseMana * 0.03);

            calculatedStats.priorityRotation.calculateRotationMPS();

            #endregion
            #region August 2009 Mana Regen

            calculatedStats.manaRegenGearBuffs = (statsBaseGear.Mp5 + statsBuffs.Mp5) / 5;

            // Viper Regen if viper is up 100%
            calculatedStats.manaRegenViper = 0;
            if (options.selectedAspect == Aspect.Viper)
            {
                double viperGlyphAdjust = character.HunterTalents.GlyphOfAspectOfTheViper ? 1.1 : 1;
                double viperRegenShots = calculatedStats.BasicStats.Mana * rangedWeaponSpeed / 100 * totalShotsPerSecond * viperGlyphAdjust;
                double viperRegenPassive = calculatedStats.BasicStats.Mana * 0.04 / 3;
                calculatedStats.manaRegenViper = viperRegenShots + viperRegenPassive;
            }

            // Roar of Recovery - calculated in the pet model
            // calculatedStats.manaRegenRoarOfRecovery = 0;
            
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

            // Invigoration - this is being calculated in the pet model
            //calculatedStats.manaRegenInvigoration = 0;

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
            calculatedStats.manaRegenTargetDebuffs = targetDebuffsMP5 / 5;

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
            if (IsWearingTrinket(character, 35751)) manaHasAlchemistStone = true; // Assassin's Alchemist Stone
            if (IsWearingTrinket(character, 44324)) manaHasAlchemistStone = true; // Mighty Alchemist's Stone

            double manaRegenFromPotion = manaFromPotion / options.duration * (manaHasAlchemistStone ? 1.4 : 1.0);

            double beastialWrathUptime = calculatedStats.beastialWrath.freq > 0 ? calculatedStats.beastialWrath.duration / calculatedStats.beastialWrath.freq : 0;

            double beastWithinManaBenefit = beastialWrathUptime * 0.2;
            double manaExpenditureSpecial = calculatedStats.petKillCommandMPS * (1 - beastWithinManaBenefit);

            double manaExpenditure = calculatedStats.priorityRotation.MPS + manaExpenditureSpecial;

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

            double aspectUptimeBeast = options.useBeastDuringBeastialWrath ? beastialWrathUptime : 0;

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
            calculatedStats.aspectBonusAPBeast = beastAPBonus;

            #endregion

            // damage
            #region August 2009 Ranged Attack Power

            calculatedStats.apFromBase = 0.0 + HunterRatings.CHAR_LEVEL * 2;
            calculatedStats.apFromAgil = 0.0 + totalAgility - 10;
            calculatedStats.apFromCarefulAim = Math.Floor((character.HunterTalents.CarefulAim / 3) * (calculatedStats.BasicStats.Intellect));
            calculatedStats.apFromHunterVsWild = Math.Floor((character.HunterTalents.HunterVsWild * 0.1) * (calculatedStats.BasicStats.Stamina));
            calculatedStats.apFromGear = 0.0 + calculatedStats.BasicStats.AttackPower;

            // Darkmoon Card: Crusade
            if (IsWearingTrinket(character, 31856)) calculatedStats.apFromGear += 120;

            calculatedStats.apFromBloodFury = 0;
            if (character.Race == CharacterRace.Orc && calculatedStats.bloodFury.freq > 0)
            {
                calculatedStats.apFromBloodFury = (4 * HunterRatings.CHAR_LEVEL) + 2;
                calculatedStats.apFromBloodFury *= CalcUptime(calculatedStats.bloodFury.duration, calculatedStats.bloodFury.freq, options);
            }

            // Aspect of the Hawk
            calculatedStats.apFromAspectOfTheHawk = 0;
            if (options.selectedAspect == Aspect.Hawk || options.selectedAspect == Aspect.Dragonhawk)
            {
                calculatedStats.apFromAspectOfTheHawk = 300 * aspectUptimeHawk;
            }

            // Aspect Mastery
            calculatedStats.apFromAspectMastery = 0;
            if (character.HunterTalents.AspectMastery > 0)
            {
                calculatedStats.apFromAspectMastery = calculatedStats.apFromAspectOfTheHawk * 0.3 * aspectUptimeHawk;
            }

            // Furious Howl was calculated earlier by the pet model
            //calculatedStats.apFromFuriousHowl = 0;

            // Expose Weakness
            double exposeWeaknessShotsPerSecond = crittingShotsPerSecond;
            double exposeWeaknessCritChance = calculatedStats.priorityRotation.critsCompositeSum;
            double exposeWeaknessAgility = totalAgility * 0.25;
            double exposeWeaknessPercent = 0;
            if (character.HunterTalents.ExposeWeakness == 1) exposeWeaknessPercent = 0.33;
            if (character.HunterTalents.ExposeWeakness == 2) exposeWeaknessPercent = 0.66;
            if (character.HunterTalents.ExposeWeakness == 3) exposeWeaknessPercent = 1;
            double exposeWeaknessUptime = 1 - Math.Pow(1 - (exposeWeaknessPercent * exposeWeaknessCritChance), 7 * exposeWeaknessShotsPerSecond);

            calculatedStats.apFromExposeWeakness = exposeWeaknessUptime * exposeWeaknessAgility;

            // CallOfTheWild - this is calculated in the pet model
            //calculatedStats.apFromCallOfTheWild = 0;

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


            calculatedStats.apFromDebuffs = targetDebuffsAP;

            // debuffs may contain hunters mark too - only count the debuff if it's worth more
            // than our mark is
            double apFromHuntersMarkDebuff = 0;
            if (character.ActiveBuffsContains("Hunter's Mark")) apFromHuntersMarkDebuff += 500;
            if (character.ActiveBuffsContains("Improved Hunter's Mark")) apFromHuntersMarkDebuff += 150;
            calculatedStats.apFromGear -= apFromHuntersMarkDebuff;

            if (apFromHuntersMarkDebuff > calculatedStats.apFromHuntersMark)
            {
                // the hunters mark buff (not ours) is better than ours.
                // add the difference as a target debuff
                calculatedStats.apFromDebuffs += apFromHuntersMarkDebuff - calculatedStats.apFromHuntersMark;
            }

                    
            calculatedStats.apFromProc = 0;

            double crittingTriggersPerSecond = options.emulateSpreadsheetBugs // still an issue in 91b
                                             ? crittingShotsPerSecond * critHitPercent
                                             : crittingShotsPerSecond * critHitPercent * hitChance;

            // Mirror of Truth
            if (IsWearingTrinket(character, 40684))
            {
                calculatedStats.apFromProc += 1000 * CalcTrinketUptime(10, 45, 0.1, crittingTriggersPerSecond);
            }

            // Anvil of Titans
            if (IsWearingTrinket(character, 44914))
            {
                calculatedStats.apFromProc += 1000 * CalcTrinketUptime(10, 45, 0.1, totalShotsPerSecond * hitChance);
            }

            // Swordguard Embroidery
            if (character.BackEnchant != null && character.BackEnchant.Id == 3730)
            {
                if (options.emulateSpreadsheetBugs) // still an issue in 91b
                {
                    calculatedStats.apFromProc += 400 * CalcTrinketUptime(15, 45, 0.5, totalShotsPerSecond * critHitPercent);
                }
                else
                {
                    calculatedStats.apFromProc += 400 * CalcTrinketUptime(15, 45, 0.5, totalShotsPerSecond * hitChance);
                }
            }

            // Pyrite Infuser
            if (IsWearingTrinket(character, 45286))
            {
                calculatedStats.apFromProc += 1234 * CalcTrinketUptime(10, 45, 0.1, crittingTriggersPerSecond);
            }

            // Blood of the Old God
            if (IsWearingTrinket(character, 45522))
            {
                calculatedStats.apFromProc += 1284 * CalcTrinketUptime(10, 45, 0.1, crittingTriggersPerSecond);
            }

            // Fury of the Five Flights            
            if (IsWearingTrinket(character, 40431))
            {
                if (totalShotsPerSecond > 0)
                {
                    calculatedStats.apFromProc += (3040 * (1 / totalShotsPerSecond) / options.duration + 320 * (options.duration - 1 / totalShotsPerSecond * 20)) / options.duration;
                }                
            }

            // Tier-8 4-set bonus
            if (character.ActiveBuffsContains("Scourgestalker Battlegear 4 Piece Bonus"))
            {
                calculatedStats.apFromProc += 600 * CalcTrinketUptime(15, 45, 0.1, calculatedStats.steadyShot.freq > 0 ? 1 / calculatedStats.steadyShot.freq : 0);
            }

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
                + calculatedStats.apFromProc;

            // used for hunter calculations
            calculatedStats.apTotal = calculatedStats.apSelfBuffed
                + calculatedStats.apFromExposeWeakness
                + calculatedStats.apFromDebuffs
                + calculatedStats.apFromHuntersMark;

            // apply scaling
            calculatedStats.apTotal *= apScalingFactor;
            calculatedStats.apSelfBuffed *= apScalingFactor;

            double RAP = calculatedStats.apTotal;

            #endregion
            #region August 2009 Armor Penetration

            // Armor Penetration & Debuffs
            double targetArmorSubtotal = options.TargetArmor * calculatedStats.targetDebuffsArmor;
            double arpGearRating = calculatedStats.BasicStats.ArmorPenetrationRating;

            // ArPen from On-Proc trinkets
            double arpOnProcRating = 0;

            double arpProcCap = 100 * HunterRatings.ARP_RATING_PER_PERCENT - arpGearRating;

            // Grim Toll
            if (IsWearingTrinket(character, 40256))
            {
                double grimTollTimePerShot = totalShotsPerSecond > 0 ? 1 / totalShotsPerSecond / hitChance : 0; // T12
                double grimTollTimeBetween = grimTollTimePerShot > 0 ? 1 / 0.15 * grimTollTimePerShot + 45 : 0; // T13
                double grimTollUptime = grimTollTimeBetween > 0 ? 10 / grimTollTimeBetween : 0;

                arpOnProcRating += Math.Min(arpProcCap, 612) * grimTollUptime;
            }

            // Mjolnir Runestone
            if (IsWearingTrinket(character, 45931))
            {
                double mjolnirRunestoneTimePerShot = totalShotsPerSecond > 0 ? 1 / totalShotsPerSecond / hitChance : 0; // T121
                double mjolnirRunestoneTimeBetween = mjolnirRunestoneTimePerShot > 0 ? 1 / 0.15 * mjolnirRunestoneTimePerShot + 45 : 0; // T122
                double mjolnirRunestoneUptime = mjolnirRunestoneTimeBetween > 0 ? 10 / mjolnirRunestoneTimeBetween : 0;

                arpOnProcRating += Math.Min(arpProcCap, 665) * mjolnirRunestoneUptime;
            }

            // Incisor Fragment
            if (IsWearingTrinket(character, 37723))
            {
                arpOnProcRating += 291.0 * 20 / 120;
            }

            double arpTotal = arpGearRating + arpOnProcRating;

            double arpPercentReduction = arpTotal / HunterRatings.ARP_RATING_PER_PERCENT / 100;
            if (arpPercentReduction > 1) arpPercentReduction = 1;

            double arpEffectCap = (targetArmorSubtotal + (400 + 85 * character.Level + 4.5 * 85 * (character.Level - 59))) / 3;
            if (arpEffectCap > targetArmorSubtotal) arpEffectCap = targetArmorSubtotal;

            double targetArmorRemoved = arpEffectCap * arpPercentReduction;

            double effectiveArmor = targetArmorSubtotal - targetArmorRemoved;
            double armorReduction = effectiveArmor / (effectiveArmor - 22167.5 + (467.5 * character.Level));

            double armorReductionDamageAdjust = 1 - armorReduction;

            #endregion
            #region August 2009 Damage Adjustments

            //Partial Resists
            double averageResist = (options.TargetLevel - 80) * 0.02;
            double resist10 = 5 * averageResist;
            double resist20 = 2.5 * averageResist;
            double partialResistDamageAdjust = 1 - (resist10 * 0.1 + resist20 * 0.1);

            //Beast Within
            double beastWithinDamageAdjust = 1;
            if (calculatedStats.beastialWrath.freq > 0)
            {
                beastWithinDamageAdjust = 1 + (0.1 * beastialWrathUptime);
            }            

            //Focused Fire
            double focusedFireDamageAdjust = 1 + 0.01 * character.HunterTalents.FocusedFire;

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

            //Ferocious Inspiration (calculated by pet model)
            double ferociousInspirationDamageAdjust = calculatedStats.ferociousInspirationDamageAdjust;
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
            double targetPhysicalDebuffsDamageAdjust = (1 + statsBuffs.DamageTakenMultiplier)
                                                     * (1 + statsBuffs.BonusPhysicalDamageMultiplier);

            //Barrage
            double barrageDamageAdjust = 1 + 0.04 * character.HunterTalents.Barrage;

            //Sniper Training
            double sniperTrainingDamageAdjust = 1 + 0.02 * character.HunterTalents.SniperTraining;

            //Improve Stings
            double improvedStingsDamageAdjust = 1 + 0.1 * character.HunterTalents.ImprovedStings;

            //Steady Shot Glyph
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

            double baseCritDamage = (1 + mortalShotsCritDamage) * metaGemCritDamage; // CriticalHitDamage
            double specialCritDamage = (1 + mortalShotsCritDamage + markedForDeathCritDamage) * metaGemCritDamage; // SpecialCritDamage

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

            double damageFromRAP = RAP / 14 * rangedWeaponSpeed;
            double damageFromRAPNormalized = RAP / 14 * 2.8;

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
           
            #endregion
            #region August 2009 Wild Quiver

            calculatedStats.WildQuiverDPS = 0;
            if (character.HunterTalents.WildQuiver > 0)
            {
                double wildQuiverProcChance = character.HunterTalents.WildQuiver * 0.04;
                double wildQuiverProcFrequency = (autoShotSpeed / wildQuiverProcChance);
                double wildQuiverDamageNormal = 0.8 * (rangedWeaponDamage + statsBaseGear.WeaponDamage + damageFromRAP);
                double wildQuiverDamageAdjust = talentDamageAdjust * partialResistDamageAdjust * (1 + targetDebuffsNature);

                double wildQuiverDamageReal = CalcEffectiveDamage(
                                                wildQuiverDamageNormal,
                                                hitChance,
                                                critHitPercent,
                                                1,
                                                wildQuiverDamageAdjust
                                              );

                calculatedStats.WildQuiverDPS = wildQuiverDamageReal / wildQuiverProcFrequency;
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
            // to-maybe-do: Gronnstalker set bonus
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

            #endregion
            #region August 2009 Serpent Sting

            // base_damage = 1210 + (0.2 * RAP)
            double serpentStingDamageBase = Math.Round(1210 + (RAP * 0.2), 1);

            // T9 2-piece bonus
            double serpentStingT9CritAdjust = 1;
            if (character.ActiveBuffsContains("Windrunner's Pursuit 2 Piece Bonus"))
            {
                serpentStingT9CritAdjust = 1 + baseCritDamage * critHitPercent;
            }

            // damage_adjust = (sting_talent_adjusts ~ noxious stings) * improved_stings * improved_tracking
            //                  + partial_resists * tier-8_2-piece_bonus * target_nature_debuffs * 100%_noxious_stings
            double serpentStingDamageAdjust = focusedFireDamageAdjust
                                                * beastWithinDamageAdjust
                                                * sancRetributionAuraDamageAdjust
                                                * blackArrowAuraDamageAdjust
                                                * ferociousInspirationDamageAdjust
                                                * noxiousStingsSerpentDamageAdjust
                                                * improvedStingsDamageAdjust
                                                * improvedTrackingDamageAdjust
                                                * partialResistDamageAdjust
                                                * serpentStingT9CritAdjust
                                                * (1 + targetDebuffsNature);

            // T8 2-piece bonus
            serpentStingDamageAdjust += statsBuffs.BonusSerpentStingDamage;

            double serpentStingTicks = calculatedStats.serpentSting.duration / 3;
            double serpentStingDamagePerTick = Math.Round(serpentStingDamageBase * serpentStingDamageAdjust / 5, 1);
            double serpentStingDamageReal = serpentStingDamagePerTick * serpentStingTicks;

            calculatedStats.serpentSting.type = Shots.SerpentSting;
            calculatedStats.serpentSting.damage = serpentStingDamageReal;

            #endregion
            #region August 2009 Aimed Shot

            // base_damage = normalized_shot + 408 (but ammo is not normalized!)
            double aimedShotDamageNormal = (rangedWeaponDamage + rangedAmmoDamage + statsBaseGear.WeaponDamage + damageFromRAPNormalized) + 408;

            // crit_damage = 1 + mortal_shots + gem_crit + marked_for_death
            double aimedShotCritAdjust = (1 + mortalShotsCritDamage + markedForDeathCritDamage) * metaGemCritDamage;

            // damage_adjust = talent_adjust * barrage_adjust * target_debuff_adjust * sniper_training_adjust * improved_ss_adjust
            double aimedShotDamageAdjust = talentDamageAdjust * barrageDamageAdjust * targetPhysicalDebuffsDamageAdjust
                                            * sniperTrainingDamageAdjust * ISSAimedShotDamageAdjust
                                            * armorReductionDamageAdjust;

            double aimedShotDamageReal = CalcEffectiveDamage(
                                            aimedShotDamageNormal,
                                            hitChance,
                                            aimedShotCrit,
                                            aimedShotCritAdjust,
                                            aimedShotDamageAdjust
                                          );

            calculatedStats.aimedShot.damage = aimedShotDamageReal;

            #endregion
            #region August 2009 Explosive Shot

            // base_damage = 425 + 14% of RAP
            double explosiveShotDamageNormal = 425 + (RAP * 0.14);

            // crit_damage = 1 + mortal_shots + gem-crit
            double explosiveShotCritAdjust = (1 + mortalShotsCritDamage) * metaGemCritDamage;

            // damage_adjust = talent_adjust * tnt * fire_debuffs * sinper_training * partial_resist
            double explosiveShotDamageAdjust = talentDamageAdjust * TNTDamageAdjust * sniperTrainingDamageAdjust
                                             * partialResistDamageAdjust * (1 + targetDebuffsFire);

            double explosiveShotDamageReal = CalcEffectiveDamage(
                                                explosiveShotDamageNormal,
                                                hitChance,
                                                explosiveShotCrit,
                                                explosiveShotCritAdjust,
                                                explosiveShotDamageAdjust
                                              );

            double explosiveShotDamagePerShot = explosiveShotDamageReal * 3;

            calculatedStats.explosiveShot.damage = explosiveShotDamagePerShot;

            #endregion
            #region August 2009 Chimera Shot

            // base_damage = normalized_autoshot * 125%
            double chimeraShotDamageNormal = autoShotDamageNormalized * 1.25;

            // crit for 'specials'
            double chimeraShotCritAdjust = (1 + mortalShotsCritDamage + markedForDeathCritDamage) * metaGemCritDamage;

            // damage_adjust = talent_adjust * nature_debuffs * ISS_cs_bonus * partial_resist
            double chimeraShotDamageAdjust = talentDamageAdjust * ISSChimeraShotDamageAdjust
                                           * partialResistDamageAdjust * (1 + targetDebuffsNature);

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
            double chimeraShotSerpentDamageAdjust = talentDamageAdjust * (1 + targetDebuffsNature);

            double chimeraShotSerpentDamageReal = CalcEffectiveDamage(
                                                    chimeraShotSerpentDamage,
                                                    hitChance,
                                                    critHitPercent,
                                                    chimeraShotSerpentCritAdjust,
                                                    chimeraShotSerpentDamageAdjust
                                                 );

            double chimeraShotDamageTotal = chimeraShotDamageReal + chimeraShotSerpentDamageReal;

            calculatedStats.chimeraShot.damage = chimeraShotDamageTotal;

            #endregion
            #region August 2009 Arcane Shot

            // base_damage = 492 + weapon_damage_gear + (RAP * 15%)
            double arcaneShotDamageNormal = 492 + statsBaseGear.WeaponDamage + (RAP * 0.15);

            double arcaneShotCritAdjust = (1 + mortalShotsCritDamage + markedForDeathCritDamage) * metaGemCritDamage;
            double arcaneShotDamageAdjust = talentDamageAdjust * partialResistDamageAdjust * improvedArcaneShotDamageAdjust
                                            * ferociousInspirationArcaneDamageAdjust * ISSArcaneShotDamageAdjust; // missing arcane_debuffs!

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
            double blackArrowDamageAdjust = partialResistDamageAdjust * focusedFireDamageAdjust * beastWithinDamageAdjust
                                          * sancRetributionAuraDamageAdjust * noxiousStingsDamageAdjust
                                          * ferociousInspirationDamageAdjust * improvedTrackingDamageAdjust
                                          * rangedWeaponSpecializationDamageAdjust * markedForDeathDamageAdjust
                                          * (sniperTrainingDamageAdjust + trapMasteryDamageAdjust + TNTDamageAdjust - 2)
                                          * blackArrowSelfDamageAdjust * (1 + targetDebuffsShadow);

            double blackArrowDamage = blackArrowDamageNormal * blackArrowDamageAdjust;

            calculatedStats.blackArrow.damage = blackArrowDamage;

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

            #endregion
            #region August 2009 Silencing Shot

            double silencingShotDamageNormal = (rangedWeaponDamage + rangedAmmoDamage + damageFromRAPNormalized) * 0.5;
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

            #endregion
            #region August 2009 Immolation Trap

            double immolationTrapDamage = 1885 + (0.1 * RAP);
            double immolationTrapDamageAdjust = (1 - targetDebuffsFire) * partialResistDamageAdjust * trapMasteryDamageAdjust
                                              * TNTDamageAdjust * talentDamageStingAdjust;
            double immolationTrapProjectedDamage = immolationTrapDamage * immolationTrapDamageAdjust;
            double immolationTrapDamagePerTick = immolationTrapProjectedDamage / (character.HunterTalents.GlyphOfImmolationTrap ? 2.5 : 5);
            double immolationTrapTicks = character.HunterTalents.GlyphOfImmolationTrap ? 3 : 5;

            calculatedStats.immolationTrap.damage = immolationTrapDamagePerTick * immolationTrapTicks;

            #endregion
            #region August 2009 Rapid Fire

            calculatedStats.rapidFire.damage = 0;

            #endregion


            #region August 2009 On-Proc DPS

            // Bandit's Insignia
            if (IsWearingTrinket(character, 40371))
            {
                double banditsInsigniaDamageAverage = ((1504 + 2256) / 2) * (1 + spellCritTotal) * (1 + targetDebuffsArcane)
                                                        * (1 - 0.17 + calculatedStats.hitFromRating);
                double banditsInsigniaTimePer = totalShotsPerSecond > 0 ? 1 / totalShotsPerSecond / hitChance : 0; // T54
                double banditsInsigniaTimeBetween = banditsInsigniaTimePer > 0 ? 1 / 0.15 * banditsInsigniaTimePer + 45 : 0;
                double banditsInsigniaDPS = banditsInsigniaTimeBetween > 0 ? banditsInsigniaDamageAverage / banditsInsigniaTimeBetween : 0;

                calculatedStats.OnProcDPS += banditsInsigniaDPS;
            }

            // Gnomish Lightning Generator
            if (IsWearingTrinket(character, 41121))
            {
                double gnomishLGDamage = ((1530 + 1870) / 2) * (1 + spellCritTotal) * (1 + targetDebuffsFire) * (1 - 0.17 + calculatedStats.hitFromRating);
                calculatedStats.OnProcDPS += gnomishLGDamage / 60;
            }

            // Darkmoon Card: Death
            if (IsWearingTrinket(character, 42990))
            {
                double cardDeathDamage = ((1750 + 2250) / 2) * (1 + critHitPercent); // Q66
                double cardDeathTimePerShot = totalShotsPerSecond > 0 ? 1 / totalShotsPerSecond / hitChance : 0; // Q69
                double cardDeathBetweenTime = cardDeathTimePerShot > 0 ? 1 / 0.35 * cardDeathTimePerShot + 45 : 0; // Q70

                calculatedStats.OnProcDPS += cardDeathBetweenTime > 0 ? cardDeathDamage / cardDeathBetweenTime : 0;                    
            }

            // Hand-Mounted Pyro Rocket
            if (character.HandsEnchant != null && character.HandsEnchant.Id == 3603)
            {
                double pyroRocketDamage = ((1654 + 2020) / 2) * (1 + spellCritTotal) * (1 + targetDebuffsFire) * (1 - 0.17 + calculatedStats.hitFromRating);
                calculatedStats.OnProcDPS += pyroRocketDamage / 45;
            }

            // Vestige of Haldor
            if (IsWearingTrinket(character, 37064))
            {
                double vestigeDamage = ((1024 + 1536) / 2) * (1 + critHitPercent); // T40
                double vestigeTimePerShot = totalShotsPerSecond > 0 ? 1 / totalShotsPerSecond / hitChance : 0; // T43
                double vestigeBetweenTime = vestigeTimePerShot > 0 ? 1 / 0.1 * vestigeTimePerShot + 45 : 0; // T44

                calculatedStats.OnProcDPS += vestigeBetweenTime > 0 ? vestigeDamage / vestigeBetweenTime : 0;
            }

            calculatedStats.OnProcDPS *= (1 - viperDamagePenalty);

            #endregion
            #region August 2009 Shot Rotation


            calculatedStats.priorityRotation.viperDamagePenalty = viperDamagePenalty;
            calculatedStats.priorityRotation.calculateRotationDPS();
            calculatedStats.CustomDPS = calculatedStats.priorityRotation.DPS;

            #endregion
            #region August 2009 Kill Shot Sub-20% Usage

            double killShotCurrentFreq = calculatedStats.killShot.freq;
            double killShotPossibleFreq = options.useRotationTest ? calculatedStats.killShot.freq : calculatedStats.killShot.start_freq;
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

            // Agility - This gets recalculated for trinkets later
            double agi_race_talent_adjusted = Math.Floor(statsRace.Agility * (1 + character.HunterTalents.LightningReflexes * 0.03));
            double agi_part_1 = Math.Round((statsBaseGear.Agility + statsBuffs.Agility) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier) * (1 + statsTalents.BonusAgilityMultiplier));
            double agi_part_2 = Math.Round(statsRace.Agility * character.HunterTalents.HuntingParty * 0.01);
            double agi_part_3 = Math.Round(agi_race_talent_adjusted * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier));
            statsTotal.Agility = (float)(agi_part_1 + agi_part_2 + agi_part_3);

            // Intellect
            double intellectFromGear = Math.Floor(statsGearEnchantsBuffs.Intellect * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier) * (1 + statsTalents.BonusIntellectMultiplier));
            double intellectFromRace = (statsRace.Intellect * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier));
            statsTotal.Intellect = (float)(intellectFromGear + intellectFromRace);

			statsTotal.Spirit = (statsRace.Spirit + statsGearEnchantsBuffs.Spirit);  // * (1 + statsTotal.BonusSpiritMultiplier);
			statsTotal.Resilience = statsRace.Resilience + statsGearEnchantsBuffs.Resilience;

            // Armor
            double armorFromGear = (statsGearEnchantsBuffs.Armor + statsRace.Armor + statsBuffs.BonusArmor) * (1 + statsBuffs.BonusArmorMultiplier);
            double armorFromAgility = statsTotal.Agility * 2.0;
            double armorThickHideAdjust = 1 + character.HunterTalents.ThickHide * 0.0333;
			statsTotal.Armor = (float)(armorFromGear * armorThickHideAdjust + armorFromAgility);

            statsTotal.Miss = 0.0f;
			statsTotal.ArmorPenetration = statsRace.ArmorPenetration + statsGearEnchantsBuffs.ArmorPenetration;
            statsTotal.ArmorPenetrationRating = statsRace.ArmorPenetrationRating + statsGearEnchantsBuffs.ArmorPenetrationRating;
			statsTotal.BloodlustProc = statsRace.BloodlustProc + statsGearEnchantsBuffs.BloodlustProc;
            statsTotal.BonusCritMultiplier = 0.0f; // ((1 + statsRace.BonusCritMultiplier) * (1 + statsGearEnchantsBuffs.BonusCritMultiplier)) - 1;

            // Crit Rating - we will recalculate this later when we convert some crit
            //               multiplier to crit rating for a spreadsheet bug
            statsTotal.CritRating = (float)Math.Floor(
                                               (double)statsRace.CritRating +                       
                                               (double)statsBaseGear.CritRating +                   // gear crit
                                               (double)statsBuffs.CritRating +                      // master of anatomy, etc
                                               (double)statsBuffs.RangedCritRating                  // crit from scopes
                                    );

            statsTotal.HasteRating = statsRace.HasteRating + statsGearEnchantsBuffs.HasteRating + statsGearEnchantsBuffs.RangedHasteRating;
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

            // The first 20 Stam = 20 Health, while each subsequent Stam = 10 Health, so Health = (Stam-18)*10
            // (20-(20/10)) = 18
            double healthFromBase = statsRace.Health;
            double healthFromStamina = (statsTotal.Stamina - 18) * 10;
            double healthFromGearBuffs = statsGearEnchantsBuffs.Health;
            double healthFromTalents = (healthFromBase + healthFromStamina + healthFromGearBuffs) * (character.HunterTalents.EnduranceTraining * 0.01);
            double healthSubTotal = healthFromBase + healthFromStamina + healthFromGearBuffs + healthFromTalents;
            double healthTaurenAdjust = character.Race == CharacterRace.Tauren ? 1.05 : 1;
            statsTotal.Health = (float)(healthSubTotal * healthTaurenAdjust);               

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

        public static double CalcUptime(double duration, double cooldown, CalculationOptionsHunter options)
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

        public static float addCumulativePercentage(float current, double new_chance)
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

        private static bool IsWearingRing(Character character, int ring_id)
        {
            if (character.Finger1 != null && character.Finger1.Id == ring_id) return true;
            if (character.Finger2 != null && character.Finger2.Id == ring_id) return true;
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

        private ComparisonCalculationHunter comparisonFromShotSpammedDPS(ShotData shot)
        {
            ComparisonCalculationHunter comp =  new ComparisonCalculationHunter();

            double shotWait = shot.duration > shot.cooldown ? shot.duration : shot.cooldown;
            float dps = shotWait > 0 ? (float)(shot.damage / shotWait) : 0;

            comp.Name = Enum.GetName(typeof(Shots), shot.type);
            comp.HunterDpsPoints = dps;
            comp.OverallPoints = dps;
            return comp;
        }

        private ComparisonCalculationHunter comparisonFromShotSpammedMPS(ShotData shot)
        {
            ComparisonCalculationHunter comp = new ComparisonCalculationHunter();

            double shotWait = shot.duration > shot.cooldown ? shot.duration : shot.cooldown;
            float mps = shotWait > 0 ? (float)(shot.mana / shotWait) : 0;

            comp.Name = Enum.GetName(typeof(Shots), shot.type);
            comp.SubPoints = new float[] { mps };
            comp.OverallPoints = mps;
            return comp;
        }

        private ComparisonCalculationHunter comparisonFromShotRotationDPS(ShotData shot)
        {
            ComparisonCalculationHunter comp = new ComparisonCalculationHunter();
            comp.Name = Enum.GetName(typeof(Shots), shot.type);
            comp.SubPoints = new float[] { (float)shot.dps };
            comp.OverallPoints = (float)shot.dps;
            return comp;
        }

        private ComparisonCalculationHunter comparisonFromShotRotationMPS(ShotData shot)
        {
            ComparisonCalculationHunter comp = new ComparisonCalculationHunter();
            comp.Name = Enum.GetName(typeof(Shots), shot.type);
            comp.SubPoints = new float[] { (float)shot.mps };
            comp.OverallPoints = (float)shot.mps;
            return comp;
        }

        private ComparisonCalculationHunter comparisonFromShotDPM(ShotData shot)
        {
            ComparisonCalculationHunter comp = new ComparisonCalculationHunter();

            float dpm = shot.mana > 0 ? (float)(shot.damage / shot.mana) : 0;

            comp.Name = Enum.GetName(typeof(Shots), shot.type);
            comp.SubPoints = new float[] { dpm };
            comp.OverallPoints = dpm;
            return comp;
        }

        private ComparisonCalculationHunter comparisonFromStat(Character character, CharacterCalculationsHunter calcBase, Stats stats, string label)
        {
            ComparisonCalculationHunter comp = new ComparisonCalculationHunter();

            CharacterCalculationsHunter calcStat = GetCharacterCalculations(character, new Item() { Stats = stats }) as CharacterCalculationsHunter;

            comp.Name = label;
            comp.HunterDpsPoints = calcStat.HunterDpsPoints - calcBase.HunterDpsPoints;
            comp.PetDpsPoints = calcStat.PetDpsPoints - calcBase.PetDpsPoints;
            comp.OverallPoints = calcStat.OverallPoints - calcBase.OverallPoints;

            return comp;
        }

        private ComparisonCalculationHunter comparisonFromDouble(string label, double value)
        {
            return new ComparisonCalculationHunter()
            {
                Name = label,
                SubPoints = new float[] { (float)value },
                OverallPoints = (float)value,
            };
        }

        private ComparisonCalculationHunter comparisonFromDoubles(string label, double value1, double value2)
        {
            return new ComparisonCalculationHunter()
            {
                Name = label,
                SubPoints = new float[] { (float)value1, (float)value2 },
                OverallPoints = (float)(value1 + value2),
            };
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
