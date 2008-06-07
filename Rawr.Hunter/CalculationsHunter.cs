using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter
{
    [System.ComponentModel.DisplayName("Hunter|Ability_Hunter_BeastTaming")]
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
					    "DPS Stats:Agility",
					    "DPS Stats:Attack Power",
					    "DPS Stats:Crit Rating",
				        "DPS Stats:Hit Rating",
                        "DPS Stats:Haste Rating",
					    "DPS Stats:Armor Penetration",
                        "DPS Stats:% Chance to Hit*% Chance to hit vs. level 73",
                        "DPS Stats:% Chance to Crit*% Chance to crit vs. level 73",
                        "Endurance Stats:Intellect",
                        "Endurance Stats:Mana",
                        "Endurance Stats:Mp5",
                        "Survival Stats:Health",
                        "Survival Stats:Armor",
                        "Survival Stats:% Chance to Dodge",
					    "Rotation:Attack Speed",
                        "Rotation:Mp5 Used",
                        "Rotation:Mp5 Regen",
                        "Rotation:Time until OOM*Time until out of mana",
					    "Hunter DPS:Auto Shot DPS",
                        "Hunter DPS:Special Shot DPS",
                        "Hunter DPS:Total Hunter DPS",
					    "Pet DPS:Melee DPS",
                        "Pet DPS:Special DPS",
                        "Pet DPS:Killing Command DPS",
                        "Pet DPS:Total Pet DPS",
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
                    _subPointNameColors.Add("Total DPS", System.Drawing.Color.Red);
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
            int targetLevel = calcOpts.TargetLevel;
            float targetArmor = calcOpts.TargetArmor;
            string shattrathFaction = calcOpts.ShattrathFaction;
            Stats stats = GetCharacterStats(character, additionalItem);

            CharacterCalculationsHunter calculatedStats = new CharacterCalculationsHunter();
            calculatedStats.BasicStats = stats;
            calculatedStats.TargetLevel = targetLevel;

            if (stats.ShatteredSunMightProc > 0)
            {
                switch (shattrathFaction)
                {
                    case "Aldor":
                        stats.AttackPower += 39.13f;
                        break;
                }
            }

            calculatedStats.DPSPoints = 0.0f;

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
