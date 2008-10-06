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
                    "Basic Stats:Spell Power",
					//"Basic Stats:Healing",
					"Basic Stats:Mp5",
					"Basic Stats:Regen InFSR",
					"Basic Stats:Regen OutFSR",
					"Basic Stats:Holy Spell Crit",
					"Basic Stats:Spell Haste",
                    "Basic Stats:Global Cooldown",
                    //"Spells:Heal",
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
                    _customChartNames = new string[] { "Spell HpS", "Spell HpM", "Spell AoE HpS", "Spell AoE HpM" };
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

            calculatedStats.BasicStats.Health = (float)Math.Floor(calculatedStats.BasicStats.Health * (1 + character.PriestTalents.Enlightenment * 0.01f));
            calculatedStats.BasicStats.Spirit = (float)Math.Floor(calculatedStats.BasicStats.Spirit * (1 + character.PriestTalents.SpiritOfRedemption * 0.05f));

            calculatedStats.SpiritRegen = (float)Math.Floor(5 * 0.0093271 * calculatedStats.BasicStats.Spirit * Math.Sqrt(calculatedStats.BasicStats.Intellect));
            
            calculatedStats.RegenInFSR = (float)Math.Floor((calculatedStats.BasicStats.Mp5 + character.PriestTalents.Meditation * 0.1f * 
                calculatedStats.SpiritRegen * (1 + calculatedStats.BasicStats.SpellCombatManaRegeneration)));
            calculatedStats.RegenOutFSR = calculatedStats.BasicStats.Mp5 + calculatedStats.SpiritRegen;

            calculatedStats.BasicStats.SpellCrit = (float)Math.Round((calculatedStats.BasicStats.Intellect / 80) +
                (calculatedStats.BasicStats.CritRating / 22.08) + 1.24 + character.PriestTalents.HolySpecialization, 2);

            calculatedStats.BasicStats.SpellPower += calculatedStats.BasicStats.Spirit * character.PriestTalents.SpiritualGuidance * 0.05f;
            
            calculatedStats.HealPoints = calculatedStats.BasicStats.SpellPower * 1.88f
                + (calculatedStats.BasicStats.HealingDoneFor15SecOnUse2Min * 15f / 120f)
                + (calculatedStats.BasicStats.HealingDoneFor15SecOnUse90Sec * 15f / 90f)
                + (calculatedStats.BasicStats.HealingDoneFor20SecOnUse2Min * 20f / 120f)
                + (calculatedStats.BasicStats.SpiritFor20SecOnUse2Min * character.PriestTalents.SpiritualGuidance * 0.05f * 20f / 120f);

            float procSpiritRegen = 0;
            if (calculatedStats.BasicStats.SpiritFor20SecOnUse2Min > 0)
            {
                procSpiritRegen = ((float)Math.Floor(5 * 0.0093271 * calculatedStats.BasicStats.SpiritFor20SecOnUse2Min * Math.Sqrt(calculatedStats.BasicStats.Intellect)) * 20f / 120f);
                procSpiritRegen = procSpiritRegen * (100 - calculationOptions.TimeInFSR) * 0.01f +
                    character.PriestTalents.Meditation * 0.1f * procSpiritRegen * (1 + calculatedStats.BasicStats.SpellCombatManaRegeneration) * calculationOptions.TimeInFSR * 0.01f;
            }

            float procSpiritRegen2 = 0;
            if (calculatedStats.BasicStats.BangleProc > 0)
            {
                procSpiritRegen2 = ((float)Math.Floor(5 * 0.0093271 * 130f * Math.Sqrt(calculatedStats.BasicStats.Intellect)) * 20f / 120f);
                procSpiritRegen2 = procSpiritRegen2 * (100 - calculationOptions.TimeInFSR) * 0.01f +
                    character.PriestTalents.Meditation * 0.1f * procSpiritRegen2 * (1 + calculatedStats.BasicStats.SpellCombatManaRegeneration) * calculationOptions.TimeInFSR * 0.01f;
            }

            calculatedStats.RegenPoints = (calculatedStats.RegenInFSR * calculationOptions.TimeInFSR * 0.01f +
               calculatedStats.RegenOutFSR * (100 - calculationOptions.TimeInFSR) * 0.01f)
                + calculatedStats.BasicStats.MementoProc * 3f * 5f / (45f + 9.5f * 2f)
                + calculatedStats.BasicStats.ManaregenFor8SecOnUse5Min * 5f * (8f * (1 - calculatedStats.BasicStats.HasteRating / 15.7f / 100f)) / (60f * 5f)
                + (calculatedStats.BasicStats.BonusManaPotion * 2400f * 5f / 120f)
                + procSpiritRegen + procSpiritRegen2
                + (calculatedStats.BasicStats.Mp5OnCastFor20SecOnUse2Min > 0 ? 588f * 5f / 120f : 0)
                + (calculatedStats.BasicStats.ManaregenOver20SecOnUse3Min * 5f / 180f)
                + (calculatedStats.BasicStats.ManaregenOver20SecOnUse5Min * 5f / 300f)
                + (calculatedStats.BasicStats.ManacostReduceWithin15OnHealingCast / (2.0f * 50f)) * 5f
                + (calculatedStats.BasicStats.FullManaRegenFor15SecOnSpellcast > 0?(((calculatedStats.RegenOutFSR - calculatedStats.RegenInFSR) / 5f) * 15f / 125f) * 5f: 0)
                + (calculatedStats.BasicStats.BangleProc > 0 ? (((calculatedStats.RegenOutFSR - calculatedStats.RegenInFSR) / 5f) * 0.25f * 15f / 125f) * 5f : 0);
                        
            calculatedStats.HastePoints = calculatedStats.BasicStats.HasteRating / 2f
                + calculatedStats.BasicStats.SpellHasteFor20SecOnUse2Min * 20f / 120f / 2f;

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
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 57f,
                        Intellect = 145f,
                        Spirit = 151f
                    };
                case Character.CharacterRace.Dwarf:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 61f,
                        Intellect = 144f,
                        Spirit = 150f
                    };
                case Character.CharacterRace.Draenei:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 57f,
                        Intellect = 146f,
                        Spirit = 153f
                    };
                case Character.CharacterRace.Human:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 58f,
                        Intellect = 145f,
                        Spirit = 152f,
                        BonusSpiritMultiplier = 0.1f
                    };
                case Character.CharacterRace.BloodElf:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 56f,
                        Intellect = 149f,
                        Spirit = 150f
                    };
                case Character.CharacterRace.Troll:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 59f,
                        Intellect = 141f,
                        Spirit = 152f
                    };
                case Character.CharacterRace.Undead:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 59f,
                        Intellect = 143f,
                        Spirit = 156f,
                    };
            }
            return new Stats();
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTotal = statsBaseGear + statsEnchants + statsBuffs + statsRace;

            statsTotal.Stamina = (float)Math.Floor((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
			statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower + (statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit));
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
                case "Spell AoE HpS":
                    _currentChartName = "Spell HpS";
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>[] {
                                                Renew.GetAllCommonRanks(p.BasicStats, character),
                                                FlashHeal.GetAllCommonRanks(p.BasicStats, character), 
                                                GreaterHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfHealing.GetAllCommonRanks(p.BasicStats, character, 5),
                                                BindingHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfMending.GetAllCommonRanks(p.BasicStats, character, 5),
                                                CircleOfHealing.GetAllCommonRanks(p.BasicStats, character, 5),
                                                HolyNova.GetAllCommonRanks(p.BasicStats, character, 5),
                                                Penance.GetAllCommonRanks(p.BasicStats, character),
                                                PowerWordShield.GetAllCommonRanks(p.BasicStats, character)
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
                case "Spell AoE HpM":
                    _currentChartName = "Spell HpM";
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>[] {
                                                Renew.GetAllCommonRanks(p.BasicStats, character),
                                                FlashHeal.GetAllCommonRanks(p.BasicStats, character), 
                                                GreaterHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfHealing.GetAllCommonRanks(p.BasicStats, character, 5),
                                                BindingHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfMending.GetAllCommonRanks(p.BasicStats, character, 5),
                                                CircleOfHealing.GetAllCommonRanks(p.BasicStats, character, 5),
                                                HolyNova.GetAllCommonRanks(p.BasicStats, character, 5),
                                                Penance.GetAllCommonRanks(p.BasicStats, character),
                                                PowerWordShield.GetAllCommonRanks(p.BasicStats, character)
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
                                                Renew.GetAllCommonRanks(p.BasicStats, character),
                                                FlashHeal.GetAllCommonRanks(p.BasicStats, character), 
                                                GreaterHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfHealing.GetAllCommonRanks(p.BasicStats, character, 1),
                                                BindingHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfMending.GetAllCommonRanks(p.BasicStats, character, 1),
                                                CircleOfHealing.GetAllCommonRanks(p.BasicStats, character, 1),
                                                HolyNova.GetAllCommonRanks(p.BasicStats, character, 1),
                                                Penance.GetAllCommonRanks(p.BasicStats, character),
                                                PowerWordShield.GetAllCommonRanks(p.BasicStats, character)

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
                                                Renew.GetAllCommonRanks(p.BasicStats, character),
                                                FlashHeal.GetAllCommonRanks(p.BasicStats, character), 
                                                GreaterHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfHealing.GetAllCommonRanks(p.BasicStats, character, 1),
                                                BindingHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfMending.GetAllCommonRanks(p.BasicStats, character, 1),
                                                CircleOfHealing.GetAllCommonRanks(p.BasicStats, character, 1),
                                                HolyNova.GetAllCommonRanks(p.BasicStats, character, 1),
                                                Penance.GetAllCommonRanks(p.BasicStats, character),
                                                PowerWordShield.GetAllCommonRanks(p.BasicStats, character)
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
                SpellPower = stats.SpellPower,
                CritRating = stats.CritRating,
                HasteRating = stats.HasteRating,
                Health = stats.Health,
                Mana = stats.Mana,
                Spirit = stats.Spirit,
                BonusManaPotion = stats.BonusManaPotion,
                MementoProc = stats.MementoProc,
                SpellCombatManaRegeneration = stats.SpellCombatManaRegeneration,
                BonusPoHManaCostReductionMultiplier = stats.BonusPoHManaCostReductionMultiplier,
                BonusGHHealingMultiplier = stats.BonusGHHealingMultiplier,
                ManaregenFor8SecOnUse5Min = stats.ManaregenFor8SecOnUse5Min,
                HealingDoneFor15SecOnUse2Min = stats.HealingDoneFor15SecOnUse2Min,
                HealingDoneFor20SecOnUse2Min = stats.HealingDoneFor20SecOnUse2Min,
                HealingDoneFor15SecOnUse90Sec = stats.HealingDoneFor15SecOnUse90Sec,
                SpiritFor20SecOnUse2Min = stats.SpiritFor20SecOnUse2Min,
                SpellHasteFor20SecOnUse2Min = stats.SpellHasteFor20SecOnUse2Min,
                Mp5OnCastFor20SecOnUse2Min = stats.Mp5OnCastFor20SecOnUse2Min,
                ManaregenOver20SecOnUse3Min = stats.ManaregenOver20SecOnUse3Min,
                ManaregenOver20SecOnUse5Min = stats.ManaregenOver20SecOnUse5Min,
                ManacostReduceWithin15OnHealingCast = stats.ManacostReduceWithin15OnHealingCast,
                FullManaRegenFor15SecOnSpellcast = stats.FullManaRegenFor15SecOnSpellcast,
                BangleProc = stats.BangleProc
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Stamina + stats.Intellect + stats.Spirit + stats.Mp5 + stats.SpellPower * 1.88f + stats.CritRating
                + stats.HasteRating + stats.BonusSpiritMultiplier + stats.SpellDamageFromSpiritPercentage + stats.BonusIntellectMultiplier
                + stats.BonusManaPotion + stats.MementoProc + stats.ManaregenFor8SecOnUse5Min
                + stats.BonusPoHManaCostReductionMultiplier + stats.SpellCombatManaRegeneration
                + stats.HealingDoneFor15SecOnUse2Min + stats.HealingDoneFor20SecOnUse2Min
                + stats.HealingDoneFor15SecOnUse90Sec + stats.SpiritFor20SecOnUse2Min
                + stats.SpellHasteFor20SecOnUse2Min + stats.Mp5OnCastFor20SecOnUse2Min
                + stats.ManaregenOver20SecOnUse3Min + stats.ManaregenOver20SecOnUse5Min
                + stats.ManacostReduceWithin15OnHealingCast + stats.FullManaRegenFor15SecOnSpellcast
                + stats.BangleProc) > 0;
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
