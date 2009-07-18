using System;
using System.Collections.Generic;

namespace Rawr.Healadin
{
	[Rawr.Calculations.RawrModelInfo("Healadin", "Spell_Holy_HolyBolt", CharacterClass.Paladin)]
	public class CalculationsHealadin : CalculationsBase
    {

        #region Model Properties
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                ////Relevant Gem IDs
                //Green
                int[] dazzling = { 39984, 40094, 40175 };

                //Yellow
                int[] brilliant = { 39912, 40012, 40123, 42148 };

                //Orange
                int[] luminous = { 39946, 40047, 40151 };

                //Meta
                int insightful = 41401;
                // int revitalizing = 41376;

                return new List<GemmingTemplate>()
				{
					new GemmingTemplate() { Model = "Healadin", Group = "Uncommon",
						RedId = brilliant[0], YellowId = brilliant[0], BlueId = brilliant[0], PrismaticId = brilliant[0], MetaId = insightful },
					new GemmingTemplate() { Model = "Healadin", Group = "Uncommon",
						RedId = luminous[0], YellowId = brilliant[0], BlueId = dazzling[0], PrismaticId = brilliant[0], MetaId = insightful },

					new GemmingTemplate() { Model = "Healadin", Group = "Rare", Enabled = true,
						RedId = brilliant[1], YellowId = brilliant[1], BlueId = brilliant[1], PrismaticId = brilliant[1], MetaId = insightful },
					new GemmingTemplate() { Model = "Healadin", Group = "Rare", Enabled = true,
						RedId = luminous[1], YellowId = brilliant[1], BlueId = dazzling[1], PrismaticId = brilliant[1], MetaId = insightful },
						
					new GemmingTemplate() { Model = "Healadin", Group = "Epic",
						RedId = brilliant[2], YellowId = brilliant[2], BlueId = brilliant[2], PrismaticId = brilliant[2], MetaId = insightful },
					new GemmingTemplate() { Model = "Healadin", Group = "Epic",
						RedId = luminous[2], YellowId = brilliant[2], BlueId = dazzling[2], PrismaticId = brilliant[2], MetaId = insightful },
						
					new GemmingTemplate() { Model = "Healadin", Group = "Jeweler",
						RedId = brilliant[3], YellowId = brilliant[1], BlueId = brilliant[3], PrismaticId = brilliant[1], MetaId = insightful },
					new GemmingTemplate() { Model = "Healadin", Group = "Jeweler",
						RedId = brilliant[3], YellowId = brilliant[3], BlueId = brilliant[3], PrismaticId = brilliant[3], MetaId = insightful },
				};
            }
        }

        public override void SetDefaults(Character character)
        {
            character.ActiveBuffs.Add(Buff.GetBuffByName("Swift Retribution"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Arcane Intellect"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Judgements of the Wise"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Blessing of Wisdom"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Blessing of Wisdom"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Elemental Oath"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Wrath of Air Totem"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Totem of Wrath (Spell Power)"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Power Word: Fortitude"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Power Word: Fortitude"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Mark of the Wild"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Mark of the Wild"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Blessing of Kings"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Flask of the Frost Wyrm"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Fish Feast"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Tree of Life Aura"));
        }

        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            if (_relevantGlyphs == null)
            {
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of Seal of Wisdom");
                _relevantGlyphs.Add("Glyph of Seal of Light");
                _relevantGlyphs.Add("Glyph of Holy Light");
                _relevantGlyphs.Add("Glyph of Flash of Light");
                _relevantGlyphs.Add("Glyph of Holy Shock");
                _relevantGlyphs.Add("Glyph of Divinity");
                _relevantGlyphs.Add("Glyph of Beacon of Light");
                _relevantGlyphs.Add("Glyph of the Wise");
                _relevantGlyphs.Add("Glyph of Lay on Hands");
            }
            return _relevantGlyphs;
        }

        private string[] _optimizableCalculationLabels = null;
        /// <summary>
        /// Labels of the stats available to the Optimizer 
        /// </summary>
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
					"Health",
                    "Holy Light Cast Time",
                    "Holy Light HPS",
                    "Flash of Light Cast Time",
                    "Flash of Light HPS",
					};
                return _optimizableCalculationLabels;
            }
        }

#if SILVERLIGHT
        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
#else
        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
#endif
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelHealadin();
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
					"Basic Stats:Mana",
					"Basic Stats:Stamina",
					"Basic Stats:Intellect",
					"Basic Stats:Spell Power",
					"Basic Stats:Mp5",
					"Basic Stats:Spell Crit",
					"Basic Stats:Spell Haste",

					"Cycle Stats:Total Healed",
					"Cycle Stats:Total Mana",
					"Cycle Stats:Average Healing per sec",
					"Cycle Stats:Average Healing per mana",

                    "Rotation Info:Holy Light Time",
                    "Rotation Info:Flash of Light Time",
                    "Rotation Info:Holy Shock Time",
                    "Rotation Info:Sacred Shield Time",
                    "Rotation Info:Beacon of Light Time",
                    "Rotation Info:Judgement Time",

                    "Healing Breakdown:Holy Light Healed",
                    "Healing Breakdown:Flash of Light Healed",
                    "Healing Breakdown:Holy Shock Healed",
                    "Healing Breakdown:Sacred Shield Healed",
                    "Healing Breakdown:Beacon of Light Healed",
                    "Healing Breakdown:Glyph of HL Healed",
                    "Healing Breakdown:Other Healed*From trinket procs",

                    "Spell Information:Holy Light",
					"Spell Information:Flash of Light",
					"Spell Information:Holy Shock",
					"Spell Information:Sacred Shield",

				};
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] {
                    "Mana Pool Breakdown",
                    "Mana Usage Breakdown",
                    "Healing Breakdown",
                    "Rotation Breakdown",
					};
                return _customChartNames;
            }
        }

#if SILVERLIGHT
        private Dictionary<string, System.Windows.Media.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Windows.Media.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Windows.Media.Color>();
                    _subPointNameColors.Add("Fight Healing", System.Windows.Media.Colors.Red);
                    _subPointNameColors.Add("Burst Healing", System.Windows.Media.Colors.Cyan);
                }
                return _subPointNameColors;
            }
        }
#else
        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("Fight Healing", System.Drawing.Color.Red);
                    _subPointNameColors.Add("Burst Healing", System.Drawing.Color.CornflowerBlue);
                }
                return _subPointNameColors;
            }
        }
#endif

        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<ItemType>(new ItemType[]
					{
                        ItemType.Plate,
                        ItemType.Mail,
                        ItemType.Leather,
                        ItemType.Cloth,
                        ItemType.None,
						ItemType.Shield,
						ItemType.Libram,
						ItemType.OneHandAxe,
						ItemType.OneHandMace,
						ItemType.OneHandSword,
						ItemType.TwoHandAxe,
						ItemType.TwoHandMace,
						ItemType.TwoHandSword
					});
                }
                return _relevantItemTypes;
            }
        }

        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot)
        {
            if ((slot == ItemSlot.OffHand && enchant.Slot != ItemSlot.OffHand) || slot == ItemSlot.Ranged) return false;
            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique)
        {
            if (slot == CharacterSlot.OffHand && item.Slot == ItemSlot.OneHand) return false;
            return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
        }

		public override CharacterClass TargetClass { get { return CharacterClass.Paladin; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationHealadin(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsHealadin(); }

		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsHealadin));
			System.IO.StringReader reader = new System.IO.StringReader(xml);
			CalculationOptionsHealadin calcOpts = serializer.Deserialize(reader) as CalculationOptionsHealadin;
			return calcOpts;
		}
        #endregion

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            if (character == null) return new CharacterCalculationsHealadin();

            Stats stats;
            CharacterCalculationsHealadin calc = null;
            PaladinTalents talents = character.PaladinTalents;

            for (int i = 0; i < 5; i++)
            {
                float oldBurst = 0;
                if (i > 0) oldBurst = calc.BurstPoints;

                stats = GetCharacterStats(character, additionalItem, true, calc);
                calc = new CharacterCalculationsHealadin();

                Rotation rot = new Rotation(character, stats);

                if (i > 0) calc.BurstPoints = oldBurst;
                else calc.BurstPoints = rot.CalculateBurstHealing(calc);
                calc.FightPoints = rot.CalculateFightHealing(calc);
                calc.OverallPoints = calc.BurstPoints + calc.FightPoints;
            }
            calc.BasicStats = GetCharacterStats(character, additionalItem, false, null);

            return calc;
        }

        #region Stat Calculation
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            return GetCharacterStats(character, additionalItem, true, null);
        }

        public Stats GetCharacterStats(Character character, Item additionalItem, bool computeAverageStats, CharacterCalculationsHealadin calc)
        {
            PaladinTalents talents = character.PaladinTalents;
            CalculationOptionsHealadin calcOpts = character.CalculationOptions as CalculationOptionsHealadin;
            float fightLength = calcOpts.Length * 60;

            Stats statsRace = BaseStats.GetBaseStats(character.Level, CharacterClass.Paladin, character.Race);
            statsRace.Health -= 180f;
            statsRace.Mana -= 280f;

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats stats = statsBaseGear + statsBuffs + statsRace;

            ConvertRatings(stats, talents, calcOpts);
            if (computeAverageStats)
            {
                Stats statsAverage = new Stats();

                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    float trigger = 0;
                    if (calc == null)
                    {
                        trigger = 1.5f / calcOpts.Activity / (1f + stats.SpellHaste);
                        if (effect.Trigger == Trigger.HealingSpellCrit || effect.Trigger == Trigger.SpellCrit) trigger *= stats.SpellCrit;
                    }
                    else
                    {
                        if (effect.Trigger == Trigger.HealingSpellCast || effect.Trigger == Trigger.HealingSpellHit)
                            trigger = 1f / Rotation.GetHealingCastsPerSec(calc);
                        else if (effect.Trigger == Trigger.HealingSpellCrit || effect.Trigger == Trigger.SpellCrit)
                            trigger = 1f / Rotation.GetHealingCritsPerSec(calc);
                        else if (effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.SpellHit)
                            trigger = 1f / Rotation.GetSpellCastsPerSec(calc);
                        else if (effect.Trigger == Trigger.Use) trigger = 0f;
                        else continue;
                    }
                    statsAverage += effect.GetAverageStats(trigger, 1f, 1.5f, fightLength);
                }
                statsAverage.ManaRestore *= fightLength;
                statsAverage.Healed *= fightLength;

                stats = statsBaseGear + statsBuffs + statsRace + statsAverage;
                ConvertRatings(stats, talents, calcOpts);
            }

            return stats;
        }

        private void ConvertRatings(Stats stats, PaladinTalents talents, CalculationOptionsHealadin calcOpts)
        {
            stats.Stamina *= 1 + stats.BonusStaminaMultiplier;
            stats.Intellect *= (1 + stats.BonusIntellectMultiplier) * (1 + talents.DivineIntellect * (calcOpts.Mode32 ? .02f : .03f));
            stats.HighestStat *= (1 + stats.BonusIntellectMultiplier) * (1 + talents.DivineIntellect * .03f);
            stats.SpellPower += 0.04f * (stats.Intellect + stats.HighestStat) * talents.HolyGuidance;
            stats.SpellCrit = stats.SpellCrit + (stats.Intellect + stats.HighestStat) / 16666.66709f
                + stats.CritRating / 4590.598679f + talents.SanctityOfBattle * .01f + talents.Conviction * .01f;

            stats.SpellHaste = (1f + talents.JudgementsOfThePure * (calcOpts.JotP ? .03f : 0f))
                * (1f + stats.SpellHaste)
                * (1f + stats.HasteRating / 3278.998947f)
                - 1f;

            stats.Mana = (stats.Mana + stats.Intellect * 15) * (1f + stats.BonusManaMultiplier);
            stats.Health = stats.Health + stats.Stamina * 10f;
            stats.PhysicalHit += stats.HitRating / 3278.998947f;
        }
        #endregion

        #region Custom Charts
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            if (chartName == "Mana Pool Breakdown")
            {
                CharacterCalculationsHealadin calc = GetCharacterCalculations(character) as CharacterCalculationsHealadin;
                if (calc == null) calc = new CharacterCalculationsHealadin();

                ComparisonCalculationHealadin Base = new ComparisonCalculationHealadin("Base");
                ComparisonCalculationHealadin Mp5 = new ComparisonCalculationHealadin("Mp5");
                ComparisonCalculationHealadin Potion = new ComparisonCalculationHealadin("Potion");
                ComparisonCalculationHealadin Replenishment = new ComparisonCalculationHealadin("Replenishment");
                ComparisonCalculationHealadin ArcaneTorrent = new ComparisonCalculationHealadin("Arcane Torrent");
                ComparisonCalculationHealadin DivinePlea = new ComparisonCalculationHealadin("Divine Plea");
                ComparisonCalculationHealadin LoH = new ComparisonCalculationHealadin("Lay on Hands");
                ComparisonCalculationHealadin Other = new ComparisonCalculationHealadin("Other");

                Base.OverallPoints = Base.ThroughputPoints = calc.ManaBase;
                Mp5.OverallPoints = Mp5.ThroughputPoints = calc.ManaMp5;
                LoH.OverallPoints = LoH.ThroughputPoints = calc.ManaLayOnHands;
                Potion.OverallPoints = Potion.ThroughputPoints = calc.ManaPotion;
                Replenishment.OverallPoints = Replenishment.ThroughputPoints = calc.ManaReplenishment;
                ArcaneTorrent.OverallPoints = ArcaneTorrent.ThroughputPoints = calc.ManaArcaneTorrent;
                DivinePlea.OverallPoints = DivinePlea.ThroughputPoints = calc.ManaDivinePlea;
                Other.OverallPoints = Other.ThroughputPoints = calc.ManaOther;

                return new ComparisonCalculationBase[] { Base, Mp5, Potion, Replenishment, LoH, ArcaneTorrent, DivinePlea, Other };
            }
            else if (chartName == "Mana Usage Breakdown")
            {
                CharacterCalculationsHealadin calc = GetCharacterCalculations(character) as CharacterCalculationsHealadin;
                if (calc == null) calc = new CharacterCalculationsHealadin();

                ComparisonCalculationHealadin FoL = new ComparisonCalculationHealadin("Flash of Light");
                ComparisonCalculationHealadin HL = new ComparisonCalculationHealadin("Holy Light");
                ComparisonCalculationHealadin HS = new ComparisonCalculationHealadin("Holy Shock");
                ComparisonCalculationHealadin JotP = new ComparisonCalculationHealadin("Judgements and Seals");
                ComparisonCalculationHealadin BoL = new ComparisonCalculationHealadin("Beacon of Light");
                ComparisonCalculationHealadin SS = new ComparisonCalculationHealadin("Sacred Shield");

                FoL.OverallPoints = FoL.ThroughputPoints = calc.UsageFoL;
                HL.OverallPoints = HL.ThroughputPoints = calc.UsageHL;
                HS.OverallPoints = HS.ThroughputPoints = calc.UsageHS;
                JotP.OverallPoints = JotP.ThroughputPoints = calc.UsageJotP;
                BoL.OverallPoints = BoL.ThroughputPoints = calc.UsageBoL;
                SS.OverallPoints = SS.ThroughputPoints = calc.UsageSS;

                return new ComparisonCalculationBase[] { FoL, HL, HS, JotP, BoL, SS };
            }
            else if (chartName == "Healing Breakdown")
            {
                CharacterCalculationsHealadin calc = GetCharacterCalculations(character) as CharacterCalculationsHealadin;
                if (calc == null) calc = new CharacterCalculationsHealadin();

                ComparisonCalculationHealadin FoL = new ComparisonCalculationHealadin("Flash of Light");
                ComparisonCalculationHealadin HL = new ComparisonCalculationHealadin("Holy Light");
                ComparisonCalculationHealadin HS = new ComparisonCalculationHealadin("Holy Shock");
                ComparisonCalculationHealadin GHL = new ComparisonCalculationHealadin("Glyph of Holy Light");
                ComparisonCalculationHealadin BoL = new ComparisonCalculationHealadin("Beacon of Light");
                ComparisonCalculationHealadin SS = new ComparisonCalculationHealadin("Sacred Shield");

                FoL.OverallPoints = FoL.ThroughputPoints = calc.HealedFoL / calc.FightLength;
                HL.OverallPoints = HL.ThroughputPoints = calc.HealedHL / calc.FightLength;
                HS.OverallPoints = HS.ThroughputPoints = calc.HealedHS / calc.FightLength;
                GHL.OverallPoints = GHL.ThroughputPoints = calc.HealedGHL / calc.FightLength;
                BoL.OverallPoints = BoL.ThroughputPoints = calc.HealedBoL / calc.FightLength;
                SS.OverallPoints = SS.ThroughputPoints = calc.HealedSS / calc.FightLength;

                return new ComparisonCalculationBase[] { FoL, HL, HS, GHL, BoL, SS };
            }
            else if (chartName == "Rotation Breakdown")
            {
                CharacterCalculationsHealadin calc = GetCharacterCalculations(character) as CharacterCalculationsHealadin;
                if (calc == null) calc = new CharacterCalculationsHealadin();

                ComparisonCalculationHealadin FoL = new ComparisonCalculationHealadin("Flash of Light");
                ComparisonCalculationHealadin HL = new ComparisonCalculationHealadin("Holy Light");
                ComparisonCalculationHealadin HS = new ComparisonCalculationHealadin("Holy Shock");
                ComparisonCalculationHealadin JotP = new ComparisonCalculationHealadin("Judgements and Seals");
                ComparisonCalculationHealadin BoL = new ComparisonCalculationHealadin("Beacon of Light");
                ComparisonCalculationHealadin SS = new ComparisonCalculationHealadin("Sacred Shield");

                FoL.OverallPoints = FoL.ThroughputPoints = calc.RotationFoL;
                HL.OverallPoints = HL.ThroughputPoints = calc.RotationHL;
                HS.OverallPoints = HS.ThroughputPoints = calc.RotationHS;
                JotP.OverallPoints = JotP.ThroughputPoints = calc.RotationJotP;
                BoL.OverallPoints = BoL.ThroughputPoints = calc.RotationBoL;
                SS.OverallPoints = SS.ThroughputPoints = calc.RotationSS;

                return new ComparisonCalculationBase[] { FoL, HL, HS, JotP, BoL, SS };
            }
            return new ComparisonCalculationBase[] {};
        }
        #endregion Custom Charts

        #region Relevancy Methods
        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Mp5 = stats.Mp5,
                SpellPower = stats.SpellPower,
                CritRating = stats.CritRating,
                PhysicalHit = stats.PhysicalHit,
                HitRating = stats.HitRating,
                HasteRating = stats.HasteRating,
                Health = stats.Health,
                Mana = stats.Mana,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusManaPotion = stats.BonusManaPotion,
                SpellCrit = stats.SpellCrit,
                SpellHaste = stats.SpellHaste,
                FlashOfLightCrit = stats.FlashOfLightCrit,
                FlashOfLightSpellPower = stats.FlashOfLightSpellPower,
                FlashOfLightMultiplier = stats.FlashOfLightMultiplier,
                HolyShockCrit = stats.HolyShockCrit,
                HolyLightSpellPower = stats.HolyLightSpellPower,
                HolyLightCrit = stats.HolyLightCrit,
                HolyLightManaCostReduction = stats.HolyLightManaCostReduction,
                HolyLightPercentManaReduction = stats.HolyLightPercentManaReduction,
                HealingReceivedMultiplier = stats.HealingReceivedMultiplier,
                // Gear Procs
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,
                BonusManaMultiplier = stats.BonusManaMultiplier,
                BonusCritHealMultiplier = stats.BonusCritHealMultiplier,
                SpellsManaReduction = stats.SpellsManaReduction,
                SacredShieldICDReduction = stats.SacredShieldICDReduction,
                HolyShockHoTOnCrit = stats.HolyShockHoTOnCrit
            };
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (HasRelevantSpecialEffect(effect)) s.AddSpecialEffect(effect);
            }
            return s;
        }

        public bool HasRelevantSpecialEffect(SpecialEffect effect)
        {
            if (effect.Trigger == Trigger.Use
                || effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.SpellHit
                || effect.Trigger == Trigger.HealingSpellCast || effect.Trigger == Trigger.HealingSpellCrit || effect.Trigger == Trigger.HealingSpellHit)
            {
                Stats stats = effect.Stats;
                if ((stats.Intellect + stats.SpellPower + stats.CritRating + stats.HasteRating + stats.ShieldFromHealed
                    + stats.ManaRestore + stats.Mp5 + stats.Healed + stats.HighestStat) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private bool HasWantedStats(Stats stats)
        {
            return (stats.SpellCrit + stats.SpellHaste  + stats.PhysicalHit + stats.Mana
                + stats.BonusIntellectMultiplier + stats.HolyLightPercentManaReduction + stats.HolyShockCrit +
                + stats.BonusManaPotion + stats.FlashOfLightMultiplier + stats.FlashOfLightSpellPower + stats.FlashOfLightCrit + stats.HolyLightManaCostReduction
                + stats.HolyLightCrit + stats.HolyLightSpellPower + stats.ManaRestoreFromMaxManaPerSecond + stats.BonusManaMultiplier
                + stats.HealingReceivedMultiplier + stats.BonusCritHealMultiplier + stats.SpellsManaReduction
                + stats.SacredShieldICDReduction + stats.HolyShockHoTOnCrit) > 0;
        }

        private bool HasMaybeStats(Stats stats)
        {
            return (stats.Mp5 + stats.Intellect + stats.HasteRating + stats.CritRating + stats.SpellPower) > 0;
        }

        public bool HasSurvivalStats(Stats stats)
        {
            return (stats.Stamina + stats.Health) > 0;
        }

        private bool HasIgnoreStats(Stats stats)
        {
            return (stats.Agility + stats.Strength + stats.AttackPower + stats.DefenseRating + stats.Defense + stats.Dodge + stats.Parry
                + stats.HitRating + stats.ArmorPenetrationRating + stats.Spirit + stats.DodgeRating + stats.ParryRating
                + stats.ExpertiseRating + stats.Expertise + stats.BlockRating + stats.Block) > 0;
        }
    

        public override bool IsBuffRelevant(Buff buff)
        {
            foreach (SpecialEffect effect in buff.Stats.SpecialEffects())
            {
                if (HasRelevantSpecialEffect(effect)) return true;
            }
            return HasWantedStats(buff.Stats) || HasMaybeStats(buff.Stats) || HasSurvivalStats(buff.Stats);
        }

        public override bool IsEnchantRelevant(Enchant enchant)
        {
            foreach (SpecialEffect effect in enchant.Stats.SpecialEffects())
            {
                if (HasRelevantSpecialEffect(effect)) return true;
            }
            return HasWantedStats(enchant.Stats) || HasMaybeStats(enchant.Stats);
        }

        public override bool HasRelevantStats(Stats stats)
        {
            bool wantedStats = HasWantedStats(stats);
            bool maybeStats = HasMaybeStats(stats);
            bool ignoreStats = HasIgnoreStats(stats);
            bool survivalStats = HasSurvivalStats(stats);
            bool specialEffect = false;
            bool hasSpecialEffect = false;
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                hasSpecialEffect = true;
                if (HasRelevantSpecialEffect(effect))
                {
                    specialEffect = true;
                    break;
                }
            }
            return wantedStats || ((specialEffect || (maybeStats && (!hasSpecialEffect || specialEffect)) || (survivalStats && !ignoreStats)) && !ignoreStats);
        }
        #endregion
    }
}
