using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin
{
    [Rawr.Calculations.RawrModelInfo("ProtPaladin", "Ability_Paladin_JudgementsOfTheJust", Character.CharacterClass.Paladin)]
    public class CalculationsProtPaladin : CalculationsBase
    {
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                // Relevant Gem IDs for ProtPaladin
                // Red
                int[] bold = { 39900, 39996, 40111, 42142 };        // Strength
                int[] delicate = { 39905, 39997, 40112, 42143 };    // Agility
                int[] subtle = { 39907, 40000, 40115, 42151 };      // Dodge
                int[] flashing = { 39908, 40001, 40116, 42152 };    // Parry
                int[] precise = { 39910, 40003, 40118, 42154 };     // Expertise

                // Purple
                int[] shifting = { 39935, 40023, 40130, 40130 };    // Agility + Stamina
                int[] sovereign = { 39934, 40022, 40129, 40129 };   // Strength + Stamina
                int[] regal = { 39938, 40031, 40138, 40138 };       // Dodge + Stamina
                int[] defender = { 39939, 40032, 40139, 40139 };    // Parry + Stamina
                int[] guardian = { 39940, 40034, 40141, 40141 };    // Expertise + Stamina

                // Blue
                int[] solid = { 39919, 40008, 40119, 36767 };       // Stamina

                // Green
                int[] enduring = { 39976, 40089, 40167, 40167 };    // Defense + Stamina

                // Yellow
                int[] thick = { 39916, 40015, 40126, 42157 };       // Defense

                // Orange
                int[] etched = { 39948, 40038, 40143, 40143 };      // Strength + Hit
                int[] champion = { 39949, 40039, 40144, 40144 };    // Strength + Defense
                int[] stalwart = { 39964, 40056, 40160, 40160 };    // Dodge + Defense
                int[] glimmering = { 39965, 40057, 40161, 40161 };  // Parry + Defense
                int[] accurate = { 39966, 40058, 40162, 40162 };    // Expertise + Hit
                int[] resolute = { 39967, 40059, 40163, 40163 };    // Expertise + Defense

                // Meta
                int[] austere = { 41380 };
                int[] eternal = { 41396 };

                string[] groupNames = new string[] { "Uncommon", "Rare", "Epic", "Jeweler" };
                int[,][] gemmingTemplates = new int[,][]
                {
                    //Red       Yellow      Blue        Prismatic   Meta
                    { sovereign,    enduring,   solid,      solid,      austere },  // Balanced Threat + Avoidance, Austere
                    { sovereign,    enduring,   solid,      solid,      eternal },  // Balanced Threat + Avoidance, Eternal
                    { solid,        solid,      solid,      solid,      austere },  // Max Stam, Austere
                    { solid,        solid,      solid,      solid,      eternal },  // Max Stam, Eternal
                };

                // Generate List of Gemming Templates
                List<GemmingTemplate> gemmingTemplate = new List<GemmingTemplate>();
                for (int j = 0; j <= groupNames.GetUpperBound(0); j++)
                {
                    for (int i = 0; i <= gemmingTemplates.GetUpperBound(0); i++)
                    {
                        gemmingTemplate.Add(new GemmingTemplate()
                        {
                            Model = "ProtPaladin",
                            Group = groupNames[j],
                            RedId = gemmingTemplates[i, 0][j],
                            YellowId = gemmingTemplates[i, 1][j],
                            BlueId = gemmingTemplates[i, 2][j],
                            PrismaticId = gemmingTemplates[i, 3][j],
                            MetaId = gemmingTemplates[i, 4][0],
                            Enabled = j == 1
                        });
                    }
                }
                return gemmingTemplate;
            }
        }

        #region Variables and Properties
        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelProtPaladin();
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
					"Base Stats:Health",
                    "Base Stats:Strength",
                    "Base Stats:Agility",
                    "Base Stats:Stamina",

                    "Defensive Stats:Armor",
                    "Defensive Stats:Defense",
                    "Defensive Stats:Dodge",
                    "Defensive Stats:Parry",
                    "Defensive Stats:Block",
                    "Defensive Stats:Resilience",
                    "Defensive Stats:Miss",
                    "Defensive Stats:Block Value",
                    "Defensive Stats:Chance to be Crit",
                    "Defensive Stats:Guaranteed Reduction",
					"Defensive Stats:Avoidance",
                    "Defensive Stats:Avoidance + Block",
					"Defensive Stats:Total Mitigation",
                    "Defensive Stats:Attacker Speed",
					"Defensive Stats:Damage Taken",

                    "Offensive Stats:Weapon Speed",
                    "Offensive Stats:Attack Power",
                    "Offensive Stats:Spell Power",
                    "Offensive Stats:Hit",
					"Offensive Stats:Haste",
					"Offensive Stats:Armor Penetration",
					"Offensive Stats:Crit",
                    "Offensive Stats:Expertise",
					"Offensive Stats:Weapon Damage",
                    "Offensive Stats:Missed Attacks",
                    "Offensive Stats:Total Damage/sec",
                    "Offensive Stats:Threat/sec",
                    //"Offensive Stats:Unlimited Threat/sec*All white damage converted to Heroic Strikes.",

                    "Resistances:Nature Resist",
					"Resistances:Fire Resist",
					"Resistances:Frost Resist",
					"Resistances:Shadow Resist",
					"Resistances:Arcane Resist",
                    "Complex Stats:Ranking Mode*The currently selected ranking mode. Ranking modes can be changed in the Options tab.",
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
(pre-avoidance/block) you can take before dying. Unlike 
Mitigation Points, you should not attempt to maximize this, 
but rather get 'enough' of it, and then focus on Mitigation. 
'Enough' can vary greatly by fight and by your healers.
If you find that you are being killed by burst damage,
focus on Survival Points.",
                    @"Complex Stats:Threat Points*Threat Points represents the average threat per second accumulated scaled by the threat scale.",
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
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
					"Health",
                    "% Total Mitigation",
					"% Guaranteed Reduction",
					"% Chance to Avoid Attacks",
					"% Chance to be Crit",
                    "% Chance to be Avoided", 
                    "Burst Time", 
                    "TankPoints", 
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
                    "Ability Damage",
                    "Ability Threat",
					"Combat Table",
					"Item Budget",
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
                    _subPointNameColors.Add("Survival", System.Drawing.Color.Blue);
                    _subPointNameColors.Add("Mitigation", System.Drawing.Color.Red);
                    _subPointNameColors.Add("Threat", System.Drawing.Color.Yellow);
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
                        Item.ItemType.Plate,
                        Item.ItemType.None,
						Item.ItemType.Shield,
						Item.ItemType.Libram,
						Item.ItemType.OneHandAxe,
						Item.ItemType.OneHandMace,
						Item.ItemType.OneHandSword,
					});
                }
                return _relevantItemTypes;
            }
        }

        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Paladin; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationProtPaladin(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsProtPaladin(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsProtPaladin));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsProtPaladin calcOpts = serializer.Deserialize(reader) as CalculationOptionsProtPaladin;
            return calcOpts;
        }
        #endregion

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            CharacterCalculationsProtPaladin calculatedStats = new CharacterCalculationsProtPaladin();
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            Stats stats = GetCharacterStats(character, additionalItem);

            AttackModelMode amm = AttackModelMode.BasicSoV;            
            if (calcOpts.SealChoice == "Seal of Righteousness")
                amm = AttackModelMode.BasicSoR;

            DefendModel dm = new DefendModel(character, stats);
            AttackModel am = new AttackModel(character, stats, amm);

            calculatedStats.BasicStats = stats;
            calculatedStats.TargetLevel = calcOpts.TargetLevel;
            calculatedStats.ActiveBuffs = new List<Buff>(character.ActiveBuffs);
            calculatedStats.Abilities = am.Abilities;

            calculatedStats.Miss = dm.DefendTable.Miss;
            calculatedStats.Dodge = dm.DefendTable.Dodge;
            calculatedStats.Parry = dm.DefendTable.Parry;
            calculatedStats.Block = dm.DefendTable.Block;

            calculatedStats.Defense = 400.0f + stats.Defense + (float)Math.Floor(stats.DefenseRating * ProtPaladin.DefenseRatingToDefense);
            calculatedStats.BlockValue = stats.BlockValue;

            calculatedStats.DodgePlusMissPlusParry = calculatedStats.Dodge + calculatedStats.Miss + calculatedStats.Parry;
            calculatedStats.DodgePlusMissPlusParryPlusBlock = calculatedStats.Dodge + calculatedStats.Miss + calculatedStats.Parry + calculatedStats.Block;
            calculatedStats.CritReduction = Lookup.AvoidanceChance(character, stats, HitResult.Crit);
            calculatedStats.CritVulnerability = dm.DefendTable.Critical;

            calculatedStats.ArmorReduction = Lookup.ArmorReduction(character, stats);
            calculatedStats.GuaranteedReduction = dm.GuaranteedReduction;
            calculatedStats.TotalMitigation = dm.Mitigation;
            calculatedStats.BaseAttackerSpeed = calcOpts.BossAttackSpeed;
            calculatedStats.AttackerSpeed = dm.ParryModel.BossAttackSpeed;
            calculatedStats.DamageTaken = dm.DamagePerSecond;
            calculatedStats.DamageTakenPerHit = dm.DamagePerHit;
            calculatedStats.DamageTakenPerBlock = dm.DamagePerBlock;
            calculatedStats.DamageTakenPerCrit = dm.DamagePerCrit;

            calculatedStats.ArcaneReduction = (1.0f - Lookup.MagicReduction(character, stats, DamageType.Arcane));
            calculatedStats.FireReduction = (1.0f - Lookup.MagicReduction(character, stats, DamageType.Fire));
            calculatedStats.FrostReduction = (1.0f - Lookup.MagicReduction(character, stats, DamageType.Frost));
            calculatedStats.NatureReduction = (1.0f - Lookup.MagicReduction(character, stats, DamageType.Nature));
            calculatedStats.ShadowReduction = (1.0f - Lookup.MagicReduction(character, stats, DamageType.Shadow));
            calculatedStats.ArcaneSurvivalPoints = stats.Health / Lookup.MagicReduction(character, stats, DamageType.Arcane);
            calculatedStats.FireSurvivalPoints = stats.Health / Lookup.MagicReduction(character, stats, DamageType.Fire);
            calculatedStats.FrostSurvivalPoints = stats.Health / Lookup.MagicReduction(character, stats, DamageType.Frost);
            calculatedStats.NatureSurvivalPoints = stats.Health / Lookup.MagicReduction(character, stats, DamageType.Nature);
            calculatedStats.ShadowSurvivalPoints = stats.Health / Lookup.MagicReduction(character, stats, DamageType.Shadow);

            calculatedStats.Hit = Lookup.BonusHitPercentage(character, stats);
            calculatedStats.Crit = Lookup.BonusCritPercentage(character, stats);
            calculatedStats.Expertise = Lookup.BonusExpertisePercentage(character, stats);            
            calculatedStats.Haste = Lookup.BonusHastePercentage(character, stats);
            calculatedStats.ArmorPenetration = Lookup.BonusArmorPenetrationPercentage(character, stats);
            calculatedStats.AvoidedAttacks = am.Abilities[Ability.None].AttackTable.AnyMiss;
            calculatedStats.DodgedAttacks = am.Abilities[Ability.None].AttackTable.Dodge;
            calculatedStats.ParriedAttacks = am.Abilities[Ability.None].AttackTable.Parry;
            calculatedStats.MissedAttacks = am.Abilities[Ability.None].AttackTable.Miss;
            calculatedStats.WeaponSpeed = Lookup.WeaponSpeed(character, stats);
            calculatedStats.TotalDamagePerSecond = am.DamagePerSecond;

            //calculatedStats.UnlimitedThreat = am.ThreatPerSecond;
            //am.RageModelMode = RageModelMode.Limited;
            calculatedStats.ThreatPerSecond = am.ThreatPerSecond;
            calculatedStats.ThreatModel = am.Name + "\n" + am.Description;

            calculatedStats.TankPoints = dm.TankPoints;
            calculatedStats.BurstTime = dm.BurstTime;
            calculatedStats.RankingMode = calcOpts.RankingMode;
            calculatedStats.ThreatPoints = calcOpts.ThreatScale * calculatedStats.ThreatPerSecond;
            switch (calcOpts.RankingMode)
            {
                case 2:
                    // Tank Points Mode
                    calculatedStats.SurvivalPoints = (dm.EffectiveHealth);
                    calculatedStats.MitigationPoints = (dm.TankPoints - dm.EffectiveHealth);
                    calculatedStats.ThreatPoints *= 3.0f;
                    break;
                case 3:
                    // Burst Time Mode
                    float threatScale = Convert.ToSingle(Math.Pow(Convert.ToDouble(calcOpts.BossAttackValue) / 25000.0d, 4));
                    calculatedStats.SurvivalPoints = (dm.BurstTime * 100.0f);
                    calculatedStats.MitigationPoints = 0.0f;
                    calculatedStats.ThreatPoints = (calculatedStats.ThreatPoints / threatScale) * 2.0f;
                    break;
                case 4:
                    // Damage Output Mode
                    calculatedStats.SurvivalPoints = 0.0f;
                    calculatedStats.MitigationPoints = 0.0f;
                    calculatedStats.ThreatPoints = calculatedStats.TotalDamagePerSecond;
                    break;
                default:
                    // Mitigation Scale Mode
                    calculatedStats.SurvivalPoints = (dm.EffectiveHealth);
                    calculatedStats.MitigationPoints = dm.Mitigation * calcOpts.BossAttackValue * calcOpts.MitigationScale * 100.0f;
                    break;
            }
            calculatedStats.OverallPoints = calculatedStats.MitigationPoints + calculatedStats.SurvivalPoints + calculatedStats.ThreatPoints;

            return calculatedStats;
        }

        
        #region Paladin Race Stats
        private static float[,] BasePaladinRaceStats = new float[,] 
		{
        	 /* These values are well-defined by the sum of base value for Race, Class and gains from level.
        	 * For information about Race and Class values see http://www.wowwiki.com/Race
        	 * Infomation about Paladin stat gains from level is Negarines own research.
        	 * 
        	 * 						Strength,	Agility,	Stamina,	Intellect,	Spirit
        	 * ["GainLvl80"]	{	129,		70,			121,		78,			84	}
        	 * ["ClassBonus"]	{	2,			0,			2,			0,			1	}
        	 * ["Human"]		{	20, 		20, 		20, 		20, 		21	}
        	 * ["Orc"] 			{	23, 		17, 		22, 		17, 		23	}
        	 * ["Dwarf"]		{	22, 		16, 		23, 		19, 		19	}
        	 * ["NightElf"]		{	17, 		25, 		19, 		20, 		20	}
        	 * ["Undead"]		{	19, 		18, 		21, 		18, 		25	}
        	 * ["Tauren"]		{	25, 		15, 		22, 		15, 		22	}
        	 * ["Gnome"]		{	15, 		23, 		19, 		24, 		20	}
        	 * ["Troll"]		{	21, 		22, 		21, 		16, 		21	}
        	 * ["Draenei"]		{	21, 		17, 		19, 		21, 		22	}
        	 * ["BloodElf"]		{	17, 		22, 		18, 		24, 		19	}
        	 */
							//	Strength,	Agility,	Stamina,	Intellect,	Spirit,
            /*Human*/		{	151f,	    90f,	    143f,   	98f,		105f,	},
            /*Orc*/			{	154f,		87f,		145f,		95f,		108f,	},
            /*Dwarf*/		{	153f,	    86f,	    146f,   	97f,		104f,	},
			/*Night Elf*/	{	148f,	    95f,	    142f,   	98f,		105f,	},
	        /*Undead*/		{	150f,	    88f,	    144f,   	96f,		110f,	},
			/*Tauren*/		{	156f,		85f,		145f,		93f,		107f,	},
	        /*Gnome*/		{	146f,	    93f,	    142f,   	102f,		105f,	},
			/*Troll*/		{	153f,	    87f,	    144f,   	94f,		106f,	},
			/*Draenei*/		{	153f,		92f,		142f,		99f,		107f,	},
			/*BloodElf*/	{	148f,		92f,		141f,		102f,		104f,	},
		};
        

        private Stats GetRaceStats(Character character)
        {
            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.Human:
                    statsRace = new Stats()
                    {
                        Health = 6934f,
                        Strength = (float)BasePaladinRaceStats[0, 0],
                        Agility = (float)BasePaladinRaceStats[0, 1],
                        Stamina = (float)BasePaladinRaceStats[0, 2],
                        Intellect = (float)BasePaladinRaceStats[0, 3],
                        Spirit = (float)BasePaladinRaceStats[0, 4],

                        AttackPower = 220f,
                        Dodge = 3.2685f,
                        Miss = 0.05f,
                        Parry = 5f,
                        PhysicalCrit = 0.032685f,
                        //Spirit *= 1.03f	// Human spirit bonus is 3%, need to research if wowwiki race spirit value already includes this. (20*1.03 = 21)
                    };
                    if ((character.MainHand != null) &&
                        ((character.MainHand.Item.Type == Item.ItemType.OneHandSword) ||
                         (character.MainHand.Item.Type == Item.ItemType.OneHandMace)))
                    {
                        statsRace.Expertise += 3f;
                    }
                    break;
                case Character.CharacterRace.Orc:
                    statsRace = new Stats()
                    {
                        Health = 6934f,
                        Strength = (float)BasePaladinRaceStats[1, 0],
                        Agility = (float)BasePaladinRaceStats[1, 1],
                        Stamina = (float)BasePaladinRaceStats[1, 2],
                        Intellect = (float)BasePaladinRaceStats[1, 3],
                        Spirit = (float)BasePaladinRaceStats[1, 4],

                        AttackPower = 220f,
                        Dodge = 3.2685f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.032685f,
                    };

                    if ((character.MainHand != null) &&
                        (character.MainHand.Item.Type == Item.ItemType.OneHandAxe))
                    {
                        statsRace.Expertise += 5f;
                    }
                    break;
                case Character.CharacterRace.Dwarf:
                    statsRace = new Stats()
                    {
                        Health = 6934f,
                        Strength = (float)BasePaladinRaceStats[2, 0],
                        Agility = (float)BasePaladinRaceStats[2, 1],
                        Stamina = (float)BasePaladinRaceStats[2, 2],
                        Intellect = (float)BasePaladinRaceStats[2, 3],
                        Spirit = (float)BasePaladinRaceStats[2, 4],

                        AttackPower = 220f,
                        Dodge = 3.2685f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.032685f,
                    };
                    if ((character.MainHand != null) &&
                        (character.MainHand.Item.Type == Item.ItemType.OneHandMace))
                    {
                        statsRace.Expertise += 5f;
                    }
                    break;
                case Character.CharacterRace.NightElf:
                    statsRace = new Stats()
                    {
                        Health = 6934f,
                        Strength = (float)BasePaladinRaceStats[3, 0],
                        Agility = (float)BasePaladinRaceStats[3, 1],
                        Stamina = (float)BasePaladinRaceStats[3, 2],
                        Intellect = (float)BasePaladinRaceStats[3, 3],
                        Spirit = (float)BasePaladinRaceStats[3, 4],

                        AttackPower = 220f,
                        Dodge = 3.2685f,
                        Miss = 0.05f + 0.02f,
                        Parry = 5f,
                        PhysicalCrit = 0.032685f,
                    };
                    break;
                case Character.CharacterRace.Undead:
                    statsRace = new Stats()
                    {
                        Health = 6934f,
                        Strength = (float)BasePaladinRaceStats[4, 0],
                        Agility = (float)BasePaladinRaceStats[4, 1],
                        Stamina = (float)BasePaladinRaceStats[4, 2],
                        Intellect = (float)BasePaladinRaceStats[4, 3],
                        Spirit = (float)BasePaladinRaceStats[4, 4],

                        AttackPower = 220f,
                        Dodge = 3.2685f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.032685f,
                    };
                    break;
                case Character.CharacterRace.Tauren:
                    statsRace = new Stats()
                    {
                        Health = 7280f, // = 6934f * 1.05f
                        Strength = (float)BasePaladinRaceStats[5, 0],
                        Agility = (float)BasePaladinRaceStats[5, 1],
                        Stamina = (float)BasePaladinRaceStats[5, 2],
                        Intellect = (float)BasePaladinRaceStats[5, 3],
                        Spirit = (float)BasePaladinRaceStats[5, 4],

                        AttackPower = 220f,
                        Dodge = 3.2685f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.032685f,
                    };
                    break;
                case Character.CharacterRace.Gnome:
                    statsRace = new Stats()
                    {
                        Health = 6934f,
                        Strength = (float)BasePaladinRaceStats[6, 0],
                        Agility = (float)BasePaladinRaceStats[6, 1],
                        Stamina = (float)BasePaladinRaceStats[6, 2],
                        Intellect = (float)BasePaladinRaceStats[6, 3],
                        Spirit = (float)BasePaladinRaceStats[6, 4],

                        AttackPower = 220f,
                        Dodge = 3.2685f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.032685f,
                    };
                    break;
                case Character.CharacterRace.Troll:
                    statsRace = new Stats()
                    {
                        Health = 6934f,
                        Strength = (float)BasePaladinRaceStats[7, 0],
                        Agility = (float)BasePaladinRaceStats[7, 1],
                        Stamina = (float)BasePaladinRaceStats[7, 2],
                        Intellect = (float)BasePaladinRaceStats[7, 3],
                        Spirit = (float)BasePaladinRaceStats[7, 4],

                        AttackPower = 220f,
                        Dodge = 3.2685f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.032685f,
                    };
                    break;
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats()
                    {
                        Health = 6934f,
                        Strength = (float)BasePaladinRaceStats[8, 0],
                        Agility = (float)BasePaladinRaceStats[8, 1],
                        Stamina = (float)BasePaladinRaceStats[8, 2],
                        Intellect = (float)BasePaladinRaceStats[8, 3],
                        Spirit = (float)BasePaladinRaceStats[8, 4],

                        AttackPower = 220f,
                        Dodge = 3.2685f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.032685f,
                        PhysicalHit = 0.01f,
                        SpellHit = 0.01f,
                    };
                    break;
                case Character.CharacterRace.BloodElf:
                    statsRace = new Stats()
                    {
                        Health = 6934f,
                        Strength = (float)BasePaladinRaceStats[9, 0],
                        Agility = (float)BasePaladinRaceStats[9, 1],
                        Stamina = (float)BasePaladinRaceStats[9, 2],
                        Intellect = (float)BasePaladinRaceStats[9, 3],
                        Spirit = (float)BasePaladinRaceStats[9, 4],

                        AttackPower = 220f,
                        Dodge = 3.2685f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 0.032685f,
                        //TODO Magic Resistance: Reduces the chance to be hit by spells by 2% There's no definition for this in Stats.cs
                    };
                    break;
                default:
                    statsRace = new Stats();
                    break;
            }

            return statsRace;
            /* 			
            Stats uniform to all races of paladins:
            I plan to pull those variables out of the switch case to make the Race gains more distinct from Class attributes
            something like:
            statsClass = new Stats()
            {
            	AttackPower = 220f,			// Base AP = (LVL * 3f) - 20f
            	Dodge = 3.2685f,			// Base Dodge for the paladin class
            	Health = 6934f,				// this is the Base Health for any level 80 character of any class. (tauren get 5% bonus on this value)
            	Parry = 5f,					// Base Parry for a character with maxed out defense skill
            	Miss = 0.05f,				// Base Miss for a character with maxed out defense skill
            	PhysicalCrit = 0.032685f,	// Base Crit for the paladin class TODO: Check if this is different for other classes
            	Mana = 4114,				// this is the Base Mana for any level 80 character of any class
            	Block = 5f,					// look into the block formula, this is a base value after all
            };
            Stats statsBase = StatsRace + StatsClass;
            
            later in the code, the following arrays of stats are used,
            
            Stats statsGearEnchantsBuffs = statsItems + statsBuffs;
            Stats statsTotal = statsRace + statsItems + statsBuffs + statsTalents;
            
            which suggests I will have to rename the later used StatsRace to StatsBase.
			*/
        }
        #endregion

        public override Stats GetItemStats(Character character, Item additionalItem)
        {
            Stats statsItems = base.GetItemStats(character, additionalItem);

            // Assuming a GCD every 1.5s
            float abilityPerSecond = 1.0f / 1.5f;
            // Assumes 15% miss rate as we don't know the stats here--need to find a better solution
            float hitRate = 0.85f;

            //Mongoose
            if (character.MainHand != null && statsItems.MongooseProc > 0)
            {
                float procRate = 1.0f; // PPM
                float procDuration = 15.0f;
                float procPerSecond = (((procRate / 60.0f) * character.MainHand.Item.Speed) + ((procRate / 60.0f) * abilityPerSecond)) * hitRate;
                float procUptime = procDuration * procPerSecond;

                statsItems.Agility += 120.0f * procUptime;
                statsItems.HasteRating += (2.0f / ProtPaladin.HasteRatingToHaste) * procUptime;
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
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            PaladinTalents talents = character.PaladinTalents;

            Stats statsRace = GetRaceStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsItems = GetItemStats(character, additionalItem);
            Stats statsTalents = new Stats()
            {
                Parry = talents.Deflection * 1.0f,
                Dodge = talents.Anticipation * 1.0f,
                Block = (calcOpts.UseHolyShield ? talents.HolyShield * 30.0f : 0),
                BonusBlockValueMultiplier = talents.Redoubt * 0.1f,
                BonusDamageMultiplier = (1.0f + talents.Crusade * 0.01f) *
                    (talents.OneHandedWeaponSpecialization > 0 ? 1.0f + talents.OneHandedWeaponSpecialization * .03f + .01f : 1.0f) - 1.0f,
                BonusStaminaMultiplier = (1.0f + talents.SacredDuty * 0.04f) * (1.0f + talents.CombatExpertise * 0.02f) - 1.0f,
                Expertise = talents.CombatExpertise * 2.0f,
                BaseArmorMultiplier = talents.Toughness * 0.02f,
                PhysicalCrit = (talents.CombatExpertise * 0.02f) + (talents.Conviction * 0.01f) + (talents.SanctityOfBattle * 0.01f),
                SpellCrit = (talents.CombatExpertise * 0.02f) + (talents.SanctityOfBattle * 0.01f),
                BonusStrengthMultiplier = talents.DivineStrength * 0.03f,
            };
            Stats statsGearEnchantsBuffs = statsItems + statsBuffs;
            Stats statsTotal = statsRace + statsItems + statsBuffs + statsTalents;

            statsTotal.BaseAgility = statsRace.Agility + statsTalents.Agility;
            statsTotal.Stamina = (float)Math.Floor(statsRace.Stamina * (1.0f + statsTalents.BonusStaminaMultiplier));
            statsTotal.Stamina += (float)Math.Floor((statsItems.Stamina + statsBuffs.Stamina) * (1.0f + statsTalents.BonusStaminaMultiplier));
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1.0f + statsBuffs.BonusStaminaMultiplier) * (1.0f + statsItems.BonusStaminaMultiplier));
            statsTotal.Strength = (float)Math.Floor((statsRace.Strength + statsTalents.Strength) * (1.0f + statsTotal.BonusStrengthMultiplier));
            statsTotal.Strength += (float)Math.Floor((statsItems.Strength + statsBuffs.Strength) * (1.0f + statsTotal.BonusStrengthMultiplier));
            if (statsTotal.GreatnessProc > 0)
            {
                statsTotal.Strength += (float)Math.Floor(statsTotal.GreatnessProc * 15f / 48f);
            }
            if (calcOpts.GlyphSealVengeance && calcOpts.SealChoice == "Seal of Vengeance") 
            {
                statsTotal.Expertise += 10;
            }
            statsTotal.Agility = (float)Math.Floor((statsRace.Agility + statsTalents.Agility) * (1f + statsTotal.BonusAgilityMultiplier));
            statsTotal.Agility += (float)Math.Floor((statsItems.Agility + statsBuffs.Agility) * (1f + statsTotal.BonusAgilityMultiplier));
            statsTotal.Health += (statsTotal.Stamina - 18f) * 10f;
            if (character.ActiveBuffsContains("Commanding Shout"))
                statsBuffs.Health += statsBuffs.BonusCommandingShoutHP;
            statsTotal.Health *= 1f + statsTotal.BonusHealthMultiplier;
            statsTotal.Armor *= 1f + statsTotal.BaseArmorMultiplier;
            statsTotal.Armor += 2f * (float)Math.Floor(statsTotal.Agility) + statsTotal.BonusArmor;
            statsTotal.Armor = (float)Math.Floor(statsTotal.Armor * (1f + statsTotal.BonusArmorMultiplier));
            statsTotal.AttackPower += statsTotal.Strength * 2f;
            statsTotal.AttackPower = (float)Math.Floor(statsTotal.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
            statsTotal.SpellPower += (float)Math.Floor(statsTotal.Stamina * talents.TouchedByTheLight * 0.1f);
            //statsTotal.SpellPower = (float)Math.Floor(statsTotal.SpellPower * (1f + statsTotal.BonusSpellPowerMultiplier));// not sure there's such a thing
            statsTotal.NatureResistance += statsTotal.NatureResistanceBuff + statsTotal.AllResist;
            statsTotal.FireResistance += statsTotal.FireResistanceBuff + statsTotal.AllResist;
            statsTotal.FrostResistance += statsTotal.FrostResistanceBuff + statsTotal.AllResist;
            statsTotal.ShadowResistance += statsTotal.ShadowResistanceBuff + statsTotal.AllResist;
            statsTotal.ArcaneResistance += statsTotal.ArcaneResistanceBuff + statsTotal.AllResist;

            statsTotal.BlockValue += (float)Math.Floor(statsTotal.Strength * ProtPaladin.StrengthToBlockValue - 10f);
            statsTotal.BlockValue = (float)Math.Floor(statsTotal.BlockValue * (1f + statsTotal.BonusBlockValueMultiplier));

            statsTotal.ArmorPenetration = statsRace.ArmorPenetration + statsGearEnchantsBuffs.ArmorPenetration;
            statsTotal.BonusCritMultiplier = ((1 + statsRace.BonusCritMultiplier) * (1f + statsGearEnchantsBuffs.BonusCritMultiplier)) - 1f;
            statsTotal.CritRating = statsRace.CritRating + statsGearEnchantsBuffs.CritRating;            
            statsTotal.ExpertiseRating = statsRace.ExpertiseRating + statsGearEnchantsBuffs.ExpertiseRating;
            statsTotal.HasteRating = statsRace.HasteRating + statsGearEnchantsBuffs.HasteRating;
            // Haste Trinkets
            statsTotal.HasteRating += statsGearEnchantsBuffs.HasteRatingOnPhysicalAttack * 10f / 45f;
            statsTotal.HitRating = statsRace.HitRating + statsGearEnchantsBuffs.HitRating;
            statsTotal.WeaponDamage = statsRace.WeaponDamage + statsGearEnchantsBuffs.WeaponDamage;
            statsTotal.ExposeWeakness = statsRace.ExposeWeakness + statsGearEnchantsBuffs.ExposeWeakness;

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            CharacterCalculationsProtPaladin calculations = GetCharacterCalculations(character) as CharacterCalculationsProtPaladin;

            switch (chartName)
            {
                case "Ability Damage":
                case "Ability Threat":
                    {
                        ComparisonCalculationBase[] comparisons = new ComparisonCalculationBase[calculations.Abilities.Count];
                        int j = 0;
                        foreach (System.Collections.DictionaryEntry abilities in calculations.Abilities)
                        {
                            AbilityModel ability = (AbilityModel)abilities.Value;
                            ComparisonCalculationProtPaladin comparison = new ComparisonCalculationProtPaladin();

                            comparison.Name = ability.Name;
                            if (chartName == "Ability Damage")
                                comparison.MitigationPoints = ability.Damage;
                            if (chartName == "Ability Threat")
                                comparison.ThreatPoints = ability.Threat;

                            comparison.OverallPoints = comparison.SurvivalPoints + comparison.ThreatPoints + comparison.MitigationPoints;
                            comparisons[j] = comparison;
                            j++;
                        }
                        return comparisons;
                    }
                case "Combat Table":
                    {
                        ComparisonCalculationProtPaladin calcMiss = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcDodge = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcParry = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcBlock = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcCrit = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcCrush = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcHit = new ComparisonCalculationProtPaladin();
                        if (calculations != null)
                        {
                            calcMiss.Name = "Miss";
                            calcDodge.Name = "Dodge";
                            calcParry.Name = "Parry";
                            calcBlock.Name = "Block";
                            calcCrit.Name = "Crit";
                            calcCrush.Name = "Crush";
                            calcHit.Name = "Hit";

                            calcMiss.OverallPoints = calcMiss.MitigationPoints = calculations.Miss * 100.0f;
                            calcDodge.OverallPoints = calcDodge.MitigationPoints = calculations.Dodge * 100.0f;
                            calcParry.OverallPoints = calcParry.MitigationPoints = calculations.Parry * 100.0f;
                            calcBlock.OverallPoints = calcBlock.MitigationPoints = calculations.Block * 100.0f;
                            calcCrit.OverallPoints = calcCrit.SurvivalPoints = calculations.CritVulnerability * 100.0f;
                            calcHit.OverallPoints = calcHit.SurvivalPoints = (1.0f - (calculations.DodgePlusMissPlusParryPlusBlock + calculations.CritVulnerability)) * 100.0f;
                        }
                        return new ComparisonCalculationBase[] { calcMiss, calcDodge, calcParry, calcBlock, calcCrit, calcCrush, calcHit };
                    }
                case "Item Budget":
                    CharacterCalculationsProtPaladin calcBaseValue = GetCharacterCalculations(character) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcDodgeValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DodgeRating = 10f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcParryValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { ParryRating = 10f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcBlockValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { BlockRating = 10f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcHasteValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 10f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcExpertiseValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { ExpertiseRating = 10f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcHitValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 10f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcBlockValueValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { BlockValue = 10f / 0.65f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcHealthValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Health = (10f * 10f) / 0.667f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcResilValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Resilience = 10f } }) as CharacterCalculationsProtPaladin;

                    //Differential Calculations for Agi
                    CharacterCalculationsProtPaladin calcAtAdd = calcBaseValue;
                    float agiToAdd = 0f;
                    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && agiToAdd < 2)
                    {
                        agiToAdd += 0.01f;
                        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToAdd } }) as CharacterCalculationsProtPaladin;
                    }

                    CharacterCalculationsProtPaladin calcAtSubtract = calcBaseValue;
                    float agiToSubtract = 0f;
                    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && agiToSubtract > -2)
                    {
                        agiToSubtract -= 0.01f;
                        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToSubtract } }) as CharacterCalculationsProtPaladin;
                    }
                    agiToSubtract += 0.01f;

                    ComparisonCalculationProtPaladin comparisonAgi = new ComparisonCalculationProtPaladin()
                    {
                        Name = "10 Agility",
                        OverallPoints = 10 * (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (agiToAdd - agiToSubtract),
                        MitigationPoints = 10 * (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (agiToAdd - agiToSubtract),
                        SurvivalPoints = 10 * (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (agiToAdd - agiToSubtract),
                        ThreatPoints = 10 * (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (agiToAdd - agiToSubtract)
                    };

                    //Differential Calculations for Str
                    calcAtAdd = calcBaseValue;
                    float strToAdd = 0f;
                    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && strToAdd < 2)
                    {
                        strToAdd += 0.01f;
                        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToAdd } }) as CharacterCalculationsProtPaladin;
                    }

                    calcAtSubtract = calcBaseValue;
                    float strToSubtract = 0f;
                    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && strToSubtract > -2)
                    {
                        strToSubtract -= 0.01f;
                        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToSubtract } }) as CharacterCalculationsProtPaladin;
                    }
                    strToSubtract += 0.01f;

                    ComparisonCalculationProtPaladin comparisonStr = new ComparisonCalculationProtPaladin()
                    {
                        Name = "10 Strength",
                        OverallPoints = 10 * (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (strToAdd - strToSubtract),
                        MitigationPoints = 10 * (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (strToAdd - strToSubtract),
                        SurvivalPoints = 10 * (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (strToAdd - strToSubtract),
                        ThreatPoints = 10 * (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (strToAdd - strToSubtract)
                    };


                    //Differential Calculations for Def
                    calcAtAdd = calcBaseValue;
                    float defToAdd = 0f;
                    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && defToAdd < 20)
                    {
                        defToAdd += 0.01f;
                        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DefenseRating = defToAdd } }) as CharacterCalculationsProtPaladin;
                    }

                    calcAtSubtract = calcBaseValue;
                    float defToSubtract = 0f;
                    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && defToSubtract > -20)
                    {
                        defToSubtract -= 0.01f;
                        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DefenseRating = defToSubtract } }) as CharacterCalculationsProtPaladin;
                    }
                    defToSubtract += 0.01f;

                    ComparisonCalculationProtPaladin comparisonDef = new ComparisonCalculationProtPaladin()
                    {
                        Name = "10 Defense Rating",
                        OverallPoints = 10 * (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (defToAdd - defToSubtract),
                        MitigationPoints = 10 * (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (defToAdd - defToSubtract),
                        SurvivalPoints = 10 * (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (defToAdd - defToSubtract),
                        ThreatPoints = 10 * (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (defToAdd - defToSubtract)
                    };


                    //Differential Calculations for AC
                    calcAtAdd = calcBaseValue;
                    float acToAdd = 0f;
                    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && acToAdd < 2)
                    {
                        acToAdd += 0.01f;
                        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Armor = acToAdd } }) as CharacterCalculationsProtPaladin;
                    }

                    calcAtSubtract = calcBaseValue;
                    float acToSubtract = 0f;
                    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && acToSubtract > -2)
                    {
                        acToSubtract -= 0.01f;
                        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Armor = acToSubtract } }) as CharacterCalculationsProtPaladin;
                    }
                    acToSubtract += 0.01f;

                    ComparisonCalculationProtPaladin comparisonAC = new ComparisonCalculationProtPaladin()
                    {
                        Name = "100 Armor",
                        OverallPoints = 100f * (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (acToAdd - acToSubtract),
                        MitigationPoints = 100f * (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (acToAdd - acToSubtract),
                        SurvivalPoints = 100f * (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (acToAdd - acToSubtract),
                        ThreatPoints = 100f * (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (acToAdd - acToSubtract),
                    };


                    //Differential Calculations for Sta
                    calcAtAdd = calcBaseValue;
                    float staToAdd = 0f;
                    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && staToAdd < 2)
                    {
                        staToAdd += 0.01f;
                        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = staToAdd } }) as CharacterCalculationsProtPaladin;
                    }

                    calcAtSubtract = calcBaseValue;
                    float staToSubtract = 0f;
                    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && staToSubtract > -2)
                    {
                        staToSubtract -= 0.01f;
                        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = staToSubtract } }) as CharacterCalculationsProtPaladin;
                    }
                    staToSubtract += 0.01f;

                    ComparisonCalculationProtPaladin comparisonSta = new ComparisonCalculationProtPaladin()
                    {
                        Name = "15 Stamina",
                        OverallPoints = (10f / 0.667f) * (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (staToAdd - staToSubtract),
                        MitigationPoints = (10f / 0.667f) * (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (staToAdd - staToSubtract),
                        SurvivalPoints = (10f / 0.667f) * (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (staToAdd - staToSubtract),
                        ThreatPoints = (10f / 0.667f) * (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (staToAdd - staToSubtract)
                    };

                    return new ComparisonCalculationBase[] { 
                        comparisonStr,
						comparisonAgi,
						comparisonAC,
						comparisonSta,
						comparisonDef,
						new ComparisonCalculationProtPaladin() { Name = "10 Dodge Rating",
                            OverallPoints = (calcDodgeValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcDodgeValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcDodgeValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcDodgeValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "10 Parry Rating",
                            OverallPoints = (calcParryValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcParryValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcParryValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcParryValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "10 Block Rating",
                            OverallPoints = (calcBlockValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcBlockValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcBlockValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcBlockValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "10 Haste Rating",
                            OverallPoints = (calcHasteValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcHasteValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcHasteValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcHasteValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "10 Expertise Rating",
                            OverallPoints = (calcExpertiseValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcExpertiseValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcExpertiseValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcExpertiseValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "10 Hit Rating",
                            OverallPoints = (calcHitValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcHitValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcHitValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcHitValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "15.38 Block Value",
                            OverallPoints = (calcBlockValueValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcBlockValueValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcBlockValueValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcBlockValueValue.ThreatPoints - calcBaseValue.ThreatPoints)},
						new ComparisonCalculationProtPaladin() { Name = "150 Health",
                            OverallPoints = (calcHealthValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcHealthValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcHealthValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcHealthValue.ThreatPoints - calcBaseValue.ThreatPoints)},
						new ComparisonCalculationProtPaladin() { Name = "10 Resilience",
                            OverallPoints = (calcResilValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcResilValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcResilValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcResilValue.ThreatPoints - calcBaseValue.ThreatPoints)},
					};
                default:
                    return new ComparisonCalculationBase[0];
            }
        }
        //Hide the ranged weapon enchants. None of them apply to melee damage at all.
        //public override bool EnchantFitsInSlot(Enchant enchant, Character character, Item.ItemSlot slot)
        //{
        //    return enchant.Slot == Item.ItemSlot.Ranged ? false : base.EnchantFitsInSlot(enchant, character, slot);
        //}

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                AverageArmor = stats.AverageArmor,
                Stamina = stats.Stamina,
                Agility = stats.Agility,
                DodgeRating = stats.DodgeRating,
                ParryRating = stats.ParryRating,
                BlockRating = stats.BlockRating,
                BlockValue = stats.BlockValue,
                DefenseRating = stats.DefenseRating,
                Resilience = stats.Resilience,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BaseArmorMultiplier = stats.BaseArmorMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                Health = stats.Health,
                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                DamageTakenMultiplier = stats.DamageTakenMultiplier,
                Miss = stats.Miss,
                CritChanceReduction = stats.CritChanceReduction,
                AllResist = stats.AllResist,
                ArcaneResistance = stats.ArcaneResistance,
                NatureResistance = stats.NatureResistance,
                FireResistance = stats.FireResistance,
                FrostResistance = stats.FrostResistance,
                ShadowResistance = stats.ShadowResistance,
                ArcaneResistanceBuff = stats.ArcaneResistanceBuff,
                NatureResistanceBuff = stats.NatureResistanceBuff,
                FireResistanceBuff = stats.FireResistanceBuff,
                FrostResistanceBuff = stats.FrostResistanceBuff,
                ShadowResistanceBuff = stats.ShadowResistanceBuff,

                Strength = stats.Strength,
                AttackPower = stats.AttackPower,
                SpellPower = stats.SpellPower,
                CritRating = stats.CritRating,
                PhysicalCrit = stats.PhysicalCrit,
                HitRating = stats.HitRating,
                SpellHitRating = stats.SpellHitRating,
                PhysicalHit = stats.PhysicalHit,
                SpellHit = stats.SpellHit,
                HasteRating = stats.HasteRating,
                PhysicalHaste = stats.PhysicalHaste,
                ExpertiseRating = stats.ExpertiseRating,
                ArmorPenetration = stats.ArmorPenetration,
                WeaponDamage = stats.WeaponDamage,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusBlockValueMultiplier = stats.BonusBlockValueMultiplier,

                MongooseProc = stats.MongooseProc,
                MongooseProcAverage = stats.MongooseProcAverage,
                MongooseProcConstant = stats.MongooseProcConstant,

                ExecutionerProc = stats.ExecutionerProc,

                JudgementBlockValue = stats.JudgementBlockValue,
                BonusHammerOfTheRighteousMultiplier = stats.BonusHammerOfTheRighteousMultiplier,
                ShieldOfRighteousnessBlockValue = stats.ShieldOfRighteousnessBlockValue,
                BonusSealOfCorruptionDamageMultiplier = stats.BonusSealOfCorruptionDamageMultiplier,
                BonusSealOfRighteousnessDamageMultiplier = stats.BonusSealOfRighteousnessDamageMultiplier,
                BonusSealOfVengeanceDamageMultiplier = stats.BonusSealOfVengeanceDamageMultiplier,
                ConsecrationSpellPower = stats.ConsecrationSpellPower,
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (
                // Basic Stats
                stats.Strength +
                stats.Agility +
                stats.Stamina +
                stats.Health +
                // Tanking Stats
                stats.Health +
                stats.Armor +
                stats.BonusArmor + 
                stats.Miss + 
                stats.Resilience +  
                stats.ParryRating + 
                stats.BlockRating + 
                stats.BlockValue + 
                stats.DefenseRating + 
                stats.DodgeRating + 
                // Threat Stats
                stats.CritRating + 
                stats.PhysicalCrit + 
                stats.SpellCrit + 
                stats.WeaponDamage + 
                stats.AttackPower + 
                stats.SpellHitRating + 
                stats.SpellPower + 
                stats.HitRating + 
                stats.ExpertiseRating + 
                stats.ArmorPenetrationRating + 
                stats.ArmorPenetration + 
                // Paladin Stats
                stats.JudgementBlockValue +
                stats.BonusHammerOfTheRighteousMultiplier +
                stats.ConsecrationSpellPower +
                stats.ShieldOfRighteousnessBlockValue +
                stats.BonusSealOfCorruptionDamageMultiplier +
                stats.BonusSealOfRighteousnessDamageMultiplier +
                stats.BonusSealOfVengeanceDamageMultiplier +
                // Multipliers
                stats.BonusStrengthMultiplier +
                stats.BonusAgilityMultiplier +
                stats.BonusStaminaMultiplier +
                stats.BonusHealthMultiplier + 
                stats.ThreatIncreaseMultiplier + 
                stats.BonusArmorMultiplier + 
                stats.BonusAttackPowerMultiplier +
                stats.BonusHolyDamageMultiplier +
                stats.BaseArmorMultiplier + 
                stats.DamageTakenMultiplier + 
                stats.BonusBlockValueMultiplier +
                // Resistances
                stats.AllResist +
                stats.ArcaneResistance + 
                stats.NatureResistance + 
                stats.FireResistance +
                stats.FrostResistance + 
                stats.ShadowResistance + 
                stats.ArcaneResistanceBuff +
                stats.NatureResistanceBuff + 
                stats.FireResistanceBuff +
                stats.FrostResistanceBuff + 
                stats.ShadowResistanceBuff 
                ) != 0;
        }
        /// <summary>
        /// Saves the talents for the character
        /// </summary>
        /// <param name="character">The character for whom the talents should be saved</param>
        public void GetTalents(Character character)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            calcOpts.talents = character.PaladinTalents;
        }
    }

    public class ProtPaladin
    {
        public static readonly float AgilityToDodge = 1.0f / 52.08333333f;
        public static readonly float DodgeRatingToDodge = 1.0f / 39.34798813f;
        public static readonly float StrengthToAP = 2.0f;
        public static readonly float StrengthToBlockValue = 1.0f / 2.0f;
        public static readonly float AgilityToArmor = 2.0f;
        public static readonly float AgilityToCrit = 1.0f / 52.08333333f;
        public static readonly float StaminaToHP = 10.0f;
        public static readonly float HitRatingToHit = 1.0f / 32.78998947f;
        public static readonly float CritRatingToCrit = 1.0f / 45.90598679f;
        public static readonly float HasteRatingToHaste = 1.0f / 32.78998947f;
        public static readonly float ExpertiseRatingToExpertise = 1.0f / (32.78998947f / 4f);
        public static readonly float ExpertiseToDodgeParryReduction = 0.25f;
        public static readonly float DefenseRatingToDefense = 1.0f / 4.918498039f;
        public static readonly float DefenseToDodge = 0.04f;
        public static readonly float DefenseToBlock = 0.04f;
        public static readonly float DefenseToParry = 0.04f;
        public static readonly float DefenseToMiss = 0.04f;
        public static readonly float DefenseToCritReduction = 0.04f;
        public static readonly float DefenseToDazeReduction = 0.04f;
        public static readonly float ParryRatingToParry = 1.0f / 49.18498611f;
        public static readonly float BlockRatingToBlock = 1.0f / 16.39499474f;
        public static readonly float ResilienceRatingToCritReduction = 1.0f / 81.97497559f;
        public static readonly float ArPToArmorPenetration = 1.0f / 15.395298f;
    }
}