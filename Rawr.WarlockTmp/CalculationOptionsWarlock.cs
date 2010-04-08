using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.WarlockTmp {

    /// <summary>
    /// Holds the options the user selects from the options tab.
    /// </summary>
    public class CalculationOptionsWarlock : ICalculationOptionBase {

        #region constants

        private static readonly int[] hitRatesByLevelDifference 
            = { 100 - 4, 100 - 5, 100 - 6, 100 - 17, 100 - 28, 100 - 39 };

        #endregion


        #region properties

        public String Pet;
        public bool UseInfernal;
        public int TargetLevel;
        public float Duration;
        public float Latency;
        public float ManaPotSize;
        public float Replenishment;
        public string Filler;
        public List<string> SpellPriority;
        public bool NoProcs;

        #endregion


        #region constructors

        public CalculationOptionsWarlock() {

            Pet = "None";
            TargetLevel = 83;
            Duration = 300;
            Filler = "Shadow Bolt";
            SpellPriority = new List<String>();
        }

        #endregion


        #region methods

        public int GetBaseHitRate() { 
            
            return hitRatesByLevelDifference[TargetLevel - 80];
        }

        public string GetXml() {

            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsWarlock));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml, System.Globalization.CultureInfo.InvariantCulture);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

        #endregion
    }
}
