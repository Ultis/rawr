using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Elemental
{
	[Rawr.Calculations.RawrModelInfo("Elemental", "Spell_Nature_Lightning", Character.CharacterClass.Shaman)]
	public class CalculationsElemental : CalculationsBase
	{
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                return new List<GemmingTemplate>() { };
            }
        }

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
					"Summary:Overall Points*Sum of burst and sustained points",
					"Summary:Burst Points*DPS until you go out of mana",
					"Summary:Sustained Points*Total DPS of the fight",

					"Basic Stats:Health",
                    "Basic Stats:Mana",
                    "Basic Stats:Stamina",
					"Basic Stats:Intellect",
					"Basic Stats:Spell Power",
                    "Basic Stats:Hit Rating",
					"Basic Stats:Crit Rating",
                    "Basic Stats:Haste Rating",
					"Basic Stats:Mana Regen",

                    "Attacks:Lightning Bolt",
					"Attacks:Chain Lightning",
					"Attacks:Lava Burst",
                    "Attacks:Flame Shock",
                    "Attacks:Earth Shock",
                    "Attacks:Frost Shock",
					
					"Simulation:Simulation",
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
					_subPointNameColors.Add("Burst DPS", System.Drawing.Color.FromArgb(255, 0, 0));
					_subPointNameColors.Add("Sustained DPS", System.Drawing.Color.FromArgb(0, 0, 255));
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

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] {
                        "Glyphs", "Glyph combinations"
					};
                return _customChartNames;
            }
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            CharacterCalculationsElemental currentCalc, calc;
            ComparisonCalculationBase comparison;
            CalculationOptionsElemental calculationOptions;
            float[] subPoints;

            switch (chartName)
            {
                case "Glyphs":
                    calculationOptions = character.CalculationOptions as CalculationOptionsElemental;

                    currentCalc = GetCharacterCalculations(character) as CharacterCalculationsElemental;

                    for (int index = 0; index < 6; index++)
                    {
                        bool glyphEnabled = calculationOptions.GetGlyph(index);

                        if (glyphEnabled)
                        {
                            calculationOptions.SetGlyph(index, false);
                            calc = GetCharacterCalculations(character) as CharacterCalculationsElemental;

                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = calculationOptions.getGlyphName(index);
                            comparison.Equipped = true;
                            comparison.OverallPoints = (currentCalc.OverallPoints - calc.OverallPoints);
                            subPoints = new float[calc.SubPoints.Length];
                            for (int i = 0; i < calc.SubPoints.Length; i++)
                            {
                                subPoints[i] = (currentCalc.SubPoints[i] - calc.SubPoints[i]);
                            }
                            comparison.SubPoints = subPoints;

                            comparisonList.Add(comparison);
                        }
                        else
                        {
                            calculationOptions.SetGlyph(index, true);
                            calc = GetCharacterCalculations(character) as CharacterCalculationsElemental;

                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = calculationOptions.getGlyphName(index);
                            comparison.Equipped = false;
                            comparison.OverallPoints = (calc.OverallPoints - currentCalc.OverallPoints);
                            subPoints = new float[calc.SubPoints.Length];
                            for (int i = 0; i < calc.SubPoints.Length; i++)
                            {
                                subPoints[i] = (calc.SubPoints[i] - currentCalc.SubPoints[i]);
                            }
                            comparison.SubPoints = subPoints;

                            comparisonList.Add(comparison);
                        }

                        calculationOptions.SetGlyph(index, glyphEnabled);
                    }

                    return comparisonList.ToArray();
                case "Glyph combinations":
                    calculationOptions = character.CalculationOptions as CalculationOptionsElemental;

                    bool[] currentGlyphs = new bool[6];
                    for (int index = 0; index < 6; index++) currentGlyphs[index] = calculationOptions.GetGlyph(index);
                    for (int index = 0; index < 6; index++) calculationOptions.SetGlyph(index, false);
                    currentCalc = GetCharacterCalculations(character) as CharacterCalculationsElemental;

                    for (int i = 0; i < 6; i++)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            for (int k = 0; k < j; k++)
                            {
                                for (int index = 0; index < 6; index++) calculationOptions.SetGlyph(index, false);
                                calculationOptions.SetGlyph(i, true);
                                calculationOptions.SetGlyph(j, true);
                                calculationOptions.SetGlyph(k, true);

                                calc = GetCharacterCalculations(character) as CharacterCalculationsElemental;

                                comparison = CreateNewComparisonCalculation();
                                comparison.Name = calculationOptions.getShortGlyphName(i) + " + " + calculationOptions.getShortGlyphName(j) + " + " + calculationOptions.getShortGlyphName(k);
                                comparison.Equipped = true;
                                comparison.OverallPoints = (calc.OverallPoints - currentCalc.OverallPoints);
                                subPoints = new float[calc.SubPoints.Length];
                                for (int n = 0; n < calc.SubPoints.Length; n++)
                                {
                                    subPoints[n] = (calc.SubPoints[n] - currentCalc.SubPoints[n]);
                                }
                                comparison.SubPoints = subPoints;

                                comparisonList.Add(comparison);
                            }
                        }
                    }

                    for (int index = 0; index < 6; index++) calculationOptions.SetGlyph(index, currentGlyphs[index]);

                    return comparisonList.ToArray();
                default:
                    return new ComparisonCalculationBase[0];
            }
        }

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
			Stats stats = GetCharacterStats(character, additionalItem);

			CharacterCalculationsElemental calculatedStats = new CharacterCalculationsElemental();
			calculatedStats.BasicStats = stats;
            calculatedStats.LocalCharacter = character;
            calcOpts.calculatedStats = calculatedStats;

            if (calcOpts.UseSimulator)
            {
                Simulator.solve(stats, character.ShamanTalents, calcOpts);
            }
            else
            {
                Solver.solve(calculatedStats, calcOpts);
            }

            return calculatedStats;
			
		}

        public Stats GetRaceStats(Character character)
        {
            Stats statsRace;
            switch (character.Race)
            {
                // I know only the Str/Agi values for Draenei, just assuming the others are the same (should be close enough)
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Strength = 121, Agility = 71, Stamina = 135, Intellect = 128, Spirit = 145 };
                    break;
                case Character.CharacterRace.Orc:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Strength = 121, Agility = 71, Stamina = 138, Intellect = 125, Spirit = 146 };
                    break;
                case Character.CharacterRace.Tauren:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Strength = 121, Agility = 71, Stamina = 138, Intellect = 126, Spirit = 145 };
                    statsRace.BonusHealthMultiplier = 0.05f;
                    break;
                case Character.CharacterRace.Troll:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Strength = 121, Agility = 71, Stamina = 137, Intellect = 124, Spirit = 144 };
                    break;
                default:
                    statsRace = new Stats() { Health = 0, Mana = 0, Strength = 0, Agility = 0, Stamina = 0, Intellect = 0, Spirit = 0 };
                    break;
            }
            return statsRace;
        }

        public Stats GetTalentStats(ShamanTalents talents)
        {
            Stats statsTalents = new Stats()
            {
                #region Elemental
                BonusSpellCritMultiplier = 0.20f * talents.ElementalFury,
                SpellHit = 0.01f * talents.ElementalPrecision,
                ManaRegenIntPer5 = 0.04f * talents.UnrelentingStorm,
                #endregion
                #region Enhancement
                BonusIntellectMultiplier = 0.02f * talents.AncestralKnowledge,
                PhysicalCrit = 0.01f * talents.ThunderingStrikes,
                SpellCrit = 0.01f * talents.ThunderingStrikes,
                BonusFlametongueDamage = 0.10f * talents.ElementalWeapons,
                SpellPowerFromAttackPowerPercentage = 0.1f * talents.MentalQuickness,
                #endregion
            };
            return statsTalents;
        }

		public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
            Stats statsRace = GetRaceStats(character);
			Stats statsItems = GetItemStats(character, additionalItem);
			//Stats statsEnchants = GetEnchantsStats(character);
			Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsTalents = GetTalentStats(character.ShamanTalents);

			Stats statsGearEnchantsBuffs = statsItems + statsBuffs;

			CalculationOptionsElemental calcOpts = character.CalculationOptions as CalculationOptionsElemental;

			Stats statsTotal = statsRace + statsItems + statsBuffs + statsTalents;

            statsTotal.Strength *= 1 + statsTotal.BonusStrengthMultiplier;
            statsTotal.Agility *= 1 + statsTotal.BonusAgilityMultiplier;
            statsTotal.Stamina *= 1 + statsTotal.BonusStaminaMultiplier;
            statsTotal.Intellect *= 1 + statsTotal.BonusIntellectMultiplier;
            statsTotal.Spirit *= 1 + statsTotal.BonusSpiritMultiplier;

            if (statsTotal.GreatnessProc > 0)
            {
                if (statsTotal.Spirit > statsTotal.Intellect)
                {
                    statsTotal.Spirit += (statsTotal.GreatnessProc * 15f / 50f) * (1 + statsTotal.BonusSpiritMultiplier);
                }
                else
                {
                    statsTotal.Intellect += (statsTotal.GreatnessProc * 15f / 50f) * (1 + statsTotal.BonusIntellectMultiplier);
                }
            }

            statsTotal.Strength = (float)Math.Floor(statsTotal.Strength);
            statsTotal.Agility = (float)Math.Floor(statsTotal.Agility);
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina);
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect);
            statsTotal.Spirit = (float)Math.Floor(statsTotal.Spirit);

            statsTotal.AttackPower += statsTotal.Strength + statsTotal.Agility;
            statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower + statsTotal.AttackPower * statsTotal.SpellPowerFromAttackPowerPercentage);

            statsTotal.Mana += Math.Max(0f, (statsTotal.Intellect - 20f) * 15f + 20f);
            statsTotal.Mana *= (float)Math.Round(1f + statsTotal.BonusManaMultiplier);

            statsTotal.Health += Math.Max(0f, (statsTotal.Stamina - 20f) * 10f + 20f);
            statsTotal.Health *= (float)Math.Round(1f + statsTotal.BonusManaMultiplier);

            statsTotal.Mp5 += (float)Math.Floor(statsTotal.Intellect * statsTotal.ManaRegenIntPer5);

            statsTotal.SpellCrit += character.StatConversion.GetSpellCritFromRating(statsTotal.CritRating) / 100f;
            statsTotal.SpellCrit += character.StatConversion.GetSpellCritFromIntellect(statsTotal.Intellect) / 100f;
            statsTotal.SpellHaste += character.StatConversion.GetSpellHasteFromRating(statsTotal.HasteRating) / 100f;
            statsTotal.SpellHit += character.StatConversion.GetSpellHitFromRating(statsTotal.HitRating) / 100f;

            statsTotal.SpellPower += 211; // Flametongue
            if (calcOpts.glyphOfFlametongue) statsTotal.SpellCrit += .02f;

            return statsTotal;
		}

		public override bool IsItemRelevant(Item item)
		{
            if ((item.Slot == Item.ItemSlot.Ranged && item.Type != Item.ItemType.Totem))
                return false;
            return base.IsItemRelevant(item);
		}

		public override Stats GetRelevantStats(Stats stats)
		{
			return new Stats()
				{
            #region Basic stats
                Intellect = stats.Intellect,
                Mana = stats.Mana,
                Spirit= stats.Spirit,
                SpellCrit = stats.SpellCrit,
                SpellHit = stats.SpellHit,
                SpellHaste = stats.SpellHaste,
                SpellPower = stats.SpellPower,
                CritRating = stats.CritRating,
                HasteRating = stats.HasteRating,
                HitRating = stats.HitRating,
                SpellFireDamageRating = stats.SpellFireDamageRating,
                SpellNatureDamageRating = stats.SpellNatureDamageRating,
                SpellFrostDamageRating = stats.SpellFrostDamageRating,
                Mp5 = stats.Mp5,
            #endregion
            #region Multipliers
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                BonusFireDamageMultiplier = stats.BonusFireDamageMultiplier,
                BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
            #endregion
            #region Totems
                LightningSpellPower = stats.LightningSpellPower,
                LightningBoltHasteProc_15_45 = stats.LightningBoltHasteProc_15_45,
                LavaBurstBonus = stats.LavaBurstBonus,
            #endregion
            #region Sets
                LightningBoltCostReduction = stats.LightningBoltCostReduction,
                LightningBoltDamageModifier = stats.LightningBoltDamageModifier,
                BonusLavaBurstCritDamage = stats.BonusLavaBurstCritDamage,
            #endregion
                #region Trinkets
                SpellPowerFor10SecOnHit_10_45 = stats.SpellPowerFor10SecOnHit_10_45,
                SpellPowerFor10SecOnResist = stats.SpellPowerFor10SecOnResist,
                SpellPowerFor15SecOnCrit_20_45 = stats.SpellPowerFor15SecOnCrit_20_45,
                SpellPowerFor15SecOnUse90Sec = stats.SpellPowerFor15SecOnUse90Sec,
                SpellPowerFor15SecOnUse2Min = stats.SpellPowerFor15SecOnUse2Min,
                SpellPowerFor20SecOnUse2Min = stats.SpellPowerFor20SecOnUse2Min,
                SpellPowerFor20SecOnUse5Min = stats.SpellPowerFor20SecOnUse5Min,
                SpellPowerFor10SecOnCast_10_45 = stats.SpellPowerFor10SecOnCast_10_45,
                SpellPowerFor10SecOnCrit_20_45 = stats.SpellPowerFor10SecOnCrit_20_45,
                SpellPowerFor10SecOnCast_15_45 = stats.SpellPowerFor10SecOnCast_15_45,
                SpellHasteFor10SecOnCast_10_45 = stats.SpellHasteFor10SecOnCast_10_45,
                SpellHasteFor6SecOnCast_15_45 = stats.SpellHasteFor6SecOnCast_15_45,
                SpellHasteFor6SecOnHit_10_45 = stats.SpellHasteFor6SecOnHit_10_45,
                BonusManaPotion = stats.BonusManaPotion,
                HasteRatingFor20SecOnUse2Min = stats.HasteRatingFor20SecOnUse2Min,
                Mp5OnCastFor20SecOnUse2Min = stats.Mp5OnCastFor20SecOnUse2Min,
                ManaRestoreOnCast_10_45 = stats.ManaRestoreOnCast_10_45,
                ManaRestoreOnCrit_25_45 = stats.ManaRestoreOnCrit_25_45,
                ManaRestore5min = stats.ManaRestore5min,
                FullManaRegenFor15SecOnSpellcast = stats.FullManaRegenFor15SecOnSpellcast,
                ManaregenOver20SecOnUse3Min = stats.ManaregenOver20SecOnUse3Min,
                ManaregenOver20SecOnUse5Min = stats.ManaregenOver20SecOnUse5Min,
                PendulumOfTelluricCurrentsProc = stats.PendulumOfTelluricCurrentsProc,
                ThunderCapacitorProc = stats.ThunderCapacitorProc,
                LightningCapacitorProc = stats.LightningCapacitorProc,
                ExtractOfNecromanticPowerProc = stats.ExtractOfNecromanticPowerProc,
                ExtraSpiritWhileCasting = stats.ExtraSpiritWhileCasting,
                SpiritFor20SecOnUse2Min = stats.SpiritFor20SecOnUse2Min,
                MementoProc = stats.MementoProc,
                GreatnessProc = stats.GreatnessProc,
                DarkmoonCardDeathProc = stats.DarkmoonCardDeathProc,
            #endregion
                };
		}

		public override bool HasRelevantStats(Stats stats)
		{
            float elementalStats = 0;
            #region Basic stats
            elementalStats +=
                stats.Intellect +
                stats.Mana +
                stats.Spirit +
                stats.SpellCrit +
                stats.SpellHit +
                stats.SpellHaste +
                stats.SpellPower +
                stats.CritRating +
                stats.HasteRating +
                stats.HitRating +
                stats.SpellFireDamageRating +
                stats.SpellNatureDamageRating +
                stats.SpellFrostDamageRating +
                stats.Mp5;
            #endregion
            #region Multipliers
            elementalStats +=
                stats.BonusIntellectMultiplier +
                stats.BonusSpiritMultiplier +
                stats.BonusSpellCritMultiplier +
                stats.BonusSpellPowerMultiplier +
                stats.BonusFireDamageMultiplier +
                stats.BonusNatureDamageMultiplier +
                stats.BonusFrostDamageMultiplier;
            #endregion
            #region Totems
            elementalStats +=
                stats.LightningSpellPower +
                stats.LightningBoltHasteProc_15_45 +
                stats.LavaBurstBonus;
            #endregion
            #region Sets
            elementalStats += 
                stats.BonusLavaBurstCritDamage +
                stats.LightningBoltCostReduction +
                stats.LightningBoltDamageModifier;
            #endregion
            #region Trinkets
            elementalStats +=
                stats.SpellPowerFor10SecOnResist +
                stats.SpellPowerFor15SecOnCrit_20_45 +
                stats.SpellPowerFor15SecOnUse90Sec +
                stats.SpellPowerFor15SecOnUse2Min +
                stats.SpellPowerFor20SecOnUse2Min +
                stats.SpellPowerFor20SecOnUse5Min +
                stats.SpellPowerFor10SecOnCrit_20_45 +
                stats.SpellPowerFor10SecOnCast_15_45 +
                stats.SpellPowerFor10SecOnCast_10_45 +
                stats.SpellHasteFor10SecOnCast_10_45 +
                stats.SpellHasteFor6SecOnCast_15_45 +
                stats.SpellHasteFor6SecOnHit_10_45 +
                stats.SpellPowerFor10SecOnHit_10_45 +
                stats.BonusManaPotion +
                stats.HasteRatingFor20SecOnUse5Min +
                stats.Mp5OnCastFor20SecOnUse2Min +
                stats.ManaRestoreOnCast_10_45 +
                stats.ManaRestoreOnCrit_25_45 +
                stats.ManaRestore5min +
                stats.FullManaRegenFor15SecOnSpellcast +
                stats.ManaregenOver20SecOnUse3Min +
                stats.ManaregenOver20SecOnUse5Min +
                stats.PendulumOfTelluricCurrentsProc +
                stats.ThunderCapacitorProc +
                stats.LightningCapacitorProc +
                stats.ExtraSpiritWhileCasting +
                stats.SpiritFor20SecOnUse2Min +
                stats.MementoProc +
                stats.DarkmoonCardDeathProc +
                stats.ExtractOfNecromanticPowerProc +
                stats.GreatnessProc;
            #endregion
            return (elementalStats > 0);

		}
	}

    public class CharacterCalculationsElemental : CharacterCalculationsBase
    {

        #region Variable Declarations and Definitions

        public Stats BasicStats { get; set; }
        public int TargetLevel { get; set; }

        public Spell LightningBolt;
        public Spell ChainLightning;
        public Spell ChainLightning3;
        public Spell ChainLightning4;
        public Spell LavaBurst;
        public Spell FlameShock;
        public Spell EarthShock;
        public Spell FrostShock;

        public float ManaRegenInFSR;
        public float ManaRegenOutFSR;
        public float ReplenishMP5;

        public float TimeToOOM;
        public float CastRegenFraction;
        public float RotationDPS;
        public float TotalDPS;
        public float RotationMPS;
        public float CastFraction;
        public float CritFraction;
        public float MissFraction;

        public float ClearCast_FlameShock;
        public float ClearCast_LavaBurst;
        public float ClearCast_LightningBolt;

        public Character LocalCharacter { get; set; }

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

        public float BurstPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float SustainedPoints
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        #endregion

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            dictValues.Add("Overall Points", OverallPoints.ToString());
            dictValues.Add("Burst Points", BurstPoints.ToString());
            dictValues.Add("Sustained Points", SustainedPoints.ToString());

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            dictValues.Add("Spell Power", BasicStats.SpellPower.ToString());
            dictValues.Add("Hit Rating", BasicStats.HitRating.ToString());
            dictValues.Add("Crit Rating", BasicStats.CritRating.ToString());
            dictValues.Add("Haste Rating", BasicStats.HasteRating.ToString());
            dictValues.Add("Mana Regen", Math.Round(ManaRegenInFSR).ToString() + " / " + Math.Round(ManaRegenOutFSR) + " + " + Math.Round(ReplenishMP5).ToString());

            dictValues.Add("Lightning Bolt", Math.Round(LightningBolt.MinHit).ToString() + "-" + Math.Round(LightningBolt.MaxHit).ToString() + " / " + Math.Round(LightningBolt.MinCrit).ToString() + "-" + Math.Round(LightningBolt.MaxCrit).ToString() + "*Mana cost: "+Math.Round(LightningBolt.ManaCost).ToString()+"\nCrit chance: " + Math.Round(100f * LightningBolt.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * LightningBolt.MissChance, 2).ToString() + " %\nClearcast uptime: " + Math.Round(100f * ClearCast_LightningBolt, 2).ToString() + " %");
            dictValues.Add("Chain Lightning", Math.Round(ChainLightning.MinHit).ToString() + "-" + Math.Round(ChainLightning.MaxHit).ToString() + " / " + Math.Round(ChainLightning.MinCrit).ToString() + "-" + Math.Round(ChainLightning.MaxCrit).ToString() + "*Mana cost: " + Math.Round(ChainLightning.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * ChainLightning.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * ChainLightning.MissChance, 2).ToString() + " %\n3 adds: " + Math.Round(ChainLightning3.MinHit).ToString() + "-" + Math.Round(ChainLightning3.MaxHit).ToString() + " / " + Math.Round(ChainLightning3.MinCrit).ToString() + "-" + Math.Round(ChainLightning3.MaxCrit).ToString() + "\n4 adds: " + Math.Round(ChainLightning4.MinHit).ToString() + "-" + Math.Round(ChainLightning4.MaxHit).ToString() + " / " + Math.Round(ChainLightning4.MinCrit).ToString() + "-" + Math.Round(ChainLightning4.MaxCrit).ToString());
            dictValues.Add("Lava Burst", Math.Round(LavaBurst.MinHit).ToString() + "-" + Math.Round(LavaBurst.MaxHit).ToString() + " / " + Math.Round(LavaBurst.MinCrit).ToString() + "-" + Math.Round(LavaBurst.MaxCrit).ToString() + "*Mana cost: " + Math.Round(LavaBurst.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * LavaBurst.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * LavaBurst.MissChance, 2).ToString() + " %\nClearcast uptime: " + Math.Round(100f * ClearCast_LavaBurst, 2).ToString() + " %");
            dictValues.Add("Flame Shock", Math.Round(FlameShock.AvgHit).ToString() + " / " + Math.Round(FlameShock.AvgCrit).ToString() + " + " + Math.Round(FlameShock.PeriodicTick).ToString() + " (every 3s)*Mana cost: " + Math.Round(FlameShock.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * FlameShock.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * FlameShock.MissChance, 2).ToString() + " %\nClearcast uptime: " + Math.Round(100f * ClearCast_FlameShock, 2).ToString() + " %");
            dictValues.Add("Earth Shock", Math.Round(EarthShock.MinHit).ToString() + "-" + Math.Round(EarthShock.MaxHit).ToString() + " / " + Math.Round(EarthShock.MinCrit).ToString() + "-" + Math.Round(EarthShock.MaxCrit).ToString() + "*Mana cost: " + Math.Round(EarthShock.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * EarthShock.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * EarthShock.MissChance, 2).ToString() + " %");
            dictValues.Add("Frost Shock", Math.Round(FrostShock.MinHit).ToString() + "-" + Math.Round(FrostShock.MaxHit).ToString() + " / " + Math.Round(FrostShock.MinCrit).ToString() + "-" + Math.Round(FrostShock.MaxCrit).ToString() + "*Mana cost: " + Math.Round(FrostShock.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * FrostShock.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * FrostShock.MissChance, 2).ToString() + " %");

            dictValues.Add("Simulation", Math.Round(TotalDPS).ToString() + " DPS*OOM after " + Math.Round(TimeToOOM).ToString() + " sec.\nDPS until OOM: " + Math.Round(RotationDPS).ToString() + "\nMPS until OOM: " + Math.Round(RotationMPS).ToString() + "\nCast vs regen fraction after OOM: " + Math.Round(CastRegenFraction, 4).ToString() + "\n" + Math.Round(60f * CastFraction, 1).ToString() + " casts per minute\n" + Math.Round(60f * CritFraction, 1).ToString() + " crits per minute\n" + Math.Round(60f * MissFraction, 1).ToString() + " misses per minute\n");

            return dictValues;
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

		private float[] _subPoints = new float[] { 0f, 0f };
		public override float[] SubPoints
		{
			get { return _subPoints; }
			set { _subPoints = value; }
		}

        public float BurstPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float SustainedPoints
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

        private ItemInstance _itemInstance = null;
        public override ItemInstance ItemInstance
        {
            get { return _itemInstance; }
            set { _itemInstance = value; }
        }

		private bool _equipped = false;
		public override bool Equipped
		{
			get { return _equipped; }
			set { _equipped = value; }
		}

		public override string ToString()
		{
			return string.Format("{0}: ({1}O {2}Burst {3}Sustained)", Name, Math.Round(OverallPoints), Math.Round(BurstPoints), Math.Round(SustainedPoints));
		}
	}
    
    public static class Constants
    {
        // Source: http://www.wowwiki.com/Base_mana
        public static float BaseMana = 4396;
        public static float HasteRatingToHaste = 3279f;
    }
}
