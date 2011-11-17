using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Xml.Serialization;
using Rawr.ShadowPriest.CataSpells;


namespace Rawr.ShadowPriest
{
    /// <summary>
    /// Shadow Priest Model Calculations
    /// </summary>
    [Calculations.RawrModelInfo(MODEL_NAME, "Spell_Shadow_Shadowform", CharacterClass.Priest, CharacterRole.RangedDPS)]
    public class CalculationsShadowPriest : CalculationsBase
    {
        private const string MODEL_NAME = "ShadowPriest";

        private CalculationOptionsShadowPriest _calculationOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculationsShadowPriest"/> class.
        /// </summary>
        public CalculationsShadowPriest()
        {
            _calculationOptions = new CalculationOptionsShadowPriest();
        }

        private List<GemmingTemplate> _defaultGemmingTemplates;
        
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                if (_defaultGemmingTemplates == null)
                {
                    _defaultGemmingTemplates = new List<GemmingTemplate>();
                    
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Rare", true, 52207, 52239, 52208, 52205, 52236, 68780);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Rare (Jewelcrafter)", false, 52257, 52239, 52208, 52205, 52236, 68780);
                    
                    // cogwheels
                    int[] cog = new int[] { 59480, 59479, 59493, 59478 };
                    for (int i = 0; i < cog.Length; i++)
                    {
                        for (int j = i + 1; j < cog.Length; j++)
                        {
                            _defaultGemmingTemplates.Add(new GemmingTemplate() { Model = MODEL_NAME, Group = "Engineer", CogwheelId = cog[i], Cogwheel2Id = cog[j], MetaId = 68780, Enabled = false });
                        }
                    }
                }
                return _defaultGemmingTemplates;
            }
        }

        private void AddGemmingTemplateGroup(List<GemmingTemplate> list, string name, bool enabled, int brilliant, int potent, int reckless, int artful, int blue, int meta)
        {
            list.Add(new GemmingTemplate()
                         {
                             Model = MODEL_NAME,
                             Group = name,
                             RedId = brilliant,
                             YellowId = brilliant,
                             BlueId = brilliant,
                             PrismaticId = brilliant,
                             MetaId = meta,
                             Enabled = enabled
                         });

            list.Add(new GemmingTemplate()
                         {
                             Model = MODEL_NAME,
                             Group = name,
                             RedId = brilliant,
                             YellowId = potent,
                             BlueId = blue,
                             PrismaticId = brilliant,
                             MetaId = meta,
                             Enabled = enabled
                         });

            list.Add(new GemmingTemplate()
                         {
                             Model = MODEL_NAME,
                             Group = name,
                             RedId = brilliant,
                             YellowId = reckless,
                             BlueId = blue,
                             PrismaticId = brilliant,
                             MetaId = meta,
                             Enabled = enabled
                         });
            
            if (artful != 0)
            {
                list.Add(new GemmingTemplate()
                             {
                                 Model = MODEL_NAME,
                                 Group = name,
                                 RedId = brilliant,
                                 YellowId = artful,
                                 BlueId = blue,
                                 PrismaticId = brilliant,
                                 MetaId = meta,
                                 Enabled = enabled
                             });
            }
        }

        #region DeserializeDataObject

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsShadowPriest));
            StringReader reader = new StringReader(xml);
            CalculationOptionsShadowPriest calcOpts = serializer.Deserialize(reader) as CalculationOptionsShadowPriest;
            return calcOpts;
        }
        
        #endregion
        
        #region Charts
        
        #region Subpoints
        
        private Dictionary<string, Color> _subPointNameColors = null;
        
        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, Color>();
                    _subPointNameColors.Add("Burst", Color.FromArgb(255, 182, 0, 188));
                    _subPointNameColors.Add("Sustained", Color.FromArgb(255, 102, 0, 150));
                    _subPointNameColors.Add("Survivability", Color.FromArgb(255, 64, 128, 32));
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
        
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                return GetCharacterCalculationLabels();
            }
        }

        #endregion
        
        #region CalculationsOptionsPanel
        
        public ICalculationOptionsPanel _calculationOptionsPanel = null;

        public override ICalculationOptionsPanel CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelShadowPriest(_calculationOptions);
                }

                return _calculationOptionsPanel;
            }
        }
        
        #endregion
        
        #region Character

        #region Character Stats
        
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            PriestTalents talents = character.PriestTalents;

            Stats statsTotal = new Stats();
            
            Stats baseStats = BaseStats.GetBaseStats(character.Level, character.Class, character.Race);
            Stats itemStats = GetItemStats(character, additionalItem);
            Stats buffStats = GetBuffsStats(character, _calculationOptions);

            // Get the gear/enchants/buffs stats loaded in
            statsTotal.Accumulate(baseStats);
            statsTotal.Accumulate(itemStats);
            statsTotal.Accumulate(buffStats);

            Stats statsTalents = new Stats()
                                     {
                                         // we can only wear items that are cloth so we always have our specialization, even naked.
                                         BonusIntellectMultiplier = 0.05f,

                                         BonusShadowDamageMultiplier = (1 + 0.02f*talents.TwinDisciplines)*
                                                                       (1 + 0.02f*talents.TwistedFaith)*
                                                                       (1 + 0.15f*talents.Shadowform) - 1,

                                         BonusHolyDamageMultiplier = (1 + 0.02f*talents.TwinDisciplines) - 1,

                                         // this is the shadow priest model so they must have 'Shadow Power'
                                         BonusSpellPowerMultiplier = .15f,
                                     };

            statsTotal.Accumulate(statsTalents);
          
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect += (float)Math.Floor(itemStats.Intellect * statsTotal.BonusIntellectMultiplier);
            statsTotal.Spirit = (float) Math.Round(statsTotal.Spirit * (1 + statsTotal.BonusSpiritMultiplier));

            statsTotal.Health += (float)Math.Floor(StatConversion.GetHealthFromStamina(statsTotal.Stamina) * (1f + statsTotal.BonusHealthMultiplier));

            statsTotal.Mana = (float) Math.Round(statsTotal.Mana + StatConversion.GetManaFromIntellect(statsTotal.Intellect));
            statsTotal.Mana = (float) Math.Round(statsTotal.Mana*(1f + statsTotal.BonusManaMultiplier));
            
            statsTotal.SpellPower += statsTotal.Intellect - 10;

            float hasteFromRating = StatConversion.GetSpellHasteFromRating(statsTotal.HasteRating);
            float talentedHaste = (1 + hasteFromRating) * (1 + talents.Darkness * .01f) - 1;
            
            statsTotal.SpellHaste += character.Race == CharacterRace.Goblin ? talentedHaste*1.01f : talentedHaste;

            float baseBonus = (float) Math.Floor(baseStats.Spirit*statsTotal.BonusSpiritMultiplier);
            float itemBonus = (float) Math.Floor(itemStats.Spirit*statsTotal.BonusSpiritMultiplier);
            float spiritFromItemsAndEffects = baseBonus + itemBonus + itemStats.Spirit;
            float hitRatingFromSpirit = (0.5f * talents.TwistedFaith) * Math.Max(0f, spiritFromItemsAndEffects);
            
            statsTotal.HitRating += hitRatingFromSpirit;
            statsTotal.SpellHit += StatConversion.GetSpellHitFromRating(statsTotal.HitRating);

            // ignoring the base crit percentage here as the in-game tooltip says that the int -> crit conversion contains the base.
            float critFromInt = StatConversion.GetSpellCritFromIntellect(statsTotal.Intellect) + 0.012375f;
            float critFromRating = StatConversion.GetSpellCritFromRating(statsTotal.CritRating);
            
            statsTotal.SpellCrit = character.Race == CharacterRace.Worgen ? (critFromInt + critFromRating) + .01f : (critFromInt + critFromRating);
            
            // Armor
            statsTotal.Armor = statsTotal.Armor * (1f + statsTotal.BaseArmorMultiplier);
            statsTotal.BonusArmor = statsTotal.BonusArmor * (1f + statsTotal.BonusArmorMultiplier);
            statsTotal.Armor += statsTotal.BonusArmor;
            statsTotal.Armor = (float)Math.Round(statsTotal.Armor);

            return statsTotal;
        }

        private Stats GetBuffsStats(Character character, CalculationOptionsShadowPriest calcOpts)
        {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs, character.SetBonusCount);

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
            // First things first, we need to ensure that we aren't using bad data
            CharacterCalculationsShadowPriest calc = new CharacterCalculationsShadowPriest();
            if (character == null)
            {
                return calc;
            }

            var stats = GetCharacterStats(character, additionalItem);

            var results = new Dictionary<string, string>();

            BaseCharacterStatCalculations baseCalculations = new BaseCharacterStatCalculations(stats, _calculationOptions, character.BossOptions, character.PriestTalents);
            baseCalculations.Calculate(results);


            stats.SpellPower += _calculationOptions.InnerFire ? 532 : 0;

            BurstCalculations burstCalculations = new BurstCalculations(stats, _calculationOptions, character.BossOptions, character.PriestTalents);
            burstCalculations.Calculate(results);
            
            calc.BurstPoints = burstCalculations.Points;

            foreach (KeyValuePair<string, string> keyValuePair in results)
            {
                calc.AddResult(keyValuePair.Key, keyValuePair.Value);
            }

            CalculationOptionsShadowPriest calcOpts = character.CalculationOptions as CalculationOptionsShadowPriest;
            if (calcOpts == null)
            {
                return calc;
            }
            
            BossOptions bossOpts = character.BossOptions;

            calc.BasicStats = stats;
            calc.LocalCharacter = character;

            Solver.solve(calc, calcOpts, bossOpts);

            return calc;
        }

        private string[] GetCharacterCalculationLabels()
        {
            List<string> labels = new List<string>();

            var calcLabels = new List<string>();

            var baseCalcs = new BaseCharacterStatCalculations();
            baseCalcs.GetLabels(calcLabels);

            foreach (string calcLabel in calcLabels)
            {
                labels.Add("Base Stats:" + calcLabel);
            }

            calcLabels.Clear();
            var burtsCalcs = new BurstCalculations();
            burtsCalcs.GetLabels(calcLabels);

            foreach (string calcLabel in calcLabels)
            {
                labels.Add("Burst Calculations:" + calcLabel);
            }

            var otherLabels = new[]
                                  {
                                      "Simulation:Rotation",
                                      "Simulation:Castlist",
                                      "Simulation:DPS",
                                      "Shadow:Vampiric Touch",
                                      "Shadow:SW Pain",
                                      "Shadow:Devouring Plague",
                                      "Shadow:Imp. Devouring Plague",
                                      "Shadow:SW Death",
                                      "Shadow:Mind Blast",
                                      "Shadow:Mind Flay",
                                      "Shadow:Shadowfiend",
                                      "Shadow:Mind Spike",
                                      "Holy:PW Shield",

                                  };

            labels.AddRange(otherLabels);

            return labels.ToArray();
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

        /// <summary>
        /// An array of strings which define what calculations (in addition to the subpoint ratings)
        /// will be available to the optimizer
        /// </summary>
        /// <value></value>
        public override string[] OptimizableCalculationLabels
        {
            get { return Optimizations.Available; }
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            return new ComparisonCalculationBase[0];
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationShadowPriest();
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsShadowPriest();
        }

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
                MasteryRating = stats.MasteryRating,
                SpellShadowDamageRating = stats.SpellShadowDamageRating,
                SpellFrostDamageRating = stats.SpellFrostDamageRating,
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,
                ManaRestore = stats.ManaRestore,
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

            };

            #region Trinkets
            
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (IsUseful(effect.Trigger) && HasRelevantStats(effect.Stats))
                {
                    s.AddSpecialEffect(effect);
                }
            }
            
            #endregion
            
            return s;
        }

        private List<Trigger> _usefulTriggers = new List<Trigger>
                                                    {
                                                        Trigger.Use,
                                                        Trigger.SpellCast,
                                                        Trigger.SpellHit,
                                                        Trigger.SpellCrit,
                                                        Trigger.SpellMiss,
                                                        Trigger.DamageSpellCast,
                                                        Trigger.DamageSpellCrit,
                                                        Trigger.DamageSpellHit,
                                                        Trigger.DoTTick,
                                                        Trigger.DamageDone,
                                                        Trigger.DamageOrHealingDone
                                                    };

        private bool IsUseful(Trigger trigger)
        {
            return _usefulTriggers.Contains(trigger);
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
                stats.MasteryRating +
                stats.MovementSpeed +
                stats.SnareRootDurReduc +
                stats.FearDurReduc +
                stats.StunDurReduc;
            
            #endregion
            
            #region Multipliers
            
            shadowStats +=
                stats.BonusIntellectMultiplier +
                stats.BonusSpiritMultiplier +
                stats.BonusSpellCritDamageMultiplier +
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

            bool relevant = (shadowStats != 0);
            
            #region Trinkets
            
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (IsUseful(effect.Trigger))
                {
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
            if (slot == CharacterSlot.OffHand && item.Slot == ItemSlot.OneHand) 
                return false;

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
            if (!buff.AllowedClasses.Contains(CharacterClass.Priest))
            {
                return false;
            }

            return base.IsBuffRelevant(buff, character);
        }
    }

    public class BurstCalculations
    {
        private Stats _stats;
        private float _points;
        private BossHandler _boss;
        private PriestTalents _talents;
        private CalculationOptionsShadowPriest _options;

        public BurstCalculations()
        {
            
        }

        public BurstCalculations(Stats stats, CalculationOptionsShadowPriest options, BossOptions boss, PriestTalents talents)
        {
            _stats = stats;
            _boss = boss;
            _talents = talents;
            _options = options;
        }

        public float Points
        {
            get {
                return _points;
            }
        }

        public void Calculate(Dictionary<string, string> results)
        {
            var spikes = 3;
            var blasts = 1;

            MindSpike spike = new MindSpike(_talents, _stats, _boss);
            MindBlast blast = new MindBlast(_talents, _stats, _boss);

            blast.MindMelts = spikes;
            blast.MindSpikes = spikes;

            var rotationManaCost = spike.ManaCost*spikes + blast.ManaCost*blasts;
            var rotationTimeCost = spike.CastTime*spikes + blast.CastTime*blasts;
            var rotationDamage = spike.AverageDamage*spikes + blast.AverageDamage*blasts;
            var rotationDps = rotationDamage/rotationTimeCost;

            var maxRotations = _stats.Mana/rotationManaCost;
            if ( rotationTimeCost > _boss.BerserkTimer )
            {
                maxRotations = _boss.BerserkTimer/rotationTimeCost;
            }

            var totalDamage = rotationDamage*maxRotations;

            _points = rotationDps;

            results.Add("Damage Per Second", rotationDps.ToString("0.00"));
            results.Add("Rotations", maxRotations.ToString("0.00"));
            results.Add("Total Damage", totalDamage.ToString("0"));
        }

        public void GetLabels(List<string> labels)
        {
            labels.Add("Damage Per Second");
            labels.Add("Rotations");
            labels.Add("Total Damage");
        }
    }

    public class BaseCharacterStatCalculations
    {
        private Stats _stats;
        private PriestTalents _talents;
        private BossOptions _target;
        private CalculationOptionsShadowPriest _options;

        public BaseCharacterStatCalculations()
        {
            
        }

        public BaseCharacterStatCalculations(Stats stats, CalculationOptionsShadowPriest options, BossOptions boss, PriestTalents talents)
        {
            _stats = stats;
            _target = boss;
            _talents = talents;
            _options = options;
        }

        public void Calculate(Dictionary<string, string> results)
        {
            results.Add("Health", _stats.Health.ToString());
            results.Add("Mana", _stats.Mana.ToString());
            results.Add("Stamina", _stats.Stamina.ToString());
            results.Add("Intellect", _stats.Intellect.ToString());
            results.Add("Spirit", _stats.Spirit.ToString());
            results.Add("Hit", _stats.HitRating.ToString());
            
            results.Add("Spell Power", CalculateSpellPower());
            
            results.Add("Crit", _stats.CritRating.ToString());
            results.Add("Haste", _stats.HasteRating.ToString());
            results.Add("Mastery", _stats.MasteryRating.ToString());
        }

        private string CalculateSpellPower()
        {
            var baseSp = _stats.SpellPower;
            var total = baseSp + (_options.InnerFire ? 532 : 0);

            if (_options.InnerFire)
                return string.Format("{0}*{1} from Gear\r\n{2} from Inner Fire.", total, baseSp, 532);

            return total.ToString();
        }

        public void GetLabels(List<string> labels)
        {
            labels.Add("Health");
            labels.Add("Mana");
            labels.Add("Stamina");
            labels.Add("Intellect");
            labels.Add("Spirit");
            labels.Add("Hit");
            labels.Add("Spell Power");
            labels.Add("Crit");
            labels.Add("Haste");
            labels.Add("Mastery");
        }
    }

    public static class Constants
    {
        // Source: http://bobturkey.wordpress.com/2010/09/28/priest-base-mana-pool-and-mana-regen-coefficient-at-85/
        public static float BaseMana = 20590;
    }

    public static class Mechanics
    {
        /// <summary>
        /// Gets the chance to miss a target
        /// </summary>
        /// <param name="levelDelta">The level delta. that is (Attacker Level - Defender Level)</param>
        /// <returns>the chance to miss a target 0.09 = 9% chance to miss</returns>
        public static float GetSpellMiss(int levelDelta)
        {
            if (levelDelta < -3)
                return Math.Abs(0.11f * levelDelta) - 0.16f;

            if (levelDelta == -3)
                return Math.Abs(0.07f * levelDelta) - 0.04f;

            return Math.Max(0, Math.Abs((levelDelta - 4) * .01f));

        }
    }
}

namespace Rawr.ShadowPriest.CataSpells
{
    public class MindSpike
    {
        private readonly PriestTalents _talents;
        private readonly Stats _stats;

        private float _coeff = .8355f;
        private float _min = 1083;
        private float _max = 1143;
        private float _cast = 1500;
        private float _cost = 12;
        //private float _gcd = 1500;
        private BossHandler _target;

        public MindSpike(PriestTalents talents, Stats stats, BossHandler target)
        {
            _talents = talents;
            _stats = stats;
            _target = target;
        }

        public float ManaCost { get { return Constants.BaseMana * _cost / 100; } }
        public float CastTime { get { return _cast / (1 + _stats.SpellHaste) / 1000; } }
        
        public float AverageDamage
        {
            get
            {
                var delta = 85 - _target.Level;

                var missChance = Mechanics.GetSpellMiss(delta) - _stats.SpellHit;
                missChance = Math.Max(0, missChance);

                var power = _stats.SpellPower * _coeff;

                var avg = (_min + _max) / 2 + power;

                return (1 - missChance)*(avg*(1 + _stats.SpellCrit));
            }
        }

    }

    public class MindBlast
    {
        private readonly PriestTalents _talents;
        private readonly Stats _stats;

        private float _coeff = .9858f;

        private float _min = 1431;
        private float _max = 1511;
        private float _cast = 1500;
        private float _cost = 17;
        private float _gcd = 1500;
        private BossHandler _target;

        public MindBlast(PriestTalents talents, Stats stats, BossHandler target)
        {
            _talents = talents;
            _stats = stats;
            _target = target;
        }

        public float ManaCost { get { return Constants.BaseMana * _cost / 100; } }
        public float CastTime
        {
            get
            {
                float reduction = MindMelts*.5f;

                float cast = _cast/(1 + _stats.SpellHaste) * (1 - reduction);
                float gcd = _gcd/(1 + _stats.SpellHaste);

                return Math.Max(gcd, cast)/1000;
            }
        }

        public int MindSpikes { get; set; }
        public int MindMelts { get; set; }

        public float AverageDamage
        {
            get
            {
                var delta = 85 - _target.Level;

                var missChance = Mechanics.GetSpellMiss(delta) - _stats.SpellHit;
                missChance = Math.Max(0, missChance);

                var crit = _stats.SpellCrit + (MindSpikes*.3f);
                crit = Math.Min(crit, 1);

                var power = _stats.SpellPower * _coeff;

                var avg = (_min + _max) / 2 + power;

                return (1 - missChance)*(avg*(1 + crit));
            }
        }
    }

    

}
