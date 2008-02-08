using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
	public class CalculationsBear : CalculationsBase
	{
		//my insides all turned to ash / so slow
		//and blew away as i collapsed / so cold

        public override String DisplayName{get{return "Bear";}}

		private CalculationOptionsPanelBase _calculationOptionsPanel = null;
		public override CalculationOptionsPanelBase CalculationOptionsPanel
		{
			get
			{
				if (_calculationOptionsPanel == null)
				{
					_calculationOptionsPanel = new CalculationOptionsPanelBear();
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
					"Basic Stats:Dodge Rating",
					"Basic Stats:Defense Rating",
					"Basic Stats:Resilience",
					"Basic Stats:Nature Resist",
					"Basic Stats:Fire Resist",
					"Basic Stats:Frost Resist",
					"Basic Stats:Shadow Resist",
					"Basic Stats:Arcane Resist",
					"Complex Stats:Dodge",
					"Complex Stats:Miss",
					"Complex Stats:Mitigation",
					"Complex Stats:Dodge + Miss",
					"Complex Stats:Total Mitigation",
					"Complex Stats:Damage Taken",
					"Complex Stats:Chance to be Crit",
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
				}
				return _subPointNameColors;
			}
		}

		public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationBear(); }
		public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsBear(); }

		public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
		{
			_cachedCharacter = character;
			int targetLevel = int.Parse(character.CalculationOptions["TargetLevel"]);
			Stats stats = GetCharacterStats(character, additionalItem);
			float levelDifference = (targetLevel - 70f) * 0.2f;
			CharacterCalculationsBear calculatedStats = new CharacterCalculationsBear();
			calculatedStats.BasicStats = stats;
			calculatedStats.TargetLevel = targetLevel;
			calculatedStats.Miss = 5 + stats.DefenseRating / 60f + stats.Miss - levelDifference;
			calculatedStats.Dodge = Math.Min(100f - calculatedStats.Miss, stats.Agility / 14.7059f + stats.DodgeRating / 18.9231f + stats.DefenseRating / 59.134615f - levelDifference);
			calculatedStats.Mitigation = (stats.Armor / (stats.Armor - 22167.5f + (467.5f * targetLevel))) * 100f; //(stats.Armor / (stats.Armor + 11959.5f)) * 100f; for only 73s
			calculatedStats.CappedMitigation = Math.Min(75f, calculatedStats.Mitigation);
			calculatedStats.DodgePlusMiss = calculatedStats.Dodge + calculatedStats.Miss;
			calculatedStats.CritReduction = stats.DefenseRating / 60f + stats.Resilience / 39.423f;
			calculatedStats.CappedCritReduction = Math.Min(2f + levelDifference, calculatedStats.CritReduction);
			//Out of 100 attacks, you'll take...
			float crits = 2f + (0.2f * levelDifference) - calculatedStats.CappedCritReduction;
			float crushes = targetLevel == 73 ? Math.Max(Math.Min(100f - (crits + (calculatedStats.DodgePlusMiss)), 15f), 0f) : 0f;
			float hits = Math.Max(100f - (crits + crushes + (calculatedStats.DodgePlusMiss)), 0f);
			//Apply armor and multipliers for each attack type...
			crits *= (100f - calculatedStats.CappedMitigation) * .02f;
			crushes *= (100f - calculatedStats.CappedMitigation) * .015f;
			hits *= (100f - calculatedStats.CappedMitigation) * .01f;
			calculatedStats.DamageTaken = hits + crushes + crits;
			calculatedStats.TotalMitigation = 100f - calculatedStats.DamageTaken;

			calculatedStats.SurvivalPoints = (stats.Health / (1f - (calculatedStats.CappedMitigation / 100f))); // / (buffs.ShadowEmbrace ? 0.95f : 1f);
			calculatedStats.MitigationPoints = (7000f * (1f / (calculatedStats.DamageTaken / 100f))); // / (buffs.ShadowEmbrace ? 0.95f : 1f);
			calculatedStats.OverallPoints = calculatedStats.MitigationPoints + calculatedStats.SurvivalPoints;

            float cappedResist = targetLevel * 5;

            calculatedStats.NatureSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.NatureResistance + stats.AllResist) / cappedResist) * .75)));
            calculatedStats.FrostSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.FrostResistance + stats.AllResist) / cappedResist) * .75)));
            calculatedStats.FireSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.FireResistance + stats.AllResist) / cappedResist) * .75)));
            calculatedStats.ShadowSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.ShadowResistance + stats.AllResist) / cappedResist) * .75)));
            calculatedStats.ArcaneSurvivalPoints = (float) (stats.Health / ((1f - (System.Math.Min(cappedResist, stats.ArcaneResistance + stats.AllResist) / cappedResist) * .75)));

			return calculatedStats;
		}

		public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
			Stats statsRace = character.Race == Character.CharacterRace.NightElf ? new Stats() { Health = 3434, Agility = 75, Stamina = 82, DodgeRating = 59 } : new Stats() { Health = 3434, Agility = 64, Stamina = 85, DodgeRating = 40, NatureResistance=10 };
			Stats statsBaseGear = GetItemStats(character, additionalItem);
			Stats statsEnchants = GetEnchantsStats(character);
			Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
			
			statsBaseGear.Agility += statsEnchants.Agility;
			statsBaseGear.DefenseRating += statsEnchants.DefenseRating;
			statsBaseGear.DodgeRating += statsEnchants.DodgeRating;
			statsBaseGear.Resilience += statsEnchants.Resilience;
			statsBaseGear.Stamina += statsEnchants.Stamina;

			statsBuffs.Health += statsEnchants.Health;
			statsBuffs.Armor += statsEnchants.Armor;

			float agiBase = (float)Math.Floor(statsRace.Agility * 1.03f);
			float agiBonus = (float)Math.Floor((statsBaseGear.Agility + statsBuffs.Agility) * 1.03f);
			float staBase = (float)Math.Floor(statsRace.Stamina * 1.03f * 1.25f);
			float staBonus = (statsBaseGear.Stamina + statsBuffs.Stamina) * 1.03f * 1.25f;
			float staHotW = (statsRace.Stamina * 1.03f * 1.25f + staBonus) * 0.2f;
			staBonus = (float)Math.Round(Math.Floor(staBonus) + staHotW);

			Stats statsTotal = new Stats();
			statsTotal.Agility = agiBase + (float)Math.Floor((agiBase * statsBuffs.BonusAgilityMultiplier) + agiBonus * (1 + statsBuffs.BonusAgilityMultiplier));
			statsTotal.Stamina = staBase + (float)Math.Round((staBase * statsBuffs.BonusStaminaMultiplier) + staBonus * (1 + statsBuffs.BonusStaminaMultiplier));
			statsTotal.DefenseRating = statsRace.DefenseRating + statsBaseGear.DefenseRating + statsBuffs.DefenseRating;
			statsTotal.DodgeRating = statsRace.DodgeRating + statsBaseGear.DodgeRating + statsBuffs.DodgeRating;
			statsTotal.Resilience = statsRace.Resilience + statsBaseGear.Resilience + statsBuffs.Resilience;
			statsTotal.Health = (float)Math.Round(((statsRace.Health + statsBaseGear.Health + statsBuffs.Health + (statsTotal.Stamina * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
			statsTotal.Armor = (float)Math.Round(((statsBaseGear.Armor * 5.5f) + statsRace.Armor + statsBuffs.Armor + (statsTotal.Agility * 2f)) * (1 + statsBuffs.BonusArmorMultiplier));
			statsTotal.Miss = statsBuffs.Miss;
            statsTotal.NatureResistance = statsRace.NatureResistance + statsBaseGear.NatureResistance + statsBuffs.NatureResistance;
            statsTotal.FireResistance = statsRace.FireResistance + statsBaseGear.FireResistance + statsBuffs.FireResistance;
            statsTotal.FrostResistance = statsRace.FrostResistance + statsBaseGear.FrostResistance + statsBuffs.FrostResistance;
            statsTotal.ShadowResistance = statsRace.ShadowResistance + statsBaseGear.ShadowResistance + statsBuffs.ShadowResistance;
            statsTotal.ArcaneResistance = statsRace.ArcaneResistance + statsBaseGear.ArcaneResistance + statsBuffs.ArcaneResistance;
            statsTotal.AllResist = statsRace.AllResist + statsBaseGear.AllResist + statsBuffs.AllResist;
            return statsTotal;
		}

		public override ComparisonCalculationBase[] GetCombatTable(CharacterCalculationsBase currentCalculations)
		{
			CharacterCalculationsBear currentCalculationsBear = currentCalculations as CharacterCalculationsBear;
			ComparisonCalculationBear calcMiss = new ComparisonCalculationBear();
			ComparisonCalculationBear calcDodge = new ComparisonCalculationBear();
			ComparisonCalculationBear calcCrit = new ComparisonCalculationBear();
			ComparisonCalculationBear calcCrush = new ComparisonCalculationBear();
			ComparisonCalculationBear calcHit = new ComparisonCalculationBear();
			if (currentCalculationsBear != null)
			{
				calcMiss.Name = "    Miss    ";
				calcDodge.Name = "   Dodge   ";
				calcCrit.Name = "  Crit  ";
				calcCrush.Name = " Crush ";
				calcHit.Name = "Hit";



				float crits = 2f + (0.2f * (currentCalculationsBear.TargetLevel - 70)) - currentCalculationsBear.CappedCritReduction;
				float crushes = currentCalculationsBear.TargetLevel == 73 ? Math.Max(Math.Min(100f - (crits + (currentCalculationsBear.DodgePlusMiss)), 15f), 0f) : 0f;
				float hits = Math.Max(100f - (crits + crushes + (currentCalculationsBear.DodgePlusMiss)), 0f);

				calcMiss.OverallPoints = calcMiss.MitigationPoints = currentCalculationsBear.Miss;
				calcDodge.OverallPoints = calcDodge.MitigationPoints = currentCalculationsBear.Dodge;
				calcCrit.OverallPoints = calcCrit.SurvivalPoints = crits;
				calcCrush.OverallPoints = calcCrush.SurvivalPoints = crushes;
				calcHit.OverallPoints = calcHit.SurvivalPoints = hits;
			}
			return new ComparisonCalculationBase[] { calcMiss, calcDodge, calcCrit, calcCrush, calcHit };
		}

		public override Stats GetRelevantStats(Stats stats)
		{
			return new Stats()
			{
				Armor = stats.Armor,
				Stamina = stats.Stamina,
				Agility = stats.Agility,
				DodgeRating = stats.DodgeRating,
				DefenseRating = stats.DefenseRating,
				Resilience = stats.Resilience,
				TerrorProc = stats.TerrorProc,
				BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
				BonusArmorMultiplier = stats.BonusArmorMultiplier,
				BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
				Health = stats.Health,
				Miss = stats.Miss
			};
		}

		public override bool HasRelevantStats(Stats stats)
		{
			return (stats.Agility + stats.Armor + stats.BonusAgilityMultiplier + stats.BonusArmorMultiplier +
				stats.BonusStaminaMultiplier + stats.DefenseRating + stats.DodgeRating + stats.Health +
				stats.Miss + stats.Resilience + stats.Stamina + stats.TerrorProc) > 0;
		}

    }

    public class CharacterCalculationsBear : CharacterCalculationsBase
    {
		private float _overallPoints = 0f;
		public override float OverallPoints
		{
			get { return _overallPoints; }
			set { _overallPoints = value; }
		}

		private float[] _subPoints = new float[] { 0f, 0f };
		public override float[] SubPoints
		{
			get { return _subPoints; }
			set { _subPoints = value; }
		}

		public float MitigationPoints
		{
			get { return _subPoints[0]; }
			set { _subPoints[0] = value; }
		}

		public float SurvivalPoints
		{
			get { return _subPoints[1]; }
			set { _subPoints[1] = value; }
		}

		private Stats _basicStats;
		public Stats BasicStats
		{
			get { return _basicStats; }
			set { _basicStats = value; }
		}

		private int _targetLevel;
		public int TargetLevel
		{
			get { return _targetLevel; }
			set { _targetLevel = value; }
		}

		private float _dodge;
		public float Dodge
		{
			get { return _dodge; }
			set { _dodge = value; }
		}

		private float _miss;
		public float Miss
		{
			get { return _miss; }
			set { _miss = value; }
		}

		private float _mitigation;
		public float Mitigation
		{
			get { return _mitigation; }
			set { _mitigation = value; }
		}

		private float _cappedMitigation;
		public float CappedMitigation
		{
			get { return _cappedMitigation; }
			set { _cappedMitigation = value; }
		}

		private float _dodgePlusMiss;
		public float DodgePlusMiss
		{
			get { return _dodgePlusMiss; }
			set { _dodgePlusMiss = value; }
		}

		private float _totalMitigation;
		public float TotalMitigation
		{
			get { return _totalMitigation; }
			set { _totalMitigation = value; }
		}

		private float _damageTaken;
		public float DamageTaken
		{
			get { return _damageTaken; }
			set { _damageTaken = value; }
		}

		private float _critReduction;
		public float CritReduction
		{
			get { return _critReduction; }
			set { _critReduction = value; }
		}

		private float _cappedCritReduction;
		public float CappedCritReduction
		{
			get { return _cappedCritReduction; }
			set { _cappedCritReduction = value; }
		}

        public float NatureSurvivalPoints{get;set;}
        public float FrostSurvivalPoints{get;set;}
        public float FireSurvivalPoints{get;set;}
        public float ShadowSurvivalPoints{get;set;}
        public float ArcaneSurvivalPoints{get;set;}

		public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
		{
			Dictionary<string, string> dictValues = new Dictionary<string, string>();
			int armorCap = (int)Math.Ceiling((1402.5f * TargetLevel) - 66502.5f);
			float levelDifference = 0.2f * (TargetLevel - 70);

            dictValues["Nature Resist"] = (BasicStats.NatureResistance+BasicStats.AllResist).ToString();
            dictValues["Arcane Resist"] = (BasicStats.ArcaneResistance+BasicStats.AllResist).ToString();
            dictValues["Frost Resist"] = (BasicStats.FrostResistance+BasicStats.AllResist).ToString();
            dictValues["Fire Resist"] = (BasicStats.FireResistance+BasicStats.AllResist).ToString();
            dictValues["Shadow Resist"] = (BasicStats.ShadowResistance + BasicStats.AllResist).ToString();
			dictValues.Add("Health", BasicStats.Health.ToString());
			dictValues.Add("Agility", BasicStats.Agility.ToString());
			dictValues.Add("Armor", BasicStats.Armor.ToString());
			dictValues.Add("Stamina", BasicStats.Stamina.ToString());
			if (Calculations.CachedCharacter.Race == Character.CharacterRace.NightElf)
				dictValues.Add("Dodge Rating", (BasicStats.DodgeRating - 59).ToString());
			else
				dictValues.Add("Dodge Rating", (BasicStats.DodgeRating - 40).ToString());
			dictValues.Add("Defense Rating", BasicStats.DefenseRating.ToString());
			dictValues.Add("Resilience", BasicStats.Resilience.ToString());
			dictValues.Add("Dodge", Dodge.ToString() + "%");
			dictValues.Add("Miss", Miss.ToString() + "%");
			if (BasicStats.Armor == armorCap)
				dictValues.Add("Mitigation", Mitigation.ToString()
					+ string.Format("%*Exactly at the armor cap against level {0} mobs.", TargetLevel));
			else if (BasicStats.Armor > armorCap)
				dictValues.Add("Mitigation", Mitigation.ToString()
					+ string.Format("%*Over the armor cap by {0} armor.", BasicStats.Armor - armorCap));
			else
				dictValues.Add("Mitigation", Mitigation.ToString()
					+ string.Format("%*Short of the armor cap by {0} armor.", armorCap - BasicStats.Armor));
			dictValues.Add("Dodge + Miss", DodgePlusMiss.ToString() + "%");
			dictValues.Add("Total Mitigation", TotalMitigation.ToString() + "%");
			dictValues.Add("Damage Taken", DamageTaken.ToString() + "%");
			if (CritReduction == (2f + levelDifference))
				dictValues.Add("Chance to be Crit", ((2f + levelDifference) - CritReduction).ToString()
					+ "%*Exactly enough defense rating/resilience to be uncrittable by bosses.");
			else if (CritReduction < (2f + levelDifference))
				dictValues.Add("Chance to be Crit", ((2f + levelDifference) - CritReduction).ToString()
					+ string.Format("%*CRITTABLE! Short by {0} defense rating or {1} resilience to be uncrittable by bosses.",
					Math.Ceiling(((2f + levelDifference) - CritReduction) * 60f), Math.Ceiling(((2f + levelDifference) - CritReduction) * 39.423f)));
			else
				dictValues.Add("Chance to be Crit", ((2f + levelDifference) - CritReduction).ToString()
					+ string.Format("%*Uncrittable by bosses. {0} defense rating or {1} resilience over the crit cap.",
					Math.Floor(((2f + levelDifference) - CritReduction) * -60f), Math.Floor(((2f + levelDifference) - CritReduction) * -39.423f)));
			dictValues.Add("Overall Points", OverallPoints.ToString());
			dictValues.Add("Mitigation Points", MitigationPoints.ToString());
			dictValues.Add("Survival Points", SurvivalPoints.ToString());

            dictValues["Nature Survival"] = NatureSurvivalPoints.ToString();
            dictValues["Frost Survival"] = FrostSurvivalPoints.ToString();
            dictValues["Fire Survival"] = FireSurvivalPoints.ToString();
            dictValues["Shadow Survival"] = ShadowSurvivalPoints.ToString();
            dictValues["Arcane Survival"] = ArcaneSurvivalPoints.ToString(); 
			return dictValues;
		}
    }

	public class ComparisonCalculationBear : ComparisonCalculationBase
	{
		private string _name = string.Empty;
		public override string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private float _overallPoints = 0f;
		public override float OverallPoints
		{
			get { return _overallPoints; }
			set { _overallPoints = value; }
		}

		private float[] _subPoints = new float[] { 0f, 0f };
		public override float[] SubPoints
		{
			get { return _subPoints; }
			set { _subPoints = value; }
		}

		public float MitigationPoints
		{
			get { return _subPoints[0]; }
			set { _subPoints[0] = value; }
		}

		public float SurvivalPoints
		{
			get { return _subPoints[1]; }
			set { _subPoints[1] = value; }
		}

		private Item _item = null;
		public override Item Item
		{
			get { return _item; }
			set { _item = value; }
		}

		private bool _equipped = false;
		public override bool Equipped
		{
			get { return _equipped; }
			set { _equipped = value; }
		}

		public override string ToString()
		{
			return string.Format("{0}: ({1}O {2}M {3}S)", Name, Math.Round(OverallPoints), Math.Round(MitigationPoints), Math.Round(SurvivalPoints));
		}
	}
}