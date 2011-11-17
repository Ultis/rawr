using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Rawr.Moonkin
{
    [Rawr.Calculations.RawrModelInfo("Moonkin", "Spell_Nature_ForceOfNature", CharacterClass.Druid, CharacterRole.RangedDPS)]
    public class CalculationsMoonkin : CalculationsBase
    {
        #region Variables and Properties
        #region Gemming Templates
        private string[] tierNames = { "Uncommon", "Rare", "Epic", "Jewelcrafter" };

        // Red
        private int[] brilliant = { 52173, 52207, 71881, 52257 };

        // Orange
        private int[] reckless = { 52144, 52208, 71850, 52208 };
        private int[] artful = { 52140, 52205, 71854, 52205 };
        private int[] potent = { 52147, 52239, 71842, 52239 };

        // Purple
        private int[] purified = { 52100, 52236, 71868, 52236 };
        private int[] veiled = { 52153, 52217, 71864, 52217 };
        //private int[] timeless = { 52098, 52248, 52248, 52248 };

        // Meta
        private int burning = 68780;

        //Cogwheel
        private int cog_fractured = 59480;  //Mastery
        private int cog_sparkling = 59496;  //Spirit
        private int cog_quick = 59479;  //Haste
        private int cog_rigid = 59493;  //Hit
        private int cog_smooth = 59478;  //Crit

        /// <summary>
        /// List of gemming templates available to Rawr.
        /// </summary>
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                List<GemmingTemplate> retval = new List<GemmingTemplate>();
                for (int tier = 0; tier < 4; ++tier)
                {
                    retval.AddRange(MoonkinGemmingTemplateBlock(tier, burning));
                }
                retval.AddRange(new GemmingTemplate[] {
                // Engineering cogwheel templates (meta and 2 cogs each, no repeats)
                CreateMoonkinCogwheelTemplate(burning, cog_fractured, cog_quick),
                CreateMoonkinCogwheelTemplate(burning, cog_fractured, cog_rigid),
                CreateMoonkinCogwheelTemplate(burning, cog_fractured, cog_smooth),
                CreateMoonkinCogwheelTemplate(burning, cog_fractured, cog_sparkling),
                CreateMoonkinCogwheelTemplate(burning, cog_quick, cog_rigid),
                CreateMoonkinCogwheelTemplate(burning, cog_quick, cog_smooth),
                CreateMoonkinCogwheelTemplate(burning, cog_quick, cog_sparkling),
                CreateMoonkinCogwheelTemplate(burning, cog_rigid, cog_smooth),
                CreateMoonkinCogwheelTemplate(burning, cog_rigid, cog_sparkling),
                CreateMoonkinCogwheelTemplate(burning, cog_smooth, cog_sparkling),
                });
                return retval;
            }
        }

        private List<GemmingTemplate> MoonkinGemmingTemplateBlock(int tier, int meta)
        {
            List<GemmingTemplate> retval = new List<GemmingTemplate>();
            retval.AddRange(new GemmingTemplate[]
                {
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, brilliant, brilliant, brilliant, meta), // Straight Intellect
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, reckless, brilliant, brilliant, meta), // Int/Haste/Int
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, potent, brilliant, brilliant, meta), // Int/Crit/Int
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, artful, brilliant, brilliant, meta), // Int/Mastery/Int
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, reckless, veiled, brilliant, meta), // Int/Haste/Hit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, reckless, purified, brilliant, meta), // Int/Haste/Spirit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, potent, veiled, brilliant, meta), // Int/Crit/Hit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, potent, purified, brilliant, meta), // Int/Crit/Spirit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, artful, veiled, brilliant, meta), // Int/Mastery/Hit
                CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, artful, purified, brilliant, meta), // Int/Mastery/Spirit
                // Not that these aren't a good thought, but if a moonkin is gemming for stam, he's doing it wrong
                //CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, reckless, timeless, brilliant, meta), // Int/Haste/Stam
                //CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, potent, timeless, brilliant, meta), // Int/Crit/Stam
                //CreateMoonkinGemmingTemplate(tier, tierNames, brilliant, artful, timeless, brilliant, meta), // Int/Mastery/Stam
                });
            return retval;
        }

        const int DEFAULT_GEMMING_TIER = 1;
        private GemmingTemplate CreateMoonkinGemmingTemplate(int tier, string[] tierNames, int[] red, int[] yellow, int[] blue, int[] prismatic, int meta)
        {
            return new GemmingTemplate
            {
                Model = "Moonkin",
                Group = tierNames[tier],
                Enabled = (tier == DEFAULT_GEMMING_TIER),
                RedId = red[tier],
                YellowId = yellow[tier],
                BlueId = blue[tier],
                PrismaticId = prismatic[tier],
                MetaId = meta
            };
        }

        private GemmingTemplate CreateMoonkinCogwheelTemplate(int meta, int cogwheel1, int cogwheel2)
        {
            return new GemmingTemplate
            {
                Model = "Moonkin",
                Group = "Engineer",
                Enabled = false,
                MetaId = meta,
                CogwheelId = cogwheel1,
                Cogwheel2Id = cogwheel2
            };
        }
        #endregion

        /// <summary>Labels of the stats available to the Optimizer</summary>
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
                    "Hit Rating",
                    "Haste Rating",
                    "Crit Rating",
                    "Mastery Rating"
                    };
                return _optimizableCalculationLabels;
            }
        }
        private string[] _optimizableCalculationLabels = null;

        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, Color>();
                    _subPointNameColors.Add("Burst DPS", Colors.Red);
                    _subPointNameColors.Add("Sustained DPS", Colors.Blue);
                }
                return _subPointNameColors;
            }
        }
        private Dictionary<string, Color> _subPointNameColors = null;

        private string[] characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (characterDisplayCalculationLabels == null)
                {
                    characterDisplayCalculationLabels = new string[] {
                    "Basic Stats:Health",
                    "Basic Stats:Mana",
                    "Basic Stats:Agility",
                    "Basic Stats:Stamina",
                    "Basic Stats:Intellect",
                    "Basic Stats:Spirit",
                    "Basic Stats:Armor",
                    "Spell Stats:Spell Power",
                    "Spell Stats:Spell Hit",
                    "Spell Stats:Spell Crit",
                    "Spell Stats:Spell Haste",
                    "Spell Stats:Mastery",
                    "Regen:Mana Regen",
                    "Solution:Total Score",
                    "Solution:Selected Rotation",
                    "Solution:Selected DPS",
                    "Solution:Selected Time To OOM",
                    "Solution:Selected Cycle Length",
                    "Solution:Selected Spell Breakdown",
                    "Solution:Burst Rotation",
                    "Solution:Burst DPS",
                    "Solution:Burst Time To OOM",
                    "Solution:Burst Cycle Length",
                    "Solution:Burst Spell Breakdown",
                    "Solution:Nature's Grace Uptime",
                    "Solution:Solar Eclipse Uptime",
                    "Solution:Lunar Eclipse Uptime",
                    "Spell Info:Starfire",
                    "Spell Info:Wrath",
                    "Spell Info:Starsurge",
                    "Spell Info:Moonfire",
                    "Spell Info:Insect Swarm",
                    "Spell Info:Starfall",
                    "Spell Info:Treants",
                    "Spell Info:Wild Mushroom"
                    };
                }
                return characterDisplayCalculationLabels;
            }
        }

        public override ICalculationOptionsPanel CalculationOptionsPanel { get { if (calculationOptionsPanel == null) { calculationOptionsPanel = new CalculationOptionsPanelMoonkin(); } return calculationOptionsPanel; } }
        private ICalculationOptionsPanel calculationOptionsPanel = null;
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationMoonkin(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsMoonkin(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsMoonkin));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsMoonkin calcOpts = serializer.Deserialize(reader) as CalculationOptionsMoonkin;
            return calcOpts;
        }

        #endregion

        #region Relevancy
        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot)
        {
            // No enchants allowed on our ranged slot
            if (slot == ItemSlot.Ranged) return false;
            // Make an exception for enchant 4091 - Enchant Off-Hand - Superior Intellect
            if (slot == ItemSlot.OffHand && enchant.Id == 4091) return true;
            // No other enchants allowed on our offhands
            if (slot == ItemSlot.OffHand) return false;
            // Otherwise, return the base value
            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique)
        {
            if (slot == CharacterSlot.OffHand && item.Slot == ItemSlot.OneHand) return false;
            return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
        }

        public override List<Reforging> GetReforgingOptions(Item baseItem, int randomSuffixId)
        {
            List<Reforging> retval = base.GetReforgingOptions(baseItem, randomSuffixId);

            // If the item has spirit, do not allow reforging spirit -> hit
            if (baseItem.Stats.Spirit > 0)
            {
                // I put this in a sub-if because putting it above messes with the if-else/if-else code flow
                if (!_enableSpiritToHit)
                    retval.RemoveAll(rf => rf != null && rf.ReforgeFrom == AdditiveStat.Spirit && rf.ReforgeTo == AdditiveStat.HitRating);
            }
            // If it has hit, do not allow reforging hit -> spirit
            else if (baseItem.Stats.HitRating > 0)
            {
                retval.RemoveAll(rf => rf != null && rf.ReforgeFrom == AdditiveStat.HitRating && rf.ReforgeTo == AdditiveStat.Spirit);
            }
            // If it has neither, pick one based on the current calculation options.
            else
            {
                retval.RemoveAll(rf => rf != null && rf.ReforgeTo == (_reforgePriority == 0 ? AdditiveStat.HitRating : AdditiveStat.Spirit));
            }

            return retval;
        }

        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            if (_relevantGlyphs == null)
            {
                _relevantGlyphs = new List<string>();
                // Prime glyphs
                _relevantGlyphs.Add("Glyph of Insect Swarm");
                _relevantGlyphs.Add("Glyph of Moonfire");
                _relevantGlyphs.Add("Glyph of Starfire");
                _relevantGlyphs.Add("Glyph of Wrath");
                _relevantGlyphs.Add("Glyph of Starsurge");
                // Major glyphs
                _relevantGlyphs.Add("Glyph of Starfall");
                _relevantGlyphs.Add("Glyph of Focus");
            }
            return _relevantGlyphs;
        }

        /// <summary>
        /// List of itemtypes that are relevant for moonkin
        /// </summary>
        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                return _relevantItemTypes ?? (_relevantItemTypes = new List<ItemType>(new ItemType[]
                {
                            ItemType.None,
                            ItemType.Leather,
                            ItemType.Dagger,
                            ItemType.Staff,
                            ItemType.FistWeapon,
                            ItemType.OneHandMace,
                            ItemType.TwoHandMace,
                            ItemType.Relic,
                }));
            }
        }

        /// <summary>
        /// List of SpecialEffect Triggers that are relevant for moonkin model
        /// Every trigger listed here needs an implementation in ProcessSpecialEffects()
        /// A trigger not listed here should not appear in ProcessSpecialEffects()
        /// </summary>
        internal static List<Trigger> _RelevantTriggers = null;
        internal static List<Trigger> RelevantTriggers
        {
            get
            {
                return _RelevantTriggers ?? (_RelevantTriggers = new List<Trigger>() {
                            Trigger.Use,
                            Trigger.DamageSpellCast,
                            Trigger.DamageSpellCrit,        // Black magic enchant ?
                            Trigger.DamageSpellHit,
                            Trigger.SpellCast,
                            Trigger.SpellCrit,        
                            Trigger.SpellHit, 
                            Trigger.SpellMiss,
                            Trigger.DoTTick,
                            Trigger.DamageDone,
                            Trigger.DamageOrHealingDone,    // Darkmoon Card: Greatness
                            Trigger.InsectSwarmTick,
                            Trigger.MoonfireTick,
                            Trigger.MoonfireCast,
                            Trigger.EclipseProc,
                        });
            }
            //set { _RelevantTriggers = value; }
        }

        public override bool IsBuffRelevant(Buff buff, Character character)
        {
            // Any "Regalia" set bonus that applies to Druids is the Balance set
            if (!String.IsNullOrEmpty(buff.SetName) && buff.AllowedClasses.Contains(CharacterClass.Druid) && buff.SetName.EndsWith("Regalia"))
                return true;

            // for buffs that are non-exclusive, allow anything that could be useful even slightly
            if (buff.Group == "Elixirs and Flasks" || buff.Group == "Potion" || buff.Group == "Food" || buff.Group == "Scrolls")
                return base.IsBuffRelevant(buff, character);
            else
            {
                if (character != null && Rawr.Properties.GeneralSettings.Default.HideProfEnchants && !character.HasProfession(buff.Professions))
                    return false;
                // Class Restrictions Enforcement
                else if (character != null && !buff.AllowedClasses.Contains(character.Class))
                    return false;

                return HasPrimaryStats(buff.Stats) || HasSecondaryStats(buff.Stats) || HasExtraStats(buff.Stats);
            }
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                // -- State Properties --
                // Base Stats
                Health = stats.Health,
                Mana = stats.Mana,
                Agility = stats.Agility,
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                Armor = stats.Armor,
                HasteRating = stats.HasteRating,
                HitRating = stats.HitRating,
                CritRating = stats.CritRating,
                SpellPower = stats.SpellPower,
                MasteryRating = stats.MasteryRating,
                // SpellPenetration = stats.SpellPenetration,
                Mp5 = stats.Mp5,
                BonusArmor = stats.BonusArmor,

                // Buffs / Debuffs
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,

                // Combat Values
                SpellCrit = stats.SpellCrit,
                SpellCritOnTarget = stats.SpellCritOnTarget,
                SpellHit = stats.SpellHit,
                SpellCombatManaRegeneration = stats.SpellCombatManaRegeneration,
                TargetArmorReduction = stats.TargetArmorReduction,

                // Spell Combat Ratings
                SpellArcaneDamageRating = stats.SpellArcaneDamageRating,
                SpellNatureDamageRating = stats.SpellNatureDamageRating,
        
                // Equipment Effects
                ManaRestore = stats.ManaRestore,
                ShadowDamage = stats.ShadowDamage,
                NatureDamage = stats.NatureDamage,
                FireDamage = stats.FireDamage,
                HolyDamage = stats.HolyDamage,
                ArcaneDamage = stats.ArcaneDamage,
                HolySummonedDamage = stats.HolySummonedDamage,
                NatureSpellsManaCostReduction = stats.NatureSpellsManaCostReduction,

                // -- MultiplicativeStats --
                // Buffs / Debuffs
                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                BonusManaMultiplier = stats.BonusManaMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                BaseArmorMultiplier = stats.BaseArmorMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                SpellHaste = stats.SpellHaste,
                BonusCritDamageMultiplier = stats.BonusCritDamageMultiplier,
                BonusSpellCritDamageMultiplier = stats.BonusSpellCritDamageMultiplier,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusArcaneDamageMultiplier = stats.BonusArcaneDamageMultiplier,
                BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,    // Benefits trees

                // -- NoStackStats
                MovementSpeed = stats.MovementSpeed,
                SnareRootDurReduc = stats.SnareRootDurReduc,
                FearDurReduc = stats.FearDurReduc,
                StunDurReduc = stats.StunDurReduc,
                BonusManaPotionEffectMultiplier = stats.BonusManaPotionEffectMultiplier,
                HighestStat = stats.HighestStat,
                DragonwrathProc = stats.DragonwrathProc
            };

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (RelevantTriggers.Contains(effect.Trigger) && HasRelevantStats(effect.Stats))
                   s.AddSpecialEffect(effect);
            }
            return s;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return HasPrimaryStats(stats) || (HasSecondaryStats(stats) && !HasUnwantedStats(stats));
        }

        /// <summary>
        /// HasPrimaryStats() should return true if the Stats object has any stats that define the item
        /// as being 'for your class/spec'. For melee classes this is typical melee stats like Strength, 
        /// Agility, AP, Expertise... For casters it would be spellpower, intellect, ...
        /// As soon as an item/enchant/buff has any of the stats listed here, it will be assumed to be 
        /// relevant unless explicitely filtered out.
        /// Stats that could be usefull for both casters and melee such as HitRating, CritRating and Haste
        /// don't belong here, but are SecondaryStats. Specific melee versions of these do belong here 
        /// for melee, spell versions would fit here for casters.
        /// </summary>
        public bool HasPrimaryStats(Stats stats)
        {

            float ignoreStats = stats.Dodge + stats.Parry + stats.DodgeRating + stats.ParryRating + stats.ExpertiseRating + stats.Block + stats.BlockRating + stats.SpellShadowDamageRating + stats.SpellFireDamageRating + stats.SpellFrostDamageRating + stats.Health + stats.Armor + stats.PVPTrinket + stats.MovementSpeed + stats.Resilience + stats.BonusHealthMultiplier;

            bool PrimaryStats =
                // -- State Properties --
                // Base Stats
                stats.Intellect != 0 ||
                stats.Spirit != 0 ||
                stats.SpellPower != 0 ||
                // stats.SpellPenetration != 0 ||

                // Combat Values
                stats.SpellCrit != 0 ||
                stats.SpellCritOnTarget != 0 ||
                stats.SpellHit != 0 ||

                // Spell Combat Ratings
                stats.SpellArcaneDamageRating != 0 ||
                stats.SpellNatureDamageRating != 0 ||

                // -- MultiplicativeStats --
                // Buffs / Debuffs
                stats.BonusIntellectMultiplier != 0 ||
                stats.BonusSpiritMultiplier != 0 ||
                stats.SpellHaste != 0 ||
                stats.BonusSpellCritDamageMultiplier != 0 ||
                stats.BonusSpellPowerMultiplier != 0 ||
                stats.BonusArcaneDamageMultiplier != 0 ||
                stats.BonusNatureDamageMultiplier != 0;

            if (!PrimaryStats)
            {
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (RelevantTriggers.Contains(effect.Trigger) && HasPrimaryStats(effect.Stats))
                    {
                        PrimaryStats = true;
                        break;
                    }
                }
            }

            return PrimaryStats;
        }

        /// <summary>
        /// HasSecondaryStats() should return true if the Stats object has any stats that are relevant for the 
        /// model but only to a smaller degree, so small that you wouldn't typically consider the item.
        /// Stats that are usefull to both melee and casters (HitRating, CritRating & Haste) fit in here also.
        /// An item/enchant/buff having these stats would be considered only if it doesn't have any of the 
        /// unwanted stats.  Group/Party buffs are slighly different, they would be considered regardless if 
        /// they have unwanted stats.
        /// Note that a stat may be listed here since it impacts the model, but may also be listed as an unwanted stat.
        /// </summary>
        public bool HasSecondaryStats(Stats stats)
        {
            bool SecondaryStats =
                // -- State Properties --
                // Base Stats
                stats.Mana != 0 ||
                stats.HasteRating != 0 ||
                stats.HitRating != 0 ||
                stats.CritRating != 0 ||
                stats.Mp5 != 0 ||
                stats.MasteryRating != 0 ||

                // Buffs / Debuffs
                stats.ManaRestoreFromMaxManaPerSecond != 0 ||

                // Combat Values
                stats.SpellCombatManaRegeneration != 0 ||
                stats.TargetArmorReduction != 0 ||

                // Equipment Effects
                stats.ManaRestore != 0 ||
                stats.ShadowDamage != 0 ||
                stats.NatureDamage != 0 ||
                stats.FireDamage != 0 ||
                stats.HolySummonedDamage != 0 ||
                stats.NatureSpellsManaCostReduction != 0 ||

                // -- MultiplicativeStats --
                // Buffs / Debuffs
                stats.BonusManaMultiplier != 0 ||
                stats.BonusCritDamageMultiplier != 0 ||
                stats.BonusDamageMultiplier != 0 ||
                stats.BonusPhysicalDamageMultiplier != 0 ||

                // -- NoStackStats
                stats.MovementSpeed != 0 ||
                stats.SnareRootDurReduc != 0 ||
                stats.FearDurReduc != 0 ||
                stats.StunDurReduc != 0 ||
                stats.BonusManaPotionEffectMultiplier != 0 ||
                stats.DragonwrathProc != 0 ||
                stats.HighestStat != 0;

            if (!SecondaryStats)
            {
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (RelevantTriggers.Contains(effect.Trigger) && HasSecondaryStats(effect.Stats))
                    {
                        SecondaryStats = true;
                        break;
                    }
                }
            }

            return SecondaryStats;
        }

        /// <summary>
        /// Return true if the Stats object has any stats that don't influence the model but that you do want 
        /// to display in tooltips and in calculated summary values.
        /// </summary>
        public bool HasExtraStats(Stats stats)
        {
            bool ExtraStats =   
                stats.Health != 0 ||
                stats.Agility != 0 ||
                stats.Stamina != 0 ||
                stats.Armor != 0 ||
                stats.BonusArmor != 0 ||
                stats.BonusHealthMultiplier != 0 ||
                stats.BonusAgilityMultiplier != 0 ||
                stats.BonusStaminaMultiplier != 0  ||
                stats.BaseArmorMultiplier != 0 ||
                stats.BonusArmorMultiplier != 0;

            if (!ExtraStats)
            {
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (RelevantTriggers.Contains(effect.Trigger) && HasExtraStats(effect.Stats))
                    {
                        ExtraStats = true;
                        break;
                    }
                }
            }

            return ExtraStats;
        }

        /// <summary>
        /// Return true if the Stats object contains any stats that are making the item undesired.
        /// Any item having only Secondary stats would be removed if it also has one of these.
        /// </summary>
        public bool HasUnwantedStats(Stats stats)
        {
            /// List of stats that will filter out some buffs (Flasks, Elixirs & Scrolls), Enchants and Items.
            bool UnwantedStats = 
                stats.Strength > 0 ||
                stats.Agility > 0 ||
                stats.AttackPower > 0 ||
                stats.ExpertiseRating > 0 ||
                stats.DodgeRating > 0 ||
                stats.ParryRating > 0 ||
                stats.BlockRating > 0 ||
                stats.Resilience > 0;

            if (!UnwantedStats)
            {
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (/*RelevantTriggers.Contains(effect.Trigger) && */HasUnwantedStats(effect.Stats))    // An unwanted stat could be behind a trigger we don't model.
                    {
                        UnwantedStats = true;
                        break;
                    }
                }
            }

            return UnwantedStats;
        }

        public Stats GetBuffsStats(Character character, CalculationOptionsMoonkin calcOpts) {
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
            }
            // Removes the Hunter's Mark Buff and it's Children 'Glyphed', 'Improved' and 'Both' if you are
            // maintaining it yourself. We are now calculating this internally for better accuracy and to provide
            // value to relevant talents
            {
                hasRelevantBuff =  character.HunterTalents.ImprovedHuntersMark
                                + (character.HunterTalents.GlyphOfHuntersMark ? 1 : 0);
                Buff a = Buff.GetBuffByName("Hunter's Mark");
                Buff b = Buff.GetBuffByName("Glyphed Hunter's Mark");
                Buff c = Buff.GetBuffByName("Improved Hunter's Mark");
                Buff d = Buff.GetBuffByName("Improved and Glyphed Hunter's Mark");
                // Since we are doing base Hunter's mark ourselves, we still don't want to double-dip
                if (character.ActiveBuffs.Contains(a)) { character.ActiveBuffs.Remove(a); /*removedBuffs.Add(a);*//* }
                // If we have an enhanced Hunter's Mark, kill the Buff
                if (hasRelevantBuff > 0) {
                    if (character.ActiveBuffs.Contains(b)) { character.ActiveBuffs.Remove(b); /*removedBuffs.Add(b);*//* }
                    if (character.ActiveBuffs.Contains(c)) { character.ActiveBuffs.Remove(c); /*removedBuffs.Add(c);*//* }
                    if (character.ActiveBuffs.Contains(d)) { character.ActiveBuffs.Remove(d); /*removedBuffs.Add(c);*//* }
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

        public override void SetDefaults(Character character)
        {
            character.ActiveBuffsAdd(("Arcane Tactics"));
            character.ActiveBuffsAdd(("Arcane Brilliance (Mana)"));
            character.ActiveBuffsAdd(("Arcane Brilliance (SP%)"));
            character.ActiveBuffsAdd(("Blessing of Might (Mp5)"));
            character.ActiveBuffsAdd(("Moonkin Form"));
            character.ActiveBuffsAdd(("Elemental Oath"));
            character.ActiveBuffsAdd(("Enduring Winter"));
            character.ActiveBuffsAdd(("Power Word: Fortitude"));
            character.ActiveBuffsAdd(("Mark of the Wild"));
            character.ActiveBuffsAdd(("Earth and Moon"));
            character.ActiveBuffsAdd(("Critical Mass"));
            character.ActiveBuffsAdd(("Heroism/Bloodlust"));
            character.ActiveBuffsAdd(("Power Infusion"));
            character.ActiveBuffsAdd(("Flask of the Draconic Mind"));
            character.ActiveBuffsAdd(("Intellect Food"));
        }

        #endregion

        #region Custom Charts

        private string[] customChartNames = null;
        public override string[] CustomChartNames {
            get {
                if (customChartNames == null) {
                    customChartNames = new string[] { 
                    "Damage per Cast Time",
                    "Mana Gains By Source",
                    "Rotation Comparison",
                    "PTR Buff/Nerf"
                    };
                }
                return customChartNames;
            }
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            
            switch (chartName)
            {
                case "Mana Gains By Source":
                    CharacterCalculationsMoonkin calcsManaBase = GetCharacterCalculations(character) as CharacterCalculationsMoonkin;
                    RotationData manaGainsRot = calcsManaBase.SelectedRotation;
                    Character c2 = character.Clone();

                    List<ComparisonCalculationMoonkin> manaGainsList = new List<ComparisonCalculationMoonkin>();

                    // Euphoria
                    int previousTalentValue = c2.DruidTalents.Euphoria;
                    c2.DruidTalents.Euphoria = 0;
                    CharacterCalculationsMoonkin calcsManaMoonkin = GetCharacterCalculations(c2) as CharacterCalculationsMoonkin;
                    c2.DruidTalents.Euphoria = previousTalentValue;
                    foreach (RotationData rot in calcsManaMoonkin.Rotations)
                    {
                        if (rot.Name == manaGainsRot.Name)
                        {
                            manaGainsList.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = "Euphoria",
                                OverallPoints = (manaGainsRot.ManaGained - rot.ManaGained) / manaGainsRot.Duration * character.BossOptions.BerserkTimer * 60.0f,
                                BurstDamagePoints = (manaGainsRot.ManaGained - rot.ManaGained) / manaGainsRot.Duration * character.BossOptions.BerserkTimer * 60.0f,
                                ImageSource = "achievement_boss_valithradreamwalker"
                            });
                        }
                    }

                    // Replenishment
                    CalculationOptionsMoonkin calcOpts = c2.CalculationOptions as CalculationOptionsMoonkin;
                    calcOpts.Notify = false;
                    float oldReplenishmentUptime = calcOpts.ReplenishmentUptime;
                    calcOpts.ReplenishmentUptime = 0.0f;
                    CharacterCalculationsMoonkin calcsManaReplenishment = GetCharacterCalculations(c2) as CharacterCalculationsMoonkin;
                    calcOpts.ReplenishmentUptime = oldReplenishmentUptime;
                    foreach (RotationData rot in calcsManaReplenishment.Rotations)
                    {
                        if (rot.Name == manaGainsRot.Name)
                        {
                            manaGainsList.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = "Replenishment",
                                OverallPoints = (manaGainsRot.ManaGained - rot.ManaGained) / manaGainsRot.Duration * character.BossOptions.BerserkTimer * 60.0f,
                                BurstDamagePoints = (manaGainsRot.ManaGained - rot.ManaGained) / manaGainsRot.Duration * character.BossOptions.BerserkTimer * 60.0f,
                                ImageSource = "spell_magic_managain"
                            });
                        }
                    }

                    // Innervate
                    bool innervate = calcOpts.Innervate;
                    calcOpts.Innervate = false;
                    CharacterCalculationsMoonkin calcsManaInnervate = GetCharacterCalculations(c2) as CharacterCalculationsMoonkin;
                    calcOpts.Innervate = innervate;
                    foreach (RotationData rot in calcsManaInnervate.Rotations)
                    {
                        if (rot.Name == manaGainsRot.Name)
                        {
                            manaGainsList.Add(new ComparisonCalculationMoonkin()
                            {
                                Name = "Innervate",
                                OverallPoints = (manaGainsRot.ManaGained - rot.ManaGained) / manaGainsRot.Duration * character.BossOptions.BerserkTimer * 60.0f,
                                BurstDamagePoints = (manaGainsRot.ManaGained - rot.ManaGained) / manaGainsRot.Duration * character.BossOptions.BerserkTimer * 60.0f,
                                ImageSource = "spell_nature_lightning"
                            });
                        }
                    }
                    calcOpts.Notify = true;

                    // mp5
                    manaGainsList.Add(new ComparisonCalculationMoonkin()
                    {
                        Name = "MP5",
                        OverallPoints = character.BossOptions.BerserkTimer * 60.0f * calcsManaBase.ManaRegen,
                        BurstDamagePoints = character.BossOptions.BerserkTimer * 60.0f * calcsManaBase.ManaRegen
                    });
                    return manaGainsList.ToArray();

                case "Damage per Cast Time":
                    CharacterCalculationsMoonkin calcsBase = GetCharacterCalculations(character) as CharacterCalculationsMoonkin;
                    ComparisonCalculationMoonkin sf = new ComparisonCalculationMoonkin() { Name = "Starfire", ImageSource = "spell_arcane_starfire" };
                    ComparisonCalculationMoonkin mf = new ComparisonCalculationMoonkin() { Name = "Moonfire", ImageSource = "spell_nature_starfall" };
                    ComparisonCalculationMoonkin iSw = new ComparisonCalculationMoonkin() { Name = "Insect Swarm", ImageSource = "spell_nature_insectswarm" };
                    ComparisonCalculationMoonkin wr = new ComparisonCalculationMoonkin() { Name = "Wrath", ImageSource = "spell_nature_abolishmagic" };
                    ComparisonCalculationMoonkin ss = new ComparisonCalculationMoonkin() { Name = "Starsurge", ImageSource = "spell_arcane_arcane03" };
                    ComparisonCalculationMoonkin ssInst = new ComparisonCalculationMoonkin() { Name = "Shooting Stars Proc", ImageSource = "ability_mage_arcanebarrage" };
                    ComparisonCalculationMoonkin wm = new ComparisonCalculationMoonkin() { Name = "Wild Mushroom", ImageSource = "druid_ability_wildmushroom_b" };
                    ComparisonCalculationMoonkin sfa = new ComparisonCalculationMoonkin() { Name = "Starfall", ImageSource = "ability_druid_starfall" };
                    ComparisonCalculationMoonkin fon = new ComparisonCalculationMoonkin() { Name = "Force of Nature", ImageSource = "ability_druid_forceofnature" };
                    sf.BurstDamagePoints = calcsBase.SelectedRotation.StarfireAvgHit / calcsBase.SelectedRotation.StarfireAvgCast;
                    sf.OverallPoints = sf.BurstDamagePoints;
                    mf.BurstDamagePoints = calcsBase.SelectedRotation.MoonfireAvgHit / calcsBase.SelectedRotation.MoonfireAvgCast;
                    mf.OverallPoints = mf.BurstDamagePoints;
                    iSw.BurstDamagePoints = calcsBase.SelectedRotation.InsectSwarmAvgHit / calcsBase.SelectedRotation.InsectSwarmAvgCast;
                    iSw.OverallPoints = iSw.BurstDamagePoints;
                    wr.BurstDamagePoints = calcsBase.SelectedRotation.WrathAvgHit / calcsBase.SelectedRotation.WrathAvgCast;
                    wr.OverallPoints = wr.BurstDamagePoints;
                    // Use the Wrath average cast here because the Starsurge average cast is actually the combined weighted average
                    // of Starsurge and Shooting Stars
                    ss.BurstDamagePoints = calcsBase.SelectedRotation.StarSurgeAvgHit / calcsBase.SelectedRotation.WrathAvgCast;
                    ss.OverallPoints = ss.BurstDamagePoints;
                    ssInst.BurstDamagePoints = calcsBase.SelectedRotation.StarSurgeAvgHit / calcsBase.SelectedRotation.AverageInstantCast;
                    ssInst.OverallPoints = ssInst.BurstDamagePoints;
                    wm.BurstDamagePoints = calcsBase.SelectedRotation.MushroomDamage / 3f;
                    wm.OverallPoints = wm.BurstDamagePoints;
                    sfa.BurstDamagePoints = calcsBase.SelectedRotation.StarfallDamage / calcsBase.SelectedRotation.AverageInstantCast;
                    sfa.OverallPoints = sfa.BurstDamagePoints;
                    fon.BurstDamagePoints = calcsBase.SelectedRotation.TreantDamage / calcsBase.SelectedRotation.AverageInstantCast;
                    fon.OverallPoints = fon.BurstDamagePoints;
                    return new ComparisonCalculationMoonkin[] { sf, mf, iSw, wr, ss, ssInst, sfa, fon, wm };
                case "Rotation Comparison":
                    List<ComparisonCalculationMoonkin> comparisons = new List<ComparisonCalculationMoonkin>();
                    calcsBase = GetCharacterCalculations(character) as CharacterCalculationsMoonkin;
                    foreach (RotationData rot in calcsBase.Rotations)
                    {
                        comparisons.Add(new ComparisonCalculationMoonkin
                        {
                            Name = rot.Name,
                            BurstDamagePoints = rot.BurstDPS,
                            SustainedDamagePoints = rot.SustainedDPS,
                            OverallPoints = rot.BurstDPS + rot.SustainedDPS
                        });
                    }
                    return comparisons.ToArray();
                case "PTR Buff/Nerf":
                    calcsBase = GetCharacterCalculations(character) as CharacterCalculationsMoonkin;
                    c2 = character.Clone();

                    calcOpts = c2.CalculationOptions as CalculationOptionsMoonkin;
                    calcOpts.Notify = false;
                    calcOpts.PTRMode = !calcOpts.PTRMode;
                    CharacterCalculationsMoonkin calcsCompare = GetCharacterCalculations(c2) as CharacterCalculationsMoonkin;

                    calcOpts.PTRMode = !calcOpts.PTRMode;
                    calcOpts.Notify = true;

                    float burstDpsDelta = calcOpts.PTRMode ? calcsBase.SubPoints[0] - calcsCompare.SubPoints[0] : calcsCompare.SubPoints[0] - calcsBase.SubPoints[0];
                    float sustDpsDelta = calcOpts.PTRMode ? calcsBase.SubPoints[1] - calcsCompare.SubPoints[1] : calcsCompare.SubPoints[1] - calcsBase.SubPoints[1];

                    return new ComparisonCalculationMoonkin[] { new ComparisonCalculationMoonkin { Name = "PTR Mode", BurstDamagePoints = burstDpsDelta, SustainedDamagePoints = sustDpsDelta, OverallPoints = burstDpsDelta + sustDpsDelta } };
            }
            return new ComparisonCalculationBase[0];
        }

        #endregion

        #region Model Specific Variables and Functions
        private static int _reforgePriority = 0;
        private static bool _enableSpiritToHit = false;
        #endregion

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            // First things first, we need to ensure that we aren't using bad data
            CharacterCalculationsMoonkin calc = new CharacterCalculationsMoonkin();
            if (character == null) { return calc; }
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            if (calcOpts == null) { return calc; }
            //
            _reforgePriority = calcOpts.ReforgePriority;
            _enableSpiritToHit = calcOpts.AllowReforgingSpiritToHit;
            StatsMoonkin stats = (StatsMoonkin)GetCharacterStats(character, additionalItem);
            calc.BasicStats = stats;

            calc.SpellPower = (float)Math.Floor((1 + stats.BonusSpellPowerMultiplier) * (stats.SpellPower + stats.Intellect - 10));
            calc.SpellCrit = StatConversion.GetSpellCritFromIntellect(stats.Intellect) + StatConversion.GetSpellCritFromRating(stats.CritRating) + stats.SpellCrit + stats.SpellCritOnTarget;
            calc.SpellHit = StatConversion.GetSpellHitFromRating(stats.HitRating) + stats.SpellHit;
            calc.SpellHitCap = StatConversion.GetSpellMiss(character.Level - character.BossOptions.Level, false);
            calc.SpellHaste = (1 + StatConversion.GetSpellHasteFromRating(stats.HasteRating)) * (1 + stats.SpellHaste) - 1;
            calc.Mastery = 8.0f + StatConversion.GetMasteryFromRating(stats.MasteryRating);
            calc.ManaRegen = stats.Mp5 / 5f;

            // Generate the cycles
            /*if (referenceCalculation)
            {
                using (System.IO.StreamWriter writer = System.IO.File.CreateText("C:\\users\\Noah\\Desktop\\CastDistribution.txt"))
                {
                    //MoonkinCycleGenerator generator = new MoonkinCycleGenerator
                    //{
                    //EuphoriaChance = 0.24,
                    //Has4T12 = false,
                    //HasteLevel = 0,
                    //ShootingStarsChance = 0.04,
                    //StarlightWrathLevel = 3
                    //};

                    MoonkinSimulator generator = new MoonkinSimulator() { HasGlyphOfStarfire = false, Has4T13 = true };

                    writer.WriteLine("public static double[,] T13CastDistribution = new double[21, 12] {");

                    double[] baseRotationLengths = new double[21];
                    double[] baseNGUptimes = new double[21];
                    double[] baseMFExtended = new double[21];

                    for (int haste = 0; haste <= 100; haste += 5)
                    {
                        generator.HasteLevel = haste / 100.0;
                        double[] values = generator.GenerateCycle();
                        writer.Write("{");
                        for (int i = 0; i < values.Length; ++i)
                        {
                            writer.Write(String.Format(" {0},", values[i]));
                        }
                        writer.WriteLine(" },");
                        baseRotationLengths[haste / 5] = generator.GetRotationLength();
                        baseNGUptimes[haste / 5] = generator.GetNGUptime();
                        //baseMFExtended[haste / 5] = generator.GetPercentMoonfiresExtended();
                    }

                    writer.WriteLine("};");

                    generator.HasGlyphOfStarfire = true;

                    writer.WriteLine("public static double[,] T13CastDistributionGoSF = new double[21, 12] {");

                    double[] T12RotationLengths = new double[21];
                    double[] T12NGUptimes = new double[21];
                    double[] T12MFExtended = new double[21];

                    for (int haste = 0; haste <= 100; haste += 5)
                    {
                        generator.HasteLevel = haste / 100.0;
                        double[] values = generator.GenerateCycle();
                        writer.Write("{");
                        for (int i = 0; i < values.Length; ++i)
                        {
                            writer.Write(String.Format(" {0},", values[i]));
                        }
                        writer.WriteLine(" },");
                        T12RotationLengths[haste / 5] = generator.GetRotationLength();
                        T12NGUptimes[haste / 5] = generator.GetNGUptime();
                        T12MFExtended[haste / 5] = generator.GetPercentMoonfiresExtended();
                    }

                    writer.WriteLine("};");

                    writer.Write("public static double[] T13RotationDurations = new double[21] {");

                    for (int i = 0; i < baseRotationLengths.Length; ++i)
                    {
                        writer.Write(String.Format(" {0},", baseRotationLengths[i]));
                    }

                    writer.WriteLine(" };");

                    writer.Write("public static double[] T13RotationDurationsGoSF = new double[21] {");

                    for (int i = 0; i < T12RotationLengths.Length; ++i)
                    {
                        writer.Write(String.Format(" {0},", T12RotationLengths[i]));
                    }

                    writer.WriteLine(" };");

                    writer.Write("public static double[] T13NGUptimes = new double[21] {");

                    for (int i = 0; i < baseNGUptimes.Length; ++i)
                    {
                        writer.Write(String.Format(" {0},", baseNGUptimes[i]));
                    }

                    writer.WriteLine(" };");

                    writer.Write("public static double[] T13NGUptimesGoSF = new double[21] {");

                    for (int i = 0; i < T12NGUptimes.Length; ++i)
                    {
                        writer.Write(String.Format(" {0},", T12NGUptimes[i]));
                    }

                    writer.WriteLine(" };");

                    //writer.Write("public static double[] BasePercentMoonfiresExtended = new double[21] {");

                    //for (int i = 0; i < baseMFExtended.Length; ++i)
                    //{
                        //writer.Write(String.Format(" {0},", baseMFExtended[i]));
                    //}

                    //writer.WriteLine(" };");

                    writer.Write("public static double[] T13PercentMoonfiresExtended = new double[21] {");

                    for (int i = 0; i < T12MFExtended.Length; ++i)
                    {
                        writer.Write(String.Format(" {0},", T12MFExtended[i]));
                    }

                    writer.WriteLine(" };");
                }
                System.Windows.Application.Current.Shutdown();
            }*/

            // Run the solver against the generated cycle
            new MoonkinSolver().Solve(character, ref calc);

            return calc;
        }

        private static SpecialEffect _se_t114p = null;
        public static SpecialEffect _SE_T114P {
            get
            {
                // 4.2: 15% for 8 seconds, bonus reduced by 5% per crit.
                if (_se_t114p == null) {
                    _se_t114p = new SpecialEffect(Trigger.EclipseProc, new Stats() { SpellCrit = 0.15f }, 8.0f, 0f, 1f, 1);
                    _se_t114p.Stats.AddSpecialEffect(new SpecialEffect(Trigger.DamageSpellCrit, new Stats() { SpellCrit = -0.05f }, float.PositiveInfinity, 0.0f, 1f, 3));
                }
                return _se_t114p;
            }
        }

        // Placeholder special effect for T12 2-piece for easy calculating of uptime
        // The actual set bonus summons a Treant
        private static SpecialEffect _se_t122p = null;
        public static SpecialEffect _SE_T122P
        {
            get
            {
                if (_se_t122p == null)
                {
                    _se_t122p = new SpecialEffect(Trigger.MageNukeCast, new Stats() { }, 15f, 45f, 0.2f, 1);
                }
                return _se_t122p;
            }
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;

            StatsMoonkin statsTotal = new StatsMoonkin();
            Stats statsBase = BaseStats.GetBaseStats(character.Level, character.Class, character.Race, BaseStats.DruidForm.Moonkin);
            statsTotal.Accumulate(statsBase);
            
            // Get the gear/enchants/buffs stats loaded in
            statsTotal.Accumulate(GetItemStats(character, additionalItem));
            statsTotal.Accumulate(GetBuffsStats(character, calcOpts));

            #region Set Bonuses
            int T11Count, T12Count, T13Count;
            character.SetBonusCount.TryGetValue("Stormrider's Regalia", out T11Count);
            character.SetBonusCount.TryGetValue("Obsidian Arborweave Regalia", out T12Count);
            character.SetBonusCount.TryGetValue("Deep Earth Regalia", out T13Count);
            if (T11Count >= 2) {
                // 2 pieces: Increases the critical strike chance of your Insect Swarm and Moonfire spells by 5%.
                statsTotal.BonusCritChanceInsectSwarm = 0.05f;
                statsTotal.BonusCritChanceMoonfire = 0.05f;
            }
            if (T11Count >= 4) {
                // 4 pieces: Whenever Eclipse triggers, your critical strike chance with spells is increased by 99%
                //           for 8 sec. Each critical strike you achieve reduces that bonus by 33%.
                // 4.2: 15% for 8 seconds, bonus reduced by 5% per crit.
                statsTotal.AddSpecialEffect(_SE_T114P);
            }
            if (T12Count >= 2)
            {
                // 2 piece: You have a (20%) chance to summon a Burning Treant when you cast Wrath or Starfire.
                statsTotal.AddSpecialEffect(_SE_T122P);
            }
            if (T12Count >= 4)
            {
                // 4 piece: Your Wrath generates +3 and your Starfire generates +5 energy when not in Eclipse.
                statsTotal.BonusWrathEnergy = 3f + 1/3f;
                statsTotal.BonusStarfireEnergy = 5f;
            }
            if (T13Count >= 2)
            {
                // 2 piece: Your nukes deal 3% more damage when Insect Swarm is active on the target.
                statsTotal.BonusNukeDamageModifier = 0.03f;
            }
            if (T13Count >= 4)
            {
                // 4 piece: Your Starsurge generates +100% Lunar or Solar energy when not in Eclipse.
                statsTotal.T13FourPieceActive = true;
            }
            #endregion

            // Talented bonus multipliers
            Stats statsTalents = new StatsMoonkin()
            {
                BonusIntellectMultiplier = (1 + 0.02f * character.DruidTalents.HeartOfTheWild) * (Character.ValidateArmorSpecialization(character, ItemType.Leather) ? 1.05f : 1f) - 1f,
                BonusManaMultiplier = 0.05f * character.DruidTalents.Furor
            };
            statsTotal.BonusSpellDamageMultiplier = (1 + 0.01f * character.DruidTalents.BalanceOfPower) * (1 + 0.02f * character.DruidTalents.EarthAndMoon) *
                                            (1 + 0.1f * character.DruidTalents.MoonkinForm) *
                                            (1 + (character.DruidTalents.MoonkinForm > 0 ? 0.04f * character.DruidTalents.MasterShapeshifter : 0.0f)) - 1;

            statsTotal.Accumulate(statsTalents);

            // Base stats: Intellect, Stamina, Spirit, Agility
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Spirit = (float)Math.Floor(statsTotal.Spirit * (1 + statsTotal.BonusSpiritMultiplier));

            // Derived stats: Health, mana pool, armor
            statsTotal.Health = (float)Math.Round(statsTotal.Health + StatConversion.GetHealthFromStamina(statsTotal.Stamina));
            statsTotal.Health = (float)Math.Floor(statsTotal.Health * (1f + statsTotal.BonusHealthMultiplier));
            statsTotal.Mana = (float)Math.Round(statsTotal.Mana + StatConversion.GetManaFromIntellect(statsTotal.Intellect));
            statsTotal.Mana = (float)Math.Floor(statsTotal.Mana * (1f + statsTotal.BonusManaMultiplier));
            // Armor
            statsTotal.Armor = statsTotal.Armor * (1f + statsTotal.BaseArmorMultiplier);
            statsTotal.BonusArmor = statsTotal.BonusArmor * (1f + statsTotal.BonusArmorMultiplier);
            statsTotal.Armor += statsTotal.BonusArmor;
            statsTotal.Armor = (float)Math.Round(statsTotal.Armor);

            // Crit rating
            // Application order: Stats, Talents, Gear
            // All spells: Crit% + (0.02 * Nature's Majesty)
            statsTotal.SpellCrit += 0.02f * character.DruidTalents.NaturesMajesty;
            // All spells: Hit rating + 0.5f * Balance of Power * Spirit
            statsTotal.HitRating += 0.5f * character.DruidTalents.BalanceOfPower * (statsTotal.Spirit - statsBase.Spirit);

            return statsTotal;
        }
    }
}
