using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using Rawr.Elemental.Spells;

namespace Rawr.Elemental {
    [Rawr.Calculations.RawrModelInfo("Elemental", "Spell_Nature_Lightning", CharacterClass.Shaman)]
    public class CalculationsElemental : CalculationsBase {
        #region Variables and Properties

        #region Gemming Templates
        private List<GemmingTemplate> _defaultGemmingTemplates = null;
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                if (_defaultGemmingTemplates == null)
                {
                    // Meta
                    int chaotic = 52291;

                    // [0] uncommon
                    // [1] rare
                    // [2] jewelcrafting

                    // Reds
                    int[] brilliant = { 52084, 52207, 52257 }; // spell power
                    // Blue
                    int[] sparkling = { 52087, 52244, 52262 }; // spirit -> hit makes spirit better than hit
                    // Yellow
                    int[] quick = { 52093, 52232, 52268 }; // haste
                    int[] fractured = { 52094, 52219, 52269 }; // mastery
                    // Purple
                    int[] purified = { 52100, 52236 }; // int + spirit
                    // Green
                    int[] zen = { 52127, 52250 }; // mastery + spirit
                    // Orange
                    int[] artful = { 52117, 52205 }; // int + mastery
                    int[] reckless = { 52208, 52113 }; // int + haste

                    /*
                     * red: brilliant
                     * yellow: quick, fractured
                     * blue: sparkling
                     * purple: purified
                     * green: zen
                     * orange: artful, reckless
                     */

                    _defaultGemmingTemplates = new List<GemmingTemplate>();
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Uncommon", true, brilliant[0], sparkling[0], quick[0], fractured[0], purified[0], zen[0], artful[0], reckless[0], chaotic);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Uncommon", true, brilliant[1], sparkling[1], quick[1], fractured[1], purified[1], zen[1], artful[1], reckless[1], chaotic);
                    AddJCGemmingTemplateGroup(_defaultGemmingTemplates, "Jewelcrafting", false, brilliant[2], quick[2], fractured[2], sparkling[2], chaotic);
                }
                return _defaultGemmingTemplates;
            }
        }

        private void AddGemmingTemplateGroup(List<GemmingTemplate> list, string name, bool enabled, int brilliant, int sparkling, int quick, int fractured, int purified, int zen, int artful, int reckless, int meta)
        {
            // Brilliant is always good.  Sparkling and purified help get hit capped.
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, RedId = brilliant, YellowId = brilliant, BlueId = brilliant, PrismaticId = brilliant, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, RedId = sparkling, YellowId = sparkling, BlueId = sparkling, PrismaticId = sparkling, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, RedId = purified, YellowId = purified, BlueId = purified, PrismaticId = purified, MetaId = meta, Enabled = enabled });

            /*
             * red: brilliant, purified, artful, reckless
             * yellow: artful, fractured, reckless, quick, zen
             * blue: purified, zen, sparkling
             */
            // +Power and +Mastery
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, 
                RedId = brilliant, YellowId = artful, BlueId = purified, PrismaticId = brilliant, 
                MetaId = meta, Enabled = enabled });
            // +Hit
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, 
                RedId = purified, YellowId = zen, BlueId = sparkling, PrismaticId = sparkling, 
                MetaId = meta, Enabled = enabled });
            // Power and +Haste
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name,
                RedId = brilliant, YellowId = quick, BlueId = purified, PrismaticId = brilliant,
                MetaId = meta, Enabled = enabled });
        }

        private void AddJCGemmingTemplateGroup(List<GemmingTemplate> list, string name, bool enabled, int brilliant, int quick, int fractured, int sparkling, int meta)
        {
            // Overrides, only "runed" and "seers"
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, RedId = brilliant, YellowId = quick, BlueId = sparkling, PrismaticId = brilliant, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, RedId = brilliant, YellowId = fractured, BlueId = sparkling, PrismaticId = brilliant, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, RedId = brilliant, YellowId = quick, BlueId = sparkling, PrismaticId = fractured, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Elemental", Group = name, RedId = brilliant, YellowId = fractured, BlueId = sparkling, PrismaticId = fractured, MetaId = meta, Enabled = enabled });
        }
        #endregion

        // TODO: Override GetCharacterStatsString(Character character)
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
                    "Basic Stats:Spell Power",
                    "Basic Stats:Hit Rating",
                    "Basic Stats:Crit Rating",
                    "Basic Stats:Haste Rating",
                    "Basic Stats:Mastery Rating", 
                    "Basic Stats:Mana Regen",

                    "Combat Stats:Average Health",
                    "Combat Stats:Average Mana",
                    "Combat Stats:Average Stamina",
                    "Combat Stats:Average Spell Power",
                    "Combat Stats:Average Hit Rating",
                    "Combat Stats:Average Crit Rating",
                    "Combat Stats:Average Haste Rating",
                    "Combat Stats:Average Mastery Rating",
                    "Combat Stats:Average Mana Regen",

                    "Attacks:Lightning Bolt",
                    "Attacks:Chain Lightning",
                    "Attacks:Lava Burst",
                    "Attacks:Flame Shock",
                    "Attacks:Earth Shock",
                    "Attacks:Fulmination",
                    "Attacks:Fire Nova",
                    "Attacks:Searing Totem",
                    "Attacks:Magma Totem",
                    "Attacks:Unleashed Elements",
                    
                    "Simulation:Simulation",
                    "Simulation:Rotation",
                    "Simulation:Priority List"
                };
                return _characterDisplayCalculationLabels;
            }
        }

        // TODO: private string[] _optimizableCalculationLabels = null
        // TODO: public string[] OptimizableCalculationLabels { get { ... } }

        private Dictionary<string, Color> _subPointNameColors = null;
        public override Dictionary<string, Color> SubPointNameColors {
            get {
                if (_subPointNameColors == null) {
                    _subPointNameColors = new Dictionary<string, Color>();
                    _subPointNameColors.Add("Burst DPS", Colors.Red);
                    _subPointNameColors.Add("Sustained DPS", Colors.Blue);
                }
                return _subPointNameColors;
            }
        }

        public override CharacterClass TargetClass { get { return CharacterClass.Shaman; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationElemental(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsElemental(); }
        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel { get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelElemental()); } }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsElemental));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsElemental calcOpts = serializer.Deserialize(reader) as CalculationOptionsElemental;
            return calcOpts;
        }

        #endregion

        #region Relevancy
        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<ItemType>(new ItemType[]
                    {
                        ItemType.None,
                        ItemType.Cloth,
                        ItemType.Leather,
                        ItemType.Mail,
                        ItemType.Dagger,
                        ItemType.FistWeapon,
                        ItemType.OneHandAxe,
                        ItemType.OneHandMace,
                        ItemType.Shield,
                        ItemType.Staff,
                        ItemType.Totem,
                        ItemType.Relic,
                        ItemType.TwoHandAxe,
                        ItemType.TwoHandMace
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override bool IsItemRelevant(Item item)
        {
            if ((item.Slot == ItemSlot.Ranged && (item.Type != ItemType.Totem && item.Type != ItemType.Relic)))
                return false;
            return base.IsItemRelevant(item);
        }

        public override bool IsBuffRelevant(Buff buff, Character character) {
            string name = buff.Name;
            if (!buff.AllowedClasses.Contains(CharacterClass.Shaman)) {
                return false;
            }
            return base.IsBuffRelevant(buff, character);
        }

        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot)
        {
            // Filters out Non-Shield Offhand Enchants and Ranged Enchants
            if ((slot == ItemSlot.OffHand && enchant.Slot != ItemSlot.OffHand) || slot == ItemSlot.Ranged) return false;
            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        internal static List<Trigger> _RelevantTriggers = null;
        internal static List<Trigger> RelevantTriggers
        {
            get
            {
                return _RelevantTriggers ?? (_RelevantTriggers = new List<Trigger>() {
                    // General
                    Trigger.Use,
                    Trigger.SpellCast, Trigger.SpellHit, Trigger.SpellCrit, Trigger.SpellMiss,
                    Trigger.DamageSpellCast, Trigger.DamageSpellHit, Trigger.DamageSpellCrit,
                    Trigger.DoTTick,
                    Trigger.DamageDone,
                    Trigger.DamageOrHealingDone,
                    // Special
                    Trigger.DarkIntentCriticalPeriodicDamageOrHealing,
                    // Elemental Specific
                    Trigger.ShamanLightningBolt,
                    Trigger.ShamanFlameShockDoTTick,
                    Trigger.ShamanShock,
                });
            }
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                #region Basic stats
                Intellect = stats.Intellect,
                Mana = stats.Mana,
                Spirit= stats.Spirit,
                SpellCrit = stats.SpellCrit,
                SpellCritOnTarget = stats.SpellCritOnTarget,
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
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,
                ManaRestore = stats.ManaRestore,
                NatureSpellsManaCostReduction = stats.NatureSpellsManaCostReduction,
                MovementSpeed = stats.MovementSpeed,
                SnareRootDurReduc = stats.SnareRootDurReduc,
                FearDurReduc = stats.FearDurReduc,
                StunDurReduc = stats.StunDurReduc,
                #endregion
                #region Multipliers
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                BonusSpellCritDamageMultiplier = stats.BonusSpellCritDamageMultiplier,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                BonusFireDamageMultiplier = stats.BonusFireDamageMultiplier,
                BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                #endregion
                #region Sets
                BonusDamageMultiplierLavaBurst = stats.BonusDamageMultiplierLavaBurst,
                #endregion
                #region Misc Damage
                NatureDamage = stats.NatureDamage,
                ArcaneDamage = stats.ArcaneDamage,
                FireDamage = stats.FireDamage,
                ShadowDamage = stats.ShadowDamage
                #endregion
            };
            #region Trinkets
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (RelevantTriggers.Contains(effect.Trigger))
                {
                    if (HasRelevantStats(effect.Stats))
                    {
                        s.AddSpecialEffect(effect);
                    }
                }
            }
            #endregion
            return s;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            #region Basic stats
            if (stats.Mana != 0) return true;
            if (stats.Spirit != 0) return true;
            if (stats.SpellCrit != 0) return true;
            if (stats.SpellCritOnTarget != 0) return true;
            if (stats.SpellHit != 0) return true;
            if (stats.SpellHaste != 0) return true;
            if (stats.SpellPower != 0) return true;
            if (stats.CritRating != 0) return true;
            if (stats.HasteRating != 0) return true;
            if (stats.HitRating != 0) return true;
            if (stats.SpellFireDamageRating != 0) return true;
            if (stats.SpellNatureDamageRating != 0) return true;
            if (stats.SpellFrostDamageRating != 0) return true;
            if (stats.Mp5 != 0) return true;
            if (stats.ManaRestoreFromMaxManaPerSecond != 0) return true;
            if (stats.ManaRestore != 0) return true;
            if (stats.NatureSpellsManaCostReduction != 0) return true;
            if (stats.MovementSpeed != 0) return true;
            if (stats.SnareRootDurReduc != 0) return true;
            if (stats.FearDurReduc != 0) return true;
            if (stats.StunDurReduc != 0) return true;
            #endregion
            #region Multipliers
            if (stats.BonusIntellectMultiplier != 0) return true;
            if (stats.BonusSpiritMultiplier != 0) return true;
            if (stats.BonusSpellCritDamageMultiplier != 0) return true;
            if (stats.BonusSpellPowerMultiplier != 0) return true;
            if (stats.BonusFireDamageMultiplier != 0) return true;
            if (stats.BonusFrostDamageMultiplier != 0) return true;
            if (stats.BonusFrostDamageMultiplier != 0) return true;
            if (stats.BonusDamageMultiplier != 0) return true;
            #endregion
            #region Sets
            if (stats.BonusDamageMultiplierLavaBurst != 0) return true;
            #endregion
            #region Misc Damage
            if (stats.NatureDamage != 0) return true;
            if (stats.ArcaneDamage != 0) return true;
            if (stats.FireDamage != 0) return true;
            if (stats.ShadowDamage != 0) return true;
            #endregion
            #region Trinkets
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (RelevantTriggers.Contains(effect.Trigger))
                {
                    if (HasRelevantStats(effect.Stats)) return true;
                }
            }
            #endregion
            return false;
        }

        public Stats GetBuffsStats(Character character, CalculationOptionsElemental calcOpts) {
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

        public override void SetDefaults(Character character)
        {
            character.ActiveBuffsAdd("Sanctified Retribution");
            character.ActiveBuffsAdd("Heroism/Bloodlust");
            character.ActiveBuffsAdd("Swift Retribution");
            character.ActiveBuffsAdd("Arcane Intellect");
            character.ActiveBuffsAdd("Hunting Party");
            character.ActiveBuffsAdd("Blessing of Wisdom");
            character.ActiveBuffsAdd("Moonkin Form");
            character.ActiveBuffsAdd("Wrath of Air Totem");
            character.ActiveBuffsAdd("Totem of Wrath (Spell Power)");
            character.ActiveBuffsAdd("Divine Spirit");
            character.ActiveBuffsAdd("Mark of the Wild");
            character.ActiveBuffsAdd("Blessing of Kings");
            character.ActiveBuffsAdd("Totem of Wrath");
            character.ActiveBuffsAdd("Judgement of Wisdom");
            character.ActiveBuffsAdd("Improved Shadow Bolt");
            character.ActiveBuffsAdd("Curse of the Elements");
            character.ActiveBuffsAdd("Improved Faerie Fire");
            character.ActiveBuffsAdd("Flask of the Frost Wyrm");
            character.ActiveBuffsAdd("Fish Feast");
        }
        #endregion

        #region Custom Charts
        private string[] _customChartNames = {};
        public override string[] CustomChartNames { get { return _customChartNames; } }
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName) { return new ComparisonCalculationBase[0]; }
        #endregion

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            // First things first, we need to ensure that we aren't using bad data
            CharacterCalculationsElemental calc = new CharacterCalculationsElemental();
            if (character == null) { return calc; }
            CalculationOptionsElemental calcOpts = character.CalculationOptions as CalculationOptionsElemental;
            if (calcOpts == null) { return calc; }
            //
            BossOptions bossOpts = character.BossOptions;
            Stats stats = GetCharacterStats(character, additionalItem);

            calc.BasicStats = stats;
            calc.LocalCharacter = character;
            calcOpts.calculatedStats = calc;

            Rawr.Elemental.Estimation.solve(calc, calcOpts, bossOpts);

            return calc;
            
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsElemental calcOpts = character.CalculationOptions as CalculationOptionsElemental;

            Stats statsRace = BaseStats.GetBaseStats(character);
            Stats statsItems = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character, calcOpts);

            Stats statsTotal = statsRace + statsItems + statsBuffs;

            if (statsTotal.HighestStat > 0) {
                if (statsTotal.Spirit > statsTotal.Intellect) {
                    statsTotal.Spirit += (statsTotal.HighestStat * 15f / 50f);
                } else {
                    statsTotal.Intellect += (statsTotal.HighestStat * 15f / 50f);
                }
            }

            statsTotal.Strength *= 1 + statsTotal.BonusStrengthMultiplier;
            statsTotal.Agility *= 1 + statsTotal.BonusAgilityMultiplier;
            statsTotal.Stamina *= 1 + statsTotal.BonusStaminaMultiplier;
            statsTotal.Intellect *= 1 + statsTotal.BonusIntellectMultiplier;
            statsTotal.Spirit *= 1 + statsTotal.BonusSpiritMultiplier;

            statsTotal.Strength = (float)Math.Floor(statsTotal.Strength);
            statsTotal.Agility = (float)Math.Floor(statsTotal.Agility);
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina);
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect);
            statsTotal.Spirit = (float)Math.Floor(statsTotal.Spirit);

            if (ValidateMailSpecialization(character))
                statsTotal.Intellect *= 1.05f;

            statsTotal.AttackPower += statsTotal.Strength + statsTotal.Agility;

            statsTotal.Mana += StatConversion.GetManaFromIntellect(statsTotal.Intellect);
            statsTotal.Mana *= (float)Math.Round(1f + statsTotal.BonusManaMultiplier);

            statsTotal.Health += StatConversion.GetHealthFromStamina(statsTotal.Stamina);
            statsTotal.Health *= (float)Math.Round(1f + statsTotal.BonusHealthMultiplier);

            statsTotal.SpellCrit += StatConversion.GetSpellCritFromRating(statsTotal.CritRating);
            statsTotal.SpellCrit += StatConversion.GetSpellCritFromIntellect(statsTotal.Intellect);
            statsTotal.SpellCrit += statsTotal.SpellCritOnTarget;
            statsTotal.SpellHit += StatConversion.GetSpellHitFromRating(statsTotal.HitRating + 
                ((.33f * character.ShamanTalents.ElementalPrecision) * statsTotal.Spirit));

            // Flametongue weapon assumed
            statsTotal.SpellPower += (float)Math.Floor(747.78 * (1f + character.ShamanTalents.ElementalWeapons * .2f));
            if (character.ShamanTalents.GlyphofFlametongueWeapon) statsTotal.SpellCrit += .02f;

            // Water shield assumed
            statsTotal.Mp5 += 100;
            if (character.ShamanTalents.GlyphofWaterMastery) statsTotal.Mp5 += 30;

            return statsTotal;
        }
    }

    public class CharacterCalculationsElemental : CharacterCalculationsBase
    {

        #region Variable Declarations and Definitions

        public Stats BasicStats { get; set; }
        public Stats CombatStats { get; set; }
        public int TargetLevel { get; set; }
        public float Mastery { get; set; }

        public Spell LightningBolt;
        public Spell ChainLightning;
        public Spell LavaBurst;
        public Spell FlameShock;
        public Spell EarthShock;
        public Spell FrostShock;
        public Spell FireNova;
        public Spell SearingTotem;
        public Spell MagmaTotem;

        public float ManaRegenInFSR;
        public float ReplenishMP5;

        public float TimeToOOM;
        public float CastRegenFraction;
        public float RotationDPS;
        public float TotalDPS;
        public float RotationMPS;
        public float CastsPerSecond;
        public float CritsPerSecond;
        public float MissesPerSecond;

        public float LBPerSecond;
        public float LvBPerSecond;
        public float FSPerSecond;

        public float LatencyPerSecond;

        public float ClearCast_FlameShock;
        public float ClearCast_LavaBurst;
        public float ClearCast_LightningBolt;

        public string Rotation;
        public string RotationDetails;
        public string PriorityDetails;

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

            float BasicUntilGCDCap = (float)Math.Ceiling((1.5f / (1f + BasicStats.SpellHaste) - 1) * StatConversion.RATING_PER_SPELLHASTE
                - BasicStats.HasteRating);
            float BasicUntilLBCap = (float)Math.Ceiling((2f / (1f + BasicStats.SpellHaste) - 1) * StatConversion.RATING_PER_SPELLHASTE
                - BasicStats.HasteRating);

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Spell Power", BasicStats.SpellPower.ToString());
            dictValues.Add("Hit Rating", BasicStats.HitRating.ToString());
            dictValues.Add("Crit Rating", BasicStats.CritRating.ToString());
            dictValues.Add("Haste Rating", BasicStats.HasteRating.ToString() + "*" + BasicUntilGCDCap + " Haste Rating until GCD cap\n" + BasicUntilLBCap + " Haste Rating until LB cap");
            dictValues.Add("Mastery Rating", BasicStats.MasteryRating.ToString());
            dictValues.Add("Mana Regen", Math.Round(ManaRegenInFSR).ToString() + " + " + Math.Round(ReplenishMP5).ToString() + "*All values are Mana per 5 seconds.\nMP5 while casting / MP5 while not casting + Replenishment");

            float CombatUntilGCDCap = (float)Math.Ceiling((1.5f / (1f + CombatStats.SpellHaste) - 1) * StatConversion.RATING_PER_SPELLHASTE 
                - CombatStats.HasteRating);
            float CombatUntilLBCap = (float)Math.Ceiling((2f / (1f + CombatStats.SpellHaste) - 1) * StatConversion.RATING_PER_SPELLHASTE 
                - CombatStats.HasteRating);

            dictValues.Add("Average Health", CombatStats.Health.ToString());
            dictValues.Add("Average Mana", CombatStats.Mana.ToString());
            dictValues.Add("Average Stamina", CombatStats.Stamina.ToString());
            dictValues.Add("Average Intellect", CombatStats.Intellect.ToString());
            dictValues.Add("Average Spell Power", CombatStats.SpellPower.ToString());
            dictValues.Add("Average Hit Rating", CombatStats.HitRating.ToString());
            dictValues.Add("Average Crit Rating", CombatStats.CritRating.ToString());
            dictValues.Add("Average Haste Rating", CombatStats.HasteRating.ToString() + "*" + CombatUntilGCDCap + " Haste Rating until GCD cap\n" + CombatUntilLBCap + " Haste Rating until LB cap");
            dictValues.Add("Average Mastery Rating", CombatStats.MasteryRating.ToString());
            dictValues.Add("Average Mana Regen", Math.Round(ManaRegenInFSR).ToString() + " + " + Math.Round(ReplenishMP5).ToString() + "*All values are Mana per 5 seconds.\nMP5 while casting / MP5 while not casting + Replenishment");

            dictValues.Add("Lightning Bolt", Math.Round(LightningBolt.MinHit).ToString() + "-" + Math.Round(LightningBolt.MaxHit).ToString() + " / " + Math.Round(LightningBolt.MinCrit).ToString() + "-" + Math.Round(LightningBolt.MaxCrit).ToString() + "*Mana cost: " + Math.Round(LightningBolt.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * LightningBolt.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * LightningBolt.MissChance, 2).ToString() + " %\nCast time: " + Math.Round(LightningBolt.CastTime, 2) + " sec.\nClearcast uptime: " + Math.Round(100f * ClearCast_LightningBolt, 2).ToString() + " %");
            dictValues.Add("Chain Lightning", Math.Round(ChainLightning.MinHit).ToString() + "-" + Math.Round(ChainLightning.MaxHit).ToString() + " / " + Math.Round(ChainLightning.MinCrit).ToString() + "-" + Math.Round(ChainLightning.MaxCrit).ToString() + "*Targets: " + (((ChainLightning)ChainLightning).AdditionalTargets+1) + "\nMana cost: " + Math.Round(ChainLightning.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * ChainLightning.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * ChainLightning.MissChance, 2).ToString() + " %\nCast time: " + Math.Round(ChainLightning.CastTime, 2) + " sec.");
            dictValues.Add("Lava Burst", Math.Round(LavaBurst.MinHit).ToString() + "-" + Math.Round(LavaBurst.MaxHit).ToString() + " / " + Math.Round(LavaBurst.MinCrit).ToString() + "-" + Math.Round(LavaBurst.MaxCrit).ToString() + "*Mana cost: " + Math.Round(LavaBurst.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * LavaBurst.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * LavaBurst.MissChance, 2).ToString() + " %\nCast time: " + Math.Round(LavaBurst.CastTime, 2) + " sec.\nClearcast uptime: " + Math.Round(100f * ClearCast_LavaBurst, 2).ToString() + " %");
            dictValues.Add("Flame Shock", Math.Round(FlameShock.AvgHit).ToString() + " / " + Math.Round(FlameShock.AvgCrit).ToString() + " + " + Math.Round(FlameShock.PeriodicTick).ToString() + " (dot)*Duration: " + FlameShock.Duration + "\nTicks every " + Math.Round(FlameShock.PeriodicTickTime, 2).ToString()+ "s\nMana cost: " + Math.Round(FlameShock.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * FlameShock.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * FlameShock.MissChance, 2).ToString() + " %\nGCD: " + Math.Round(FlameShock.CastTime, 2) + " sec.\nClearcast uptime: " + Math.Round(100f * ClearCast_FlameShock, 2).ToString() + " %");
            dictValues.Add("Earth Shock", Math.Round(EarthShock.MinHit).ToString() + "-" + Math.Round(EarthShock.MaxHit).ToString() + " / " + Math.Round(EarthShock.MinCrit).ToString() + "-" + Math.Round(EarthShock.MaxCrit).ToString() + "*Mana cost: " + Math.Round(EarthShock.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * EarthShock.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * EarthShock.MissChance, 2).ToString() + " %\nGCD: " + Math.Round(EarthShock.CastTime, 2) + " sec.\n");
            dictValues.Add("Frost Shock", Math.Round(FrostShock.MinHit).ToString() + "-" + Math.Round(FrostShock.MaxHit).ToString() + " / " + Math.Round(FrostShock.MinCrit).ToString() + "-" + Math.Round(FrostShock.MaxCrit).ToString() + "*Mana cost: " + Math.Round(FrostShock.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * FrostShock.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * FrostShock.MissChance, 2).ToString() + " %\nGCD: " + Math.Round(FrostShock.CastTime, 2) + " sec.\n");
            dictValues.Add("Fire Nova", Math.Round(FireNova.MinHit).ToString() + "-" + Math.Round(FireNova.MaxHit).ToString() + " / " + Math.Round(FireNova.MinCrit).ToString() + "-" + Math.Round(FireNova.MaxCrit).ToString() + "*Targets: " + (((FireNova)FireNova).AdditionalTargets+1) + "\nMana cost: " + Math.Round(FireNova.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * FireNova.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * FireNova.MissChance, 2).ToString() + " %\nGCD: " + Math.Round(FireNova.CastTime, 2) + " sec.\n");
            dictValues.Add("Searing Totem", Math.Round(SearingTotem.PeriodicTick).ToString() + "*Mana cost: " + Math.Round(SearingTotem.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f*SearingTotem.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f*SearingTotem.MissChance, 2).ToString() + " %\nGCD: " + Math.Round(SearingTotem.CastTime,2).ToString() + " sec.\n");
            dictValues.Add("Magma Totem", Math.Round(MagmaTotem.PeriodicTick).ToString() + "*Targets: " + (((MagmaTotem)MagmaTotem).AdditionalTargets+1) + "\nMana cost: " + Math.Round(MagmaTotem.ManaCost).ToString() + "\nCrit chance: " + Math.Round(100f * MagmaTotem.CritChance, 2).ToString() + " %\nMiss chance: " + Math.Round(100f * MagmaTotem.MissChance, 2).ToString() + " %\nGCD: " + Math.Round(MagmaTotem.CastTime, 2).ToString() + " sec.\n");
            //dictValues.Add("Unleash Elements");

            dictValues.Add("Simulation", Math.Round(RotationDPS).ToString() + ((Math.Abs(RotationDPS - TotalDPS) >= 1) ? (" (" + Math.Round(TotalDPS).ToString() + ")") : "") + " DPS*OOM after " + Math.Round(TimeToOOM).ToString() + " sec.\nDPS until OOM: " + Math.Round(RotationDPS).ToString() + "\nMPS until OOM: " + Math.Round(RotationMPS).ToString() + "\nCast vs regen fraction after OOM: " + Math.Round(CastRegenFraction, 4).ToString() + "\n" + Math.Round(60f * CastsPerSecond, 1).ToString() + " casts per minute\n" + Math.Round(60f * CritsPerSecond, 1).ToString() + " crits per minute\n" + Math.Round(60f * MissesPerSecond, 1).ToString() + " misses per minute\n" + Math.Round(60f * LvBPerSecond, 1).ToString() + " Lava Bursts per minute\n" + Math.Round(60f * FSPerSecond, 1).ToString() + " Flame Shocks per minute\n" + Math.Round(60f * LBPerSecond, 1).ToString() + " Lightning Bolts per minute\n" + Math.Round(60f * LatencyPerSecond, 1).ToString() + " seconds lost to latency per minute\n");
            dictValues.Add("Rotation", Rotation + "*" + RotationDetails);

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

        private string _desc = string.Empty;
        public override string Description
        {
            get { return _desc; }
            set { _desc = value; }
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

        public float Burst
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float Sustained
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

        public override bool PartEquipped { get; set; }

        /*public override string ToString()
        {
            return string.Format("{0}: ({1}O {2}Burst {3}Sustained)", Name, Math.Round(OverallPoints), Math.Round(BurstPoints), Math.Round(SustainedPoints));
        }*/
    }

    public static class Constants
    {
        // Source: http://www.wowwiki.com/Base_mana
        public static float BaseMana = 4396;
    }
}
