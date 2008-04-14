using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Rawr.Hunter
{
    [System.ComponentModel.DisplayName("Hunter|Inv_Weapon_Bow_07")]
    public class CalculationsHunter : CalculationsBase
    {
        #region Instance Variables

        private CalculationOptionsPanelBase calculationOptionsPanel = null;
        private string[] characterDisplayCalculationLabels = null;
        private string[] customChartNames = null;
        private List<Item.ItemType> relevantItemTypes = null;
        private Dictionary<string, Color> subPointNameColors = null;

        #endregion

        #region CalculationsBase Overrides

        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (calculationOptionsPanel == null)
                    calculationOptionsPanel = new CalculationOptionsPanelHunter();

                return calculationOptionsPanel;
            }
        }

        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (characterDisplayCalculationLabels == null)
                    characterDisplayCalculationLabels = new string[] {
					"Basic Stats:Stamina",
					"Basic Stats:Intellect",
					"Basic Stats:Spirit",
					"Basic Stats:Armor",
					"Basic Stats:Health",
					"Basic Stats:Mana",
                    "Spell Stats:Spell Crit Rate",
                    "Spell Stats:Spell Hit Rate",
                    "Spell Stats:Spell Penetration",
                    "Spell Stats:Casting Speed",
                    "Spell Stats:Arcane Damage",
                    "Spell Stats:Fire Damage",
                    "Spell Stats:Frost Damage",
                    "Solution:Total Damage",
                    "Solution:Dps",
                    "Solution:Tps*Threat per second",
                    "Solution:Spell Cycles",                   
                    "Survivability:Arcane Resist",
                    "Survivability:Fire Resist",
                    "Survivability:Nature Resist",
                    "Survivability:Frost Resist",
                    "Survivability:Shadow Resist",
                    "Survivability:Physical Mitigation",
                    "Survivability:Resilience",
                    "Survivability:Defense",
                    "Survivability:Crit Reduction",
                    "Survivability:Dodge",
				};

                return characterDisplayCalculationLabels;
            }
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsHunter();
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationHunter();
        }

        public override string[] CustomChartNames
        {
            get
            {
                if (customChartNames == null)
                    customChartNames = new string[]{};

                return customChartNames;
            }
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            return GetCharacterCalculations(character, additionalItem, false);
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CompiledCalculationOptions calculationOptions = new CompiledCalculationOptions(character);
            return GetCharacterStats(character, additionalItem, GetRawStats(character, additionalItem, calculationOptions, new List<string>(), null), calculationOptions);
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {            
            return new ComparisonCalculationBase[0];            
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats();
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return true;
        }
       
        public override List<Item.ItemType> RelevantItemTypes
        {
            get
            {
                if (relevantItemTypes == null)
                {
                    relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
					{
						Item.ItemType.None,
						Item.ItemType.AmmoPouch,
                        Item.ItemType.Arrow,
                        Item.ItemType.Bow,
                        Item.ItemType.Bullet,
                        Item.ItemType.Cloth,
                        Item.ItemType.Crossbow,
                        Item.ItemType.Dagger,
						Item.ItemType.FistWeapon,
                        Item.ItemType.Gun,
                        Item.ItemType.Leather,
                        Item.ItemType.Mail,
                        Item.ItemType.OneHandAxe,                        
                        Item.ItemType.OneHandSword,
                        Item.ItemType.Polearm,
                        Item.ItemType.Quiver,
                        Item.ItemType.Staff,                        
                        Item.ItemType.Thrown,
                        Item.ItemType.TwoHandAxe,
                        Item.ItemType.TwoHandSword
					});
                }

                return relevantItemTypes;
            }
        }

        
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (subPointNameColors == null)
                {
                    subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    subPointNameColors.Add("Dps", System.Drawing.Color.FromArgb(0, 128, 255));
                }

                return subPointNameColors;
            }
        }

   
        public override Character.CharacterClass TargetClass
        {
            get { return Character.CharacterClass.Hunter; }
        }

        #endregion

        #region Instance Variables

        public CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool computeIncrementalSet)
        {
            CompiledCalculationOptions calculationOptions = new CompiledCalculationOptions(character);
            CharacterCalculationsBase calc = GetCharacterCalculations(character, additionalItem, calculationOptions, null, computeIncrementalSet);
            return calc;           
        }

        public CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, CompiledCalculationOptions calculationOptions, string armor, bool computeIncrementalSet)
        {
            CharacterCalculationsHunter calculatedStats = null;
            
            return calculatedStats;
        }

        public Stats GetCharacterStats(Character character, Item additionalItem, Stats rawStats, CompiledCalculationOptions calculationOptions)
        {
            Stats statsRace;

            switch (character.Race)
            {
                case Character.CharacterRace.Dwarf:
                    statsRace = new Stats()
                    {
                        Health = 0f,
                        Mana = 0f,
                        Strength = 0f,
                        Agility = 0f,
                        Stamina = 0f,
                        Intellect = 0f,
                        Spirit = 0f                       
                    };
                    break;
                case Character.CharacterRace.NightElf:
                    statsRace = new Stats()
                    {
                        Health = 0f,
                        Mana = 0f,
                        Strength = 0f,
                        Agility = 0f,
                        Stamina = 0f,
                        Intellect = 0f,
                        Spirit = 0f      
                    };
                    break;
                case Character.CharacterRace.Orc:
                    statsRace = new Stats()
                    {
                        Health = 0f,
                        Mana = 0f,
                        Strength = 0f,
                        Agility = 0f,
                        Stamina = 0f,
                        Intellect = 0f,
                        Spirit = 0f      
                    };
                    break;
                case Character.CharacterRace.Tauren:
                    statsRace = new Stats()
                    {
                        Health = 0f,
                        Mana = 0f,
                        Strength = 0f,
                        Agility = 0f,
                        Stamina = 0f,
                        Intellect = 0f,
                        Spirit = 0f      
                    };
                    break;
                case Character.CharacterRace.Troll:
                    statsRace = new Stats()
                    {
                        Health = 0f,
                        Mana = 0f,
                        Strength = 0f,
                        Agility = 0f,
                        Stamina = 0f,
                        Intellect = 0f,
                        Spirit = 0f     
                    };
                    break;
                case Character.CharacterRace.BloodElf:
                    statsRace = new Stats()
                    {
                        Health = 0f,
                        Mana = 0f,
                        Strength = 0f,
                        Agility = 0f,
                        Stamina = 0f,
                        Intellect = 0f,
                        Spirit = 0f      
                    };
                    break;
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats()
                    {
                        Health = 0f,
                        Mana = 0f,
                        Strength = 0f,
                        Agility = 0f,
                        Stamina = 0f,
                        Intellect = 0f,
                        Spirit = 0f      
                    };
                    break;
                default:
                    statsRace = new Stats();
                    break;
            }

            return statsRace;
        }

        private Stats GetRawStats(Character character, Item additionalItem, CompiledCalculationOptions calculationOptions, List<string> autoActivatedBuffs, string armor)
        {
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            List<string> activeBuffs = new List<string>();
            activeBuffs.AddRange(character.ActiveBuffs);

            Stats statsBuffs = GetBuffsStats(activeBuffs);

            Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;

            return statsGearEnchantsBuffs;
        }

        #endregion

    }
}
