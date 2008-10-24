using System;
using System.Collections.Generic;

using Rawr;

namespace Rawr.ShadowPriest
{
    [Rawr.Calculations.RawrModelInfo("ShadowPriest", "Spell_Shadow_Shadowform", Character.CharacterClass.Priest)]
    public class CalculationsShadowPriest : CalculationsBase 
    {
        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Priest; } }

        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("DPS-Burst", System.Drawing.Color.Red);
                    _subPointNameColors.Add("DPS-Sustained", System.Drawing.Color.Blue);
                    _subPointNameColors.Add("Survivability", System.Drawing.Color.Green);
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
                    _customChartNames = new string[] { "Stat Values" };
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

            switch (chartName)
            {
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
                    _customChartNames = null;
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

            calculatedStats.SpiritRegen = (float)Math.Floor(5 * (0.001f + 0.0093271 * calculatedStats.BasicStats.Spirit * Math.Sqrt(calculatedStats.BasicStats.Intellect)));
            calculatedStats.RegenInFSR = calculatedStats.SpiritRegen * calculatedStats.BasicStats.SpellCombatManaRegeneration + stats.Mp5;
            calculatedStats.RegenOutFSR = calculatedStats.SpiritRegen + stats.Mp5;

            SolverShadow solver = new SolverShadow(stats, character);
            solver.Calculate(calculatedStats);

            calculatedStats.DpsPoints = solver.DPS;
            calculatedStats.SustainPoints = solver.SustainDPS;
            // If opponent has 25% crit, each 39.42308044 resilience gives -1% damage from dots and -1% chance to be crit. Also reduces crits by 2%.
            // This effectively means you gain 12.5% extra health from removing 12.5% dot and 12.5% crits at resilience cap (492.5 (39.42308044*12.5))
            // In addition, the remaining 12.5% crits are reduced by 25% (12.5%*200%damage*75% = 18.75%)
            // At resilience cap I'd say that your hp's are scaled by 1.125*1.1875 = ~30%. Probably wrong but good enough.
            calculatedStats.SurvivalPoints = calculatedStats.BasicStats.Health * 5f / 100f * (1 + 0.3f * calculatedStats.BasicStats.Resilience / 492.7885f);
            calculatedStats.OverallPoints = calculatedStats.DpsPoints + calculatedStats.SustainPoints + calculatedStats.SurvivalPoints;

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
                        BonusSpiritMultiplier = 0.03f
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
            return null;
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
            statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower + (statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit));
            statsTotal.Mana += (statsTotal.Intellect - 20f) * 15f + 20f;
            statsTotal.Health += statsTotal.Stamina * 10f;
            statsTotal.SpellCrit += (statsTotal.Intellect / 80f / 100f) + (statsTotal.CritRating / 22.07692337F / 100f) + 0.0124f;
            statsTotal.SpellHaste += (statsTotal.HasteRating / 15.76923275f / 100f);
            statsTotal.SpellHit += (statsTotal.HitRating / 12.61538506f / 100f);

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
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusHolyDamageMultiplier = stats.BonusHolyDamageMultiplier,
                ManaRestorePerCast_5_15 = stats.ManaRestorePerCast_5_15,
                SpellPowerFor15SecOnUse90Sec = stats.SpellPowerFor15SecOnUse90Sec,
                SpellPowerFor15SecOnUse2Min = stats.SpellPowerFor15SecOnUse2Min,
                SpellPowerFor20SecOnUse2Min = stats.SpellPowerFor20SecOnUse2Min,
                HasteRatingFor20SecOnUse2Min = stats.HasteRatingFor20SecOnUse2Min,
                HasteRatingFor20SecOnUse5Min = stats.HasteRatingFor20SecOnUse5Min,
                TimbalsProc = stats.TimbalsProc,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (
                  stats.Stamina
                + stats.Health
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
                + stats.BonusShadowDamageMultiplier
                + stats.BonusHolyDamageMultiplier
                + stats.ManaRestorePerCast_5_15
                + stats.SpellPowerFor15SecOnUse90Sec
                + stats.SpellPowerFor15SecOnUse2Min
                + stats.SpellPowerFor20SecOnUse2Min
                + stats.HasteRatingFor20SecOnUse2Min
                + stats.HasteRatingFor20SecOnUse5Min
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
