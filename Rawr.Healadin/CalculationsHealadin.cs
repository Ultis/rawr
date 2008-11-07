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
					"Basic Stats:Spell Power",
					"Basic Stats:Mp5",
					"Basic Stats:Spell Crit",
					"Basic Stats:Spell Haste",
					"Cycle Stats:Total Healed",
					"Cycle Stats:Total Mana",
					"Cycle Stats:Average Hps",
					"Cycle Stats:Average Hpm",
                    "Flash of Light:FoL Average Heal",
                    "Flash of Light:FoL Crit",
                    "Flash of Light:FoL Cast Time",
                    "Flash of Light:FoL Healing per sec",
                    "Flash of Light:FoL Healing per mana",
                    "Flash of Light:FoL Rotation Time",
                    "Flash of Light:FoL Healed",
                    "Flash of Light:FoL Mana Usage",
                    "Holy Light:HL Average Heal",
                    "Holy Light:HL Crit",
                    "Holy Light:HL Cast Time",
                    "Holy Light:HL Healing per sec",
                    "Holy Light:HL Healing per mana",
                    "Holy Light:HL Rotation Time",
                    "Holy Light:HL Healed",
                    "Holy Light:HL Mana Usage",
                    "Holy Shock:HS Average Heal",
                    "Holy Shock:HS Crit",
                    "Holy Shock:HS Cast Time",
                    "Holy Shock:HS Healing per sec",
                    "Holy Shock:HS Healing per mana",
                    "Holy Shock:HS Rotation Time",
                    "Holy Shock:HS Healed",
                    "Holy Shock:HS Mana Usage",
                    "Beacon of Light:BoL Healed",
                    "Beacon of Light:BoL Casts",
                    "Beacon of Light:BoL Mana Usage",
                    "Judgement:JotP Effective Haste",
                    "Judgement:JotP Casts",
                    "Judgement:JotP Mana Usage"
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
                    "Mana Pool Breakdown",
                    "Mana Usage Breakdown"
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
            Stats stats = GetCharacterStats(character, additionalItem);
            CharacterCalculationsHealadin calc = new CharacterCalculationsHealadin();
            calc.BasicStats = stats;
            PaladinTalents talents = character.PaladinTalents;

			CalculationOptionsHealadin calcOpts = character.CalculationOptions as CalculationOptionsHealadin;
            if (calcOpts == null) calcOpts = new CalculationOptionsHealadin();

            float base_mana = 2953;// 4394;
            float fight_length = calcOpts.Length * 60;
			float active_length = fight_length * calcOpts.Activity;

            calc.ManaBase = stats.Mana;
            calc.ManaArcaneTorrent = (character.Race == Character.CharacterRace.BloodElf ? stats.Mana * .06f * (float)Math.Ceiling(fight_length / 60f - .25f) : 0);
            calc.ManaDivinePlea = stats.Mana * .25f * (float)Math.Ceiling((fight_length - 60f) / (60f * calcOpts.DivinePlea));
            calc.ManaMp5 = fight_length * stats.Mp5 / 5;
            calc.ManaPotion = (1 + stats.BonusManaPotion) * calcOpts.ManaAmt;
            calc.ManaReplenishment = stats.ManaRestoreFromMaxManaPerSecond * stats.Mana * fight_length * calcOpts.Replenishment;
            calc.ManaSpiritual = calcOpts.Spiritual;
            if (stats.MementoProc > 0)
            {
                calc.ManaOther+= (float)Math.Ceiling(fight_length / 60f - .25f) * stats.MementoProc * 3f;
            }
            calc.TotalMana = calc.ManaBase + calc.ManaDivinePlea + calc.ManaMp5 + calc.ManaOther + calc.ManaPotion + calc.ManaReplenishment + calc.ManaSpiritual;

            float benediction = 1f - talents.Benediction * .02f;

            if (calcOpts.JotP && talents.JudgementsOfThePure > 0)
            {
                float oldhaste = fight_length * (1f + stats.SpellHaste);
                stats.SpellHaste += talents.JudgementsOfThePure * .03f;

                float seals_cast = (float)Math.Ceiling((fight_length - 60f) / 120f);
                calc.JotPCasts += seals_cast;
                calc.JotPUsage += (float)Math.Round(base_mana * .14f) * seals_cast;

                float judgements_cast = (float)Math.Ceiling(fight_length / 60f);
                calc.JotPCasts += judgements_cast;
                calc.JotPUsage += (float)Math.Round(base_mana * .05f) * judgements_cast;

                float newhaste = fight_length * (1f + stats.SpellHaste) - (float)Math.Max(1f, 1.5f / (1f + stats.SpellHaste)) * calc.JotPCasts;
                calc.JotPHaste = 1f - oldhaste / newhaste;
            }
            if (talents.BeaconOfLight > 0)
            {
                calc.BoLCasts = (float)Math.Ceiling(fight_length * calcOpts.BoLUp / 60f);
                calc.BoLUsage = calc.BoLCasts * (float)Math.Round(base_mana * .35f * benediction);
            }

            float ied = stats.ManaRestorePerCast_5_15 * .035f; 
            
            #region Flash of Light
            calc.FoLAvgHeal = (635f + stats.SpellPower + stats.FoLHeal) * (1f + talents.HealingLight * .04f) * (1f + stats.FoLMultiplier);
            float fol_baseMana = (float)Math.Round(base_mana * .07f);
            calc.FoLCrit = stats.SpellCrit + stats.FoLCrit + talents.HolyPower * .01f;
            calc.FoLCost = fol_baseMana * (1 - .12f * talents.Illumination * calc.FoLCrit) - ied;
            float fol_heal = calc.FoLAvgHeal * (1f + .5f * calc.FoLCrit);
            calc.FoLCastTime = (float)Math.Max(1f, 1.5f / (1f + stats.SpellHaste));
            calc.FoLHPS = fol_heal / calc.FoLCastTime;
            float fol_mps = calc.FoLCost / calc.FoLCastTime;
            calc.FoLHPM = fol_heal / calc.FoLCost;
            #endregion

            #region Holy Light
            calc.HLAvgHeal = (2978f + (stats.HLHeal + stats.SpellPower) * 1.66f) * (1f + talents.HealingLight * .04f);
            float hl_baseMana = (float)Math.Round(base_mana * .29f);
            calc.HLCrit = stats.SpellCrit + stats.HLCrit + talents.HolyPower * .01f + talents.SanctifiedLight * .02f;
            calc.HLCost = hl_baseMana * (1 - .12f * talents.Illumination * calc.HLCrit) - ied;
            float hl_heal = calc.HLAvgHeal * (1f + .5f * calc.HLCrit);
            calc.HLCastTime = 2f / (1f + stats.SpellHaste);
            calc.HLHPS = hl_heal / calc.HLCastTime;
            float hl_mps = calc.HLCost / calc.HLCastTime;
            calc.HLHPM = hl_heal / calc.HLCost;
            #endregion

            #region Holy Shock
            calc.HSAvgHeal = (1310f + stats.SpellPower) * (1f + talents.HealingLight * .04f);
            float hs_baseMana = (float)Math.Round(base_mana * .18f * benediction);
            calc.HSCrit = stats.SpellCrit + talents.HolyPower * .01f + talents.SanctifiedLight * .02f;
            calc.HSCost = hs_baseMana * (1 - .12f * talents.Illumination * calc.HSCrit) - ied;
            float hs_heal = calc.HSAvgHeal * (1f + .5f * calc.HSCrit);
            calc.HSCastTime = (float)Math.Max(1f, 1.5f / (1f + stats.SpellHaste));
            calc.HSHPS = hs_heal / calc.HSCastTime;
            float hs_mps = calc.HSCost / calc.HSCastTime;
            calc.HSHPM = hs_heal / calc.HSCost;
            #endregion

            if (talents.HolyShock > 0 && calcOpts.HolyShock > 0)
            {
                calc.HSTime = (float)(Math.Floor(fight_length * calcOpts.HolyShock / 6f) * calc.HSCastTime);
                calc.HSHealed = calc.HSTime * calc.HSHPS;
                calc.HSUsage = calc.HSTime * hs_mps;
            }

            float healing_mana = calc.TotalMana - calc.BoLUsage - calc.JotPUsage - calc.HSUsage;
            float healing_time = active_length - (calc.HSCastTime * (calc.BoLCasts + calc.JotPCasts)) - calc.HSTime;
            calc.HLTime = Math.Min(active_length, Math.Max(0, (healing_mana - (healing_time * fol_mps)) / (hl_mps - fol_mps)));
            calc.FoLTime = healing_time - calc.HLTime;
            if (calc.HLTime == 0)
            {
                calc.FoLTime = Math.Min(healing_time, healing_mana / fol_mps);
            }

            calc.FoLUsage = calc.FoLTime * fol_mps;
            calc.HLUsage = calc.HLTime * hl_mps;

            calc.FoLHealed = calc.FoLTime * calc.FoLHPS;
            calc.HLHealed = calc.HLTime * calc.HLHPS;
            
            calc.TotalHealed = calc.FoLHealed + calc.HLHealed + calc.HSHealed;
            if (talents.BeaconOfLight > 0) calc.TotalHealed += calc.BoLHealed = calcOpts.BoLEff * calcOpts.BoLUp * calc.TotalHealed;

            calc.AvgHPM = calc.TotalHealed / calc.TotalMana;
            calc.OverallPoints = calc.ThroughputPoints = calc.AvgHPS = calc.TotalHealed / fight_length;
            return calc;
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
                statsRace = new Stats() { Health = 3197, Mana = 2672, Stamina = 118, Intellect = 87, Spirit = 88 };
            }


            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTotal = statsBaseGear + statsEnchants + statsBuffs + statsRace;
            statsTotal.Stamina = (float)Math.Round(statsTotal.Stamina * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Round(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier) * (1 + talents.DivineIntellect * .03f));
            statsTotal.Spirit = (float)Math.Round(statsTotal.Spirit * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower + (0.2f * statsTotal.Intellect));
            statsTotal.SpellCrit = .03336f + statsTotal.SpellCrit + statsTotal.Intellect / 8000 + statsTotal.CritRating / 2208 +
                talents.SanctifiedSeals * .01f + talents.Conviction * .01f;
            statsTotal.SpellHaste += statsTotal.HasteRating / 1570;
            statsTotal.Mana = statsTotal.Mana + (statsTotal.Intellect * 15);
            statsTotal.Health = statsTotal.Health + (statsTotal.Stamina * 10f);

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            if (chartName == "Mana Pool Breakdown")
            {
                CharacterCalculationsHealadin calc = GetCharacterCalculations(character) as CharacterCalculationsHealadin;
                if (calc == null) calc = new CharacterCalculationsHealadin();

                ComparisonCalculationHealadin Base = new ComparisonCalculationHealadin("Base");
                ComparisonCalculationHealadin Mp5 = new ComparisonCalculationHealadin("Mp5");
                ComparisonCalculationHealadin Potion = new ComparisonCalculationHealadin("Potion");
                ComparisonCalculationHealadin Replenishment = new ComparisonCalculationHealadin("Replenishment");
                ComparisonCalculationHealadin ArcaneTorrent = new ComparisonCalculationHealadin("Arcane Torrent");
                ComparisonCalculationHealadin DivinePlea = new ComparisonCalculationHealadin("Divine Plea");
                ComparisonCalculationHealadin Spiritual = new ComparisonCalculationHealadin("Spiritual Atunement");
                ComparisonCalculationHealadin Other = new ComparisonCalculationHealadin("Other");

                Base.OverallPoints = Base.ThroughputPoints = calc.ManaBase;
                Mp5.OverallPoints = Mp5.ThroughputPoints = calc.ManaMp5;
                Potion.OverallPoints = Potion.ThroughputPoints = calc.ManaPotion;
                Replenishment.OverallPoints = Replenishment.ThroughputPoints = calc.ManaReplenishment;
                ArcaneTorrent.OverallPoints = ArcaneTorrent.ThroughputPoints = calc.ManaArcaneTorrent;
                DivinePlea.OverallPoints = DivinePlea.ThroughputPoints = calc.ManaDivinePlea;
                Spiritual.OverallPoints = Spiritual.ThroughputPoints = calc.ManaSpiritual;
                Other.OverallPoints = Other.ThroughputPoints = calc.ManaOther;

                return new ComparisonCalculationBase[] { Base, Mp5, Potion, Replenishment, ArcaneTorrent, DivinePlea, Spiritual, Other };
            }
            else if (chartName == "Mana Usage Breakdown")
            {
                CharacterCalculationsHealadin calc = GetCharacterCalculations(character) as CharacterCalculationsHealadin;
                if (calc == null) calc = new CharacterCalculationsHealadin();

                ComparisonCalculationHealadin FoL = new ComparisonCalculationHealadin("Flash of Light");
                ComparisonCalculationHealadin HL = new ComparisonCalculationHealadin("Holy Light");
                ComparisonCalculationHealadin HS = new ComparisonCalculationHealadin("Holy Shock");
                ComparisonCalculationHealadin JotP = new ComparisonCalculationHealadin("Judgements and Seals");
                ComparisonCalculationHealadin BoL = new ComparisonCalculationHealadin("Beacon of Light");

                FoL.OverallPoints = FoL.ThroughputPoints = calc.FoLUsage;
                HL.OverallPoints = HL.ThroughputPoints = calc.HLUsage;
                HS.OverallPoints = HS.ThroughputPoints = calc.HSUsage;
                JotP.OverallPoints = JotP.ThroughputPoints = calc.JotPUsage;
                BoL.OverallPoints = BoL.ThroughputPoints = calc.BoLUsage;

                return new ComparisonCalculationBase[] { FoL, HL, HS, JotP, BoL };
            }
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
                SpellCrit = stats.SpellCrit,
                SpellHaste = stats.SpellHaste,
                FoLBoL = stats.FoLBoL,
                FoLCrit = stats.FoLCrit,
                FoLHeal = stats.FoLHeal,
                FoLMultiplier = stats.FoLMultiplier,
                HLHeal = stats.HLHeal,
                HLCrit = stats.HLCrit,
                HLCost = stats.HLCost,
                HLBoL = stats.HLBoL,
                MementoProc = stats.MementoProc,
                AverageHeal = stats.AverageHeal,
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Intellect + stats.Spirit + stats.Mp5 + stats.SpellPower + stats.CritRating + stats.SpellCrit + stats.SpellHaste
                + stats.HasteRating + stats.BonusSpiritMultiplier + stats.SpellDamageFromSpiritPercentage + stats.BonusIntellectMultiplier
                + stats.BonusManaPotion + stats.FoLMultiplier + stats.FoLHeal + stats.FoLCrit + stats.FoLBoL + stats.HLBoL + stats.HLCost
                + stats.HLCrit + stats.HLHeal + stats.MementoProc + stats.AverageHeal + stats.ManaRestoreFromMaxManaPerSecond) > 0;
        }
    }
}
