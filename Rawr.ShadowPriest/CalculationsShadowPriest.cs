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
                    _subPointNameColors.Add("Dps", System.Drawing.Color.FromArgb(0, 128, 255));
                    _subPointNameColors.Add("Survivability", System.Drawing.Color.FromArgb(64, 128, 32));
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
					"Basic Stats:Spell Crit",
					"Basic Stats:Spell Hit",
					"Basic Stats:Spell Haste",
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
                    "Holy:Penance",
                    "Simulation:Damage done",
                    "Simulation:Dps"
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
                    _customChartNames = new string[] {};
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
            throw new NotImplementedException();
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
            calculatedStats.RegenInFSR = calculatedStats.SpiritRegen * calculatedStats.BasicStats.SpellCombatManaRegeneration;
            calculatedStats.RegenOutFSR = calculatedStats.SpiritRegen;


            Solver solver = new Solver(stats, character);
            solver.Calculate();

            calculatedStats.DpsPoints = solver.OverallDps;
            calculatedStats.SurvivalPoints = calculatedStats.BasicStats.Stamina / 10;
            calculatedStats.OverallPoints = calculatedStats.DpsPoints + calculatedStats.SurvivalPoints;

            return calculatedStats;
        }

        public static int GetSpellHitCap(Character character)
        {
            return (int)Math.Round((int)Math.Ceiling(17f * 12.6f) - (character.PriestTalents.ShadowFocus + character.PriestTalents.Misery) * 12.6f );
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
                BonusHolyDamageMultiplier = stats.BonusHolyDamageMultiplier
//                SpellPowerFor15SecOnUse2Min = stats.SpellPowerFor15SecOnUse2Min,
//                SpellDamageFor15SecOnUse90Sec = stats.SpellDamageFor15SecOnUse90Sec,
//                SpellDamageFor20SecOnUse2Min = stats.SpellDamageFor20SecOnUse2Min,
//                SpellHasteFor20SecOnUse2Min = stats.SpellHasteFor20SecOnUse2Min,
//                SpellHasteFor20SecOnUse5Min = stats.SpellHasteFor20SecOnUse5Min
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
                ) > 0;
                //+ stats.SpellPowerFor15SecOnUse2Min;
                //+ stats.SpellDamageFor15SecOnUse90Sec
                //+ stats.SpellDamageFor20SecOnUse2Min + stats.SpellHasteFor20SecOnUse2Min
                //+ stats.SpellHasteFor20SecOnUse5Min) > 0;
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
