using System;
using System.Collections.Generic;
using System.Text;
using Rawr.ShadowPriest.Spells;
using System.Windows.Media;

namespace Rawr.ShadowPriest
{
    [Rawr.Calculations.RawrModelInfo("ShadowPriest", "Spell_Shadow_Shadowform", CharacterClass.Priest)]
    public class CalculationsShadowPriest : CalculationsBase
    {
        private List<GemmingTemplate> _defaultGemmingTemplates = null;
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                if (_defaultGemmingTemplates == null)
                {
                    _defaultGemmingTemplates = new List<GemmingTemplate>();
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Old", false, 40113, 40152, 40155, 0, 40153, 41285);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Old (Jewelcrafting)", false, 42144, 40152, 40155, 0, 40153, 41285);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Epic", true, 52207, 52239, 52208, 52205, 52217, 52296);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Epic (Jewelcrafting)", false, 52257, 52239, 52208, 52205, 52217, 52296);
                }
                return _defaultGemmingTemplates;
            }
        }

        private void AddGemmingTemplateGroup(List<GemmingTemplate> list, string name, bool enabled, int brilliant, int potent, int reckless, int artful, int blue, int meta)
        {
            list.Add(new GemmingTemplate() { Model = "ShadowPriest", Group = name, RedId = brilliant, YellowId = brilliant, BlueId = brilliant, PrismaticId = brilliant, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "ShadowPriest", Group = name, RedId = brilliant, YellowId = potent, BlueId = blue, PrismaticId = brilliant, MetaId = meta, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "ShadowPriest", Group = name, RedId = brilliant, YellowId = reckless, BlueId = blue, PrismaticId = brilliant, MetaId = meta, Enabled = enabled });
            if (artful != 0)
            {
                list.Add(new GemmingTemplate() { Model = "ShadowPriest", Group = name, RedId = brilliant, YellowId = artful, BlueId = blue, PrismaticId = brilliant, MetaId = meta, Enabled = enabled });
            }
        }

        /*
        #region Gemming
        private List<GemmingTemplate> _defaultGemmingTemplates = null;
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                if (_defaultGemmingTemplates == null)
                {
                    // Meta
                    int chaotic = 41285;

                    /* NYI
                    //Cogwheel
                    int fracturedGW = 59480; //mastery
                    int ridgidGW = 59493; //hit
                    int smoothGW = 59378; //crit
                    int quickGW = 59479; //haste
                    int sparklingGW = 59496; //spirit
                    * /

                    // [0] uncommon
                    // [1] rare
                    // [2] jewelcrafting
                    
                    // Reds
                    int[] brilliant = { 52084, 52207, 52257 }; //int
                    // Blue
                    int[] sparkling = { 52087, 0, 0 };//spirit
                    int[] rigid = { 0, 0, 52264 };//hit
                    // Yellow
                    int[] fractured = { 52094, 0, 52269 }; //mastery 
                    int[] quick = { 0, 0, 52268  }; //haste
                    int[] smooth = { 0, 0, 52266 }; //crit
                    // Purple
                    int[] veiled = { 52104, 0 }; //int+hit
                    int[] timeless = { 52098, 52248 }; //int+stam
                    int[] purified = { 0, 52236 }; //int+spirit
                    // Green
                    int[] senseis = { 52128, 52237 }; //mastery+hit
                    int[] piercing = { 52122, 52228 }; //crit+hit
                    int[] lightning = { 0, 52225 }; //haste+hit
                    int[] zen = { 52127, 52250 }; //mastery+spirit
                    int[] puissant = { 52126, 52231 }; //mastery+stam
                    int[] jagged = { 52121, 0 }; //crit+stam
                    int[] forceful = { 52124, 52218 }; //haste+stam
                    // Orange
                    int[] artful = { 52117, 52205 }; //int+mastery
                    int[] reckless = { 52113, 0 }; //int+haste
                    int[] potent = { 0, 52239 }; //int+crit

                    _defaultGemmingTemplates = new List<GemmingTemplate>();
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Uncommon", false, brilliant[0], sparkling[0], rigid[0], quick[0], smooth[0], veiled[0], timeless[0], purified[0], senseis[0], piercing[0], lightning[0], zen[0], puissant[0], jagged[0], forceful[0],artful[0], reckless[0], potent[0], chaotic);

                    AddJCGemmingTemplateGroup(_defaultGemmingTemplates, "Jewelcrafting", false, brilliant[2], sparkling[2], rigid[2], fractured[2], quick[2], smooth[2], chaotic);
                }
             return _defaultGemmingTemplates;
            }
        }
        private void AddGemmingTemplateGroup(List<GemmingTemplate> list, string name, bool enabled, int brilliant, int sparkling, int rigid, int quick, int smooth, int veiled, int timeless, int purified, int senseis, int piercing, int lightning, int zen, int puissant, int jagged, int forceful, int artful, int reckless, int potent, int meta)
        {
            //int only
            list.Add(new GemmingTemplate() { Model = "ShadowPriest", Group = name, RedId = brilliant, YellowId = brilliant, BlueId = brilliant, PrismaticId = brilliant, MetaId = meta, Enabled = enabled });
        }

        private void AddJCGemmingTemplateGroup(List<GemmingTemplate> list, string name, bool enabled, int brilliant, int sparkling, int rigid, int fractured, int quick, int smooth, int meta)
        {
            //int only
            list.Add(new GemmingTemplate() { Model = "ShadowPriest", Group = name, RedId = brilliant, YellowId = brilliant, BlueId = brilliant, PrismaticId = brilliant, MetaId = meta, Enabled = enabled });
        }
        #endregion
        */
        #region DeserializeDataObject
        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsShadowPriest));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsShadowPriest calcOpts = serializer.Deserialize(reader) as CalculationOptionsShadowPriest;
            return calcOpts;
        }
        #endregion
        #region Charts
        #region Subpoints
        //shows bar for each on graphs
        private Dictionary<string, Color> _subPointNameColors = null;
        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, Color>();
                    _subPointNameColors.Add("DPS", Color.FromArgb(255, 0, 0, 255));
                    _subPointNameColors.Add("Survival", Color.FromArgb(255, 0, 0, 0));
                }
                return _subPointNameColors;
            }
        }
        #endregion
        #region Custom charts
        private string[] _customChartNames = {};
        public override string[] CustomChartNames
        {
            get
            {
                return _customChartNames;
            }
        }
        #endregion
        #endregion
        #region CharacterDisplayCalculationLabels
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
					"Basic Stats:Hit",
					"Basic Stats:Spell Power",
					"Basic Stats:Crit",
					"Basic Stats:Haste",
                    "Basic Stats:Mastery",
                    "Simulation:Rotation",
                    "Simulation:Castlist",
                    "Simulation:DPS",
//                    "Simulation:SustainDPS",
                    "Shadow:Vampiric Touch",
                    "Shadow:SW Pain",
                    "Shadow:Devouring Plague",
                    "Shadow:Imp. Devouring Plague",
				    "Shadow:SW Death",
                    "Shadow:Mind Blast",
                    "Shadow:Mind Flay",
                    "Shadow:Shadowfiend",
                    "Shadow:Mind Spike",
                    "Shadow:Mind Sear",
                    "Holy:PW Shield",
                     
                };
                return _characterDisplayCalculationLabels;
            }
        }
        #endregion
        #region CalculationsOptionsPanel
        public ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelShadowPriest();
                }
                return _calculationOptionsPanel;
            }
        }
        #endregion
        #region Character
        public override CharacterClass TargetClass { get { return CharacterClass.Priest; } }
        #region Character Stats
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsShadowPriest calcOpts = character.CalculationOptions as CalculationOptionsShadowPriest;


            Stats statsTotal = new Stats();
            statsTotal.Accumulate(BaseStats.GetBaseStats(character.Level, character.Class, character.Race));

            // Get the gear/enchants/buffs stats loaded in
            statsTotal.Accumulate(GetItemStats(character, additionalItem));
            statsTotal.Accumulate(GetBuffsStats(character, calcOpts));

            bool clothSpecialization = character.Head != null && character.Head.Type == ItemType.Cloth &&
                                            character.Shoulders != null && character.Shoulders.Type == ItemType.Cloth &&
                                            character.Chest != null && character.Chest.Type == ItemType.Cloth &&
                                            character.Wrist != null && character.Wrist.Type == ItemType.Cloth &&
                                            character.Hands != null && character.Hands.Type == ItemType.Cloth &&
                                            character.Waist != null && character.Waist.Type == ItemType.Cloth &&
                                            character.Legs != null && character.Legs.Type == ItemType.Cloth &&
                                            character.Feet != null && character.Feet.Type == ItemType.Cloth;


            // Talented bonus multipliers

            Stats statsTalents = new Stats()
            {
                BonusIntellectMultiplier = (clothSpecialization ? 1.05f : 1f) - 1,
                BonusShadowDamageMultiplier = (1 + 0.02f * character.PriestTalents.TwinDisciplines) *
                                              (1 + 0.02f * character.PriestTalents.TwistedFaith) - 1,
                BonusHolyDamageMultiplier = (1 + 0.02f * character.PriestTalents.TwinDisciplines) - 1
            };

            statsTotal.Accumulate(statsTalents);

            // Base stats: Intellect, Stamina, Spirit, Agility
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Floor(statsTotal.Spirit * (1 + statsTotal.BonusSpiritMultiplier));

            // Derived stats: Health, mana pool, armor
            statsTotal.Health = (float)Math.Floor(statsTotal.Health * (1f + statsTotal.BonusHealthMultiplier));
            statsTotal.Mana = (float)Math.Round(statsTotal.Mana + StatConversion.GetManaFromIntellect(statsTotal.Intellect));
            statsTotal.Mana = (float)Math.Floor(statsTotal.Mana * (1f + statsTotal.BonusManaMultiplier));
            // Armor
            statsTotal.Armor = statsTotal.Armor * (1f + statsTotal.BaseArmorMultiplier);
            statsTotal.BonusArmor += statsTotal.Agility * 2f;
            statsTotal.BonusArmor = statsTotal.BonusArmor * (1f + statsTotal.BonusArmorMultiplier);
            statsTotal.Armor += statsTotal.BonusArmor;
            statsTotal.Armor = (float)Math.Round(statsTotal.Armor);

            // Crit rating
            // Application order: Stats, Talents, Gear
            // All spells: Haste% + (0.01 * Darkness)
            statsTotal.SpellHaste += 0.02f * character.PriestTalents.Darkness;
            // All spells: Hit rating + 0.5f * Twisted Faith * Spirit
            statsTotal.HitRating += 0.5f * character.PriestTalents.TwistedFaith * statsTotal.Spirit;

            return statsTotal;
        }
        public Stats GetBuffsStats(Character character, CalculationOptionsShadowPriest calcOpts)
        {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            foreach (Buff b in removedBuffs)
            {
                character.ActiveBuffsAdd(b);
            }
            foreach (Buff b in addedBuffs)
            {
                character.ActiveBuffs.Remove(b);
            }

            return statsBuffs;
        }

        #endregion
        #region Character Calculations
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            CalculationOptionsShadowPriest calcOpts = character.CalculationOptions as CalculationOptionsShadowPriest;
            if (calcOpts == null) calcOpts = new CalculationOptionsShadowPriest();
            BossOptions bossOpts = character.BossOptions;
            if (bossOpts == null) bossOpts = new BossOptions();
            Stats stats = GetCharacterStats(character, additionalItem);

            CharacterCalculationsShadowPriest calculatedStats = new CharacterCalculationsShadowPriest();
            calculatedStats.BasicStats = stats;
            calculatedStats.LocalCharacter = character;
            calcOpts.calculatedStats = calculatedStats;


            Rawr.ShadowPriest.Solver.solve(calculatedStats, calcOpts, bossOpts);

            return calculatedStats;
        }
        #endregion
        #region Relevant Items
        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<ItemType>(new ItemType[]{
                        ItemType.None,
                        ItemType.Cloth,
                        ItemType.Dagger,
                        ItemType.Wand,
                        ItemType.OneHandMace,
                        ItemType.Staff
                    });
                }
                return _relevantItemTypes;
            }
        }
        #endregion
        #endregion
        #region CalculationBase
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            return new ComparisonCalculationBase[0];
        }  
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationShadowPriest(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsShadowPriest(); }
        #endregion
        #region Stats
        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                #region Basic stats
                Intellect = stats.Intellect,
                Mana = stats.Mana,
                Spirit = stats.Spirit,
                SpellCrit = stats.SpellCrit,
                SpellCritOnTarget = stats.SpellCritOnTarget,
                SpellHit = stats.SpellHit,
                SpellHaste = stats.SpellHaste,
                SpellPower = stats.SpellPower,
                CritRating = stats.CritRating,
                HasteRating = stats.HasteRating,
                HitRating = stats.HitRating,
                SpellShadowDamageRating = stats.SpellShadowDamageRating,
                SpellFrostDamageRating = stats.SpellFrostDamageRating,
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,
                ManaRestore = stats.ManaRestore,
                ManaRestoreFromBaseManaPPM = stats.ManaRestoreFromBaseManaPPM,
                MovementSpeed = stats.MovementSpeed,
                #endregion
                #region Multipliers
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                #endregion
                #region Misc Damage
                HolyDamage = stats.HolyDamage,
                FrostDamage = stats.FrostDamage,
                ShadowDamage = stats.ShadowDamage,
                #endregion
                #region Resistance
                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                ArcaneResistance = stats.ArcaneResistance,
                ArcaneResistanceBuff = stats.ArcaneResistanceBuff,
                FireResistance = stats.FireResistance,
                FireResistanceBuff = stats.FireResistanceBuff,
                FrostResistance = stats.FrostResistance,
                FrostResistanceBuff = stats.FrostResistanceBuff,
                NatureResistance = stats.NatureResistance,
                NatureResistanceBuff = stats.NatureResistanceBuff,
                ShadowResistance = stats.ShadowResistance,
                ShadowResistanceBuff = stats.ShadowResistanceBuff,
                #endregion

                /*
                Stamina = stats.Stamina,
                Health = stats.Health,
                Resilience = stats.Resilience,
                Intellect = stats.Intellect,
                Mana = stats.Mana,
                Spirit = stats.Spirit,
                SpellPower = stats.SpellPower,
                SpellShadowDamageRating = stats.SpellShadowDamageRating,
                CritRating = stats.CritRating,
                SpellCrit = stats.SpellCrit,
                SpellCritOnTarget = stats.SpellCritOnTarget,
                HitRating = stats.HitRating,
                SpellHit = stats.SpellHit,
                SpellHaste = stats.SpellHaste,
                HasteRating = stats.HasteRating,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusManaPotion = stats.BonusManaPotion,
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusHolyDamageMultiplier = stats.BonusHolyDamageMultiplier,
                BonusDiseaseDamageMultiplier = stats.BonusDiseaseDamageMultiplier,
                PriestInnerFire = stats.PriestInnerFire,
                MovementSpeed = stats.MovementSpeed,
                SWPDurationIncrease = stats.SWPDurationIncrease,
                BonusMindBlastMultiplier = stats.BonusMindBlastMultiplier,
                MindBlastCostReduction = stats.MindBlastCostReduction,
                ShadowWordDeathCritIncrease = stats.ShadowWordDeathCritIncrease,
                WeakenedSoulDurationDecrease = stats.WeakenedSoulDurationDecrease,
                DevouringPlagueBonusDamage = stats.DevouringPlagueBonusDamage,
                MindBlastHasteProc = stats.MindBlastHasteProc,
                PriestDPS_T9_2pc = stats.PriestDPS_T9_2pc,
                PriestDPS_T9_4pc = stats.PriestDPS_T9_4pc,
                PriestDPS_T10_2pc = stats.PriestDPS_T10_2pc,
                PriestDPS_T10_4pc = stats.PriestDPS_T10_4pc,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                ManaRestore = stats.ManaRestore,
                SpellsManaReduction = stats.SpellsManaReduction,
                HighestStat = stats.HighestStat,
                ArcaneDamage = stats.ArcaneDamage,
                FireDamage = stats.FireDamage,
                FrostDamage = stats.FrostDamage,
                HolyDamage = stats.HolyDamage,
                NatureDamage = stats.NatureDamage,
                ShadowDamage = stats.ShadowDamage,
                ValkyrDamage = stats.ValkyrDamage,

                /*ManaRestoreFromBaseManaPerHit = stats.ManaRestoreFromBaseManaPerHit,
                SpellPowerFor15SecOnUse90Sec = stats.SpellPowerFor15SecOnUse90Sec,
                SpellPowerFor15SecOnUse2Min = stats.SpellPowerFor15SecOnUse2Min,
                SpellPowerFor20SecOnUse2Min = stats.SpellPowerFor20SecOnUse2Min,
                HasteRatingFor20SecOnUse2Min = stats.HasteRatingFor20SecOnUse2Min,
                HasteRatingFor20SecOnUse5Min = stats.HasteRatingFor20SecOnUse5Min,
                SpellPowerFor10SecOnCast_15_45 = stats.SpellPowerFor10SecOnCast_15_45,
                SpellPowerFor10SecOnHit_10_45 = stats.SpellPowerFor10SecOnHit_10_45,
                SpellHasteFor10SecOnCast_10_45 = stats.SpellHasteFor10SecOnCast_10_45,
                TimbalsProc = stats.TimbalsProc,
                PendulumOfTelluricCurrentsProc = stats.PendulumOfTelluricCurrentsProc,
                ExtractOfNecromanticPowerProc = stats.ExtractOfNecromanticPowerProc,
                * /

                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                ArcaneResistance = stats.ArcaneResistance,
                ArcaneResistanceBuff = stats.ArcaneResistanceBuff,
                FireResistance = stats.FireResistance,
                FireResistanceBuff = stats.FireResistanceBuff,
                FrostResistance = stats.FrostResistance,
                FrostResistanceBuff = stats.FrostResistanceBuff,
                NatureResistance = stats.NatureResistance,
                NatureResistanceBuff = stats.NatureResistanceBuff,
                ShadowResistance = stats.ShadowResistance,
                ShadowResistanceBuff = stats.ShadowResistanceBuff,
                */
            };
            #region Trinkets
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use || 
                    effect.Trigger == Trigger.SpellCast || 
                    effect.Trigger == Trigger.SpellHit || 
                    effect.Trigger == Trigger.SpellCrit || 
                    effect.Trigger == Trigger.SpellMiss || 
                    effect.Trigger == Trigger.DamageSpellCast || 
                    effect.Trigger == Trigger.DamageSpellCrit || 
                    effect.Trigger == Trigger.DamageSpellHit || 
                    effect.Trigger == Trigger.DoTTick || 
                    effect.Trigger == Trigger.DamageDone ||
                    effect.Trigger == Trigger.DamageOrHealingDone)
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
            float shadowStats = 0;
                #region Basic stats
            shadowStats +=
                stats.Intellect +
                stats.Mana +
                stats.Spirit + 
                stats.SpellCrit +
                stats.SpellCritOnTarget +
                stats.SpellHit +
                stats.SpellHaste +
                stats.SpellPower +
                stats.CritRating +
                stats.HasteRating +
                stats.HitRating +
                stats.SpellShadowDamageRating +
                stats.SpellFrostDamageRating +
                stats.ManaRestoreFromMaxManaPerSecond +
                stats.ManaRestore +
                stats.ManaRestoreFromBaseManaPPM +
                stats.MovementSpeed;
                #endregion
                #region Multipliers
                shadowStats +=
                stats.BonusIntellectMultiplier +
                stats.BonusSpiritMultiplier +
                stats.BonusSpellCritMultiplier +
                stats.BonusSpellPowerMultiplier +
                stats.BonusShadowDamageMultiplier +
                stats.BonusFrostDamageMultiplier +
                stats.BonusDamageMultiplier;
                #endregion
                #region Misc Damage
                shadowStats +=
                stats.HolyDamage +
                stats.FrostDamage +
                stats.ShadowDamage;
                #endregion
                #region Resistance
                shadowStats +=
                stats.Armor +
                stats.BonusArmor +
                stats.ArcaneResistance +
                stats.ArcaneResistanceBuff +
                stats.FireResistance +
                stats.FireResistanceBuff +
                stats.FrostResistance +
                stats.FrostResistanceBuff +
                stats.NatureResistance +
                stats.NatureResistanceBuff +
                stats.ShadowResistance +
                stats.ShadowResistanceBuff;
                #endregion

            bool relevant = (shadowStats > 0);
            #region Trinkets
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use ||
                    effect.Trigger == Trigger.SpellCast ||
                    effect.Trigger == Trigger.SpellHit ||
                    effect.Trigger == Trigger.SpellCrit ||
                    effect.Trigger == Trigger.SpellMiss ||
                    effect.Trigger == Trigger.DamageSpellCast ||
                    effect.Trigger == Trigger.DamageSpellCrit ||
                    effect.Trigger == Trigger.DamageSpellHit ||
                    effect.Trigger == Trigger.DoTTick ||
                    effect.Trigger == Trigger.DamageDone ||
                    effect.Trigger == Trigger.DamageOrHealingDone)                {
                    relevant |= HasRelevantStats(effect.Stats);
                    if (relevant) break;
                }
            }
            #endregion
            return relevant;

        }
        #endregion
        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique)
        {
            if (slot == CharacterSlot.OffHand && item.Slot == ItemSlot.OneHand) return false;
            return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
        }
        public override void SetDefaults(Character character)
        {
            //character.ActiveBuffsAdd(("Sanctified Retribution"));
        }
        public override bool IsItemRelevant(Item item)
        {
            if ((item.Slot == ItemSlot.Ranged && item.Type != ItemType.Wand))
                return false;
            return base.IsItemRelevant(item);
        }
        public override bool IsBuffRelevant(Buff buff, Character character)
        {
            string name = buff.Name;
            if (!buff.AllowedClasses.Contains(CharacterClass.Priest))
            {
                return false;
            }
            return base.IsBuffRelevant(buff, character);
        }


    }

    public static class Constants
    {
        // Source: http://bobturkey.wordpress.com/2010/09/28/priest-base-mana-pool-and-mana-regen-coefficient-at-85/
        public static float BaseMana = 20590;
    }
}
