using System;
using System.Text;

namespace Rawr.Rogue
{
    [Serializable]
    public class CalculationOptionsRogue : ICalculationOptionBase
    {
        public string GetXml()
        {
            var s = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRogue));
            var xml = new StringBuilder();
            var sw = new System.IO.StringWriter(xml);
            s.Serialize(sw, this);
            return xml.ToString();
        }

        public int TargetLevel = 83;
        public int TargetArmor = 10000;
        public Cycle DPSCycle = new Cycle("4s5r");
        public IPoison TempMainHandEnchant = new NoPoison();
        public IPoison TempOffHandEnchant = new NoPoison();
    }
}