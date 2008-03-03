using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Healadin
{
    [System.ComponentModel.DisplayName("Healadin")]
    public class CalculationsHealadin : CalculationsBase
    {
        //my insides all turned to ash / so slow
        //and blew away as i collapsed / so cold
        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelHealadin();
                }
                return _calculationOptionsPanel;
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
					"Basic Stats:Spell Crit Rating",
					"Basic Stats:Spell Haste Rating",
					"Complex Stats:FoL Hps",
					"Complex Stats:FoL Crit",
					"Complex Stats:Time to OOM",
					"Complex Stats:Cycle Hps",
				};
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] {
					//"Combat Table",
					//"Relative Stat Values",
					//"Agi Test"
					};
                return _customChartNames;
            }
        }

        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("Output", System.Drawing.Color.Red);
                    _subPointNameColors.Add("Longevity", System.Drawing.Color.Blue);
                }
                return _subPointNameColors;
            }
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
                        Item.ItemType.Plate,
                        Item.ItemType.None,
						Item.ItemType.Shield,
						Item.ItemType.Libram,
						Item.ItemType.OneHandAxe,
						Item.ItemType.OneHandMace,
						Item.ItemType.OneHandSword,
						Item.ItemType.TwoHandAxe,
						Item.ItemType.TwoHandMace,
						Item.ItemType.TwoHandSword
					});
                }
                return _relevantItemTypes;
            }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationHealadin(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsHealadin(); }


        public void TimeToOom(Character character, CharacterCalculationsHealadin stats)
        {
      /*      float mana = stats.BasicStats.Mana;
            float[] cycle = {int.Parse(character.CalculationOptions["NumberFoL"]) * stats.CastTime[0],
                int.Parse(character.CalculationOptions["NumberHL11"]) * stats.CastTime[1],
                int.Parse(character.CalculationOptions["NumberHL9"]) * stats.CastTime[2],
                int.Parse(character.CalculationOptions["NumberHL5"]) * stats.CastTime[3]};
            float cycleTime = cycle[0] + cycle[1] + cycle[2] + cycle[3];
            cycle[0] /= cycleTime;
            cycle[1] /= cycleTime;
            cycle[2] /= cycleTime;
            cycle[3] /= cycleTime;

            float[] mps = { (stats.ManaCost[0] * (1 - (.6f*stats.Crit[0]))) / stats.CastTime[0],
                          (stats.ManaCost[1] * (1 - (.6f*stats.Crit[1]))) / stats.CastTime[1],
                          (stats.ManaCost[2] * (1 - (.6f*stats.Crit[2]))) / stats.CastTime[2],
                          (stats.ManaCost[3] * (1 - (.6f*stats.Crit[3]))) / stats.CastTime[3],};
            float mpslost = (mps[0] * cycle[0]) + (mps[1] * cycle[1]) + (mps[2] * cycle[2]) + (mps[3] * cycle[3]);
            float mpsgain = stats.BasicStats.Mp5 / 5;
            stats.LongevityPoints = mana / (mpslost - mpsgain);
            stats.ThroughputPoints = ((stats.Hps[0] * cycle[0]) + (stats.Hps[1] * cycle[1]) + (stats.Hps[2] * cycle[2]) + (stats.Hps[3] * cycle[3]));
            stats.LongevityPoints *= 5;*/
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            _cachedCharacter = character;
            //int targetLevel = int.Parse(character.CalculationOptions["TargetLevel"]);
            Stats stats = GetCharacterStats(character, additionalItem);
            CharacterCalculationsHealadin calculatedStats = new CharacterCalculationsHealadin();
            calculatedStats.BasicStats = stats;

            calculatedStats[0] = new Spell("Flash of Light", 7);

            //TimeToOom(character, calculatedStats);

            calculatedStats.ThroughputPoints = calculatedStats[0].Hps;
            calculatedStats.LongevityPoints = stats.Mp5;
            calculatedStats.OverallPoints = calculatedStats.LongevityPoints + calculatedStats.ThroughputPoints;

            return calculatedStats;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace = new Stats() { Health = 3434, Mana = 2673, Stamina = 118, Intellect = 85, Spirit = 88 };
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTotal = new Stats();
            statsTotal.Stamina = (statsBaseGear.Stamina + statsEnchants.Stamina + statsRace.Stamina + statsBuffs.Stamina) * (1 + statsBuffs.BonusStaminaMultiplier);
            statsTotal.Intellect = (1.1f * (statsBaseGear.Intellect + statsEnchants.Intellect + statsBuffs.Intellect + statsRace.Intellect)) * (1 + statsBuffs.BonusIntellectMultiplier);
            statsTotal.Spirit = (statsRace.Spirit + statsBaseGear.Spirit + statsEnchants.Spirit + statsBuffs.Spirit) * (1 + statsBuffs.BonusSpiritMultiplier);
            statsTotal.Healing = statsBaseGear.Healing + statsBuffs.Healing + statsEnchants.Healing + (0.35f * statsTotal.Intellect) + (statsBuffs.SpellDamageFromSpiritPercentage * statsTotal.Spirit);
            statsTotal.Mp5 = statsBaseGear.Mp5 + statsEnchants.Mp5 + statsBuffs.Mp5;
            statsTotal.SpellCritRating = statsBaseGear.SpellCritRating + statsEnchants.SpellCritRating + statsBuffs.SpellCritRating;
            statsTotal.SpellHasteRating = statsBaseGear.SpellHasteRating + statsEnchants.SpellHasteRating + statsBuffs.SpellCritRating;
            statsTotal.Mana = statsRace.Mana + statsBaseGear.Mana + statsEnchants.Mana + (statsTotal.Intellect * 15);
            statsTotal.Health = ((statsRace.Health + statsBaseGear.Health + statsBuffs.Health + (statsTotal.Stamina * 10f)));
            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            return new ComparisonCalculationBase[0];
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
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Intellect + stats.Spirit + stats.Stamina + stats.Mp5 + stats.Healing + stats.SpellCritRating
                + stats.SpellHasteRating + stats.BonusSpiritMultiplier + stats.SpellDamageFromSpiritPercentage + stats.BonusIntellectMultiplier) > 0;
        }
    }
}
