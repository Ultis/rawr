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

					new GemmingTemplate() { Model = "Healadin", Group = "Rare",
						RedId = brilliant[1], YellowId = brilliant[1], BlueId = brilliant[1], PrismaticId = brilliant[1], MetaId = insightful },
					new GemmingTemplate() { Model = "Healadin", Group = "Rare",
						RedId = luminous[1], YellowId = brilliant[1], BlueId = dazzling[1], PrismaticId = brilliant[1], MetaId = insightful },
						
					new GemmingTemplate() { Model = "Healadin", Group = "Epic", Enabled = true,
						RedId = brilliant[2], YellowId = brilliant[2], BlueId = brilliant[2], PrismaticId = brilliant[2], MetaId = insightful },
					new GemmingTemplate() { Model = "Healadin", Group = "Epic", Enabled = true,
						RedId = luminous[2], YellowId = brilliant[2], BlueId = dazzling[2], PrismaticId = brilliant[2], MetaId = insightful },
						
					new GemmingTemplate() { Model = "Healadin", Group = "Jeweler",
						RedId = brilliant[2], YellowId = brilliant[3], BlueId = brilliant[2], PrismaticId = brilliant[2], MetaId = insightful },
					new GemmingTemplate() { Model = "Healadin", Group = "Jeweler",
						RedId = luminous[2], YellowId = brilliant[3], BlueId = dazzling[2], PrismaticId = brilliant[2], MetaId = insightful },
				};
            }
        }

        public override void SetDefaults(Character character)
        {
            character.ActiveBuffsAdd(("Swift Retribution"));
            character.ActiveBuffsAdd(("Arcane Intellect"));
            character.ActiveBuffsAdd(("Judgements of the Wise"));
            character.ActiveBuffsAdd(("Blessing of Wisdom"));
            character.ActiveBuffsAdd(("Improved Blessing of Wisdom"));
            character.ActiveBuffsAdd(("Elemental Oath"));
            character.ActiveBuffsAdd(("Wrath of Air Totem"));
            character.ActiveBuffsAdd(("Totem of Wrath (Spell Power)"));
            character.ActiveBuffsAdd(("Power Word: Fortitude"));
            character.ActiveBuffsAdd(("Improved Power Word: Fortitude"));
            character.ActiveBuffsAdd(("Mark of the Wild"));
            character.ActiveBuffsAdd(("Improved Mark of the Wild"));
            character.ActiveBuffsAdd(("Blessing of Kings"));
            character.ActiveBuffsAdd(("Flask of the Frost Wyrm"));
            character.ActiveBuffsAdd(("Fish Feast"));
            character.ActiveBuffsAdd(("Tree of Life Aura"));
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

#if RAWR3
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

#if RAWR3
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
                    _subPointNameColors.Add("Fight Healing", System.Drawing.Color.FromArgb(255, 255, 0, 0));
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
            cacheChar = character;

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
            cacheChar = character;
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
            Stats statsBuffs = GetBuffsStats(character, calcOpts);
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
                        else if (effect.Trigger == Trigger.HolyLightCast) trigger = 1f / Rotation.GetHolyLightCastsPerSec(calc);
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
            stats.Intellect *= (1 + stats.BonusIntellectMultiplier) * (1 + talents.DivineIntellect * .02f);
            stats.HighestStat *= (1 + stats.BonusIntellectMultiplier) * (1 + talents.DivineIntellect * .02f);
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
            if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.HolyLightCast
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

        private static bool isSpiritIrrelevant = true;
        internal static bool IsSpiritIrrelevant
        {
            get { return isSpiritIrrelevant; }
            set { isSpiritIrrelevant = value; }
        }

        private static bool isHitIrrelevant = true;
        internal static bool IsHitIrrelevant
        {
            get { return isHitIrrelevant; }
            set { isHitIrrelevant = value; }
        }

        private bool HasWantedStats(Stats stats)
        {
            return (stats.SpellCrit + stats.SpellHaste  + stats.PhysicalHit + stats.Mana
                + stats.BonusIntellectMultiplier + stats.HolyLightPercentManaReduction + stats.HolyShockCrit +
                + stats.BonusManaPotion + stats.FlashOfLightMultiplier + stats.FlashOfLightSpellPower + stats.FlashOfLightCrit + stats.HolyLightManaCostReduction
                + stats.HolyLightCrit + stats.HolyLightSpellPower + stats.ManaRestoreFromMaxManaPerSecond + stats.BonusManaMultiplier
                + stats.HealingReceivedMultiplier + stats.BonusCritHealMultiplier + stats.SpellsManaReduction
                + stats.SacredShieldICDReduction + stats.HolyShockHoTOnCrit + stats.MovementSpeed) > 0;
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
                + stats.ArmorPenetrationRating + stats.DodgeRating + stats.ParryRating
                + stats.ExpertiseRating + stats.Expertise + stats.BlockRating + stats.Block) > 0
                || (stats.HitRating > 0 && isHitIrrelevant)
                || (stats.Spirit > 0 && isSpiritIrrelevant);
        }

        public override bool IsItemRelevant(Item item)
        {
            if (item.Slot == ItemSlot.Prismatic)
            {
                Stats stats = item.Stats;
                bool wantedStats = HasWantedStats(stats);
                bool maybeStats = HasMaybeStats(stats);
                bool ignoreStats = HasIgnoreStats(stats);
                bool survivalStats = HasSurvivalStats(stats);
                bool specialEffect = false;
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (HasRelevantSpecialEffect(effect))
                    {
                        specialEffect = true;
                        break;
                    }
                }
                return wantedStats || specialEffect || maybeStats || survivalStats;
            }
            else return base.IsItemRelevant(item);
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
                if (IsProfEnchantRelevant(enchant) && HasRelevantSpecialEffect(effect)) { return true; }
            }
            return IsProfEnchantRelevant(enchant) && (HasWantedStats(enchant.Stats) || HasMaybeStats(enchant.Stats));
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

        public Stats GetBuffsStats(Character character, CalculationOptionsHealadin calcOpts) {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            //float hasRelevantBuff;

            #region Racials to Force Enable
            // Draenei should always have this buff activated
            // NOTE: for other races we don't wanna take it off if the user has it active, so not adding code for that
            if (character.Race == CharacterRace.Draenei
                && !character.ActiveBuffs.Contains(Buff.GetBuffByName("Heroic Presence")))
            {
                character.ActiveBuffsAdd(("Heroic Presence"));
            }
            #endregion

            #region Passive Ability Auto-Fixing
            // Removes the Trueshot Aura Buff and it's equivalents Unleashed Rage and Abomination's Might if you are
            // maintaining it yourself. We are now calculating this internally for better accuracy and to provide
            // value to relevant talents
            /*{
                hasRelevantBuff = character.HunterTalents.TrueshotAura;
                Buff a = Buff.GetBuffByName("Trueshot Aura");
                Buff b = Buff.GetBuffByName("Unleashed Rage");
                Buff c = Buff.GetBuffByName("Abomination's Might");
                if (hasRelevantBuff > 0)
                {
                    if (character.ActiveBuffs.Contains(a)) { character.ActiveBuffs.Remove(a); removedBuffs.Add(a); }
                    if (character.ActiveBuffs.Contains(b)) { character.ActiveBuffs.Remove(b); removedBuffs.Add(b); }
                    if (character.ActiveBuffs.Contains(c)) { character.ActiveBuffs.Remove(c); removedBuffs.Add(c); }
                }
            }*/
            #endregion

            #region Special Pot Handling
            /*foreach (Buff potionBuff in character.ActiveBuffs.FindAll(b => b.Name.Contains("Potion")))
            {
                if (potionBuff.Stats._rawSpecialEffectData != null
                    && potionBuff.Stats._rawSpecialEffectData[0] != null)
                {
                    Stats newStats = new Stats();
                    newStats.AddSpecialEffect(new SpecialEffect(potionBuff.Stats._rawSpecialEffectData[0].Trigger,
                                                                potionBuff.Stats._rawSpecialEffectData[0].Stats,
                                                                potionBuff.Stats._rawSpecialEffectData[0].Duration,
                                                                calcOpts.Duration,
                                                                potionBuff.Stats._rawSpecialEffectData[0].Chance,
                                                                potionBuff.Stats._rawSpecialEffectData[0].MaxStack));

                    Buff newBuff = new Buff() { Stats = newStats };
                    character.ActiveBuffs.Remove(potionBuff);
                    character.ActiveBuffsAdd(newBuff);
                    removedBuffs.Add(potionBuff);
                    addedBuffs.Add(newBuff);
                }
            }*/
            #endregion

            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            foreach (Buff b in removedBuffs) {
                character.ActiveBuffsAdd(b);
            }
            foreach (Buff b in addedBuffs) {
                character.ActiveBuffs.Remove(b);
            }

            return statsBuffs;
        }
        #endregion
    }
}
