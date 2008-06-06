using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Rawr.Hunter
{
    public class CompiledCalculationOptions
    {
        public int TargetLevel { get; set; }
        
        public CompiledCalculationOptions(Character character)
        {
            TargetLevel = int.Parse(character.CalculationOptions["TargetLevel"], CultureInfo.InvariantCulture);        
        }
    }

    public class CharacterCalculationsHunter : CharacterCalculationsBase
    {
        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            return dictValues;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            return base.GetOptimizableCalculationValue(calculation);
        }

        public override float OverallPoints
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override float[] SubPoints
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
