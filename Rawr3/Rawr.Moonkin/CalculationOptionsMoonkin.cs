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

namespace Rawr.Moonkin
{
    public class CalculationOptionsMoonkin : ICalculationOptionBase
    {
        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsMoonkin));
            System.Text.StringBuilder xml = new System.Text.StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

        public CalculationOptionsMoonkin()
        {
        }

        public CalculationOptionsMoonkin(Character character)
        {
        }

        public int TargetLevel = 83;
        public float Latency = 0.2f;
        public float FightLength = 5;
        public bool Innervate = false;
        public float InnervateDelay = 1;
        public bool ManaPots = false;
        public string ManaPotType = "Runic Mana Potion";
        public string AldorScryer = "Aldor";
        public float ReplenishmentUptime = 1.0f;
        public float TreantLifespan = 1.0f;
        public bool LunarEclipse = true;
        public bool MoonfireAlways = true;
        public string userRotation = "None";
    }
}
