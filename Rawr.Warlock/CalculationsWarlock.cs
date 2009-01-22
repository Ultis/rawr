using System;
using System.Collections.Generic;

using Rawr;

namespace Rawr.Warlock
{
    [Rawr.Calculations.RawrModelInfo("Warlock", "Spell_Nature_FaerieFire", Character.CharacterClass.Warlock)]
    public class CalculationsWarlock : CalculationsBase
    {
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
                        _subPointNameColors.Add(string.Format("MP5 Sources ({0} Total)", _currentChartTotal.ToString("0")), System.Drawing.Color.Blue);
                        break;
                    case "DPS Sources":
                        _subPointNameColors.Add(string.Format("DPS Sources ({0} total)", _currentChartTotal.ToString("0")), System.Drawing.Color.Red);
                        break;
                    case "Mana Usage":
                        _subPointNameColors.Add(string.Format("Mana Usage ({0} total)", _currentChartTotal.ToString("0")), System.Drawing.Color.Blue);
                        break;
                    case "Haste Rating Gain":
                        _subPointNameColors.Add(string.Format("DPS-Burst"), System.Drawing.Color.Red);
                        _subPointNameColors.Add(string.Format("DPS-Sustained"), System.Drawing.Color.Blue);
                        break;
                    default:
                        _subPointNameColors.Add("DPS-Burst", System.Drawing.Color.Red);
                        _subPointNameColors.Add("DPS-Sustained", System.Drawing.Color.Blue);
                        _subPointNameColors.Add("Survivability", System.Drawing.Color.Green);
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
					"Basic Stats:Health",
                    "Basic Stats:Resilience",
					"Basic Stats:Mana",
					"Basic Stats:Stamina",
					"Basic Stats:Intellect",
					"Basic Stats:Spirit",
					"Basic Stats:Spell Power",
					"Basic Stats:Regen",
					"Basic Stats:Crit",
					"Basic Stats:Hit",
					"Basic Stats:Haste",
                    "Simulation:Rotation",
                    "Simulation:Burst DPS",
                    "Simulation:Sustained DPS",
                    "Shadow:Shadow Bolt",
                    "Shadow:Curse of Agony",
                    "Shadow:Curse of Doom",
                    "Shadow:Corruption",
                    "Shadow:Siphon Life",
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

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] { "Stat Values", "Mana Sources", "DPS Sources", "Mana Usage", "Haste Rating Gain" };
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
                    SolverBase mssolver = mscalcs.GetSolver(character, mscalcs.BasicStats);
                    mssolver.Calculate(mscalcs);
                    foreach (SolverBase.ManaSource Source in mssolver.ManaSources)
                    {
                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = Source.Name;
                        comparison.SubPoints[0] = Source.Value * 5;
                        _currentChartTotal += comparison.SubPoints[0];
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparison.Equipped = false;
                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                case "DPS Sources":
                    CharacterCalculationsWarlock dpscalcs = GetCharacterCalculations(character) as CharacterCalculationsWarlock;
                    SolverBase dpssolver = dpscalcs.GetSolver(character, dpscalcs.BasicStats);
                    dpssolver.Calculate(dpscalcs);
                    foreach (Spell spell in dpssolver.SpellPriority)
                    {
                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.SubPoints[0] = spell.SpellStatistics.DamageDone;
                        _currentChartTotal += comparison.SubPoints[0];
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparison.Equipped = false;
                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                case "Mana Usage":
                    CharacterCalculationsWarlock mucalcs = GetCharacterCalculations(character) as CharacterCalculationsWarlock;
                    SolverBase musolver = mucalcs.GetSolver(character, mucalcs.BasicStats);
                    musolver.Calculate(mucalcs);
                    foreach (Spell spell in musolver.SpellPriority)
                    {
                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.SubPoints[0] = spell.SpellStatistics.ManaUsed * 5;
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
                        comparison.Name = string.Format("{0} Haste Rating", x);
                        comparison.SubPoints[0] = hrnew.DpsPoints - hrbase.DpsPoints;
                        comparison.SubPoints[1] = hrnew.SustainPoints - hrbase.SustainPoints;
                        comparison.Equipped = false;
                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                case "Stat Values":
                    CharacterCalculationsWarlock calcsBase = GetCharacterCalculations(character) as CharacterCalculationsWarlock;
                    CharacterCalculationsWarlock calcsIntellect = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = 50 } }) as CharacterCalculationsWarlock;
                    CharacterCalculationsWarlock calcsSpirit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Spirit = 50 } }) as CharacterCalculationsWarlock;
                    CharacterCalculationsWarlock calcsMP5 = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Mp5 = 50 } }) as CharacterCalculationsWarlock;
                    CharacterCalculationsWarlock calcsSpellPower = GetCharacterCalculations(character, new Item() { Stats = new Stats() { SpellPower = 50 } }) as CharacterCalculationsWarlock;
                    CharacterCalculationsWarlock calcsHaste = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 50 } }) as CharacterCalculationsWarlock;
                    CharacterCalculationsWarlock calcsCrit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 50 } }) as CharacterCalculationsWarlock;
                    CharacterCalculationsWarlock calcsHit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 50 } }) as CharacterCalculationsWarlock;
                    CharacterCalculationsWarlock calcsSta = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = 50 } }) as CharacterCalculationsWarlock;
                    CharacterCalculationsWarlock calcsRes = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Resilience = 50 } }) as CharacterCalculationsWarlock;

                    return new ComparisonCalculationBase[] {
                        new ComparisonCalculationWarlock() { Name = "1 Intellect",
                            OverallPoints = (calcsIntellect.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsIntellect.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsIntellect.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsIntellect.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationWarlock() { Name = "1 Spirit",
                            OverallPoints = (calcsSpirit.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsSpirit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsSpirit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsSpirit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationWarlock() { Name = "1 MP5",
                            OverallPoints = (calcsMP5.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsMP5.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsMP5.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsMP5.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationWarlock() { Name = "1 Spell Power",
                            OverallPoints = (calcsSpellPower.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsSpellPower.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsSpellPower.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsSpellPower.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationWarlock() { Name = "1 Haste",
                            OverallPoints = (calcsHaste.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsHaste.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsHaste.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsHaste.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationWarlock() { Name = "1 Crit",
                            OverallPoints = (calcsCrit.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsCrit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsCrit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsCrit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationWarlock() { Name = "1 Hit",
                            OverallPoints = (calcsHit.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsHit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsHit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsHit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationWarlock() { Name = "1 Stamina",
                            OverallPoints = (calcsSta.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsSta.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsSta.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsSta.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationWarlock() { Name = "1 Resilience",
                            OverallPoints = (calcsRes.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsRes.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsRes.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsRes.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        }};
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

            calculatedStats.SpiritRegen = (float)Math.Floor(5f * character.StatConversion.GetSpiritRegenSec(calculatedStats.BasicStats.Spirit, calculatedStats.BasicStats.Intellect));
            calculatedStats.RegenInFSR = calculatedStats.SpiritRegen * calculatedStats.BasicStats.SpellCombatManaRegeneration + calculatedStats.BasicStats.Mp5;
            calculatedStats.RegenOutFSR = calculatedStats.SpiritRegen + calculatedStats.BasicStats.Mp5;

            SolverBase solver = calculatedStats.GetSolver(character, stats);
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
            Stats statsEnchants = GetEnchantsStats(character);
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

            Stats statsTotal = statsBaseGear + statsEnchants + statsBuffs + statsRace + statsTalents;

            statsTotal.Stamina = (float)Math.Floor((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.Mana += (statsTotal.Intellect - 20f) * 15f + 20f;
            statsTotal.Health += statsTotal.Stamina * 10f;
            statsTotal.SpellCrit += character.StatConversion.GetSpellCritFromIntellect(statsTotal.Intellect) / 100f
                + character.StatConversion.GetSpellCritFromRating(statsTotal.CritRating + statsTotal.WarlockGrandFirestone * 49) / 100f
                + 0.01701f;
            statsTotal.SpellHaste += character.StatConversion.GetSpellHasteFromRating(statsTotal.HasteRating + statsTotal.WarlockGrandSpellstone * 60) / 100f;
            statsTotal.SpellHit += character.StatConversion.GetSpellHitFromRating(statsTotal.HitRating) / 100f;
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

            return statsTotal;
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Stamina = stats.Stamina,
                Health = stats.Health,
                Resilience = stats.Resilience,
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
//add talent/glyph modifiers or is it set bonuses?
/*                BonusDiseaseDamageMultiplier = stats.BonusDiseaseDamageMultiplier,
                SWPDurationIncrease = stats.SWPDurationIncrease,
                BonusMindBlastMultiplier = stats.BonusMindBlastMultiplier,
                MindBlastCostReduction = stats.MindBlastCostReduction,
                ShadowWordDeathCritIncrease = stats.ShadowWordDeathCritIncrease,
                WeakenedSoulDurationDecrease = stats.WeakenedSoulDurationDecrease,*/
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
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (
//                  stats.Stamina
                stats.Health
                + stats.Resilience
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
                + stats.BonusSpellCritMultiplier
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
    }
}

/*using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Warlock
{
	[Rawr.Calculations.RawrModelInfo("Warlock", "Spell_Nature_FaerieFire", Character.CharacterClass.Warlock)]
	class CalculationsWarlock : CalculationsBase
    {
        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        /// <summary>
        /// Dictionary<string, Color> that includes the names of each rating which your model will use,
        /// and a color for each. These colors will be used in the charts.
        /// 
        /// EXAMPLE:
        /// subPointNameColors = new Dictionary<string, System.Drawing.Color>();
        /// subPointNameColors.Add("Mitigation", System.Drawing.Colors.Red);
        /// subPointNameColors.Add("Survival", System.Drawing.Colors.Blue);
        /// </summary>
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get {
                
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("DPS", System.Drawing.Color.Blue);
                }
                return _subPointNameColors;
                }
        }


        private string[] _characterDisplayCalculationLabels = null;
        /// <summary>
        /// An array of strings which will be used to build the calculation display.
        /// Each string must be in the format of "Heading:Label". Heading will be used as the
        /// text of the group box containing all labels that have the same Heading.
        /// Label will be the label of that calculation, and may be appended with '*' followed by
        /// a description of that calculation which will be displayed in a tooltip for that label.
        /// Label (without the tooltip string) must be unique.
        /// 
        /// EXAMPLE:
        /// characterDisplayCalculationLabels = new string[]
        /// {
        ///		"Basic Stats:Health",
        ///		"Basic Stats:Armor",
        ///		"Advanced Stats:Dodge",
        ///		"Advanced Stats:Miss*Chance to be missed"
        /// };
        /// </summary>
        public override string[] CharacterDisplayCalculationLabels
        {
            get 
            {
                if (_characterDisplayCalculationLabels == null)
                {
                    List<string> labels = new List<string>(new string[]
                    {
                        "Basic Stats:Health",
                        "Basic Stats:Mana",
                        "Basic Stats:Stamina",
                        "Basic Stats:Intellect",
                        "Basic Stats:Spirit",
                        "Spell Stats:Total Crit %",
                        "Spell Stats:Hit %",
                        "Spell Stats:Haste %",
                        //"Spell Stats:Casting Speed",
                        "Spell Stats:Shadow Damage*Includes trinket and proc effects",
                        "Spell Stats:Fire Damage*Includes trinket and proc effects",
                        "Overall Stats:ISB Uptime", 
                        "Overall Stats:RDPS from ISB*Raid DPS loss from switching to Fire",
                        "Overall Stats:Total Damage", 
                        "Overall Stats:DPS",
                        "Shadowbolt Stats:SB Min Hit",
                        "Shadowbolt Stats:SB Max Hit",
                        "Shadowbolt Stats:SB Min Crit",
                        "Shadowbolt Stats:SB Max Crit",
                        "Shadowbolt Stats:SB Average Hit",
                        "Shadowbolt Stats:SB Crit Rate",
                        "Shadowbolt Stats:ISB Uptime",
                        "Shadowbolt Stats:#SB Casts",
                        "Incinerate Stats:Incinerate Min Hit",
                        "Incinerate Stats:Incinerate Max Hit",
                        "Incinerate Stats:Incinerate Min Crit",
                        "Incinerate Stats:Incinerate Max Crit",
                        "Incinerate Stats:Incinerate Average Hit",
                        "Incinerate Stats:Incinerate Crit Rate",
                        "Incinerate Stats:#Incinerate Casts",
                        "Immolate Stats:ImmolateMin Hit",
                        "Immolate Stats:ImmolateMax Hit",
                        "Immolate Stats:ImmolateMin Crit",
                        "Immolate Stats:ImmolateMax Crit",
                        "Immolate Stats:ImmolateAverage Hit",
                        "Immolate Stats:ImmolateCrit Rate",
                        "Immolate Stats:#Immolate Casts",
                        "Curse of Agony Stats:CoA Tick",
                        "Curse of Agony Stats:CoA Total Damage",
                        "Curse of Agony Stats:#CoA Casts",
                        "Curse of Doom Stats:CoD Total Damage",
                        "Curse of Doom Stats:#CoD Casts",
                        "Corruption Stats:Corr Tick",
                        "Corruption Stats:Corr Total Damage",
                        "Corruption Stats:#Corr Casts",
                        "Unstable Affliction Stats:UA Tick",
                        "Unstable Affliction Stats:UA Total Damage",
                        "Unstable Affliction Stats:#UA Casts",
                        "SiphonLife Stats:SL Tick",
                        "SiphonLife Stats:SL Total Damage",
                        "SiphonLife Stats:#SL Casts",
                        "Lifetap Stats:#Lifetaps",
                        "Lifetap Stats:Mana Per LT"

                    });
                    _characterDisplayCalculationLabels = labels.ToArray();
                }
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] { "Stats vs Spell Damage", "Stats (Item Budget)", "Talent Specs" };
                return _customChartNames;
            }
        }


        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelWarlock()); }
        }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
					{
						Item.ItemType.None,
						Item.ItemType.Cloth,
						Item.ItemType.Dagger,
						Item.ItemType.OneHandSword,
						Item.ItemType.Staff,
						Item.ItemType.Wand,
					});
                }
                return _relevantItemTypes;
            }
        }

	public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Warlock; } }
	public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationWarlock();
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsWarlock();
        }


		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsWarlock));
			System.IO.StringReader reader = new System.IO.StringReader(xml);
			CalculationOptionsWarlock calcOpts = serializer.Deserialize(reader) as CalculationOptionsWarlock;
			return calcOpts;
		}

        public static float ChanceToHit(float targetLevel, float hitPercent)
        {
            return Math.Min(1, ((targetLevel <= 82) ? (0.96f - (targetLevel - 80) * 0.01f) : (0.94f - (targetLevel - 82) * 0.11f)) + 0.01f * hitPercent);
        }

        public static void LoadTalentCode(Character character, string talentCode)
        {
            //http://www.worldofwarcraft.com/info/classes/warlock/talents.html?tal=000000000000000000000000000000000000000000000000000000000000000000000000000000000
            if (talentCode == null || talentCode.Length < 81)
                return;

            CalculationOptionsWarlock calculationOptions = character.CalculationOptions as CalculationOptionsWarlock;

            calculationOptions.ImprovedCurseOfAgony = int.Parse(talentCode.Substring(0, 1));
            calculationOptions.Suppression = int.Parse(talentCode.Substring(1, 1));
            calculationOptions.ImprovedCorruption = int.Parse(talentCode.Substring(2, 1));
            calculationOptions.Frailty = int.Parse(talentCode.Substring(3, 1));
            calculationOptions.ImprovedDrainSoul = int.Parse(talentCode.Substring(4, 1));
            calculationOptions.ImprovedLifeTap = int.Parse(talentCode.Substring(5, 1));
            calculationOptions.SoulSiphon = int.Parse(talentCode.Substring(6, 1));
            calculationOptions.ImprovedFear = int.Parse(talentCode.Substring(7, 1));
            calculationOptions.FelConcentration = int.Parse(talentCode.Substring(8, 1));
            calculationOptions.AmplifyCurse = int.Parse(talentCode.Substring(9, 1));
            calculationOptions.GrimReach = int.Parse(talentCode.Substring(10, 1));
            calculationOptions.Nightfall = int.Parse(talentCode.Substring(11, 1));
            calculationOptions.EmpoweredCorruption = int.Parse(talentCode.Substring(12, 1));
            calculationOptions.ShadowEmbrace = int.Parse(talentCode.Substring(13, 1));
            calculationOptions.SiphonLife = int.Parse(talentCode.Substring(14, 1));
            calculationOptions.CurseOfExhaustion = int.Parse(talentCode.Substring(15, 1));
            calculationOptions.ImprovedFelhunter = int.Parse(talentCode.Substring(16, 1));
            calculationOptions.ShadowMastery = int.Parse(talentCode.Substring(17, 1));
            calculationOptions.Eradication = int.Parse(talentCode.Substring(18, 1));
            calculationOptions.Contagion = int.Parse(talentCode.Substring(19, 1));
            calculationOptions.DarkPact = int.Parse(talentCode.Substring(20, 1));
            calculationOptions.ImprovedHowlOfTerror = int.Parse(talentCode.Substring(21, 1));
            calculationOptions.Malediction = int.Parse(talentCode.Substring(22, 1));
            calculationOptions.DeathsEmbrace = int.Parse(talentCode.Substring(23, 1));
            calculationOptions.UnstableAffliction = int.Parse(talentCode.Substring(24, 1));
            calculationOptions.Pandemic = int.Parse(talentCode.Substring(25, 1));
            calculationOptions.EverlastingAffliction = int.Parse(talentCode.Substring(26, 1));
            calculationOptions.Haunt = int.Parse(talentCode.Substring(27, 1));
            calculationOptions.ImprovedHealthstone = int.Parse(talentCode.Substring(28, 1));
            calculationOptions.ImprovedImp = int.Parse(talentCode.Substring(29, 1));
            calculationOptions.DemonicEmbrace = int.Parse(talentCode.Substring(30, 1));
            calculationOptions.ImprovedHealthFunnel = int.Parse(talentCode.Substring(31, 1));
            calculationOptions.DemonicBrutality = int.Parse(talentCode.Substring(32, 1));
            calculationOptions.FelVitality = int.Parse(talentCode.Substring(33, 1));
            calculationOptions.ImprovedSuccubus = int.Parse(talentCode.Substring(34, 1));
            calculationOptions.SoulLink = int.Parse(talentCode.Substring(35, 1));
            calculationOptions.FelDomination = int.Parse(talentCode.Substring(36, 1));
            calculationOptions.DemonicAegis = int.Parse(talentCode.Substring(37, 1));
            calculationOptions.UnholyPower = int.Parse(talentCode.Substring(38, 1));
            calculationOptions.MasterSummoner = int.Parse(talentCode.Substring(39, 1));
            calculationOptions.DemonicSacrifice = int.Parse(talentCode.Substring(40, 1));
            calculationOptions.MasterConjuror = int.Parse(talentCode.Substring(41, 1));
            calculationOptions.ManaFeed = int.Parse(talentCode.Substring(42, 1));
            calculationOptions.MasterDemonologist = int.Parse(talentCode.Substring(43, 1));
            calculationOptions.ImprovedEnslaveDemon = int.Parse(talentCode.Substring(44, 1));
            calculationOptions.DemonicResilience = int.Parse(talentCode.Substring(45, 1));
            calculationOptions.DemonicEmpowerment = int.Parse(talentCode.Substring(46, 1));
            calculationOptions.DemonicKnowledge = int.Parse(talentCode.Substring(47, 1));
            calculationOptions.DemonicTactics = int.Parse(talentCode.Substring(48, 1));
            calculationOptions.FelSynergy = int.Parse(talentCode.Substring(49, 1));
            calculationOptions.ImprovedDemonicTactics = int.Parse(talentCode.Substring(50, 1));
            calculationOptions.SummonFelguard = int.Parse(talentCode.Substring(51, 1));
            calculationOptions.DemonicEmpathy = int.Parse(talentCode.Substring(52, 1));
            calculationOptions.DemonicPact = int.Parse(talentCode.Substring(53, 1));
            calculationOptions.Metamorphosis = int.Parse(talentCode.Substring(54, 1));
            calculationOptions.ImprovedShadowBolt = int.Parse(talentCode.Substring(55, 1));
            calculationOptions.Bane = int.Parse(talentCode.Substring(56, 1));
            calculationOptions.Aftermath = int.Parse(talentCode.Substring(57, 1));
            calculationOptions.MoltenCore = int.Parse(talentCode.Substring(58, 1));
            calculationOptions.Cataclysm = int.Parse(talentCode.Substring(59, 1));
            calculationOptions.DemonicPower = int.Parse(talentCode.Substring(60, 1));
            calculationOptions.Shadowburn = int.Parse(talentCode.Substring(61, 1));
            calculationOptions.Ruin = int.Parse(talentCode.Substring(62, 1));
            calculationOptions.Intensity = int.Parse(talentCode.Substring(63, 1));
            calculationOptions.DestructiveReach = int.Parse(talentCode.Substring(64, 1));
            calculationOptions.ImprovedSearingPain = int.Parse(talentCode.Substring(65, 1));
            calculationOptions.Pyroclasm = int.Parse(talentCode.Substring(66, 1));
            calculationOptions.ImprovedImmolate = int.Parse(talentCode.Substring(67, 1));
            calculationOptions.Devastation = int.Parse(talentCode.Substring(68, 1));
            calculationOptions.NetherProtection = int.Parse(talentCode.Substring(69, 1));
            calculationOptions.Emberstorm = int.Parse(talentCode.Substring(70, 1));
            calculationOptions.Conflagrate = int.Parse(talentCode.Substring(71, 1));
            calculationOptions.SoulLeech = int.Parse(talentCode.Substring(72, 1));
            calculationOptions.Backlash = int.Parse(talentCode.Substring(73, 1));
            calculationOptions.ShadowAndFlame = int.Parse(talentCode.Substring(74, 1));
            calculationOptions.ImprovedSoulLeech = int.Parse(talentCode.Substring(75, 1));
            calculationOptions.Backdraft = int.Parse(talentCode.Substring(76, 1));
            calculationOptions.Shadowfury = int.Parse(talentCode.Substring(77, 1));
            calculationOptions.EmpoweredImp = int.Parse(talentCode.Substring(78, 1));
            calculationOptions.FireAndBrimstone = int.Parse(talentCode.Substring(79, 1));
            calculationOptions.ChaosBolt = int.Parse(talentCode.Substring(80, 1));
        }

        public static void LoadTalentSpec(Character character, string talentSpec)
        {
            string talentCode = String.Empty;
            CalculationOptionsWarlock options = character.CalculationOptions as CalculationOptionsWarlock;
            switch (talentSpec)
            {
                case "UA Affliction (43/0/18)":
                    talentCode = "235002200102351005351033135100000000000000000000000000005023005000000000000000000";
                    if (options != null)
                    {
                        options.CastUnstableAffliction = true;
                        options.CastCorruption = true;
                        options.CastImmolate = true;
                        options.CastSiphonLife = true;
                        options.FillerSpell = FillerSpell.Shadowbolt;
                        options.PetSacrificed = false;
                        options.Pet = Pet.Felhunter;
                    }
                    break;
//not updated
                case "Ruin Affliction (40/0/21)":
                    talentCode = "0502210502035105510300000000000000000000000505000512200010000000";
                    if(options != null)
                    {
                        options.CastUnstableAffliction = false;
                        options.CastCorruption = true;
                        options.CastImmolate = true;
                        options.CastSiphonLife = true;
                        options.FillerSpell = FillerSpell.Shadowbolt;
                        options.PetSacrificed = false;
                        options.Pet = Pet.Imp;
                    }
                    break;
//not updated
                case "Shadow Destro (0/21/40)":
                    talentCode = "0000000000000000000002050031133200100000000555000512210013030250";
                    if (options != null)
                    {
                        options.CastUnstableAffliction = false;
                        options.CastCorruption = false;
                        options.CastImmolate = false;
                        options.CastSiphonLife = false;
                        options.FillerSpell = FillerSpell.Shadowbolt;
                        options.PetSacrificed = true;
                        options.Pet = Pet.Succubus;
                    }
                    break;
//not updated
                case "Fire Destro (0/21/40)":
                    talentCode = "0000000000000000000002050031133200100000000055000512200510530150";
                    if (options != null)
                    {
                        options.CastUnstableAffliction = false;
                        options.CastCorruption = false;
                        options.CastImmolate = true;
                        options.CastSiphonLife = false;
                        options.FillerSpell = FillerSpell.Incinerate;
                        options.PetSacrificed = true;
                        options.Pet = Pet.Imp;
                    }
                    break;
            }

            LoadTalentCode(character, talentCode);
        }
        
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
        /// <returns>A custom CharacterCalculations object which inherits from CharacterCalculationsBase,
        /// containing all of the final calculations defined in CharacterDisplayCalculationLabels. See
        /// CharacterCalculationsBase comments for more details.</returns>
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            CharacterCalculationsWarlock calculations = new CharacterCalculationsWarlock();
            calculations.BasicStats = GetCharacterStats(character, additionalItem);
			calculations.CalculationOptions = character.CalculationOptions as CalculationOptionsWarlock;

            int targetLevel = calculations.CalculationOptions.TargetLevel;
            calculations.HitPercent = calculations.BasicStats.HitRating / 26.23f;
            calculations.CritPercent = calculations.BasicStats.CritRating / 45.91f;
            calculations.HastePercent = calculations.BasicStats.HasteRating / 32.79f;
            calculations.GlobalCooldown = 1.5f / (1 + 0.01f * calculations.HastePercent);
            if (calculations.GlobalCooldown < 1)
                calculations.GlobalCooldown = 1;
            calculations.ShadowDamage = calculations.BasicStats.SpellPower + calculations.BasicStats.SpellShadowDamageRating;
            calculations.FireDamage = calculations.BasicStats.SpellPower + calculations.BasicStats.SpellFireDamageRating;

            calculations.SpellRotation = new WarlockSpellRotation(calculations);
            calculations.SpellRotation.Calculate(false);
            //calculations.SpellRotation.CalculateAdvancedInfo();

            Stats totalStats = calculations.BasicStats;
            //T4 2 piece bonus
            totalStats.SpellShadowDamageRating += totalStats.BonusWarlockSchoolDamageOnCast * (1 - (float)Math.Pow(0.95, 15 * calculations.SpellRotation.ShadowSpellsPerSecond));
            totalStats.SpellFireDamageRating += totalStats.BonusWarlockSchoolDamageOnCast * (1 - (float)Math.Pow(0.95, 15 * calculations.SpellRotation.FireSpellsPerSecond));

            //Spellstrike 2 piece bonus
            totalStats.SpellPower += totalStats.SpellDamageFor10SecOnHit_5 * (1 - (float)Math.Pow(0.95, 10 * calculations.SpellRotation.SpellsPerSecond));
            
            //Quagmirran's Eye
            totalStats.HasteRating += totalStats.SpellHasteFor6SecOnHit_10_45 * 6 / (45 + 9 / calculations.SpellRotation.SpellsPerSecond);

            //Band of the Eternal Sage
            totalStats.SpellPower += totalStats.SpellPowerFor10SecOnHit_10_45 * 10 / (45 + 9 / calculations.SpellRotation.SpellsPerSecond);

            calculations.HastePercent = totalStats.HasteRating / 32.79f;
            calculations.GlobalCooldown = 1.5f / (1 + 0.01f * calculations.HastePercent);
            if (calculations.GlobalCooldown < 1)
                calculations.GlobalCooldown = 1;
            calculations.ShadowDamage = totalStats.SpellPower + totalStats.SpellShadowDamageRating;
            calculations.FireDamage = totalStats.SpellPower + totalStats.SpellFireDamageRating;

            float dps = calculations.SpellRotation.Calculate(true);
            //calculations.SpellRotation.CalculateDps();

            calculations.TotalDamage = (float)Math.Round(dps * calculations.CalculationOptions.FightDuration);
            calculations.SubPoints = new float[] { dps };
            calculations.OverallPoints = dps;

            return calculations;
        }


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
        /// <returns>A Stats object containing the final totaled values of all character stats.</returns>
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.Orc:
                    statsRace = new Stats()
                    {
                        Health = 3310f,
                        Mana = 2335f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 100f,
                        Intellect = 100f,
                        Spirit = 144,
                    };
                    break;
                case Character.CharacterRace.BloodElf:
                    statsRace = new Stats()
                    {
                        Health = 3310f,
                        Mana = 2335f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 100f,
                        Intellect = 100f,
                        Spirit = 144,
                    };
                    break;
                case Character.CharacterRace.Gnome:
                    statsRace = new Stats()
                    {
                        Health = 3310f,
                        Mana = 2335f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 75f,
                        Intellect = 135f,
                        Spirit = 145,
                        ArcaneResistance = 10,
                        BonusIntellectMultiplier = .05f 
                    };
                    break;
                case Character.CharacterRace.Human:
                    statsRace = new Stats()
                    {
                        Health = 3310f,
                        Mana = 2335f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 76f,
                        Intellect = 129f,
                        Spirit = 145,
                        BonusSpiritMultiplier = 0.03f
                    };
                    break;
                case Character.CharacterRace.Troll:
                    statsRace = new Stats()
                    {
                        Health = 3310f,
                        Mana = 2335f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 52f,
                        Intellect = 147f,
                        Spirit = 146
                    };
                    break;
                case Character.CharacterRace.Undead:
                    statsRace = new Stats()
                    {
                        Health = 8121f,
                        Mana = 5931f,
                        Strength = 58f,
                        Agility = 65f,
                        Stamina = 90f,
                        Intellect = 157f,
                        Spirit = 171f
                    };
                    break;
                default:
                    statsRace = new Stats();
                    break;
            }

            CalculationOptionsWarlock options = character.CalculationOptions as CalculationOptionsWarlock;
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;

            Stats statsTotal = statsGearEnchantsBuffs + statsRace;

			statsTotal.NatureResistance = statsGearEnchantsBuffs.NatureResistance + statsRace.NatureResistance + statsGearEnchantsBuffs.NatureResistanceBuff;
			statsTotal.FireResistance = statsGearEnchantsBuffs.FireResistance + statsRace.FireResistance + statsGearEnchantsBuffs.FireResistanceBuff;
			statsTotal.FrostResistance = statsGearEnchantsBuffs.FrostResistance + statsRace.FrostResistance + statsGearEnchantsBuffs.FrostResistanceBuff;
			statsTotal.ShadowResistance = statsGearEnchantsBuffs.ShadowResistance + statsRace.ShadowResistance + statsGearEnchantsBuffs.ShadowResistanceBuff;
			statsTotal.ArcaneResistance = statsGearEnchantsBuffs.ArcaneResistance + statsRace.ArcaneResistance + statsGearEnchantsBuffs.ArcaneResistanceBuff;
			statsTotal.AllResist = statsGearEnchantsBuffs.AllResist + statsRace.AllResist;
            

            statsTotal.BonusSpellPowerMultiplier += 1;
            statsTotal.BonusShadowDamageMultiplier += 1;
            statsTotal.BonusFireDamageMultiplier += 1;

            //strength
            statsTotal.Strength = (float)Math.Floor((Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier)) + statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier)) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier));

            //agility
            statsTotal.Agility = (float)Math.Floor((Math.Floor(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier)) + statsGearEnchantsBuffs.Agility * (1 + statsRace.BonusAgilityMultiplier)) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier));

            //intellect
            statsTotal.Intellect = (float)Math.Floor((Math.Floor(statsRace.Intellect * (1 + statsRace.BonusIntellectMultiplier)) + statsGearEnchantsBuffs.Intellect * (1 + statsRace.BonusIntellectMultiplier)) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier));

            //stamina
            statsTotal.Stamina = (float)Math.Floor((Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier)) + statsGearEnchantsBuffs.Stamina * (1 + statsRace.BonusStaminaMultiplier)) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier));
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1 + 0.02f * options.DemonicEmbrace));

            //spirit
            statsTotal.Spirit = (float)Math.Floor((Math.Floor(statsRace.Spirit * (1 + statsRace.BonusSpiritMultiplier)) + statsGearEnchantsBuffs.Spirit * (1 + statsRace.BonusSpiritMultiplier)) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier));

            //spell damage
            statsTotal.SpellPower += statsTotal.SpellPowerFor20SecOnUse2Min * 20 / 120;
            statsTotal.SpellPower += statsTotal.SpellPowerFor15SecOnUse90Sec * 15 / 90;
            statsTotal.SpellPower += statsTotal.SpellPowerFor15SecOnUse2Min * 15 / 120;
            statsTotal.SpellPower += 180 + options.DemonicAegis * 18; //assume Fel Armor
            statsTotal.SpellPower += statsTotal.Spirit * 0.3f;

            //spell crit rating
//1.701 should be the base crit of a level 80 Lock
            statsTotal.CritRating += (1.701f + statsTotal.Intellect / 166.6667f + 2 * options.DemonicTactics + options.Backlash + options.Devastation) * 45.91f;

            //spell haste rating
//Drums of Battle update needed
            statsTotal.HasteRating += statsTotal.DrumsOfBattle * 30 / 120;
            statsTotal.HasteRating += statsTotal.HasteRatingFor20SecOnUse2Min * 20 / 120;

            //mp5
//Mana Pots are 1 per fight
            //statsTotal.Mp5 += 100; //Assume Super Mana Potions
//Should be converted to Replenishment
            //statsTotal.Mp5 += options.ShadowPriestDps * 0.05f * 5 * 0.95f;

            //health
            statsTotal.Health = (float)Math.Round(statsRace.Health + statsGearEnchantsBuffs.Health + (statsTotal.Stamina * 10f));
            statsTotal.Health = (float)Math.Round(statsTotal.Health * (1 + 0.01f * options.FelVitality));

            //mana
            statsTotal.Mana = (float)Math.Round(statsRace.Mana + 15f * statsTotal.Intellect + statsGearEnchantsBuffs.Mana);
            statsTotal.Mana = (float)Math.Round(statsTotal.Mana * (1 + 0.01f * options.FelVitality));

            //armor
            statsTotal.Armor = (float)Math.Round(statsGearEnchantsBuffs.Armor + statsTotal.Agility * 2);

            if (!options.PetSacrificed)
            {
                switch (options.Pet)
                {
                    case Pet.Imp:
                        statsTotal.BonusFireDamageMultiplier *= 1 + (0.01f * options.MasterDemonologist);
//                        statsTotal.BonusFireCritMultiplier *= 1 + (0.01f * options.MasterDemonologist);
                        break;
                    case Pet.Succubus:
                        statsTotal.BonusShadowDamageMultiplier *= 1 + (0.01f * options.MasterDemonologist);
//                        statsTotal.BonusShadowCritMultiplier *= 1 + (0.01f * options.MasterDemonologist);
                        break;
                    //case Pet.Felhunter:
                    //    statsTotal.SpellDamageTakenMultiplier *= (0.2f * options.MasterDemonologist);
                    //    break;
                    //case Pet.Voidwalker:
                    //    statsTotal.PhysicalDamageTakenMultiplier *= (0.2f * options.MasterDemonologist);
                    //    break;
                    case Pet.Felguard:
                        statsTotal.BonusSpellPowerMultiplier *= 1 + (0.01f * options.MasterDemonologist);
                    //    statsTotal.DamageTakenMultiplier += (0.1f * options.MasterDemonologist);
                        break;
                }
            }
            else if (options.DemonicSacrifice == 1)
            {
                switch (options.Pet)
                {
                    case Pet.Imp:
                        statsTotal.BonusFireDamageMultiplier *= 1 + 0.10f;
                        break;
                    case Pet.Succubus:
                        statsTotal.BonusShadowDamageMultiplier *= 1 + 0.10f;
                        break;
                    case Pet.Felhunter:
                        statsTotal.Mp5 += statsTotal.Mana * 0.03f;
                        break;
                    case Pet.Felguard:
                        statsTotal.BonusShadowDamageMultiplier *= 1 + 0.07f;
                        statsTotal.Mp5 += statsTotal.Mana * 0.02f;
                        break;
                    case Pet.Voidwalker:
                        statsTotal.Hp5 += statsTotal.Health * 0.02f;
                        break;
                }
            }

            //Emberstorm
            statsTotal.BonusFireDamageMultiplier *= 1 + 0.03f * options.Emberstorm;

            //Shadow Mastery
            statsTotal.BonusShadowDamageMultiplier *= 1 + 0.03f * options.ShadowMastery;

            //Soul Link
//Not implemented atm
            //statsTotal.PetDamageTaken *= 1 + options.SoulLink * 0.2f;

//Demonic Knowledge
//TODO: Add pet stats and make Demonic Knowledge model correctly
//int demonicKnowledge = tree.GetTalent("DemonicKnowledge").PointsInvested;

            //statsTotal.SpellDamageRating *= 1f + 0.05f * pointsInDemonicKnowledge;
            //statsTotal.SpellShadowDamageRating *= 1f + 0.05f * pointsInDemonicKnowledge;
            //statsTotal.SpellFireDamageRating *= 1f + 0.05f * pointsInDemonicKnowledge;

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            CharacterCalculationsWarlock baseCalc, currentCalc, calc;
            ComparisonCalculationBase comparison;
            float[] subPoints;
            Item[] itemList;
            string[] statList;

            switch (chartName)
            {
                case "Stats vs Spell Damage":
                    itemList = new Item[] 
                    {
                        new Item() { Stats = new Stats() { SpellPower = 1 } },
                        new Item() { Stats = new Stats() { SpellShadowDamageRating = 1 } }, 
                        new Item() { Stats = new Stats() { SpellFireDamageRating = 1 } }, 
                        new Item() { Stats = new Stats() { CritRating = 1 } },
                        new Item() { Stats = new Stats() { HasteRating = 1 } },
                        new Item() { Stats = new Stats() { HitRating = 1 } },
                        new Item() { Stats = new Stats() { Mp5 = 1 } }
                    };
                    statList = new string[] 
                    {
                        "1 Spell Damage", 
                        "1 Shadow Damage", 
                        "1 Fire Damage", 
                        "1 Spell Crit Rating", 
                        "1 Spell Haste Rating", 
                        "1 Spell Hit Rating",
                        "1 Mana per 5 sec"
                    };

                    baseCalc = GetCharacterCalculations(character) as CharacterCalculationsWarlock;

                    //get values relative to spell damage
                    calc = GetCharacterCalculations(character, itemList[0]) as CharacterCalculationsWarlock;
                    ComparisonCalculationBase spellDamageComparison = CreateNewComparisonCalculation();
                    spellDamageComparison.Name = statList[0];
                    spellDamageComparison.Equipped = false;
                    spellDamageComparison.OverallPoints = calc.OverallPoints - baseCalc.OverallPoints;
                    subPoints = new float[calc.SubPoints.Length];
                    for (int i = 0; i < calc.SubPoints.Length; i++)
                    {
                        subPoints[i] = calc.SubPoints[i] - baseCalc.SubPoints[i];
                    }
                    spellDamageComparison.SubPoints = subPoints;
                    comparisonList.Add(spellDamageComparison);

                    for (int index = 1; index < statList.Length; index++)
                    {
                        calc = GetCharacterCalculations(character, itemList[index]) as CharacterCalculationsWarlock;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = statList[index];
                        comparison.Equipped = false;
                        comparison.OverallPoints = (calc.OverallPoints - baseCalc.OverallPoints) / spellDamageComparison.OverallPoints;
                        subPoints = new float[calc.SubPoints.Length];
                        for (int i = 0; i < calc.SubPoints.Length; i++)
                        {
                            subPoints[i] = (calc.SubPoints[i] - baseCalc.SubPoints[i]) / spellDamageComparison.SubPoints[i];
                        }
                        comparison.SubPoints = subPoints;

                        comparisonList.Add(comparison);
                    }

                    return comparisonList.ToArray();
                case "Stats (Item Budget)":
                    itemList = new Item[] 
                    {
                        new Item() { Stats = new Stats() { SpellPower = 11.7f } },
                        new Item() { Stats = new Stats() { SpellShadowDamageRating = 14.3f } }, 
                        new Item() { Stats = new Stats() { SpellFireDamageRating = 14.3f } }, 
                        new Item() { Stats = new Stats() { CritRating = 10 } },
                        new Item() { Stats = new Stats() { HasteRating = 10 } },
                        new Item() { Stats = new Stats() { HitRating = 10 } },
                        new Item() { Stats = new Stats() { Mp5 = 4 } }
                    };
                    statList = new string[] 
                    {
                        "11.7 Spell Damage", 
                        "14.3 Shadow Damage", 
                        "14.3 Fire Damage", 
                        "10 Spell Crit Rating", 
                        "10 Spell Haste Rating", 
                        "10 Spell Hit Rating",
                        "4 Mana per 5 sec"
                    };

                    baseCalc = GetCharacterCalculations(character) as CharacterCalculationsWarlock;

                    for (int index = 0; index < statList.Length; index++)
                    {
                        calc = GetCharacterCalculations(character, itemList[index]) as CharacterCalculationsWarlock;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = statList[index];
                        comparison.Equipped = false;
                        comparison.OverallPoints = calc.OverallPoints - baseCalc.OverallPoints;
                        subPoints = new float[calc.SubPoints.Length];
                        for (int i = 0; i < calc.SubPoints.Length; i++)
                        {
                            subPoints[i] = calc.SubPoints[i] - baseCalc.SubPoints[i];
                        }
                        comparison.SubPoints = subPoints;

                        comparisonList.Add(comparison);
                    }

                    return comparisonList.ToArray();
                case "Talent Specs":
                    currentCalc = GetCharacterCalculations(character) as CharacterCalculationsWarlock;
                    comparison = CreateNewComparisonCalculation();
                    comparison.Name = "Current";
                    comparison.Equipped = true;
                    comparison.OverallPoints = currentCalc.OverallPoints;
                    subPoints = new float[currentCalc.SubPoints.Length];
                    for (int i = 0; i < currentCalc.SubPoints.Length; i++)
                    {
                        subPoints[i] = currentCalc.SubPoints[i];
                    }
                    comparison.SubPoints = subPoints;

                    comparisonList.Add(comparison);

                    Character charClone = character.Clone();
                    string[] talentSpecList = { "UA Affliction (43/0/18)", "Ruin Affliction (40/0/21)", "Shadow Destro (0/21/40)", "Fire Destro (0/21/40)" };
                    CalculationOptionsWarlock calculations = charClone.CalculationOptions as CalculationOptionsWarlock;
                    charClone.CalculationOptions = calculations.Clone();

                    for (int index = 0; index < talentSpecList.Length; index++)
                    {
                        LoadTalentSpec(charClone, talentSpecList[index]);

                        calc = GetCharacterCalculations(charClone) as CharacterCalculationsWarlock;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = talentSpecList[index];
                        comparison.Equipped = false;
                        comparison.OverallPoints = calc.OverallPoints;
                        subPoints = new float[calc.SubPoints.Length];
                        for (int i = 0; i < calc.SubPoints.Length; i++)
                        {
                            subPoints[i] = calc.SubPoints[i];
                        }
                        comparison.SubPoints = subPoints;

                        comparisonList.Add(comparison);
                    }

                    return comparisonList.ToArray();
                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                AllResist = stats.AllResist,
                ArcaneResistance = stats.ArcaneResistance,
                FireResistance = stats.FireResistance,
                FrostResistance = stats.FrostResistance,
                NatureResistance = stats.NatureResistance,
                 ShadowResistance = stats.ShadowResistance,
		 ArcaneResistanceBuff = stats.ArcaneResistanceBuff,
		 NatureResistanceBuff = stats.NatureResistanceBuff,
		 FireResistanceBuff = stats.FireResistanceBuff,
		 FrostResistanceBuff = stats.FrostResistanceBuff,
		 ShadowResistanceBuff = stats.ShadowResistanceBuff,
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                DodgeRating = stats.DodgeRating,
                Health = stats.Health,
                Mp5 = stats.Mp5,
                Resilience = stats.Resilience,
                CritRating = stats.CritRating,
                SpellPower = stats.SpellPower,
                SpellFireDamageRating = stats.SpellFireDamageRating,
                SpellShadowDamageRating = stats.SpellShadowDamageRating,
                SpellArcaneDamageRating = stats.SpellArcaneDamageRating,
                SpellFrostDamageRating = stats.SpellFrostDamageRating,
                SpellPenetration = stats.SpellPenetration,
                HasteRating = stats.HasteRating,
                HitRating = stats.HitRating,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                Mana = stats.Mana,
                Armor = stats.Armor,
                Hp5 = stats.Hp5,
                SpellCombatManaRegeneration = stats.SpellCombatManaRegeneration,
                BonusShadowDamageMultiplier = stats.BonusAgilityMultiplier, 
                BonusArcaneDamageMultiplier = stats.BonusArcaneDamageMultiplier,
                BonusFireDamageMultiplier = stats.BonusFireDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                BonusWarlockNukeMultiplier = stats.BonusWarlockNukeMultiplier, 
                LightningCapacitorProc = stats.LightningCapacitorProc,
                TimbalsProc = stats.TimbalsProc, 
                SpellPowerFor20SecOnUse2Min = stats.SpellPowerFor20SecOnUse2Min,
                HasteRatingFor20SecOnUse2Min = stats.HasteRatingFor20SecOnUse2Min,
                Mp5OnCastFor20SecOnUse2Min = stats.Mp5OnCastFor20SecOnUse2Min,
                ManaRestorePerCast = stats.ManaRestorePerCast,
                ManaRestoreFromMaxManaPerHit = stats.ManaRestoreFromMaxManaPerHit,
                SpellPowerFor10SecOnHit_10_45 = stats.SpellPowerFor10SecOnHit_10_45,
                SpellDamageFromIntellectPercentage = stats.SpellDamageFromIntellectPercentage,
                SpellDamageFromSpiritPercentage = stats.SpellDamageFromSpiritPercentage,
                SpellPowerFor10SecOnResist = stats.SpellPowerFor10SecOnResist,
                SpellPowerFor15SecOnCrit_20_45 = stats.SpellPowerFor15SecOnCrit_20_45,
                SpellPowerFor15SecOnUse90Sec = stats.SpellPowerFor15SecOnUse90Sec,
                SpellPowerFor15SecOnUse2Min = stats.SpellPowerFor15SecOnUse2Min, 
                SpellHasteFor6SecOnCast_15_45 = stats.SpellHasteFor6SecOnCast_15_45,
                SpellDamageFor10SecOnHit_5 = stats.SpellDamageFor10SecOnHit_5,
                SpellHasteFor6SecOnHit_10_45 = stats.SpellHasteFor6SecOnHit_10_45,
                SpellPowerFor10SecOnCrit_20_45 = stats.SpellPowerFor10SecOnCrit_20_45,
                BonusManaPotion = stats.BonusManaPotion,
                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            float warlockStats = stats.Stamina
                + stats.Intellect
                + stats.Spirit
                + stats.Mp5
                + stats.CritRating
                + stats.HasteRating
                + stats.HitRating
                + stats.SpellPower
                + stats.SpellFireDamageRating
                + stats.SpellShadowDamageRating
                + stats.BonusStaminaMultiplier
                + stats.BonusIntellectMultiplier
                + stats.BonusSpellCritMultiplier
                + stats.BonusSpellPowerMultiplier
                + stats.BonusFireDamageMultiplier
                + stats.BonusShadowDamageMultiplier
                + stats.BonusWarlockNukeMultiplier
                + stats.BonusSpiritMultiplier
                + stats.SpellPenetration
                + stats.Mana
                + stats.Health
                + stats.LightningCapacitorProc
                + stats.SpellPowerFor20SecOnUse2Min
                + stats.HasteRatingFor20SecOnUse2Min
                + stats.Mp5OnCastFor20SecOnUse2Min
                + stats.ManaRestoreFromMaxManaPerHit
                + stats.SpellPowerFor10SecOnHit_10_45
                + stats.SpellDamageFromSpiritPercentage
                + stats.SpellPowerFor10SecOnResist
                + stats.SpellPowerFor15SecOnCrit_20_45
                + stats.SpellPowerFor15SecOnUse90Sec
                + stats.SpellHasteFor6SecOnCast_15_45
                + stats.SpellDamageFor10SecOnHit_5
                + stats.SpellHasteFor6SecOnHit_10_45
                + stats.SpellPowerFor10SecOnCrit_20_45
                + stats.SpellPowerFor15SecOnUse2Min
                + stats.TimbalsProc
                + stats.BonusManaPotion
                + stats.ThreatReductionMultiplier
                + stats.AllResist
                + stats.ArcaneResistance
                + stats.FireResistance
                + stats.FrostResistance
                + stats.NatureResistance
		 + stats.ShadowResistance
		 + stats.ArcaneResistanceBuff
		 + stats.FireResistanceBuff
		 + stats.FrostResistanceBuff
		 + stats.NatureResistanceBuff
		 + stats.ShadowResistanceBuff
                + stats.Bloodlust
                + stats.DrumsOfBattle
                + stats.DrumsOfWar;
            
            return warlockStats > 0;
        }

        public float CalculateISBUptime(CharacterCalculationsWarlock calculations)
        {
            float chanceToHit = CalculationsWarlock.ChanceToHit(calculations.CalculationOptions.TargetLevel, calculations.HitPercent);
            float myDirectShadowHitsPerSecond = calculations.SpellRotation.ShadowBoltCastRatio / calculations.SpellRotation.ShadowBoltCastTime * chanceToHit;
            float myEffectiveCritRate = myDirectShadowHitsPerSecond * calculations.CritPercent;

            float totalIsbUptime = 1 - (float)Math.Pow(myEffectiveCritRate, 4);

            return totalIsbUptime;
        }
    }
}
*/