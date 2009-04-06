using System;
using System.Collections.Generic;

namespace Rawr.DPSWarr
{
    public class CharacterCalculationsDPSWarr : CharacterCalculationsBase
    {
        public CharacterCalculationsDPSWarr() : this(new Stats()){}
        public CharacterCalculationsDPSWarr(Stats stats)
        {
            BasicStats = stats;
            _dictValues.Add(DisplayValue.Strength, stats.Strength.ToString());
            _dictValues.Add(DisplayValue.Agility, stats.Agility.ToString());
            _dictValues.Add(DisplayValue.AttackPower, stats.AttackPower.ToString());
            _dictValues.Add(DisplayValue.ArmorPenetration, (stats.ArmorPenetration * 100f).ToString() + "%");    
            _dictValues.Add(DisplayValue.TotalDPS, "0");
        }

        private readonly Dictionary<DisplayValue, string> _dictValues = new Dictionary<DisplayValue, string>();

        public Stats BasicStats { get; private set; }

        public override float OverallPoints
        {
            get { return TotalDPS; }
            set { }
        }
        public override float[] SubPoints
        {
            get { return new[] { TotalDPS }; }
            set { }
        }
        
        public float TotalDPS
        {
            get 
            {
                float value;
                float.TryParse(_dictValues[DisplayValue.TotalDPS], out value);
                return value;
            }
            set { _dictValues[DisplayValue.TotalDPS] = Round(value); }
        }

        public void AddDisplayValue(DisplayValue key, string value)
        {
            _dictValues[key] = value;
        }

        public void AddRoundedDisplayValue(DisplayValue key, float value)
        {
            AddDisplayValue(key, Round(value));
        }

        private static string Round(float value)
        {
            return Math.Round(value, 2).ToString();
        }
        
        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            var _returnValue = new Dictionary<string, string>();
            foreach(var kvp in _dictValues)
            {
                _returnValue.Add(kvp.Key.Name, kvp.Value);
            }
            return _returnValue;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Health": return BasicStats.Health;
                case "Haste Rating": return BasicStats.HasteRating;
                case "Expertise Rating": return BasicStats.ExpertiseRating;
                case "Hit Rating": return BasicStats.HitRating;
                case "Agility": return BasicStats.Agility;
                case "Attack Power": return BasicStats.AttackPower;
            }

            return 0f;
        }
    }
}
