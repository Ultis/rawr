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
                        "Base Stats:Armor Penetration",
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
                        /*@"Base Stats:MH Expertise*6.50% chance to dodge/parry for attacks
Weapon Mastery 0- 6.50%-0%=6.50%=214 Rating Cap
Weapon Mastery 1- 6.50%-1%=6.50%=181 Rating Cap
Weapon Mastery 2- 6.50%-2%=6.50%=148 Rating Cap

Don't forget your weapons used matched with races can affect these numbers.",
                    @"Base Stats:OH Expertise*6.50% chance to dodge/parry for attacks
Weapon Mastery 0- 6.50%-0%=6.50%=214 Rating Cap
Weapon Mastery 1- 6.50%-1%=6.50%=181 Rating Cap
Weapon Mastery 2- 6.50%-2%=6.50%=148 Rating Cap

Don't forget your weapons used matched with races can affect these numbers.",*/
                        //"Base Stats:Missed Attacks",

                        //"Buffed Stats:Base MH Crit",
                        //"Buffed Stats:Base OH Crit",
                        //"Buffed Stats:Boss Armor Reduction %",
                        //"Buffed Stats:Effective Damage Dealt",
                        
                        "DPS Breakdown (Fury):Bloodsurge",
                        "DPS Breakdown (Fury):Bloodthirst",
                        "DPS Breakdown (Fury):Whirlwind",
                        "DPS Breakdown (Arms):Mortal Strike",
                        "DPS Breakdown (Arms):Slam",
                        "DPS Breakdown (Arms):Rend",
                        "DPS Breakdown (Arms):Sudden Death",
                        "DPS Breakdown (Arms):Overpower",
                        "DPS Breakdown (Arms):Bladestorm",
                        "DPS Breakdown (Arms):Sword Spec",
                        "DPS Breakdown (General):Heroic Strike",
                        "DPS Breakdown (General):Deep Wounds",
                        "DPS Breakdown (General):White DPS",
                        "DPS Breakdown (General):Total DPS",
                      
                        "Rage Details:White DPS Rage",
                        "Rage Details:Other Rage",
                        "Rage Details:Free Rage",
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

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            CharacterCalculationsDPSWarr    calculatedStats = new CharacterCalculationsDPSWarr();
            CalculationOptionsDPSWarr       calcOpts        = character.CalculationOptions as CalculationOptionsDPSWarr;
            Stats                           stats           = GetCharacterStats(character, additionalItem);

            CombatFactors combatFactors = new CombatFactors(character, stats);
            WhiteAttacks whiteAttacks = new WhiteAttacks(character.WarriorTalents, stats, combatFactors);
            Skills skillAttacks = new Skills(character.WarriorTalents, stats, combatFactors, whiteAttacks);

            calculatedStats.BasicStats = stats;
            calculatedStats.TargetLevel = calcOpts.TargetLevel;
            // Attack Table
            calculatedStats.Miss = stats.Miss;
            calculatedStats.HitRating = stats.HitRating;
            calculatedStats.HitPercent = stats.HitRating * DPSWarr.HitRatingToHit;
            calculatedStats.ExpertiseRating = stats.ExpertiseRating;
            calculatedStats.Expertise = stats.ExpertiseRating * DPSWarr.ExpertiseRatingToExpertise;
            calculatedStats.AgilityCritBonus = stats.Agility * DPSWarr.AgilityToCrit / 100.0f;
            calculatedStats.CritRating = stats.CritRating;
            float stancecritbonus = 0.0f;
            if (calcOpts.FuryStance) { stancecritbonus = 3.0f; }
            calculatedStats.CritPercent = (stats.CritRating * DPSWarr.CritRatingToCrit + character.WarriorTalents.Cruelty + stancecritbonus) / 100.0f
                + calculatedStats.AgilityCritBonus;
            calculatedStats.MhCrit = combatFactors.MhCrit;// +stancecritbonus / 100.0f;
            calculatedStats.OhCrit = combatFactors.OhCrit;// +stancecritbonus / 100.0f;
            // Offensive
            float teethbonus = stats.Armor;
            teethbonus *= (float)character.WarriorTalents.ArmoredToTheTeeth;
            teethbonus /= 180.0f;
            calculatedStats.TeethBonus = (int)teethbonus;
            calculatedStats.ArmorPenetration = stats.ArmorPenetrationRating;
            calculatedStats.buffedArmorPenetration = combatFactors.DamageReduction * 100.0f;
            calculatedStats.HasteRating = stats.HasteRating;
            calculatedStats.HastePercent = stats.HasteRating * DPSWarr.HasteRatingToHaste / 100.0f;
            // DPS
            calculatedStats.WhiteDPS = whiteAttacks.CalcMhWhiteDPS() + whiteAttacks.CalcOhWhiteDPS();// + swordSpecDPS);
            calculatedStats.DeepWoundsDPS = skillAttacks.Deepwounds();
            calculatedStats.HeroicStrikeDPS = skillAttacks.HeroicStrike();
            calculatedStats.SlamDPS = skillAttacks.Slam();
            calculatedStats.RendDPS = skillAttacks.Rend();
            calculatedStats.MortalStrikeDPS = skillAttacks.MortalStrike();
            calculatedStats.OverpowerDPS = skillAttacks.Overpower();
            calculatedStats.SwordSpecDPS = skillAttacks.SwordSpec();
            calculatedStats.BladestormDPS = skillAttacks.BladeStorm();
            calculatedStats.BloodsurgeDPS = skillAttacks.Bloodsurge();
            calculatedStats.SuddenDeathDPS = skillAttacks.SuddenDeath();
            calculatedStats.BloodthirstDPS = skillAttacks.Bloodthirst();
            calculatedStats.WhirlwindDPS = skillAttacks.Whirlwind();
            // Neutral
            // Defensive
            calculatedStats.Armor = (int)stats.Armor;
            calculatedStats.damageReduc = combatFactors.DamageReduction;

            calculatedStats.WhiteRage = whiteAttacks.whiteRageGen();
            calculatedStats.OtherRage = skillAttacks.OtherRage();
            calculatedStats.FreeRage = skillAttacks.freeRage();

            calculatedStats.TotalDPS = whiteAttacks.CalcMhWhiteDPS() + whiteAttacks.CalcOhWhiteDPS() + skillAttacks.Bloodthirst() + skillAttacks.Whirlwind() +
                                       skillAttacks.HeroicStrike() + skillAttacks.Bloodsurge() + skillAttacks.Deepwounds() +
                                       skillAttacks.MortalStrike() + skillAttacks.SuddenDeath() + skillAttacks.Slam() + skillAttacks.Overpower() + skillAttacks.Rend() + skillAttacks.SwordSpec() + skillAttacks.BladeStorm();
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

        public override Stats GetItemStats(Character character, Item additionalItem)
        {
            Stats statsItems = base.GetItemStats(character,additionalItem);// new Stats();

            // Assuming a GCD every 1.5s
            float abilityPerSecond = 1.0f / 1.5f;
            // Assumes 15% miss rate as we don't know the stats here--need to find a better solution
            float hitRate = 1.0f - (28.0f - statsItems.HitRating * DPSWarr.HitRatingToHit) * 0.01f;
            
            //Mongoose
            if (character.MainHand != null && statsItems.MongooseProc > 0) {
                float procRate = 1.0f; // PPM
                float procDuration = 15.0f;
                float procPerSecond = (((procRate / 60.0f) * character.MainHand.Item.Speed) + ((procRate / 60.0f) * abilityPerSecond)) * hitRate;
                float procUptime = procDuration * procPerSecond;

                statsItems.Agility += 120.0f * procUptime;
                statsItems.HasteRating += (2.0f / DPSWarr.HasteRatingToHaste) * procUptime;
            }

            //Executioner
            if (character.MainHand != null && statsItems.ExecutionerProc > 0)
            {
                float procRate = 1.2f; // PPM
                float procDuration = 15.0f;
                float procPerSecond = (((procRate / 60.0f) * character.MainHand.Item.Speed) + ((procRate / 60.0f) * abilityPerSecond)) * hitRate;
                float procUptime = procDuration * procPerSecond;

                statsItems.ArmorPenetrationRating += 120.0f * procUptime;
            }
            return statsItems;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
            WarriorTalents talents = character.WarriorTalents;

            Stats statsRace = GetRaceStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsItems = GetItemStats(character, additionalItem);
            Stats statsTalents = new Stats()
            {
                Parry = talents.Deflection * 1.0f,
                PhysicalCrit = talents.Cruelty * 0.01f,
                Dodge = talents.Anticipation * 1.0f,
                Block = talents.ShieldSpecialization * 1.0f,
                BonusBlockValueMultiplier = talents.ShieldMastery * 0.15f,
                BonusDamageMultiplier = talents.OneHandedWeaponSpecialization * 0.02f,
                BonusStaminaMultiplier = talents.Vitality * 0.02f + talents.StrengthOfArms * 0.02f,
                BonusStrengthMultiplier = talents.Vitality * 0.02f + talents.StrengthOfArms * 0.02f,
                Expertise = talents.Vitality * 2.0f + talents.StrengthOfArms * 2.0f,
                BonusShieldSlamDamage = talents.GagOrder * 0.05f,
                DevastateCritIncrease = talents.SwordAndBoard * 0.05f,
                BaseArmorMultiplier = talents.Toughness * 0.02f,
                PhysicalHaste = talents.BloodFrenzy * 0.03f,
            };
            Stats statsGearEnchantsBuffs = statsItems + statsBuffs;
            Stats statsTotal = statsRace + statsItems + statsBuffs + statsTalents;


            float agiBase = (float)Math.Floor(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier));
            statsTotal.BaseAgility = agiBase;
            float agiBonus = (float)Math.Floor(statsGearEnchantsBuffs.Agility * (1 + statsRace.BonusAgilityMultiplier));
            float strBase = (float)Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float strBonus = (float)Math.Floor(statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float staBase = (float)Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier));
            float staBonus = (float)Math.Floor((statsGearEnchantsBuffs.Stamina + (float)calcOpts.ToughnessLvl) * (1 + statsRace.BonusStaminaMultiplier));
            statsTotal.Stamina = (float)Math.Floor((statsRace.Stamina + statsTalents.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Stamina += (float)Math.Floor((statsItems.Stamina + statsBuffs.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));

            statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier)) - 1;
            statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier) * (1 + 0.02f * character.WarriorTalents.StrengthOfArms) * (1 + character.WarriorTalents.ImprovedBerserkerStance * 0.04f)) - 1;
            statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;
            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;

            statsTotal.Agility = (float)Math.Floor(agiBase * (1f + statsTotal.BonusAgilityMultiplier)) + (float)Math.Floor(agiBonus * (1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Strength = (float)Math.Floor(strBase * (1f + statsTotal.BonusStrengthMultiplier)) + (float)Math.Floor(strBonus * (1 + statsTotal.BonusStrengthMultiplier));
            statsTotal.Stamina = (float)Math.Floor((staBase) * (1f + statsTotal.BonusStaminaMultiplier)) + (float)Math.Floor(staBonus * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + ((statsTotal.Stamina - staBase) * 10f))));

            statsTotal.Armor = statsGearEnchantsBuffs.Armor + statsTotal.Agility * 2;

            statsTotal.AttackPower = (statsTotal.Strength * 2 + statsRace.AttackPower) + statsGearEnchantsBuffs.AttackPower;
            statsTotal.AttackPower += (statsTotal.Armor / 180) * character.WarriorTalents.ArmoredToTheTeeth;
            statsTotal.AttackPower += statsTotal.AttackPower * statsTotal.BonusAttackPowerMultiplier;

            statsTotal.PhysicalHit = character.WarriorTalents.Precision;
            statsTotal.HitRating = statsGearEnchantsBuffs.HitRating;

            statsTotal.ExpertiseRating = statsGearEnchantsBuffs.ExpertiseRating;
            statsTotal.Expertise += 2 * character.WarriorTalents.StrengthOfArms;
            statsTotal.HasteRating = statsGearEnchantsBuffs.HasteRating;
            statsTotal.PhysicalHaste = statsGearEnchantsBuffs.PhysicalHaste;
            statsTotal.PhysicalHaste *= (1 + 0.03f * character.WarriorTalents.BloodFrenzy);

            statsTotal.ArmorPenetration = statsGearEnchantsBuffs.ArmorPenetration;
            statsTotal.ArmorPenetrationRating = statsGearEnchantsBuffs.ArmorPenetrationRating;

            statsTotal.CritRating = statsRace.CritRating + statsGearEnchantsBuffs.CritRating;
            statsTotal.PhysicalCrit = statsRace.PhysicalCrit;
            statsTotal.PhysicalCrit += statsTotal.Agility * DPSWarr.AgilityToCrit / 100;
            statsTotal.PhysicalCrit += character.WarriorTalents.Cruelty * 0.01f;
            statsTotal.PhysicalCrit += statsGearEnchantsBuffs.PhysicalCrit;

            statsTotal.BonusCritMultiplier = statsGearEnchantsBuffs.BonusCritMultiplier;

            statsTotal.BonusDamageMultiplier = statsGearEnchantsBuffs.BonusDamageMultiplier;
            statsTotal.BonusPhysicalDamageMultiplier = statsGearEnchantsBuffs.BonusPhysicalDamageMultiplier;

            statsTotal.WeaponDamage = statsGearEnchantsBuffs.WeaponDamage;

            statsTotal.BonusBleedDamageMultiplier = statsGearEnchantsBuffs.BonusBleedDamageMultiplier;
            statsTotal.BonusSlamDamage = statsGearEnchantsBuffs.BonusSlamDamage;
            statsTotal.DreadnaughtBonusRageProc = statsGearEnchantsBuffs.DreadnaughtBonusRageProc;

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
                        new Item() { Stats = new Stats() { Strength = 10 } },
                        new Item() { Stats = new Stats() { Agility = 10 } },
                        new Item() { Stats = new Stats() { AttackPower = 20 } },
                        new Item() { Stats = new Stats() { CritRating = 10 } },
                        new Item() { Stats = new Stats() { HitRating = 10 } },
                        new Item() { Stats = new Stats() { ExpertiseRating = 8.19f } },
                        new Item() { Stats = new Stats() { HasteRating = 10 } },
                        new Item() { Stats = new Stats() { ArmorPenetrationRating = 10 } },
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

        //Hide the ranged weapon enchants. None of them apply to melee damage at all.
        public override bool EnchantFitsInSlot(Enchant enchant, Character character, Item.ItemSlot slot)
        {
            if (character != null && (character.WarriorTalents != null && enchant != null))
            {
                return enchant.Slot == Item.ItemSlot.Ranged ? false : base.EnchantFitsInSlot(enchant,character,slot) || (character.WarriorTalents.TitansGrip == 1 && enchant.Slot == Item.ItemSlot.TwoHand && slot == Item.ItemSlot.OffHand);
            }
            return enchant.Slot == Item.ItemSlot.Ranged ? false : enchant.FitsInSlot(slot);
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats
            {
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
                WeaponDamage = stats.WeaponDamage,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                MongooseProc = stats.MongooseProc,
                MongooseProcAverage = stats.MongooseProcAverage,
                MongooseProcConstant = stats.MongooseProcConstant,
                ExecutionerProc = stats.ExecutionerProc,
                BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
                Armor = stats.Armor,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHaste = stats.PhysicalHaste,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                BonusSlamDamage = stats.BonusSlamDamage,
                DreadnaughtBonusRageProc = stats.DreadnaughtBonusRageProc,
                BerserkingProc = stats.BerserkingProc
            };
        }
        public override bool HasRelevantStats(Stats stats)
        {
            return (
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
                stats.WeaponDamage +
                stats.BonusCritMultiplier +
                stats.BonusDamageMultiplier +
                stats.MongooseProc +
                stats.MongooseProcAverage +
                stats.MongooseProcConstant +
                stats.ExecutionerProc +
                stats.BonusBleedDamageMultiplier +
                stats.BonusCritChance +
                stats.Armor +
                stats.PhysicalHaste +
                stats.PhysicalCrit +
                stats.BonusPhysicalDamageMultiplier +
                stats.DreadnaughtBonusRageProc +
                stats.BonusSlamDamage +
                stats.BerserkingProc) != 0;
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
    public class DPSWarr {
        // Offensive
        public static readonly float StrengthToAP = 2.0f;
        public static readonly float AgilityToCrit = 1.0f / 62.5f;
        public static readonly float HitRatingToHit = 1.0f / 32.78998947f;
        public static readonly float CritRatingToCrit = 1.0f / 45.90598679f;
        public static readonly float CritToCritRating = 45.90598f; //14*82/52
        public static readonly float HasteRatingToHaste = 1.0f / 32.78998947f;
        public static readonly float ExpertiseRatingToExpertise = 1.0f / (32.78998947f / 4f);
        public static readonly float ExpertiseToDodgeParryReduction = 0.25f;
        public static readonly float ArPToArmorPenetration = 1.0f / 15.395298f;
        // Neutral
        public static readonly float StaminaToHP = 10.0f;
        // Defensive
        public static readonly float AgilityToArmor = 2.0f;
        public static readonly float AgilityToDodge = 1.0f / 73.52941176f;
        public static readonly float DodgeRatingToDodge = 1.0f / 39.34798813f;
        public static readonly float StrengthToBlockValue = 1.0f / 2.0f;
        public static readonly float DefenseRatingToDefense = 1.0f / 4.918498039f;
        public static readonly float DefenseToDodge = 0.04f;
        public static readonly float DefenseToBlock = 0.04f;
        public static readonly float DefenseToParry = 0.04f;
        public static readonly float DefenseToMiss = 0.04f;
        public static readonly float DefenseToCritReduction = 0.04f;
        public static readonly float DefenseToDazeReduction = 0.04f;
        public static readonly float ParryRatingToParry = 1.0f / 49.18498611f;
        public static readonly float BlockRatingToBlock = 1.0f / 16.39499474f;
        // PvP
        public static readonly float ResilienceRatingToCritReduction = 1.0f / 81.97497559f;
    }
}