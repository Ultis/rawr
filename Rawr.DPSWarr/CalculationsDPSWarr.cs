using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.DPSWarr
{
    [Rawr.Calculations.RawrModelInfo("DPSWarr", "Ability_Rogue_Ambush", Character.CharacterClass.Warrior)]
    public class CalculationsDPSWarr : CalculationsBase
    {
        public override List<GemmingTemplate> DefaultGemmingTemplates {
            get {
				////Relevant Gem IDs for DPSWarrs
				//Red
				int[] bold = { 39900, 39996, 40111, 42142 };

				//Purple
				int[] sovereign = { 39934, 40022, 40129 };

				//Orange
				int[] inscribed = { 39947, 40037, 40142 };

				//Meta
				int chaotic = 41285;

				return new List<GemmingTemplate>() {
					new GemmingTemplate() { Model = "DPSWarr", Group = "Uncommon", //Max Strength
						RedId = bold[0], YellowId = bold[0], BlueId = bold[0], PrismaticId = bold[0], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSWarr", Group = "Uncommon", //Strength
						RedId = bold[0], YellowId = inscribed[0], BlueId = sovereign[0], PrismaticId = bold[0], MetaId = chaotic },
						
					new GemmingTemplate() { Model = "DPSWarr", Group = "Rare", Enabled = true, //Max Strength
						RedId = bold[1], YellowId = bold[1], BlueId = bold[1], PrismaticId = bold[1], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSWarr", Group = "Rare", Enabled = true, //Strength
						RedId = bold[1], YellowId = inscribed[1], BlueId = sovereign[1], PrismaticId = bold[1], MetaId = chaotic },
						
					new GemmingTemplate() { Model = "DPSWarr", Group = "Epic", //Max Strength
						RedId = bold[2], YellowId = bold[2], BlueId = bold[2], PrismaticId = bold[2], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSWarr", Group = "Epic", //Strength
						RedId = bold[2], YellowId = inscribed[2], BlueId = sovereign[2], PrismaticId = bold[2], MetaId = chaotic },
						
					new GemmingTemplate() { Model = "DPSWarr", Group = "Jeweler", //Max Strength
						RedId = bold[3], YellowId = bold[3], BlueId = bold[3], PrismaticId = bold[3], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSWarr", Group = "Jeweler", //Strength
						RedId = bold[2], YellowId = bold[3], BlueId = bold[3], PrismaticId = bold[2], MetaId = chaotic },
				};
            }
        }

        #region Variables and Properties
        public CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelDPSWarr();
                }
                return _calculationOptionsPanel;
            }
        }

		private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null) {
                    _characterDisplayCalculationLabels = new string[] {
    					"Base Stats:Health",
    					"Base Stats:Stamina",
                        "Base Stats:Armor",
                        "Base Stats:Strength*1 STR = 2 AP x Kings Buff",
                        "Base Stats:Attack Power*2 AP is just 2 AP, go for STR",
                        "Base Stats:Agility",
                        "Base Stats:Haste",
                        "Base Stats:Crit",
                        @"Base Stats:Armor Penetration*Rating to Cap with buffs/debuffs applied
1232-None
1074-Sndr
1039-Sndr+FF
0916-Sndr+FF+Arms
0731-Sndr+FF+Arms+Mace",
                        "Base Stats:Damage Reduction*Should be exact opposite of ArP in Percentage (%)",
                        @"Base Stats:Hit Rating*8.00% chance to miss base for Yellow Attacks
Precision 0- 8%-0%=8%=263 Rating soft cap
Precision 1- 8%-1%=7%=230 Rating soft cap
Precision 2- 8%-2%=6%=197 Rating soft cap
Precision 3- 8%-3%=5%=164 Rating soft cap",
                        @"Base Stats:Expertise*6.50% chance to dodge/parry for attacks
Weapon Mastery 0- 6.50%-0%=6.50%=214 Rating Cap
Weapon Mastery 1- 6.50%-1%=6.50%=181 Rating Cap
Weapon Mastery 2- 6.50%-2%=6.50%=148 Rating Cap

Don't forget your weapons used matched with races can affect these numbers.",
                        
                        "DPS Breakdown (Fury):Bloodsurge*The First number is per second or per tick. The second number is the normal damage (factoring mitigation and hit/miss and crits)",
                        "DPS Breakdown (Fury):Bloodthirst",
                        "DPS Breakdown (Fury):Whirlwind",
                        "DPS Breakdown (Arms):Mortal Strike",
                        "DPS Breakdown (Arms):Slam",
                        "DPS Breakdown (Arms):Rend",
                        "DPS Breakdown (Arms):Sudden Death*If this number is zero, it most likely means that using the execute spamming isn't increasing your dps, so don't use it in your rotation.",
                        "DPS Breakdown (Arms):Overpower",
                        "DPS Breakdown (Arms):Bladestorm",
                        "DPS Breakdown (Arms):Sword Spec",
                        "DPS Breakdown (General):Heroic Strike",
                        "DPS Breakdown (General):Deep Wounds",
                        "DPS Breakdown (General):White DPS",
                        "DPS Breakdown (General):Total DPS",
                      
                        "Rage Details:Generated White DPS Rage",
                        "Rage Details:Generated Other Rage",
                        "Rage Details:Ability's Rage Used (BT)",
                        "Rage Details:Ability's Rage Used (WW)",
                        "Rage Details:Ability's Rage Used (MS)",
                        "Rage Details:Ability's Rage Used (OP)",
                        "Rage Details:Ability's Rage Used (SD)",
                        "Rage Details:Ability's Rage Used (SL)",
                        "Rage Details:Ability's Rage Used (BS)",
                        "Rage Details:Ability's Rage Used (BLS)",
                        "Rage Details:Ability's Rage Used (SW)",
                        "Rage Details:Ability's Rage Used (RND)",
                        "Rage Details:Available Free Rage",
                    };
                }
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
                    "Health",
                    "Haste Rating",
                    "Expertise Rating",
                    "Hit Rating",
					"Crit Rating",
					"Agility",
					"Attack Power",
					"Armor Penetration",
					"Armor Penetration Rating",
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
                {
                    _customChartNames = new string[] {
                        "Item Budget",
					};
                }
                return _customChartNames;
            }
        }

        private Dictionary<string, Color> _subPointNameColors = null;
        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null){
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("DPS", Color.Red);
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
                    _relevantItemTypes = new List<Item.ItemType>(new[] {
                        Item.ItemType.None,
                        Item.ItemType.Leather,
                        Item.ItemType.Mail,
                        Item.ItemType.Plate,
                        Item.ItemType.Bow,
                        Item.ItemType.Crossbow,
                        Item.ItemType.Gun,
                        Item.ItemType.Thrown,
                        Item.ItemType.Dagger,
                        Item.ItemType.FistWeapon,
                        Item.ItemType.OneHandMace,
                        Item.ItemType.OneHandSword,
                        Item.ItemType.OneHandAxe,
                        Item.ItemType.Staff,
                        Item.ItemType.Polearm,
                        Item.ItemType.TwoHandMace,
                        Item.ItemType.TwoHandSword,
                        Item.ItemType.TwoHandAxe
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Warrior; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationsDPSWarr(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsDPSWarr(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            XmlSerializer s = new XmlSerializer(typeof(CalculationOptionsDPSWarr));
            StringReader sr = new StringReader(xml);
            CalculationOptionsDPSWarr calcOpts = s.Deserialize(sr) as CalculationOptionsDPSWarr;
            return calcOpts;
        }
        #endregion

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations) {
            CharacterCalculationsDPSWarr    calculatedStats = new CharacterCalculationsDPSWarr();
            CalculationOptionsDPSWarr       calcOpts        = character.CalculationOptions as CalculationOptionsDPSWarr;
            Stats                           stats           = GetCharacterStats(character, additionalItem);

            CombatFactors combatFactors = new CombatFactors(character, stats);
            WhiteAttacks whiteAttacks = new WhiteAttacks(character.WarriorTalents, stats, combatFactors);
            Skills skillAttacks = new Skills(character,character.WarriorTalents, stats, combatFactors, whiteAttacks);
            Stats statsRace = GetRaceStats(character);

            calculatedStats.BasicStats = stats;
            calculatedStats.SkillAttacks = skillAttacks;
            calculatedStats.TargetLevel = calcOpts.TargetLevel;
            calculatedStats.BaseHealth = statsRace.Health;
            // Attack Table
            calculatedStats.Miss = stats.Miss;
            calculatedStats.HitRating = stats.HitRating;
            calculatedStats.HitPercent = StatConversion.GetHitFromRating(stats.HitRating);
            calculatedStats.ExpertiseRating = stats.ExpertiseRating;
            calculatedStats.Expertise = StatConversion.GetExpertiseFromRating(stats.ExpertiseRating,character.Class);
            calculatedStats.MhExpertise = combatFactors.MhExpertise;
            calculatedStats.OhExpertise = combatFactors.OhExpertise;
            calculatedStats.AgilityCritBonus = StatConversion.GetCritFromAgility(stats.Agility,character.Class);
            calculatedStats.CritRating = stats.CritRating;
			calculatedStats.CritPercent = StatConversion.GetCritFromRating(stats.CritRating) + stats.PhysicalCrit
				- (0.006f * (calcOpts.TargetLevel - 80) + (calcOpts.TargetLevel == 83 ? 0.03f : 0f));
            calculatedStats.MhCrit = combatFactors.MhCrit;// +stancecritbonus / 100.0f;
            calculatedStats.OhCrit = combatFactors.OhCrit;// +stancecritbonus / 100.0f;
            // Offensive
            float teethbonus = stats.Armor;
            teethbonus *= (float)character.WarriorTalents.ArmoredToTheTeeth;
            teethbonus /= 180.0f;
            calculatedStats.TeethBonus = (int)teethbonus;
            calculatedStats.ArmorPenetrationMaceSpec = ((character.MainHand != null && character.MainHand.Type == Item.ItemType.TwoHandMace) ? character.WarriorTalents.MaceSpecialization * 0.03f : 0.00f);
            calculatedStats.ArmorPenetrationStance = ((!calcOpts.FuryStance) ? 0.10f : 0.00f);
            calculatedStats.ArmorPenetrationRating = stats.ArmorPenetrationRating;
            calculatedStats.ArmorPenetrationRating2Perc = StatConversion.GetArmorPenetrationFromRating(stats.ArmorPenetrationRating);
            calculatedStats.ArmorPenetration = calculatedStats.ArmorPenetrationMaceSpec
                + calculatedStats.ArmorPenetrationStance
                + calculatedStats.ArmorPenetrationRating2Perc;
            calculatedStats.HasteRating = stats.HasteRating;
            calculatedStats.HastePercent = StatConversion.GetHasteFromRating(stats.HasteRating,Character.CharacterClass.Warrior);
            // DPS

            calculatedStats.WhiteDPSMH = whiteAttacks.CalcMhWhiteDPS();
            calculatedStats.WhiteDPSOH = whiteAttacks.CalcOhWhiteDPS();
            calculatedStats.WhiteDPS   = calculatedStats.WhiteDPSMH + calculatedStats.WhiteDPSOH;
            calculatedStats.HS = new Skills.HeroicStrike(character, stats, combatFactors, whiteAttacks);//calculatedStats.HeroicStrikeDPS = skillAttacks.HeroicStrike();
            calculatedStats.DW = new Skills.DeepWounds(character, stats, combatFactors, whiteAttacks);//calculatedStats.DeepWoundsDPS = skillAttacks.Deepwounds();
            calculatedStats.SL = new Skills.Slam(character, stats, combatFactors, whiteAttacks);
            calculatedStats.RND = new Skills.Rend(character, stats, combatFactors, whiteAttacks);//calculatedStats.RendDPS = skillAttacks.Rend();
            calculatedStats.MS = new Skills.Mortalstrike(character, stats, combatFactors, whiteAttacks);
            calculatedStats.OP = new Skills.OverPower(character, stats, combatFactors, whiteAttacks); //calculatedStats.OverpowerDPS = skillAttacks.Overpower();
            calculatedStats.SS = new Skills.Swordspec(character, stats, combatFactors, whiteAttacks); //calculatedStats.SwordSpecDPS = skillAttacks.SwordSpec();
            calculatedStats.SW = new Skills.SweepingStrikes(character, stats, combatFactors, whiteAttacks);
            calculatedStats.BLS = new Skills.Bladestorm(character, stats, combatFactors, whiteAttacks); //calculatedStats.BladestormDPS = skillAttacks.BladeStorm();
            calculatedStats.BS = new Skills.BloodSurge(character, stats, combatFactors, whiteAttacks);
            calculatedStats.SD = new Skills.Suddendeath(character, stats, combatFactors, whiteAttacks); //calculatedStats.SuddenDeathDPS = skillAttacks.SuddenDeath();
            calculatedStats.BT = new Skills.BloodThirst(character, stats, combatFactors, whiteAttacks);
            calculatedStats.WW = new Skills.WhirlWind(character, stats, combatFactors, whiteAttacks);
            // Neutral
            // Defensive
            calculatedStats.Armor = (int)stats.Armor;
            calculatedStats.damageReduc = combatFactors.DamageReduction;

            calculatedStats.WhiteRage = whiteAttacks.whiteRageGen();
            calculatedStats.OtherRage = skillAttacks.OtherRage();
            calculatedStats.FreeRage = skillAttacks.freeRage();

            calculatedStats.TotalDPS = whiteAttacks.CalcMhWhiteDPS() + whiteAttacks.CalcOhWhiteDPS() + calculatedStats.BT.GetDPS()
                + calculatedStats.WW.GetDPS() + calculatedStats.HS.GetDPS() + calculatedStats.BS.GetDPS()
                + calculatedStats.DW.GetDPS() + calculatedStats.MS.GetDPS() + calculatedStats.SD.GetDPS()
                + calculatedStats.SL.GetDPS() + calculatedStats.OP.GetDPS() + calculatedStats.RND.GetDPS()
                + calculatedStats.SS.GetDPS() + calculatedStats.BLS.GetDPS();
            calculatedStats.OverallPoints = calculatedStats.TotalDPS;

            return calculatedStats;
        }

        #region Warrior Race Stats
        private static float[,] BaseWarriorRaceStats = new float[,] {
							//	Strength,	Agility,	Stamina
            /*Human*/		{	174f,	    113f,	    159f,   },
            /*Orc*/			{	178f,		110f,		162f,	},
            /*Dwarf*/		{	176f,	    109f,	    162f,   },
			/*Night Elf*/	{	142f,	    101f,	    132f,   },
	        /*Undead*/		{	174f,	    111f,	    160f,   },
			/*Tauren*/		{	180f,		108f,		162f,	},
	        /*Gnome*/		{	170f,	    116f,	    159f,   },
			/*Troll*/		{	175f,	    115f,	    160f,   },	
			/*BloodElf*/	{	0f,		    0f,		    0f,	    },
			/*Draenei*/		{	176f,		110f,		159f,	},
		};
        private Stats GetRaceStats(Character character)
        {
            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.Human:
                    statsRace = new Stats()
                    {
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[0, 0],
                        Agility = (float)BaseWarriorRaceStats[0, 1],
                        Stamina = (float)BaseWarriorRaceStats[0, 2],

                        AttackPower = 220f,
                        Dodge = 3.4636f,
                        Miss = 0.05f,
                        Parry = 5f,
                        PhysicalCrit = 0.03186f,
                    };
                    break;
                case Character.CharacterRace.Orc:
                    statsRace = new Stats()
                    {
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[1, 0],
                        Agility = (float)BaseWarriorRaceStats[1, 1],
                        Stamina = (float)BaseWarriorRaceStats[1, 2],

                        AttackPower = 220f,
                        Dodge = 3.4636f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.03186f,
                    };
                    break;
                case Character.CharacterRace.Dwarf:
                    statsRace = new Stats()
                    {
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[2, 0],
                        Agility = (float)BaseWarriorRaceStats[2, 1],
                        Stamina = (float)BaseWarriorRaceStats[2, 2],

                        AttackPower = 220f,
                        Dodge = 3.4636f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.03186f,
                    };
                    break;
                case Character.CharacterRace.NightElf:
                    statsRace = new Stats()
                    {
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[3, 0],
                        Agility = (float)BaseWarriorRaceStats[3, 1],
                        Stamina = (float)BaseWarriorRaceStats[3, 2],

                        AttackPower = 220f,
                        Dodge = 3.4636f,
                        Miss = 0.05f + 0.02f,
                        Parry = 5f,
                        PhysicalCrit = 0.03186f,
                    };
                    break;
                case Character.CharacterRace.Undead:
                    statsRace = new Stats()
                    {
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[4, 0],
                        Agility = (float)BaseWarriorRaceStats[4, 1],
                        Stamina = (float)BaseWarriorRaceStats[4, 2],

                        AttackPower = 220f,
                        Dodge = 3.4636f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.03186f,
                    };
                    break;
                case Character.CharacterRace.Tauren:
                    statsRace = new Stats()
                    {
                        Health = 8338f,
                        Strength = (float)BaseWarriorRaceStats[5, 0],
                        Agility = (float)BaseWarriorRaceStats[5, 1],
                        Stamina = (float)BaseWarriorRaceStats[5, 2],

                        AttackPower = 220f,
                        Dodge = 3.4636f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.03186f,
                    };
                    break;
                case Character.CharacterRace.Gnome:
                    statsRace = new Stats()
                    {
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[6, 0],
                        Agility = (float)BaseWarriorRaceStats[6, 1],
                        Stamina = (float)BaseWarriorRaceStats[6, 2],

                        AttackPower = 220f,
                        Dodge = 3.4636f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.03186f,
                    };
                    break;
                case Character.CharacterRace.Troll:
                    statsRace = new Stats()
                    {
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[7, 0],
                        Agility = (float)BaseWarriorRaceStats[7, 1],
                        Stamina = (float)BaseWarriorRaceStats[7, 2],

                        AttackPower = 220f,
                        Dodge = 3.4636f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.03186f,
                    };
                    break;
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats()
                    {
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[9, 0],
                        Agility = (float)BaseWarriorRaceStats[9, 1],
                        Stamina = (float)BaseWarriorRaceStats[9, 2],

                        AttackPower = 220f,
                        PhysicalHit = 1f,
                        Dodge = 3.4636f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.03186f,
                    };
                    break;
                default:
                    statsRace = new Stats();
                    break;
            }
            return statsRace;
        }
        #endregion

        public override Stats GetItemStats(Character character, Item additionalItem) {
            Stats statsItems = base.GetItemStats(character,additionalItem);
            return statsItems;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem) {
            CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
            WarriorTalents talents = character.WarriorTalents;

            Stats statsRace = GetRaceStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsItems = GetItemStats(character, additionalItem);
            Stats statsTalents = new Stats()
            {
                //Parry = talents.Deflection * 1.0f,
                PhysicalCrit = talents.Cruelty * 0.01f + (calcOpts.FuryStance ? 0.03f : 0f),
                //Dodge = talents.Anticipation * 1.0f,
                //Block = talents.ShieldSpecialization * 1.0f,
                //BonusBlockValueMultiplier = talents.ShieldMastery * 0.15f,
                BonusDamageMultiplier = talents.OneHandedWeaponSpecialization * 0.02f,
                BonusStaminaMultiplier = talents.Vitality * 0.02f + talents.StrengthOfArms * 0.02f,
                BonusStrengthMultiplier = talents.Vitality * 0.02f + talents.StrengthOfArms * 0.02f,
                Expertise = talents.Vitality * 2.0f + talents.StrengthOfArms * 2.0f,
                //BonusShieldSlamDamage = talents.GagOrder * 0.05f,
                DevastateCritIncrease = talents.SwordAndBoard * 0.05f,
                BaseArmorMultiplier = talents.Toughness * 0.02f,
                PhysicalHaste = talents.BloodFrenzy * 0.03f,
                ArmorPenetration = ((character.MainHand != null && character.MainHand.Type == Item.ItemType.TwoHandMace) ? talents.MaceSpecialization * 0.03f : 0.00f),
                PhysicalHit = talents.Precision * 1.0f,
            };
            Stats statsGearEnchantsBuffs = statsItems + statsBuffs;
            Stats statsTotal = statsRace + statsItems + statsBuffs + statsTalents;

            float strBase = (float)Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float strBonus = (float)Math.Floor(statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier));
            //float staBase = (float)Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier));
            //float staBonus = (float)Math.Floor((statsGearEnchantsBuffs.Stamina + (float)calcOpts.ToughnessLvl) * (1 + statsRace.BonusStaminaMultiplier));
            //statsTotal.Stamina = (float)Math.Floor((statsRace.Stamina + statsTalents.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            //statsTotal.Stamina += (float)Math.Floor((statsItems.Stamina + statsBuffs.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));

            //statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier)) - 1;
            //statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier) * (1 + 0.02f * character.WarriorTalents.StrengthOfArms) * (1 + character.WarriorTalents.ImprovedBerserkerStance * ((calcOpts.FuryStance) ? 0.04f: 0.00f))) - 1;
            //statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;

            statsTotal.Strength = (float)Math.Floor(strBase * (1f + statsTotal.BonusStrengthMultiplier)) + (float)Math.Floor(strBonus * (1 + statsTotal.BonusStrengthMultiplier));
            //statsTotal.Stamina  = (float)Math.Floor(staBase * (1f + statsTotal.BonusStaminaMultiplier )) + (float)Math.Floor(staBonus * (1 + statsTotal.BonusStaminaMultiplier ));
            //statsTotal.Health   = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + (statsTotal.Stamina * 10f))));

            //statsTotal.Armor += statsTotal.Agility * 2;

            statsTotal.AttackPower = (statsTotal.Strength * 2 + statsRace.AttackPower) + statsGearEnchantsBuffs.AttackPower;
            statsTotal.AttackPower += (statsTotal.Armor / 180) * talents.ArmoredToTheTeeth;
            statsTotal.AttackPower += statsTotal.AttackPower * statsTotal.BonusAttackPowerMultiplier;

            //statsTotal.HitRating = statsGearEnchantsBuffs.HitRating;

            //statsTotal.ExpertiseRating = statsGearEnchantsBuffs.ExpertiseRating;

            //statsTotal.HasteRating = statsGearEnchantsBuffs.HasteRating;
            //statsTotal.PhysicalHaste = statsGearEnchantsBuffs.PhysicalHaste;
            //statsTotal.PhysicalHaste += statsTalents.PhysicalHaste;

            statsTotal.ArmorPenetration += ((!calcOpts.FuryStance) ? 0.10f : 0.00f);

            statsTotal.PhysicalCrit += StatConversion.GetCritFromAgility(statsTotal.Agility, character.Class);

            //statsTotal.BonusCritMultiplier += statsGearEnchantsBuffs.BonusCritMultiplier;

            //statsTotal.BonusDamageMultiplier += statsGearEnchantsBuffs.BonusDamageMultiplier;
            //statsTotal.BonusPhysicalDamageMultiplier += statsGearEnchantsBuffs.BonusPhysicalDamageMultiplier;

            //statsTotal.WeaponDamage = statsGearEnchantsBuffs.WeaponDamage;

            //statsTotal.BonusBleedDamageMultiplier = statsGearEnchantsBuffs.BonusBleedDamageMultiplier;
            //statsTotal.BonusSlamDamage = statsGearEnchantsBuffs.BonusSlamDamage;
            statsTotal.DreadnaughtBonusRageProc = statsGearEnchantsBuffs.DreadnaughtBonusRageProc;



            CombatFactors combatFactors = new CombatFactors(character, statsTotal);
            WhiteAttacks whiteAttacks = new WhiteAttacks(talents, statsTotal, combatFactors);

            Skills.Ability fake = new Skills.BloodThirst(character, statsTotal, combatFactors, whiteAttacks);
            // TODO: This is new and stolen from the Cat model per Astrylian and is supposed to handle all procs
            // such as Berserking, Mirror of Truth, Grim Toll, etc.

            float fightDuration = 600f;  //TODO: Assuming 10min fight for now
            float hasteBonus = StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, Character.CharacterClass.Warrior);
            hasteBonus = (1f + hasteBonus) * (1f + statsTotal.PhysicalHaste) * (1f + statsTotal.Bloodlust * 40f / fightDuration) - 1f;
            float meleeHitsPerSecond = 1f / 1.5f;
            if (character.MainHand != null && character.MainHand.Speed > 0f)
                meleeHitsPerSecond += (1f / character.MainHand.Speed) * (1f + hasteBonus);
            if (character.OffHand != null && character.OffHand.Speed > 0f)
                meleeHitsPerSecond += (1f / character.OffHand.Speed) * (1f + hasteBonus);
            float meleeHitInterval = 1f / meleeHitsPerSecond;
            
            
            float chanceCrit = StatConversion.GetCritFromRating(statsTotal.CritRating) + statsTotal.PhysicalCrit 
				- (0.006f * (calcOpts.TargetLevel - 80) + (calcOpts.TargetLevel == 83 ? 0.03f : 0f));


            Stats statsProcs = new Stats();
            foreach (SpecialEffect effect in statsTotal.SpecialEffects()) {
                switch (effect.Trigger) {
                    case Trigger.Use:
                        statsProcs += effect.GetAverageStats(0f, 1f, 1f, 600f/*fake.GetRotation()*//*calcOpts.Duration*/);
                        break;
                    case Trigger.MeleeHit: case Trigger.PhysicalHit:
                        statsProcs += effect.GetAverageStats(meleeHitInterval, 1f, 1f, 600f/*fake.GetRotation()*//*calcOpts.Duration*/);
                        break;
                    case Trigger.MeleeCrit: case Trigger.PhysicalCrit:
                        statsProcs += effect.GetAverageStats(meleeHitInterval, chanceCrit, 1f, 600f/*fake.GetRotation()*//*calcOpts.Duration*/);
                        break;
                    case Trigger.DoTTick:
                        statsProcs += effect.GetAverageStats(1.5f, 1f, 1f, 600f/*fake.GetRotation()*//*calcOpts.Duration*/);
                        break;
                    case Trigger.DamageDone:
                        statsProcs += effect.GetAverageStats(meleeHitInterval / 2f, 1f, 1f, 600f/*fake.GetRotation()*//*calcOpts.Duration*/);
                        break;
                }
            }

            statsProcs.Stamina = (float)Math.Floor(statsProcs.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
            statsProcs.Strength = (float)Math.Floor(statsProcs.Strength * (1f + statsTotal.BonusStrengthMultiplier));
            statsProcs.Agility = (float)Math.Floor(statsProcs.Agility * (1f + statsTotal.BonusAgilityMultiplier));
            statsProcs.AttackPower += statsProcs.Strength * 2f;
            statsProcs.AttackPower = (float)Math.Floor(statsProcs.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
            statsProcs.Health += (float)Math.Floor(statsProcs.Stamina * 10f);
            statsProcs.Armor += 2f * statsProcs.Agility;
            statsProcs.Armor = (float)Math.Floor(statsProcs.Armor * (1f + statsTotal.BonusArmorMultiplier));
            statsTotal += statsProcs;



            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            CharacterCalculationsDPSWarr baseCalc, calc;
            ComparisonCalculationBase comparison;
            float[] subPoints;

            switch (chartName)
            {
                case "Item Budget":
                    Item[] itemList = new Item[] {
                        new Item() { Stats = new Stats() { Strength = 10f } },
                        new Item() { Stats = new Stats() { Agility = 10f } },
                        new Item() { Stats = new Stats() { AttackPower = 20f } },
                        new Item() { Stats = new Stats() { CritRating = 10f } },
                        new Item() { Stats = new Stats() { HitRating = 10f } },
                        new Item() { Stats = new Stats() { ExpertiseRating = 8.19f } },
                        new Item() { Stats = new Stats() { HasteRating = 10f } },
                        new Item() { Stats = new Stats() { ArmorPenetrationRating = 10f } },
                    };
                    string[] statList = new string[] {
                        "Strength",
                        "Agility",
                        "Attack Power",
                        "Crit Rating",
                        "Hit Rating",
                        "Expertise Rating",
                        "Haste Rating",
                        "Armor Penetration Rating",
                    };

                    baseCalc = GetCharacterCalculations(character) as CharacterCalculationsDPSWarr;

                    float mod = 1;
                    for (int index = 0; index < itemList.Length; index++) {
                        calc = GetCharacterCalculations(character, itemList[index]) as CharacterCalculationsDPSWarr;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = statList[index];
                        comparison.Equipped = false;
                        comparison.OverallPoints = (calc.OverallPoints - baseCalc.OverallPoints) / 10;
                        subPoints = new float[calc.SubPoints.Length];
                        for (int i = 0; i < calc.SubPoints.Length; i++){
                            if (comparison.Name == "Strength"){
                                subPoints[i] = 1;
                            }else{
                                subPoints[i] = calc.SubPoints[i] - baseCalc.SubPoints[i];
                                subPoints[i] /= 10;
                                subPoints[i] /= mod;
                            }
                        }
                        comparison.SubPoints = subPoints;
                        if (comparison.Name == "Strength"){
                            mod = comparison.OverallPoints;
                            comparison.OverallPoints = 1;
                        }else if (comparison.Name == "Expertise Rating"){
                            comparison.OverallPoints /= mod;
                            comparison.OverallPoints /= 8.197f;
                        }else{
                            comparison.OverallPoints /= mod;
                        }

                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        public override bool EnchantFitsInSlot(Enchant enchant, Character character, Item.ItemSlot slot) {
            //Hide the ranged weapon enchants. None of them apply to melee damage at all.
            if (character != null && (character.WarriorTalents != null && enchant != null)) {
                return enchant.Slot == Item.ItemSlot.Ranged ? false : base.EnchantFitsInSlot(enchant,character,slot) || (character.WarriorTalents.TitansGrip == 1 && enchant.Slot == Item.ItemSlot.TwoHand && slot == Item.ItemSlot.OffHand);
            }
            return enchant.Slot == Item.ItemSlot.Ranged ? false : enchant.FitsInSlot(slot);
        }

        public override Stats GetRelevantStats(Stats stats)
        {
			Stats s = new Stats() {
                Stamina = stats.Stamina,
                Agility = stats.Agility,
                Strength = stats.Strength,
                AttackPower = stats.AttackPower,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                Health = stats.Health,
                CritRating = stats.CritRating,
                HitRating = stats.HitRating,
                HasteRating = stats.HasteRating,
                ExpertiseRating = stats.ExpertiseRating,
				ArmorPenetration = stats.ArmorPenetration,
				ArmorPenetrationRating = stats.ArmorPenetrationRating,
                WeaponDamage = stats.WeaponDamage,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                //MongooseProc = stats.MongooseProc,
                //MongooseProcAverage = stats.MongooseProcAverage,
                //MongooseProcConstant = stats.MongooseProcConstant,
                //ExecutionerProc = stats.ExecutionerProc,
                BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
                Armor = stats.Armor,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHaste = stats.PhysicalHaste,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                BonusSlamDamage = stats.BonusSlamDamage,
                DreadnaughtBonusRageProc = stats.DreadnaughtBonusRageProc,
                //BerserkingProc = stats.BerserkingProc
            };
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit
                || effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit || effect.Trigger == Trigger.DoTTick
                    || effect.Trigger == Trigger.DamageDone)
                {
                    if (HasRelevantStats(effect.Stats))
                    {
                        s.AddSpecialEffect(effect);
                    }
                }
            }
            return s;
        }
        public override bool HasRelevantStats(Stats stats) {
            bool relevant = (
                stats.Agility +
                stats.Strength +
                stats.BonusAgilityMultiplier +
                stats.BonusStrengthMultiplier +
                stats.AttackPower +
                stats.BonusAttackPowerMultiplier +
                stats.CritRating + stats.HitRating +
                stats.HasteRating +
                stats.ExpertiseRating +
                stats.ArmorPenetration +
                stats.ArmorPenetrationRating +
                stats.WeaponDamage +
                stats.BonusCritMultiplier +
                stats.BonusDamageMultiplier +
                //stats.MongooseProc +
                //stats.MongooseProcAverage +
                //stats.MongooseProcConstant +
                //stats.ExecutionerProc +
                stats.BonusBleedDamageMultiplier +
                stats.BonusCritChance +
                stats.Armor +
                stats.PhysicalHaste +
                stats.PhysicalCrit +
                stats.BonusPhysicalDamageMultiplier +
                stats.DreadnaughtBonusRageProc +
                stats.BonusSlamDamage +
                //stats.BerserkingProc
                // Copied from Cat
                stats.BloodlustProc +
                //stats.BladeWardProc +
                stats.DarkmoonCardDeathProc +
                stats.DreadnaughtBonusRageProc
                ) != 0;
            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit
                    || effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit)
                {
                    relevant |= HasRelevantStats(effect.Stats);
                    if (relevant) break;
                }
            }
            return relevant;
        }

        public override bool ItemFitsInSlot(Item item, Character character, Character.CharacterSlot slot) {
            if (item == null || character == null) {
                return false;
            } else if (character.WarriorTalents.TitansGrip == 1 && item.Type == Item.ItemType.Polearm) {
                return false;
            } else if (slot == Character.CharacterSlot.OffHand && item.Slot == Item.ItemSlot.TwoHand && character.WarriorTalents.TitansGrip == 1) {
                return true;
            } else if (slot == Character.CharacterSlot.OffHand && character.MainHand != null && character.MainHand.Slot == Item.ItemSlot.TwoHand) {
                return false;
            } else if (item.Type == Item.ItemType.Polearm && slot == Character.CharacterSlot.MainHand) {
                return true;
            } else {
                return base.ItemFitsInSlot(item, character, slot);
            }
        }
        public override bool IncludeOffHandInCalculations(Character character) {
            if (character.OffHand == null) { return false; }
            if (character.CurrentTalents is WarriorTalents) {
                WarriorTalents talents = (WarriorTalents)character.CurrentTalents;
                if (talents.TitansGrip > 0) {
                    return true;
                } else {// if (character.MainHand.Slot != Item.ItemSlot.TwoHand)
                    return base.IncludeOffHandInCalculations(character);
                }
            }
            return false;
        }
        public Stats GetBuffsStats(Character character) {
            var statsBuffs = GetBuffsStats(character.ActiveBuffs);

            //Mongoose
            if (character.MainHand != null && (character.MainHandEnchant != null && (character.MainHandEnchant.Id == 2673)))
            {
                statsBuffs.Agility += 120f * ((40f * (1f / (60f / character.MainHand.Item.Speed)) / 6f));
                statsBuffs.HasteRating += (15.76f * 2f) * ((40f * (1f / (60f / character.MainHand.Item.Speed)) / 6f));
            }
            if (character.OffHand != null && (character.OffHandEnchant != null && character.OffHandEnchant.Id == 2673))
            {
                statsBuffs.Agility += 120f * ((40f * (1f / (60f / character.OffHand.Item.Speed)) / 6f));
                statsBuffs.HasteRating += (15.76f * 2f) * ((40f * (1f / (60f / character.OffHand.Item.Speed)) / 6f));
            }

            //Executioner
			//if (character.MainHand != null && (character.MainHandEnchant != null && (character.MainHandEnchant.Id == 3225)))
			//{
			//    statsBuffs.ArmorPenetration += 840f * ((40f * (1f / (60f / character.MainHand.Item.Speed)) / 6f));
			//}

            return statsBuffs;
        }

        public void GetTalents(Character character) {
            CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.talents = character.WarriorTalents;
        }
    }
}
