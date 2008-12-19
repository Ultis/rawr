using System;
using System.Collections.Generic;

using Rawr;

namespace Rawr.ShadowPriest
{
    [Rawr.Calculations.RawrModelInfo("ShadowPriest", "Spell_Shadow_Shadowform", Character.CharacterClass.Priest)]
    public class CalculationsShadowPriest : CalculationsBase 
    {
        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Priest; } }

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
                    "Simulation:DPS",
                    "Simulation:SustainDPS",
                    "Shadow:Vampiric Touch",
                    "Shadow:SW Pain",
                    "Shadow:Devouring Plague",
				    "Shadow:SW Death",
                    "Shadow:Mind Blast",
                    "Shadow:Mind Flay",
                    "Shadow:Vampiric Embrace",
                    "Holy:PW Shield",
                    "Holy:Smite",
                    "Holy:Holy Fire",
                    "Holy:Penance"
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
                    _calculationOptionsPanel = new CalculationOptionsPanelShadowPriest();
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

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationShadowPriest(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsShadowPriest(); }

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
                        Item.ItemType.OneHandMace,
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
                    CharacterCalculationsShadowPriest mscalcs = GetCharacterCalculations(character) as CharacterCalculationsShadowPriest;
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
                    CharacterCalculationsShadowPriest dpscalcs = GetCharacterCalculations(character) as CharacterCalculationsShadowPriest;
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
                    CharacterCalculationsShadowPriest mucalcs = GetCharacterCalculations(character) as CharacterCalculationsShadowPriest;
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
                    CharacterCalculationsShadowPriest hrbase = GetCharacterCalculations(character) as CharacterCalculationsShadowPriest;
                    for (int x = 0; x < 100; x++)
                    {
                        CharacterCalculationsShadowPriest hrnew = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = x } }) as CharacterCalculationsShadowPriest;
                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = string.Format("{0} Haste Rating", x);
                        comparison.SubPoints[0] = hrnew.DpsPoints - hrbase.DpsPoints;
                        comparison.SubPoints[1] = hrnew.SustainPoints - hrbase.SustainPoints;
                        comparison.Equipped = false;
                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                case "Stat Values":
                    CharacterCalculationsShadowPriest calcsBase = GetCharacterCalculations(character) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsIntellect = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsSpirit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Spirit = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsMP5 = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Mp5 = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsSpellPower = GetCharacterCalculations(character, new Item() { Stats = new Stats() { SpellPower = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsHaste = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsCrit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsHit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsSta = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsRes = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Resilience = 50 } }) as CharacterCalculationsShadowPriest;

                    return new ComparisonCalculationBase[] {
                        new ComparisonCalculationShadowPriest() { Name = "1 Intellect",
                            OverallPoints = (calcsIntellect.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsIntellect.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsIntellect.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsIntellect.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Spirit",
                            OverallPoints = (calcsSpirit.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsSpirit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsSpirit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsSpirit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 MP5",
                            OverallPoints = (calcsMP5.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsMP5.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsMP5.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsMP5.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Spell Power",
                            OverallPoints = (calcsSpellPower.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsSpellPower.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsSpellPower.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsSpellPower.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Haste",
                            OverallPoints = (calcsHaste.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsHaste.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsHaste.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsHaste.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Crit",
                            OverallPoints = (calcsCrit.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsCrit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsCrit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsCrit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Hit",
                            OverallPoints = (calcsHit.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsHit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsHit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsHit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Stamina",
                            OverallPoints = (calcsSta.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsSta.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsSta.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsSta.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Resilience",
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
            CharacterCalculationsShadowPriest calculatedStats = new CharacterCalculationsShadowPriest();
            CalculationOptionsShadowPriest calculationOptions = character.CalculationOptions as CalculationOptionsShadowPriest;
            
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
                stats.Health = 3211f + (character.Level - 70) * (6960 - 3211) / 10;
                stats.Mana = 2620 + (character.Level - 70) * (3863 - 2620) / 10;
            }
            else
            {
                stats.Mana = 2620;
                stats.Health = 3211f;
            }

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
                        stats.Stamina = 68f;
                        stats.Intellect = 172f;
                        stats.Spirit = 186f;
                    }
                    else
                    {
                        stats.Stamina = 59f;
                        stats.Intellect = 143f;
                        stats.Spirit = 156f;
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
                BonusStaminaMultiplier = character.PriestTalents.Enlightenment * 0.01f,
                BonusSpiritMultiplier = (1 + character.PriestTalents.Enlightenment * 0.01f) * (1f + character.PriestTalents.SpiritOfRedemption * 0.05f) - 1f,
                BonusIntellectMultiplier = character.PriestTalents.MentalStrength * 0.03f,
                SpellDamageFromSpiritPercentage = character.PriestTalents.SpiritualGuidance * 0.05f + character.PriestTalents.TwistedFaith * 0.02f,
                SpellHaste = character.PriestTalents.Enlightenment * 0.01f,
                SpellCombatManaRegeneration = character.PriestTalents.Meditation * 0.1f
            };

            Stats statsTotal = statsBaseGear + statsEnchants + statsBuffs + statsRace + statsTalents;

            statsTotal.Stamina = (float)Math.Floor((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.SpellPower += (float)Math.Round(statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit
                + GetInnerFireSpellPowerBonus(character));
            statsTotal.Mana += (statsTotal.Intellect - 20f) * 15f + 20f;
            statsTotal.Health += statsTotal.Stamina * 10f;
            statsTotal.SpellCrit += character.StatConversion.GetSpellCritFromIntellect(statsTotal.Intellect) / 100f
                + character.StatConversion.GetSpellCritFromRating(statsTotal.CritRating) / 100f
                + 0.0124f;
            statsTotal.SpellHaste += character.StatConversion.GetSpellHasteFromRating(statsTotal.HasteRating) / 100f;
            statsTotal.SpellHit += character.StatConversion.GetSpellHitFromRating(statsTotal.HitRating) / 100f;

            return statsTotal;
        }

        public static float GetInnerFireSpellPowerBonus(Character character)
        {
            float InnerFireSpellPowerBonus = 0;
            if (character.Level >= 77)
                InnerFireSpellPowerBonus = 120;
            else if (character.Level >= 71)
                InnerFireSpellPowerBonus = 95;
            return InnerFireSpellPowerBonus * (1f + character.PriestTalents.ImprovedInnerFire * 0.15f);
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
                BonusHolyDamageMultiplier = stats.BonusHolyDamageMultiplier,
                BonusDiseaseDamageMultiplier = stats.BonusDiseaseDamageMultiplier,
                ManaRestoreOnCast_5_15 = stats.ManaRestoreOnCast_5_15,
                ManaRestoreFromMaxManaPerHit = stats.ManaRestoreFromMaxManaPerHit,
                SpellPowerFor15SecOnUse90Sec = stats.SpellPowerFor15SecOnUse90Sec,
                SpellPowerFor15SecOnUse2Min = stats.SpellPowerFor15SecOnUse2Min,
                SpellPowerFor20SecOnUse2Min = stats.SpellPowerFor20SecOnUse2Min,
                HasteRatingFor20SecOnUse2Min = stats.HasteRatingFor20SecOnUse2Min,
                HasteRatingFor20SecOnUse5Min = stats.HasteRatingFor20SecOnUse5Min,
                SpellPowerFor10SecOnCast_15_45 = stats.SpellPowerFor10SecOnCast_15_45,
                SpellPowerFor10SecOnHit_10_45 = stats.SpellPowerFor10SecOnHit_10_45,
                TimbalsProc = stats.TimbalsProc,
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
                + stats.BonusHolyDamageMultiplier
                + stats.BonusDiseaseDamageMultiplier
                + stats.ManaRestoreOnCast_5_15
                + stats.ManaRestoreFromMaxManaPerHit
                + stats.SpellPowerFor15SecOnUse90Sec
                + stats.SpellPowerFor15SecOnUse2Min
                + stats.SpellPowerFor20SecOnUse2Min
                + stats.HasteRatingFor20SecOnUse2Min
                + stats.HasteRatingFor20SecOnUse5Min
                + stats.SpellPowerFor10SecOnCast_15_45
                + stats.SpellPowerFor10SecOnHit_10_45
                + stats.TimbalsProc
                + stats.BonusSpellCritMultiplier
                ) > 0;
        }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsShadowPriest));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsShadowPriest calcOpts = serializer.Deserialize(reader) as CalculationOptionsShadowPriest;
            return calcOpts;
        }
    }
}
