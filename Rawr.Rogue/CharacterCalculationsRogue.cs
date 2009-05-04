using System;
using System.Collections.Generic;

namespace Rawr.Rogue
{
    public class CharacterCalculationsRogue : CharacterCalculationsBase
    {
        public CharacterCalculationsRogue() : this(new Stats()){}
        public CharacterCalculationsRogue(Stats stats)
        {
            BasicStats = stats;
            AddDisplayValue(DisplayValue.Health, stats.Health.ToString());
            AddDisplayValue(DisplayValue.Stamina, stats.Stamina.ToString());
            AddDisplayValue(DisplayValue.Strength, stats.Strength.ToString());
            AddDisplayValue(DisplayValue.Agility, stats.Agility.ToString());
            AddDisplayValue(DisplayValue.AttackPower, stats.AttackPower.ToString());
            AddDisplayValue(DisplayValue.TotalDps, "0");
        }

        private readonly Dictionary<DisplayValue, StatAndToolTip> _dictValues = new Dictionary<DisplayValue, StatAndToolTip>();
        
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
                float.TryParse(_dictValues[DisplayValue.TotalDps].Stat, out value);
                return value;
            }
            set { _dictValues[DisplayValue.TotalDps].Stat = Round(value); }
        }

        public void AddDisplayValue(DisplayValue key, string value)
        {
            if(!_dictValues.ContainsKey(key))
            {
                _dictValues.Add(key, new StatAndToolTip());
            }
            
            _dictValues[key].Stat = value;
        }

        public void AddRoundedDisplayValue(DisplayValue key, float value)
        {
            AddDisplayValue(key, Round(value));
        }

        public void AddPercentageToolTip(DisplayValue key, string prefix, float value)
        {
            AddToolTip(key, prefix + Round(value*100));    
        }

        public void AddToolTip(DisplayValue key, params string[] toolTips)
        {
            if (!_dictValues.ContainsKey(key))
            {
                _dictValues.Add(key, new StatAndToolTip());
            }
            
            _dictValues[key].ToolTips.AddRange(toolTips);
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
                var toolTip = kvp.Value.ToolTips.Count == 0 ? "" : "*" + string.Join("\r\n", kvp.Value.ToolTips.ToArray());
                _returnValue.Add(kvp.Key.Name, kvp.Value.Stat + toolTip);
            }
            return _returnValue;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            /* "Health",
             * "Haste Rating",
             * "Expertise Rating",
             * "Hit Rating",
             * "Agility",
             * "Attack Power"
             */
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

        private class StatAndToolTip
        {
            public string Stat = "";
            public readonly List<string> ToolTips = new List<string>();
        }
    }
}
