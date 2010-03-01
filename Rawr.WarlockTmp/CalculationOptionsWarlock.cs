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

        public String Pet { get; set; }
        public bool UseInfernal { get; set; }
        public int TargetLevel { get; set; }
        public int Duration { get; set; }
        public float Latency { get; set; }
        public float ManaPotSize { get; set; }
        public float Replenishment { get; set; }
        public List<String> SpellPriority { get; set; }

        #endregion


        #region constructors

        public CalculationOptionsWarlock() {
            Pet = "None";
            TargetLevel = 83;
            Duration = 300;
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
