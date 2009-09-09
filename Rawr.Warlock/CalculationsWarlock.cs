using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Globalization;
using Rawr;

namespace Rawr.Warlock 
{
    [Rawr.Calculations.RawrModelInfo("Warlock", "Spell_Nature_FaerieFire", CharacterClass.Warlock)]
    public class CalculationsWarlock : CalculationsBase 
    {
        public override List<GemmingTemplate> DefaultGemmingTemplates 
        {
            get 
            {
				////Relevant Gem IDs for Warlocks
				//Red
				int[] runed = { 39911, 39998, 40113, 42144 };

				//Purple
				int[] purified = { 39941, 40026, 40133 };

				//Orange
				int[] reckless = { 39959, 40051, 40155 };
				int[] veiled = { 39957, 40049, 40153 };

				//Meta
				int ember = 41333;
				int chaotic = 41285;

				return new List<GemmingTemplate>() 
                {
				    new GemmingTemplate() { Model = "Warlock", Group = "Uncommon", //Max SP - Ember
				        RedId = runed[0], YellowId = runed[0], BlueId = runed[0], PrismaticId = runed[0], MetaId = ember },
				    new GemmingTemplate() { Model = "Warlock", Group = "Uncommon", //SP/Hit - Ember
				        RedId = runed[0], YellowId = veiled[0], BlueId = purified[0], PrismaticId = runed[0], MetaId = ember },
				    new GemmingTemplate() { Model = "Warlock", Group = "Uncommon", //SP/Haste - Ember
				        RedId = runed[0], YellowId = reckless[0], BlueId = purified[0], PrismaticId = runed[0], MetaId = ember },
				    new GemmingTemplate() { Model = "Warlock", Group = "Uncommon", //Max SP - Chaotic
				        RedId = runed[0], YellowId = runed[0], BlueId = runed[0], PrismaticId = runed[0], MetaId = chaotic },
				    new GemmingTemplate() { Model = "Warlock", Group = "Uncommon", //SP/Hit - Chaotic
				        RedId = runed[0], YellowId = veiled[0], BlueId = purified[0], PrismaticId = runed[0], MetaId = chaotic },
				    new GemmingTemplate() { Model = "Warlock", Group = "Uncommon", //SP/Haste - Chaotic
				        RedId = runed[0], YellowId = reckless[0], BlueId = purified[0], PrismaticId = runed[0], MetaId = chaotic },
						
				    new GemmingTemplate() { Model = "Warlock", Group = "Rare", Enabled = true, //Max SP - Ember
				        RedId = runed[1], YellowId = runed[1], BlueId = runed[1], PrismaticId = runed[1], MetaId = ember },
				    new GemmingTemplate() { Model = "Warlock", Group = "Rare", Enabled = true, //SP/Hit - Ember
				        RedId = runed[1], YellowId = veiled[1], BlueId = purified[1], PrismaticId = runed[1], MetaId = ember },
				    new GemmingTemplate() { Model = "Warlock", Group = "Rare", Enabled = true, //SP/Haste - Ember
				        RedId = runed[1], YellowId = reckless[1], BlueId = purified[1], PrismaticId = runed[1], MetaId = ember },
				    new GemmingTemplate() { Model = "Warlock", Group = "Rare", Enabled = true, //Max SP - Chaotic
				        RedId = runed[1], YellowId = runed[1], BlueId = runed[1], PrismaticId = runed[1], MetaId = chaotic },
				    new GemmingTemplate() { Model = "Warlock", Group = "Rare", Enabled = true, //SP/Hit - Chaotic
				        RedId = runed[1], YellowId = veiled[1], BlueId = purified[1], PrismaticId = runed[1], MetaId = chaotic },
				    new GemmingTemplate() { Model = "Warlock", Group = "Rare", Enabled = true, //SP/Haste - Chaotic
				        RedId = runed[1], YellowId = reckless[1], BlueId = purified[1], PrismaticId = runed[1], MetaId = chaotic },
						
				    new GemmingTemplate() { Model = "Warlock", Group = "Epic", //Max SP - Ember
				        RedId = runed[2], YellowId = runed[2], BlueId = runed[2], PrismaticId = runed[2], MetaId = ember },
				    new GemmingTemplate() { Model = "Warlock", Group = "Epic", //SP/Hit - Ember
				        RedId = runed[2], YellowId = veiled[2], BlueId = purified[2], PrismaticId = runed[2], MetaId = ember },
				    new GemmingTemplate() { Model = "Warlock", Group = "Epic", //SP/Haste - Ember
				        RedId = runed[2], YellowId = reckless[2], BlueId = purified[2], PrismaticId = runed[2], MetaId = ember },
				    new GemmingTemplate() { Model = "Warlock", Group = "Epic", //Max SP - Chaotic
				        RedId = runed[2], YellowId = runed[2], BlueId = runed[2], PrismaticId = runed[2], MetaId = chaotic },
				    new GemmingTemplate() { Model = "Warlock", Group = "Epic", //SP/Hit - Chaotic
				        RedId = runed[2], YellowId = veiled[2], BlueId = purified[2], PrismaticId = runed[2], MetaId = chaotic },
				    new GemmingTemplate() { Model = "Warlock", Group = "Epic", //SP/Haste - Chaotic
				        RedId = runed[2], YellowId = reckless[2], BlueId = purified[2], PrismaticId = runed[2], MetaId = chaotic },
						
				    new GemmingTemplate() { Model = "Warlock", Group = "Jeweler", //Max SP - Ember
				        RedId = runed[3], YellowId = runed[3], BlueId = runed[3], PrismaticId = runed[3], MetaId = ember },
				    new GemmingTemplate() { Model = "Warlock", Group = "Jeweler", //SP/Hit - Ember
				        RedId = runed[2], YellowId = runed[3], BlueId = runed[3], PrismaticId = runed[2], MetaId = ember },
				    new GemmingTemplate() { Model = "Warlock", Group = "Jeweler", //Max SP - Chaotic
				        RedId = runed[3], YellowId = runed[3], BlueId = runed[3], PrismaticId = runed[3], MetaId = chaotic },
				    new GemmingTemplate() { Model = "Warlock", Group = "Jeweler", //SP/Hit - Chaotic
				        RedId = runed[2], YellowId = runed[3], BlueId = runed[3], PrismaticId = runed[2], MetaId = chaotic },
				};
            }
        }

        public override CharacterClass TargetClass { get { return CharacterClass.Warlock; } }

        private string _currentChartName;
        private float _currentChartTotal;

        private Dictionary<string, System.Drawing.Color> _subPointNameColors;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors 
        {
            get 
            {
                _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                switch (_currentChartName) 
                {
                    case "Mana Sources": _subPointNameColors.Add(String.Format(CultureInfo.InvariantCulture, "Mana Sources ({0} Total)", _currentChartTotal.ToString("0", CultureInfo.InvariantCulture)), System.Drawing.Color.Blue); break;
                    case "DPS Sources": _subPointNameColors.Add(String.Format(CultureInfo.InvariantCulture, "DPS Sources ({0} total)", _currentChartTotal.ToString("0", CultureInfo.InvariantCulture)), System.Drawing.Color.Red); break;
                    case "Mana Usage": _subPointNameColors.Add(String.Format(CultureInfo.InvariantCulture, "Mana Usage ({0} total)", _currentChartTotal.ToString("0", CultureInfo.InvariantCulture)), System.Drawing.Color.Blue); break;
                    case "Haste Rating Gain": _subPointNameColors.Add(String.Format(CultureInfo.InvariantCulture, "DPS"), System.Drawing.Color.Red); break;
                    default:
                        _subPointNameColors.Add("DPS", System.Drawing.Color.Red);
                        _subPointNameColors.Add("Pet DPS", System.Drawing.Color.Blue);
                        break;
                }
                _currentChartName = null;
                return _subPointNameColors;
            }
        }

        private string[] _characterDisplayCalculationLabels;
        public override string[] CharacterDisplayCalculationLabels 
        {
            get 
            {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[] {
                    "Simulation:Rotation",
                    "Simulation:DPS",
                    "Simulation:Pet DPS",
                    "Simulation:Total DPS",
                    "Basic Stats:Health",
                    "Basic Stats:Mana",
                    "Basic Stats:Stamina",
                    "Basic Stats:Intellect",
                    "Basic Stats:Spirit",
                    "Basic Stats:Spell Power",
                    "Basic Stats:Regen",
                    "Basic Stats:Crit",
                    "Basic Stats:Miss Chance",
                    "Basic Stats:Haste",
                    "Shadow:Shadow Bolt",
                    "Shadow:Curse of Agony",
                    "Shadow:Curse of Doom",
                    "Shadow:Corruption",
                    "Shadow:Unstable Affliction",
                    "Shadow:Death Coil",
                    "Shadow:Drain Life",
                    "Shadow:Drain Soul",
                    "Shadow:Haunt",
                    "Shadow:Seed of Corruption",
                    "Shadow:Shadowflame",
                    "Shadow:Shadowburn",
                    "Shadow:Shadowfury",
                    "Fire:Incinerate",
                    "Fire:Immolate",
                    "Fire:Rain of Fire",
                    "Fire:Hellfire",
                    "Fire:Searing Pain",
                    "Fire:Soul Fire",
                    "Fire:Conflagrate",
                    "Fire:Chaos Bolt"
				};
                return _characterDisplayCalculationLabels;
            }
        }

        private CalculationOptionsPanelBase _calculationOptionsPanel;
        public override CalculationOptionsPanelBase CalculationOptionsPanel 
        {
            get 
            {
                if (_calculationOptionsPanel == null) 
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelWarlock();
                }
                return _calculationOptionsPanel;
            }
        }

        private string[] _optimizableCalculationLabels;
	    /// <summary>
	    /// Labels of the stats available to the Optimizer
	    /// </summary>
	    public override string[] OptimizableCalculationLabels 
        {
                get 
                {
                    if (_optimizableCalculationLabels == null)
                        _optimizableCalculationLabels = new string[] {"Miss chance",};
                    return _optimizableCalculationLabels;
                }
	    }

        private string[] _customChartNames;
        public override string[] CustomChartNames 
        {
            get 
            {
                if (_customChartNames == null) 
                {
                    _customChartNames = new string[] 
                    { 
                        "DPS Sources", 
                        "Mana Sources", 
                        "Mana Usage", 
                        //*"Glyphs",
                        "Haste Rating Gain" 
                    };
                }
                return _customChartNames;
            }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationWarlock(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsWarlock(); }

        private List<ItemType> _relevantItemTypes;
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
                        ItemType.Dagger,
                        ItemType.Wand,
                        ItemType.OneHandSword,
                        ItemType.Staff
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName) 
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            ComparisonCalculationBase comparison;

            _currentChartTotal = 0;
            _currentChartName = chartName;

            switch (chartName) 
            {
                case "Mana Sources":
                    CharacterCalculationsWarlock mscalcs = GetCharacterCalculations(character) as CharacterCalculationsWarlock;
                    Solver mssolver = mscalcs.GetSolver(character, mscalcs.BasicStats);
                    mssolver.Calculate(mscalcs);
                    foreach (Solver.ManaSource Source in mssolver.ManaSources) 
                    {
                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = Source.Name;
                        comparison.SubPoints[0] = (float)Source.Value;
                        _currentChartTotal += comparison.SubPoints[0];
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparison.Equipped = false;
                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                case "DPS Sources":
                    CharacterCalculationsWarlock dpscalcs = GetCharacterCalculations(character) as CharacterCalculationsWarlock;
                    Solver dpssolver = dpscalcs.GetSolver(character, dpscalcs.BasicStats);
                    dpssolver.Calculate(dpscalcs);
                    foreach (Spell spell in dpssolver.SpellPriority)
                    {
                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.SubPoints[0] = spell.SpellStatistics.DamageDone / (float)dpssolver.time;
                        _currentChartTotal += comparison.SubPoints[0];
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparison.Equipped = false;
                        comparisonList.Add(comparison);
                    }
                    if (dpssolver.CalculationOptions.Pet != "None")
                    {
                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = dpssolver.CalculationOptions.Pet;
                        comparison.SubPoints[0] = dpssolver.PetDPS;
                        _currentChartTotal += comparison.SubPoints[0];
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparison.Equipped = false;
                        comparisonList.Add(comparison);
                    }
                    if (dpssolver.TotalDPS - _currentChartTotal > 1)
                    {
                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = "Other";
                        comparison.SubPoints[0] = dpssolver.TotalDPS - _currentChartTotal;
                        _currentChartTotal += comparison.SubPoints[0];
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparison.Equipped = false;
                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                case "Mana Usage":
                    CharacterCalculationsWarlock mucalcs = GetCharacterCalculations(character) as CharacterCalculationsWarlock;
                    Solver musolver = mucalcs.GetSolver(character, mucalcs.BasicStats);
                    musolver.Calculate(mucalcs);
                    foreach (Spell spell in musolver.SpellPriority)
                    {
                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.SubPoints[0] = spell.SpellStatistics.ManaUsed;
                        _currentChartTotal += comparison.SubPoints[0];
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparison.Equipped = false;
                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                case "Haste Rating Gain":
                    CharacterCalculationsWarlock hrbase = GetCharacterCalculations(character) as CharacterCalculationsWarlock;
                    for (int x = 0; x < 100; x++)
                    {
                        CharacterCalculationsWarlock hrnew = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = x } }) as CharacterCalculationsWarlock;
                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = String.Format(CultureInfo.InvariantCulture, "{0} Haste Rating", x);
                        comparison.SubPoints[0] = hrnew.DpsPoints - hrbase.DpsPoints;
                        comparison.SubPoints[1] = hrnew.PetDPSPoints - hrbase.PetDPSPoints;
						comparison.OverallPoints = comparison.SubPoints[0] + comparison.SubPoints[1];
                        comparison.Equipped = false;
                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                default:
                    //_customChartNames = null;
                    _currentChartName = null;
                    return new ComparisonCalculationBase[0];
            }
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations) 
        {
            Stats stats = GetCharacterStats(character, additionalItem);
            CharacterCalculationsWarlock calculatedStats = new CharacterCalculationsWarlock();

            calculatedStats.Race = character.Race;
            calculatedStats.BasicStats = stats;
            calculatedStats.Character = character;

            calculatedStats.SpiritRegen = (float)Math.Floor(5f * StatConversion.GetSpiritRegenSec(calculatedStats.BasicStats.Spirit, calculatedStats.BasicStats.Intellect));
            calculatedStats.RegenInFSR = calculatedStats.SpiritRegen * calculatedStats.BasicStats.SpellCombatManaRegeneration + calculatedStats.BasicStats.Mp5;
            calculatedStats.RegenOutFSR = calculatedStats.SpiritRegen + calculatedStats.BasicStats.Mp5;

            Solver solver = calculatedStats.GetSolver(character, stats);
            solver.Calculate(calculatedStats);

            return calculatedStats;
        }

        public Stats GetBuffStats(Character character) { return GetBuffsStats(character.ActiveBuffs); }

        public override Stats GetCharacterStats(Character character, Item additionalItem) 
        {
            WarlockTalents talents = character.WarlockTalents;

            Stats statsBase = BaseStats.GetBaseStats(character);
            Stats statsItem = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTalents = new Stats() 
            {
                //Demonic Embrace: increases your stamina by 4/7/10%
                BonusStaminaMultiplier      = (talents.DemonicEmbrace == 1) ? 0.04f : (talents.DemonicEmbrace == 2) ? 0.07f : (talents.DemonicEmbrace == 3) ? 0.10f : 0f,
                
                //Fel Vitality: increases your maximum Health & Mana by 1/2/3%
                BonusHealthMultiplier       = (talents.FelVitality    * 0.01f),
                BonusManaMultiplier         = (talents.FelVitality    * 0.01f),
                
                //Malediction: increases your spell damage by 1/2/3%
                BonusSpellPowerMultiplier   = (talents.Malediction    * 0.01f),
                
                //Demonic Tactics: increases your spell crit chance by 2/4/6/8/10%
                //Backlash: increases your spell crit chance by 1/2/3%
                BonusCritChance             = (talents.DemonicTactics * 0.02f)
                                            + (talents.Backlash       * 0.01f),

                //Suppression: increases your chance to hit with spells by 1/2/3%
                SpellHit                    = (talents.Suppression * 0.01f),
            };
            
            Stats statsTotal = statsBase + statsItem + statsBuffs + statsTalents;

            //make sure that the bonus multipliers have been applied to each stat
            statsTotal.Stamina      = (float)Math.Floor(statsTotal.Stamina   * (1f + statsTotal.BonusStaminaMultiplier  ));
            statsTotal.Intellect    = (float)Math.Floor(statsTotal.Intellect * (1f + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit       = (float)Math.Floor(statsTotal.Spirit    * (1f + statsTotal.BonusSpiritMultiplier   ));

            //Health is calculated from stamina rating first, then its bonus multiplier (in this case, "Fel Vitality" talent) gets applied
            statsTotal.Health      += StatConversion.GetHealthFromStamina(statsTotal.Stamina);
            statsTotal.Health      *= (1 + statsTotal.BonusHealthMultiplier);

            //Mana is calculated from intellect rating first, then its bonus multiplier (in this case, "Expansive Mind" - Gnome racial) is applied
            statsTotal.Mana        += StatConversion.GetManaFromIntellect(statsTotal.Intellect);
            statsTotal.Mana        *= (1 + statsTotal.BonusManaMultiplier);
            
            //Crit rating - the MasterConjuror talent improves the firestone
            statsTotal.CritRating  += statsTotal.WarlockFirestoneSpellCritRating * (1f + (talents.MasterConjuror * 1.5f));
            statsTotal.SpellCrit   += StatConversion.GetSpellCritFromIntellect(statsTotal.Intellect);
            statsTotal.SpellCrit   += StatConversion.GetSpellCritFromRating(statsTotal.CritRating);
            statsTotal.SpellCrit   += statsTotal.BonusCritChance;

            //Haste rating - the MasterConjuror talent improves the spellstone
            statsTotal.HasteRating += statsTotal.WarlockSpellstoneHasteRating * (1f + (talents.MasterConjuror * 1.5f));
            statsTotal.SpellHaste  +=  StatConversion.GetSpellHasteFromRating(statsTotal.HasteRating);
            
            //Hit rating 
            statsTotal.SpellHit    += StatConversion.GetSpellHitFromRating(statsTotal.HitRating);

            if (statsTotal.WarlockFelArmor > 0) 
            {
                statsTotal.SpellDamageFromSpiritPercentage += 0.3f;
                statsTotal.SpellPower += 180f;
                statsTotal.Hp5 += statsTotal.Health * 0.02f;
                if (talents.DemonicAegis > 0) 
                {
                    statsTotal.SpellDamageFromSpiritPercentage += 0.09f;
                    statsTotal.SpellPower += 54f;
                    statsTotal.Hp5 += statsTotal.Health * 0.006f;
                }
            } 
            else if (statsTotal.WarlockDemonArmor > 0) 
            {
                statsTotal.Armor += 950f;
                statsTotal.HealingReceivedMultiplier += 0.2f;
                if (talents.DemonicAegis > 0) 
                {
                    statsTotal.Armor += 285f;
                    statsTotal.HealingReceivedMultiplier += 0.06f;
                }
            }
            
            statsTotal.SpellPower += (float)Math.Round(statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit);
            
            if (talents.DemonicKnowledge > 0) 
            {
                PetCalculations pet = new PetCalculations(statsTotal, character);
                statsTotal.SpellPower += (pet.petStats.Intellect + pet.petStats.Stamina) * talents.DemonicKnowledge * 0.04f;
            }

            return statsTotal;
        }

        public override Stats GetRelevantStats(Stats stats) 
        {
            Stats s = new Stats() 
            {
                Stamina = stats.Stamina,
                Health = stats.Health,
                Intellect = stats.Intellect,
                Mana = stats.Mana,
                Spirit = stats.Spirit,
                Mp5 = stats.Mp5,
                SpellPower = stats.SpellPower,
                SpellShadowDamageRating = stats.SpellShadowDamageRating,
                SpellFireDamageRating = stats.SpellFireDamageRating,
                SpellCritRating = stats.SpellCritRating,
                CritRating = stats.CritRating,
                SpellCrit = stats.SpellCrit,
                SpellHitRating = stats.SpellHitRating,
                HitRating = stats.HitRating,
                SpellHit = stats.SpellHit,
                SpellHasteRating = stats.SpellHasteRating,
                SpellHaste = stats.SpellHaste,
                HasteRating = stats.HasteRating,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                SpellDamageFromSpiritPercentage = stats.SpellDamageFromSpiritPercentage,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusManaPotion = stats.BonusManaPotion,
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusFireDamageMultiplier = stats.BonusFireDamageMultiplier,
                //ManaRestoreOnCast_5_15 = stats.ManaRestoreOnCast_5_15,
                ManaRestoreFromBaseManaPPM = stats.ManaRestoreFromBaseManaPPM,
                //SpellPowerFor15SecOnUse90Sec = stats.SpellPowerFor15SecOnUse90Sec,
                //SpellPowerFor15SecOnUse2Min = stats.SpellPowerFor15SecOnUse2Min,
                //SpellPowerFor20SecOnUse2Min = stats.SpellPowerFor20SecOnUse2Min,
                //HasteRatingFor20SecOnUse2Min = stats.HasteRatingFor20SecOnUse2Min,
                //HasteRatingFor20SecOnUse5Min = stats.HasteRatingFor20SecOnUse5Min,
                //SpellPowerFor10SecOnCast_15_45 = stats.SpellPowerFor10SecOnCast_15_45,
                //SpellPowerFor10SecOnHit_10_45 = stats.SpellPowerFor10SecOnHit_10_45,
                //SpellHasteFor10SecOnCast_10_45 = stats.SpellHasteFor10SecOnCast_10_45,
                //TimbalsProc = stats.TimbalsProc,
                //PendulumOfTelluricCurrentsProc = stats.PendulumOfTelluricCurrentsProc,
                //ExtractOfNecromanticPowerProc = stats.ExtractOfNecromanticPowerProc,
                //LightweaveEmbroideryProc = stats.LightweaveEmbroideryProc,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                CorruptionTriggersCrit = stats.CorruptionTriggersCrit,
                LifeTapBonusSpirit = stats.LifeTapBonusSpirit,
                Warlock2T8 = stats.Warlock2T8,
                Warlock4T8 = stats.Warlock4T8,
                Warlock2T9 = stats.Warlock2T9,
                Warlock4T9 = stats.Warlock4T9
            };
            
            foreach (SpecialEffect effect in stats.SpecialEffects()) 
            {
                if (effect.Trigger == Trigger.Use ||
                    effect.Trigger == Trigger.DamageSpellCast ||
                    effect.Trigger == Trigger.DamageSpellCrit ||
                    effect.Trigger == Trigger.DamageSpellHit ||
                    effect.Trigger == Trigger.SpellCast ||
                    effect.Trigger == Trigger.SpellCrit ||
                    effect.Trigger == Trigger.SpellHit ||
                    effect.Trigger == Trigger.SpellMiss ||
                    effect.Trigger == Trigger.DoTTick ||
                    effect.Trigger == Trigger.DamageDone)
                {
                    if (effect.Stats.SpellPower > 0 ||
                        effect.Stats.CritRating > 0 ||
                        effect.Stats.HasteRating > 0 ||
                        effect.Stats.HighestStat > 0 ||
                        effect.Stats.ShadowDamage > 0 ||
                        effect.Stats.Spirit > 0 ||
                        effect.Stats.Mp5 > 0)
                    {
                        s.AddSpecialEffect(effect);
                    }
                }
            }
            return s;
        }

        public override bool HasRelevantStats(Stats stats) 
        {
            foreach (SpecialEffect effect in stats.SpecialEffects()) 
            {
                if (effect.Trigger == Trigger.Use ||
                    effect.Trigger == Trigger.DamageSpellCast ||
                    effect.Trigger == Trigger.DamageSpellCrit ||
                    effect.Trigger == Trigger.DamageSpellHit ||
                    effect.Trigger == Trigger.SpellCast ||
                    effect.Trigger == Trigger.SpellCrit ||
                    effect.Trigger == Trigger.SpellHit ||
                    effect.Trigger == Trigger.SpellMiss ||
                    effect.Trigger == Trigger.DoTTick ||
                    effect.Trigger == Trigger.DamageDone)
                {
                    if (effect.Stats.SpellPower > 0 ||
                        effect.Stats.CritRating > 0 ||
                        effect.Stats.HasteRating > 0 ||
                        effect.Stats.HighestStat > 0 ||
                        effect.Stats.ShadowDamage > 0 ||
                        effect.Stats.Spirit > 0 ||
                        effect.Stats.Mp5 > 0)
                    {
                        return true;
                    }
                }
            }
            return (
                //stats.Stamina
                stats.Health
                + stats.Intellect
                + stats.Mana
                + stats.Spirit
                + stats.Mp5
                + stats.SpellPower
                + stats.SpellShadowDamageRating
                + stats.SpellFireDamageRating
                + stats.SpellCritRating
                + stats.CritRating
                + stats.SpellCrit
                + stats.SpellHitRating
                + stats.HitRating
                + stats.SpellHit
                + stats.SpellHasteRating
                + stats.SpellHaste
                + stats.HasteRating
                + stats.BonusSpiritMultiplier
                + stats.SpellDamageFromSpiritPercentage
                + stats.BonusIntellectMultiplier
                + stats.BonusManaPotion
                + stats.ThreatReductionMultiplier
                + stats.BonusDamageMultiplier
                + stats.BonusShadowDamageMultiplier
                + stats.BonusFireDamageMultiplier
                + stats.WarlockFelArmor
                + stats.WarlockDemonArmor
                + stats.WarlockSpellstoneDotDamageMultiplier
                + stats.WarlockSpellstoneHasteRating
                + stats.WarlockFirestoneDirectDamageMultiplier
                + stats.WarlockFirestoneSpellCritRating
                //+ stats.ManaRestoreOnCast_5_15
                + stats.ManaRestoreFromBaseManaPPM
                //+ stats.SpellPowerFor15SecOnUse90Sec
                //+ stats.SpellPowerFor15SecOnUse2Min
                //+ stats.SpellPowerFor20SecOnUse2Min
                //+ stats.HasteRatingFor20SecOnUse2Min
                //+ stats.HasteRatingFor20SecOnUse5Min
                //+ stats.SpellPowerFor10SecOnCast_15_45
                //+ stats.SpellPowerFor10SecOnHit_10_45
                //+ stats.SpellHasteFor10SecOnCast_10_45
                //+ stats.TimbalsProc
                //+ stats.PendulumOfTelluricCurrentsProc
                //+ stats.ExtractOfNecromanticPowerProc
                //+ stats.LightweaveEmbroideryProc
                + stats.BonusSpellCritMultiplier
                + stats.CorruptionTriggersCrit
                + stats.LifeTapBonusSpirit
                + stats.Warlock2T8
                + stats.Warlock4T8
                + stats.Warlock2T9
                + stats.Warlock4T9
                ) > 0;
        }

        #region RelevantGlyphs
        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs() 
        {
            if (_relevantGlyphs == null) 
            {
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of Chaos Bolt");
                _relevantGlyphs.Add("Glyph of Conflagrate");
                _relevantGlyphs.Add("Glyph of Corruption");
                _relevantGlyphs.Add("Glyph of Curse of Agony");
                _relevantGlyphs.Add("Glyph of Felguard");
                _relevantGlyphs.Add("Glyph of Haunt");
                _relevantGlyphs.Add("Glyph of Immolate");
                _relevantGlyphs.Add("Glyph of Imp");
                _relevantGlyphs.Add("Glyph of Incinerate");
                _relevantGlyphs.Add("Glyph of Life Tap");
                _relevantGlyphs.Add("Glyph of Metamorphosis");
                _relevantGlyphs.Add("Glyph of Searing Pain");
                _relevantGlyphs.Add("Glyph of Shadowbolt");
                _relevantGlyphs.Add("Glyph of Shadowburn");
                _relevantGlyphs.Add("Glyph of Siphon Life");
                _relevantGlyphs.Add("Glyph of Unstable Affliction");
            }
            return _relevantGlyphs;
        }
        #endregion

        public override ICalculationOptionBase DeserializeDataObject(string xml) 
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsWarlock));
            StringReader reader = new StringReader(xml);
            CalculationOptionsWarlock calcOpts = serializer.Deserialize(reader) as CalculationOptionsWarlock;
            return calcOpts;
        }

        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot) 
        {
            if (slot == ItemSlot.OffHand || slot == ItemSlot.Ranged) { return false; }
            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique) 
        {
            if (slot == CharacterSlot.OffHand && item.Slot == ItemSlot.OneHand) return false;
            return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
        }
    }
}