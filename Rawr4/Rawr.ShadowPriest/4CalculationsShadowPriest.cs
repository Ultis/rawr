using System;
using System.Collections.Generic;
using System.Text;
using Rawr.ShadowPriest.Spells;
using System.Windows.Media;

namespace Rawr.ShadowPriest
{
    [Rawr.Calculations.RawrModelInfo("ShadowPriest", "Spell_Shadow_Shadowform", CharacterClass.Priest)]
    public class CalculationsShadowPriest : CalculationsBase
    {    
        #region started
        private List<GemmingTemplate> _defaultGemmingTemplates = null;
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                if (_defaultGemmingTemplates == null)
                {
                    //Meta
                    int chaotic = 0;

                    // [0] uncommon
                    // [1] perfect uncommon
                    // [2] rare
                    // [3] epic
                    // [4] jewelcrafting

                    //Red
                    //Blue
                    //Yellow
                    //Purple
                    //Green
                    //Orange
              
                    _defaultGemmingTemplates = new List<GemmingTemplate>();
                    //AddGemmingTemplateGroup(_defaultGemmingTemplates, "Uncommon", false, runed[0], royal[0], reckless[0], quick[0], dazzling[0], rigid[0], veiled[0], lambent[0], chaotic);
                }
             return _defaultGemmingTemplates;
            }
        }
    
        //1
        private Dictionary<string, Color> _subPointNameColors = null;
        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, Color>();
                    //_subPointNameColors.Add("Burst DPS", Color.FromArgb(255, 255, 0, 0));
                    _subPointNameColors.Add("Sustained DPS", Color.FromArgb(255, 0, 0, 255));
                }
                return _subPointNameColors;
            }
        }
        //2
        private string[] _customChartNames = {};
        public override string[] CustomChartNames
        {
            get
            {
                return _customChartNames;
            }
        }
         //3
        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[] {
					"Basic Stats:Health",
                    "Basic Stats:Resilience",
					"Basic Stats:Mana",
					"Basic Stats:Stamina",
					"Basic Stats:Intellect",
					"Basic Stats:Spirit",
					"Basic Stats:Spell Power",
					"Basic Stats:Regen",
					"Basic Stats:Crit",
					"Basic Stats:Hit",
					"Basic Stats:Haste",
                    "Basic Stats:Armor",
                    "Basic Stats:Resistance",
                    "Simulation:Rotation",
                    "Simulation:Castlist",
                    "Simulation:DPS",
//                    "Simulation:SustainDPS",
                    "Shadow:Vampiric Touch",
                    "Shadow:SW Pain",
                    "Shadow:Devouring Plague",
                    "Shadow:Imp. Devouring Plague",
				    "Shadow:SW Death",
                    "Shadow:Mind Blast",
                    "Shadow:Mind Flay",
                    "Shadow:Shadowfiend",
                    "Holy:PW Shield",
                    "Holy:Smite",
                    "Holy:Holy Fire",
                    "Holy:Penance"
                     
                };
                return _characterDisplayCalculationLabels;
            }
        }
        //4
        public ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelShadowPriest();
                }
                return _calculationOptionsPanel;
            }
        }
        //5
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsShadowPriest calcOpts = character.CalculationOptions as CalculationOptionsShadowPriest;

            Stats statsRace = BaseStats.GetBaseStats(character);
            //Stats statsItems = GetItemStats(character, additionalItem);
            //Stats statsBuffs = GetBuffsStats(character, calcOpts);
            //Stats statsTalents = GetTalentStats(character.ShamanTalents);

            Stats statsTotal = statsRace; //+ statsItems + statsBuffs + statsTalents;

            return statsTotal;
        }
        //6
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            CalculationOptionsShadowPriest calcOpts = character.CalculationOptions as CalculationOptionsShadowPriest;
            if (calcOpts == null) calcOpts = new CalculationOptionsShadowPriest();
            BossOptions bossOpts = character.BossOptions;
            if (bossOpts == null) bossOpts = new BossOptions();
            Stats stats = GetCharacterStats(character, additionalItem);

            CharacterCalculationsShadowPriest calculatedStats = new CharacterCalculationsShadowPriest();
            calculatedStats.BasicStats = stats;
            calculatedStats.LocalCharacter = character;

            return calculatedStats;
        }
        //7
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            return new ComparisonCalculationBase[0];
        }
        
        //8
        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<ItemType>(new ItemType[]{
                        ItemType.None,
                        ItemType.Cloth,
                        ItemType.Dagger,
                        ItemType.Wand,
                        ItemType.OneHandMace,
                        ItemType.Staff
                    });
                }
                return _relevantItemTypes;
            }
        }
        #endregion
        //9
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationShadowPriest(); }
        //10
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsShadowPriest(); }

        public override CharacterClass TargetClass
        {
            get { throw new NotImplementedException(); }
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            throw new NotImplementedException();
        }
        public override bool HasRelevantStats(Stats stats)
        {
            throw new NotImplementedException();
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
    public static class Constants
    {
        // Source: http://bobturkey.wordpress.com/2010/09/28/priest-base-mana-pool-and-mana-regen-coefficient-at-85/
        public static float BaseMana = 20590;
    }
}
