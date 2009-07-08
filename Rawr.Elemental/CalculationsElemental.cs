using System;
using System.Collections.Generic;
using System.Text;
using Rawr.Elemental.Estimation;

namespace Rawr.Elemental
{
	[Rawr.Calculations.RawrModelInfo("Elemental", "Spell_Nature_Lightning", Character.CharacterClass.Shaman)]
	public class CalculationsElemental : CalculationsBase
	{
        private List<GemmingTemplate> _defaultGemmingTemplates = null;
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                if (_defaultGemmingTemplates == null)
                {
                    // Meta
                    int chaotic = 41285;

                    // [0] uncommon
                    // [1] perfect uncommon
                    // [2] rare
                    // [3] epic
                    // [4] jewelcrafting

                    // Reds
                    int[] runed = { 39911, 41438, 39998, 40113, 42144 }; // spell power
                    // Blue
                    int[] lustrous = { 39927, 41440, 40010, 40121, 42146 }; // mp5
                    // Yellow
                    int[] quick = { 39918, 41446, 40017, 40128, 42150 }; // haste
                    int[] rigid = { 39915, 41447, 40014, 40125, 42156 }; // hit
                    // Purple
                    int[] royal = { 39943, 41459, 40027, 40134 }; // spell power + mp5
                    // Green
                    int[] dazzling = { 39984, 41463, 40094, 40175 }; // int + mp5
                    int[] lambent = { 39986, 41469, 40100, 40177 }; // hit + mp5
                    // Orange
                    int[] luminous = { 39946, 41494, 40047, 40151 }; // spell power + int
                    int[] reckless = { 39959, 41497, 40051, 40155 }; // spell power + haste
                    int[] potent = { 39956, 41495, 40048, 40152}; // spell power + crit
                    int[] veiled = { 39957, 41502, 40049, 40153 }; // spell power + hit

                    /*
                     * red: runed, royal
                     * yellow: reckless, quick
                     * blue: royal, dazzling(, lustrous)
                     */

                    _defaultGemmingTemplates = new List<GemmingTemplate>();
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Uncommon", false, runed[0], royal[0], reckless[0], quick[0], dazzling[0], rigid[0], veiled[0], lambent[0], chaotic);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Perfect", false, runed[1], royal[1], reckless[1], quick[1], dazzling[1], rigid[1], veiled[1], lambent[1], chaotic);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Rare", true, runed[2], royal[2], reckless[2], quick[2], dazzling[2], rigid[2], veiled[2], lambent[2], chaotic);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Epic", false, runed[3], royal[3], reckless[3], quick[3], dazzling[3], rigid[3], veiled[3], lambent[3], chaotic);
                    AddJCGemmingTemplateGroup(_defaultGemmingTemplates, "Jewelcrafting", false, runed[4], quick[4], lustrous[4], rigid[4], chaotic);
                }
                return _defaultGemmingTemplates;
            }
        }

        private void AddGemmingTemplateGroup(List<GemmingTemplate> list, string name, bool enabled, int runed, int royal, int reckless, int quick, int dazzling, int rigid, int veiled, int lambent, int meta)
        {
            // if no mana problem, runed and rigid are always very good
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, RedId = runed, YellowId = runed, BlueId = runed, PrismaticId = runed, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, RedId = rigid, YellowId = rigid, BlueId = rigid, PrismaticId = rigid, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, RedId = veiled, YellowId = veiled, BlueId = veiled, PrismaticId = veiled, MetaId = meta, Enabled = enabled });
            // if mana problem, dazzling is often very good
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, RedId = dazzling, YellowId = dazzling, BlueId = dazzling, PrismaticId = dazzling, MetaId = meta, Enabled = enabled });

            /*
             * red: runed, royal
             * yellow: reckless, quick
             * blue: royal, dazzling, lambent (, lustrous)
             */
            // +Power and +Crit
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, 
                RedId = runed, YellowId = reckless, BlueId = royal, PrismaticId = runed, 
                MetaId = meta, Enabled = enabled });
            // +Hit
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, 
                RedId = veiled, YellowId = rigid, BlueId = royal, PrismaticId = rigid, 
                MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, 
                RedId = veiled, YellowId = rigid, BlueId = lambent, PrismaticId = rigid, 
                MetaId = meta, Enabled = enabled });
            // MP5
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, 
                RedId = veiled, YellowId = rigid, BlueId = dazzling, PrismaticId = veiled, 
                MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, 
                RedId = royal, YellowId = dazzling, BlueId = dazzling, PrismaticId = royal, 
                MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, 
                RedId = royal, YellowId = dazzling, BlueId = dazzling, PrismaticId = dazzling, 
                MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, 
                RedId = runed, YellowId = dazzling, BlueId = royal, PrismaticId = royal, 
                MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, 
                RedId = runed, YellowId = dazzling, BlueId = royal, PrismaticId = dazzling, 
                MetaId = meta, Enabled = enabled });
        }

        private void AddJCGemmingTemplateGroup(List<GemmingTemplate> list, string name, bool enabled, int runed, int quick, int lustrous, int rigid, int meta)
        {
            // Overrides, only "runed" and "seers"
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, RedId = runed, YellowId = runed, BlueId = runed, PrismaticId = runed, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, RedId = lustrous, YellowId = lustrous, BlueId = lustrous, PrismaticId = lustrous, MetaId = meta, Enabled = enabled });

            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, RedId = runed, YellowId = quick, BlueId = lustrous, PrismaticId = runed, MetaId = meta, Enabled = enabled });
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
					"Simulation:Rotation",
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

        private string[] _customChartNames = {};
        public override string[] CustomChartNames
        {
            get
            {
                return _customChartNames;
            }
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            return new ComparisonCalculationBase[0];
        }

		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsElemental));
			System.IO.StringReader reader = new System.IO.StringReader(xml);
			CalculationOptionsElemental calcOpts = serializer.Deserialize(reader) as CalculationOptionsElemental;
			return calcOpts;
		}

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
		{
			//_cachedCharacter = character;
			CalculationOptionsElemental calcOpts = character.CalculationOptions as CalculationOptionsElemental;
			if (calcOpts == null) calcOpts = new CalculationOptionsElemental();
			Stats stats = GetCharacterStats(character, additionalItem);

			CharacterCalculationsElemental calculatedStats = new CharacterCalculationsElemental();
			calculatedStats.BasicStats = stats;
            calculatedStats.LocalCharacter = character;
            calcOpts.calculatedStats = calculatedStats;

            Rawr.Elemental.Estimation.Estimation.solve(calculatedStats, calcOpts);

            return calculatedStats;
			
		}

        public Stats GetTalentStats(ShamanTalents talents)
        {
            Stats statsTalents = new Stats()
            {
                #region Elemental
                SpellHit = 0.01f * talents.ElementalPrecision,
                ManaRegenIntPer5 = 0.04f * talents.UnrelentingStorm,
                #endregion
                #region Enhancement
                BonusIntellectMultiplier = 0.02f * talents.AncestralKnowledge,
                PhysicalCrit = 0.01f * talents.ThunderingStrikes,
                SpellCrit = 0.01f * talents.ThunderingStrikes,
                BonusFlametongueDamage = 0.10f * talents.ElementalWeapons,
                SpellPowerFromAttackPowerPercentage = 0.1f * talents.MentalQuickness,
                ShockManaCostReduction = talents.ShamanisticFocus * 0.45f,
                #endregion
                #region Glyphs
                SpellPower = talents.GlyphofTotemofWrath?talents.TotemOfWrath * 84:0
                #endregion
            };
            return statsTalents;
        }

		public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
            Stats statsRace = BaseStats.GetBaseStats(character);
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

            statsTotal.SpellCrit += StatConversion.GetSpellCritFromRating(statsTotal.CritRating);
            statsTotal.SpellCrit += StatConversion.GetSpellCritFromIntellect(statsTotal.Intellect);
            statsTotal.SpellHit += StatConversion.GetSpellHitFromRating(statsTotal.HitRating);

            // Flametongue weapon
            statsTotal.SpellPower += 211 * (1f + character.ShamanTalents.ElementalWeapons * .1f);
            statsTotal.Mp5 += 100; // Water shield
            if (character.ShamanTalents.GlyphofWaterMastery) statsTotal.Mp5 += 30;

            if (character.ShamanTalents.GlyphofFlametongueWeapon) statsTotal.SpellCrit += .02f;

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
                FlameShockDoTCanCrit = stats.FlameShockDoTCanCrit,
                LightningBoltCritDamageModifier = stats.LightningBoltCritDamageModifier,
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
                ManaregenOver12SecOnUse3Min = stats.ManaregenOver12SecOnUse3Min,
                ManaregenOver12SecOnUse5Min = stats.ManaregenOver12SecOnUse5Min,
                PendulumOfTelluricCurrentsProc = stats.PendulumOfTelluricCurrentsProc,
                ThunderCapacitorProc = stats.ThunderCapacitorProc,
                LightningCapacitorProc = stats.LightningCapacitorProc,
                ExtractOfNecromanticPowerProc = stats.ExtractOfNecromanticPowerProc,
                ExtraSpiritWhileCasting = stats.ExtraSpiritWhileCasting,
                SpiritFor20SecOnUse2Min = stats.SpiritFor20SecOnUse2Min,
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
                stats.LightningBoltDamageModifier +
                stats.LightningBoltCritDamageModifier + 
                stats.FlameShockDoTCanCrit;
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
                stats.ManaregenOver12SecOnUse3Min +
                stats.ManaregenOver12SecOnUse5Min +
                stats.PendulumOfTelluricCurrentsProc +
                stats.ThunderCapacitorProc +
                stats.LightningCapacitorProc +
                stats.ExtraSpiritWhileCasting +
                stats.SpiritFor20SecOnUse2Min +
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

        public float LBFraction;
        public float LvBFraction;
        public float FSFraction;

        public float LBPerSecond;
        public float LvBPerSecond;
        public float FSPerSecond;

        public float ClearCast_FlameShock;
        public float ClearCast_LavaBurst;
        public float ClearCast_LightningBolt;

        public float nLBfirst;
        public float nLBsecond;
        public float WaitAfterLB;

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

            dictValues.Add("Lightning Bolt", Math.Round(LightningBolt.MinHit).ToString() + "-" + Math.Round(LightningBolt.MaxHit).ToString() + " / " + Math.Round(LightningBolt.MinCrit).ToString() + "-" + Math.Round(LightningBolt.MaxCrit).ToString() + "*Mana cost: "+Math.Round(LightningBolt.ManaCost).ToString()+"\nCrit chance: " + Math.Round(100f * LightningBolt.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * LightningBolt.MissChance, 2).ToString() + " %\nCast time: " + Math.Round(LightningBolt.CastTime, 2) + " sec.\nClearcast uptime: " + Math.Round(100f * ClearCast_LightningBolt, 2).ToString() + " %");
            dictValues.Add("Chain Lightning", Math.Round(ChainLightning.MinHit).ToString() + "-" + Math.Round(ChainLightning.MaxHit).ToString() + " / " + Math.Round(ChainLightning.MinCrit).ToString() + "-" + Math.Round(ChainLightning.MaxCrit).ToString() + "*Mana cost: " + Math.Round(ChainLightning.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * ChainLightning.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * ChainLightning.MissChance, 2).ToString() + " %\nCast time: " + Math.Round(ChainLightning.CastTime, 2) + " sec.\n3 adds: " + Math.Round(ChainLightning3.MinHit).ToString() + "-" + Math.Round(ChainLightning3.MaxHit).ToString() + " / " + Math.Round(ChainLightning3.MinCrit).ToString() + "-" + Math.Round(ChainLightning3.MaxCrit).ToString() + "\n4 adds: " + Math.Round(ChainLightning4.MinHit).ToString() + "-" + Math.Round(ChainLightning4.MaxHit).ToString() + " / " + Math.Round(ChainLightning4.MinCrit).ToString() + "-" + Math.Round(ChainLightning4.MaxCrit).ToString());
            dictValues.Add("Lava Burst", Math.Round(LavaBurst.MinHit).ToString() + "-" + Math.Round(LavaBurst.MaxHit).ToString() + " / " + Math.Round(LavaBurst.MinCrit).ToString() + "-" + Math.Round(LavaBurst.MaxCrit).ToString() + "*Mana cost: " + Math.Round(LavaBurst.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * LavaBurst.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * LavaBurst.MissChance, 2).ToString() + " %\nCast time: " + Math.Round(LavaBurst.CastTime, 2) + " sec.\nClearcast uptime: " + Math.Round(100f * ClearCast_LavaBurst, 2).ToString() + " %");
            dictValues.Add("Flame Shock", Math.Round(FlameShock.AvgHit).ToString() + " / " + Math.Round(FlameShock.AvgCrit).ToString() + " + " + Math.Round(FlameShock.PeriodicTick).ToString() + " (every 3s)*Mana cost: " + Math.Round(FlameShock.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * FlameShock.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * FlameShock.MissChance, 2).ToString() + " %\nGCD: " + Math.Round(FlameShock.CastTime, 2) + " sec.\nClearcast uptime: " + Math.Round(100f * ClearCast_FlameShock, 2).ToString() + " %");
            dictValues.Add("Earth Shock", Math.Round(EarthShock.MinHit).ToString() + "-" + Math.Round(EarthShock.MaxHit).ToString() + " / " + Math.Round(EarthShock.MinCrit).ToString() + "-" + Math.Round(EarthShock.MaxCrit).ToString() + "*Mana cost: " + Math.Round(EarthShock.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * EarthShock.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * EarthShock.MissChance, 2).ToString() + " %\nGCD: " + Math.Round(EarthShock.CastTime, 2) + " sec.\n");
            dictValues.Add("Frost Shock", Math.Round(FrostShock.MinHit).ToString() + "-" + Math.Round(FrostShock.MaxHit).ToString() + " / " + Math.Round(FrostShock.MinCrit).ToString() + "-" + Math.Round(FrostShock.MaxCrit).ToString() + "*Mana cost: " + Math.Round(FrostShock.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * FrostShock.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * FrostShock.MissChance, 2).ToString() + " %\nGCD: " + Math.Round(FrostShock.CastTime, 2) + " sec.\n");

            dictValues.Add("Simulation", Math.Round(TotalDPS).ToString() + " DPS*OOM after " + Math.Round(TimeToOOM).ToString() + " sec.\nDPS until OOM: " + Math.Round(RotationDPS).ToString() + "\nMPS until OOM: " + Math.Round(RotationMPS).ToString() + "\nCast vs regen fraction after OOM: " + Math.Round(CastRegenFraction, 4).ToString() + "\n" + Math.Round(60f * CastFraction, 1).ToString() + " casts per minute\n" + Math.Round(60f * CritFraction, 1).ToString() + " crits per minute\n" + Math.Round(60f * MissFraction, 1).ToString() + " misses per minute\n" + Math.Round(60f * LvBPerSecond, 1).ToString() + " Lava Bursts per minute\n" + Math.Round(60f * FSPerSecond, 1).ToString() + " Flame Shocks per minute\n" + Math.Round(60f * LBPerSecond, 1).ToString() + " Lightning Bolts per minute\n");
            CalculationOptionsElemental calcOpts = (CalculationOptionsElemental)LocalCharacter.CalculationOptions;
            if (LocalCharacter.ShamanTalents.GlyphofFlameShock)
            {
                dictValues.Add("Rotation", "FS/LvB/"+Math.Round(nLBfirst, 2).ToString() + "LB/LvB/"+Math.Round(nLBsecond, 2).ToString() + "LB");
            }
            else
            {
                dictValues.Add("Rotation", "LvB/FS/" + Math.Round(nLBfirst, 2).ToString() + "LB/LvB/FS/" + Math.Round(nLBsecond, 2).ToString() + "LB");
            }

            

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

    public class Rotation
    {
        public float CastFraction;
        public float CritFraction;
        public float MissFraction;
        public float DPS;
        public float MPS;

        public float LBFraction;
        public float LvBFraction;
        public float FSFraction;

        public float LBPerSecond;
        public float LvBPerSecond;
        public float FSPerSecond;

        public float CC_FS;
        public float CC_LvB;
        public float CC_LB;

        public float nLBfirst, nLBsecond;
        public float WaitAfterLB;

        public LightningBolt LB;
        public ChainLightning CL;
        public ChainLightning CL3;
        public ChainLightning CL4;
        public LavaBurst LvB;
        public LavaBurst LvBFS;
        public FlameShock FS;
        public EarthShock ES;
        public FrostShock FrS;

        public static Rotation operator +(Rotation A, Rotation B)
        {
            return new Rotation()
            {
                DPS = (A.DPS + B.DPS),
                MPS = (A.MPS + B.MPS),
                CastFraction = (A.CastFraction + B.CastFraction),
                CritFraction = (A.CritFraction + B.CritFraction),
                MissFraction = (A.MissFraction + B.MissFraction),
                LB = (LightningBolt)A.LB + B.LB,
                CL = (ChainLightning)A.CL + B.CL,
                CL3 = (ChainLightning)A.CL3 + B.CL3,
                CL4 = (ChainLightning)A.CL4 + B.CL4,
                LvB = (LavaBurst)A.LvB + B.LvB,
                LvBFS = (LavaBurst)A.LvBFS + B.LvBFS,
                FS = (FlameShock)A.FS + B.FS,
                ES = (EarthShock)A.ES + B.ES,
                FrS = (FrostShock)A.FrS + B.FrS,
                CC_FS = A.CC_FS + B.CC_FS,
                CC_LvB = A.CC_LvB + B.CC_LvB,
                CC_LB = A.CC_LB + B.CC_LB,
                LBFraction = (A.LBFraction + B.LBFraction),
                LvBFraction = (A.LvBFraction + B.LvBFraction),
                FSFraction = (A.FSFraction + B.FSFraction),
                LBPerSecond = (A.LBPerSecond + B.LBPerSecond),
                LvBPerSecond = (A.LvBPerSecond + B.LvBPerSecond),
                FSPerSecond = (A.FSPerSecond + B.FSPerSecond),
                nLBfirst = (A.nLBfirst + B.nLBfirst),
                nLBsecond = (A.nLBsecond + B.nLBsecond),
                WaitAfterLB = (A.WaitAfterLB + B.WaitAfterLB)
            };
        }

        public static Rotation operator *(Rotation A, float b)
        {
            return new Rotation()
            {
                DPS = b * A.DPS,
                MPS = b * A.MPS,
                CastFraction = b * A.CastFraction,
                CritFraction = b * A.CritFraction,
                MissFraction = b * A.MissFraction,
                LB = (LightningBolt)A.LB * b,
                CL = (ChainLightning)A.CL * b,
                CL3 = (ChainLightning)A.CL3 * b,
                CL4 = (ChainLightning)A.CL4 * b,
                LvB = (LavaBurst)A.LvB * b,
                LvBFS = (LavaBurst)A.LvBFS * b,
                FS = (FlameShock)A.FS * b,
                ES = (EarthShock)A.ES * b,
                FrS = (FrostShock)A.FrS * b,
                CC_FS = A.CC_FS * b,
                CC_LvB = A.CC_LvB * b,
                CC_LB = A.CC_LB * b,
                LBFraction = A.LBFraction * b,
                LvBFraction = A.LvBFraction * b,
                FSFraction = A.FSFraction * b,
                LBPerSecond = A.LBPerSecond * b,
                LvBPerSecond = A.LvBPerSecond * b,
                FSPerSecond = A.FSPerSecond * b,
                nLBfirst = A.nLBfirst * b,
                nLBsecond = A.nLBsecond * b,
                WaitAfterLB = A.WaitAfterLB * b
            };
        }
    }

    public static class Constants
    {
        // Source: http://www.wowwiki.com/Base_mana
        public static float BaseMana = 4396;
        public static float HasteRatingToHaste = 3279f;
    }
}
