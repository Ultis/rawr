using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    [System.ComponentModel.DisplayName("ProtWarr|Ability_Warrior_DefensiveStance")]
	public class CalculationsProtWarr : CalculationsBase
	{
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
					"Basic Stats:Expertise Rating",
					"Basic Stats:Haste Rating",
					"Basic Stats:Armor Penetration",
					"Basic Stats:Crit Rating",
					"Basic Stats:Weapon Damage",
					"Basic Stats:Dodge Rating",
                    "Basic Stats:Parry Rating",
                    "Basic Stats:Block Rating",
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
                    "Complex Stats:Block Value",
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
                    "% to be Crushed",
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
                        Item.ItemType.Shield
					});
				}
				return _relevantItemTypes;
			}
		}

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Warrior; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationProtWarr(); }
		public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsProtWarr(); }

		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsProtWarr));
			System.IO.StringReader reader = new System.IO.StringReader(xml);
			CalculationOptionsProtWarr calcOpts = serializer.Deserialize(reader) as CalculationOptionsProtWarr;
			return calcOpts;
		}

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            _cachedCharacter = character;
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;
            int targetLevel = calcOpts.TargetLevel;

            Stats stats = GetCharacterStats(character, additionalItem);
            float levelDifference = (targetLevel - 70f) * 0.2f;
            CharacterCalculationsProtWarr calculatedStats = new CharacterCalculationsProtWarr();
            calculatedStats.BasicStats = stats;
            calculatedStats.TargetLevel = targetLevel;
            calculatedStats.Miss = 5f + (((float)Math.Floor(stats.Defense + stats.DefenseRating * WarriorConversions.DefenseRatingToDefense)) * WarriorConversions.DefenseToMiss) +
                                   stats.Miss - levelDifference;
            calculatedStats.Dodge = Math.Min(100f - calculatedStats.Miss,
                                    stats.Agility * WarriorConversions.AgilityToDodge +
                                    stats.DodgeRating * WarriorConversions.DodgeRatingToDodge +
                                    (((float)Math.Floor(stats.Defense + stats.DefenseRating * WarriorConversions.DefenseRatingToDefense)) * WarriorConversions.DefenseToDodge) +
                                    stats.Dodge - levelDifference);
            calculatedStats.Parry = Math.Min(100f - calculatedStats.Miss - calculatedStats.Dodge,
                                    5f + stats.ParryRating * WarriorConversions.ParryRatingToParry +
                                    (((float)Math.Floor(stats.Defense + stats.DefenseRating * WarriorConversions.DefenseRatingToDefense)) * WarriorConversions.DefenseToParry) +
                                    stats.Parry - levelDifference);

            float block = 5f + stats.BlockRating * WarriorConversions.BlockRatingToBlock +
                                    (((float)Math.Floor(stats.Defense + stats.DefenseRating * WarriorConversions.DefenseRatingToDefense)) * WarriorConversions.DefenseToBlock) +
                                    stats.Block - levelDifference;
            calculatedStats.Block = Math.Min(100f - calculatedStats.Miss - calculatedStats.Dodge - calculatedStats.Parry, block);
            calculatedStats.BlockOverCap = ((block - calculatedStats.Block) > 0 ? (block - calculatedStats.Block) : 0.0f);
            calculatedStats.BlockValue = (float)Math.Floor((float)Math.Floor((stats.BlockValue * (1 + stats.BonusBlockValueMultiplier)) +
                                         ((float)Math.Floor(stats.Strength * WarriorConversions.StrengthToBlockValue))));
            calculatedStats.Mitigation = (stats.Armor / (stats.Armor - 22167.5f + (467.5f * targetLevel))) * 100f;
            calculatedStats.CappedMitigation = Math.Min(75f, calculatedStats.Mitigation);
            calculatedStats.DodgePlusMissPlusParry = calculatedStats.Dodge + calculatedStats.Miss + calculatedStats.Parry;
            calculatedStats.DodgePlusMissPlusParryPlusBlock = calculatedStats.Dodge + calculatedStats.Miss + calculatedStats.Parry + calculatedStats.Block;
            calculatedStats.CritReduction = (((float)Math.Floor(stats.DefenseRating * WarriorConversions.DefenseRatingToDefense)) * WarriorConversions.DefenseToCritReduction) +
                                            (stats.Resilience * WarriorConversions.ResilienceRatingToCritReduction);
            calculatedStats.CappedCritReduction = Math.Min(5f + levelDifference, calculatedStats.CritReduction);

            //Miss -> Dodge -> Parry -> Block -> Crushing Blow -> Critical Strikes -> Hit
            //Out of 100 attacks, you'll take...

            float crits = Math.Max(5f + (0.2f * levelDifference) - calculatedStats.CritReduction, 0f);
            float blocked = Math.Min(100f - (crits + (calculatedStats.DodgePlusMissPlusParry)), calculatedStats.Block);
            float crushes = targetLevel == 73 ? Math.Max(Math.Min(100f - (crits + (calculatedStats.DodgePlusMissPlusParryPlusBlock)), 15f) - stats.CrushChanceReduction, 0f) : 0f;
            float hits = Math.Max(100f - (crits + crushes + blocked + (calculatedStats.DodgePlusMissPlusParry)), 0f);

            calculatedStats.CrushChance = crushes;

            float damagePerBossAttack = calcOpts.BossAttackValue;
            //Apply armor and multipliers for each attack type...
            crits *= (100f - calculatedStats.CappedMitigation) * .02f;
            crushes *= (100f - calculatedStats.CappedMitigation) * .015f;
            hits *= (100f - calculatedStats.CappedMitigation) * .01f;
            blocked *= (100f - calculatedStats.CappedMitigation) * .01f * (1f - Math.Max((calculatedStats.BlockValue / (damagePerBossAttack * (100f - calculatedStats.CappedMitigation) / 100)), 0f)); //assume the block value is negligiable
            calculatedStats.DamageTaken = blocked + hits + crushes + crits;
            calculatedStats.TotalMitigation = 100f - calculatedStats.DamageTaken;

            int mitigationScale = calcOpts.MitigationScale;

            calculatedStats.SurvivalPoints = (stats.Health / ((1f - (calculatedStats.CappedMitigation / 100f)) * 0.9f)); // / (buffs.ShadowEmbrace ? 0.95f : 1f);
            calculatedStats.MitigationPoints = mitigationScale * (1f * (1f / (calculatedStats.DamageTaken / 100f)) * 0.9f); // / (buffs.ShadowEmbrace ? 0.95f : 1f);

            float cappedResist = targetLevel * 5;

            calculatedStats.NatureSurvivalPoints = (float)(stats.Health / ((1f - (System.Math.Min(cappedResist, stats.NatureResistance + stats.AllResist) / cappedResist) * .75)));
            calculatedStats.FrostSurvivalPoints = (float)(stats.Health / ((1f - (System.Math.Min(cappedResist, stats.FrostResistance + stats.AllResist) / cappedResist) * .75)));
            calculatedStats.FireSurvivalPoints = (float)(stats.Health / ((1f - (System.Math.Min(cappedResist, stats.FireResistance + stats.AllResist) / cappedResist) * .75)));
            calculatedStats.ShadowSurvivalPoints = (float)(stats.Health / ((1f - (System.Math.Min(cappedResist, stats.ShadowResistance + stats.AllResist) / cappedResist) * .75)));
            calculatedStats.ArcaneSurvivalPoints = (float)(stats.Health / ((1f - (System.Math.Min(cappedResist, stats.ArcaneResistance + stats.AllResist) / cappedResist) * .75)));

            float targetArmor = calcOpts.TargetArmor;
            float baseArmor = Math.Max(0f, targetArmor - stats.ArmorPenetration);
            float modArmor = 1 - (baseArmor / (baseArmor + 10557.5f));

            float critMultiplier = 2 * (1 + stats.BonusCritMultiplier);
            float attackPower = stats.AttackPower * (1 + stats.BonusAttackPowerMultiplier);


            float hasteBonus = stats.HasteRating * WarriorConversions.HasteRatingToHaste / 100f;

            float weaponSpeed;
            float weaponMinDamage;
            float weaponMaxDamage;

            if (character != null && character.MainHand != null)
            {
                weaponSpeed = character.MainHand.Speed;
                weaponMinDamage = character.MainHand.MinDamage;
                weaponMaxDamage = character.MainHand.MaxDamage;
            }
            else
            {
                //Assume a lvl 70 green dagger if the main hand is empty
                //so that threat values are not crazy for weapons.
                weaponSpeed = 1.7f;
                weaponMinDamage = 77f;
                weaponMaxDamage = 144f;
            }

            float attackSpeed = (weaponSpeed) / (1f + hasteBonus);
            if (attackSpeed < 1f)
                attackSpeed = 1.0f;

            float hitBonus = stats.HitRating * WarriorConversions.HitRatingToHit / 100f;
            float expertiseBonus = (stats.ExpertiseRating * WarriorConversions.ExpertiseRatingToExpertise +
                                    stats.Expertise) * WarriorConversions.ExpertiseToDodgeParryReduction / 100f;


            float chanceCrit = Math.Min(0.75f, (stats.CritRating * WarriorConversions.CritRatingToCrit +
                                               (stats.Agility * WarriorConversions.AgilityToCrit) +
                                                stats.Crit) / 100f) + 0.05f;
            float chanceDodge = Math.Max(0f, 0.05f + levelDifference / 100f - expertiseBonus);
            float chanceParry = Math.Max(0f, 0.05f + levelDifference / 100f + 0.08f - expertiseBonus);
            float chanceMiss = Math.Max(0f, 0.09f - hitBonus);
            float defStanceThreatMod = 1.3f *
                                       (1 + character.Talents.GetTalent("Defiance").PointsInvested * 0.05f) *
                                       (1 + stats.ThreatIncreaseMultiplier);
            float chanceAvoided = chanceMiss + chanceDodge + chanceParry;


            calculatedStats.AvoidedAttacks = (chanceMiss + chanceDodge + chanceParry) * 100f;
            calculatedStats.DodgedAttacks = chanceDodge * 100f;
            calculatedStats.ParriedAttacks = chanceParry * 100f;
            calculatedStats.MissedAttacks = chanceMiss * 100f;

            float averageDamage = 1 - chanceAvoided + (1 + stats.BonusCritMultiplier) * chanceCrit;

            float reducedDamage = 0.9f * (1f + stats.BonusPhysicalDamageMultiplier) * modArmor;

            float weaponDamage = ((weaponMinDamage + weaponMaxDamage) / 2f +
                                 (attackSpeed * attackPower / 14f));

            float whiteTPS = defStanceThreatMod * reducedDamage * weaponDamage / attackSpeed;

            //Max threat cycle is shield slam -> revenge -> devastate -> devastate -> repeat
            //with as many herioc strikes as possible.
            //In this basic model going to assume unlimited threat is heroic every swing, and
            //limited threat is zero heroic strikes.

            float shieldSlamTPS = (307f + defStanceThreatMod * reducedDamage *
                                  ((440f + 420f) / 2f + calculatedStats.BlockValue) * averageDamage) / 6f;

            float revengeTPS = (201f + defStanceThreatMod * reducedDamage *
                               ((414f + 506f) / 2f) * averageDamage) / 6f;

            float devastateTPS = 2f * (106f + 14f * 5f + defStanceThreatMod * reducedDamage *
                                 (0.5f * weaponDamage + 35f * 5f) * averageDamage) / 6f;

            float heroicStrikeTPS = (6f / attackSpeed) * (196f + defStanceThreatMod * reducedDamage *
                                    208f * averageDamage) / 6f;


            calculatedStats.ThreatScale = calcOpts.ThreatScale;

            calculatedStats.ThreatPoints = calculatedStats.ThreatScale * (whiteTPS + shieldSlamTPS + revengeTPS + devastateTPS);
            calculatedStats.UnlimitedThreat = calculatedStats.ThreatScale * (whiteTPS + shieldSlamTPS + revengeTPS + devastateTPS + heroicStrikeTPS);


            calculatedStats.OverallPoints = calculatedStats.MitigationPoints + calculatedStats.SurvivalPoints + calculatedStats.ThreatPoints;
            return calculatedStats;
        }

        #region Warrior Race Stats
        private static float[,] BaseWarriorRaceStats = new float[,] 
		{
							//	Strength,	Agility,	Stamina
            /*Human*/		{	144f,	    96f,	    132f,   },
            /*Orc*/			{	148f,		93f,		135f,	},
            /*Dwarf*/		{	146f,	    92f,	    135f,   },
			/*Night Elf*/	{	142f,	    101f,	    132f,   },
	        /*Undead*/		{	144f,	    94f,	    133f,   },
			/*Tauren*/		{	150f,		91f,		135f,	},
	        /*Gnome*/		{	140f,	    99f,	    132f,   },
			/*Troll*/		{	145f,	    98f,	    133f,   },	
			/*BloodElf*/	{	0f,		    0f,		    0f,	    },
			/*Draenei*/		{	146f,		93f,		132f,	},
		};

        private Stats GetRaceStats(Character character)
        {
            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.Human:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[0, 0],
                        Agility = (float)BaseWarriorRaceStats[0, 1],
                        Stamina = (float)BaseWarriorRaceStats[0, 2],

                        AttackPower = 190f,
                        Dodge = 0.75f,
                    };
                    if ((character.MainHand != null) &&
                        ((character.MainHand.Type == Item.ItemType.OneHandSword) ||
                         (character.MainHand.Type == Item.ItemType.OneHandMace)))
                    {
                        statsRace.Expertise += 5f;
                    }
                    break;
                case Character.CharacterRace.Orc:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[1, 0],
                        Agility = (float)BaseWarriorRaceStats[1, 1],
                        Stamina = (float)BaseWarriorRaceStats[1, 2],

                        AttackPower = 190f,
                        Dodge = 0.75f,
                    };

                    if ((character.MainHand != null) &&
                        (character.MainHand.Type == Item.ItemType.OneHandAxe))
                    {
                        statsRace.Expertise += 5f;
                    }
                    break;
                case Character.CharacterRace.Dwarf:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[2, 0],
                        Agility = (float)BaseWarriorRaceStats[2, 1],
                        Stamina = (float)BaseWarriorRaceStats[2, 2],

                        AttackPower = 190f,
                        Dodge = 0.75f,
                    };
                    break;
                case Character.CharacterRace.NightElf:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[3, 0],
                        Agility = (float)BaseWarriorRaceStats[3, 1],
                        Stamina = (float)BaseWarriorRaceStats[3, 2],

                        AttackPower = 190f,
                        Dodge = 1f + 0.75f,
                    };
                    break;
                case Character.CharacterRace.Undead:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[4, 0],
                        Agility = (float)BaseWarriorRaceStats[4, 1],
                        Stamina = (float)BaseWarriorRaceStats[4, 2],

                        AttackPower = 190f,
                        Dodge = 0.75f,
                    };
                    break;
                case Character.CharacterRace.Tauren:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[5, 0],
                        Agility = (float)BaseWarriorRaceStats[5, 1],
                        Stamina = (float)BaseWarriorRaceStats[5, 2],

                        AttackPower = 190f,
                        BonusHealthMultiplier = 0.05f,
                        Dodge = 0.75f,
                    };
                    break;
                case Character.CharacterRace.Gnome:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[6, 0],
                        Agility = (float)BaseWarriorRaceStats[6, 1],
                        Stamina = (float)BaseWarriorRaceStats[6, 2],

                        AttackPower = 190f,
                        Dodge = 0.75f,
                    };
                    break;
                case Character.CharacterRace.Troll:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[7, 0],
                        Agility = (float)BaseWarriorRaceStats[7, 1],
                        Stamina = (float)BaseWarriorRaceStats[7, 2],

                        AttackPower = 190f,
                        Dodge = 0.75f,
                    };
                    break;
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[9, 0],
                        Agility = (float)BaseWarriorRaceStats[9, 1],
                        Stamina = (float)BaseWarriorRaceStats[9, 2],

                        AttackPower = 190f,
                        Hit = 1f,
                        Dodge = 0.75f,
                    };
                    break;
                default:
                    statsRace = new Stats();
                    break;
            }

            return statsRace;
        }
        #endregion

        public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;

            Stats statsRace = GetRaceStats(character);
			Stats statsBaseGear = GetItemStats(character, additionalItem);
			Stats statsEnchants = GetEnchantsStats(character);
			Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            //parse talents
            TalentTree tree = character.Talents;

            Stats statsTalents = new Stats()
                {
                    Parry = tree.GetTalent("Deflection").PointsInvested * 1.0f,
                    Crit = tree.GetTalent("Cruelty").PointsInvested * 1.0f,
                    Defense = tree.GetTalent("Anticipation").PointsInvested * 4.0f,
                    Block = tree.GetTalent("Shield Specialization").PointsInvested * 1.0f,
                    BonusArmorMultiplier = tree.GetTalent("Toughness").PointsInvested * 0.02f,
                    BonusBlockValueMultiplier = tree.GetTalent("Shield Mastery").PointsInvested * 0.1f,
                    BonusPhysicalDamageMultiplier = tree.GetTalent("One-Handed Weapon Specialization").PointsInvested * 0.02f,
                    BonusStaminaMultiplier = tree.GetTalent("Vitality").PointsInvested * 0.01f,
                    BonusStrengthMultiplier = tree.GetTalent("Vitality").PointsInvested * 0.02f,
                    Expertise = tree.GetTalent("Defiance").PointsInvested * 2f,
                };
			
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

            float staBase = (float)Math.Floor((statsRace.Stamina) *
                             (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier) *
                             (1 + statsRace.BonusStaminaMultiplier) *
                             (1 + statsTalents.BonusStaminaMultiplier));
            float stamina = (float)Math.Floor(staBase + (statsBaseGear.Stamina + statsBuffs.Stamina) *
                             (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier) *
                             (1 + statsRace.BonusStaminaMultiplier) *
                             (1 + statsTalents.BonusStaminaMultiplier));

            float health = (float)Math.Floor((statsRace.Health + statsBaseGear.Health + statsBuffs.Health +
                            stamina * WarriorConversions.StaminaToHP) *
                             (1 + statsGearEnchantsBuffs.BonusHealthMultiplier) *
                             (1 + statsRace.BonusHealthMultiplier) *
                             (1 + statsTalents.BonusHealthMultiplier));

            float strBase = (float)Math.Floor((statsRace.Strength) *
                             (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier) *
                             (1 + statsRace.BonusStrengthMultiplier) *
                             (1 + statsTalents.BonusStrengthMultiplier));
            float strength = (float)Math.Floor(strBase + (statsBaseGear.Strength + statsBuffs.Strength) *
                              (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier) *
                              (1 + statsRace.BonusStrengthMultiplier) *
                              (1 + statsTalents.BonusStrengthMultiplier));
            
			Stats statsTotal = new Stats();
			statsTotal.Stamina = stamina;
			statsTotal.DefenseRating = statsRace.DefenseRating + statsBaseGear.DefenseRating + statsBuffs.DefenseRating;
			statsTotal.DodgeRating = statsRace.DodgeRating + statsBaseGear.DodgeRating + statsBuffs.DodgeRating;
            statsTotal.ParryRating = statsRace.ParryRating + statsBaseGear.ParryRating + statsBuffs.ParryRating;
            statsTotal.BlockRating = statsRace.BlockRating + statsBaseGear.BlockRating + statsBuffs.BlockRating;
            statsTotal.BlockValue = statsRace.BlockValue + statsBaseGear.BlockValue + statsBuffs.BlockValue;
			statsTotal.Resilience = statsRace.Resilience + statsBaseGear.Resilience + statsBuffs.Resilience;
            statsTotal.Health = health;
			statsTotal.Miss = statsRace.Miss + statsBaseGear.Miss + statsBuffs.Miss;
            statsTotal.CrushChanceReduction = statsGearEnchantsBuffs.CrushChanceReduction + statsRace.CrushChanceReduction;
            statsTotal.NatureResistance = statsGearEnchantsBuffs.NatureResistance + statsRace.NatureResistance;
            statsTotal.FireResistance = statsGearEnchantsBuffs.FireResistance + statsRace.FireResistance;
            statsTotal.FrostResistance = statsGearEnchantsBuffs.FrostResistance + statsRace.FrostResistance;
            statsTotal.ShadowResistance = statsGearEnchantsBuffs.ShadowResistance + statsRace.ShadowResistance;
            statsTotal.ArcaneResistance = statsGearEnchantsBuffs.ArcaneResistance + statsRace.ArcaneResistance;
            statsTotal.AllResist = statsGearEnchantsBuffs.AllResist + statsRace.AllResist;
            statsTotal.Defense = statsRace.Defense + statsTalents.Defense;
            statsTotal.Dodge = statsRace.Dodge + statsTalents.Dodge;
            statsTotal.Parry = statsRace.Parry + statsTalents.Parry;
            statsTotal.Block = statsRace.Block + statsTalents.Block;
            statsTotal.Hit = statsRace.Hit + statsTalents.Hit;
            statsTotal.Crit = statsRace.Crit + statsTalents.Crit;
            statsTotal.Expertise = statsRace.Expertise + statsTalents.Expertise;

            statsTotal.Block += (calcOpts.UseShieldBlock ? 75f : 0f);

            statsTotal.BonusArmorMultiplier = ((1 + statsRace.BonusArmorMultiplier) *
                                                  (1 + statsGearEnchantsBuffs.BonusArmorMultiplier) *
                                                  (1 + statsTalents.BonusArmorMultiplier)) - 1;
            statsTotal.BonusBlockValueMultiplier = ((1 + statsRace.BonusBlockValueMultiplier) *
                                                  (1 + statsGearEnchantsBuffs.BonusBlockValueMultiplier) *
                                                  (1 + statsTalents.BonusBlockValueMultiplier)) - 1;
            statsTotal.BonusPhysicalDamageMultiplier = ((1 + statsRace.BonusPhysicalDamageMultiplier) *
                                                  (1 + statsGearEnchantsBuffs.BonusPhysicalDamageMultiplier) *
                                                  (1 + statsTalents.BonusPhysicalDamageMultiplier)) - 1;
            statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) *
                                                  (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier) *
                                                  (1 + statsTalents.BonusStaminaMultiplier)) - 1;
            statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier) *
                                                     (1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier) *
                                                     (1 + statsTalents.BonusAttackPowerMultiplier)) - 1;
            statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier) *
                                                  (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier) *
                                                  (1 + statsTalents.BonusStrengthMultiplier)) - 1;
            statsTotal.ThreatIncreaseMultiplier = ((1 + statsRace.ThreatIncreaseMultiplier) *
                                                   (1 + statsGearEnchantsBuffs.ThreatIncreaseMultiplier) *
                                                    (1 + statsTalents.ThreatIncreaseMultiplier)) - 1;
            statsTotal.Agility = statsRace.Agility + statsGearEnchantsBuffs.Agility + statsTalents.Agility; ;
            statsTotal.Strength = strength;
 
            statsTotal.ArmorPenetration = statsRace.ArmorPenetration + statsGearEnchantsBuffs.ArmorPenetration;
            statsTotal.AttackPower = (float)Math.Floor((statsRace.AttackPower + statsGearEnchantsBuffs.AttackPower + (statsTotal.Strength * 2)) * (1f + statsTotal.BonusAttackPowerMultiplier));
            statsTotal.BonusCritMultiplier = ((1 + statsRace.BonusCritMultiplier) * (1 + statsGearEnchantsBuffs.BonusCritMultiplier)) - 1;
            statsTotal.CritRating = statsRace.CritRating + statsGearEnchantsBuffs.CritRating;
            statsTotal.ExpertiseRating = statsRace.ExpertiseRating + statsGearEnchantsBuffs.ExpertiseRating;
            statsTotal.HasteRating = statsRace.HasteRating + statsGearEnchantsBuffs.HasteRating;
            statsTotal.HitRating = statsRace.HitRating + statsGearEnchantsBuffs.HitRating;
            statsTotal.WeaponDamage = statsRace.WeaponDamage + statsGearEnchantsBuffs.WeaponDamage;
            statsTotal.ExposeWeakness = statsRace.ExposeWeakness + statsGearEnchantsBuffs.ExposeWeakness;
            statsTotal.Bloodlust = statsRace.Bloodlust + statsGearEnchantsBuffs.Bloodlust;

            statsTotal.Armor = (float)Math.Floor(((statsBaseGear.Armor * (1 + statsTalents.BonusArmorMultiplier)) +
                                statsRace.Armor + statsBuffs.Armor +
                                (statsTotal.Agility * 2f)) * (1 + statsBuffs.BonusArmorMultiplier));

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
                    ComparisonCalculationProtWarr calcBlock = new ComparisonCalculationProtWarr();
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
					CharacterCalculationsProtWarr calcDodgeValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DodgeRating = 1 } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcParryValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { ParryRating = 1 } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcBlockValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { BlockRating = 1 } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcBlockValueValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { BlockValue = 1 } }) as CharacterCalculationsProtWarr;
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

					ComparisonCalculationProtWarr comparisonAgi = new ComparisonCalculationProtWarr()
                    { 
                        Name = "Agility",
                        OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (agiToAdd - agiToSubtract),
						MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (agiToAdd - agiToSubtract),
                        SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (agiToAdd - agiToSubtract),
                        ThreatPoints = (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (agiToAdd - agiToSubtract)
                    };


					//Differential Calculations for Def
					calcAtAdd = calcBaseValue;
					float defToAdd = 0f;
					while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && defToAdd < 2)
					{
						defToAdd += 0.01f;
						calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DefenseRating = defToAdd } }) as CharacterCalculationsProtWarr;
					}

					calcAtSubtract = calcBaseValue;
					float defToSubtract = 0f;
					while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && defToSubtract > -2)
					{
						defToSubtract -= 0.01f;
						calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DefenseRating = defToSubtract } }) as CharacterCalculationsProtWarr;
					}
					defToSubtract += 0.01f;

					ComparisonCalculationProtWarr comparisonDef = new ComparisonCalculationProtWarr()
					{
						Name = "Defense Rating",
						OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (defToAdd - defToSubtract),
						MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (defToAdd - defToSubtract),
						SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (defToAdd - defToSubtract),
						ThreatPoints = (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (defToAdd - defToSubtract)
					};


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

					ComparisonCalculationProtWarr comparisonAC = new ComparisonCalculationProtWarr()
                    {
                        Name = "Armor",
                        OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (acToAdd - acToSubtract),
						MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (acToAdd - acToSubtract),
                        SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (acToAdd - acToSubtract),
                        ThreatPoints = (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (acToAdd - acToSubtract),
                    };


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

					ComparisonCalculationProtWarr comparisonSta = new ComparisonCalculationProtWarr()
                    {
                        Name = "Stamina",
                        OverallPoints = (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (staToAdd - staToSubtract),
						MitigationPoints = (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (staToAdd - staToSubtract),
                        SurvivalPoints = (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (staToAdd - staToSubtract),
                        ThreatPoints = (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (staToAdd - staToSubtract)
                    };

					return new ComparisonCalculationBase[] { 
						comparisonAgi,
						comparisonAC,
						comparisonSta,
						comparisonDef,
						new ComparisonCalculationProtWarr() { Name = "Dodge Rating",
                            OverallPoints = (calcDodgeValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcDodgeValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcDodgeValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcDodgeValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtWarr() { Name = "Parry Rating",
                            OverallPoints = (calcParryValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcParryValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcParryValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcParryValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtWarr() { Name = "Block Rating",
                            OverallPoints = (calcBlockValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcBlockValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcBlockValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcBlockValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtWarr() { Name = "Block Value",
                            OverallPoints = (calcBlockValueValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcBlockValueValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcBlockValueValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcBlockValueValue.ThreatPoints - calcBaseValue.ThreatPoints)},
						new ComparisonCalculationProtWarr() { Name = "Health",
                            OverallPoints = (calcHealthValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcHealthValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcHealthValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcHealthValue.ThreatPoints - calcBaseValue.ThreatPoints)},
						new ComparisonCalculationProtWarr() { Name = "Resilience",
                            OverallPoints = (calcResilValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcResilValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcResilValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcResilValue.ThreatPoints - calcBaseValue.ThreatPoints)},
					};
				
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
                BonusCritMultiplier = stats.BonusCritMultiplier,
                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,

                MongooseProc = stats.MongooseProc

			};
		}

		public override bool HasRelevantStats(Stats stats)
		{
			return (stats.Agility + stats.Armor + stats.BonusAgilityMultiplier + stats.BonusArmorMultiplier +
				    stats.BonusStaminaMultiplier + stats.DefenseRating + stats.DodgeRating + stats.ParryRating +
                    stats.BlockRating + stats.BlockValue + stats.Health + 
				    stats.Miss + stats.Resilience + stats.Stamina + stats.AllResist +
				    stats.ArcaneResistance + stats.NatureResistance + stats.FireResistance +
				    stats.FrostResistance + stats.ShadowResistance +
                    stats.Strength + stats.AttackPower + stats.CritRating + stats.HitRating + stats.HasteRating +
                    stats.ExpertiseRating + stats.ArmorPenetration + stats.WeaponDamage +
                    stats.BonusCritMultiplier + stats.MongooseProc + stats.CrushChanceReduction +
                    stats.ThreatIncreaseMultiplier + stats.BonusPhysicalDamageMultiplier
                   ) != 0;
		}
    }

    public class WarriorConversions
    {
        public static readonly float AgilityToDodge = 1.0f / 30.0f;
        public static readonly float DodgeRatingToDodge = 1.0f / 18.930f;
        public static readonly float StrengthToAP = 2.0f;
        public static readonly float StrengthToBlockValue = 1.0f / 20.0f;
        public static readonly float AgilityToArmor = 2.0f;
        public static readonly float AgilityToCrit = 1.0f / 33.0f;
        public static readonly float StaminaToHP = 10.0f;
        public static readonly float HitRatingToHit = 1.0f / 15.7692f;
        public static readonly float CritRatingToCrit = 1.0f / 22.0769f;
        public static readonly float HasteRatingToHaste = 1.0f / 15.77f;
        public static readonly float ExpertiseRatingToExpertise = 1.0f / 3.9423f;
        public static readonly float ExpertiseToDodgeParryReduction = 0.25f;
        public static readonly float DefenseRatingToDefense = 1.0f / 2.3654f;
        public static readonly float DefenseToDodge = 0.04f;
        public static readonly float DefenseToBlock = 0.04f;
        public static readonly float DefenseToParry = 0.04f;
        public static readonly float DefenseToMiss = 0.04f;
        public static readonly float DefenseToCritReduction = 0.04f;
        public static readonly float DefenseToDazeReduction = 0.04f;
        public static readonly float ParryRatingToParry = 1.0f / 23.6538461538462f;
        public static readonly float BlockRatingToBlock = 1.0f / 7.8846f;
        public static readonly float ResilienceRatingToCritReduction = 1.0f / 39.4231f;
    }
}
