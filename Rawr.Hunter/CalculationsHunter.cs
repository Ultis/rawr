using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter
{
    [System.ComponentModel.DisplayName("Hunter|temp")]
    public class CalculationsHunter : CalculationsBase
    {
  
        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelHunter();
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
                    _characterDisplayCalculationLabels = new string[]
                    {
					    "Basic Stats:Agility",
					    "Basic Stats:Attack Power",
					    "Basic Stats:Crit Rating",
				        "Basic Stats:Hit Rating",
                        "Basic Stats:Intellect",
					    "Basic Stats:Haste Rating",
					    "Basic Stats:Armor Penetration",
                        "Basic Stats:Health",
                        "Basic Stats:Armor",
                        "Basic Stats:Mana",
					    "Not Basic Stats:Attack Speed",
                        "Not Basic Stats:% Chance to Hit*% Chance to hit vs. level 73",
                        "Not Basic Stats:% Chance to Crit*% Chance to crit vs. level 73",
                        "Not Basic Stats:% Chance to Dodge",
					    "Not Basic Stats:Hunter DPS",
					    "Not Basic Stats:Pet DPS",
					    "Not Basic Stats:Total DPS*Your DPS and your pet's DPS combined"
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
                        _customChartNames = new string[] {};
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
                    _subPointNameColors.Add("Hunter DPS", System.Drawing.Color.Red);
                    _subPointNameColors.Add("Total DPS", System.Drawing.Color.Blue);
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
						Item.ItemType.None,
						Item.ItemType.Leather,
                        Item.ItemType.Mail,
						Item.ItemType.Gun,
                        Item.ItemType.Bow,
                        Item.ItemType.Bullet,
                        Item.ItemType.AmmoPouch,
                        Item.ItemType.Crossbow,
                        Item.ItemType.Arrow,
                        Item.ItemType.Quiver,
                        Item.ItemType.Dagger,
                        Item.ItemType.FistWeapon,
                        Item.ItemType.OneHandAxe,
                        Item.ItemType.OneHandSword,
                       	Item.ItemType.Staff,
                        Item.ItemType.Polearm,
                        Item.ItemType.TwoHandAxe,
                        Item.ItemType.TwoHandSword
					});
                }
                return _relevantItemTypes;
            }
        }

        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Hunter; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationHunter(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsHunter(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsHunter));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsHunter calcOpts = serializer.Deserialize(reader) as CalculationOptionsHunter;
            return calcOpts;
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            CalculationOptionsHunter calcOpts = character.CalculationOptions as CalculationOptionsHunter;

            //_cachedCharacter = character;
            int targetLevel = calcOpts.TargetLevel;
            Stats stats = GetCharacterStats(character, additionalItem);
            // Talents talents = new Talents();
            float targetDefense = targetLevel * 5;
            CharacterCalculationsHunter calculatedStats = new CharacterCalculationsHunter();
            calculatedStats.BasicStats = stats;
            calculatedStats.TargetLevel = targetLevel;

            return calculatedStats;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTotal = statsBaseGear + statsEnchants + statsBuffs;           
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
					Agility = stats.Agility,
					AttackPower = stats.AttackPower,
					CritRating = stats.CritRating,
					HitRating = stats.HitRating,
					Stamina = stats.Stamina,
					HasteRating = stats.HasteRating,
                    ArmorPenetration = stats.ArmorPenetration,
					BloodlustProc = stats.BloodlustProc,
					TerrorProc = stats.TerrorProc,
					WeaponDamage = stats.WeaponDamage,
					BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
					BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
					BonusCritMultiplier = stats.BonusCritMultiplier,
					BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
					BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
					Health = stats.Health,
					ShatteredSunMightProc = stats.ShatteredSunMightProc
				};
		}

		public override bool HasRelevantStats(Stats stats)
		{
			return (stats.Agility + stats.ArmorPenetration + stats.AttackPower + stats.BonusAgilityMultiplier +
                stats.BonusAttackPowerMultiplier + stats.BonusCritMultiplier +stats.BonusPhysicalDamageMultiplier +
				stats.BonusStaminaMultiplier + stats.BonusStrengthMultiplier + stats.CritRating + stats.HasteRating +
                stats.Health + stats.HitRating + stats.Stamina + stats.ShatteredSunMightProc > 0 || (stats.SpellDamageRating == 0));
		}
    }
}
