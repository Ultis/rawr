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


        //TODO:
        public override Dictionary<string, Color> SubPointNameColors
        {
            get { throw new NotImplementedException(); }
        }
        public override string[] CharacterDisplayCalculationLabels
        {
            get { throw new NotImplementedException(); }
        }
        public override string[] CustomChartNames
        {
            get { throw new NotImplementedException(); }
        }
        public override ICalculationOptionsPanel CalculationOptionsPanel
        {
            get { throw new NotImplementedException(); }
        }
        public override List<ItemType> RelevantItemTypes
        {
            get { throw new NotImplementedException(); }
        }
        public override CharacterClass TargetClass
        {
            get { throw new NotImplementedException(); }
        }
        public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            throw new NotImplementedException();
        }
        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            throw new NotImplementedException();
        }
        public override CharacterCalculationsBase  GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
 	        throw new NotImplementedException();
        }
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            throw new NotImplementedException();
        }
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            throw new NotImplementedException();
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
