using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tankadin
{
    [System.ComponentModel.DisplayName("Tankadin|Spell_Holy_AvengersShield")]
    public class CalculationsTankadin : CalculationsBase
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
                    _calculationOptionsPanel = new CalculationOptionsPanelTankadin();
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
					"Basic Stats:Armor",
					"Basic Stats:Stamina",
					"Basic Stats:Agility",
					"Basic Stats:Defense",
					"Basic Stats:Miss",
					"Basic Stats:Dodge",
					"Basic Stats:Parry",
					"Basic Stats:Block",
					"Basic Stats:Block Value",
					"Basic Stats:Spell Damage",
					"Complex Stats:Avoidance",
					"Complex Stats:Mitigation",
					"Complex Stats:Total Mitigation",
					"Complex Stats:Chance to be Crit",
					@"Complex Stats:Overall Points*Overall Points are a sum of Mitigation and Survival Points. 
Overall is typically, but not always, the best way to rate gear. 
For specific encounters, closer attention to Mitigation and 
Survival Points individually may be important.",
					@"Complex Stats:Mitigation Points*Mitigation Points represent the amount of damage you mitigate, 
on average, through armor mitigation and avoidance. It is directly 
relational to your Damage Taken. Ideally, you want to maximize 
Mitigation Points, while maintaining 'enough' Survival Points 
(see Survival Points). If you find yourself dying due to healers 
running OOM, or being too busy healing you and letting other 
raid members die, then focus on Mitigation Points.",
					@"Complex Stats:Survival Points*Survival Points represents the total raw physical damage 
(pre-mitigation) you can take before dying. Unlike 
Mitigation Points, you should not attempt to maximize this, 
but rather get 'enough' of it, and then focus on Mitigation. 
'Enough' can vary greatly by fight and by your healers, but 
keeping it roughly even with Mitigation Points is a good 
way to maintain 'enough' as you progress. If you find that 
you are being killed by burst damage, focus on Survival Points.",
                     "Threat:Overall",
                     "Threat:Holy Shield",
                     "Threat:Seal of Right",
                     "Threat:Judgement of Right",
                     "Threat:Consecrate",
                     "Threat:Misc"
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
                    _subPointNameColors.Add("Mitigation", System.Drawing.Color.Red);
                    _subPointNameColors.Add("Survival", System.Drawing.Color.Blue);
                    _subPointNameColors.Add("Threat", System.Drawing.Color.DarkOliveGreen);
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

        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Paladin; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationTankadin(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsTankadin(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsTankadin));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsTankadin calcOpts = serializer.Deserialize(reader) as CalculationOptionsTankadin;
            return calcOpts;
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            CalculationOptionsTankadin calcOpts = character.CalculationOptions as CalculationOptionsTankadin;
            //_cachedCharacter = character;
            int targetLevel = calcOpts.TargetLevel;
            Stats stats = GetCharacterStats(character, additionalItem);
            Talents talents = new Talents();
            float targetDefense = targetLevel * 5;
            CharacterCalculationsTankadin calculatedStats = new CharacterCalculationsTankadin();
            calculatedStats.BasicStats = stats;
            calculatedStats.TargetLevel = targetLevel;


            //Avoidance calculations
            calculatedStats.Defense = 350 + (float)Math.Floor(stats.DefenseRating / (123f / 52f)) + talents.Anticipation;
            calculatedStats.Miss = 5 + (calculatedStats.Defense - targetDefense) * .04f + stats.Miss;
            calculatedStats.Parry = 5 + (calculatedStats.Defense - targetDefense) * .04f + stats.ParryRating / 23.6538461538462f + talents.Deflection;
            calculatedStats.Dodge = (calculatedStats.Defense - targetDefense) * .04f + stats.Agility / 25f + (stats.DodgeRating / (984f / 52f));
            calculatedStats.Avoidance = calculatedStats.Dodge + calculatedStats.Miss + calculatedStats.Parry;

            calculatedStats.Block = 5 + (calculatedStats.Defense - targetDefense) * .04f + stats.BlockRating / 7.884614944458f;
            calculatedStats.BlockValue = stats.BlockValue * talents.ShieldSpecializaiton;
            calculatedStats.CrushAvoidance = calculatedStats.Avoidance + calculatedStats.Block + 30;
            calculatedStats.CritAvoidance = (calculatedStats.Defense - targetDefense) * .04f + stats.Resilience / 39.423f;
            calculatedStats.Mitigation = Math.Min(75f, (stats.Armor / (stats.Armor - 22167.5f + (467.5f * targetLevel))) * 100f);

            float reduction = (1f - (calculatedStats.Mitigation * .01f)) * .96f;
            float attacks = calcOpts.NumberAttackers / calcOpts.AttackSpeed * 10;
            //Apply armor and multipliers for each attack type...
            float miss = Math.Min(0.01f * attacks * calculatedStats.Avoidance, attacks);
            float block = Math.Min(Math.Min(8, attacks * (.3f + 0.01f*calculatedStats.Block)), attacks - miss);
            if (block > 8) block += Math.Min((attacks - block) * .01f * calculatedStats.Block, attacks - miss - block);
            float crit = Math.Min(0.01f * Math.Max(5 - calculatedStats.CritAvoidance, 0) * attacks, attacks - miss - block);
            float crush = Math.Min((targetLevel == 73 ? .15f : 0f) * attacks, attacks - miss - block - crit);
            float hit = attacks - miss - block - crit - crush;

            float modifier = talents.OneHandSpec * (1 + (.6f * talents.ImpRF));
            calculatedStats.HolyShieldTPS = modifier * Math.Min(block, 8f) * 1.2f * 1.35f * (155 + .05f * stats.SpellDamageRating) / 10f;

            crit *= calcOpts.AverageHit * reduction * 2f;
            crush *= calcOpts.AverageHit * reduction * 1.5f;
            hit *= calcOpts.AverageHit * reduction;
            block *= calcOpts.AverageHit * reduction * Math.Max(1f - (calculatedStats.BlockValue / calcOpts.AverageHit / reduction), 0);
            calculatedStats.DamageTaken = (hit + crush + crit + block) / (attacks * calcOpts.AverageHit) * 100;
            calculatedStats.TotalMitigation = 100f - calculatedStats.DamageTaken;

            calculatedStats.SurvivalPoints = stats.Health / reduction;
            calculatedStats.MitigationPoints = calcOpts.MitigationScale * (1f * (1f / (calculatedStats.DamageTaken / 100f)));
            float ws = character.MainHand == null ? 0 : character.MainHand.Speed;
            float wd = character.MainHand == null ? 0 : ((character.MainHand.MinDamage + character.MainHand.MaxDamage) / 2f);
            calculatedStats.SoRTPS = ws == 0 ? 0 : ((0.85f * (2610.43f * ws / 100f) + 0.03f * wd - 1f + (0.108f * ws * stats.SpellDamageRating * ws)) / ws) * modifier;
            calculatedStats.ConsecrateTPS = calcOpts.NumberAttackers * (512 + .9524f * stats.SpellDamageRating) / 8f * modifier;
            calculatedStats.JoRTPS = (218f + stats.SpellDamageRating * .7143f) / 9f * modifier;
            calculatedStats.OverallTPS = calculatedStats.SoRTPS + calculatedStats.JoRTPS + 
                calculatedStats.HolyShieldTPS + calculatedStats.ConsecrateTPS + calculatedStats.MiscTPS;
            calculatedStats.ThreatPoints = calculatedStats.OverallTPS * calcOpts.ThreatScale;

            calculatedStats.OverallPoints = calculatedStats.MitigationPoints + calculatedStats.SurvivalPoints + calculatedStats.ThreatPoints;

            return calculatedStats;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace = new Stats() { Health = 3197, Mana = 2673, Stamina = 118, Intellect = 86, Spirit = 88, Agility = 79, DodgeRating = 12.3f };
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Talents talents = new Talents();

            Stats statsTotal = statsBaseGear + statsEnchants + statsBuffs + statsRace;
            statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1 + statsBuffs.BonusAgilityMultiplier));
            statsTotal.Stamina = (float)Math.Round(statsTotal.Stamina * (1 + statsBuffs.BonusStaminaMultiplier) * talents.SacredDuty * talents.CombatExpertise);
            statsTotal.Health = (float)Math.Round(statsTotal.Health + (statsTotal.Stamina * 10));
            statsTotal.Armor = (float)Math.Round((statsTotal.Armor + (statsTotal.Agility * 2f)) * (1 + statsBuffs.BonusArmorMultiplier) * talents.Thoughness);
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
                Armor = stats.Armor,
                Stamina = stats.Stamina,
                Agility = stats.Agility,
                DodgeRating = stats.DodgeRating,
                DefenseRating = stats.DefenseRating,
                Resilience = stats.Resilience,
                ParryRating = stats.ParryRating,
                BlockRating = stats.BlockRating,
                BlockValue = stats.BlockValue,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                Health = stats.Health,
                Miss = stats.Miss,
                SpellDamageRating = stats.SpellDamageRating,
                HitRating = stats.HitRating,
                SpellHitRating = stats.SpellHitRating,
                ArmorPenetration = stats.ArmorPenetration,
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Agility + stats.Armor + stats.BonusAgilityMultiplier + stats.BonusArmorMultiplier +
                stats.BonusStaminaMultiplier + stats.DefenseRating + stats.DodgeRating + stats.Health +
                stats.Miss + stats.Resilience + stats.Stamina + stats.ParryRating + stats.BlockRating + stats.BlockValue +
                stats.SpellHitRating + stats.SpellDamageRating + stats.HitRating + stats.ArmorPenetration) > 0;
        }
    }

}