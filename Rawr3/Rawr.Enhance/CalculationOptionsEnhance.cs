using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text;

namespace Rawr.Enhance
{
    public class CalculationOptionsEnhance : ICalculationOptionBase
    {
        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsEnhance));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

        public bool BaseStatOption { get; set; }
        public bool Magma { get; set; }

        public int TargetLevel = 83;
        public int TargetArmor = (int)StatConversion.NPC_ARMOR[83 - 80];
        public int AverageLag = 250;
        public string MainhandImbue = "Windfury";
        public string OffhandImbue = "Flametongue";
        public float FightLength = 6.0f;
        public int TargetFireResistance = 0;
        public int TargetNatureResistance = 0;

    }
}
