using System;
using System.Collections.Generic;

using Rawr;

namespace Rawr.Warlock
{
    [Rawr.Calculations.RawrModelInfo("Warlock", "Spell_Nature_FaerieFire", Character.CharacterClass.Warlock)]
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

        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Warlock; } }

        private string _currentChartName = null;
        private float _currentChartTotal = 0;

        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                switch (_currentChartName)
                {
                    case "Mana Sources":
                        _subPointNameColors.Add(string.Format("Mana Sources ({0} Total)", _currentChartTotal.ToString("0")), System.Drawing.Color.Blue);
                        break;
                    case "DPS Sources":
                        _subPointNameColors.Add(string.Format("DPS Sources ({0} total)", _currentChartTotal.ToString("0")), System.Drawing.Color.Red);
                        break;
                    case "Mana Usage":
                        _subPointNameColors.Add(string.Format("Mana Usage ({0} total)", _currentChartTotal.ToString("0")), System.Drawing.Color.Blue);
                        break;
                    case "Haste Rating Gain":
                        _subPointNameColors.Add(string.Format("DPS"), System.Drawing.Color.Red);
                        break;
                    default:
                        _subPointNameColors.Add("DPS", System.Drawing.Color.Red);
                        _subPointNameColors.Add("Pet DPS", System.Drawing.Color.Blue);
                        break;
                }
                _currentChartName = null;
                return _subPointNameColors;
            }
        }

        private string[] _characterDisplayCalculationLabels = null;
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

        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelWarlock();
                }
                return _calculationOptionsPanel;
            }
        }

        private string[] _optimizableCalculationLabels = null;
	/// <summary>
	/// Labels of the stats available to the Optimizer
	/// </summary>
	public override string[]  OptimizableCalculationLabels
	{
            get
	    {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {"Miss chance",};
                return _optimizableCalculationLabels;
            }
	}

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] { "DPS Sources", "Mana Sources", "Mana Usage", "Glyphs", "Haste Rating Gain" };
                return _customChartNames;
            }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationWarlock(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsWarlock(); }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get {
                if (_relevantItemTypes == null) {
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]{
                        Item.ItemType.None,
                        Item.ItemType.Cloth,
                        Item.ItemType.Dagger,
                        Item.ItemType.Wand,
                        Item.ItemType.OneHandSword,
                        Item.ItemType.Staff
                    });
                }
                return _relevantItemTypes;
            }
        }

        private static string[] GlyphList = { "GlyphChaosBolt", "GlyphConflag", "GlyphCorruption", "GlyphCoA", "GlyphFelguard", "GlyphHaunt", "GlyphImmolate", "GlyphImp", "GlyphIncinerate", "GlyphLifeTap", "GlyphMetamorphosis", "GlyphSearingPain", "GlyphSB", "GlyphShadowburn", "GlyphSiphonLife", "GlyphUA" };
        private static string[] GlyphListFriendly = { "Glyph of Chaos Bolt", "Glyph of Conflagrate", "Glyph of Corruption", "Glyph of Curse of Agony", "Glyph of Felguard", "Glyph of Haunt", "Glyph of Immolate", "Glyph of Imp", "Glyph of Incinerate", "Glyph of Life Tap", "Glyph of Metamorphosis", "Glyph of Searing Pain", "Glyph of Shadowbolt", "Glyph of Shadowburn", "Glyph of Siphon Life", "Glyph of Unstable Affliction" };

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
                case "Glyphs":
                    CalculationOptionsWarlock calcOpts = character.CalculationOptions as CalculationOptionsWarlock;
                    CharacterCalculationsWarlock glyphcalcs = GetCharacterCalculations(character) as CharacterCalculationsWarlock;

                    for (int index = 0; index < GlyphList.Length; index++)
                    {
                        string glyph = GlyphList[index];
                        bool glyphEnabled = calcOpts.GetGlyphByName(glyph);

                        if (glyphEnabled)
                        {
                            calcOpts.SetGlyphByName(glyph, false);
                            CharacterCalculationsWarlock calc = GetCharacterCalculations(character, null) as CharacterCalculationsWarlock;

                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = GlyphListFriendly[index];
                            comparison.Equipped = true;
                            comparison.SubPoints[0] = (glyphcalcs.DpsPoints - calc.DpsPoints);
                            comparison.SubPoints[1] = (glyphcalcs.PetDPSPoints - calc.PetDPSPoints);
                            comparison.OverallPoints = comparison.SubPoints[0] + comparison.SubPoints[1];
                            comparisonList.Add(comparison);
                        }
                        else
                        {
                            calcOpts.SetGlyphByName(glyph, true);
                            CharacterCalculationsWarlock calc = GetCharacterCalculations(character, null) as CharacterCalculationsWarlock;

                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = GlyphListFriendly[index];
                            comparison.Equipped = false;
                            comparison.SubPoints[0] = (calc.DpsPoints - glyphcalcs.DpsPoints);
                            comparison.SubPoints[1] = (calc.PetDPSPoints - glyphcalcs.PetDPSPoints);
                            comparison.OverallPoints = comparison.SubPoints[0] + comparison.SubPoints[1];
                            comparisonList.Add(comparison);
                        }
                        calcOpts.SetGlyphByName(glyph, glyphEnabled);
                    }
                    return comparisonList.ToArray();
                case "Haste Rating Gain":
                    CharacterCalculationsWarlock hrbase = GetCharacterCalculations(character) as CharacterCalculationsWarlock;
                    for (int x = 0; x < 100; x++)
                    {
                        CharacterCalculationsWarlock hrnew = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = x } }) as CharacterCalculationsWarlock;
                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = string.Format("{0} Haste Rating", x);
                        comparison.SubPoints[0] = hrnew.DpsPoints - hrbase.DpsPoints;
                        comparison.SubPoints[1] = hrnew.PetDPSPoints - hrbase.PetDPSPoints;
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

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            Stats stats = GetCharacterStats(character, additionalItem);
            Stats statsRace = GetRaceStats(character);
            CharacterCalculationsWarlock calculatedStats = new CharacterCalculationsWarlock();
            CalculationOptionsWarlock calculationOptions = character.CalculationOptions as CalculationOptionsWarlock;

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

        public static Stats GetRaceStats(Character character)
        {
            Stats stats = new Stats();
            if (character.Level >= 70 && character.Level <= 80)
            {
                stats.Health = 3471f + (character.Level - 70) * (7164 - 3471) / 10;
                stats.Mana = 2871 + (character.Level - 70) * (3856 - 2871) / 10;
            }
            else
            {
                stats.Mana = 2871;
                stats.Health = 3471f;
            }

//fix all. Undead should be correct
            switch (character.Race)
            {
                case Character.CharacterRace.NightElf:
                    if (character.Level == 80)
                    {
                        stats.Stamina = 66f;
                        stats.Intellect = 174f;
                        stats.Spirit = 181f;
                    }
                    else
                    {
                        stats.Stamina = 57f;
                        stats.Intellect = 145f;
                        stats.Spirit = 151f;
                    }
                    break;
                case Character.CharacterRace.Dwarf:
                    if (character.Level == 80)
                    {
                        stats.Stamina = 70f;
                        stats.Intellect = 173f;
                        stats.Spirit = 180f;
                    }
                    else
                    {
                        stats.Stamina = 61f;
                        stats.Intellect = 144f;
                        stats.Spirit = 150f;
                    }
                    break;
                case Character.CharacterRace.Draenei:
                    if (character.Level == 80)
                    {
                        stats.Stamina = 66f;
                        stats.Intellect = 175f;
                        stats.Spirit = 183f;
                    }
                    else
                    {
                        stats.Stamina = 57f;
                        stats.Intellect = 146f;
                        stats.Spirit = 153f;
                    }
                    break;
                case Character.CharacterRace.Human:
                    if (character.Level == 80)
                    {
                        stats.Stamina = 67f;
                        stats.Intellect = 174f;
                        stats.Spirit = 181f;
                    }
                    else
                    {
                        stats.Stamina = 58f;
                        stats.Intellect = 145f;
                        stats.Spirit = 152f;
                    }
                    stats.BonusSpiritMultiplier = 0.03f;
                    break;
                case Character.CharacterRace.BloodElf:
                    if (character.Level == 80)
                    {
                        stats.Stamina = 65f;
                        stats.Intellect = 178f;
                        stats.Spirit = 180f;
                    }
                    else
                    {
                        stats.Stamina = 56f;
                        stats.Intellect = 149f;
                        stats.Spirit = 150f;
                    }
                    break;
                case Character.CharacterRace.Troll:
                    if (character.Level == 80)
                    {
                        stats.Stamina = 68f;
                        stats.Intellect = 170f;
                        stats.Spirit = 182f;
                    }
                    else
                    {
                        stats.Stamina = 59f;
                        stats.Intellect = 141f;
                        stats.Spirit = 152f;
                    }
                    break;
                case Character.CharacterRace.Undead:
                    if (character.Level == 80)
                    {
                        stats.Stamina = 90f;
                        stats.Intellect = 157f;
                        stats.Spirit = 171f;
                    }
                    else
                    {
                        stats.Stamina = 76f;
                        stats.Intellect = 131f;
                        stats.Spirit = 144f;
                    }
                    break;
            }
            return stats;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            //Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTalents = new Stats()
            {
                BonusStaminaMultiplier = character.WarlockTalents.DemonicEmbrace * 0.02f,
                BonusHealthMultiplier = character.WarlockTalents.FelVitality * 0.01f,
                BonusManaMultiplier = character.WarlockTalents.FelVitality * 0.01f,
                BonusSpellPowerMultiplier = character.WarlockTalents.Malediction * 0.03f,
//Demonic Resilience
//Demonic Knowledge
                BonusCritChance = character.WarlockTalents.DemonicTactics * 0.02f + character.WarlockTalents.Backlash * 0.01f,
            };

            Stats statsTotal = statsBaseGear + statsBuffs + statsRace + statsTalents;

            statsTotal.Stamina = (float)Math.Floor((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.Mana += (statsTotal.Intellect - 20f) * 15f + 20f;
            statsTotal.Health += statsTotal.Stamina * 10f;
            statsTotal.SpellCrit += StatConversion.GetSpellCritFromIntellect(statsTotal.Intellect)
                                 + StatConversion.GetSpellCritFromRating(statsTotal.CritRating + (statsTotal.WarlockGrandFirestone * 49) * (1 + character.WarlockTalents.MasterConjuror * 1.5f))
                                 + 0.01701f;
            statsTotal.HasteRating += (statsTotal.WarlockGrandSpellstone * 60) * (1 + character.WarlockTalents.MasterConjuror * 1.5f);
            statsTotal.SpellHaste += StatConversion.GetSpellHasteFromRating(statsTotal.HasteRating);
            statsTotal.SpellHit += (StatConversion.GetSpellHitFromRating(statsTotal.HitRating) + character.WarlockTalents.Suppression * 0.01f);
            if (statsTotal.WarlockFelArmor > 0)
            {
                statsTotal.SpellDamageFromSpiritPercentage += 0.3f;
                statsTotal.SpellPower += 180;
                statsTotal.Hp5 += statsTotal.Health * 0.02f;
                if (character.WarlockTalents.DemonicAegis > 0)
                {
                    statsTotal.SpellDamageFromSpiritPercentage += 0.09f;
                    statsTotal.SpellPower += 54;
                    statsTotal.Hp5 += statsTotal.Health * 0.006f;
                }
            }
            else if (statsTotal.WarlockDemonArmor > 0)
            {
                statsTotal.Armor += 950;
                statsTotal.HealingReceivedMultiplier += 0.2f;
                if (character.WarlockTalents.DemonicAegis > 0)
                {
                    statsTotal.Armor += 285;
                    statsTotal.HealingReceivedMultiplier += 0.06f;
                }
            }
            statsTotal.SpellPower += (float)Math.Round(statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit);
            if (character.WarlockTalents.DemonicKnowledge > 0)
            {
                PetCalculations pet = new PetCalculations(statsTotal, character);
                statsTotal.SpellPower += (pet.petStats.Intellect + pet.petStats.Stamina) * character.WarlockTalents.DemonicKnowledge * 0.04f;
            }

            return statsTotal;
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
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
                ManaRestoreOnCast_5_15 = stats.ManaRestoreOnCast_5_15,
                ManaRestoreFromBaseManaPerHit = stats.ManaRestoreFromBaseManaPerHit,
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
                LightweaveEmbroideryProc = stats.LightweaveEmbroideryProc,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                CorruptionTriggersCrit = stats.CorruptionTriggersCrit,
                LifeTapBonusSpirit = stats.LifeTapBonusSpirit,
                Warlock2T8 = stats.Warlock2T8,
                Warlock4T8 = stats.Warlock4T8
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (
//                  stats.Stamina
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
                + stats.WarlockGrandSpellstone
                + stats.WarlockGrandFirestone
                + stats.ManaRestoreOnCast_5_15
                + stats.ManaRestoreFromBaseManaPerHit
                + stats.SpellPowerFor15SecOnUse90Sec
                + stats.SpellPowerFor15SecOnUse2Min
                + stats.SpellPowerFor20SecOnUse2Min
                + stats.HasteRatingFor20SecOnUse2Min
                + stats.HasteRatingFor20SecOnUse5Min
                + stats.SpellPowerFor10SecOnCast_15_45
                + stats.SpellPowerFor10SecOnHit_10_45
                + stats.SpellHasteFor10SecOnCast_10_45
                + stats.TimbalsProc
                + stats.PendulumOfTelluricCurrentsProc
                + stats.ExtractOfNecromanticPowerProc
                + stats.LightweaveEmbroideryProc
                + stats.BonusSpellCritMultiplier
                + stats.CorruptionTriggersCrit
                + stats.LifeTapBonusSpirit
                + stats.Warlock2T8
                + stats.Warlock4T8
                ) > 0;
        }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsWarlock));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsWarlock calcOpts = serializer.Deserialize(reader) as CalculationOptionsWarlock;
            return calcOpts;
        }

        public override bool EnchantFitsInSlot(Enchant enchant, Character character, Item.ItemSlot slot)
        {
            if (slot == Item.ItemSlot.OffHand || slot == Item.ItemSlot.Ranged) return false;
            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        public override bool ItemFitsInSlot(Item item, Character character, Character.CharacterSlot slot)
        {
            if (slot == Character.CharacterSlot.OffHand && item.Slot == Item.ItemSlot.OneHand) return false;
            return base.ItemFitsInSlot(item, character, slot);
        }
    }
}