using System;
using System.Collections.Generic;

using Rawr;

namespace Rawr.HolyPriest
{
	[Rawr.Calculations.RawrModelInfo("HolyPriest", "Spell_Holy_Renew", Character.CharacterClass.Priest)]
	public class CalculationsHolyPriest : CalculationsBase 
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
                    case "MP5 Sources":
                        _subPointNameColors.Add(string.Format("MP5 Sources ({0} total)", _currentChartTotal.ToString("0")), System.Drawing.Color.Blue);
                        break;
                    case "Spell HpS":
                        _subPointNameColors.Add("HpS", System.Drawing.Color.Red);
                        break;
                    case "Spell HpM":
                        _subPointNameColors.Add("HpM", System.Drawing.Color.Red);
                        break;
                    default:
                        _subPointNameColors.Add("HPS-Burst", System.Drawing.Color.Red);
                        _subPointNameColors.Add("HPS-Sustained", System.Drawing.Color.Blue);
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
					"Basic Stats:Mana",
					"Basic Stats:Stamina",
                    "Basic Stats:Resilience",
					"Basic Stats:Intellect",
					"Basic Stats:Spirit",
                    "Basic Stats:Spell Power",
					"Basic Stats:In FSR MP5",
                    "Basic Stats:Spell Crit",
					"Basic Stats:Healing Crit",
					"Basic Stats:Spell Haste",
                    "Simulation:Role",
                    "Simulation:Burst*This is the HPS you are expected to have if you are not limited by Mana.",
                    "Simulation:Sustained*This is the HPS are expected to have when restricted by Mana.\r\nIf this value is lower than your Burst HPS, you are running out of mana in the simulation.",
                    "Spells:Greater Heal",
                    "Spells:Flash Heal",
				    "Spells:Binding Heal",
                    "Spells:Renew",
                    "Spells:Prayer of Mending",
                    "Spells:Power Word Shield",
                    "Spells:PoH",
				    "Spells:Holy Nova",
                    "Spells:Lightwell",
				    "Spells:CoH",
                    "Spells:Penance",
                    "Spells:Gift of the Naaru"
				};
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
					"Health",
                    "Resilience",
                    "Mana",
                    "InFSR Regen",
                    "OutFSR Regen",
					"Haste Rating",
                    "Haste %",
                    "Crit Rating",
                    "Healing Crit %",
                    "PW:Shield",
                    "GHeal Avg",
                    "FHeal Avg",
                    "CoH Avg",
					};
                return _optimizableCalculationLabels;
            }
        }


        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelHolyPriest();
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
                    _customChartNames = new string[] { "MP5 Sources", "Spell HpS", "Spell HpM", "Spell AoE HpS", "Spell AoE HpM", "Relative Stat Values" };
                return _customChartNames;
            }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationHolyPriest(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsHolyPriest(); }

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

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            Stats stats = GetCharacterStats(character, additionalItem);
            Stats statsRace = GetRaceStats(character);
            CharacterCalculationsHolyPriest calculatedStats = new CharacterCalculationsHolyPriest();
            CalculationOptionsPriest calculationOptions = character.CalculationOptions as CalculationOptionsPriest;
            if (calculationOptions == null)
                return null;

            calculatedStats.Race = character.Race;
            calculatedStats.BasicStats = stats;
            calculatedStats.Character = character;

            calculatedStats.SpiritRegen = (float)Math.Floor(5 * character.StatConversion.GetSpiritRegenSec(calculatedStats.BasicStats.Spirit, calculatedStats.BasicStats.Intellect));
            calculatedStats.RegenInFSR = calculatedStats.SpiritRegen * calculatedStats.BasicStats.SpellCombatManaRegeneration;
            calculatedStats.RegenOutFSR = calculatedStats.SpiritRegen;

            Solver solver = new Solver(stats, character, statsRace.Mana);
            solver.Calculate(calculatedStats);

            return calculatedStats;
        }

        public static float GetRaptureConst(Character character)
        {
            if (character.Level < 70)
                return 0.01035f; // Dunno
            return 0.01035f - (0.00845f * (character.Level - 70) / 10f);
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
            statsTotal.Mana *= (1f + statsTotal.BonusManaMultiplier);
            statsTotal.Health += statsTotal.Stamina * 10f;
            statsTotal.SpellCrit += character.StatConversion.GetSpellCritFromIntellect(statsTotal.Intellect) / 100f
                + character.StatConversion.GetSpellCritFromRating(statsTotal.CritRating) / 100f
                + 0.0124f;
            statsTotal.SpellHaste += character.StatConversion.GetSpellHasteFromRating(statsTotal.HasteRating) / 100f;
                 
            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            ComparisonCalculationBase comparison;
            CharacterCalculationsHolyPriest p;
            List<Spell> spellList;

            _currentChartTotal = 0;
            switch (chartName)
            {
                case "MP5 Sources":
                    _currentChartName = chartName;
                    CharacterCalculationsHolyPriest mscalcs = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    Solver mssolver = new Solver(mscalcs.BasicStats, character, CalculationsHolyPriest.GetRaceStats(character).Mana);
                    mssolver.Calculate(mscalcs);
                    foreach (Solver.ManaSource Source in mssolver.ManaSources)
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
                case "Spell AoE HpS":
                    _currentChartName = chartName;
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>();
                    spellList.Add(new Renew(p.BasicStats, character));
                    spellList.Add(new FlashHeal(p.BasicStats, character));
                    spellList.Add(new Heal(p.BasicStats, character));
                    spellList.Add(new PrayerOfHealing(p.BasicStats, character, 5));
                    spellList.Add(new BindingHeal(p.BasicStats, character));
                    spellList.Add(new PrayerOfMending(p.BasicStats, character, 5));
                    spellList.Add(new CircleOfHealing(p.BasicStats, character, 5));
                    spellList.Add(new HolyNova(p.BasicStats, character, 5));
                    spellList.Add(new Penance(p.BasicStats, character));
                    spellList.Add(new PowerWordShield(p.BasicStats, character));

                    foreach (Spell spell in spellList)
                    {
                        if(spell.AvgHeal == 0)
                            continue;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.Equipped = false;
                        comparison.SubPoints[0] = spell.HpS;
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparisonList.Add(comparison);
                    }

                    return comparisonList.ToArray();
                case "Spell AoE HpM":
                    _currentChartName = chartName;
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>();
                    spellList.Add(new Renew(p.BasicStats, character));
                    spellList.Add(new FlashHeal(p.BasicStats, character));
                    spellList.Add(new Heal(p.BasicStats, character));
                    spellList.Add(new PrayerOfHealing(p.BasicStats, character, 5));
                    spellList.Add(new BindingHeal(p.BasicStats, character));
                    spellList.Add(new PrayerOfMending(p.BasicStats, character, 5));
                    spellList.Add(new CircleOfHealing(p.BasicStats, character, 5));
                    spellList.Add(new HolyNova(p.BasicStats, character, 5));
                    spellList.Add(new Penance(p.BasicStats, character));
                    spellList.Add(new PowerWordShield(p.BasicStats, character));

                    foreach (Spell spell in spellList)
                    {
                        if (spell.AvgHeal == 0)
                            continue;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.Equipped = false;
                        comparison.SubPoints[0] = spell.HpM;
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparisonList.Add(comparison);
                    }

                    return comparisonList.ToArray();
                case "Spell HpS":
                    _currentChartName = chartName;
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>();
                    spellList.Add(new Renew(p.BasicStats, character));
                    spellList.Add(new FlashHeal(p.BasicStats, character));
                    spellList.Add(new Heal(p.BasicStats, character));
                    spellList.Add(new PrayerOfHealing(p.BasicStats, character, 1));
                    spellList.Add(new BindingHeal(p.BasicStats, character));
                    spellList.Add(new PrayerOfMending(p.BasicStats, character, 1));
                    spellList.Add(new CircleOfHealing(p.BasicStats, character, 1));
                    spellList.Add(new HolyNova(p.BasicStats, character, 1));
                    spellList.Add(new Penance(p.BasicStats, character));
                    spellList.Add(new PowerWordShield(p.BasicStats, character));

                    foreach (Spell spell in spellList)
                    {
                        if (spell.AvgHeal == 0)
                            continue;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.Equipped = false;
                        comparison.SubPoints[0] = spell.HpS;
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparisonList.Add(comparison);
                    }

                    return comparisonList.ToArray();
                case "Spell HpM":
                    _currentChartName = chartName;
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>();
                    spellList.Add(new Renew(p.BasicStats, character));
                    spellList.Add(new FlashHeal(p.BasicStats, character));
                    spellList.Add(new Heal(p.BasicStats, character));
                    spellList.Add(new PrayerOfHealing(p.BasicStats, character, 1));
                    spellList.Add(new BindingHeal(p.BasicStats, character));
                    spellList.Add(new PrayerOfMending(p.BasicStats, character, 1));
                    spellList.Add(new CircleOfHealing(p.BasicStats, character, 1));
                    spellList.Add(new HolyNova(p.BasicStats, character, 1));
                    spellList.Add(new Penance(p.BasicStats, character));
                    spellList.Add(new PowerWordShield(p.BasicStats, character));

                    foreach (Spell spell in spellList)
                    {
                        if (spell.AvgHeal == 0)
                            continue;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.Equipped = false;
                        comparison.SubPoints[0] = spell.HpM;
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparisonList.Add(comparison);
                    }

                    return comparisonList.ToArray();

                case "Relative Stat Values":
                    _currentChartName = chartName;
                    CharacterCalculationsHolyPriest calcsBase = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsIntellect = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsSpirit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Spirit = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsMP5 = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Mp5 = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsSpellPower = GetCharacterCalculations(character, new Item() { Stats = new Stats() { SpellPower = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsHaste = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsCrit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsSta = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsRes = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Resilience = 50 } }) as CharacterCalculationsHolyPriest;

                    return new ComparisonCalculationBase[] {
                        new ComparisonCalculationHolyPriest() { Name = "1 Intellect",
                            OverallPoints = (calcsIntellect.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsIntellect.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsIntellect.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsIntellect.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Spirit",
                            OverallPoints = (calcsSpirit.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsSpirit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsSpirit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsSpirit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 MP5",
                            OverallPoints = (calcsMP5.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsMP5.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsMP5.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsMP5.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Spell Power",
                            OverallPoints = (calcsSpellPower.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsSpellPower.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsSpellPower.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsSpellPower.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Haste",
                            OverallPoints = (calcsHaste.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsHaste.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsHaste.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsHaste.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Crit",
                            OverallPoints = (calcsCrit.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsCrit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsCrit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsCrit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Stamina",
                            OverallPoints = (calcsSta.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsSta.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsSta.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsSta.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Resilience",
                            OverallPoints = (calcsRes.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsRes.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsRes.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsRes.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        }};
                default:
                    _currentChartName = null;
                    return new ComparisonCalculationBase[0];
            }
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                Health = stats.Health,
                Mana = stats.Mana,
                Mp5 = stats.Mp5,
                SpellPower = stats.SpellPower,

                SpellHaste = stats.SpellHaste,
                SpellCrit = stats.SpellCrit,

                Resilience = stats.Resilience,
                CritRating = stats.CritRating,
                HasteRating = stats.HasteRating,
                
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                BonusManaMultiplier = stats.BonusManaMultiplier,
                BonusCritHealMultiplier = stats.BonusCritHealMultiplier,

                SpellDamageFromSpiritPercentage = stats.SpellDamageFromSpiritPercentage,
                HealingReceivedMultiplier = stats.HealingReceivedMultiplier,
                BonusManaPotion = stats.BonusManaPotion,
                SpellCombatManaRegeneration = stats.SpellCombatManaRegeneration,

                MementoProc = stats.MementoProc,
                ManaGainOnGreaterHealOverheal = stats.ManaGainOnGreaterHealOverheal,
                RenewDurationIncrease = stats.RenewDurationIncrease,
                BonusPoHManaCostReductionMultiplier = stats.BonusPoHManaCostReductionMultiplier,
                BonusGHHealingMultiplier = stats.BonusGHHealingMultiplier,
                PrayerOfMendingExtraJumps = stats.PrayerOfMendingExtraJumps,
                GreaterHealCostReduction = stats.GreaterHealCostReduction,
                WeakenedSoulDurationDecrease = stats.WeakenedSoulDurationDecrease,

                ManaregenFor8SecOnUse5Min = stats.ManaregenFor8SecOnUse5Min,
                HealingDoneFor15SecOnUse2Min = stats.HealingDoneFor15SecOnUse2Min,
                SpellPowerFor20SecOnUse2Min = stats.SpellPowerFor20SecOnUse2Min,
                SpellPowerFor15SecOnUse90Sec = stats.SpellPowerFor15SecOnUse90Sec,
                SpiritFor20SecOnUse2Min = stats.SpiritFor20SecOnUse2Min,
                HasteRatingFor20SecOnUse2Min = stats.HasteRatingFor20SecOnUse2Min,
                Mp5OnCastFor20SecOnUse2Min = stats.Mp5OnCastFor20SecOnUse2Min,
                ManaregenOver20SecOnUse3Min = stats.ManaregenOver20SecOnUse3Min,
                ManaregenOver20SecOnUse5Min = stats.ManaregenOver20SecOnUse5Min,
                ManacostReduceWithin15OnHealingCast = stats.ManacostReduceWithin15OnHealingCast,
                FullManaRegenFor15SecOnSpellcast = stats.FullManaRegenFor15SecOnSpellcast,
                BangleProc = stats.BangleProc,
                SpellHasteFor10SecOnCast_10_45 = stats.SpellHasteFor10SecOnCast_10_45,
                ManaRestoreOnCrit_25 = stats.ManaRestoreOnCrit_25,
                ManaRestoreOnCast_10_45 = stats.ManaRestoreOnCast_10_45,

                GLYPH_CircleOfHealing =  stats.GLYPH_CircleOfHealing,
                GLYPH_Dispel = stats.GLYPH_Dispel,
                GLYPH_FlashHeal = stats.GLYPH_FlashHeal,
                GLYPH_PowerWordShield = stats.GLYPH_PowerWordShield,
                GLYPH_PrayerOfHealing = stats.GLYPH_PrayerOfHealing,
                GLYPH_Renew = stats.GLYPH_Renew,
                GLYPH_HolyNova = stats.GLYPH_HolyNova,
                GLYPH_Lightwell = stats.GLYPH_Lightwell,
                GLYPH_MassDispel = stats.GLYPH_MassDispel,
            };
        }

        // Trinket Status:
        // Correctly implemented:
        // http://www.wowhead.com/?item=40382 - Soul of the Dead
        
        // Wrongly implemented:
        // http://www.wowhead.com/?item=37835 - Je'Tze's Bell

        // Not implemented:
        // http://www.wowhead.com/?item=44253 - Darkmoon Card: Greatness (But for 90 spi/90 int versions, not yet known)
        // http://www.wowhead.com/?item=42988 - Darkmoon Card: Illusion
        // http://www.wowhead.com/?item=40258 - Forethought Talisman
        // http://www.wowhead.com/?item=40432 - Illustration of the Dragon Soul
        // http://www.wowhead.com/?item=40532 - Living Ice Crystals
        // http://www.wowhead.com/?item=40430 - Majestic Dragon Figurine

        public override bool HasRelevantStats(Stats stats)
        {
            return (
                stats.Stamina + stats.Intellect + stats.Spirit + stats.Health + stats.Mana + stats.Mp5 + stats.SpellPower
                + stats.SpellHaste + stats.SpellCrit
                + stats.Resilience +  + stats.CritRating + stats.HasteRating 
                + stats.BonusIntellectMultiplier + stats.BonusSpiritMultiplier + stats.BonusManaMultiplier + stats.BonusCritHealMultiplier
                + stats.SpellDamageFromSpiritPercentage + stats.HealingReceivedMultiplier + stats.BonusManaPotion + stats.SpellCombatManaRegeneration
                
                + stats.MementoProc + stats.ManaGainOnGreaterHealOverheal + stats.RenewDurationIncrease
                + stats.BonusPoHManaCostReductionMultiplier + stats.BonusGHHealingMultiplier
                + stats.PrayerOfMendingExtraJumps + stats.GreaterHealCostReduction
                + stats.WeakenedSoulDurationDecrease

                + stats.ManaregenFor8SecOnUse5Min + stats.HealingDoneFor15SecOnUse2Min + stats.SpellPowerFor20SecOnUse2Min
                + stats.SpellPowerFor15SecOnUse90Sec + stats.SpiritFor20SecOnUse2Min + stats.HasteRatingFor20SecOnUse2Min
                + stats.Mp5OnCastFor20SecOnUse2Min + stats.ManaregenOver20SecOnUse3Min + stats.ManaregenOver20SecOnUse5Min
                + stats.ManacostReduceWithin15OnHealingCast + stats.FullManaRegenFor15SecOnSpellcast
                + stats.BangleProc + stats.SpellHasteFor10SecOnCast_10_45 + stats.ManaRestoreOnCrit_25
                + stats.ManaRestoreOnCast_10_45

                + stats.GLYPH_CircleOfHealing + stats.GLYPH_Dispel + stats.GLYPH_FlashHeal
                + stats.GLYPH_PowerWordShield + stats.GLYPH_PrayerOfHealing + stats.GLYPH_Renew
                + stats.GLYPH_HolyNova + stats.GLYPH_Lightwell + stats.GLYPH_MassDispel
                ) > 0;
        }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsPriest));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsPriest calcOpts = serializer.Deserialize(reader) as CalculationOptionsPriest;
            return calcOpts;
        }
    }
}
