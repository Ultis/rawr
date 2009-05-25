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

        //---------------------------------------------------------------------
        // Hack/Workaround
        // the base class is looking for the display values in a dictionary 
        // with a "*" splitting up the values bewteen the value and tooltip.
        // Formatting a string like that is a pain, so I stuff it in a object
        // that does the tranlations for me.
        // The DisplayValue class groups the stat and which box it goes in,
        // then this class keeps track of the tooltips for each of the 
        // DisplayValues.  The GetCharacterDisplayCalculationValues override
        // then translates the DisplayValues and Tooltips into the format
        // the UI is expecting

        // the _dictValues is a horrible name, since it describes what the
        // object is, instead of what the object does.  I just can't think
        // of a good name for it yet
        //---------------------------------------------------------------------
        private readonly Dictionary<DisplayValue, StatAndToolTip> _dictValues = new Dictionary<DisplayValue, StatAndToolTip>();

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

        public void AddRoundedToolTip(DisplayValue key, string text, float value)
        {
            AddToolTip(key, string.Format(text, Round(value)));    
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
            var returnValue = new Dictionary<string, string>();
            foreach(var kvp in _dictValues)
            {
                var toolTip = kvp.Value.ToolTips.Count == 0 ? "" : "*" + string.Join("\r\n", kvp.Value.ToolTips.ToArray());
                returnValue.Add(kvp.Key.Name, kvp.Value.Stat + toolTip);
            }
            return returnValue;
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
