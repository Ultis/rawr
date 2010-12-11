using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Media;

namespace Rawr.RestoSham
{
    [Rawr.Calculations.RawrModelInfo("RestoSham", "Spell_Nature_Magicimmunity", CharacterClass.Shaman)]
    public class CalculationsRestoSham : CalculationsBase
    {
        #region Fields

        // Base mana at level 80 and 85
        private const float _BaseMana = 23430f;

        // Carry over calculation variables
        private float _HealPerSec = 0f;
        private float _HealHitPerSec = 0f;
        private float _CritPerSec = 0f;
        private float _FightSeconds = 0f;
        private float _CastingActivity = 0f;

        private ReferenceCharacter _ReferenceShaman = null;

        #endregion

        #region Basic Model Information and Initialization

        //
        // This model is for shammies!
        //
        public override CharacterClass TargetClass
        {
            get { return CharacterClass.Shaman; }
        }

        /// <summary>
        /// Method to get a new instance of the model's custom ComparisonCalculation class.
        /// </summary>
        /// <returns>
        /// A new instance of the model's custom ComparisonCalculation class,
        /// which inherits from ComparisonCalculationBase
        /// </returns>
        public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationRestoSham();
        }

        /// <summary>
        /// Method to get a new instance of the model's custom CharacterCalculations class.
        /// </summary>
        /// <returns>
        /// A new instance of the model's custom CharacterCalculations class,
        /// which inherits from CharacterCalculationsBase
        /// </returns>
        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsRestoSham();
        }

        #endregion

        #region Stats Methods

        /// <summary>
        /// GetCharacterStats is the 2nd-most calculation intensive method in a model. Here the model will
        /// combine all of the information about the character, including race, gear, enchants, buffs,
        /// calculationoptions, etc., to form a single combined Stats object. Three of the methods below
        /// can be called from this method to help total up stats: GetItemStats(character, additionalItem),
        /// GetEnchantsStats(character), and GetBuffsStats(character.ActiveBuffs).
        /// </summary>
        /// <param name="character">The character whose stats should be totaled.</param>
        /// <param name="additionalItem">An additional item to treat the character as wearing.
        /// This is used for gems, which don't have a slot on the character to fit in, so are just
        /// added onto the character, in order to get gem calculations.</param>
        /// <returns>
        /// A Stats object containing the final totaled values of all character stats.
        /// </returns>
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            return GetCharacterStats(character, additionalItem, null);
        }

        /// <summary>
        /// Filters a Stats object to just the stats relevant to the model.
        /// </summary>
        /// <param name="stats">A complete Stats object containing all stats.</param>
        /// <returns>
        /// A filtered Stats object containing only the stats relevant to the model.
        /// </returns>
        public override Stats GetRelevantStats(Stats stats)
        {
            Stats relevantStats = new Stats();
            if (stats == null)
                return relevantStats;

            Type statsType = typeof(Stats);

            foreach (string relevantStat in Relevants.RelevantStats)
            {
                float v = (float)statsType.GetProperty(relevantStat).GetValue(stats, null);
                if (v > 0)
                {
                    statsType.GetProperty(relevantStat).SetValue(relevantStats, v, null);
                }
            }

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.HealingSpellCast ||
                    effect.Trigger == Trigger.HealingSpellCrit ||
                    effect.Trigger == Trigger.HealingSpellHit ||
                    effect.Trigger == Trigger.SpellCast ||
                    effect.Trigger == Trigger.SpellCrit ||
                    effect.Trigger == Trigger.SpellHit ||
                    effect.Trigger == Trigger.Use)
                {
                    if (HasRelevantStats(effect.Stats))
                        relevantStats.AddSpecialEffect(effect);
                }
            }

            return relevantStats;
        }

        /// <summary>
        /// Tests whether there are positive relevant stats in the Stats object.
        /// </summary>
        /// <param name="stats">The complete Stats object containing all stats.</param>
        /// <returns>
        /// True if any of the positive stats in the Stats are relevant.
        /// </returns>
        public override bool HasRelevantStats(Stats stats)
        {
            if (stats == null)
                return false;

            // Accumulate the "base" stats with the special effect stats.
            Stats comparison = new Stats();
            comparison.Accumulate(stats);
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                comparison.Accumulate(effect.Stats);
            }

            float statTotal = 0f;

            // Loop over each relevant stat and get its value
            Type statsType = typeof(Stats);
            foreach (string relevantStat in Relevants.RelevantStats)
            {
                float v = (float)statsType.GetProperty(relevantStat).GetValue(comparison, null);
                if (v > 0)
                    statTotal += v;
            }

            // if statTotal > 0 then we have relevant stats
            return statTotal > 0;
        }

        #endregion

        #region Default Buff Setup -- NEEDS UPDATING

        /// <summary>
        /// Sets the defaults for a RestoShaman character
        /// </summary>
        /// <param name="character">The character object to set the defaults for</param>
        public override void SetDefaults(Character character)
        {
            if (character == null)
                return;

            character.ActiveBuffsAdd(("Improved Moonkin Form"));
            character.ActiveBuffsAdd(("Tree of Life Aura"));
            character.ActiveBuffsAdd(("Arcane Intellect"));
            character.ActiveBuffsAdd(("Vampiric Touch"));
            character.ActiveBuffsAdd(("Mana Spring Totem"));
            character.ActiveBuffsAdd(("Restorative Totems"));
            character.ActiveBuffsAdd(("Moonkin Form"));
            character.ActiveBuffsAdd(("Wrath of Air Totem"));
            character.ActiveBuffsAdd(("Totem of Wrath (Spell Power)"));
            character.ActiveBuffsAdd(("Power Word: Fortitude"));
            character.ActiveBuffsAdd(("Improved Power Word: Fortitude"));
            character.ActiveBuffsAdd(("Mark of the Wild"));
            character.ActiveBuffsAdd(("Improved Mark of the Wild"));
            character.ActiveBuffsAdd(("Blessing of Kings"));
            character.ActiveBuffsAdd(("Flask of the Frost Wyrm"));
            character.ActiveBuffsAdd(("Spell Power Food"));
        }

        #endregion

        #region Gemming Templates (Updated for 4.0.3a and Beyond.  Rare ID's used as placeholders for epics.)

        /// <summary>
        /// List of default gemming templates recommended by the model
        /// </summary>
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                ////Relevant Gem IDs for Resto Shaman - Goes Normal, Rare, Epic, and JC.
                //Red
                int[] Brilliant = { 52084, 52207, 52207, 52257 };  // Int
                //Purple
                int[] Purified = { 52100, 52236, 52236 };  // Int/Spir
                int[] Timeless = { 52098, 52248, 52248 };  //Int/Stam
                //Blue
                int[] Sparkling = { 52087, 52244, 52244, 52262 };  // Spir
                //Green
                int[] Forceful = { 52124, 52218, 52218 };  // Haste/Stam
                int[] Jagged = { 52121, 52223, 52223 };    // Crit/Stam
                int[] Puissant = { 52126, 52231, 52231 };  //Mastery / Stam
                int[] Zen = { 52127, 52250, 52250 };       //Mastery / Spirit
                //Yellow
                int[] Quick = { 52093, 52232, 52232, 52268 };      // Haste
                int[] Smooth = { 52091, 52241, 52241, 52266 };     // Crit
                int[] Fractured = { 52094, 52219, 52219, 52269 };  // Mastery
                //Orange
                int[] Artful = { 52117, 52205, 52205 };   // Int/Mastery
                int[] Reckless = { 52113, 52208, 52208 }; // Haste/Int
                int[] Potent = { 52110, 52239, 52239 };   // Crit/Int
                //Meta
                int Ember = 52296;         //  Int/MaxMana2%
                int Fleet = 52289;         //  Mastery/Reduced Threat
                int Revitalizing = 52297;  //  Spirit/IncreaseHealCrit

                return new List<GemmingTemplate>()
                {
                    #region Mana Regen Templates
                    new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", RedId = Sparkling[0], YellowId = Sparkling[0], BlueId = Sparkling[0], PrismaticId = Sparkling[0], MetaId = Revitalizing },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Rare", Enabled = true, RedId = Sparkling[1], YellowId = Sparkling[1], BlueId = Sparkling[1], PrismaticId = Sparkling[1], MetaId = Revitalizing },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", RedId = Sparkling[3], YellowId = Sparkling[3], BlueId = Sparkling[3], PrismaticId = Sparkling[3], MetaId = Revitalizing },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", RedId = Purified[0], YellowId = Zen[0], BlueId = Sparkling[0], PrismaticId = Sparkling[0], MetaId = Revitalizing },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Rare", Enabled = true, RedId = Purified[1], YellowId = Zen[1], BlueId = Sparkling[1], PrismaticId = Sparkling[1], MetaId = Revitalizing },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", RedId = Purified[1], YellowId = Zen[1], BlueId = Sparkling[3], PrismaticId = Sparkling[3], MetaId = Revitalizing },
                    #endregion
                    #region Intellect Templates
                    new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", RedId = Brilliant[0], YellowId = Brilliant[0], BlueId = Brilliant[0], PrismaticId = Brilliant[0], MetaId = Ember },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Rarw", Enabled = true, RedId = Brilliant[1], YellowId = Brilliant[1], BlueId = Brilliant[1], PrismaticId = Brilliant[1], MetaId = Ember },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", RedId = Brilliant[3], YellowId = Brilliant[3], BlueId = Brilliant[3], PrismaticId = Brilliant[3], MetaId = Ember },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", RedId = Brilliant[0], YellowId = Reckless[0], BlueId = Timeless[0], PrismaticId = Brilliant[0], MetaId = Ember },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Rare", Enabled = true, RedId = Brilliant[1], YellowId = Reckless[1], BlueId = Timeless[1], PrismaticId = Brilliant[1], MetaId = Ember },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", RedId = Brilliant[3], YellowId = Reckless[1], BlueId = Timeless[1], PrismaticId = Brilliant[3], MetaId = Ember },
                    #endregion
                    #region Mastery Templates
                    new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", RedId = Fractured[0], YellowId = Fractured[0], BlueId = Fractured[0], PrismaticId = Fractured[0], MetaId = Fleet },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Rare", Enabled = true, RedId = Fractured[1], YellowId = Fractured[1], BlueId = Fractured[1], PrismaticId = Fractured[1], MetaId = Fleet },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", RedId = Fractured[3], YellowId = Fractured[3], BlueId = Fractured[3], PrismaticId = Fractured[3], MetaId = Fleet },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", RedId = Artful[0], YellowId = Fractured[0], BlueId = Puissant[0], PrismaticId = Fractured[0], MetaId = Fleet },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Rare", Enabled = true, RedId = Artful[1], YellowId = Fractured[1], BlueId = Puissant[1], PrismaticId = Fractured[1], MetaId = Fleet },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", RedId = Artful[1], YellowId = Fractured[3], BlueId = Puissant[1], PrismaticId = Fractured[3], MetaId = Fleet },
                    #endregion
                    #region Crit Templates
                    new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", RedId = Smooth[0], YellowId = Smooth[0], BlueId = Smooth[0], PrismaticId = Smooth[0], MetaId = Revitalizing },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Rare", Enabled = true, RedId = Smooth[1], YellowId = Smooth[1], BlueId = Smooth[1], PrismaticId = Smooth[1], MetaId = Revitalizing },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", RedId = Smooth[3], YellowId = Smooth[3], BlueId = Smooth[3], PrismaticId = Smooth[3], MetaId = Revitalizing },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", RedId = Potent[0], YellowId = Smooth[0], BlueId = Jagged[0], PrismaticId = Smooth[0], MetaId = Revitalizing },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Rare", Enabled = true, RedId = Potent[1], YellowId = Smooth[1], BlueId = Jagged[1], PrismaticId = Smooth[1], MetaId = Revitalizing },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", RedId = Potent[1], YellowId = Smooth[3], BlueId = Jagged[1], PrismaticId = Smooth[3], MetaId = Revitalizing },
                    #endregion
                    #region Haste Templates
                    new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", RedId = Quick[0], YellowId = Quick[0], BlueId = Quick[0], PrismaticId = Quick[0], MetaId = Ember },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Rare", Enabled = true, RedId = Quick[1], YellowId = Quick[1], BlueId = Quick[1], PrismaticId = Quick[1], MetaId = Ember },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", RedId = Quick[3], YellowId = Quick[3], BlueId = Quick[3], PrismaticId = Quick[3], MetaId = Ember },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", RedId = Reckless[0], YellowId = Quick[0], BlueId = Forceful[0], PrismaticId = Quick[0], MetaId = Ember },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Rare, Enabled = true", RedId = Reckless[1], YellowId = Quick[1], BlueId = Forceful[1], PrismaticId = Quick[1], MetaId = Ember },
                    new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", RedId = Reckless[1], YellowId = Quick[3], BlueId = Forceful[1], PrismaticId = Quick[3], MetaId = Ember },
                    #endregion
                };
            }
        }

        #endregion

        #region Relevant Glyphs and Item Types

        /// <summary>
        /// Which glyphs are relevant to this model.
        /// </summary>
        public override List<string> GetRelevantGlyphs()
        {
            return Relevants.RelevantGlyphs;
        }

        /// <summary>
        /// List&lt;ItemType&gt; containing all of the ItemTypes relevant to this model. Typically this
        /// means all types of armor/weapons that the intended class is able to use, but may also
        /// be trimmed down further if some aren't typically used. ItemType.None should almost
        /// always be included, because that type includes items with no proficiancy requirement, such
        /// as rings, necklaces, cloaks, held in off hand items, etc.
        /// </summary>
        public override List<ItemType> RelevantItemTypes
        {
            get { return Relevants.RelevantItemTypes; }
        }

        #endregion

        #region Chart Methods

        public override Dictionary<string, Color> SubPointNameColors
        {
            get { return RestoShamConfiguration.SubPointNameColors; }
        }

        /// <summary>
        /// An array of strings which will be used to build the calculation display.
        /// Each string must be in the format of "Heading:Label". Heading will be used as the
        /// text of the group box containing all labels that have the same Heading.
        /// Label will be the label of that calculation, and may be appended with '*' followed by
        /// a description of that calculation which will be displayed in a tooltip for that label.
        /// Label (without the tooltip string) must be unique.
        /// EXAMPLE:
        /// characterDisplayCalculationLabels = new string[]
        /// {
        /// "Basic Stats:Health",
        /// "Basic Stats:Armor",
        /// "Advanced Stats:Dodge",
        /// "Advanced Stats:Miss*Chance to be missed"
        /// };
        /// </summary>
        public override string[] CharacterDisplayCalculationLabels
        {
            get { return RestoShamConfiguration.CharacterDisplayCalculationLabels; }
        }

        /// <summary>
        /// An array of strings which define what calculations (in addition to the subpoint ratings)
        /// will be available to the optimizer
        /// </summary>
        public override string[] OptimizableCalculationLabels
        {
            get { return RestoShamConfiguration.OptimizableCalculationLabels; }
        }

        /// <summary>
        /// The names of all custom charts provided by the model, if any.
        /// </summary>
        public override string[] CustomChartNames
        {
            get { return CustomCharts.CustomChartNames; }
        }
#if !RAWR3 && !RAWR4
        // for RAWR3 || RAWR4 include all charts in CustomChartNames
        public override string[] CustomRenderedChartNames
        {
            get { return CustomCharts.CustomRenderedChartNames; }
        }
#endif

        /// <summary>
        /// Gets data to fill a custom chart, based on the chart name, as defined in CustomChartNames.
        /// </summary>
        /// <param name="character">The character to build the chart for.</param>
        /// <param name="chartName">The name of the custom chart to get data for.</param>
        /// <returns>The data for the custom chart.</returns>
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
#if !RAWR3 && !RAWR4
            ChartCalculator chartCalc = CustomCharts.GetChartCalculator(chartName);
            if (chartCalc == null)
                return new ComparisonCalculationBase[0];

            ICollection<ComparisonCalculationBase> list = chartCalc(character, this);
            ComparisonCalculationBase[] retVal = new ComparisonCalculationBase[list.Count];
            if (list.Count > 0)
                list.CopyTo(retVal, 0);
            return retVal;
#else
            return new ComparisonCalculationBase[0];
#endif
        }
#if !RAWR3 && !RAWR4
        public override void RenderCustomChart(Character character, string chartName, Graphics g, int width, int height)
        {
            string calc = chartName.Substring(0, chartName.IndexOf("Stats Graph") - 1);
            Base.Graph.RenderStatsGraph(g, width, height, character,
                CustomCharts.StatsGraphStatsList, CustomCharts.StatsGraphColors,
                200, "", calc, Base.Graph.Style.DpsWarr);
        }
#endif

        #endregion

        #region XML Methods

        /// <summary>
        /// Deserializes the model's CalculationOptions data object from xml
        /// </summary>
        /// <param name="xml">The serialized xml representing the model's CalculationOptions data object.</param>
        /// <returns>
        /// The model's CalculationOptions data object.
        /// </returns>
        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                            new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRestoSham));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsRestoSham calcOpts = serializer.Deserialize(reader) as CalculationOptionsRestoSham;
            return calcOpts;
        }

        #endregion

        #region Set calculation options and item usable options

#if RAWR3 || RAWR4
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
                    _calculationOptionsPanel = new CalculationOptionsPanelRestoSham();
                return _calculationOptionsPanel;
            }
        }

        /// <summary>
        /// Determines if the specified item fits in the specified slot.
        /// </summary>
        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique)
        {
            if (character == null || item == null)
                return false;

            // Same as base class, except resto shaman can't have an offhand weapon
            if (slot == CharacterSlot.OffHand && item.Slot == ItemSlot.OneHand)
                return false;
            return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
        }

        #endregion

        #region Calculations Area (Spells, Armor, Ratings, and Burst/Sustained Points)

        /// <summary>
        /// GetCharacterCalculations is the primary method of each model, where a majority of the calculations
        /// and formulae will be used. GetCharacterCalculations should call GetCharacterStats(), and based on
        /// those total stats for the character, and any calculationoptions on the character, perform all the
        /// calculations required to come up with the final calculations defined in
        /// CharacterDisplayCalculationLabels, including an Overall rating, and all Sub ratings defined in
        /// SubPointNameColors.
        /// </summary>
        /// <param name="character">The character to perform calculations for.</param>
        /// <param name="additionalItem">An additional item to treat the character as wearing.
        /// This is used for gems, which don't have a slot on the character to fit in, so are just
        /// added onto the character, in order to get gem calculations.</param>
        /// <param name="referenceCalculation">True if the subsequent calculations should treat this calculation as a reference, for example when called for
        /// the character displayed in main window; False for comparison calculations in comparison charts.</param>
        /// <param name="significantChange">True if the difference from reference calculation can potentially result in significantly
        /// different result, for example when changing talents or glyphs.</param>
        /// <param name="needsDisplayCalculations">When False the model can ignore any calculations that are not used for rating directly (such as GetCharacterDisplayCalculationValues).</param>
        /// <returns>
        /// A custom CharacterCalculations object which inherits from CharacterCalculationsBase,
        /// containing all of the final calculations defined in CharacterDisplayCalculationLabels. See
        /// CharacterCalculationsBase comments for more details.
        /// </returns>
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            if (character == null || character.CalculationOptions == null)
                return new CharacterCalculationsRestoSham();

            if (_ReferenceShaman == null)
                _ReferenceShaman = new ReferenceCharacter(this);

            if (referenceCalculation || significantChange)
                _ReferenceShaman.FullCalculate(character, additionalItem);
            else
                _ReferenceShaman.IncrementalCalculate(character, additionalItem);

            return _ReferenceShaman.GetCharacterCalculations(character);

            //return GetCharacterCalculations(character, additionalItem, null);
        }

        internal CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, Stats statModifier)
        {
            // First things first, we need to ensure that we aren't using bad data
            CharacterCalculationsRestoSham calc = new CharacterCalculationsRestoSham();
            if (character == null) { return calc; }
            CalculationOptionsRestoSham calcOpts = character.CalculationOptions as CalculationOptionsRestoSham;
            if (calcOpts == null) { return calc; }

            Stats stats = GetCharacterStats(character, additionalItem, statModifier);
            calc.BasicStats = stats;

            #region Armor Specialization (Thanks to Astrylian), FightSeconds, and CastingActivity
            _FightSeconds = calcOpts.FightLength * 60f;
            _CastingActivity = 1f;
            bool armorSpecialization = GetArmorSpecializationStatus(character);
            calc.MailSpecialization = armorSpecialization ? 0.05f : 0f;
            //stats.BonusStaminaMultiplier += (MailSpecialization ? .05f : 0);
            //stats.BonusSpiritMultiplier += (MailSpecialization ? .05f : 0);
            stats.BonusIntellectMultiplier += calc.MailSpecialization;
            #endregion
            #region Spell Power and Haste Based Calcs
            stats.SpellPower += stats.Intellect - 10f;
            stats.SpellPower += 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f);
            stats.SpellHaste = (1 + StatConversion.GetSpellHasteFromRating(stats.HasteRating)) * (1 + stats.SpellHaste) - 1;
            calc.SpellHaste = stats.SpellHaste;
            float Healing = 1.88f * stats.SpellPower;
            float LBSpellPower = Healing - (1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f));
            #endregion
            #region Overhealing/Latency/Interval/CH jumps values
            float GcdLatency = calcOpts.Latency / 1000;
            float Latency = Math.Max(Math.Min(GcdLatency, 0.275f) - 0.14f, 0) + Math.Max(GcdLatency - 0.275f, 0);
#if true
            float ESOverheal = 0;
            float ELWOverheal = 0;
            float CHOverheal = 0;
            float RTOverheal = 0;
            float HWOverheal = 0;
            float HWSelfOverheal = 0;
            float GHWOverheal = 0;
            float GHWSelfOverheal = 0;
            float HSrgOverheal = 0;
            float AAOverheal = 0;
            float HSTOverheal = 0;
            float ESInterval = 0;
#else
            float ESOverheal = 0.1f;
            float ELWOverheal = 0.7f;
            float CHOverheal = 0.7f;
            float RTOverheal = 0.8f;
            float HWOverheal = 0.4f;
            float HWSelfOverheal = 0.9f;
            float HSrgOverheal = 0.75f;
            float AAOverheal = 0.6f;
            float HSTOverheal = 0.9f;
            float ESInterval = 6;
#endif
            bool TankHeal = calcOpts.Targets == "Tank";
            bool RaidHeal = calcOpts.Targets == "Heavy Raid";
            bool SelfHeal = RaidHeal || calcOpts.Targets == "Self";
            calc.DeepHeals += .2f + ((stats.MasteryRating / 45.92f) * .025f); // %Health_Deficit*Mastery% = Additional Healing
            #region Theoretical deep healing based off oposite of over-heal.  Will require tweaking.  Much, Much tweaking.
            float DeepHeal = calc.DeepHeals * .39f;
            #endregion
            float ELWOverwriteScale = RaidHeal ? 0.875f : TankHeal ? 0.5f : 0.6f;
            float CHRTConsumption = RaidHeal ? 0.07f : TankHeal ? 0.5f : 0.19f;
            float CHJumps = RaidHeal ? 4 : SelfHeal ? 1.73f : TankHeal ? 1.86f : 2.5f;
            float HSTTargets = RaidHeal ? 5f : 1f;
            #endregion
            #region Intellect, Spell Crit, and MP5 Based Calcs
            stats.Mp5 += (StatConversion.GetSpiritRegenSec(stats.Spirit, stats.Intellect)) * 2.5f;
            float CritPenalty = 1f - (((CHOverheal + RTOverheal + HWOverheal + HWSelfOverheal + HSrgOverheal + AAOverheal) / 6f) / 2f);
            stats.SpellCrit = 0.022f // Base
                            + StatConversion.GetSpellCritFromIntellect(stats.Intellect) // From Int
                            + StatConversion.GetSpellCritFromRating(stats.CritRating) // From Rating
                            + stats.SpellCrit // From Percentages
                            + (0.01f * (character.ShamanTalents.Acuity)); // From Talent
            calc.SpellCrit = stats.SpellCrit;
            float CriticalScale = 1.5f * (1 + stats.BonusCritHealMultiplier);
            float CriticalChance = calc.SpellCrit;
            float Critical = 1f + ((CriticalChance * Math.Min(CriticalScale - 1, 1)) * (CritPenalty));  //  The penalty is set to ensure that while no crit will ever be valued less then a full heal, it will however be reduced more so due to overhealing.  The average currently works out close to current HEP reports and combat logs.
            float ChCritical = 1f + (((CriticalChance + (stats.RestoSham4T9 * .05f)) * Math.Min(CriticalScale - 1, 1)) * (CritPenalty));

            #endregion
            #region Healing Bonuses and scales
            //  Cost scale
            float CostScale = 1f - character.ShamanTalents.TidalFocus * .02f;
            //  Healing scale from Purification
            float PurificationScale = 1.1f + (character.ShamanTalents.SparkOfLife * .02f) + (TankHeal ? .15f : 0f);
            //  AA scale
            float AAScale = CriticalScale * character.ShamanTalents.AncestralAwakening * .1f * PurificationScale;
            //  TW chance
            float TWChance = character.ShamanTalents.TidalWaves * 0.1f;
            #endregion
            #region Water Shield and Mana Calculations
            float Orb;
            if (calcOpts.WaterShield && calcOpts.CataOrLive)
            {
                stats.Mp5 +=(character.ShamanTalents.GlyphofWaterMastery ? 1350 : 900) + 900f * stats.WaterShieldIncrease;
                Orb = (3852 * (1 + (character.ShamanTalents.ImprovedShields * .05f))) * (1 + stats.WaterShieldIncrease);
                Orb = Orb * character.ShamanTalents.ImprovedWaterShield / 3;
            }
            else
                Orb = 0;
            #endregion
            #region Earthliving Weapon Calculations
            //  ELW bonus healing = spell power
            float ELWBonusHealing = stats.SpellPower;
            //  ... * generic healing scale * HoT scale
            ELWBonusHealing *= 1.88f * 0.3636f;
            //  ELW healing scale = purification scale
            float ELWHealingScale = PurificationScale + (character.ShamanTalents.GlyphofEarthlivingWeapon ? .2f : 0);
            float ELWHPS = (((2304) + ELWBonusHealing) * ELWHealingScale / 12) * (1 + stats.BonusHealingDoneMultiplier);
            float ELWChance = 1 * ELWOverwriteScale;
            #endregion
            #region Earth Shield Calculations
            bool UseES = (calcOpts.EarthShield); // Wether or not to use ES at all - Make sure the option and the talent are on.
            float ESBonusHealing = stats.SpellPower;  //  ES bonus healing = spell power
            ESBonusHealing *= 1.88f * (1f / 3.5f);  //  ... * generic healing scale * HoT scale
            float ESHealingScale = PurificationScale;  //  ES healing scale = purification scale
            //  ... + 5%/10% Improved Earth Shield + 5%/10%/15% Improved Shields
            ESHealingScale *= 1 + character.ShamanTalents.ImprovedShields * 0.05f;
            //  ... + 20% if Glyphed
            if (character.ShamanTalents.GlyphofEarthShield)
                ESHealingScale *= 1.2f;
            float ESChargeHeal = ((1770) + ESBonusHealing) * ESHealingScale * (UseES ? 1 : 0); //  Heal per ES Charge
            float ESHeal = ESChargeHeal * 9;  //  ES if all charges heal
            float ESCost = (float)Math.Round(.15 * ((_BaseMana))) * CostScale * (UseES ? 1 : 0);
            float ESTimer = 9 * Math.Max(ESInterval, 4);
            calc.ESHPS = ((ESHeal * Critical) / ESTimer) * (1 + stats.BonusHealingDoneMultiplier);
            float ESMPS = ESCost / ESTimer;
            #endregion
            #region Base Variables ( Heals per sec and Crits per sec )
            float RTPerSec = 0;
            float RTTicksPerSec = 0;
            float HWPerSec = 0;
            float GHWPerSec = 0;
            float GHWCPerSec = 0;
            float CHPerSec = 0;
            float CHHitsPerSec = 0;
            float HSrgPerSec = 0;
            float HSrgCPerSec = 0f;
            float CHCPerSec = 0f;
            float CHCHitsPerSec = 0;
            float HWCPerSec = 0f;
            float RTCPerSec = 0f;
            float AAsPerSec = 0f;
            float ELWTicksPerSec = 0;
            #endregion
            #region Base Speeds ( Hasted / RTCast / HSrgCast / HWCast / CHCast )
            float HasteScale = 1f / (1f + calc.SpellHaste);
            float RTHaste = stats.RestoSham2T10 * 0.2f;
            float RTCast = (float)Math.Max(1.5f * HasteScale + Latency, 1f + GcdLatency);
            float RTCD = 6 - stats.RTCDDecrease;
            float RTCDCast = RTCD + GcdLatency;
            float RTDuration = 15 + (character.ShamanTalents.GlyphofRiptide ? 6 : 0);
            float HRCast = (float)Math.Max(1.5f * HasteScale + Latency, 1f + GcdLatency);
            float HRCD = 6 - stats.RTCDDecrease;
            float HRCDCast = HRCD + GcdLatency;
            float HRDuration = 15;
            float ELWDuration = 12;
            float HWCastBase = 2.5f;
            float GHWCastBase = 2.5f;
            calc.RealHWCast = HWCastBase * HasteScale;
            float HWCast = (float)Math.Max(HWCastBase * HasteScale + Latency, 1f + GcdLatency);
            float HWCastTWLatency = (Latency * 0.25f + GcdLatency * 0.75f) * TWChance + (Latency * 0.5f + GcdLatency * 0.5f) * (1 - TWChance);
            float HWCastTW = (float)Math.Max(HWCastBase * HasteScale * 0.7f + HWCastTWLatency, 1f + GcdLatency);
            float HWCast_RT = (float)Math.Max(HWCastBase / (1f + calc.SpellHaste + RTHaste), 1f) + GcdLatency;
            float HWCastTW_RT = (float)Math.Max(HWCastBase / (1f + calc.SpellHaste + RTHaste) * 0.7f + HWCastTWLatency, 1f + GcdLatency);
            calc.RealGHWCast = GHWCastBase * HasteScale;
            float GHWCast = (float)Math.Max(GHWCastBase * HasteScale + Latency, 1f + GcdLatency);
            float GHWCastTWLatency = (Latency * 0.25f + GcdLatency * 0.75f) * TWChance + (Latency * 0.5f + GcdLatency * 0.5f) * (1 - TWChance);
            float GHWCastTW = (float)Math.Max(GHWCastBase * HasteScale * 0.7f + GHWCastTWLatency, 1f + GcdLatency);
            float GHWCast_RT = (float)Math.Max(GHWCastBase / (1f + calc.SpellHaste + RTHaste), 1f) + GcdLatency;
            float GHWCastTW_RT = (float)Math.Max(GHWCastBase / (1f + calc.SpellHaste + RTHaste) * 0.7f + GHWCastTWLatency, 1f + GcdLatency);
            calc.RealHSrgCast = 1.5f * HasteScale;
            float HSrgCast = (float)Math.Max(1.5f * HasteScale, 1f) + GcdLatency;
            float HSrgCast_RT = (float)Math.Max(1.5f / (1f + calc.SpellHaste + RTHaste), 1f) + GcdLatency;
            float CHCastBase = 2.5f - stats.CHCTDecrease;
            calc.RealCHCast = CHCastBase * HasteScale;
            float CHCast = (float)Math.Max(CHCastBase * HasteScale + Latency, 1f + GcdLatency);
            float CHCast_RT = (float)Math.Max(CHCastBase / (1f + calc.SpellHaste + RTHaste), 1f) + GcdLatency;

            // This totally heals the boss backwards! Yeah! :D
            // Don't worry about this messing with procs or anything, it's just to show on the stats page. :)
            calc.LBCast = (float)Math.Max(2.5f * HasteScale, 1f);
            if (character.ShamanTalents.TelluricCurrents == 0)
                calc.LBRestore = 0f;
            else
                calc.LBRestore = (((770) + LBSpellPower) * 1.08f * (character.ShamanTalents.GlyphofLightningBolt ? 1.04f : 1f)) * (.2f * character.ShamanTalents.TelluricCurrents) - ((_BaseMana) * .06f); //Make an 85 Version (719+831) (PENGUIN)

            #endregion
            #region Base Spells ( TankCH / RTHeal / HSrgHeal / GHWHeal / HWHeal / CHHeal )
            #region Riptide area
            //  RT bonus healing = spell power
            float RTBonusHealing = stats.SpellPower;
            //  ... * generic healing scale * HoT scale
            RTBonusHealing *= 1.88f * (1f / 3.5f);
            //  RT healing scale = purification scale
            float RTHealingScale = PurificationScale + DeepHeal;
            //  ... + 20% 2pc T9 bonus
            RTHealingScale *= 1 + stats.RestoSham2T9 * .2f;
            //  ... set to zero if RT talent is not taken
            float RTHeal = ((calcOpts.CataOrLive ? 2118 : 2363) + RTBonusHealing) * RTHealingScale * character.ShamanTalents.Riptide;
            //  RT HoT bonus healing = spell power
            float RTHotBonusHealing = stats.SpellPower;
            //  ... * generic healing scale * HoT scale
            RTHotBonusHealing *= 1.88f * 0.5f;
            float RTHotHeal = ((calcOpts.CataOrLive ? 3340 : 3725) + RTHotBonusHealing) * RTHealingScale * character.ShamanTalents.Riptide;
            float RTHotTickHeal = RTHotHeal / 5;
            RTHotHeal = RTDuration / 3 * RTHotTickHeal;
            float RTHotHPS = RTHotTickHeal / 3;
            #endregion
            #region Healing Rains
            //  RT bonus healing = spell power
            float HRBonusHealing = stats.SpellPower;
            //  ... * generic healing scale * HoT scale
            HRBonusHealing *= 1.88f * (1f / 3.5f);
            //  RT healing scale = purification scale
            float HRHealingScale = PurificationScale;
            //  ... + 20% 2pc T9 bonus
            HRHealingScale *= 1f;
            //  ... set to zero if RT talent is not taken
            float HRHeal = (1670 + HRBonusHealing) * HRHealingScale;
            //  RT HoT bonus healing = spell power
            float HRHotBonusHealing = stats.SpellPower;
            //  ... * generic healing scale * HoT scale
            HRHotBonusHealing *= 1.88f * 0.5f;
            float HRHotHeal = (1670 + RTHotBonusHealing) * RTHealingScale * character.ShamanTalents.Riptide;
            float HRHotTickHeal = HRHotHeal / 5;
            HRHotHeal = HRDuration / 3 * HRHotTickHeal;
            float HRHotHPS = HRHotTickHeal / 3;
            #endregion
            #region Healing Surge Area
            //  HSrg bonus healing = spell power + totem spell power bonus
            float HSrgBonusHealing = stats.SpellPower;
            //  ... * generic healing scale + bonus from TW
            HSrgBonusHealing *= 1.88f * (1.5f / 3.5f);
            //  HSrg healing scale = purification scale
            float HSrgHealingScale = PurificationScale + DeepHeal;
            float HSrgHeal = ((calcOpts.CataOrLive ? 5381 : 6004) + HSrgBonusHealing) * HSrgHealingScale;
            #endregion
            #region Healing Wave Area
            //  HW bonus healing = spell power + totem spell power bonus
            float HWBonusHealing = stats.SpellPower;
            //  ... * generic healing scale + bonus from TW
            HWBonusHealing *= 1.88f * (3.0f / 3.5f) + character.ShamanTalents.TidalWaves * .04f;
            //  HW healing scale = purification scale
            float HWHealingScale = PurificationScale + DeepHeal;
            //  ... + 8%/16%/25% Healing Way bonus
            HWHealingScale *= 1f / 3f;
            //  ... + 5% 4pc T7 bonus
            HWHealingScale *= 1 + stats.CHHWHealIncrease;
            float HWHeal = ((calcOpts.CataOrLive ? 5381 : 6005) + HWBonusHealing) * HWHealingScale;
            //  HW self-healing scale = 20% if w/Glyph (no longer benefits from Purification since patch 3.2)
            float HWSelfHealingScale = SelfHeal && character.ShamanTalents.GlyphofHealingWave ? 0.2f : 0;
            //      * correction due to the fact it's just not smart to use GoHW for self-healing if you're _really_ hammered down
            HWSelfHealingScale *= 1f / HWHealingScale;
            #endregion
            #region Greater Healing Wave Area
            //  HW bonus healing = spell power
            float GHWBonusHealing = stats.SpellPower;
            //  ... * generic healing scale
            GHWBonusHealing *= 1.88f * (3.0f / 3.5f);
            //  HW healing scale = purification scale
            float GHWHealingScale = PurificationScale + DeepHeal;
            GHWHealingScale *= 1f / 3f;
            //  ... + 5% 4pc T7 bonus
            GHWHealingScale *= 1 + stats.CHHWHealIncrease;
            float GHWHeal = ((calcOpts.CataOrLive ? 7174 : 8005) + GHWBonusHealing) * GHWHealingScale;
            //  HW self-healing scale = 20% if w/Glyph (no longer benefits from Purification since patch 3.2)
            float GHWSelfHealingScale = SelfHeal && character.ShamanTalents.GlyphofHealingWave ? 0.2f : 0;
            //      * correction due to the fact it's just not smart to use GoHW for self-healing if you're _really_ hammered down
            GHWSelfHealingScale *= 1f / GHWHealingScale;
            #endregion
            #region Chain Heal Area
            //  CH bonus healing = spell power
            float CHBonusHealing = stats.SpellPower;
            //  ... * generic healing scale
            CHBonusHealing *= 1.88f * (2.5f / 3.5f);
            float CHHealingScale = PurificationScale + DeepHeal;
            CHHealingScale *= 1f;
            //  ... + 5% 4pc T7 + HoT 4pc T10
            CHHealingScale *= 1f + stats.CHHWHealIncrease + .25f * CriticalChance * stats.RestoSham4T10;
            float CHHeal = ((calcOpts.CataOrLive ? 2848 : 3178) + CHBonusHealing) * (CHHealingScale - (character.ShamanTalents.GlyphofChainHeal ? .1f : 0));
            float CHJumpHeal = 0;
            float scale = 1f;
            int jump;
            for (jump = 0; jump < CHJumps; jump++)
            {
                CHJumpHeal += scale;
                scale *= 0.3f + (character.ShamanTalents.GlyphofChainHeal ? .15f : 0);
            }
            CHJumpHeal += scale * (CHJumps - jump);
            CHJumpHeal *= CHHeal;
            #endregion
            #region Healing Stream Totem Area
            //  HST bonus healing = spell power
            float HSTBonusHealing = stats.SpellPower;
            //      * generic healing scale * HoT scale
            HSTBonusHealing *= 1.88f * 0.044f;
            //  HST healing scale = purification scale
            float HSTHealingScale = PurificationScale;
            //      + 25%/50% Healing Rains
            HSTHealingScale *= 1 + (.25f * character.ShamanTalents.SoothingRains);
            #endregion
            #endregion
            #region Base Costs ( Preserve / RTCost / HSrgCost / CHCost )
            float Preserve = stats.ManacostReduceWithin15OnHealingCast * .02f;
            float RTCost = ((float)Math.Round((_BaseMana - (calcOpts.CataOrLive ? 19034 : 0)) * .18f) - Preserve) * CostScale;
            float HRCost = ((float)Math.Round((_BaseMana - (calcOpts.CataOrLive ? 19034 : 0)) * .46f) - Preserve) * CostScale;
            float HSrgCost = ((float)Math.Round((_BaseMana - (calcOpts.CataOrLive ? 19034 : 0)) * .27f) - Preserve) * CostScale;
            float HWCost = ((float)Math.Round((_BaseMana - (calcOpts.CataOrLive ? 19034 : 0)) * .09f) - Preserve) * CostScale;
            float GHWCost = ((float)Math.Round((_BaseMana - (calcOpts.CataOrLive ? 19034 : 0)) * .30f) - Preserve) * CostScale;
            float HRNCost = ((float)Math.Round((_BaseMana - (calcOpts.CataOrLive ? 19034 : 0)) * .46f) - Preserve) * CostScale;
            float DecurseCost = ((float)Math.Round((_BaseMana - (calcOpts.CataOrLive ? 19034 : 0)) * .14f) - Preserve) * CostScale;
            float CHCost = ((float)Math.Round((_BaseMana - (calcOpts.CataOrLive ? 19034 : 0)) * .17f) - Preserve) * CostScale;
            #endregion
            #region RT + HSrg Rotation (RTHSrgMPS / RTHSrgHPS / RTHSrgTime)  (Adjusted based on Casting Activity)
            if (character.ShamanTalents.Riptide != 0)
            {
                float RTHSrgTime = RTCast;
                float RTHSrgRemainingTime = RTCDCast - RTHSrgTime;
                int RTHSrgHSrgCasts = 0;
                if (RTHSrgRemainingTime > GcdLatency)
                {
                    RTHSrgRemainingTime -= HSrgCast_RT;
                    RTHSrgTime += HSrgCast_RT;
                    ++RTHSrgHSrgCasts;
                    if (RTHSrgRemainingTime > GcdLatency)
                    {
                        int RTHSrgHSrgRemainingCasts = (int)Math.Ceiling((RTHSrgRemainingTime - GcdLatency) / HSrgCast);
                        RTHSrgTime += RTHSrgHSrgRemainingCasts * HSrgCast;
                        RTHSrgHSrgCasts += RTHSrgHSrgRemainingCasts;
                    }
                }
                float RTHSrgHSrgCrits = Math.Min(2, RTHSrgHSrgCasts) * Math.Min(CriticalChance + TWChance * 0.25f, 1) + (RTHSrgHSrgCasts - 2) * CriticalChance;
                float RTHSrgRTHeal = RTHeal * Critical;
                float RTHSrgHSrgCritHeal = HSrgHeal * Critical;
                float RTHSrgHSrgHeal = (HSrgHeal * (RTHSrgHSrgCasts - RTHSrgHSrgCrits)) + (RTHSrgHSrgCrits * RTHSrgHSrgCritHeal);
                float RTHSrgAA = (RTHeal * CriticalChance + HSrgHeal * RTHSrgHSrgCrits) * AAScale;
                float RTTargets = TankHeal ? 1 : RTDuration / RTHSrgTime;
                float RTHSrgELWTargets = ELWChance * (TankHeal ? 1 : RTHSrgHSrgCasts * ELWDuration / RTHSrgTime);
                calc.RTHSrgHPS = (((RTHSrgRTHeal * (1 - RTOverheal) + RTHSrgHSrgHeal * (1 - HSrgOverheal) + RTHSrgAA * (1 - AAOverheal)) / RTHSrgTime + RTTargets * RTHotHPS * (1 - RTOverheal) + RTHSrgELWTargets * ELWHPS * (1 - ELWOverheal)) * _CastingActivity) * (1 + stats.BonusHealingDoneMultiplier);
                calc.RTHSrgMPS = ((RTCost + (HSrgCost * RTHSrgHSrgCasts)) / RTHSrgTime) * _CastingActivity;
                if (calcOpts.SustStyle.Equals("RT+HSrg"))
                {
                    RTPerSec = 1f / RTHSrgTime;
                    RTTicksPerSec = RTTargets / 3;
                    HSrgPerSec = RTHSrgHSrgCasts / RTHSrgTime;
                    RTCPerSec = RTPerSec * CriticalChance;
                    HSrgCPerSec = RTHSrgHSrgCrits / RTHSrgTime;
                    AAsPerSec += (CriticalChance + RTHSrgHSrgCrits) / RTHSrgTime;
                    ELWTicksPerSec += RTHSrgELWTargets;
                }
            }
            #endregion
            #region RT + HW Rotation (RTHWMPS / RTHWHPS / RTHWTime) (Adjusted based on Casting Activity)
            if (character.ShamanTalents.Riptide != 0)
            {
                float RTHWTime = RTCast;
                float RTHWRemainingTime = RTCDCast - RTHWTime;
                int RTHWHWCasts = 0;
                if (RTHWRemainingTime > GcdLatency)
                {
                    float RTHWHWCastTW_RT = HWCastTW_RT * TWChance + HWCast_RT * (1 - TWChance);
                    RTHWRemainingTime -= RTHWHWCastTW_RT;
                    RTHWTime += RTHWHWCastTW_RT;
                    ++RTHWHWCasts;
                    if (RTHWRemainingTime > GcdLatency)
                    {
                        float RTHWHWCastTW = HWCastTW * TWChance + HWCast * (1 - TWChance);
                        RTHWRemainingTime -= RTHWHWCastTW;
                        RTHWTime += RTHWHWCastTW;
                        ++RTHWHWCasts;
                        if (RTHWRemainingTime > GcdLatency)
                        {
                            int RTHWHWRemainingCasts = (int)Math.Ceiling((RTHWRemainingTime - GcdLatency) / HWCast);
                            RTHWTime += RTHWHWRemainingCasts * HWCast;
                            RTHWHWCasts += RTHWHWRemainingCasts;
                        }
                    }
                }
                float RTHWRTHeal = RTHeal * Critical;
                float RTHWHWHCrits = RTHWHWCasts * CriticalChance;
                float RTHWHWHeal = HWHeal * RTHWHWCasts * Critical;
                float RTHWHWSelfHeal = RTHWHWHeal * HWSelfHealingScale * Critical;
                float RTHWAA = (RTHeal * CriticalChance + HWHeal * RTHWHWHCrits) * AAScale;
                //  Multi-target ELW handling not in yet due to low priority
                float RTTargets = TankHeal ? 1 : RTDuration / RTHWTime;
                float RTHWELWTargets = ELWChance * (TankHeal ? 1 : RTHWHWCasts * ELWDuration / RTHWTime);
                calc.RTHWHPS = (((RTHWRTHeal * (1 - RTOverheal) + RTHWHWHeal * (1 - HWOverheal) + RTHWHWSelfHeal * (1 - HWSelfOverheal) + RTHWAA * (1 - AAOverheal)) / RTHWTime + RTTargets * RTHotHPS * (1 - RTOverheal) + RTHWELWTargets * ELWHPS * (1 - ELWOverheal)) * _CastingActivity) * (1 + stats.BonusHealingDoneMultiplier);
                calc.RTHWMPS = ((RTCost + (HWCost * RTHWHWCasts)) / RTHWTime) * _CastingActivity;
                if (calcOpts.SustStyle.Equals("RT+HW"))
                {
                    RTPerSec = 1f / RTHWTime;
                    RTTicksPerSec = RTTargets / 3;
                    HWPerSec = RTHWHWCasts / RTHWTime;
                    RTCPerSec = RTPerSec * CriticalChance;
                    HWCPerSec = RTHWHWHCrits / RTHWTime;
                    AAsPerSec += (CriticalChance + RTHWHWHCrits) / RTHWTime;
                    ELWTicksPerSec += RTHWELWTargets;
                }
            }
            #endregion
            #region RT + GHW Rotation (RTGHWMPS / RTGHWHPS / RTGHWTime) (Adjusted based on Casting Activity)
            if (character.ShamanTalents.Riptide != 0)
            {
                float RTGHWTime = RTCast;
                float RTGHWRemainingTime = RTCDCast - RTGHWTime;
                int RTGHWGHWCasts = 0;
                if (RTGHWRemainingTime > GcdLatency)
                {
                    float RTGHWGHWCastTW_RT = GHWCastTW_RT * TWChance + GHWCast_RT * (1 - TWChance);
                    RTGHWRemainingTime -= RTGHWGHWCastTW_RT;
                    RTGHWTime += RTGHWGHWCastTW_RT;
                    ++RTGHWGHWCasts;
                    if (RTGHWRemainingTime > GcdLatency)
                    {
                        float RTGHWGHWCastTW = GHWCastTW * TWChance + GHWCast * (1 - TWChance);
                        RTGHWRemainingTime -= RTGHWGHWCastTW;
                        RTGHWTime += RTGHWGHWCastTW;
                        ++RTGHWGHWCasts;
                        if (RTGHWRemainingTime > GcdLatency)
                        {
                            int RTGHWGHWRemainingCasts = (int)Math.Ceiling((RTGHWRemainingTime - GcdLatency) / GHWCast);
                            RTGHWTime += RTGHWGHWRemainingCasts * GHWCast;
                            RTGHWGHWCasts += RTGHWGHWRemainingCasts;
                        }
                    }
                }
                float RTGHWRTHeal = RTHeal * Critical;
                float RTGHWGHWHCrits = RTGHWGHWCasts * CriticalChance;
                float RTGHWGHWHeal = GHWHeal * RTGHWGHWCasts * Critical;
                float RTGHWGHWSelfHeal = RTGHWGHWHeal * GHWSelfHealingScale * Critical;
                float RTGHWAA = (RTHeal * CriticalChance + GHWHeal * RTGHWGHWHCrits) * AAScale;
                //  Multi-target ELW handling not in yet due to low priority
                float RTTargets = TankHeal ? 1 : RTDuration / RTGHWTime;
                float RTGHWELWTargets = ELWChance * (TankHeal ? 1 : RTGHWGHWCasts * ELWDuration / RTGHWTime);
                calc.RTGHWHPS = (((RTGHWRTHeal * (1 - RTOverheal) + RTGHWGHWHeal * (1 - GHWOverheal) + RTGHWGHWSelfHeal * (1 - GHWSelfOverheal) + RTGHWAA * (1 - AAOverheal)) / RTGHWTime + RTTargets * RTHotHPS * (1 - RTOverheal) + RTGHWELWTargets * ELWHPS * (1 - ELWOverheal)) * _CastingActivity) * (1 + stats.BonusHealingDoneMultiplier);
                calc.RTGHWMPS = ((RTCost + (GHWCost * RTGHWGHWCasts)) / RTGHWTime) * _CastingActivity;
                if (calcOpts.SustStyle.Equals("RT+GHW"))
                {
                    RTPerSec = 1f / RTGHWTime;
                    RTTicksPerSec = RTTargets / 3;
                    GHWPerSec = RTGHWGHWCasts / RTGHWTime;
                    RTCPerSec = RTPerSec * CriticalChance;
                    GHWCPerSec = RTGHWGHWHCrits / RTGHWTime;
                    AAsPerSec += (CriticalChance + RTGHWGHWHCrits) / RTGHWTime;
                    ELWTicksPerSec += RTGHWELWTargets;
                }
            }
            #endregion
            #region RT + CH Rotation (RTCHMPS / RTCHHPS / RTCHTime / TankCH) (Adjusted based on Casting Activity)
            if (character.ShamanTalents.Riptide != 0)
            {
                float RTCHTime = RTCast;
                float RTCHRemainingTime = RTCDCast - RTCHTime;
                int RTCHCHCasts = 0;
                if (RTCHRemainingTime > GcdLatency)
                {
                    RTCHRemainingTime -= CHCast_RT;
                    RTCHTime += CHCast_RT;
                    ++RTCHCHCasts;
                    if (RTCHRemainingTime > GcdLatency)
                    {
                        int RTCHCHRemainingCasts = (int)Math.Ceiling((RTCHRemainingTime - GcdLatency) / CHCast);
                        RTCHTime += RTCHCHRemainingCasts * CHCast;
                        RTCHCHCasts += RTCHCHRemainingCasts;
                    }
                }
                float RTCHRTHeal = RTHeal * Critical;
                float RTCHCHHeal = (CHJumpHeal + CHHeal * CHRTConsumption * .25f) * RTCHCHCasts * ChCritical;
                float RTCHAA = RTHeal * CriticalChance * AAScale;
                float RTTargets = TankHeal ? Math.Max(RTDuration / RTCHTime - CHRTConsumption, 0) : (RTCast + (RTDuration - RTCast) * (1 - CHRTConsumption)) / RTCHTime;
                float RTCHELWTargets = ELWChance * (CHJumps * RTCHCHCasts + RTTargets) * ELWDuration / RTCHTime;
                calc.RTCHHPS = (((RTCHRTHeal * (1 - RTOverheal) + RTCHCHHeal * (1 - CHOverheal) + RTCHAA * (1 - AAOverheal)) / RTCHTime + RTTargets * RTHotHPS * (1 - RTOverheal) + RTCHELWTargets * ELWHPS * (1 - ELWOverheal)) * _CastingActivity) * (1 + stats.BonusHealingDoneMultiplier);
                calc.RTCHMPS = ((RTCost + (CHCost * RTCHCHCasts)) / RTCHTime) * _CastingActivity;
                if (calcOpts.SustStyle.Equals("RT+CH"))
                {
                    RTPerSec = 1f / RTCHTime;
                    RTCPerSec = RTPerSec * CriticalChance;
                    RTTicksPerSec = RTTargets / 3;
                    CHPerSec = RTCHCHCasts / RTCHTime;
                    CHHitsPerSec = CHPerSec * CHJumps;
                    CHCPerSec = RTCHCHCasts * (CriticalChance + (stats.RestoSham4T9 * .05f)) / RTCHTime;
                    CHCHitsPerSec = CHCPerSec * CHJumps;
                    AAsPerSec += CriticalChance / RTCHTime;
                    ELWTicksPerSec += RTCHELWTargets;
                }
            }
            #endregion
            #region CH Spam (CHHPS / CHMPS)
            float CHELWTargets = ELWChance * CHJumps * ELWDuration / CHCast;
            calc.CHSpamHPS = ((CHJumpHeal * ChCritical * (1 - CHOverheal) / CHCast + CHELWTargets * ELWHPS * (1 - ELWOverheal)) * _CastingActivity) * (1 + stats.BonusHealingDoneMultiplier);
            calc.CHSpamMPS = (CHCost / CHCast) * _CastingActivity;
            if (calcOpts.SustStyle.Equals("CH Spam"))
            {
                CHPerSec = 1f / CHCast;
                CHHitsPerSec = CHPerSec * CHJumps;
                CHCPerSec = (CriticalChance + (stats.RestoSham4T9 * .05f)) / CHCast;
                CHCHitsPerSec = CHCPerSec * CHJumps;
                ELWTicksPerSec += CHELWTargets;
            }
            #endregion
            #region HSrg Spam (HSrgHPS / HSrgMPS)
            float HSrgHSrgHeal = HSrgHeal * Critical;
            float HSrgAA = HSrgHeal * CriticalChance * AAScale;
            float HSrgELWTargets = ELWChance * ELWDuration / HSrgCast;
            calc.HSrgSpamHPS = (((HSrgHSrgHeal * (1 - HSrgOverheal) + HSrgAA * (1 - AAOverheal)) / HSrgCast + HSrgELWTargets * ELWHPS * (1 - ELWOverheal)) * _CastingActivity) * (1 + stats.BonusHealingDoneMultiplier);
            calc.HSrgSpamMPS = (HSrgCost / HSrgCast) * _CastingActivity;
            if (calcOpts.SustStyle.Equals("HSrg Spam"))
            {
                HSrgPerSec = 1f / HSrgCast;
                HSrgCPerSec = CriticalChance / HSrgCast;
                ELWTicksPerSec += HSrgELWTargets;
            }
            #endregion
            #region HW Spam (HWHPS / HWMPS)
            float HWHWHeal = HWHeal * Critical;
            float HWHWSelfHeal = HWHWHeal * HWSelfHealingScale * Critical;
            float HWAA = HWHeal * CriticalChance * AAScale;
            float HWELWTargets = ELWChance * ELWDuration / HWCast;
            calc.HWSpamHPS = (((HWHWHeal * (1 - HWOverheal) + HWHWSelfHeal * (1 - HWSelfOverheal) + HWAA * (1 - AAOverheal)) / HWCast + HWELWTargets * ELWHPS * (1 - ELWOverheal)) * _CastingActivity) * (1 + stats.BonusHealingDoneMultiplier);
            calc.HWSpamMPS = (HWCost / HWCast) * _CastingActivity;
            if (calcOpts.SustStyle.Equals("HW Spam"))
            {
                HWPerSec = 1f / HWCast;
                HWCPerSec = CriticalChance / HWCast;
                ELWTicksPerSec += HWELWTargets;
            }
            #endregion
            #region GHW Spam (GHWHPS / GHWMPS)
            float GHWGHWHeal = GHWHeal * Critical;
            float GHWGHWSelfHeal = GHWGHWHeal * GHWSelfHealingScale * Critical;
            float GHWAA = GHWHeal * CriticalChance * AAScale;
            float GHWELWTargets = ELWChance * ELWDuration / GHWCast;
            calc.GHWSpamHPS = (((GHWGHWHeal * (1 - GHWOverheal) + GHWGHWSelfHeal * (1 - GHWSelfOverheal) + GHWAA * (1 - AAOverheal)) / GHWCast + GHWELWTargets * ELWHPS * (1 - ELWOverheal)) * _CastingActivity) * (1 + stats.BonusHealingDoneMultiplier);
            calc.GHWSpamMPS = (GHWCost / GHWCast) * _CastingActivity;
            if (calcOpts.SustStyle.Equals("GHW Spam"))
            {
                GHWPerSec = 1f / GHWCast;
                GHWCPerSec = CriticalChance / GHWCast;
                ELWTicksPerSec += GHWELWTargets;
            }
            #endregion
            #region Variables if Riptide not taken in talents
            if (character.ShamanTalents.Riptide == 0)
            {
                calc.RTHSrgHPS = calc.HSrgSpamHPS;
                calc.RTHSrgMPS = calc.HSrgSpamMPS;
                calc.RTHWHPS = calc.HWSpamHPS;
                calc.RTHWMPS = calc.HWSpamMPS;
                calc.RTGHWHPS = calc.GHWSpamHPS;
                calc.RTGHWMPS = calc.GHWSpamMPS;
                calc.RTCHHPS = calc.CHSpamHPS;
                calc.RTCHMPS = calc.CHSpamMPS;
            }
            #endregion
            #region Create Final calcs via spell cast (HealPerSec/HealHitPerSec/CritPerSec)
            _HealPerSec = (RTPerSec + HSrgPerSec + HWPerSec + CHPerSec + GHWPerSec) * _CastingActivity;
            _HealHitPerSec = (RTPerSec + RTTicksPerSec + HSrgPerSec + HWPerSec + CHHitsPerSec + AAsPerSec + ELWTicksPerSec + GHWPerSec) * _CastingActivity;
            _CritPerSec = (RTCPerSec + HSrgCPerSec + HWCPerSec + CHCPerSec + GHWCPerSec) * _CastingActivity;
            #endregion
            #region Proc Handling for Mana Restore only
            Stats statsProcs2 = new Stats();
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                switch (effect.Trigger)
                {
                    case (Trigger.HealingSpellCast):
                        if (_HealPerSec != 0)
                            effect.AccumulateAverageStats(statsProcs2, (1f / _HealPerSec), 1f, 0f, _FightSeconds);
                        break;
                    case (Trigger.HealingSpellHit):
                        if (_HealHitPerSec != 0)
                            effect.AccumulateAverageStats(statsProcs2, (1f / _HealHitPerSec), 1f, 0f, _FightSeconds);
                        break;
                    case (Trigger.HealingSpellCrit):
                        if (_CritPerSec != 0)
                            effect.AccumulateAverageStats(statsProcs2, (1f / _CritPerSec), 1f, 0f, _FightSeconds);
                        break;
                    case (Trigger.SpellCast):
                        if (_HealPerSec != 0)
                            effect.AccumulateAverageStats(statsProcs2, (1f / _HealPerSec), 1f, 0f, _FightSeconds);
                        break;
                    case (Trigger.SpellHit):
                        if (_HealHitPerSec != 0)
                            effect.AccumulateAverageStats(statsProcs2, (1f / _HealHitPerSec), 1f, 0f, _FightSeconds);
                        break;
                    case (Trigger.SpellCrit):
                        if (_CritPerSec != 0)
                            effect.AccumulateAverageStats(statsProcs2, (1f / _CritPerSec), 1f, 0f, _FightSeconds);
                        break;
                    case Trigger.Use:
                        effect.AccumulateAverageStats(statsProcs2, 0f, 1f, 0f, _FightSeconds);
                        break;
                }
            }
            #endregion
            #region Calculate Sequence HPS/MPS
            float HSTHPS = (25 + HSTBonusHealing) * HSTHealingScale / 2f * (1 - HSTOverheal);
            calc.HSTHeals = HSTHPS * HSTTargets;

            // Burst
            calc.BurstSequence = calcOpts.BurstStyle;
            float BurstHPS = 0;
            float BurstMUPS = 0;
            switch (calcOpts.BurstStyle)
            {
                case "CH Spam":
                    BurstHPS = calc.CHSpamHPS;
                    BurstMUPS = calc.CHSpamMPS;
                    break;
                case "HW Spam":
                    BurstHPS = calc.HWSpamHPS;
                    BurstMUPS = calc.HWSpamMPS;
                    break;
                case "GHW Spam":
                    BurstHPS = calc.GHWSpamHPS;
                    BurstMUPS = calc.GHWSpamMPS;
                    break;
                case "HSrg Spam":
                    BurstHPS = calc.HSrgSpamHPS;
                    BurstMUPS = calc.HSrgSpamMPS;
                    break;
                case "RT+HW":
                    BurstHPS = calc.RTHWHPS;
                    BurstMUPS = calc.RTHWMPS;
                    break;
                case "RT+GHW":
                    BurstHPS = calc.RTGHWHPS;
                    BurstMUPS = calc.RTGHWMPS;
                    break;
                case "RT+HSrg":
                    BurstHPS = calc.RTHSrgHPS;
                    BurstMUPS = calc.RTHSrgMPS;
                    break;
                case "RT+CH":
                    BurstHPS = calc.RTCHHPS;
                    BurstMUPS = calc.RTCHMPS;
                    break;
            }

            // Sustained
            calc.SustainedSequence = calcOpts.SustStyle;
            float SustHPS = 0;
            float SustMUPS = 0;
            switch (calcOpts.SustStyle)
            {
                case "CH Spam":
                    SustHPS = calc.CHSpamHPS;
                    SustMUPS = calc.CHSpamMPS;
                    break;
                case "HW Spam":
                    SustHPS = calc.HWSpamHPS;
                    SustMUPS = calc.HWSpamMPS;
                    break;
                case "GHW Spam":
                    SustHPS = calc.GHWSpamHPS;
                    SustMUPS = calc.GHWSpamMPS;
                    break;
                case "HSrg Spam":
                    SustHPS = calc.HSrgSpamHPS;
                    SustMUPS = calc.HSrgSpamMPS;
                    break;
                case "RT+HW":
                    SustHPS = calc.RTHWHPS;
                    SustMUPS = calc.RTHWMPS;
                    break;
                case "RT+GHW":
                    SustHPS = calc.RTGHWHPS;
                    SustMUPS = calc.RTGHWMPS;
                    break;
                case "RT+HSrg":
                    SustHPS = calc.RTHSrgHPS;
                    SustMUPS = calc.RTHSrgMPS;
                    break;
                case "RT+CH":
                    SustHPS = calc.RTCHHPS;
                    SustMUPS = calc.RTCHMPS;
                    break;
            }

            calc.BurstHPS += calc.HSTHeals;
            SustHPS += calc.HSTHeals;
            calc.MUPS = ((SustMUPS * calcOpts.ActivityPerc) + (BurstMUPS * (100 - calcOpts.ActivityPerc))) * .01f;
            calc.MUPS += (DecurseCost * calcOpts.Decurse) / _FightSeconds;
            #endregion
            #region Final Stats
            calc.LBNumber = calcOpts.LBUse;
            float ESUsage = UseES ? (float)Math.Round((_FightSeconds / ESTimer), 0) : 0;
            float ESDowntime = (_FightSeconds - ((RTCast * ESUsage) + (calcOpts.LBUse * calc.LBCast)) - 3) / _FightSeconds;  // Rip tide cast time is used to simulate ES cast time, as they are exactly the same.  The 3 Simulates the time of two full totem drops.
            calc.MAPS = ((stats.Mana) / (_FightSeconds))
                + (stats.ManaRestore / _FightSeconds)
                + (stats.ManaRestoreFromMaxManaPerSecond * stats.Mana)
                + (stats.Mp5 / 5f)
                + (calcOpts.Innervates * 7866f / _FightSeconds)
                + statsProcs2.ManaRestore
                + ((RTCPerSec * Orb) * _CastingActivity * ESDowntime)
                + ((HSrgCPerSec * Orb * .6f) * _CastingActivity * ESDowntime)
                + ((HWCPerSec * Orb) * _CastingActivity * ESDowntime)
                + ((GHWCPerSec * Orb) * _CastingActivity * ESDowntime)
                + ((CHCHitsPerSec * Orb * .3f) * _CastingActivity * ESDowntime)
                + (calc.LBRestore * calc.LBNumber / _FightSeconds)
                - ESMPS;
            if (calcOpts.WSPops > 0)
                calc.MAPS += ((calcOpts.WSPops * Orb) / 60);
            calc.ManaUsed = calc.MAPS * _FightSeconds;
            float MAPSConvert = (float)Math.Min((calc.MAPS / ((calc.MUPS) * ESDowntime)), 1);
            float HealedHPS = stats.Healed * (1 + stats.BonusHealingDoneMultiplier);
            calc.BurstHPS = (BurstHPS * ESDowntime) + calc.ESHPS * (1 - ESOverheal) + HealedHPS;
            calc.SustainedHPS = (SustHPS * MAPSConvert) + calc.ESHPS * (1 - ESOverheal) + HealedHPS;
            calc.Survival = (calc.BasicStats.Health + calc.BasicStats.Hp5) * (calcOpts.SurvivalPerc * .01f);
            calc.OverallPoints = calc.BurstHPS + calc.SustainedHPS + calc.Survival;
            calc.SubPoints[0] = calc.BurstHPS;
            calc.SubPoints[1] = calc.SustainedHPS;
            calc.SubPoints[2] = calc.Survival;

            return calc;
            #endregion
        }

        /// <summary>
        /// Gets the armor specialization status.
        /// </summary>
        private bool GetArmorSpecializationStatus(Character character)
        {
            List<CharacterSlot> relevantSlots = new List<CharacterSlot>(8)
            {
                CharacterSlot.Head,
                CharacterSlot.Shoulders,
                CharacterSlot.Chest,
                CharacterSlot.Wrist,
                CharacterSlot.Hands,
                CharacterSlot.Waist,
                CharacterSlot.Legs,
                CharacterSlot.Feet
            };

            foreach (CharacterSlot s in relevantSlots)
            {
                if (character[s] == null)
                    return false;
                if (character[s].Type != ItemType.Mail)
                    return false;
                // if we get to here, the character has a mail item in this slot
            }

            return true;
        }

        #endregion

        #region Character Stats and other Final Stats

        private Stats GetCharacterStats(Character character, Item additionalItem, Stats statModifier)
        {
            if (character == null)
                return null;

            // Get the calculation options and base stats
            CalculationOptionsRestoSham calcOpts = character.CalculationOptions as CalculationOptionsRestoSham;

            // Base stats
            Stats totalStats = new Stats();
            Stats raceStats = Rawr.BaseStats.GetBaseStats(character);
            Stats buffStats = GetBuffsStats(character.ActiveBuffs);
            Stats itemStats = GetItemStats(character, additionalItem);
            totalStats.Accumulate(raceStats);
            totalStats.Accumulate(itemStats);
            totalStats.Accumulate(buffStats);
            if (statModifier != null)
                totalStats.Accumulate(statModifier);

            // Armor specialization
            if (GetArmorSpecializationStatus(character))
                totalStats.BonusIntellectMultiplier += 0.05f;

            // Procs
            totalStats.Accumulate(GetProcStats(totalStats.SpecialEffects()));

            // Bonuses
            totalStats.Stamina *= (1 + totalStats.BonusStaminaMultiplier);
            totalStats.Intellect *= (1 + totalStats.BonusIntellectMultiplier);
            totalStats.Spirit *= (1 + totalStats.BonusSpiritMultiplier);
            totalStats.Mana = totalStats.Mana + 20 + ((totalStats.Intellect - 20) * 15);
            totalStats.Health = (totalStats.Health + 20 + ((totalStats.Stamina - 20) * 10f)) * (1f + totalStats.BonusHealthMultiplier);

            // Other
            totalStats.SpellPower += totalStats.Intellect - 10f;
            totalStats.SpellHaste = (1 + StatConversion.GetSpellHasteFromRating(totalStats.HasteRating)) * (1 + totalStats.SpellHaste) - 1;
            totalStats.Mp5 += (StatConversion.GetSpiritRegenSec(totalStats.Spirit, totalStats.Intellect)) * 2.5f;
            totalStats.SpellCrit = .022f + StatConversion.GetSpellCritFromIntellect(totalStats.Intellect) + StatConversion.GetSpellCritFromRating(totalStats.CritRating) + totalStats.SpellCrit + (.01f * (character.ShamanTalents.Acuity));

            return totalStats;
        } 

        #region Basic Stats Methods

        /// <summary>
        /// Gets the stats of the buffs currently active on a character
        /// </summary>
        /// <param name="character">The character to evaluate</param>
        /// <returns>
        /// Stats object containing the stats of all the active buffs on the character
        /// </returns>
        private Stats GetBuffsStats(Character character)
        {
            return GetBuffsStats(character.ActiveBuffs);
        }

        /// <summary>
        /// Gets the stats of procs or special effects
        /// </summary>
        /// <param name="specialEffects">List of special effects to aggregate</param>
        /// <returns>
        /// Stats object containing the accumulated stats from the provided special effects
        /// </returns>
        private Stats GetProcStats(Stats.SpecialEffectEnumerator specialEffects)
        {
            Stats statsProcs = new Stats();
            foreach (SpecialEffect effect in specialEffects.Where(i => i != null))
            {
                statsProcs.Accumulate(GetProcStats_Inner(effect));
            }
            return statsProcs;
        }

        private Stats GetProcStats_Inner(SpecialEffect effect)
        {
            Stats procStats = new Stats();
            switch (effect.Trigger)
            {
                case (Trigger.HealingSpellCast):
                    if (_HealPerSec != 0)
                        procStats = effect.GetAverageStats((1f / _HealPerSec), 1f, 0f, _FightSeconds);
                    break;
                case (Trigger.HealingSpellHit):
                    if (_HealHitPerSec != 0)
                        procStats = effect.GetAverageStats((1f / _HealHitPerSec), 1f, 0f, _FightSeconds);
                    break;
                case (Trigger.HealingSpellCrit):
                    if (_CritPerSec != 0)
                        procStats = effect.GetAverageStats((1f / _CritPerSec), 1f, 0f, _FightSeconds);
                    break;
                case (Trigger.SpellCast):
                    if (_HealPerSec != 0)
                        procStats = effect.GetAverageStats((1f / _HealPerSec), 1f, 0f, _FightSeconds);
                    break;
                case (Trigger.SpellHit):
                    if (_HealHitPerSec != 0)
                        procStats = effect.GetAverageStats((1f / _HealHitPerSec), 1f, 0f, _FightSeconds);
                    break;
                case (Trigger.SpellCrit):
                    if (_CritPerSec != 0)
                        procStats = effect.GetAverageStats((1f / _CritPerSec), 1f, 0f, _FightSeconds);
                    break;
                case Trigger.Use:
                    if (effect.Stats._rawSpecialEffectData != null)
                    {
                        // Handles Recursive Effects
                        Stats SubStats = GetProcStats_Inner(effect.Stats._rawSpecialEffectData[0]);
                        float upTime = effect.GetAverageUptime(0f, 1f, 0f, _FightSeconds);
                        procStats.Accumulate(SubStats, upTime);
                    }
                    else
                    {
                        procStats = effect.GetAverageStats(0f, 1f, 0f, _FightSeconds);
                    }
                    break;
            }
            return procStats;
        }

        #endregion

        #endregion
    }
}
