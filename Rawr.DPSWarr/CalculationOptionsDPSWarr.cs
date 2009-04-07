using System;
using System.Text;

namespace Rawr.DPSWarr {
    [Serializable]
    public class CalculationOptionsDPSWarr : ICalculationOptionBase {
        public string GetXml() {
            var s = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsDPSWarr));
            var xml = new StringBuilder();
            var sw = new System.IO.StringWriter(xml);
            s.Serialize(sw, this);
            return xml.ToString();
        }
        public int TargetLevel = 83;
        public int TargetArmor = 12900;
        public int ToughnessLvl = 0;
        public bool FuryStance = true;
    }
}