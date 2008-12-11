using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Elemental
{
	[Rawr.Calculations.RawrModelInfo("Elemental", "Spell_Nature_Lightning", Character.CharacterClass.Shaman)]
	public class CalculationsElemental : CalculationsBase
	{

        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
		public override CalculationOptionsPanelBase CalculationOptionsPanel
		{
			get
			{
				if (_calculationOptionsPanel == null)
				{
					_calculationOptionsPanel = new CalculationOptionsPanelElemental();
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
					"Summary:Overall Points*Sum of your DPS Points and Survivability Points",
					"Summary:DPS Points*DPS Points is your theoretical DPS.",
					"Summary:Sustainability Points*How long you can keep casting.",

					"Basic Stats:Health",
                    "Basic Stats:Mana",
                    "Basic Stats:Stamina",
					"Basic Stats:Intellect",
					"Basic Stats:Spell Power",
                    "Basic Stats:Hit Rating",
					"Basic Stats:Crit Rating",
                    "Basic Stats:Haste Rating",
					"Basic Stats:Mana Regen",
					
					"Complex Stats:Crit Chance",
					
					"Attacks:Lightning Bolt*Avg Dmg/Avg Crit",
					"Attacks:Chain Lightning*Avg Dmg/Avg Crit",
					"Attacks:Flame Shock(DD)*Avg Dmg/Avg Crit",
					"Attacks:Flame Shock(DoT)*Avg Dmg/Avg Crit",
					"Attacks:Lava Burst*Avg Dmg/Avg Crit",

					"Rotation:Optimal",
					"Rotation:Optimal DPS",
					"Rotation:Custom DPS",
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
					"Custom Rotation DPS",
					"Health",
                    "Nature Resist",
                    "Fire Resist",
                    "Frost Resist",
                    "Shadow Resist",
                    "Arcane Resist",
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
					"Relative Stat Values",
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
					_subPointNameColors.Add("DPS", System.Drawing.Color.FromArgb(160, 0, 224));
					_subPointNameColors.Add("Sustainability", System.Drawing.Color.FromArgb(64, 128, 32));
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
						Item.ItemType.Cloth,						
                        Item.ItemType.Leather,
                        Item.ItemType.Mail,
						Item.ItemType.Dagger,
                        Item.ItemType.FistWeapon,
						Item.ItemType.OneHandAxe,
                        Item.ItemType.OneHandMace,
                        Item.ItemType.Shield,
                        Item.ItemType.Staff,
                        Item.ItemType.Totem,
                        Item.ItemType.TwoHandAxe,
						Item.ItemType.TwoHandMace
					});
				}
				return _relevantItemTypes;
			}
		}

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Shaman; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationElemental(); }
		public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsElemental(); }

		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsElemental));
			System.IO.StringReader reader = new System.IO.StringReader(xml);
			CalculationOptionsElemental calcOpts = serializer.Deserialize(reader) as CalculationOptionsElemental;
			return calcOpts;
		}

		public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
		{
			//_cachedCharacter = character;
			CalculationOptionsElemental calcOpts = character.CalculationOptions as CalculationOptionsElemental;
			if (calcOpts == null) calcOpts = new CalculationOptionsElemental();
			int targetLevel = calcOpts.TargetLevel;
			Stats stats = GetCharacterStats(character, additionalItem);
			CharacterCalculationsElemental calculatedStats = new CharacterCalculationsElemental();
			calculatedStats.BasicStats = stats;
			calculatedStats.TargetLevel = targetLevel;

            #region Spell Coefficients

            float LightningBoltCoEff = 71.43f;
            float ChainLightningCoEff = 57.14f;
            float FlameShockDDCoeff = 21.4f;
            float FlameShockDoTCoeff = 10.0f;
            float LavaBurstCoEff = 67.14f;
            float ThunderstormCoeff = 0.0f;

            #endregion


            #region Basic Chances and Constants

            float hasteBonus = stats.HasteRating / 32.78998947f / 100f;
            float hitBonus = stats.HitRating / 26.23199272f / 100f;

			float chanceMiss = Math.Max(0f, 0.17f - hitBonus);
			if ((targetLevel - 80f) < 3)
			{
				chanceMiss = Math.Max(0f, 0.04f + (0.01f * (targetLevel - 80f)) - hitBonus);
			}

			float chanceCrit = (stats.CritRating / 45.90598679f / 100f) + (stats.Intellect / 166.6666709f / 100f);

			
			#endregion

			#region Attack Damages

            float LightningBoltDamage = 807.0f;
            float ChainLightningDamage = 1094.0f;
            float LavaBurstDamage = 1230.0f;
            float FlameShockDDDamage = 500.0f;
            float FlameShockDoTDamage = 139.0f;

			#endregion

			#region Mana Costs
			float LightningBoltManaRaw = 0.10f;
            float ChainLightningManaRaw = 0.26f;
            float LavaBurstManaRaw = 0.10f;
            float FlameShockManaRaw = 0.17f;

            #endregion

			#region Rotations
			ElementalRotationCalculator.ElementalRotationCalculation rotationCalculationDPS = new ElementalRotationCalculator.ElementalRotationCalculation();

/*			StringBuilder rotations = new StringBuilder();
			for (int roarCP = 1; roarCP < 6; roarCP++)
				for (int useShred = 0; useShred < 2; useShred++)
					for (int useRip = 0; useRip < 2; useRip++)
						for (int useFerociousBite = 0; useFerociousBite < 2; useFerociousBite++)
						{
						}
*/

			calculatedStats.HighestDPSRotation = rotationCalculationDPS;
			#endregion


			calculatedStats.MissedAttacks = chanceMiss * 100f;
			calculatedStats.CritChance = chanceCrit * 100f;

			calculatedStats.DPSPoints = calculatedStats.HighestDPSRotation.DPS;
			calculatedStats.SustainabilityPoints = stats.Health / 100f;
			calculatedStats.OverallPoints = calculatedStats.DPSPoints + calculatedStats.SustainabilityPoints;
			return calculatedStats;
			
		}

		public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
            Stats statsRace; 
            switch (character.Race)
            {
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Stamina = 113, Intellect = 109, Spirit = 122 };
                    break;
                case Character.CharacterRace.Orc:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Stamina = 138, Intellect = 125, Spirit = 146 };
                    break;
                case Character.CharacterRace.Tauren:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Stamina = 116, Intellect = 103, Spirit = 122 };
                    statsRace.BonusHealthMultiplier = 0.05f;
                    break;
                case Character.CharacterRace.Troll:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Stamina = 137, Intellect = 124, Spirit = 144 };
                    break;
                default :
                    statsRace = new Stats() { Health = 0, Mana = 0, Stamina = 0, Intellect = 0, Spirit = 0 };
                    break;
            }



			Stats statsItems = GetItemStats(character, additionalItem);
			Stats statsEnchants = GetEnchantsStats(character);
			Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);


			ShamanTalents talents = character.ShamanTalents;
			Stats statsTalents = new Stats()
            {
                #region Elemental
                BonusFireDamageMultiplier = 0.01f * talents.Concussion,
                BonusFrostDamageMultiplier = 0.01f * talents.Concussion,
                BonusNatureDamageMultiplier = 0.01f * talents.Concussion,
                BonusLavaBurstDamage = 0.02f * talents.CallOfFlame,
                BonusSpellCritMultiplier = 0.20f * talents.ElementalFury,
                BonusThunderCritChance = 0.05f * talents.CallOfThunder,
                BonusShamanHit = 0.01f * talents.ElementalPrecision,
                ThreatReductionMultiplier = 0.10f * talents.ElementalPrecision,
                ManaRegenIntPer5 = 0.02f * talents.UnrelentingStorm,
                ShamanCastTimeReduction = 0.1f * talents.LightningMastery,
                LightningOverloadProc = 0.04f * talents.LightningOverload,
                BonusLavaBurstCritDamage = 6f * talents.LavaFlows,
                ChainLightningCooldownReduction = 0.5f * talents.StormEarthAndFire,
                BonusFlameShockDoTDamage = 0.10f * talents.StormEarthAndFire,
                #endregion
                #region Enhancement
                BonusIntellectMultiplier = 0.02f * talents.AncestralKnowledge,
                BonusCritMultiplier = 1.00f * talents.ThunderingStrikes,
                BonusFlametongueDamage = 0.10f * talents.ElementalWeapons,
                ShockManaCostReduction = 0.45f * talents.ShamanisticFocus,
                #endregion

                
			};

			Stats statsGearEnchantsBuffs = statsItems + statsEnchants + statsBuffs;

			CalculationOptionsElemental calcOpts = character.CalculationOptions as CalculationOptionsElemental;
			
			Stats statsTotal = statsRace + statsItems + statsEnchants + statsBuffs + statsTalents;

            statsTotal.Stamina *= (1 + statsTotal.BonusStaminaMultiplier);
			statsTotal.Intellect *= (1 + statsTotal.BonusIntellectMultiplier);
			statsTotal.Health += (statsTotal.Stamina * 10f) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f);
            if (statsTotal.Intellect != 0)
            {
                statsTotal.Mana += ((statsTotal.Intellect - 20) * 15f) + 20;
            }
			
                       
            statsTotal.NatureResistance += statsTotal.NatureResistanceBuff + statsTotal.AllResist;
			statsTotal.FireResistance += statsTotal.FireResistanceBuff + statsTotal.AllResist;
			statsTotal.FrostResistance += statsTotal.FrostResistanceBuff + statsTotal.AllResist;
			statsTotal.ShadowResistance += statsTotal.ShadowResistanceBuff + statsTotal.AllResist;
			statsTotal.ArcaneResistance += statsTotal.ArcaneResistanceBuff + statsTotal.AllResist;


			return statsTotal;
		}

		public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
		{
			switch (chartName)
			{
				case "Hit Test":
					float dpsBaseHit = GetCharacterCalculations(character).OverallPoints;
					float dps1 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 1 } }).OverallPoints - dpsBaseHit);
					float dps2 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 2 } }).OverallPoints - dpsBaseHit);
					float dps3 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 3 } }).OverallPoints - dpsBaseHit);
					float dps4 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 4 } }).OverallPoints - dpsBaseHit);
					float dps5 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 5 } }).OverallPoints - dpsBaseHit);
					float dps10 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 10 } }).OverallPoints - dpsBaseHit);
					float dps15 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 15 } }).OverallPoints - dpsBaseHit);
					float dps20 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 20 } }).OverallPoints - dpsBaseHit);
					float dps25 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 25 } }).OverallPoints - dpsBaseHit);
					float dps50 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 50 } }).OverallPoints - dpsBaseHit);
					float dps75 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 75 } }).OverallPoints - dpsBaseHit);
					float dps83 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 83 } }).OverallPoints - dpsBaseHit);
					float dps100 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 100 } }).OverallPoints - dpsBaseHit);
					float dps200 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 200 } }).OverallPoints - dpsBaseHit);
					float dps300 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 300 } }).OverallPoints - dpsBaseHit);
					float dps400 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 400 } }).OverallPoints - dpsBaseHit);
					float dps500 = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 500 } }).OverallPoints - dpsBaseHit);

					return new ComparisonCalculationBase[] { 
						new ComparisonCalculationElemental() { Name = "dps1", OverallPoints = dps1, DPSPoints = dps1 },
						new ComparisonCalculationElemental() { Name = "dps2", OverallPoints = dps2, DPSPoints = dps2 },
						new ComparisonCalculationElemental() { Name = "dps3", OverallPoints = dps3, DPSPoints = dps3 },
						new ComparisonCalculationElemental() { Name = "dps4", OverallPoints = dps4, DPSPoints = dps4 },
						new ComparisonCalculationElemental() { Name = "dps5", OverallPoints = dps5, DPSPoints = dps5 },
						new ComparisonCalculationElemental() { Name = "dps10", OverallPoints = dps10, DPSPoints = dps10 },
						new ComparisonCalculationElemental() { Name = "dps15", OverallPoints = dps15, DPSPoints = dps15 },
						new ComparisonCalculationElemental() { Name = "dps20", OverallPoints = dps20, DPSPoints = dps20 },
						new ComparisonCalculationElemental() { Name = "dps25", OverallPoints = dps25, DPSPoints = dps25 },
						new ComparisonCalculationElemental() { Name = "dps50", OverallPoints = dps50, DPSPoints = dps50 },
						new ComparisonCalculationElemental() { Name = "dps75", OverallPoints = dps75, DPSPoints = dps75 },
						new ComparisonCalculationElemental() { Name = "dps83", OverallPoints = dps83, DPSPoints = dps83 },
						new ComparisonCalculationElemental() { Name = "dps100", OverallPoints = dps100, DPSPoints = dps100 },
						new ComparisonCalculationElemental() { Name = "dps200", OverallPoints = dps200, DPSPoints = dps200 },
						new ComparisonCalculationElemental() { Name = "dps300", OverallPoints = dps300, DPSPoints = dps300 },
						new ComparisonCalculationElemental() { Name = "dps400", OverallPoints = dps400, DPSPoints = dps400 },
						new ComparisonCalculationElemental() { Name = "dps500", OverallPoints = dps500, DPSPoints = dps500 },
					};
					
					break;

				case "Relative Stat Values":
					float dpsBase =		GetCharacterCalculations(character).OverallPoints;
					//float dpsStr =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = 10 } }).OverallPoints - dpsBase) / 10f;
					//float dpsAgi =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 10 } }).OverallPoints - dpsBase) / 10f;
					//float dpsAP  =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { AttackPower = 1 } }).OverallPoints - dpsBase) / 10f;
					float dpsCrit =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 1 } }).OverallPoints - dpsBase);
					float dpsExp =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { ExpertiseRating = 1 } }).OverallPoints - dpsBase);
					float dpsHaste =	(GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 1 } }).OverallPoints - dpsBase);
					float dpsHit =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 1 } }).OverallPoints - dpsBase);
					float dpsDmg =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { WeaponDamage = 1 } }).OverallPoints - dpsBase);
					float dpsPen =		(GetCharacterCalculations(character, new Item() { Stats = new Stats() { ArmorPenetration = 1 } }).OverallPoints - dpsBase);

					//Differential Calculations for Agi
					float dpsAtAdd = dpsBase;
					float agiToAdd = 0f;
					while (dpsBase == dpsAtAdd && agiToAdd < 2)
					{
						agiToAdd += 0.01f;
						dpsAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToAdd } }).OverallPoints;
					}

					float dpsAtSubtract = dpsBase;
					float agiToSubtract = 0f;
					while (dpsBase == dpsAtSubtract && agiToSubtract > -2)
					{
						agiToSubtract -= 0.01f;
						dpsAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToSubtract } }).OverallPoints;
					}
					agiToSubtract += 0.01f;

					ComparisonCalculationElemental comparisonAgi = new ComparisonCalculationElemental()
					{
						Name = "Agility",
						OverallPoints = (dpsAtAdd - dpsBase) / (agiToAdd - agiToSubtract),
						DPSPoints = (dpsAtAdd - dpsBase) / (agiToAdd - agiToSubtract),
					};


					//Differential Calculations for Str
					dpsAtAdd = dpsBase;
					float strToAdd = 0f;
					while (dpsBase == dpsAtAdd && strToAdd < 2)
					{
						strToAdd += 0.01f;
						dpsAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToAdd } }).OverallPoints;
					}

					dpsAtSubtract = dpsBase;
					float strToSubtract = 0f;
					while (dpsBase == dpsAtSubtract && strToSubtract > -2)
					{
						strToSubtract -= 0.01f;
						dpsAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToSubtract } }).OverallPoints;
					}
					strToSubtract += 0.01f;

					ComparisonCalculationElemental comparisonStr = new ComparisonCalculationElemental()
					{
						Name = "Strength",
						OverallPoints = (dpsAtAdd - dpsBase) / (strToAdd - strToSubtract),
						DPSPoints = (dpsAtAdd - dpsBase) / (strToAdd - strToSubtract),
					};


					//Differential Calculations for AP
					dpsAtAdd = dpsBase;
					float apToAdd = 0f;
					while (dpsBase == dpsAtAdd && apToAdd < 2)
					{
						apToAdd += 0.01f;
						dpsAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { AttackPower = apToAdd } }).OverallPoints;
					}

					dpsAtSubtract = dpsBase;
					float apToSubtract = 0f;
					while (dpsBase == dpsAtSubtract && apToSubtract > -2)
					{
						apToSubtract -= 0.01f;
						dpsAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { AttackPower = apToSubtract } }).OverallPoints;
					}
					apToSubtract += 0.01f;

					ComparisonCalculationElemental comparisonAP = new ComparisonCalculationElemental()
					{
						Name = "Attack Power",
						OverallPoints = (dpsAtAdd - dpsBase) / (apToAdd - apToSubtract),
						DPSPoints = (dpsAtAdd - dpsBase) / (apToAdd - apToSubtract),
					};



					return new ComparisonCalculationBase[] { 
						comparisonAgi,
						comparisonStr,
						comparisonAP,
						new ComparisonCalculationElemental() { Name = "Crit Rating", OverallPoints = dpsCrit, DPSPoints = dpsCrit },
						new ComparisonCalculationElemental() { Name = "Haste Rating", OverallPoints = dpsHaste, DPSPoints = dpsHaste },
						new ComparisonCalculationElemental() { Name = "Hit Rating", OverallPoints = dpsHit, DPSPoints = dpsHit },
					};

				default:
					return new ComparisonCalculationBase[0];
			}
		}

		public override bool IsItemRelevant(Item item)
		{
			if ((item.Slot == Item.ItemSlot.Ranged && item.Type != Item.ItemType.Totem) ||
				(item.Slot == Item.ItemSlot.TwoHand && item.Stats.SpellPower < 100)) 
				return false;
			return base.IsItemRelevant(item);
		}

		public override Stats GetRelevantStats(Stats stats)
		{
			return new Stats()
				{
					Intellect = stats.Intellect,
					SpellPower = stats.SpellPower,
					CritRating = stats.CritRating,
					HitRating = stats.HitRating,
					Stamina = stats.Stamina,
					HasteRating = stats.HasteRating,
					BloodlustProc = stats.BloodlustProc,
					BonusCritMultiplier = stats.BonusCritMultiplier,
					BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                    BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
					Health = stats.Health,
					DrumsOfBattle = stats.DrumsOfBattle,
					DrumsOfWar = stats.DrumsOfWar,
					ThreatReductionMultiplier = stats.ThreatReductionMultiplier,

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
				};
		}

		public override bool HasRelevantStats(Stats stats)
		{

            return (stats.Intellect + stats.SpellPower + stats.SpellHitRating + stats.SpellCritRating + 
                    stats.SpellHasteRating +stats.SpellCrit + stats.CritRating + stats.HitRating + 
                    stats.HasteRating + stats.Mana + stats.Health > 0 )
                    || (stats.Stamina > 0 && stats.AttackPower == 0);
                
                #region Old Relevancy
                /*
                (stats.Agility + stats.ArmorPenetration + stats.AttackPower + stats.BloodlustProc +
				stats.BonusAgilityMultiplier + stats.BonusAttackPowerMultiplier + stats.BonusCritMultiplier +
				stats.BonusMangleCatDamage + stats.BonusDamageMultiplier + stats.BonusRipDamageMultiplier + stats.BonusShredDamage +
				stats.BonusStaminaMultiplier + stats.BonusStrengthMultiplier + stats.CritRating + stats.ExpertiseRating +
				stats.HasteRating + stats.HitRating + stats.MangleCatCostReduction + 
				stats.Strength + stats.CatFormStrength + stats.TerrorProc + stats.WeaponDamage + stats.ExposeWeakness + stats.Bloodlust +
				stats.PhysicalHit + stats.BonusRipDamagePerCPPerTick + stats.ShatteredSunMightProc +
				stats.PhysicalHaste + stats.ArmorPenetrationRating + stats.BonusRipDuration +
				stats.BonusSpellPowerMultiplier + stats.BonusArcaneDamageMultiplier + stats.ThreatReductionMultiplier + stats.AllResist +
				stats.ArcaneResistance + stats.NatureResistance + stats.FireResistance +
				stats.FrostResistance + stats.ShadowResistance + stats.ArcaneResistanceBuff +
				stats.NatureResistanceBuff + stats.FireResistanceBuff +
				stats.FrostResistanceBuff + stats.ShadowResistanceBuff) > 0 || (stats.Stamina > 0 && stats.SpellPower == 0);
               */
                #endregion


		}
	}

    public class CharacterCalculationsElemental : CharacterCalculationsBase
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

		public float DPSPoints
		{
			get { return _subPoints[0]; }
			set { _subPoints[0] = value; }
		}

		public float SustainabilityPoints
		{
			get { return _subPoints[1]; }
			set { _subPoints[1] = value; }
		}

		public Stats BasicStats { get; set; }
		public int TargetLevel { get; set; }

		public float MissedAttacks { get; set; }
		public float CritChance { get; set; }


		public ElementalRotationCalculator.ElementalRotationCalculation HighestDPSRotation { get; set; }
		public ElementalRotationCalculator.ElementalRotationCalculation CustomRotation { get; set; }
		
		public string Rotations { get; set; }

		public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
		{
			Dictionary<string, string> dictValues = new Dictionary<string, string>();
			dictValues.Add("Overall Points", OverallPoints.ToString());
			dictValues.Add("DPS Points", DPSPoints.ToString());
            dictValues.Add("Sustainability Points", SustainabilityPoints.ToString());
			
			dictValues.Add("Health", BasicStats.Health.ToString());
			dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
			dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            dictValues.Add("Spell Power", BasicStats.SpellPower.ToString());
			dictValues.Add("Hit Rating", BasicStats.HitRating.ToString());
            dictValues.Add("Crit Rating", BasicStats.CritRating.ToString());
			dictValues.Add("Haste Rating", BasicStats.HasteRating.ToString());
			dictValues.Add("Mana Regen", BasicStats.ManaRestore5min.ToString());
			

			dictValues.Add("Crit Chance", CritChance.ToString() + "%");

			
			dictValues.Add("Lightning Bolt", "TODO");
			dictValues.Add("Chain Lightning", "TODO");
			dictValues.Add("Flame Shock(DD)", "TODO");
			dictValues.Add("Flame Shock(DoT)", "TODO");
			dictValues.Add("Lava Burst", "TODO");



            dictValues.Add("Optimal", "TODO");
            dictValues.Add("Optimal DPS", "TODO");
            dictValues.Add("Custom DPS", "TODO");
			
			return dictValues;
		}

		public override float GetOptimizableCalculationValue(string calculation)
		{
			switch (calculation)
			{
				case "Health": return BasicStats.Health;
				case "Nature Resist": return BasicStats.NatureResistance;
				case "Fire Resist": return BasicStats.FireResistance;
				case "Frost Resist": return BasicStats.FrostResistance;
				case "Shadow Resist": return BasicStats.ShadowResistance;
				case "Arcane Resist": return BasicStats.ArcaneResistance;
				case "Custom Rotation DPS": return CustomRotation.DPS;
			}
			return 0f;
		}
    }

	public class ComparisonCalculationElemental : ComparisonCalculationBase
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

		private float[] _subPoints = new float[] { 0f };
		public override float[] SubPoints
		{
			get { return _subPoints; }
			set { _subPoints = value; }
		}

		public float DPSPoints
		{
			get { return _subPoints[0]; }
			set { _subPoints[0] = value; }
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
			return string.Format("{0}: ({1}O {2}DPS)", Name, Math.Round(OverallPoints), Math.Round(DPSPoints));
		}
	}
}
