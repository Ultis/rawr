using System;
using System.Collections.Generic;

namespace Rawr.Healadin
{
	[Rawr.Calculations.RawrModelInfo("Healadin", "Spell_Holy_HolyBolt", Character.CharacterClass.Paladin)]
	public class CalculationsHealadin : CalculationsBase
    {
        
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
					"Basic Stats:Spell Crit",
					"Basic Stats:Spell Haste",
					"Cycle Stats:Total Healed",
					"Cycle Stats:Average Hps",
					"Cycle Stats:Average Hpm",
					"Cycle Stats:Holy Light Time",
					"Cycle Stats:Holy Light Healing",
                    "Spell Stats:Flash of Light",
                    "Spell Stats:Holy Light",
                    "Spell Stats:Holy Shock"
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
                    //"Healing per second",
                    //"Healing per mana",
                    //"Mana per second",
                    //"Average heal"
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
                    _subPointNameColors.Add("Fight HPS", System.Drawing.Color.Red);
                    //_subPointNameColors.Add("Longevity", System.Drawing.Color.Blue);
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
                        Item.ItemType.Mail,
                        Item.ItemType.Leather,
                        Item.ItemType.Cloth,
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

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Paladin; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationHealadin(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsHealadin(); }

		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsHealadin));
			System.IO.StringReader reader = new System.IO.StringReader(xml);
			CalculationOptionsHealadin calcOpts = serializer.Deserialize(reader) as CalculationOptionsHealadin;
			return calcOpts;
		}

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            //_cachedCharacter = character;
            Stats stats = GetCharacterStats(character, additionalItem);
            CharacterCalculationsHealadin calculatedStats = new CharacterCalculationsHealadin();
            //CharacterCalculationsHealadin oldStats = _cachedCharacterStatsWithSlotEmpty as CharacterCalculationsHealadin;
            calculatedStats.BasicStats = stats;
            PaladinTalents talents = character.PaladinTalents;

			CalculationOptionsHealadin calcOpts = character.CalculationOptions as CalculationOptionsHealadin;
            if (calcOpts == null) calcOpts = new CalculationOptionsHealadin();
			float activity = calcOpts.Activity / 100f;
            float baseMana = 2672;
            float time = calcOpts.Length * 60;
			float length = time * activity;
            float totalMana = stats.Mana + (time * stats.Mp5 / 5) + (calcOpts.Spriest > 0 ? stats.Mana * .0025f * time : 0) +
                ((1 + stats.BonusManaPotion) * calcOpts.ManaAmt) + calcOpts.Spiritual;
            if (stats.MementoProc > 0)
            {
                totalMana += (float)Math.Ceiling(time / 60 - .25) * stats.MementoProc * 3;
            }

            #region Flash of Light
            float fol_heal = (623f + stats.SpellPower + stats.FoLHeal) * (1f + talents.HealingLight * .04f) * (1f + stats.FoLMultiplier);
            float fol_baseMana = baseMana * .07f;
            float fol_crit = stats.SpellCrit + stats.FoLCrit + talents.HolyPower * .01f;
            float fol_avgMana = fol_baseMana * (1 - .6f * fol_crit);
            float fol_avgHeal = fol_heal * (1f + .5f * fol_crit);
            float fol_castTime = 1.5f / (1f + stats.SpellHaste);
            float fol_hps = fol_avgHeal / fol_castTime;
            float fol_mps = fol_avgMana / fol_castTime;
            #endregion

            #region Holy Light
            float hl_heal = (2978f + (stats.HLHeal + stats.SpellPower) * 1.66f) * (1f + talents.HealingLight * .04f);
            float hl_baseMana = baseMana * .29f;
            float hl_crit = stats.SpellCrit + stats.HLCrit + talents.HolyPower * .01f + talents.SanctifiedLight * .02f;
            float hl_avgMana = hl_baseMana * (1 - .6f * hl_crit);
            float hl_avgHeal = hl_heal * (1f + .5f * hl_crit);
            float hl_castTime = 2f / (1f + stats.SpellHaste);
            float hl_hps = hl_avgHeal / hl_castTime;
            float hl_mps = hl_avgMana / hl_castTime;
            #endregion

            float hl_time = Math.Min(length, Math.Max(0, (totalMana - (length * fol_mps)) / (hl_mps - fol_mps)));
            float fol_time = length - hl_time;
            if (hl_time == 0)
            {
                fol_time = Math.Min(length, totalMana / fol_mps);
            }
            calculatedStats.TimeHL = hl_time / length;

            float fol_healing = fol_time * fol_mps;
            float hl_healing = hl_time * hl_mps;

            calculatedStats.Healed += fol_healing + hl_healing;
            calculatedStats.HLHPS = fol_hps;
            calculatedStats.FoLHPS = hl_hps;

            calculatedStats.OverallPoints = calculatedStats.ThroughputPoints = calculatedStats.Healed / time;

            return calculatedStats;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            PaladinTalents talents = character.PaladinTalents;

            Stats statsRace;
            if (character.Race == Character.CharacterRace.Draenei)
            {
                statsRace = new Stats() { Health = 3197, Mana = 2672, Stamina = 123, Intellect = 84, Spirit = 91 };
            }
            else if (character.Race == Character.CharacterRace.Dwarf)
            {
                statsRace = new Stats() { Health = 3197, Mana = 2672, Stamina = 129, Intellect = 82, Spirit = 88 };
            }
            else if (character.Race == Character.CharacterRace.Human)
            {
                statsRace = new Stats() { Health = 3197, Mana = 2672, Stamina = 126, Intellect = 83, Spirit = 90, BonusSpiritMultiplier = 1.1f };
            }
            else
            {
                statsRace = new Stats() { Health = 3197, Mana = 2672, Stamina = 118, Intellect = 86, Spirit = 88 };
            }


            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTotal = statsBaseGear + statsEnchants + statsBuffs + statsRace;
            statsTotal.Stamina = (float)Math.Round(statsTotal.Stamina * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Round(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier) * (1 + talents.DivineIntellect * .03f));
            statsTotal.Spirit = (float)Math.Round(statsTotal.Spirit * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower + (0.2f * statsTotal.Intellect));
            statsTotal.SpellCrit = .03336f + statsTotal.Crit + statsTotal.Intellect / 8000 + statsTotal.CritRating / 2208 +
                talents.SanctifiedSeals * .01f + talents.Conviction * .01f;
            statsTotal.SpellHaste = statsTotal.HasteRating / 1570;
            statsTotal.Mana = statsTotal.Mana + (statsTotal.Intellect * 15);
            statsTotal.Health = statsTotal.Health + (statsTotal.Stamina * 10f);
            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            //CharacterCalculationsHealadin calc = GetCharacterCalculations(character) as CharacterCalculationsHealadin;
            //if (calc == null) calc = new CharacterCalculationsHealadin();
            //ComparisonCalculationHealadin FoL = new ComparisonCalculationHealadin("Flash of Light");
            //ComparisonCalculationHealadin HL11 = new ComparisonCalculationHealadin("Holy Light 11");
            //ComparisonCalculationHealadin HL10 = new ComparisonCalculationHealadin("Holy Light 10");
            //ComparisonCalculationHealadin HL9 = new ComparisonCalculationHealadin("Holy Light 9");
            //ComparisonCalculationHealadin HL8 = new ComparisonCalculationHealadin("Holy Light 8");
            //ComparisonCalculationHealadin HL7 = new ComparisonCalculationHealadin("Holy Light 7");
            //ComparisonCalculationHealadin HL6 = new ComparisonCalculationHealadin("Holy Light 6");
            //ComparisonCalculationHealadin HL5 = new ComparisonCalculationHealadin("Holy Light 5");
            //ComparisonCalculationHealadin HL4 = new ComparisonCalculationHealadin("Holy Light 4");

            //CalculationOptionsHealadin calcOpts = character.CalculationOptions as CalculationOptionsHealadin;
            //if (calcOpts == null) calcOpts = new CalculationOptionsHealadin();

            //calc[0] = new Spell("Flash of Light", 7, calcOpts.BoL);
            //calc[1] = new Spell("Holy Light", 11, calcOpts.BoL);
            //calc[2] = new Spell("Holy Light", 10, calcOpts.BoL);
            //calc[3] = new Spell("Holy Light", 9, calcOpts.BoL);
            //calc[4] = new Spell("Holy Light", 8, calcOpts.BoL);
            //calc[5] = new Spell("Holy Light", 7, calcOpts.BoL);
            //calc[6] = new Spell("Holy Light", 6, calcOpts.BoL);
            //calc[7] = new Spell("Holy Light", 5, calcOpts.BoL);
            //calc[8] = new Spell("Holy Light", 4, calcOpts.BoL);

            //switch (chartName)
            //{
            //    case "Healing per second":
            //        FoL.OverallPoints = FoL.ThroughputPoints = calc[0].Hps;
            //        HL11.OverallPoints = HL11.ThroughputPoints = calc[1].Hps;
            //        HL10.OverallPoints = HL10.ThroughputPoints = calc[2].Hps;
            //        HL9.OverallPoints = HL9.ThroughputPoints = calc[3].Hps;
            //        HL8.OverallPoints = HL8.ThroughputPoints = calc[4].Hps;
            //        HL7.OverallPoints = HL7.ThroughputPoints = calc[5].Hps;
            //        HL6.OverallPoints = HL6.ThroughputPoints = calc[6].Hps;
            //        HL5.OverallPoints = HL5.ThroughputPoints = calc[7].Hps;
            //        HL4.OverallPoints = HL4.ThroughputPoints = calc[8].Hps;
            //        break;
            //    case "Average heal":
            //        FoL.OverallPoints = FoL.ThroughputPoints = calc[0].AverageHeal;
            //        HL11.OverallPoints = HL11.ThroughputPoints = calc[1].AverageHeal;
            //        HL10.OverallPoints = HL10.ThroughputPoints = calc[2].AverageHeal;
            //        HL9.OverallPoints = HL9.ThroughputPoints = calc[3].AverageHeal;
            //        HL8.OverallPoints = HL8.ThroughputPoints = calc[4].AverageHeal;
            //        HL7.OverallPoints = HL7.ThroughputPoints = calc[5].AverageHeal;
            //        HL6.OverallPoints = HL6.ThroughputPoints = calc[6].AverageHeal;
            //        HL5.OverallPoints = HL5.ThroughputPoints = calc[7].AverageHeal;
            //        HL4.OverallPoints = HL4.ThroughputPoints = calc[8].AverageHeal;
            //        break;
            //    case "Healing per mana":
            //        FoL.OverallPoints = FoL.LongevityPoints = calc[0].Hpm;
            //        HL11.OverallPoints = HL11.LongevityPoints = calc[1].Hpm;
            //        HL10.OverallPoints = HL10.LongevityPoints = calc[2].Hpm;
            //        HL9.OverallPoints = HL9.LongevityPoints = calc[3].Hpm;
            //        HL8.OverallPoints = HL8.LongevityPoints = calc[4].Hpm;
            //        HL7.OverallPoints = HL7.LongevityPoints = calc[5].Hpm;
            //        HL6.OverallPoints = HL6.LongevityPoints = calc[6].Hpm;
            //        HL5.OverallPoints = HL5.LongevityPoints = calc[7].Hpm;
            //        HL4.OverallPoints = HL4.LongevityPoints = calc[8].Hpm;
            //        break;
            //    case "Mana per second":
            //        FoL.OverallPoints = FoL.LongevityPoints = calc[0].Mps;
            //        HL11.OverallPoints = HL11.LongevityPoints = calc[1].Mps;
            //        HL10.OverallPoints = HL10.LongevityPoints = calc[2].Mps;
            //        HL9.OverallPoints = HL9.LongevityPoints = calc[3].Mps;
            //        HL8.OverallPoints = HL8.LongevityPoints = calc[4].Mps;
            //        HL7.OverallPoints = HL7.LongevityPoints = calc[5].Mps;
            //        HL6.OverallPoints = HL6.LongevityPoints = calc[6].Mps;
            //        HL5.OverallPoints = HL5.LongevityPoints = calc[7].Mps;
            //        HL4.OverallPoints = HL4.LongevityPoints = calc[8].Mps;
            //        break;
            //}

            //return new ComparisonCalculationBase[] { FoL, HL11, HL10, HL9, HL8, HL7, HL6, HL5, HL4 };
            return new ComparisonCalculationBase[] {};
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
                FoLBoL = stats.FoLBoL,
                FoLCrit = stats.FoLCrit,
                FoLHeal = stats.FoLHeal,
                FoLMultiplier = stats.FoLMultiplier,
                HLHeal = stats.HLHeal,
                HLCrit = stats.HLCrit,
                HLCost = stats.HLCost,
                HLBoL = stats.HLBoL,
                MementoProc = stats.MementoProc,
                AverageHeal = stats.AverageHeal
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Intellect + stats.Spirit + stats.Mp5 + stats.SpellPower * 1.88f + stats.CritRating
                + stats.HasteRating + stats.BonusSpiritMultiplier + stats.SpellDamageFromSpiritPercentage + stats.BonusIntellectMultiplier
                + stats.BonusManaPotion + stats.FoLMultiplier + stats.FoLHeal + stats.FoLCrit + stats.FoLBoL + stats.HLBoL + stats.HLCost
                + stats.HLCrit + stats.HLHeal + stats.MementoProc + stats.AverageHeal) > 0;
        }
    }
}
