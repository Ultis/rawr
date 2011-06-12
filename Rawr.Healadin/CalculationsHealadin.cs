using System;
using System.Windows.Media;
using System.Collections.Generic;

namespace Rawr.Healadin
{
    [Rawr.Calculations.RawrModelInfo("Healadin", "Spell_Holy_HolyBolt", CharacterClass.Paladin)]
    public class CalculationsHealadin : CalculationsBase
    {

        #region Model Properties
        public override List<GemmingTemplate> DefaultGemmingTemplates {
            get {
                // Relevant Gem IDs for Healadin
                // Red
                int[] brilliant = { 52084, 52207, 52207, 52257 };   // Intellect

                // Purple
                int[] purified = { 52100, 52236, 52236, 52236 };    // Intellect + Spirit

                // Blue
                int[] lustrous = { 52087, 52244, 52244, 52262 };    // Spirit

                // Green

                // Yellow

                // Orange
                int[] reckless = { 39959, 52208, 52208, 52208 };    // Intellect + Haste

                // Prismatic

                // Meta
                int insightful = 41401;
                int revitalizing = 52297;
                int ember = 52296;
            
                string[] qualityGroupNames = new string[] { "Uncommon", "Rare", "Epic", "Jeweler" };
                string[] typeGroupNames = new string[] { "Intellect" };

                int[] metaTemplates = new int[] { insightful, revitalizing, ember };

                //    Red           Yellow      Blue        Prismatic
 
                int[,][] intellectTemplates = new int[,][]
                { // Intellect
                    { brilliant,    brilliant,  brilliant,  brilliant },
                    { brilliant,    reckless,   brilliant,  brilliant },
                    { brilliant,    brilliant,  purified,   brilliant },
                    { brilliant,    reckless,   purified,   brilliant },
                };

                int[][,][] gemmingTemplates = new int[][,][] { intellectTemplates };

                // Generate List of Gemming Templates
                List<GemmingTemplate> gemmingTemplate = new List<GemmingTemplate>();
                for (int qualityId = 0; qualityId <= qualityGroupNames.GetUpperBound(0); qualityId++) {
                    for (int typeId = 0; typeId <= typeGroupNames.GetUpperBound(0); typeId++) {
                        for (int templateId = 0; templateId <= gemmingTemplates[typeId].GetUpperBound(0); templateId++) {
                            for (int metaId = 0; metaId <= metaTemplates.GetUpperBound(0); metaId++) {
                                gemmingTemplate.Add(new GemmingTemplate()
                                {
                                    Model = "Healadin",
                                    Group = string.Format("{0} - {1}", qualityGroupNames[qualityId], typeGroupNames[typeId]),
                                    RedId = gemmingTemplates[typeId][templateId, 0][qualityId],
                                    YellowId = gemmingTemplates[typeId][templateId, 1][qualityId],
                                    BlueId = gemmingTemplates[typeId][templateId, 2][qualityId],
                                    PrismaticId = gemmingTemplates[typeId][templateId, 3][qualityId],
                                    MetaId = metaTemplates[metaId],
                                    Enabled = qualityGroupNames[qualityId] == "Epic" && typeGroupNames[typeId] == "Intellect",
                                });
                            }
                        }
                    }
                }
                return gemmingTemplate;
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
                    "Holy Light Time",
                    "Flash of Light Cast Time",
                    "Flash of Light HPS",
                    "Flash of Light Time",
                    };
                return _optimizableCalculationLabels;
            }
        }

        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
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
                    "Basic Stats:Spirit",
                    "Basic Stats:Mastery Rating",
                    "Basic Stats:Spell Power",
                    "Basic Stats:Mana Regen",
                    "Basic Stats:Combat Regen",
                    // "Basic Stats:Mp5",
                    "Basic Stats:Spell Crit",
                    "Basic Stats:Spell Haste",

                    "Cycle Stats:Total Healed",
                    "Cycle Stats:Total Mana",
                    "Cycle Stats:Average Healing per sec",
                    "Cycle Stats:Average Healing per mana",
                    "Cycle Stats:Fight Length",
                   
                    "Healing Breakdown:Holy Light Healed",
                    "Healing Breakdown:Divine Light Healed",
                    "Healing Breakdown:Flash of Light Healed",
                    "Healing Breakdown:Holy Shock Healed",
                    "Healing Breakdown:Word of Glory Healed",
                    "Healing Breakdown:Light of Dawn Healed",
                    "Healing Breakdown:Holy Radiance Healed",
                    "Healing Breakdown:Beacon of Light Healed",
                    "Healing Breakdown:Lay on Hands Healed",
                    "Healing Breakdown:Protector of the Innocent ",
                    "Healing Breakdown:Enlightened Judgements ",
                    "Healing Breakdown:Illuminated Healing",
                    "Healing Breakdown:Cleanse casting",
                    // "Healing Breakdown:Glyph of HL Healed", take this out, seems like this glyph no longer exists
                    "Healing Breakdown:Other Healed*From trinket procs",

                    "Spell Information:Holy Light",
                    "Spell Information:Flash of Light",
                    "Spell Information:Holy Shock",
                    "Spell Information:Divine Light",  
                    "Spell Information:Word of Glory(3 holy power)",   
                    "Spell Information:LoD(3 hp, max targets)",  
                    "Spell Information:Holy Radiance (max)",  
                    "Spell Information:Lay on Hands",  
                    "Spell Information:Protector of the Innocent",
                    "Spell Information:Enlightened Judgements",
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
                    "Mana Pool Breakdown (needs updating)",
                    "Mana Usage Breakdown (needs updating)",
                    "Healing Breakdown (needs updating)",
                    "Rotation Breakdown (needs updating)",
                    "Stats Graph",
                    };
                return _customChartNames;
            }
        }

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
            // First things first, we need to ensure that we aren't using bad data
            CharacterCalculationsHealadin calc = new CharacterCalculationsHealadin();
            if (character == null) { return calc; }
            CalculationOptionsHealadin calcOpts = character.CalculationOptions as CalculationOptionsHealadin;
            if (calcOpts == null) { return calc; }
            //
            Stats stats;
            calc = null;
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
            CalculationOptionsHealadin calcOpts = character.CalculationOptions as CalculationOptionsHealadin;
            BossOptions bossOpts = character.BossOptions;
            PaladinTalents talents = character.PaladinTalents;
            
            float fightLength = bossOpts.BerserkTimer;

            Stats statsRace = BaseStats.GetBaseStats(character.Level, CharacterClass.Paladin, character.Race);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character, calcOpts);
            Stats stats = statsBaseGear + statsBuffs + statsRace;

            ConvertRatings(stats, talents, calcOpts);
            if (computeAverageStats)
            {
                Stats statsAverage = new Stats();

                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    float trigger = 0f;
                    if (calc == null)
                    {
                        trigger = 1.5f / calcOpts.Activity / (1f + stats.SpellHaste);
                        if (effect.Trigger == Trigger.HealingSpellCrit || effect.Trigger == Trigger.SpellCrit)
                            trigger *= stats.SpellCrit;
                    }
                    else
                    {
                        if (effect.Trigger == Trigger.HealingSpellCast || effect.Trigger == Trigger.HealingSpellHit)
                            trigger = 1f / Rotation.GetHealingCastsPerSec(calc);
                        else if (effect.Trigger == Trigger.HealingSpellCrit || effect.Trigger == Trigger.SpellCrit)
                            trigger = 1f / Rotation.GetHealingCritsPerSec(calc);
                        else if (effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.SpellHit)
                            trigger = 1f / Rotation.GetSpellCastsPerSec(calc);
                        else if (effect.Trigger == Trigger.DamageOrHealingDone)
                            trigger = 1f / Rotation.GetHealingCastsPerSec(calc);
                        else if (effect.Trigger == Trigger.Use)
                        {
                            trigger = 0f;
                            foreach (SpecialEffect childEffect in effect.Stats.SpecialEffects())
                            {
                                float childTrigger = 0f;

                                if (effect.Trigger == Trigger.HealingSpellCast || effect.Trigger == Trigger.HealingSpellHit)
                                    childTrigger = 1f / Rotation.GetHealingCastsPerSec(calc);
                                else if (effect.Trigger == Trigger.HealingSpellCrit || effect.Trigger == Trigger.SpellCrit)
                                    childTrigger = 1f / Rotation.GetHealingCritsPerSec(calc);
                                else if (effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.SpellHit)
                                    childTrigger = 1f / Rotation.GetSpellCastsPerSec(calc);

                                statsAverage.Accumulate(childEffect.Stats,
                                                        effect.GetAverageUptime(0.0f, 1.0f)
                                                            * childEffect.GetAverageStackSize(childTrigger, 1f, 1.5f, effect.Duration));
                            }
                        }
                        else continue;
                    }
                    statsAverage.Accumulate(effect.GetAverageStats(trigger, 1f, 1.5f, fightLength));
                }
                statsAverage.ManaRestore *= fightLength;
                statsAverage.Healed *= fightLength;

                stats = statsBaseGear + statsBuffs + statsRace + statsAverage;
                ConvertRatings(stats, talents, calcOpts);
            }

            #region Set Bonuses
            int T11Count;
            character.SetBonusCount.TryGetValue("Reinforced Sapphirium Regalia", out T11Count);
            stats.BonusCritChanceDeathCoil = 0; // using this for Holy Light crit bonus, for now
            stats.BonusCritChanceFrostStrike = 0;  // yes, I'm pure evil, using this to track 4T11 
            if (T11Count >= 2)
            {
               
                // T11 Pally 2 piece bonus: add 5% crit to HL
                stats.BonusCritChanceDeathCoil = 0.05f;
            }
            if (T11Count >= 4)
            {
                // T11 Pally 4 piece bonus: 540 spirit buff for 6 secs after HS cast
                stats.BonusCritChanceFrostStrike = 1; 
            }
            #endregion Set Bonuses

            return stats;
        }

        private void ConvertRatings(Stats stats, PaladinTalents talents, CalculationOptionsHealadin calcOpts)
        {
            stats.Stamina *= 1f + stats.BonusStaminaMultiplier;

            // Intellect is used to calculate initial mana pool.
            // To avoid temporary intellect from highest stat procs changing initial mana pool
            // we track temporary intellect separatly in HighestStat property and combine it with intellect
            // when needed.
            // TODO: However this doesn't help to deal with pure temporary intellect procs (if there are any).
            // NOTE: If we add highest stat to intellect after we calculate mana, the only visible change
            // will be the displayed intellect for the character.
            stats.Intellect *= (1f + stats.BonusIntellectMultiplier);
            stats.HighestStat *= (1f + stats.BonusIntellectMultiplier);

            stats.SpellCrit = stats.SpellCrit +
                StatConversion.GetSpellCritFromIntellect(
                    stats.Intellect + stats.HighestStat,
                    CharacterClass.Paladin) +
                StatConversion.GetSpellCritFromRating(stats.CritRating, CharacterClass.Paladin); 
            
            // I want to track haste before talent seperately, going to use RangedHaste for that.
            // I plan to use this on the "Stats" page so I can report sources of haste seperatly
            stats.RangedHaste = (1f + stats.SpellHaste) *
                (1f + StatConversion.GetSpellHasteFromRating(stats.HasteRating, CharacterClass.Paladin))
                - 1f;

            // calculating physical haste for use in melee attacks, which will generate mana
            // can also divide spellhaste / physicalhaste to get orignal value of spellhaste, which is from buffs as far as I can tell
            stats.PhysicalHaste = (1f + talents.JudgementsOfThePure * 0.03f) *
                (1f + talents.SpeedOfLight * 0.01f) *
                (1f + StatConversion.GetSpellHasteFromRating(stats.HasteRating, CharacterClass.Paladin))
                - 1f;

            stats.SpellHaste = (1f + talents.JudgementsOfThePure * 0.03f) *
                (1f + talents.SpeedOfLight * 0.01f) *
                (1f + stats.SpellHaste) *
                (1f + StatConversion.GetSpellHasteFromRating(stats.HasteRating, CharacterClass.Paladin))
                - 1f;

            // GetManaFromIntellect/GetHealthFromStamina account for the fact 
            // that the first 20 Int/Sta only give 1 Mana/Health each.
            stats.Mana += StatConversion.GetManaFromIntellect(stats.Intellect, CharacterClass.Paladin) * 
                (1f + stats.BonusManaMultiplier);
            stats.Health += StatConversion.GetHealthFromStamina(stats.Stamina, CharacterClass.Paladin);

            stats.PhysicalHit += StatConversion.GetPhysicalHitFromRating(
                stats.HitRating, 
                CharacterClass.Paladin);
        }
        #endregion


        #region Custom Charts
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)

        {
            if (chartName == "Mana Pool Breakdown (needs updating)")
            {
                CharacterCalculationsHealadin calc = GetCharacterCalculations(character) as CharacterCalculationsHealadin;
                if (calc == null) calc = new CharacterCalculationsHealadin();

                ComparisonCalculationHealadin Base = new ComparisonCalculationHealadin("Base");
                ComparisonCalculationHealadin Mp5 = new ComparisonCalculationHealadin("Mp5");
                ComparisonCalculationHealadin Replenishment = new ComparisonCalculationHealadin("Replenishment");
                ComparisonCalculationHealadin ArcaneTorrent = new ComparisonCalculationHealadin("Arcane Torrent");
                ComparisonCalculationHealadin DivinePlea = new ComparisonCalculationHealadin("Divine Plea");
                ComparisonCalculationHealadin LoH = new ComparisonCalculationHealadin("Lay on Hands");
                ComparisonCalculationHealadin Other = new ComparisonCalculationHealadin("Potion & Other");

                Base.OverallPoints = Base.ThroughputPoints = calc.ManaBase;
                Mp5.OverallPoints = Mp5.ThroughputPoints = calc.ManaMp5;
                LoH.OverallPoints = LoH.ThroughputPoints = calc.ManaLayOnHands;
                Replenishment.OverallPoints = Replenishment.ThroughputPoints = calc.ManaReplenishment;
                ArcaneTorrent.OverallPoints = ArcaneTorrent.ThroughputPoints = calc.ManaArcaneTorrent;
                DivinePlea.OverallPoints = DivinePlea.ThroughputPoints = calc.ManaDivinePlea;
                Other.OverallPoints = Other.ThroughputPoints = calc.ManaOther;

                return new ComparisonCalculationBase[] { Base, Mp5, Replenishment, LoH, ArcaneTorrent, DivinePlea, Other };
            }
            else if (chartName == "Mana Usage Breakdown (needs updating)")
            {
                CharacterCalculationsHealadin calc = GetCharacterCalculations(character) as CharacterCalculationsHealadin;
                if (calc == null) calc = new CharacterCalculationsHealadin();

                ComparisonCalculationHealadin FoL = new ComparisonCalculationHealadin("Flash of Light");
                ComparisonCalculationHealadin HL = new ComparisonCalculationHealadin("Holy Light");
                ComparisonCalculationHealadin HS = new ComparisonCalculationHealadin("Holy Shock");
                ComparisonCalculationHealadin JotP = new ComparisonCalculationHealadin("Judgements and Seals");
                ComparisonCalculationHealadin BoL = new ComparisonCalculationHealadin("Beacon of Light");

                FoL.OverallPoints = FoL.ThroughputPoints = calc.UsageFoL;
                HL.OverallPoints = HL.ThroughputPoints = calc.UsageHL;
                HS.OverallPoints = HS.ThroughputPoints = calc.UsageHS;
                JotP.OverallPoints = JotP.ThroughputPoints = calc.UsageJudge;
                BoL.OverallPoints = BoL.ThroughputPoints = calc.UsageBoL;

                return new ComparisonCalculationBase[] { FoL, HL, HS, JotP, BoL };
            }
            else if (chartName == "Healing Breakdown (needs updating)")
            {
                CharacterCalculationsHealadin calc = GetCharacterCalculations(character) as CharacterCalculationsHealadin;
                if (calc == null) calc = new CharacterCalculationsHealadin();

                ComparisonCalculationHealadin FoL = new ComparisonCalculationHealadin("Flash of Light");
                ComparisonCalculationHealadin HL = new ComparisonCalculationHealadin("Holy Light");
                ComparisonCalculationHealadin HS = new ComparisonCalculationHealadin("Holy Shock");
                ComparisonCalculationHealadin GHL = new ComparisonCalculationHealadin("Glyph of Holy Light");
                ComparisonCalculationHealadin BoL = new ComparisonCalculationHealadin("Beacon of Light");

                FoL.OverallPoints = FoL.ThroughputPoints = calc.HealedFoL / calc.FightLength;
                HL.OverallPoints = HL.ThroughputPoints = calc.HealedHL / calc.FightLength;
                HS.OverallPoints = HS.ThroughputPoints = calc.HealedHS / calc.FightLength;
                GHL.OverallPoints = GHL.ThroughputPoints = calc.HealedGHL / calc.FightLength;
                BoL.OverallPoints = BoL.ThroughputPoints = calc.HealedBoL / calc.FightLength;

                return new ComparisonCalculationBase[] { FoL, HL, HS, GHL, BoL };
            }
            else if (chartName == "Rotation Breakdown (needs updating)")
            {
                CharacterCalculationsHealadin calc = GetCharacterCalculations(character) as CharacterCalculationsHealadin;
                if (calc == null) calc = new CharacterCalculationsHealadin();

                ComparisonCalculationHealadin FoL = new ComparisonCalculationHealadin("Flash of Light");
                ComparisonCalculationHealadin HL = new ComparisonCalculationHealadin("Holy Light");
                ComparisonCalculationHealadin HS = new ComparisonCalculationHealadin("Holy Shock");
                ComparisonCalculationHealadin JotP = new ComparisonCalculationHealadin("Judgements and Seals");
                ComparisonCalculationHealadin BoL = new ComparisonCalculationHealadin("Beacon of Light");

                FoL.OverallPoints = FoL.ThroughputPoints = calc.RotationFoL;
                HL.OverallPoints = HL.ThroughputPoints = calc.RotationHL;
                HS.OverallPoints = HS.ThroughputPoints = calc.RotationHS;
                JotP.OverallPoints = JotP.ThroughputPoints = calc.RotationJudge;
                BoL.OverallPoints = BoL.ThroughputPoints = calc.RotationBoL;

                return new ComparisonCalculationBase[] { FoL, HL, HS, JotP, BoL };
            }
            
            return new ComparisonCalculationBase[] {};
        }
        /*
        public override void UpdateCustomChartData(Character character, string chartName, System.Windows.Controls.Control control)
        {
            Color[] statColors = new Color[] { 
                Color.FromArgb(0xFF, 0xFF, 0, 0), 
                Color.FromArgb(0xFF, 0xFF, 165, 0), 
                Color.FromArgb(0xFF, 0x80, 0x80, 0x00), 
                Color.FromArgb(0xFF, 154, 205, 50), 
                Color.FromArgb(0xFF, 0x00, 0xFF, 0xFF), 
                Color.FromArgb(0xFF, 0, 0, 0xFF), 
            };

            List<float> X = new List<float>();
            List<ComparisonCalculationBase[]> Y = new List<ComparisonCalculationBase[]>();

            float fMultiplier = 1;
            Stats[] statsList = new Stats[] {
                        new Stats() { Intellect = fMultiplier },
                        new Stats() { SpellPower = fMultiplier },
                        new Stats() { HasteRating = fMultiplier },
                        new Stats() { CritRating = fMultiplier },
                        new Stats() { Spirit = fMultiplier },
                        new Stats() { MasteryRating = fMultiplier },
                    };

            switch (chartName)
            {
                case "Stats Graph":
                    Graph.Instance.UpdateStatsGraph(character, statsList, statColors, 200, "", null);
                    break;
            }
        }
        */

        #endregion Custom Charts

        #region Relevancy Methods

        private static bool isHitIrrelevant = true;
        internal static bool IsHitIrrelevant
        {
            get { return isHitIrrelevant; }
            set { isHitIrrelevant = value; }
        }

        private List<ItemType> relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                if (relevantItemTypes == null)
                {
                    relevantItemTypes = new List<ItemType>(new ItemType[]
                    {
                        ItemType.Plate,
                        ItemType.None,
                        ItemType.Shield,
                        ItemType.Libram,ItemType.Relic,
                        ItemType.OneHandAxe,
                        ItemType.OneHandMace,
                        ItemType.OneHandSword
                    });
                }
                return relevantItemTypes;
            }
        }

        private static List<string> relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            if (relevantGlyphs == null)
            {
                relevantGlyphs = new List<string>();
                //Prime                                            
                relevantGlyphs.Add("Glyph of Word of Glory");    
                relevantGlyphs.Add("Glyph of Seal of Insight");  
                relevantGlyphs.Add("Glyph of Holy Shock");       
                relevantGlyphs.Add("Glyph of Divine Favor");
                //Major
                relevantGlyphs.Add("Glyph of Beacon of Light");
                relevantGlyphs.Add("Glyph of Divine Plea");
                relevantGlyphs.Add("Glyph of Cleansing");
                relevantGlyphs.Add("Glyph of Divinity");
                relevantGlyphs.Add("Glyph of Salvation");
                relevantGlyphs.Add("Glyph of Long Word"); 
                relevantGlyphs.Add("Glyph of Light of Dawn");
                relevantGlyphs.Add("Glyph of Lay on Hands");  
                //Minor
                relevantGlyphs.Add("Glyph of Insight");
                relevantGlyphs.Add("Glyph of Blessing of Might");
                relevantGlyphs.Add("Glyph of Blessing of Kings");
            }
            return relevantGlyphs;
        }

        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot)
        {
            // Filters out Non-Shield Offhand Enchants and Ranged Enchants
            if ((slot == ItemSlot.OffHand && enchant.Slot != ItemSlot.OffHand) || slot == ItemSlot.Ranged) return false;
            // Filters out Death Knight and Two-Hander Enchants
            if (enchant.Name.StartsWith("Rune of the") || enchant.Slot == ItemSlot.TwoHand) return false;

            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique)
        {
            if (slot == CharacterSlot.OffHand && !(item.Type == ItemType.Shield || item.Type == ItemType.None)) return false;
            return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
        }

        private bool IsTriggerRelevant(Trigger trigger)
        {
            return (
                trigger == Trigger.Use                 ||
                trigger == Trigger.SpellCast           || trigger == Trigger.SpellCrit         ||
                trigger == Trigger.SpellHit            || trigger == Trigger.HealingSpellCast  ||
                trigger == Trigger.HealingSpellCrit    || trigger == Trigger.HealingSpellHit   ||
                trigger == Trigger.DamageOrHealingDone ||
                trigger == Trigger.HolyRadianceActive
            );
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                #region These aren't really relevant stats, but they should still appear in the tooltip

                Stamina = stats.Stamina,
                Health = stats.Health,
                Spirit = stats.Spirit,
                HitRating = stats.HitRating,

                #endregion

                Intellect = stats.Intellect,
                Mp5 = stats.Mp5,
                SpellPower = stats.SpellPower,
                CritRating = stats.CritRating,
                HasteRating = stats.HasteRating,
                MasteryRating = stats.MasteryRating,
                Mana = stats.Mana,
                ManaRestore = stats.ManaRestore,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusManaPotionEffectMultiplier = stats.BonusManaPotionEffectMultiplier,
                SpellCrit = stats.SpellCrit,
                SpellHaste = stats.SpellHaste,
                HealingReceivedMultiplier = stats.HealingReceivedMultiplier,
                BonusHealingDoneMultiplier = stats.BonusHealingDoneMultiplier,
                MovementSpeed = stats.MovementSpeed,
                SnareRootDurReduc = stats.SnareRootDurReduc,
                FearDurReduc = stats.FearDurReduc,
                StunDurReduc = stats.StunDurReduc,
                ShieldFromHealedProc = stats.ShieldFromHealedProc,

                // Gear Procs
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,
                BonusManaMultiplier = stats.BonusManaMultiplier,
                BonusCritHealMultiplier = stats.BonusCritHealMultiplier,
                SpellsManaCostReduction = stats.SpellsManaCostReduction,
                HolySpellsManaCostReduction = stats.HolySpellsManaCostReduction,

                // Ony Shiny Shard of the Flame
                Healed = stats.Healed,
            };
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (IsTriggerRelevant(effect.Trigger) && HasRelevantStats(effect.Stats))
                {
                    s.AddSpecialEffect(effect);
                }
            }
            return s;
        }

        bool useIrrelevancy = false;
        public override bool IsItemRelevant(Item item)
        {
            if (!item.IsGem) { useIrrelevancy = true; }
            bool result = base.IsItemRelevant(item);
            useIrrelevancy = false;
            return result;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            if (useIrrelevancy)
            {
                if (isHitIrrelevant && stats.HitRating > 0 ) return false;
            }

            if (stats.Strength > 0 || stats.Agility > 0) return false;

            bool relevant = (
                stats.Intellect +
                stats.Mp5 +
                stats.SpellPower +
                stats.CritRating +
                stats.HasteRating +
                stats.MasteryRating +
                stats.Mana +
                stats.ManaRestore +
                stats.BonusIntellectMultiplier +
                stats.BonusManaPotionEffectMultiplier +
                stats.SpellCrit +
                stats.SpellHaste +
                stats.HealingReceivedMultiplier +
                stats.BonusHealingDoneMultiplier +
                stats.MovementSpeed +
                stats.SnareRootDurReduc +
                stats.FearDurReduc +
                stats.StunDurReduc +
                stats.ShieldFromHealedProc +
                stats.HighestStat +

                stats.ManaRestoreFromMaxManaPerSecond +
                stats.BonusManaMultiplier +
                stats.BonusCritHealMultiplier +
                stats.SpellsManaCostReduction +
                stats.HolySpellsManaCostReduction +

                stats.Healed
                ) != 0;

            bool hasTrigger = false;
            bool hasRelevantTrigger = false;
            bool triggerHasRelevantStats;

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                hasTrigger = true;
                if (IsTriggerRelevant(effect.Trigger))
                {
                    triggerHasRelevantStats = HasRelevantStats(effect.Stats);
                    relevant |= triggerHasRelevantStats;
                    hasRelevantTrigger |= triggerHasRelevantStats;
                }
            }
            return relevant && (!hasTrigger || hasRelevantTrigger);
        }

        public override bool IsBuffRelevant(Buff buff, Character character)
        {
            Stats stats = buff.Stats;
            bool hasRelevantBuffStats = HasRelevantStats(stats);

            bool NotClassSetBonus = buff.Group == "Set Bonuses" && (buff.AllowedClasses.Count != 1 || (buff.AllowedClasses.Count == 1 && !buff.AllowedClasses.Contains(CharacterClass.Paladin)));

            bool relevant = hasRelevantBuffStats && !NotClassSetBonus;
            return relevant;
        }

        public Stats GetBuffsStats(Character character, CalculationOptionsHealadin calcOpts) {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            //float hasRelevantBuff;

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

            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs, character.SetBonusCount);

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
