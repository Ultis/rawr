using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
	[Rawr.Calculations.RawrModelInfo("ProtWarr", "Ability_Warrior_DefensiveStance", Character.CharacterClass.Warrior)]
	public class CalculationsProtWarr : CalculationsBase
    {
        #region Variables and Properties
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
                    "Defensive Stats:Chance Crushed",
                    "Defensive Stats:Mitigation",
					"Defensive Stats:Avoidance",
                    "Defensive Stats:Avoidance + Block",
					"Defensive Stats:Total Mitigation",
					"Defensive Stats:Damage Taken",

                    "Offensive Stats:Attack Power",
                    "Offensive Stats:Hit",
					"Offensive Stats:Expertise",
					"Offensive Stats:Haste",
					"Offensive Stats:Armor Penetration",
					"Offensive Stats:Crit",
					"Offensive Stats:Weapon Damage",
                    "Offensive Stats:Missed Attacks",
                    "Offensive Stats:Limited Threat*Shield Slam -> Revenge -> Devastate -> Devastate",
                    @"Offensive Stats:Unlimited Threat*Shield Slam -> Revenge -> Devastate -> Devastate.
With Heroic Strikes every auto attack.",

                    "Resistances:Nature Resist",
					"Resistances:Fire Resist",
					"Resistances:Frost Resist",
					"Resistances:Shadow Resist",
					"Resistances:Arcane Resist",
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
                    @"Complex Stats:Threat Points*Threat Points represents the average between unlimited
threat and limited threat scaled by the threat scale.",
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
        #endregion

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;
            int targetLevel = calcOpts.TargetLevel;

            Stats stats = GetCharacterStats(character, additionalItem);
            float levelDifference = (targetLevel - 80f) * 0.2f;
            CharacterCalculationsProtWarr calculatedStats = new CharacterCalculationsProtWarr();

            #region Tanking Calculations
            calculatedStats.BasicStats = stats;
			calculatedStats.TargetLevel = targetLevel;
			calculatedStats.ActiveBuffs = new List<Buff>(character.ActiveBuffs);

            float defSkill = (float)Math.Floor(stats.DefenseRating * ProtWarr.DefenseRatingToDefense);
            float dodgeNonDR = stats.Dodge + stats.BaseAgility * ProtWarr.AgilityToDodge - levelDifference;
            float missNonDR = stats.Miss * 100f - levelDifference;
            float parryNonDR = stats.Parry - levelDifference;
            float dodgePreDR = (stats.Agility - stats.BaseAgility) * ProtWarr.AgilityToDodge + stats.DodgeRating * ProtWarr.DodgeRatingToDodge +
                               defSkill * ProtWarr.DefenseToDodge;
            float missPreDR = defSkill * ProtWarr.DefenseToMiss;
            float parryPreDR = stats.ParryRating * ProtWarr.ParryRatingToParry + defSkill * ProtWarr.DefenseToParry;
            float dodgePostDR = 1f / (1f / 88.129021f + 0.9560f / dodgePreDR);
            float missPostDR = 1f / (1f / 50 + 0.9560f / missPreDR);
            float parryPostDR = 1f / (1f / 47.003525f + 0.9560f / parryPreDR);
            float dodgeTotal = dodgeNonDR + dodgePostDR;
            float missTotal = missNonDR + missPostDR;
            float parryTotal = parryNonDR + parryPostDR;

            calculatedStats.Miss = missTotal;
            calculatedStats.Dodge = Math.Min(100f - calculatedStats.Miss, dodgeTotal);
            calculatedStats.Parry = Math.Min(100f - calculatedStats.Miss - calculatedStats.Dodge, parryTotal);
            calculatedStats.Defense = (float)Math.Floor(400f + stats.Defense + stats.DefenseRating *
                                      ProtWarr.DefenseRatingToDefense);

            float block = 5f + stats.BlockRating * ProtWarr.BlockRatingToBlock +
                                    (((float)Math.Floor(stats.Defense + stats.DefenseRating * ProtWarr.DefenseRatingToDefense)) * ProtWarr.DefenseToBlock) +
                                    stats.Block - levelDifference;
            calculatedStats.Block = Math.Min(100f - calculatedStats.Miss - calculatedStats.Dodge - calculatedStats.Parry, block);
            calculatedStats.BlockOverCap = ((block - calculatedStats.Block) > 0 ? (block - calculatedStats.Block) : 0.0f);
            calculatedStats.BlockValue = (float)Math.Floor((float)Math.Floor((stats.BlockValue * (1 + stats.BonusBlockValueMultiplier)) +
                                         ((float)Math.Floor(stats.Strength * ProtWarr.StrengthToBlockValue))) * (1f + (character.WarriorTalents.CriticalBlock * 0.1f)));
            calculatedStats.Mitigation = (stats.Armor / (stats.Armor - 22167.5f + (467.5f * targetLevel))) * 100f;
            calculatedStats.CappedMitigation = Math.Min(75f, calculatedStats.Mitigation);
            calculatedStats.DodgePlusMissPlusParry = calculatedStats.Dodge + calculatedStats.Miss + calculatedStats.Parry;
            calculatedStats.DodgePlusMissPlusParryPlusBlock = calculatedStats.Dodge + calculatedStats.Miss + calculatedStats.Parry + calculatedStats.Block;
            calculatedStats.CritReduction = (((float)Math.Floor(stats.Defense + stats.DefenseRating * ProtWarr.DefenseRatingToDefense)) * ProtWarr.DefenseToCritReduction) +
                                            (stats.Resilience * ProtWarr.ResilienceRatingToCritReduction);
            calculatedStats.CappedCritReduction = Math.Min(5f + levelDifference, calculatedStats.CritReduction);

            //Miss -> Dodge -> Parry -> Block -> Crushing Blow -> Critical Strikes -> Hit
            //Out of 100 attacks, you'll take...

            float crits = Math.Max(5f + levelDifference - calculatedStats.CritReduction, 0f);
            float blocked = Math.Min(100f - (crits + (calculatedStats.DodgePlusMissPlusParry)), calculatedStats.Block);
            //float crushes = targetLevel == 73 ? Math.Max(Math.Min(100f - (crits + (calculatedStats.DodgePlusMissPlusParryPlusBlock)), 15f) - stats.CritChanceReduction, 0f) : 0f;
            float hits = Math.Max(100f - (crits + /*crushes + */blocked + (calculatedStats.DodgePlusMissPlusParry)), 0f);

            //calculatedStats.CrushChance = crushes;

            float damagePerBossAttack = calcOpts.BossAttackValue;
            //Apply armor and multipliers for each attack type...
            crits *= (100f - calculatedStats.CappedMitigation) * .02f;
            //crushes *= (100f - calculatedStats.CappedMitigation) * .015f;
            hits *= (100f - calculatedStats.CappedMitigation) * .01f;
            blocked *= (100f - calculatedStats.CappedMitigation) * .01f * (1f - Math.Max((calculatedStats.BlockValue / (damagePerBossAttack * (100f - calculatedStats.CappedMitigation) / 100)), 0f));
            calculatedStats.DamageTaken = blocked + hits + /*crushes + */crits;
            calculatedStats.TotalMitigation = 100f - calculatedStats.DamageTaken;

            int mitigationScale = calcOpts.MitigationScale;

            calculatedStats.SurvivalPoints = (stats.Health / ((1f - (calculatedStats.CappedMitigation / 100f)) * 0.9f)); // / (buffs.ShadowEmbrace ? 0.95f : 1f);
            calculatedStats.MitigationPoints = mitigationScale * (1f * (1f / (calculatedStats.DamageTaken / 100f)) * 0.9f); // / (buffs.ShadowEmbrace ? 0.95f : 1f);

            float cappedResist = targetLevel * 5;

            float impDefStance = 0.9f *
                (1f - character.WarriorTalents.ImprovedDefensiveStance * 0.02f);

            calculatedStats.NatureSurvivalPoints = (float)(stats.Health / (((1f - (System.Math.Min(cappedResist, stats.NatureResistance + stats.AllResist) / cappedResist) * .75) * impDefStance)));
            calculatedStats.FrostSurvivalPoints = (float)(stats.Health / (((1f - (System.Math.Min(cappedResist, stats.FrostResistance + stats.AllResist) / cappedResist) * .75) * impDefStance)));
            calculatedStats.FireSurvivalPoints = (float)(stats.Health / (((1f - (System.Math.Min(cappedResist, stats.FireResistance + stats.AllResist) / cappedResist) * .75) * impDefStance)));
            calculatedStats.ShadowSurvivalPoints = (float)(stats.Health / (((1f - (System.Math.Min(cappedResist, stats.ShadowResistance + stats.AllResist) / cappedResist) * .75) * impDefStance)));
            calculatedStats.ArcaneSurvivalPoints = (float)(stats.Health / (((1f - (System.Math.Min(cappedResist, stats.ArcaneResistance + stats.AllResist) / cappedResist) * .75) * impDefStance)));
            #endregion

            #region Threat Calcuations
            float targetArmor = calcOpts.TargetArmor;
            float baseArmor = Math.Max(0f, targetArmor - stats.ArmorPenetration);
            float modArmor = 1 - (baseArmor / (baseArmor + 10557.5f));

            float critMultiplier = 2 * (1 + stats.BonusCritMultiplier);
            float attackPower = stats.AttackPower;


            float hasteBonus = stats.HasteRating * ProtWarr.HasteRatingToHaste / 100f;

            float weaponSpeed;
            float weaponMinDamage;
            float weaponMaxDamage;
            float normalizedSpeed;

            if (character != null && character.MainHand != null)
            {
                weaponSpeed = character.MainHand.Speed;
                weaponMinDamage = character.MainHand.MinDamage;
                weaponMaxDamage = character.MainHand.MaxDamage;
                if(character.MainHand.Type == Item.ItemType.Dagger)
                    normalizedSpeed = 1.7f;
                else
                    normalizedSpeed = 2.4f;
            }
            else
            {
                //Assume a lvl 70 green dagger if the main hand is empty
                //so that threat values are not crazy for weapons.
                weaponSpeed = 1.7f;
                weaponMinDamage = 77f;
                weaponMaxDamage = 144f;
                normalizedSpeed = 1.7f;
            }

            float attackSpeed = (weaponSpeed) / (1f + hasteBonus);
            if (attackSpeed < 1f)
                attackSpeed = 1.0f;

            float hitBonus = (stats.HitRating * ProtWarr.HitRatingToHit + stats.PhysicalHit) / 100f;
            float expertiseBonus = (stats.ExpertiseRating * ProtWarr.ExpertiseRatingToExpertise +
                                    stats.Expertise) * ProtWarr.ExpertiseToDodgeParryReduction / 100f;


            float chanceCrit = Math.Min(0.75f, (stats.CritRating * ProtWarr.CritRatingToCrit +
                                                stats.LotPCritRating * ProtWarr.CritRatingToCrit +
                                               (stats.Agility * ProtWarr.AgilityToCrit) +
                                                stats.PhysicalCrit - levelDifference) / 100f);
            calculatedStats.Crit = chanceCrit * 100f;
            float chanceDodge = Math.Max(0f, 0.064f - expertiseBonus);
            float chanceParry = Math.Max(0f, 0.1375f - expertiseBonus);
            if((targetLevel - 80f) < 3)
                chanceParry = Math.Max(0f, 0.065f - expertiseBonus);
            
            /* Hit Chance
              * v. Level 80 mob: 5.0% / dual-wield: 24%
              * v. Level 81 mob: 5.5% / dual-wield: 24.5%
              * v. Level 82 mob: 6.0% / dual-wield: 25%
              * v. Level 83 mob: 8.0% / dual-wield: 27%
            */
            float chanceMiss = Math.Max(0f, 0.08f - hitBonus);
            if((targetLevel - 80f) < 3)
                chanceMiss = Math.Max(0f, 0.05f + 0.005f * (targetLevel - 80f) - hitBonus);

            float defStanceThreatMod = 2.0735f * (1 + stats.ThreatIncreaseMultiplier);
            float chanceAvoided = chanceMiss + chanceDodge + chanceParry;


            calculatedStats.AvoidedAttacks = (chanceMiss + chanceDodge + chanceParry) * 100f;
            calculatedStats.DodgedAttacks = chanceDodge * 100f;
            calculatedStats.ParriedAttacks = chanceParry * 100f;
            calculatedStats.MissedAttacks = chanceMiss * 100f;

            float averageDamage = 1 - chanceAvoided + (1 + stats.BonusCritMultiplier) * chanceCrit;

            float reducedDamage = 0.9f * (1f + stats.BonusDamageMultiplier) * modArmor;

            float weaponDamage = ((weaponMinDamage + weaponMaxDamage) / 2f +
                                 (attackSpeed * attackPower / 14f)) + stats.WeaponDamage;
            float normalizedWeaponDamage = ((weaponMinDamage + weaponMaxDamage) / 2f +
                                            (normalizedSpeed * attackPower / 14f)) + stats.WeaponDamage;

            float chanceToGlance = 0.25f, glancingAmount = 0.35f;

            #region Windfury
            float avgTimeBetnWF = (attackSpeed / (1.0f - chanceAvoided)) * 5.0f;
            float wfAPIncrease = stats.WindfuryAPBonus;
            float wfHitPre = weaponDamage + (wfAPIncrease / 14f) * ((character.MainHand == null) ? 0 : character.MainHand.Speed);
            float wfHitPost = (wfHitPre * averageDamage) - (wfHitPre * glancingAmount * chanceToGlance);
            if (wfAPIncrease > 0)
            {
                wfHitPost *= reducedDamage;
            }
            else
            {
                wfHitPost = 0f;
            }
            float wfTPS = defStanceThreatMod * wfHitPost / avgTimeBetnWF;
            calculatedStats.WindfuryThreat = wfTPS;
            #endregion 

            float whiteHitNoGlancePost = weaponDamage * averageDamage;
            float whiteHitPost = whiteHitNoGlancePost - (weaponDamage * glancingAmount * chanceToGlance);
            float whiteTPS = defStanceThreatMod * whiteHitPost / attackSpeed;
            calculatedStats.WhiteThreat = (float)Math.Round(whiteTPS, 2);

            //Max threat cycle is shield slam -> revenge -> devastate -> devastate -> repeat
            //with as many herioc strikes as possible.
            //For first wotlk rev I'm going to keep this cycle and model a flat percentage for Sword and Board
            //First rev will only include full sword and board talent specs for the threat model.
            //Later revs we'll want to do cycles and put in concussion and shockwave (which do large amounts of threat now)
            //In this basic model going to assume unlimited threat is heroic every swing, and
            //limited threat is zero heroic strikes.

            float shieldSlamTPS = (770f + defStanceThreatMod * reducedDamage *
                                  ((990f + 1040f) / 2f + calculatedStats.BlockValue) * averageDamage *
                                  (1f + stats.BonusShieldSlamDamage)) * (47f / 225f); //This last number is a hardcoded number from a spreadsheet I made on ability useage based on sword&board
            calculatedStats.ShieldSlamThreat = (float)Math.Round(shieldSlamTPS, 2);

            float revengeTPS = (121f + defStanceThreatMod * reducedDamage *
                               ((1454f + 1776f) / 2f + attackPower * 0.207f) * averageDamage) * (36f / 225f);
            calculatedStats.RevengeThreat = (float)Math.Round(revengeTPS, 2);

            float devastateTPS = attackPower * 0.05f + (defStanceThreatMod * reducedDamage *
                                 (0.5f * normalizedWeaponDamage + 101f * 5f) * averageDamage) * (67f / 225f);
            calculatedStats.DevastateThreat = (float)Math.Round(devastateTPS, 2);

            float heroicStrikeTPS = (259f + whiteHitNoGlancePost + 495) * averageDamage / weaponSpeed;
            calculatedStats.HeroicStrikeThreat = (float)Math.Round(heroicStrikeTPS, 2);


            calculatedStats.ThreatScale = calcOpts.ThreatScale;

            calculatedStats.LimitedThreat = calculatedStats.ThreatScale * (whiteTPS + shieldSlamTPS + revengeTPS + devastateTPS + wfTPS);
            calculatedStats.UnlimitedThreat = calculatedStats.ThreatScale * (heroicStrikeTPS + shieldSlamTPS + revengeTPS + devastateTPS + wfTPS);
            calculatedStats.ThreatPoints = (calculatedStats.LimitedThreat +
                                            calculatedStats.UnlimitedThreat) / 2f;
            #endregion

            calculatedStats.OverallPoints = calculatedStats.MitigationPoints + calculatedStats.SurvivalPoints + calculatedStats.ThreatPoints;
            return calculatedStats;
        }

        #region Warrior Race Stats
        private static float[,] BaseWarriorRaceStats = new float[,] 
		{
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
                        PhysicalCrit = 3.9f - (91f / 33f),
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
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[1, 0],
                        Agility = (float)BaseWarriorRaceStats[1, 1],
                        Stamina = (float)BaseWarriorRaceStats[1, 2],

                        AttackPower = 220f,
                        Dodge = 3.4636f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 3.9f - (91f / 33f),
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
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[2, 0],
                        Agility = (float)BaseWarriorRaceStats[2, 1],
                        Stamina = (float)BaseWarriorRaceStats[2, 2],

                        AttackPower = 220f,
                        Dodge = 3.4636f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 3.9f - (91f / 33f),
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
                        PhysicalCrit = 3.9f - (91f / 33f),
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
                        PhysicalCrit = 3.9f - (91f / 33f),
                    };
                    break;
                case Character.CharacterRace.Tauren:
                    statsRace = new Stats()
                    {
                        Health = 7941f,
                        Strength = (float)BaseWarriorRaceStats[5, 0],
                        Agility = (float)BaseWarriorRaceStats[5, 1],
                        Stamina = (float)BaseWarriorRaceStats[5, 2],

                        AttackPower = 220f,
                        BonusHealthMultiplier = 0.05f,
                        Dodge = 3.4636f,
                        Parry = 5f,
                        Miss = 0.05f,
                        PhysicalCrit = 3.9f - (91f / 33f),
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
                        PhysicalCrit = 3.9f - (91f / 33f),
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
                        PhysicalCrit = 3.9f - (91f / 33f),
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
                        PhysicalCrit = 3.9f - (91f / 33f),
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
            WarriorTalents tree = character.WarriorTalents;

            Stats statsTalents = new Stats()
                {
                    Parry = tree.Deflection * 1.0f,
                    PhysicalCrit = tree.Cruelty * 1.0f,
                    Dodge = tree.Anticipation * 1.0f,
                    Block = tree.ShieldSpecialization * 1.0f,
                    BonusBlockValueMultiplier = tree.ShieldMastery * 0.15f,
                    BonusDamageMultiplier = tree.OneHandedWeaponSpecialization * 0.02f,
                    BonusStaminaMultiplier = tree.Vitality * 0.02f,
                    BonusStrengthMultiplier = tree.Vitality * 0.02f,
                    Expertise = tree.Vitality * 2f,
                    BonusShieldSlamDamage = tree.GagOrder * 0.05f,
                };

            float oneProcPerMinAveUptime = 0f;
            float procChance = 0f;
            float attacksPer15Seconds = 0f;
            float attacksPerCycle = 3f;
            float cycleLength = 6f;
            float procDuration = 15f;
            float attackSpeed = 0f;

            if (character.MainHand != null && character.MainHandEnchant != null)
            {
                procChance = character.MainHand.Speed / 60f; // 1 PPM
                attackSpeed = character.MainHand.Speed;

                // Assumes the default threat cycle (SS, Revenge, Devastate x2)
                // Only 3 of those special attacks per 6 second cycle can proc weapon enchants
            }

            Stats statsItems = GetItemStats(character, additionalItem);
            float toughnessMultiplier = new float[] { 1f, 1.02f, 1.04f, 1.06f, 1.08f, 1.1f }[character.WarriorTalents.Toughness];
            statsItems.Armor *= toughnessMultiplier;

            //Mongoose
            if (statsEnchants.MongooseProc > 0 && statsBuffs.MongooseProcAverage > 0)
            {
                attacksPer15Seconds = procDuration / (attackSpeed / (1.02f)) +
                    attacksPerCycle * procDuration / cycleLength;
                oneProcPerMinAveUptime = (float)(1f - Math.Pow(1f - procChance, attacksPer15Seconds));
                statsBuffs.Agility += 120f * oneProcPerMinAveUptime;
                statsBuffs.HasteRating += (2f / ProtWarr.HasteRatingToHaste) *
                                           oneProcPerMinAveUptime;
            }
            else if (statsEnchants.MongooseProc > 0 && statsBuffs.MongooseProcConstant > 0)
            {
                statsBuffs.Agility += 120f;
                statsBuffs.HasteRating += 2f / ProtWarr.HasteRatingToHaste;
            }

            //Executioner
            if (statsEnchants.ExecutionerProc > 0)
            {
                attacksPer15Seconds = procDuration / attackSpeed +
                    attacksPerCycle * procDuration / cycleLength;
                oneProcPerMinAveUptime = (float)(1f - Math.Pow(1f - procChance, attacksPer15Seconds));
                statsBuffs.ArmorPenetration += 840f * oneProcPerMinAveUptime;
            }

            if (character.ActiveBuffsContains("Commanding Shout"))
            {
                statsBuffs.Health += statsBuffs.BonusCommandingShoutHP;
            }

            Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;
            Stats statsTotal = statsRace + statsItems + statsEnchants + statsBuffs + statsTalents;		
			
            statsTotal.Stamina *= (1 + statsTotal.BonusStaminaMultiplier);
            statsTotal.Strength *= (1 + statsTotal.BonusStrengthMultiplier);
            statsTotal.Agility *= (1 + statsTotal.BonusAgilityMultiplier);
            statsTotal.AttackPower += statsTotal.Strength * 2;
            statsTotal.AttackPower *= (1 + statsTotal.BonusAttackPowerMultiplier);
            statsTotal.Health += statsTotal.Stamina * 10f;
            statsTotal.Armor += 2 * statsTotal.Agility;
            statsTotal.Armor *= 1 + statsTotal.BonusArmorMultiplier;
            statsTotal.BlockValue *= 1 + statsTotal.BonusBlockValueMultiplier;
            statsTotal.NatureResistance += statsTotal.NatureResistanceBuff + statsTotal.AllResist;
            statsTotal.FireResistance += statsTotal.FireResistanceBuff + statsTotal.AllResist;
            statsTotal.FrostResistance += statsTotal.FrostResistanceBuff + statsTotal.AllResist;
            statsTotal.ShadowResistance += statsTotal.ShadowResistanceBuff + statsTotal.AllResist;
            statsTotal.ArcaneResistance += statsTotal.ArcaneResistanceBuff + statsTotal.AllResist;

            statsTotal.Block += (calcOpts.UseShieldBlock ? 75f : 0f);

            statsTotal.BaseAgility = statsRace.Agility + statsTalents.Agility;
 
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
            statsTotal.WindfuryAPBonus = statsGearEnchantsBuffs.WindfuryAPBonus;

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

						float crits = 5f + (0.2f * (currentCalculationsProtWarr.TargetLevel - 80f)) - currentCalculationsProtWarr.CappedCritReduction;
                        float crushes = currentCalculationsProtWarr.CrushChance;
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

                case "Item Budget":
					CharacterCalculationsProtWarr calcBaseValue = GetCharacterCalculations(character) as CharacterCalculationsProtWarr;
					CharacterCalculationsProtWarr calcDodgeValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DodgeRating = 10f } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcParryValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { ParryRating = 10f } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcBlockValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { BlockRating = 10f } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcHasteValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 10f } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcExpertiseValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { ExpertiseRating = 10f } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcHitValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 10f } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcBlockValueValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { BlockValue = 10f / 0.65f } }) as CharacterCalculationsProtWarr;
					CharacterCalculationsProtWarr calcHealthValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Health = (10f * 10f) / 0.667f  } }) as CharacterCalculationsProtWarr;
					CharacterCalculationsProtWarr calcResilValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Resilience = 10f } }) as CharacterCalculationsProtWarr;
					
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
                        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToAdd } }) as CharacterCalculationsProtWarr;
                    }

                    calcAtSubtract = calcBaseValue;
                    float strToSubtract = 0f;
                    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && strToSubtract > -2)
                    {
                        strToSubtract -= 0.01f;
                        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToSubtract } }) as CharacterCalculationsProtWarr;
                    }
                    strToSubtract += 0.01f;

                    ComparisonCalculationProtWarr comparisonStr = new ComparisonCalculationProtWarr()
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
						new ComparisonCalculationProtWarr() { Name = "10 Dodge Rating",
                            OverallPoints = (calcDodgeValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcDodgeValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcDodgeValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcDodgeValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtWarr() { Name = "10 Parry Rating",
                            OverallPoints = (calcParryValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcParryValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcParryValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcParryValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtWarr() { Name = "10 Block Rating",
                            OverallPoints = (calcBlockValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcBlockValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcBlockValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcBlockValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtWarr() { Name = "10 Haste Rating",
                            OverallPoints = (calcHasteValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcHasteValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcHasteValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcHasteValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtWarr() { Name = "10 Expertise Rating",
                            OverallPoints = (calcExpertiseValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcExpertiseValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcExpertiseValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcExpertiseValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtWarr() { Name = "10 Hit Rating",
                            OverallPoints = (calcHitValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcHitValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcHitValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcHitValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtWarr() { Name = "15.38 Block Value",
                            OverallPoints = (calcBlockValueValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcBlockValueValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcBlockValueValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcBlockValueValue.ThreatPoints - calcBaseValue.ThreatPoints)},
						new ComparisonCalculationProtWarr() { Name = "150 Health",
                            OverallPoints = (calcHealthValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcHealthValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcHealthValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcHealthValue.ThreatPoints - calcBaseValue.ThreatPoints)},
						new ComparisonCalculationProtWarr() { Name = "10 Resilience",
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
				BonusArmorMultiplier = stats.BonusArmorMultiplier,
				BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
				Health = stats.Health,
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
                CritRating = stats.CritRating,
                HitRating = stats.HitRating,
                HasteRating = stats.HasteRating,
                ExpertiseRating = stats.ExpertiseRating,
                ArmorPenetration = stats.ArmorPenetration,
                WeaponDamage = stats.WeaponDamage,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusBlockValueMultiplier = stats.BonusBlockValueMultiplier,
                LotPCritRating = stats.LotPCritRating,
                WindfuryAPBonus = stats.WindfuryAPBonus,

                MongooseProc = stats.MongooseProc,
                MongooseProcAverage = stats.MongooseProcAverage,
                MongooseProcConstant = stats.MongooseProcConstant,

                ExecutionerProc = stats.ExecutionerProc,

                BonusCommandingShoutHP = stats.BonusCommandingShoutHP,
                BonusShieldSlamDamage = stats.BonusShieldSlamDamage

			};
		}

		public override bool HasRelevantStats(Stats stats)
		{
			return (stats.Agility + stats.Armor + stats.AverageArmor +
                    stats.BonusAgilityMultiplier + stats.BonusStrengthMultiplier +
                    stats.BonusAttackPowerMultiplier + stats.BonusArmorMultiplier +
				    stats.BonusStaminaMultiplier + stats.DefenseRating + stats.DodgeRating + stats.ParryRating +
                    stats.BlockRating + stats.BlockValue + stats.Health + 
				    stats.Miss + stats.Resilience + stats.Stamina + stats.AllResist +
				    stats.ArcaneResistance + stats.NatureResistance + stats.FireResistance +
					stats.FrostResistance + stats.ShadowResistance + stats.ArcaneResistanceBuff +
					stats.NatureResistanceBuff + stats.FireResistanceBuff +
					stats.FrostResistanceBuff + stats.ShadowResistanceBuff + 
                    stats.Strength + stats.AttackPower + stats.CritRating + stats.HitRating + stats.HasteRating +
                    stats.ExpertiseRating + stats.ArmorPenetration + stats.WeaponDamage +
                    stats.BonusCritMultiplier + stats.CritChanceReduction +
                    stats.ThreatIncreaseMultiplier + stats.BonusDamageMultiplier + stats.BonusBlockValueMultiplier +
                    stats.LotPCritRating + stats.WindfuryAPBonus +
                    stats.MongooseProc + stats.MongooseProcAverage + stats.MongooseProcConstant +
                    stats.ExecutionerProc +
                    stats.BonusCommandingShoutHP + stats.BonusShieldSlamDamage
                   ) != 0;
		}
    }

    public class ProtWarr
    {
        public static readonly float AgilityToDodge = 1.0f / 73.52941176f;
        public static readonly float DodgeRatingToDodge = 1.0f / 39.34798813f;
        public static readonly float StrengthToAP = 2.0f;
        public static readonly float StrengthToBlockValue = 1.0f / 2.0f;
        public static readonly float AgilityToArmor = 2.0f;
        public static readonly float AgilityToCrit = 1.0f / 62.5f;
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
    }
}
