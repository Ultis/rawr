using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;
using System.IO;

namespace Rawr.WarlockTmp {

    /// <summary>
    /// Core class representing the Rawr.Warlock model
    /// </summary>
    [Rawr.Calculations.RawrModelInfo(
        "WarlockTmp",
        "Spell_Nature_FaerieFire",
        CharacterClass.Warlock)]
    public class CalculationsWarlock : CalculationsBase {

        #region Related Classes

        /// <summary>
        /// The class that Rawr.Warlock is designed for (Warlock)
        /// </summary>
        public override CharacterClass TargetClass {
            get { return CharacterClass.Warlock; }
        }

        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        /// <summary>
        /// Panel to be placed on the Options tab of the main form
        /// </summary>
        public override CalculationOptionsPanelBase CalculationOptionsPanel {
            get {
                if (_calculationOptionsPanel == null) {
                    _calculationOptionsPanel
                        = new CalculationOptionsPanelWarlock();
                }
                return _calculationOptionsPanel;
            }
        }

        /// <summary>
        /// Creates a new ComparisonCalculationWarlock instance
        /// </summary>
        /// <returns>
        /// A new ComparisonCalculationWarlock instance
        /// </returns>
        public override ComparisonCalculationBase
            CreateNewComparisonCalculation() {

            return new ComparisonCalculationWarlock();
        }

        /// <summary>
        /// Creates a new CharacterCalculationsWarlock instance
        /// </summary>
        /// <returns>
        /// A new CharacterCalculationsWarlock instance
        /// </returns>
        public override CharacterCalculationsBase
            CreateNewCharacterCalculations() {

            return new CharacterCalculationsWarlock();
        }

        #endregion


        #region Basic Model Properties and Methods

        public const float AVG_UNHASTED_CAST_TIME = 2f; // total SWAG

        public override void SetDefaults(Character character) { }

        private string[] _characterDisplayCalculationLabels = null;
        /// <summary>
        /// Labels of the stats to display on the Stats tab of the main form
        /// </summary>
        public override string[] CharacterDisplayCalculationLabels {
            get {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[] {
                        "Simulation:Personal DPS",
                        "Simulation:Pet DPS",
                        "Simulation:Total DPS",
                        "Basic Stats:Health",
                        "Basic Stats:Mana",
                        "Basic Stats:Spirit",
                        "Basic Stats:Bonus Damage",
                        "Basic Stats:Hit Rating",
                        "Basic Stats:Crit Chance",
                        "Basic Stats:Haste Rating",
                        "Affliction:Corruption",
                        "Affliction:Curse Of Agony",
                        "Affliction:Curse Of The Elements",
                        "Affliction:Haunt",
                        "Affliction:Life Tap",
                        "Affliction:Unstable Affliction",
                        "Demonology:Metamorphosis",
                        "Destruction:Shadow Bolt",
                        "Destruction:Shadow Bolt (Instant)" };
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _optimizableCalculationLabels = null;
        /// <summary>
        /// Labels of the additional constraints available to the Optimizer 
        /// </summary>
        public override string[] OptimizableCalculationLabels {
            get {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
					    "Miss Chance"};
                return _optimizableCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        /// <summary>
        /// Names of the custom charts that Rawr.Warlock provides
        /// </summary>
        public override string[] CustomChartNames {
            get {
                if (_customChartNames == null)
                    _customChartNames = new string[] { };
                return _customChartNames;
            }
        }

        private Dictionary<string, System.Drawing.Color>
            _subPointNameColors = null;
        /// <summary>
        /// Names and colors for the SubPoints that Rawr.Warlock uses
        /// </summary>
        public override Dictionary<string, System.Drawing.Color>
            SubPointNameColors {
            get {
                if (_subPointNameColors == null) {
                    _subPointNameColors = new Dictionary<string, Color>();
                    _subPointNameColors.Add(
                        "DPS", Color.FromArgb(255, 255, 0, 0));
                    _subPointNameColors.Add(
                        "Pet DPS", Color.FromArgb(255, 0, 0, 255));
                }
                return _subPointNameColors;
            }
        }

        /// <summary>
        /// Deserializes the CalculationOptionsBear object contained 
        /// in xml
        /// </summary>
        /// <param name="xml">
        /// The CalculationOptionsBear object, serialized as xml
        /// </param>
        /// <returns>
        /// The deserialized CalculationOptionsBear object
        /// </returns>
        public override ICalculationOptionBase
            DeserializeDataObject(string xml) {

            XmlSerializer serializer = new XmlSerializer(
                typeof(CalculationOptionsWarlock));
            StringReader reader = new StringReader(xml);
            CalculationOptionsWarlock calcOpts
                = serializer.Deserialize(reader)
                    as CalculationOptionsWarlock;
            return calcOpts;
        }

        #endregion


        #region Primary Calculation Methods

        /// <summary>
        /// Gets the results of the Character provided
        /// </summary>
        /// <param name="character">
        /// The Character to calculate resutls for
        /// </param>
        /// <param name="additionalItem">
        /// An additional item to grant the Character the stats of (as if it
        /// were worn)
        /// </param>
        /// <returns>
        /// The CharacterCalculationsWarlock containing the results of the
        /// calculations
        /// </returns>
        public override CharacterCalculationsBase
            GetCharacterCalculations(
                Character character,
                Item additionalItem,
                bool referenceCalculation,
                bool significantChange,
                bool needsDisplayCalculations) {

            Stats stats = GetCharacterStats(character, additionalItem);
            CharacterCalculationsWarlock calculatedStats
                = new CharacterCalculationsWarlock(character, stats);
            return calculatedStats;
        }

        /// <summary>Gets the total Stats of the Character</summary>
        /// <param name="character">
        /// The Character to get the total Stats of
        /// </param>
        /// <param name="additionalItem">
        /// An additional item to grant the Character the stats of (as if it
        /// were worn)
        /// </param>
        /// <returns>The total stats for the Character</returns>
        public override Stats GetCharacterStats(
            Character character, Item additionalItem) {

            WarlockTalents talents = character.WarlockTalents;
            CalculationOptionsWarlock options
                = character.CalculationOptions as CalculationOptionsWarlock;

            Stats statsBase = BaseStats.GetBaseStats(character);
            Stats statsItem = GetItemStats(character, additionalItem);

            ////Potion of Speed is a consumable that can only be used once per fight even tho its tooltip / wowhead info indicates it has a 1 min cooldown.
            ////This means that its actual cooldown is equal to the length of the fight.
            ////At the moment, it has been hardcoded [in Buff.cs (rawr.base)] for a 20min fight, so we have to correct it here to get the appropriate +haste bonus effect.
            //if (character.ActiveBuffsContains("Potion of Speed"))
            //{
            //    //get the fight length
            //    CalculationOptionsWarlock options = (CalculationOptionsWarlock)character.CalculationOptions;
            //    float fightLength = (options.Duration * 60); //i.e. in seconds

            //    //remove the existing speedpotion buff (which has the incorrect cooldown)
            //    Buff speedpotion = Buff.GetBuffByName("Potion of Speed");
            //    character.ActiveBuffs.Remove(speedpotion);

            //    //redefine its stats (this time using the correct cooldown)
            //    speedpotion.Stats = new Stats();
            //    speedpotion.Stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats { HasteRating = 500f }, 15f, fightLength));
            //    character.ActiveBuffs.Add(speedpotion);

            //    //now repeat the process with the if the pot trick buff had been selected
            //    if (character.ActiveBuffsContains("Potion of Speed (Double Pot Trick)"))
            //    {
            //        speedpotion = Buff.GetBuffByName("Potion of Speed (Double Pot Trick)");
            //        character.ActiveBuffs.Remove(speedpotion);
            //        speedpotion.Stats = new Stats();
            //        speedpotion.Stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats { HasteRating = 500f }, (15f - 1f), fightLength));
            //        character.ActiveBuffs.Add(speedpotion);
            //    }
            //}

            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            float[] talentValues = { 0f, .04f, .07f, .1f };
            Stats statsTalents = new Stats {

                //Demonic Embrace: increases your stamina by 4/7/10%
                BonusStaminaMultiplier = talentValues[talents.DemonicEmbrace],

                //Fel Vitality: increases your maximum Health & Mana by 1/2/3%
                BonusHealthMultiplier = talents.FelVitality * 0.01f,
                BonusManaMultiplier = talents.FelVitality * 0.01f,

                //Suppression: increases your chance to hit with spells by
                //1/2/3%
                SpellHit = (talents.Suppression * 0.01f),

                //Demonic Tactics: increases your spell crit chance by
                //2/4/6/8/10%
                //Backlash: increases your spell crit chance by 1/2/3%
                BonusCritChance
                    = talents.DemonicTactics * 0.02f + talents.Backlash * 0.01f
            };
            if (talents.Eradication > 0) {
                talentValues = new float[] { 0f, .06f, .12f, .20f };
                statsTalents.AddSpecialEffect(
                    new SpecialEffect(
                        Trigger.CorruptionTick,
                        new Stats() {
                            SpellHaste = talentValues[talents.Eradication] },
                        6f,
                        0f,
                        .06f));
            }

            Stats stats = new Stats();
            stats.Accumulate(statsBase);
            stats.Accumulate(statsItem);
            stats.Accumulate(statsBuffs);
            stats.Accumulate(statsTalents);

            //make sure that the bonus multipliers have been applied to each
            //stat
            stats.Stamina = stats.Stamina * (1f + stats.BonusStaminaMultiplier);
            stats.Intellect
                = stats.Intellect * (1f + stats.BonusIntellectMultiplier);
            stats.Spirit = stats.Spirit * (1f + stats.BonusSpiritMultiplier);
            stats.Strength
                = stats.Strength * (1f + stats.BonusStrengthMultiplier);
            stats.Agility = stats.Agility * (1f + stats.BonusAgilityMultiplier);
            stats.Armor = stats.Armor * (1f + stats.BonusArmorMultiplier);

            //Agility increases Armor by 2 per point
            //(http://www.wowwiki.com/Agility#Agility)
            stats.BonusArmor += stats.Agility * 2;
            stats.Armor += stats.BonusArmor;

            //Health is calculated from stamina rating first, then its bonus
            //multiplier (in this case, "Fel Vitality" talent) gets applied
            stats.Health += StatConversion.GetHealthFromStamina(stats.Stamina);
            stats.Health *= 1 + stats.BonusHealthMultiplier;

            //Mana is calculated from intellect rating first, then its bonus
            //multiplier (in this case, "Expansive Mind" - Gnome racial) is
            //applied
            stats.Mana += StatConversion.GetManaFromIntellect(stats.Intellect);
            stats.Mana *= 1 + stats.BonusManaMultiplier;

            //Crit rating - the MasterConjuror talent improves the firestone
            float conjuror = 1f + talents.MasterConjuror * 1.5f;
            stats.CritRating
                += stats.WarlockFirestoneSpellCritRating * conjuror;
            stats.SpellCrit
                += StatConversion.GetSpellCritFromIntellect(stats.Intellect)
                    + StatConversion.GetSpellCritFromRating(stats.CritRating)
                    + stats.BonusCritChance
                    + stats.SpellCritOnTarget;

            //Haste rating - the MasterConjuror talent improves the spellstone
            stats.HasteRating += stats.WarlockSpellstoneHasteRating * conjuror;
            stats.SpellHaste
                = (1 + stats.SpellHaste)
                        * (1
                            + StatConversion.GetSpellHasteFromRating(
                                stats.HasteRating))
                    - 1;

            //Hit rating 
            stats.SpellHit
                += StatConversion.GetSpellHitFromRating(stats.HitRating);

            // Bonuses from armor choice
            if (stats.WarlockFelArmor > 0) {
                float aegis = 1 + talents.DemonicAegis * 0.10f;
                stats.SpellPower += stats.WarlockFelArmor * aegis;
                stats.SpellDamageFromSpiritPercentage += 0.30f * aegis;
                stats.Hp5 += stats.Health * .02f * aegis;
            } else if (stats.WarlockDemonArmor > 0) {
                float aegis = 1 + talents.DemonicAegis * 0.10f;
                stats.Armor += stats.WarlockDemonArmor * aegis;
                stats.HealingReceivedMultiplier += 0.2f * aegis;
            }

            // Spell Power
            stats.SpellPower
                += stats.SpellDamageFromSpiritPercentage * stats.Spirit;
            stats.SpellPower *= 1f + stats.BonusSpellPowerMultiplier;

            if (talents.DemonicKnowledge > 0) {
                //PetCalculations pet = new PetCalculations(statsTotal, character);
                //statsTotal.SpellPower += (pet.petStats.Intellect + pet.petStats.Stamina) * talents.DemonicKnowledge * 0.04f;
            }


            #region add special effects to the stats

            // the trigger rates are all guestimates, since the real values
            // depend on special effects (if that effect uses haste) and the
            // special effect trigger rates depend on the real values.
            // TODO: move all but the haste effects to the dps calculations,
            // where we'll have real values.

            Dictionary<Trigger, float> periods
                = new Dictionary<Trigger, float>();
            Dictionary<Trigger, float> chances
                = new Dictionary<Trigger, float>();
            periods[Trigger.Use] = 0f;
            periods[Trigger.SpellHit]
                = periods[Trigger.SpellCrit]
                = periods[Trigger.SpellCast]
                = periods[Trigger.SpellMiss]
                = periods[Trigger.DamageSpellHit]
                = periods[Trigger.DamageSpellCrit]
                = periods[Trigger.DamageSpellCast]
                = AVG_UNHASTED_CAST_TIME / stats.SpellHaste;
            periods[Trigger.DoTTick] = 1.5f;
            periods[Trigger.DamageDone]
                = periods[Trigger.DamageOrHealingDone]
                = 1f
                    / (1 / periods[Trigger.DoTTick]
                        + 1 / periods[Trigger.SpellHit]);

            chances[Trigger.Use] = 1f;
            chances[Trigger.SpellHit]
                = chances[Trigger.DamageSpellHit]
                = Math.Min(1f, stats.SpellHit);
            chances[Trigger.SpellCrit]
                = chances[Trigger.DamageSpellCrit]
                = chances[Trigger.DamageDone]
                = chances[Trigger.DamageOrHealingDone]
                = chances[Trigger.SpellHit]
                    * (StatConversion.GetSpellCritFromIntellect(stats.Intellect)
                        + StatConversion.GetSpellCritFromRating(
                            stats.CritRating
                                + stats.WarlockFirestoneSpellCritRating
                                    * conjuror)
                        + stats.BonusCritChance
                        + stats.SpellCritOnTarget);
            chances[Trigger.SpellCast] = chances[Trigger.DamageSpellCast] = 1f;
            chances[Trigger.SpellMiss] = 1 - chances[Trigger.SpellHit];
            chances[Trigger.DoTTick] = 1f;

            if (options.SpellPriority.Contains("Corruption")) {
                periods[Trigger.CorruptionTick] = 3.1f;
                if (talents.GlyphQuickDecay) {
                    periods[Trigger.CorruptionTick] /= 1 + stats.SpellHaste;
                }
                chances[Trigger.CorruptionTick] = 1f;
            } else {
                periods[Trigger.CorruptionTick] = 0f;
                chances[Trigger.CorruptionTick] = 0f;
            }
            float duration
                = (character.CalculationOptions as CalculationOptionsWarlock)
                    .Duration;
            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                stats.Accumulate(
                    effect.GetAverageStats(
                        periods[effect.Trigger],
                        chances[effect.Trigger],
                        AVG_UNHASTED_CAST_TIME,
                        options.Duration));
            }

            #endregion


            return stats;
        }

        /// <summary>
        /// Gets data for a custom chart that Rawr.Warlock provides
        /// </summary>
        /// <param name="character">
        /// The Character to get the chart data for
        /// </param>
        /// <param name="chartName">
        /// The name of the custom chart to get data for
        /// </param>
        /// <returns>The data for the custom chart</returns>
        public override ComparisonCalculationBase[]
            GetCustomChartData(Character character, string chartName) {

            return new ComparisonCalculationBase[0];
        }

        #endregion


        #region Relevancy Methods

        private List<ItemType> _relevantItemTypes = null;
        /// <summary>
        /// ItemTypes that are relevant to Rawr.Warlock
        /// </summary>
        public override List<ItemType> RelevantItemTypes {
            get {
                if (_relevantItemTypes == null) {
                    _relevantItemTypes = new List<ItemType>(
                        new ItemType[] {
                            ItemType.None,
                            ItemType.Cloth,
                            ItemType.Dagger,
                            ItemType.Wand,
                            ItemType.OneHandSword,
                            ItemType.Staff });
                }
                return _relevantItemTypes;
            }
        }

        /// <summary>
        /// GemmingTemplates to be used by default, when none are defined
        /// </summary>
        public override List<GemmingTemplate> DefaultGemmingTemplates {
            get {
                //Relevant Gem IDs for Warlocks
                //Red
                int[] runed = { 39911, 39998, 40113, 42144 };

                //Purple
                int[] purified = { 39941, 40026, 40133 };

                //Orange
                int[] reckless = { 39959, 40051, 40155 };
                int[] veiled = { 39957, 40049, 40153 };

                //Meta
                const int ember = 41333;
                const int chaotic = 41285;

                return new List<GemmingTemplate>
                {
                    #region uncommon
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //Max SP - Ember
                        RedId = runed[0], YellowId = runed[0], BlueId = runed[0], PrismaticId = runed[0], MetaId = ember
                    },
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //SP/Hit - Ember
                        RedId = runed[0], YellowId = veiled[0], BlueId = purified[0], PrismaticId = runed[0], MetaId = ember
                    },
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //SP/Haste - Ember
                        RedId = runed[0], YellowId = reckless[0], BlueId = purified[0], PrismaticId = runed[0], MetaId = ember
                    },
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //Max SP - Chaotic
                        RedId = runed[0], YellowId = runed[0], BlueId = runed[0], PrismaticId = runed[0], MetaId = chaotic
                    },
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //SP/Hit - Chaotic
                        RedId = runed[0], YellowId = veiled[0], BlueId = purified[0], PrismaticId = runed[0], MetaId = chaotic
                    },
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //SP/Haste - Chaotic
                        RedId = runed[0], YellowId = reckless[0], BlueId = purified[0], PrismaticId = runed[0], MetaId = chaotic
                    },
                    #endregion

                    #region rare
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Rare", //Max SP - Ember
				        RedId = runed[1], YellowId = runed[1], BlueId = runed[1], PrismaticId = runed[1], MetaId = ember
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Rare", //SP/Hit - Ember
				        RedId = runed[1], YellowId = veiled[1], BlueId = purified[1], PrismaticId = runed[1], MetaId = ember
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Rare", //SP/Haste - Ember
				        RedId = runed[1], YellowId = reckless[1], BlueId = purified[1], PrismaticId = runed[1], MetaId = ember
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Rare", //Max SP - Chaotic
				        RedId = runed[1], YellowId = runed[1], BlueId = runed[1], PrismaticId = runed[1], MetaId = chaotic
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Rare", //SP/Hit - Chaotic
				        RedId = runed[1], YellowId = veiled[1], BlueId = purified[1], PrismaticId = runed[1], MetaId = chaotic
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Rare", //SP/Haste - Chaotic
				        RedId = runed[1], YellowId = reckless[1], BlueId = purified[1], PrismaticId = runed[1], MetaId = chaotic
                    },
                    #endregion

                    #region epic
                    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //Max SP - Ember
				        RedId = runed[2], YellowId = runed[2], BlueId = runed[2], PrismaticId = runed[2], MetaId = ember
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //SP/Hit - Ember
				        RedId = runed[2], YellowId = veiled[2], BlueId = purified[2], PrismaticId = runed[2], MetaId = ember
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //SP/Haste - Ember
				        RedId = runed[2], YellowId = reckless[2], BlueId = purified[2], PrismaticId = runed[2], MetaId = ember
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //Max SP - Chaotic
				        RedId = runed[2], YellowId = runed[2], BlueId = runed[2], PrismaticId = runed[2], MetaId = chaotic
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //SP/Hit - Chaotic
				        RedId = runed[2], YellowId = veiled[2], BlueId = purified[2], PrismaticId = runed[2], MetaId = chaotic
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //SP/Haste - Chaotic
				        RedId = runed[2], YellowId = reckless[2], BlueId = purified[2], PrismaticId = runed[2], MetaId = chaotic
                    },
                    #endregion

                    #region jeweler
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Jeweler", //Max SP - Ember
				        RedId = runed[3], YellowId = runed[3], BlueId = runed[3], PrismaticId = runed[3], MetaId = ember
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Jeweler", //SP/Hit - Ember
				        RedId = runed[2], YellowId = runed[3], BlueId = runed[3], PrismaticId = runed[2], MetaId = ember
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Jeweler", //Max SP - Chaotic
				        RedId = runed[3], YellowId = runed[3], BlueId = runed[3], PrismaticId = runed[3], MetaId = chaotic
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Jeweler", //SP/Hit - Chaotic
				        RedId = runed[2], YellowId = runed[3], BlueId = runed[3], PrismaticId = runed[2], MetaId = chaotic
                    },
                    #endregion
                };
            }
        }

        public override Stats GetRelevantStats(Stats stats) {
            Stats s = new Stats {
                //primary stats
                SpellPower = stats.SpellPower,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                HitRating = stats.HitRating,
                SpellHit = stats.SpellHit,
                HasteRating = stats.HasteRating,
                SpellHaste = stats.SpellHaste,
                CritRating = stats.CritRating,
                SpellCrit = stats.SpellCrit,
                SpellCritOnTarget = stats.SpellCritOnTarget,

                ShadowDamage = stats.ShadowDamage,
                SpellShadowDamageRating = stats.SpellShadowDamageRating,
                FireDamage = stats.FireDamage,
                SpellFireDamageRating = stats.SpellFireDamageRating,

                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusFireDamageMultiplier = stats.BonusFireDamageMultiplier,
                SpellDamageFromSpiritPercentage = stats.SpellDamageFromSpiritPercentage,

                //set bonuses
                Warlock2T7 = stats.Warlock2T7,
                Warlock4T7 = stats.Warlock4T7,
                Warlock2T8 = stats.Warlock2T8,
                Warlock4T8 = stats.Warlock4T8,
                Warlock2T9 = stats.Warlock2T9,
                Warlock4T9 = stats.Warlock4T9,
                Warlock2T10 = stats.Warlock2T10,
                Warlock4T10 = stats.Warlock4T10,

                //These stats can be used by warlocks, but they dont affect our dps calculations at all.
                //Included for display purposes only.
                Stamina = stats.Stamina,
                //Health = stats.Health,
                //Mana = stats.Mana,
                //Mp5 = stats.Mp5,

                //The following are custom stat properties belonging to buffs, items (or procs) that can be used/applied to warlocks.
                HighestStat = stats.HighestStat,                                    //trinket - darkmoon card: greatness
                ManaRestoreFromBaseManaPPM = stats.ManaRestoreFromBaseManaPPM,      //paladin buff: judgement of wisdom
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,    //replenishment
                BonusManaPotion = stats.BonusManaPotion,                            //triggered when a mana pot is consumed
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,        //Bracing Eathsiege Diamond (metagem) effect
                ManaRestore = stats.ManaRestore,                                    //quite a few items that restore mana on spell cast or crit. Also used to model replenishment.
                SpellsManaReduction = stats.SpellsManaReduction,                    //spark of hope -> http://www.wowhead.com/?item=45703
            };

            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (RelevantTrinket(effect)) {
                    s.AddSpecialEffect(effect);
                }
            }
            return s;
        }

        protected bool RelevantTrinket(SpecialEffect effect) {
            if (effect.Trigger == Trigger.Use ||
                effect.Trigger == Trigger.DamageSpellCast ||
                effect.Trigger == Trigger.DamageSpellCrit ||
                effect.Trigger == Trigger.DamageSpellHit ||
                effect.Trigger == Trigger.SpellCast ||
                effect.Trigger == Trigger.SpellCrit ||
                effect.Trigger == Trigger.SpellHit ||
                effect.Trigger == Trigger.SpellMiss ||
                effect.Trigger == Trigger.DoTTick ||
                effect.Trigger == Trigger.DamageDone ||
                effect.Trigger == Trigger.DamageOrHealingDone) {
                return _HasRelevantStats(effect.Stats);
            }
            return false;
        }

        public override bool HasRelevantStats(Stats stats) {
            bool isRelevant = _HasRelevantStats(stats);

            foreach (SpecialEffect se in stats.SpecialEffects()) {
                isRelevant |= RelevantTrinket(se);
            }
            return isRelevant;
        }

        protected bool _HasRelevantStats(Stats stats) {
            bool yes = (
                //our primary stats
                stats.SpellPower
                + stats.Intellect
                + stats.Spirit
                + stats.HitRating + stats.SpellHit
                + stats.HasteRating + stats.SpellHaste
                + stats.CritRating + stats.SpellCrit + stats.SpellCritOnTarget
                + stats.ShadowDamage + stats.SpellShadowDamageRating
                + stats.FireDamage + stats.SpellFireDamageRating

                //multipliers
                + stats.BonusIntellectMultiplier
                + stats.BonusSpiritMultiplier + stats.SpellDamageFromSpiritPercentage
                + stats.BonusSpellCritMultiplier
                + stats.BonusDamageMultiplier + stats.BonusShadowDamageMultiplier + stats.BonusFireDamageMultiplier

                //warlock class buffs
                + stats.WarlockFelArmor
                + stats.WarlockDemonArmor
                + stats.WarlockSpellstoneDotDamageMultiplier
                + stats.WarlockSpellstoneHasteRating
                + stats.WarlockFirestoneDirectDamageMultiplier
                + stats.WarlockFirestoneSpellCritRating

                //set bonuses
                + stats.Warlock2T7
                + stats.Warlock4T7
                + stats.Warlock2T8
                + stats.Warlock4T8
                + stats.Warlock2T9
                + stats.Warlock4T9
                + stats.Warlock2T10
                + stats.Warlock4T10
            ) > 0;

            bool maybe = (
                //can be used by warlocks, but it does not affect our DPS calculations
                stats.Stamina

                //miscellaneous stats belonging to items (or trinket procs) that can be used/applied to warlocks
                //these stats are listed here so that those items (which supply them) can be listed
                //(these are terrible trinkets anyway - I should probably just remove them...)
                + stats.HighestStat                     //darkmoon card: greatness
                + stats.SpellsManaReduction             //spark of hope -> http://www.wowhead.com/?item=45703

                + stats.BonusManaPotion                 //triggered when a mana pot is consumed
                + stats.ManaRestoreFromBaseManaPPM      //judgement of wisdom
                + stats.ManaRestoreFromMaxManaPerSecond //replenishment sources
                + stats.ManaRestore                     //quite a few items that restore mana on spell cast or crit. Also used to model replenishment.
            ) > 0;

            bool no = (
                //ignore items with any of these stats
                stats.Health
                + stats.Mana + stats.Mp5
                + stats.Resilience
                + stats.Armor + stats.BonusArmor + stats.Agility
                + stats.ArmorPenetration + stats.ArmorPenetrationRating
                + stats.Strength + stats.AttackPower
                + stats.Expertise + stats.ExpertiseRating
                + stats.Dodge + stats.DodgeRating
                + stats.Parry + stats.ParryRating
                + stats.Defense + stats.DefenseRating
                + stats.ArcaneResistance + stats.ArcaneResistanceBuff
                + stats.FireResistance + stats.FireResistanceBuff
                + stats.FrostResistance + stats.FrostResistanceBuff
                + stats.NatureResistance + stats.NatureResistanceBuff
                + stats.ShadowResistance + stats.ShadowResistanceBuff
                + stats.ThreatReductionMultiplier       //bracing earthsiege diamond (metagem) effect
            ) > 0;

            return yes || (maybe && !no);
        }

        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot) {
            if (slot == ItemSlot.OffHand || slot == ItemSlot.Ranged) { return false; }
            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique) {
            if (slot == CharacterSlot.OffHand && item.Slot == ItemSlot.OneHand)
                return false;
            return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
        }

        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs() {
            if (_relevantGlyphs == null) {
                _relevantGlyphs
                    = new List<string>{
                        "Glyph of Metamorphosis",
                        "Glyph of Quick Decay",
                        "Glyph of Corruption",
                        "Glyph of Life Tap",
                        "Glyph of Curse of Agony",
                        "Glyph of Unstable Affliction",
                        "Glyph of Haunt" };
            }
            return _relevantGlyphs;
        }

        #endregion
    }
}
//3456789 223456789 323456789 423456789 523456789 623456789 723456789 8234567890