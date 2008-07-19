using System;
using System.Collections.Generic;

using Rawr;

namespace Rawr.HolyPriest
{
    [System.ComponentModel.DisplayName("HolyPriest|Spell_Holy_Renew")]
    public class CalculationsHolyPriest : CalculationsBase 
    {
        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Priest; } }

        private string _currentChartName = null;
        
        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                switch (_currentChartName)
                {
                    case "Spell HpS":
                        _subPointNameColors.Add("HpS", System.Drawing.Color.Red);
                        break;
                    case "Spell HpM":
                        _subPointNameColors.Add("HpM", System.Drawing.Color.Red);
                        break;
                    default:
                        _subPointNameColors.Add("Healing", System.Drawing.Color.Red);
                        _subPointNameColors.Add("Regen", System.Drawing.Color.Blue);
                        _subPointNameColors.Add("Haste", System.Drawing.Color.Gold);
                        break;
                }
               
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
					"Basic Stats:Intellect",
					"Basic Stats:Spirit",
					"Basic Stats:Healing",
					"Basic Stats:Mp5",
					"Basic Stats:Regen InFSR",
					"Basic Stats:Regen OutFSR",
					"Basic Stats:Spell Crit",
					"Basic Stats:Spell Haste",
                    "Basic Stats:Global Cooldown",
                    "Spells:Renew",
                    "Spells:Greater Heal",
                    "Spells:Flash Heal",
				    "Spells:Binding Heal",
                    "Spells:Prayer of Mending",
                    "Spells:PoH",
				    "Spells:CoH",
                    "Spells:Power Word Shield",
                    "Spells:Heal",
				    "Spells:Holy Nova",
                    "Spells:Lightwell",
                    "Spells:Gift of the Naaru"
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
                    _customChartNames = new string[] { "Spell HpS", "Spell HpM", "Spell HpS All", "Spell HpM All" };
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

            calculatedStats.Race = character.Race;
            calculatedStats.BasicStats = stats;
            calculatedStats.Talents = character.Talents;

            calculatedStats.BasicStats.Spirit = statsRace.Spirit + (calculatedStats.BasicStats.Spirit - statsRace.Spirit) * (1 + character.Talents.GetTalent("Spirit of Redemption").PointsInvested * 0.05f);

            calculatedStats.SpiritRegen = (float)Math.Floor(5 * 0.0093271 * calculatedStats.BasicStats.Spirit * Math.Sqrt(calculatedStats.BasicStats.Intellect));
            calculatedStats.RegenInFSR = (float)Math.Floor((calculatedStats.BasicStats.Mp5 + character.Talents.GetTalent("Meditation").PointsInvested * 0.1f * calculatedStats.SpiritRegen * (1 + calculatedStats.BasicStats.SpellCombatManaRegeneration)));
            calculatedStats.RegenOutFSR = calculatedStats.BasicStats.Mp5 + calculatedStats.SpiritRegen;
            
            calculatedStats.BasicStats.SpellCrit = (float)Math.Round((calculatedStats.BasicStats.Intellect / 80) +
                (calculatedStats.BasicStats.SpellCritRating / 22.08) + 1.85 + character.Talents.GetTalent("Holy Specialization").PointsInvested, 2);

            calculatedStats.BasicStats.Healing += calculatedStats.BasicStats.Spirit * character.Talents.GetTalent("Spiritual Guidance").PointsInvested * 0.05f;
            
            calculatedStats.HealPoints = calculatedStats.BasicStats.Healing;
            calculatedStats.RegenPoints = (calculatedStats.RegenInFSR * calculationOptions.TimeInFSR*0.01f +
                                           calculatedStats.RegenOutFSR * (100 - calculationOptions.TimeInFSR) * 0.01f);
            calculatedStats.HastePoints = calculatedStats.BasicStats.SpellHasteRating / 2f;
            calculatedStats.OverallPoints = calculatedStats.HealPoints + calculatedStats.RegenPoints + calculatedStats.HastePoints;

            return calculatedStats;
        }

        public Stats GetRaceStats(Character character)
        {
            switch (character.Race)
            {
                case Character.CharacterRace.NightElf:
                    return new Stats()
                    {
                        Health = 3434f,
                        Mana = 2470f,
                        Stamina = 57f,
                        Agility = 50f,
                        Intellect = 147f,
                        Spirit = 151f
                    };
                case Character.CharacterRace.Dwarf:
                    return new Stats()
                    {
                        Health = 3434f,
                        Mana = 2470f,
                        Stamina = 61f,
                        Agility = 41f,
                        Intellect = 144f,
                        Spirit = 150f
                    };
                case Character.CharacterRace.Draenei:
                    return new Stats()
                    {
                        Health = 3434f,
                        Mana = 2470f,
                        Stamina = 57f,
                        Agility = 42f,
                        Intellect = 146f,
                        Spirit = 160f
                    };
                case Character.CharacterRace.Human:
                    return new Stats()
                    {
                        Health = 3434f,
                        Mana = 2470f,
                        Stamina = 58f,
                        Agility = 45f,
                        Intellect = 145f,
                        Spirit = 174f
                    };
                case Character.CharacterRace.BloodElf:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 56f,
                        Agility = 47f,
                        Intellect = 149f,
                        Spirit = 157f
                    };
                case Character.CharacterRace.Troll:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 59f,
                        Agility = 59f,
                        Intellect = 141f,
                        Spirit = 159f
                    };
                case Character.CharacterRace.Undead:
                    return new Stats()
                    {
                        Health = 3181,
                        Mana = 2530f,
                        Stamina = 59f,
                        Agility = 43f,
                        Intellect = 145f,
                        Spirit = 156f,
                    };
            }
            return null;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTotal = statsBaseGear + statsEnchants + statsBuffs + statsRace;

            statsTotal.Stamina = (float)Math.Round((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Round(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Round((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.Healing = (float)Math.Round(statsTotal.Healing + (statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit));
            statsTotal.Mana = statsTotal.Mana + ((statsTotal.Intellect - 20f) * 15f + 20f);
            statsTotal.Health = statsTotal.Health + (statsTotal.Stamina * 10f);

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            ComparisonCalculationBase comparison;
            CharacterCalculationsHolyPriest p;
            List<Spell>[] spellList;

            switch (chartName)
            {
                case "Spell HpS All":
                    _currentChartName = "Spell HpS";
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>[] {
                                                Renew.GetAllRanks(p.BasicStats, character.Talents),
                                                FlashHeal.GetAllRanks(p.BasicStats, character.Talents), 
                                                GreaterHeal.GetAllRanks(p.BasicStats, character.Talents),
                                                Heal.GetAllRanks(p.BasicStats, character.Talents),
                                                PrayerOfHealing.GetAllRanks(p.BasicStats, character.Talents, 3),
                                                PrayerOfHealing.GetAllRanks(p.BasicStats, character.Talents, 4),
                                                PrayerOfHealing.GetAllRanks(p.BasicStats, character.Talents, 5),
                                                BindingHeal.GetAllRanks(p.BasicStats, character.Talents),
                                                PrayerOfMending.GetAllRanks(p.BasicStats, character.Talents, 3),
                                                PrayerOfMending.GetAllRanks(p.BasicStats, character.Talents, 4),
                                                PrayerOfMending.GetAllRanks(p.BasicStats, character.Talents, 5),
                                                CircleOfHealing.GetAllRanks(p.BasicStats, character.Talents, 3),
                                                CircleOfHealing.GetAllRanks(p.BasicStats, character.Talents, 4),
                                                CircleOfHealing.GetAllRanks(p.BasicStats, character.Talents, 5),
                                                HolyNova.GetAllRanks(p.BasicStats, character.Talents, 3),
                                                HolyNova.GetAllRanks(p.BasicStats, character.Talents, 4),
                                                HolyNova.GetAllRanks(p.BasicStats, character.Talents, 5)
                                            };

                    foreach (List<Spell> spells in spellList)
                    {
                        if(spells[0].AvgHeal == 0)
                            continue;

                        for (int i = 0; i < spells.Count; i++)
                        {
                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = spells[i].Name + " [Rank " + spells[i].Rank + "]";
                            comparison.Equipped = false;
                            comparison.SubPoints[0] = spells[i].HpS;
                            comparison.OverallPoints = comparison.SubPoints[0];
                            comparisonList.Add(comparison);
                        }
                    }

                    return comparisonList.ToArray();
                case "Spell HpM All":
                    _currentChartName = "Spell HpM";
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>[] {
                                                Renew.GetAllRanks(p.BasicStats, character.Talents),
                                                FlashHeal.GetAllRanks(p.BasicStats, character.Talents), 
                                                GreaterHeal.GetAllRanks(p.BasicStats, character.Talents),
                                                Heal.GetAllRanks(p.BasicStats, character.Talents),
                                                PrayerOfHealing.GetAllRanks(p.BasicStats, character.Talents, 3),
                                                PrayerOfHealing.GetAllRanks(p.BasicStats, character.Talents, 4),
                                                PrayerOfHealing.GetAllRanks(p.BasicStats, character.Talents, 5),
                                                BindingHeal.GetAllRanks(p.BasicStats, character.Talents),
                                                PrayerOfMending.GetAllRanks(p.BasicStats, character.Talents, 3),
                                                PrayerOfMending.GetAllRanks(p.BasicStats, character.Talents, 4),
                                                PrayerOfMending.GetAllRanks(p.BasicStats, character.Talents, 5),
                                                CircleOfHealing.GetAllRanks(p.BasicStats, character.Talents, 3),
                                                CircleOfHealing.GetAllRanks(p.BasicStats, character.Talents, 4),
                                                CircleOfHealing.GetAllRanks(p.BasicStats, character.Talents, 5),
                                                HolyNova.GetAllRanks(p.BasicStats, character.Talents, 3),
                                                HolyNova.GetAllRanks(p.BasicStats, character.Talents, 4),
                                                HolyNova.GetAllRanks(p.BasicStats, character.Talents, 5)
                                            };

                    foreach (List<Spell> spells in spellList)
                    {
                        if (spells[0].AvgHeal == 0)
                            continue;

                        for (int i = 0; i < spells.Count; i++)
                        {
                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = spells[i].Name + " [Rank " + spells[i].Rank + "]";
                            comparison.Equipped = false;
                            comparison.SubPoints[0] = spells[i].HpM;
                            comparison.OverallPoints = comparison.SubPoints[0];
                            comparisonList.Add(comparison);
                        }
                    }

                    return comparisonList.ToArray();
                case "Spell HpS":
                    _currentChartName = "Spell HpS";
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>[] {
                                                Renew.GetAllCommonRanks(p.BasicStats, character.Talents),
                                                FlashHeal.GetAllCommonRanks(p.BasicStats, character.Talents), 
                                                GreaterHeal.GetAllCommonRanks(p.BasicStats, character.Talents),
                                                Heal.GetAllCommonRanks(p.BasicStats, character.Talents),
                                                PrayerOfHealing.GetAllCommonRanks(p.BasicStats, character.Talents, 3),
                                                PrayerOfHealing.GetAllCommonRanks(p.BasicStats, character.Talents, 4),
                                                PrayerOfHealing.GetAllCommonRanks(p.BasicStats, character.Talents, 5),
                                                BindingHeal.GetAllCommonRanks(p.BasicStats, character.Talents),
                                                PrayerOfMending.GetAllCommonRanks(p.BasicStats, character.Talents, 3),
                                                PrayerOfMending.GetAllCommonRanks(p.BasicStats, character.Talents, 4),
                                                PrayerOfMending.GetAllCommonRanks(p.BasicStats, character.Talents, 5),
                                                CircleOfHealing.GetAllCommonRanks(p.BasicStats, character.Talents, 3),
                                                CircleOfHealing.GetAllCommonRanks(p.BasicStats, character.Talents, 4),
                                                CircleOfHealing.GetAllCommonRanks(p.BasicStats, character.Talents, 5),
                                                HolyNova.GetAllCommonRanks(p.BasicStats, character.Talents, 3),
                                                HolyNova.GetAllCommonRanks(p.BasicStats, character.Talents, 4),
                                                HolyNova.GetAllCommonRanks(p.BasicStats, character.Talents, 5)
                                            };

                    foreach (List<Spell> spells in spellList)
                    {
                        if (spells[0].AvgHeal == 0)
                            continue;

                        for (int i = 0; i < spells.Count; i++)
                        {
                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = spells[i].Name + " [Rank " + spells[i].Rank + "]";
                            comparison.Equipped = false;
                            comparison.SubPoints[0] = spells[i].HpS;
                            comparison.OverallPoints = comparison.SubPoints[0];
                            comparisonList.Add(comparison);
                        }
                    }

                    return comparisonList.ToArray();
                case "Spell HpM":
                    _currentChartName = "Spell HpM";
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>[] {
                                                Renew.GetAllCommonRanks(p.BasicStats, character.Talents),
                                                FlashHeal.GetAllCommonRanks(p.BasicStats, character.Talents), 
                                                GreaterHeal.GetAllCommonRanks(p.BasicStats, character.Talents),
                                                Heal.GetAllCommonRanks(p.BasicStats, character.Talents),
                                                PrayerOfHealing.GetAllCommonRanks(p.BasicStats, character.Talents, 3),
                                                PrayerOfHealing.GetAllCommonRanks(p.BasicStats, character.Talents, 4),
                                                PrayerOfHealing.GetAllCommonRanks(p.BasicStats, character.Talents, 5),
                                                BindingHeal.GetAllCommonRanks(p.BasicStats, character.Talents),
                                                PrayerOfMending.GetAllCommonRanks(p.BasicStats, character.Talents, 3),
                                                PrayerOfMending.GetAllCommonRanks(p.BasicStats, character.Talents, 4),
                                                PrayerOfMending.GetAllCommonRanks(p.BasicStats, character.Talents, 5),
                                                CircleOfHealing.GetAllCommonRanks(p.BasicStats, character.Talents, 3),
                                                CircleOfHealing.GetAllCommonRanks(p.BasicStats, character.Talents, 4),
                                                CircleOfHealing.GetAllCommonRanks(p.BasicStats, character.Talents, 5),
                                                HolyNova.GetAllCommonRanks(p.BasicStats, character.Talents, 3),
                                                HolyNova.GetAllCommonRanks(p.BasicStats, character.Talents, 4),
                                                HolyNova.GetAllCommonRanks(p.BasicStats, character.Talents, 5)
                                            };

                    foreach (List<Spell> spells in spellList)
                    {
                        if (spells[0].AvgHeal == 0)
                            continue;

                        for (int i = 0; i < spells.Count; i++)
                        {
                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = spells[i].Name + " [Rank " + spells[i].Rank + "]";
                            comparison.Equipped = false;
                            comparison.SubPoints[0] = spells[i].HpM;
                            comparison.OverallPoints = comparison.SubPoints[0];
                            comparisonList.Add(comparison);
                        }
                    }

                    return comparisonList.ToArray();
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
                Mp5 = stats.Mp5,
                Healing = stats.Healing,
                SpellCritRating = stats.SpellCritRating,
                SpellHasteRating = stats.SpellHasteRating,
                Health = stats.Health,
                Mana = stats.Mana,
                Spirit = stats.Spirit,
                BonusManaPotion = stats.BonusManaPotion,
                MementoProc = stats.MementoProc,
                SpellCombatManaRegeneration = stats.SpellCombatManaRegeneration,
                BonusPoHManaCostReductionMultiplier = stats.BonusPoHManaCostReductionMultiplier,
                BonusGHHealingMultiplier = stats.BonusGHHealingMultiplier
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Stamina + stats.Intellect + stats.Spirit + stats.Mp5 + stats.Healing + stats.SpellCritRating
                + stats.SpellHasteRating + stats.BonusSpiritMultiplier + stats.SpellDamageFromSpiritPercentage + stats.BonusIntellectMultiplier
                + stats.BonusManaPotion + stats.MementoProc + stats.SpellCombatManaRegeneration
                + stats.BonusPoHManaCostReductionMultiplier + stats.SpellCombatManaRegeneration) > 0;
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
